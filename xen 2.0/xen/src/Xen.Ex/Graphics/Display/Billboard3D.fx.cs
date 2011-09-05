// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = Billboard3D.fx
// Namespace = Xen.Ex.Graphics.Display

#if XBOX360
namespace Xen.Ex.Graphics.Display
{
	
	/// <summary><para>Technique 'DrawBillboardParticles_GpuTex3D' generated from file 'Billboard3D.fx'</para><para>Vertex Shader: approximately 56 instruction slots used (4 texture, 52 arithmetic), 10 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticles_GpuTex3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticles_GpuTex3D' shader</summary>
		public DrawBillboardParticles_GpuTex3D()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawBillboardParticles_GpuTex3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticles_GpuTex3D.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticles_GpuTex3D.cid1 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawBillboardParticles_GpuTex3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticles_GpuTex3D.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticles_GpuTex3D.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticles_GpuTex3D.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticles_GpuTex3D.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticles_GpuTex3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticles_GpuTex3D.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[7], ref this.sc0));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[4], ref this.vreg[5], ref this.vreg[6], ref unused, ref this.sc1));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc2));
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
				DrawBillboardParticles_GpuTex3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticles_GpuTex3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticles_GpuTex3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticles_GpuTex3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticles_GpuTex3D.fx, DrawBillboardParticles_GpuTex3D.fxb, 51, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticles_GpuTex3D.vin[i]));
			index = DrawBillboardParticles_GpuTex3D.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,168,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,240,135,0,1,10,131,0,1,4,131,0,1,1,229,0,0,190,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,20,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,56,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,92,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,5,131,0,0,1,1,131,0,0,1,7,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,252,1,0,1,0,1,1,1,16,138,0,0,1,1,1,32,1,0,1,0,1,1,1,52,138,0,0,1,1,1,68,1,0,1,0,1,1,1,88,138,0,0,1,1,1,156,135,0,0,1,1,1,0,1,0,1,1,1,152,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,108,1,0,1,0,1,1,1,104,131,0,0,1,93,134,0,0,1,1,1,132,1,0,1,0,1,1,1,128,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,20,1,14,131,0,0,1,252,1,252,1,27,1,225,1,2,1,1,1,2,1,12,1,135,1,128,1,0,1,0,1,21,1,108,1,108,1,225,151,0,0,132,255,0,138,0,0,1,4,1,44,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,208,1,0,1,0,1,2,1,92,135,0,0,1,36,1,0,1,0,1,1,1,108,1,0,1,0,1,1,1,148,138,0,0,1,1,1,68,131,0,0,1,28,1,0,1,0,1,1,1,54,1,255,1,254,1,3,132,0,0,1,3,131,0,0,1,28,134,0,0,1,1,1,47,131,0,0,1,88,1,0,1,2,131,0,0,1,10,133,0,0,1,96,131,0,0,1,112,1,0,1,0,1,1,1,16,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,24,134,0,0,1,1,1,40,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,1,24,132,0,0,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,10,229,0,0,193,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,2,1,28,1,0,1,17,1,0,1,6,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,43,1,0,1,0,1,16,1,42,160,0,0,1,192,1,73,1,15,1,219,1,64,1,201,1,15,1,219,136,0,0,1,62,1,34,1,249,1,131,1,63,135,0,0,1,63,1,128,1,0,1,0,1,16,1,9,1,96,1,5,1,64,1,11,1,18,1,0,1,18,1,0,1,0,1,80,132,0,0,1,96,1,15,1,194,1,0,1,18,133,0,0,1,96,1,21,1,96,1,27,1,18,1,0,1,18,133,0,0,1,96,1,33,1,48,1,39,1,18,1,0,1,18,135,0,0,1,32,1,42,1,196,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,4,1,120,132,0,0,1,180,1,35,1,0,1,2,1,0,1,176,1,177,1,192,1,1,1,9,1,255,1,9,1,168,1,64,133,0,0,1,65,1,194,1,0,1,0,1,9,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,128,1,0,1,0,1,200,1,8,1,0,1,2,1,1,1,198,1,177,1,177,1,237,131,0,0,1,168,1,64,1,2,132,0,0,1,128,1,194,1,0,1,0,1,9,1,200,1,3,131,0,0,1,110,1,25,1,0,1,224,1,2,1,2,1,0,1,184,1,64,134,0,0,1,194,1,0,1,0,1,255,1,101,1,24,1,16,1,1,1,15,1,31,1,255,1,223,1,0,1,0,1,64,1,0,1,101,1,8,1,48,1,1,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,1,0,1,4,1,0,1,6,1,190,1,27,1,76,1,255,1,3,1,255,1,200,1,2,1,0,1,2,1,0,1,62,1,0,1,0,1,111,1,5,2,4,0,3,200,4,0,4,2,0,62,0,5,0,111,6,4,0,2,200,14,131,0,6,1,188,177,139,1,255,7,255,44,17,0,2,0,62,8,0,177,111,4,4,0,200,1,131,0,8,108,177,108,139,0,254,254,200,9,7,0,4,2,192,192,0,160,2,10,7,0,200,7,0,2,0,101,192,0,11,161,4,8,0,200,7,0,5,1,192,101,12,192,171,4,8,2,200,1,0,2,0,192,192,13,0,240,5,5,0,88,24,2,2,0,108,198,108,14,166,128,255,130,200,7,0,5,0,180,108,0,225,5,15,2,0,200,7,0,2,0,205,190,0,225,5,4,0,200,16,7,0,6,1,180,101,192,235,5,4,2,196,18,2,2,0,17,190,190,108,240,6,6,0,88,39,2,4,0,98,108,177,225,5,18,2,130,192,23,0,6,0,192,177,108,225,6,2,0,200,7,0,4,19,0,192,108,192,235,6,0,4,200,7,0,2,0,192,108,0,225,6,2,20,0,200,7,0,2,1,98,108,192,235,5,0,2,200,7,0,2,0,192,27,21,0,225,2,1,0,200,7,0,1,0,192,198,192,235,4,1,2,200,7,0,2,22,0,192,27,192,235,1,3,3,200,1,128,62,0,62,62,0,111,0,2,0,200,2,23,128,62,0,62,62,0,111,1,2,0,200,4,128,62,0,62,62,0,111,2,2,0,200,15,8,128,62,0,62,62,0,111,3,2,0,36,254,192,1,131,0,14,108,226,0,0,128,200,3,128,0,0,26,26,0,226,142,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 worldSpaceYAxis'</summary>
		public Microsoft.Xna.Framework.Vector3 WorldSpaceYAxis
		{
			set
			{
				this.SetWorldSpaceYAxis(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc2;
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[10];
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
			if ((DrawBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D.cid1))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticlesColour_GpuTex3D' generated from file 'Billboard3D.fx'</para><para>Vertex Shader: approximately 58 instruction slots used (6 texture, 52 arithmetic), 10 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticlesColour_GpuTex3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticlesColour_GpuTex3D' shader</summary>
		public DrawBillboardParticlesColour_GpuTex3D()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawBillboardParticlesColour_GpuTex3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticlesColour_GpuTex3D.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticlesColour_GpuTex3D.cid1 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawBillboardParticlesColour_GpuTex3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticlesColour_GpuTex3D.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawBillboardParticlesColour_GpuTex3D.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticlesColour_GpuTex3D.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticlesColour_GpuTex3D.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticlesColour_GpuTex3D.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawBillboardParticlesColour_GpuTex3D.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticlesColour_GpuTex3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticlesColour_GpuTex3D.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[7], ref this.sc0));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[4], ref this.vreg[5], ref this.vreg[6], ref unused, ref this.sc1));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc2));
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
				DrawBillboardParticlesColour_GpuTex3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticlesColour_GpuTex3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticlesColour_GpuTex3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticlesColour_GpuTex3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticlesColour_GpuTex3D.fx, DrawBillboardParticlesColour_GpuTex3D.fxb, 53, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticlesColour_GpuTex3D.vin[i]));
			index = DrawBillboardParticlesColour_GpuTex3D.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,204,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,240,135,0,1,10,131,0,1,4,131,0,1,1,229,0,0,190,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,20,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,56,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,92,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,128,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,252,1,0,1,0,1,1,1,16,138,0,0,1,1,1,32,1,0,1,0,1,1,1,52,138,0,0,1,1,1,68,1,0,1,0,1,1,1,88,138,0,0,1,1,1,104,1,0,1,0,1,1,1,124,138,0,0,1,1,1,192,135,0,0,1,1,1,0,1,0,1,1,1,188,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,144,1,0,1,0,1,1,1,140,131,0,0,1,93,134,0,0,1,1,1,168,1,0,1,0,1,1,1,164,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,20,1,14,131,0,0,1,252,1,252,1,27,1,225,1,2,1,1,1,2,1,12,1,135,1,128,1,0,1,0,1,21,1,108,1,108,1,225,151,0,0,132,255,0,138,0,0,1,4,1,84,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,236,1,0,1,0,1,2,1,104,135,0,0,1,36,1,0,1,0,1,1,1,136,1,0,1,0,1,1,1,176,138,0,0,1,1,1,96,131,0,0,1,28,1,0,1,0,1,1,1,81,1,255,1,254,1,3,132,0,0,1,4,131,0,0,1,28,134,0,0,1,1,1,74,131,0,0,1,108,1,0,1,2,131,0,0,1,10,133,0,0,1,116,131,0,0,1,132,1,0,1,0,1,1,1,36,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,44,134,0,0,1,1,1,60,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,1,44,134,0,0,1,1,1,67,1,0,1,3,1,0,1,2,1,0,1,1,132,0,0,1,1,1,44,132,0,0,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,2,1,0,3,4,0,10,229,0,0,193,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,2,1,40,1,0,1,17,1,0,1,7,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,43,1,0,1,0,1,16,1,44,160,0,0,1,192,1,73,1,15,1,219,1,64,1,201,1,15,1,219,136,0,0,1,62,1,34,1,249,1,131,1,63,135,0,0,1,63,1,128,1,0,1,0,1,16,1,9,1,96,1,5,1,80,1,11,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,16,1,194,1,0,1,18,133,0,0,1,96,1,22,1,96,1,28,1,18,1,0,1,18,133,0,0,1,96,1,34,1,48,1,40,1,18,1,0,1,18,135,0,0,1,32,1,43,1,196,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,4,1,120,132,0,0,1,180,1,35,1,0,1,2,1,0,1,176,1,177,1,192,1,1,1,9,1,255,1,9,1,168,1,64,133,0,0,1,65,1,194,1,0,1,0,1,9,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,128,1,0,1,0,1,200,1,8,1,0,1,2,1,1,1,198,1,177,1,177,1,237,131,0,0,1,168,1,64,1,2,132,0,0,1,128,1,194,1,0,1,0,1,9,1,200,1,3,131,0,0,1,110,1,25,1,0,1,224,1,2,1,2,1,0,1,184,1,64,134,0,0,1,194,1,0,1,0,1,255,1,101,1,24,1,32,1,1,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,101,1,40,1,16,1,1,1,15,1,31,1,255,1,223,1,0,1,0,1,64,1,0,1,101,1,8,1,64,1,1,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,1,0,1,5,2,0,6,3,190,27,76,4,255,4,255,200,5,2,0,3,0,62,6,0,0,111,5,5,0,7,200,4,0,3,0,62,0,7,0,111,6,5,0,200,14,131,0,8,1,188,177,139,1,255,255,44,9,17,0,3,0,62,0,177,111,4,4,5,0,200,1,131,0,10,108,177,108,139,0,254,254,200,7,0,11,5,2,192,192,0,160,3,7,0,200,7,12,0,3,0,101,192,0,161,5,8,0,200,7,13,0,6,1,192,101,192,171,5,8,3,200,1,0,14,3,0,192,192,0,240,6,6,0,88,24,3,3,0,15,108,198,108,166,128,255,131,200,7,0,6,0,180,108,0,16,225,6,3,0,200,7,0,3,0,205,190,0,225,6,5,0,17,200,7,0,7,1,180,101,192,235,6,5,3,196,18,3,3,0,18,190,190,108,240,7,7,0,88,39,3,5,0,98,108,177,225,6,3,19,131,192,23,0,7,0,192,177,108,225,7,3,0,200,7,0,5,0,192,20,108,192,235,7,0,5,200,7,0,3,0,192,108,0,225,7,3,0,200,7,21,0,3,1,98,108,192,235,6,0,3,200,7,0,3,0,192,27,0,225,3,1,22,0,200,7,0,1,0,192,198,192,235,5,1,3,200,7,0,3,0,192,27,192,235,23,1,4,4,200,1,128,62,0,62,62,0,111,0,3,0,200,2,128,62,0,62,62,0,24,111,1,3,0,200,4,128,62,0,62,62,0,111,2,3,0,200,8,128,62,0,62,62,0,13,111,3,3,0,200,3,128,0,0,26,26,0,226,131,0,4,200,15,128,1,132,0,3,226,2,2,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 worldSpaceYAxis'</summary>
		public Microsoft.Xna.Framework.Vector3 WorldSpaceYAxis
		{
			set
			{
				this.SetWorldSpaceYAxis(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc2;
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[10];
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
			if ((DrawBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.cid1))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticles_GpuTex3D_UserOffset' generated from file 'Billboard3D.fx'</para><para>Vertex Shader: approximately 59 instruction slots used (6 texture, 53 arithmetic), 10 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticles_GpuTex3D_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticles_GpuTex3D_UserOffset' shader</summary>
		public DrawBillboardParticles_GpuTex3D_UserOffset()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawBillboardParticles_GpuTex3D_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticles_GpuTex3D_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticles_GpuTex3D_UserOffset.cid1 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawBillboardParticles_GpuTex3D_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticles_GpuTex3D_UserOffset.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticles_GpuTex3D_UserOffset.sid2 = state.GetNameUniqueID("UserSampler");
			DrawBillboardParticles_GpuTex3D_UserOffset.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticles_GpuTex3D_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticles_GpuTex3D_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticles_GpuTex3D_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawBillboardParticles_GpuTex3D_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticles_GpuTex3D_UserOffset.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[7], ref this.sc0));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[4], ref this.vreg[5], ref this.vreg[6], ref unused, ref this.sc1));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc2));
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
				DrawBillboardParticles_GpuTex3D_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticles_GpuTex3D_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticles_GpuTex3D_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticles_GpuTex3D_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticles_GpuTex3D_UserOffset.fx, DrawBillboardParticles_GpuTex3D_UserOffset.fxb, 54, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticles_GpuTex3D_UserOffset.vin[i]));
			index = DrawBillboardParticles_GpuTex3D_UserOffset.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,204,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,240,135,0,1,10,131,0,1,4,131,0,1,1,229,0,0,190,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,20,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,56,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,92,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,128,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,252,1,0,1,0,1,1,1,16,138,0,0,1,1,1,32,1,0,1,0,1,1,1,52,138,0,0,1,1,1,68,1,0,1,0,1,1,1,88,138,0,0,1,1,1,104,1,0,1,0,1,1,1,124,138,0,0,1,1,1,192,135,0,0,1,1,1,0,1,0,1,1,1,188,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,144,1,0,1,0,1,1,1,140,131,0,0,1,93,134,0,0,1,1,1,168,1,0,1,0,1,1,1,164,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,20,1,14,131,0,0,1,252,1,252,1,27,1,225,1,2,1,1,1,2,1,12,1,135,1,128,1,0,1,0,1,21,1,108,1,108,1,225,151,0,0,132,255,0,138,0,0,1,4,1,84,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,236,1,0,1,0,1,2,1,104,135,0,0,1,36,1,0,1,0,1,1,1,136,1,0,1,0,1,1,1,176,138,0,0,1,1,1,96,131,0,0,1,28,1,0,1,0,1,1,1,81,1,255,1,254,1,3,132,0,0,1,4,131,0,0,1,28,134,0,0,1,1,1,74,131,0,0,1,108,1,0,1,2,131,0,0,1,10,133,0,0,1,116,131,0,0,1,132,1,0,1,0,1,1,1,36,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,44,134,0,0,1,1,1,60,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,1,44,134,0,0,1,1,1,67,1,0,1,3,1,0,1,2,1,0,1,1,132,0,0,1,1,1,44,132,0,0,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,2,1,0,3,4,0,10,229,0,0,193,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,2,1,40,1,0,1,17,1,0,1,7,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,44,1,0,1,0,1,16,1,43,160,0,0,1,192,1,73,1,15,1,219,1,64,1,201,1,15,1,219,136,0,0,1,62,1,34,1,249,1,131,1,63,135,0,0,1,63,1,128,1,0,1,0,1,16,1,9,1,96,1,5,1,80,1,11,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,16,1,194,1,0,1,18,133,0,0,1,96,1,22,1,96,1,28,1,18,1,0,1,18,133,0,0,1,96,1,34,1,48,1,40,1,18,1,0,1,18,135,0,0,1,32,1,43,1,196,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,4,1,120,132,0,0,1,180,1,35,1,0,1,2,1,0,1,176,1,177,1,192,1,1,1,9,1,255,1,9,1,168,1,64,133,0,0,1,65,1,194,1,0,1,0,1,9,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,128,1,0,1,0,1,200,1,8,1,0,1,2,1,1,1,198,1,177,1,177,1,237,131,0,0,1,168,1,64,1,2,132,0,0,1,128,1,194,1,0,1,0,1,9,1,200,1,3,131,0,0,1,110,1,25,1,0,1,224,1,2,1,2,1,0,1,184,1,64,134,0,0,1,194,1,0,1,0,1,255,1,101,1,24,1,16,1,1,1,15,1,31,1,255,1,223,1,0,1,0,1,64,1,0,1,101,1,8,1,32,1,1,1,15,1,31,1,240,1,139,1,0,1,0,1,64,1,0,1,101,1,40,1,0,1,1,1,15,1,31,1,254,1,209,1,0,1,0,1,64,1,0,1,36,1,135,1,3,1,3,2,0,195,3,192,108,224,4,2,0,128,200,1,14,131,0,5,1,188,177,139,1,6,255,255,200,2,0,4,7,0,62,62,0,111,5,3,8,0,200,4,0,4,0,62,62,9,0,111,6,3,0,44,17,0,4,10,0,62,62,177,111,4,3,0,200,1,131,0,10,108,177,108,139,0,254,254,200,7,0,11,5,2,192,192,0,160,4,7,0,200,7,12,0,4,0,101,192,0,161,5,8,0,200,14,13,0,4,1,252,65,252,171,5,8,4,200,1,0,14,4,0,21,21,0,240,4,4,0,88,24,4,2,0,15,108,198,108,166,128,255,132,200,7,0,6,0,201,108,0,16,225,4,4,0,200,7,0,4,0,205,190,0,225,6,5,0,17,200,7,0,7,1,180,101,192,235,6,5,4,196,18,4,4,0,18,190,190,108,240,7,7,0,88,39,4,5,0,98,108,177,225,6,4,19,132,192,30,0,4,0,252,177,108,225,7,4,0,200,7,0,5,0,21,20,108,192,235,4,0,5,200,7,0,4,0,21,108,0,225,4,4,0,200,7,21,0,4,1,98,108,192,235,6,0,4,200,7,0,4,0,192,27,0,225,4,1,22,0,200,7,0,1,0,192,198,192,235,5,1,4,200,7,0,2,0,192,108,192,235,23,1,2,3,200,1,128,62,0,62,62,0,111,0,2,0,200,2,128,62,0,62,62,0,24,111,1,2,0,200,4,128,62,0,62,62,0,111,2,2,0,200,8,128,62,0,62,62,0,8,111,3,2,0,36,254,192,1,131,0,14,108,226,0,0,128,200,3,128,0,0,26,26,0,226,142,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 worldSpaceYAxis'</summary>
		public Microsoft.Xna.Framework.Vector3 WorldSpaceYAxis
		{
			set
			{
				this.SetWorldSpaceYAxis(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc2;
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[10];
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
			if ((DrawBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.cid1))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.sid2))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticlesColour_GpuTex3D_UserOffset' generated from file 'Billboard3D.fx'</para><para>Vertex Shader: approximately 61 instruction slots used (8 texture, 53 arithmetic), 10 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticlesColour_GpuTex3D_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticlesColour_GpuTex3D_UserOffset' shader</summary>
		public DrawBillboardParticlesColour_GpuTex3D_UserOffset()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
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
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.cid1 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid3 = state.GetNameUniqueID("UserSampler");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid4 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[7], ref this.sc0));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[4], ref this.vreg[5], ref this.vreg[6], ref unused, ref this.sc1));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc2));
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
				DrawBillboardParticlesColour_GpuTex3D_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticlesColour_GpuTex3D_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticlesColour_GpuTex3D_UserOffset.fx, DrawBillboardParticlesColour_GpuTex3D_UserOffset.fxb, 56, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticlesColour_GpuTex3D_UserOffset.vin[i]));
			index = DrawBillboardParticlesColour_GpuTex3D_UserOffset.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,192,135,0,1,3,131,0,1,1,131,0,1,192,135,0,1,10,131,0,1,4,131,0,1,1,229,0,0,190,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,228,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,8,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,44,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,80,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,51,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,116,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,204,131,0,0,1,224,139,0,0,1,240,1,0,1,0,1,1,1,4,138,0,0,1,1,1,20,1,0,1,0,1,1,1,40,138,0,0,1,1,1,56,1,0,1,0,1,1,1,76,138,0,0,1,1,1,92,1,0,1,0,1,1,1,112,138,0,0,1,1,1,180,135,0,0,1,1,1,0,1,0,1,1,1,176,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,132,1,0,1,0,1,1,1,128,131,0,0,1,93,134,0,0,1,1,1,156,1,0,1,0,1,1,1,152,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,20,1,14,131,0,0,1,252,1,252,1,27,1,225,1,2,1,1,1,2,1,12,1,135,1,128,1,0,1,0,1,21,1,108,1,108,1,225,151,0,0,132,255,0,138,0,0,1,4,1,120,1,16,1,42,1,17,1,1,1,0,1,0,1,2,1,4,1,0,1,0,1,2,1,116,135,0,0,1,36,1,0,1,0,1,1,1,160,1,0,1,0,1,1,1,200,138,0,0,1,1,1,120,131,0,0,1,28,1,0,1,0,1,1,1,108,1,255,1,254,1,3,132,0,0,1,5,131,0,0,1,28,134,0,0,1,1,1,101,131,0,0,1,128,1,0,1,2,131,0,0,1,10,133,0,0,1,136,131,0,0,1,152,1,0,1,0,1,1,1,56,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,64,134,0,0,1,1,1,80,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,1,64,134,0,0,1,1,1,87,1,0,1,3,1,0,1,2,1,0,1,1,132,0,0,1,1,1,64,134,0,0,1,1,1,94,1,0,2,3,0,3,3,0,1,132,0,2,1,64,132,0,1,95,2,118,115,3,95,99,0,4,171,171,0,1,5,0,3,0,1,0,3,4,0,10,229,0,0,193,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,95,1,118,1,115,1,95,1,115,1,51,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,136,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,2,1,52,1,0,1,17,1,0,1,8,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,44,1,0,1,0,1,16,1,45,160,0,0,1,192,1,73,1,15,1,219,1,64,1,201,1,15,1,219,136,0,0,1,62,1,34,1,249,1,131,1,63,135,0,0,1,63,1,128,1,0,1,0,1,16,1,9,1,96,1,5,1,96,1,11,1,18,1,0,1,18,1,0,1,5,1,80,132,0,0,1,96,1,17,1,194,1,0,1,18,133,0,0,1,96,1,23,1,96,1,29,1,18,1,0,1,18,133,0,0,1,96,1,35,1,48,1,41,1,18,1,0,1,18,135,0,0,1,32,1,44,1,196,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,4,1,120,132,0,0,1,180,1,35,1,0,1,2,1,0,1,176,1,177,1,192,1,1,1,9,1,255,1,9,1,168,1,64,133,0,0,1,65,1,194,1,0,1,0,1,9,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,128,1,0,1,0,1,200,1,8,1,0,1,2,1,1,1,198,1,177,1,177,1,237,131,0,0,1,168,1,64,1,2,132,0,0,1,128,1,194,1,0,1,0,1,9,1,200,1,3,131,0,0,1,110,1,25,1,0,1,224,1,2,1,2,1,0,1,184,1,64,134,0,0,1,194,1,0,1,0,1,255,1,101,1,24,1,32,1,1,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,101,1,40,1,16,1,1,1,15,1,31,1,255,1,223,1,0,1,0,1,64,1,0,1,101,1,8,1,48,1,1,1,15,1,31,1,240,1,139,2,0,0,3,64,0,101,4,56,0,1,15,5,31,254,209,0,0,6,64,0,36,135,4,4,7,0,195,192,108,224,3,0,3,128,200,14,131,0,8,1,188,177,139,1,255,255,200,9,2,0,5,0,62,62,0,111,5,10,4,0,200,4,0,5,0,62,62,0,11,111,6,4,0,44,17,0,5,0,62,62,7,177,111,4,4,0,200,1,131,0,12,108,177,108,139,0,254,254,200,7,0,6,2,13,192,192,0,160,5,7,0,200,7,0,5,0,101,14,192,0,161,6,8,0,200,14,0,5,1,252,65,252,15,171,6,8,5,200,1,0,5,0,21,21,0,240,5,5,16,0,88,24,5,3,0,108,198,108,166,128,255,133,200,7,0,17,7,0,201,108,0,225,5,5,0,200,7,0,5,0,205,190,0,18,225,7,6,0,200,7,0,8,1,180,101,192,235,7,6,5,196,18,19,5,5,0,190,190,108,240,8,8,0,88,39,5,6,0,98,108,177,225,20,7,5,133,192,30,0,5,0,252,177,108,225,8,5,0,200,7,0,6,0,21,21,108,192,235,5,0,6,200,7,0,5,0,21,108,0,225,5,5,0,200,7,22,0,5,1,98,108,192,235,7,0,5,200,7,0,5,0,192,27,0,225,5,1,0,23,200,7,0,1,0,192,198,192,235,6,1,5,200,7,0,3,0,192,108,192,235,1,3,24,4,200,1,128,62,0,62,62,0,111,0,3,0,200,2,128,62,0,62,62,0,111,1,3,25,0,200,4,128,62,0,62,62,0,111,2,3,0,200,8,128,62,0,62,62,0,111,3,3,0,9,200,3,128,0,0,26,26,0,226,131,0,4,200,15,128,1,132,0,3,226,2,2,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 worldSpaceYAxis'</summary>
		public Microsoft.Xna.Framework.Vector3 WorldSpaceYAxis
		{
			set
			{
				this.SetWorldSpaceYAxis(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc2;
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[10];
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
			if ((DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.cid1))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid3))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid4))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid4))
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
	
	/// <summary><para>Technique 'DrawBillboardParticles_GpuTex3D' generated from file 'Billboard3D.fx'</para><para>Vertex Shader: approximately 56 instruction slots used (4 texture, 52 arithmetic), 10 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticles_GpuTex3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticles_GpuTex3D' shader</summary>
		public DrawBillboardParticles_GpuTex3D()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawBillboardParticles_GpuTex3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticles_GpuTex3D.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticles_GpuTex3D.cid1 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawBillboardParticles_GpuTex3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticles_GpuTex3D.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticles_GpuTex3D.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticles_GpuTex3D.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticles_GpuTex3D.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticles_GpuTex3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticles_GpuTex3D.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[7], ref this.sc0));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[4], ref this.vreg[5], ref this.vreg[6], ref unused, ref this.sc1));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc2));
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
				DrawBillboardParticles_GpuTex3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticles_GpuTex3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticles_GpuTex3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticles_GpuTex3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticles_GpuTex3D.fx, DrawBillboardParticles_GpuTex3D.fxb, 51, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticles_GpuTex3D.vin[i]));
			index = DrawBillboardParticles_GpuTex3D.vin[(i + 1)];
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
				return new byte[] {68,8,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,95,247,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,253,107,252,193,159,20,191,54,222,167,255,255,79,218,206,188,175,224,254,95,253,252,122,244,255,223,255,178,249,253,167,244,243,55,250,53,4,239,223,170,131,248,175,175,109,154,29,215,230,96,168,205,174,107,243,251,198,218,172,4,14,190,250,205,180,157,255,252,90,244,255,223,52,242,185,1,5,24,175,231,217,140,40,255,107,252,186,250,57,62,195,28,224,157,223,206,123,103,159,254,255,210,251,251,255,160,255,255,102,30,78,41,253,190,239,253,253,148,126,255,189,189,191,255,34,253,29,63,254,44,253,29,248,253,41,250,125,73,159,205,233,255,127,170,254,253,135,209,239,127,144,215,206,60,255,55,61,22,44,126,249,181,255,239,255,251,255,250,191,127,215,95,227,228,205,241,19,240,216,151,191,134,124,134,175,148,231,210,111,211,63,59,191,134,140,235,215,36,104,7,250,186,210,239,175,253,117,136,202,191,38,255,39,15,125,124,239,247,223,249,53,190,40,166,117,213,84,231,109,186,245,234,78,250,237,231,175,159,167,66,173,244,164,90,172,10,98,216,244,225,120,239,211,241,195,251,123,227,189,131,253,253,95,227,119,33,84,137,142,127,16,245,244,39,201,239,191,38,13,225,55,229,223,9,236,159,244,107,252,6,191,233,95,244,132,209,248,77,169,205,127,70,127,255,103,127,209,175,203,127,255,250,248,155,218,254,103,127,18,225,240,107,253,154,191,198,111,78,191,255,223,127,18,127,247,27,152,239,126,141,63,72,254,254,13,232,239,255,155,255,254,191,255,111,69,248,215,16,186,152,223,255,38,76,222,175,253,127,17,93,126,127,75,151,207,127,77,249,12,52,48,116,249,61,49,71,191,134,32,151,208,191,127,0,253,92,253,26,50,175,134,86,191,157,18,101,139,63,3,141,126,189,95,99,71,63,19,94,255,107,255,218,95,147,191,249,117,88,86,255,191,240,168,252,245,230,93,101,174,247,249,229,215,228,135,159,32,177,74,126,141,223,244,47,250,67,127,213,71,71,4,230,247,248,119,127,211,127,250,247,252,119,127,211,179,127,8,159,255,134,244,57,62,163,105,252,7,165,151,63,232,247,80,62,249,131,126,141,14,207,152,223,127,77,247,59,218,252,71,134,223,136,254,250,251,175,249,7,253,90,244,57,77,255,175,131,207,126,141,95,227,191,254,147,126,67,18,61,252,255,215,226,41,253,53,255,160,31,251,53,254,26,244,43,188,69,127,3,214,143,209,247,191,37,195,253,181,240,247,31,251,27,243,119,191,14,127,71,255,255,131,127,11,134,135,182,127,13,253,255,43,252,255,15,246,218,255,65,6,182,180,255,234,15,6,236,95,83,191,251,177,95,227,171,191,232,215,228,182,191,230,31,4,60,126,29,86,45,191,54,125,254,159,1,15,240,252,31,36,159,253,70,12,251,55,252,53,254,52,194,247,111,251,139,126,127,134,1,57,249,223,254,32,200,201,239,207,125,200,223,191,38,253,45,239,160,207,255,251,15,34,190,251,139,18,234,231,183,212,126,0,215,125,255,107,208,247,127,13,125,255,127,255,69,191,59,125,255,107,81,223,238,251,223,148,250,255,221,168,207,127,144,250,252,111,254,34,162,195,175,253,107,51,222,192,233,63,211,191,127,29,254,251,215,181,127,255,154,252,247,175,71,127,203,184,127,253,63,232,215,166,191,127,253,95,227,95,249,139,127,93,254,94,228,245,55,248,53,254,105,198,81,254,254,87,232,239,127,229,47,162,118,127,240,239,70,56,252,58,250,142,223,254,215,249,53,254,105,197,73,218,255,58,244,127,211,222,180,49,186,130,250,229,247,77,123,194,149,62,195,188,252,103,76,251,95,235,215,248,141,185,205,175,243,107,252,105,220,230,215,210,254,208,230,215,34,250,249,58,135,230,244,79,50,112,126,45,254,251,171,63,233,215,240,96,139,238,249,191,121,158,100,30,127,3,162,215,255,205,180,32,250,254,71,2,227,63,211,191,127,173,255,72,218,155,191,127,29,254,251,215,178,127,255,6,252,55,225,194,60,1,94,5,44,232,177,255,39,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 worldSpaceYAxis'</summary>
		public Microsoft.Xna.Framework.Vector3 WorldSpaceYAxis
		{
			set
			{
				this.SetWorldSpaceYAxis(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc2;
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[10];
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
			if ((DrawBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D.cid1))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticlesColour_GpuTex3D' generated from file 'Billboard3D.fx'</para><para>Vertex Shader: approximately 58 instruction slots used (6 texture, 52 arithmetic), 10 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticlesColour_GpuTex3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticlesColour_GpuTex3D' shader</summary>
		public DrawBillboardParticlesColour_GpuTex3D()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawBillboardParticlesColour_GpuTex3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticlesColour_GpuTex3D.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticlesColour_GpuTex3D.cid1 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawBillboardParticlesColour_GpuTex3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticlesColour_GpuTex3D.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawBillboardParticlesColour_GpuTex3D.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticlesColour_GpuTex3D.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticlesColour_GpuTex3D.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticlesColour_GpuTex3D.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawBillboardParticlesColour_GpuTex3D.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticlesColour_GpuTex3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticlesColour_GpuTex3D.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[7], ref this.sc0));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[4], ref this.vreg[5], ref this.vreg[6], ref unused, ref this.sc1));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc2));
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
				DrawBillboardParticlesColour_GpuTex3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticlesColour_GpuTex3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticlesColour_GpuTex3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticlesColour_GpuTex3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticlesColour_GpuTex3D.fx, DrawBillboardParticlesColour_GpuTex3D.fxb, 53, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticlesColour_GpuTex3D.vin[i]));
			index = DrawBillboardParticlesColour_GpuTex3D.vin[(i + 1)];
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
				return new byte[] {180,8,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,191,244,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,253,107,252,193,159,20,191,54,222,167,255,255,79,218,206,188,175,224,254,95,253,252,122,244,255,223,255,178,249,253,167,244,243,55,250,53,4,239,223,170,131,248,175,175,109,154,29,215,230,96,168,205,174,107,243,251,14,181,217,115,109,254,160,88,155,149,244,133,175,126,51,109,231,63,191,22,253,255,55,141,124,110,64,1,198,235,121,54,163,217,249,117,126,61,253,252,55,160,255,99,158,240,206,111,231,189,179,79,255,127,233,253,253,127,208,255,127,51,15,167,148,126,223,247,254,126,74,191,255,222,222,223,115,250,253,103,188,191,255,49,253,29,63,254,62,253,29,248,254,41,250,253,159,68,159,253,113,244,255,63,85,255,254,171,232,247,191,204,107,103,158,255,155,30,11,22,191,252,218,255,247,255,253,127,253,223,191,235,175,113,242,230,248,9,248,242,203,95,67,62,195,87,202,167,233,183,233,159,157,95,67,198,249,107,18,180,3,125,93,233,249,215,254,58,68,245,95,147,255,147,135,62,190,247,251,239,252,26,95,20,211,186,106,170,243,54,221,122,117,39,253,246,243,215,207,83,161,94,122,82,45,86,5,49,121,250,112,188,247,233,248,225,253,189,241,222,193,254,254,175,241,187,16,170,191,46,77,29,245,244,39,201,239,191,38,77,227,111,202,191,19,216,63,233,215,248,13,126,211,191,232,9,163,241,155,82,155,255,140,254,254,207,254,162,95,151,255,254,245,241,55,181,253,207,64,134,95,235,215,252,53,126,115,250,253,255,254,147,248,187,223,192,124,247,107,252,65,242,247,111,64,127,255,223,252,247,255,253,127,43,194,191,134,208,197,252,254,223,97,50,127,237,255,139,232,242,211,150,46,205,175,41,159,121,242,155,150,244,89,249,107,8,114,9,253,219,210,207,63,140,254,255,187,253,154,142,86,35,37,202,99,254,12,52,250,245,120,174,241,188,225,207,126,45,250,44,177,60,45,50,243,215,254,181,191,38,183,254,117,88,230,255,191,240,168,28,247,120,65,101,55,254,249,94,255,243,203,175,201,59,63,241,107,252,26,191,110,242,107,252,166,127,209,31,250,171,62,58,34,48,191,199,191,251,155,254,211,191,231,191,251,155,158,253,67,248,252,55,164,207,241,25,77,249,63,40,189,252,65,191,135,242,212,31,244,107,116,248,203,252,254,107,122,191,255,90,238,119,180,255,143,12,159,210,28,233,239,191,230,31,244,107,209,231,191,22,79,251,175,249,7,253,216,175,241,215,160,63,225,63,250,27,239,253,216,175,241,107,252,69,191,37,195,248,181,240,247,31,251,27,243,119,191,14,127,71,255,255,131,127,11,250,227,215,228,182,127,13,253,255,43,252,255,15,246,218,255,65,6,182,180,255,234,15,6,236,95,83,191,251,177,95,227,171,191,232,215,228,182,191,230,31,244,27,82,63,191,14,171,163,95,155,62,255,207,128,7,228,226,15,146,207,126,35,134,253,27,254,26,127,218,95,244,27,254,26,127,219,95,244,251,83,35,224,253,107,252,26,255,219,31,244,107,146,44,253,58,140,195,175,77,127,255,215,127,18,224,224,255,191,63,247,3,121,251,223,254,32,200,219,239,207,120,200,223,191,150,190,35,120,253,223,127,16,241,234,95,148,16,46,191,165,226,130,190,221,247,191,6,125,255,215,208,247,255,247,95,244,187,211,247,191,22,225,231,190,255,77,9,199,223,141,240,250,7,169,207,255,230,47,34,90,253,218,191,54,143,13,120,255,103,250,247,175,195,127,255,186,246,239,95,147,255,254,245,232,111,161,205,175,255,7,253,218,244,247,175,255,107,252,43,127,241,175,203,223,139,220,255,6,191,198,63,205,56,202,223,255,10,253,253,175,252,69,212,238,15,254,221,8,135,95,71,223,241,219,255,58,191,198,63,173,56,73,251,95,135,254,111,218,155,54,70,231,80,191,252,190,105,79,184,210,103,152,187,255,140,231,231,215,250,53,126,99,110,243,235,252,26,127,26,183,249,181,180,63,180,249,181,136,126,190,238,162,121,255,147,12,156,95,139,255,254,234,79,250,53,60,216,162,195,254,111,158,75,153,235,223,128,232,245,127,51,45,136,190,255,145,192,248,207,244,239,95,235,63,146,246,230,239,95,135,255,254,181,236,223,191,1,255,77,184,252,69,208,129,255,79,0,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 worldSpaceYAxis'</summary>
		public Microsoft.Xna.Framework.Vector3 WorldSpaceYAxis
		{
			set
			{
				this.SetWorldSpaceYAxis(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc2;
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[10];
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
			if ((DrawBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.cid1))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticles_GpuTex3D_UserOffset' generated from file 'Billboard3D.fx'</para><para>Vertex Shader: approximately 59 instruction slots used (6 texture, 53 arithmetic), 10 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticles_GpuTex3D_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticles_GpuTex3D_UserOffset' shader</summary>
		public DrawBillboardParticles_GpuTex3D_UserOffset()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawBillboardParticles_GpuTex3D_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticles_GpuTex3D_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticles_GpuTex3D_UserOffset.cid1 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawBillboardParticles_GpuTex3D_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticles_GpuTex3D_UserOffset.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticles_GpuTex3D_UserOffset.sid2 = state.GetNameUniqueID("UserSampler");
			DrawBillboardParticles_GpuTex3D_UserOffset.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticles_GpuTex3D_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticles_GpuTex3D_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticles_GpuTex3D_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawBillboardParticles_GpuTex3D_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticles_GpuTex3D_UserOffset.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[7], ref this.sc0));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[4], ref this.vreg[5], ref this.vreg[6], ref unused, ref this.sc1));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc2));
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
				DrawBillboardParticles_GpuTex3D_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticles_GpuTex3D_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticles_GpuTex3D_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticles_GpuTex3D_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticles_GpuTex3D_UserOffset.fx, DrawBillboardParticles_GpuTex3D_UserOffset.fxb, 54, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticles_GpuTex3D_UserOffset.vin[i]));
			index = DrawBillboardParticles_GpuTex3D_UserOffset.vin[(i + 1)];
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
				return new byte[] {200,8,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,191,244,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,253,107,252,193,159,20,191,54,222,167,255,255,79,218,206,188,175,224,254,95,253,252,122,244,255,223,255,178,249,253,167,244,243,55,250,53,4,239,223,170,131,248,175,175,109,154,29,215,230,96,168,205,174,107,243,251,14,181,217,115,109,254,160,88,155,21,247,245,235,224,171,223,76,219,249,207,175,69,255,255,77,35,159,27,80,128,241,122,158,205,104,118,126,157,95,79,63,255,13,232,255,152,39,188,243,219,121,239,236,211,255,95,122,127,255,31,244,255,223,204,195,41,165,223,247,189,191,159,210,239,191,183,247,247,156,126,255,25,239,239,127,76,127,199,143,191,79,127,7,190,127,138,126,255,39,209,103,127,28,253,255,79,213,191,255,42,250,253,47,243,218,153,231,255,166,199,130,197,47,191,246,255,253,127,255,95,255,247,239,250,107,156,188,57,126,2,190,252,242,215,144,207,240,149,242,105,250,109,250,103,231,215,144,113,254,154,4,237,64,95,87,122,254,181,191,14,81,253,215,228,255,228,161,143,239,253,254,59,191,198,23,197,180,174,154,234,188,77,183,94,221,73,191,253,252,245,243,84,168,151,158,84,139,85,65,76,158,62,28,239,125,58,126,120,127,111,188,119,176,191,255,107,252,46,132,234,175,75,83,71,61,253,73,242,251,175,73,211,248,155,242,239,4,246,79,250,53,126,131,223,244,47,122,194,104,252,166,212,230,63,163,191,255,179,191,232,215,229,191,127,125,252,77,109,255,51,144,225,215,250,53,127,141,223,156,126,255,191,255,36,254,238,55,48,223,253,26,127,144,252,253,27,208,223,255,55,255,253,127,255,223,138,240,175,33,116,177,127,80,195,95,227,215,254,191,136,46,63,109,233,210,252,154,242,153,39,191,105,73,159,149,191,134,32,151,208,191,45,253,252,195,232,255,191,219,175,233,104,53,82,162,60,230,207,64,163,95,143,231,26,207,27,254,236,215,162,207,18,203,211,34,51,127,237,95,251,107,114,235,95,135,101,254,255,11,143,202,113,143,23,84,118,227,159,239,245,63,191,252,154,188,243,19,52,107,201,175,241,155,254,69,127,232,175,250,232,136,192,252,30,255,238,111,250,79,255,158,255,238,111,122,246,15,225,243,223,144,62,199,103,52,229,255,160,244,242,7,253,30,202,83,127,208,175,209,225,47,243,251,175,233,253,254,107,185,223,209,254,63,50,124,74,115,164,191,255,154,127,208,175,69,159,19,111,252,58,248,236,215,248,53,254,235,63,233,55,252,53,126,141,191,8,255,255,181,152,21,126,205,63,232,199,126,141,191,6,56,8,79,210,223,128,245,99,244,253,111,201,112,127,45,252,253,199,254,198,252,221,175,195,223,209,255,255,224,223,130,225,161,237,95,67,255,255,10,255,255,131,189,246,127,144,129,45,237,191,250,131,1,251,215,212,239,126,236,215,248,234,47,250,53,185,237,175,249,7,1,143,95,135,85,212,175,77,159,255,103,192,3,178,242,7,201,103,191,17,195,254,13,127,141,63,141,240,253,219,254,162,223,159,97,64,190,254,183,63,232,215,34,249,194,223,191,150,254,253,107,232,223,191,134,254,253,107,210,223,2,3,56,252,223,127,16,241,234,95,148,80,191,191,165,246,139,126,220,247,191,6,125,255,215,208,247,255,247,95,244,187,211,247,191,54,225,226,198,240,235,19,62,191,138,126,103,57,165,143,126,3,194,231,255,254,139,126,204,142,7,184,254,103,250,247,175,195,127,255,186,246,239,95,147,255,254,245,232,239,95,139,255,254,245,121,108,191,254,175,241,175,252,197,191,46,227,46,127,255,6,191,198,63,205,184,202,223,255,10,253,253,175,80,251,255,236,15,254,221,168,191,95,135,62,251,181,24,166,107,255,235,252,26,255,52,227,254,107,106,251,95,135,254,111,218,155,54,191,174,246,247,107,243,56,254,51,219,158,112,165,207,190,66,27,158,147,95,251,215,248,141,185,205,175,243,107,252,105,127,144,193,225,215,210,54,191,54,209,209,192,1,76,154,235,63,201,192,249,181,248,239,175,254,36,55,87,210,230,215,34,90,171,78,11,104,69,116,254,143,126,13,126,231,63,211,191,127,173,255,72,245,162,254,253,235,240,223,191,150,253,251,55,224,191,127,109,250,251,215,228,113,253,166,255,17,96,65,47,254,63,1,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 worldSpaceYAxis'</summary>
		public Microsoft.Xna.Framework.Vector3 WorldSpaceYAxis
		{
			set
			{
				this.SetWorldSpaceYAxis(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc2;
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[10];
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
			if ((DrawBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.cid1))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.sid2))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticles_GpuTex3D_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticlesColour_GpuTex3D_UserOffset' generated from file 'Billboard3D.fx'</para><para>Vertex Shader: approximately 61 instruction slots used (8 texture, 53 arithmetic), 10 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticlesColour_GpuTex3D_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticlesColour_GpuTex3D_UserOffset' shader</summary>
		public DrawBillboardParticlesColour_GpuTex3D_UserOffset()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
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
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.cid1 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid3 = state.GetNameUniqueID("UserSampler");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid4 = state.GetNameUniqueID("VelocitySampler");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[7], ref this.sc0));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[4], ref this.vreg[5], ref this.vreg[6], ref unused, ref this.sc1));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc2));
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
				DrawBillboardParticlesColour_GpuTex3D_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticlesColour_GpuTex3D_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticlesColour_GpuTex3D_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticlesColour_GpuTex3D_UserOffset.fx, DrawBillboardParticlesColour_GpuTex3D_UserOffset.fxb, 56, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticlesColour_GpuTex3D_UserOffset.vin[i]));
			index = DrawBillboardParticlesColour_GpuTex3D_UserOffset.vin[(i + 1)];
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
				return new byte[] {248,8,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,63,246,107,202,239,191,54,254,166,255,255,67,250,93,66,255,255,117,244,179,255,183,63,191,30,253,255,247,191,108,126,255,41,253,252,141,126,13,193,251,63,235,180,249,245,181,77,179,227,218,252,6,157,193,217,54,187,174,205,104,168,205,158,107,243,114,168,205,61,215,166,141,181,89,49,62,255,55,190,250,205,180,157,255,252,90,244,255,223,52,242,185,1,5,24,175,231,217,44,175,133,6,248,252,55,248,53,100,46,241,78,234,189,243,207,209,255,255,35,239,239,255,9,109,60,156,126,43,250,125,203,251,251,128,126,127,238,253,253,251,210,239,43,239,239,191,75,127,199,143,191,73,127,7,190,127,138,126,255,135,209,103,127,16,253,255,79,213,191,255,60,250,253,207,242,218,153,231,255,166,199,130,197,47,191,246,255,253,127,255,95,255,247,239,250,107,156,188,57,126,242,59,209,159,95,254,26,242,25,190,250,157,184,209,175,145,126,155,254,217,249,53,12,207,254,90,191,198,129,190,174,244,252,107,127,29,162,250,175,201,255,201,67,31,223,251,253,119,126,141,47,138,105,93,53,213,121,155,110,189,186,147,126,251,249,235,231,169,80,47,61,169,22,171,162,164,95,30,142,247,62,29,63,188,191,55,222,59,216,223,255,53,126,23,66,245,215,165,97,80,79,127,146,252,254,107,210,144,126,83,254,157,192,254,73,191,198,111,240,155,254,69,79,24,141,223,148,218,252,103,244,247,127,246,23,253,186,252,247,175,143,191,169,237,127,246,39,17,14,191,214,175,249,107,252,230,244,251,255,253,39,241,119,191,129,249,238,215,248,131,228,239,223,128,254,254,191,249,239,255,251,255,86,132,127,13,161,139,249,253,49,53,252,53,126,237,255,139,232,178,182,116,249,11,127,77,249,12,95,25,186,128,190,4,140,145,75,232,223,63,138,126,254,89,191,134,204,165,161,213,239,169,68,121,201,159,129,70,191,222,175,241,123,235,103,115,254,236,215,162,207,18,59,215,127,16,127,134,150,191,201,175,241,71,233,103,34,107,127,237,95,251,107,242,231,191,14,235,138,255,47,60,42,255,61,254,80,153,143,127,190,55,240,249,189,254,231,151,95,147,207,126,226,215,248,53,126,221,228,215,248,77,255,34,2,241,123,16,197,255,65,129,246,7,253,30,248,252,55,164,207,255,208,95,245,209,17,190,251,119,127,211,127,250,247,252,119,127,211,179,127,72,249,239,15,250,53,58,188,104,126,255,53,189,223,127,45,239,247,95,219,253,142,119,255,35,195,223,52,143,250,251,175,249,7,253,90,244,249,175,197,236,242,107,254,65,63,246,107,252,53,192,73,248,150,254,198,123,63,246,107,252,26,127,209,111,201,48,126,45,252,253,199,254,198,252,221,175,195,223,209,255,255,224,223,130,213,10,218,254,53,244,255,175,240,255,63,216,107,255,7,25,216,210,254,171,63,24,176,127,77,253,238,199,126,141,175,254,162,95,147,219,254,154,127,16,241,212,95,244,235,176,26,251,181,233,243,255,12,120,16,158,255,217,31,36,159,253,70,12,59,249,53,254,180,191,40,249,53,254,182,191,232,247,167,70,192,251,215,248,53,254,183,63,232,215,36,25,252,117,24,135,95,155,254,254,175,255,36,192,193,255,127,127,238,7,114,250,191,253,65,191,54,181,209,119,248,239,95,67,255,254,53,244,239,95,75,97,8,158,255,247,31,244,27,210,251,191,33,225,246,91,42,110,192,197,125,255,107,208,247,127,13,125,255,127,255,69,191,59,125,79,178,226,141,243,215,39,156,127,21,253,206,242,78,31,253,6,132,243,255,253,23,253,152,29,51,198,243,159,233,223,191,14,255,253,235,218,191,127,77,254,251,215,163,191,127,45,254,251,215,231,241,255,250,191,198,191,242,23,255,186,140,187,252,253,27,252,26,255,52,227,42,127,255,43,244,247,191,66,237,255,179,63,248,119,163,254,126,29,250,236,215,98,152,174,253,175,243,107,252,211,127,144,208,71,218,255,58,244,127,211,222,180,249,117,181,191,95,155,199,241,159,217,246,132,43,125,246,21,218,240,188,253,218,191,198,111,204,109,126,157,95,227,79,251,131,12,14,191,150,182,249,181,137,142,6,14,96,18,63,252,73,6,206,175,197,127,127,245,39,185,249,148,54,191,22,209,90,117,99,64,43,162,243,127,132,207,127,13,165,13,125,253,31,169,126,213,191,127,29,254,251,215,178,127,255,6,252,247,175,77,127,67,167,254,63,1,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 worldSpaceYAxis'</summary>
		public Microsoft.Xna.Framework.Vector3 WorldSpaceYAxis
		{
			set
			{
				this.SetWorldSpaceYAxis(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc2;
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[10];
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
			if ((DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.cid1))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid3))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.sid4))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawBillboardParticlesColour_GpuTex3D_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticles_BillboardCpu3D' generated from file 'Billboard3D.fx'</para><para>Vertex Shader: approximately 44 instruction slots used, 159 registers</para><para>Pixel Shader: approximately 6 instruction slots used (1 texture, 5 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticles_BillboardCpu3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticles_BillboardCpu3D' shader</summary>
		public DrawBillboardParticles_BillboardCpu3D()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawBillboardParticles_BillboardCpu3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticles_BillboardCpu3D.cid0 = state.GetNameUniqueID("positionData");
			DrawBillboardParticles_BillboardCpu3D.cid1 = state.GetNameUniqueID("velocityData");
			DrawBillboardParticles_BillboardCpu3D.cid2 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawBillboardParticles_BillboardCpu3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticles_BillboardCpu3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticles_BillboardCpu3D.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[157], ref this.sc0));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[154], ref this.vreg[155], ref this.vreg[156], ref unused, ref this.sc1));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[150], ref this.vreg[151], ref this.vreg[152], ref this.vreg[153], ref this.sc2));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawBillboardParticles_BillboardCpu3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticles_BillboardCpu3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticles_BillboardCpu3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticles_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticles_BillboardCpu3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticles_BillboardCpu3D.fx, DrawBillboardParticles_BillboardCpu3D.fxb, 38, 9);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticles_BillboardCpu3D.vin[i]));
			index = DrawBillboardParticles_BillboardCpu3D.vin[(i + 1)];
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
				return new byte[] {96,25,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,31,150,200,239,191,54,254,166,255,255,102,250,247,95,72,255,255,117,244,179,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,255,255,126,126,61,250,255,239,127,217,252,254,83,250,249,27,253,26,226,7,238,171,95,104,158,95,31,109,86,205,239,223,236,168,223,168,237,252,231,215,162,255,255,166,145,207,197,167,252,191,255,111,192,120,61,207,102,121,45,109,241,57,218,254,218,250,51,245,222,249,157,168,255,29,15,135,119,250,59,222,89,233,239,128,241,167,232,247,79,233,179,223,147,254,255,167,234,223,191,47,253,254,123,123,237,204,243,127,211,99,124,220,223,8,191,252,90,255,247,255,253,127,253,223,191,235,175,113,242,230,248,201,239,68,127,126,249,107,200,103,248,234,119,226,86,191,70,250,109,250,103,231,215,48,62,243,175,245,107,28,232,251,74,143,191,246,215,33,170,253,154,252,159,60,244,241,222,239,191,243,107,124,81,76,235,170,169,206,219,116,235,213,157,244,219,207,95,63,79,101,244,233,73,181,88,21,37,253,242,112,188,247,233,248,225,253,189,241,222,193,254,254,175,241,187,8,170,127,16,245,244,55,153,223,127,205,95,227,55,53,191,255,73,191,198,111,240,155,254,69,79,24,141,223,148,218,252,103,127,211,175,241,27,252,103,127,209,175,203,127,255,250,248,155,218,254,103,127,19,225,240,107,253,154,191,198,111,71,191,255,223,127,19,190,251,181,236,119,255,247,31,36,127,255,6,244,247,255,205,127,163,45,193,252,131,126,45,250,254,255,254,191,21,249,95,67,104,100,126,255,237,126,99,250,231,215,250,191,136,70,127,254,175,101,104,244,123,36,242,153,79,163,131,68,104,4,68,255,66,165,209,183,127,13,195,87,127,237,95,251,107,18,154,191,38,205,50,226,140,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,255,255,127,46,191,166,79,252,19,191,198,175,241,235,254,133,191,198,111,250,23,17,136,223,131,220,225,223,3,176,254,208,95,245,209,17,62,255,139,232,243,127,247,55,253,167,127,207,127,247,55,61,251,135,76,63,248,252,47,166,207,127,205,223,248,95,250,219,179,223,240,239,248,59,255,218,191,230,91,135,127,244,31,245,71,61,196,231,127,9,125,254,215,254,53,127,205,223,71,205,254,1,133,245,123,56,159,251,55,253,147,126,29,14,67,126,237,255,232,215,248,53,254,235,63,137,252,212,191,8,255,255,45,249,251,95,147,190,255,53,254,164,95,139,125,109,254,253,15,198,223,99,249,238,111,250,53,216,79,199,239,191,233,31,244,23,254,26,255,217,95,68,64,126,221,95,19,62,122,250,187,1,111,250,254,79,167,119,126,250,15,250,49,246,191,127,45,248,233,127,208,159,67,237,228,239,95,135,255,254,115,237,223,191,38,255,253,231,209,223,232,239,215,36,255,29,254,249,159,255,107,252,43,127,177,241,231,241,253,95,240,107,252,211,232,231,215,145,191,255,21,250,251,95,161,246,255,217,31,252,187,17,30,191,182,190,227,183,255,181,127,141,127,250,15,66,251,95,83,219,255,218,244,127,211,222,180,1,222,24,223,239,149,254,223,138,247,255,141,177,254,65,30,13,254,32,161,145,252,78,141,254,162,191,232,215,248,234,47,250,221,25,238,175,205,159,253,197,132,247,95,242,107,184,184,4,120,252,154,246,189,95,159,250,197,223,95,81,219,255,236,15,254,117,121,124,191,49,125,246,167,217,54,102,188,104,243,107,254,26,255,219,31,100,224,32,134,249,53,126,141,191,70,231,72,240,253,53,126,141,175,254,164,95,67,240,254,245,108,27,131,123,250,159,241,79,153,151,223,128,230,229,43,166,47,225,254,15,161,221,159,161,244,166,175,249,239,63,211,254,253,235,240,223,127,150,253,251,55,224,191,255,108,250,91,98,171,223,244,63,2,44,196,75,255,79,0,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'positionData'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float4 positionData[75]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
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
						> 75)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 75)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 positionData[75]'</summary>
		public Microsoft.Xna.Framework.Vector4[] PositionData
		{
			set
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'velocityData'</summary>
		private static int cid1;
		/// <summary>Set the shader array value 'float4 velocityData[75]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
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
						> 75)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 75)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 75)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 velocityData[75]'</summary>
		public Microsoft.Xna.Framework.Vector4[] VelocityData
		{
			set
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid2;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[158] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 worldSpaceYAxis'</summary>
		public Microsoft.Xna.Framework.Vector3 WorldSpaceYAxis
		{
			set
			{
				this.SetWorldSpaceYAxis(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc2;
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[159];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawBillboardParticles_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_BillboardCpu3D.cid2))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector4[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector4[] value)
		{
			if ((DrawBillboardParticles_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_BillboardCpu3D.cid0))
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawBillboardParticles_BillboardCpu3D.cid1))
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticles_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_BillboardCpu3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticles_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticles_BillboardCpu3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawBillboardParticlesColour_BillboardCpu3D' generated from file 'Billboard3D.fx'</para><para>Vertex Shader: approximately 44 instruction slots used, 234 registers</para><para>Pixel Shader: approximately 6 instruction slots used (1 texture, 5 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawBillboardParticlesColour_BillboardCpu3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawBillboardParticlesColour_BillboardCpu3D' shader</summary>
		public DrawBillboardParticlesColour_BillboardCpu3D()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(197));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawBillboardParticlesColour_BillboardCpu3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawBillboardParticlesColour_BillboardCpu3D.cid0 = state.GetNameUniqueID("colourData");
			DrawBillboardParticlesColour_BillboardCpu3D.cid1 = state.GetNameUniqueID("positionData");
			DrawBillboardParticlesColour_BillboardCpu3D.cid2 = state.GetNameUniqueID("velocityData");
			DrawBillboardParticlesColour_BillboardCpu3D.cid3 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawBillboardParticlesColour_BillboardCpu3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawBillboardParticlesColour_BillboardCpu3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawBillboardParticlesColour_BillboardCpu3D.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[232], ref this.sc0));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[229], ref this.vreg[230], ref this.vreg[231], ref unused, ref this.sc1));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[225], ref this.vreg[226], ref this.vreg[227], ref this.vreg[228], ref this.sc2));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawBillboardParticlesColour_BillboardCpu3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawBillboardParticlesColour_BillboardCpu3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawBillboardParticlesColour_BillboardCpu3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawBillboardParticlesColour_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawBillboardParticlesColour_BillboardCpu3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawBillboardParticlesColour_BillboardCpu3D.fx, DrawBillboardParticlesColour_BillboardCpu3D.fxb, 38, 9);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawBillboardParticlesColour_BillboardCpu3D.vin[i]));
			index = DrawBillboardParticlesColour_BillboardCpu3D.vin[(i + 1)];
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
				return new byte[] {196,34,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,254,111,42,191,255,218,248,155,254,255,15,253,38,242,247,127,67,255,255,117,244,179,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,255,13,207,175,71,255,255,253,47,155,223,127,74,63,127,163,95,67,226,214,255,76,227,88,243,252,250,104,179,106,126,255,102,71,98,218,223,76,219,249,207,175,69,255,71,56,220,253,92,98,224,255,251,255,6,140,215,243,108,150,215,210,22,159,163,237,175,173,63,83,239,157,127,142,250,255,143,60,28,182,52,206,198,59,169,254,14,24,127,138,126,255,203,169,237,255,68,255,255,83,245,239,223,136,218,252,6,94,59,243,252,223,244,152,152,252,55,194,47,191,214,255,253,127,255,95,255,247,239,250,107,156,188,57,126,242,59,209,159,95,254,26,242,25,190,250,157,184,213,175,145,126,155,254,217,249,53,76,140,255,107,253,26,7,250,190,210,227,175,253,117,136,106,191,38,255,39,15,125,188,247,251,239,252,26,95,20,211,186,106,170,243,54,221,122,117,39,253,246,243,215,207,83,25,125,122,82,45,86,69,73,191,60,28,239,125,58,126,120,127,111,188,119,176,191,255,107,252,46,130,234,31,68,61,253,77,230,247,95,243,215,248,77,205,239,127,18,13,232,47,122,194,104,252,166,212,230,63,251,155,126,141,223,224,63,251,139,126,93,254,251,215,199,223,212,246,63,251,155,8,135,95,235,215,252,53,126,59,250,253,255,254,155,240,221,175,101,191,251,191,255,32,249,251,55,160,191,255,111,254,27,109,9,230,31,244,107,209,247,255,247,255,173,200,255,26,66,35,243,251,63,247,155,211,63,191,214,255,69,52,250,167,127,109,67,163,255,241,55,145,207,124,26,253,87,191,137,208,8,136,254,55,74,163,111,255,26,134,175,254,218,191,246,215,36,52,127,77,154,101,228,69,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,255,183,60,151,145,24,254,54,49,252,79,252,26,191,198,175,251,223,252,26,191,233,95,68,32,126,15,10,223,127,15,192,250,67,127,213,71,71,248,252,191,165,207,255,221,223,244,159,254,61,255,221,223,244,236,31,50,253,224,243,255,142,62,255,53,127,227,127,233,111,207,126,195,191,227,239,252,107,255,154,111,29,254,209,127,212,31,245,16,159,255,247,244,249,95,251,215,252,53,127,31,53,251,7,20,214,239,225,114,4,191,233,159,244,235,112,218,228,215,254,143,126,141,95,227,191,254,147,40,174,254,139,240,255,223,146,191,255,53,233,251,95,227,79,250,181,56,55,192,191,255,193,248,123,44,223,253,77,191,6,231,21,240,251,111,250,7,253,55,191,198,127,246,23,17,144,95,247,215,68,78,33,253,221,128,55,125,255,167,211,59,63,253,7,253,24,231,11,126,45,228,21,254,160,255,156,218,201,223,191,14,255,253,95,216,191,127,77,254,251,191,164,191,209,223,175,73,249,6,228,19,254,171,95,227,95,249,139,77,254,1,223,255,215,191,198,63,141,126,126,29,249,251,95,161,191,255,21,106,255,159,253,193,191,27,225,241,107,235,59,126,251,95,251,215,248,167,255,32,180,255,53,181,253,175,77,255,55,237,77,27,224,141,241,253,94,233,255,173,120,255,223,24,235,31,228,209,224,15,18,26,201,239,255,45,209,231,191,253,53,190,250,139,126,119,134,251,107,243,103,255,29,225,253,223,255,26,46,143,2,60,126,77,251,222,175,79,253,226,239,175,168,237,127,246,7,255,186,60,190,223,152,62,251,211,108,27,51,94,180,249,53,127,141,255,237,15,50,112,144,115,249,53,126,141,191,70,231,72,240,253,53,126,141,175,254,164,95,67,240,254,245,108,27,131,123,250,159,241,207,95,147,251,248,77,255,163,63,195,253,77,99,249,13,104,158,190,98,122,211,88,254,33,188,247,31,43,253,233,107,254,251,63,177,127,255,58,252,247,127,106,255,254,13,248,239,255,140,254,70,142,231,255,9,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'colourData'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float4 colourData[75]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
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
						> 75)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 75)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 150)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 colourData[75]'</summary>
		public Microsoft.Xna.Framework.Vector4[] ColourData
		{
			set
			{
				this.SetColourData(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'positionData'</summary>
		private static int cid1;
		/// <summary>Set the shader array value 'float4 positionData[75]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
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
						> 75)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 75)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 positionData[75]'</summary>
		public Microsoft.Xna.Framework.Vector4[] PositionData
		{
			set
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'velocityData'</summary>
		private static int cid2;
		/// <summary>Set the shader array value 'float4 velocityData[75]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
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
						> 75)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 75)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 75)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 velocityData[75]'</summary>
		public Microsoft.Xna.Framework.Vector4[] VelocityData
		{
			set
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid3;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[233] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float3 worldSpaceYAxis'</summary>
		public Microsoft.Xna.Framework.Vector3 WorldSpaceYAxis
		{
			set
			{
				this.SetWorldSpaceYAxis(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc2;
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[234];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawBillboardParticlesColour_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_BillboardCpu3D.cid3))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector4[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector4[] value)
		{
			if ((DrawBillboardParticlesColour_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_BillboardCpu3D.cid0))
			{
				this.SetColourData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawBillboardParticlesColour_BillboardCpu3D.cid1))
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawBillboardParticlesColour_BillboardCpu3D.cid2))
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawBillboardParticlesColour_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_BillboardCpu3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawBillboardParticlesColour_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawBillboardParticlesColour_BillboardCpu3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
}
#endif
