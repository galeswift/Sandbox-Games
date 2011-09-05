using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Xen.Graphics.ShaderSystem.CustomTool.FX
{
	//data not stored within an ASM shader listing
	public sealed class TechniqueExtraData
	{
		public Vector4[] PixelShaderConstants, VertexShaderConstants;
		public bool[] PixelShaderBooleanConstants, VertexShaderBooleanConstants;
		public TextureSamplerState[] PixelSamplerStates, VertexSamplerStates;
		public Register[] TechniqueTextures;
		public int[] PixelSamplerTextureIndex, VertexSamplerTextureIndex;
		public string[] ClassBaseTypes;
		public Dictionary<string, Vector4[]> DefaultSingleValues;
	}

	//this class takes an FX file, and decompiled it into raw shader assmebly
	//it always compiles the effect for the PC, as an Xbox effect can not be dissaembled.
	//
	//It also attempts to extract default values for shader constants and samplers.
	public sealed class DecompiledEffect
	{
		private readonly Dictionary<string, TechniqueExtraData> techniqueDefaults;

		private readonly string decompiledAsm;
		private readonly RegisterSet effectRegisters;
		
		internal class EffectHandler : Native.IEffectCallback
		{
			public Effect Effect;

			//debug help
			public string source, filename;
			public bool buildForXbox;
			public byte[] shaderByteCode;

			public System.Reflection.Pointer GetID3DXEffectInterface(byte[] shaderCode)
			{
				//XNA 4 embeds a small header on the front of the effect code... sigh
				//header is the following 4 bytes:
				//207,11,240,188
				//the offset of the shader source code
				//plus uints storing state flag information about each pass in each technique (eek)
				//then the souce

				uint passBytes = 2048; //no idea how many passes there are yet, so guess!

				uint headerSize = passBytes * 4 + 8;
				byte[] paddedCode = new byte[shaderCode.Length + headerSize];
				this.shaderByteCode = paddedCode;
				byte[] offset = BitConverter.GetBytes(headerSize);
				paddedCode[0] = 207;
				paddedCode[1] = 11;
				paddedCode[2] = 240;
				paddedCode[3] = 188;
				paddedCode[4] = offset[0];
				paddedCode[5] = offset[1];
				paddedCode[6] = offset[2];
				paddedCode[7] = offset[3];
				for (int i = 0; i < shaderCode.Length; i++)
					paddedCode[i + headerSize] = shaderCode[i];

				try
				{
					Effect = new Effect(Graphics.GraphicsDevice, paddedCode);
					object ptr = Effect.GetType().GetField("pComPtr", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(Effect);

					return (System.Reflection.Pointer)ptr;
				}
				catch (Exception ex)
				{
					//an exception can be thrown if the animation generation failed. In which case, compile the shader using XNA to pick out the errors
					try
					{
						string errors;
						EffectCompiler.CompileEffect(source, filename, buildForXbox, out errors);
						if (errors != null)
						{
							ShaderExtensionGenerator.ThrowGeneratorError(errors, filename);
						}
					}
					catch
					{
					}

					ShaderExtensionGenerator.ThrowGeneratorError(ex.Message, filename);
					return null;
				}
			}
		}

		public DecompiledEffect(SourceShader source, Platform platform)
		{
			this.techniqueDefaults = new Dictionary<string, TechniqueExtraData>();

			var handler = new EffectHandler();
			handler.source = source.ShaderSource;
			handler.filename = source.FileName;
			handler.buildForXbox = platform == Platform.Xbox;

			//create the native DX decompiler wrapper
			var decompiler = new Native.ShaderDecompiler(ASCIIEncoding.ASCII.GetBytes(source.ShaderSource), source.FileName, platform == Platform.Xbox, handler);
			
			Effect effect = handler.Effect;

			if (decompiler.Errors != null && decompiler.GetShaderCode() == null)
				Common.ThrowError(decompiler.Errors);
			
			//now pull the good stuff out.

			List<Register> registers = new List<Register>();
			List<Register> textures = new List<Register>();

			for (int i = 0; i < effect.Parameters.Count; i++)
			{
				Register register = new Register();

				if (effect.Parameters[i].ParameterType == EffectParameterType.Single ||
					effect.Parameters[i].ParameterType == EffectParameterType.Int32 ||
					effect.Parameters[i].ParameterType == EffectParameterType.Bool)
				{
					register.Name = effect.Parameters[i].Name;
					register.Semantic = effect.Parameters[i].Semantic;
					register.ArraySize = effect.Parameters[i].Elements.Count;
					register.Size = effect.Parameters[i].RowCount * Math.Max(1, register.ArraySize);
					registers.Add(register);
				}

				if (effect.Parameters[i].ParameterType >= EffectParameterType.Texture &&
					effect.Parameters[i].ParameterType <= EffectParameterType.TextureCube)
				{
					EffectParameterType type = effect.Parameters[i].ParameterType;
					if (type == EffectParameterType.Texture1D)
						type = EffectParameterType.Texture2D;

					register.Name = effect.Parameters[i].Name;
					register.Semantic = effect.Parameters[i].Semantic;
					register.Type = type.ToString();
					textures.Add(register);
					register.Category = RegisterCategory.Texture;
					registers.Add(register);
				}

			}

			//iterate the samplers (XNA 4 removed the ability to query samplers!)
			var samplers = decompiler.GetSamplers();
			for (int i = 0; i < samplers.Length; i++)
			{
				var sampler = new Register();
				sampler.Name = samplers[i].Name;
				sampler.Semantic = samplers[i].Semantic;
				sampler.Type = samplers[i].Type;
				sampler.Category = RegisterCategory.Sampler;
				registers.Add(sampler);
			}
			
			//null any empty semantics
			for (int i = 0; i < registers.Count; i++)
			{
				Register reg = registers[i];
				if (reg.Semantic != null && reg.Semantic.Length == 0)
					reg.Semantic = null;
				registers[i] = reg;
			}

			this.effectRegisters = new RegisterSet(registers.ToArray());
			this.decompiledAsm = decompiler.DisassembledCode;

			ExtractEffectDefaults(effect, textures, source, platform);

			effect.Dispose();
			effect = null;
		}

		public string DecompiledAsm { get { return decompiledAsm; } }
		public RegisterSet EffectRegisters { get { return effectRegisters; } }

		public TechniqueExtraData GetTechniqueDefaultValues(string name)
		{
			TechniqueExtraData value = null;
			techniqueDefaults.TryGetValue(name, out value);
			return value;
		}




		private void ExtractEffectDefaults(Effect effect, List<Register> textures, SourceShader source, Platform platform)
		{
			//nasty-ness ensues!
			GraphicsDevice device = Graphics.GraphicsDevice;

			int maxVsConst = 256;
			int maxPsConst = 32;

			int maxPsTextures = 16;
			int maxVsTextures = 4;

			bool[] shaderBooleanConstants = new bool[16];

			List<Texture> allTextures = new List<Texture>();

			foreach (EffectTechnique technique in effect.Techniques)
			{
				//Thanks to Darren Grant (again :)
				//annotate a Technique with 'BaseTypes' and the generates class will inherit from those types
				string[] baseTypes = new string[0];
				foreach (EffectAnnotation annotation in technique.Annotations)
				{
					if (annotation.Name.Equals("BaseTypes", StringComparison.InvariantCulture) ||
						annotation.Name.Equals("BaseType", StringComparison.InvariantCulture))
					{
						baseTypes = annotation.GetValueString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

						for (int i = 0; i < baseTypes.Length; i++)
							baseTypes[i] = baseTypes[i].Trim();
					}
				}

				float[] psConstants = new float[maxPsConst * 4]; // pixel 
				float[] vsConstants = new float[maxVsConst * 4]; // not-pixel 

				TextureSamplerState[] psSamplers = new TextureSamplerState[maxPsTextures];
				TextureSamplerState[] vsSamplers = new TextureSamplerState[maxVsTextures];

				int[] psTexturesIndex = new int[maxPsTextures];
				int[] vsTexturesIndex = new int[maxVsTextures];

				int[] psBooleanConstants = new int[16];
				int[] vsBooleanConstants = new int[16];

				allTextures.Clear();

				Dictionary<string, Vector4[]> techniqueSingleValues = new Dictionary<string, Vector4[]>();

				//get the device
				object devicePtr = device.GetType().GetField("pComPtr", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(device);
				var deviceInterop = new Xen.Graphics.ShaderSystem.Native.DeviceInterop((System.Reflection.Pointer)devicePtr);

				try
				{
					//I'm sure XNA won't mind...
					deviceInterop.ZeroShaderConstants();

					for (int i = 0; i < maxPsTextures; i++)
					{
						deviceInterop.SetTextureFilters(i, false);
						ResetSampler(device, i, true);
					}
					for (int i = 0; i < maxVsTextures; i++)
					{
						deviceInterop.SetTextureFilters(i, true);
						ResetSampler(device, i, false);
					}

					//assign the technique textures
					foreach (Register texReg in textures)
					{
						Type type = Common.GetTextureType(texReg.Type);
						Texture tex = Graphics.BeginGetTempTexture(type);
						effect.Parameters[texReg.Name].SetValue(tex);

						allTextures.Add(tex);
					}

					//bind the effect technique
					effect.CurrentTechnique = technique;

					if (technique.Passes.Count > 0)
					{
						EffectPass pass = technique.Passes[0];
						pass.Apply();
					}


					foreach (var param in effect.Parameters)
					{
						try
						{
							if (param.ParameterType == EffectParameterType.Single ||
								param.ParameterType == EffectParameterType.Int32)
							{
								Vector4[] values = param.GetValueVector4Array(param.RowCount);
								techniqueSingleValues.Add(param.Name, values);
							}
						}
						catch
						{
						}
					}


					//all done. Now read back what has changed. :D
					deviceInterop.GetShaderConstantsPS(psConstants);
					deviceInterop.GetShaderConstantsVS(vsConstants);
					//psConstants = device.GetPixelShaderVector4ArrayConstant(0, maxPsConst);
					//vsConstants = device.GetVertexShaderVector4ArrayConstant(0, maxVsConst);

					for (int i = 0; i < maxPsTextures; i++)
						psSamplers[i] = GetState(device, deviceInterop, i, true, allTextures, out psTexturesIndex[i]);

					for (int i = 0; i < maxVsTextures; i++)
						vsSamplers[i] = GetState(device, deviceInterop, i, false, allTextures, out vsTexturesIndex[i]);

					for (int i = 0; i < allTextures.Count; i++)
						Graphics.EndGetTempTexture(allTextures[i]);
					allTextures.Clear();

					deviceInterop.GetShaderConstantsPS(psBooleanConstants);
					deviceInterop.GetShaderConstantsVS(vsBooleanConstants);
					//vsBooleanConstants = device.GetVertexShaderBooleanConstant(0, 16);
					//psBooleanConstants = device.GetPixelShaderBooleanConstant(0, 16);
				}
				catch
				{
					//something went wrong... Eg, binding a SM 3.0 shader on SM 2.0 hardware device

					throw new CompileException("An unexpected error occured while compiling shader: The DirectX device may not be XNA 'HiDef' capable");
				}

				TechniqueExtraData defaults = new TechniqueExtraData();

				defaults.PixelSamplerStates = psSamplers;
				defaults.PixelShaderConstants = Convert(psConstants);
				defaults.VertexSamplerStates = vsSamplers;
				defaults.VertexShaderConstants = Convert(vsConstants);
				defaults.PixelSamplerTextureIndex = psTexturesIndex;
				defaults.VertexSamplerTextureIndex = vsTexturesIndex;
				defaults.TechniqueTextures = textures.ToArray();
				defaults.ClassBaseTypes = baseTypes;
				defaults.PixelShaderBooleanConstants = Convert(psBooleanConstants);
				defaults.VertexShaderBooleanConstants = Convert(vsBooleanConstants);
				defaults.DefaultSingleValues = techniqueSingleValues;

				if (this.techniqueDefaults.ContainsKey(technique.Name) == false)
					this.techniqueDefaults.Add(technique.Name, defaults);
			}
		}

		private static Vector4[] Convert(float[] array)
		{
			Vector4[] output = new Vector4[array.Length / 4];
			int o = 0;
			for (int i = 0; i < array.Length;)
				output[o++] = new Vector4(array[i++], array[i++], array[i++], array[i++]);
			return output;
		}
		private static bool[] Convert(int[] array)
		{
			bool[] values = new bool[array.Length];
			for (int i = 0; i < array.Length; i++)
				values[i] = array[i] != 0;
			return values;
		}

		private static TextureAddressMode Translate(Native.DeviceInterop.Address mode)
		{
			switch (mode)
			{
				case Native.DeviceInterop.Address.D3DTADDRESS_CLAMP:
					return TextureAddressMode.Clamp;
				case Native.DeviceInterop.Address.D3DTADDRESS_MIRROR:
					return TextureAddressMode.Mirror;
				default:
					return TextureAddressMode.Wrap;
			}
		}

		private static TextureFilter Translate(Native.DeviceInterop.Filter min, Native.DeviceInterop.Filter mag, Native.DeviceInterop.Filter mip)
		{
			if (mip == Native.DeviceInterop.Filter.D3DTEXF_NONE) mip = Native.DeviceInterop.Filter.D3DTEXF_POINT;
			if (mag == Native.DeviceInterop.Filter.D3DTEXF_NONE) mag = Native.DeviceInterop.Filter.D3DTEXF_POINT;
			if (min == Native.DeviceInterop.Filter.D3DTEXF_NONE) min = Native.DeviceInterop.Filter.D3DTEXF_POINT;

			if (min == Native.DeviceInterop.Filter.D3DTEXF_LINEAR &&  mag == Native.DeviceInterop.Filter.D3DTEXF_LINEAR && mip == Native.DeviceInterop.Filter.D3DTEXF_LINEAR)
				return TextureFilter.Linear;
			if (min == Native.DeviceInterop.Filter.D3DTEXF_POINT && mag == Native.DeviceInterop.Filter.D3DTEXF_POINT && mip == Native.DeviceInterop.Filter.D3DTEXF_POINT)
				return TextureFilter.Point;
			if (min == Native.DeviceInterop.Filter.D3DTEXF_ANISOTROPIC || mip == Native.DeviceInterop.Filter.D3DTEXF_ANISOTROPIC)
				return TextureFilter.Anisotropic;
			if (min == Native.DeviceInterop.Filter.D3DTEXF_POINT && mag == Native.DeviceInterop.Filter.D3DTEXF_POINT && mip == Native.DeviceInterop.Filter.D3DTEXF_LINEAR)
				return TextureFilter.PointMipLinear;
			if (min == Native.DeviceInterop.Filter.D3DTEXF_LINEAR && mag == Native.DeviceInterop.Filter.D3DTEXF_POINT && mip == Native.DeviceInterop.Filter.D3DTEXF_LINEAR)
				return TextureFilter.MinLinearMagPointMipLinear;
			if (min == Native.DeviceInterop.Filter.D3DTEXF_LINEAR && mag == Native.DeviceInterop.Filter.D3DTEXF_POINT && mip == Native.DeviceInterop.Filter.D3DTEXF_POINT)
				return TextureFilter.MinLinearMagPointMipPoint;
			if (min == Native.DeviceInterop.Filter.D3DTEXF_POINT && mag == Native.DeviceInterop.Filter.D3DTEXF_LINEAR && mip == Native.DeviceInterop.Filter.D3DTEXF_LINEAR)
				return TextureFilter.MinPointMagLinearMipLinear;
			if (min == Native.DeviceInterop.Filter.D3DTEXF_POINT && mag == Native.DeviceInterop.Filter.D3DTEXF_LINEAR && mip == Native.DeviceInterop.Filter.D3DTEXF_POINT)
				return TextureFilter.MinPointMagLinearMipPoint;
			return TextureFilter.LinearMipPoint;
		}

		private TextureSamplerState GetState(GraphicsDevice device, Native.DeviceInterop io, int index, bool PS, List<Texture> textures, out int textureIndex)
		{
			TextureSamplerState tss = new TextureSamplerState();
			textureIndex = -1;

			try
			{
				Texture texture = null;

				if (PS)
				{
					texture = device.Textures[index];

					var state = io.GetSamplerState(index, false);

					tss = TextureSamplerState.BilinearFiltering;
					tss.AddressU = Translate(state.U);
					tss.AddressV = Translate(state.V);
					tss.AddressW = Translate(state.W);
					tss.Filter = Translate(state.Min, state.Mag, state.Mip);
					tss.MaxAnisotropy = (int)state.MaxAni;
				}
				else
				{
					texture = device.VertexTextures[index];

					var state = io.GetSamplerState(index, true);

					tss = TextureSamplerState.PointFiltering;
					tss.AddressU = Translate(state.U);
					tss.AddressV = Translate(state.V);
					tss.AddressW = Translate(state.W);
				}

				//special case, force texture cubes to always be clamped. (xbox helper)
				if (texture is TextureCube)
				{
					tss.AddressU = TextureAddressMode.Clamp;
					tss.AddressV = TextureAddressMode.Clamp;
					tss.AddressW = TextureAddressMode.Clamp;
				}

				for (int i = 0; i < textures.Count; i++)
				{
					if (texture == textures[i] &&
						texture != null && textures[i] != null)
					{
						textureIndex = i;
					}
				}
			}
			catch
			{
				//hmph...
			}
			return tss;
		}
		private void ResetSampler(GraphicsDevice device, int index, bool PS)
		{
			SamplerState ss;
			TextureSamplerState tss = PS ? TextureSamplerState.BilinearFiltering : TextureSamplerState.PointFiltering;

			try
			{
				ss = new SamplerState();

				//reset everything
				ss.AddressU = tss.AddressU;
				ss.AddressV = tss.AddressV;
				ss.AddressW = tss.AddressW;

				ss.Filter = tss.Filter;

				ss.MaxAnisotropy = tss.MaxAnisotropy;
				ss.MaxMipLevel = tss.MaxMipmapLevel;

				if (PS)
				{
					device.SamplerStates[index] = ss;
					device.Textures[index] = null;
				}
				else
				{
					device.VertexSamplerStates[index] = ss;
					device.VertexTextures[index] = null;
				}
			}
			catch
			{
				//ignore...
			}
		}
	}
}
