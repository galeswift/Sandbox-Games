// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = VelocityBillboard3D.fx
// Namespace = Xen.Ex.Graphics.Display

#if XBOX360
namespace Xen.Ex.Graphics.Display
{
	
	/// <summary><para>Technique 'DrawVelocityBillboardParticles_GpuTex3D' generated from file 'VelocityBillboard3D.fx'</para><para>Vertex Shader: approximately 52 instruction slots used (4 texture, 48 arithmetic), 11 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityBillboardParticles_GpuTex3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityBillboardParticles_GpuTex3D' shader</summary>
		public DrawVelocityBillboardParticles_GpuTex3D()
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
			DrawVelocityBillboardParticles_GpuTex3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityBillboardParticles_GpuTex3D.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawVelocityBillboardParticles_GpuTex3D.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityBillboardParticles_GpuTex3D.cid2 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawVelocityBillboardParticles_GpuTex3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityBillboardParticles_GpuTex3D.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityBillboardParticles_GpuTex3D.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityBillboardParticles_GpuTex3D.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityBillboardParticles_GpuTex3D.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityBillboardParticles_GpuTex3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityBillboardParticles_GpuTex3D.gd))
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
				DrawVelocityBillboardParticles_GpuTex3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityBillboardParticles_GpuTex3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityBillboardParticles_GpuTex3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityBillboardParticles_GpuTex3D.fx, DrawVelocityBillboardParticles_GpuTex3D.fxb, 55, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityBillboardParticles_GpuTex3D.vin[i]));
			index = DrawVelocityBillboardParticles_GpuTex3D.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,184,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,4,1,0,0,1,136,0,1,11,131,0,1,4,131,0,1,1,229,0,0,206,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,36,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,72,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,108,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,5,131,0,0,1,1,131,0,0,1,7,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,138,0,0,1,1,1,12,1,0,1,0,1,1,1,32,138,0,0,1,1,1,48,1,0,1,0,1,1,1,68,138,0,0,1,1,1,84,1,0,1,0,1,1,1,104,138,0,0,1,1,1,172,135,0,0,1,1,1,0,1,0,1,1,1,168,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,124,1,0,1,0,1,1,1,120,131,0,0,1,93,134,0,0,1,1,1,148,1,0,1,0,1,1,1,144,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,20,1,14,131,0,0,1,252,1,252,1,27,1,225,1,2,1,1,1,2,1,12,1,135,1,128,1,0,1,0,1,21,1,108,1,108,1,225,151,0,0,132,255,0,138,0,0,1,4,1,72,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,224,1,0,1,0,1,2,1,104,135,0,0,1,36,1,0,1,0,1,1,1,124,1,0,1,0,1,1,1,164,138,0,0,1,1,1,84,131,0,0,1,28,1,0,1,0,1,1,1,70,1,255,1,254,1,3,132,0,0,1,3,131,0,0,1,28,134,0,0,1,1,1,63,131,0,0,1,88,1,0,1,2,131,0,0,1,11,133,0,0,1,96,131,0,0,1,112,1,0,1,0,1,1,1,32,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,40,134,0,0,1,1,1,56,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,1,40,132,0,0,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,11,229,0,0,209,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,2,1,40,1,0,1,17,1,0,1,7,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,44,1,0,1,0,1,16,1,43,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,64,1,11,1,18,1,0,1,18,1,0,1,0,1,80,132,0,0,1,96,1,15,1,194,1,0,1,18,133,0,0,1,96,1,21,1,96,1,27,1,18,1,0,1,18,133,0,0,1,96,1,33,1,64,1,39,1,18,1,0,1,18,135,0,0,1,32,1,43,1,196,1,0,1,34,131,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,10,1,255,1,10,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,10,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,10,1,200,1,3,1,0,1,1,1,0,1,196,1,179,1,0,1,224,1,1,1,1,1,0,1,188,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,255,1,33,1,8,1,48,1,33,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,33,1,24,1,32,1,33,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,2,1,0,1,1,1,0,1,27,1,177,1,108,1,139,1,2,1,8,1,8,1,200,1,7,1,0,1,5,1,0,1,192,1,198,1,0,1,167,1,2,1,255,1,0,1,200,1,15,1,0,1,4,1,0,1,70,1,190,1,108,1,76,1,255,1,3,1,255,1,200,1,1,1,0,1,1,1,0,1,62,1,0,1,0,1,111,1,5,1,4,1,0,1,200,1,4,1,0,1,1,1,0,1,62,1,0,1,0,1,111,1,4,1,4,1,0,1,200,1,2,131,0,0,1,64,1,0,1,0,1,243,1,5,1,0,1,0,1,48,2,40,0,3,1,0,62,4,0,177,111,6,5,4,0,184,39,0,6,5,2,20,205,65,128,7,1,7,255,200,7,0,4,8,0,177,205,205,171,0,9,2,9,168,18,1,0,0,98,98,130,208,10,5,5,255,88,40,0,1,0,190,190,11,177,240,2,2,128,168,71,1,7,0,192,12,177,131,193,5,0,255,160,130,1,0,0,98,13,190,27,240,7,2,129,20,7,0,2,1,180,205,14,177,225,7,4,128,200,7,0,2,1,205,180,192,235,15,7,4,2,4,130,1,0,0,192,192,27,240,2,2,1,16,88,34,0,1,0,27,177,177,225,1,1,128,180,23,2,6,17,0,180,177,65,193,2,0,255,180,39,2,4,0,98,27,128,193,18,6,0,255,180,71,2,5,0,98,205,130,193,7,6,255,200,7,0,19,1,1,180,180,192,235,7,6,5,36,135,1,1,0,192,108,108,225,1,4,2,128,200,7,131,0,20,192,198,192,235,1,0,4,200,7,0,1,0,192,27,192,235,0,3,3,200,21,1,128,62,0,62,62,0,111,0,1,0,200,2,128,62,0,62,62,0,111,1,22,1,0,200,4,128,62,0,62,62,0,111,2,1,0,200,8,128,62,0,62,62,0,8,111,3,1,0,36,254,192,1,131,0,16,108,226,0,0,128,200,3,128,0,0,197,197,0,226,2,2,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[10] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid2;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[11];
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
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.cid2))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityBillboardParticlesColour_GpuTex3D' generated from file 'VelocityBillboard3D.fx'</para><para>Vertex Shader: approximately 54 instruction slots used (6 texture, 48 arithmetic), 11 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityBillboardParticlesColour_GpuTex3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityBillboardParticlesColour_GpuTex3D' shader</summary>
		public DrawVelocityBillboardParticlesColour_GpuTex3D()
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
			DrawVelocityBillboardParticlesColour_GpuTex3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityBillboardParticlesColour_GpuTex3D.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawVelocityBillboardParticlesColour_GpuTex3D.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityBillboardParticlesColour_GpuTex3D.cid2 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawVelocityBillboardParticlesColour_GpuTex3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityBillboardParticlesColour_GpuTex3D.gd))
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
				DrawVelocityBillboardParticlesColour_GpuTex3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityBillboardParticlesColour_GpuTex3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityBillboardParticlesColour_GpuTex3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityBillboardParticlesColour_GpuTex3D.fx, DrawVelocityBillboardParticlesColour_GpuTex3D.fxb, 57, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityBillboardParticlesColour_GpuTex3D.vin[i]));
			index = DrawVelocityBillboardParticlesColour_GpuTex3D.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,220,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,4,1,0,0,1,136,0,1,11,131,0,1,4,131,0,1,1,229,0,0,206,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,36,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,72,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,108,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,144,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,138,0,0,1,1,1,12,1,0,1,0,1,1,1,32,138,0,0,1,1,1,48,1,0,1,0,1,1,1,68,138,0,0,1,1,1,84,1,0,1,0,1,1,1,104,138,0,0,1,1,1,120,1,0,1,0,1,1,1,140,138,0,0,1,1,1,208,135,0,0,1,1,1,0,1,0,1,1,1,204,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,160,1,0,1,0,1,1,1,156,131,0,0,1,93,134,0,0,1,1,1,184,1,0,1,0,1,1,1,180,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,20,1,14,131,0,0,1,252,1,252,1,27,1,225,1,2,1,1,1,2,1,12,1,135,1,128,1,0,1,0,1,21,1,108,1,108,1,225,151,0,0,132,255,0,138,0,0,1,4,1,112,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,252,1,0,1,0,1,2,1,116,135,0,0,1,36,1,0,1,0,1,1,1,152,1,0,1,0,1,1,1,192,138,0,0,1,1,1,112,131,0,0,1,28,1,0,1,0,1,1,1,97,1,255,1,254,1,3,132,0,0,1,4,131,0,0,1,28,134,0,0,1,1,1,90,131,0,0,1,108,1,0,1,2,131,0,0,1,11,133,0,0,1,116,131,0,0,1,132,1,0,1,0,1,1,1,52,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,60,134,0,0,1,1,1,76,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,1,60,134,0,0,1,1,1,83,1,0,1,3,1,0,1,2,1,0,1,1,132,0,0,1,1,1,60,132,0,0,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,11,229,0,0,209,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,2,1,52,1,0,1,17,1,0,1,8,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,44,1,0,1,0,1,16,1,45,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,80,1,11,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,16,1,194,1,0,1,18,133,0,0,1,96,1,22,1,96,1,28,1,18,1,0,1,18,133,0,0,1,96,1,34,1,64,1,40,1,18,1,0,1,18,135,0,0,1,32,1,44,1,196,1,0,1,34,131,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,10,1,255,1,10,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,10,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,10,1,200,1,3,1,0,1,1,1,0,1,196,1,179,1,0,1,224,1,1,1,1,1,0,1,188,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,255,1,33,1,24,1,32,1,33,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,33,1,8,1,64,1,33,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,33,1,40,1,48,1,33,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,2,1,0,1,1,1,0,1,27,1,177,1,108,1,139,1,3,1,8,1,8,1,200,1,7,1,0,1,6,1,0,1,192,1,198,1,0,1,167,1,3,1,255,1,0,1,200,1,15,1,0,1,5,1,0,1,70,1,190,1,108,1,76,1,255,1,4,1,255,1,200,1,1,1,0,1,1,1,0,1,62,1,0,1,0,1,111,1,5,1,5,1,0,1,200,2,4,0,3,1,0,62,4,0,0,111,4,4,5,0,200,2,131,0,5,64,0,0,243,6,6,0,0,48,40,0,1,7,0,62,0,177,111,6,5,8,0,184,39,0,6,2,20,205,9,65,128,1,7,255,200,7,0,5,10,0,177,205,205,171,0,9,3,168,18,11,1,0,0,98,98,130,208,6,6,255,88,12,40,0,1,0,190,190,177,240,3,3,128,168,13,71,1,8,0,192,177,131,193,6,0,255,160,130,14,1,0,0,98,190,27,240,8,3,129,20,7,0,3,15,1,180,205,177,225,8,5,128,200,7,0,3,1,205,180,16,192,235,8,5,3,4,130,1,0,0,192,192,27,240,3,3,17,1,88,34,0,1,0,27,177,177,225,1,1,128,180,23,3,7,18,0,180,177,65,193,3,0,255,180,39,3,5,0,98,27,128,193,7,19,0,255,180,71,3,6,0,98,205,130,193,8,7,255,200,7,0,1,1,20,180,180,192,235,8,7,6,36,135,1,1,0,192,108,108,225,1,3,128,200,1,7,131,0,21,192,198,192,235,1,0,5,200,7,0,1,0,192,27,192,235,0,4,4,200,1,22,128,62,0,62,62,0,111,0,1,0,200,2,128,62,0,62,62,0,111,1,1,0,23,200,4,128,62,0,62,62,0,111,2,1,0,200,8,128,62,0,62,62,0,111,3,1,17,0,200,3,128,0,0,197,197,0,226,3,3,0,200,15,128,1,132,0,3,226,2,2,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[10] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid2;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[11];
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
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.cid2))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityBillboardParticles_GpuTex3D_UserOffset' generated from file 'VelocityBillboard3D.fx'</para><para>Vertex Shader: approximately 55 instruction slots used (6 texture, 49 arithmetic), 11 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityBillboardParticles_GpuTex3D_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityBillboardParticles_GpuTex3D_UserOffset' shader</summary>
		public DrawVelocityBillboardParticles_GpuTex3D_UserOffset()
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
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid2 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid2 = state.GetNameUniqueID("UserSampler");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd))
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
				DrawVelocityBillboardParticles_GpuTex3D_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityBillboardParticles_GpuTex3D_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityBillboardParticles_GpuTex3D_UserOffset.fx, DrawVelocityBillboardParticles_GpuTex3D_UserOffset.fxb, 58, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityBillboardParticles_GpuTex3D_UserOffset.vin[i]));
			index = DrawVelocityBillboardParticles_GpuTex3D_UserOffset.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,220,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,4,1,0,0,1,136,0,1,11,131,0,1,4,131,0,1,1,229,0,0,206,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,36,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,72,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,108,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,144,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,138,0,0,1,1,1,12,1,0,1,0,1,1,1,32,138,0,0,1,1,1,48,1,0,1,0,1,1,1,68,138,0,0,1,1,1,84,1,0,1,0,1,1,1,104,138,0,0,1,1,1,120,1,0,1,0,1,1,1,140,138,0,0,1,1,1,208,135,0,0,1,1,1,0,1,0,1,1,1,204,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,160,1,0,1,0,1,1,1,156,131,0,0,1,93,134,0,0,1,1,1,184,1,0,1,0,1,1,1,180,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,20,1,14,131,0,0,1,252,1,252,1,27,1,225,1,2,1,1,1,2,1,12,1,135,1,128,1,0,1,0,1,21,1,108,1,108,1,225,151,0,0,132,255,0,138,0,0,1,4,1,112,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,252,1,0,1,0,1,2,1,116,135,0,0,1,36,1,0,1,0,1,1,1,152,1,0,1,0,1,1,1,192,138,0,0,1,1,1,112,131,0,0,1,28,1,0,1,0,1,1,1,97,1,255,1,254,1,3,132,0,0,1,4,131,0,0,1,28,134,0,0,1,1,1,90,131,0,0,1,108,1,0,1,2,131,0,0,1,11,133,0,0,1,116,131,0,0,1,132,1,0,1,0,1,1,1,52,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,60,134,0,0,1,1,1,76,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,1,60,134,0,0,1,1,1,83,1,0,1,3,1,0,1,2,1,0,1,1,132,0,0,1,1,1,60,132,0,0,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,11,229,0,0,209,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,2,1,52,1,0,1,17,1,0,1,8,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,45,1,0,1,0,1,16,1,44,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,80,1,11,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,16,1,194,1,0,1,18,133,0,0,1,96,1,22,1,96,1,28,1,18,1,0,1,18,133,0,0,1,96,1,34,1,64,1,40,1,18,1,0,1,18,135,0,0,1,32,1,44,1,196,1,0,1,34,131,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,10,1,255,1,10,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,10,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,10,1,200,1,3,1,0,1,1,1,0,1,196,1,179,1,0,1,224,1,1,1,1,1,0,1,188,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,255,1,33,1,8,1,64,1,33,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,33,1,40,1,48,1,33,1,15,1,31,1,254,1,209,1,0,1,0,1,64,1,0,1,33,1,24,1,32,1,33,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,2,1,0,1,1,1,0,1,27,1,177,1,108,1,139,1,2,1,8,1,8,1,200,1,13,1,0,1,1,1,0,1,240,1,198,1,0,1,167,1,2,1,255,1,0,1,36,1,130,1,3,1,0,1,0,1,84,1,0,1,108,1,243,1,1,1,0,1,128,1,48,1,39,1,0,1,3,1,0,1,192,1,192,1,177,1,224,1,4,1,3,1,0,1,200,2,4,0,3,1,0,62,4,62,0,111,4,5,3,0,200,8,0,6,1,0,62,62,0,111,7,6,3,0,184,33,0,1,8,0,62,62,65,79,5,3,255,9,200,7,0,5,0,177,205,205,171,10,0,9,2,200,7,0,6,2,20,205,11,0,160,1,7,0,168,18,1,0,0,98,12,98,130,208,6,6,255,88,40,0,1,0,190,13,190,177,240,2,2,128,168,71,1,8,0,192,177,14,131,193,6,0,255,160,130,1,0,0,98,190,27,240,15,8,2,129,20,7,0,2,1,180,205,177,225,8,5,128,16,200,7,0,2,1,205,180,192,235,8,5,2,4,130,1,0,17,0,192,192,27,240,2,2,1,88,34,0,1,0,27,177,177,225,18,1,1,128,180,23,2,7,0,180,177,65,193,2,0,255,180,39,2,19,5,0,98,27,128,193,7,0,255,180,71,2,6,0,98,205,130,193,8,20,7,255,200,7,0,1,1,180,180,192,235,8,7,6,36,135,1,1,0,192,8,108,108,225,1,2,128,200,7,131,0,21,192,198,192,235,1,0,5,200,7,0,1,0,192,27,192,235,0,4,3,200,1,22,128,62,0,62,62,0,111,0,1,0,200,2,128,62,0,62,62,0,111,1,1,0,23,200,4,128,62,0,62,62,0,111,2,1,0,200,8,128,62,0,62,62,0,111,3,1,5,0,36,254,192,1,131,0,16,108,226,0,0,128,200,3,128,0,0,197,197,0,226,2,2,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[10] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid2;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[11];
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
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid2))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid2))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset' generated from file 'VelocityBillboard3D.fx'</para><para>Vertex Shader: approximately 57 instruction slots used (8 texture, 49 arithmetic), 11 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset' shader</summary>
		public DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset()
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
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid2 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid3 = state.GetNameUniqueID("UserSampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid4 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd))
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
				DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.fx, DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.fxb, 60, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.vin[i]));
			index = DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,208,135,0,1,3,131,0,1,1,131,0,1,208,135,0,1,11,131,0,1,4,131,0,1,1,229,0,0,206,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,244,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,24,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,60,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,96,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,51,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,132,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,6,131,0,0,1,1,131,0,0,1,8,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,220,131,0,0,1,240,138,0,0,1,1,131,0,0,1,1,1,20,138,0,0,1,1,1,36,1,0,1,0,1,1,1,56,138,0,0,1,1,1,72,1,0,1,0,1,1,1,92,138,0,0,1,1,1,108,1,0,1,0,1,1,1,128,138,0,0,1,1,1,196,135,0,0,1,1,1,0,1,0,1,1,1,192,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,148,1,0,1,0,1,1,1,144,131,0,0,1,93,134,0,0,1,1,1,172,1,0,1,0,1,1,1,168,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,0,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,84,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,84,1,16,1,0,1,2,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,32,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,22,1,16,133,0,0,1,27,1,226,1,0,1,0,1,1,1,20,1,14,131,0,0,1,252,1,252,1,27,1,225,1,2,1,1,1,2,1,12,1,135,1,128,1,0,1,0,1,21,1,108,1,108,1,225,151,0,0,132,255,0,138,0,0,1,4,1,148,1,16,1,42,1,17,1,1,1,0,1,0,1,2,1,20,1,0,1,0,1,2,1,128,135,0,0,1,36,1,0,1,0,1,1,1,176,1,0,1,0,1,1,1,216,138,0,0,1,1,1,136,131,0,0,1,28,1,0,1,0,1,1,1,124,1,255,1,254,1,3,132,0,0,1,5,131,0,0,1,28,134,0,0,1,1,1,117,131,0,0,1,128,1,0,1,2,131,0,0,1,11,133,0,0,1,136,131,0,0,1,152,1,0,1,0,1,1,1,72,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,80,134,0,0,1,1,1,96,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,1,80,134,0,0,1,1,1,103,1,0,1,3,1,0,1,2,1,0,1,1,132,0,0,1,1,1,80,134,0,0,1,1,1,110,1,0,1,3,1,0,1,3,1,0,1,1,132,0,0,1,1,1,80,132,0,0,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,2,0,1,3,0,4,0,1,11,229,0,0,209,0,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,95,1,118,1,115,1,95,1,115,1,51,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,136,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,2,1,64,1,0,1,17,1,0,1,9,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,1,131,0,0,1,2,1,0,1,0,1,2,1,144,131,0,0,1,5,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,45,1,0,1,0,1,16,1,46,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,5,1,96,1,11,1,18,1,0,1,18,1,0,1,5,1,80,132,0,0,1,96,1,17,1,194,1,0,1,18,133,0,0,1,96,1,23,1,96,1,29,1,18,1,0,1,18,133,0,0,1,96,1,35,1,64,1,41,1,18,1,0,1,18,135,0,0,1,32,1,45,1,196,1,0,1,34,131,0,0,1,5,1,248,132,0,0,1,4,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,10,1,255,1,10,1,168,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,10,1,52,1,18,131,0,0,1,198,1,0,1,198,1,232,1,129,1,0,1,1,1,200,1,8,1,0,1,1,1,1,1,198,1,177,1,177,1,237,1,1,1,0,1,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,10,1,200,1,3,1,0,1,1,1,0,1,196,1,179,1,0,1,224,1,1,1,1,1,0,1,188,1,64,1,1,132,0,0,1,65,1,194,1,0,1,0,1,255,1,33,1,24,1,32,1,33,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,33,1,8,1,80,1,33,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,33,1,56,1,64,1,33,1,15,1,31,1,254,1,209,1,0,1,0,1,64,1,0,1,33,1,40,1,48,1,33,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,2,1,0,1,1,1,0,1,27,1,177,1,108,1,139,1,3,1,8,1,8,1,200,1,13,1,0,1,1,1,0,1,240,1,198,2,0,167,3,3,255,0,4,36,130,4,0,5,0,84,0,108,243,6,1,0,128,48,39,0,7,4,0,192,192,177,224,5,8,4,0,200,4,0,1,0,62,9,62,0,111,4,4,0,200,8,0,10,1,0,62,62,0,111,6,4,0,184,11,33,0,1,0,62,62,65,79,5,4,255,12,200,7,0,6,0,177,205,205,171,0,9,3,13,200,7,0,7,2,20,205,0,160,1,7,0,168,14,18,1,0,0,98,98,130,208,7,7,255,88,40,0,15,1,0,190,190,177,240,3,3,128,168,71,1,9,0,192,16,177,131,193,7,0,255,160,130,1,0,0,98,190,27,240,9,17,3,129,20,7,0,3,1,180,205,177,225,9,6,128,200,7,0,18,3,1,205,180,192,235,9,6,3,4,130,1,0,0,192,192,27,240,19,3,3,1,88,34,0,1,0,27,177,177,225,1,1,128,180,23,3,8,20,0,180,177,65,193,3,0,255,180,39,3,6,0,98,27,128,193,8,0,255,21,180,71,3,7,0,98,205,130,193,9,8,255,200,7,0,1,1,180,180,192,235,17,9,8,7,36,135,1,1,0,192,108,108,225,1,3,128,200,7,131,0,22,192,198,192,235,1,0,6,200,7,0,1,0,192,27,192,235,0,5,4,200,1,128,23,62,0,62,62,0,111,0,1,0,200,2,128,62,0,62,62,0,111,1,1,0,200,4,24,128,62,0,62,62,0,111,2,1,0,200,8,128,62,0,62,62,0,111,3,1,0,200,3,14,128,0,0,197,197,0,226,3,3,0,200,15,128,1,132,0,3,226,2,2,140,0,1,0};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[10] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid2;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[11];
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
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid2))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid3))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid4))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid4))
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
	
	/// <summary><para>Technique 'DrawVelocityBillboardParticles_GpuTex3D' generated from file 'VelocityBillboard3D.fx'</para><para>Vertex Shader: approximately 52 instruction slots used (4 texture, 48 arithmetic), 11 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityBillboardParticles_GpuTex3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityBillboardParticles_GpuTex3D' shader</summary>
		public DrawVelocityBillboardParticles_GpuTex3D()
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
			DrawVelocityBillboardParticles_GpuTex3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityBillboardParticles_GpuTex3D.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawVelocityBillboardParticles_GpuTex3D.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityBillboardParticles_GpuTex3D.cid2 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawVelocityBillboardParticles_GpuTex3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityBillboardParticles_GpuTex3D.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityBillboardParticles_GpuTex3D.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityBillboardParticles_GpuTex3D.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityBillboardParticles_GpuTex3D.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityBillboardParticles_GpuTex3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityBillboardParticles_GpuTex3D.gd))
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
				DrawVelocityBillboardParticles_GpuTex3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityBillboardParticles_GpuTex3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityBillboardParticles_GpuTex3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityBillboardParticles_GpuTex3D.fx, DrawVelocityBillboardParticles_GpuTex3D.fxb, 55, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityBillboardParticles_GpuTex3D.vin[i]));
			index = DrawVelocityBillboardParticles_GpuTex3D.vin[(i + 1)];
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
				return new byte[] {152,8,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,223,247,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,253,107,252,15,235,167,191,54,222,255,53,204,63,191,198,175,241,27,234,251,250,231,255,167,158,95,143,254,255,251,95,54,191,255,148,126,254,70,191,134,140,227,119,235,12,228,215,215,54,205,142,107,243,237,161,54,187,174,77,25,107,179,18,56,248,234,55,211,118,254,243,107,209,255,127,211,200,231,6,20,96,188,158,103,51,154,137,95,227,215,213,207,241,25,230,4,239,252,118,222,59,251,244,255,151,222,223,191,17,53,78,61,156,118,232,247,167,222,223,111,232,247,185,247,247,223,164,191,227,199,95,165,191,3,191,63,69,191,255,25,250,236,29,253,255,79,213,191,255,52,250,253,79,242,218,153,231,255,166,199,130,197,47,191,246,255,253,127,255,95,255,247,239,250,107,156,188,57,126,2,158,251,242,215,144,207,240,149,242,96,250,109,250,103,231,215,144,113,253,154,4,237,64,95,87,250,253,181,191,14,81,249,215,228,255,228,161,143,239,253,254,59,191,198,23,197,180,174,154,234,188,77,183,94,221,73,191,253,252,245,243,84,168,149,158,84,139,85,65,12,156,62,28,239,125,58,126,120,127,111,188,119,176,191,255,107,252,46,132,42,209,241,15,162,158,254,36,249,253,215,252,131,126,205,95,227,55,229,223,9,236,159,244,107,252,6,191,233,95,244,132,209,248,77,169,205,127,70,127,255,103,127,209,175,203,127,255,250,248,155,218,254,103,24,242,175,245,107,254,26,191,57,253,254,127,255,73,252,221,111,96,190,251,53,254,32,249,251,55,160,191,255,111,254,251,255,254,191,21,225,95,67,232,98,126,255,229,152,188,95,251,255,34,186,76,45,93,190,251,107,202,103,160,129,161,203,75,250,236,247,254,53,4,185,223,144,254,253,3,232,231,10,95,252,154,142,86,91,74,148,3,254,12,52,250,245,126,141,223,83,63,19,94,255,107,255,218,95,147,191,249,117,88,118,255,191,248,168,60,246,248,64,101,176,247,249,229,215,228,143,159,32,49,251,13,127,141,223,244,47,34,16,191,7,77,223,63,40,208,254,160,223,67,249,227,15,250,53,58,188,98,126,255,53,221,239,104,243,31,25,62,35,186,235,239,191,230,31,244,107,209,231,52,237,191,14,62,251,53,126,141,255,250,79,162,185,248,139,240,255,95,139,167,242,215,252,131,146,95,227,175,65,191,194,83,244,55,96,145,174,254,139,126,75,134,251,107,225,239,63,246,55,230,239,126,29,254,142,254,255,7,255,22,12,15,109,255,26,250,255,87,248,255,31,236,181,255,131,12,108,105,255,213,31,12,216,191,166,126,151,252,26,95,253,69,191,38,183,253,53,255,32,224,241,235,176,74,249,181,233,243,255,12,120,128,215,255,32,249,236,55,98,216,191,225,175,241,167,17,190,127,219,95,244,251,51,12,200,199,255,246,7,65,62,126,127,238,67,254,254,53,233,111,188,67,99,165,247,127,55,122,231,31,164,119,254,155,191,232,199,168,205,175,77,253,254,90,12,243,63,211,191,127,29,254,251,215,181,127,255,154,252,247,175,71,127,3,239,95,139,100,238,215,166,191,127,253,95,227,95,249,139,127,3,254,251,55,224,239,127,45,254,236,215,248,181,228,239,255,155,101,14,109,229,187,255,251,15,50,109,33,147,248,255,111,100,97,255,223,127,48,190,247,223,253,245,188,223,127,45,109,71,131,249,131,127,195,95,227,255,230,113,252,218,140,195,175,241,7,253,216,175,241,79,19,77,254,105,238,235,215,209,190,128,155,105,243,107,253,26,255,10,253,253,79,99,108,127,240,239,70,48,127,29,197,253,215,101,152,210,254,215,225,239,93,251,95,135,223,249,207,254,96,3,243,215,97,124,255,154,63,9,248,255,26,140,239,63,205,248,203,28,96,206,48,182,175,254,162,223,192,227,25,224,77,115,245,199,73,27,59,207,127,16,240,55,122,235,215,102,184,191,134,194,17,93,69,109,254,164,95,199,194,54,250,235,255,230,57,23,158,248,13,24,198,143,73,63,255,145,210,82,255,254,181,254,35,105,111,254,254,117,248,239,95,203,254,253,27,240,223,212,47,243,23,248,30,176,160,11,255,159,0,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[10] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid2;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[11];
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
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.cid2))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityBillboardParticlesColour_GpuTex3D' generated from file 'VelocityBillboard3D.fx'</para><para>Vertex Shader: approximately 54 instruction slots used (6 texture, 48 arithmetic), 11 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityBillboardParticlesColour_GpuTex3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityBillboardParticlesColour_GpuTex3D' shader</summary>
		public DrawVelocityBillboardParticlesColour_GpuTex3D()
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
			DrawVelocityBillboardParticlesColour_GpuTex3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityBillboardParticlesColour_GpuTex3D.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawVelocityBillboardParticlesColour_GpuTex3D.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityBillboardParticlesColour_GpuTex3D.cid2 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawVelocityBillboardParticlesColour_GpuTex3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityBillboardParticlesColour_GpuTex3D.gd))
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
				DrawVelocityBillboardParticlesColour_GpuTex3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityBillboardParticlesColour_GpuTex3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityBillboardParticlesColour_GpuTex3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityBillboardParticlesColour_GpuTex3D.fx, DrawVelocityBillboardParticlesColour_GpuTex3D.fxb, 57, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityBillboardParticlesColour_GpuTex3D.vin[i]));
			index = DrawVelocityBillboardParticlesColour_GpuTex3D.vin[(i + 1)];
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
				return new byte[] {8,9,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,127,244,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,253,107,252,15,235,167,191,54,222,255,53,204,63,191,198,175,241,27,234,251,250,231,255,167,158,95,143,254,255,251,95,54,191,255,148,126,254,70,191,134,140,227,119,235,12,228,215,215,54,205,142,107,243,237,161,54,187,174,77,57,212,102,207,181,249,147,98,109,86,220,215,255,141,175,126,51,109,231,63,191,22,253,255,55,141,124,110,64,1,198,235,121,54,163,217,226,241,225,243,223,128,254,143,121,195,59,191,157,247,206,62,253,255,165,247,247,111,68,141,83,15,167,29,250,253,169,247,247,27,250,125,238,253,253,142,126,255,227,188,191,255,53,253,29,63,254,57,253,29,248,254,41,250,253,95,68,159,253,121,244,255,63,85,255,254,187,232,247,191,205,107,103,158,255,155,30,11,22,191,252,218,255,247,255,253,127,253,223,191,235,175,113,242,230,248,9,248,244,203,95,67,62,195,87,202,183,233,183,233,159,157,95,67,198,249,107,18,180,3,125,93,233,249,215,254,58,68,245,95,147,255,147,135,62,190,247,251,239,252,26,95,20,211,186,106,170,243,54,221,122,117,39,253,246,243,215,207,83,161,94,122,82,45,86,5,49,125,250,112,188,247,233,248,225,253,189,241,222,193,254,254,175,241,187,16,170,191,238,175,241,107,252,65,212,211,159,36,191,255,154,127,208,175,249,107,252,166,252,59,129,253,147,126,141,223,224,55,253,139,158,48,26,191,41,181,249,207,232,239,255,236,47,250,117,249,239,95,31,127,83,219,255,12,211,254,107,253,154,191,198,111,78,191,255,223,127,18,127,247,27,152,239,126,141,63,72,254,254,13,232,239,255,155,255,254,191,255,111,69,248,215,16,186,152,223,119,168,225,175,241,107,255,95,68,151,165,165,203,31,250,107,202,103,158,60,167,63,67,159,149,191,134,32,247,27,210,191,45,253,252,195,232,255,251,191,166,163,213,99,37,202,115,254,12,52,250,245,120,174,241,204,248,179,95,139,62,75,44,79,139,204,252,181,127,237,175,201,173,127,29,214,1,255,95,124,84,174,123,188,161,178,28,255,124,175,255,249,229,215,228,165,159,248,53,126,141,95,247,55,252,53,126,211,191,136,64,252,30,52,213,255,160,64,251,131,126,15,229,165,63,232,215,232,240,149,249,253,215,244,126,255,181,220,239,104,255,31,25,254,164,185,209,223,127,205,63,232,215,162,207,127,45,158,238,95,243,15,74,126,141,191,6,253,9,223,209,223,120,143,108,192,95,244,91,50,140,95,11,127,255,177,191,49,127,247,235,240,119,244,255,63,248,183,160,63,126,77,110,251,215,208,255,191,194,255,255,96,175,253,31,100,96,75,251,175,254,96,192,254,53,245,187,228,215,248,10,98,255,107,225,123,226,147,191,232,215,97,53,244,107,211,231,255,25,240,128,60,252,65,242,217,111,196,176,127,195,95,227,79,251,139,126,195,95,227,111,251,139,126,127,106,4,188,127,141,95,227,127,251,131,126,77,146,161,95,135,113,248,181,233,239,255,250,79,2,28,252,255,247,231,126,32,103,255,219,31,4,57,251,253,25,15,249,251,215,210,119,8,6,245,241,187,17,220,127,144,222,249,111,254,162,31,163,54,196,207,132,51,250,253,207,244,239,95,135,255,254,117,237,223,191,38,255,253,235,209,223,191,22,227,241,235,255,65,191,54,253,253,235,255,26,255,202,95,252,27,240,223,191,1,127,255,107,241,103,191,198,175,37,127,255,223,44,187,104,43,223,253,223,127,144,105,11,217,198,255,127,35,11,251,255,254,131,241,189,255,238,175,231,253,254,107,105,59,26,204,31,252,27,254,26,255,55,143,227,215,102,28,126,141,63,232,199,126,141,127,154,232,246,79,115,95,191,142,246,5,220,76,155,95,235,215,248,87,232,239,127,26,99,251,131,127,55,130,249,235,40,238,191,46,195,148,246,191,14,127,239,218,255,58,252,206,127,246,7,27,152,191,14,227,251,215,252,73,192,255,215,96,124,255,105,198,95,230,9,243,138,177,125,245,23,209,247,127,145,153,123,224,77,243,249,199,73,27,203,11,127,16,240,55,250,239,215,102,184,191,134,194,17,157,71,109,254,164,95,199,194,54,122,240,255,102,190,16,190,249,13,24,198,143,73,63,255,145,210,82,255,254,181,254,35,105,111,254,254,117,248,239,95,203,254,253,27,240,223,212,239,95,4,61,250,255,4,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[10] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid2;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[11];
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
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.cid2))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityBillboardParticles_GpuTex3D_UserOffset' generated from file 'VelocityBillboard3D.fx'</para><para>Vertex Shader: approximately 55 instruction slots used (6 texture, 49 arithmetic), 11 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityBillboardParticles_GpuTex3D_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityBillboardParticles_GpuTex3D_UserOffset' shader</summary>
		public DrawVelocityBillboardParticles_GpuTex3D_UserOffset()
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
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid2 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid2 = state.GetNameUniqueID("UserSampler");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd))
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
				DrawVelocityBillboardParticles_GpuTex3D_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityBillboardParticles_GpuTex3D_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityBillboardParticles_GpuTex3D_UserOffset.fx, DrawVelocityBillboardParticles_GpuTex3D_UserOffset.fxb, 58, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityBillboardParticles_GpuTex3D_UserOffset.vin[i]));
			index = DrawVelocityBillboardParticles_GpuTex3D_UserOffset.vin[(i + 1)];
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
				return new byte[] {28,9,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,127,244,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,253,107,252,15,235,167,191,54,222,255,53,204,63,191,198,175,241,27,234,251,250,231,255,167,158,95,143,254,255,251,95,54,191,255,148,126,254,70,191,134,140,227,119,235,12,228,215,215,54,205,142,107,243,237,161,54,187,174,77,57,212,102,207,181,249,147,98,109,86,220,215,255,141,175,126,51,109,231,63,191,22,253,255,55,141,124,110,64,1,198,235,121,54,163,217,226,241,225,243,223,128,254,143,121,195,59,191,157,247,206,62,253,255,165,247,247,111,68,141,83,15,167,29,250,253,169,247,247,27,250,125,238,253,253,142,126,255,227,188,191,255,53,253,29,63,254,57,253,29,248,254,41,250,253,95,68,159,253,121,244,255,63,85,255,254,187,232,247,191,205,107,103,158,255,155,30,11,22,191,252,218,255,247,255,253,127,253,223,191,235,175,113,242,230,248,9,248,244,203,95,67,62,195,87,202,183,233,183,233,159,157,95,67,198,249,107,18,180,3,125,93,233,249,215,254,58,68,245,95,147,255,147,135,62,190,247,251,239,252,26,95,20,211,186,106,170,243,54,221,122,117,39,253,246,243,215,207,83,161,94,122,82,45,86,5,49,125,250,112,188,247,233,248,225,253,189,241,222,193,254,254,175,241,187,16,170,191,238,175,241,107,252,65,212,211,159,36,191,255,154,127,208,175,249,107,252,166,252,59,129,253,147,126,141,223,224,55,253,139,158,48,26,191,41,181,249,207,232,239,255,236,47,250,117,249,239,95,31,127,83,219,255,12,211,254,107,253,154,191,198,111,78,191,255,223,127,18,127,247,27,152,239,126,141,63,72,254,254,13,232,239,255,155,255,254,191,255,111,69,248,215,16,186,152,223,159,82,195,95,227,215,254,191,136,46,75,75,151,63,244,215,148,207,60,121,78,127,134,62,43,127,13,65,238,55,164,127,91,250,249,135,209,255,247,127,77,71,171,199,74,148,231,252,25,104,244,235,241,92,227,153,241,103,191,22,125,150,88,158,22,153,249,107,255,218,95,147,91,255,58,172,3,254,191,248,168,92,247,120,67,101,57,254,249,94,255,243,203,175,201,75,63,241,107,252,26,191,238,111,248,107,252,166,127,17,129,248,61,104,170,255,65,129,246,7,253,30,202,75,127,208,175,209,225,43,243,251,175,233,253,254,107,185,223,209,254,63,50,252,73,115,163,191,255,154,127,208,175,69,159,19,79,252,58,248,236,215,248,53,254,235,63,137,230,235,47,194,255,127,45,102,129,95,243,15,74,126,141,191,6,56,8,47,210,223,128,69,118,225,47,250,45,25,238,175,133,191,255,216,223,152,191,251,117,248,59,250,255,31,252,91,48,60,180,253,107,232,255,95,225,255,127,176,215,254,15,50,176,165,253,87,127,48,96,255,154,250,93,242,107,124,5,85,240,107,225,123,224,241,235,176,106,250,181,233,243,255,12,120,64,70,254,32,249,236,55,98,216,191,225,175,241,167,17,190,127,219,95,244,251,51,12,200,213,255,246,7,253,90,36,87,248,251,215,210,191,127,13,253,251,215,208,191,127,77,250,251,215,226,246,191,62,193,251,85,132,15,203,23,201,221,111,64,240,254,239,191,232,199,248,221,95,75,251,250,207,244,239,95,135,255,254,117,237,223,191,38,255,253,235,253,26,2,235,215,34,88,128,243,235,255,26,255,202,95,252,27,48,236,223,128,255,150,207,4,54,201,52,203,176,105,139,191,77,91,200,56,254,79,250,255,215,254,181,25,246,255,253,7,227,123,255,221,95,207,251,253,215,210,118,52,168,63,24,56,131,38,191,54,193,165,191,255,160,31,251,53,254,105,162,213,63,205,125,253,58,218,215,175,173,116,251,181,249,239,127,133,254,254,167,49,182,63,248,119,35,152,191,14,191,247,159,113,251,95,91,219,255,58,252,189,107,255,235,240,59,255,217,31,108,96,254,58,140,239,95,243,39,1,127,204,213,175,69,237,127,13,237,67,230,25,99,251,234,47,250,13,60,94,2,222,52,135,127,156,180,177,243,207,52,55,122,240,215,102,184,191,134,194,17,221,71,109,254,164,95,199,194,150,207,126,45,214,129,50,111,191,134,55,111,4,247,63,82,90,234,223,191,214,127,36,250,211,252,253,235,252,71,242,190,249,251,55,224,191,169,95,230,59,200,3,96,65,183,254,63,1,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[10] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid2;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[11];
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
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.cid2))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid2))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityBillboardParticles_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_GpuTex3D_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset' generated from file 'VelocityBillboard3D.fx'</para><para>Vertex Shader: approximately 57 instruction slots used (8 texture, 49 arithmetic), 11 registers</para><para>Pixel Shader: approximately 5 instruction slots used (1 texture, 4 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset' shader</summary>
		public DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset()
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
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid0 = state.GetNameUniqueID("invTextureSizeOffset");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid2 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid1 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid2 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid3 = state.GetNameUniqueID("UserSampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid4 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd))
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
				DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.fx, DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.fxb, 60, 8);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.vin[i]));
			index = DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.vin[(i + 1)];
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
				return new byte[] {76,9,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,191,246,107,202,239,191,54,254,166,255,255,75,250,221,111,72,255,255,117,244,179,255,175,61,191,30,253,255,247,191,108,126,255,41,253,252,141,126,13,25,199,47,239,180,249,245,181,77,179,227,218,252,118,157,193,218,54,187,174,205,227,161,54,123,174,205,31,48,212,230,158,107,243,135,197,218,172,24,159,95,31,95,253,102,218,206,127,126,45,250,255,111,26,249,92,64,253,223,255,55,96,188,158,103,179,188,22,26,224,243,223,224,215,144,185,197,59,169,247,206,191,71,255,255,159,58,64,126,43,15,167,223,141,126,63,240,254,254,54,253,254,251,122,127,151,244,251,31,228,253,253,79,233,239,248,241,15,233,239,192,247,79,209,239,255,52,250,236,79,162,255,255,169,250,247,95,71,191,255,85,94,59,243,252,223,244,88,176,248,229,215,254,191,255,239,255,235,255,254,93,127,141,147,55,199,79,126,39,250,243,203,95,67,62,195,87,191,19,55,250,53,210,111,211,63,59,191,134,225,225,95,235,215,56,208,215,149,158,127,237,175,67,84,255,53,249,63,121,232,227,123,191,255,206,175,241,69,49,173,171,166,58,111,211,173,87,119,210,111,63,127,253,60,21,234,165,39,213,98,85,148,244,203,195,241,222,167,227,135,247,247,198,123,7,251,251,191,198,239,66,168,254,186,52,108,234,233,79,146,223,127,77,34,193,111,202,191,19,216,63,233,215,248,13,126,211,191,232,9,163,241,155,82,155,255,140,254,254,207,254,162,95,151,255,254,245,241,55,181,253,207,64,130,95,235,215,252,53,126,115,250,253,255,254,147,248,187,223,192,124,247,107,252,65,242,247,111,64,127,255,223,252,247,255,253,127,43,194,191,134,208,197,252,142,134,191,198,175,253,127,17,93,174,45,93,254,198,95,83,62,195,87,134,46,160,47,1,99,228,126,67,250,247,143,162,159,127,214,175,33,115,105,104,245,82,137,242,7,240,103,160,209,175,247,107,204,245,179,119,252,217,175,69,159,37,118,174,255,36,254,12,45,127,147,95,227,207,210,207,68,214,254,218,191,246,215,228,207,127,29,214,29,255,95,124,84,31,244,248,69,117,64,252,243,189,129,207,239,245,63,191,252,154,124,247,19,191,198,175,241,235,254,134,191,198,111,250,23,17,136,223,131,102,244,31,20,104,127,208,239,161,124,247,7,253,26,29,30,52,191,255,154,222,239,191,150,247,251,175,237,126,199,187,255,145,225,107,154,63,253,253,215,252,131,126,45,250,252,215,98,54,249,53,255,160,228,215,248,107,208,183,240,43,253,141,247,146,95,227,215,248,139,126,75,134,241,107,225,239,63,246,55,230,239,126,29,254,142,254,255,7,255,22,244,199,175,201,109,255,26,250,255,87,248,255,31,236,181,255,131,12,108,105,255,213,31,12,216,191,166,126,151,252,26,95,253,69,191,38,183,253,53,255,32,226,165,191,232,215,97,245,245,107,211,231,255,25,240,32,60,255,179,63,72,62,251,141,24,246,111,248,107,252,105,127,209,111,248,107,252,109,127,209,239,79,141,128,247,175,241,107,252,111,127,208,175,73,178,247,235,48,14,191,54,253,253,95,255,73,128,131,255,255,254,220,15,228,243,127,251,131,126,109,106,163,239,240,223,191,134,254,253,107,232,223,191,22,253,253,107,113,251,95,159,250,252,85,132,51,203,41,201,239,111,64,125,254,223,127,209,143,241,187,191,150,226,243,159,233,223,191,14,255,253,235,218,191,127,77,254,251,215,251,53,4,214,175,69,176,0,231,215,255,53,254,149,191,248,55,96,216,191,1,255,45,159,9,108,210,13,172,11,76,91,252,109,218,66,87,224,255,100,71,126,237,95,155,97,255,223,127,48,190,247,223,253,245,188,223,127,45,109,71,131,250,131,129,51,104,242,107,19,92,250,251,15,250,177,95,227,159,38,122,254,211,220,215,175,163,125,253,218,74,219,95,155,255,254,87,232,239,127,26,99,251,131,127,55,130,249,235,240,123,255,25,183,255,181,181,253,175,195,223,187,246,191,14,191,243,159,253,193,6,230,175,195,248,254,53,127,18,240,199,124,254,90,212,254,215,208,62,132,23,48,182,175,254,34,250,254,47,50,60,1,188,105,158,255,56,105,99,121,132,105,110,244,233,175,205,112,127,13,133,35,58,148,218,252,73,191,142,133,45,159,253,90,172,75,101,222,126,13,111,222,8,238,127,164,180,212,191,127,173,255,72,244,176,249,251,215,249,143,228,125,243,247,111,192,127,83,191,127,17,244,242,255,19,0,0,255,255};
			}
		}
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'invTextureSizeOffset'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float3 invTextureSizeOffset'</summary><param name="value"/>
		public void SetInvTextureSizeOffset(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[10] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[8] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid2;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[9] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[11];
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
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid0))
			{
				this.SetInvTextureSizeOffset(ref value);
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.cid2))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid1))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid2))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid3))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.sid4))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityBillboardParticles_BillboardCpu3D' generated from file 'VelocityBillboard3D.fx'</para><para>Vertex Shader: approximately 42 instruction slots used, 160 registers</para><para>Pixel Shader: approximately 6 instruction slots used (1 texture, 5 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityBillboardParticles_BillboardCpu3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityBillboardParticles_BillboardCpu3D' shader</summary>
		public DrawVelocityBillboardParticles_BillboardCpu3D()
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
			DrawVelocityBillboardParticles_BillboardCpu3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityBillboardParticles_BillboardCpu3D.cid0 = state.GetNameUniqueID("positionData");
			DrawVelocityBillboardParticles_BillboardCpu3D.cid1 = state.GetNameUniqueID("velocityData");
			DrawVelocityBillboardParticles_BillboardCpu3D.cid2 = state.GetNameUniqueID("velocityScale");
			DrawVelocityBillboardParticles_BillboardCpu3D.cid3 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawVelocityBillboardParticles_BillboardCpu3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityBillboardParticles_BillboardCpu3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityBillboardParticles_BillboardCpu3D.gd))
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
				DrawVelocityBillboardParticles_BillboardCpu3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityBillboardParticles_BillboardCpu3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityBillboardParticles_BillboardCpu3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityBillboardParticles_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityBillboardParticles_BillboardCpu3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityBillboardParticles_BillboardCpu3D.fx, DrawVelocityBillboardParticles_BillboardCpu3D.fxb, 40, 9);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityBillboardParticles_BillboardCpu3D.vin[i]));
			index = DrawVelocityBillboardParticles_BillboardCpu3D.vin[(i + 1)];
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
				return new byte[] {160,25,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,159,150,200,239,191,54,254,166,255,167,250,247,95,68,255,255,117,244,179,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,191,158,95,143,254,255,251,95,54,191,255,148,126,254,70,191,134,248,133,79,213,79,52,207,175,143,54,171,230,247,111,118,196,103,252,205,180,157,255,252,90,244,255,223,52,242,185,248,152,255,247,255,13,24,175,231,217,44,175,165,45,62,71,219,95,91,127,166,222,59,35,234,255,247,244,112,248,163,244,119,188,243,7,233,239,128,241,167,232,247,111,232,179,151,244,255,63,85,255,46,233,247,185,215,206,60,255,55,61,198,231,253,141,240,203,175,245,127,255,223,255,215,255,253,187,254,26,39,111,142,159,252,78,244,231,151,191,134,124,134,175,126,39,110,245,107,164,223,166,127,118,126,13,227,67,255,90,191,198,129,190,175,244,248,107,127,29,162,218,175,201,255,201,67,31,239,253,254,59,191,198,23,197,180,174,154,234,188,77,183,94,221,73,191,253,252,245,243,84,70,159,158,84,139,85,81,210,47,15,199,123,159,142,31,222,223,27,239,29,236,239,255,26,191,139,160,250,7,81,79,127,147,249,253,215,252,53,126,83,243,251,159,244,107,252,6,191,233,95,244,132,209,248,77,169,205,127,246,55,253,26,191,193,127,246,23,253,186,252,247,175,143,191,169,237,127,246,55,17,14,191,214,175,249,107,252,118,244,251,255,253,55,225,187,95,203,126,247,127,255,65,242,247,111,64,127,255,223,252,55,218,18,204,63,232,215,162,239,255,239,255,91,145,255,53,132,70,230,247,111,255,198,244,207,175,245,127,17,141,254,226,95,203,210,40,145,207,2,26,37,66,35,32,250,23,41,141,190,253,107,24,190,250,107,255,218,95,147,208,252,53,105,150,255,34,215,205,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,159,71,207,229,215,244,145,127,226,215,248,53,126,221,191,232,215,248,77,255,34,2,241,123,144,123,252,123,24,120,206,119,254,77,255,164,95,135,195,137,95,251,63,250,53,126,141,255,250,79,162,134,127,17,254,255,91,242,247,191,38,125,255,107,252,73,191,22,251,204,252,251,31,140,191,199,242,221,223,244,107,176,191,141,223,127,189,63,232,47,250,53,254,179,191,136,128,252,186,236,107,167,191,27,250,163,239,255,116,122,231,167,255,160,31,163,247,41,14,248,131,224,87,255,57,212,78,254,254,117,248,239,63,215,254,253,107,242,223,127,30,253,253,107,169,143,78,254,249,31,244,231,255,26,255,202,95,252,27,240,223,191,1,127,143,255,255,250,220,39,254,22,31,221,248,243,248,251,55,96,28,126,131,63,232,247,74,255,51,198,193,252,252,141,108,31,255,247,31,140,118,62,140,95,207,251,253,215,210,118,52,174,63,248,47,250,53,190,250,139,126,77,142,1,126,83,11,15,180,250,53,25,183,95,227,15,250,11,127,141,127,154,112,253,167,53,78,176,49,197,31,100,218,252,26,191,198,191,66,127,255,211,136,23,254,224,223,141,250,248,181,116,76,191,46,247,33,237,241,190,223,254,215,226,119,254,179,63,216,192,252,181,120,92,127,205,159,244,27,240,28,1,255,127,218,226,242,227,110,142,254,32,161,25,143,15,127,255,193,50,167,191,22,125,254,127,255,65,127,1,141,227,47,160,57,253,117,44,140,95,131,254,255,213,31,132,241,25,250,253,154,220,207,175,241,7,73,27,67,207,175,254,36,244,255,235,112,200,171,159,165,255,55,247,253,107,40,14,191,166,210,14,176,126,76,112,248,135,208,238,207,208,121,165,175,249,239,63,211,254,253,235,240,223,127,150,253,251,55,224,191,255,108,250,91,98,177,223,244,63,2,44,196,87,255,79,0,0,0,255,255};
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
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid2;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[158] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid3;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[159] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[160];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityBillboardParticles_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_BillboardCpu3D.cid2))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityBillboardParticles_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_BillboardCpu3D.cid3))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector4[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector4[] value)
		{
			if ((DrawVelocityBillboardParticles_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_BillboardCpu3D.cid0))
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawVelocityBillboardParticles_BillboardCpu3D.cid1))
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityBillboardParticles_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_BillboardCpu3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityBillboardParticles_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticles_BillboardCpu3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityBillboardParticlesColour_BillboardCpu3D' generated from file 'VelocityBillboard3D.fx'</para><para>Vertex Shader: approximately 42 instruction slots used, 235 registers</para><para>Pixel Shader: approximately 6 instruction slots used (1 texture, 5 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityBillboardParticlesColour_BillboardCpu3D : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityBillboardParticlesColour_BillboardCpu3D' shader</summary>
		public DrawVelocityBillboardParticlesColour_BillboardCpu3D()
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
			DrawVelocityBillboardParticlesColour_BillboardCpu3D.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityBillboardParticlesColour_BillboardCpu3D.cid0 = state.GetNameUniqueID("colourData");
			DrawVelocityBillboardParticlesColour_BillboardCpu3D.cid1 = state.GetNameUniqueID("positionData");
			DrawVelocityBillboardParticlesColour_BillboardCpu3D.cid2 = state.GetNameUniqueID("velocityData");
			DrawVelocityBillboardParticlesColour_BillboardCpu3D.cid3 = state.GetNameUniqueID("velocityScale");
			DrawVelocityBillboardParticlesColour_BillboardCpu3D.cid4 = state.GetNameUniqueID("worldSpaceYAxis");
			DrawVelocityBillboardParticlesColour_BillboardCpu3D.sid0 = state.GetNameUniqueID("DisplaySampler");
			DrawVelocityBillboardParticlesColour_BillboardCpu3D.tid4 = state.GetNameUniqueID("DisplayTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityBillboardParticlesColour_BillboardCpu3D.gd))
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
				DrawVelocityBillboardParticlesColour_BillboardCpu3D.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityBillboardParticlesColour_BillboardCpu3D.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityBillboardParticlesColour_BillboardCpu3D.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityBillboardParticlesColour_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityBillboardParticlesColour_BillboardCpu3D.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityBillboardParticlesColour_BillboardCpu3D.fx, DrawVelocityBillboardParticlesColour_BillboardCpu3D.fxb, 40, 9);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityBillboardParticlesColour_BillboardCpu3D.vin[i]));
			index = DrawVelocityBillboardParticlesColour_BillboardCpu3D.vin[(i + 1)];
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
				return new byte[] {4,35,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,211,223,84,126,255,181,241,55,253,255,95,250,77,228,239,255,150,254,255,235,232,103,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,255,27,159,95,143,254,255,251,95,54,191,255,148,126,254,70,191,134,196,177,191,92,227,90,243,252,250,104,179,106,126,255,102,71,98,220,223,76,219,249,207,175,69,255,71,120,220,253,92,98,226,255,251,255,6,140,215,243,108,150,215,210,22,159,163,237,175,173,63,83,239,157,127,143,250,255,159,60,28,14,52,238,198,59,59,250,59,96,252,41,250,253,175,131,207,232,255,127,170,254,253,59,209,239,191,157,215,206,60,255,55,61,38,70,255,141,240,203,175,245,127,255,223,255,215,255,253,187,254,26,39,111,142,159,252,78,244,231,151,191,134,124,134,175,126,39,110,245,107,164,223,166,127,118,126,13,19,243,255,90,191,198,129,190,175,244,248,107,127,29,162,218,175,201,255,201,67,31,239,253,254,59,191,198,23,197,180,174,154,234,188,77,183,94,221,73,191,253,252,245,243,84,70,159,158,84,139,85,81,210,47,15,199,123,159,142,31,222,223,27,239,29,236,239,255,26,191,139,160,250,7,81,79,127,147,249,253,215,252,53,126,83,243,251,159,244,107,252,6,191,233,95,244,132,209,248,77,169,205,127,246,55,253,26,191,193,127,246,23,253,186,252,247,175,143,191,169,237,127,246,55,17,14,191,214,175,249,107,252,118,244,251,255,253,55,225,187,95,203,126,247,127,255,65,242,247,111,64,127,255,223,252,55,218,18,204,63,232,215,162,239,255,239,255,91,145,255,53,132,70,230,247,255,227,55,167,127,126,173,255,139,104,244,207,255,218,134,70,255,247,111,34,159,249,52,250,223,126,19,161,17,16,253,111,149,70,223,254,53,12,95,253,181,127,237,175,73,104,254,154,52,203,200,147,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,255,111,122,124,92,46,191,102,76,255,19,191,198,175,241,235,254,183,191,198,111,250,23,17,136,223,131,194,249,223,195,192,115,177,254,111,250,39,253,58,156,254,248,181,255,163,95,227,215,248,175,255,36,138,143,255,34,252,255,183,228,239,127,77,250,254,215,248,147,126,45,142,241,249,247,63,24,127,143,229,187,191,233,215,224,252,0,126,255,245,254,160,255,246,215,248,207,254,34,2,242,235,114,110,32,253,221,208,31,125,255,167,211,59,63,253,7,253,24,189,79,121,139,63,8,121,128,255,156,218,201,223,191,14,255,253,95,216,191,127,77,254,251,191,164,191,127,45,205,41,80,62,225,15,250,175,126,141,127,229,47,254,13,248,239,223,128,191,199,255,127,125,238,19,127,75,78,193,228,31,240,247,111,192,56,252,6,127,208,239,149,254,103,140,131,249,249,27,217,62,254,239,63,24,237,124,24,191,158,247,251,175,165,237,104,92,127,240,127,251,107,124,245,23,253,154,156,179,248,77,45,60,208,234,215,100,220,126,141,63,232,191,249,53,254,105,194,245,159,214,188,134,205,129,252,65,166,205,175,241,107,252,43,244,247,63,141,252,198,31,252,187,81,31,191,150,142,233,215,229,62,164,61,222,247,219,255,90,252,206,127,246,7,27,152,191,22,143,235,175,249,147,126,3,158,35,224,255,79,91,92,126,220,205,209,31,36,52,227,241,225,239,63,88,230,244,215,162,207,255,239,63,232,191,166,113,252,215,52,167,191,142,133,241,107,208,255,191,250,131,48,62,67,191,95,147,251,249,53,254,32,105,99,232,249,213,159,132,254,127,29,78,209,233,103,233,255,205,125,255,26,138,195,175,201,99,249,77,255,163,63,195,253,205,180,4,236,31,19,156,254,33,188,247,31,235,60,211,215,252,247,127,98,255,254,117,248,239,255,212,254,253,27,240,223,255,25,253,141,156,208,255,19,0,0,255,255};
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
		/// <summary>Name ID for 'velocityScale'</summary>
		private static int cid3;
		/// <summary>Set the shader value 'float2 velocityScale'</summary><param name="value"/>
		public void SetVelocityScale(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[233] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
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
		/// <summary>Name ID for 'worldSpaceYAxis'</summary>
		private static int cid4;
		/// <summary>Set the shader value 'float3 worldSpaceYAxis'</summary><param name="value"/>
		public void SetWorldSpaceYAxis(ref Microsoft.Xna.Framework.Vector3 value)
		{
			this.vreg[234] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, value.Z, 0F);
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
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[235];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[1];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[1];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityBillboardParticlesColour_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_BillboardCpu3D.cid3))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityBillboardParticlesColour_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_BillboardCpu3D.cid4))
			{
				this.SetWorldSpaceYAxis(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector4[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector4[] value)
		{
			if ((DrawVelocityBillboardParticlesColour_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_BillboardCpu3D.cid0))
			{
				this.SetColourData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_BillboardCpu3D.cid1))
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawVelocityBillboardParticlesColour_BillboardCpu3D.cid2))
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityBillboardParticlesColour_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_BillboardCpu3D.sid0))
			{
				this.DisplaySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityBillboardParticlesColour_BillboardCpu3D.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityBillboardParticlesColour_BillboardCpu3D.tid4))
			{
				this.DisplayTexture = value;
				return true;
			}
			return false;
		}
	}
}
#endif
