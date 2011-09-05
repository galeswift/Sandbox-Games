// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = VelocityBillboard.fx
// Namespace = Xen.Ex.Graphics.Display

#if XBOX360
namespace Xen.Ex.Graphics.Display
{
	
	/// <summary><para>Technique 'DrawVelocityParticles_GpuTex' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 39 instruction slots used (4 texture, 35 arithmetic), 6 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
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
			state.CreateEffect(out DrawVelocityParticles_GpuTex.fx, DrawVelocityParticles_GpuTex.fxb, 44, 8);
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,104,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,176,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,212,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,248,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,28,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,5,131,0,0,1,1,131,0,0,1,7,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,188,131,0,0,1,208,139,0,0,1,224,131,0,0,1,244,138,0,0,1,1,1,4,1,0,1,0,1,1,1,24,138,0,0,1,1,1,92,135,0,0,1,1,1,0,1,0,1,1,1,88,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,44,1,0,1,0,1,1,1,40,131,0,0,1,93,134,0,0,1,1,1,68,1,0,1,0,1,1,1,64,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,2,1,20,1,14,131,0,3,252,252,27,4,225,2,1,2,5,12,135,128,0,0,4,21,108,108,225,151,0,0,132,255,0,138,0,0,1,3,1,152,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,144,1,0,1,0,1,2,1,8,135,0,0,1,36,1,0,1,0,1,1,1,44,1,0,1,0,1,1,1,84,138,0,0,1,1,1,4,131,0,0,1,28,131,0,0,1,246,1,255,1,254,1,3,132,0,0,1,3,131,0,0,1,28,135,0,0,1,239,131,0,0,1,88,1,0,1,2,131,0,0,1,6,133,0,0,1,96,131,0,0,1,112,131,0,0,1,208,1,0,1,3,131,0,1,1,133,0,0,1,216,135,0,0,1,232,1,0,1,3,1,0,2,1,0,1,1,133,0,1,216,132,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,200,1,0,1,17,1,0,1,4,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,36,1,0,1,0,1,16,1,35,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,64,1,11,1,18,1,0,1,18,1,0,1,0,1,80,132,0,0,1,96,1,15,1,194,1,0,1,18,133,0,0,1,96,1,21,1,96,1,27,1,18,1,0,1,18,133,0,0,1,32,1,33,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,35,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,1,0,1,1,1,0,1,196,1,179,1,0,1,224,1,1,1,1,1,0,1,188,1,64,1,1,132,0,0,1,65,2,194,0,3,0,255,33,4,8,32,33,15,5,31,254,200,0,0,6,64,0,33,24,64,33,7,15,31,254,200,0,0,64,8,0,200,8,0,1,0,198,177,9,108,139,4,5,5,200,3,0,1,10,0,176,198,0,167,4,255,0,200,2,131,0,9,96,0,0,243,1,0,0,48,32,133,0,2,177,226,131,0,2,184,32,133,0,6,65,194,0,0,255,200,7,8,0,4,0,177,177,0,8,224,0,4,0,200,1,0,1,9,0,24,24,198,209,4,4,255,88,10,38,0,1,0,22,177,108,161,0,255,11,129,160,17,1,3,0,27,177,108,225,4,12,0,129,200,1,0,1,0,27,108,0,225,1,13,1,0,20,7,0,1,0,192,176,177,160,1,255,6,0,12,34,0,3,0,131,108,14,225,3,1,4,20,4,0,3,1,177,108,177,225,0,15,1,0,12,135,3,3,0,192,26,198,225,3,0,0,200,1,3,131,0,9,24,178,0,224,3,3,0,200,3,131,0,16,176,198,176,235,0,2,2,200,1,128,62,0,109,109,27,145,131,0,16,200,2,128,62,0,109,109,27,145,0,1,1,200,4,128,62,17,0,109,109,27,145,0,2,2,200,8,128,62,0,109,109,27,145,7,0,3,3,36,254,192,1,131,0,16,108,226,0,0,128,200,3,128,0,0,197,197,0,226,1,1,140,0,1,0};
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
	/// <summary><para>Technique 'DrawVelocityParticlesColour_GpuTex' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 41 instruction slots used (6 texture, 35 arithmetic), 6 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
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
			state.CreateEffect(out DrawVelocityParticlesColour_GpuTex.fx, DrawVelocityParticlesColour_GpuTex.fxb, 46, 8);
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,140,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,176,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,212,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,248,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,28,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,64,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,188,131,0,0,1,208,139,0,0,1,224,131,0,0,1,244,138,0,0,1,1,1,4,1,0,1,0,1,1,1,24,138,0,0,1,1,1,40,1,0,1,0,1,1,1,60,138,0,0,1,1,1,128,135,0,0,1,1,1,0,1,0,1,1,1,124,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,80,1,0,1,0,1,1,1,76,131,0,0,1,93,134,0,0,1,1,1,104,1,0,1,0,1,1,1,100,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,20,1,14,131,0,2,252,252,3,27,225,2,4,1,2,12,135,5,128,0,0,21,108,2,108,225,151,0,0,132,255,0,138,0,0,1,3,1,192,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,172,1,0,1,0,1,2,1,20,135,0,0,1,36,1,0,1,0,1,1,1,72,1,0,1,0,1,1,1,112,138,0,0,1,1,1,32,131,0,0,1,28,1,0,1,0,1,1,1,17,1,255,1,254,1,3,132,0,0,1,4,131,0,0,1,28,134,0,0,1,1,1,10,131,0,0,1,108,2,0,2,131,0,1,6,133,0,0,1,116,131,0,1,132,131,0,1,228,2,0,3,131,0,1,1,133,0,0,1,236,135,0,0,1,252,1,0,1,3,1,0,2,1,0,1,1,133,0,1,236,134,0,0,1,1,1,3,1,0,2,3,0,3,2,0,1,133,0,1,236,132,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,212,1,0,1,17,1,0,1,5,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,36,1,0,1,0,1,16,1,37,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,80,1,11,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,16,1,194,1,0,1,18,133,0,0,1,96,1,22,1,96,1,28,1,18,1,0,1,18,133,0,0,1,32,1,34,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,36,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,1,0,1,1,1,0,1,196,1,179,1,0,1,224,1,1,2,1,0,3,188,64,1,132,0,2,65,194,3,0,0,255,4,33,24,32,33,5,15,31,246,136,0,6,0,64,0,33,8,48,7,33,15,31,254,200,0,0,8,64,0,33,40,80,33,15,31,9,254,200,0,0,64,0,200,8,0,6,1,0,198,177,108,139,131,5,10,200,3,0,1,0,176,198,0,167,5,4,255,0,200,2,131,0,9,96,0,0,243,1,0,0,48,32,133,0,2,177,226,131,0,2,184,32,133,0,7,65,194,0,0,255,200,8,8,0,5,0,177,177,0,224,0,9,5,0,200,1,0,1,0,24,24,10,198,209,5,5,255,88,38,0,1,0,11,22,177,108,161,0,255,129,160,17,1,4,12,0,27,177,108,225,5,0,129,200,1,0,1,13,0,27,108,0,225,1,1,0,20,7,0,1,0,12,192,176,177,160,1,255,0,12,34,0,4,0,131,108,14,225,4,1,5,20,4,0,4,1,177,108,177,225,0,15,1,0,12,135,4,4,0,192,26,198,225,4,0,0,200,1,3,131,0,9,24,178,0,224,4,4,0,200,3,131,0,16,176,198,176,235,0,3,3,200,1,128,62,0,109,109,27,145,131,0,16,200,2,128,62,0,109,109,27,145,0,1,1,200,4,128,62,17,0,109,109,27,145,0,2,2,200,8,128,62,0,109,109,27,145,18,0,3,3,200,3,128,0,0,197,197,0,226,1,1,0,200,15,128,1,1,132,0,3,226,2,2,140,0,1,0};
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
	/// <summary><para>Technique 'DrawVelocityParticles_GpuTex_UserOffset' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 42 instruction slots used (6 texture, 36 arithmetic), 6 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
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
			state.CreateEffect(out DrawVelocityParticles_GpuTex_UserOffset.fx, DrawVelocityParticles_GpuTex_UserOffset.fxb, 47, 8);
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,140,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,176,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,212,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,248,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,28,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,64,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,188,131,0,0,1,208,139,0,0,1,224,131,0,0,1,244,138,0,0,1,1,1,4,1,0,1,0,1,1,1,24,138,0,0,1,1,1,40,1,0,1,0,1,1,1,60,138,0,0,1,1,1,128,135,0,0,1,1,1,0,1,0,1,1,1,124,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,80,1,0,1,0,1,1,1,76,131,0,0,1,93,134,0,0,1,1,1,104,1,0,1,0,1,1,1,100,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,20,1,14,131,0,2,252,252,3,27,225,2,4,1,2,12,135,5,128,0,0,21,108,2,108,225,151,0,0,132,255,0,138,0,0,1,3,1,204,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,172,1,0,1,0,1,2,1,32,135,0,0,1,36,1,0,1,0,1,1,1,72,1,0,1,0,1,1,1,112,138,0,0,1,1,1,32,131,0,0,1,28,1,0,1,0,1,1,1,17,1,255,1,254,1,3,132,0,0,1,4,131,0,0,1,28,134,0,0,1,1,1,10,131,0,0,1,108,2,0,2,131,0,1,6,133,0,0,1,116,131,0,1,132,131,0,1,228,2,0,3,131,0,1,1,133,0,0,1,236,135,0,0,1,252,1,0,1,3,1,0,2,1,0,1,1,133,0,1,236,134,0,0,1,1,1,3,1,0,2,3,0,3,2,0,1,133,0,1,236,132,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,224,1,0,1,17,1,0,1,4,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,38,1,0,1,0,1,16,1,37,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,80,1,11,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,16,1,194,1,0,1,18,133,0,0,1,96,1,22,1,96,1,28,1,18,1,0,1,18,133,0,0,1,48,1,34,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,37,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,1,0,1,1,1,0,1,196,1,179,1,0,1,224,1,1,2,1,0,3,188,64,1,132,0,2,65,194,3,0,0,255,4,33,8,32,33,5,15,31,254,200,0,6,0,64,0,33,40,48,7,33,15,31,254,143,0,0,8,64,0,33,24,64,33,15,31,9,254,200,0,0,64,0,200,8,0,10,1,0,198,177,108,139,4,5,5,200,11,3,0,1,0,176,198,0,167,4,255,0,2,200,2,131,0,9,96,0,0,243,1,0,0,48,32,133,0,2,177,226,131,0,2,184,32,133,0,8,65,194,0,0,255,200,8,0,9,4,0,177,177,0,224,0,4,0,10,200,1,0,1,0,24,24,198,209,4,11,4,255,88,38,0,1,0,22,177,108,161,12,0,255,129,160,17,1,3,0,27,177,108,225,5,4,0,129,20,2,131,0,13,177,108,27,225,0,4,1,12,24,1,3,4,177,14,198,108,225,0,0,1,200,7,0,1,0,192,176,0,15,160,1,255,0,20,3,0,2,0,176,197,108,224,2,3,16,3,12,36,3,3,0,177,108,108,225,0,1,1,200,7,0,11,3,0,192,26,0,225,3,0,0,200,3,131,0,9,24,178,0,224,3,3,0,200,3,131,0,16,176,198,176,235,0,2,2,200,1,128,62,0,109,109,27,145,131,0,17,200,2,128,62,0,109,109,27,145,0,1,1,200,4,128,62,0,18,109,109,27,145,0,2,2,200,8,128,62,0,109,109,27,145,0,3,5,3,36,254,192,1,131,0,16,108,226,0,0,128,200,3,128,0,0,197,197,0,226,1,1,140,0,1,0};
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
	/// <summary><para>Technique 'DrawVelocityParticlesColour_GpuTex_UserOffset' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 44 instruction slots used (8 texture, 36 arithmetic), 6 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
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
			state.CreateEffect(out DrawVelocityParticlesColour_GpuTex_UserOffset.fx, DrawVelocityParticlesColour_GpuTex_UserOffset.fxb, 49, 8);
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,128,135,0,1,3,131,0,1,1,131,0,1,128,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,164,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,200,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,131,0,0,1,236,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,16,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,51,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,52,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,140,131,0,0,1,160,139,0,0,1,176,131,0,0,1,196,139,0,0,1,212,131,0,0,1,232,139,0,0,1,248,1,0,1,0,1,1,1,12,138,0,0,1,1,1,28,1,0,1,0,1,1,1,48,138,0,0,1,1,1,116,135,0,0,1,1,1,0,1,0,1,1,1,112,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,68,1,0,1,0,1,1,1,64,131,0,0,1,93,134,0,0,1,1,1,92,1,0,1,0,1,1,1,88,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,2,0,0,3,64,0,22,1,16,133,0,2,27,226,3,0,0,1,2,20,14,131,0,4,252,252,27,225,5,2,1,2,12,135,6,128,0,0,21,108,108,1,225,151,0,0,132,255,0,138,0,0,1,3,1,240,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,196,1,0,1,0,1,2,1,44,135,0,0,1,36,1,0,1,0,1,1,1,96,1,0,1,0,1,1,1,136,138,0,0,1,1,1,56,131,0,0,1,28,1,0,1,0,1,1,1,44,1,255,1,254,1,3,132,0,0,1,5,131,0,1,28,134,0,0,1,1,1,37,131,0,0,1,128,2,0,2,131,0,1,6,133,0,0,1,136,131,0,1,152,131,0,1,248,2,0,3,131,0,1,1,132,0,1,1,135,0,0,1,1,1,16,1,0,1,3,2,0,1,2,0,1,132,0,1,1,135,0,0,1,1,1,23,1,0,2,3,0,3,2,0,1,132,0,1,1,135,0,0,1,1,1,30,1,0,2,3,0,3,3,0,1,132,0,1,1,133,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,95,1,118,1,115,1,95,1,115,1,51,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,136,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,236,1,0,1,17,1,0,1,5,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,38,1,0,1,0,1,16,1,39,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,96,1,11,1,18,1,0,1,18,1,0,1,5,1,80,132,0,0,1,96,1,17,1,194,1,0,1,18,133,0,0,1,96,1,23,1,96,1,29,1,18,1,0,1,18,133,0,0,1,48,1,35,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,38,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,1,0,1,1,1,0,2,196,179,3,0,224,1,4,1,0,188,64,1,1,132,0,4,65,194,0,0,5,255,33,24,32,33,6,15,31,246,136,0,0,7,64,0,33,8,48,33,15,8,31,254,200,0,0,64,0,33,9,56,64,33,15,31,254,143,0,0,10,64,0,33,40,80,33,15,31,254,200,11,0,0,64,0,200,8,0,1,0,198,177,2,108,139,131,5,12,200,3,0,1,0,176,198,0,167,5,255,0,2,200,2,131,0,9,96,0,0,243,1,0,0,48,32,133,0,2,177,226,131,0,2,184,32,133,0,9,65,194,0,0,255,200,8,0,5,10,0,177,177,0,224,0,5,0,200,1,11,0,1,0,24,24,198,209,5,5,255,88,12,38,0,1,0,22,177,108,161,0,255,129,160,13,17,1,4,0,27,177,108,225,5,0,129,20,2,131,0,13,177,108,27,225,0,5,1,12,24,1,4,4,177,14,198,108,225,0,0,1,200,7,0,1,0,192,176,0,15,160,1,255,0,20,3,0,3,0,176,197,108,224,3,4,16,4,12,36,4,4,0,177,108,108,225,0,1,1,200,7,0,11,4,0,192,26,0,225,4,0,0,200,3,131,0,9,24,178,0,224,4,4,0,200,3,131,0,16,176,198,176,235,0,3,3,200,1,128,62,0,109,109,27,145,131,0,17,200,2,128,62,0,109,109,27,145,0,1,1,200,4,128,62,0,18,109,109,27,145,0,2,2,200,8,128,62,0,109,109,27,145,0,3,17,3,200,3,128,0,0,197,197,0,226,1,1,0,200,15,128,1,132,0,3,226,2,2,140,0,1,0};
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
namespace Xen.Ex.Graphics.Display
{
	
	/// <summary><para>Technique 'DrawVelocityParticles_GpuTex' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 39 instruction slots used (4 texture, 35 arithmetic), 6 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
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
			state.CreateEffect(out DrawVelocityParticles_GpuTex.fx, DrawVelocityParticles_GpuTex.fxb, 44, 8);
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
				return new byte[] {64,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,87,249,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,141,207,127,109,188,79,255,255,155,180,221,175,167,239,43,184,159,213,7,125,253,254,151,205,239,63,165,159,191,209,175,33,253,254,107,157,54,191,190,182,105,118,92,155,255,109,168,205,174,107,243,59,117,6,192,109,86,2,7,95,253,102,218,206,127,126,45,250,255,111,26,249,92,64,253,83,191,14,96,188,158,103,51,80,238,215,213,207,241,25,104,136,119,126,59,239,157,125,250,255,75,239,239,191,143,254,255,47,121,127,255,71,244,255,95,238,253,253,235,16,176,223,206,195,249,15,208,223,241,227,247,214,223,129,223,159,162,223,143,232,179,45,250,255,159,170,127,63,165,223,127,79,175,157,121,254,111,122,44,88,252,242,107,255,223,255,247,255,245,127,255,174,191,198,201,155,227,39,224,145,47,127,13,249,12,95,41,207,164,223,166,127,118,126,13,195,27,191,214,175,113,160,175,43,253,254,218,95,135,168,252,107,242,127,242,208,199,247,126,255,157,95,227,139,98,90,87,77,117,222,166,91,175,238,164,223,126,254,250,121,42,212,74,79,170,197,170,32,134,75,31,142,247,62,29,63,188,191,55,222,59,216,223,255,53,126,23,66,149,232,248,7,81,79,127,146,252,254,107,254,65,191,230,175,241,155,242,239,4,246,79,250,53,126,131,223,244,47,122,194,104,252,166,212,230,63,163,191,255,179,191,232,215,229,191,127,125,252,77,109,255,179,63,137,112,248,181,126,205,95,227,55,167,223,255,239,63,137,191,251,13,204,119,191,198,31,36,127,255,6,244,247,255,205,127,255,223,255,183,34,252,107,8,93,204,239,255,29,6,250,107,255,95,68,151,47,45,93,126,253,95,83,62,195,87,134,46,24,240,239,253,107,8,114,191,30,253,251,7,208,207,213,175,33,243,106,104,245,111,41,204,255,138,63,3,141,126,189,95,227,127,210,207,132,215,255,218,191,246,215,228,111,126,29,230,255,31,198,163,242,211,155,55,149,153,222,231,151,95,115,62,127,130,196,226,215,251,53,126,211,191,136,64,252,30,68,238,127,80,160,253,65,191,135,206,231,31,244,107,116,230,214,252,254,107,186,223,209,230,63,50,124,65,116,210,223,127,205,63,232,215,162,207,127,29,22,147,95,251,63,250,53,126,141,255,250,79,34,218,253,69,248,255,175,197,164,255,53,255,160,95,231,215,248,107,208,175,240,0,253,13,88,212,254,47,250,45,25,238,175,133,191,255,216,223,152,191,251,117,248,59,250,255,31,252,91,48,60,180,253,107,232,255,95,225,255,127,176,215,254,15,50,176,165,253,87,127,48,96,255,154,250,221,175,243,107,124,245,23,253,154,220,246,215,252,131,128,199,175,195,42,224,215,166,207,255,51,224,1,222,252,131,228,179,223,136,97,255,122,191,198,159,70,248,254,109,127,209,239,207,48,192,207,255,219,31,244,107,18,63,255,254,220,135,252,109,248,251,215,34,56,191,166,240,183,226,240,235,208,207,175,240,251,31,244,27,217,49,252,53,127,48,240,54,223,255,154,140,255,95,243,7,255,90,22,199,191,134,250,252,191,21,47,249,254,215,84,24,191,62,227,253,235,104,27,200,143,252,46,244,253,13,32,75,36,59,95,1,151,0,31,211,70,224,253,53,220,14,125,8,93,126,109,130,45,109,204,223,250,14,203,232,175,197,99,248,53,254,96,157,67,208,228,79,250,181,126,141,255,218,163,147,105,255,127,255,65,42,191,140,35,224,255,6,50,15,255,209,175,193,223,255,114,253,251,215,250,143,164,157,249,251,215,225,191,127,45,251,247,111,192,127,255,218,244,183,244,255,155,254,71,128,5,29,240,255,4,0,0,255,255};
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
	/// <summary><para>Technique 'DrawVelocityParticlesColour_GpuTex' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 41 instruction slots used (6 texture, 35 arithmetic), 6 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
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
			state.CreateEffect(out DrawVelocityParticlesColour_GpuTex.fx, DrawVelocityParticlesColour_GpuTex.fxb, 46, 8);
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
				return new byte[] {176,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,159,244,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,141,207,127,109,188,79,255,255,155,180,221,175,167,239,43,184,159,213,7,125,253,254,151,205,239,63,165,159,191,209,175,33,253,254,107,157,54,191,190,182,105,118,92,155,255,109,168,205,174,107,243,59,117,6,96,219,236,185,54,191,103,172,205,138,251,250,131,240,213,111,166,237,252,231,215,162,255,255,166,145,207,13,40,192,120,61,207,102,68,221,63,232,215,211,207,127,131,95,67,232,140,119,126,59,239,157,125,250,255,75,239,239,191,143,254,255,47,121,127,255,71,244,255,95,238,253,253,235,16,176,223,206,195,121,139,126,127,236,253,253,135,233,239,248,241,51,250,59,240,253,83,244,251,151,244,217,115,250,255,159,170,127,207,233,247,153,215,206,60,255,55,61,22,44,126,249,181,255,239,255,251,255,250,191,127,215,95,227,228,205,241,19,240,213,151,191,134,124,134,175,148,207,210,111,211,63,59,191,134,225,167,95,235,215,56,208,215,149,158,127,237,175,67,84,255,53,249,63,121,232,227,123,191,255,206,175,241,69,49,173,171,166,58,111,211,173,87,119,210,111,63,127,253,60,21,234,165,39,213,98,85,16,147,166,15,199,123,159,142,31,222,223,27,239,29,236,239,255,26,191,11,161,250,235,254,26,191,198,31,68,61,253,73,242,251,175,73,115,245,155,242,239,4,246,79,250,53,126,131,223,244,47,122,194,104,252,166,212,230,63,163,191,255,179,191,232,215,229,191,127,125,252,77,109,255,51,136,202,175,245,107,254,26,191,57,253,254,127,255,73,252,221,111,96,190,251,53,254,32,249,251,55,160,191,255,111,254,251,255,254,191,21,225,95,67,232,98,126,223,194,100,254,218,255,23,209,229,167,44,93,238,253,154,242,153,39,127,233,136,62,43,127,13,65,238,215,163,127,91,250,249,135,209,255,255,179,95,195,209,234,191,83,152,255,7,127,6,26,253,122,60,215,120,126,171,95,19,159,253,90,244,89,98,121,90,100,230,175,253,107,127,77,110,253,235,176,28,253,48,30,149,195,222,92,170,236,197,63,223,235,127,126,249,53,231,254,39,126,141,95,227,215,253,245,126,141,223,244,47,34,16,191,7,77,205,63,40,208,254,160,223,67,231,254,15,250,53,58,124,96,126,255,53,189,223,127,45,247,59,218,255,71,134,159,136,150,250,251,175,249,7,253,90,244,249,175,197,211,243,107,254,65,191,206,175,241,215,160,63,225,19,250,27,239,209,252,254,69,191,37,195,248,181,240,247,31,251,27,243,119,191,14,127,71,255,255,131,127,11,22,85,180,253,107,232,255,95,225,255,127,176,215,254,15,50,176,165,253,87,127,48,96,255,154,250,221,175,243,107,124,245,23,253,154,220,246,215,132,254,248,139,126,29,86,27,191,54,125,254,159,1,15,240,239,31,36,159,253,70,12,251,215,251,53,254,180,191,232,215,251,53,254,182,191,232,247,167,70,192,155,180,227,31,244,107,18,207,255,58,140,195,175,77,127,255,215,127,18,224,224,255,191,63,247,3,185,248,223,254,160,95,139,218,252,254,140,135,252,109,228,228,215,162,190,126,77,145,19,197,243,215,161,159,95,225,247,63,232,55,178,227,252,107,254,96,140,205,124,255,107,242,24,255,154,63,248,215,98,248,24,199,95,67,120,253,223,138,187,124,255,107,42,140,95,159,199,246,235,104,27,200,161,252,46,248,254,6,144,73,146,193,175,128,75,128,143,105,35,240,254,26,110,135,62,132,118,191,54,193,150,54,230,111,125,231,79,2,45,127,45,30,195,175,241,7,43,77,64,183,63,233,215,250,53,254,107,143,150,166,253,255,253,7,169,30,96,28,1,255,55,144,185,250,143,240,57,105,99,253,251,215,250,143,164,157,249,251,215,225,191,127,45,251,247,111,192,127,255,218,244,55,244,199,255,19,0,0,255,255};
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
	/// <summary><para>Technique 'DrawVelocityParticles_GpuTex_UserOffset' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 42 instruction slots used (6 texture, 36 arithmetic), 6 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
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
			state.CreateEffect(out DrawVelocityParticles_GpuTex_UserOffset.fx, DrawVelocityParticles_GpuTex_UserOffset.fxb, 47, 8);
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
				return new byte[] {204,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,159,244,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,141,207,127,109,188,79,255,255,155,180,221,175,167,239,43,184,159,213,7,125,253,254,151,205,239,63,165,159,191,209,175,33,253,254,107,157,54,191,190,182,105,118,92,155,255,109,168,205,174,107,243,59,117,6,96,219,236,185,54,191,103,172,205,138,251,250,131,240,213,111,166,237,252,231,215,162,255,255,166,145,207,13,40,192,120,61,207,102,68,221,63,232,215,211,207,127,131,95,67,232,140,119,126,59,239,157,125,250,255,75,239,239,191,143,254,255,47,121,127,255,71,244,255,95,238,253,253,235,16,176,223,206,195,121,139,126,127,236,253,253,135,233,239,248,241,51,250,59,240,253,83,244,251,151,244,217,115,250,255,159,170,127,207,233,247,153,215,206,60,255,55,61,22,44,126,249,181,255,239,255,251,255,250,191,127,215,95,227,228,205,241,19,240,213,151,191,134,124,134,175,148,207,210,111,211,63,59,191,134,225,167,95,235,215,56,208,215,149,158,127,237,175,67,84,255,53,249,63,121,232,227,123,191,255,206,175,241,69,49,173,171,166,58,111,211,173,87,119,210,111,63,127,253,60,21,234,165,39,213,98,85,16,147,166,15,199,123,159,142,31,222,223,27,239,29,236,239,255,26,191,11,161,250,235,254,26,191,198,31,68,61,253,73,242,251,175,73,115,245,155,242,239,4,246,79,250,53,126,131,223,244,47,122,194,104,252,166,212,230,63,163,191,255,179,191,232,215,229,191,127,125,252,77,109,255,51,136,202,175,245,107,254,26,191,57,253,254,127,255,73,252,221,111,96,190,251,53,254,32,249,251,55,160,191,255,111,254,251,255,254,191,21,225,95,67,232,98,126,127,138,201,252,181,255,47,162,203,79,89,186,220,251,53,229,51,79,254,210,17,125,86,254,26,130,220,175,71,255,182,244,243,15,163,255,255,103,191,134,163,213,127,167,48,255,15,254,12,52,250,245,120,174,241,252,86,191,38,62,251,181,232,179,196,242,180,200,204,95,251,215,254,154,220,250,215,97,57,250,97,60,42,135,189,185,84,217,139,127,190,215,255,252,242,107,206,253,79,252,26,191,198,175,251,235,253,26,191,233,95,68,32,126,15,154,154,127,80,160,253,65,191,135,206,253,31,244,107,116,248,192,252,254,107,122,191,255,90,238,119,180,255,143,12,63,17,45,245,247,95,243,15,250,181,232,243,95,135,197,237,215,254,143,126,141,95,227,191,254,147,136,190,127,17,254,255,107,241,148,253,154,127,208,175,243,107,252,53,192,65,120,135,254,6,44,106,255,23,253,150,12,247,215,194,223,127,236,111,204,223,253,58,252,29,253,255,15,254,45,24,30,218,254,53,244,255,175,240,255,63,216,107,255,7,25,216,210,254,171,63,24,176,127,77,253,238,215,249,53,190,250,139,126,77,110,251,107,66,167,252,69,191,14,171,146,95,155,62,255,207,128,7,120,250,15,146,207,126,35,134,253,235,253,26,127,26,225,251,183,253,69,191,63,195,128,28,252,111,127,208,175,169,114,240,107,209,123,191,166,200,129,246,249,107,209,207,175,240,251,31,244,27,233,223,232,255,215,144,207,248,239,95,83,241,253,181,44,78,232,227,255,86,60,228,251,95,83,97,252,250,118,76,104,3,57,251,117,248,119,161,231,111,0,153,35,25,251,10,184,4,248,152,54,2,239,175,225,118,232,67,232,240,107,19,108,105,99,254,150,119,254,154,63,9,116,249,181,168,15,234,244,15,214,57,67,187,63,233,215,250,53,254,107,110,243,251,115,31,66,131,95,139,104,240,251,243,152,228,111,232,6,25,35,224,255,215,127,16,116,128,161,173,192,255,191,255,32,213,11,191,22,230,18,248,252,6,50,79,255,209,175,193,223,255,114,253,251,215,250,143,164,157,249,251,215,225,191,127,45,251,247,111,192,127,255,218,244,183,224,251,155,254,71,128,5,221,242,255,4,0,0,255,255};
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
	/// <summary><para>Technique 'DrawVelocityParticlesColour_GpuTex_UserOffset' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 44 instruction slots used (8 texture, 36 arithmetic), 6 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
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
			state.CreateEffect(out DrawVelocityParticlesColour_GpuTex_UserOffset.fx, DrawVelocityParticlesColour_GpuTex_UserOffset.fxb, 49, 8);
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
				return new byte[] {252,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,31,246,107,202,239,191,54,254,166,255,255,65,250,221,175,71,255,255,117,244,179,159,237,7,125,253,254,151,205,239,63,165,159,191,209,175,33,253,254,101,157,54,191,190,182,105,118,92,155,127,106,168,205,174,107,243,223,13,181,217,115,109,126,179,206,32,109,155,123,174,205,126,172,205,74,240,193,87,191,153,182,243,159,95,139,254,255,155,70,62,23,80,255,247,255,13,24,175,231,217,44,175,133,6,248,252,55,248,53,100,46,240,78,234,189,243,199,209,255,255,34,239,239,191,137,254,255,143,121,127,255,107,244,255,255,202,251,251,127,163,255,255,70,30,206,191,19,253,190,227,253,253,78,127,199,143,149,254,14,124,255,20,253,254,41,125,246,123,210,255,255,84,253,251,247,165,223,127,111,175,157,121,254,111,122,44,88,252,242,107,255,223,255,247,255,245,127,255,174,191,198,201,155,227,39,191,19,253,249,229,175,33,159,225,171,223,137,27,253,26,233,183,233,159,157,95,195,240,220,175,245,107,28,232,235,74,207,191,246,215,33,170,255,154,252,159,60,244,241,189,223,127,231,215,248,162,152,214,85,83,157,183,233,214,171,59,233,183,159,191,126,158,10,245,210,147,106,177,42,74,250,229,225,120,239,211,241,195,251,123,227,189,131,253,253,95,227,119,33,84,127,93,102,233,95,251,79,146,223,127,205,63,232,215,252,53,126,83,254,157,192,254,73,191,198,111,240,155,254,69,79,24,141,223,148,218,252,103,244,247,127,246,23,253,186,252,247,175,143,191,169,237,127,246,39,17,14,191,214,175,249,107,252,230,244,251,255,253,39,241,119,191,129,249,238,215,248,131,228,239,223,128,254,254,191,249,239,255,251,255,86,132,127,13,161,139,249,253,15,194,100,254,218,255,23,209,37,183,116,249,253,127,77,249,140,96,88,186,128,190,4,140,145,251,245,232,223,63,138,126,254,89,191,134,204,165,161,149,33,10,120,246,215,102,26,253,122,191,198,111,167,159,109,241,103,191,22,125,150,216,185,254,61,249,51,180,252,77,126,141,111,235,103,34,107,127,237,95,251,107,242,231,191,14,243,222,15,227,81,249,237,205,175,202,108,252,243,189,129,207,239,245,63,191,252,154,124,242,19,191,198,175,241,235,254,122,191,198,111,250,23,17,136,223,131,102,224,31,20,104,127,208,239,161,124,242,7,253,26,29,158,49,191,255,154,222,239,191,150,247,251,175,237,126,199,187,255,145,225,67,162,183,254,254,107,254,65,191,22,125,254,107,241,180,254,154,196,30,127,13,250,22,254,162,191,127,13,97,153,191,232,183,100,24,191,22,254,254,99,127,99,254,238,215,225,239,232,255,127,240,111,65,127,252,154,220,246,175,161,255,127,133,255,255,193,94,251,63,200,192,150,246,95,253,193,128,253,107,234,119,191,206,175,241,213,95,244,107,114,219,95,243,15,162,185,255,139,126,29,86,55,191,54,125,254,159,1,15,194,243,63,251,131,228,179,223,136,97,255,122,191,198,159,246,23,253,122,191,198,223,246,23,253,254,212,8,120,19,71,254,65,191,38,201,202,175,195,56,252,218,244,247,127,253,39,1,14,254,255,251,115,63,144,167,255,237,15,250,181,169,141,190,195,127,255,26,250,247,175,161,127,255,90,244,247,175,197,237,127,109,234,243,191,38,156,255,179,63,72,240,68,191,79,249,255,50,142,95,135,190,135,140,253,53,127,208,111,100,233,240,215,252,193,24,187,249,94,104,240,215,252,193,242,55,198,249,215,16,222,255,183,142,205,124,255,21,195,248,245,121,236,191,142,182,129,124,203,239,210,246,55,248,131,32,207,191,46,209,136,112,81,125,240,107,255,65,144,121,191,205,175,201,116,255,191,185,143,16,231,255,219,142,225,215,148,119,254,164,95,211,246,247,107,252,193,134,214,132,203,159,68,116,163,54,255,131,194,148,62,126,45,30,39,235,23,126,7,240,127,3,153,203,255,72,112,248,229,250,247,175,245,31,137,30,50,127,255,58,255,145,188,111,254,254,13,248,239,95,155,254,134,94,250,127,2,0,0,255,255};
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
	/// <summary><para>Technique 'DrawVelocityParticles_BillboardCpu' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 28 instruction slots used, 165 registers</para><para>Pixel Shader: approximately 6 instruction slots used (1 texture, 5 arithmetic), 0 registers</para></summary>
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
			state.CreateEffect(out DrawVelocityParticles_BillboardCpu.fx, DrawVelocityParticles_BillboardCpu.fxb, 30, 9);
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
				return new byte[] {124,25,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,127,150,200,239,191,54,254,166,255,175,244,239,191,156,254,255,235,232,103,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,124,200,243,235,209,255,127,255,203,230,247,159,210,207,223,232,215,16,63,243,79,83,191,211,60,191,62,218,172,154,223,191,217,17,31,244,55,211,118,254,243,107,209,255,127,211,200,231,226,179,254,223,255,55,96,188,158,103,179,188,150,182,248,28,109,127,109,253,153,122,239,252,12,245,255,39,121,56,252,91,250,59,222,249,151,244,119,192,248,83,244,251,191,140,62,251,139,232,255,127,170,254,253,247,209,239,127,151,215,206,60,255,55,61,198,135,254,141,240,203,175,245,127,255,223,255,215,255,253,187,254,26,39,111,142,159,252,78,244,231,151,191,134,124,134,175,126,39,110,245,107,164,223,166,127,118,126,13,227,147,255,90,191,198,129,190,175,244,248,107,127,29,162,218,175,201,255,201,67,31,239,253,254,59,191,198,23,197,180,174,154,234,188,77,183,94,221,73,191,253,252,245,243,84,70,159,158,84,139,85,81,210,47,15,199,123,159,142,31,222,223,27,239,29,236,239,255,26,191,139,160,250,7,81,79,127,147,249,253,215,252,53,126,83,243,251,159,244,107,252,6,191,233,95,244,132,209,248,77,169,205,127,246,55,253,26,191,193,127,246,23,253,186,252,247,175,143,191,169,237,127,246,55,17,14,191,214,175,249,107,252,118,244,251,255,253,55,225,187,95,203,126,247,127,255,65,242,247,111,64,127,255,223,252,55,218,18,204,63,232,215,162,239,255,239,255,91,145,255,53,132,70,230,247,127,13,12,241,107,253,95,68,163,191,253,215,50,52,250,11,19,249,204,167,209,159,149,8,141,128,232,95,174,52,250,246,175,97,248,234,175,253,107,127,77,66,243,215,164,89,70,28,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,135,62,151,95,211,231,254,137,95,227,215,248,117,255,242,95,227,55,253,139,8,196,239,65,238,246,239,97,224,57,95,252,55,253,147,126,29,14,79,126,237,255,232,215,248,53,254,235,63,137,252,215,191,8,255,255,45,249,251,95,147,190,255,53,254,164,95,139,125,112,254,253,15,198,223,99,249,238,111,194,251,228,115,195,237,254,131,94,166,255,25,250,248,155,204,79,121,231,215,162,119,190,194,123,127,208,111,228,254,254,131,241,153,247,253,31,252,151,255,26,95,253,69,244,247,175,243,107,106,251,151,233,87,12,67,240,250,53,201,143,255,138,255,15,56,191,190,195,235,15,250,245,248,247,95,139,127,255,53,25,222,111,64,239,254,223,222,187,191,206,31,132,88,224,47,35,248,127,25,141,73,62,51,56,253,53,127,16,250,197,123,20,217,208,123,191,6,191,247,235,242,223,191,54,98,13,134,43,241,199,175,205,239,224,51,247,55,190,255,107,254,36,137,71,126,29,244,255,7,43,29,209,150,62,255,175,185,205,175,195,225,167,182,87,220,126,13,165,17,199,39,244,46,240,248,13,132,198,255,16,218,253,69,191,198,47,215,191,127,45,254,251,47,182,127,255,58,252,247,95,98,255,254,13,248,239,191,148,254,22,60,126,211,255,8,176,16,235,252,63,1,0,0,255,255};
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
	/// <summary><para>Technique 'DrawVelocityParticlesColour_BillboardCpu' generated from file 'VelocityBillboard.fx'</para><para>Vertex Shader: approximately 28 instruction slots used, 245 registers</para><para>Pixel Shader: approximately 6 instruction slots used (1 texture, 5 arithmetic), 0 registers</para></summary>
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
			state.CreateEffect(out DrawVelocityParticlesColour_BillboardCpu.fx, DrawVelocityParticlesColour_BillboardCpu.fxb, 30, 9);
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
				return new byte[] {128,35,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,127,246,155,202,239,191,54,254,166,255,175,244,239,95,65,255,255,117,244,179,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,251,63,191,30,253,255,247,191,108,126,255,41,253,252,141,126,13,137,179,255,52,141,187,205,243,235,163,205,170,249,253,155,29,137,193,127,51,109,231,63,191,22,253,31,175,117,63,151,152,253,255,254,191,1,227,245,60,155,229,181,180,197,231,104,251,107,235,207,212,123,231,103,8,208,159,228,225,240,111,233,239,120,231,95,210,223,1,227,79,209,239,255,50,250,236,47,162,255,255,169,250,247,223,71,191,255,93,94,59,243,252,223,244,152,28,194,111,132,95,126,173,255,251,255,254,191,254,239,223,245,215,56,121,115,252,228,119,162,63,191,252,53,228,51,124,245,59,113,171,95,35,253,54,253,179,243,107,152,156,196,175,245,107,28,232,251,74,143,191,246,215,33,170,253,154,252,159,60,244,241,222,239,191,243,107,124,81,76,235,170,169,206,219,116,235,213,157,244,219,207,95,63,79,101,244,233,73,181,88,21,37,253,242,112,188,247,233,248,225,253,189,241,222,193,254,254,175,241,187,8,170,127,16,245,244,55,153,223,127,205,95,227,55,53,191,255,73,191,198,111,240,155,254,69,79,24,141,223,148,218,252,103,127,211,175,241,27,252,103,127,209,175,203,127,255,250,248,155,218,254,103,127,19,225,240,107,253,154,191,198,111,71,191,255,223,127,19,190,251,181,236,119,255,247,31,36,127,255,6,244,247,255,205,127,163,45,193,252,131,126,45,250,254,255,254,191,21,249,95,67,104,100,126,255,183,126,115,250,231,215,250,191,136,70,191,226,215,54,52,250,11,127,83,249,204,167,209,159,245,155,10,141,128,232,175,80,26,125,251,215,48,124,245,215,254,181,191,38,161,249,107,210,44,35,143,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,126,61,63,26,237,55,247,92,126,205,156,195,79,252,26,191,198,175,251,43,126,141,223,244,47,34,16,191,7,165,27,126,15,3,207,229,34,126,211,63,233,215,225,244,204,175,253,31,253,26,191,198,127,253,39,81,252,254,23,225,255,191,37,127,255,107,210,247,191,198,159,244,107,113,14,130,127,255,131,241,247,88,190,251,155,240,62,229,28,144,118,248,131,94,166,255,25,250,248,155,204,79,121,231,215,162,119,190,194,123,127,208,111,228,254,254,131,241,153,247,253,31,252,43,126,141,175,254,34,250,251,215,249,53,181,253,203,244,43,134,33,120,253,154,148,199,248,138,255,15,56,191,190,195,235,15,250,245,248,247,95,139,127,255,53,25,222,111,64,239,254,223,222,187,191,206,31,132,92,200,47,39,248,191,156,198,36,159,25,156,254,154,63,8,253,226,61,202,236,208,123,191,6,191,247,235,242,223,191,54,114,45,12,87,242,47,191,54,191,131,207,220,223,248,254,175,249,147,36,31,243,235,160,255,63,88,233,136,182,244,249,127,205,109,126,29,78,191,105,123,197,237,215,80,26,73,223,191,233,127,244,23,185,191,65,6,198,235,55,16,154,255,67,120,239,127,250,53,126,185,254,253,107,241,223,255,179,253,251,215,225,191,255,23,251,247,111,192,127,255,50,250,27,249,158,255,39,0,0,255,255};
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
