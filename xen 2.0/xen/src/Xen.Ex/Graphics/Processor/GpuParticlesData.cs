using System;
using System.Collections.Generic;
using System.Text;
using Xen.Graphics;
using Xen.Camera;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Xen.Ex.Graphics.Display;
using Xen.Ex.Graphics;
using Xen.Ex.Graphics2D;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Xen.Ex.Graphics.Content;

namespace Xen.Ex.Graphics.Processor
{

	/// <summary>
	/// stores the raw data for a particle system shader
	/// </summary>
	public struct ParticleSystemCompiledShaderData
	{
		/// <summary></summary>
		public readonly byte[] CompressedShaderCode;
		/// <summary></summary>
		public readonly int ColourSamplerIndex, UserSamplerIndex, LifeSamplerIndex;

		/// <summary></summary><param name="reader"></param>
		public ParticleSystemCompiledShaderData(ContentReader reader)
		{
			this.CompressedShaderCode = reader.ReadBytes(reader.ReadInt32());
			this.ColourSamplerIndex = reader.ReadInt32();
			this.UserSamplerIndex = reader.ReadInt32();
			this.LifeSamplerIndex = reader.ReadInt32();
		}

		internal ParticleSystemCompiledShaderData(byte[] shaderCode, bool isXbox, int colourIndex, int userIndex, int lifeIndex)
		{
			if (shaderCode == null)
				throw new ArgumentNullException();

			//compress the shader code
			if (isXbox)
			{
				//deflate stream isn't supported
				shaderCode = Xen.Graphics.ShaderSystem.ShaderSystemBase.SimpleCompress(shaderCode);
			}
			else
			{
#if !XBOX360
				using (MemoryStream stream = new MemoryStream())
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					writer.Write(shaderCode.Length);
					using (System.IO.Compression.DeflateStream d = new System.IO.Compression.DeflateStream(stream, System.IO.Compression.CompressionMode.Compress))
					{
						d.Write(shaderCode, 0, shaderCode.Length);
						d.Flush();
					}
					shaderCode = stream.ToArray();
				}
#endif
			}

			this.CompressedShaderCode = shaderCode;
			this.ColourSamplerIndex = colourIndex;
			this.UserSamplerIndex = userIndex;
			this.LifeSamplerIndex = lifeIndex;
		}
		
#if DEBUG && !XBOX360

		internal void Write(BinaryWriter writer)
		{
			writer.Write(this.CompressedShaderCode.Length);
			writer.Write(this.CompressedShaderCode);
			writer.Write(this.ColourSamplerIndex);
			writer.Write(this.UserSamplerIndex);
			writer.Write(this.LifeSamplerIndex);
		}

#endif
	}

	/// <summary>
	/// Data storage for the GPU particle system shader code
	/// </summary>
	public sealed class GpuParticleProcessorData
	{
		/// <summary></summary>
		public readonly ParticleSystemCompiledShaderData OnceShaderData, OnceCloneShaderData, FrameShaderData, FrameMoveShaderData;
		private readonly GpuParticleShader onceShader, onceCloneShader, frameShader, frameMoveShader;

		/// <summary></summary>
		/// <param name="onceShaderData"></param>
		/// <param name="onceCloneShaderData"></param>
		/// <param name="frameShaderData"></param>
		/// <param name="frameMoveShaderData"></param>
		public GpuParticleProcessorData(ParticleSystemCompiledShaderData onceShaderData, ParticleSystemCompiledShaderData onceCloneShaderData, ParticleSystemCompiledShaderData frameShaderData, ParticleSystemCompiledShaderData frameMoveShaderData)
		{
			this.OnceShaderData = onceShaderData;
			this.OnceCloneShaderData = onceCloneShaderData;
			this.FrameShaderData = frameShaderData;
			this.FrameMoveShaderData = frameMoveShaderData;

			this.onceShader = new GpuParticleShader(this.OnceShaderData);
			this.onceCloneShader = new GpuParticleShader(this.OnceCloneShaderData);
			this.frameShader = new GpuParticleShader(this.FrameShaderData);
			this.frameMoveShader = new GpuParticleShader(this.FrameMoveShaderData);
		}

		internal GpuParticleProcessorData(ContentReader reader) :
			this(new ParticleSystemCompiledShaderData(reader), new ParticleSystemCompiledShaderData(reader), new ParticleSystemCompiledShaderData(reader), new ParticleSystemCompiledShaderData(reader))
		{
		}

		/// <summary></summary>
		public IShader OnceShader { get { return onceShader; } }
		/// <summary></summary>
		public IShader OnceCloneShader { get { return onceCloneShader; } }
		/// <summary></summary>
		public IShader FrameShader { get { return frameShader; } }
		/// <summary></summary>
		public IShader FrameMoveShader { get { return frameMoveShader; } }

#if DEBUG && !XBOX360

		internal void Write(BinaryWriter writer)
		{
			OnceShaderData.Write(writer);
			OnceCloneShaderData.Write(writer);
			FrameShaderData.Write(writer);
			FrameMoveShaderData.Write(writer);
		}

		internal GpuParticleProcessorData(ParticleSystemTypeData typeData, bool useColourValues, bool usesUserValues, bool storeLifeData, ContentTargetPlatform targetPlatform, string pathToShaderSystem) : this(
			GpuParticleShaderBuilder.BuildGpuLogicPixelShader(typeData.ParticleLogicData.Once, GpuParticleShaderBuilder.LogicType.Once, GpuParticleShaderBuilder.VertexShaderType.Once, usesUserValues, useColourValues, storeLifeData, targetPlatform, typeData.GpuBufferPosition, pathToShaderSystem),
			GpuParticleShaderBuilder.BuildGpuLogicPixelShader(typeData.ParticleLogicData.Once, GpuParticleShaderBuilder.LogicType.OnceClone, GpuParticleShaderBuilder.VertexShaderType.Clone, usesUserValues, useColourValues, storeLifeData, targetPlatform, typeData.GpuBufferPosition, pathToShaderSystem),
			GpuParticleShaderBuilder.BuildGpuLogicPixelShader(typeData.ParticleLogicData.Frame, GpuParticleShaderBuilder.LogicType.Frame, GpuParticleShaderBuilder.VertexShaderType.Frame, usesUserValues, useColourValues, storeLifeData, targetPlatform, typeData.GpuBufferPosition, pathToShaderSystem),
			GpuParticleShaderBuilder.BuildGpuLogicPixelShader(typeData.ParticleLogicData.Frame, GpuParticleShaderBuilder.LogicType.FrameMove, GpuParticleShaderBuilder.VertexShaderType.Clone, usesUserValues, useColourValues, storeLifeData, targetPlatform, typeData.GpuBufferPosition, pathToShaderSystem))
		{
		}

#endif
	}

	sealed class ConstantCache
	{
		public Vector4[] 
			buffer240 = new Vector4[240 + GpuParticleShader.ConstantCacheOffset],
			buffer128 = new Vector4[128 + GpuParticleShader.ConstantCacheOffset], 
			buffer64 = new Vector4[64 + GpuParticleShader.ConstantCacheOffset],
			buffer32 = new Vector4[32 + GpuParticleShader.ConstantCacheOffset],
			buffer16 = new Vector4[16 + GpuParticleShader.ConstantCacheOffset];
	}

	//manual implementation of the IShader interface
	sealed class GpuParticleShader : IShader, IDisposable
	{
		internal const int ConstantCacheOffset = 7;
		//the actual shaders
		private Xen.Graphics.ShaderSystem.ShaderEffect fx;
		private readonly byte[] fxb;

		//texture sampler indices for the optional PS textures
		private readonly int colourSamplerIndex, userSamplerIndex, lifeSamplerIndex;

		//source textures being used
		private Texture2D positionSize, velocityRotation, colourValues, userValues, randTexture, lifeTexture;
		//shader constant name indices
		private int world=-1, viewsize=-1, device=-1;
		//vertex shader constants
		private Vector4[] vreg = new Vector4[ConstantCacheOffset];
		private Vector4[] psConstants;

		//shader has three modes, per-frame, 'copy' (move) and add.
		private bool enabledMoveVS, enabledAddVS;
		private Vector4[] vsMoveConstants;
		private ConstantCache constantCache;
		static string ConstantCacheName = typeof(GpuParticleShader).FullName + ".ConstantCache";
		private readonly Random random = new Random();

		private readonly Dictionary<ParticleSpawnValues, float> spawnIndices;

		public GpuParticleShader(ParticleSystemCompiledShaderData pixelShaderData)
		{
			this.fxb = pixelShaderData.CompressedShaderCode;

			this.colourSamplerIndex = pixelShaderData.ColourSamplerIndex;
			this.userSamplerIndex = pixelShaderData.UserSamplerIndex;
			this.lifeSamplerIndex = pixelShaderData.LifeSamplerIndex;

			this.spawnIndices = new Dictionary<ParticleSpawnValues, float>(new ParticleSpawnValues.ParticleSpawnValuesComparer());
			this.psConstants = new Vector4[6];
		}

		public void SetTextures(Texture2D positionSize, Texture2D velocityRot, Texture2D colour, Texture2D userValues, Texture2D randTexture, Texture2D lifeTexture)
		{
			this.positionSize		= positionSize;
			this.velocityRotation	= velocityRot;
			this.colourValues		= colour;
			this.userValues			= userValues;
			this.randTexture		= randTexture;
			this.lifeTexture		= lifeTexture;
		}

		public void SetMoveShaderDisabled(DrawState state)
		{
			SetMoveShaderEnabled(state, null, null, null, null, 0, 0);
		}

		//setup the shaders that copy or add particles
		public int SetMoveShaderEnabled(DrawState state, GpuParticleProcessor target, GpuParticleProcessor moveSource, Vector4[] constants, ParticleSpawnValues[] initialData, int count, int startIndex)
		{
			if (constants != null)
			{
				if (constantCache == null)
				{
					constantCache = state.Application.UserValues[ConstantCacheName] as ConstantCache;
					if (constantCache == null)
					{
						constantCache = new ConstantCache();
						state.Application.UserValues[ConstantCacheName] = constantCache;
					}
				}

				Vector4[] buffer = null;
				int copyCount = 0;

				int regCount = count;

				if (initialData != null)
					regCount = Math.Min(240, count * 5); //adding requires potentially lots of space

				if (count > 128)
				{
					copyCount = Math.Min(240, regCount);
					buffer = constantCache.buffer240;
				}
				else if (regCount > 64)
				{
					copyCount = Math.Min(128, regCount);
					buffer = constantCache.buffer128;
				}
				else if (regCount > 32)
				{
					copyCount = Math.Min(64, regCount);
					buffer = constantCache.buffer64;
				}
				else if (regCount > 16)
				{
					copyCount = Math.Min(32, regCount);
					buffer = constantCache.buffer32;
				}
				else
				{
					copyCount = Math.Min(16, regCount);
					buffer = constantCache.buffer16;
				}


				if (initialData != null)
					this.enabledAddVS = true;
				else
					this.enabledMoveVS = true;

				this.vsMoveConstants = buffer;

				//write from the 7th index on
				int index = ConstantCacheOffset;

				if (initialData != null)
				{
					//initial data gets rather complex.
					//the initial particle data is huge (64bytes), but often there is loads of duplicates.
					//
					//the constants array also doesn't use the 'y' value.
					//So, the starting info will be put in a dictionary, to remove duplicates.
					//Then, the y value will be used to write indices to read from.

					//at this point, the copyCount value is useless.

					spawnIndices.Clear();
					int space = buffer.Length - index;
					int written = 0;
					float indexF = 0;

					int endIndex = count + startIndex;

					//copy as many entries as possible
					copyCount = 0;
					float offset = 0;

					for (int i = startIndex; i < endIndex; i++)
					{
						float dataIndex;

						if (written == space)
							break;

						if (!spawnIndices.TryGetValue(initialData[i], out dataIndex))
						{
							//have to write data
							if (written + 5 > space)
								break;

							Vector4 value = constants[i];
							value.Y = indexF;

							buffer[index++] = value;
							spawnIndices.Add(initialData[i], indexF);

							written += 5;
							indexF += 4;//value takes 4 registers
						}
						else
						{
							//this is a duplicate
							Vector4 value = constants[i];
							value.Y = dataIndex;

							buffer[index++] = value;

							written ++;
						}
						copyCount++;
						offset++;
					}

					//thats as much as can be written
					//fill in the details...

					//offset the indices with the starting point to read from
					for (int i = 0; i < copyCount; i++)
						buffer[ConstantCacheOffset + i].Y += offset;

					indexF = 0;
					foreach (ParticleSpawnValues value in spawnIndices.Keys)
					{
						//this should be in logical order

						if (indexF != spawnIndices[value])
							throw new InvalidOperationException();
						indexF += 4;

						buffer[index++] = value.PositionSize;
						buffer[index++] = value.VelocityRotation;
						buffer[index++] = value.Colour;
						buffer[index++] = value.UserValues;
					}
				}
				else
				{
					for (int i = 0; i < copyCount; i++)
						buffer[index++] = constants[startIndex++];
				}

				//write target size into index 4 XY
				//write destination size into index 5 XY

				moveSource = moveSource ?? target;

				buffer[4] = new Vector4(target.ResolutionX, target.ResolutionY, 1.0f / target.ResolutionX, 1.0f / target.ResolutionY);
				buffer[5] = new Vector4(moveSource.ResolutionX, moveSource.ResolutionY, 1.0f / moveSource.ResolutionX, 1.0f / moveSource.ResolutionY);
			
				return copyCount;
			}
			else
			{
				this.vsMoveConstants = null;
				this.enabledMoveVS = false;
				this.enabledAddVS = false;
				return 0;
			}
		}

		//stepSize (delta time), current step, noise base XY
		public void SetConstants(float[] globals, float deltaTime, float time, float maxTimeStep)
		{
			this.psConstants[0].X = globals[0];
			this.psConstants[0].Y = globals[1];
			this.psConstants[0].Z = globals[2];
			this.psConstants[0].W = globals[3];

			this.psConstants[1].X = globals[4];
			this.psConstants[1].Y = globals[5];
			this.psConstants[1].Z = globals[6];
			this.psConstants[1].W = globals[7];

			this.psConstants[2].X = globals[8];
			this.psConstants[2].Y = globals[9];
			this.psConstants[2].Z = globals[10];
			this.psConstants[2].W = globals[11];

			this.psConstants[3].X = globals[12];
			this.psConstants[3].Y = globals[13];
			this.psConstants[3].Z = globals[14];
			this.psConstants[3].W = globals[15];

			this.psConstants[4].X = maxTimeStep;
			this.psConstants[4].Y = time;

			this.psConstants[5].Z = deltaTime;
		}

		public void Begin(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			if (device != state.DeviceUniqueIndex)
			{
				device = state.DeviceUniqueIndex;

				fx.Dispose();
				state.CreateEffect(out fx, fxb, 0, 0);
			}

			float randX = (float)(random.NextDouble());
			float randY = (float)(random.NextDouble());

			this.psConstants[4].Z = randX;
			this.psConstants[4].W = randY;

			//random texture is limited in size, so randomly offset by a tiny amount
			randX = (float)((random.NextDouble() * 2 - 1) / 256.0);
			randY = (float)((random.NextDouble() * 2 - 1) / 256.0);

			this.psConstants[5].X = randX;
			this.psConstants[5].Y = randY;

			TextureSamplerState point = TextureSamplerState.PointFiltering;

			Xen.Graphics.ShaderSystem.ShaderSystemBase shaderSystem = (Xen.Graphics.ShaderSystem.ShaderSystemBase)state;
			shaderSystem.SetPixelShaderSampler(0, randTexture, point);

			if (positionSize != null)
				shaderSystem.SetPixelShaderSampler(1, positionSize, point);
			if (velocityRotation != null)
				shaderSystem.SetPixelShaderSampler(2, velocityRotation, point);
			
			if (this.colourSamplerIndex != -1)
				shaderSystem.SetPixelShaderSampler(this.colourSamplerIndex, colourValues, point);
			
			if (this.userSamplerIndex != -1)
				shaderSystem.SetPixelShaderSampler(this.userSamplerIndex, userValues, point);

			if (this.lifeSamplerIndex != -1)
				shaderSystem.SetPixelShaderSampler(this.lifeSamplerIndex, lifeTexture, point);


			state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.world);

			const float randTextureSize = (float)RandomValueTexture.Resolution;
			const float invRandTextureSize = 1.0f / randTextureSize; 

			if (enabledMoveVS || enabledAddVS)
			{
				for (int i = 0; i < 4; i++) //copy the WVP matrix
					this.vsMoveConstants[i] = this.vreg[i];

				vsMoveConstants[6] = new Vector4(randTextureSize, invRandTextureSize, 0, 0);
				
				fx.vs_c.SetValue(this.vsMoveConstants);
				fx.ps_c.SetValue(psConstants);
			}
			else
			{
				state.SetWindowSizeVector2(ref this.vreg[4], ref this.viewsize);
				this.vreg[6] = new Vector4(randTextureSize, invRandTextureSize, 0, 0);
				
				fx.vs_c.SetValue(this.vreg);
				fx.ps_c.SetValue(psConstants);
			}

			state.SetEffect(this, ref fx, Xen.Graphics.ShaderSystem.ShaderExtension.None);
		}

		//vertex shader input requirements... 
		void IShader.GetVertexInput(int index, out VertexElementUsage elementUsage, out int elementIndex)
		{
			elementIndex = 0;
			elementUsage = index == 0 ? VertexElementUsage.Position : VertexElementUsage.TextureCoordinate;
		}

		int IShader.GetVertexInputCount()
		{
			if (enabledMoveVS || enabledAddVS)
				return 1;
			return 2;
		}

		void IShader.GetExtensionSupport(out bool blending, out bool instancing)
		{
			blending = false;
			instancing = false;
		}

		bool IShader.HasChanged
		{
			get { return true; }
		}

		//ignore
		#region attributes

		bool IShader.SetAttribute(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, bool value)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetAttribute(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, float[] value)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetAttribute(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, Vector2[] value)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetAttribute(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, Vector3[] value)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetAttribute(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, Vector4[] value)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetAttribute(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, Matrix[] value)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetAttribute(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, float value)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetAttribute(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, ref Vector2 value)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetAttribute(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, ref Vector3 value)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetAttribute(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, ref Vector4 value)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetAttribute(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, ref Matrix value)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetSamplerState(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, TextureSamplerState sampler)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetTexture(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, TextureCube texture)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetTexture(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, Texture3D texture)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetTexture(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, Texture2D texture)
		{
			throw new NotImplementedException();
		}

		bool IShader.SetTexture(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int name_uid, Texture texture)
		{
			throw new NotImplementedException();
		}

		#endregion


		public void Dispose()
		{
			fx.Dispose();

			positionSize = null;
			velocityRotation = null;
			colourValues = null;
			userValues = null;
			randTexture = null;
			lifeTexture = null;

			vreg = null;
			psConstants = null;

			vsMoveConstants = null;
			constantCache = null;
		}
	}

}
