// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = Shader.fx
// Namespace = Xen.Ex.Filters

namespace Xen.Ex.Filters
{
	
	/// <summary><para>Technique 'Kernel16' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 21 instruction slots used, 21 registers</para><para>Pixel Shader: approximately 49 instruction slots used (16 texture, 33 arithmetic), 16 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class Kernel16 : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'Kernel16' shader</summary>
		public Kernel16()
		{
			this.vreg[20] = new Microsoft.Xna.Framework.Vector4(1F, 1F, 0F, 0F);
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			Kernel16.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			Kernel16.cid0 = state.GetNameUniqueID("kernel");
			Kernel16.cid1 = state.GetNameUniqueID("textureSize");
			Kernel16.sid0 = state.GetNameUniqueID("TextureSampler");
			Kernel16.tid0 = state.GetNameUniqueID("Texture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != Kernel16.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.preg_change = (this.preg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[16], ref this.vreg[17], ref this.vreg[18], ref this.vreg[19], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				Kernel16.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((this.preg_change == true))
			{
				Kernel16.fx.ps_c.SetValue(this.preg);
				this.preg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref Kernel16.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((Kernel16.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((Kernel16.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			Kernel16.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out Kernel16.fx, Kernel16.fxb, 23, 58);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.preg_change) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 2;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(Kernel16.vin[i]));
			index = Kernel16.vin[(i + 2)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary/>
		private bool preg_change;
		/// <summary>Name ID for 'kernel'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float3 kernel[16]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetKernel(Microsoft.Xna.Framework.Vector3[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector3 val;
			int i;
			uint ri;
			uint wi;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 16)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 16)); i = (i + 1))
			{
				val = value[ri];
				this.preg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.preg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float3 kernel[16]'</summary>
		public Microsoft.Xna.Framework.Vector3[] Kernel
		{
			set
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'textureSize'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 textureSize'</summary><param name="value"/>
		public void SetTextureSize(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[20] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 textureSize'</summary>
		public Microsoft.Xna.Framework.Vector2 TextureSize
		{
			set
			{
				this.SetTextureSize(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D TextureSampler'</summary>
		public Xen.Graphics.TextureSamplerState TextureSampler
		{
			get
			{
				return this.pts[0];
			}
			set
			{
				if ((value != this.pts[0]))
				{
					this.pts[0] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D Texture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[0]));
			}
			set
			{
				if ((value != this.ptx[0]))
				{
					this.ptc = true;
					this.ptx[0] = value;
				}
			}
		}
		/// <summary>Name uid for sampler for 'Sampler2D TextureSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D Texture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[21];
		/// <summary>Pixel shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] preg = new Microsoft.Xna.Framework.Vector4[16];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,3,8,135,0,1,3,131,0,5,1,0,0,1,112,135,0,1,21,131,0,1,4,131,0,1,1,229,0,0,229,0,0,229,0,0,164,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,1,0,1,0,1,2,1,152,135,0,0,1,16,131,0,0,1,4,131,0,0,1,1,229,0,0,229,0,0,185,0,0,1,6,1,95,1,112,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,2,1,188,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,3,131,0,0,1,1,131,0,0,1,4,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,138,0,0,1,1,1,124,1,0,1,0,1,1,1,152,138,0,0,1,2,1,164,1,0,1,0,1,2,1,184,138,0,0,1,2,1,252,135,0,0,1,1,1,0,1,0,1,2,1,248,135,0,0,1,2,131,0,0,1,92,134,0,0,1,2,1,204,1,0,1,0,1,2,1,200,131,0,0,1,93,134,0,0,1,2,1,228,1,0,1,0,1,2,1,224,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,3,1,172,1,16,1,42,1,17,131,0,0,1,1,1,240,1,0,1,0,1,1,1,188,135,0,0,1,36,134,0,0,1,1,1,176,138,0,0,1,1,1,136,131,0,0,1,28,1,0,1,0,1,1,1,123,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,134,0,0,1,1,1,116,131,0,0,1,68,1,0,1,2,131,0,0,1,16,133,0,0,1,76,131,0,0,1,92,1,0,1,0,1,1,1,92,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,100,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,16,229,0,0,229,0,0,188,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,134,0,0,1,1,1,188,1,16,1,0,1,16,132,0,0,1,4,134,0,0,1,129,1,8,1,0,1,255,1,0,1,255,131,0,0,1,1,1,0,1,0,1,240,1,80,1,0,1,0,1,241,1,81,1,0,1,0,1,242,1,82,1,0,1,0,1,243,1,83,1,0,1,0,1,244,1,84,1,0,1,0,1,245,1,85,1,0,1,0,1,246,1,86,1,0,1,0,1,247,1,87,1,5,1,85,1,96,1,4,1,96,1,10,1,18,1,0,1,18,1,0,1,5,1,85,1,0,1,85,1,64,1,16,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,20,1,96,1,26,1,18,1,0,1,18,133,0,0,1,64,1,32,1,0,1,0,1,34,133,0,0,1,16,1,8,1,128,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,144,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,160,1,65,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,176,1,97,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,192,1,129,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,208,1,161,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,224,1,193,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,240,1,225,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,9,1,0,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,16,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,32,1,65,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,48,1,97,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,64,1,129,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,80,1,161,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,96,1,193,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,0,1,225,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,132,0,0,1,198,1,0,1,161,1,0,1,15,1,0,1,200,1,15,132,0,0,1,198,1,0,1,171,1,6,1,14,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,5,1,13,1,0,1,200,1,15,131,0,0,1,248,1,198,1,148,1,171,1,4,1,12,1,0,1,200,1,15,132,0,0,1,198,1,148,1,171,1,3,1,11,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,2,1,10,1,0,1,200,1,15,131,0,0,1,248,1,198,1,148,1,171,1,1,1,9,1,0,1,200,1,15,132,0,0,1,198,1,148,1,171,1,16,1,8,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,15,1,7,1,0,1,200,1,15,131,0,0,1,248,1,198,1,148,1,171,1,14,1,6,1,0,1,200,1,15,132,0,0,1,198,1,148,1,171,1,13,1,5,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,12,1,4,1,0,1,200,1,15,131,0,0,1,248,1,198,1,148,1,171,1,11,1,3,1,0,1,200,1,15,132,0,0,1,198,1,148,1,171,1,10,1,2,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,9,1,1,1,0,1,200,1,15,1,128,131,0,0,1,198,1,248,1,171,1,8,150,0,0,132,255,0,138,0,0,1,3,1,196,1,16,1,42,1,17,1,1,1,0,1,0,1,2,1,68,1,0,1,0,1,1,1,128,135,0,0,1,36,134,0,0,1,1,1,212,138,0,0,1,1,1,172,131,0,0,1,28,1,0,1,0,1,1,1,159,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,134,0,0,1,1,1,152,131,0,0,1,48,1,0,1,2,131,0,0,1,21,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,21,229,0,0,229,0,0,229,0,0,167,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,134,0,0,1,1,1,128,1,0,1,113,1,0,1,8,138,0,0,1,129,1,8,131,0,0,1,1,131,0,0,1,2,131,0,0,1,8,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,4,1,0,1,48,1,80,1,5,1,0,1,0,1,240,1,80,1,0,1,1,1,241,1,81,1,0,1,2,1,242,1,82,1,0,1,3,1,243,1,83,1,0,1,4,1,244,1,84,1,0,1,5,1,245,1,85,1,0,1,6,1,246,1,86,1,0,1,7,1,247,1,87,1,0,1,0,1,16,1,23,1,0,1,0,1,16,1,24,1,0,1,0,1,16,1,25,1,0,1,0,1,16,1,26,1,0,1,0,1,16,1,27,1,0,1,0,1,16,1,28,1,0,1,0,1,16,1,29,1,0,1,0,1,16,1,30,1,48,1,5,1,32,1,4,1,0,1,0,1,18,1,0,1,194,133,0,0,1,64,1,6,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,10,1,96,1,16,1,18,1,0,1,18,133,0,0,1,96,1,22,1,48,1,28,1,18,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,15,1,200,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,16,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,17,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,18,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,19,1,0,1,200,1,3,1,0,1,1,1,0,1,176,1,176,1,0,1,33,1,7,1,20,1,0,1,20,1,12,1,0,1,1,1,0,1,172,1,172,1,108,1,1,1,15,1,20,1,6,1,12,1,28,1,2,1,2,1,0,1,172,1,172,1,108,1,1,1,14,131,20,0,1,3,1,0,1,3,1,0,1,176,1,176,1,177,1,1,1,5,1,20,1,6,1,12,1,44,1,2,1,3,1,0,1,172,1,172,1,177,1,1,1,13,131,20,0,1,3,1,0,1,4,1,0,1,176,1,176,1,108,1,1,1,4,1,20,1,12,1,12,1,67,1,4,1,5,1,0,1,176,1,176,1,108,1,1,1,3,131,20,0,1,12,1,0,1,5,1,0,1,172,1,172,1,177,1,1,1,11,1,20,1,12,1,12,1,131,1,4,1,6,1,0,1,176,1,176,1,177,1,1,1,2,131,20,0,1,12,1,0,1,6,1,0,1,172,1,172,1,108,1,1,1,10,1,20,1,1,1,12,1,28,1,7,1,7,1,0,1,172,1,172,1,108,1,1,1,9,131,20,0,1,3,1,0,1,8,1,0,1,176,1,176,1,177,1,1,1,0,1,20,1,1,1,12,1,44,1,7,1,8,1,0,1,172,1,172,1,177,1,1,1,8,1,20,1,20,1,200,1,15,1,128,131,0,0,1,160,1,0,1,224,1,8,1,0,1,0,1,200,1,15,1,128,1,1,1,0,1,0,1,160,1,0,1,224,1,7,1,0,1,0,1,200,1,15,1,128,1,2,1,0,1,0,1,160,1,0,1,224,1,6,1,0,1,0,1,200,1,15,1,128,1,3,1,0,1,0,1,160,1,0,1,224,1,5,1,0,1,0,1,200,1,15,1,128,1,4,1,0,1,0,1,160,1,0,1,224,1,4,1,0,1,0,1,200,1,15,1,128,1,5,1,0,1,0,1,160,1,0,1,224,1,3,1,0,1,0,1,200,1,15,1,128,1,6,1,0,1,0,1,160,1,0,1,224,1,2,1,0,1,0,1,200,1,15,1,128,1,7,1,0,1,0,1,160,1,0,1,224,1,1,141,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {136,12,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,111,244,107,203,239,248,241,107,210,255,87,191,166,252,253,91,211,255,127,29,253,236,71,207,55,251,252,122,244,255,223,255,178,249,253,167,191,134,163,251,159,245,107,201,119,191,217,175,241,35,186,155,135,233,180,18,58,253,70,191,134,208,229,239,83,58,153,231,215,215,54,205,142,208,236,55,211,118,254,131,87,126,211,200,231,76,227,255,91,96,188,158,103,179,188,118,243,129,182,191,182,254,76,189,119,126,134,190,252,179,188,201,249,203,8,248,223,229,227,164,242,132,38,255,155,126,142,31,127,138,126,253,207,209,31,255,20,253,255,79,213,191,255,51,250,253,63,242,218,153,231,255,166,199,116,179,243,235,226,203,255,251,255,254,191,254,239,242,215,56,121,115,252,228,119,162,63,127,241,175,41,159,225,157,223,73,154,165,45,125,246,84,225,252,102,244,239,115,250,249,251,226,255,191,166,25,215,175,245,107,204,20,168,208,245,175,253,107,127,77,250,230,215,164,81,254,102,94,223,63,159,31,229,165,191,246,215,33,142,251,53,249,63,121,232,227,189,223,127,231,215,248,162,152,214,85,83,157,183,233,214,171,59,233,183,159,191,126,158,10,231,164,39,213,98,85,148,244,203,195,241,222,167,227,135,247,247,198,123,7,251,251,191,198,239,34,211,241,7,17,7,254,77,230,247,95,211,251,253,215,242,126,255,181,189,223,127,29,239,247,95,215,251,253,215,243,126,255,245,221,239,127,210,175,241,27,252,166,127,209,19,158,230,223,148,224,255,103,127,211,175,241,27,252,103,252,55,245,69,125,187,191,169,63,234,211,253,77,125,82,191,238,111,234,151,250,118,127,83,223,212,191,251,155,250,39,28,220,223,132,3,225,33,127,255,186,218,63,245,71,56,252,53,127,209,175,195,226,195,248,208,103,127,205,95,132,207,205,103,132,3,253,63,252,140,240,160,255,135,159,253,58,252,123,248,25,225,67,255,15,63,35,156,232,255,225,103,132,23,253,95,62,163,121,252,181,104,54,129,199,223,36,191,255,90,244,251,255,205,191,255,90,244,249,175,169,159,255,90,244,249,175,169,159,147,100,0,71,253,253,215,162,223,229,243,95,135,62,255,181,245,243,95,135,62,255,181,245,243,95,151,62,255,117,244,243,95,151,62,255,117,244,243,95,143,62,255,117,245,243,95,143,62,255,117,245,243,95,159,62,255,245,244,243,95,159,62,255,245,244,243,223,128,62,255,245,245,243,223,128,62,255,245,233,115,51,151,76,203,112,46,237,223,58,151,246,111,157,75,251,183,206,165,253,91,231,210,254,173,115,105,255,254,13,232,239,223,64,255,246,231,242,55,136,204,229,143,69,230,50,137,204,229,111,24,153,203,223,40,50,151,191,113,100,46,127,147,206,103,192,237,55,245,230,151,100,224,15,194,239,255,55,233,116,243,64,143,154,223,255,48,40,193,95,235,255,34,61,186,182,122,244,47,252,53,229,51,252,248,157,164,89,250,103,209,31,59,248,248,215,128,31,244,107,253,26,7,244,243,219,191,134,177,217,78,103,194,71,250,209,243,205,63,151,31,174,107,255,36,252,254,235,138,174,253,147,126,140,245,210,175,249,15,17,111,252,73,191,217,175,241,159,253,69,242,247,175,197,127,255,230,246,239,95,135,255,254,45,236,223,191,1,255,253,91,210,223,204,91,191,198,175,253,7,253,86,191,134,145,131,95,251,63,162,239,254,34,225,187,255,236,79,194,103,196,19,255,209,175,217,249,236,215,162,207,126,173,206,103,191,54,125,246,107,119,62,251,117,232,179,95,167,243,217,175,75,159,253,186,157,207,126,61,250,236,215,235,124,246,235,211,103,191,126,231,51,242,150,254,163,223,224,215,120,74,159,61,165,207,158,42,126,191,209,127,244,99,157,207,126,45,250,44,233,124,246,107,211,103,191,97,231,51,178,133,255,209,111,212,249,236,215,165,207,126,227,206,103,191,30,125,246,155,116,62,251,245,233,179,223,212,251,12,178,249,255,4,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((Kernel16.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel16.cid1))
			{
				this.SetTextureSize(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector3[] value)
		{
			if ((Kernel16.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel16.cid0))
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((Kernel16.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel16.sid0))
			{
				this.TextureSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((Kernel16.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel16.tid0))
			{
				this.Texture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'Kernel8' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 13 instruction slots used, 13 registers</para><para>Pixel Shader: approximately 17 instruction slots used (8 texture, 9 arithmetic), 8 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class Kernel8 : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'Kernel8' shader</summary>
		public Kernel8()
		{
			this.vreg[12] = new Microsoft.Xna.Framework.Vector4(1F, 1F, 0F, 0F);
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			Kernel8.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			Kernel8.cid0 = state.GetNameUniqueID("kernel");
			Kernel8.cid1 = state.GetNameUniqueID("textureSize");
			Kernel8.sid0 = state.GetNameUniqueID("TextureSampler");
			Kernel8.tid0 = state.GetNameUniqueID("Texture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != Kernel8.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.preg_change = (this.preg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[8], ref this.vreg[9], ref this.vreg[10], ref this.vreg[11], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				Kernel8.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((this.preg_change == true))
			{
				Kernel8.fx.ps_c.SetValue(this.preg);
				this.preg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref Kernel8.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((Kernel8.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((Kernel8.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			Kernel8.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out Kernel8.fx, Kernel8.fxb, 15, 26);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.preg_change) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 2;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(Kernel8.vin[i]));
			index = Kernel8.vin[(i + 2)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary/>
		private bool preg_change;
		/// <summary>Name ID for 'kernel'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float3 kernel[16]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetKernel(Microsoft.Xna.Framework.Vector3[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector3 val;
			int i;
			uint ri;
			uint wi;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 8)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 8)); i = (i + 1))
			{
				val = value[ri];
				this.preg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.preg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float3 kernel[16]'</summary>
		public Microsoft.Xna.Framework.Vector3[] Kernel
		{
			set
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'textureSize'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 textureSize'</summary><param name="value"/>
		public void SetTextureSize(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[12] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 textureSize'</summary>
		public Microsoft.Xna.Framework.Vector2 TextureSize
		{
			set
			{
				this.SetTextureSize(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D TextureSampler'</summary>
		public Xen.Graphics.TextureSamplerState TextureSampler
		{
			get
			{
				return this.pts[0];
			}
			set
			{
				if ((value != this.pts[0]))
				{
					this.pts[0] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D Texture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[0]));
			}
			set
			{
				if ((value != this.ptx[0]))
				{
					this.ptc = true;
					this.ptx[0] = value;
				}
			}
		}
		/// <summary>Name uid for sampler for 'Sampler2D TextureSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D Texture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[13];
		/// <summary>Pixel shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] preg = new Microsoft.Xna.Framework.Vector4[8];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,2,8,135,0,1,3,131,0,1,1,131,0,1,240,135,0,1,13,131,0,1,4,131,0,1,1,229,0,0,229,0,0,137,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,1,0,1,0,1,1,1,152,135,0,0,1,8,131,0,0,1,4,131,0,0,1,1,229,0,0,158,0,0,1,6,1,95,1,112,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,188,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,3,131,0,0,1,1,131,0,0,1,4,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,252,1,0,1,0,1,1,1,24,138,0,0,1,1,1,164,1,0,1,0,1,1,1,184,138,0,0,1,1,1,252,135,0,0,1,1,1,0,1,0,1,1,1,248,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,204,1,0,1,0,1,1,1,200,131,0,0,1,93,134,0,0,1,1,1,228,1,0,1,0,1,1,1,224,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,2,1,96,1,16,1,42,1,17,131,0,0,1,1,1,112,131,0,0,1,240,135,0,0,1,36,134,0,0,1,1,1,48,138,0,0,1,1,1,8,131,0,0,1,28,131,0,0,1,251,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,135,0,0,1,244,131,0,0,1,68,1,0,1,2,131,0,0,1,8,133,0,0,1,76,131,0,0,1,92,131,0,0,1,220,1,0,1,3,131,0,0,1,1,133,0,0,1,228,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,8,229,0,0,161,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,240,1,16,1,0,1,8,132,0,0,1,4,134,0,0,1,65,1,8,1,0,1,255,1,0,1,255,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,49,1,81,1,0,1,0,1,50,1,82,1,0,1,0,1,51,1,83,1,0,1,0,1,52,1,84,1,0,1,0,1,53,1,85,1,0,1,0,1,54,1,86,1,0,1,0,1,55,1,87,1,5,1,85,1,96,1,3,1,32,1,9,1,18,1,0,1,18,1,0,1,0,1,5,132,0,0,1,96,1,11,1,196,1,0,1,18,133,0,0,1,32,1,17,1,0,1,0,1,34,133,0,0,1,16,1,8,1,128,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,16,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,32,1,65,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,48,1,97,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,64,1,129,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,80,1,161,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,96,1,193,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,0,1,225,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,132,0,0,1,198,1,0,1,161,1,0,1,7,1,0,1,200,1,15,131,0,0,1,248,1,198,1,248,1,171,1,6,1,6,1,0,1,200,1,15,132,0,0,1,198,1,148,1,171,1,5,1,5,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,4,1,4,1,0,1,200,1,15,131,0,0,1,248,1,198,1,148,1,171,1,3,1,3,1,0,1,200,1,15,132,0,0,1,198,1,148,1,171,1,2,1,2,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,1,1,1,1,0,1,200,1,15,1,128,131,0,0,1,198,1,248,1,171,1,8,150,0,0,132,255,0,138,0,0,1,2,1,252,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,196,1,0,1,0,1,1,1,56,135,0,0,1,36,134,0,0,1,1,1,84,138,0,0,1,1,1,44,131,0,0,1,28,1,0,1,0,1,1,1,31,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,134,0,0,1,1,1,24,131,0,0,1,48,1,0,1,2,131,0,0,1,13,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,13,229,0,0,229,0,0,140,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,134,0,0,1,1,1,56,1,0,1,113,1,0,1,4,138,0,0,1,65,1,8,131,0,0,1,1,131,0,0,1,2,131,0,0,1,8,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,4,1,0,1,48,1,80,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,2,1,50,1,82,1,0,1,3,1,51,1,83,1,0,1,4,1,52,1,84,1,0,1,5,1,53,1,85,1,0,1,6,1,54,1,86,1,0,1,7,1,55,1,87,1,0,1,0,1,16,1,17,1,0,1,0,1,16,1,18,1,0,1,0,1,16,1,19,1,0,1,0,1,16,1,20,1,0,1,0,1,16,1,21,1,0,1,0,1,16,1,22,1,0,1,0,1,16,1,23,1,0,1,0,1,16,1,24,1,48,1,5,1,32,1,4,1,0,1,0,1,18,1,0,1,194,133,0,0,1,64,1,6,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,10,1,96,1,16,1,18,1,0,1,18,133,0,0,1,48,1,22,1,0,1,0,1,34,133,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,15,1,200,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,8,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,9,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,10,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,11,1,0,1,200,1,3,1,0,1,1,1,0,1,176,1,176,1,0,1,33,1,7,1,12,1,0,1,200,1,12,1,0,1,1,1,0,1,172,1,172,1,0,1,33,1,6,1,12,1,0,1,200,1,3,1,0,1,2,1,0,1,176,1,176,1,0,1,33,1,5,1,12,1,0,1,20,1,12,1,0,1,2,1,0,1,172,1,172,1,108,1,1,1,4,1,12,1,3,1,12,1,28,1,3,1,3,1,0,1,172,1,172,1,108,1,1,1,2,1,12,1,12,1,20,1,3,1,0,1,4,1,0,1,176,1,176,1,177,1,1,1,1,1,12,1,3,1,12,1,44,1,3,1,4,1,0,1,172,1,172,1,177,1,1,1,0,1,12,1,12,1,200,1,3,1,128,1,0,1,0,1,26,1,176,1,0,1,224,1,4,1,0,1,0,1,200,1,3,1,128,1,1,1,0,1,176,1,176,1,0,1,224,1,4,1,0,1,0,1,200,1,3,1,128,1,2,1,0,1,26,1,176,1,0,1,224,1,3,1,0,1,0,1,200,1,3,1,128,1,3,1,0,1,176,1,176,1,0,1,224,1,3,1,0,1,0,1,200,1,3,1,128,2,4,0,3,26,176,0,4,224,2,0,0,5,200,3,128,5,0,6,176,176,0,224,2,0,7,0,200,3,128,6,0,26,8,176,0,224,1,0,0,200,3,8,128,7,0,176,176,0,224,1,141,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {8,8,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,111,244,107,201,239,191,54,254,166,255,255,79,250,221,111,76,255,255,117,244,179,255,191,61,191,30,253,255,247,191,108,126,255,233,175,225,198,253,103,233,64,127,131,95,227,255,61,227,102,60,87,130,231,111,244,107,8,94,127,95,7,177,95,95,219,52,59,130,243,111,166,237,252,7,83,252,155,70,62,103,80,255,247,255,253,127,3,198,235,121,54,203,107,71,15,180,253,181,245,103,234,189,243,127,208,255,127,59,15,135,191,140,126,255,187,124,156,148,159,240,209,255,166,159,227,163,63,69,191,254,231,232,179,127,138,254,255,167,234,223,255,25,253,254,31,121,237,204,67,104,253,223,6,236,191,132,47,126,173,255,251,255,254,191,254,239,231,191,198,201,155,227,39,191,19,253,249,171,127,13,249,12,95,253,78,220,234,215,72,127,57,253,243,84,225,252,6,244,239,115,250,249,251,210,255,255,189,95,195,140,235,215,250,53,254,51,133,41,116,253,107,255,218,95,147,190,249,53,105,148,191,129,126,254,115,253,232,92,254,181,191,14,205,248,175,201,255,201,67,31,239,253,254,59,191,198,23,197,180,174,154,234,188,77,183,94,221,73,191,253,252,245,243,84,102,46,61,169,22,171,162,164,95,30,142,247,62,29,63,188,191,55,222,59,216,223,255,53,126,23,33,199,31,68,20,248,155,204,239,191,166,247,251,175,229,253,254,107,123,191,255,58,222,239,191,174,247,251,175,231,253,254,235,187,223,255,164,95,227,55,248,77,255,162,39,76,230,223,148,224,255,103,127,211,175,241,27,252,103,252,247,175,73,127,255,26,222,223,191,22,253,253,107,121,127,255,218,244,247,175,237,253,253,235,208,223,191,142,247,247,175,75,127,255,186,222,223,191,30,253,253,235,121,127,255,250,244,247,175,175,127,255,186,218,63,245,71,56,252,53,127,209,175,195,236,203,248,208,103,127,205,95,132,207,205,103,132,3,253,63,252,140,240,160,255,135,159,253,58,252,123,248,25,225,67,255,15,63,35,156,232,255,225,103,132,23,253,95,62,251,53,65,167,223,64,112,251,191,255,239,95,195,62,224,115,243,251,140,249,252,255,34,62,255,202,242,249,239,242,107,202,103,248,97,248,28,242,183,243,107,8,225,127,99,250,247,128,126,126,251,215,48,58,205,241,52,116,248,255,31,159,203,15,150,133,223,244,79,194,239,191,46,203,194,111,250,39,253,24,243,205,175,249,15,209,220,252,73,191,193,175,241,159,253,69,242,247,175,197,127,255,152,253,251,215,225,191,19,251,247,111,192,127,255,134,244,55,207,237,175,241,107,255,65,191,17,253,46,115,255,107,255,71,244,221,95,36,243,254,159,253,73,248,140,230,132,212,92,248,25,201,222,127,244,107,117,62,35,25,252,143,126,237,206,103,36,139,255,209,175,211,249,140,100,242,63,250,117,59,159,145,108,254,71,191,94,231,51,146,209,255,232,215,247,62,3,239,253,63,1,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((Kernel8.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel8.cid1))
			{
				this.SetTextureSize(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector3[] value)
		{
			if ((Kernel8.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel8.cid0))
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((Kernel8.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel8.sid0))
			{
				this.TextureSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((Kernel8.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel8.tid0))
			{
				this.Texture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'Kernel4' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 9 instruction slots used, 9 registers</para><para>Pixel Shader: approximately 9 instruction slots used (4 texture, 5 arithmetic), 4 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class Kernel4 : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'Kernel4' shader</summary>
		public Kernel4()
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(1F, 1F, 0F, 0F);
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			Kernel4.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			Kernel4.cid0 = state.GetNameUniqueID("kernel");
			Kernel4.cid1 = state.GetNameUniqueID("textureSize");
			Kernel4.sid0 = state.GetNameUniqueID("TextureSampler");
			Kernel4.tid0 = state.GetNameUniqueID("Texture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != Kernel4.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.preg_change = (this.preg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				Kernel4.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((this.preg_change == true))
			{
				Kernel4.fx.ps_c.SetValue(this.preg);
				this.preg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref Kernel4.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((Kernel4.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((Kernel4.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			Kernel4.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out Kernel4.fx, Kernel4.fxb, 11, 14);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.preg_change) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 2;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(Kernel4.vin[i]));
			index = Kernel4.vin[(i + 2)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary/>
		private bool preg_change;
		/// <summary>Name ID for 'kernel'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float3 kernel[16]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetKernel(Microsoft.Xna.Framework.Vector3[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector3 val;
			int i;
			uint ri;
			uint wi;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 4)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 4)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 4)); i = (i + 1))
			{
				val = value[ri];
				this.preg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.preg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float3 kernel[16]'</summary>
		public Microsoft.Xna.Framework.Vector3[] Kernel
		{
			set
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'textureSize'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 textureSize'</summary><param name="value"/>
		public void SetTextureSize(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 textureSize'</summary>
		public Microsoft.Xna.Framework.Vector2 TextureSize
		{
			set
			{
				this.SetTextureSize(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D TextureSampler'</summary>
		public Xen.Graphics.TextureSamplerState TextureSampler
		{
			get
			{
				return this.pts[0];
			}
			set
			{
				if ((value != this.pts[0]))
				{
					this.pts[0] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D Texture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[0]));
			}
			set
			{
				if ((value != this.ptx[0]))
				{
					this.ptc = true;
					this.ptx[0] = value;
				}
			}
		}
		/// <summary>Name uid for sampler for 'Sampler2D TextureSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D Texture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[9];
		/// <summary>Pixel shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] preg = new Microsoft.Xna.Framework.Vector4[4];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,136,135,0,1,3,131,0,1,1,131,0,1,176,135,0,1,9,131,0,1,4,131,0,1,1,229,0,0,174,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,1,0,1,0,1,1,1,24,135,0,0,1,4,131,0,0,1,4,131,0,0,1,1,195,0,0,1,6,1,95,1,112,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,60,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,3,131,0,0,1,1,131,0,0,1,4,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,188,131,0,0,1,216,138,0,0,1,1,1,36,1,0,1,0,1,1,1,56,138,0,0,1,1,1,124,135,0,0,1,1,1,0,1,0,1,1,1,120,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,76,1,0,1,0,1,1,1,72,131,0,0,1,93,134,0,0,1,1,1,100,1,0,1,0,1,1,1,96,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,164,1,16,1,42,1,17,131,0,0,1,1,1,32,131,0,0,1,132,135,0,0,1,36,135,0,0,1,240,139,0,0,1,200,131,0,0,1,28,131,0,0,1,187,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,135,0,0,1,180,131,0,0,1,68,1,0,1,2,131,0,0,1,4,133,0,0,1,76,131,0,0,1,92,131,0,0,1,156,1,0,1,3,131,0,0,1,1,133,0,0,1,164,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,4,198,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,132,1,16,1,0,1,4,132,0,0,1,4,134,0,0,1,32,1,132,1,0,1,15,1,0,1,15,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,49,1,81,1,0,1,0,1,50,1,82,1,0,1,0,1,51,1,83,1,0,1,85,1,64,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,64,1,6,1,0,1,0,1,34,133,0,0,1,16,1,8,1,64,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,16,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,32,1,65,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,0,1,97,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,132,0,0,1,198,1,0,1,161,1,0,1,3,1,0,1,200,1,15,132,0,0,1,198,1,0,1,171,1,2,1,2,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,1,1,1,1,0,1,200,1,15,1,128,131,0,0,1,198,1,248,1,171,1,4,150,0,0,132,255,0,138,0,0,1,2,1,60,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,100,131,0,0,1,216,135,0,0,1,36,134,0,0,1,1,1,20,139,0,0,1,236,131,0,0,1,28,131,0,0,1,223,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,216,131,0,0,1,48,1,0,1,2,131,0,0,1,9,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,9,229,0,0,177,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,216,1,0,1,49,1,0,1,2,138,0,0,1,32,1,132,131,0,0,1,1,131,0,0,1,2,131,0,0,1,4,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,48,1,80,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,2,1,50,1,82,1,0,1,3,1,51,1,83,1,0,1,0,1,16,1,13,1,0,1,0,1,16,1,14,1,0,1,0,1,16,1,15,1,0,1,0,1,16,1,16,1,48,1,5,1,32,1,3,1,0,1,0,1,18,1,0,1,194,133,0,0,1,64,1,5,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,9,1,32,1,15,1,18,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,15,1,200,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,0,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,1,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,2,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,3,1,0,1,200,1,3,1,0,1,1,1,0,1,176,1,176,1,0,1,33,1,7,1,8,1,0,1,200,1,12,1,0,1,1,1,0,1,172,1,172,1,0,1,33,1,6,1,8,1,0,1,200,1,3,1,0,1,2,1,0,1,176,1,176,1,0,1,33,1,5,1,8,1,0,1,200,1,12,1,0,1,2,1,0,1,172,1,172,1,0,1,33,1,4,1,8,1,0,2,200,3,3,128,0,0,4,26,176,0,224,5,2,0,0,200,3,6,128,1,0,176,176,0,7,224,2,0,0,200,3,128,8,2,0,26,176,0,224,1,0,9,0,200,3,128,3,0,176,176,0,2,224,1,141,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {248,5,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,31,247,107,202,239,191,54,254,166,255,255,77,250,221,143,209,255,127,29,253,236,255,109,207,175,71,255,255,253,47,155,223,127,250,107,56,188,127,59,69,244,215,209,255,127,19,120,115,63,43,233,231,55,250,53,4,238,227,14,224,95,95,219,52,59,210,231,111,166,237,252,231,215,162,255,255,166,145,207,25,212,255,253,127,255,223,128,241,122,158,205,242,218,141,7,109,127,109,253,153,122,239,252,125,244,255,127,203,251,251,119,163,198,7,30,78,127,144,254,142,31,239,244,119,244,255,167,232,247,207,233,179,111,211,255,255,84,253,123,70,191,255,1,94,59,243,16,90,255,183,1,251,47,225,151,95,235,255,254,191,255,175,255,251,241,175,113,242,230,248,201,239,68,127,254,189,191,134,124,134,119,126,39,110,245,107,164,127,27,253,243,84,225,252,58,244,239,115,250,249,251,210,255,255,188,95,195,140,235,215,250,53,254,50,133,41,116,253,107,255,218,95,147,190,249,53,169,117,151,54,95,247,209,185,248,107,127,29,154,177,95,147,255,147,135,62,222,251,253,119,126,141,47,138,105,93,53,213,121,155,110,189,186,147,126,251,249,235,231,169,80,62,61,169,22,171,162,164,95,30,142,247,62,29,63,188,191,55,222,59,216,223,255,53,126,23,25,206,31,68,35,248,155,204,239,191,166,247,251,175,229,253,254,107,187,223,255,164,95,227,55,248,77,255,162,39,60,236,223,148,218,255,103,127,211,175,241,27,252,103,252,247,175,73,127,255,26,222,223,191,22,253,253,107,121,127,255,218,244,247,175,173,127,255,186,250,62,181,39,24,127,205,95,244,235,48,59,48,60,250,236,175,249,139,240,185,249,140,96,208,255,195,207,8,14,253,95,62,251,53,129,215,111,32,176,254,239,255,219,209,11,243,108,126,255,215,120,158,255,47,154,231,83,59,207,255,225,175,33,159,225,43,51,207,255,22,253,179,243,107,200,64,127,140,254,61,160,159,223,254,53,140,76,186,57,133,14,249,127,227,115,249,193,188,240,155,254,73,248,253,215,101,94,248,77,255,164,31,227,121,250,53,255,33,162,237,159,68,255,255,139,228,239,95,139,255,254,53,237,223,191,14,255,253,107,217,191,127,3,254,155,230,232,47,226,185,249,53,126,237,63,232,55,160,223,101,238,126,237,255,232,215,161,223,101,222,254,179,63,9,159,17,77,255,163,95,183,243,25,241,222,127,244,235,117,62,35,30,252,143,126,125,239,51,204,245,255,19,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((Kernel4.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel4.cid1))
			{
				this.SetTextureSize(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector3[] value)
		{
			if ((Kernel4.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel4.cid0))
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((Kernel4.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel4.sid0))
			{
				this.TextureSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((Kernel4.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel4.tid0))
			{
				this.Texture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'Kernel15' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 21 instruction slots used, 21 registers</para><para>Pixel Shader: approximately 45 instruction slots used (15 texture, 30 arithmetic), 15 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class Kernel15 : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'Kernel15' shader</summary>
		public Kernel15()
		{
			this.vreg[20] = new Microsoft.Xna.Framework.Vector4(1F, 1F, 0F, 0F);
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			Kernel15.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			Kernel15.cid0 = state.GetNameUniqueID("kernel");
			Kernel15.cid1 = state.GetNameUniqueID("textureSize");
			Kernel15.sid0 = state.GetNameUniqueID("TextureSampler");
			Kernel15.tid0 = state.GetNameUniqueID("Texture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != Kernel15.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.preg_change = (this.preg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[16], ref this.vreg[17], ref this.vreg[18], ref this.vreg[19], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				Kernel15.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((this.preg_change == true))
			{
				Kernel15.fx.ps_c.SetValue(this.preg);
				this.preg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref Kernel15.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((Kernel15.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((Kernel15.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			Kernel15.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out Kernel15.fx, Kernel15.fxb, 23, 54);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.preg_change) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 2;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(Kernel15.vin[i]));
			index = Kernel15.vin[(i + 2)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary/>
		private bool preg_change;
		/// <summary>Name ID for 'kernel'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float3 kernel[16]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetKernel(Microsoft.Xna.Framework.Vector3[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector3 val;
			int i;
			uint ri;
			uint wi;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 16)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 15)); i = (i + 1))
			{
				val = value[ri];
				this.preg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.preg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float3 kernel[16]'</summary>
		public Microsoft.Xna.Framework.Vector3[] Kernel
		{
			set
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'textureSize'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 textureSize'</summary><param name="value"/>
		public void SetTextureSize(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[20] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 textureSize'</summary>
		public Microsoft.Xna.Framework.Vector2 TextureSize
		{
			set
			{
				this.SetTextureSize(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D TextureSampler'</summary>
		public Xen.Graphics.TextureSamplerState TextureSampler
		{
			get
			{
				return this.pts[0];
			}
			set
			{
				if ((value != this.pts[0]))
				{
					this.pts[0] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D Texture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[0]));
			}
			set
			{
				if ((value != this.ptx[0]))
				{
					this.ptc = true;
					this.ptx[0] = value;
				}
			}
		}
		/// <summary>Name uid for sampler for 'Sampler2D TextureSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D Texture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[21];
		/// <summary>Pixel shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] preg = new Microsoft.Xna.Framework.Vector4[15];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,2,248,135,0,1,3,131,0,5,1,0,0,1,112,135,0,1,21,131,0,1,4,131,0,1,1,229,0,0,229,0,0,229,0,0,164,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,1,0,1,0,1,2,1,136,135,0,0,1,15,131,0,0,1,4,131,0,0,1,1,229,0,0,229,0,0,169,0,0,1,6,1,95,1,112,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,2,1,172,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,3,131,0,0,1,1,131,0,0,1,4,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,138,0,0,1,1,1,124,1,0,1,0,1,1,1,152,138,0,0,1,2,1,148,1,0,1,0,1,2,1,168,138,0,0,1,2,1,236,135,0,0,1,1,1,0,1,0,1,2,1,232,135,0,0,1,2,131,0,0,1,92,134,0,0,1,2,1,188,1,0,1,0,1,2,1,184,131,0,0,1,93,134,0,0,1,2,1,212,1,0,1,0,1,2,1,208,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,3,1,132,1,16,1,42,1,17,131,0,0,1,1,1,224,1,0,1,0,1,1,1,164,135,0,0,1,36,134,0,0,1,1,1,160,138,0,0,1,1,1,120,131,0,0,1,28,1,0,1,0,1,1,1,107,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,134,0,0,1,1,1,100,131,0,0,1,68,1,0,1,2,131,0,0,1,15,133,0,0,1,76,131,0,0,1,92,1,0,1,0,1,1,1,76,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,84,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,15,229,0,0,229,0,0,172,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,134,0,0,1,1,1,164,1,16,1,0,1,15,132,0,0,1,4,134,0,0,1,121,1,8,1,0,1,255,1,0,1,255,131,0,0,1,1,1,0,1,0,1,240,1,80,1,0,1,0,1,241,1,81,1,0,1,0,1,242,1,82,1,0,1,0,1,243,1,83,1,0,1,0,1,244,1,84,1,0,1,0,1,245,1,85,1,0,1,0,1,246,1,86,1,0,1,0,1,55,1,87,1,5,1,85,1,96,1,4,1,96,1,10,1,18,1,0,1,18,1,0,1,5,1,85,1,0,1,21,1,48,1,16,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,19,1,96,1,25,1,18,1,0,1,18,133,0,0,1,48,1,31,1,0,1,0,1,34,133,0,0,1,16,1,8,1,128,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,144,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,160,1,65,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,176,1,97,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,192,1,129,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,208,1,161,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,224,1,193,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,112,1,225,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,240,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,16,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,32,1,65,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,48,1,97,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,64,1,129,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,80,1,161,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,184,1,8,1,0,1,193,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,132,0,0,1,198,1,0,1,161,1,0,1,14,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,5,1,13,1,0,1,200,1,15,131,0,0,1,248,1,198,1,148,1,171,1,4,1,12,1,0,1,200,1,15,132,0,0,1,198,1,148,1,171,1,3,1,11,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,2,1,10,1,0,1,200,1,15,131,0,0,1,248,1,198,1,148,1,171,1,1,1,9,1,0,1,200,1,15,132,0,0,1,198,1,148,1,171,1,15,1,8,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,7,1,7,1,0,1,200,1,15,131,0,0,1,248,1,198,1,148,1,171,1,14,1,6,1,0,1,200,1,15,132,0,0,1,198,1,148,1,171,1,13,1,5,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,12,1,4,1,0,1,200,1,15,131,0,0,1,248,1,198,1,148,1,171,1,11,1,3,1,0,1,200,1,15,132,0,0,1,198,1,148,1,171,1,10,1,2,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,9,1,1,1,0,1,200,1,15,1,128,131,0,0,1,198,1,248,1,171,1,8,150,0,0,132,255,0,138,0,0,1,3,1,196,1,16,1,42,1,17,1,1,1,0,1,0,1,2,1,68,1,0,1,0,1,1,1,128,135,0,0,1,36,134,0,0,1,1,1,212,138,0,0,1,1,1,172,131,0,0,1,28,1,0,1,0,1,1,1,159,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,134,0,0,1,1,1,152,131,0,0,1,48,1,0,1,2,131,0,0,1,21,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,21,229,0,0,229,0,0,229,0,0,167,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,134,0,0,1,1,1,128,1,0,1,113,1,0,1,8,138,0,0,1,129,1,8,131,0,0,1,1,131,0,0,1,2,131,0,0,1,8,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,4,1,0,1,48,1,80,1,5,1,0,1,0,1,240,1,80,1,0,1,1,1,241,1,81,1,0,1,2,1,242,1,82,1,0,1,3,1,243,1,83,1,0,1,4,1,244,1,84,1,0,1,5,1,245,1,85,1,0,1,6,1,246,1,86,1,0,1,7,1,247,1,87,1,0,1,0,1,16,1,23,1,0,1,0,1,16,1,24,1,0,1,0,1,16,1,25,1,0,1,0,1,16,1,26,1,0,1,0,1,16,1,27,1,0,1,0,1,16,1,28,1,0,1,0,1,16,1,29,1,0,1,0,1,16,1,30,1,48,1,5,1,32,1,4,1,0,1,0,1,18,1,0,1,194,133,0,0,1,64,1,6,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,10,1,96,1,16,1,18,1,0,1,18,133,0,0,1,96,1,22,1,48,1,28,1,18,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,15,1,200,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,16,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,17,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,18,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,19,1,0,1,200,1,3,1,0,1,1,1,0,1,176,1,176,1,0,1,33,1,7,1,20,1,0,1,20,1,12,1,0,1,1,1,0,1,172,1,172,1,108,1,1,1,15,1,20,1,6,1,12,1,28,1,2,1,2,1,0,1,172,1,172,1,108,1,1,1,14,131,20,0,1,3,1,0,1,3,1,0,1,176,1,176,1,177,1,1,1,5,1,20,1,6,1,12,1,44,1,2,1,3,1,0,1,172,1,172,1,177,1,1,1,13,131,20,0,1,3,1,0,1,4,1,0,1,176,1,176,1,108,1,1,1,4,1,20,1,12,1,12,1,67,1,4,1,5,1,0,1,176,1,176,1,108,1,1,1,3,131,20,0,1,12,1,0,1,5,1,0,1,172,1,172,1,177,1,1,1,11,1,20,1,12,1,12,1,131,1,4,1,6,1,0,1,176,1,176,1,177,1,1,1,2,131,20,0,1,12,1,0,1,6,1,0,1,172,1,172,1,108,1,1,1,10,1,20,1,1,1,12,1,28,1,7,1,7,1,0,1,172,1,172,1,108,1,1,1,9,131,20,0,1,3,1,0,1,8,1,0,1,176,1,176,1,177,1,1,1,0,1,20,1,1,1,12,1,44,1,7,1,8,1,0,1,172,1,172,1,177,1,1,1,8,1,20,1,20,1,200,1,15,1,128,131,0,0,1,160,1,0,1,224,1,8,1,0,1,0,1,200,1,15,1,128,1,1,1,0,1,0,1,160,1,0,1,224,1,7,1,0,1,0,1,200,1,15,1,128,1,2,1,0,1,0,1,160,1,0,1,224,1,6,1,0,1,0,1,200,1,15,1,128,1,3,1,0,1,0,1,160,1,0,1,224,1,5,1,0,1,0,1,200,1,15,1,128,1,4,1,0,1,0,1,160,1,0,1,224,1,4,1,0,1,0,1,200,1,15,1,128,1,5,1,0,1,0,1,160,1,0,1,224,1,3,1,0,1,0,1,200,1,15,1,128,1,6,1,0,1,0,1,160,1,0,1,224,1,2,1,0,1,0,1,200,1,15,1,128,1,7,1,0,1,0,1,160,1,0,1,224,1,1,141,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {44,12,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,255,241,107,201,239,191,54,254,166,255,175,126,77,249,251,183,166,255,255,58,250,217,143,158,111,246,249,245,232,255,191,255,101,243,251,79,127,141,63,222,210,253,143,210,121,248,77,127,141,159,191,116,103,186,172,64,151,95,227,215,248,141,126,13,161,195,95,167,116,49,207,175,175,109,154,29,161,209,111,166,237,252,7,175,24,58,250,15,211,244,255,254,191,255,111,192,120,61,207,102,121,237,248,30,109,127,109,253,153,122,239,252,12,125,249,103,121,147,241,167,17,240,191,202,195,233,127,210,223,209,228,191,210,223,241,227,79,209,239,255,62,250,227,239,162,255,255,169,250,247,191,70,191,255,75,94,59,243,16,90,255,183,233,230,63,3,18,191,214,255,253,127,255,95,255,247,252,215,56,121,115,252,228,119,162,63,223,254,154,242,25,222,249,157,164,89,58,163,207,158,42,156,223,148,254,125,78,63,127,95,250,255,243,95,211,140,235,215,250,53,222,40,80,161,235,95,251,215,254,154,244,205,175,73,163,252,77,229,227,159,119,143,242,206,95,251,235,16,135,253,154,252,159,60,244,241,222,239,191,243,107,124,81,76,235,170,169,206,219,116,235,213,157,244,219,207,95,63,79,133,83,210,147,106,177,42,74,250,229,225,120,239,211,241,195,251,123,227,189,131,253,253,95,227,119,17,242,255,65,52,3,127,147,249,253,215,244,126,255,181,188,223,127,109,239,247,95,199,251,253,215,245,126,255,245,188,223,127,253,95,227,215,54,191,255,73,191,198,111,240,155,254,69,79,120,90,127,83,130,255,159,253,77,191,198,111,240,159,241,223,212,23,245,237,254,166,254,168,79,247,55,245,73,253,186,191,169,95,234,219,253,77,125,83,255,238,111,234,159,112,112,127,255,250,244,247,175,175,127,255,186,218,63,245,71,56,252,53,127,209,175,195,226,194,248,208,103,127,205,95,132,207,205,103,132,3,253,63,252,140,240,160,255,135,159,253,58,252,123,248,25,225,67,255,15,63,35,156,232,255,225,103,132,23,253,95,62,163,121,252,181,104,54,129,199,223,36,191,255,90,244,251,255,205,191,255,90,244,249,175,169,159,255,90,244,249,175,169,159,147,36,0,71,253,253,215,162,223,229,243,95,135,62,255,181,245,243,95,135,62,255,181,245,243,95,151,62,255,117,244,243,95,151,62,255,117,244,243,95,143,62,255,117,245,243,95,143,62,255,117,245,243,95,159,62,255,245,244,243,95,159,62,255,245,232,115,51,103,76,179,112,206,236,223,58,103,246,111,157,51,251,183,206,153,253,91,231,204,254,173,115,198,127,251,115,244,27,68,230,232,199,34,115,148,68,230,232,55,140,204,209,111,20,153,163,223,56,50,71,191,137,55,71,196,199,204,63,255,247,255,253,107,216,7,186,207,252,254,135,65,113,253,90,255,23,233,190,181,213,125,127,225,175,41,159,225,199,239,36,205,82,232,228,29,124,252,107,192,87,249,181,126,141,3,250,249,237,95,195,216,85,167,231,224,199,252,232,249,230,159,203,15,215,151,127,18,126,255,117,69,95,254,73,63,198,186,229,215,252,135,136,55,254,164,223,236,215,248,207,254,34,249,251,215,226,191,127,115,251,247,175,195,127,255,22,246,239,223,128,255,254,45,233,111,230,173,95,227,215,254,131,126,171,95,195,240,252,175,253,31,209,119,127,145,240,221,127,246,39,225,51,226,137,255,232,215,236,124,246,107,209,103,191,86,231,179,95,155,62,251,181,59,159,253,58,244,217,175,211,249,236,215,165,207,126,221,206,103,191,30,125,246,235,117,62,35,61,254,31,253,250,157,207,200,195,249,143,126,131,95,227,41,125,246,148,62,123,170,248,253,70,255,209,143,117,62,251,181,232,179,164,243,217,175,77,159,253,134,157,207,200,158,253,71,191,81,231,179,95,151,62,251,141,59,159,253,122,244,217,111,210,249,236,215,167,207,126,83,239,51,200,230,255,19,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((Kernel15.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel15.cid1))
			{
				this.SetTextureSize(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector3[] value)
		{
			if ((Kernel15.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel15.cid0))
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((Kernel15.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel15.sid0))
			{
				this.TextureSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((Kernel15.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel15.tid0))
			{
				this.Texture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'Kernel7' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 13 instruction slots used, 13 registers</para><para>Pixel Shader: approximately 15 instruction slots used (7 texture, 8 arithmetic), 7 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class Kernel7 : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'Kernel7' shader</summary>
		public Kernel7()
		{
			this.vreg[12] = new Microsoft.Xna.Framework.Vector4(1F, 1F, 0F, 0F);
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			Kernel7.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			Kernel7.cid0 = state.GetNameUniqueID("kernel");
			Kernel7.cid1 = state.GetNameUniqueID("textureSize");
			Kernel7.sid0 = state.GetNameUniqueID("TextureSampler");
			Kernel7.tid0 = state.GetNameUniqueID("Texture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != Kernel7.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.preg_change = (this.preg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[8], ref this.vreg[9], ref this.vreg[10], ref this.vreg[11], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				Kernel7.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((this.preg_change == true))
			{
				Kernel7.fx.ps_c.SetValue(this.preg);
				this.preg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref Kernel7.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((Kernel7.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((Kernel7.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			Kernel7.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out Kernel7.fx, Kernel7.fxb, 15, 23);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.preg_change) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 2;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(Kernel7.vin[i]));
			index = Kernel7.vin[(i + 2)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary/>
		private bool preg_change;
		/// <summary>Name ID for 'kernel'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float3 kernel[16]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetKernel(Microsoft.Xna.Framework.Vector3[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector3 val;
			int i;
			uint ri;
			uint wi;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 8)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 7)); i = (i + 1))
			{
				val = value[ri];
				this.preg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.preg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float3 kernel[16]'</summary>
		public Microsoft.Xna.Framework.Vector3[] Kernel
		{
			set
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'textureSize'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 textureSize'</summary><param name="value"/>
		public void SetTextureSize(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[12] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 textureSize'</summary>
		public Microsoft.Xna.Framework.Vector2 TextureSize
		{
			set
			{
				this.SetTextureSize(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D TextureSampler'</summary>
		public Xen.Graphics.TextureSamplerState TextureSampler
		{
			get
			{
				return this.pts[0];
			}
			set
			{
				if ((value != this.pts[0]))
				{
					this.pts[0] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D Texture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[0]));
			}
			set
			{
				if ((value != this.ptx[0]))
				{
					this.ptc = true;
					this.ptx[0] = value;
				}
			}
		}
		/// <summary>Name uid for sampler for 'Sampler2D TextureSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D Texture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[13];
		/// <summary>Pixel shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] preg = new Microsoft.Xna.Framework.Vector4[7];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,248,135,0,1,3,131,0,1,1,131,0,1,240,135,0,1,13,131,0,1,4,131,0,1,1,229,0,0,229,0,0,137,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,1,0,1,0,1,1,1,136,135,0,0,1,7,131,0,0,1,4,131,0,0,1,1,229,0,0,142,0,0,1,6,1,95,1,112,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,172,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,3,131,0,0,1,1,131,0,0,1,4,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,252,1,0,1,0,1,1,1,24,138,0,0,1,1,1,148,1,0,1,0,1,1,1,168,138,0,0,1,1,1,236,135,0,0,1,1,1,0,1,0,1,1,1,232,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,188,1,0,1,0,1,1,1,184,131,0,0,1,93,134,0,0,1,1,1,212,1,0,1,0,1,1,1,208,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,2,1,52,1,16,1,42,1,17,131,0,0,1,1,1,92,131,0,0,1,216,135,0,0,1,36,134,0,0,1,1,1,32,139,0,0,1,248,131,0,0,1,28,131,0,0,1,235,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,135,0,0,1,228,131,0,0,1,68,1,0,1,2,131,0,0,1,7,133,0,0,1,76,131,0,0,1,92,131,0,0,1,204,1,0,1,3,131,0,0,1,1,133,0,0,1,212,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,7,229,0,0,145,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,216,1,16,1,0,1,7,132,0,0,1,4,134,0,0,1,56,1,231,1,0,1,127,1,0,1,127,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,49,1,81,1,0,1,0,1,50,1,82,1,0,1,0,1,51,1,83,1,0,1,0,1,52,1,84,1,0,1,0,1,53,1,85,1,0,1,0,1,54,1,86,1,5,1,85,1,96,1,3,1,16,1,9,1,18,1,0,1,18,1,0,1,0,1,1,132,0,0,1,96,1,10,1,196,1,0,1,18,133,0,0,1,16,1,16,1,0,1,0,1,34,133,0,0,1,16,1,8,1,112,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,16,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,32,1,65,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,48,1,97,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,64,1,129,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,80,1,161,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,0,1,193,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,132,0,0,1,198,1,0,1,161,1,0,1,6,1,0,1,200,1,15,132,0,0,1,198,1,0,1,171,1,5,1,5,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,4,1,4,1,0,1,200,1,15,131,0,0,1,248,1,198,1,148,1,171,1,3,1,3,1,0,1,200,1,15,132,0,0,1,198,1,148,1,171,1,2,1,2,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,1,1,1,1,0,1,200,1,15,1,128,131,0,0,1,198,1,248,1,171,1,7,150,0,0,132,255,0,138,0,0,1,2,1,252,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,196,1,0,1,0,1,1,1,56,135,0,0,1,36,134,0,0,1,1,1,84,138,0,0,1,1,1,44,131,0,0,1,28,1,0,1,0,1,1,1,31,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,134,0,0,1,1,1,24,131,0,0,1,48,1,0,1,2,131,0,0,1,13,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,13,229,0,0,229,0,0,140,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,134,0,0,1,1,1,56,1,0,1,113,1,0,1,4,138,0,0,1,65,1,8,131,0,0,1,1,131,0,0,1,2,131,0,0,1,8,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,4,1,0,1,48,1,80,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,2,1,50,1,82,1,0,1,3,1,51,1,83,1,0,1,4,1,52,1,84,1,0,1,5,1,53,1,85,1,0,1,6,1,54,1,86,1,0,1,7,1,55,1,87,1,0,1,0,1,16,1,17,1,0,1,0,1,16,1,18,1,0,1,0,1,16,1,19,1,0,1,0,1,16,1,20,1,0,1,0,1,16,1,21,1,0,1,0,1,16,1,22,1,0,1,0,1,16,1,23,1,0,1,0,1,16,1,24,1,48,1,5,1,32,1,4,1,0,1,0,1,18,1,0,1,194,133,0,0,1,64,1,6,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,10,1,96,1,16,1,18,1,0,1,18,133,0,0,1,48,1,22,1,0,1,0,1,34,133,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,15,1,200,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,8,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,9,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,10,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,11,1,0,1,200,1,3,1,0,1,1,1,0,1,176,1,176,1,0,1,33,1,7,1,12,1,0,1,200,1,12,1,0,1,1,1,0,1,172,1,172,1,0,1,33,1,6,1,12,1,0,1,200,1,3,1,0,1,2,1,0,1,176,1,176,1,0,1,33,1,5,1,12,1,0,1,20,1,12,1,0,1,2,1,0,1,172,1,172,1,108,1,1,1,4,1,12,1,3,1,12,1,28,1,3,1,3,1,0,1,172,1,172,1,108,1,1,1,2,1,12,1,12,1,20,1,3,1,0,1,4,1,0,1,176,1,176,1,177,1,1,1,1,1,12,1,3,1,12,1,44,1,3,1,4,1,0,1,172,1,172,1,177,1,1,1,0,1,12,1,12,1,200,1,3,1,128,1,0,1,0,1,26,1,176,1,0,1,224,1,4,1,0,1,0,1,200,1,3,1,128,1,1,1,0,1,176,1,176,1,0,1,224,1,4,1,0,1,0,1,200,1,3,1,128,1,2,1,0,1,26,1,176,1,0,1,224,1,3,1,0,1,0,1,200,1,3,1,128,1,3,1,0,1,176,1,176,1,0,1,224,1,3,2,0,0,3,200,3,128,4,4,0,26,176,5,0,224,2,0,0,6,200,3,128,5,0,176,7,176,0,224,2,0,0,200,8,3,128,6,0,26,176,0,224,9,1,0,0,200,3,128,7,0,176,4,176,0,224,1,141,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {184,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,255,241,107,202,239,191,54,254,166,255,255,79,250,221,111,76,255,255,117,244,179,255,191,61,191,30,253,255,247,191,108,126,255,233,175,225,198,253,71,233,64,127,253,95,227,231,110,220,140,215,74,240,250,141,126,13,193,227,175,235,32,2,252,208,166,217,17,28,127,51,109,231,63,191,22,253,255,55,141,124,206,160,254,239,255,251,255,6,140,215,243,108,150,215,110,252,104,251,107,235,207,212,123,231,255,160,255,255,118,30,14,127,26,253,254,87,121,127,255,79,250,59,126,252,87,250,59,250,255,83,244,251,191,143,62,251,187,232,255,127,170,254,253,175,209,239,255,146,215,206,60,132,214,255,109,192,254,73,248,226,215,250,191,255,239,255,235,255,254,246,175,113,242,230,248,201,239,68,127,254,183,191,134,124,134,175,126,39,110,245,107,164,255,25,253,243,84,225,252,250,244,239,115,250,249,251,210,255,255,185,95,195,140,235,215,250,53,254,53,133,41,116,253,107,255,218,95,147,190,249,53,105,148,191,190,126,254,195,126,116,238,254,218,95,135,102,248,215,228,255,228,161,143,247,126,255,157,95,227,139,98,90,87,77,117,222,166,91,175,238,164,223,126,254,250,121,42,51,149,158,84,139,85,81,210,47,15,199,123,159,142,31,222,223,27,239,29,236,239,255,26,191,139,12,255,15,162,17,255,77,230,247,95,211,251,253,215,242,126,255,181,189,223,127,29,239,247,95,215,251,253,215,115,191,255,73,191,198,111,240,155,254,69,79,152,148,191,41,193,252,207,254,166,95,227,55,248,207,248,239,95,147,254,254,53,188,191,127,45,250,251,215,242,254,254,181,233,239,95,219,251,251,215,161,191,127,29,239,239,95,151,254,254,117,189,191,127,61,250,251,215,211,191,127,93,237,143,224,83,159,127,205,95,244,235,48,75,114,255,244,217,95,243,23,225,115,243,25,245,73,255,15,63,163,126,233,255,225,103,191,14,255,30,126,70,253,211,255,195,207,8,7,250,191,124,246,107,130,6,191,129,224,241,127,255,223,58,73,191,134,240,169,249,125,198,124,250,127,17,159,126,101,249,244,119,249,53,229,51,252,48,124,10,249,217,249,53,132,168,191,49,253,123,64,63,191,253,107,24,29,228,120,18,58,247,255,143,207,229,7,243,246,111,250,39,225,247,95,151,121,251,55,253,147,126,140,121,228,215,252,135,104,110,254,164,223,224,215,248,207,254,34,249,251,215,226,191,127,204,254,253,235,240,223,137,253,251,55,224,191,127,67,250,155,231,246,215,248,181,255,160,223,136,126,151,185,255,181,255,35,250,238,47,146,121,255,207,254,36,124,70,115,242,31,253,154,157,207,72,150,254,163,95,171,243,25,201,212,127,244,107,119,62,35,217,250,143,126,157,206,103,36,99,255,209,175,219,249,140,100,237,63,250,245,58,159,253,250,244,217,175,239,125,6,222,251,127,2,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((Kernel7.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel7.cid1))
			{
				this.SetTextureSize(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector3[] value)
		{
			if ((Kernel7.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel7.cid0))
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((Kernel7.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel7.sid0))
			{
				this.TextureSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((Kernel7.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel7.tid0))
			{
				this.Texture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'Kernel3' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 9 instruction slots used, 9 registers</para><para>Pixel Shader: approximately 7 instruction slots used (3 texture, 4 arithmetic), 3 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class Kernel3 : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'Kernel3' shader</summary>
		public Kernel3()
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(1F, 1F, 0F, 0F);
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			Kernel3.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			Kernel3.cid0 = state.GetNameUniqueID("kernel");
			Kernel3.cid1 = state.GetNameUniqueID("textureSize");
			Kernel3.sid0 = state.GetNameUniqueID("TextureSampler");
			Kernel3.tid0 = state.GetNameUniqueID("Texture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != Kernel3.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.preg_change = (this.preg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				Kernel3.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((this.preg_change == true))
			{
				Kernel3.fx.ps_c.SetValue(this.preg);
				this.preg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref Kernel3.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((Kernel3.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((Kernel3.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			Kernel3.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out Kernel3.fx, Kernel3.fxb, 11, 11);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.preg_change) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 2;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(Kernel3.vin[i]));
			index = Kernel3.vin[(i + 2)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary/>
		private bool preg_change;
		/// <summary>Name ID for 'kernel'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float3 kernel[16]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetKernel(Microsoft.Xna.Framework.Vector3[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector3 val;
			int i;
			uint ri;
			uint wi;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 4)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 4)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 3)); i = (i + 1))
			{
				val = value[ri];
				this.preg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.preg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float3 kernel[16]'</summary>
		public Microsoft.Xna.Framework.Vector3[] Kernel
		{
			set
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'textureSize'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 textureSize'</summary><param name="value"/>
		public void SetTextureSize(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 textureSize'</summary>
		public Microsoft.Xna.Framework.Vector2 TextureSize
		{
			set
			{
				this.SetTextureSize(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D TextureSampler'</summary>
		public Xen.Graphics.TextureSamplerState TextureSampler
		{
			get
			{
				return this.pts[0];
			}
			set
			{
				if ((value != this.pts[0]))
				{
					this.pts[0] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D Texture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[0]));
			}
			set
			{
				if ((value != this.ptx[0]))
				{
					this.ptc = true;
					this.ptx[0] = value;
				}
			}
		}
		/// <summary>Name uid for sampler for 'Sampler2D TextureSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D Texture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[9];
		/// <summary>Pixel shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] preg = new Microsoft.Xna.Framework.Vector4[3];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,120,135,0,1,3,131,0,1,1,131,0,1,176,135,0,1,9,131,0,1,4,131,0,1,1,229,0,0,174,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,1,0,1,0,1,1,1,8,135,0,0,1,3,131,0,0,1,4,131,0,0,1,1,179,0,0,1,6,1,95,1,112,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,44,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,3,131,0,0,1,1,131,0,0,1,4,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,188,131,0,0,1,216,138,0,0,1,1,1,20,1,0,1,0,1,1,1,40,138,0,0,1,1,1,108,135,0,0,1,1,1,0,1,0,1,1,1,104,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,60,1,0,1,0,1,1,1,56,131,0,0,1,93,134,0,0,1,1,1,84,1,0,1,0,1,1,1,80,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,120,1,16,1,42,1,17,131,0,0,1,1,1,12,131,0,0,1,108,135,0,0,1,36,135,0,0,1,224,139,0,0,1,184,131,0,0,1,28,131,0,0,1,171,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,135,0,0,1,164,131,0,0,1,68,1,0,1,2,131,0,0,1,3,133,0,0,1,76,131,0,0,1,92,131,0,0,1,140,1,0,1,3,131,0,0,1,1,133,0,0,1,148,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,3,182,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,108,1,16,1,0,1,3,132,0,0,1,4,134,0,0,1,24,1,99,1,0,1,7,1,0,1,7,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,49,1,81,1,0,1,0,1,50,1,82,1,0,1,21,1,48,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,5,1,0,1,0,1,34,133,0,0,1,16,1,8,1,48,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,16,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,0,1,65,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,132,0,0,1,198,1,0,1,161,1,0,1,2,1,0,1,200,1,15,131,0,0,1,148,1,198,1,148,1,171,1,1,1,1,1,0,1,200,1,15,1,128,131,0,0,1,198,1,248,1,171,1,3,150,0,0,132,255,0,138,0,0,1,2,1,60,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,100,131,0,0,1,216,135,0,0,1,36,134,0,0,1,1,1,20,139,0,0,1,236,131,0,0,1,28,131,0,0,1,223,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,216,131,0,0,1,48,1,0,1,2,131,0,0,1,9,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,9,229,0,0,177,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,216,1,0,1,49,1,0,1,2,138,0,0,1,32,1,132,131,0,0,1,1,131,0,0,1,2,131,0,0,1,4,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,48,1,80,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,2,1,50,1,82,1,0,1,3,1,51,1,83,1,0,1,0,1,16,1,13,1,0,1,0,1,16,1,14,1,0,1,0,1,16,1,15,1,0,1,0,1,16,1,16,1,48,1,5,1,32,1,3,1,0,1,0,1,18,1,0,1,194,133,0,0,1,64,1,5,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,9,1,32,1,15,1,18,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,15,1,200,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,0,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,1,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,2,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,3,1,0,1,200,1,3,1,0,1,1,1,0,1,176,1,176,1,0,1,33,1,7,1,8,1,0,1,200,1,12,1,0,1,1,1,0,1,172,1,172,1,0,1,33,1,6,1,8,1,0,1,200,1,3,1,0,1,2,1,0,1,176,1,176,1,0,1,33,1,5,1,8,1,0,1,200,1,12,1,0,2,2,0,3,172,172,0,4,33,4,8,0,5,200,3,128,0,0,6,26,176,0,224,2,0,7,0,200,3,128,1,0,176,8,176,0,224,2,0,0,200,3,9,128,2,0,26,176,0,224,1,0,10,0,200,3,128,3,0,176,176,0,224,1,1,141,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {168,5,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,207,252,154,242,251,175,141,191,233,255,127,147,126,247,99,244,255,95,71,63,251,127,219,243,235,209,255,127,255,203,230,247,159,254,26,14,239,223,192,27,199,215,197,155,225,174,4,238,111,244,107,8,156,81,7,208,175,175,109,154,29,233,227,55,211,118,254,243,107,209,255,127,211,200,231,12,234,255,254,191,255,111,192,120,61,207,102,121,237,240,71,91,131,123,234,189,243,247,209,255,255,45,239,239,223,138,26,111,121,56,173,244,119,252,152,235,239,232,255,79,209,239,31,211,103,7,244,255,63,85,255,126,67,191,191,244,218,153,135,208,250,191,13,216,63,9,191,252,90,255,247,255,253,127,253,223,7,191,198,201,155,227,39,191,19,253,249,215,254,26,242,25,222,249,157,184,213,175,145,254,101,244,207,83,133,243,107,211,191,207,233,231,239,75,255,255,227,126,13,51,174,95,235,215,248,211,20,166,208,245,175,253,107,127,77,250,230,215,164,81,254,218,250,249,251,62,74,251,191,246,215,161,25,250,53,249,63,121,232,227,189,223,127,231,215,248,162,152,214,85,83,157,183,233,214,171,59,233,183,159,191,126,158,10,165,211,147,106,177,42,74,250,229,225,120,239,211,241,195,251,123,227,189,131,253,253,95,227,119,17,244,255,32,194,248,111,50,191,255,154,222,239,191,150,251,253,79,250,53,126,131,223,244,47,122,194,67,251,77,169,205,127,246,55,253,26,191,193,127,198,127,255,154,244,247,175,225,253,253,107,209,223,191,150,254,253,235,106,123,250,158,222,249,107,254,162,95,135,167,152,223,167,207,254,154,191,8,159,155,207,232,29,250,191,124,246,107,162,207,223,64,222,251,191,255,111,55,126,204,147,249,253,95,227,121,250,191,104,158,78,237,60,253,135,191,134,124,134,175,204,60,253,91,244,207,206,175,33,131,248,49,250,247,128,126,126,251,215,48,50,228,230,4,50,255,255,198,231,242,131,231,246,55,253,147,240,251,175,203,115,251,155,254,73,63,198,115,242,107,254,67,68,219,63,137,254,255,23,201,223,191,22,255,253,107,218,191,127,29,254,251,215,178,127,255,6,252,247,175,77,127,243,220,252,26,191,246,31,244,27,208,239,50,119,191,246,127,244,235,208,239,50,111,255,217,159,132,207,136,166,255,209,175,219,249,140,120,233,63,250,245,58,159,253,218,244,217,175,239,125,134,185,254,127,2,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((Kernel3.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel3.cid1))
			{
				this.SetTextureSize(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector3[] value)
		{
			if ((Kernel3.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel3.cid0))
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((Kernel3.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel3.sid0))
			{
				this.TextureSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((Kernel3.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel3.tid0))
			{
				this.Texture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'Kernel2' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 7 instruction slots used, 7 registers</para><para>Pixel Shader: approximately 5 instruction slots used (2 texture, 3 arithmetic), 2 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class Kernel2 : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'Kernel2' shader</summary>
		public Kernel2()
		{
			this.vreg[6] = new Microsoft.Xna.Framework.Vector4(1F, 1F, 0F, 0F);
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			Kernel2.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			Kernel2.cid0 = state.GetNameUniqueID("kernel");
			Kernel2.cid1 = state.GetNameUniqueID("textureSize");
			Kernel2.sid0 = state.GetNameUniqueID("TextureSampler");
			Kernel2.tid0 = state.GetNameUniqueID("Texture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != Kernel2.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.preg_change = (this.preg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				Kernel2.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((this.preg_change == true))
			{
				Kernel2.fx.ps_c.SetValue(this.preg);
				this.preg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref Kernel2.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((Kernel2.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((Kernel2.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			Kernel2.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out Kernel2.fx, Kernel2.fxb, 9, 8);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.preg_change) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 2;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(Kernel2.vin[i]));
			index = Kernel2.vin[(i + 2)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary/>
		private bool preg_change;
		/// <summary>Name ID for 'kernel'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float3 kernel[16]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetKernel(Microsoft.Xna.Framework.Vector3[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector3 val;
			int i;
			uint ri;
			uint wi;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 2)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 4)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
			ri = readIndex;
			wi = writeIndex;
			if ((value == null))
			{
				throw new System.ArgumentNullException("value");
			}
			if ((((ri + count) 
						> value.Length) 
						|| ((wi + count) 
						> 16)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 2)); i = (i + 1))
			{
				val = value[ri];
				this.preg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.preg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float3 kernel[16]'</summary>
		public Microsoft.Xna.Framework.Vector3[] Kernel
		{
			set
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'textureSize'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 textureSize'</summary><param name="value"/>
		public void SetTextureSize(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[6] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 textureSize'</summary>
		public Microsoft.Xna.Framework.Vector2 TextureSize
		{
			set
			{
				this.SetTextureSize(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D TextureSampler'</summary>
		public Xen.Graphics.TextureSamplerState TextureSampler
		{
			get
			{
				return this.pts[0];
			}
			set
			{
				if ((value != this.pts[0]))
				{
					this.pts[0] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D Texture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[0]));
			}
			set
			{
				if ((value != this.ptx[0]))
				{
					this.ptc = true;
					this.ptx[0] = value;
				}
			}
		}
		/// <summary>Name uid for sampler for 'Sampler2D TextureSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D Texture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[7];
		/// <summary>Pixel shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] preg = new Microsoft.Xna.Framework.Vector4[2];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,72,135,0,1,3,131,0,1,1,131,0,1,144,135,0,1,7,131,0,1,4,131,0,1,1,229,0,0,142,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,131,0,0,1,216,135,0,0,1,2,131,0,0,1,4,131,0,0,1,1,163,0,0,1,6,1,95,1,112,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,252,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,3,131,0,0,1,1,131,0,0,1,4,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,156,131,0,0,1,184,139,0,0,1,228,131,0,0,1,248,138,0,0,1,1,1,60,135,0,0,1,1,1,0,1,0,1,1,1,56,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,12,1,0,1,0,1,1,1,8,131,0,0,1,93,134,0,0,1,1,1,36,1,0,1,0,1,1,1,32,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,76,1,16,1,42,1,17,132,0,0,1,248,131,0,0,1,84,135,0,0,1,36,135,0,0,1,208,139,0,0,1,168,131,0,0,1,28,131,0,0,1,155,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,135,0,0,1,148,131,0,0,1,68,1,0,1,2,131,0,0,1,2,133,0,0,1,76,131,0,0,1,92,131,0,0,1,124,1,0,1,3,131,0,0,1,1,133,0,0,1,132,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,2,166,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,16,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,49,1,81,1,0,1,5,1,32,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,4,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,0,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,132,0,0,1,198,1,0,1,161,1,0,1,1,1,0,1,200,1,15,1,128,131,0,0,1,198,1,0,1,171,1,2,150,0,0,132,255,0,138,0,0,1,1,1,220,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,52,131,0,0,1,168,135,0,0,1,36,135,0,0,1,244,139,0,0,1,204,131,0,0,1,28,131,0,0,1,191,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,184,131,0,0,1,48,1,0,1,2,131,0,0,1,7,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,7,229,0,0,145,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,168,1,0,1,17,1,0,1,1,138,0,0,1,16,1,66,131,0,0,1,1,131,0,0,1,2,131,0,0,1,2,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,48,1,80,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,0,1,16,1,11,1,0,1,0,1,16,1,12,1,48,1,5,1,32,1,3,1,0,1,0,1,18,1,0,1,194,133,0,0,1,64,1,5,1,0,1,0,1,18,1,0,1,196,133,0,0,1,64,1,9,1,0,1,0,1,34,133,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,15,1,200,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,0,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,1,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,2,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,3,1,0,1,200,1,3,1,0,2,1,0,3,176,176,0,4,33,5,6,0,5,200,12,0,1,0,6,172,172,0,33,4,6,7,0,200,3,128,0,0,26,8,176,0,224,1,0,0,200,3,8,128,1,0,176,176,0,224,1,141,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {240,4,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,243,95,83,126,255,181,241,55,253,255,79,210,239,126,125,250,255,175,163,159,253,176,159,95,143,254,255,251,95,54,191,255,244,215,112,120,253,91,250,221,175,245,107,220,30,47,134,179,18,56,191,209,175,33,239,253,31,157,54,24,39,218,52,59,2,243,55,211,118,254,131,62,127,211,200,231,140,195,255,253,127,255,223,128,241,122,158,205,242,218,225,139,182,191,182,254,76,189,119,254,60,250,255,223,229,253,253,159,209,255,255,55,239,239,223,83,7,134,31,7,250,59,250,255,83,244,251,223,136,62,251,13,232,255,127,170,254,253,187,209,239,169,215,206,60,132,214,255,109,104,244,18,191,252,90,255,247,255,253,127,253,223,251,191,198,201,155,227,39,191,19,253,249,231,254,26,242,25,222,249,157,184,213,175,145,254,105,244,207,83,133,243,107,209,127,207,233,231,239,75,255,255,153,95,195,140,235,215,250,53,254,48,133,41,116,253,107,255,218,95,147,190,249,53,105,148,126,223,155,30,165,245,95,251,235,208,140,252,154,252,159,60,244,241,222,239,191,243,107,124,81,76,235,170,169,206,219,116,235,213,157,244,219,207,95,63,79,133,178,233,73,181,88,21,37,253,242,112,188,247,233,248,225,253,189,241,222,193,254,254,175,241,187,8,186,127,16,97,248,55,153,223,127,77,247,251,159,244,107,252,6,191,233,95,244,132,209,255,77,233,243,255,236,111,250,53,126,131,255,140,255,254,53,233,239,95,67,255,254,117,245,123,250,155,218,252,53,127,209,175,195,211,198,237,233,179,191,230,47,146,207,9,222,111,32,109,254,239,255,219,141,7,116,54,191,255,113,76,231,255,139,232,252,153,165,243,63,248,107,200,103,248,202,208,249,239,162,127,118,126,13,65,240,215,167,127,15,232,231,183,127,13,195,243,142,166,224,171,159,139,231,242,131,231,226,55,253,147,240,251,175,203,115,241,155,254,73,63,198,244,253,53,255,33,162,221,159,68,255,255,139,228,239,95,139,255,254,53,237,223,191,14,255,253,107,217,191,127,3,254,251,215,166,191,153,246,191,198,175,253,7,253,122,244,187,204,205,175,253,31,253,58,244,187,204,203,127,246,39,225,51,162,217,127,244,235,122,159,97,142,254,159,0,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((Kernel2.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel2.cid1))
			{
				this.SetTextureSize(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector3[] value)
		{
			if ((Kernel2.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel2.cid0))
			{
				this.SetKernel(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((Kernel2.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel2.sid0))
			{
				this.TextureSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((Kernel2.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Kernel2.tid0))
			{
				this.Texture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'Downsample8' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 13 instruction slots used, 5 registers</para><para>Pixel Shader: approximately 17 instruction slots used (8 texture, 9 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class Downsample8 : Xen.Graphics.ShaderSystem.BaseShader, IDownsampleShader
	{
		/// <summary>Construct an instance of the 'Downsample8' shader</summary>
		public Downsample8()
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(1F, 1F, 0F, 0F);
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(69));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			Downsample8.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			Downsample8.cid0 = state.GetNameUniqueID("sampleDirection");
			Downsample8.sid0 = state.GetNameUniqueID("PointSampler");
			Downsample8.tid0 = state.GetNameUniqueID("Texture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != Downsample8.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				Downsample8.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref Downsample8.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((Downsample8.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((Downsample8.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			Downsample8.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out Downsample8.fx, Downsample8.fxb, 16, 27);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 2;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(Downsample8.vin[i]));
			index = Downsample8.vin[(i + 2)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'sampleDirection'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float2 sampleDirection'</summary><param name="value"/>
		public void SetSampleDirection(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 sampleDirection'</summary>
		public Microsoft.Xna.Framework.Vector2 SampleDirection
		{
			set
			{
				this.SetSampleDirection(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D PointSampler'</summary>
		public Xen.Graphics.TextureSamplerState PointSampler
		{
			get
			{
				return this.pts[0];
			}
			set
			{
				if ((value != this.pts[0]))
				{
					this.pts[0] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D Texture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[0]));
			}
			set
			{
				if ((value != this.ptx[0]))
				{
					this.ptc = true;
					this.ptx[0] = value;
				}
			}
		}
		/// <summary>Name uid for sampler for 'Sampler2D PointSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D Texture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[5];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,4,254,255,9,1,131,0,1,224,135,0,1,3,131,0,1,1,131,0,1,112,135,0,1,5,131,0,1,4,131,0,1,1,211,0,6,6,95,118,115,95,99,134,0,1,12,131,0,1,4,131,0,1,148,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,2,131,0,0,1,1,131,0,0,1,4,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,124,131,0,0,1,144,139,0,0,1,212,135,0,0,1,1,131,0,0,1,208,135,0,0,1,2,131,0,0,1,92,135,0,0,1,164,131,0,0,1,160,131,0,0,1,93,135,0,0,1,188,131,0,0,1,184,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,2,1,28,1,16,1,42,1,17,132,0,0,1,236,1,0,1,0,1,1,1,48,135,0,0,1,36,131,0,0,1,132,131,0,0,1,172,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,240,1,16,1,0,1,8,132,0,0,1,4,134,0,0,1,65,1,8,1,0,1,255,1,0,1,255,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,49,1,81,1,0,1,0,1,50,1,82,1,0,1,0,1,51,1,83,1,0,2,0,52,3,84,0,0,4,53,85,0,0,5,54,86,0,0,55,1,87,176,0,0,1,62,143,0,0,1,5,1,85,1,96,1,3,1,32,1,9,1,18,1,0,1,18,1,0,1,0,1,5,132,0,0,1,96,1,11,1,196,1,0,1,18,133,0,0,1,32,1,17,1,0,1,0,1,34,133,0,0,1,16,1,8,1,112,1,225,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,96,1,193,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,80,1,161,1,31,1,31,1,246,1,136,1,0,1,0,2,64,0,3,16,8,64,4,129,31,31,246,5,136,0,0,64,0,6,16,8,48,97,31,31,7,246,136,0,0,64,0,16,8,8,32,65,31,31,246,136,0,9,0,64,0,16,8,128,1,31,31,10,246,136,0,0,64,0,16,8,0,33,10,31,31,246,136,0,0,64,0,200,15,134,0,6,224,8,0,0,200,15,134,0,5,224,0,2,0,200,1,15,134,0,3,224,0,3,3,0,200,15,134,0,1,224,2,0,4,3,0,200,15,134,0,0,1,224,2,0,5,3,0,200,15,134,0,0,1,224,2,0,6,3,0,200,15,134,0,0,1,224,2,0,7,3,0,200,15,1,128,131,0,4,108,0,161,0,1,255,149,0,0,132,255,0,138,0,0,1,2,1,180,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,108,1,0,1,0,1,1,1,72,135,0,0,1,36,131,0,0,1,212,131,0,0,1,252,139,0,0,1,172,131,0,0,1,28,131,0,0,1,159,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,152,131,0,0,1,48,1,0,1,2,131,0,0,1,5,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,2,1,0,3,3,0,1,4,0,4,0,5,214,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,8,1,0,1,113,1,0,1,3,138,0,0,1,65,1,8,131,0,0,1,1,131,0,0,1,2,131,0,0,1,8,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,48,1,80,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,2,1,50,1,82,1,0,1,3,1,51,1,83,1,0,1,4,1,52,1,84,1,0,1,5,1,53,1,85,1,0,1,6,1,54,1,86,1,0,1,7,1,55,1,87,1,0,1,0,1,16,1,13,1,0,1,0,1,16,1,14,1,0,1,0,1,16,1,15,1,0,1,0,1,16,1,16,1,0,1,0,1,16,1,17,1,0,1,0,1,16,1,18,1,0,1,0,1,16,1,19,1,0,1,0,1,16,1,20,160,0,0,1,64,1,32,1,0,1,0,1,192,1,32,1,0,1,0,1,64,1,96,1,0,1,0,1,192,1,96,1,0,1,0,1,63,131,0,0,1,191,131,0,0,1,63,1,192,1,0,1,0,1,191,1,192,1,0,1,0,1,48,1,5,2,32,3,3,0,0,18,2,0,194,133,0,2,64,5,3,0,0,18,2,0,196,133,0,2,96,9,3,96,15,18,2,0,34,131,0,3,5,248,16,131,0,2,6,136,132,0,2,5,248,132,0,2,15,200,132,0,1,200,2,1,128,3,62,0,167,4,167,0,175,1,5,0,0,200,2,128,6,62,0,167,167,0,175,7,1,1,0,200,4,128,62,8,0,167,167,0,175,1,2,0,9,200,8,128,62,0,167,167,0,175,10,1,3,0,200,15,0,3,0,160,97,11,160,43,4,255,0,200,15,0,2,0,160,12,203,160,43,4,255,0,200,15,0,1,0,160,8,97,160,43,4,254,0,200,15,131,0,13,160,203,160,43,4,254,0,200,3,128,0,0,26,3,26,0,226,131,0,14,200,3,128,1,0,26,26,0,226,1,1,0,200,3,15,128,2,0,26,26,0,226,2,2,0,200,3,128,3,0,16,26,26,0,226,3,3,0,200,3,128,4,0,176,176,0,226,17,3,3,0,200,3,128,5,0,176,176,0,226,2,2,0,200,3,18,128,6,0,176,176,0,226,1,1,0,200,3,128,7,0,176,176,0,1,226,142,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {184,5,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,127,166,191,255,218,248,155,254,191,210,191,127,93,250,255,175,163,159,125,211,207,175,71,255,255,253,47,155,223,127,74,63,127,163,95,67,250,249,211,58,109,126,125,180,89,53,191,127,179,35,56,252,102,218,206,127,126,45,250,255,111,26,249,220,224,12,24,175,231,217,44,175,165,45,62,71,219,95,91,127,166,222,59,63,67,255,255,147,188,191,255,45,253,137,119,254,37,253,29,48,254,20,253,253,47,163,255,255,69,244,255,63,85,255,254,251,232,255,127,151,215,206,60,255,55,61,6,159,20,95,252,90,255,247,255,253,127,253,223,191,235,175,113,242,230,248,201,239,68,127,126,249,107,200,103,104,243,59,113,171,95,35,253,54,253,179,243,107,152,57,249,181,126,141,3,125,95,233,241,215,254,58,68,181,95,147,255,147,135,62,222,251,253,119,126,141,47,138,105,93,53,213,121,155,110,189,186,147,126,251,249,235,231,169,140,62,61,169,22,171,162,164,95,30,142,247,62,29,63,188,191,55,222,59,216,223,255,53,126,130,167,249,55,253,139,8,196,145,130,226,231,119,145,33,252,65,132,193,223,100,126,255,53,189,223,127,45,239,247,95,219,251,253,215,241,126,255,117,189,223,127,61,239,247,95,223,253,254,39,253,26,191,193,111,250,23,61,225,97,254,166,212,215,127,246,55,253,26,191,193,127,198,127,255,154,244,247,175,233,253,253,107,209,223,191,150,247,247,175,77,127,255,218,222,223,191,14,253,253,235,120,127,255,186,244,247,175,235,253,253,235,209,223,191,158,247,247,175,79,127,255,250,250,247,175,229,250,71,159,127,144,249,155,250,227,207,204,223,191,118,231,239,95,167,243,247,175,219,249,251,215,235,252,253,235,235,223,191,174,215,31,209,224,47,250,53,65,139,223,64,254,254,191,255,111,55,7,224,27,243,251,255,129,137,254,181,254,47,226,155,251,150,111,254,194,95,67,62,243,249,230,207,250,53,132,111,64,220,95,87,249,230,219,191,134,145,181,191,246,175,165,57,36,158,249,117,88,182,127,54,158,203,15,224,195,95,151,249,240,15,248,61,105,24,244,255,127,232,247,32,112,191,135,227,195,223,244,79,194,239,191,46,243,225,111,250,39,253,24,211,240,215,252,135,136,102,127,18,253,255,47,146,191,127,45,254,251,215,180,127,255,58,252,247,175,101,255,254,13,248,111,154,71,161,249,175,241,107,99,14,255,162,95,135,85,193,175,253,31,233,220,208,119,255,217,159,132,207,126,77,251,217,87,246,179,95,203,126,246,215,216,207,126,109,251,217,255,109,63,251,117,220,103,127,177,249,236,215,117,239,218,207,126,61,215,135,253,236,215,119,184,240,103,224,137,255,39,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((Downsample8.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Downsample8.cid0))
			{
				this.SetSampleDirection(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((Downsample8.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Downsample8.sid0))
			{
				this.PointSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((Downsample8.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Downsample8.tid0))
			{
				this.Texture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'Downsample4' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 9 instruction slots used, 5 registers</para><para>Pixel Shader: approximately 9 instruction slots used (4 texture, 5 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class Downsample4 : Xen.Graphics.ShaderSystem.BaseShader, IDownsampleShader
	{
		/// <summary>Construct an instance of the 'Downsample4' shader</summary>
		public Downsample4()
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(1F, 1F, 0F, 0F);
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(69));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			Downsample4.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			Downsample4.cid0 = state.GetNameUniqueID("sampleDirection");
			Downsample4.sid0 = state.GetNameUniqueID("PointSampler");
			Downsample4.tid0 = state.GetNameUniqueID("Texture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != Downsample4.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				Downsample4.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref Downsample4.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((Downsample4.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((Downsample4.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			Downsample4.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out Downsample4.fx, Downsample4.fxb, 12, 15);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 2;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(Downsample4.vin[i]));
			index = Downsample4.vin[(i + 2)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'sampleDirection'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float2 sampleDirection'</summary><param name="value"/>
		public void SetSampleDirection(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 sampleDirection'</summary>
		public Microsoft.Xna.Framework.Vector2 SampleDirection
		{
			set
			{
				this.SetSampleDirection(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D PointSampler'</summary>
		public Xen.Graphics.TextureSamplerState PointSampler
		{
			get
			{
				return this.pts[0];
			}
			set
			{
				if ((value != this.pts[0]))
				{
					this.pts[0] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D Texture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[0]));
			}
			set
			{
				if ((value != this.ptx[0]))
				{
					this.ptc = true;
					this.ptx[0] = value;
				}
			}
		}
		/// <summary>Name uid for sampler for 'Sampler2D PointSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D Texture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[5];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,4,254,255,9,1,131,0,1,224,135,0,1,3,131,0,1,1,131,0,1,112,135,0,1,5,131,0,1,4,131,0,1,1,211,0,6,6,95,118,115,95,99,134,0,1,12,131,0,1,4,131,0,1,148,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,2,131,0,0,1,1,131,0,0,1,4,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,124,131,0,0,1,144,139,0,0,1,212,135,0,0,1,1,131,0,0,1,208,135,0,0,1,2,131,0,0,1,92,135,0,0,1,164,131,0,0,1,160,131,0,0,1,93,135,0,0,1,188,131,0,0,1,184,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,160,1,16,1,42,1,17,132,0,0,1,220,131,0,0,1,196,135,0,0,1,36,131,0,0,1,132,131,0,0,1,172,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,132,1,16,1,0,1,4,132,0,0,1,4,134,0,0,1,32,1,132,1,0,1,15,1,0,1,15,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,49,1,81,1,0,1,0,1,50,1,82,1,0,1,0,1,51,1,83,176,0,0,1,62,1,128,143,0,0,1,85,1,64,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,64,1,6,1,0,1,0,1,34,133,0,0,1,16,1,8,1,48,1,97,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,32,1,65,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,64,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,0,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,134,0,0,1,224,1,4,1,0,2,0,200,1,15,134,0,0,1,224,2,0,2,3,0,200,15,134,0,0,1,224,2,0,3,3,0,200,15,1,128,131,0,4,108,0,161,0,1,255,149,0,0,132,255,0,138,0,0,1,2,1,76,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,76,1,0,1,0,1,1,136,0,0,1,36,131,0,0,1,212,131,0,0,1,252,139,0,0,1,172,131,0,0,1,28,131,0,0,1,159,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,152,131,0,0,1,48,1,0,1,2,131,0,0,1,5,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,2,3,0,3,1,0,4,2,0,5,214,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,192,1,0,1,49,1,0,1,1,138,0,0,1,32,1,132,131,0,0,1,1,131,0,0,1,2,131,0,0,1,4,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,48,1,80,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,2,1,50,1,82,1,0,1,3,1,51,1,83,1,0,1,0,1,16,1,11,1,0,1,0,1,16,1,12,1,0,1,0,1,16,1,13,1,0,1,0,1,16,1,14,176,0,0,1,63,131,0,0,1,191,131,0,0,1,63,1,192,1,0,1,0,1,191,1,192,1,0,1,0,1,48,1,5,1,32,1,3,1,0,1,0,1,18,1,0,1,194,133,0,0,1,64,1,5,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,9,1,0,1,0,1,34,133,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,15,1,200,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,0,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,1,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,2,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,3,1,0,2,200,15,3,0,1,0,4,160,97,160,43,5,4,255,0,200,15,131,0,5,160,203,160,43,4,6,255,0,200,3,128,0,5,0,26,26,0,226,131,0,7,200,3,128,1,0,26,26,8,0,226,1,1,0,200,3,128,9,2,0,176,176,0,226,1,1,0,9,200,3,128,3,0,176,176,0,226,142,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {184,4,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,127,166,191,255,218,248,155,254,191,210,191,127,93,250,255,175,163,159,125,211,207,175,71,255,255,253,47,155,223,127,74,63,127,163,95,67,250,249,211,58,109,126,125,180,89,53,191,127,179,35,56,252,102,218,206,127,126,45,250,255,111,26,249,92,112,254,191,255,111,192,120,61,207,102,121,45,109,241,57,218,254,218,250,51,245,222,249,25,250,255,159,228,253,253,111,233,79,188,243,47,233,239,128,241,167,232,239,127,25,253,255,47,162,255,255,169,250,247,223,71,255,255,187,188,118,230,249,191,233,49,52,92,225,151,95,235,255,254,191,255,175,255,251,119,253,53,78,222,28,63,249,157,232,207,47,127,13,249,12,95,253,78,220,234,215,72,191,77,255,236,252,26,102,78,126,173,95,227,64,223,87,122,252,181,191,14,81,237,215,228,255,20,110,243,251,239,253,254,59,191,198,23,197,180,174,154,234,188,77,183,94,221,73,191,253,252,245,243,84,70,159,158,84,139,85,81,210,47,15,199,123,159,142,31,222,223,27,239,29,236,239,255,26,63,193,211,252,155,254,69,191,198,175,241,7,29,41,40,126,126,23,25,194,31,68,24,252,77,230,247,95,211,251,253,215,242,126,255,181,221,239,127,210,175,241,27,252,166,127,209,19,70,251,55,165,119,255,179,191,233,215,248,13,254,51,254,251,215,164,191,127,77,239,239,95,139,254,254,181,188,191,127,109,250,251,215,214,191,127,45,247,62,222,249,131,204,223,212,158,63,51,127,255,218,250,247,175,235,181,39,28,254,162,95,19,184,252,6,242,247,255,253,127,187,49,97,30,204,239,127,29,207,195,255,69,243,112,223,206,195,95,248,107,200,103,254,60,252,89,191,134,204,3,6,247,235,234,60,124,251,215,48,188,251,215,254,181,68,19,154,131,95,135,101,229,103,227,185,252,128,121,253,117,121,94,255,161,223,131,192,224,255,255,32,253,254,15,186,121,253,77,255,36,252,254,235,242,188,254,166,127,210,143,49,13,127,205,127,136,104,246,39,209,255,255,34,249,251,215,226,191,127,77,251,247,175,195,127,255,90,246,239,223,128,255,166,121,16,154,255,26,191,246,31,244,235,208,239,191,14,139,214,175,253,31,233,220,208,119,255,217,159,132,207,126,77,251,217,87,246,179,95,203,126,246,215,216,207,126,109,251,217,255,205,159,97,14,255,159,0,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((Downsample4.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Downsample4.cid0))
			{
				this.SetSampleDirection(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((Downsample4.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Downsample4.sid0))
			{
				this.PointSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((Downsample4.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Downsample4.tid0))
			{
				this.Texture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'Downsample2' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 7 instruction slots used, 5 registers</para><para>Pixel Shader: approximately 5 instruction slots used (2 texture, 3 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class Downsample2 : Xen.Graphics.ShaderSystem.BaseShader, IDownsampleShader
	{
		/// <summary>Construct an instance of the 'Downsample2' shader</summary>
		public Downsample2()
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(1F, 1F, 0F, 0F);
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(69));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			Downsample2.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			Downsample2.cid0 = state.GetNameUniqueID("sampleDirection");
			Downsample2.sid0 = state.GetNameUniqueID("PointSampler");
			Downsample2.tid0 = state.GetNameUniqueID("Texture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != Downsample2.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				Downsample2.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref Downsample2.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((Downsample2.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((Downsample2.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			Downsample2.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out Downsample2.fx, Downsample2.fxb, 10, 9);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 2;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(Downsample2.vin[i]));
			index = Downsample2.vin[(i + 2)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'sampleDirection'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float2 sampleDirection'</summary><param name="value"/>
		public void SetSampleDirection(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 sampleDirection'</summary>
		public Microsoft.Xna.Framework.Vector2 SampleDirection
		{
			set
			{
				this.SetSampleDirection(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D PointSampler'</summary>
		public Xen.Graphics.TextureSamplerState PointSampler
		{
			get
			{
				return this.pts[0];
			}
			set
			{
				if ((value != this.pts[0]))
				{
					this.pts[0] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D Texture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[0]));
			}
			set
			{
				if ((value != this.ptx[0]))
				{
					this.ptc = true;
					this.ptx[0] = value;
				}
			}
		}
		/// <summary>Name uid for sampler for 'Sampler2D PointSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D Texture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[5];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,4,254,255,9,1,131,0,1,224,135,0,1,3,131,0,1,1,131,0,1,112,135,0,1,5,131,0,1,4,131,0,1,1,211,0,6,6,95,118,115,95,99,134,0,1,12,131,0,1,4,131,0,1,148,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,2,131,0,0,1,1,131,0,0,1,4,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,124,131,0,0,1,144,139,0,0,1,212,135,0,0,1,1,131,0,0,1,208,135,0,0,1,2,131,0,0,1,92,135,0,0,1,164,131,0,0,1,160,131,0,0,1,93,135,0,0,1,188,131,0,0,1,184,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,104,1,16,1,42,1,17,132,0,0,1,212,131,0,0,1,148,135,0,0,1,36,131,0,0,1,132,131,0,0,1,172,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,16,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,49,1,81,176,0,0,1,63,144,0,0,1,5,1,32,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,4,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,16,1,8,1,0,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,134,0,0,1,224,1,2,1,0,1,0,1,200,1,15,1,128,131,0,0,1,108,1,0,1,161,1,0,1,255,149,0,0,132,255,0,138,0,0,1,2,1,24,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,60,131,0,0,1,220,135,0,0,1,36,131,0,0,1,212,131,0,0,1,252,139,0,0,1,172,131,0,0,1,28,131,0,0,1,159,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,152,131,0,0,1,48,1,0,1,2,131,0,0,1,5,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,5,214,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,156,1,0,1,17,1,0,1,1,138,0,0,1,16,1,66,131,0,0,1,1,131,0,0,1,2,131,0,0,1,2,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,48,1,80,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,0,1,16,1,10,1,0,1,0,1,16,1,11,176,0,0,1,63,131,0,0,1,191,139,0,0,1,48,1,5,1,32,1,3,1,0,1,0,1,18,1,0,1,194,133,0,0,1,64,1,5,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,9,1,0,1,0,1,34,133,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,15,1,200,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,0,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,1,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,2,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,3,1,0,1,200,1,15,131,0,0,1,160,1,97,1,160,1,43,1,4,1,255,1,0,1,200,1,3,1,128,1,0,1,0,1,26,1,26,1,0,1,226,131,0,0,1,200,1,3,1,128,1,1,1,0,1,176,1,176,1,0,1,226,142,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {56,4,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,127,166,191,255,218,248,155,254,191,210,191,127,93,250,255,175,163,159,125,211,207,175,71,255,255,253,47,155,223,127,74,63,127,163,95,67,250,249,211,58,109,126,125,180,89,53,191,127,179,35,56,252,102,218,206,127,126,45,250,255,111,26,249,92,112,254,191,255,111,192,120,61,207,102,121,45,109,241,57,218,254,218,250,51,245,222,249,25,250,255,159,228,253,253,111,233,79,188,243,47,233,239,128,241,167,232,239,127,25,253,255,47,162,255,255,169,250,247,223,71,255,255,187,188,118,230,249,191,233,49,52,252,237,240,203,175,245,127,255,223,255,215,255,253,187,254,26,39,111,142,159,252,78,244,231,151,191,134,124,134,175,126,39,110,245,107,164,223,166,127,118,126,13,51,39,191,214,175,113,160,239,43,61,254,218,95,135,168,246,107,242,127,242,208,199,123,191,255,206,175,241,69,49,173,171,166,58,111,211,173,87,119,210,111,63,127,253,60,149,209,167,39,213,98,85,148,244,203,195,241,222,167,227,135,247,247,198,123,7,251,251,191,198,79,240,52,255,166,127,17,129,248,61,20,20,63,191,139,12,225,15,34,12,254,38,243,251,175,233,126,255,147,126,141,223,224,55,253,139,158,48,122,191,41,181,249,207,254,166,95,227,55,248,207,248,239,95,147,254,254,53,245,239,95,203,125,143,207,254,160,95,215,251,155,96,252,69,191,38,96,253,6,242,247,255,253,127,187,190,65,47,243,251,31,198,244,250,191,136,94,247,45,189,254,194,95,67,62,243,233,245,103,253,26,66,47,32,247,235,42,189,190,253,107,24,30,251,107,255,90,194,157,104,245,235,48,79,255,108,60,151,31,64,255,95,215,209,255,31,52,240,28,253,127,211,63,9,191,255,186,76,255,223,244,79,250,49,166,225,175,249,15,17,205,254,36,250,255,95,36,127,255,90,252,247,175,105,255,254,117,248,239,95,203,254,253,27,240,223,191,54,253,205,52,255,53,126,237,63,232,215,161,223,127,29,22,129,95,251,63,250,53,100,110,232,187,255,236,79,194,103,191,166,253,236,43,254,12,115,243,255,4,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((Downsample2.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Downsample2.cid0))
			{
				this.SetSampleDirection(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((Downsample2.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Downsample2.sid0))
			{
				this.PointSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((Downsample2.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Downsample2.tid0))
			{
				this.Texture = value;
				return true;
			}
			return false;
		}
	}
}
