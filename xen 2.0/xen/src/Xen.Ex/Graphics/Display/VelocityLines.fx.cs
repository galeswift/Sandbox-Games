// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = VelocityLines.fx
// Namespace = Xen.Ex.Graphics.Display

#if XBOX360
namespace Xen.Ex.Graphics.Display
{
	
	/// <summary><para>Technique 'DrawVelocityParticles_LinesGpuTex' generated from file 'VelocityLines.fx'</para><para>Vertex Shader: approximately 26 instruction slots used (4 texture, 22 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticles_LinesGpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticles_LinesGpuTex' shader</summary>
		public DrawVelocityParticles_LinesGpuTex()
		{
			this.sc0 = -1;
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticles_LinesGpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticles_LinesGpuTex.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticles_LinesGpuTex.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticles_LinesGpuTex.sid0 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticles_LinesGpuTex.sid1 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticles_LinesGpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticles_LinesGpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticles_LinesGpuTex.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticles_LinesGpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticles_LinesGpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticles_LinesGpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticles_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticles_LinesGpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticles_LinesGpuTex.fx, DrawVelocityParticles_LinesGpuTex.fxb, 30, 4);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.vtc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticles_LinesGpuTex.vin[i]));
			index = DrawVelocityParticles_LinesGpuTex.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,68,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,176,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,212,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,248,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,4,131,0,0,1,1,131,0,0,1,6,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,188,131,0,0,1,208,139,0,0,1,224,131,0,0,1,244,138,0,0,1,1,1,56,135,0,0,1,1,1,0,1,0,1,1,1,52,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,8,1,0,1,0,1,1,1,4,131,0,0,1,93,134,0,0,1,1,1,32,1,0,1,0,1,1,1,28,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,135,0,0,1,184,1,16,1,42,1,17,132,0,0,1,124,131,0,0,1,60,135,0,0,1,36,135,0,0,1,88,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,60,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,16,1,33,1,0,1,1,1,0,1,1,131,0,0,1,1,1,0,1,0,1,240,1,81,132,0,0,1,48,1,1,1,196,1,0,1,34,131,0,0,1,22,1,128,1,1,132,0,0,1,27,1,226,131,0,0,1,200,1,7,1,0,1,1,1,0,1,192,1,27,1,0,1,225,1,0,1,1,1,0,1,200,1,15,1,128,133,0,0,1,226,1,1,1,1,149,0,0,132,255,0,138,0,0,1,2,1,232,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,136,1,0,1,0,1,1,1,96,135,0,0,1,36,1,0,1,0,1,1,1,44,1,0,1,0,1,1,1,84,138,0,0,1,1,1,4,131,0,0,1,28,131,0,0,1,246,1,255,1,254,1,3,132,0,0,1,3,131,0,0,1,28,135,0,0,1,239,131,0,0,1,88,1,0,1,2,131,0,0,1,6,133,0,0,1,96,131,0,0,1,112,131,0,0,1,208,1,0,1,3,131,0,0,1,1,133,0,0,1,216,135,0,0,1,232,1,0,1,3,1,0,1,1,1,0,1,1,133,0,0,1,216,132,0,0,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,32,1,0,1,1,1,0,1,2,138,0,0,1,16,1,33,131,0,0,1,1,131,0,0,1,1,131,0,0,1,1,1,0,1,0,1,2,1,144,131,0,0,1,4,1,0,1,0,1,240,1,81,1,0,1,0,1,16,1,22,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,4,1,64,1,10,1,18,1,0,1,18,1,0,1,0,1,80,132,0,0,1,96,1,14,1,194,1,0,1,18,133,0,0,1,32,1,20,1,0,1,0,1,18,1,0,1,196,133,0,0,1,16,1,22,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,14,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,128,133,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,27,1,0,1,27,1,232,1,128,1,0,1,0,1,200,1,8,1,0,1,1,1,1,1,27,1,177,1,177,1,237,131,0,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,131,0,0,1,196,1,179,1,0,1,224,1,1,1,1,1,0,1,184,1,128,133,0,0,1,65,1,194,1,0,1,0,1,255,1,49,1,8,1,16,1,1,1,15,1,31,1,254,1,200,1,0,1,0,1,64,1,0,1,49,1,24,1,32,1,1,1,15,1,31,1,254,1,200,1,0,1,0,1,64,1,0,1,200,1,6,131,0,0,1,198,1,188,1,0,1,225,1,0,1,2,1,0,1,200,1,1,131,0,0,1,198,1,177,1,108,1,139,1,2,2,5,5,2,200,3,131,0,3,197,108,0,1,225,131,0,2,200,3,131,0,4,176,198,176,235,5,0,1,1,200,1,6,128,62,0,109,109,27,1,145,131,0,7,200,2,128,62,0,109,109,8,27,145,0,1,1,200,4,128,9,62,0,109,109,27,145,0,2,2,10,200,8,128,62,0,109,109,27,145,0,5,3,3,36,254,192,132,0,5,108,226,0,0,128,139,0,1,0};
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
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid1;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
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
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticles_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticles_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticles_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.sid0))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.sid1))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticles_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticlesColour_LinesGpuTex' generated from file 'VelocityLines.fx'</para><para>Vertex Shader: approximately 28 instruction slots used (6 texture, 22 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticlesColour_LinesGpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticlesColour_LinesGpuTex' shader</summary>
		public DrawVelocityParticlesColour_LinesGpuTex()
		{
			this.sc0 = -1;
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticlesColour_LinesGpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticlesColour_LinesGpuTex.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticlesColour_LinesGpuTex.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticlesColour_LinesGpuTex.sid0 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityParticlesColour_LinesGpuTex.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticlesColour_LinesGpuTex.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticlesColour_LinesGpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticlesColour_LinesGpuTex.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityParticlesColour_LinesGpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticlesColour_LinesGpuTex.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticlesColour_LinesGpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticlesColour_LinesGpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticlesColour_LinesGpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticlesColour_LinesGpuTex.fx, DrawVelocityParticlesColour_LinesGpuTex.fxb, 32, 4);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.vtc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticlesColour_LinesGpuTex.vin[i]));
			index = DrawVelocityParticlesColour_LinesGpuTex.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,104,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,176,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,212,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,248,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,28,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,5,131,0,0,1,1,131,0,0,1,7,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,188,131,0,0,1,208,139,0,0,1,224,131,0,0,1,244,138,0,0,1,1,1,4,1,0,1,0,1,1,1,24,138,0,0,1,1,1,92,135,0,0,1,1,1,0,1,0,1,1,1,88,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,44,1,0,1,0,1,1,1,40,131,0,0,1,93,134,0,0,1,1,1,68,1,0,1,0,1,1,1,64,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,135,0,0,1,184,1,16,1,42,1,17,132,0,0,1,124,131,0,0,1,60,135,0,0,1,36,135,0,0,1,88,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,60,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,16,1,33,1,0,1,1,1,0,1,1,131,0,0,1,1,1,0,1,0,1,240,1,81,132,0,0,1,48,1,1,1,196,1,0,1,34,131,0,0,1,22,1,128,1,1,132,0,0,1,27,1,226,131,0,0,1,200,1,7,1,0,1,1,1,0,1,192,1,27,1,0,1,225,1,0,1,1,1,0,1,200,1,15,1,128,133,0,0,1,226,1,1,1,1,149,0,0,132,255,0,138,0,0,1,3,1,16,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,164,1,0,1,0,1,1,1,108,135,0,0,1,36,1,0,1,0,1,1,1,72,1,0,1,0,1,1,1,112,138,0,0,1,1,1,32,131,0,0,1,28,1,0,1,0,1,1,1,17,1,255,1,254,1,3,132,0,0,1,4,131,0,0,1,28,134,0,0,1,1,1,10,131,0,0,1,108,1,0,1,2,131,0,0,1,6,133,0,0,1,116,131,0,0,1,132,131,0,0,1,228,1,0,1,3,131,0,0,1,1,133,0,0,1,236,135,0,0,1,252,1,0,1,3,1,0,1,1,1,0,1,1,133,0,0,1,236,134,0,0,1,1,1,3,1,0,1,3,1,0,1,2,1,0,1,1,133,0,0,1,236,132,0,0,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,44,1,0,1,1,1,0,1,3,138,0,0,1,16,1,33,131,0,0,1,1,131,0,0,1,1,131,0,0,1,1,1,0,1,0,1,2,1,144,131,0,0,1,4,1,0,1,0,1,240,1,81,1,0,1,0,1,16,1,23,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,4,1,80,1,10,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,15,1,194,1,0,1,18,133,0,0,1,32,1,21,1,0,1,0,1,18,1,0,1,196,133,0,0,1,16,1,23,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,14,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,128,133,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,27,1,0,1,27,1,232,1,128,1,0,1,0,1,200,1,8,1,0,1,1,1,1,1,27,1,177,1,177,1,237,131,0,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,131,0,0,1,196,1,179,1,0,1,224,1,1,1,1,1,0,1,184,1,128,133,0,0,1,65,1,194,1,0,1,0,1,255,1,49,1,24,1,16,1,1,1,15,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,49,1,8,1,32,1,1,1,15,1,31,1,254,1,200,1,0,1,0,1,64,1,0,1,49,1,40,1,48,1,1,2,15,31,3,254,200,0,4,0,64,0,200,1,6,131,0,5,198,188,0,225,0,4,3,0,200,1,131,0,6,198,177,108,139,3,5,3,5,200,3,131,0,4,197,108,0,225,131,0,2,200,3,131,0,7,176,198,176,235,0,2,2,8,200,1,128,62,0,109,109,27,1,145,131,0,9,200,2,128,62,0,109,109,27,145,10,0,1,1,200,4,128,62,0,109,109,11,27,145,0,2,2,200,8,128,62,0,109,9,109,27,145,0,3,3,200,15,128,133,0,3,226,1,1,140,0,1,0};
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
		/// <summary>Name uid for sampler for 'Sampler2D ColourSampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid2;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D ColourTexture'</summary>
		static int tid1;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
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
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.sid0))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticles_LinesGpuTex_UserOffset' generated from file 'VelocityLines.fx'</para><para>Vertex Shader: approximately 29 instruction slots used (6 texture, 23 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticles_LinesGpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticles_LinesGpuTex_UserOffset' shader</summary>
		public DrawVelocityParticles_LinesGpuTex_UserOffset()
		{
			this.sc0 = -1;
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticles_LinesGpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticles_LinesGpuTex_UserOffset.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticles_LinesGpuTex_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticles_LinesGpuTex_UserOffset.sid0 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticles_LinesGpuTex_UserOffset.sid1 = state.GetNameUniqueID("UserSampler");
			DrawVelocityParticles_LinesGpuTex_UserOffset.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticles_LinesGpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticles_LinesGpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticles_LinesGpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticles_LinesGpuTex_UserOffset.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticles_LinesGpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticles_LinesGpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticles_LinesGpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticles_LinesGpuTex_UserOffset.fx, DrawVelocityParticles_LinesGpuTex_UserOffset.fxb, 33, 4);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.vtc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticles_LinesGpuTex_UserOffset.vin[i]));
			index = DrawVelocityParticles_LinesGpuTex_UserOffset.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,104,135,0,1,10,131,0,1,4,131,0,1,28,143,0,17,17,95,95,117,110,117,115,101,100,95,115,97,109,112,108,101,114,135,0,1,3,131,0,1,1,131,0,1,176,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,212,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,248,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,28,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,5,131,0,0,1,1,131,0,0,1,7,131,0,0,1,3,131,0,0,1,4,131,0,0,1,24,139,0,0,1,52,131,0,0,1,80,139,0,0,1,188,131,0,0,1,208,139,0,0,1,224,131,0,0,1,244,138,0,0,1,1,1,4,1,0,1,0,1,1,1,24,138,0,0,1,1,1,92,135,0,0,1,1,1,0,1,0,1,1,1,88,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,44,1,0,1,0,1,1,1,40,131,0,0,1,93,134,0,0,1,1,1,68,1,0,1,0,1,1,1,64,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,135,0,0,1,184,1,16,1,42,1,17,132,0,0,1,124,131,0,0,1,60,135,0,0,1,36,135,0,0,1,88,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,60,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,16,1,33,1,0,1,1,1,0,1,1,131,0,0,1,1,1,0,1,0,1,240,1,81,132,0,0,1,48,1,1,1,196,1,0,1,34,131,0,0,1,22,1,128,1,1,132,0,0,1,27,1,226,131,0,0,1,200,1,7,1,0,1,1,1,0,1,192,1,27,1,0,1,225,1,0,1,1,1,0,1,200,1,15,1,128,133,0,0,1,226,1,1,1,1,149,0,0,132,255,0,138,0,0,1,3,1,28,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,164,1,0,1,0,1,1,1,120,135,0,0,1,36,1,0,1,0,1,1,1,72,1,0,1,0,1,1,1,112,138,0,0,1,1,1,32,131,0,0,1,28,1,0,1,0,1,1,1,17,1,255,1,254,1,3,132,0,0,1,4,131,0,0,1,28,134,0,0,1,1,1,10,131,0,0,1,108,1,0,1,2,131,0,0,1,6,133,0,0,1,116,131,0,0,1,132,131,0,0,1,228,1,0,1,3,131,0,0,1,1,133,0,0,1,236,135,0,0,1,252,1,0,1,3,1,0,1,1,1,0,1,1,133,0,0,1,236,134,0,0,1,1,1,3,1,0,1,3,1,0,1,2,1,0,1,1,133,0,0,1,236,132,0,0,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,131,171,0,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,56,1,0,1,1,1,0,1,2,138,0,0,1,16,1,33,131,0,0,1,1,131,0,0,1,1,131,0,0,1,1,1,0,1,0,1,2,1,144,131,0,0,1,4,1,0,1,0,1,240,1,81,1,0,1,0,1,16,1,24,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,4,1,80,1,10,1,18,1,0,1,18,1,0,1,1,1,80,132,0,0,1,96,1,15,1,194,1,0,1,18,133,0,0,1,48,1,21,1,0,1,0,1,18,1,0,1,196,133,0,0,1,16,1,24,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,14,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,128,133,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,27,1,0,1,27,1,232,1,128,1,0,1,0,1,200,1,8,1,0,1,1,1,1,1,27,1,177,1,177,1,237,131,0,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,131,0,0,1,196,1,179,1,0,1,224,1,1,1,1,1,0,1,184,1,128,133,0,0,1,65,1,194,1,0,1,0,1,255,1,49,1,24,1,32,1,1,1,15,1,31,1,254,1,200,1,0,1,0,1,64,1,0,1,49,1,8,1,16,1,1,1,15,1,31,1,254,1,200,1,0,1,0,1,64,1,0,1,49,1,40,1,0,1,1,2,15,31,3,245,207,0,4,0,64,0,200,1,10,131,0,5,188,17,0,224,1,6,0,0,200,3,0,2,7,0,198,176,0,225,0,2,3,0,200,1,131,0,8,198,177,108,139,2,5,5,200,1,5,131,0,9,176,108,0,225,2,0,0,200,3,131,0,9,196,198,25,235,0,1,0,200,1,7,128,62,0,109,109,27,145,131,0,10,200,2,128,62,0,109,109,27,145,0,11,1,1,200,4,128,62,0,109,109,27,145,12,0,2,2,200,8,128,62,0,109,109,27,145,6,0,3,3,36,254,192,132,0,5,108,226,0,0,128,139,0,1,0};
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
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D UserSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid2;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D UserTexture'</summary>
		static int tid3;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
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
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.sid0))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.sid1))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticlesColour_LinesGpuTex_UserOffset' generated from file 'VelocityLines.fx'</para><para>Vertex Shader: approximately 31 instruction slots used (8 texture, 23 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticlesColour_LinesGpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticlesColour_LinesGpuTex_UserOffset' shader</summary>
		public DrawVelocityParticlesColour_LinesGpuTex_UserOffset()
		{
			this.sc0 = -1;
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[3] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid0 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid2 = state.GetNameUniqueID("UserSampler");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticlesColour_LinesGpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticlesColour_LinesGpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticlesColour_LinesGpuTex_UserOffset.fx, DrawVelocityParticlesColour_LinesGpuTex_UserOffset.fxb, 35, 4);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.vtc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticlesColour_LinesGpuTex_UserOffset.vin[i]));
			index = DrawVelocityParticlesColour_LinesGpuTex_UserOffset.vin[(i + 1)];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,1,92,135,0,1,3,131,0,1,1,131,0,1,128,135,0,1,6,131,0,1,4,131,0,1,1,227,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,164,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,131,0,0,1,200,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,131,0,0,1,236,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,1,1,16,143,0,0,1,7,1,95,1,118,1,115,1,95,1,115,1,51,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,5,131,0,0,1,1,131,0,0,1,7,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,139,0,0,1,140,131,0,0,1,160,139,0,0,1,176,131,0,0,1,196,139,0,0,1,212,131,0,0,1,232,139,0,0,1,248,1,0,1,0,1,1,1,12,138,0,0,1,1,1,80,135,0,0,1,1,1,0,1,0,1,1,1,76,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,32,1,0,1,0,1,1,1,28,131,0,0,1,93,134,0,0,1,1,1,56,1,0,1,0,1,1,1,52,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,135,0,0,1,184,1,16,1,42,1,17,132,0,0,1,124,131,0,0,1,60,135,0,0,1,36,135,0,0,1,88,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,60,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,16,1,33,1,0,1,1,1,0,1,1,131,0,0,1,1,1,0,1,0,1,240,1,81,132,0,0,1,48,1,1,1,196,1,0,1,34,131,0,0,1,22,1,128,1,1,132,0,0,1,27,1,226,131,0,0,1,200,1,7,1,0,1,1,1,0,1,192,1,27,1,0,1,225,1,0,1,1,1,0,1,200,1,15,1,128,133,0,0,1,226,1,1,1,1,149,0,0,132,255,0,138,0,0,1,3,1,64,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,188,1,0,1,0,1,1,1,132,135,0,0,1,36,1,0,1,0,1,1,1,96,1,0,1,0,1,1,1,136,138,0,0,1,1,1,56,131,0,0,1,28,1,0,1,0,1,1,1,44,1,255,1,254,1,3,132,0,0,1,5,131,0,0,1,28,134,0,0,1,1,1,37,131,0,0,1,128,1,0,1,2,131,0,0,1,6,133,0,0,1,136,131,0,0,1,152,131,0,0,1,248,1,0,1,3,131,0,0,1,1,132,0,0,1,1,135,0,0,1,1,1,16,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,135,0,0,1,1,1,23,1,0,1,3,1,0,1,2,1,0,1,1,132,0,0,1,1,135,0,0,1,1,1,30,1,0,1,3,1,0,1,3,1,0,1,1,132,0,0,1,1,133,0,0,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,2,3,0,3,1,0,4,2,0,6,229,0,0,1,0,1,95,1,118,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,118,1,115,1,95,1,115,1,49,1,0,1,95,1,118,1,115,1,95,1,115,1,50,1,0,1,95,1,118,1,115,1,95,1,115,1,51,1,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,136,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,68,1,0,1,1,1,0,1,3,138,0,0,1,16,1,33,131,0,0,1,1,131,0,0,1,1,131,0,0,1,1,1,0,1,0,1,2,1,144,131,0,0,1,4,1,0,1,0,1,240,1,81,1,0,1,0,1,16,1,25,176,0,0,1,63,1,128,1,0,1,0,1,63,139,0,0,1,16,1,9,1,96,1,4,1,96,1,10,1,18,1,0,1,18,1,0,1,5,1,80,132,0,0,1,96,1,16,1,194,1,0,1,18,133,0,0,1,48,1,22,1,0,1,0,1,18,1,0,1,196,133,0,0,1,16,1,25,1,0,1,0,1,34,133,0,0,1,5,1,248,132,0,0,1,14,1,120,132,0,0,1,176,1,35,1,0,1,1,1,0,1,176,1,177,1,192,1,1,1,4,1,255,1,4,1,168,1,128,133,0,0,1,65,1,194,1,0,1,0,1,4,1,52,1,18,131,0,0,1,27,1,0,1,27,1,232,1,128,1,0,1,0,1,200,1,8,1,0,1,1,1,1,1,27,1,177,1,177,1,237,131,0,0,1,168,1,64,1,1,132,0,0,1,128,1,194,1,0,1,0,1,4,1,200,1,3,131,0,0,1,196,1,179,1,0,1,224,1,1,1,1,1,0,1,184,1,128,133,0,0,1,65,1,194,1,0,1,0,1,255,1,49,1,24,1,16,1,1,1,15,1,31,1,246,1,136,1,0,1,0,1,64,2,0,49,3,40,48,1,4,15,31,254,200,5,0,0,64,0,49,6,8,32,1,15,31,254,7,200,0,0,64,0,49,56,8,0,1,15,31,245,207,0,0,4,64,0,200,10,131,0,9,188,17,0,224,2,0,0,200,3,10,0,3,0,198,176,0,225,0,3,0,2,200,1,131,0,9,198,177,108,139,3,5,5,200,5,131,0,9,176,108,0,225,3,0,0,200,3,131,0,11,196,198,25,235,0,2,0,200,1,128,62,5,0,109,109,27,145,131,0,12,200,2,128,62,0,109,109,27,145,0,1,1,13,200,4,128,62,0,109,109,27,145,0,2,2,200,14,8,128,62,0,109,109,27,145,0,3,3,200,15,128,133,0,3,226,1,1,140,0,1,0};
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
		/// <summary>Name uid for sampler for 'Sampler2D ColourSampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D UserSampler'</summary>
		static int sid2;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid3;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D ColourTexture'</summary>
		static int tid1;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D UserTexture'</summary>
		static int tid3;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
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
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid0))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid2))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			return false;
		}
	}
}
#else
namespace Xen.Ex.Graphics.Display
{
	
	/// <summary><para>Technique 'DrawVelocityParticles_LinesGpuTex' generated from file 'VelocityLines.fx'</para><para>Vertex Shader: approximately 26 instruction slots used (4 texture, 22 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticles_LinesGpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticles_LinesGpuTex' shader</summary>
		public DrawVelocityParticles_LinesGpuTex()
		{
			this.sc0 = -1;
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticles_LinesGpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticles_LinesGpuTex.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticles_LinesGpuTex.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticles_LinesGpuTex.sid0 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticles_LinesGpuTex.sid1 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticles_LinesGpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticles_LinesGpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticles_LinesGpuTex.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticles_LinesGpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticles_LinesGpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticles_LinesGpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticles_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticles_LinesGpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticles_LinesGpuTex.fx, DrawVelocityParticles_LinesGpuTex.fxb, 30, 4);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.vtc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticles_LinesGpuTex.vin[i]));
			index = DrawVelocityParticles_LinesGpuTex.vin[(i + 1)];
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
				return new byte[] {196,5,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,183,127,77,249,61,161,255,255,58,244,255,223,201,251,30,207,111,78,255,255,253,127,255,245,114,221,228,179,223,191,201,22,171,50,175,233,205,95,251,215,198,251,244,255,191,73,219,253,122,250,190,130,251,89,125,208,215,239,127,217,252,254,83,250,249,27,253,26,210,239,191,214,105,243,235,107,155,102,199,181,249,223,134,218,236,10,222,191,153,182,243,159,95,139,254,255,155,70,62,151,113,254,207,191,14,96,188,158,103,51,80,197,140,31,248,129,62,248,251,183,243,222,217,167,255,191,244,254,254,251,232,255,255,146,247,247,127,68,255,255,229,222,223,143,149,152,248,177,175,191,3,159,63,69,191,255,13,232,179,95,135,254,255,167,234,223,41,253,254,59,121,237,204,243,127,211,99,230,229,207,194,63,191,246,255,253,127,255,95,255,247,111,243,107,156,188,57,126,130,249,254,113,253,204,123,37,197,231,171,230,247,191,247,251,239,252,26,95,20,211,186,106,170,243,54,221,122,117,39,253,246,243,215,207,83,25,113,122,82,45,86,5,49,68,250,112,188,247,233,248,225,253,189,241,222,193,254,254,175,241,187,80,247,191,238,175,241,107,254,65,68,185,63,233,215,100,84,126,115,250,253,255,254,147,126,93,144,229,55,248,245,233,247,95,131,254,255,159,201,119,191,193,111,192,127,251,189,3,95,243,251,111,4,66,254,218,255,23,225,251,165,197,247,215,255,53,229,51,124,165,252,154,130,72,191,247,175,33,227,254,245,232,223,63,128,126,174,126,13,161,175,240,234,175,245,107,252,91,10,243,191,226,207,126,77,250,239,215,251,53,254,39,253,76,248,233,175,253,107,127,77,254,230,215,225,57,252,97,60,202,163,127,237,175,67,92,250,107,242,127,222,231,187,253,207,47,191,230,156,252,196,175,241,107,252,186,191,222,175,241,155,254,69,4,226,247,32,114,255,131,2,237,15,250,61,126,23,33,217,31,132,185,210,223,255,164,95,227,55,248,77,255,34,243,251,175,233,126,71,155,255,200,204,237,175,73,191,255,90,76,218,95,243,15,250,117,126,141,191,6,112,101,126,121,222,127,13,250,236,215,248,139,126,75,126,239,215,194,223,127,236,111,204,223,253,58,252,29,253,255,15,254,45,152,117,209,246,175,161,255,127,133,255,255,193,94,251,63,200,192,150,246,95,253,193,128,253,107,234,119,191,206,175,241,213,95,36,124,245,107,254,65,52,79,127,209,175,195,162,246,107,211,231,255,25,240,32,220,254,179,63,72,62,251,141,24,246,175,247,107,252,105,127,209,175,247,107,252,109,127,209,239,207,48,126,83,250,236,127,251,131,126,205,223,224,63,227,191,127,13,253,251,215,160,191,229,157,95,135,222,255,191,255,160,95,151,250,160,62,255,34,233,247,215,102,152,4,235,79,50,127,3,111,215,143,249,254,255,166,255,255,103,127,144,224,246,235,80,191,255,247,95,244,27,200,56,254,163,95,131,191,255,229,250,247,175,245,31,73,59,243,247,175,195,127,255,90,246,239,223,128,255,254,181,233,111,192,2,173,1,11,18,250,255,4,0,0,255,255};
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
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid1;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
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
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticles_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticles_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticles_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.sid0))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.sid1))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticles_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticlesColour_LinesGpuTex' generated from file 'VelocityLines.fx'</para><para>Vertex Shader: approximately 28 instruction slots used (6 texture, 22 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticlesColour_LinesGpuTex : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticlesColour_LinesGpuTex' shader</summary>
		public DrawVelocityParticlesColour_LinesGpuTex()
		{
			this.sc0 = -1;
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticlesColour_LinesGpuTex.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticlesColour_LinesGpuTex.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticlesColour_LinesGpuTex.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticlesColour_LinesGpuTex.sid0 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityParticlesColour_LinesGpuTex.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticlesColour_LinesGpuTex.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticlesColour_LinesGpuTex.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticlesColour_LinesGpuTex.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityParticlesColour_LinesGpuTex.tid2 = state.GetNameUniqueID("VelocityTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticlesColour_LinesGpuTex.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticlesColour_LinesGpuTex.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticlesColour_LinesGpuTex.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticlesColour_LinesGpuTex.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticlesColour_LinesGpuTex.fx, DrawVelocityParticlesColour_LinesGpuTex.fxb, 32, 4);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.vtc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticlesColour_LinesGpuTex.vin[i]));
			index = DrawVelocityParticlesColour_LinesGpuTex.vin[(i + 1)];
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
				return new byte[] {52,6,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,87,249,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,77,111,254,218,191,54,222,167,255,255,77,218,238,215,211,247,21,220,207,234,131,190,126,255,203,230,247,159,210,207,223,232,215,144,126,255,181,78,155,95,95,219,52,59,174,205,255,54,212,102,215,181,249,157,58,3,176,109,246,100,108,191,153,182,243,159,95,139,254,255,155,70,62,55,160,0,227,245,60,155,129,114,191,174,126,142,207,64,67,188,243,219,121,239,236,211,255,95,122,127,255,125,244,255,127,201,251,251,63,162,255,255,114,239,239,95,135,128,253,118,30,206,127,128,254,142,31,191,183,254,14,252,254,20,253,126,68,159,109,209,255,255,84,253,251,41,253,254,123,122,237,204,243,127,211,99,192,254,89,248,231,215,254,191,255,239,255,235,255,254,109,126,141,147,55,199,79,192,35,63,174,159,121,175,164,248,124,213,252,254,247,126,255,157,95,227,139,98,90,87,77,117,222,166,91,175,238,164,223,126,254,250,121,42,20,72,79,170,197,170,32,38,74,31,142,247,62,29,63,188,191,55,222,59,216,223,255,53,126,23,234,254,215,253,53,126,205,63,136,40,249,39,253,154,140,202,111,78,191,255,223,127,210,175,11,50,253,6,191,62,253,254,107,208,255,255,51,249,238,55,248,13,248,111,191,119,224,107,126,255,54,8,251,107,255,95,132,239,79,89,124,239,253,154,242,153,199,227,41,104,81,254,26,50,238,95,143,254,109,233,231,31,70,255,255,207,126,13,153,155,95,147,62,251,239,20,230,255,193,159,253,154,244,223,175,199,52,199,243,91,253,154,248,236,215,162,207,18,203,55,194,151,127,237,95,251,107,114,235,95,135,121,245,135,241,40,175,255,181,191,14,113,242,175,201,255,121,159,239,14,124,190,215,255,252,242,107,206,223,79,16,107,255,122,191,198,111,250,23,17,136,223,131,166,230,31,20,104,127,208,239,241,187,8,121,255,32,204,171,254,254,39,253,26,191,193,111,250,23,153,223,127,77,239,247,95,203,253,142,246,255,145,225,137,95,147,126,255,181,120,74,126,205,63,232,215,249,53,254,26,244,33,124,193,252,242,107,208,103,191,198,95,244,91,242,123,191,22,254,254,99,127,99,254,238,215,225,239,232,255,127,240,111,193,98,130,182,127,13,253,255,43,252,255,15,246,218,255,65,6,182,180,255,234,15,6,236,95,83,191,251,117,126,141,175,254,34,225,199,95,243,15,162,185,252,139,126,29,22,217,95,155,62,255,207,128,7,225,246,159,253,65,242,217,111,196,176,127,189,95,227,79,251,139,126,189,95,227,111,251,139,126,127,134,241,155,254,71,164,117,254,160,95,243,55,248,207,204,223,127,16,254,254,181,244,239,95,67,255,254,53,232,111,129,241,235,16,188,255,251,15,250,117,169,79,194,225,47,18,60,126,109,238,131,96,255,73,230,111,140,195,245,107,190,255,191,233,255,255,217,31,36,184,254,58,132,199,255,253,23,253,6,50,174,255,8,159,147,214,208,191,127,173,255,72,218,153,191,127,29,254,251,215,178,127,255,6,252,247,175,77,127,67,186,255,159,0,0,0,255,255};
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
		/// <summary>Name uid for sampler for 'Sampler2D ColourSampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid2;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D ColourTexture'</summary>
		static int tid1;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
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
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.sid0))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticles_LinesGpuTex_UserOffset' generated from file 'VelocityLines.fx'</para><para>Vertex Shader: approximately 29 instruction slots used (6 texture, 23 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticles_LinesGpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticles_LinesGpuTex_UserOffset' shader</summary>
		public DrawVelocityParticles_LinesGpuTex_UserOffset()
		{
			this.sc0 = -1;
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticles_LinesGpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticles_LinesGpuTex_UserOffset.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticles_LinesGpuTex_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticles_LinesGpuTex_UserOffset.sid0 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticles_LinesGpuTex_UserOffset.sid1 = state.GetNameUniqueID("UserSampler");
			DrawVelocityParticles_LinesGpuTex_UserOffset.sid2 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticles_LinesGpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticles_LinesGpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticles_LinesGpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticles_LinesGpuTex_UserOffset.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticles_LinesGpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticles_LinesGpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticles_LinesGpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticles_LinesGpuTex_UserOffset.fx, DrawVelocityParticles_LinesGpuTex_UserOffset.fxb, 33, 4);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.vtc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticles_LinesGpuTex_UserOffset.vin[i]));
			index = DrawVelocityParticles_LinesGpuTex_UserOffset.vin[(i + 1)];
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
				return new byte[] {80,6,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,87,249,107,202,239,9,253,255,215,161,255,255,78,222,247,120,126,115,250,255,239,255,251,175,151,235,38,159,253,254,77,182,88,149,121,77,111,254,218,191,54,222,167,255,255,77,218,238,215,211,247,21,220,207,234,131,190,126,255,203,230,247,159,254,26,255,243,175,243,27,253,26,210,239,191,214,105,243,235,107,155,102,231,215,248,105,211,230,127,27,106,179,251,107,252,26,166,205,239,212,25,128,109,179,39,99,251,205,180,157,255,252,90,244,255,223,52,242,185,128,186,199,48,94,207,179,25,40,247,235,234,231,248,12,52,196,59,191,157,247,206,62,253,255,165,247,247,223,71,255,255,151,188,191,255,35,250,255,47,247,254,254,117,8,216,111,231,225,252,7,232,239,248,241,123,235,239,192,239,79,209,239,71,244,217,22,253,255,79,213,191,159,210,239,191,167,215,206,60,255,55,61,6,236,159,133,127,126,237,255,251,255,254,191,254,239,223,230,215,56,121,115,252,4,60,242,227,250,153,247,74,138,207,87,205,239,127,239,247,223,249,53,190,40,166,117,213,84,231,109,186,245,234,78,250,237,231,175,159,167,66,129,244,164,90,172,10,98,162,244,225,120,239,211,241,195,251,123,227,189,131,253,253,95,227,119,161,238,127,221,95,227,215,252,131,136,146,127,210,175,201,168,252,230,244,251,255,253,39,253,186,32,211,111,240,235,211,239,191,6,253,255,63,147,239,126,131,223,128,255,246,123,7,190,230,247,25,8,251,107,255,95,132,239,79,89,124,239,253,154,242,153,199,227,41,104,81,254,26,50,238,95,143,254,109,233,231,31,70,255,255,207,126,13,153,155,95,147,62,251,239,20,230,255,193,159,253,154,244,223,175,199,52,199,243,91,253,154,248,236,215,162,207,18,203,55,194,151,127,237,95,251,107,114,235,95,135,121,245,135,241,40,175,255,181,191,14,113,242,175,201,255,121,159,239,14,124,190,215,255,252,242,107,206,223,79,16,107,255,122,191,198,111,250,23,17,136,223,131,166,230,31,20,104,127,208,239,241,187,8,121,255,32,204,171,254,254,39,253,26,191,193,111,250,23,153,223,127,77,239,247,95,203,253,142,246,255,145,225,137,95,147,126,255,181,120,74,126,205,63,232,215,249,53,254,26,244,33,124,193,252,242,107,208,103,191,198,95,244,91,242,123,191,22,254,254,99,127,99,254,238,215,225,239,232,255,127,240,111,193,98,130,182,127,13,253,255,43,252,255,15,246,218,255,65,6,182,180,255,234,15,6,236,95,83,191,251,117,126,141,175,254,34,225,199,95,243,15,162,185,252,139,126,29,22,217,95,155,62,255,207,128,7,225,246,159,253,65,242,217,111,196,176,127,189,95,227,79,251,139,126,189,95,227,111,251,139,126,127,134,241,155,210,103,255,219,31,244,107,253,6,255,25,255,253,107,233,223,191,134,254,253,107,232,223,191,38,253,253,107,113,251,95,155,224,253,215,132,143,129,137,49,252,223,127,208,175,75,56,16,78,127,145,140,249,215,166,207,254,51,244,165,52,192,223,127,13,127,102,112,195,239,191,22,189,7,220,4,247,95,135,240,250,191,255,162,223,64,198,249,31,201,251,191,92,255,254,181,248,239,95,211,254,253,235,252,71,242,190,249,251,55,224,191,127,109,250,27,176,48,23,128,5,201,255,127,2,0,0,255,255};
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
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D UserSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid2;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D UserTexture'</summary>
		static int tid3;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
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
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.sid0))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.sid1))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.sid2))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticles_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticles_LinesGpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticlesColour_LinesGpuTex_UserOffset' generated from file 'VelocityLines.fx'</para><para>Vertex Shader: approximately 31 instruction slots used (8 texture, 23 arithmetic), 6 registers</para><para>Pixel Shader: approximately 3 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticlesColour_LinesGpuTex_UserOffset : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticlesColour_LinesGpuTex_UserOffset' shader</summary>
		public DrawVelocityParticlesColour_LinesGpuTex_UserOffset()
		{
			this.sc0 = -1;
			this.vts[1] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[0] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[3] = ((Xen.Graphics.TextureSamplerState)(64));
			this.vts[2] = ((Xen.Graphics.TextureSamplerState)(64));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.cid0 = state.GetNameUniqueID("textureSizeOffset");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.cid1 = state.GetNameUniqueID("velocityScale");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid0 = state.GetNameUniqueID("ColourSampler");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid1 = state.GetNameUniqueID("PositionSampler");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid2 = state.GetNameUniqueID("UserSampler");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid3 = state.GetNameUniqueID("VelocitySampler");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid0 = state.GetNameUniqueID("PositionTexture");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid1 = state.GetNameUniqueID("ColourTexture");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid2 = state.GetNameUniqueID("VelocityTexture");
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid3 = state.GetNameUniqueID("UserTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.vtc))
			{
				state.SetVertexShaderSamplers(this.vtx, this.vts);
				this.vtc = false;
			}
			if ((this.vreg_change == true))
			{
				DrawVelocityParticlesColour_LinesGpuTex_UserOffset.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticlesColour_LinesGpuTex_UserOffset.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticlesColour_LinesGpuTex_UserOffset.fx, DrawVelocityParticlesColour_LinesGpuTex_UserOffset.fxb, 35, 4);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.vtc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticlesColour_LinesGpuTex_UserOffset.vin[i]));
			index = DrawVelocityParticlesColour_LinesGpuTex_UserOffset.vin[(i + 1)];
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
				return new byte[] {128,6,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,31,240,107,202,239,191,54,254,166,255,255,65,250,221,175,71,255,255,117,244,179,159,237,7,125,253,254,151,205,239,63,253,53,170,242,55,250,53,164,223,191,172,211,230,215,215,54,205,206,175,241,107,152,54,255,212,80,155,93,215,230,191,27,106,179,247,107,252,58,166,205,111,214,25,164,109,115,79,198,255,155,105,59,255,249,181,232,255,191,105,228,115,1,245,91,252,246,128,241,122,158,205,242,250,215,248,131,126,93,253,28,159,129,206,120,39,245,222,249,227,232,255,127,145,247,247,223,68,255,255,199,188,191,255,53,250,255,127,229,253,253,191,209,255,127,35,15,231,55,250,59,126,60,215,223,129,223,159,162,223,167,244,217,239,68,255,255,83,245,239,3,250,125,223,107,103,158,255,155,30,3,246,207,194,63,191,246,255,253,127,255,95,255,247,111,243,107,156,188,57,126,242,59,209,159,63,174,159,121,175,164,248,124,213,252,254,247,126,255,157,95,227,139,98,90,87,77,117,222,166,91,175,238,164,223,126,254,250,121,42,20,72,79,170,197,170,40,233,151,135,227,189,79,199,15,239,239,141,247,14,246,247,127,141,223,133,186,255,117,127,141,95,243,15,34,74,254,73,191,38,163,242,155,211,239,255,247,159,244,235,130,76,191,193,175,79,191,255,26,244,255,255,76,190,251,13,126,3,254,219,239,29,248,154,223,255,34,16,246,215,254,191,8,223,220,226,251,251,255,154,242,25,232,255,59,73,179,244,247,254,53,133,199,49,238,95,143,254,253,163,126,13,25,235,255,246,107,24,25,248,181,44,211,131,47,126,109,250,227,215,164,150,191,157,126,182,197,159,253,90,244,89,242,107,236,232,103,191,39,127,134,150,191,201,175,241,109,253,76,248,249,175,253,107,127,77,254,252,215,97,30,255,97,60,42,35,127,237,175,67,18,240,107,242,127,222,231,187,3,159,239,13,124,126,175,255,249,229,215,156,235,159,248,53,126,141,95,247,215,251,53,126,211,191,136,64,252,30,52,3,255,160,64,251,131,126,143,223,69,166,226,15,2,15,232,239,127,210,175,241,27,252,166,127,145,249,253,215,244,126,255,181,188,223,127,109,247,59,222,253,143,12,47,253,154,244,251,175,197,83,249,107,254,65,191,206,175,241,215,160,63,225,39,230,179,95,131,62,251,53,254,162,223,146,223,251,181,240,247,31,251,27,243,119,191,14,127,71,255,255,131,127,11,250,227,215,228,182,127,13,253,255,43,252,255,15,246,218,255,65,6,182,180,255,234,15,6,236,95,83,191,251,117,126,141,175,254,34,225,227,95,243,15,162,249,254,139,126,29,22,245,95,155,62,255,207,128,7,225,246,159,253,65,242,217,111,196,176,127,189,95,227,79,251,139,126,189,95,227,111,251,139,126,127,134,241,155,254,71,196,133,127,208,175,249,27,252,103,230,239,63,8,127,255,218,250,247,175,165,127,255,26,250,247,175,161,127,255,90,244,247,175,197,237,127,109,130,255,95,19,126,166,15,140,233,255,38,221,243,213,95,68,56,254,69,66,131,95,155,62,251,207,208,183,210,4,127,255,53,252,153,193,21,191,255,90,244,30,112,149,177,252,58,132,231,255,253,23,253,6,50,238,255,72,222,255,229,250,247,175,197,127,255,154,246,239,95,231,63,146,247,205,223,191,1,255,253,107,211,223,208,26,255,79,0,0,0,255,255};
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
		/// <summary>Name uid for sampler for 'Sampler2D ColourSampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'Sampler2D PositionSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D UserSampler'</summary>
		static int sid2;
		/// <summary>Name uid for sampler for 'Sampler2D VelocitySampler'</summary>
		static int sid3;
		/// <summary>Name uid for texture for 'Texture2D PositionTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D ColourTexture'</summary>
		static int tid1;
		/// <summary>Name uid for texture for 'Texture2D VelocityTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D UserTexture'</summary>
		static int tid3;
		/// <summary>Vertex samplers/textures changed</summary>
		bool vtc;
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
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.cid1))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector3 value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.cid0))
			{
				this.SetTextureSizeOffset(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid0))
			{
				this.ColourSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid1))
			{
				this.PositionSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid2))
			{
				this.UserSampler = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.sid3))
			{
				this.VelocitySampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DrawVelocityParticlesColour_LinesGpuTex_UserOffset.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid0))
			{
				this.PositionTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid1))
			{
				this.ColourTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid2))
			{
				this.VelocityTexture = value;
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesGpuTex_UserOffset.tid3))
			{
				this.UserTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticles_LinesCpu' generated from file 'VelocityLines.fx'</para><para>Vertex Shader: approximately 14 instruction slots used, 165 registers</para><para>Pixel Shader: approximately 3 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticles_LinesCpu : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticles_LinesCpu' shader</summary>
		public DrawVelocityParticles_LinesCpu()
		{
			this.sc0 = -1;
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticles_LinesCpu.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticles_LinesCpu.cid0 = state.GetNameUniqueID("positionData");
			DrawVelocityParticles_LinesCpu.cid1 = state.GetNameUniqueID("velocityData");
			DrawVelocityParticles_LinesCpu.cid2 = state.GetNameUniqueID("velocityScale");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticles_LinesCpu.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[160], ref this.vreg[161], ref this.vreg[162], ref this.vreg[163], ref this.sc0));
			if ((this.vreg_change == true))
			{
				DrawVelocityParticles_LinesCpu.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticles_LinesCpu.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticles_LinesCpu.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticles_LinesCpu.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticles_LinesCpu.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticles_LinesCpu.fx, DrawVelocityParticles_LinesCpu.fxb, 16, 4);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return this.vreg_change;
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticles_LinesCpu.vin[i]));
			index = DrawVelocityParticles_LinesCpu.vin[(i + 1)];
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
				return new byte[] {232,23,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,63,148,200,239,191,54,254,166,255,175,244,239,191,156,254,255,235,232,103,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,124,200,243,235,209,255,127,255,203,230,247,159,254,26,226,95,254,102,191,134,248,154,254,243,107,209,255,127,211,200,231,198,31,253,245,233,255,175,231,217,44,175,229,51,252,255,215,214,255,227,157,212,123,231,111,83,159,22,109,254,58,253,29,240,255,20,253,254,15,162,207,126,134,254,255,167,234,223,127,22,253,254,167,121,237,204,243,127,211,99,250,255,179,248,203,255,251,255,254,191,254,239,223,230,215,56,121,115,252,228,119,162,63,127,92,63,243,94,73,241,249,170,249,253,247,126,255,157,95,227,139,98,90,87,77,117,222,166,91,175,238,164,223,126,254,250,121,42,35,72,79,170,197,170,40,233,151,135,227,189,79,199,15,239,239,141,247,14,246,247,127,141,223,69,186,255,131,126,205,95,227,55,253,155,126,77,254,253,183,163,223,255,239,191,233,215,229,97,254,250,127,16,97,68,127,255,103,242,221,111,240,155,210,223,255,217,31,228,247,14,124,205,239,255,213,111,8,220,254,47,194,247,111,255,181,12,190,127,97,34,159,97,76,191,147,52,75,49,246,157,95,67,58,254,203,233,223,3,250,249,237,95,195,204,215,95,251,215,254,154,212,245,175,73,20,70,124,240,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,135,62,151,95,211,87,254,137,95,227,215,248,117,255,242,95,227,55,253,139,200,93,254,61,124,120,198,135,166,104,226,79,250,45,249,247,95,147,126,255,53,254,164,95,139,125,104,254,253,15,198,223,99,249,238,111,250,53,216,223,198,119,191,193,31,244,50,253,191,9,222,175,241,55,253,58,28,82,160,237,255,253,7,253,101,191,198,87,127,209,95,246,107,252,26,127,209,175,203,159,253,122,244,217,87,127,210,203,244,95,226,118,226,151,255,218,128,73,255,255,175,255,160,95,135,67,29,252,253,159,253,65,191,134,194,250,53,210,255,140,127,138,63,255,235,252,65,228,71,255,69,191,129,224,242,15,161,221,95,244,107,252,114,253,251,215,226,191,255,98,251,247,175,195,127,255,37,246,239,223,128,255,254,75,233,111,192,162,24,225,63,2,44,68,31,255,79,0,0,0,255,255};
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
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[165];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticles_LinesCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesCpu.cid2))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector4[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector4[] value)
		{
			if ((DrawVelocityParticles_LinesCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticles_LinesCpu.cid0))
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawVelocityParticles_LinesCpu.cid1))
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'DrawVelocityParticlesColour_LinesCpu' generated from file 'VelocityLines.fx'</para><para>Vertex Shader: approximately 14 instruction slots used, 245 registers</para><para>Pixel Shader: approximately 3 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawVelocityParticlesColour_LinesCpu : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawVelocityParticlesColour_LinesCpu' shader</summary>
		public DrawVelocityParticlesColour_LinesCpu()
		{
			this.sc0 = -1;
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawVelocityParticlesColour_LinesCpu.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawVelocityParticlesColour_LinesCpu.cid0 = state.GetNameUniqueID("colourData");
			DrawVelocityParticlesColour_LinesCpu.cid1 = state.GetNameUniqueID("positionData");
			DrawVelocityParticlesColour_LinesCpu.cid2 = state.GetNameUniqueID("velocityData");
			DrawVelocityParticlesColour_LinesCpu.cid3 = state.GetNameUniqueID("velocityScale");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawVelocityParticlesColour_LinesCpu.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[240], ref this.vreg[241], ref this.vreg[242], ref this.vreg[243], ref this.sc0));
			if ((this.vreg_change == true))
			{
				DrawVelocityParticlesColour_LinesCpu.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawVelocityParticlesColour_LinesCpu.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawVelocityParticlesColour_LinesCpu.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawVelocityParticlesColour_LinesCpu.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawVelocityParticlesColour_LinesCpu.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawVelocityParticlesColour_LinesCpu.fx, DrawVelocityParticlesColour_LinesCpu.fxb, 16, 4);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return this.vreg_change;
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 1;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawVelocityParticlesColour_LinesCpu.vin[i]));
			index = DrawVelocityParticlesColour_LinesCpu.vin[(i + 1)];
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
				return new byte[] {236,33,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,63,244,155,202,239,191,54,254,166,255,175,244,239,95,65,255,255,117,244,179,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,251,63,191,30,253,255,247,191,108,126,255,233,175,33,241,245,111,246,107,72,172,237,63,191,22,253,31,161,120,247,115,19,143,255,250,244,255,215,243,108,150,215,242,25,254,255,107,235,255,241,78,234,189,243,183,105,76,143,54,127,157,254,14,248,127,138,126,255,7,209,103,63,67,255,255,83,245,239,63,139,126,255,211,188,118,230,249,191,233,49,253,255,89,252,229,255,253,127,255,95,255,247,111,243,107,156,188,57,126,242,59,209,159,63,174,159,121,175,164,248,124,213,252,254,123,191,255,206,175,241,69,49,173,171,166,58,111,211,173,87,119,210,111,63,127,253,60,149,17,164,39,213,98,85,148,244,203,195,241,222,167,227,135,247,247,198,123,7,251,251,191,198,239,34,221,255,65,191,230,175,241,155,254,77,191,38,255,254,219,209,239,255,247,223,244,235,242,48,127,253,63,136,48,162,191,255,51,249,238,55,248,77,233,239,255,236,15,242,123,7,190,230,247,255,238,55,3,110,255,23,225,251,43,126,109,131,239,95,248,155,202,103,24,211,239,196,173,126,141,20,99,223,249,53,164,227,95,65,255,30,208,207,111,255,26,102,190,254,218,191,246,215,164,174,127,77,162,48,242,35,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,255,27,159,255,47,224,116,249,53,115,5,63,241,107,252,26,191,238,175,248,53,126,211,191,136,210,5,191,135,15,207,228,16,40,155,242,39,253,150,252,251,175,73,191,255,26,127,210,175,197,57,4,254,253,15,198,223,99,249,238,111,250,53,56,223,128,239,126,131,63,232,101,250,127,19,188,95,227,111,250,117,56,165,130,182,255,247,31,244,203,127,141,175,254,162,95,254,107,252,26,127,209,175,203,159,253,122,244,217,87,127,210,203,244,95,226,118,146,151,248,181,1,147,254,255,95,255,65,191,14,167,122,240,247,127,246,7,253,26,10,235,215,72,255,51,254,137,62,40,175,241,31,253,69,238,111,234,255,215,249,131,40,175,240,23,253,6,130,219,63,132,247,254,167,95,227,151,235,223,191,22,255,253,63,219,191,127,29,254,251,127,177,127,255,6,252,247,47,163,191,145,129,249,127,2,0,0,255,255};
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
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[245];
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((DrawVelocityParticlesColour_LinesCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesCpu.cid3))
			{
				this.SetVelocityScale(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector4[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector4[] value)
		{
			if ((DrawVelocityParticlesColour_LinesCpu.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawVelocityParticlesColour_LinesCpu.cid0))
			{
				this.SetColourData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesCpu.cid1))
			{
				this.SetPositionData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			if ((id == DrawVelocityParticlesColour_LinesCpu.cid2))
			{
				this.SetVelocityData(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
	}
}
#endif
