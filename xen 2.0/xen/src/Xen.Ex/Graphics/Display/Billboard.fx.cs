// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = Billboard.fx
// Namespace = Xen.Ex.Graphics.Display

#if XBOX360
namespace Xen.Ex.Graphics.Display
{
	
	/// <summary><para>Technique 'DrawBillboardParticles_GpuTex' generated from file 'Billboard.fx'</para><para>Vertex Shader: approximately 38 instruction slots used (4 texture, 34 arithmetic), 5 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticles_GpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticles_GpuTex' shader</summary>
		public DrawBillboardParticles_GpuTex()
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
			DrawBillboardParticles_GpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticles_GpuTex.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticles_GpuTex.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticles_GpuTex.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticles_GpuTex.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticles_GpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticles_GpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticles_GpuTex.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticles_GpuTex.gd))
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
				DrawBillboardParticles_GpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticles_GpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticles_GpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticles_GpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticles_GpuTex.fx, DrawBillboardParticles_GpuTex.fxb, 37, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticles_GpuTex.vin[i]));
			index = DrawBillboardParticles_GpuTex.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,88,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,160,135,0,1,5,131,0,1,4,131,0,1,1,211,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,196,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,232,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,12,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,5,131,0,0,1,1,131,0,0,1,7,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,172,131,0,0,1,192,139,0,0,1,208,131,0,0,1,228,139,0,0,1,244,1,0,1,0,1,1,1,8,138,0,0,1,1,1,76,135,0,0,1,1,1,0,1,0,1,1,1,72,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,28,1,0,1,0,1,1,1,24,131,0,0,1,93,134,0,0,1,1,1,52,1,0,1,0,1,1,1,48,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,2,32,1,3,31,31,246,4,136,0,0,64,3,0,22,16,133,0,3,27,226,0,4,0,1,20,14,131,0,4,252,252,27,225,5,2,1,2,12,135,6,128,0,0,21,108,108,1,225,151,0,0,132,255,0,138,0,0,1,3,1,52,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,128,1,0,1,0,1,1,1,180,135,0,0,1,36,1,0,1,0,1,1,1,28,1,0,1,0,1,1,1,68,139,0,0,1,244,131,0,0,1,28,131,0,0,1,230,1,255,1,254,1,3,132,0,0,1,3,131,0,0,1,28,135,0,0,1,223,131,0,0,1,88,1,0,1,2,131,0,0,1,5,133,0,0,1,96,131,0,0,1,112,131,0,0,1,192,1,0,1,3,131,0,1,1,133,0,0,1,200,135,0,0,1,216,1,0,1,3,1,0,1,1,2,0,1,133,0,0,1,200,132,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,5,214,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,116,1,0,1,17,1,0,1,3,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,29,1,0,1,0,1,16,1,28,160,0,0,1,192,1,73,1,15,1,219,1,63,131,0,0,1,62,1,34,1,249,1,131,1,63,1,128,1,0,1,0,1,64,1,201,1,15,1,219,140,0,0,1,16,1,9,1,96,1,4,1,64,1,10,1,18,1,0,1,18,1,0,1,0,1,80,132,0,0,1,96,1,14,1,194,1,0,1,18,133,0,0,1,96,1,20,1,32,1,26,1,18,1,0,1,18,135,0,0,1,32,1,28,1,196,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,4,1,120,132,0,0,1,180,1,35,1,0,1,2,1,0,1,176,1,177,1,192,1,1,1,4,1,254,1,4,1,168,1,64,133,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,128,2,0,0,3,200,8,0,4,2,1,198,177,2,177,237,131,0,3,168,64,2,132,0,4,128,194,0,0,3,4,200,3,131,0,5,110,25,0,224,2,4,2,0,184,64,134,0,3,194,0,0,4,254,101,8,32,5,1,15,31,254,200,6,0,0,64,0,101,24,7,16,1,15,31,255,223,0,5,0,64,0,200,13,131,0,8,5,178,0,161,1,254,0,176,1,32,133,0,7,128,194,0,0,254,44,32,133,0,2,177,226,131,0,2,168,32,133,0,3,65,194,0,4,0,255,200,14,131,0,4,1,188,0,160,5,0,254,0,196,16,1,3,132,0,2,177,226,131,0,3,192,32,3,132,0,2,177,226,131,0,4,200,15,0,1,5,0,188,218,0,225,6,3,1,0,0,18,1,6,1,2,177,27,102,224,131,1,7,200,3,0,1,0,176,198,8,176,235,1,2,2,200,1,128,9,62,0,109,109,27,145,1,0,0,9,200,2,128,62,0,109,109,27,145,131,1,10,200,4,128,62,0,109,109,27,145,1,11,2,2,200,8,128,62,0,109,109,27,145,7,1,3,3,36,254,192,1,131,0,12,108,226,0,0,128,200,3,128,0,0,26,26,2,0,226,142,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 invTextureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 InvTextureSizeOffset
		{
			set
			{
				this.SetInvTextureSizeOffset(ref value);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[5];
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
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawBillboardParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticlesColour_GpuTex' generated from file 'Billboard.fx'</para><para>Vertex Shader: approximately 40 instruction slots used (6 texture, 34 arithmetic), 5 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticlesColour_GpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticlesColour_GpuTex' shader</summary>
		public DrawBillboardParticlesColour_GpuTex()
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
			DrawBillboardParticlesColour_GpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticlesColour_GpuTex.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticlesColour_GpuTex.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticlesColour_GpuTex.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawBillboardParticlesColour_GpuTex.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticlesColour_GpuTex.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticlesColour_GpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticlesColour_GpuTex.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawBillboardParticlesColour_GpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticlesColour_GpuTex.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticlesColour_GpuTex.gd))
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
				DrawBillboardParticlesColour_GpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticlesColour_GpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticlesColour_GpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticlesColour_GpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticlesColour_GpuTex.fx, DrawBillboardParticlesColour_GpuTex.fxb, 39, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticlesColour_GpuTex.vin[i]));
			index = DrawBillboardParticlesColour_GpuTex.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,124,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,160,135,0,1,5,131,0,1,4,131,0,1,1,211,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,196,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,232,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,12,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,48,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,172,131,0,0,1,192,139,0,0,1,208,131,0,0,1,228,139,0,0,1,244,1,0,1,0,1,1,1,8,138,0,0,1,1,1,24,1,0,1,0,1,1,1,44,138,0,0,1,1,1,112,135,0,0,1,1,1,0,1,0,1,1,1,108,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,64,1,0,1,0,1,1,1,60,131,0,0,1,93,134,0,0,1,1,1,88,1,0,1,0,1,1,1,84,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,2,31,31,3,246,136,0,4,0,64,0,22,1,16,133,0,3,27,226,0,4,0,1,20,14,131,0,4,252,252,27,225,5,2,1,2,12,135,6,128,0,0,21,108,108,1,225,151,0,0,132,255,0,138,0,0,1,3,1,92,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,156,1,0,1,0,1,1,1,192,135,0,0,1,36,1,0,1,0,1,1,1,56,1,0,1,0,1,1,1,96,138,0,0,1,1,1,16,131,0,0,1,28,1,0,1,0,1,1,1,1,1,255,1,254,1,3,132,0,0,1,4,131,0,1,28,135,0,0,1,250,131,0,0,1,108,1,0,1,2,131,0,1,5,133,0,0,1,116,131,0,0,1,132,131,0,1,212,2,0,3,131,0,1,1,133,0,0,1,220,135,0,0,1,236,1,0,1,3,1,0,2,1,0,1,1,133,0,1,220,135,0,0,1,243,1,0,1,3,1,0,2,2,0,1,1,133,0,1,220,132,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,5,214,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,128,1,0,1,17,1,0,1,4,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,29,1,0,1,0,1,16,1,30,160,0,0,1,192,1,73,1,15,1,219,1,63,131,0,0,1,62,1,34,1,249,1,131,1,63,1,128,1,0,1,0,1,64,1,201,1,15,1,219,140,0,0,1,16,1,9,1,96,1,4,1,80,1,10,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,15,1,194,1,0,1,18,133,0,0,1,96,1,21,1,32,1,27,1,18,1,0,1,18,135,0,0,1,32,1,29,1,196,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,4,1,120,132,0,0,1,180,1,35,1,0,1,2,1,0,1,176,1,177,1,192,1,1,1,4,1,254,1,4,1,168,1,64,133,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,1,198,2,0,198,3,232,128,0,4,0,200,8,0,5,2,1,198,177,177,1,237,131,0,3,168,64,2,132,0,5,128,194,0,0,4,2,200,3,131,0,6,110,25,0,224,2,2,3,0,184,64,134,0,4,194,0,0,254,5,101,24,32,1,15,6,31,246,136,0,0,64,7,0,101,8,48,1,15,31,8,254,200,0,0,64,0,101,40,9,16,1,15,31,255,223,0,0,64,3,0,200,13,131,0,9,5,178,0,161,1,254,0,176,32,133,0,7,128,194,0,0,254,44,32,133,0,2,177,226,131,0,2,168,32,133,0,4,65,194,0,0,3,255,200,7,131,0,5,21,176,0,160,0,5,254,0,196,16,4,132,0,2,108,226,131,0,3,192,32,4,132,0,2,108,226,131,0,4,200,15,0,1,5,0,188,218,0,225,6,4,1,0,0,18,1,6,1,2,177,27,102,224,131,1,7,200,3,0,1,0,176,198,8,176,235,1,3,3,200,1,128,9,62,0,109,109,27,145,1,0,0,9,200,2,128,62,0,109,109,27,145,131,1,10,200,4,128,62,0,109,109,27,145,1,11,2,2,200,8,128,62,0,109,109,27,145,12,1,3,3,200,3,128,0,0,197,197,0,226,131,0,4,200,15,128,1,132,0,3,226,2,2,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 invTextureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 InvTextureSizeOffset
		{
			set
			{
				this.SetInvTextureSizeOffset(ref value);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[5];
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
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawBillboardParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticles_GpuTex_UserOffset' generated from file 'Billboard.fx'</para><para>Vertex Shader: approximately 41 instruction slots used (6 texture, 35 arithmetic), 5 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticles_GpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticles_GpuTex_UserOffset' shader</summary>
		public DrawBillboardParticles_GpuTex_UserOffset()
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
			DrawBillboardParticles_GpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticles_GpuTex_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticles_GpuTex_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticles_GpuTex_UserOffset.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticles_GpuTex_UserOffset.sid2 = state.GetNameUniqueID("UserSampler");
			DrawBillboardParticles_GpuTex_UserOffset.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticles_GpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticles_GpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticles_GpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawBillboardParticles_GpuTex_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticles_GpuTex_UserOffset.gd))
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
				DrawBillboardParticles_GpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticles_GpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticles_GpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticles_GpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticles_GpuTex_UserOffset.fx, DrawBillboardParticles_GpuTex_UserOffset.fxb, 40, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticles_GpuTex_UserOffset.vin[i]));
			index = DrawBillboardParticles_GpuTex_UserOffset.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,124,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,160,135,0,1,5,131,0,1,4,131,0,1,1,211,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,196,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,232,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,12,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,48,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,172,131,0,0,1,192,139,0,0,1,208,131,0,0,1,228,139,0,0,1,244,1,0,1,0,1,1,1,8,138,0,0,1,1,1,24,1,0,1,0,1,1,1,44,138,0,0,1,1,1,112,135,0,0,1,1,1,0,1,0,1,1,1,108,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,64,1,0,1,0,1,1,1,60,131,0,0,1,93,134,0,0,1,1,1,88,1,0,1,0,1,1,1,84,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,2,31,31,3,246,136,0,4,0,64,0,22,1,16,133,0,3,27,226,0,4,0,1,20,14,131,0,4,252,252,27,225,5,2,1,2,12,135,6,128,0,0,21,108,108,1,225,151,0,0,132,255,0,138,0,0,1,3,1,92,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,156,1,0,1,0,1,1,1,192,135,0,0,1,36,1,0,1,0,1,1,1,56,1,0,1,0,1,1,1,96,138,0,0,1,1,1,16,131,0,0,1,28,1,0,1,0,1,1,1,1,1,255,1,254,1,3,132,0,0,1,4,131,0,1,28,135,0,0,1,250,131,0,0,1,108,1,0,1,2,131,0,1,5,133,0,0,1,116,131,0,0,1,132,131,0,1,212,2,0,3,131,0,1,1,133,0,0,1,220,135,0,0,1,236,1,0,1,3,1,0,2,1,0,1,1,133,0,1,220,135,0,0,1,243,1,0,1,3,1,0,2,2,0,1,1,133,0,1,220,132,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,5,214,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,128,1,0,1,17,1,0,1,3,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,30,1,0,1,0,1,16,1,29,160,0,0,1,192,1,73,1,15,1,219,1,63,131,0,0,1,62,1,34,1,249,1,131,1,63,1,128,1,0,1,0,1,64,1,201,1,15,1,219,140,0,0,1,16,1,9,1,96,1,4,1,80,1,10,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,15,1,194,1,0,1,18,133,0,0,1,96,1,21,1,32,1,27,1,18,1,0,1,18,135,0,0,1,32,1,29,1,196,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,4,1,120,132,0,0,1,180,1,35,1,0,1,2,1,0,1,176,1,177,1,192,1,1,1,4,1,254,1,4,1,168,1,64,133,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,1,198,2,0,198,3,232,128,0,4,0,200,8,0,5,2,1,198,177,177,1,237,131,0,3,168,64,2,132,0,5,128,194,0,0,4,2,200,3,131,0,6,110,25,0,224,2,2,3,0,184,64,134,0,4,194,0,0,254,5,101,8,32,1,15,6,31,254,200,0,0,64,7,0,101,40,48,1,15,31,8,254,143,0,0,64,0,101,24,9,16,1,15,31,255,223,0,0,64,3,0,200,13,131,0,9,5,178,0,161,1,254,0,176,32,133,0,7,128,194,0,0,254,44,32,133,0,2,177,226,131,0,2,168,32,133,0,4,65,194,0,0,3,255,200,14,131,0,5,1,188,0,160,0,5,254,0,196,16,3,132,0,2,177,226,131,0,5,192,35,3,2,0,6,176,197,177,224,2,3,7,0,200,15,0,1,0,188,8,218,0,225,3,1,0,0,18,7,1,1,2,177,27,102,224,131,1,9,200,3,0,1,0,176,198,176,235,10,1,2,2,200,1,128,62,0,109,109,11,27,145,1,0,0,200,2,128,62,0,109,3,109,27,145,131,1,12,200,4,128,62,0,109,109,27,145,1,2,2,13,200,8,128,62,0,109,109,27,145,1,3,3,36,3,254,192,1,131,0,14,108,226,0,0,128,200,3,128,0,0,26,26,0,226,142,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 invTextureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 InvTextureSizeOffset
		{
			set
			{
				this.SetInvTextureSizeOffset(ref value);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[5];
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
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawBillboardParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.sid2))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticlesColour_GpuTex_UserOffset' generated from file 'Billboard.fx'</para><para>Vertex Shader: approximately 43 instruction slots used (8 texture, 35 arithmetic), 5 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticlesColour_GpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticlesColour_GpuTex_UserOffset' shader</summary>
		public DrawBillboardParticlesColour_GpuTex_UserOffset()
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
			DrawBillboardParticlesColour_GpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticlesColour_GpuTex_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticlesColour_GpuTex_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticlesColour_GpuTex_UserOffset.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawBillboardParticlesColour_GpuTex_UserOffset.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticlesColour_GpuTex_UserOffset.sid3 = state.GetNameUniqueID("UserSampler");
			DrawBillboardParticlesColour_GpuTex_UserOffset.sid4 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticlesColour_GpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticlesColour_GpuTex_UserOffset.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawBillboardParticlesColour_GpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticlesColour_GpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawBillboardParticlesColour_GpuTex_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticlesColour_GpuTex_UserOffset.gd))
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
				DrawBillboardParticlesColour_GpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticlesColour_GpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticlesColour_GpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticlesColour_GpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticlesColour_GpuTex_UserOffset.fx, DrawBillboardParticlesColour_GpuTex_UserOffset.fxb, 42, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticlesColour_GpuTex_UserOffset.vin[i]));
			index = DrawBillboardParticlesColour_GpuTex_UserOffset.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,112,135,0,1,3,131,0,1,1,131,0,1,112,135,0,1,5,131,0,1,4,131,0,1,1,211,0,6,6,95,118,115,95,99,134,0,1,12,131,0,1,4,131,0,1,148,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,184,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,131,0,0,1,220,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,144,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,51,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,36,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,124,131,0,0,1,144,139,0,0,1,160,131,0,0,1,180,139,0,0,1,196,131,0,0,1,216,139,0,0,1,232,131,0,0,1,252,138,0,0,1,1,1,12,1,0,1,0,1,1,1,32,138,0,0,1,1,1,100,135,0,0,1,1,1,0,1,0,1,1,1,96,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,52,1,0,1,0,1,1,1,48,131,0,0,1,93,134,0,0,1,1,1,76,1,0,1,0,1,1,1,72,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,2,246,136,3,0,0,64,3,0,22,16,133,0,2,27,226,3,0,0,1,2,20,14,131,0,4,252,252,27,225,5,2,1,2,12,135,6,128,0,0,21,108,108,1,225,151,0,0,132,255,0,138,0,0,1,3,1,128,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,180,1,0,1,0,1,1,1,204,135,0,0,1,36,1,0,1,0,1,1,1,80,1,0,1,0,1,1,1,120,138,0,0,1,1,1,40,131,0,0,1,28,1,0,1,0,1,1,1,28,1,255,1,254,1,3,132,0,0,1,5,131,0,1,28,134,0,0,1,1,1,21,131,0,0,1,128,2,0,2,131,0,1,5,133,0,0,1,136,131,0,1,152,131,0,1,232,2,0,3,131,0,1,1,133,0,0,1,240,134,0,0,1,1,1,0,1,0,2,3,0,3,1,0,1,133,0,1,240,134,0,0,1,1,1,7,1,0,2,3,0,3,2,0,1,133,0,1,240,134,0,0,1,1,1,14,1,0,2,3,0,3,3,0,1,133,0,1,240,132,0,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,5,214,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,95,1,118,1,115,1,95,1,115,1,51,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,136,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,140,1,0,1,17,1,0,1,4,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,30,1,0,1,0,1,16,1,31,160,0,0,1,192,1,73,1,15,1,219,1,63,131,0,0,1,62,1,34,1,249,1,131,1,63,1,128,1,0,1,0,1,64,1,201,1,15,1,219,140,0,0,1,16,1,9,1,96,1,4,1,96,1,10,1,18,1,0,1,18,1,0,1,5,1,80,132,0,0,1,96,1,16,1,194,1,0,1,18,133,0,0,1,96,1,22,1,32,1,28,1,18,1,0,1,18,135,0,0,1,32,1,30,1,196,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,4,1,120,132,0,0,1,180,1,35,1,0,1,2,1,0,1,176,1,177,1,192,1,1,1,4,1,254,1,4,1,168,1,64,133,0,0,1,65,1,194,2,0,0,3,4,52,18,131,0,3,198,0,198,4,232,128,0,0,5,200,8,0,2,1,4,198,177,177,237,131,0,3,168,64,2,132,0,5,128,194,0,0,4,2,200,3,131,0,6,110,25,0,224,2,2,3,0,184,64,134,0,4,194,0,0,254,5,101,24,32,1,15,6,31,246,136,0,0,64,7,0,101,8,48,1,15,31,8,254,200,0,0,64,0,101,56,9,64,1,15,31,254,143,0,0,64,10,0,101,40,16,1,15,31,255,223,0,5,0,64,0,200,13,131,0,9,5,178,0,161,1,254,0,176,32,133,0,7,128,194,0,0,254,44,32,133,0,2,177,226,131,0,2,168,32,133,0,5,65,194,0,0,255,2,200,7,131,0,6,21,176,0,160,0,254,4,0,196,16,4,132,0,2,108,226,131,0,6,192,35,4,3,0,176,7,197,108,224,3,4,0,200,8,15,0,1,0,188,218,0,225,9,4,1,0,0,18,1,1,2,177,3,27,102,224,131,1,10,200,3,0,1,0,176,198,176,235,1,11,3,3,200,1,128,62,0,109,109,27,145,12,1,0,0,200,2,128,62,0,109,109,27,145,131,1,12,200,4,128,62,0,109,109,27,145,1,2,2,13,200,8,128,62,0,109,109,27,145,1,3,3,200,8,3,128,0,0,197,197,0,226,131,0,4,200,15,128,1,132,0,3,226,2,2,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 invTextureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 InvTextureSizeOffset
		{
			set
			{
				this.SetInvTextureSizeOffset(ref value);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[5];
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
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawBillboardParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.sid3))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.sid4))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.tid4))
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
	
	/// <summary><para>Technique 'DrawBillboardParticles_GpuTex' generated from file 'Billboard.fx'</para><para>Vertex Shader: approximately 38 instruction slots used (4 texture, 34 arithmetic), 5 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticles_GpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticles_GpuTex' shader</summary>
		public DrawBillboardParticles_GpuTex()
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
			DrawBillboardParticles_GpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticles_GpuTex.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticles_GpuTex.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticles_GpuTex.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticles_GpuTex.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticles_GpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticles_GpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticles_GpuTex.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticles_GpuTex.gd))
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
				DrawBillboardParticles_GpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticles_GpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticles_GpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticles_GpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticles_GpuTex.fx, DrawBillboardParticles_GpuTex.fxb, 37, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticles_GpuTex.vin[i]));
			index = DrawBillboardParticles_GpuTex.vin[(i + 1)];
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
				return new byte[] {184,6,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,239,251,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,253,107,252,248,95,241,123,253,218,120,159,254,255,23,105,187,95,87,223,87,112,223,232,243,235,209,255,127,255,203,230,247,159,210,207,223,232,215,144,126,254,177,78,155,95,95,219,52,59,174,205,127,53,212,102,215,181,249,141,58,8,115,155,149,192,193,87,191,153,182,243,159,95,139,254,255,155,70,62,23,80,191,14,195,120,61,207,102,68,41,166,11,62,199,103,191,54,127,251,107,252,26,191,157,247,206,62,253,255,165,247,247,95,71,255,255,135,188,191,255,37,250,255,127,230,253,253,203,233,255,191,129,135,243,75,253,29,63,190,173,191,3,191,63,69,191,255,157,232,179,223,142,254,255,167,234,223,251,244,251,142,215,206,60,255,55,61,22,44,126,249,181,255,239,255,251,255,250,191,127,215,95,227,228,205,241,19,240,196,151,191,134,124,134,175,148,71,210,111,211,63,59,191,134,140,235,215,36,104,7,250,186,210,239,175,253,117,136,202,191,38,255,39,15,125,124,239,247,223,249,53,190,40,166,117,213,84,231,109,186,245,234,78,250,237,231,175,159,167,66,173,244,164,90,172,10,98,176,244,225,120,239,211,241,195,251,123,227,189,131,253,253,95,227,119,33,84,137,142,127,16,245,244,39,201,239,191,230,31,244,107,254,26,191,41,255,78,96,255,164,95,227,55,248,77,255,162,39,140,198,111,74,109,254,51,250,251,63,251,139,126,93,254,251,215,199,223,212,246,63,251,147,8,135,95,235,215,252,53,126,115,250,253,255,254,147,248,187,223,192,124,247,107,252,65,242,247,111,64,127,255,223,252,247,255,253,127,43,194,191,134,208,197,252,222,98,160,191,246,255,69,116,249,189,44,93,126,229,175,33,159,225,43,67,151,255,137,254,249,189,127,13,65,238,215,165,127,255,0,250,185,250,53,100,94,13,173,254,41,133,249,111,241,103,160,209,175,247,107,252,71,250,153,240,250,95,251,215,254,154,252,205,175,195,60,244,179,241,168,188,244,230,73,101,164,247,249,229,215,156,191,159,32,50,252,186,191,198,111,250,23,253,161,191,234,163,35,2,243,123,252,187,191,233,63,253,123,254,187,191,233,217,63,132,207,127,61,250,28,159,17,217,255,65,233,229,15,250,61,116,94,255,160,95,163,51,199,230,247,95,211,253,142,54,255,145,225,15,162,151,254,254,107,254,65,191,22,125,78,178,246,235,224,179,95,227,215,248,175,255,36,210,33,127,17,254,255,107,241,20,252,154,127,208,175,243,107,252,53,232,87,120,129,254,6,44,106,255,23,253,150,12,247,215,194,223,127,236,111,204,223,253,58,252,29,253,255,15,254,45,24,30,218,254,53,244,255,175,240,255,63,216,107,255,7,25,216,210,254,171,63,24,176,127,77,253,238,215,249,53,190,250,139,126,77,110,251,107,254,65,192,227,215,97,85,240,107,211,231,255,25,240,0,143,254,65,242,217,111,196,176,127,189,95,227,79,35,124,255,182,191,232,247,103,24,224,235,255,237,15,250,53,137,175,127,127,238,67,254,6,159,203,59,191,14,248,26,124,76,124,255,149,142,1,120,255,53,10,83,126,255,117,105,204,191,238,175,241,127,255,69,191,59,203,194,175,173,159,1,254,111,66,239,191,164,191,103,127,18,26,255,90,132,243,175,201,99,248,107,72,110,254,239,63,24,227,250,181,88,230,254,26,250,255,87,10,243,215,166,241,254,103,127,16,100,70,229,136,251,252,245,8,254,111,32,116,248,143,126,13,254,254,151,235,223,191,214,127,36,237,204,223,191,14,255,253,107,217,191,127,3,254,251,215,166,191,1,11,243,7,88,144,197,255,39,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 invTextureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 InvTextureSizeOffset
		{
			set
			{
				this.SetInvTextureSizeOffset(ref value);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[5];
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
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawBillboardParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticles_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticlesColour_GpuTex' generated from file 'Billboard.fx'</para><para>Vertex Shader: approximately 40 instruction slots used (6 texture, 34 arithmetic), 5 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticlesColour_GpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticlesColour_GpuTex' shader</summary>
		public DrawBillboardParticlesColour_GpuTex()
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
			DrawBillboardParticlesColour_GpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticlesColour_GpuTex.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticlesColour_GpuTex.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticlesColour_GpuTex.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawBillboardParticlesColour_GpuTex.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticlesColour_GpuTex.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticlesColour_GpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticlesColour_GpuTex.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawBillboardParticlesColour_GpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticlesColour_GpuTex.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticlesColour_GpuTex.gd))
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
				DrawBillboardParticlesColour_GpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticlesColour_GpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticlesColour_GpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticlesColour_GpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticlesColour_GpuTex.fx, DrawBillboardParticlesColour_GpuTex.fxb, 39, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticlesColour_GpuTex.vin[i]));
			index = DrawBillboardParticlesColour_GpuTex.vin[(i + 1)];
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
				return new byte[] {40,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,31,244,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,253,107,252,248,95,241,123,253,218,120,159,254,255,23,105,187,95,87,223,87,112,223,232,243,235,209,255,127,255,203,230,247,159,210,207,223,232,215,144,126,254,177,78,155,95,95,219,52,59,174,205,127,53,212,102,215,181,249,141,58,8,219,54,123,174,205,78,172,205,74,250,194,87,191,153,182,243,159,95,139,254,255,155,70,62,55,160,0,227,245,60,155,17,53,121,124,248,252,55,160,255,131,174,120,231,183,243,222,217,167,255,191,244,254,254,235,232,255,255,144,247,247,191,68,255,255,207,188,191,127,57,253,255,55,240,112,254,237,232,247,145,247,119,171,191,227,71,169,191,3,223,63,69,191,255,61,233,179,199,244,255,63,85,255,254,189,233,247,55,94,59,243,252,223,244,88,176,248,229,215,254,191,255,239,255,235,255,254,93,127,141,147,55,199,79,192,71,95,254,26,242,25,190,82,190,74,191,77,255,236,252,26,50,206,95,147,160,29,232,235,74,207,191,246,215,33,170,255,154,252,159,60,244,241,189,223,127,231,215,248,162,152,214,85,83,157,183,233,214,171,59,233,183,159,191,126,158,10,245,210,147,106,177,42,136,41,211,135,227,189,79,199,15,239,239,141,247,14,246,247,127,141,223,133,80,37,158,252,131,168,167,63,73,126,255,53,137,213,127,83,254,157,192,254,73,191,198,111,240,155,254,69,79,24,141,223,148,218,252,103,244,247,127,246,23,253,186,252,247,175,143,191,169,237,127,246,39,17,14,191,214,175,249,107,252,230,244,251,255,253,39,241,119,191,129,249,238,215,248,131,228,239,223,128,254,254,191,249,239,255,251,255,86,132,127,13,161,139,249,253,111,194,64,127,237,255,139,232,242,147,150,46,63,254,107,202,103,158,188,165,191,19,125,86,254,26,130,220,175,75,255,182,244,243,15,163,255,255,107,191,134,163,213,191,167,48,255,59,254,12,52,250,245,120,174,241,252,58,191,38,62,251,181,232,179,196,242,180,200,204,95,251,215,254,154,220,250,215,97,25,253,217,120,84,238,122,115,167,178,22,255,124,175,255,249,229,215,156,235,159,32,146,253,186,191,198,111,250,23,17,136,223,131,166,226,31,20,104,127,208,239,129,207,127,61,250,252,15,253,85,31,29,225,187,127,247,55,253,167,127,207,127,247,55,61,251,135,148,7,254,160,95,163,195,15,230,247,95,211,251,253,215,114,191,163,253,127,100,248,138,104,170,191,255,154,127,208,175,69,159,255,90,60,77,191,230,31,244,235,252,26,127,13,240,16,126,161,191,241,30,205,243,95,244,91,50,140,95,11,127,255,177,191,49,127,247,235,240,119,244,255,63,248,183,224,233,67,219,191,134,254,255,21,254,255,7,123,237,255,32,3,91,218,127,245,7,3,246,175,169,223,253,58,191,198,87,127,209,175,201,109,127,77,240,228,95,244,235,176,250,248,181,233,243,255,12,120,128,143,255,32,249,236,55,98,216,191,238,175,241,167,17,159,255,109,127,209,239,79,141,128,247,175,241,107,252,111,127,208,175,73,188,255,235,48,14,191,54,253,253,95,3,119,200,2,183,249,53,89,62,254,183,63,8,242,241,251,51,30,242,247,175,165,239,8,94,255,247,31,68,58,236,47,250,245,8,151,223,82,113,209,113,235,247,191,6,125,255,215,208,247,255,247,95,244,187,211,247,191,22,225,135,207,140,188,253,90,191,198,191,70,127,255,219,127,18,218,255,90,52,174,95,139,199,249,215,252,73,244,255,63,248,215,98,60,1,227,43,126,199,140,239,215,98,57,252,191,121,124,50,254,95,135,224,253,223,127,209,111,32,180,250,143,32,167,164,9,245,239,95,235,63,18,185,53,127,255,58,252,247,175,101,255,254,13,248,239,95,155,254,134,28,255,63,1,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 invTextureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 InvTextureSizeOffset
		{
			set
			{
				this.SetInvTextureSizeOffset(ref value);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[5];
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
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawBillboardParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticlesColour_GpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticles_GpuTex_UserOffset' generated from file 'Billboard.fx'</para><para>Vertex Shader: approximately 41 instruction slots used (6 texture, 35 arithmetic), 5 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticles_GpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticles_GpuTex_UserOffset' shader</summary>
		public DrawBillboardParticles_GpuTex_UserOffset()
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
			DrawBillboardParticles_GpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticles_GpuTex_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticles_GpuTex_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticles_GpuTex_UserOffset.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticles_GpuTex_UserOffset.sid2 = state.GetNameUniqueID("UserSampler");
			DrawBillboardParticles_GpuTex_UserOffset.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticles_GpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticles_GpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticles_GpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawBillboardParticles_GpuTex_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticles_GpuTex_UserOffset.gd))
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
				DrawBillboardParticles_GpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticles_GpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticles_GpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticles_GpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticles_GpuTex_UserOffset.fx, DrawBillboardParticles_GpuTex_UserOffset.fxb, 40, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticles_GpuTex_UserOffset.vin[i]));
			index = DrawBillboardParticles_GpuTex_UserOffset.vin[(i + 1)];
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
				return new byte[] {68,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,31,244,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,253,107,252,248,95,241,123,253,218,120,159,254,255,23,105,187,95,87,223,87,112,223,232,243,235,209,255,127,255,203,230,247,159,210,207,223,232,215,144,126,254,177,78,155,95,95,219,52,59,174,205,127,53,212,102,215,181,249,141,58,8,219,54,123,174,205,78,172,205,74,250,194,87,191,153,182,243,159,95,139,254,255,155,70,62,55,160,0,227,245,60,155,17,53,121,124,248,252,55,160,255,131,174,120,231,183,243,222,217,167,255,191,244,254,254,235,232,255,255,144,247,247,191,68,255,255,207,188,191,127,57,253,255,55,240,112,254,237,232,247,145,247,119,171,191,227,71,169,191,3,223,63,69,191,255,61,233,179,199,244,255,63,85,255,254,189,233,247,55,94,59,243,252,223,244,88,176,248,229,215,254,191,255,239,255,235,255,254,93,127,141,147,55,199,79,192,71,95,254,26,242,25,190,82,190,74,191,77,255,236,252,26,50,206,95,147,160,29,232,235,74,207,191,246,215,33,170,255,154,252,159,60,244,241,189,223,127,231,215,248,162,152,214,85,83,157,183,233,214,171,59,233,183,159,191,126,158,10,245,210,147,106,177,42,136,41,211,135,227,189,79,199,15,239,239,141,247,14,246,247,127,141,223,133,80,37,158,252,131,168,167,63,73,126,255,53,137,213,127,83,254,157,192,254,73,191,198,111,240,155,254,69,79,24,141,223,148,218,252,103,244,247,127,246,23,253,186,252,247,175,143,191,169,237,127,246,39,17,14,191,214,175,249,107,252,230,244,251,255,253,39,241,119,191,129,249,238,215,248,131,228,239,223,128,254,254,191,249,239,255,251,255,86,132,127,13,161,139,249,253,159,195,64,127,237,255,139,232,242,147,150,46,63,254,107,202,103,158,188,165,191,19,125,86,254,26,130,220,175,75,255,182,244,243,15,163,255,255,107,191,134,163,213,191,167,48,255,59,254,12,52,250,245,120,174,241,252,58,191,38,62,251,181,232,179,196,242,180,200,204,95,251,215,254,154,220,250,215,97,25,253,217,120,84,238,122,115,167,178,22,255,124,175,255,249,229,215,156,235,159,32,146,253,186,191,198,111,250,23,253,161,191,234,163,35,2,243,123,252,187,191,233,63,253,123,254,187,191,233,217,63,132,207,127,61,250,28,159,209,20,253,131,210,203,31,244,123,40,15,252,65,191,70,135,31,204,239,191,166,247,251,175,229,126,71,251,255,200,240,21,209,84,127,255,53,255,160,95,139,62,255,117,120,10,126,237,255,232,215,248,53,254,235,63,137,228,249,47,194,255,127,45,158,186,95,243,15,250,117,126,141,191,6,56,8,15,209,223,128,69,237,255,162,223,146,225,254,90,248,251,143,253,141,249,187,95,135,191,163,255,255,193,191,5,195,67,219,191,134,254,255,21,254,255,7,123,237,255,32,3,91,218,127,245,7,3,246,175,169,223,253,58,191,198,87,127,209,175,201,109,127,205,63,8,120,252,58,172,82,126,109,250,252,63,3,30,224,237,63,72,62,251,141,24,246,175,247,107,252,105,132,239,223,246,23,253,254,12,3,242,240,191,253,65,191,22,201,3,254,254,181,244,239,95,67,255,254,53,244,239,95,147,254,22,24,192,225,255,134,60,144,252,124,165,99,178,99,212,239,33,47,127,13,125,255,127,255,69,191,59,125,255,107,19,46,110,12,191,54,225,243,95,211,239,255,217,31,36,99,248,245,255,160,95,251,215,248,215,232,251,127,251,79,194,251,191,22,141,233,215,230,49,254,53,36,143,127,205,31,252,107,49,78,144,229,175,88,238,204,216,240,254,175,69,120,168,124,254,90,160,229,175,71,253,253,6,66,167,255,8,159,147,102,212,191,127,173,255,72,218,153,191,127,29,254,251,215,178,127,255,6,252,247,175,77,127,3,22,230,23,176,32,227,255,79,0,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 invTextureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 InvTextureSizeOffset
		{
			set
			{
				this.SetInvTextureSizeOffset(ref value);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[5];
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
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawBillboardParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.sid2))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticles_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticlesColour_GpuTex_UserOffset' generated from file 'Billboard.fx'</para><para>Vertex Shader: approximately 43 instruction slots used (8 texture, 35 arithmetic), 5 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticlesColour_GpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticlesColour_GpuTex_UserOffset' shader</summary>
		public DrawBillboardParticlesColour_GpuTex_UserOffset()
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
			DrawBillboardParticlesColour_GpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticlesColour_GpuTex_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticlesColour_GpuTex_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticlesColour_GpuTex_UserOffset.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawBillboardParticlesColour_GpuTex_UserOffset.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticlesColour_GpuTex_UserOffset.sid3 = state.GetNameUniqueID("UserSampler");
			DrawBillboardParticlesColour_GpuTex_UserOffset.sid4 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticlesColour_GpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticlesColour_GpuTex_UserOffset.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawBillboardParticlesColour_GpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticlesColour_GpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawBillboardParticlesColour_GpuTex_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticlesColour_GpuTex_UserOffset.gd))
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
				DrawBillboardParticlesColour_GpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticlesColour_GpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticlesColour_GpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticlesColour_GpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticlesColour_GpuTex_UserOffset.fx, DrawBillboardParticlesColour_GpuTex_UserOffset.fxb, 42, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticlesColour_GpuTex_UserOffset.vin[i]));
			index = DrawBillboardParticlesColour_GpuTex_UserOffset.vin[(i + 1)];
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
				return new byte[] {116,7,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,87,251,107,202,239,191,54,254,166,255,175,244,187,95,151,254,255,235,232,103,223,244,243,235,209,255,127,255,203,230,247,159,210,207,223,232,215,144,126,254,180,78,155,95,95,219,52,59,174,205,223,53,212,102,215,181,249,247,134,218,236,185,54,221,65,217,54,247,92,155,223,45,214,102,197,248,252,58,248,234,55,211,118,254,243,107,209,255,127,211,200,231,2,234,255,254,191,1,227,245,60,155,229,245,175,241,235,252,122,250,249,111,240,107,8,237,241,78,234,189,243,51,244,255,63,201,251,251,47,162,255,255,109,222,223,255,24,253,255,223,242,254,254,175,232,255,255,135,247,247,111,68,192,83,111,12,115,253,29,63,254,0,253,29,248,254,41,250,253,62,125,182,67,255,255,83,245,239,231,244,251,183,189,118,230,249,191,233,177,96,241,203,175,253,127,255,223,255,215,255,253,187,254,26,39,111,142,159,252,78,244,231,151,191,134,124,134,175,126,39,110,244,107,164,223,166,127,118,126,13,195,99,191,214,175,113,160,175,43,61,255,218,95,135,168,254,107,242,127,242,208,199,247,126,255,157,95,227,139,98,90,87,77,117,222,166,91,175,238,164,223,126,254,250,121,42,212,75,79,170,197,170,40,233,151,135,227,189,79,199,15,239,239,141,247,14,246,247,127,141,223,133,80,37,190,253,131,168,167,63,73,126,255,53,255,160,95,243,215,248,77,249,247,95,3,228,252,13,126,211,191,232,9,163,241,155,82,155,255,140,254,254,207,254,162,95,151,255,254,245,241,55,181,253,207,254,36,194,225,215,250,53,127,141,223,156,126,255,191,255,36,254,238,55,48,223,253,26,127,144,252,253,27,208,223,255,55,255,253,127,255,223,138,240,175,33,116,49,191,255,6,152,204,95,251,255,34,186,100,142,46,191,166,124,6,185,178,116,249,53,25,89,70,238,215,165,127,255,40,250,249,103,253,26,50,151,134,86,255,147,1,250,107,226,51,208,232,215,251,53,126,3,37,212,111,199,159,253,90,244,89,98,231,122,135,63,67,203,223,228,215,56,208,207,68,214,254,218,191,246,215,228,207,127,29,150,237,159,141,71,229,181,55,159,42,163,241,207,247,6,62,191,215,255,252,242,107,242,197,79,16,121,127,221,95,227,55,253,139,8,196,239,65,20,255,7,5,218,31,244,123,224,243,95,143,62,255,67,127,213,71,71,248,238,223,253,77,255,233,223,243,223,253,77,207,254,33,229,151,63,232,215,232,240,142,249,253,215,244,126,255,181,188,223,127,109,247,59,222,253,143,12,63,18,221,245,247,95,243,15,250,181,232,243,95,139,167,247,215,252,131,126,157,95,227,175,1,78,194,103,244,55,222,35,214,249,139,126,75,134,241,107,225,239,63,246,55,230,239,126,29,254,142,254,255,7,255,22,191,6,244,15,218,254,53,244,255,175,240,255,63,216,107,255,7,25,216,210,254,171,63,24,176,127,77,253,238,215,249,53,190,250,139,126,77,110,251,107,130,151,255,162,95,135,213,206,175,77,159,255,103,192,131,240,252,207,254,32,249,236,55,98,216,191,238,175,241,167,145,124,252,109,127,209,239,79,141,128,247,175,241,107,252,111,127,208,175,73,50,243,235,48,14,191,54,253,253,95,3,119,200,16,183,249,53,89,174,254,183,63,232,215,166,54,250,14,255,253,107,232,223,191,134,254,253,107,41,12,193,243,255,254,131,72,23,254,69,191,30,225,246,91,42,110,74,7,253,254,215,160,239,255,26,250,254,255,254,139,126,119,250,158,120,219,27,231,175,77,56,255,215,244,251,127,246,7,201,56,127,253,63,232,215,254,53,254,53,250,254,223,254,147,240,254,175,69,227,254,181,153,14,127,205,159,244,107,254,26,127,205,31,252,107,49,78,208,9,95,253,65,16,61,51,126,188,255,107,17,30,42,231,191,22,232,253,235,82,127,191,129,208,242,63,194,231,191,198,175,241,203,245,239,95,235,63,146,118,230,239,95,135,255,254,181,236,223,191,1,255,253,107,211,223,208,15,255,79,0,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 invTextureSizeOffset'</summary>
		public Microsoft.Xna.Framework.Vector3 InvTextureSizeOffset
		{
			set
			{
				this.SetInvTextureSizeOffset(ref value);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[5];
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
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawBillboardParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.sid3))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.sid4))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticlesColour_GpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticles_BillboardCpu' generated from file 'Billboard.fx'</para><para>Vertex Shader: approximately 26 instruction slots used, 124 registers</para><para>Pixel Shader: approximately 6 instruction slots used (1 texture, 5 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticles_BillboardCpu : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticles_BillboardCpu' shader</summary>
		public DrawBillboardParticles_BillboardCpu()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawBillboardParticles_BillboardCpu.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticles_BillboardCpu.cid0 = state.GetNameUniqueID("positionData");
			DrawBillboardParticles_BillboardCpu.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticles_BillboardCpu.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticles_BillboardCpu.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[120], ref this.vreg[121], ref this.vreg[122], ref this.vreg[123], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawBillboardParticles_BillboardCpu.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticles_BillboardCpu.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticles_BillboardCpu.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticles_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticles_BillboardCpu.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticles_BillboardCpu.fx, DrawBillboardParticles_BillboardCpu.fxb, 24, 9);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticles_BillboardCpu.vin[i]));
			index = DrawBillboardParticles_BillboardCpu.vin[(i + 1)];
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
				return new byte[] {16,20,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,155,223,64,126,255,181,241,55,253,255,63,250,245,229,239,159,161,255,255,58,250,217,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,255,123,207,175,71,255,255,253,47,155,223,127,74,63,127,163,95,67,236,250,175,243,27,132,109,96,246,127,255,85,243,251,55,59,98,243,127,51,109,231,63,191,22,253,255,55,141,124,46,62,194,255,253,127,3,198,235,121,54,203,107,105,139,207,209,246,215,214,159,169,247,206,127,135,198,191,129,251,251,219,250,59,222,249,61,245,119,192,248,83,244,251,223,138,62,251,205,232,255,127,170,254,61,162,223,183,188,118,230,249,191,233,49,62,203,111,132,95,126,173,255,251,255,254,191,254,239,223,245,215,56,121,115,252,228,119,162,63,191,252,53,228,51,124,245,59,113,171,95,35,253,54,253,179,243,107,24,31,232,215,250,53,14,244,125,165,199,95,251,235,16,213,126,77,254,79,30,250,120,239,247,223,249,53,190,40,166,117,213,84,231,109,186,245,234,78,250,237,231,175,159,167,50,250,244,164,90,172,138,146,126,121,56,222,251,116,252,240,254,222,120,239,96,127,255,215,248,93,4,213,63,136,122,250,155,204,239,191,230,175,241,155,154,223,255,164,95,227,55,248,77,255,162,39,140,198,111,74,109,254,179,191,233,215,248,13,254,179,191,232,215,229,191,127,125,252,77,109,255,179,191,137,112,248,181,126,205,95,227,183,163,223,255,239,191,9,223,253,90,246,187,255,251,15,146,191,127,3,250,251,255,230,191,209,150,96,254,65,191,22,125,255,127,255,223,138,252,175,33,52,50,191,255,111,63,70,255,252,90,255,23,209,232,55,255,181,12,141,126,211,223,64,62,243,105,244,27,252,6,66,35,32,250,51,74,163,111,255,26,134,175,254,218,191,246,215,36,52,127,77,154,101,248,141,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,255,111,62,151,95,211,199,249,137,95,227,215,248,117,127,230,215,248,77,255,34,2,241,123,252,161,191,234,163,163,127,247,55,253,167,127,207,127,247,55,61,251,135,240,249,47,225,207,255,160,223,195,239,7,159,255,82,250,252,215,252,141,255,165,191,61,251,13,255,142,191,243,175,253,107,190,117,248,71,255,81,127,212,67,124,254,7,210,231,127,237,95,243,215,252,125,212,236,31,208,247,126,15,231,67,253,166,127,210,175,195,110,229,175,253,31,253,26,191,198,127,253,39,145,223,241,23,225,255,191,37,127,255,107,210,247,191,198,159,244,107,177,239,196,191,255,193,248,123,44,223,253,77,191,6,251,93,248,253,215,254,131,126,230,215,248,207,254,34,2,242,235,114,187,244,175,1,222,244,253,87,120,231,15,242,96,253,65,210,151,252,254,51,191,198,95,67,125,253,223,127,209,239,78,159,145,223,195,159,253,82,130,243,7,254,26,206,95,251,53,127,141,127,141,62,255,183,25,71,242,41,209,31,253,253,215,252,73,244,255,63,24,29,255,154,12,75,250,249,117,216,69,255,181,225,223,17,14,255,183,224,144,254,103,252,83,240,252,117,254,160,95,66,99,251,13,100,60,255,16,252,189,119,191,198,47,215,191,127,45,254,251,218,254,253,235,240,223,63,176,127,255,6,252,247,47,166,191,197,119,252,77,255,35,192,130,63,248,255,4,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'positionData'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float4 positionData[120]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
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
						> 120)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 120)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 positionData[120]'</summary>
		public Microsoft.Xna.Framework.Vector4[] PositionData
		{
			set
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[124];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector4[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector4[] value)
		{
			if ((DrawBillboardParticles_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_BillboardCpu.cid0))
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticles_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_BillboardCpu.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticles_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_BillboardCpu.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticlesColour_BillboardCpu' generated from file 'Billboard.fx'</para><para>Vertex Shader: approximately 26 instruction slots used, 244 registers</para><para>Pixel Shader: approximately 6 instruction slots used (1 texture, 5 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticlesColour_BillboardCpu : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticlesColour_BillboardCpu' shader</summary>
		public DrawBillboardParticlesColour_BillboardCpu()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawBillboardParticlesColour_BillboardCpu.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticlesColour_BillboardCpu.cid0 = state.GetNameUniqueID("colourData");
			DrawBillboardParticlesColour_BillboardCpu.cid1 = state.GetNameUniqueID("positionData");
			DrawBillboardParticlesColour_BillboardCpu.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticlesColour_BillboardCpu.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticlesColour_BillboardCpu.gd))
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
				DrawBillboardParticlesColour_BillboardCpu.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticlesColour_BillboardCpu.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticlesColour_BillboardCpu.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticlesColour_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticlesColour_BillboardCpu.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticlesColour_BillboardCpu.fx, DrawBillboardParticlesColour_BillboardCpu.fxb, 24, 9);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticlesColour_BillboardCpu.vin[i]));
			index = DrawBillboardParticlesColour_BillboardCpu.vin[(i + 1)];
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
				return new byte[] {20,35,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,191,246,155,202,239,191,54,254,166,255,255,1,250,247,47,167,255,255,58,250,217,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,230,231,215,163,255,255,254,151,205,239,63,165,159,191,209,175,33,113,245,31,166,113,182,121,126,125,180,89,53,191,127,179,35,49,247,111,166,237,252,231,215,162,255,227,181,238,231,18,163,255,223,255,55,96,188,158,103,179,188,150,182,248,28,109,127,109,253,153,122,239,148,4,232,15,242,112,248,167,244,119,188,243,15,233,239,128,241,167,232,247,127,26,125,246,39,209,255,255,84,253,251,175,163,223,255,42,175,157,121,254,111,122,76,206,224,55,194,47,191,214,255,253,127,255,95,255,247,239,250,107,156,188,57,126,242,59,209,159,95,254,26,242,25,190,250,157,184,213,175,145,126,155,254,217,249,53,76,14,226,215,250,53,14,244,125,165,199,95,251,235,16,213,126,77,254,79,30,250,120,239,247,223,249,53,190,40,166,117,213,84,231,109,186,245,234,78,250,237,231,175,159,167,50,250,244,164,90,172,138,146,126,121,56,222,251,116,252,240,254,222,120,239,96,127,255,215,248,93,4,213,63,136,122,250,155,204,239,191,230,175,241,155,154,223,255,164,95,227,55,248,77,255,162,39,140,198,111,74,109,254,179,191,233,215,248,13,254,179,191,232,215,229,191,127,125,252,77,109,255,179,191,137,112,248,181,126,205,95,227,183,163,223,255,239,191,9,223,253,90,246,187,255,251,15,146,191,127,3,250,251,255,230,191,209,150,96,254,65,191,22,125,255,127,255,223,138,252,175,33,52,50,191,255,204,111,78,255,252,90,255,23,209,232,127,254,181,13,141,254,196,223,84,62,243,105,244,71,253,166,66,35,32,250,203,149,70,223,254,53,12,95,253,181,127,237,175,73,104,254,154,52,203,200,219,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,159,223,207,143,70,127,251,231,242,107,230,24,126,226,215,248,53,126,221,95,254,107,252,166,127,17,165,22,126,15,31,30,62,255,21,252,249,175,241,123,252,161,191,234,163,163,127,247,55,253,167,127,207,127,247,55,61,251,135,240,249,255,74,159,255,154,191,241,191,244,183,103,191,225,223,241,119,254,181,127,205,183,14,255,232,63,234,143,122,136,207,127,37,125,254,215,254,53,127,205,223,71,239,252,3,10,239,247,112,57,140,223,244,79,250,117,56,173,243,107,255,71,191,198,175,241,95,255,73,191,226,215,248,53,254,34,252,255,183,228,239,127,77,250,254,215,248,147,126,45,206,93,240,239,127,48,254,30,203,119,127,211,175,193,121,15,252,254,107,255,65,191,226,215,248,207,254,34,2,242,235,114,187,244,175,1,126,244,253,87,120,231,15,242,96,253,65,210,151,252,254,43,126,141,191,134,250,250,191,255,162,223,157,62,163,188,3,127,246,191,18,156,95,249,107,184,124,201,175,249,107,252,107,244,249,191,205,56,82,78,7,253,209,223,127,205,159,68,255,255,131,209,241,175,201,176,164,159,95,135,83,100,191,54,242,43,132,195,255,45,56,164,255,25,255,252,53,185,237,111,250,31,189,115,127,211,235,191,206,31,68,121,142,191,232,55,144,241,253,67,200,191,252,79,191,198,47,215,191,127,45,254,251,127,182,127,255,58,252,247,255,98,255,254,13,248,239,95,70,127,35,39,243,255,4,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'colourData'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float4 colourData[120]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
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
						> 120)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 120)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 120)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 colourData[120]'</summary>
		public Microsoft.Xna.Framework.Vector4[] ColourData
		{
			set
			{
				this.SetColourData(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'positionData'</summary>
		private static int cid1;
		/// <summary>Set the shader array value 'float4 positionData[120]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
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
						> 120)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 120)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 positionData[120]'</summary>
		public Microsoft.Xna.Framework.Vector4[] PositionData
		{
			set
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[244];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector4[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector4[] value)
		{
			if ((DrawBillboardParticlesColour_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_BillboardCpu.cid0))
			{
				this.SetColourData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawBillboardParticlesColour_BillboardCpu.cid1))
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticlesColour_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_BillboardCpu.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticlesColour_BillboardCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_BillboardCpu.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
}
#endif
