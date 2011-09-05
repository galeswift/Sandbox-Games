// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = Shader.fx
// Namespace = Xen.Ex.Graphics2D

namespace Xen.Ex.Graphics2D
{
	
	/// <summary><para>Technique 'InstancingSprite' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 34 instruction slots used, 8 registers</para><para>Pixel Shader: approximately 3 instruction slots used (1 texture, 2 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class InstancingSprite : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'InstancingSprite' shader</summary>
		public InstancingSprite()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(192));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			InstancingSprite.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			InstancingSprite.cid0 = state.GetNameUniqueID("spriteWorldMatrix");
			InstancingSprite.sid0 = state.GetNameUniqueID("CustomTextureSampler");
			InstancingSprite.tid0 = state.GetNameUniqueID("CustomTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != InstancingSprite.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'viewProj'
			this.vreg_change = (this.vreg_change | state.SetViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				InstancingSprite.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref InstancingSprite.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((InstancingSprite.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((InstancingSprite.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			InstancingSprite.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out InstancingSprite.fx, InstancingSprite.fxb, 36, 6);
		}
		/// <summary>True if a shader constant has changed since the last Bind()</summary>
		protected override bool Changed()
		{
			return (this.vreg_change | this.ptc);
		}
		/// <summary>Returns the number of vertex inputs used by this shader</summary>
		protected override int GetVertexInputCountImpl()
		{
			return 5;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(InstancingSprite.vin[i]));
			index = InstancingSprite.vin[(i + 5)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary>Return the supported modes for this shader</summary><param name="blendingSupport"/><param name="instancingSupport"/>
		protected override void GetExtensionSupportImpl(out bool blendingSupport, out bool instancingSupport)
		{
			blendingSupport = false;
			instancingSupport = true;
		}
		/// <summary>Name ID for 'spriteWorldMatrix'</summary>
		private static int cid0;
		/// <summary>Set the shader value 'float4x4 spriteWorldMatrix'</summary><param name="value"/>
		public void SetSpriteWorldMatrix(ref Microsoft.Xna.Framework.Matrix value)
		{
			this.vreg[4] = new Microsoft.Xna.Framework.Vector4(value.M11, value.M21, value.M31, value.M41);
			this.vreg[5] = new Microsoft.Xna.Framework.Vector4(value.M12, value.M22, value.M32, value.M42);
			this.vreg[6] = new Microsoft.Xna.Framework.Vector4(value.M13, value.M23, value.M33, value.M43);
			this.vreg[7] = new Microsoft.Xna.Framework.Vector4(value.M14, value.M24, value.M34, value.M44);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float4x4 spriteWorldMatrix'</summary>
		public Microsoft.Xna.Framework.Matrix SpriteWorldMatrix
		{
			set
			{
				this.SetSpriteWorldMatrix(ref value);
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'viewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D CustomTextureSampler'</summary>
		public Xen.Graphics.TextureSamplerState CustomTextureSampler
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
		/// <summary>Get/Set the Bound texture for 'Texture2D CustomTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D CustomTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D CustomTextureSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D CustomTexture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0,0,0,0,0,12,13,14,15};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[8];
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
				return new byte[] {4,188,240,11,207,131,0,1,24,144,0,8,254,255,9,1,0,0,1,80,135,0,1,3,131,0,1,1,131,0,1,160,135,0,1,8,131,0,1,4,131,0,1,1,229,0,0,158,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,131,0,0,1,196,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,3,131,0,0,1,16,131,0,0,1,4,143,0,0,1,4,131,0,0,1,15,131,0,0,1,4,143,0,0,1,11,1,73,1,110,1,115,1,116,1,97,1,110,1,99,1,105,1,110,1,103,133,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,2,131,0,0,1,1,131,0,0,1,7,131,0,0,1,5,131,0,0,1,4,131,0,0,1,32,139,0,0,1,172,131,0,0,1,192,138,0,0,1,1,1,68,135,0,0,1,2,1,0,1,0,1,1,136,0,0,1,2,131,0,0,1,92,135,0,0,1,212,131,0,0,1,208,131,0,0,1,93,135,0,0,1,236,131,0,0,1,232,1,0,1,0,1,1,1,52,135,0,0,1,2,131,0,0,1,92,134,0,0,1,1,1,8,1,0,1,0,1,1,1,4,131,0,0,1,93,134,0,0,1,1,1,32,1,0,1,0,1,1,1,28,135,0,0,1,4,135,0,0,1,1,132,255,0,131,0,0,1,1,135,0,0,1,232,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,60,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,60,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,16,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,0,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,1,128,133,0,0,1,225,1,0,1,1,148,0,0,1,1,132,255,0,138,0,0,1,3,1,80,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,120,1,0,1,0,1,1,1,216,135,0,0,1,36,1,0,1,0,1,1,1,4,1,0,1,0,1,1,1,44,139,0,0,1,220,131,0,0,1,28,131,0,0,1,207,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,200,131,0,0,1,48,1,0,1,2,131,0,0,1,8,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,8,229,0,0,161,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,152,1,0,1,17,1,0,1,8,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,5,131,0,0,1,2,1,0,1,0,1,2,1,144,1,0,1,28,1,0,1,4,1,0,1,13,1,0,1,5,1,0,1,14,1,0,1,6,1,0,1,15,1,0,1,7,1,0,1,32,1,0,1,8,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,31,1,0,1,0,1,16,1,32,160,0,0,1,192,1,73,1,15,1,219,1,63,131,0,0,1,62,1,34,1,249,1,131,1,63,1,128,1,0,1,0,1,64,1,201,1,15,1,219,140,0,0,1,241,1,85,1,80,1,4,1,0,1,0,1,18,1,1,1,194,133,0,0,1,96,1,9,1,96,1,15,1,18,1,0,1,18,133,0,0,1,96,1,21,1,32,1,27,1,18,1,0,1,18,135,0,0,1,64,1,29,1,196,1,0,1,34,131,0,0,1,5,1,248,1,96,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,48,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,32,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,112,131,0,0,1,14,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,4,1,67,132,0,0,1,200,1,12,1,0,1,5,1,0,1,91,1,91,1,0,1,226,1,1,1,1,1,0,1,200,1,1,131,0,0,1,108,1,198,1,177,1,139,1,7,1,254,1,254,1,20,1,18,131,0,0,1,108,1,0,1,27,1,232,1,0,1,0,1,1,1,200,1,4,131,0,0,1,177,1,108,1,108,1,139,1,0,1,255,1,254,1,196,1,35,1,0,1,4,1,2,1,197,1,197,1,198,1,224,1,1,1,7,1,0,1,192,1,76,1,0,1,4,1,0,1,172,1,6,1,198,1,225,1,4,1,6,1,0,1,20,1,3,1,0,2,4,0,3,111,26,198,4,161,1,4,4,5,12,135,8,8,1,6,26,193,198,225,4,0,7,0,168,70,4,0,0,28,8,182,192,192,8,8,6,200,3,131,0,8,197,26,197,235,7,6,0,172,9,131,4,5,0,176,176,0,192,0,4,6,6,200,1,131,0,9,176,176,108,177,5,4,4,200,4,131,0,9,176,176,198,177,5,6,4,200,2,131,0,9,208,208,0,175,5,5,0,200,8,131,0,9,208,208,0,175,5,7,0,200,5,131,0,10,196,25,0,224,0,4,0,200,1,128,6,62,0,167,167,0,175,131,0,11,200,2,128,62,0,167,167,0,175,0,1,12,0,200,4,128,62,0,167,167,0,175,0,2,13,0,200,8,128,62,0,167,167,0,175,0,3,0,14,20,1,0,0,4,198,27,177,160,1,254,1,12,18,131,0,14,108,27,198,225,0,3,3,200,3,128,0,0,176,176,9,0,224,0,3,0,200,15,128,1,132,0,3,226,2,2,149,0,0,132,255,0,131,0,0,1,1,135,0,0,1,232,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,60,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,2,0,1,3,0,1,0,1,1,134,0,1,112,2,115,95,3,51,95,48,4,0,50,46,48,5,46,49,49,54,50,5,54,46,48,0,171,135,0,2,60,16,2,0,1,132,0,1,4,134,0,0,1,24,1,66,2,0,3,2,0,3,131,0,3,1,0,0,4,48,80,0,0,5,241,81,0,1,16,6,2,0,0,18,0,196,133,0,4,16,3,0,0,1,34,133,0,3,16,8,0,4,1,31,31,246,5,136,0,0,64,0,3,200,15,128,133,0,3,225,0,1,149,0,0,132,255,0,138,0,0,1,3,1,80,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,120,1,0,1,0,1,1,1,216,135,0,0,1,36,1,0,1,0,1,1,1,4,1,0,1,0,1,1,1,44,139,0,0,1,220,131,0,0,1,28,131,0,0,1,207,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,200,131,0,0,1,48,1,0,1,2,131,0,0,1,8,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,2,99,0,3,171,171,0,4,1,0,3,0,5,1,0,4,0,8,229,0,0,161,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,152,1,0,1,17,1,0,1,8,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,5,131,0,0,1,2,1,0,1,0,1,2,1,144,1,0,1,28,1,0,1,4,1,0,1,13,1,0,1,5,1,0,1,14,1,0,1,6,1,0,1,15,1,0,1,7,1,0,1,32,1,0,1,8,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,31,1,0,1,0,1,16,1,32,160,0,0,1,192,1,73,1,15,1,219,1,63,131,0,0,1,62,1,34,1,249,1,131,1,63,1,128,1,0,1,0,1,64,1,201,1,15,1,219,140,0,0,1,241,1,85,1,80,1,4,1,0,1,0,1,18,1,1,1,194,133,0,0,1,96,1,9,1,96,1,15,1,18,1,0,1,18,133,0,0,1,96,1,21,1,32,1,27,1,18,1,0,1,18,135,0,0,1,64,1,29,1,196,1,0,1,34,131,0,0,1,5,1,248,1,96,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,48,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,32,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,112,131,0,0,1,14,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,4,1,67,132,0,0,1,200,1,12,1,0,1,5,1,0,1,91,1,91,1,0,1,226,1,1,1,1,1,0,1,200,1,1,131,0,0,1,108,1,198,1,177,1,139,1,7,1,254,1,254,1,20,1,18,131,0,0,1,108,1,0,1,27,1,232,1,0,1,0,1,1,1,200,1,4,131,0,0,1,177,1,108,1,108,1,139,1,0,1,255,1,254,1,196,1,35,1,0,1,4,1,2,1,197,1,197,1,198,1,224,1,1,1,7,1,0,1,192,1,76,1,0,1,4,1,0,1,172,1,6,1,198,1,225,2,4,6,3,0,20,3,4,0,4,0,111,5,26,198,161,1,4,6,4,12,135,8,8,1,7,26,193,198,225,4,0,0,8,168,70,4,0,0,28,182,192,6,192,8,8,6,200,3,131,0,9,197,26,197,235,7,6,0,172,131,10,4,5,0,176,176,0,192,0,6,6,2,200,1,131,0,9,176,176,108,177,5,4,4,200,4,131,0,9,176,176,198,177,5,6,4,200,2,131,0,9,208,208,0,175,5,5,0,200,8,131,0,9,208,208,0,175,5,7,0,200,5,131,0,11,196,25,0,224,0,4,0,200,1,128,62,5,0,167,167,0,175,131,0,12,200,2,128,62,0,167,167,0,175,0,1,0,13,200,4,128,62,0,167,167,0,175,0,2,0,200,14,8,128,62,0,167,167,0,175,0,3,0,20,1,0,11,0,4,198,27,177,160,1,254,1,12,18,131,0,15,108,27,198,225,0,3,3,200,3,128,0,0,176,176,0,8,224,0,3,0,200,15,128,1,132,0,3,226,2,2,140,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {208,10,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,187,95,163,255,252,154,63,246,127,255,95,111,126,77,249,253,215,198,223,244,255,191,72,191,251,13,232,255,191,142,126,246,115,253,252,122,244,255,223,255,178,249,253,167,244,243,55,250,53,4,175,127,172,211,230,215,71,155,85,243,251,55,59,130,243,111,166,237,252,231,215,162,255,255,166,145,207,205,24,65,131,223,44,242,61,254,142,189,247,27,210,255,207,150,77,155,45,167,197,242,66,112,120,61,207,102,121,45,125,1,46,62,251,117,245,221,212,123,247,175,163,255,255,67,222,223,223,86,36,240,158,65,8,191,255,41,250,253,191,70,255,255,151,232,255,127,170,254,253,223,209,255,255,43,250,255,65,164,237,111,68,159,253,6,191,166,107,251,187,209,239,169,182,51,99,192,159,255,55,61,102,236,128,255,107,252,218,255,247,255,253,127,253,223,191,235,175,113,242,230,248,201,239,68,127,126,169,159,161,13,254,166,39,253,54,253,179,243,107,24,126,249,181,126,141,3,125,95,105,255,215,254,58,52,67,191,38,255,39,15,125,124,239,247,223,249,53,190,40,166,117,213,84,231,109,186,245,234,78,250,237,231,175,159,167,66,169,244,164,90,172,138,146,126,121,56,222,251,116,252,240,254,222,120,239,96,127,255,215,248,93,104,72,68,183,63,136,122,250,147,228,247,95,243,15,250,53,127,141,223,148,127,39,176,127,210,175,241,27,252,166,127,209,19,70,227,55,165,54,255,25,253,253,159,253,69,191,46,254,254,13,248,111,106,251,159,253,73,255,247,255,45,56,152,177,42,74,191,198,115,32,255,107,255,95,52,214,99,59,214,127,241,215,144,207,252,177,254,83,191,134,140,21,29,254,6,58,214,111,255,26,134,23,255,218,191,246,215,164,238,126,77,162,232,111,96,0,255,28,63,151,95,147,214,63,65,44,250,27,252,26,191,233,95,244,135,254,170,143,142,8,204,239,241,239,254,166,255,244,239,249,239,254,166,103,255,16,62,255,49,250,156,166,225,247,160,255,255,131,166,31,153,131,223,232,15,250,53,236,124,252,198,222,220,252,38,127,208,175,101,127,255,77,255,160,95,219,205,217,31,244,235,120,191,211,119,255,145,153,99,162,163,254,254,107,226,221,255,232,215,97,54,253,181,169,253,127,246,39,253,216,175,241,159,253,69,63,246,107,252,39,127,17,62,67,59,153,219,255,225,79,194,252,74,187,95,147,250,248,53,254,164,223,128,84,215,111,240,107,124,245,23,253,150,12,255,215,252,131,164,63,249,30,191,255,6,191,198,95,67,223,255,223,127,209,239,78,223,19,28,254,140,249,133,126,255,53,127,141,255,248,15,250,177,95,227,63,254,139,126,77,254,238,55,162,191,159,254,65,248,253,215,82,28,8,224,175,141,223,127,45,234,251,215,254,53,254,235,63,249,215,245,254,254,53,24,151,95,227,215,250,181,153,30,255,217,159,132,239,0,255,215,254,53,254,7,180,229,239,8,13,130,247,149,194,252,117,168,47,224,138,190,127,45,133,241,159,253,65,242,55,198,255,175,80,187,255,237,15,66,159,191,6,227,201,223,43,156,223,72,105,130,62,126,77,254,238,215,97,250,224,239,95,139,255,254,117,237,223,191,14,255,253,235,217,191,127,3,254,251,215,215,191,169,175,255,232,215,20,216,250,247,175,197,127,255,154,246,239,95,135,255,254,181,236,223,191,1,255,253,107,211,223,50,142,223,244,63,250,181,60,25,195,227,235,147,255,8,255,252,90,17,125,242,107,125,176,62,217,251,58,60,238,248,238,215,254,155,204,239,196,179,230,247,174,62,249,155,172,62,145,191,65,151,191,137,231,64,245,75,119,220,230,247,63,0,3,249,181,34,186,229,215,250,255,188,110,249,90,116,127,95,221,130,207,19,250,252,215,252,141,255,165,191,61,251,13,255,142,191,243,175,253,107,190,117,248,71,255,81,127,212,67,124,254,27,210,231,127,237,95,243,215,252,125,212,236,31,144,247,126,141,223,227,67,116,209,144,158,249,53,190,25,61,243,235,24,61,147,16,236,223,240,215,48,252,244,243,86,223,252,67,29,125,243,15,117,244,205,63,212,209,55,255,144,175,111,126,77,79,223,252,63,1,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Matrix' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Matrix value)
		{
			if ((InstancingSprite.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == InstancingSprite.cid0))
			{
				this.SetSpriteWorldMatrix(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((InstancingSprite.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == InstancingSprite.sid0))
			{
				this.CustomTextureSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((InstancingSprite.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == InstancingSprite.tid0))
			{
				this.CustomTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'NonInstancingSprite' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 47 instruction slots used, 244 registers</para><para>Pixel Shader: approximately 3 instruction slots used (1 texture, 2 arithmetic), 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class NonInstancingSprite : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'NonInstancingSprite' shader</summary>
		public NonInstancingSprite()
		{
			this.sc0 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(192));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			NonInstancingSprite.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			NonInstancingSprite.cid0 = state.GetNameUniqueID("instances");
			NonInstancingSprite.sid0 = state.GetNameUniqueID("CustomTextureSampler");
			NonInstancingSprite.tid0 = state.GetNameUniqueID("CustomTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != NonInstancingSprite.gd))
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
				NonInstancingSprite.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref NonInstancingSprite.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((NonInstancingSprite.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((NonInstancingSprite.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			NonInstancingSprite.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out NonInstancingSprite.fx, NonInstancingSprite.fxb, 47, 6);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(NonInstancingSprite.vin[i]));
			index = NonInstancingSprite.vin[(i + 2)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'instances'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float4x4 instances[60]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetInstances(Microsoft.Xna.Framework.Matrix[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Matrix val;
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
						> 60)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 60)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 4) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.M11, val.M21, val.M31, val.M41);
				this.vreg[((wi * 4) 
							+ 1)] = new Microsoft.Xna.Framework.Vector4(val.M12, val.M22, val.M32, val.M42);
				this.vreg[((wi * 4) 
							+ 2)] = new Microsoft.Xna.Framework.Vector4(val.M13, val.M23, val.M33, val.M43);
				this.vreg[((wi * 4) 
							+ 3)] = new Microsoft.Xna.Framework.Vector4(val.M14, val.M24, val.M34, val.M44);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4x4 instances[60]'</summary>
		public Microsoft.Xna.Framework.Matrix[] Instances
		{
			set
			{
				this.SetInstances(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D CustomTextureSampler'</summary>
		public Xen.Graphics.TextureSamplerState CustomTextureSampler
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
		/// <summary>Get/Set the Bound texture for 'Texture2D CustomTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D CustomTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D CustomTextureSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D CustomTexture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[244];
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
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,15,208,135,0,1,3,131,0,5,1,0,0,15,96,135,0,1,244,131,0,1,4,131,0,1,1,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,197,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,12,131,0,0,1,4,1,0,1,0,1,15,1,132,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,2,131,0,0,1,1,131,0,0,1,4,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,138,0,0,1,15,1,108,1,0,1,0,1,15,1,128,138,0,0,1,15,1,196,135,0,0,1,1,1,0,1,0,1,15,1,192,135,0,0,1,2,131,0,0,1,92,134,0,0,1,15,1,148,1,0,1,0,1,15,1,144,131,0,0,1,93,134,0,0,1,15,1,172,1,0,1,0,1,15,1,168,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,135,0,0,1,232,1,16,1,42,1,17,132,0,0,1,172,131,0,0,1,60,135,0,0,1,36,135,0,0,1,132,139,0,0,1,92,131,0,0,1,28,131,0,0,1,79,1,255,1,255,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,72,131,0,0,1,48,1,0,1,3,131,0,0,1,1,133,0,0,1,56,132,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,60,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,24,1,66,1,0,1,3,1,0,1,3,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,241,1,81,1,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,16,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,0,1,1,1,31,1,31,1,246,1,136,1,0,1,0,1,64,1,0,1,200,1,15,1,128,133,0,0,1,225,1,0,1,1,149,0,0,132,255,0,138,0,0,1,18,1,80,1,16,1,42,1,17,1,1,1,0,1,0,1,16,1,48,1,0,1,0,1,2,1,32,135,0,0,1,36,1,0,1,0,1,15,1,196,1,0,1,0,1,15,1,236,138,0,0,1,15,1,156,131,0,0,1,28,1,0,1,0,1,15,1,143,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,134,0,0,1,15,1,136,131,0,0,1,48,1,0,1,2,131,0,0,1,244,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,244,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,200,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,224,1,0,1,17,1,0,1,4,138,0,0,1,24,1,66,131,0,0,1,1,131,0,0,1,2,131,0,0,1,3,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,5,1,0,1,48,1,80,1,6,1,0,1,0,1,48,1,80,1,0,1,1,1,241,1,81,1,0,1,0,1,16,1,36,131,0,0,1,37,1,0,1,0,1,16,1,38,160,0,0,1,64,1,201,1,15,1,219,1,192,1,73,1,15,1,219,1,62,1,34,1,249,1,131,136,0,0,1,63,1,128,1,0,1,0,1,64,1,128,1,0,1,0,1,63,131,0,0,1,48,1,5,1,32,1,5,1,0,1,0,1,18,1,0,1,194,133,0,0,1,96,1,7,1,96,1,13,1,18,1,0,1,18,133,0,0,1,96,1,19,1,96,1,25,1,18,1,0,1,18,133,0,0,1,64,1,31,1,0,1,0,1,18,1,0,1,196,133,0,0,1,64,1,35,1,0,1,0,1,34,133,0,0,1,5,1,248,1,48,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,15,1,248,132,0,0,1,200,1,2,1,0,1,1,1,0,1,199,1,199,1,108,1,145,1,3,1,242,1,255,1,200,1,4,1,0,1,1,1,0,1,199,1,199,1,108,1,145,1,3,1,243,1,255,1,200,1,4,1,0,1,2,1,0,1,199,1,199,1,108,1,145,1,3,1,240,1,255,1,200,1,8,1,0,1,2,1,0,1,199,1,199,1,108,1,145,1,3,1,241,1,255,1,48,1,16,133,0,0,1,108,1,226,131,0,0,1,168,1,16,1,1,132,0,0,1,192,1,194,1,0,1,0,1,255,1,92,1,2,1,0,1,2,1,4,1,177,1,177,1,108,1,160,1,3,1,255,1,1,1,200,1,2,1,0,1,0,1,160,1,108,1,177,1,0,1,161,1,3,1,2,1,0,1,200,1,1,1,0,1,2,1,162,1,177,1,27,1,0,1,160,1,3,1,2,1,0,1,200,1,4,1,0,1,0,1,162,1,108,1,27,1,0,1,160,1,3,1,1,1,0,1,200,1,1,1,0,1,0,1,160,1,27,1,198,1,27,1,11,1,0,1,254,1,255,1,44,1,20,1,0,1,0,1,160,1,198,1,108,1,108,1,161,1,0,1,2,1,0,1,200,1,8,131,0,0,1,108,1,108,1,177,1,139,1,0,1,254,1,254,1,196,1,38,1,2,1,3,1,160,1,188,1,188,1,27,1,161,1,2,1,3,1,0,1,192,1,17,1,2,1,0,1,0,1,198,1,177,1,27,1,225,1,0,1,2,1,0,1,200,1,4,1,0,1,0,1,2,1,198,1,108,1,0,1,225,1,0,1,2,1,0,1,200,1,5,131,0,0,1,177,1,176,1,196,1,235,1,3,1,2,1,0,1,200,1,1,1,0,1,3,1,224,1,27,1,108,1,198,1,43,1,2,1,3,1,0,1,200,1,1,1,0,1,0,1,224,1,27,1,108,1,108,1,43,1,1,1,2,1,0,1,200,1,3,1,0,1,0,1,160,1,176,1,176,1,0,1,160,131,0,0,1,200,1,12,1,0,1,0,1,160,1,236,1,172,1,0,1,160,1,3,1,1,1,0,1,200,1,3,1,0,1,3,1,0,1,196,1,176,1,0,1,161,1,0,1,241,1,0,1,200,1,3,1,0,1,4,1,0,1,196,1,176,1,0,1,161,1,0,1,243,1,0,1,200,1,12,1,0,1,4,1,0,1,236,1,172,1,0,1,161,1,0,1,242,1,0,1,0,1,28,1,2,1,3,1,0,1,236,1,172,1,97,1,161,1,0,1,240,1,4,1,0,1,35,1,2,1,3,1,0,1,196,1,25,1,203,1,224,1,3,1,3,1,4,1,200,1,3,1,128,1,62,1,0,1,109,1,26,1,0,1,224,1,3,1,2,1,0,1,200,1,12,1,128,1,62,1,0,1,113,1,241,1,0,1,224,1,2,1,1,1,0,1,92,134,0,0,1,108,1,226,1,0,1,0,1,1,1,200,1,3,1,128,1,0,1,0,1,25,1,25,1,0,1,226,131,0,0,1,200,1,3,1,128,1,1,1,96,1,176,1,198,1,198,1,12,1,255,1,0,1,1,1,200,1,12,1,128,1,1,1,96,1,172,1,198,1,198,1,12,1,255,1,2,1,3,139,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {92,36,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,191,246,155,202,239,191,54,254,166,255,255,1,250,247,47,167,255,255,58,250,217,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,230,231,215,163,255,255,254,151,205,239,63,253,53,126,139,95,247,55,250,53,36,174,254,195,52,206,54,207,175,143,54,171,230,247,111,118,36,230,254,205,180,157,255,252,90,244,127,188,214,253,92,98,244,255,251,255,6,140,215,243,108,150,215,210,22,159,163,237,175,173,63,83,239,157,146,0,253,65,30,14,255,148,254,142,119,254,33,253,29,48,254,20,253,254,79,163,207,254,36,250,255,159,170,127,255,117,244,251,95,229,181,51,207,255,77,143,201,25,252,71,252,229,255,253,127,255,95,255,247,239,250,107,156,188,57,126,242,59,209,159,95,234,103,104,131,191,233,73,191,77,255,236,252,26,38,7,241,107,253,26,7,250,190,210,227,175,253,117,126,141,223,136,62,255,53,117,156,191,198,175,65,31,239,253,254,59,191,198,23,197,180,174,154,234,188,77,183,94,221,73,191,253,252,245,243,84,70,159,158,84,139,85,81,210,47,15,199,123,159,142,31,222,223,27,239,29,236,239,255,26,191,139,160,250,7,81,79,127,147,249,253,215,252,53,126,83,243,251,159,244,107,252,6,191,233,95,244,132,209,248,77,169,205,127,246,55,253,26,191,193,127,246,23,253,186,238,111,106,251,159,253,77,191,38,218,254,6,242,247,255,253,127,43,66,191,134,140,219,252,254,63,253,22,244,207,175,245,127,209,184,255,231,95,219,140,251,79,252,77,229,51,127,220,127,212,111,42,227,70,231,191,92,199,253,237,95,195,240,202,95,251,215,254,154,212,245,175,73,51,135,92,204,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,115,219,231,231,123,187,203,175,153,55,248,137,95,227,215,248,117,127,249,175,241,155,254,69,148,46,248,61,255,208,95,245,209,17,129,250,61,232,247,223,3,159,255,10,250,252,223,253,77,255,233,223,243,223,253,77,207,254,33,250,236,31,52,159,255,175,210,254,247,144,207,228,193,231,191,146,62,255,53,127,227,127,233,111,207,126,195,191,227,239,252,107,255,154,111,29,254,209,127,212,31,245,16,159,255,111,244,249,95,251,215,252,53,127,31,53,251,7,228,189,95,227,247,112,249,138,223,244,79,194,239,191,174,228,43,254,164,95,135,211,57,191,54,125,254,159,253,73,255,235,175,241,159,253,69,255,235,175,241,159,252,69,191,17,231,41,126,29,250,254,215,248,147,232,255,127,242,111,201,239,254,6,252,55,253,242,107,83,246,132,218,255,223,127,48,254,150,182,191,1,255,77,255,255,131,4,222,175,67,127,255,53,248,12,239,252,65,191,174,194,195,103,148,127,248,139,198,12,239,215,252,155,240,247,175,201,240,126,195,63,232,215,74,255,51,26,227,175,241,55,209,203,212,252,215,252,143,126,13,198,239,171,63,232,215,72,191,226,207,165,221,175,245,7,253,154,250,183,180,251,181,168,221,87,127,208,175,109,219,124,197,240,232,125,106,247,127,219,247,40,107,70,240,255,57,254,251,215,226,191,121,188,127,48,198,252,107,42,254,191,118,250,107,240,247,191,174,253,254,127,248,131,36,79,163,227,83,120,50,190,223,128,199,246,203,127,141,175,254,162,95,254,107,252,53,127,145,161,143,27,191,252,254,43,104,172,191,130,218,252,238,244,217,175,197,48,255,239,63,232,87,18,141,255,183,95,67,114,65,191,54,125,246,107,253,26,255,49,181,251,31,254,34,206,5,17,141,126,249,175,241,127,227,59,106,207,244,183,227,248,13,44,157,255,51,122,7,255,71,27,158,7,194,205,226,254,235,252,218,252,222,255,109,199,35,239,253,154,252,222,175,109,223,251,181,232,111,192,150,54,232,251,215,250,53,126,35,180,249,147,126,76,218,255,67,232,227,127,34,60,229,239,95,139,255,254,159,237,223,191,14,255,253,191,216,191,127,3,254,251,151,209,223,50,79,52,127,233,95,99,96,99,222,254,163,95,51,248,251,215,249,143,126,173,224,239,223,224,63,250,181,245,111,228,193,254,159,0,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Matrix[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Matrix[] value)
		{
			if ((NonInstancingSprite.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == NonInstancingSprite.cid0))
			{
				this.SetInstances(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((NonInstancingSprite.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == NonInstancingSprite.sid0))
			{
				this.CustomTextureSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((NonInstancingSprite.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == NonInstancingSprite.tid0))
			{
				this.CustomTexture = value;
				return true;
			}
			return false;
		}
	}
}
