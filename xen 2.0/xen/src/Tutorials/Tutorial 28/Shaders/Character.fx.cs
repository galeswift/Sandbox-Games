// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = Character.fx
// Namespace = Tutorials.Tutorial_28.Shaders

namespace Tutorials.Tutorial_28.Shaders
{
	
	/// <summary><para>Technique 'Character' generated from file 'Character.fx'</para><para>Vertex Shader: approximately 23 instruction slots used, 13 registers</para><para>Pixel Shader: approximately 148 instruction slots used (12 texture, 136 arithmetic), 17 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	public sealed class Character : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'Character' shader</summary>
		public Character()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.sc3 = -1;
			this.sc4 = -1;
			this.gc0 = -1;
			this.gc1 = -1;
			this.gc2 = -1;
			this.gc3 = -1;
			this.gc4 = -1;
			this.gc5 = -1;
			this.gc6 = -1;
			this.gc7 = -1;
			this.gc8 = -1;
			this.gc9 = -1;
			this.pts[3] = ((Xen.Graphics.TextureSamplerState)(7296));
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(213));
			this.pts[4] = ((Xen.Graphics.TextureSamplerState)(7296));
			this.pts[1] = ((Xen.Graphics.TextureSamplerState)(64));
			this.pts[2] = ((Xen.Graphics.TextureSamplerState)(192));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			Character.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			Character.gid0 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Matrix>("ShadowMapProjection");
			Character.gid1 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Vector3>("AmbientDiffuseSpecularScale");
			Character.gid2 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Vector4[]>("EnvironmentSH");
			Character.gid3 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Vector2>("RgbmImageRenderScale");
			Character.gid4 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Vector2>("ShadowMapSize");
			Character.gid5 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Vector3>("SkinLightScatter");
			Character.gid6 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Vector3>("SunDirection");
			Character.gid7 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Vector4>("SunRgbIntensity");
			Character.gid8 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Vector2>("SunSpecularPowerIntensity");
			Character.gid9 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Vector3>("UseAlbedoOcclusionShadow");
			Character.sid0 = state.GetNameUniqueID("AlbedoSampler");
			Character.sid1 = state.GetNameUniqueID("CubeRgbmSampler");
			Character.sid2 = state.GetNameUniqueID("NormalSampler");
			Character.sid3 = state.GetNameUniqueID("ShadowSampler");
			Character.sid4 = state.GetNameUniqueID("SofSampler");
			Character.tid0 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Graphics.TextureCube>("CubeRgbmTexture");
			Character.tid1 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Graphics.Texture2D>("ShadowTexture");
			Character.tid2 = state.GetNameUniqueID("SofTexture");
			Character.tid3 = state.GetNameUniqueID("AlbedoTexture");
			Character.tid4 = state.GetNameUniqueID("NormalTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != Character.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.preg_change = (this.preg_change | ic);
			this.vbreg_change = (this.vbreg_change | ic);
			this.vireg_change = (this.vireg_change | ic);
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector4(ref this.vreg[12], ref this.sc0));
			// Set the value for attribute 'world'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[8], ref this.vreg[9], ref this.vreg[10], ref this.vreg[11], ref this.sc1));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[4], ref this.vreg[5], ref this.vreg[6], ref this.vreg[7], ref this.sc2));
			// Set the value for global 'ShadowMapProjection'
			this.vreg_change = (this.vreg_change | state.SetGlobalMatrix4(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], Character.gid0, ref this.gc0));
			// Set the value for global 'AmbientDiffuseSpecularScale'
			this.preg_change = (this.preg_change | state.SetGlobalVector3(ref this.preg[15], Character.gid1, ref this.gc1));
			// Set the value for global 'EnvironmentSH'
			this.preg_change = (this.preg_change | state.SetGlobalVector4(this.preg, 0, 9, Character.gid2, ref this.gc2));
			// Set the value for global 'RgbmImageRenderScale'
			this.preg_change = (this.preg_change | state.SetGlobalVector2(ref this.preg[9], Character.gid3, ref this.gc3));
			// Set the value for global 'ShadowMapSize'
			this.preg_change = (this.preg_change | state.SetGlobalVector2(ref this.preg[10], Character.gid4, ref this.gc4));
			// Set the value for global 'SkinLightScatter'
			this.preg_change = (this.preg_change | state.SetGlobalVector3(ref this.preg[11], Character.gid5, ref this.gc5));
			// Set the value for global 'SunDirection'
			this.preg_change = (this.preg_change | state.SetGlobalVector3(ref this.preg[13], Character.gid6, ref this.gc6));
			// Set the value for global 'SunRgbIntensity'
			this.preg_change = (this.preg_change | state.SetGlobalVector4(ref this.preg[12], Character.gid7, ref this.gc7));
			// Set the value for global 'SunSpecularPowerIntensity'
			this.preg_change = (this.preg_change | state.SetGlobalVector2(ref this.preg[14], Character.gid8, ref this.gc8));
			// Set the value for global 'UseAlbedoOcclusionShadow'
			this.preg_change = (this.preg_change | state.SetGlobalVector3(ref this.preg[16], Character.gid9, ref this.gc9));
			// Assign global textures
			this.CubeRgbmTexture = state.GetGlobalTextureCube(Character.tid0);
			this.ShadowTexture = state.GetGlobalTexture2D(Character.tid1);
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				Character.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((this.preg_change == true))
			{
				Character.fx.ps_c.SetValue(this.preg);
				this.preg_change = false;
				ic = true;
			}
			if ((ext == Xen.Graphics.ShaderSystem.ShaderExtension.Blending))
			{
				ic = (ic | state.SetBlendMatricesDirect(Character.fx.vsb_c, ref this.sc3));
			}
			if ((ext == Xen.Graphics.ShaderSystem.ShaderExtension.Instancing))
			{
				this.vireg_change = (this.vireg_change | state.SetViewProjectionMatrix(ref this.vireg[0], ref this.vireg[1], ref this.vireg[2], ref this.vireg[3], ref this.sc4));
				if ((this.vireg_change == true))
				{
					Character.fx.vsi_c.SetValue(this.vireg);
					this.vireg_change = false;
					ic = true;
				}
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref Character.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((Character.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((Character.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			Character.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out Character.fx, Character.fxb, 35, 146);
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
			return 5;
		}
		/// <summary>Returns a vertex input used by this shader</summary><param name="i"/><param name="usage"/><param name="index"/>
		protected override void GetVertexInputImpl(int i, out Microsoft.Xna.Framework.Graphics.VertexElementUsage usage, out int index)
		{
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(Character.vin[i]));
			index = Character.vin[(i + 5)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary/>
		private bool preg_change;
		/// <summary/>
		private bool vbreg_change;
		/// <summary/>
		private bool vireg_change;
		/// <summary>Return the supported modes for this shader</summary><param name="blendingSupport"/><param name="instancingSupport"/>
		protected override void GetExtensionSupportImpl(out bool blendingSupport, out bool instancingSupport)
		{
			blendingSupport = true;
			instancingSupport = true;
		}
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'world'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc2;
		/// <summary>Change ID for Semantic bound attribute '__BLENDMATRICES__GENMATRIX'</summary>
		private int sc3;
		/// <summary>Change ID for Semantic bound attribute '__VIEWPROJECTION__GENMATRIX'</summary>
		private int sc4;
		/// <summary>TypeID for global attribute 'float4x4 ShadowMapProjection'</summary>
		private static int gid0;
		/// <summary>Change ID for global attribute 'float4x4 ShadowMapProjection'</summary>
		private int gc0;
		/// <summary>TypeID for global attribute 'float3 AmbientDiffuseSpecularScale'</summary>
		private static int gid1;
		/// <summary>Change ID for global attribute 'float3 AmbientDiffuseSpecularScale'</summary>
		private int gc1;
		/// <summary>TypeID for global attribute 'float4 EnvironmentSH'</summary>
		private static int gid2;
		/// <summary>Change ID for global attribute 'float4 EnvironmentSH'</summary>
		private int gc2;
		/// <summary>TypeID for global attribute 'float2 RgbmImageRenderScale'</summary>
		private static int gid3;
		/// <summary>Change ID for global attribute 'float2 RgbmImageRenderScale'</summary>
		private int gc3;
		/// <summary>TypeID for global attribute 'float2 ShadowMapSize'</summary>
		private static int gid4;
		/// <summary>Change ID for global attribute 'float2 ShadowMapSize'</summary>
		private int gc4;
		/// <summary>TypeID for global attribute 'float3 SkinLightScatter'</summary>
		private static int gid5;
		/// <summary>Change ID for global attribute 'float3 SkinLightScatter'</summary>
		private int gc5;
		/// <summary>TypeID for global attribute 'float3 SunDirection'</summary>
		private static int gid6;
		/// <summary>Change ID for global attribute 'float3 SunDirection'</summary>
		private int gc6;
		/// <summary>TypeID for global attribute 'float4 SunRgbIntensity'</summary>
		private static int gid7;
		/// <summary>Change ID for global attribute 'float4 SunRgbIntensity'</summary>
		private int gc7;
		/// <summary>TypeID for global attribute 'float2 SunSpecularPowerIntensity'</summary>
		private static int gid8;
		/// <summary>Change ID for global attribute 'float2 SunSpecularPowerIntensity'</summary>
		private int gc8;
		/// <summary>TypeID for global attribute 'float3 UseAlbedoOcclusionShadow'</summary>
		private static int gid9;
		/// <summary>Change ID for global attribute 'float3 UseAlbedoOcclusionShadow'</summary>
		private int gc9;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D AlbedoSampler'</summary>
		public Xen.Graphics.TextureSamplerState AlbedoSampler
		{
			get
			{
				return this.pts[3];
			}
			set
			{
				if ((value != this.pts[3]))
				{
					this.pts[3] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'SamplerCUBE CubeRgbmSampler'</summary>
		public Xen.Graphics.TextureSamplerState CubeRgbmSampler
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
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D NormalSampler'</summary>
		public Xen.Graphics.TextureSamplerState NormalSampler
		{
			get
			{
				return this.pts[4];
			}
			set
			{
				if ((value != this.pts[4]))
				{
					this.pts[4] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D ShadowSampler'</summary>
		public Xen.Graphics.TextureSamplerState ShadowSampler
		{
			get
			{
				return this.pts[1];
			}
			set
			{
				if ((value != this.pts[1]))
				{
					this.pts[1] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D SofSampler'</summary>
		public Xen.Graphics.TextureSamplerState SofSampler
		{
			get
			{
				return this.pts[2];
			}
			set
			{
				if ((value != this.pts[2]))
				{
					this.pts[2] = value;
					this.ptc = true;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'TextureCube CubeRgbmTexture'</summary>
		private Microsoft.Xna.Framework.Graphics.TextureCube CubeRgbmTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.TextureCube)(this.ptx[0]));
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
		/// <summary>Get/Set the Bound texture for 'Texture2D ShadowTexture'</summary>
		private Microsoft.Xna.Framework.Graphics.Texture2D ShadowTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[1]));
			}
			set
			{
				if ((value != this.ptx[1]))
				{
					this.ptc = true;
					this.ptx[1] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D SofTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D SofTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[2]));
			}
			set
			{
				if ((value != this.ptx[2]))
				{
					this.ptc = true;
					this.ptx[2] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D AlbedoTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D AlbedoTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[3]));
			}
			set
			{
				if ((value != this.ptx[3]))
				{
					this.ptc = true;
					this.ptx[3] = value;
				}
			}
		}
		/// <summary>Get/Set the Bound texture for 'Texture2D NormalTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D NormalTexture
		{
			get
			{
				return ((Microsoft.Xna.Framework.Graphics.Texture2D)(this.ptx[4]));
			}
			set
			{
				if ((value != this.ptx[4]))
				{
					this.ptc = true;
					this.ptx[4] = value;
				}
			}
		}
		/// <summary>Name uid for sampler for 'Sampler2D AlbedoSampler'</summary>
		static int sid0;
		/// <summary>Name uid for sampler for 'SamplerCUBE CubeRgbmSampler'</summary>
		static int sid1;
		/// <summary>Name uid for sampler for 'Sampler2D NormalSampler'</summary>
		static int sid2;
		/// <summary>Name uid for sampler for 'Sampler2D ShadowSampler'</summary>
		static int sid3;
		/// <summary>Name uid for sampler for 'Sampler2D SofSampler'</summary>
		static int sid4;
		/// <summary>Name uid for texture for 'TextureCube CubeRgbmTexture'</summary>
		static int tid0;
		/// <summary>Name uid for texture for 'Texture2D ShadowTexture'</summary>
		static int tid1;
		/// <summary>Name uid for texture for 'Texture2D SofTexture'</summary>
		static int tid2;
		/// <summary>Name uid for texture for 'Texture2D AlbedoTexture'</summary>
		static int tid3;
		/// <summary>Name uid for texture for 'Texture2D NormalTexture'</summary>
		static int tid4;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,3,4,5,0,0,0,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[13];
		/// <summary>Pixel shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] preg = new Microsoft.Xna.Framework.Vector4[17];
		/// <summary>Instancing shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vireg = new Microsoft.Xna.Framework.Vector4[4];
		/// <summary>Bound pixel textures</summary>
readonly 
		Microsoft.Xna.Framework.Graphics.Texture[] ptx = new Microsoft.Xna.Framework.Graphics.Texture[5];
		/// <summary>Bound pixel samplers</summary>
readonly 
		Xen.Graphics.TextureSamplerState[] pts = new Xen.Graphics.TextureSamplerState[5];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,32,152,0,8,254,255,9,1,0,0,17,184,135,0,1,3,131,0,1,1,131,0,1,240,135,0,1,13,131,0,1,4,131,0,1,1,229,0,0,229,0,0,137,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,1,0,1,0,1,2,1,40,135,0,0,1,17,131,0,0,1,4,131,0,0,1,1,229,0,0,229,0,0,201,0,0,1,6,1,95,1,112,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,1,0,1,0,1,15,1,208,135,0,0,1,216,131,0,0,1,4,131,0,0,1,1,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,153,0,0,1,7,1,95,1,118,1,115,1,98,1,95,1,99,133,0,0,1,3,131,0,0,1,1,1,0,1,0,1,16,1,56,135,0,0,1,4,131,0,0,1,4,131,0,0,1,1,195,0,0,1,7,1,95,1,118,1,115,1,105,1,95,1,99,133,0,0,1,14,131,0,0,1,4,1,0,1,0,1,16,1,92,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,16,1,128,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,49,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,16,1,164,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,50,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,16,1,200,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,51,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,16,1,236,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,52,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,3,131,0,0,1,16,131,0,0,1,4,143,0,0,1,4,131,0,0,1,15,131,0,0,1,4,143,0,0,1,9,1,66,1,108,1,101,1,110,1,100,1,105,1,110,1,103,135,0,0,1,5,131,0,0,1,16,131,0,0,1,4,143,0,0,1,6,131,0,0,1,15,131,0,0,1,4,143,0,0,1,11,1,73,1,110,1,115,1,116,1,97,1,110,1,99,1,105,1,110,1,103,133,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,9,131,0,0,1,1,131,0,0,1,14,131,0,0,1,7,131,0,0,1,4,131,0,0,1,32,139,0,0,1,252,1,0,1,0,1,1,1,24,138,0,0,1,2,1,52,1,0,1,0,1,2,1,80,138,0,0,1,15,1,220,1,0,1,0,1,15,1,248,138,0,0,1,16,1,68,1,0,1,0,1,16,1,88,138,0,0,1,16,1,104,1,0,1,0,1,16,1,124,138,0,0,1,16,1,140,1,0,1,0,1,16,1,160,138,0,0,1,16,1,176,1,0,1,0,1,16,1,196,138,0,0,1,16,1,212,1,0,1,0,1,16,1,232,138,0,0,1,17,1,172,135,0,0,1,3,1,0,1,0,1,17,1,40,135,0,0,1,2,131,0,0,1,92,134,0,0,1,16,1,252,1,0,1,0,1,16,1,248,131,0,0,1,93,134,0,0,1,17,1,20,1,0,1,0,1,17,1,16,1,0,1,0,1,17,1,92,135,0,0,1,2,131,0,0,1,92,134,0,0,1,17,1,48,1,0,1,0,1,17,1,44,131,0,0,1,93,134,0,0,1,17,1,72,1,0,1,0,1,17,1,68,1,0,1,0,1,17,1,156,135,0,0,1,2,131,0,0,1,92,134,0,0,1,17,1,112,1,0,1,0,1,17,1,108,131,0,0,1,93,134,0,0,1,17,1,136,1,0,1,0,1,17,1,132,135,0,0,1,6,135,0,0,1,2,132,255,0,131,0,0,1,1,134,0,0,1,8,1,88,1,16,1,42,1,17,131,0,0,1,2,1,156,1,0,1,0,1,5,1,188,135,0,0,1,36,1,0,1,0,1,2,1,60,1,0,1,0,1,2,1,100,138,0,0,1,2,1,20,131,0,0,1,28,1,0,1,0,1,2,1,8,1,255,1,255,1,3,132,0,0,1,6,131,0,0,1,28,134,0,0,1,2,1,1,131,0,0,1,148,1,0,1,2,131,0,0,1,17,133,0,0,1,156,131,0,0,1,172,1,0,1,0,1,1,1,188,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,196,134,0,0,1,1,1,212,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,1,220,134,0,0,1,1,1,236,1,0,1,3,1,0,1,2,1,0,1,1,132,0,0,1,1,1,220,134,0,0,1,1,1,243,1,0,1,3,1,0,1,3,1,0,1,1,132,0,0,1,1,1,220,134,0,0,1,1,1,250,1,0,1,3,1,0,1,4,1,0,1,1,132,0,0,1,1,1,220,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,17,229,0,0,229,0,0,204,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,14,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,112,1,115,1,95,1,115,1,49,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,112,1,115,1,95,1,115,1,50,1,0,1,95,1,112,1,115,1,95,1,115,1,51,1,0,1,95,1,112,1,115,1,95,1,115,1,52,1,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,136,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,5,1,124,1,16,1,0,1,12,132,0,0,1,4,134,0,0,1,72,1,198,1,0,1,63,1,0,1,63,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,113,1,81,1,0,1,0,1,114,1,82,1,0,1,0,1,115,1,83,1,0,1,0,1,116,1,84,1,0,1,0,1,245,1,85,1,62,1,170,1,170,1,171,144,0,0,1,65,1,112,1,0,1,0,1,62,1,128,1,0,1,0,1,63,1,64,1,0,1,0,1,190,1,170,1,170,1,171,1,63,1,42,1,170,1,171,1,63,1,192,1,0,1,0,1,59,1,128,1,128,1,129,1,63,131,0,0,1,191,131,0,0,1,63,1,128,1,0,1,0,1,67,1,127,131,0,0,1,9,1,80,1,12,1,32,1,17,1,16,1,0,1,86,1,0,1,0,1,9,1,0,1,0,1,32,1,19,1,64,1,8,1,16,1,0,1,176,131,0,0,1,4,1,0,1,96,1,21,1,96,1,27,1,18,1,0,1,18,1,0,1,0,1,149,1,0,1,0,1,96,1,33,1,48,1,39,1,18,1,0,1,18,132,0,0,1,9,1,96,1,42,1,96,1,48,1,18,1,0,1,18,133,0,0,1,32,1,54,1,64,1,56,1,16,1,0,1,82,1,0,1,0,1,64,132,0,0,1,48,1,60,1,196,1,0,1,82,133,0,0,1,96,1,63,1,96,1,69,1,86,1,0,1,86,133,0,0,1,96,1,75,1,96,1,81,1,18,1,0,1,18,133,0,0,1,96,1,87,1,96,1,93,1,18,1,0,1,18,133,0,0,1,96,1,99,1,96,1,105,1,18,1,0,1,18,133,0,0,1,80,1,111,1,0,1,0,1,34,133,0,0,1,16,1,32,1,96,1,1,1,31,1,31,1,254,1,136,1,0,1,0,1,64,1,0,1,116,1,0,1,0,1,128,131,0,0,1,108,1,194,1,0,1,0,1,16,1,200,1,7,1,0,1,8,1,0,1,198,1,198,1,0,1,34,1,255,1,255,1,0,1,200,1,7,1,0,1,7,1,0,1,198,1,198,1,0,1,34,1,255,1,255,1,0,1,168,1,136,1,4,1,3,1,0,1,198,1,108,1,196,1,165,1,6,1,255,1,15,1,16,1,56,1,112,1,1,1,159,1,31,1,254,1,136,1,128,1,0,1,64,1,0,1,200,1,7,1,0,1,7,1,24,1,192,1,192,1,0,1,225,1,7,1,7,1,0,1,200,1,8,1,0,1,130,1,0,1,177,1,177,1,198,1,110,1,16,1,6,1,8,1,116,1,0,1,0,1,128,131,0,0,1,198,1,194,1,0,1,0,1,16,1,76,1,64,133,0,0,1,27,1,226,1,0,1,0,1,5,1,200,1,7,1,0,1,6,1,0,1,198,1,192,1,0,1,161,1,0,1,255,1,0,1,76,1,71,1,0,1,5,1,0,1,205,1,205,1,108,1,193,1,6,1,5,1,10,1,76,1,137,1,0,1,8,1,0,1,109,1,108,1,177,1,128,1,5,1,255,1,10,1,0,1,134,1,0,1,8,1,0,1,107,1,203,1,203,1,224,1,8,1,0,1,0,1,152,1,24,1,97,1,1,1,15,1,31,1,255,1,248,1,0,1,0,1,64,1,0,1,184,1,24,1,97,1,1,1,15,1,31,1,255,1,199,1,0,1,0,1,64,1,0,1,16,1,24,1,97,1,1,1,15,1,31,1,254,1,63,1,0,1,0,1,64,1,0,1,48,1,24,1,1,1,1,1,15,1,31,1,254,1,63,1,0,1,0,1,64,1,0,1,0,1,72,1,0,1,1,1,6,1,27,1,198,1,97,1,192,1,0,1,0,1,10,1,20,1,8,1,0,1,0,1,4,1,27,1,198,1,27,1,224,1,0,1,5,1,1,1,4,1,131,1,6,1,5,1,0,1,111,1,109,1,198,1,161,1,8,1,10,1,5,1,44,1,23,1,5,1,6,1,2,1,27,1,192,1,108,1,224,1,0,1,6,1,5,1,45,1,47,1,5,1,8,1,0,1,0,1,177,1,177,1,161,1,6,1,253,1,5,1,200,1,3,1,0,1,5,1,0,1,176,1,27,1,198,1,139,1,5,1,253,1,253,1,188,1,79,1,5,1,6,1,0,1,0,1,198,1,196,1,193,1,6,1,0,1,255,1,189,1,143,1,5,1,6,1,0,1,0,1,108,1,197,1,129,1,6,1,255,1,255,1,200,1,15,1,0,1,5,1,0,1,51,1,78,1,0,1,225,1,5,1,5,1,0,1,200,1,8,131,0,0,1,37,1,208,1,0,1,239,1,5,1,8,1,0,1,184,1,72,1,0,1,1,1,0,1,37,1,208,1,195,1,207,1,5,1,6,1,255,1,188,1,33,1,8,1,8,1,0,1,198,1,198,1,195,1,193,1,0,1,0,1,255,1,16,1,64,1,0,1,1,1,31,1,31,1,254,1,136,1,0,1,0,1,64,1,0,1,200,1,11,131,0,0,1,98,1,177,1,0,1,160,1,0,1,255,1,0,1,200,1,7,1,0,1,1,1,0,1,108,1,192,1,0,1,225,1,0,1,1,1,0,1,200,1,7,131,0,0,1,177,1,192,1,192,1,235,1,0,1,3,1,1,1,200,1,14,131,0,0,1,27,1,140,1,140,1,235,1,0,1,2,1,0,1,200,1,1,131,0,0,1,18,1,18,1,0,1,240,131,0,0,1,88,1,16,133,0,0,1,108,1,226,1,0,1,0,1,128,1,200,1,7,1,0,1,5,1,0,1,201,1,108,1,0,1,225,131,0,0,1,200,1,1,131,0,0,1,190,1,190,1,0,1,240,1,5,1,4,1,0,1,0,1,16,133,0,0,1,108,1,226,131,0,0,1,200,1,14,1,0,1,0,1,4,1,252,1,108,1,252,1,235,1,5,1,0,1,4,1,200,1,1,131,0,0,1,195,1,195,1,0,1,240,131,0,0,1,88,1,16,133,0,0,1,108,1,226,1,0,1,0,1,128,1,112,1,7,1,0,1,2,1,0,1,21,1,108,1,27,1,225,1,0,1,0,1,3,1,200,1,15,1,0,1,0,1,16,1,166,1,205,1,0,1,242,1,2,1,2,1,0,1,76,1,20,1,1,1,1,1,16,1,27,1,27,1,198,1,226,1,0,1,0,1,128,1,200,1,3,1,0,1,1,1,16,1,109,1,108,1,198,1,203,1,0,1,1,1,254,1,144,1,8,1,0,1,33,1,159,1,31,1,246,1,136,1,0,1,0,1,192,1,0,1,168,1,16,1,1,1,0,1,16,1,0,1,0,1,67,1,194,1,0,1,0,1,9,1,200,1,7,1,0,1,0,1,16,1,108,1,192,1,0,1,225,1,1,1,0,1,0,1,200,1,7,1,0,1,4,1,16,1,192,1,192,1,0,1,225,131,0,0,1,200,1,7,1,0,1,0,1,24,1,108,1,98,1,98,1,139,1,2,1,1,1,0,1,200,1,7,1,0,1,0,1,24,1,177,1,190,1,180,1,171,1,2,1,2,1,0,1,200,1,3,1,0,1,1,1,24,1,108,1,196,1,0,1,225,1,2,1,2,1,0,1,200,1,15,1,0,1,6,1,24,1,114,1,130,1,0,1,225,1,2,1,2,1,0,1,200,1,8,1,0,1,0,1,26,1,108,1,177,1,0,1,224,1,1,1,6,1,0,1,200,1,1,1,0,1,1,1,26,1,108,1,108,1,0,1,160,1,6,1,252,1,0,1,200,1,7,1,0,1,0,1,24,1,198,1,98,1,180,1,171,1,2,1,3,1,0,1,200,1,7,1,0,1,0,1,24,1,27,1,190,1,180,1,171,1,6,1,4,1,0,1,200,1,7,1,0,1,0,1,24,1,198,1,98,1,180,1,171,1,6,1,5,1,0,1,200,1,7,1,0,1,0,1,24,1,177,1,190,1,180,1,171,1,1,1,6,1,0,1,200,1,7,1,0,1,0,1,24,1,108,1,98,1,180,1,171,1,1,1,7,1,0,1,200,1,7,1,0,1,0,1,24,1,27,1,190,1,180,1,171,1,0,1,8,1,0,1,200,1,7,1,0,1,4,1,24,1,101,1,108,1,0,1,162,1,0,1,253,1,0,1,20,1,14,1,0,1,6,1,0,1,108,1,140,1,108,1,129,1,5,1,6,1,11,1,12,1,135,1,1,1,10,1,0,1,177,1,180,1,177,1,129,1,5,1,5,1,15,1,200,1,13,131,0,0,1,108,1,114,1,114,1,139,1,5,1,1,1,0,1,76,1,39,1,0,1,1,1,0,1,98,1,98,1,177,1,1,1,12,1,12,1,9,1,172,1,23,1,3,1,11,1,0,1,108,1,180,1,2,1,129,1,5,1,4,1,12,1,200,1,7,1,0,1,12,1,0,1,177,1,190,1,200,1,171,1,5,1,2,1,0,1,172,1,39,1,3,1,9,1,0,1,190,1,190,1,0,1,193,1,5,1,5,1,12,1,173,1,72,1,3,1,0,1,0,1,190,1,190,1,1,1,144,1,5,1,13,1,12,1,101,1,17,1,6,1,0,1,0,1,190,1,190,1,182,1,176,1,2,1,13,1,9,1,8,1,68,1,1,1,0,1,0,1,190,1,190,1,108,1,176,1,5,1,13,1,0,1,200,1,7,1,0,1,2,1,0,1,198,1,98,1,180,1,171,1,5,1,3,1,12,1,64,1,72,1,0,1,9,1,0,1,198,1,108,1,108,1,161,1,0,1,252,1,0,1,200,1,7,1,0,1,2,1,0,1,205,1,177,1,180,1,235,1,11,1,5,1,2,1,200,1,7,1,0,1,2,1,0,1,101,1,198,1,180,1,235,1,10,1,5,1,2,1,168,1,19,1,0,1,1,1,0,1,111,1,109,1,66,1,128,1,9,1,254,1,14,1,58,1,24,1,0,1,1,1,0,1,27,1,108,1,108,1,225,1,1,1,1,1,0,1,200,1,7,1,0,1,5,1,0,1,18,1,198,1,180,1,235,1,6,1,5,1,2,1,172,1,20,131,0,0,1,108,1,177,1,128,1,129,1,0,1,14,1,15,1,200,1,8,1,0,1,5,1,0,1,27,1,198,1,198,1,236,1,3,1,0,1,1,1,168,1,23,1,1,1,2,1,0,1,27,1,192,1,192,1,193,1,4,1,4,1,11,1,200,1,7,1,0,1,4,1,0,1,177,1,98,1,180,1,171,1,1,1,7,1,5,1,200,1,7,1,0,1,4,1,0,1,108,1,190,1,180,1,171,1,6,1,8,1,4,1,168,1,39,1,1,1,5,1,0,1,101,1,108,1,128,1,130,1,4,1,253,1,11,1,200,1,7,1,0,1,1,1,0,1,27,1,108,1,99,1,172,1,3,1,253,1,1,1,168,1,23,1,0,1,1,1,0,1,192,1,108,1,131,1,193,1,1,1,8,1,15,1,200,1,1,131,0,0,1,27,1,27,1,108,1,235,1,4,1,5,1,0,1,200,1,13,131,0,0,1,108,1,177,1,240,1,235,1,0,1,8,1,1,1,168,1,23,1,0,1,1,1,0,1,180,1,20,1,67,1,225,1,3,1,0,1,15,1,200,1,7,1,0,1,1,1,0,1,192,1,27,1,192,1,235,1,2,1,2,1,1,1,200,1,13,131,0,0,1,108,1,240,1,240,1,235,1,0,1,5,1,1,1,200,1,7,1,0,1,1,1,0,1,20,1,192,1,0,1,225,1,0,1,7,1,0,1,20,1,16,133,0,0,1,182,1,226,1,0,1,0,1,1,1,168,1,33,131,0,0,1,108,1,108,1,1,1,194,1,0,1,1,1,255,1,200,1,1,131,0,0,1,177,1,108,1,0,1,225,131,0,0,1,52,1,16,1,0,1,0,1,1,1,0,1,0,1,108,1,226,131,0,0,1,201,1,8,1,128,1,0,1,4,1,108,1,27,1,0,1,161,1,0,1,254,1,0,1,200,1,1,1,0,1,0,1,4,1,108,1,27,1,0,1,161,1,0,1,254,1,0,1,168,1,16,133,0,0,1,128,1,194,1,0,1,0,1,9,1,76,1,16,133,0,0,1,108,1,226,131,0,0,1,201,1,7,1,128,1,0,1,0,1,192,1,108,1,0,1,225,1,1,149,0,0,1,2,132,255,0,138,0,0,1,4,1,16,1,16,1,42,1,17,1,1,1,0,1,0,1,2,1,72,1,0,1,0,1,1,1,200,135,0,0,1,36,134,0,0,1,1,1,192,138,0,0,1,1,1,152,131,0,0,1,28,1,0,1,0,1,1,1,139,1,255,1,254,1,3,132,0,0,1,2,131,0,0,1,28,134,0,0,1,1,1,132,131,0,0,1,68,1,0,1,2,131,0,0,1,13,133,0,0,1,76,131,0,0,1,92,1,0,1,0,1,1,1,44,1,0,1,2,1,0,1,13,1,0,1,4,132,0,0,1,1,1,52,1,0,1,0,1,1,1,68,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,13,229,0,0,229,0,0,140,0,0,1,95,1,118,1,115,1,105,1,95,1,99,1,0,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,4,198,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,134,0,0,1,1,1,200,1,0,1,81,1,0,1,9,138,0,0,1,72,1,198,131,0,0,1,1,131,0,0,1,9,131,0,0,1,9,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,5,1,0,1,0,1,80,1,6,1,0,1,0,1,48,1,7,1,0,1,0,1,96,1,8,1,0,1,0,1,112,1,9,1,0,1,12,1,0,1,10,1,0,1,13,1,0,1,11,1,0,1,14,1,0,1,12,1,0,1,63,1,0,1,13,1,0,1,0,1,48,1,80,1,0,1,1,1,113,1,81,1,0,1,2,1,114,1,82,1,0,1,3,1,115,1,83,1,0,1,4,1,116,1,84,1,0,1,5,1,245,1,85,1,0,1,0,1,16,1,32,1,0,1,0,1,16,1,29,1,0,1,0,1,16,1,30,1,0,1,0,1,16,1,31,1,0,1,0,1,16,1,28,131,0,0,1,33,131,0,0,1,34,131,0,0,1,35,1,0,1,0,1,16,1,36,1,245,1,85,1,96,1,5,1,48,1,11,1,18,1,3,1,18,1,0,1,112,1,21,132,0,0,1,96,1,14,1,194,1,0,1,18,133,0,0,1,32,1,20,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,22,1,96,1,28,1,18,1,0,1,18,133,0,0,1,48,1,34,1,0,1,0,1,34,133,0,0,1,5,1,248,1,96,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,15,1,200,132,0,0,1,5,1,248,1,80,131,0,0,1,1,1,209,132,0,0,1,5,1,248,1,64,131,0,0,1,1,1,209,132,0,0,1,5,1,248,1,32,131,0,0,1,1,1,209,132,0,0,1,5,1,248,1,48,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,112,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,144,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,6,1,136,132,0,0,1,200,1,15,131,0,0,1,27,1,0,1,0,1,225,1,6,1,0,1,0,1,200,1,15,131,0,0,1,198,1,0,1,0,1,235,1,6,1,9,1,0,1,200,1,15,131,0,0,1,177,1,148,1,148,1,235,1,6,1,7,1,0,1,200,1,15,131,0,0,1,108,1,248,1,148,1,235,1,6,1,3,1,0,1,200,1,1,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,13,1,0,1,200,1,2,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,14,1,0,1,200,1,4,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,15,1,0,1,200,1,8,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,16,1,0,1,200,1,7,1,0,1,6,1,0,1,177,1,180,1,0,1,225,1,5,1,9,1,0,1,200,1,7,1,0,1,8,1,0,1,177,1,180,1,0,1,225,1,4,1,9,1,0,1,200,1,7,1,0,1,9,1,0,1,177,1,180,1,0,1,225,1,2,1,9,1,0,1,200,1,7,1,0,1,2,1,0,1,108,1,180,1,192,1,235,1,2,1,7,1,9,1,200,1,7,1,0,1,4,1,0,1,108,1,180,1,192,1,235,1,4,1,7,1,8,1,200,1,7,1,0,1,5,1,0,1,108,1,180,1,192,1,235,1,5,1,7,1,6,1,200,1,7,1,128,1,4,1,2,1,20,1,192,1,0,1,160,1,0,1,12,1,0,1,200,1,7,1,128,1,1,1,0,1,27,1,192,1,180,1,235,1,5,1,3,1,5,1,200,1,7,1,128,1,2,1,0,1,27,1,192,1,180,1,235,1,4,1,3,1,4,1,200,1,7,1,128,1,3,1,0,1,27,1,192,1,180,1,235,1,2,1,3,1,2,1,200,1,3,1,128,1,0,1,0,1,176,1,176,1,0,1,226,1,1,1,1,1,0,1,200,1,1,1,128,1,5,1,0,1,233,1,167,1,0,1,175,131,0,0,1,200,1,2,1,128,1,5,1,0,1,233,1,167,1,0,1,175,1,0,1,1,1,0,1,200,1,4,1,128,1,5,1,0,1,233,1,167,1,0,1,175,1,0,1,2,1,0,1,200,1,8,1,128,1,5,1,0,1,233,1,167,1,0,1,175,1,0,1,3,148,0,0,1,1,132,255,0,131,0,0,1,1,134,0,0,1,8,1,88,1,16,1,42,1,17,131,0,0,1,2,1,156,1,0,1,0,1,5,1,188,135,0,0,1,36,1,0,1,0,1,2,1,60,1,0,1,0,1,2,1,100,138,0,0,1,2,1,20,131,0,0,1,28,1,0,1,0,1,2,1,8,1,255,1,255,1,3,132,0,0,1,6,131,0,0,1,28,134,0,0,1,2,1,1,131,0,0,1,148,1,0,1,2,131,0,0,1,17,133,0,0,1,156,131,0,0,1,172,1,0,1,0,1,1,1,188,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,196,134,0,0,1,1,1,212,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,1,220,134,0,0,1,1,1,236,1,0,1,3,1,0,1,2,1,0,1,1,132,0,0,1,1,1,220,134,0,0,1,1,1,243,1,0,1,3,1,0,1,3,1,0,1,1,132,0,0,1,1,1,220,134,0,0,1,1,1,250,1,0,1,3,1,0,1,4,1,0,1,1,132,0,0,1,1,1,220,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,17,229,0,0,229,0,0,204,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,14,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,112,1,115,1,95,1,115,1,49,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,112,1,115,1,95,1,115,1,50,1,0,1,95,1,112,1,115,1,95,1,115,1,51,1,0,1,95,1,112,1,115,1,95,1,115,1,52,1,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,136,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,5,1,124,1,16,1,0,1,12,132,0,0,1,4,134,0,0,1,72,1,198,1,0,1,63,1,0,1,63,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,113,1,81,1,0,1,0,1,114,1,82,1,0,1,0,1,115,1,83,1,0,1,0,1,116,1,84,1,0,1,0,1,245,1,85,1,62,1,170,1,170,1,171,144,0,0,1,65,1,112,1,0,1,0,1,62,1,128,1,0,1,0,1,63,1,64,1,0,1,0,1,190,1,170,1,170,1,171,1,63,1,42,1,170,1,171,1,63,1,192,1,0,1,0,1,59,1,128,1,128,1,129,1,63,131,0,0,1,191,131,0,0,1,63,1,128,1,0,1,0,1,67,1,127,131,0,0,1,9,1,80,1,12,1,32,1,17,1,16,1,0,1,86,1,0,1,0,1,9,1,0,1,0,1,32,1,19,1,64,1,8,1,16,1,0,1,176,131,0,0,1,4,1,0,1,96,1,21,1,96,1,27,1,18,1,0,1,18,1,0,1,0,1,149,1,0,1,0,1,96,1,33,1,48,1,39,1,18,1,0,1,18,132,0,0,1,9,1,96,1,42,1,96,1,48,1,18,1,0,1,18,133,0,0,1,32,1,54,1,64,1,56,1,16,1,0,1,82,1,0,1,0,1,64,132,0,0,1,48,1,60,1,196,1,0,1,82,133,0,0,1,96,1,63,1,96,1,69,1,86,1,0,1,86,133,0,0,1,96,1,75,1,96,1,81,1,18,1,0,1,18,133,0,0,1,96,1,87,1,96,1,93,1,18,1,0,1,18,133,0,0,1,96,1,99,1,96,1,105,1,18,1,0,1,18,133,0,0,1,80,1,111,1,0,1,0,1,34,133,0,0,1,16,1,32,1,96,1,1,1,31,1,31,1,254,1,136,1,0,1,0,1,64,1,0,1,116,1,0,1,0,1,128,131,0,0,1,108,1,194,1,0,1,0,1,16,1,200,1,7,1,0,1,8,1,0,1,198,1,198,1,0,1,34,1,255,1,255,1,0,1,200,1,7,1,0,1,7,1,0,1,198,1,198,1,0,1,34,1,255,1,255,1,0,1,168,1,136,1,4,1,3,1,0,1,198,1,108,1,196,1,165,1,6,1,255,1,15,1,16,1,56,1,112,1,1,1,159,1,31,1,254,1,136,1,128,1,0,1,64,1,0,1,200,1,7,1,0,1,7,1,24,1,192,1,192,1,0,1,225,1,7,1,7,1,0,1,200,1,8,1,0,1,130,1,0,1,177,1,177,1,198,1,110,1,16,1,6,1,8,1,116,1,0,1,0,1,128,131,0,0,1,198,1,194,1,0,1,0,1,16,1,76,1,64,133,0,0,1,27,1,226,1,0,1,0,1,5,1,200,1,7,1,0,1,6,1,0,1,198,1,192,1,0,1,161,1,0,1,255,1,0,1,76,1,71,1,0,1,5,1,0,1,205,1,205,1,108,1,193,1,6,1,5,1,10,1,76,1,137,1,0,1,8,1,0,1,109,1,108,1,177,1,128,1,5,1,255,1,10,1,0,1,134,1,0,1,8,1,0,1,107,1,203,1,203,1,224,1,8,1,0,1,0,1,152,1,24,1,97,1,1,1,15,1,31,1,255,1,248,1,0,1,0,1,64,1,0,1,184,1,24,1,97,1,1,1,15,1,31,1,255,1,199,1,0,1,0,1,64,1,0,1,16,1,24,1,97,1,1,1,15,1,31,1,254,1,63,1,0,1,0,1,64,1,0,1,48,1,24,1,1,1,1,1,15,1,31,1,254,1,63,1,0,1,0,1,64,1,0,1,0,1,72,1,0,1,1,1,6,1,27,1,198,1,97,1,192,1,0,1,0,1,10,1,20,1,8,1,0,1,0,1,4,1,27,1,198,1,27,1,224,1,0,1,5,1,1,1,4,1,131,1,6,1,5,1,0,1,111,1,109,1,198,1,161,1,8,1,10,1,5,1,44,1,23,1,5,1,6,1,2,1,27,1,192,1,108,1,224,1,0,1,6,1,5,1,45,1,47,1,5,1,8,1,0,1,0,1,177,1,177,1,161,1,6,1,253,1,5,1,200,1,3,1,0,1,5,1,0,1,176,1,27,1,198,1,139,1,5,1,253,1,253,1,188,1,79,1,5,1,6,1,0,1,0,1,198,1,196,1,193,1,6,1,0,1,255,1,189,1,143,1,5,1,6,1,0,1,0,1,108,1,197,1,129,1,6,1,255,1,255,1,200,1,15,1,0,1,5,1,0,1,51,1,78,1,0,1,225,1,5,1,5,1,0,1,200,1,8,131,0,0,1,37,1,208,1,0,1,239,1,5,1,8,1,0,1,184,1,72,1,0,1,1,1,0,1,37,1,208,1,195,1,207,1,5,1,6,1,255,1,188,1,33,1,8,1,8,1,0,1,198,1,198,1,195,1,193,1,0,1,0,1,255,1,16,1,64,1,0,1,1,1,31,1,31,1,254,1,136,1,0,1,0,1,64,1,0,1,200,1,11,131,0,0,1,98,1,177,1,0,1,160,1,0,1,255,1,0,1,200,1,7,1,0,1,1,1,0,1,108,1,192,1,0,1,225,1,0,1,1,1,0,1,200,1,7,131,0,0,1,177,1,192,1,192,1,235,1,0,1,3,1,1,1,200,1,14,131,0,0,1,27,1,140,1,140,1,235,1,0,1,2,1,0,1,200,1,1,131,0,0,1,18,1,18,1,0,1,240,131,0,0,1,88,1,16,133,0,0,1,108,1,226,1,0,1,0,1,128,1,200,1,7,1,0,1,5,1,0,1,201,1,108,1,0,1,225,131,0,0,1,200,1,1,131,0,0,1,190,1,190,1,0,1,240,1,5,1,4,1,0,1,0,1,16,133,0,0,1,108,1,226,131,0,0,1,200,1,14,1,0,1,0,1,4,1,252,1,108,1,252,1,235,1,5,1,0,1,4,1,200,1,1,131,0,0,1,195,1,195,1,0,1,240,131,0,0,1,88,1,16,133,0,0,1,108,1,226,1,0,1,0,1,128,1,112,1,7,1,0,1,2,1,0,1,21,1,108,1,27,1,225,1,0,1,0,1,3,1,200,1,15,1,0,1,0,1,16,1,166,1,205,1,0,1,242,1,2,1,2,1,0,1,76,1,20,1,1,1,1,1,16,1,27,1,27,1,198,1,226,1,0,1,0,1,128,1,200,1,3,1,0,1,1,1,16,1,109,1,108,1,198,1,203,1,0,1,1,1,254,1,144,1,8,1,0,1,33,1,159,1,31,1,246,1,136,1,0,1,0,1,192,1,0,1,168,1,16,1,1,1,0,1,16,1,0,1,0,1,67,1,194,1,0,1,0,1,9,1,200,1,7,1,0,1,0,1,16,1,108,1,192,1,0,1,225,1,1,1,0,1,0,1,200,1,7,1,0,1,4,1,16,1,192,1,192,1,0,1,225,131,0,0,1,200,1,7,1,0,1,0,1,24,1,108,1,98,1,98,1,139,1,2,1,1,1,0,1,200,1,7,1,0,1,0,1,24,1,177,1,190,1,180,1,171,1,2,1,2,1,0,1,200,1,3,1,0,1,1,1,24,1,108,1,196,1,0,1,225,1,2,1,2,1,0,1,200,1,15,1,0,1,6,1,24,1,114,1,130,1,0,1,225,1,2,1,2,1,0,1,200,1,8,1,0,1,0,1,26,1,108,1,177,1,0,1,224,1,1,1,6,1,0,1,200,1,1,1,0,1,1,1,26,1,108,1,108,1,0,1,160,1,6,1,252,1,0,1,200,1,7,1,0,1,0,1,24,1,198,1,98,1,180,1,171,1,2,1,3,1,0,1,200,1,7,1,0,1,0,1,24,1,27,1,190,1,180,1,171,1,6,1,4,1,0,1,200,1,7,1,0,1,0,1,24,1,198,1,98,1,180,1,171,1,6,1,5,1,0,1,200,1,7,1,0,1,0,1,24,1,177,1,190,1,180,1,171,1,1,1,6,1,0,1,200,1,7,1,0,1,0,1,24,1,108,1,98,1,180,1,171,1,1,1,7,1,0,1,200,1,7,1,0,1,0,1,24,1,27,1,190,1,180,1,171,1,0,1,8,1,0,1,200,1,7,1,0,1,4,1,24,1,101,1,108,1,0,1,162,1,0,1,253,1,0,1,20,1,14,1,0,1,6,1,0,1,108,1,140,1,108,1,129,1,5,1,6,1,11,1,12,1,135,1,1,1,10,1,0,1,177,1,180,1,177,1,129,1,5,1,5,1,15,1,200,1,13,131,0,0,1,108,1,114,1,114,1,139,1,5,1,1,1,0,1,76,1,39,1,0,1,1,1,0,1,98,1,98,1,177,1,1,1,12,1,12,1,9,1,172,1,23,1,3,1,11,1,0,1,108,1,180,1,2,1,129,1,5,1,4,1,12,1,200,1,7,1,0,1,12,1,0,1,177,1,190,1,200,1,171,1,5,1,2,1,0,1,172,1,39,1,3,1,9,1,0,1,190,1,190,1,0,1,193,1,5,1,5,1,12,1,173,1,72,1,3,1,0,1,0,1,190,1,190,1,1,1,144,1,5,1,13,1,12,1,101,1,17,1,6,1,0,1,0,1,190,1,190,1,182,1,176,1,2,1,13,1,9,1,8,1,68,1,1,1,0,1,0,1,190,1,190,1,108,1,176,1,5,1,13,1,0,1,200,1,7,1,0,1,2,1,0,1,198,1,98,1,180,1,171,1,5,1,3,1,12,1,64,1,72,1,0,1,9,1,0,1,198,1,108,1,108,1,161,1,0,1,252,1,0,1,200,1,7,1,0,1,2,1,0,1,205,1,177,1,180,1,235,1,11,1,5,1,2,1,200,1,7,1,0,1,2,1,0,1,101,1,198,1,180,1,235,1,10,1,5,1,2,1,168,1,19,1,0,1,1,1,0,1,111,1,109,1,66,1,128,1,9,1,254,1,14,1,58,1,24,1,0,1,1,1,0,1,27,1,108,1,108,1,225,1,1,1,1,1,0,1,200,1,7,1,0,1,5,1,0,1,18,1,198,1,180,1,235,1,6,1,5,1,2,1,172,1,20,131,0,0,1,108,1,177,1,128,1,129,1,0,1,14,1,15,1,200,1,8,1,0,1,5,1,0,1,27,1,198,1,198,1,236,1,3,1,0,1,1,1,168,1,23,1,1,1,2,1,0,1,27,1,192,1,192,1,193,1,4,1,4,1,11,1,200,1,7,1,0,1,4,1,0,1,177,1,98,1,180,1,171,1,1,1,7,1,5,1,200,1,7,1,0,1,4,1,0,1,108,1,190,1,180,1,171,1,6,1,8,1,4,1,168,1,39,1,1,1,5,1,0,1,101,1,108,1,128,1,130,1,4,1,253,1,11,1,200,1,7,1,0,1,1,1,0,1,27,1,108,1,99,1,172,1,3,1,253,1,1,1,168,1,23,1,0,1,1,1,0,1,192,1,108,1,131,1,193,1,1,1,8,1,15,1,200,1,1,131,0,0,1,27,1,27,1,108,1,235,1,4,1,5,1,0,1,200,1,13,131,0,0,1,108,1,177,1,240,1,235,1,0,1,8,1,1,1,168,1,23,1,0,1,1,1,0,1,180,1,20,1,67,1,225,1,3,1,0,1,15,1,200,1,7,1,0,1,1,1,0,1,192,1,27,1,192,1,235,1,2,1,2,1,1,1,200,1,13,131,0,0,1,108,1,240,1,240,1,235,1,0,1,5,1,1,1,200,1,7,1,0,1,1,1,0,1,20,1,192,1,0,1,225,1,0,1,7,1,0,1,20,1,16,133,0,0,1,182,1,226,1,0,1,0,1,1,1,168,1,33,131,0,0,1,108,1,108,1,1,1,194,1,0,1,1,1,255,1,200,1,1,131,0,0,1,177,1,108,1,0,1,225,131,0,0,1,52,1,16,1,0,1,0,1,1,1,0,1,0,1,108,1,226,131,0,0,1,201,1,8,1,128,1,0,1,4,1,108,1,27,1,0,1,161,1,0,1,254,1,0,1,200,1,1,1,0,1,0,1,4,1,108,1,27,1,0,1,161,1,0,1,254,1,0,1,168,1,16,133,0,0,1,128,1,194,1,0,1,0,1,9,1,76,1,16,133,0,0,1,108,1,226,131,0,0,1,201,1,7,1,128,1,0,1,0,1,192,1,108,1,0,1,225,1,1,149,0,0,1,1,132,255,0,138,0,0,1,19,1,72,1,16,1,42,1,17,1,1,1,0,1,0,1,15,1,192,1,0,1,0,1,3,1,136,135,0,0,1,36,1,0,1,0,1,15,131,0,0,1,15,1,40,138,0,0,1,14,1,216,131,0,0,1,28,1,0,1,0,1,14,1,203,1,255,1,254,1,3,132,0,0,1,2,131,0,0,1,28,134,0,0,1,14,1,196,131,0,0,1,68,1,0,1,2,131,0,0,1,13,133,0,0,1,76,131,0,0,1,92,1,0,1,0,1,1,1,44,1,0,1,2,1,0,1,13,1,0,1,216,132,0,0,1,1,1,52,1,0,1,0,1,1,1,68,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,13,229,0,0,229,0,0,140,0,0,1,95,1,118,1,115,1,98,1,95,1,99,1,0,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,216,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,156,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,3,1,72,1,0,1,81,1,0,1,11,138,0,0,1,72,1,198,131,0,0,1,1,131,0,0,1,7,131,0,0,1,15,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,7,1,0,1,0,1,80,1,8,1,0,1,0,1,48,1,9,1,0,1,0,1,96,1,10,1,0,1,0,1,112,1,11,1,0,1,0,1,16,1,12,1,0,1,48,1,32,1,13,1,0,1,0,1,48,1,80,1,0,1,1,1,113,1,81,1,0,1,4,1,114,1,82,1,0,1,7,1,115,1,83,1,0,1,10,1,116,1,84,1,0,1,11,1,245,1,85,1,0,1,0,1,16,1,54,131,0,0,1,55,131,0,0,1,56,1,0,1,0,1,16,1,57,131,0,0,1,58,131,0,0,1,59,1,0,1,0,1,16,1,60,131,0,0,1,61,131,0,0,1,62,1,0,1,0,1,16,1,63,1,0,1,0,1,16,1,64,131,0,0,1,65,131,0,0,1,66,131,0,0,1,67,1,0,1,0,1,16,1,68,180,0,0,1,63,1,128,1,0,1,0,1,64,1,64,134,0,0,1,245,1,85,1,96,1,7,1,16,1,13,1,18,1,3,1,18,1,0,1,16,1,1,132,0,0,1,96,1,14,1,194,1,0,1,18,133,0,0,1,96,1,20,1,96,1,26,1,18,1,0,1,18,133,0,0,1,96,1,32,1,16,1,38,1,18,1,0,1,18,135,0,0,1,96,1,39,1,196,1,0,1,18,133,0,0,1,96,1,45,1,96,1,51,1,18,1,0,1,18,133,0,0,1,96,1,57,1,96,1,63,1,18,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,4,1,88,132,0,0,1,5,1,248,1,64,131,0,0,1,15,1,200,132,0,0,1,5,1,248,1,80,131,0,0,1,14,1,136,132,0,0,1,5,1,248,1,144,131,0,0,1,14,1,136,132,0,0,1,5,1,248,1,32,131,0,0,1,14,1,136,132,0,0,1,5,1,248,1,96,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,6,1,136,132,0,0,1,200,1,15,1,0,1,8,1,0,1,0,1,198,1,0,1,161,1,0,1,255,1,0,1,92,1,8,1,0,1,10,1,0,1,177,1,27,1,27,1,161,1,1,1,7,1,8,1,200,1,15,1,0,1,0,1,160,1,27,1,136,1,0,1,161,1,6,1,13,1,0,1,200,1,15,1,0,1,3,1,160,1,27,1,136,1,0,1,161,1,6,1,14,1,0,1,92,1,15,1,0,1,7,1,160,1,27,1,136,1,198,1,161,1,6,1,15,1,8,1,200,1,15,1,0,1,7,1,160,1,198,1,136,1,0,1,171,1,6,1,15,1,7,1,200,1,15,1,0,1,3,1,160,1,198,1,136,1,0,1,171,1,6,1,14,1,3,1,200,1,15,1,0,1,0,1,160,1,198,1,136,1,0,1,171,1,6,1,13,1,0,1,92,1,2,1,0,1,11,1,0,1,177,1,27,1,177,1,161,1,1,1,5,1,8,1,200,1,15,1,0,1,0,1,160,1,177,1,52,1,148,1,171,1,6,1,13,1,0,1,200,1,15,1,0,1,3,1,160,1,177,1,52,1,148,1,171,1,6,1,14,1,3,1,200,1,15,1,0,1,7,1,160,1,177,1,52,1,148,1,171,1,6,1,15,1,7,1,92,1,8,1,0,1,11,1,0,1,177,1,27,1,108,1,161,1,1,1,4,1,8,1,200,1,15,1,0,1,7,1,160,1,108,1,208,1,148,1,171,1,6,1,15,1,7,1,200,1,15,1,0,1,3,1,160,1,108,1,255,1,143,1,171,1,6,1,14,1,3,1,200,1,15,1,0,1,8,1,160,1,108,1,208,1,148,1,171,1,6,1,13,1,0,1,200,1,1,1,0,1,6,1,0,1,170,1,233,1,0,1,239,1,8,1,1,1,0,1,200,1,2,1,0,1,6,1,0,1,248,1,233,1,0,1,239,1,3,1,1,1,0,1,200,1,4,1,0,1,6,1,0,1,170,1,233,1,0,1,239,1,7,1,1,1,0,1,200,1,1,1,0,1,10,1,0,1,190,1,190,1,0,1,176,1,6,1,6,1,0,1,200,1,4,1,0,1,10,1,0,1,190,1,190,1,0,1,176,1,6,1,7,1,0,1,20,1,17,1,0,1,11,1,0,1,190,1,190,1,177,1,176,1,6,1,5,1,1,1,168,1,36,1,10,1,11,1,0,1,190,1,190,1,0,1,144,1,6,1,4,1,6,1,200,1,3,1,128,1,62,1,0,1,110,1,179,1,0,1,224,1,11,1,11,1,0,1,200,1,12,1,128,1,62,1,0,1,236,1,49,1,0,1,224,1,10,1,10,1,0,1,200,1,2,131,0,0,1,191,1,190,1,0,1,240,1,8,1,2,1,0,1,200,1,4,131,0,0,1,195,1,190,1,0,1,240,1,3,1,2,1,0,1,200,1,8,131,0,0,1,191,1,190,1,0,1,240,1,7,1,2,1,0,1,200,1,1,1,0,1,2,1,0,1,191,1,190,1,0,1,240,1,8,1,9,1,0,1,200,1,2,1,0,1,2,1,0,1,195,1,190,1,0,1,240,1,3,1,9,1,0,1,200,1,4,1,0,1,2,1,0,1,191,1,190,1,0,1,240,1,7,1,9,1,0,1,200,1,1,1,0,1,3,1,0,1,191,1,190,1,0,1,240,1,8,1,5,1,0,1,20,1,18,1,0,1,3,1,0,1,195,1,190,1,177,1,240,1,3,1,5,1,1,1,168,1,20,1,1,1,3,1,0,1,191,1,190,1,0,1,208,1,7,1,5,1,8,1,20,1,17,1,0,1,5,1,0,1,190,1,190,1,177,1,176,1,6,1,8,1,1,1,168,1,66,1,1,1,5,1,0,1,190,1,190,1,0,1,144,1,6,1,10,1,10,1,20,1,20,1,0,1,5,1,0,1,190,1,190,1,177,1,176,1,6,1,11,1,1,1,200,1,15,1,0,1,6,1,0,1,176,1,177,1,166,1,108,1,255,1,1,1,6,1,168,1,130,1,1,1,1,1,0,1,85,1,62,1,0,1,143,1,6,1,9,1,11,1,200,1,13,1,0,1,1,1,0,1,240,1,4,1,0,1,224,1,5,1,1,1,0,1,200,1,3,1,128,1,0,1,0,1,176,1,176,1,0,1,226,1,4,1,4,1,0,1,200,1,1,1,128,1,1,1,0,1,190,1,190,1,0,1,176,1,3,1,8,1,0,1,200,1,2,1,128,1,1,1,0,1,190,1,190,1,0,1,176,1,3,1,9,1,0,1,200,1,4,1,128,1,1,1,0,1,190,1,190,1,0,1,176,1,3,1,10,1,0,1,200,1,1,1,128,1,2,1,0,1,190,1,190,1,0,1,176,1,2,1,8,1,0,1,200,1,2,1,128,1,2,1,0,1,190,1,190,1,0,1,176,1,2,1,9,1,0,1,200,1,4,1,128,1,2,1,0,1,190,1,190,1,0,1,176,1,2,1,10,1,0,1,200,1,1,1,128,1,3,1,0,1,195,1,190,1,0,1,176,1,0,1,8,1,0,1,200,1,2,1,128,1,3,1,0,1,195,1,190,1,0,1,176,1,0,1,9,1,0,1,200,1,4,1,128,1,3,1,0,1,195,1,190,1,0,1,176,1,0,1,10,1,0,1,200,1,7,1,128,1,4,1,2,1,192,1,192,1,0,1,160,1,1,1,12,1,0,1,200,1,1,1,128,1,5,1,0,1,167,1,167,1,0,1,175,1,1,1,0,1,0,1,200,1,2,1,128,1,5,1,0,1,167,1,167,1,0,1,175,1,1,1,1,1,0,1,200,1,4,1,128,1,5,1,0,1,167,1,167,1,0,1,175,1,1,1,2,1,0,1,200,1,8,1,128,1,5,1,0,1,167,1,167,1,0,1,175,1,1,1,3,149,0,0,132,255,0,131,0,0,1,1,134,0,0,1,8,1,88,1,16,1,42,1,17,131,0,0,1,2,1,156,1,0,1,0,1,5,1,188,135,0,0,1,36,1,0,1,0,1,2,1,60,1,0,1,0,1,2,1,100,138,0,0,1,2,1,20,131,0,0,1,28,1,0,1,0,1,2,1,8,1,255,1,255,1,3,132,0,0,1,6,131,0,0,1,28,134,0,0,1,2,1,1,131,0,0,1,148,1,0,1,2,131,0,0,1,17,133,0,0,1,156,131,0,0,1,172,1,0,1,0,1,1,1,188,1,0,1,3,131,0,0,1,1,132,0,0,1,1,1,196,134,0,0,1,1,1,212,1,0,1,3,1,0,1,1,1,0,1,1,132,0,0,1,1,1,220,134,0,0,1,1,1,236,1,0,1,3,1,0,1,2,1,0,1,1,132,0,0,1,1,1,220,134,0,0,1,1,1,243,1,0,1,3,1,0,1,3,1,0,1,1,132,0,0,1,1,1,220,134,0,0,1,1,1,250,1,0,1,3,1,0,1,4,1,0,1,1,132,0,0,1,1,1,220,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,17,229,0,0,229,0,0,204,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,14,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,112,1,115,1,95,1,115,1,49,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,95,1,112,1,115,1,95,1,115,1,50,1,0,1,95,1,112,1,115,1,95,1,115,1,51,1,0,1,95,1,112,1,115,1,95,1,115,1,52,1,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,136,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,5,1,124,1,16,1,0,1,12,132,0,0,1,4,134,0,0,1,72,1,198,1,0,1,63,1,0,1,63,131,0,0,1,1,1,0,1,0,1,48,1,80,1,0,1,0,1,113,1,81,1,0,1,0,1,114,1,82,1,0,1,0,1,115,1,83,1,0,1,0,1,116,1,84,1,0,1,0,1,245,1,85,1,62,1,170,1,170,1,171,144,0,0,1,65,1,112,1,0,1,0,1,62,1,128,1,0,1,0,1,63,1,64,1,0,1,0,1,190,1,170,1,170,1,171,1,63,1,42,1,170,1,171,1,63,1,192,1,0,1,0,1,59,1,128,1,128,1,129,1,63,131,0,0,1,191,131,0,0,1,63,1,128,1,0,1,0,1,67,1,127,131,0,0,1,9,1,80,1,12,1,32,1,17,1,16,1,0,1,86,1,0,1,0,1,9,1,0,1,0,1,32,1,19,1,64,1,8,1,16,1,0,1,176,131,0,0,1,4,1,0,1,96,1,21,1,96,1,27,1,18,1,0,1,18,1,0,1,0,1,149,1,0,1,0,1,96,1,33,1,48,1,39,1,18,1,0,1,18,132,0,0,1,9,1,96,1,42,1,96,1,48,1,18,1,0,1,18,133,0,0,1,32,1,54,1,64,1,56,1,16,1,0,1,82,1,0,1,0,1,64,132,0,0,1,48,1,60,1,196,1,0,1,82,133,0,0,1,96,1,63,1,96,1,69,1,86,1,0,1,86,133,0,0,1,96,1,75,1,96,1,81,1,18,1,0,1,18,133,0,0,1,96,1,87,1,96,1,93,1,18,1,0,1,18,133,0,0,1,96,1,99,1,96,1,105,1,18,1,0,1,18,133,0,0,1,80,1,111,1,0,1,0,1,34,133,0,0,1,16,1,32,1,96,1,1,1,31,1,31,1,254,1,136,1,0,1,0,1,64,1,0,1,116,1,0,1,0,1,128,131,0,0,1,108,1,194,1,0,1,0,1,16,1,200,1,7,1,0,1,8,1,0,1,198,1,198,1,0,1,34,1,255,1,255,1,0,1,200,1,7,1,0,1,7,1,0,1,198,1,198,1,0,1,34,1,255,1,255,1,0,1,168,1,136,1,4,1,3,1,0,1,198,1,108,1,196,1,165,1,6,1,255,1,15,1,16,1,56,1,112,1,1,1,159,1,31,1,254,1,136,1,128,1,0,1,64,1,0,1,200,1,7,1,0,1,7,1,24,1,192,1,192,1,0,1,225,1,7,1,7,1,0,1,200,1,8,1,0,1,130,1,0,1,177,1,177,1,198,1,110,1,16,1,6,1,8,1,116,1,0,1,0,1,128,131,0,0,1,198,1,194,1,0,1,0,1,16,1,76,1,64,133,0,0,1,27,1,226,1,0,1,0,1,5,1,200,1,7,1,0,1,6,1,0,1,198,1,192,1,0,1,161,1,0,1,255,1,0,1,76,1,71,1,0,1,5,1,0,1,205,1,205,1,108,1,193,1,6,1,5,1,10,1,76,1,137,1,0,1,8,1,0,1,109,1,108,1,177,1,128,1,5,1,255,1,10,1,0,1,134,1,0,1,8,1,0,1,107,1,203,1,203,1,224,1,8,1,0,1,0,1,152,1,24,1,97,1,1,1,15,1,31,1,255,1,248,1,0,1,0,1,64,1,0,1,184,1,24,1,97,1,1,1,15,1,31,1,255,1,199,1,0,1,0,1,64,1,0,1,16,1,24,1,97,1,1,1,15,1,31,1,254,1,63,1,0,1,0,1,64,1,0,1,48,1,24,1,1,1,1,1,15,1,31,1,254,1,63,1,0,1,0,1,64,1,0,1,0,1,72,1,0,1,1,1,6,1,27,1,198,1,97,1,192,1,0,1,0,1,10,1,20,1,8,1,0,1,0,1,4,1,27,1,198,1,27,1,224,1,0,1,5,1,1,1,4,1,131,1,6,1,5,1,0,1,111,1,109,1,198,1,161,1,8,1,10,1,5,1,44,1,23,1,5,1,6,1,2,1,27,1,192,1,108,1,224,1,0,1,6,1,5,1,45,1,47,1,5,1,8,1,0,1,0,1,177,1,177,1,161,1,6,1,253,1,5,1,200,1,3,1,0,1,5,1,0,1,176,1,27,1,198,1,139,1,5,1,253,1,253,1,188,1,79,1,5,1,6,1,0,1,0,1,198,1,196,1,193,1,6,1,0,1,255,1,189,1,143,1,5,1,6,1,0,1,0,1,108,1,197,1,129,1,6,1,255,1,255,1,200,1,15,1,0,1,5,1,0,1,51,1,78,1,0,1,225,1,5,1,5,1,0,1,200,1,8,131,0,0,1,37,1,208,1,0,1,239,1,5,1,8,1,0,1,184,1,72,1,0,1,1,1,0,1,37,1,208,1,195,1,207,1,5,1,6,1,255,1,188,1,33,1,8,1,8,1,0,1,198,1,198,1,195,1,193,1,0,1,0,1,255,1,16,1,64,1,0,1,1,1,31,1,31,1,254,1,136,1,0,1,0,1,64,1,0,1,200,1,11,131,0,0,1,98,1,177,1,0,1,160,1,0,1,255,1,0,1,200,1,7,1,0,1,1,1,0,1,108,1,192,1,0,1,225,1,0,1,1,1,0,1,200,1,7,131,0,0,1,177,1,192,1,192,1,235,1,0,1,3,1,1,1,200,1,14,131,0,0,1,27,1,140,1,140,1,235,1,0,1,2,1,0,1,200,1,1,131,0,0,1,18,1,18,1,0,1,240,131,0,0,1,88,1,16,133,0,0,1,108,1,226,1,0,1,0,1,128,1,200,1,7,1,0,1,5,1,0,1,201,1,108,1,0,1,225,131,0,0,1,200,1,1,131,0,0,1,190,1,190,1,0,1,240,1,5,1,4,1,0,1,0,1,16,133,0,0,1,108,1,226,131,0,0,1,200,1,14,1,0,1,0,1,4,1,252,1,108,1,252,1,235,1,5,1,0,1,4,1,200,1,1,131,0,0,1,195,1,195,1,0,1,240,131,0,0,1,88,1,16,133,0,0,1,108,1,226,1,0,1,0,1,128,1,112,1,7,1,0,1,2,1,0,1,21,1,108,1,27,1,225,1,0,1,0,1,3,1,200,1,15,1,0,1,0,1,16,1,166,1,205,1,0,1,242,1,2,1,2,1,0,1,76,1,20,1,1,1,1,1,16,1,27,1,27,1,198,1,226,1,0,1,0,1,128,1,200,1,3,1,0,1,1,1,16,1,109,1,108,1,198,1,203,1,0,1,1,1,254,1,144,1,8,1,0,1,33,1,159,1,31,1,246,1,136,1,0,1,0,1,192,1,0,1,168,1,16,1,1,1,0,1,16,1,0,1,0,1,67,1,194,1,0,1,0,1,9,1,200,1,7,1,0,1,0,1,16,1,108,1,192,1,0,1,225,1,1,1,0,1,0,1,200,1,7,1,0,1,4,1,16,1,192,1,192,1,0,1,225,131,0,0,1,200,1,7,1,0,1,0,1,24,1,108,1,98,1,98,1,139,1,2,1,1,1,0,1,200,1,7,1,0,1,0,1,24,1,177,1,190,1,180,1,171,1,2,1,2,1,0,1,200,1,3,1,0,1,1,1,24,1,108,1,196,1,0,1,225,1,2,1,2,1,0,1,200,1,15,1,0,1,6,1,24,1,114,1,130,1,0,1,225,1,2,1,2,1,0,1,200,1,8,1,0,1,0,1,26,1,108,1,177,1,0,1,224,1,1,1,6,1,0,1,200,1,1,1,0,1,1,1,26,1,108,1,108,1,0,1,160,1,6,1,252,1,0,1,200,1,7,1,0,1,0,1,24,1,198,1,98,1,180,1,171,1,2,1,3,1,0,1,200,1,7,1,0,1,0,1,24,1,27,1,190,1,180,1,171,1,6,1,4,1,0,1,200,1,7,1,0,1,0,1,24,1,198,1,98,1,180,1,171,1,6,1,5,1,0,1,200,1,7,1,0,1,0,1,24,1,177,1,190,1,180,1,171,1,1,1,6,1,0,1,200,1,7,1,0,1,0,1,24,1,108,1,98,1,180,1,171,1,1,1,7,1,0,1,200,1,7,1,0,1,0,1,24,1,27,1,190,1,180,1,171,1,0,1,8,1,0,1,200,1,7,1,0,1,4,1,24,1,101,1,108,1,0,1,162,1,0,1,253,1,0,1,20,1,14,1,0,1,6,1,0,1,108,1,140,1,108,1,129,1,5,1,6,1,11,1,12,1,135,1,1,1,10,1,0,1,177,1,180,1,177,1,129,1,5,1,5,1,15,1,200,1,13,131,0,0,1,108,1,114,1,114,1,139,1,5,1,1,1,0,1,76,1,39,1,0,1,1,1,0,1,98,1,98,1,177,1,1,1,12,1,12,1,9,1,172,1,23,1,3,1,11,1,0,1,108,1,180,1,2,1,129,1,5,1,4,1,12,1,200,1,7,1,0,1,12,1,0,1,177,1,190,1,200,1,171,1,5,1,2,1,0,1,172,1,39,1,3,1,9,1,0,1,190,1,190,1,0,1,193,1,5,1,5,1,12,1,173,1,72,1,3,1,0,1,0,1,190,1,190,1,1,1,144,1,5,1,13,1,12,1,101,1,17,1,6,1,0,1,0,1,190,1,190,1,182,1,176,1,2,1,13,1,9,1,8,1,68,1,1,1,0,1,0,1,190,1,190,1,108,1,176,1,5,1,13,1,0,1,200,1,7,1,0,1,2,1,0,1,198,1,98,1,180,1,171,1,5,1,3,1,12,1,64,1,72,1,0,1,9,1,0,1,198,1,108,1,108,1,161,1,0,1,252,1,0,1,200,1,7,1,0,1,2,1,0,1,205,1,177,1,180,1,235,1,11,1,5,1,2,1,200,1,7,1,0,1,2,1,0,1,101,1,198,1,180,1,235,1,10,1,5,1,2,1,168,1,19,1,0,1,1,1,0,1,111,1,109,1,66,1,128,1,9,1,254,1,14,1,58,1,24,1,0,1,1,1,0,1,27,1,108,1,108,1,225,1,1,1,1,1,0,1,200,1,7,1,0,1,5,1,0,1,18,1,198,1,180,1,235,1,6,1,5,1,2,1,172,1,20,131,0,0,1,108,1,177,1,128,1,129,1,0,1,14,1,15,1,200,1,8,1,0,1,5,1,0,1,27,1,198,1,198,1,236,1,3,1,0,1,1,1,168,1,23,1,1,1,2,1,0,1,27,1,192,1,192,1,193,1,4,1,4,1,11,1,200,1,7,1,0,1,4,1,0,1,177,1,98,1,180,1,171,1,1,1,7,1,5,1,200,1,7,1,0,1,4,1,0,1,108,1,190,1,180,1,171,1,6,1,8,1,4,1,168,1,39,1,1,1,5,1,0,1,101,1,108,1,128,1,130,1,4,1,253,1,11,1,200,1,7,1,0,1,1,1,0,1,27,1,108,1,99,1,172,1,3,1,253,1,1,1,168,1,23,1,0,1,1,1,0,1,192,1,108,1,131,1,193,1,1,1,8,1,15,1,200,1,1,131,0,0,1,27,1,27,1,108,1,235,1,4,1,5,1,0,1,200,1,13,131,0,0,1,108,1,177,1,240,1,235,1,0,1,8,1,1,1,168,1,23,1,0,1,1,1,0,1,180,1,20,1,67,1,225,1,3,1,0,1,15,1,200,1,7,1,0,1,1,1,0,1,192,1,27,1,192,1,235,1,2,1,2,1,1,1,200,1,13,131,0,0,1,108,1,240,1,240,1,235,1,0,1,5,1,1,1,200,1,7,1,0,1,1,1,0,1,20,1,192,1,0,1,225,1,0,1,7,1,0,1,20,1,16,133,0,0,1,182,1,226,1,0,1,0,1,1,1,168,1,33,131,0,0,1,108,1,108,1,1,1,194,1,0,1,1,1,255,1,200,1,1,131,0,0,1,177,1,108,1,0,1,225,131,0,0,1,52,1,16,1,0,1,0,1,1,1,0,1,0,1,108,1,226,131,0,0,1,201,1,8,1,128,1,0,1,4,1,108,1,27,1,0,1,161,1,0,1,254,1,0,1,200,1,1,1,0,1,0,1,4,1,108,1,27,1,0,1,161,1,0,1,254,1,0,1,168,1,16,133,0,0,1,128,1,194,1,0,1,0,1,9,1,76,1,16,133,0,0,1,108,1,226,131,0,0,1,201,1,7,1,128,1,0,1,0,1,192,1,108,1,0,1,225,1,1,150,0,0,132,255,0,138,0,0,1,3,1,112,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,228,1,0,1,0,1,1,1,140,135,0,0,1,36,134,0,0,1,1,1,84,138,0,0,1,1,1,44,131,0,0,1,28,1,0,1,0,1,1,1,31,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,134,0,0,1,1,1,24,131,0,0,1,48,1,0,1,2,131,0,0,1,13,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,13,229,0,0,229,0,0,140,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,134,0,0,1,1,1,140,1,0,1,81,1,0,1,5,138,0,0,1,72,1,198,131,0,0,1,1,131,0,0,1,5,131,0,0,1,15,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,4,1,0,1,0,1,80,1,5,1,0,1,0,1,48,1,6,1,0,1,0,1,96,1,7,1,0,1,32,1,112,1,8,1,0,1,0,1,48,1,80,1,0,1,1,1,113,1,81,1,0,1,4,1,114,1,82,1,0,1,7,1,115,1,83,1,0,1,10,1,116,1,84,1,0,1,11,1,245,1,85,1,0,1,0,1,16,1,17,131,0,0,1,18,131,0,0,1,19,1,0,1,0,1,16,1,20,131,0,0,1,21,131,0,0,1,22,1,0,1,0,1,16,1,23,131,0,0,1,24,131,0,0,1,25,1,0,1,0,1,16,1,26,1,0,1,0,1,16,1,27,131,0,0,1,28,131,0,0,1,29,131,0,0,1,30,1,0,1,0,1,16,1,31,1,241,1,85,1,80,1,4,1,0,1,0,1,18,1,1,1,194,133,0,0,1,64,1,9,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,13,1,96,1,19,1,18,1,0,1,18,133,0,0,1,96,1,25,1,16,1,31,1,18,1,0,1,34,131,0,0,1,5,1,248,1,80,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,64,131,0,0,1,15,1,200,132,0,0,1,5,1,248,1,48,131,0,0,1,14,1,136,132,0,0,1,5,1,248,1,32,131,0,0,1,14,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,14,1,136,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,5,1,4,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,5,1,5,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,5,1,6,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,5,1,7,1,0,1,200,1,1,131,0,0,1,167,1,167,1,0,1,175,1,5,1,8,1,0,1,200,1,2,131,0,0,1,167,1,167,1,0,1,175,1,5,1,9,1,0,1,200,1,4,131,0,0,1,167,1,167,1,0,1,175,1,5,1,10,1,0,1,200,1,8,131,0,0,1,167,1,167,1,0,1,175,1,5,1,11,1,0,1,200,1,3,1,128,1,0,1,0,1,176,1,176,1,0,1,226,1,4,1,4,1,0,1,200,1,1,1,128,1,1,1,0,1,190,1,190,1,0,1,176,1,3,1,8,1,0,1,200,1,2,1,128,1,1,1,0,1,190,1,190,1,0,1,176,1,3,1,9,1,0,1,200,1,4,1,128,1,1,1,0,1,190,1,190,1,0,1,176,1,3,1,10,1,0,1,200,1,1,1,128,1,2,1,0,1,190,1,190,1,0,1,176,1,2,1,8,1,0,1,200,1,2,1,128,1,2,1,0,1,190,1,190,1,0,1,176,1,2,1,9,1,0,1,200,1,4,1,128,1,2,1,0,1,190,1,190,1,0,1,176,1,2,1,10,1,0,1,200,1,1,1,128,1,3,1,0,1,190,1,190,1,0,1,176,1,1,1,8,1,0,1,200,1,2,1,128,1,3,1,0,1,190,1,190,1,0,1,176,1,1,1,9,1,0,1,200,1,4,1,128,1,3,1,0,1,190,1,190,1,0,1,176,1,1,1,10,1,0,1,200,1,7,1,128,1,4,1,2,1,192,1,192,1,0,1,160,1,0,1,12,1,0,1,200,1,1,1,128,1,5,1,0,1,167,1,167,1,0,1,175,131,0,0,1,200,1,2,1,128,1,5,1,0,1,167,1,167,1,0,1,175,1,0,1,1,1,0,1,200,1,4,1,128,1,5,1,0,1,167,1,167,1,0,1,175,1,0,1,2,1,0,1,200,1,8,1,128,1,5,1,0,1,167,1,167,1,0,1,175,1,0,1,3,140,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {36,80,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,210,95,99,248,249,53,127,236,255,254,191,254,190,223,92,126,255,181,241,55,253,255,127,210,239,126,99,250,255,175,163,159,253,255,237,249,245,232,255,191,255,101,243,251,79,127,13,55,238,173,95,75,190,3,57,254,255,58,238,15,125,152,110,171,144,110,255,210,111,42,223,253,91,191,198,143,232,246,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,236,231,215,255,53,56,190,157,32,80,51,113,218,193,111,38,223,253,58,250,255,111,34,78,211,126,10,244,243,155,252,26,2,247,247,253,205,34,109,40,102,108,118,126,141,95,227,55,210,54,127,208,80,155,93,215,230,47,27,106,179,231,218,252,83,67,109,238,185,54,255,221,80,155,125,161,1,190,254,117,228,43,251,32,21,128,176,182,251,185,161,25,104,250,155,69,190,199,223,177,247,126,140,254,255,164,204,151,179,98,121,1,196,126,221,129,247,17,95,199,222,255,13,233,255,103,203,166,205,150,83,64,224,49,188,158,103,179,188,22,216,192,11,244,199,231,120,55,245,222,253,63,232,255,191,157,55,217,251,52,184,151,154,235,192,243,239,81,135,255,155,198,240,120,158,254,102,191,198,175,241,123,255,102,238,239,57,253,254,51,222,223,127,28,253,254,23,121,127,255,77,244,251,63,230,253,253,175,209,239,255,149,255,189,151,95,218,210,223,209,253,159,162,223,255,31,212,246,127,163,255,255,169,250,247,111,69,109,126,51,250,255,31,16,105,187,79,159,237,252,230,174,237,115,250,253,219,244,255,191,40,210,182,165,207,86,94,219,63,142,126,255,163,180,221,175,39,63,184,253,255,77,143,33,207,62,8,253,107,255,223,255,247,255,245,127,255,121,191,198,201,155,227,39,191,19,253,121,248,107,201,103,120,231,119,146,102,41,104,248,167,233,251,191,57,253,251,231,209,207,191,142,254,255,247,253,154,70,222,126,173,95,227,31,83,160,255,26,127,246,107,210,127,191,222,175,241,239,233,103,255,29,127,246,107,209,103,201,175,241,203,245,179,95,231,215,194,103,104,249,155,252,26,191,145,206,207,239,196,159,253,58,244,217,111,241,107,252,110,250,153,228,95,254,218,191,246,215,228,182,191,206,175,161,67,250,209,211,121,84,231,252,181,191,14,209,243,215,228,255,188,207,119,241,249,111,212,255,124,111,224,243,123,3,159,239,247,63,167,143,239,253,254,59,191,198,23,197,180,174,154,234,188,77,183,94,221,73,191,253,252,245,243,84,36,54,61,169,22,171,162,164,95,30,142,247,62,29,63,188,191,55,222,59,216,223,255,53,126,130,212,194,111,254,107,252,166,127,17,129,248,61,72,67,210,255,127,141,127,16,240,240,249,111,193,159,175,142,127,141,95,227,247,196,119,71,127,237,95,243,215,252,3,248,252,183,148,246,191,231,175,241,107,252,129,39,127,240,31,244,7,29,162,253,239,66,108,73,250,229,15,34,206,249,147,228,247,95,243,15,250,53,127,141,95,95,127,255,181,254,160,95,203,254,254,107,255,65,191,182,253,253,215,249,131,126,29,251,251,175,251,7,253,186,191,198,111,202,191,19,184,63,235,215,248,13,126,211,191,72,127,255,147,126,77,239,247,95,203,251,253,215,246,126,255,117,232,247,39,44,6,191,41,225,240,159,81,187,255,236,47,250,117,249,239,95,147,254,254,53,254,160,223,244,215,248,107,254,34,102,107,234,243,215,248,53,254,154,63,24,226,251,227,252,238,111,240,7,253,102,191,198,87,127,209,157,95,227,215,253,181,126,51,81,49,127,49,224,252,154,10,231,215,86,56,52,22,26,207,127,198,255,255,214,175,193,186,239,215,194,103,191,57,189,251,9,253,253,123,179,10,252,181,232,157,255,251,15,198,103,191,198,175,241,213,31,36,48,255,26,130,249,215,252,197,191,158,246,245,235,254,26,255,247,159,4,120,191,22,209,129,218,210,223,255,217,159,132,239,72,170,254,160,132,250,151,223,127,45,250,253,171,191,232,215,97,241,252,181,137,118,255,25,245,243,95,65,221,240,24,126,29,166,225,127,198,159,3,143,95,139,198,244,155,255,26,255,247,95,244,251,211,119,68,67,250,252,47,251,131,126,77,194,91,190,251,13,136,198,95,253,65,248,238,215,227,239,254,58,251,29,241,16,253,253,167,241,119,191,62,125,247,235,252,26,255,27,127,247,251,115,31,248,251,151,243,223,191,22,143,255,55,160,62,127,13,250,255,87,127,16,240,146,177,252,53,127,18,198,240,107,210,152,1,15,243,76,248,255,65,242,251,175,243,7,253,250,246,119,224,240,107,252,65,128,243,107,51,93,121,220,127,176,204,199,111,192,99,197,216,127,93,237,23,99,147,54,248,251,119,161,119,49,126,249,254,215,166,191,241,253,111,161,127,27,250,36,191,198,127,246,23,253,150,60,94,249,219,167,221,111,65,240,127,11,157,255,95,139,199,252,148,231,72,222,7,77,174,232,255,201,31,244,99,138,143,188,255,159,241,223,191,166,254,45,244,150,239,205,28,27,186,252,154,250,247,175,235,254,230,255,27,62,193,59,248,94,120,230,55,176,60,243,68,251,7,159,253,58,74,103,146,19,157,239,191,70,199,139,191,191,66,255,127,146,140,233,55,4,30,152,227,63,233,215,166,121,150,207,208,230,175,1,111,254,73,191,22,205,217,239,198,60,36,112,126,3,134,249,107,98,60,127,146,25,3,254,22,24,191,134,125,95,191,251,131,209,14,239,255,58,250,190,240,16,243,62,243,227,175,171,159,99,62,127,141,240,179,191,8,124,246,235,42,237,127,93,129,73,159,253,53,252,25,198,242,235,209,103,191,206,175,241,95,211,255,255,49,219,134,250,3,207,7,239,253,186,252,89,8,235,215,227,207,254,26,251,153,240,245,95,195,255,255,45,136,247,77,187,95,255,215,0,29,255,111,219,167,225,255,95,71,249,86,254,230,113,19,206,255,247,31,108,222,251,13,188,247,126,67,197,21,191,67,174,48,79,191,46,203,3,241,165,234,3,192,33,89,6,143,240,223,138,247,31,36,48,220,223,248,255,239,173,125,252,26,138,255,175,167,184,201,103,110,236,102,238,127,109,143,182,250,217,95,4,185,115,124,45,180,253,181,9,222,175,165,176,126,61,254,236,191,166,255,255,99,182,141,161,173,255,158,161,173,255,153,161,173,251,12,178,254,215,240,255,13,109,209,206,208,214,244,41,237,190,226,255,187,247,126,13,254,191,161,45,222,51,180,245,113,197,239,191,169,210,238,215,167,191,127,35,106,131,255,155,191,169,47,250,236,255,254,139,126,3,219,207,127,246,7,253,198,244,61,254,254,181,127,141,223,156,231,226,55,214,246,208,151,166,223,244,215,128,206,248,205,249,247,223,196,234,12,240,63,254,254,234,47,194,92,252,218,108,19,254,26,125,7,180,16,24,192,241,183,180,239,252,90,252,221,111,170,114,109,254,38,122,252,197,242,247,111,194,127,255,134,191,198,159,164,48,127,19,134,9,126,1,174,34,55,191,157,229,7,99,135,208,39,232,242,107,176,12,254,223,12,95,120,64,100,29,122,227,215,254,53,126,21,255,13,25,132,29,1,191,252,222,74,75,163,155,126,115,111,94,192,63,191,134,55,167,191,134,242,213,175,193,176,252,207,254,51,166,171,153,135,95,131,251,251,207,248,115,240,252,175,201,56,126,197,227,192,223,191,150,226,76,99,249,131,140,253,250,49,165,135,232,73,233,195,252,253,107,48,253,190,98,61,44,176,254,111,171,227,197,78,200,56,127,77,219,246,175,177,176,48,110,192,54,253,24,184,191,166,226,173,239,19,43,252,46,140,243,255,253,127,255,26,252,80,99,246,165,127,13,125,158,211,72,127,141,95,251,255,34,95,122,101,125,233,63,246,215,148,207,208,246,119,226,86,191,70,250,135,209,103,79,245,253,223,152,254,125,78,63,127,95,250,255,136,59,249,141,65,176,95,99,31,109,126,77,179,102,235,124,223,223,216,116,246,255,179,71,99,106,59,206,95,199,125,245,94,79,183,241,229,215,244,77,197,183,251,141,254,160,95,195,250,133,191,49,205,189,249,253,55,33,254,51,191,195,103,176,190,35,241,187,252,254,235,50,191,201,239,191,54,243,176,252,14,157,248,235,235,239,240,81,126,3,239,93,130,245,31,153,119,137,14,250,59,100,245,215,215,223,161,7,204,239,191,54,245,101,126,255,117,168,47,243,251,175,139,190,254,35,177,253,108,55,254,36,225,101,216,250,175,216,78,67,38,229,111,244,249,159,253,73,224,105,247,25,235,98,251,153,248,180,44,203,252,153,248,41,191,230,127,4,185,128,14,148,191,127,45,254,251,55,177,127,255,58,252,247,111,106,255,254,13,248,239,223,236,215,16,223,79,253,89,198,75,237,59,225,252,213,159,132,239,126,109,145,57,198,81,236,3,249,218,170,55,240,189,250,6,230,251,255,136,108,199,159,4,157,228,236,130,192,251,245,9,158,179,29,14,6,62,3,13,127,253,206,123,198,199,254,13,244,61,252,253,27,232,123,134,22,160,247,111,160,239,25,90,252,122,74,139,95,67,199,250,235,41,45,126,77,251,183,208,226,215,178,127,11,45,96,67,161,159,48,111,248,155,236,207,95,44,180,249,181,255,35,248,229,70,171,64,125,252,40,94,255,255,231,243,163,120,253,71,241,250,143,226,245,31,197,235,63,138,215,127,20,175,255,40,94,255,81,188,254,163,120,221,125,247,107,234,56,191,201,120,157,62,11,226,245,125,234,92,226,245,127,232,215,54,190,244,63,251,155,200,103,4,216,250,210,255,216,111,178,57,94,255,183,0,235,215,252,121,23,175,99,173,221,142,243,223,186,241,141,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,77,62,95,119,189,243,39,40,87,255,159,255,26,191,233,95,244,107,252,26,191,231,239,233,195,211,156,249,31,100,214,65,127,93,142,241,221,186,166,89,7,69,238,200,172,131,34,151,107,214,65,37,142,151,223,145,3,48,235,160,10,243,27,91,239,148,220,192,111,250,7,253,231,156,23,255,207,254,164,49,247,241,155,254,77,191,134,228,47,126,29,124,247,235,210,218,222,111,156,254,103,52,198,175,254,38,250,224,215,197,103,242,247,175,241,55,209,31,127,18,218,134,159,255,53,127,147,228,173,187,159,255,223,127,19,114,243,248,92,242,190,200,95,252,103,127,146,233,235,215,210,190,126,19,175,47,124,246,155,120,125,105,254,197,251,92,250,234,127,46,125,253,90,182,175,95,139,251,210,188,204,175,131,28,57,250,250,77,189,190,240,217,111,234,245,165,121,29,239,115,233,171,255,185,244,245,107,219,190,126,29,238,203,228,168,52,23,243,39,73,238,251,215,252,143,100,93,212,173,1,255,154,140,147,91,3,150,117,93,183,6,44,235,178,146,151,251,117,120,158,133,102,242,247,175,197,127,155,60,244,175,67,125,255,90,218,55,254,166,252,210,127,244,235,240,239,242,254,175,69,253,73,222,221,252,253,235,240,223,137,7,255,215,246,224,35,135,246,235,116,250,251,181,189,254,8,127,254,222,239,255,215,246,250,255,53,120,253,201,253,253,107,119,240,249,181,59,248,252,218,125,124,254,163,95,195,107,255,235,232,58,177,105,79,253,241,223,104,111,214,22,64,175,223,208,254,253,107,242,223,191,129,253,251,215,226,191,127,204,254,253,235,240,223,230,253,159,141,117,105,172,25,184,236,217,143,214,165,255,255,250,252,104,93,250,71,235,210,63,90,151,254,209,186,244,143,214,165,127,180,46,253,163,117,233,31,173,75,255,104,93,218,125,247,107,234,56,191,201,117,105,60,254,186,244,239,75,208,100,93,250,43,235,75,255,46,191,166,124,134,31,191,147,52,75,127,59,250,99,231,215,224,206,120,93,250,128,126,126,251,215,248,249,179,6,253,117,115,29,234,111,253,65,95,55,167,161,239,126,99,185,11,177,217,191,230,127,196,54,84,227,177,95,67,226,181,63,201,143,167,127,13,246,179,92,60,141,191,77,60,141,248,152,100,226,79,242,227,99,252,237,199,199,248,59,177,250,231,215,252,143,126,109,175,61,226,215,95,219,107,143,248,245,215,246,218,35,126,253,117,188,246,136,95,127,29,175,61,226,215,95,71,219,27,31,4,248,249,241,235,175,161,239,235,248,248,111,63,126,253,53,188,247,127,54,227,215,255,39,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((Character.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Character.sid0))
			{
				this.AlbedoSampler = value;
				return true;
			}
			if ((id == Character.sid1))
			{
				this.CubeRgbmSampler = value;
				return true;
			}
			if ((id == Character.sid2))
			{
				this.NormalSampler = value;
				return true;
			}
			if ((id == Character.sid3))
			{
				this.ShadowSampler = value;
				return true;
			}
			if ((id == Character.sid4))
			{
				this.SofSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((Character.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == Character.tid2))
			{
				this.SofTexture = value;
				return true;
			}
			if ((id == Character.tid3))
			{
				this.AlbedoTexture = value;
				return true;
			}
			if ((id == Character.tid4))
			{
				this.NormalTexture = value;
				return true;
			}
			return false;
		}
	}
}
