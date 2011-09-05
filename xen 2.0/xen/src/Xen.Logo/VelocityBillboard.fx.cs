// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = VelocityBillboard.fx
// Namespace = Xen.Logo

#if XBOX360
namespace Xen.Logo
{
	
	/// <summary><para>Technique 'DrawVelocityParticles_GpuTex' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 39 instruction slots used (4 texture, 35 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used (1 texture, 2 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticles_GpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticles_GpuTex' shader</summary>
		public DrawVelocityParticles_GpuTex()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticles_GpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticles_GpuTex.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticles_GpuTex.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticles_GpuTex.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityParticles_GpuTex.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticles_GpuTex.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticles_GpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticles_GpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticles_GpuTex.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticles_GpuTex.gd))
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
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticles_GpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticles_GpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticles_GpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticles_GpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticles_GpuTex.fx, DrawVelocityParticles_GpuTex.fxb, 44, 5);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.vtc) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticles_GpuTex.vin[i]));
			index = DrawVelocityParticles_GpuTex.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,104,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,176,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,212,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,248,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,28,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,5,131,0,0,1,1,131,0,0,1,7,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,188,131,0,0,1,208,139,0,0,1,224,131,0,0,1,244,138,0,0,1,1,1,4,1,0,1,0,1,1,1,24,138,0,0,1,1,1,92,135,0,0,1,1,1,0,1,0,1,1,1,88,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,44,1,0,1,0,1,1,1,40,131,0,0,1,93,134,0,0,1,1,1,68,1,0,1,0,1,1,1,64,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,135,0,0,1,244,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,72,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,72,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,200,2,15,128,131,0,2,108,0,2,225,2,150,0,0,132,255,0,138,0,0,1,3,1,152,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,144,1,0,1,0,1,2,1,8,135,0,0,1,36,1,0,1,0,1,1,1,44,1,0,1,0,1,1,1,84,138,0,0,1,1,1,4,131,0,0,1,28,131,0,0,1,246,1,255,1,254,1,3,132,0,0,1,3,131,0,0,1,28,135,0,0,1,239,131,0,0,1,88,1,0,1,2,131,0,0,1,6,133,0,0,1,96,131,0,0,1,112,131,0,0,1,208,1,0,1,3,131,0,0,1,1,133,0,0,1,216,135,0,0,1,232,1,0,1,3,1,0,1,1,2,0,1,133,0,0,1,216,132,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,200,1,0,1,17,1,0,1,4,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,36,1,0,1,0,1,16,1,35,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,64,1,11,1,18,1,0,1,18,1,0,1,0,1,80,132,0,0,1,96,1,15,1,194,1,0,1,18,133,0,0,1,96,1,21,1,96,1,27,1,18,1,0,1,18,133,0,0,1,32,1,33,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,35,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,1,0,1,1,1,0,1,196,1,179,1,0,1,224,1,1,1,1,1,0,1,188,1,64,1,1,132,0,0,1,65,2,194,0,3,0,255,33,4,8,32,33,15,5,31,254,200,0,0,6,64,0,33,24,64,33,7,15,31,254,200,0,0,64,8,0,200,8,0,1,0,198,177,9,108,139,4,5,5,200,3,0,1,10,0,176,198,0,167,4,255,0,200,2,131,0,9,96,0,0,243,1,0,0,48,32,133,0,2,177,226,131,0,2,184,32,133,0,6,65,194,0,0,255,200,7,8,0,4,0,177,177,0,8,224,0,4,0,200,1,0,1,9,0,24,24,198,209,4,4,255,88,10,38,0,1,0,22,177,108,161,0,255,11,129,160,17,1,3,0,27,177,108,225,4,12,0,129,200,1,0,1,0,27,108,0,225,1,13,1,0,20,7,0,1,0,192,176,177,160,1,255,6,0,12,34,0,3,0,131,108,14,225,3,1,4,20,4,0,3,1,177,108,177,225,0,15,1,0,12,135,3,3,0,192,26,198,225,3,0,0,200,1,3,131,0,9,24,178,0,224,3,3,0,200,3,131,0,16,176,198,176,235,0,2,2,200,1,128,62,0,109,109,27,145,131,0,16,200,2,128,62,0,109,109,27,145,0,1,1,200,4,128,62,17,0,109,109,27,145,0,2,2,200,8,128,62,0,109,109,27,145,7,0,3,3,36,254,192,1,131,0,16,108,226,0,0,128,200,3,128,0,0,197,197,0,226,1,1,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'textureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 textureSizeOffset'</summary><param name="value"/>
		public void SetTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 textureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 TextureSizeOffset
		{
			set
			{
				this.SetTextureSizeOffset(ref value);
			}
		}
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[5] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 velocityScale'</summary>
		public Microsoft.Xna.Framework.Vector2 VelocityScale
		{
			set
			{
				this.SetVelocityScale(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D DisplaySampler'</summary>
		public Xen.Graphics.TextureSamplerState DisplaySampler
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
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D PositionSampler'</summary>
		public Xen.Graphics.TextureSamplerState PositionSampler
		{
			get
			{
				return this.vts[0];
			}
			set
			{
				if ((value != this.vts[0]))
				{
					this.vts[0] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D VelocitySampler'</summary>
		public Xen.Graphics.TextureSamplerState VelocitySampler
		{
			get
			{
				return this.vts[1];
			}
			set
			{
				if ((value != this.vts[1]))
				{
					this.vts[1] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D PositionTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D PositionTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[0]));
			}
			set
			{
				if ((value != this.vtx[0]))
				{
					this.vtc = true;
					this.vtx[0] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D VelocityTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D VelocityTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[1]));
			}
			set
			{
				if ((value != this.vtx[1]))
				{
					this.vtc = true;
					this.vtx[1] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D DisplayTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D DisplayTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D DisplaySampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid2;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D DisplayTexture'</summary>
		static int tid4;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[6];
		/// <summary>Bound vertex textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] vtx = new Microsoft.Xna.Framework.Graphics.Texture[2];
		/// <summary>Bound vertex samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] vts = new Xen.Graphics.TextureSamplerState[2];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticlesColour_GpuTex' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 41 instruction slots used (6 texture, 35 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used (1 texture, 2 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticlesColour_GpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticlesColour_GpuTex' shader</summary>
		public DrawVelocityParticlesColour_GpuTex()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticlesColour_GpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticlesColour_GpuTex.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticlesColour_GpuTex.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticlesColour_GpuTex.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityParticlesColour_GpuTex.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityParticlesColour_GpuTex.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticlesColour_GpuTex.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticlesColour_GpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticlesColour_GpuTex.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityParticlesColour_GpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticlesColour_GpuTex.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticlesColour_GpuTex.gd))
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
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticlesColour_GpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticlesColour_GpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticlesColour_GpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticlesColour_GpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticlesColour_GpuTex.fx, DrawVelocityParticlesColour_GpuTex.fxb, 46, 5);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.vtc) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticlesColour_GpuTex.vin[i]));
			index = DrawVelocityParticlesColour_GpuTex.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,140,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,176,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,212,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,248,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,28,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,64,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,188,131,0,0,1,208,139,0,0,1,224,131,0,0,1,244,138,0,0,1,1,1,4,1,0,1,0,1,1,1,24,138,0,0,1,1,1,40,1,0,1,0,1,1,1,60,138,0,0,1,1,1,128,135,0,0,1,1,1,0,1,0,1,1,1,124,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,80,1,0,1,0,1,1,1,76,131,0,0,1,93,134,0,0,1,1,1,104,1,0,1,0,1,1,1,100,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,135,0,0,1,244,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,72,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,72,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,200,1,15,1,128,131,0,1,108,2,0,225,1,2,150,0,0,132,255,0,138,0,0,1,3,1,192,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,172,1,0,1,0,1,2,1,20,135,0,0,1,36,1,0,1,0,1,1,1,72,1,0,1,0,1,1,1,112,138,0,0,1,1,1,32,131,0,0,1,28,1,0,1,0,1,1,1,17,1,255,1,254,1,3,132,0,0,1,4,131,0,0,1,28,134,0,0,1,1,1,10,131,0,0,1,108,1,0,1,2,131,0,1,6,133,0,0,1,116,131,0,0,1,132,131,0,1,228,2,0,3,131,0,1,1,133,0,0,1,236,135,0,0,1,252,1,0,1,3,1,0,2,1,0,1,1,133,0,1,236,134,0,0,1,1,1,3,1,0,2,3,0,3,2,0,1,133,0,1,236,132,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,212,1,0,1,17,1,0,1,5,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,36,1,0,1,0,1,16,1,37,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,80,1,11,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,16,1,194,1,0,1,18,133,0,0,1,96,1,22,1,96,1,28,1,18,1,0,1,18,133,0,0,1,32,1,34,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,36,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,1,0,1,1,1,0,1,196,1,179,1,0,1,224,1,1,2,1,0,3,188,64,1,132,0,2,65,194,3,0,0,255,4,33,24,32,33,5,15,31,246,136,0,6,0,64,0,33,8,48,7,33,15,31,254,200,0,0,8,64,0,33,40,80,33,15,31,9,254,200,0,0,64,0,200,8,0,6,1,0,198,177,108,139,131,5,10,200,3,0,1,0,176,198,0,167,5,4,255,0,200,2,131,0,9,96,0,0,243,1,0,0,48,32,133,0,2,177,226,131,0,2,184,32,133,0,7,65,194,0,0,255,200,8,8,0,5,0,177,177,0,224,0,9,5,0,200,1,0,1,0,24,24,10,198,209,5,5,255,88,38,0,1,0,11,22,177,108,161,0,255,129,160,17,1,4,12,0,27,177,108,225,5,0,129,200,1,0,1,13,0,27,108,0,225,1,1,0,20,7,0,1,0,12,192,176,177,160,1,255,0,12,34,0,4,0,131,108,14,225,4,1,5,20,4,0,4,1,177,108,177,225,0,15,1,0,12,135,4,4,0,192,26,198,225,4,0,0,200,1,3,131,0,9,24,178,0,224,4,4,0,200,3,131,0,16,176,198,176,235,0,3,3,200,1,128,62,0,109,109,27,145,131,0,16,200,2,128,62,0,109,109,27,145,0,1,1,200,4,128,62,17,0,109,109,27,145,0,2,2,200,8,128,62,0,109,109,27,145,18,0,3,3,200,3,128,0,0,197,197,0,226,1,1,0,200,15,128,1,1,132,0,3,226,2,2,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'textureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 textureSizeOffset'</summary><param name="value"/>
		public void SetTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 textureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 TextureSizeOffset
		{
			set
			{
				this.SetTextureSizeOffset(ref value);
			}
		}
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[5] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 velocityScale'</summary>
		public Microsoft.Xna.Framework.Vector2 VelocityScale
		{
			set
			{
				this.SetVelocityScale(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D DisplaySampler'</summary>
		public Xen.Graphics.TextureSamplerState DisplaySampler
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
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D ColourSampler'</summary>
		public Xen.Graphics.TextureSamplerState ColourSampler
		{
			get
			{
				return this.vts[1];
			}
			set
			{
				if ((value != this.vts[1]))
				{
					this.vts[1] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D PositionSampler'</summary>
		public Xen.Graphics.TextureSamplerState PositionSampler
		{
			get
			{
				return this.vts[0];
			}
			set
			{
				if ((value != this.vts[0]))
				{
					this.vts[0] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D VelocitySampler'</summary>
		public Xen.Graphics.TextureSamplerState VelocitySampler
		{
			get
			{
				return this.vts[2];
			}
			set
			{
				if ((value != this.vts[2]))
				{
					this.vts[2] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D PositionTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D PositionTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[0]));
			}
			set
			{
				if ((value != this.vtx[0]))
				{
					this.vtc = true;
					this.vtx[0] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D ColourTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D ColourTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[1]));
			}
			set
			{
				if ((value != this.vtx[1]))
				{
					this.vtc = true;
					this.vtx[1] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D VelocityTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D VelocityTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[2]));
			}
			set
			{
				if ((value != this.vtx[2]))
				{
					this.vtc = true;
					this.vtx[2] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D DisplayTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D DisplayTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D DisplaySampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D ColourSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid2;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid3;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D ColourTexture'</summary>
		static int tid1;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D DisplayTexture'</summary>
		static int tid4;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[6];
		/// <summary>Bound vertex textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] vtx = new Microsoft.Xna.Framework.Graphics.Texture[3];
		/// <summary>Bound vertex samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] vts = new Xen.Graphics.TextureSamplerState[3];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticles_GpuTex_UserOffset' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 42 instruction slots used (6 texture, 36 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used (1 texture, 2 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticles_GpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticles_GpuTex_UserOffset' shader</summary>
		public DrawVelocityParticles_GpuTex_UserOffset()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticles_GpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticles_GpuTex_UserOffset.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticles_GpuTex_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticles_GpuTex_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityParticles_GpuTex_UserOffset.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticles_GpuTex_UserOffset.sid2 = state.GetNameUniqueID("UserSampler");
			DrawVelocityParticles_GpuTex_UserOffset.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticles_GpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticles_GpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticles_GpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawVelocityParticles_GpuTex_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticles_GpuTex_UserOffset.gd))
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
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticles_GpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticles_GpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticles_GpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticles_GpuTex_UserOffset.fx, DrawVelocityParticles_GpuTex_UserOffset.fxb, 47, 5);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.vtc) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticles_GpuTex_UserOffset.vin[i]));
			index = DrawVelocityParticles_GpuTex_UserOffset.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,140,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,176,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,212,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,248,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,28,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,64,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,188,131,0,0,1,208,139,0,0,1,224,131,0,0,1,244,138,0,0,1,1,1,4,1,0,1,0,1,1,1,24,138,0,0,1,1,1,40,1,0,1,0,1,1,1,60,138,0,0,1,1,1,128,135,0,0,1,1,1,0,1,0,1,1,1,124,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,80,1,0,1,0,1,1,1,76,131,0,0,1,93,134,0,0,1,1,1,104,1,0,1,0,1,1,1,100,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,135,0,0,1,244,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,72,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,72,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,33,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,200,1,15,1,128,131,0,1,108,2,0,225,1,2,150,0,0,132,255,0,138,0,0,1,3,1,204,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,172,1,0,1,0,1,2,1,32,135,0,0,1,36,1,0,1,0,1,1,1,72,1,0,1,0,1,1,1,112,138,0,0,1,1,1,32,131,0,0,1,28,1,0,1,0,1,1,1,17,1,255,1,254,1,3,132,0,0,1,4,131,0,0,1,28,134,0,0,1,1,1,10,131,0,0,1,108,1,0,1,2,131,0,1,6,133,0,0,1,116,131,0,0,1,132,131,0,1,228,2,0,3,131,0,1,1,133,0,0,1,236,135,0,0,1,252,1,0,1,3,1,0,2,1,0,1,1,133,0,1,236,134,0,0,1,1,1,3,1,0,2,3,0,3,2,0,1,133,0,1,236,132,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,224,1,0,1,17,1,0,1,4,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,38,1,0,1,0,1,16,1,37,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,80,1,11,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,16,1,194,1,0,1,18,133,0,0,1,96,1,22,1,96,1,28,1,18,1,0,1,18,133,0,0,1,48,1,34,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,37,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,1,0,1,1,1,0,1,196,1,179,1,0,1,224,1,1,2,1,0,3,188,64,1,132,0,2,65,194,3,0,0,255,4,33,8,32,33,5,15,31,254,200,0,6,0,64,0,33,40,48,7,33,15,31,254,143,0,0,8,64,0,33,24,64,33,15,31,9,254,200,0,0,64,0,200,8,0,10,1,0,198,177,108,139,4,5,5,200,11,3,0,1,0,176,198,0,167,4,255,0,2,200,2,131,0,9,96,0,0,243,1,0,0,48,32,133,0,2,177,226,131,0,2,184,32,133,0,8,65,194,0,0,255,200,8,0,9,4,0,177,177,0,224,0,4,0,10,200,1,0,1,0,24,24,198,209,4,11,4,255,88,38,0,1,0,22,177,108,161,12,0,255,129,160,17,1,3,0,27,177,108,225,5,4,0,129,20,2,131,0,13,177,108,27,225,0,4,1,12,24,1,3,4,177,14,198,108,225,0,0,1,200,7,0,1,0,192,176,0,15,160,1,255,0,20,3,0,2,0,176,197,108,224,2,3,16,3,12,36,3,3,0,177,108,108,225,0,1,1,200,7,0,11,3,0,192,26,0,225,3,0,0,200,3,131,0,9,24,178,0,224,3,3,0,200,3,131,0,16,176,198,176,235,0,2,2,200,1,128,62,0,109,109,27,145,131,0,17,200,2,128,62,0,109,109,27,145,0,1,1,200,4,128,62,0,18,109,109,27,145,0,2,2,200,8,128,62,0,109,109,27,145,0,3,5,3,36,254,192,1,131,0,16,108,226,0,0,128,200,3,128,0,0,197,197,0,226,1,1,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'textureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 textureSizeOffset'</summary><param name="value"/>
		public void SetTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 textureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 TextureSizeOffset
		{
			set
			{
				this.SetTextureSizeOffset(ref value);
			}
		}
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[5] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 velocityScale'</summary>
		public Microsoft.Xna.Framework.Vector2 VelocityScale
		{
			set
			{
				this.SetVelocityScale(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D DisplaySampler'</summary>
		public Xen.Graphics.TextureSamplerState DisplaySampler
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
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D PositionSampler'</summary>
		public Xen.Graphics.TextureSamplerState PositionSampler
		{
			get
			{
				return this.vts[0];
			}
			set
			{
				if ((value != this.vts[0]))
				{
					this.vts[0] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D UserSampler'</summary>
		public Xen.Graphics.TextureSamplerState UserSampler
		{
			get
			{
				return this.vts[2];
			}
			set
			{
				if ((value != this.vts[2]))
				{
					this.vts[2] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D VelocitySampler'</summary>
		public Xen.Graphics.TextureSamplerState VelocitySampler
		{
			get
			{
				return this.vts[1];
			}
			set
			{
				if ((value != this.vts[1]))
				{
					this.vts[1] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D PositionTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D PositionTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[0]));
			}
			set
			{
				if ((value != this.vtx[0]))
				{
					this.vtc = true;
					this.vtx[0] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D VelocityTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D VelocityTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[1]));
			}
			set
			{
				if ((value != this.vtx[1]))
				{
					this.vtc = true;
					this.vtx[1] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D UserTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D UserTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[2]));
			}
			set
			{
				if ((value != this.vtx[2]))
				{
					this.vtc = true;
					this.vtx[2] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D DisplayTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D DisplayTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D DisplaySampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D UserSampler'</summary>
		static int sid2;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid3;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D UserTexture'</summary>
		static int tid3;
		/// <summary>Name uid for texture for 'Texture2D DisplayTexture'</summary>
		static int tid4;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[6];
		/// <summary>Bound vertex textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] vtx = new Microsoft.Xna.Framework.Graphics.Texture[3];
		/// <summary>Bound vertex samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] vts = new Xen.Graphics.TextureSamplerState[3];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.sid2))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticlesColour_GpuTex_UserOffset' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 44 instruction slots used (8 texture, 36 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used (1 texture, 2 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticlesColour_GpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticlesColour_GpuTex_UserOffset' shader</summary>
		public DrawVelocityParticlesColour_GpuTex_UserOffset()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[3] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticlesColour_GpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticlesColour_GpuTex_UserOffset.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticlesColour_GpuTex_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticlesColour_GpuTex_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityParticlesColour_GpuTex_UserOffset.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityParticlesColour_GpuTex_UserOffset.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticlesColour_GpuTex_UserOffset.sid3 = state.GetNameUniqueID("UserSampler");
			DrawVelocityParticlesColour_GpuTex_UserOffset.sid4 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticlesColour_GpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticlesColour_GpuTex_UserOffset.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityParticlesColour_GpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticlesColour_GpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawVelocityParticlesColour_GpuTex_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticlesColour_GpuTex_UserOffset.gd))
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
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticlesColour_GpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticlesColour_GpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticlesColour_GpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticlesColour_GpuTex_UserOffset.fx, DrawVelocityParticlesColour_GpuTex_UserOffset.fxb, 49, 5);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.vtc) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticlesColour_GpuTex_UserOffset.vin[i]));
			index = DrawVelocityParticlesColour_GpuTex_UserOffset.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,128,135,0,1,3,131,0,1,1,131,0,1,128,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,164,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,200,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,131,0,0,1,236,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,16,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,51,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,52,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,140,131,0,0,1,160,139,0,0,1,176,131,0,0,1,196,139,0,0,1,212,131,0,0,1,232,139,0,0,1,248,1,0,1,0,1,1,1,12,138,0,0,1,1,1,28,1,0,1,0,1,1,1,48,138,0,0,1,1,1,116,135,0,0,1,1,1,0,1,0,1,1,1,112,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,68,1,0,1,0,1,1,1,64,131,0,0,1,93,134,0,0,1,1,1,92,1,0,1,0,1,1,1,88,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,135,0,0,1,244,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,72,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,72,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,33,1,31,1,31,1,246,1,136,1,0,1,0,2,64,0,2,22,16,133,0,1,27,2,226,0,3,0,1,200,2,15,128,131,0,4,108,0,225,2,150,0,0,132,255,0,138,0,0,1,3,1,240,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,196,1,0,1,0,1,2,1,44,135,0,0,1,36,1,0,1,0,1,1,1,96,1,0,1,0,1,1,1,136,138,0,0,1,1,1,56,131,0,0,1,28,1,0,1,0,1,1,1,44,1,255,1,254,1,3,132,0,0,1,5,131,0,0,1,28,134,0,0,1,1,1,37,131,0,0,1,128,1,0,1,2,131,0,1,6,133,0,0,1,136,131,0,1,152,131,0,1,248,2,0,3,131,0,1,1,132,0,1,1,135,0,0,1,1,1,16,1,0,1,3,2,0,1,2,0,1,132,0,1,1,135,0,0,1,1,1,23,1,0,2,3,0,3,2,0,1,132,0,1,1,135,0,0,1,1,1,30,1,0,2,3,0,3,3,0,1,132,0,1,1,133,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,95,1,118,1,115,1,95,1,115,1,51,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,136,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,236,1,0,1,17,1,0,1,5,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,38,1,0,1,0,1,16,1,39,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,96,1,11,1,18,1,0,1,18,1,0,1,5,1,80,132,0,0,1,96,1,17,1,194,1,0,1,18,133,0,0,1,96,1,23,1,96,1,29,1,18,1,0,1,18,133,0,0,1,48,1,35,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,38,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,1,0,1,1,1,0,2,196,179,3,0,224,1,4,1,0,188,64,1,1,132,0,4,65,194,0,0,5,255,33,24,32,33,6,15,31,246,136,0,0,7,64,0,33,8,48,33,15,8,31,254,200,0,0,64,0,33,9,56,64,33,15,31,254,143,0,0,10,64,0,33,40,80,33,15,31,254,200,11,0,0,64,0,200,8,0,1,0,198,177,2,108,139,131,5,12,200,3,0,1,0,176,198,0,167,5,255,0,2,200,2,131,0,9,96,0,0,243,1,0,0,48,32,133,0,2,177,226,131,0,2,184,32,133,0,9,65,194,0,0,255,200,8,0,5,10,0,177,177,0,224,0,5,0,200,1,11,0,1,0,24,24,198,209,5,5,255,88,12,38,0,1,0,22,177,108,161,0,255,129,160,13,17,1,4,0,27,177,108,225,5,0,129,20,2,131,0,13,177,108,27,225,0,5,1,12,24,1,4,4,177,14,198,108,225,0,0,1,200,7,0,1,0,192,176,0,15,160,1,255,0,20,3,0,3,0,176,197,108,224,3,4,16,4,12,36,4,4,0,177,108,108,225,0,1,1,200,7,0,11,4,0,192,26,0,225,4,0,0,200,3,131,0,9,24,178,0,224,4,4,0,200,3,131,0,16,176,198,176,235,0,3,3,200,1,128,62,0,109,109,27,145,131,0,17,200,2,128,62,0,109,109,27,145,0,1,1,200,4,128,62,0,18,109,109,27,145,0,2,2,200,8,128,62,0,109,109,27,145,0,3,17,3,200,3,128,0,0,197,197,0,226,1,1,0,200,15,128,1,132,0,3,226,2,2,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'textureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 textureSizeOffset'</summary><param name="value"/>
		public void SetTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 textureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 TextureSizeOffset
		{
			set
			{
				this.SetTextureSizeOffset(ref value);
			}
		}
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[5] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 velocityScale'</summary>
		public Microsoft.Xna.Framework.Vector2 VelocityScale
		{
			set
			{
				this.SetVelocityScale(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D DisplaySampler'</summary>
		public Xen.Graphics.TextureSamplerState DisplaySampler
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
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D ColourSampler'</summary>
		public Xen.Graphics.TextureSamplerState ColourSampler
		{
			get
			{
				return this.vts[1];
			}
			set
			{
				if ((value != this.vts[1]))
				{
					this.vts[1] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D PositionSampler'</summary>
		public Xen.Graphics.TextureSamplerState PositionSampler
		{
			get
			{
				return this.vts[0];
			}
			set
			{
				if ((value != this.vts[0]))
				{
					this.vts[0] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D UserSampler'</summary>
		public Xen.Graphics.TextureSamplerState UserSampler
		{
			get
			{
				return this.vts[3];
			}
			set
			{
				if ((value != this.vts[3]))
				{
					this.vts[3] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D VelocitySampler'</summary>
		public Xen.Graphics.TextureSamplerState VelocitySampler
		{
			get
			{
				return this.vts[2];
			}
			set
			{
				if ((value != this.vts[2]))
				{
					this.vts[2] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D PositionTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D PositionTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[0]));
			}
			set
			{
				if ((value != this.vtx[0]))
				{
					this.vtc = true;
					this.vtx[0] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D ColourTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D ColourTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[1]));
			}
			set
			{
				if ((value != this.vtx[1]))
				{
					this.vtc = true;
					this.vtx[1] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D VelocityTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D VelocityTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[2]));
			}
			set
			{
				if ((value != this.vtx[2]))
				{
					this.vtc = true;
					this.vtx[2] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D UserTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D UserTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[3]));
			}
			set
			{
				if ((value != this.vtx[3]))
				{
					this.vtc = true;
					this.vtx[3] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D DisplayTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D DisplayTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D DisplaySampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D ColourSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid2;
		/// <summary>Name uid for sampler for 'Sampler2D UserSampler'</summary>
		static int sid3;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid4;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D ColourTexture'</summary>
		static int tid1;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D UserTexture'</summary>
		static int tid3;
		/// <summary>Name uid for texture for 'Texture2D DisplayTexture'</summary>
		static int tid4;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[6];
		/// <summary>Bound vertex textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] vtx = new Microsoft.Xna.Framework.Graphics.Texture[4];
		/// <summary>Bound vertex samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] vts = new Xen.Graphics.TextureSamplerState[4];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.sid3))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.sid4))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
}
#else
namespace Xen.Logo
{
	
	/// <summary><para>Technique 'DrawVelocityParticles_GpuTex' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 39 instruction slots used (4 texture, 35 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used (1 texture, 2 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticles_GpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticles_GpuTex' shader</summary>
		public DrawVelocityParticles_GpuTex()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticles_GpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticles_GpuTex.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticles_GpuTex.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticles_GpuTex.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityParticles_GpuTex.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticles_GpuTex.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticles_GpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticles_GpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticles_GpuTex.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticles_GpuTex.gd))
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
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticles_GpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticles_GpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticles_GpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticles_GpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticles_GpuTex.fx, DrawVelocityParticles_GpuTex.fxb, 44, 5);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.vtc) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticles_GpuTex.vin[i]));
			index = DrawVelocityParticles_GpuTex.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {20,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,87,249,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,141,207,127,109,188,79,255,255,155,180,221,175,167,239,43,184,159,213,7,125,253,254,151,205,239,63,165,159,191,209,175,33,253,254,107,157,54,191,190,182,105,118,92,155,255,109,168,205,174,107,243,59,117,6,192,109,86,2,7,95,253,102,218,206,127,126,45,250,255,111,26,249,220,128,2,140,215,243,108,6,202,253,186,250,57,62,3,13,241,206,111,231,189,179,79,255,127,233,253,253,247,209,255,255,37,239,239,255,136,254,255,203,189,191,127,29,2,246,219,121,56,255,1,250,59,126,252,222,250,59,240,251,83,244,251,17,125,182,69,255,255,83,245,239,167,244,251,239,233,181,51,207,255,77,143,1,203,180,253,181,255,239,255,251,255,250,191,127,215,95,227,228,205,241,19,240,200,151,250,25,218,40,207,164,223,166,127,118,126,13,195,27,191,214,175,113,160,239,43,253,254,218,95,135,168,252,107,242,127,242,208,199,247,126,255,157,95,227,139,98,90,87,77,117,222,166,91,175,238,164,223,126,254,250,121,42,212,74,79,170,197,170,32,134,75,31,142,247,62,29,63,188,191,55,222,59,216,223,255,53,126,23,66,245,215,253,53,126,205,63,232,215,248,53,126,195,63,233,119,17,180,255,164,95,227,55,248,77,255,162,39,220,245,111,74,159,255,103,244,247,127,246,23,81,63,191,214,175,249,107,252,230,244,247,255,253,39,253,186,248,238,55,224,239,254,32,250,252,15,250,191,255,111,69,226,215,144,177,154,223,255,59,32,255,107,255,95,52,214,47,237,88,127,253,95,83,62,195,87,102,172,24,196,239,253,107,72,231,191,30,253,251,7,96,60,191,134,204,149,25,255,191,165,48,255,43,254,12,227,254,245,126,141,255,73,63,19,254,253,107,255,218,95,147,191,249,117,152,167,127,24,143,202,68,111,46,84,14,122,159,95,126,205,57,250,9,98,245,95,239,215,248,77,255,34,2,241,123,16,185,255,65,129,246,7,253,30,58,95,127,16,205,83,48,119,230,247,95,211,253,142,54,255,145,204,245,175,65,115,246,107,235,239,191,230,31,244,107,209,231,191,14,179,254,175,253,31,253,26,191,198,127,253,39,17,237,254,34,252,255,215,98,210,255,154,127,208,175,243,107,252,53,232,87,230,156,249,228,215,160,207,126,141,191,232,183,100,184,191,22,254,254,99,127,99,254,238,215,225,239,232,255,127,240,111,193,240,208,246,175,161,255,127,133,255,255,193,94,251,63,200,192,150,246,95,253,193,128,253,107,234,119,191,206,175,241,21,243,26,190,7,30,191,14,139,245,175,77,159,255,103,192,131,112,255,207,254,32,249,236,55,98,216,191,222,175,241,167,17,190,127,219,95,244,251,51,12,240,228,255,246,7,253,154,196,175,191,63,247,33,127,131,127,209,199,175,69,112,126,77,230,217,255,76,113,248,117,232,231,87,248,253,15,250,141,236,24,254,154,63,24,120,155,239,127,77,198,255,175,249,131,127,45,139,227,95,67,125,254,223,138,151,124,255,107,42,140,95,159,241,254,117,180,13,228,69,126,23,250,254,6,212,238,255,254,131,126,93,26,31,225,18,224,99,218,8,188,191,134,219,161,15,161,203,175,77,176,165,141,249,91,223,249,147,64,167,95,139,199,240,107,252,193,58,135,160,201,159,244,107,253,26,255,181,71,39,211,254,255,254,131,84,94,25,71,192,255,13,100,30,254,163,95,131,191,255,229,250,247,175,245,31,73,59,243,247,175,195,127,255,90,246,239,223,128,255,254,181,233,111,233,255,55,253,143,0,11,58,224,255,9,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'textureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 textureSizeOffset'</summary><param name="value"/>
		public void SetTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 textureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 TextureSizeOffset
		{
			set
			{
				this.SetTextureSizeOffset(ref value);
			}
		}
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[5] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 velocityScale'</summary>
		public Microsoft.Xna.Framework.Vector2 VelocityScale
		{
			set
			{
				this.SetVelocityScale(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D DisplaySampler'</summary>
		public Xen.Graphics.TextureSamplerState DisplaySampler
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
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D PositionSampler'</summary>
		public Xen.Graphics.TextureSamplerState PositionSampler
		{
			get
			{
				return this.vts[0];
			}
			set
			{
				if ((value != this.vts[0]))
				{
					this.vts[0] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D VelocitySampler'</summary>
		public Xen.Graphics.TextureSamplerState VelocitySampler
		{
			get
			{
				return this.vts[1];
			}
			set
			{
				if ((value != this.vts[1]))
				{
					this.vts[1] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D PositionTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D PositionTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[0]));
			}
			set
			{
				if ((value != this.vtx[0]))
				{
					this.vtc = true;
					this.vtx[0] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D VelocityTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D VelocityTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[1]));
			}
			set
			{
				if ((value != this.vtx[1]))
				{
					this.vtc = true;
					this.vtx[1] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D DisplayTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D DisplayTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D DisplaySampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid2;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D DisplayTexture'</summary>
		static int tid4;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[6];
		/// <summary>Bound vertex textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] vtx = new Microsoft.Xna.Framework.Graphics.Texture[2];
		/// <summary>Bound vertex samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] vts = new Xen.Graphics.TextureSamplerState[2];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticlesColour_GpuTex' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 41 instruction slots used (6 texture, 35 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used (1 texture, 2 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticlesColour_GpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticlesColour_GpuTex' shader</summary>
		public DrawVelocityParticlesColour_GpuTex()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticlesColour_GpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticlesColour_GpuTex.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticlesColour_GpuTex.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticlesColour_GpuTex.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityParticlesColour_GpuTex.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityParticlesColour_GpuTex.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticlesColour_GpuTex.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticlesColour_GpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticlesColour_GpuTex.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityParticlesColour_GpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticlesColour_GpuTex.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticlesColour_GpuTex.gd))
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
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticlesColour_GpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticlesColour_GpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticlesColour_GpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticlesColour_GpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticlesColour_GpuTex.fx, DrawVelocityParticlesColour_GpuTex.fxb, 46, 5);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.vtc) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticlesColour_GpuTex.vin[i]));
			index = DrawVelocityParticlesColour_GpuTex.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {132,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,159,244,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,141,207,127,109,188,79,255,255,155,180,221,175,167,239,43,184,159,213,7,125,253,254,151,205,239,63,165,159,191,209,175,33,253,254,107,157,54,191,190,182,105,118,92,155,255,109,168,205,174,107,243,59,117,6,96,219,236,185,54,191,103,172,205,74,250,194,87,191,153,182,243,159,95,139,254,255,155,70,62,55,160,0,227,245,60,155,129,186,191,158,126,254,27,252,26,66,103,188,243,219,121,239,236,211,255,95,122,127,255,125,244,255,127,201,251,251,63,162,255,255,114,239,239,95,135,128,253,118,30,206,91,244,251,99,239,239,63,76,127,199,143,159,209,223,129,239,159,162,223,191,164,207,158,211,255,255,84,253,123,78,191,207,188,118,230,249,191,233,49,96,121,62,126,237,255,251,255,254,191,254,239,223,245,215,56,121,115,252,4,124,245,165,126,134,54,202,103,233,183,233,159,157,95,195,240,211,175,245,107,28,232,251,74,207,191,246,215,33,170,255,154,252,159,60,244,241,189,223,127,231,215,248,162,152,214,85,83,157,183,233,214,171,59,233,183,159,191,126,158,10,245,210,147,106,177,42,136,73,211,135,227,189,79,199,15,239,239,141,247,14,246,247,127,141,223,133,80,253,117,127,141,95,243,15,250,53,126,141,223,240,79,250,93,4,237,63,233,215,248,13,126,211,191,232,9,119,253,155,210,231,255,25,253,253,159,253,69,212,207,175,245,107,254,26,191,57,253,253,127,255,73,191,46,190,251,13,248,187,63,136,62,255,131,254,239,255,91,145,248,53,100,172,230,247,45,76,208,175,253,127,209,88,127,202,142,245,222,175,41,159,121,50,149,142,232,179,242,215,144,206,127,61,250,183,165,159,127,24,253,255,63,251,53,220,248,255,59,133,249,127,240,103,24,247,175,199,243,135,231,183,250,53,241,217,175,69,159,37,150,79,69,14,254,218,191,246,215,228,214,191,14,243,206,15,227,81,217,234,205,143,202,83,252,243,189,254,231,151,95,115,62,127,226,215,248,53,126,221,95,239,215,248,77,255,34,2,241,123,208,212,252,131,2,237,15,250,61,116,110,255,32,154,211,96,158,205,239,191,166,247,251,175,229,126,71,251,255,72,120,228,215,160,185,254,181,245,247,95,243,15,250,181,232,243,95,139,167,231,215,252,131,126,157,95,227,175,65,127,194,23,204,75,191,6,125,246,107,252,69,191,37,195,248,181,240,247,31,251,27,243,119,191,14,127,71,255,255,131,127,11,22,63,180,253,107,232,255,95,225,255,127,176,215,254,15,50,176,165,253,87,127,48,96,255,154,250,221,175,243,107,124,197,252,136,239,105,94,255,162,95,135,85,193,175,77,159,255,103,192,131,240,252,207,254,32,249,236,55,98,216,191,222,175,241,167,253,69,191,222,175,241,183,253,69,191,63,53,2,222,164,241,254,160,95,147,120,250,215,97,28,126,109,250,251,191,254,147,0,7,255,255,253,185,31,240,246,255,246,7,253,90,212,230,247,103,60,228,111,200,193,175,203,48,126,109,238,3,255,255,181,116,92,191,22,245,67,191,255,65,191,145,29,231,95,243,7,99,108,230,251,95,147,199,248,215,252,193,191,22,195,199,56,254,26,194,235,255,86,220,229,251,95,83,97,252,250,60,182,95,71,219,64,238,228,119,193,247,55,160,118,255,247,31,244,235,18,13,8,151,0,31,211,70,224,253,53,220,14,125,8,237,126,109,130,45,109,204,223,250,206,159,4,90,254,90,60,134,95,227,15,86,154,128,110,127,210,175,245,107,252,215,30,45,77,251,255,251,15,82,185,103,28,1,255,55,144,185,250,143,240,57,105,88,253,251,215,250,143,164,157,249,251,215,225,191,127,45,251,247,111,192,127,255,218,244,55,244,199,255,19,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'textureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 textureSizeOffset'</summary><param name="value"/>
		public void SetTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 textureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 TextureSizeOffset
		{
			set
			{
				this.SetTextureSizeOffset(ref value);
			}
		}
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[5] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 velocityScale'</summary>
		public Microsoft.Xna.Framework.Vector2 VelocityScale
		{
			set
			{
				this.SetVelocityScale(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D DisplaySampler'</summary>
		public Xen.Graphics.TextureSamplerState DisplaySampler
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
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D ColourSampler'</summary>
		public Xen.Graphics.TextureSamplerState ColourSampler
		{
			get
			{
				return this.vts[1];
			}
			set
			{
				if ((value != this.vts[1]))
				{
					this.vts[1] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D PositionSampler'</summary>
		public Xen.Graphics.TextureSamplerState PositionSampler
		{
			get
			{
				return this.vts[0];
			}
			set
			{
				if ((value != this.vts[0]))
				{
					this.vts[0] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D VelocitySampler'</summary>
		public Xen.Graphics.TextureSamplerState VelocitySampler
		{
			get
			{
				return this.vts[2];
			}
			set
			{
				if ((value != this.vts[2]))
				{
					this.vts[2] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D PositionTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D PositionTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[0]));
			}
			set
			{
				if ((value != this.vtx[0]))
				{
					this.vtc = true;
					this.vtx[0] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D ColourTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D ColourTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[1]));
			}
			set
			{
				if ((value != this.vtx[1]))
				{
					this.vtc = true;
					this.vtx[1] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D VelocityTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D VelocityTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[2]));
			}
			set
			{
				if ((value != this.vtx[2]))
				{
					this.vtc = true;
					this.vtx[2] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D DisplayTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D DisplayTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D DisplaySampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D ColourSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid2;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid3;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D ColourTexture'</summary>
		static int tid1;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D DisplayTexture'</summary>
		static int tid4;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[6];
		/// <summary>Bound vertex textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] vtx = new Microsoft.Xna.Framework.Graphics.Texture[3];
		/// <summary>Bound vertex samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] vts = new Xen.Graphics.TextureSamplerState[3];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticles_GpuTex_UserOffset' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 42 instruction slots used (6 texture, 36 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used (1 texture, 2 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticles_GpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticles_GpuTex_UserOffset' shader</summary>
		public DrawVelocityParticles_GpuTex_UserOffset()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticles_GpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticles_GpuTex_UserOffset.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticles_GpuTex_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticles_GpuTex_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityParticles_GpuTex_UserOffset.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticles_GpuTex_UserOffset.sid2 = state.GetNameUniqueID("UserSampler");
			DrawVelocityParticles_GpuTex_UserOffset.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticles_GpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticles_GpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticles_GpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawVelocityParticles_GpuTex_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticles_GpuTex_UserOffset.gd))
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
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticles_GpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticles_GpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticles_GpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticles_GpuTex_UserOffset.fx, DrawVelocityParticles_GpuTex_UserOffset.fxb, 47, 5);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.vtc) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticles_GpuTex_UserOffset.vin[i]));
			index = DrawVelocityParticles_GpuTex_UserOffset.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {160,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,159,244,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,141,207,127,109,188,79,255,255,155,180,221,175,167,239,43,184,159,213,7,125,253,254,151,205,239,63,165,159,191,209,175,33,253,254,107,157,54,191,190,182,105,118,92,155,255,109,168,205,174,107,243,59,117,6,96,219,236,185,54,191,103,172,205,74,250,194,87,191,153,182,243,159,95,139,254,255,155,70,62,55,160,0,227,245,60,155,129,186,191,158,126,254,27,252,26,66,103,188,243,219,121,239,236,211,255,95,122,127,255,125,244,255,127,201,251,251,63,162,255,255,114,239,239,95,135,128,253,118,30,206,91,244,251,99,239,239,63,76,127,199,143,159,209,223,129,239,159,162,223,191,164,207,158,211,255,255,84,253,123,78,191,207,188,118,230,249,191,233,49,96,121,62,126,237,255,251,255,254,191,254,239,223,245,215,56,121,115,252,4,124,245,165,126,134,54,202,103,233,183,233,159,157,95,195,240,211,175,245,107,28,232,251,74,207,191,246,215,33,170,255,154,252,159,60,244,241,189,223,127,231,215,248,162,152,214,85,83,157,183,233,214,171,59,233,183,159,191,126,158,10,245,210,147,106,177,42,136,73,211,135,227,189,79,199,15,239,239,141,247,14,246,247,127,141,223,133,80,253,117,127,141,95,243,15,250,53,126,141,223,240,79,250,93,4,237,63,233,215,248,13,126,211,191,232,9,119,253,155,210,231,255,25,253,253,159,253,69,212,207,175,245,107,254,26,191,57,253,253,127,255,73,191,46,190,251,13,248,187,63,136,62,255,131,254,239,255,91,145,248,53,100,172,230,247,167,152,160,95,251,255,162,177,254,148,29,235,189,95,83,62,243,100,42,29,209,103,229,175,33,157,255,122,244,111,75,63,255,48,250,255,127,246,107,184,241,255,119,10,243,255,224,207,48,238,95,143,231,15,207,111,245,107,226,179,95,139,62,75,44,159,138,28,252,181,127,237,175,201,173,127,29,230,157,31,198,163,178,213,155,31,149,167,248,231,123,253,207,47,191,230,124,254,196,175,241,107,252,186,191,222,175,241,155,254,69,4,226,247,160,169,249,7,5,218,31,244,123,232,220,254,65,52,167,193,60,155,223,127,77,239,247,95,203,253,142,246,255,145,240,200,175,65,115,253,107,235,239,191,230,31,244,107,209,231,191,14,139,208,175,253,31,253,26,191,198,127,253,39,17,125,255,34,252,255,215,226,41,251,53,255,160,95,231,215,248,107,128,131,240,10,243,215,175,65,159,253,26,127,209,111,201,112,127,45,252,253,199,254,198,252,221,175,195,223,209,255,255,224,223,130,225,161,237,95,67,255,255,10,255,255,131,189,246,127,144,129,45,237,191,250,131,1,251,215,212,239,126,157,95,227,43,230,81,124,15,60,126,29,86,15,191,54,125,254,159,1,15,194,253,63,251,131,228,179,223,136,97,255,122,191,198,159,70,248,254,109,127,209,239,207,48,192,203,255,219,31,244,107,18,159,3,230,175,69,239,253,154,204,219,255,153,246,249,107,209,207,175,240,251,31,244,27,233,223,232,255,215,144,207,248,239,95,83,241,253,181,44,78,232,227,255,86,60,228,251,95,83,97,252,250,118,76,104,3,185,250,117,248,119,161,231,111,64,237,254,239,63,232,215,165,241,16,46,1,62,166,141,192,251,107,184,29,250,16,58,252,218,4,91,218,152,191,229,157,191,230,79,2,93,126,45,234,131,58,253,131,117,206,208,238,79,250,181,126,141,255,154,219,252,254,220,135,208,224,215,34,26,252,254,60,38,249,27,178,47,99,4,252,255,250,15,130,204,27,218,10,252,255,251,15,82,61,240,107,97,46,129,207,111,32,243,244,31,253,26,252,253,47,215,191,127,173,255,72,218,153,191,127,29,254,251,215,178,127,255,6,252,247,175,77,127,11,190,191,233,127,4,88,208,45,255,79,0,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'textureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 textureSizeOffset'</summary><param name="value"/>
		public void SetTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 textureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 TextureSizeOffset
		{
			set
			{
				this.SetTextureSizeOffset(ref value);
			}
		}
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[5] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 velocityScale'</summary>
		public Microsoft.Xna.Framework.Vector2 VelocityScale
		{
			set
			{
				this.SetVelocityScale(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D DisplaySampler'</summary>
		public Xen.Graphics.TextureSamplerState DisplaySampler
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
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D PositionSampler'</summary>
		public Xen.Graphics.TextureSamplerState PositionSampler
		{
			get
			{
				return this.vts[0];
			}
			set
			{
				if ((value != this.vts[0]))
				{
					this.vts[0] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D UserSampler'</summary>
		public Xen.Graphics.TextureSamplerState UserSampler
		{
			get
			{
				return this.vts[2];
			}
			set
			{
				if ((value != this.vts[2]))
				{
					this.vts[2] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D VelocitySampler'</summary>
		public Xen.Graphics.TextureSamplerState VelocitySampler
		{
			get
			{
				return this.vts[1];
			}
			set
			{
				if ((value != this.vts[1]))
				{
					this.vts[1] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D PositionTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D PositionTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[0]));
			}
			set
			{
				if ((value != this.vtx[0]))
				{
					this.vtc = true;
					this.vtx[0] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D VelocityTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D VelocityTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[1]));
			}
			set
			{
				if ((value != this.vtx[1]))
				{
					this.vtc = true;
					this.vtx[1] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D UserTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D UserTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[2]));
			}
			set
			{
				if ((value != this.vtx[2]))
				{
					this.vtc = true;
					this.vtx[2] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D DisplayTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D DisplayTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D DisplaySampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D UserSampler'</summary>
		static int sid2;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid3;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D UserTexture'</summary>
		static int tid3;
		/// <summary>Name uid for texture for 'Texture2D DisplayTexture'</summary>
		static int tid4;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[6];
		/// <summary>Bound vertex textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] vtx = new Microsoft.Xna.Framework.Graphics.Texture[3];
		/// <summary>Bound vertex samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] vts = new Xen.Graphics.TextureSamplerState[3];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.sid2))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_GpuTex_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticlesColour_GpuTex_UserOffset' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 44 instruction slots used (8 texture, 36 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used (1 texture, 2 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticlesColour_GpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticlesColour_GpuTex_UserOffset' shader</summary>
		public DrawVelocityParticlesColour_GpuTex_UserOffset()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[3] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticlesColour_GpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticlesColour_GpuTex_UserOffset.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticlesColour_GpuTex_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticlesColour_GpuTex_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityParticlesColour_GpuTex_UserOffset.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityParticlesColour_GpuTex_UserOffset.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticlesColour_GpuTex_UserOffset.sid3 = state.GetNameUniqueID("UserSampler");
			DrawVelocityParticlesColour_GpuTex_UserOffset.sid4 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticlesColour_GpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticlesColour_GpuTex_UserOffset.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityParticlesColour_GpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticlesColour_GpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawVelocityParticlesColour_GpuTex_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticlesColour_GpuTex_UserOffset.gd))
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
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticlesColour_GpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticlesColour_GpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticlesColour_GpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticlesColour_GpuTex_UserOffset.fx, DrawVelocityParticlesColour_GpuTex_UserOffset.fxb, 49, 5);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return ((this.vreg_change | this.vtc) 
						| this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticlesColour_GpuTex_UserOffset.vin[i]));
			index = DrawVelocityParticlesColour_GpuTex_UserOffset.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {208,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,31,246,107,202,239,191,54,254,166,255,255,65,250,221,175,71,255,255,117,244,179,159,237,7,125,253,254,151,205,239,63,165,159,191,209,175,33,253,254,101,157,54,191,190,182,105,118,92,155,127,106,168,205,174,107,243,223,13,181,217,115,109,126,179,206,32,109,155,123,174,205,126,172,205,74,240,193,87,191,153,182,243,159,95,139,254,255,155,70,62,55,160,0,227,245,60,155,229,181,208,0,159,255,6,191,134,204,5,222,73,189,119,254,56,250,255,95,228,253,253,55,209,255,255,49,239,239,127,141,254,255,95,121,127,255,111,244,255,223,200,195,249,119,162,223,119,188,191,223,233,239,248,177,210,223,129,239,159,162,223,63,165,207,126,79,250,255,159,170,127,255,190,244,251,239,237,181,51,207,255,77,143,1,11,28,126,141,95,251,255,254,191,255,175,255,251,119,253,53,78,222,28,63,249,157,232,207,47,245,51,180,193,223,244,164,223,166,127,118,126,13,195,115,191,214,175,113,160,239,43,61,255,218,95,135,168,254,107,242,127,242,208,199,247,126,255,157,95,227,139,98,90,87,77,117,222,166,91,175,238,164,223,126,254,250,121,42,212,75,79,170,197,170,40,233,151,135,227,189,79,199,15,239,239,141,247,14,246,247,127,141,223,133,80,253,117,127,141,95,243,15,250,53,126,141,223,240,79,250,93,4,237,63,233,215,248,13,126,211,191,232,9,119,253,155,210,231,255,25,253,253,159,253,69,212,207,175,245,107,254,26,191,57,253,253,127,255,73,191,46,190,251,13,248,187,63,136,62,255,131,254,239,255,91,145,248,53,100,172,230,247,63,8,19,244,107,255,95,52,214,220,142,245,247,255,53,229,51,130,97,199,10,154,17,48,238,252,215,163,127,255,40,250,249,103,253,26,50,63,102,252,102,160,224,195,95,155,199,253,235,253,26,191,157,126,182,197,159,253,90,244,89,98,231,239,247,228,207,208,242,55,249,53,190,173,159,137,252,252,181,127,237,175,201,159,255,58,204,79,63,140,71,101,178,55,103,42,135,241,207,247,6,62,191,215,255,252,242,107,206,253,79,252,26,191,198,175,251,235,253,26,191,233,95,68,32,126,15,154,129,127,80,160,253,65,191,135,242,193,31,68,243,31,240,132,249,253,215,244,126,255,181,188,223,127,109,247,59,222,253,143,132,183,126,13,226,145,95,91,127,255,53,255,160,95,139,62,255,181,120,90,127,77,98,143,191,6,125,11,63,49,15,50,203,252,69,191,37,195,248,181,240,247,31,251,27,243,119,191,14,127,71,255,255,131,127,11,250,227,215,228,182,127,13,253,255,43,252,255,15,246,218,255,65,6,182,180,255,234,15,6,236,95,83,191,251,117,126,141,175,254,162,95,147,219,254,154,127,16,205,253,95,244,235,176,10,249,181,233,243,255,12,120,16,158,255,217,31,36,159,253,70,12,251,215,251,53,254,180,191,232,215,251,53,254,182,191,232,247,167,70,192,155,56,242,15,250,53,73,22,126,29,198,225,215,166,191,255,235,63,9,112,240,255,223,159,251,129,76,252,111,127,208,175,77,109,244,29,254,251,215,208,191,127,13,253,251,215,162,191,127,45,110,255,107,83,159,255,53,225,252,159,253,65,130,39,250,125,202,255,151,113,252,58,244,253,255,77,255,255,107,254,160,223,200,210,225,175,249,131,49,118,243,189,208,224,175,249,131,229,111,140,243,175,33,188,255,111,29,155,249,254,43,134,241,235,243,216,127,29,109,3,121,150,223,165,237,111,64,191,255,223,132,199,87,127,17,225,130,255,255,218,160,13,100,220,111,243,107,50,221,255,111,238,35,196,249,255,182,99,248,53,229,157,63,233,215,180,253,253,26,127,176,161,53,225,242,39,17,221,168,205,255,160,48,165,143,95,139,199,201,250,132,223,1,252,223,64,230,242,63,18,28,126,185,254,253,107,253,71,162,119,204,223,191,206,127,36,239,155,191,127,3,254,251,215,166,191,161,151,254,159,0,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'textureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 textureSizeOffset'</summary><param name="value"/>
		public void SetTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 textureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 TextureSizeOffset
		{
			set
			{
				this.SetTextureSizeOffset(ref value);
			}
		}
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[5] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 velocityScale'</summary>
		public Microsoft.Xna.Framework.Vector2 VelocityScale
		{
			set
			{
				this.SetVelocityScale(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D DisplaySampler'</summary>
		public Xen.Graphics.TextureSamplerState DisplaySampler
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
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D ColourSampler'</summary>
		public Xen.Graphics.TextureSamplerState ColourSampler
		{
			get
			{
				return this.vts[1];
			}
			set
			{
				if ((value != this.vts[1]))
				{
					this.vts[1] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D PositionSampler'</summary>
		public Xen.Graphics.TextureSamplerState PositionSampler
		{
			get
			{
				return this.vts[0];
			}
			set
			{
				if ((value != this.vts[0]))
				{
					this.vts[0] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D UserSampler'</summary>
		public Xen.Graphics.TextureSamplerState UserSampler
		{
			get
			{
				return this.vts[3];
			}
			set
			{
				if ((value != this.vts[3]))
				{
					this.vts[3] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D VelocitySampler'</summary>
		public Xen.Graphics.TextureSamplerState VelocitySampler
		{
			get
			{
				return this.vts[2];
			}
			set
			{
				if ((value != this.vts[2]))
				{
					this.vts[2] = value;
					this.vtc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D PositionTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D PositionTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[0]));
			}
			set
			{
				if ((value != this.vtx[0]))
				{
					this.vtc = true;
					this.vtx[0] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D ColourTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D ColourTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[1]));
			}
			set
			{
				if ((value != this.vtx[1]))
				{
					this.vtc = true;
					this.vtx[1] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D VelocityTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D VelocityTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[2]));
			}
			set
			{
				if ((value != this.vtx[2]))
				{
					this.vtc = true;
					this.vtx[2] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D UserTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D UserTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.vtx[3]));
			}
			set
			{
				if ((value != this.vtx[3]))
				{
					this.vtc = true;
					this.vtx[3] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D DisplayTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D DisplayTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D DisplaySampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D ColourSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid2;
		/// <summary>Name uid for sampler for 'Sampler2D UserSampler'</summary>
		static int sid3;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid4;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D ColourTexture'</summary>
		static int tid1;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D UserTexture'</summary>
		static int tid3;
		/// <summary>Name uid for texture for 'Texture2D DisplayTexture'</summary>
		static int tid4;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[6];
		/// <summary>Bound vertex textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] vtx = new Microsoft.Xna.Framework.Graphics.Texture[4];
		/// <summary>Bound vertex samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] vts = new Xen.Graphics.TextureSamplerState[4];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.sid3))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.sid4))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_GpuTex_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticles_BillboardCpu' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 28 instruction slots used, 165 registers</para><para>Pixel Shader: approximately 4 instruction slots used (1 texture, 3 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticles_BillboardCpu : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticles_BillboardCpu' shader</summary>
		public DrawVelocityParticles_BillboardCpu()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticles_BillboardCpu.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticles_BillboardCpu.cid0 = state.GetNameUniqueID("positionData");
			DrawVelocityParticles_BillboardCpu.cid1 = state.GetNameUniqueID("velocityData");
			DrawVelocityParticles_BillboardCpu.cid2 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticles_BillboardCpu.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityParticles_BillboardCpu.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticles_BillboardCpu.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[160], ref this.vreg[161], ref this.vreg[162], ref this.vreg[163], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticles_BillboardCpu.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticles_BillboardCpu.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticles_BillboardCpu.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticles_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticles_BillboardCpu.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticles_BillboardCpu.fx, DrawVelocityParticles_BillboardCpu.fxb, 30, 6);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticles_BillboardCpu.vin[i]));
			index = DrawVelocityParticles_BillboardCpu.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {80,25,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,127,150,200,239,191,54,254,166,255,175,244,239,191,156,254,255,235,232,103,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,124,200,243,235,209,255,127,255,203,230,247,159,210,207,223,232,215,16,63,243,79,83,191,211,60,191,62,218,172,154,223,191,217,17,31,244,55,211,118,254,243,107,209,255,127,211,200,231,226,179,254,223,255,55,96,188,158,103,179,188,150,182,248,28,109,127,109,253,153,122,239,252,12,245,255,39,121,56,252,91,250,59,222,249,151,244,119,192,248,83,244,251,191,140,62,251,139,232,255,127,170,254,253,247,209,239,127,151,215,206,60,255,55,61,198,135,254,143,248,203,255,251,255,254,191,254,239,223,245,215,56,121,115,252,228,119,162,63,191,212,207,208,6,127,211,147,126,155,254,217,249,53,140,79,254,107,253,26,7,250,190,210,227,175,253,117,136,106,191,38,255,39,15,125,188,247,251,239,252,26,95,20,211,186,106,170,243,54,221,122,117,39,253,246,243,215,207,83,25,125,122,82,45,86,69,73,191,60,28,239,125,58,126,120,127,111,188,119,176,191,255,107,252,46,130,234,31,244,107,254,26,191,233,223,164,191,255,73,191,198,111,240,155,254,69,79,184,235,223,148,62,255,207,254,166,95,227,55,248,207,254,34,234,231,215,250,53,127,141,223,142,254,254,191,255,166,95,87,191,251,53,126,141,255,12,127,255,65,248,142,222,225,191,255,239,255,91,17,250,53,100,220,230,247,127,13,147,252,107,253,95,52,238,191,253,215,50,227,254,11,19,249,204,31,247,159,149,200,184,129,200,95,174,227,254,246,175,97,120,229,175,253,107,127,77,234,250,215,164,153,67,108,242,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,135,62,151,95,211,143,254,137,95,227,215,248,117,255,242,95,227,55,253,139,8,196,239,65,238,244,239,97,224,25,255,154,252,229,63,233,215,225,144,227,215,254,143,126,141,95,227,191,254,147,200,127,253,139,240,255,223,146,191,255,53,233,251,95,227,79,250,181,216,175,230,223,255,96,252,61,150,239,254,38,188,79,62,55,220,238,63,232,101,250,159,161,143,191,201,252,148,119,126,45,122,231,43,188,247,7,253,70,238,239,63,24,159,121,223,255,193,127,249,175,241,213,95,68,127,255,58,191,166,182,127,153,126,197,48,4,175,95,147,252,248,175,248,255,128,243,235,59,188,254,160,95,143,127,255,181,248,247,95,147,225,253,6,244,238,255,237,189,251,235,208,119,255,247,31,244,151,17,252,191,140,198,36,159,25,156,254,154,63,8,253,226,61,138,86,232,189,95,131,223,251,117,249,239,95,27,241,5,195,149,152,226,215,230,119,240,153,251,27,223,255,53,127,146,196,31,191,14,250,255,131,149,142,104,75,159,255,215,220,230,215,225,144,82,219,43,110,191,134,210,136,227,19,122,23,120,252,6,66,227,127,8,237,254,162,95,227,151,235,223,191,22,255,253,23,219,191,127,29,254,251,47,177,127,255,6,252,247,95,74,127,11,30,191,233,127,4,88,136,117,254,159,0,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'positionData'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float4 positionData[80]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetPositionData(Microsoft.Xna.Framework.Vector4[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector4 val;
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
						> 80)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 80)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 positionData[80]'</summary>
		public Microsoft.Xna.Framework.Vector4[] PositionData
		{
			set
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'velocityData'</summary>
		private static int cid1;
		/// <summary>Set the shader array value 'float4 velocityData[80]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetVelocityData(Microsoft.Xna.Framework.Vector4[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector4 val;
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
						> 80)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 80)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 80)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 velocityData[80]'</summary>
		public Microsoft.Xna.Framework.Vector4[] VelocityData
		{
			set
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid2;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[164] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 velocityScale'</summary>
		public Microsoft.Xna.Framework.Vector2 VelocityScale
		{
			set
			{
				this.SetVelocityScale(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D DisplaySampler'</summary>
		public Xen.Graphics.TextureSamplerState DisplaySampler
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
		/// <summary>Get/Set the Bound texture for 'Texture2D DisplayTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D DisplayTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D DisplaySampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D DisplayTexture'</summary>
		static int tid4;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[165];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticles_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_BillboardCpu.cid2))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector4[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector4[] value)
		{
			if ((DrawVelocityParticles_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_BillboardCpu.cid0))
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawVelocityParticles_BillboardCpu.cid1))
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticles_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_BillboardCpu.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticles_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_BillboardCpu.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticlesColour_BillboardCpu' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 28 instruction slots used, 245 registers</para><para>Pixel Shader: approximately 4 instruction slots used (1 texture, 3 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticlesColour_BillboardCpu : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticlesColour_BillboardCpu' shader</summary>
		public DrawVelocityParticlesColour_BillboardCpu()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticlesColour_BillboardCpu.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticlesColour_BillboardCpu.cid0 = state.GetNameUniqueID("colourData");
			DrawVelocityParticlesColour_BillboardCpu.cid1 = state.GetNameUniqueID("positionData");
			DrawVelocityParticlesColour_BillboardCpu.cid2 = state.GetNameUniqueID("velocityData");
			DrawVelocityParticlesColour_BillboardCpu.cid3 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticlesColour_BillboardCpu.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityParticlesColour_BillboardCpu.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticlesColour_BillboardCpu.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[240], ref this.vreg[241], ref this.vreg[242], ref this.vreg[243], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticlesColour_BillboardCpu.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticlesColour_BillboardCpu.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticlesColour_BillboardCpu.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticlesColour_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticlesColour_BillboardCpu.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticlesColour_BillboardCpu.fx, DrawVelocityParticlesColour_BillboardCpu.fxb, 30, 6);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticlesColour_BillboardCpu.vin[i]));
			index = DrawVelocityParticlesColour_BillboardCpu.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {84,35,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,127,246,155,202,239,191,54,254,166,255,175,244,239,95,65,255,255,117,244,179,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,251,63,191,30,253,255,247,191,108,126,255,41,253,252,141,126,13,137,179,255,52,141,187,205,243,235,163,205,170,249,253,155,29,137,193,127,51,109,231,63,191,22,253,31,175,117,63,151,152,253,255,254,191,1,227,245,60,155,229,181,180,197,231,104,251,107,235,207,212,123,231,103,8,208,159,228,225,240,111,233,239,120,231,95,210,223,1,227,79,209,239,255,50,250,236,47,162,255,255,169,250,247,223,71,191,255,93,94,59,243,252,223,244,152,28,194,127,196,95,254,223,255,247,255,245,127,255,174,191,198,201,155,227,39,191,19,253,249,165,126,134,54,248,155,158,244,219,244,207,206,175,97,114,18,191,214,175,113,160,239,43,61,254,218,95,135,168,246,107,242,127,242,208,199,123,191,255,206,175,241,69,49,173,171,166,58,111,211,173,87,119,210,111,63,127,253,60,149,209,167,39,213,98,85,148,244,203,195,241,222,167,227,135,247,247,198,123,7,251,251,191,198,239,34,168,254,65,191,230,175,241,155,254,77,250,251,159,244,107,252,6,191,233,95,244,132,187,254,77,233,243,255,236,111,250,53,126,131,255,236,47,162,126,126,173,95,243,215,248,237,232,239,255,251,111,250,117,245,187,95,227,215,248,207,240,247,31,132,239,232,29,254,251,255,254,191,21,161,95,67,198,109,126,255,183,126,115,250,231,215,250,191,104,220,191,226,215,54,227,254,11,127,83,249,204,31,247,159,245,155,202,184,129,200,175,208,113,127,251,215,48,188,242,215,254,181,191,38,117,253,107,210,204,33,55,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,12,61,63,250,124,243,115,249,53,243,8,63,241,107,252,26,191,238,175,248,53,126,211,191,136,64,252,30,148,78,248,61,12,60,147,95,160,124,193,159,244,235,112,202,229,215,254,143,126,141,95,227,191,254,147,40,126,255,139,240,255,223,146,191,255,53,233,251,95,227,79,250,181,56,175,192,191,255,193,248,123,44,223,253,77,120,159,114,14,72,59,252,65,47,211,255,12,125,252,77,230,167,188,243,107,209,59,95,225,189,63,232,55,114,127,255,193,248,204,251,254,15,254,21,191,198,87,127,17,253,253,235,252,154,218,254,101,250,21,195,16,188,126,77,202,99,124,197,255,7,156,95,223,225,245,7,253,122,252,251,175,197,191,255,154,12,239,55,160,119,255,111,239,221,95,135,190,251,191,255,160,95,78,240,127,57,141,73,62,51,56,253,53,127,16,250,197,123,148,173,161,247,126,13,126,239,215,229,191,127,109,228,87,24,174,228,84,126,109,126,7,159,185,191,241,253,95,243,39,73,254,229,215,65,255,127,176,210,17,109,233,243,255,154,219,252,58,156,82,211,246,138,219,175,161,52,146,190,127,211,255,232,47,114,127,131,12,140,215,111,32,52,255,135,240,222,255,244,107,252,114,253,251,215,226,191,255,103,251,247,175,195,127,255,47,246,239,223,128,255,254,101,244,55,242,61,255,79,0,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'colourData'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float4 colourData[80]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetColourData(Microsoft.Xna.Framework.Vector4[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector4 val;
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
						> 80)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 80)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 160)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 colourData[80]'</summary>
		public Microsoft.Xna.Framework.Vector4[] ColourData
		{
			set
			{
				this.SetColourData(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'positionData'</summary>
		private static int cid1;
		/// <summary>Set the shader array value 'float4 positionData[80]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetPositionData(Microsoft.Xna.Framework.Vector4[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector4 val;
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
						> 80)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 80)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 positionData[80]'</summary>
		public Microsoft.Xna.Framework.Vector4[] PositionData
		{
			set
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'velocityData'</summary>
		private static int cid2;
		/// <summary>Set the shader array value 'float4 velocityData[80]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetVelocityData(Microsoft.Xna.Framework.Vector4[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector4 val;
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
						> 80)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 80)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 80)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 velocityData[80]'</summary>
		public Microsoft.Xna.Framework.Vector4[] VelocityData
		{
			set
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid3;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[244] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 velocityScale'</summary>
		public Microsoft.Xna.Framework.Vector2 VelocityScale
		{
			set
			{
				this.SetVelocityScale(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D DisplaySampler'</summary>
		public Xen.Graphics.TextureSamplerState DisplaySampler
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
		/// <summary>Get/Set the Bound texture for 'Texture2D DisplayTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D DisplayTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D DisplaySampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D DisplayTexture'</summary>
		static int tid4;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[245];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticlesColour_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_BillboardCpu.cid3))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector4[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector4[] value)
		{
			if ((DrawVelocityParticlesColour_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_BillboardCpu.cid0))
			{
				this.SetColourData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawVelocityParticlesColour_BillboardCpu.cid1))
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawVelocityParticlesColour_BillboardCpu.cid2))
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticlesColour_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_BillboardCpu.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticlesColour_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_BillboardCpu.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
}
#endif
