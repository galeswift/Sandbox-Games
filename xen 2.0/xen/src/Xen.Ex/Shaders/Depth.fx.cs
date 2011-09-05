// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = Depth.fx
// Namespace = Xen.Ex.Shaders

namespace Xen.Ex.Shaders
{
	
	/// <summary><para>Technique 'DepthOutRg' generated from file 'Depth.fx'</para><para>Vertex Shader: approximately 13 instruction slots used, 10 registers</para><para>Pixel Shader: approximately 5 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	public sealed class DepthOutRg : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DepthOutRg' shader</summary>
		public DepthOutRg()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.sc3 = -1;
			this.sc4 = -1;
			this.sc5 = -1;
			this.sc6 = -1;
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DepthOutRg.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DepthOutRg.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.vbreg_change = (this.vbreg_change | ic);
			this.vireg_change = (this.vireg_change | ic);
			// Set the value for attribute 'cameraNearFar'
			this.vreg_change = (this.vreg_change | state.SetCameraNearFarVector2(ref this.vreg[9], ref this.sc0));
			// Set the value for attribute 'viewDirection'
			this.vreg_change = (this.vreg_change | state.SetViewDirectionVector3(ref this.vreg[7], ref this.sc1));
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[8], ref this.sc2));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[4], ref this.vreg[5], ref this.vreg[6], ref unused, ref this.sc3));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc4));
			if ((this.vreg_change == true))
			{
				DepthOutRg.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((ext == Xen.Graphics.ShaderSystem.ShaderExtension.Blending))
			{
				ic = (ic | state.SetBlendMatricesDirect(DepthOutRg.fx.vsb_c, ref this.sc5));
			}
			if ((ext == Xen.Graphics.ShaderSystem.ShaderExtension.Instancing))
			{
				this.vireg_change = (this.vireg_change | state.SetViewProjectionMatrix(ref this.vireg[0], ref this.vireg[1], ref this.vireg[2], ref this.vireg[3], ref this.sc6));
				if ((this.vireg_change == true))
				{
					DepthOutRg.fx.vsi_c.SetValue(this.vireg);
					this.vireg_change = false;
					ic = true;
				}
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DepthOutRg.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DepthOutRg.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DepthOutRg.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DepthOutRg.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DepthOutRg.fx, DepthOutRg.fxb, 14, 7);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DepthOutRg.vin[i]));
			index = DepthOutRg.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
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
		/// <summary>Change ID for Semantic bound attribute 'cameraNearFar'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'viewDirection'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc2;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc3;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc4;
		/// <summary>Change ID for Semantic bound attribute '__BLENDMATRICES__GENMATRIX'</summary>
		private int sc5;
		/// <summary>Change ID for Semantic bound attribute 'viewProj'</summary>
		private int sc6;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[10];
		/// <summary>Instancing shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vireg = new Microsoft.Xna.Framework.Vector4[4];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,32,152,0,8,254,255,9,1,0,0,15,156,135,0,1,3,131,0,1,1,131,0,1,192,135,0,1,10,131,0,1,4,131,0,1,1,229,0,0,190,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,1,0,1,0,1,14,1,104,135,0,0,1,216,131,0,0,1,4,131,0,0,1,1,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,153,0,0,1,7,1,95,1,118,1,115,1,98,1,95,1,99,133,0,0,1,3,131,0,0,1,1,1,0,1,0,1,14,1,208,135,0,0,1,4,131,0,0,1,4,131,0,0,1,1,195,0,0,1,7,1,95,1,118,1,115,1,105,1,95,1,99,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,3,131,0,0,1,16,131,0,0,1,4,143,0,0,1,4,131,0,0,1,15,131,0,0,1,4,143,0,0,1,9,1,66,1,108,1,101,1,110,1,100,1,105,1,110,1,103,135,0,0,1,5,131,0,0,1,16,131,0,0,1,4,143,0,0,1,6,131,0,0,1,15,131,0,0,1,4,143,0,0,1,11,1,73,1,110,1,115,1,116,1,97,1,110,1,99,1,105,1,110,1,103,133,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,3,131,0,0,1,1,131,0,0,1,9,131,0,0,1,7,131,0,0,1,4,131,0,0,1,32,139,0,0,1,204,131,0,0,1,232,138,0,0,1,14,1,116,1,0,1,0,1,14,1,144,138,0,0,1,15,1,144,135,0,0,1,3,1,0,1,0,1,15,1,12,135,0,0,1,2,131,0,0,1,92,134,0,0,1,14,1,224,1,0,1,0,1,14,1,220,131,0,0,1,93,134,0,0,1,14,1,248,1,0,1,0,1,14,1,244,1,0,1,0,1,15,1,64,135,0,0,1,2,131,0,0,1,92,134,0,0,1,15,1,20,1,0,1,0,1,15,1,16,131,0,0,1,93,134,0,0,1,15,1,44,1,0,1,0,1,15,1,40,1,0,1,0,1,15,1,128,135,0,0,1,2,131,0,0,1,92,134,0,0,1,15,1,84,1,0,1,0,1,15,1,80,131,0,0,1,93,134,0,0,1,15,1,108,1,0,1,0,1,15,1,104,135,0,0,1,6,135,0,0,1,2,132,255,0,131,0,0,1,1,135,0,0,1,172,1,16,1,42,1,17,132,0,0,1,124,131,0,0,1,48,135,0,0,1,36,135,0,0,1,88,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,48,1,16,134,0,0,1,4,134,0,0,1,4,1,33,1,0,1,1,1,0,1,1,131,0,0,1,1,1,0,1,0,1,16,1,80,132,0,0,1,32,1,1,1,196,1,0,1,34,131,0,0,1,8,1,32,133,0,0,1,108,1,226,131,0,0,1,200,1,139,1,192,1,0,1,0,1,176,1,176,1,0,1,226,150,0,0,1,2,132,255,0,138,0,0,1,2,1,208,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,212,131,0,0,1,252,135,0,0,1,36,134,0,0,1,1,1,144,138,0,0,1,1,1,104,131,0,0,1,28,1,0,1,0,1,1,1,91,1,255,1,254,1,3,132,0,0,1,2,131,0,0,1,28,134,0,0,1,1,1,84,131,0,0,1,68,1,0,1,2,131,0,0,1,10,133,0,0,1,76,131,0,0,1,92,131,0,0,1,252,1,0,1,2,1,0,1,10,1,0,1,4,132,0,0,1,1,1,4,1,0,1,0,1,1,1,20,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,10,229,0,0,193,0,0,1,95,1,118,1,115,1,105,1,95,1,99,1,0,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,4,198,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,252,1,0,1,1,1,0,1,4,138,0,0,1,8,1,33,131,0,0,1,1,131,0,0,1,5,131,0,0,1,1,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,12,1,0,1,4,1,0,1,13,1,0,1,5,1,0,1,14,1,0,1,6,1,0,1,63,1,0,1,7,1,0,1,0,1,48,1,80,1,0,1,0,1,16,1,19,1,241,1,85,1,80,1,3,1,0,1,0,1,18,1,1,1,194,133,0,0,1,96,1,8,1,32,1,14,1,18,1,0,1,18,135,0,0,1,64,1,16,1,196,1,0,1,34,131,0,0,1,5,1,248,1,32,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,48,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,64,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,6,1,136,132,0,0,1,200,1,15,131,0,0,1,27,1,0,1,0,1,225,1,2,1,0,1,0,1,200,1,15,131,0,0,1,198,1,0,1,0,1,235,1,2,1,4,1,0,1,200,1,15,131,0,0,1,177,1,148,1,148,1,235,1,2,1,3,1,0,1,200,1,15,131,0,0,1,108,1,248,1,148,1,235,1,2,1,1,1,0,1,200,1,1,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,10,1,0,1,200,1,2,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,11,1,0,1,200,1,4,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,12,1,0,1,200,1,8,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,13,1,0,1,200,1,7,1,0,1,0,1,2,1,20,1,192,1,0,1,160,1,0,1,8,1,0,1,100,1,18,131,0,0,1,190,1,190,1,188,1,144,1,0,1,7,1,9,1,76,1,18,1,0,1,0,1,2,1,177,1,108,1,108,1,160,1,0,1,9,1,0,1,200,1,3,1,128,1,0,1,0,1,177,1,108,1,0,1,225,150,0,0,1,1,132,255,0,131,0,0,1,1,135,0,0,1,172,1,16,1,42,1,17,132,0,0,1,124,131,0,0,1,48,135,0,0,1,36,135,0,0,1,88,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,48,1,16,134,0,0,1,4,134,0,0,1,4,1,33,1,0,1,1,1,0,1,1,131,0,0,1,1,1,0,1,0,1,16,1,80,132,0,0,1,32,1,1,1,196,1,0,1,34,131,0,0,1,8,1,32,133,0,0,1,108,1,226,131,0,0,1,200,1,139,1,192,1,0,1,0,1,176,1,176,1,0,1,226,150,0,0,1,1,132,255,0,138,0,0,1,17,1,120,1,16,1,42,1,17,1,1,1,0,1,0,1,15,1,52,1,0,1,0,1,2,1,68,135,0,0,1,36,1,0,1,0,1,14,1,208,1,0,1,0,1,14,1,248,138,0,0,1,14,1,168,131,0,0,1,28,1,0,1,0,1,14,1,155,1,255,1,254,1,3,132,0,0,1,2,131,0,0,1,28,134,0,0,1,14,1,148,131,0,0,1,68,1,0,1,2,131,0,0,1,10,133,0,0,1,76,131,0,0,1,92,131,0,0,1,252,1,0,1,2,1,0,1,10,1,0,1,216,132,0,0,1,1,1,4,1,0,1,0,1,1,1,20,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,10,229,0,0,193,0,0,1,95,1,118,1,115,1,98,1,95,1,99,1,0,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,216,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,156,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,2,1,4,1,0,1,1,1,0,1,6,138,0,0,1,8,1,33,131,0,0,1,1,131,0,0,1,3,131,0,0,1,1,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,5,1,0,1,0,1,16,1,6,1,0,1,48,1,32,1,7,1,0,1,0,1,48,1,80,1,0,1,0,1,16,1,41,180,0,0,1,63,1,128,1,0,1,0,1,64,1,64,134,0,0,1,112,1,21,1,48,1,5,1,0,1,0,1,18,1,0,1,194,133,0,0,1,96,1,8,1,96,1,14,1,18,1,0,1,18,133,0,0,1,96,1,20,1,96,1,26,1,18,1,0,1,18,133,0,0,1,16,1,32,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,33,1,48,1,39,1,18,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,4,1,200,132,0,0,1,5,1,248,1,64,131,0,0,1,2,1,208,132,0,0,1,5,1,248,132,0,0,1,6,1,136,132,0,0,1,200,1,15,1,0,1,6,1,0,1,0,1,198,1,0,1,161,1,0,1,255,1,0,1,92,1,8,1,0,1,3,1,0,1,198,1,27,1,27,1,161,1,1,1,3,1,6,1,200,1,15,1,0,1,2,1,160,1,198,1,136,1,0,1,161,1,4,1,10,1,0,1,200,1,15,1,0,1,5,1,160,1,198,1,136,1,0,1,161,1,4,1,11,1,0,1,92,1,15,1,0,1,0,1,160,1,198,1,136,1,198,1,161,1,4,1,12,1,6,1,200,1,15,1,0,1,0,1,160,1,177,1,136,1,0,1,171,1,4,1,12,1,0,1,200,1,15,1,0,1,5,1,160,1,177,1,136,1,0,1,171,1,4,1,11,1,5,1,200,1,15,1,0,1,2,1,160,1,177,1,136,1,0,1,171,1,4,1,10,1,2,1,92,1,2,1,0,1,4,1,0,1,198,1,27,1,177,1,161,1,1,1,1,1,6,1,200,1,15,1,0,1,2,1,160,1,27,1,52,1,148,1,171,1,4,1,10,1,2,1,200,1,15,1,0,1,5,1,160,1,27,1,52,1,148,1,171,1,4,1,11,1,5,1,200,1,15,1,0,1,0,1,160,1,27,1,52,1,148,1,171,1,4,1,12,1,0,1,92,1,8,1,0,1,4,1,0,1,198,1,27,1,108,1,161,1,1,1,0,1,6,1,200,1,15,1,0,1,0,1,160,1,108,1,208,1,148,1,171,1,4,1,12,1,0,1,200,1,15,1,0,1,5,1,160,1,108,1,208,1,148,1,171,1,4,1,11,1,5,1,200,1,15,1,0,1,2,1,160,1,108,1,208,1,148,1,171,1,4,1,10,1,2,1,200,1,1,1,0,1,2,1,0,1,170,1,170,1,0,1,239,1,2,1,1,1,0,1,200,1,4,1,0,1,2,1,0,1,170,1,170,1,0,1,239,1,0,1,1,1,0,1,172,1,34,1,0,1,2,1,0,1,170,1,170,1,2,1,207,1,5,1,1,1,4,1,200,1,1,1,0,1,3,1,0,1,190,1,190,1,0,1,176,1,2,1,2,1,0,1,200,1,4,1,0,1,3,1,0,1,190,1,190,1,0,1,176,1,2,1,3,1,0,1,20,1,17,1,0,1,4,1,0,1,190,1,190,1,198,1,176,1,2,1,1,1,1,1,168,1,36,1,3,1,4,1,0,1,190,1,190,1,0,1,144,1,2,1,0,1,2,1,200,1,3,1,128,1,62,1,0,1,110,1,179,1,0,1,224,1,4,1,4,1,0,1,200,1,12,1,128,1,62,1,0,1,236,1,49,1,0,1,224,1,3,1,3,1,0,1,200,1,1,1,0,1,1,1,0,1,190,1,190,1,0,1,176,1,2,1,4,1,0,1,20,1,18,1,0,1,1,1,0,1,190,1,190,1,198,1,176,1,2,1,6,1,1,1,200,1,15,1,0,1,2,1,0,1,176,1,198,1,166,1,108,1,255,1,1,1,2,1,168,1,132,131,0,0,1,85,1,62,1,0,1,143,1,2,1,5,1,6,1,200,1,10,131,0,0,1,188,1,17,1,0,1,224,1,1,1,0,1,0,1,200,1,7,1,0,1,0,1,2,1,21,1,192,1,0,1,160,1,0,1,8,1,0,1,100,1,18,131,0,0,1,190,1,190,1,188,1,144,1,0,1,7,1,9,1,76,1,18,1,0,1,0,1,2,1,177,1,108,1,108,1,160,1,0,1,9,1,0,1,200,1,3,1,128,1,0,1,0,1,177,1,108,1,0,1,225,151,0,0,132,255,0,131,0,0,1,1,135,0,0,1,172,1,16,1,42,1,17,132,0,0,1,124,131,0,0,1,48,135,0,0,1,36,135,0,0,1,88,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,48,1,16,134,0,0,1,4,134,0,0,1,4,1,33,1,0,1,1,1,0,1,1,131,0,0,1,1,1,0,1,0,1,16,1,80,132,0,0,1,32,1,1,1,196,1,0,1,34,131,0,0,1,8,1,32,133,0,0,1,108,1,226,131,0,0,1,200,1,139,1,192,1,0,1,0,1,176,1,176,1,0,1,226,151,0,0,132,255,0,138,0,0,1,2,1,24,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,88,131,0,0,1,192,135,0,0,1,36,134,0,0,1,1,1,36,139,0,0,1,252,131,0,0,1,28,131,0,0,1,239,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,232,131,0,0,1,48,1,0,1,2,131,0,0,1,10,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,10,229,0,0,193,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,192,1,0,1,1,1,0,1,1,138,0,0,1,8,1,33,131,0,0,1,1,131,0,0,1,1,131,0,0,1,1,1,0,1,0,1,2,1,144,131,0,0,1,3,1,0,1,0,1,48,1,80,1,0,1,0,1,16,1,14,1,16,1,1,1,16,1,3,1,0,1,0,1,18,1,0,1,194,133,0,0,1,64,1,4,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,8,1,16,1,14,1,18,1,0,1,34,131,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,0,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,1,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,2,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,1,1,3,1,0,1,200,1,1,131,0,0,1,167,1,167,1,0,1,175,1,1,1,4,1,0,1,200,1,2,131,0,0,1,167,1,167,1,0,1,175,1,1,1,5,1,0,1,200,1,4,131,0,0,1,167,1,167,1,0,1,175,1,1,1,6,1,0,1,200,1,7,1,0,1,0,1,2,1,192,1,192,1,0,1,160,1,0,1,8,1,0,1,100,1,18,131,0,0,1,190,1,190,1,188,1,144,1,0,1,7,1,9,1,76,1,18,1,0,1,0,1,2,1,177,1,108,1,108,1,160,1,0,1,9,1,0,1,200,1,3,1,128,1,0,1,0,1,177,1,108,1,0,1,225,142,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {148,41,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,210,95,99,248,249,53,127,236,255,254,191,254,162,223,84,126,255,181,241,55,253,255,31,210,239,18,250,255,175,163,159,253,191,253,249,245,232,255,191,255,101,243,251,79,9,101,51,142,249,111,34,223,253,91,191,198,255,119,198,241,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,255,127,121,126,253,95,131,227,180,9,2,53,19,167,253,75,26,167,253,58,250,255,111,34,78,211,126,10,244,3,120,191,217,175,33,176,253,231,215,162,255,35,244,237,126,46,253,255,42,142,35,127,179,200,247,248,59,246,222,143,209,255,159,148,249,114,86,44,47,0,252,215,29,120,31,177,106,236,253,223,144,254,127,182,108,218,108,57,5,4,30,195,235,121,54,203,107,71,43,244,129,207,241,110,234,189,251,207,209,255,255,43,239,239,150,104,250,39,41,93,241,252,105,94,140,255,27,233,239,24,255,159,162,223,255,71,212,246,223,163,255,255,169,250,247,255,70,191,255,114,250,255,211,72,219,223,142,62,251,173,126,83,215,118,135,126,31,209,255,255,176,72,219,223,155,62,123,227,181,93,209,239,165,182,251,245,228,7,183,255,191,233,49,243,254,55,225,159,95,251,255,254,191,255,175,255,251,183,249,53,78,222,28,63,249,157,232,207,31,215,207,180,9,158,20,159,175,154,223,255,222,239,191,243,107,124,81,76,235,170,169,206,219,116,235,213,157,244,219,207,95,63,79,133,114,233,73,181,88,21,37,253,242,112,188,247,233,248,225,253,189,241,222,193,254,254,175,241,19,60,61,191,233,95,36,160,254,160,223,195,0,253,93,8,29,250,226,15,34,106,255,73,191,46,200,245,27,252,90,244,251,175,241,39,225,255,191,38,80,253,13,126,205,63,200,253,254,27,209,239,79,255,34,131,149,25,135,129,245,159,225,131,95,251,255,162,113,204,236,56,190,247,107,202,103,248,234,119,146,102,233,27,250,236,169,190,159,252,26,191,224,215,120,78,63,127,95,250,255,255,193,159,37,34,23,212,230,183,250,53,77,142,227,175,253,107,127,77,66,237,215,164,111,144,163,249,255,194,163,178,104,241,238,242,254,215,125,46,191,230,252,255,46,76,110,204,223,111,250,39,201,239,191,241,31,244,107,218,223,127,147,63,232,215,178,191,255,166,127,208,175,109,127,255,53,254,160,95,199,251,157,190,251,143,12,191,208,184,254,163,95,83,219,255,58,191,198,127,198,188,67,240,168,205,87,244,221,127,246,39,253,58,60,133,248,27,239,253,103,196,63,255,217,31,228,62,251,107,168,63,247,217,175,193,159,253,223,212,175,124,246,99,44,182,191,230,127,68,239,253,65,201,175,241,159,253,69,242,247,175,197,127,255,134,246,239,95,135,255,254,141,236,223,191,1,255,253,27,211,223,191,22,255,253,235,163,223,63,232,55,248,53,254,179,191,248,55,16,120,127,208,175,79,223,225,179,95,75,255,6,110,244,238,95,44,127,255,90,252,251,143,253,26,95,253,69,191,30,143,11,114,240,213,31,36,227,250,181,255,35,25,199,87,127,144,225,118,176,181,47,195,127,31,254,249,181,34,50,252,107,69,101,120,239,235,204,97,40,195,230,249,131,126,15,55,63,191,230,223,244,235,234,88,126,13,81,43,127,147,204,17,143,85,127,7,15,252,103,127,145,200,51,232,254,159,245,198,100,32,191,252,205,233,159,95,11,242,252,183,253,218,102,76,127,238,111,34,159,249,242,252,167,253,38,190,60,255,90,61,121,254,183,126,141,255,95,200,243,196,151,231,127,235,198,55,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,48,158,31,245,241,163,231,71,207,207,159,231,242,3,98,233,255,132,99,233,223,243,247,244,225,121,185,14,206,123,252,154,156,235,144,223,233,11,206,147,72,140,253,155,114,14,227,63,249,53,126,141,191,104,44,121,144,191,9,177,244,175,171,249,140,95,243,215,248,234,79,74,82,228,27,190,250,155,126,29,137,219,255,32,249,155,227,240,63,9,109,195,207,255,26,250,252,175,137,124,254,127,211,231,255,55,127,174,121,145,63,8,249,20,211,215,175,169,125,253,134,94,95,248,236,55,244,250,210,60,139,247,185,244,213,255,92,250,114,57,152,95,139,251,250,53,59,125,253,70,157,190,126,163,129,190,126,163,129,190,126,163,104,95,191,142,237,75,242,20,191,1,253,253,127,255,73,58,230,127,8,227,69,222,66,241,226,191,127,77,151,11,226,191,127,45,151,11,226,191,127,109,253,251,215,20,154,33,79,165,127,243,184,104,76,230,111,238,251,15,250,245,188,220,17,240,248,38,114,71,191,70,36,119,132,231,255,111,185,35,60,126,238,232,55,0,84,206,29,157,217,49,253,143,191,134,124,134,113,155,220,209,127,69,255,236,252,26,130,4,114,71,7,244,243,219,191,198,255,119,243,68,95,87,31,249,122,199,227,249,63,169,195,243,127,82,135,231,255,164,14,207,255,73,134,231,141,158,48,60,111,100,217,240,188,145,55,159,231,127,141,159,37,158,255,127,2,0,0,255,255};
			}
		}
#endif
	}
	/// <summary><para>Technique 'DepthOutRgTextureAlphaClip' generated from file 'Depth.fx'</para><para>Vertex Shader: approximately 14 instruction slots used, 10 registers</para><para>Pixel Shader: approximately 8 instruction slots used (1 texture, 7 arithmetic), 1 register</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	public sealed class DepthOutRgTextureAlphaClip : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DepthOutRgTextureAlphaClip' shader</summary>
		public DepthOutRgTextureAlphaClip()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.sc3 = -1;
			this.sc4 = -1;
			this.sc5 = -1;
			this.sc6 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(192));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DepthOutRgTextureAlphaClip.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DepthOutRgTextureAlphaClip.cid0 = state.GetNameUniqueID("clipThreshold");
			DepthOutRgTextureAlphaClip.sid0 = state.GetNameUniqueID("AlphaTextureSampler");
			DepthOutRgTextureAlphaClip.tid0 = state.GetNameUniqueID("AlphaTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DepthOutRgTextureAlphaClip.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.preg_change = (this.preg_change | ic);
			this.vbreg_change = (this.vbreg_change | ic);
			this.vireg_change = (this.vireg_change | ic);
			// Set the value for attribute 'cameraNearFar'
			this.vreg_change = (this.vreg_change | state.SetCameraNearFarVector2(ref this.vreg[9], ref this.sc0));
			// Set the value for attribute 'viewDirection'
			this.vreg_change = (this.vreg_change | state.SetViewDirectionVector3(ref this.vreg[7], ref this.sc1));
			// Set the value for attribute 'viewPoint'
			this.vreg_change = (this.vreg_change | state.SetViewPointVector3(ref this.vreg[8], ref this.sc2));
			Microsoft.Xna.Framework.Vector4 unused = new Microsoft.Xna.Framework.Vector4();
			// Set the value for attribute 'worldMatrix'
			this.vreg_change = (this.vreg_change | state.SetWorldMatrix(ref this.vreg[4], ref this.vreg[5], ref this.vreg[6], ref unused, ref this.sc3));
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc4));
			// Assign pixel shader textures and samplers
			if ((ic | this.ptc))
			{
				state.SetPixelShaderSamplers(this.ptx, this.pts);
				this.ptc = false;
			}
			if ((this.vreg_change == true))
			{
				DepthOutRgTextureAlphaClip.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((this.preg_change == true))
			{
				DepthOutRgTextureAlphaClip.fx.ps_c.SetValue(this.preg);
				this.preg_change = false;
				ic = true;
			}
			if ((ext == Xen.Graphics.ShaderSystem.ShaderExtension.Blending))
			{
				ic = (ic | state.SetBlendMatricesDirect(DepthOutRgTextureAlphaClip.fx.vsb_c, ref this.sc5));
			}
			if ((ext == Xen.Graphics.ShaderSystem.ShaderExtension.Instancing))
			{
				this.vireg_change = (this.vireg_change | state.SetViewProjectionMatrix(ref this.vireg[0], ref this.vireg[1], ref this.vireg[2], ref this.vireg[3], ref this.sc6));
				if ((this.vireg_change == true))
				{
					DepthOutRgTextureAlphaClip.fx.vsi_c.SetValue(this.vireg);
					this.vireg_change = false;
					ic = true;
				}
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DepthOutRgTextureAlphaClip.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DepthOutRgTextureAlphaClip.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DepthOutRgTextureAlphaClip.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DepthOutRgTextureAlphaClip.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DepthOutRgTextureAlphaClip.fx, DepthOutRgTextureAlphaClip.fxb, 16, 12);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DepthOutRgTextureAlphaClip.vin[i]));
			index = DepthOutRgTextureAlphaClip.vin[(i + 2)];
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
		/// <summary>Name ID for 'clipThreshold'</summary>
		private static int cid0;
		/// <summary>Assign the shader value 'float clipThreshold'</summary>
		public float ClipThreshold
		{
			set
			{
				this.preg[0] = new Microsoft.Xna.Framework.Vector4(value, 0F, 0F, 0F);
				this.preg_change = true;
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'cameraNearFar'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute 'viewDirection'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'viewPoint'</summary>
		private int sc2;
		/// <summary>Change ID for Semantic bound attribute 'worldMatrix'</summary>
		private int sc3;
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc4;
		/// <summary>Change ID for Semantic bound attribute '__BLENDMATRICES__GENMATRIX'</summary>
		private int sc5;
		/// <summary>Change ID for Semantic bound attribute 'viewProj'</summary>
		private int sc6;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D AlphaTextureSampler'</summary>
		public Xen.Graphics.TextureSamplerState AlphaTextureSampler
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
		/// <summary>Get/Set the Bound texture for 'Texture2D AlphaTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D AlphaTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D AlphaTextureSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D AlphaTexture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[10];
		/// <summary>Pixel shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] preg = new Microsoft.Xna.Framework.Vector4[1];
		/// <summary>Instancing shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vireg = new Microsoft.Xna.Framework.Vector4[4];
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
				return new byte[] {4,188,240,11,207,131,0,1,32,152,0,8,254,255,9,1,0,0,15,248,135,0,1,3,131,0,1,1,131,0,1,192,135,0,1,10,131,0,1,4,131,0,1,1,229,0,0,190,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,131,0,0,1,248,135,0,0,1,1,131,0,0,1,4,131,0,0,1,1,147,0,0,1,6,1,95,1,112,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,1,0,1,0,1,14,1,160,135,0,0,1,216,131,0,0,1,4,131,0,0,1,1,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,153,0,0,1,7,1,95,1,118,1,115,1,98,1,95,1,99,133,0,0,1,3,131,0,0,1,1,1,0,1,0,1,15,1,8,135,0,0,1,4,131,0,0,1,4,131,0,0,1,1,195,0,0,1,7,1,95,1,118,1,115,1,105,1,95,1,99,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,15,1,44,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,3,131,0,0,1,16,131,0,0,1,4,143,0,0,1,4,131,0,0,1,15,131,0,0,1,4,143,0,0,1,9,1,66,1,108,1,101,1,110,1,100,1,105,1,110,1,103,135,0,0,1,5,131,0,0,1,16,131,0,0,1,4,143,0,0,1,6,131,0,0,1,15,131,0,0,1,4,143,0,0,1,11,1,73,1,110,1,115,1,116,1,97,1,110,1,99,1,105,1,110,1,103,133,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,5,131,0,0,1,1,131,0,0,1,10,131,0,0,1,7,131,0,0,1,4,131,0,0,1,32,139,0,0,1,204,131,0,0,1,232,138,0,0,1,1,1,4,1,0,1,0,1,1,1,32,138,0,0,1,14,1,172,1,0,1,0,1,14,1,200,138,0,0,1,15,1,20,1,0,1,0,1,15,1,40,138,0,0,1,15,1,236,135,0,0,1,3,1,0,1,0,1,15,1,104,135,0,0,1,2,131,0,0,1,92,134,0,0,1,15,1,60,1,0,1,0,1,15,1,56,131,0,0,1,93,134,0,0,1,15,1,84,1,0,1,0,1,15,1,80,1,0,1,0,1,15,1,156,135,0,0,1,2,131,0,0,1,92,134,0,0,1,15,1,112,1,0,1,0,1,15,1,108,131,0,0,1,93,134,0,0,1,15,1,136,1,0,1,0,1,15,1,132,1,0,1,0,1,15,1,220,135,0,0,1,2,131,0,0,1,92,134,0,0,1,15,1,176,1,0,1,0,1,15,1,172,131,0,0,1,93,134,0,0,1,15,1,200,1,0,1,0,1,15,1,196,135,0,0,1,6,135,0,0,1,2,132,255,0,131,0,0,1,1,134,0,0,1,1,1,164,1,16,1,42,1,17,131,0,0,1,1,1,16,131,0,0,1,148,135,0,0,1,36,131,0,0,1,192,131,0,0,1,232,139,0,0,1,152,131,0,0,1,28,131,0,0,1,139,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,135,0,0,1,132,131,0,0,1,68,1,0,1,2,131,0,0,1,1,133,0,0,1,76,131,0,0,1,92,131,0,0,1,108,1,0,1,3,131,0,0,1,1,133,0,0,1,116,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,1,150,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,84,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,12,1,66,1,0,1,3,1,0,1,3,131,0,0,1,33,1,0,1,0,1,16,1,80,1,0,1,0,1,49,1,81,193,0,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,0,1,33,1,31,1,31,1,255,1,223,1,0,1,0,1,64,1,0,1,184,1,32,1,0,1,0,1,1,1,0,1,0,1,65,1,194,131,0,0,1,8,1,32,131,0,0,1,108,1,177,1,108,1,121,1,255,1,0,1,0,1,200,1,139,1,192,1,0,1,0,1,176,1,176,1,0,1,226,150,0,0,1,2,132,255,0,138,0,0,1,2,1,244,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,224,1,0,1,0,1,1,1,20,135,0,0,1,36,134,0,0,1,1,1,144,138,0,0,1,1,1,104,131,0,0,1,28,1,0,1,0,1,1,1,91,1,255,1,254,1,3,132,0,0,1,2,131,0,0,1,28,134,0,0,1,1,1,84,131,0,0,1,68,1,0,1,2,131,0,0,1,10,133,0,0,1,76,131,0,0,1,92,131,0,0,1,252,1,0,1,2,1,0,1,10,1,0,1,4,132,0,0,1,1,1,4,1,0,1,0,1,1,1,20,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,10,229,0,0,193,0,0,1,95,1,118,1,115,1,105,1,95,1,99,1,0,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,4,198,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,134,0,0,1,1,1,20,1,0,1,17,1,0,1,5,138,0,0,1,16,1,66,131,0,0,1,1,131,0,0,1,6,131,0,0,1,2,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,0,1,80,1,4,1,0,1,12,1,0,1,5,1,0,1,13,1,0,1,6,1,0,1,14,1,0,1,7,1,0,1,63,1,0,1,8,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,0,1,16,1,21,1,0,1,0,1,16,1,18,1,245,1,85,1,96,1,3,1,0,1,0,1,18,1,3,1,194,133,0,0,1,96,1,9,1,32,1,15,1,18,1,0,1,18,135,0,0,1,80,1,17,1,196,1,0,1,34,131,0,0,1,5,1,248,1,48,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,15,1,200,132,0,0,1,5,1,248,1,32,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,64,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,80,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,6,1,136,132,0,0,1,200,1,15,131,0,0,1,27,1,0,1,0,1,225,1,3,1,0,1,0,1,200,1,15,131,0,0,1,198,1,0,1,0,1,235,1,3,1,5,1,0,1,200,1,15,131,0,0,1,177,1,148,1,148,1,235,1,3,1,4,1,0,1,200,1,15,131,0,0,1,108,1,248,1,148,1,235,1,3,1,2,1,0,1,200,1,1,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,10,1,0,1,200,1,2,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,11,1,0,1,200,1,4,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,12,1,0,1,200,1,8,1,128,1,62,1,0,1,233,1,167,1,0,1,175,1,0,1,13,1,0,1,200,1,7,1,0,1,0,1,2,1,20,1,192,1,0,1,160,1,0,1,8,1,0,1,200,1,3,1,128,1,1,1,0,1,176,1,176,1,0,1,226,1,1,1,1,1,0,1,100,1,18,131,0,0,1,190,1,190,1,188,1,144,1,0,1,7,1,9,1,76,1,18,1,0,1,0,1,2,1,177,1,108,1,108,1,160,1,0,1,9,1,0,1,200,1,3,1,128,1,0,1,0,1,177,1,108,1,0,1,225,150,0,0,1,1,132,255,0,131,0,0,1,1,134,0,0,1,1,1,164,1,16,1,42,1,17,131,0,0,1,1,1,16,131,0,0,1,148,135,0,0,1,36,131,0,0,1,192,131,0,0,1,232,139,0,0,1,152,131,0,0,1,28,131,0,0,1,139,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,135,0,0,1,132,131,0,0,1,68,1,0,1,2,131,0,0,1,1,133,0,0,1,76,131,0,0,1,92,131,0,0,1,108,1,0,1,3,131,0,0,1,1,133,0,0,1,116,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,1,150,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,84,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,12,1,66,1,0,1,3,1,0,1,3,131,0,0,1,33,1,0,1,0,1,16,1,80,1,0,1,0,1,49,1,81,193,0,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,0,1,33,1,31,1,31,1,255,1,223,1,0,1,0,1,64,1,0,1,184,1,32,1,0,1,0,1,1,1,0,1,0,1,65,1,194,131,0,0,1,8,1,32,131,0,0,1,108,1,177,1,108,1,121,1,255,1,0,1,0,1,200,1,139,1,192,1,0,1,0,1,176,1,176,1,0,1,226,150,0,0,1,1,132,255,0,138,0,0,1,17,1,168,1,16,1,42,1,17,1,1,1,0,1,0,1,15,1,64,1,0,1,0,1,2,1,104,135,0,0,1,36,1,0,1,0,1,14,1,208,1,0,1,0,1,14,1,248,138,0,0,1,14,1,168,131,0,0,1,28,1,0,1,0,1,14,1,155,1,255,1,254,1,3,132,0,0,1,2,131,0,0,1,28,134,0,0,1,14,1,148,131,0,0,1,68,1,0,1,2,131,0,0,1,10,133,0,0,1,76,131,0,0,1,92,131,0,0,1,252,1,0,1,2,1,0,1,10,1,0,1,216,132,0,0,1,1,1,4,1,0,1,0,1,1,1,20,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,10,229,0,0,193,0,0,1,95,1,118,1,115,1,98,1,95,1,99,1,0,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,216,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,156,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,2,1,40,1,0,1,17,1,0,1,7,138,0,0,1,16,1,66,131,0,0,1,1,131,0,0,1,4,131,0,0,1,2,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,5,1,0,1,0,1,80,1,6,1,0,1,0,1,16,1,7,1,0,1,48,1,32,1,8,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,0,1,16,1,44,1,0,1,0,1,16,1,40,180,0,0,1,63,1,128,1,0,1,0,1,64,1,64,134,0,0,1,240,1,85,1,64,1,5,1,0,1,0,1,18,1,0,1,194,133,0,0,1,96,1,9,1,96,1,15,1,18,1,0,1,18,133,0,0,1,96,1,21,1,96,1,27,1,18,1,0,1,18,133,0,0,1,16,1,33,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,34,1,80,1,40,1,18,1,0,1,34,131,0,0,1,5,1,248,1,32,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,15,1,200,132,0,0,1,5,1,248,1,80,131,0,0,1,2,1,208,132,0,0,1,5,1,248,132,0,0,1,6,1,136,132,0,0,1,200,1,15,1,0,1,7,1,0,1,0,1,198,1,0,1,161,1,0,1,255,1,0,1,92,1,8,1,0,1,4,1,0,131,27,0,1,161,1,2,1,3,1,7,1,200,1,15,1,0,1,3,1,160,1,198,1,136,1,0,1,161,1,5,1,10,1,0,1,200,1,15,1,0,1,6,1,160,1,198,1,136,1,0,1,161,1,5,1,11,1,0,1,92,1,15,1,0,1,0,1,160,1,198,1,136,1,198,1,161,1,5,1,12,1,7,1,200,1,15,1,0,1,0,1,160,1,177,1,136,1,0,1,171,1,5,1,12,1,0,1,200,1,15,1,0,1,6,1,160,1,177,1,136,1,0,1,171,1,5,1,11,1,6,1,200,1,15,1,0,1,3,1,160,1,177,1,136,1,0,1,171,1,5,1,10,1,3,1,92,1,2,1,0,1,5,1,0,1,27,1,27,1,177,1,161,1,2,1,1,1,7,1,200,1,15,1,0,1,3,1,160,1,27,1,52,1,148,1,171,1,5,1,10,1,3,1,200,1,15,1,0,1,6,1,160,1,27,1,52,1,148,1,171,1,5,1,11,1,6,1,200,1,15,1,0,1,0,1,160,1,27,1,52,1,148,1,171,1,5,1,12,1,0,1,92,1,8,1,0,1,5,1,0,1,27,1,27,1,108,1,161,1,2,1,0,1,7,1,200,1,15,1,0,1,0,1,160,1,108,1,208,1,148,1,171,1,5,1,12,1,0,1,200,1,15,1,0,1,6,1,160,1,108,1,208,1,148,1,171,1,5,1,11,1,6,1,200,1,15,1,0,1,3,1,160,1,108,1,208,1,148,1,171,1,5,1,10,1,3,1,200,1,1,1,0,1,3,1,0,1,170,1,167,1,0,1,239,1,3,1,2,1,0,1,200,1,2,1,0,1,3,1,0,1,170,1,167,1,0,1,239,1,6,1,2,1,0,1,200,1,4,1,0,1,3,1,0,1,170,1,167,1,0,1,239,1,0,1,2,1,0,1,200,1,1,1,0,1,4,1,0,1,190,1,190,1,0,1,176,1,3,1,2,1,0,1,200,1,4,1,0,1,4,1,0,1,190,1,190,1,0,1,176,1,3,1,3,1,0,1,20,1,17,1,0,1,5,1,0,1,190,1,190,1,27,1,176,1,3,1,1,1,2,1,168,1,36,1,4,1,5,1,0,1,190,1,190,1,0,1,144,1,3,1,0,1,2,1,200,1,3,1,128,1,62,1,0,1,110,1,179,1,0,1,224,1,5,1,5,1,0,1,200,1,12,1,128,1,62,1,0,1,236,1,49,1,0,1,224,1,4,1,4,1,0,1,200,1,2,131,0,0,1,27,1,27,1,0,1,161,1,2,1,4,1,0,1,200,1,4,1,0,1,1,1,0,1,190,1,190,1,0,1,176,1,3,1,4,1,0,1,20,1,24,1,0,1,1,1,0,1,190,1,190,1,27,1,176,1,3,1,6,1,2,1,200,1,15,1,0,1,2,1,0,1,176,1,27,1,166,1,108,1,255,1,2,1,3,1,168,1,132,131,0,0,1,85,1,62,1,0,1,143,1,2,1,5,1,6,1,200,1,10,131,0,0,1,22,1,17,1,0,1,224,1,1,1,0,1,0,1,200,1,3,1,128,1,1,1,0,1,176,1,176,1,0,1,226,1,1,1,1,1,0,1,200,1,7,1,0,1,0,1,2,1,21,1,192,1,0,1,160,1,0,1,8,1,0,1,100,1,18,131,0,0,1,190,1,190,1,188,1,144,1,0,1,7,1,9,1,76,1,18,1,0,1,0,1,2,1,177,1,108,1,108,1,160,1,0,1,9,1,0,1,200,1,3,1,128,1,0,1,0,1,177,1,108,1,0,1,225,151,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,164,1,16,1,42,1,17,131,0,0,1,1,1,16,131,0,0,1,148,135,0,0,1,36,131,0,0,1,192,131,0,0,1,232,139,0,0,1,152,131,0,0,1,28,131,0,0,1,139,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,135,0,0,1,132,131,0,0,1,68,1,0,1,2,131,0,0,1,1,133,0,0,1,76,131,0,0,1,92,131,0,0,1,108,1,0,1,3,131,0,0,1,1,133,0,0,1,116,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,1,150,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,84,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,12,1,66,1,0,1,3,1,0,1,3,131,0,0,1,33,1,0,1,0,1,16,1,80,1,0,1,0,1,49,1,81,193,0,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,0,1,33,1,31,1,31,1,255,1,223,1,0,1,0,1,64,1,0,1,184,1,32,1,0,1,0,1,1,1,0,1,0,1,65,1,194,131,0,0,1,8,1,32,131,0,0,1,108,1,177,1,108,1,121,1,255,1,0,1,0,1,200,1,139,1,192,1,0,1,0,1,176,1,176,1,0,1,226,151,0,0,132,255,0,138,0,0,1,2,1,60,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,100,131,0,0,1,216,135,0,0,1,36,134,0,0,1,1,1,36,139,0,0,1,252,131,0,0,1,28,131,0,0,1,239,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,232,131,0,0,1,48,1,0,1,2,131,0,0,1,10,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,10,229,0,0,193,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,216,1,0,1,17,1,0,1,2,138,0,0,1,16,1,66,131,0,0,1,1,131,0,0,1,2,131,0,0,1,2,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,32,1,80,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,0,1,16,1,16,1,0,1,0,1,16,1,12,1,48,1,5,1,32,1,3,1,0,1,0,1,18,1,0,1,194,133,0,0,1,64,1,5,1,0,1,0,1,18,1,0,1,196,133,0,0,1,96,1,9,1,32,1,15,1,18,1,0,1,34,131,0,0,1,5,1,248,1,32,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,15,1,200,132,0,0,1,200,1,1,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,2,1,0,1,0,1,200,1,2,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,2,1,1,1,0,1,200,1,4,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,2,1,2,1,0,1,200,1,8,1,128,1,62,1,0,1,167,1,167,1,0,1,175,1,2,1,3,1,0,1,200,1,1,131,0,0,1,167,1,167,1,0,1,175,1,2,1,4,1,0,1,200,1,2,131,0,0,1,167,1,167,1,0,1,175,1,2,1,5,1,0,1,200,1,4,131,0,0,1,167,1,167,1,0,1,175,1,2,1,6,1,0,1,200,1,3,1,128,1,1,1,0,1,176,1,176,1,0,1,226,1,1,1,1,1,0,1,200,1,7,1,0,1,0,1,2,1,192,1,192,1,0,1,160,1,0,1,8,1,0,1,100,1,18,131,0,0,1,190,1,190,1,188,1,144,1,0,1,7,1,9,1,76,1,18,1,0,1,0,1,2,1,177,1,108,1,108,1,160,1,0,1,9,1,0,1,200,1,3,1,128,1,0,1,0,1,177,1,108,1,0,1,225,142,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {92,44,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,210,95,99,248,249,53,127,236,255,254,191,254,143,223,84,126,255,181,241,55,253,255,31,210,239,18,250,255,175,163,159,253,191,253,249,245,232,255,191,255,101,243,251,79,9,101,51,142,255,77,191,195,239,67,227,224,247,86,252,158,29,255,95,244,155,200,119,255,214,134,247,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,207,206,243,235,255,26,28,223,77,16,168,153,56,237,55,208,184,245,215,209,255,127,19,113,154,246,83,160,159,223,232,215,16,184,35,237,39,104,67,49,99,179,35,125,254,102,218,206,127,126,45,250,63,94,235,126,46,56,254,223,255,55,198,240,155,69,190,199,223,177,247,126,140,254,255,164,204,151,179,98,121,129,191,127,221,95,35,254,62,226,217,216,251,191,33,253,255,108,217,180,217,114,202,16,48,134,215,243,108,150,215,191,198,24,176,128,23,226,125,124,142,119,83,239,221,127,142,254,255,95,121,127,255,58,212,56,245,136,253,215,81,188,252,79,105,204,140,231,183,34,4,182,60,154,253,79,94,126,97,174,191,131,62,127,138,126,255,152,62,59,160,255,255,169,250,247,27,250,253,37,253,255,47,138,180,109,233,179,149,215,246,143,163,223,255,40,250,255,127,20,105,251,183,209,103,127,147,215,246,159,163,223,255,41,109,247,235,201,15,110,255,127,211,99,134,243,123,227,151,95,251,255,254,191,255,175,255,123,231,215,56,121,115,252,228,119,162,63,255,216,95,67,62,67,219,223,137,91,253,26,233,31,70,255,60,213,247,127,77,250,247,57,253,252,125,233,255,229,175,97,248,243,215,250,53,90,133,41,249,133,191,246,175,253,53,233,155,95,147,168,59,196,167,202,83,127,237,175,67,156,247,107,242,127,242,208,199,247,126,255,157,95,227,139,98,90,87,77,117,222,166,91,175,238,164,223,126,254,250,121,42,51,152,158,84,139,85,81,210,47,15,199,123,159,142,31,222,223,27,239,29,236,239,255,26,63,65,108,242,107,254,26,191,233,95,36,80,254,160,223,195,244,243,187,16,218,52,231,127,16,97,249,39,201,239,191,230,31,68,184,241,239,244,245,159,68,130,245,23,61,225,97,252,166,244,249,127,70,127,255,103,127,209,175,165,127,19,181,232,255,191,198,95,124,204,44,243,155,254,65,191,46,62,255,13,126,173,63,136,223,163,255,255,154,128,241,27,252,154,127,144,251,253,55,34,24,79,255,162,255,251,255,150,190,13,189,13,46,191,1,136,245,107,255,95,68,239,153,165,247,247,126,77,249,204,167,247,155,95,211,209,59,249,53,126,129,165,247,255,193,159,37,162,3,168,205,111,245,107,154,60,144,163,55,248,250,255,11,143,234,29,139,119,87,134,191,238,115,249,53,249,71,248,225,55,250,131,104,158,149,55,126,99,154,75,243,251,111,242,7,253,90,246,247,223,244,15,250,181,237,239,191,198,31,244,235,232,239,224,177,95,215,251,156,218,253,71,30,191,233,239,191,6,193,249,181,255,163,95,83,225,252,58,196,111,224,169,95,147,121,237,43,230,191,95,135,167,22,127,3,198,127,70,124,245,159,253,65,238,179,191,134,222,119,159,25,30,253,181,245,179,31,99,190,253,53,255,35,122,239,15,74,126,141,255,236,47,146,191,127,45,254,251,55,180,127,255,58,252,247,111,100,255,254,13,248,239,223,248,215,48,124,255,235,163,223,63,232,55,248,53,254,179,191,248,55,16,120,127,208,175,79,223,225,179,95,75,255,6,110,244,238,95,44,127,255,90,252,251,143,253,26,95,253,69,191,30,143,11,242,241,21,203,10,198,42,227,192,216,126,141,95,11,116,248,117,9,87,35,17,96,125,95,31,205,240,203,175,21,209,71,191,214,207,169,62,218,251,58,252,20,234,35,243,252,65,191,135,227,143,95,243,111,50,191,19,110,230,247,174,62,250,155,110,212,71,76,239,95,227,111,194,255,133,175,120,126,244,119,232,163,255,236,47,18,221,132,247,255,179,63,168,75,123,131,217,252,55,167,127,126,45,232,166,191,237,215,54,180,255,115,127,19,249,204,167,253,159,246,155,248,186,201,209,222,232,166,127,235,215,248,255,133,110,154,248,186,233,223,186,241,141,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,207,213,243,163,126,127,244,252,232,249,209,243,163,231,255,159,207,229,7,196,226,255,9,199,226,191,231,239,233,195,243,114,53,54,135,99,114,62,191,38,231,106,228,119,106,196,57,159,95,87,227,112,228,91,254,19,74,217,142,37,143,243,55,33,174,254,117,53,31,243,107,253,26,95,253,73,73,138,124,201,87,127,211,175,195,169,235,223,244,15,146,191,127,141,191,233,215,226,188,33,231,110,188,207,255,26,250,252,175,137,124,254,127,211,231,255,55,127,174,121,157,63,8,249,32,211,215,175,169,125,253,134,94,95,248,236,55,244,250,210,60,145,247,185,244,213,255,92,250,114,57,164,95,139,251,250,53,59,125,253,70,157,190,126,163,129,190,126,163,129,190,126,163,104,95,191,142,237,75,114,22,191,1,253,253,127,255,73,58,230,127,8,227,253,53,92,46,139,255,254,53,237,223,191,14,255,253,107,185,92,22,255,253,107,235,223,191,166,208,12,121,54,253,155,199,69,99,50,127,115,223,127,208,175,231,229,190,128,199,55,145,251,250,53,34,185,175,95,211,203,125,225,249,81,238,235,231,38,247,133,199,207,125,165,232,157,115,95,103,150,246,255,227,175,33,159,129,22,134,246,255,21,253,179,243,107,8,178,200,125,29,208,207,111,255,26,255,223,205,115,125,93,125,58,164,55,61,153,253,147,58,50,251,39,117,100,246,79,234,200,236,159,100,100,214,232,57,35,179,70,23,25,153,53,250,194,151,217,95,227,135,32,179,255,79,0,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Single' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, float value)
		{
			if ((DepthOutRgTextureAlphaClip.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DepthOutRgTextureAlphaClip.cid0))
			{
				this.ClipThreshold = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((DepthOutRgTextureAlphaClip.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DepthOutRgTextureAlphaClip.sid0))
			{
				this.AlphaTextureSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((DepthOutRgTextureAlphaClip.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DepthOutRgTextureAlphaClip.tid0))
			{
				this.AlphaTexture = value;
				return true;
			}
			return false;
		}
	}
	/// <summary><para>Technique 'NonLinearDepthOut' generated from file 'Depth.fx'</para><para>Vertex Shader: approximately 6 instruction slots used, 4 registers</para><para>Pixel Shader: approximately 4 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	public sealed class NonLinearDepthOut : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'NonLinearDepthOut' shader</summary>
		public NonLinearDepthOut()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			NonLinearDepthOut.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != NonLinearDepthOut.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.vbreg_change = (this.vbreg_change | ic);
			this.vireg_change = (this.vireg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[0], ref this.vreg[1], ref this.vreg[2], ref this.vreg[3], ref this.sc0));
			if ((this.vreg_change == true))
			{
				NonLinearDepthOut.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((ext == Xen.Graphics.ShaderSystem.ShaderExtension.Blending))
			{
				ic = (ic | state.SetBlendMatricesDirect(NonLinearDepthOut.fx.vsb_c, ref this.sc1));
			}
			if ((ext == Xen.Graphics.ShaderSystem.ShaderExtension.Instancing))
			{
				this.vireg_change = (this.vireg_change | state.SetViewProjectionMatrix(ref this.vireg[0], ref this.vireg[1], ref this.vireg[2], ref this.vireg[3], ref this.sc2));
				if ((this.vireg_change == true))
				{
					NonLinearDepthOut.fx.vsi_c.SetValue(this.vireg);
					this.vireg_change = false;
					ic = true;
				}
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref NonLinearDepthOut.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((NonLinearDepthOut.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((NonLinearDepthOut.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			NonLinearDepthOut.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out NonLinearDepthOut.fx, NonLinearDepthOut.fxb, 7, 6);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(NonLinearDepthOut.vin[i]));
			index = NonLinearDepthOut.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
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
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute '__BLENDMATRICES__GENMATRIX'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'viewProj'</summary>
		private int sc2;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[4];
		/// <summary>Instancing shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vireg = new Microsoft.Xna.Framework.Vector4[4];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,32,152,0,8,254,255,9,1,0,0,15,60,135,0,1,3,131,0,1,1,131,0,1,96,135,0,1,4,131,0,1,4,131,0,1,1,195,0,6,6,95,118,115,95,99,134,0,1,3,131,0,5,1,0,0,14,8,135,0,1,216,131,0,1,4,131,0,1,1,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,153,0,0,1,7,1,95,1,118,1,115,1,98,1,95,1,99,133,0,0,1,3,131,0,0,1,1,1,0,1,0,1,14,1,112,135,0,0,1,4,131,0,0,1,4,131,0,0,1,1,195,0,0,1,7,1,95,1,118,1,115,1,105,1,95,1,99,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,3,131,0,0,1,16,131,0,0,1,4,143,0,0,1,4,131,0,0,1,15,131,0,0,1,4,143,0,0,1,9,1,66,1,108,1,101,1,110,1,100,1,105,1,110,1,103,135,0,0,1,5,131,0,0,1,16,131,0,0,1,4,143,0,0,1,6,131,0,0,1,15,131,0,0,1,4,143,0,0,1,11,1,73,1,110,1,115,1,116,1,97,1,110,1,99,1,105,1,110,1,103,133,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,3,131,0,0,1,1,131,0,0,1,9,131,0,0,1,7,131,0,0,1,4,131,0,0,1,32,139,0,0,1,108,131,0,0,1,136,138,0,0,1,14,1,20,1,0,1,0,1,14,1,48,138,0,0,1,15,1,48,135,0,0,1,3,1,0,1,0,1,14,1,172,135,0,0,1,2,131,0,0,1,92,134,0,0,1,14,1,128,1,0,1,0,1,14,1,124,131,0,0,1,93,134,0,0,1,14,1,152,1,0,1,0,1,14,1,148,1,0,1,0,1,14,1,224,135,0,0,1,2,131,0,0,1,92,134,0,0,1,14,1,180,1,0,1,0,1,14,1,176,131,0,0,1,93,134,0,0,1,14,1,204,1,0,1,0,1,14,1,200,1,0,1,0,1,15,1,32,135,0,0,1,2,131,0,0,1,92,134,0,0,1,14,1,244,1,0,1,0,1,14,1,240,131,0,0,1,93,134,0,0,1,15,1,12,1,0,1,0,1,15,1,8,135,0,0,1,6,135,0,0,1,2,132,255,0,131,0,0,1,1,135,0,0,1,172,1,16,1,42,1,17,132,0,0,1,124,131,0,0,1,48,135,0,0,1,36,135,0,0,1,88,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,48,1,16,134,0,0,1,4,134,0,0,1,8,1,33,1,0,1,1,1,0,1,1,131,0,0,1,1,1,0,1,0,1,48,1,80,132,0,0,1,32,1,1,1,196,1,0,1,34,131,0,0,1,76,1,64,133,0,0,1,177,1,226,131,0,0,1,200,1,143,1,192,1,0,1,0,1,198,1,108,1,0,1,225,150,0,0,1,2,132,255,0,138,0,0,1,1,1,236,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,8,131,0,0,1,228,135,0,0,1,36,135,0,0,1,196,139,0,0,1,156,131,0,0,1,28,131,0,0,1,143,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,136,131,0,0,1,48,1,0,1,2,1,0,1,4,1,0,1,4,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,105,1,95,1,99,1,0,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,4,198,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,228,1,0,1,1,1,0,1,4,138,0,0,1,8,1,33,131,0,0,1,1,131,0,0,1,5,131,0,0,1,1,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,12,1,0,1,4,1,0,1,13,1,0,1,5,1,0,1,14,1,0,1,6,1,0,1,63,1,0,1,7,1,0,1,0,1,48,1,80,1,0,1,0,1,16,1,17,1,241,1,85,1,80,1,3,1,0,1,0,1,18,1,1,1,194,133,0,0,1,96,1,8,1,48,1,14,1,18,1,0,1,18,135,0,0,1,16,1,17,1,196,1,0,1,34,131,0,0,1,5,1,248,1,32,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,48,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,64,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,6,1,136,132,0,0,1,200,1,15,131,0,0,1,27,1,0,1,0,1,225,1,2,1,0,1,0,1,200,1,15,131,0,0,1,198,1,0,1,0,1,235,1,2,1,4,1,0,1,200,1,15,131,0,0,1,177,1,148,1,148,1,235,1,2,1,3,1,0,1,200,1,15,1,0,1,1,1,0,1,108,1,248,1,148,1,235,1,2,1,1,1,0,1,200,1,8,131,0,0,1,233,1,167,1,0,1,175,1,1,1,7,1,0,1,200,1,4,131,0,0,1,233,1,167,1,0,1,175,1,1,1,6,1,0,1,200,1,2,131,0,0,1,233,1,167,1,0,1,175,1,1,1,5,1,0,1,200,1,1,131,0,0,1,233,1,167,1,0,1,175,1,1,1,4,1,0,1,200,1,15,1,128,1,62,132,0,0,1,226,131,0,0,1,200,1,3,1,128,1,0,1,0,1,26,1,26,1,0,1,226,150,0,0,1,1,132,255,0,131,0,0,1,1,135,0,0,1,172,1,16,1,42,1,17,132,0,0,1,124,131,0,0,1,48,135,0,0,1,36,135,0,0,1,88,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,48,1,16,134,0,0,1,4,134,0,0,1,8,1,33,1,0,1,1,1,0,1,1,131,0,0,1,1,1,0,1,0,1,48,1,80,132,0,0,1,32,1,1,1,196,1,0,1,34,131,0,0,1,76,1,64,133,0,0,1,177,1,226,131,0,0,1,200,1,143,1,192,1,0,1,0,1,198,1,108,1,0,1,225,150,0,0,1,1,132,255,0,138,0,0,1,16,1,196,1,16,1,42,1,17,1,1,1,0,1,0,1,14,1,212,1,0,1,0,1,1,1,240,135,0,0,1,36,1,0,1,0,1,14,1,112,1,0,1,0,1,14,1,152,138,0,0,1,14,1,72,131,0,0,1,28,1,0,1,0,1,14,1,59,1,255,1,254,1,3,132,0,0,1,2,131,0,0,1,28,134,0,0,1,14,1,52,131,0,0,1,68,1,0,1,2,131,0,0,1,4,133,0,0,1,76,131,0,0,1,92,131,0,0,1,156,1,0,1,2,1,0,1,4,1,0,1,216,133,0,0,1,164,131,0,0,1,180,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,4,198,0,0,1,95,1,118,1,115,1,98,1,95,1,99,1,0,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,216,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,156,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,176,1,0,1,1,1,0,1,5,138,0,0,1,8,1,33,131,0,0,1,1,131,0,0,1,3,131,0,0,1,1,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,5,1,0,1,0,1,16,1,6,1,0,1,48,1,32,1,7,1,0,1,0,1,48,1,80,1,0,1,0,1,16,1,34,180,0,0,1,63,1,128,1,0,1,0,1,64,1,64,134,0,0,1,112,1,21,1,48,1,5,1,0,1,0,1,18,1,0,1,194,133,0,0,1,96,1,8,1,96,1,14,1,18,1,0,1,18,133,0,0,1,96,1,20,1,96,1,26,1,18,1,0,1,18,133,0,0,1,32,1,32,1,0,1,0,1,18,1,0,1,196,133,0,0,1,16,1,34,1,0,1,0,1,34,133,0,0,1,5,1,248,1,32,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,2,1,208,132,0,0,1,5,1,248,132,0,0,1,6,1,136,132,0,0,1,200,1,15,1,0,1,5,1,0,1,0,1,198,1,0,1,161,1,0,1,255,1,0,1,92,134,0,0,1,27,1,226,1,0,1,0,1,5,1,200,1,15,1,0,1,0,1,160,1,198,1,136,1,0,1,161,1,1,1,4,1,0,1,200,1,15,1,0,1,4,1,160,1,198,1,136,1,0,1,161,1,1,1,5,1,0,1,92,1,15,1,0,1,3,1,160,1,198,1,136,1,198,1,161,1,1,1,6,1,5,1,200,1,15,1,0,1,3,1,160,1,177,1,136,1,0,1,171,1,1,1,6,1,3,1,200,1,15,1,0,1,4,1,160,1,177,1,136,1,0,1,171,1,1,1,5,1,4,1,200,1,15,1,0,1,0,1,160,1,177,1,136,1,0,1,171,1,1,1,4,1,0,1,92,1,2,1,0,1,1,1,0,1,27,1,27,1,177,1,161,1,2,1,0,1,5,1,200,1,15,1,0,1,0,1,160,1,27,1,52,1,148,1,171,1,1,1,4,1,0,1,200,1,15,1,0,1,4,1,160,1,27,1,52,1,148,1,171,1,1,1,5,1,4,1,200,1,15,1,0,1,3,1,160,1,27,1,52,1,148,1,171,1,1,1,6,1,3,1,92,1,8,1,0,1,1,1,0,1,27,1,27,1,108,1,161,1,2,1,1,1,5,1,200,1,15,1,0,1,3,1,160,1,108,1,208,1,148,1,171,1,1,1,6,1,3,1,200,1,15,1,0,1,4,1,160,1,108,1,208,1,148,1,171,1,1,1,5,1,4,1,200,1,15,1,0,1,0,1,160,1,108,1,208,1,148,1,171,1,1,1,4,1,0,1,200,1,1,131,0,0,1,170,1,167,1,0,1,239,1,0,1,2,1,0,1,200,1,2,131,0,0,1,170,1,167,1,0,1,239,1,4,1,2,1,0,1,200,1,4,131,0,0,1,170,1,167,1,0,1,239,1,3,1,2,1,0,1,200,1,1,1,0,1,1,1,0,1,190,1,190,1,0,1,176,131,0,0,1,200,1,4,1,0,1,1,1,0,1,190,1,190,1,0,1,176,1,0,1,1,1,0,1,200,1,15,1,0,1,2,1,0,1,176,1,27,1,166,1,108,1,255,1,2,1,0,1,200,1,8,131,0,0,1,85,1,62,1,0,1,175,1,2,1,3,1,0,1,200,1,4,131,0,0,1,85,1,62,1,0,1,175,1,2,1,2,1,0,1,200,1,3,131,0,0,1,196,1,25,1,0,1,224,1,1,1,1,1,0,1,200,1,15,1,128,1,62,132,0,0,1,226,131,0,0,1,200,1,3,1,128,1,0,1,0,1,26,1,26,1,0,1,226,151,0,0,132,255,0,131,0,0,1,1,135,0,0,1,172,1,16,1,42,1,17,132,0,0,1,124,131,0,0,1,48,135,0,0,1,36,135,0,0,1,88,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,48,1,16,134,0,0,1,4,134,0,0,1,8,1,33,1,0,1,1,1,0,1,1,131,0,0,1,1,1,0,1,0,1,48,1,80,132,0,0,1,32,1,1,1,196,1,0,1,34,131,0,0,1,76,1,64,133,0,0,1,177,1,226,131,0,0,1,200,1,143,1,192,1,0,1,0,1,198,1,108,1,0,1,225,151,0,0,132,255,0,138,0,0,1,1,1,124,1,16,1,42,1,17,1,1,131,0,0,1,248,131,0,0,1,132,135,0,0,1,36,135,0,0,1,196,139,0,0,1,156,131,0,0,1,28,131,0,0,1,143,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,136,131,0,0,1,48,1,0,1,2,131,0,0,1,4,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,4,198,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,132,1,0,1,1,1,0,1,1,138,0,0,1,8,1,33,131,0,0,1,1,131,0,0,1,1,131,0,0,1,1,1,0,1,0,1,2,1,144,131,0,0,1,3,1,0,1,0,1,48,1,80,1,0,1,0,1,16,1,9,1,16,1,1,1,16,1,3,1,0,1,0,1,18,1,0,1,194,133,0,0,1,80,1,4,1,0,1,0,1,18,1,0,1,196,133,0,0,1,16,1,9,1,0,1,0,1,34,133,0,0,1,5,1,248,1,16,131,0,0,1,6,1,136,132,0,0,1,200,1,8,131,0,0,1,167,1,167,1,0,1,175,1,1,1,3,1,0,1,200,1,4,131,0,0,1,167,1,167,1,0,1,175,1,1,1,2,1,0,1,200,1,2,131,0,0,1,167,1,167,1,0,1,175,1,1,1,1,1,0,1,200,1,1,131,0,0,1,167,1,167,1,0,1,175,1,1,1,0,1,0,1,200,1,15,1,128,1,62,132,0,0,1,226,131,0,0,1,200,1,3,1,128,1,0,1,0,1,26,1,26,1,0,1,226,142,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {124,38,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,210,95,99,248,249,53,127,236,255,254,191,126,207,223,84,126,255,181,241,55,253,255,15,208,239,126,29,253,255,175,169,127,127,200,243,235,209,255,127,255,203,230,247,159,254,26,174,159,223,224,55,145,239,254,173,95,227,155,235,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,220,238,249,245,127,13,142,211,38,8,212,76,156,182,210,56,237,155,140,7,181,159,2,253,0,222,111,246,107,8,108,255,249,181,232,255,8,77,187,159,155,254,129,223,111,22,249,30,127,199,222,251,49,250,255,147,50,95,206,138,229,5,254,254,117,127,141,248,251,136,85,99,239,255,134,244,255,179,101,211,102,203,41,67,192,24,94,207,179,89,94,255,26,191,142,161,21,250,192,231,120,55,245,222,45,233,255,127,148,247,247,111,69,52,221,81,186,226,217,247,98,240,191,78,63,199,248,255,20,253,254,15,162,207,126,134,254,255,167,234,223,127,22,253,254,167,209,255,255,179,72,219,191,139,62,251,219,188,182,255,18,253,254,207,209,255,127,55,237,195,111,251,191,209,231,191,220,107,251,155,81,155,223,72,219,253,122,242,131,219,255,223,244,24,186,255,77,248,231,215,254,191,255,239,255,235,255,254,109,126,141,147,55,199,79,126,39,250,243,199,245,51,109,130,39,197,231,171,230,247,191,247,251,239,252,26,95,20,211,186,106,170,243,54,221,122,117,39,253,246,243,215,207,83,161,92,122,82,45,86,69,73,191,60,28,239,125,58,126,120,127,111,188,119,176,191,255,107,252,4,79,207,111,250,23,209,200,127,15,15,230,175,241,187,16,58,244,197,31,68,157,253,73,191,30,163,246,107,210,239,95,253,73,191,46,72,247,27,252,250,244,59,190,251,53,254,164,95,19,223,253,6,191,1,126,255,139,12,86,102,28,6,214,191,134,1,253,218,255,23,141,99,215,142,227,79,252,53,228,51,124,245,59,113,171,95,35,253,163,232,159,29,126,255,215,225,121,61,160,255,127,251,215,176,252,251,215,254,154,212,245,175,169,223,125,19,207,229,215,164,217,239,194,67,252,141,254,32,162,219,159,36,191,255,198,127,208,175,105,127,255,77,254,160,95,203,254,254,155,254,65,191,182,253,253,215,248,131,126,29,239,119,250,238,63,50,52,166,113,253,71,191,166,182,255,117,126,141,255,140,105,76,240,168,205,87,244,221,127,246,39,209,120,127,29,249,27,239,253,103,68,243,255,236,15,114,159,253,53,212,159,251,236,215,224,207,254,111,234,87,62,251,49,102,245,95,243,63,250,53,248,251,255,236,47,146,191,127,45,254,251,215,213,191,137,166,128,251,7,253,122,246,239,223,128,255,254,245,233,111,193,235,55,250,143,0,11,191,11,174,255,195,31,100,102,23,243,231,243,236,223,135,127,126,173,8,207,254,90,81,158,221,251,58,244,223,196,179,191,134,240,236,223,36,60,251,27,208,239,95,253,77,204,179,191,6,120,246,255,166,255,255,26,127,211,175,105,191,251,53,100,124,191,1,104,246,159,245,198,100,224,254,204,111,70,255,252,90,224,223,63,239,215,54,99,58,252,77,228,51,116,104,248,119,159,62,123,250,107,8,18,191,14,253,251,156,126,254,190,244,255,63,143,63,251,117,56,255,246,151,209,255,255,182,95,195,228,232,254,218,111,156,167,213,166,88,184,255,214,141,111,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,125,159,31,181,255,209,179,233,185,252,128,88,247,223,227,88,247,247,252,61,125,120,94,30,129,115,10,191,38,231,17,228,119,250,130,115,16,18,247,254,166,156,31,248,247,40,214,29,75,142,225,111,146,216,95,114,5,191,38,229,116,126,157,244,63,251,139,16,39,255,58,18,87,255,65,242,55,199,201,127,146,228,13,252,207,255,26,250,252,175,137,124,254,127,211,231,255,55,127,174,57,135,63,8,185,10,211,215,175,169,125,253,186,94,95,248,236,215,245,250,210,28,134,247,185,244,213,255,92,250,114,249,141,95,139,251,250,53,59,125,253,122,157,190,126,189,129,190,126,189,129,190,126,189,104,95,191,142,237,203,229,17,254,239,63,73,199,252,15,97,188,191,134,203,179,240,223,191,102,39,207,242,107,117,242,44,191,182,203,179,252,67,14,110,152,103,193,243,255,183,60,11,30,63,207,178,143,193,253,90,145,60,225,175,53,148,39,148,60,203,193,175,97,243,132,63,43,57,149,175,43,187,190,140,122,252,241,39,117,248,227,79,50,252,97,120,203,240,135,208,238,63,251,147,124,254,248,53,60,254,248,53,148,63,254,159,0,0,0,255,255};
			}
		}
#endif
	}
	/// <summary><para>Technique 'NonLinearDepthOutTextureAlphaClip' generated from file 'Depth.fx'</para><para>Vertex Shader: approximately 7 instruction slots used, 4 registers</para><para>Pixel Shader: approximately 7 instruction slots used (1 texture, 6 arithmetic), 1 register</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	public sealed class NonLinearDepthOutTextureAlphaClip : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'NonLinearDepthOutTextureAlphaClip' shader</summary>
		public NonLinearDepthOutTextureAlphaClip()
		{
			this.sc0 = -1;
			this.sc1 = -1;
			this.sc2 = -1;
			this.pts[0] = ((Xen.Graphics.TextureSamplerState)(192));
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			NonLinearDepthOutTextureAlphaClip.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			NonLinearDepthOutTextureAlphaClip.cid0 = state.GetNameUniqueID("clipThreshold");
			NonLinearDepthOutTextureAlphaClip.sid0 = state.GetNameUniqueID("AlphaTextureSampler");
			NonLinearDepthOutTextureAlphaClip.tid0 = state.GetNameUniqueID("AlphaTexture");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != NonLinearDepthOutTextureAlphaClip.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			this.preg_change = (this.preg_change | ic);
			this.vbreg_change = (this.vbreg_change | ic);
			this.vireg_change = (this.vireg_change | ic);
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
				NonLinearDepthOutTextureAlphaClip.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			if ((this.preg_change == true))
			{
				NonLinearDepthOutTextureAlphaClip.fx.ps_c.SetValue(this.preg);
				this.preg_change = false;
				ic = true;
			}
			if ((ext == Xen.Graphics.ShaderSystem.ShaderExtension.Blending))
			{
				ic = (ic | state.SetBlendMatricesDirect(NonLinearDepthOutTextureAlphaClip.fx.vsb_c, ref this.sc1));
			}
			if ((ext == Xen.Graphics.ShaderSystem.ShaderExtension.Instancing))
			{
				this.vireg_change = (this.vireg_change | state.SetViewProjectionMatrix(ref this.vireg[0], ref this.vireg[1], ref this.vireg[2], ref this.vireg[3], ref this.sc2));
				if ((this.vireg_change == true))
				{
					NonLinearDepthOutTextureAlphaClip.fx.vsi_c.SetValue(this.vireg);
					this.vireg_change = false;
					ic = true;
				}
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref NonLinearDepthOutTextureAlphaClip.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((NonLinearDepthOutTextureAlphaClip.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((NonLinearDepthOutTextureAlphaClip.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			NonLinearDepthOutTextureAlphaClip.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out NonLinearDepthOutTextureAlphaClip.fx, NonLinearDepthOutTextureAlphaClip.fxb, 9, 11);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(NonLinearDepthOutTextureAlphaClip.vin[i]));
			index = NonLinearDepthOutTextureAlphaClip.vin[(i + 2)];
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
		/// <summary>Name ID for 'clipThreshold'</summary>
		private static int cid0;
		/// <summary>Assign the shader value 'float clipThreshold'</summary>
		public float ClipThreshold
		{
			set
			{
				this.preg[0] = new Microsoft.Xna.Framework.Vector4(value, 0F, 0F, 0F);
				this.preg_change = true;
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>Change ID for Semantic bound attribute '__BLENDMATRICES__GENMATRIX'</summary>
		private int sc1;
		/// <summary>Change ID for Semantic bound attribute 'viewProj'</summary>
		private int sc2;
		/// <summary>Get/Set the Texture Sampler State for 'Sampler2D AlphaTextureSampler'</summary>
		public Xen.Graphics.TextureSamplerState AlphaTextureSampler
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
		/// <summary>Get/Set the Bound texture for 'Texture2D AlphaTexture'</summary>
		public Microsoft.Xna.Framework.Graphics.Texture2D AlphaTexture
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
		/// <summary>Name uid for sampler for 'Sampler2D AlphaTextureSampler'</summary>
		static int sid0;
		/// <summary>Name uid for texture for 'Texture2D AlphaTexture'</summary>
		static int tid0;
		/// <summary>Pixel samplers/textures changed</summary>
		bool ptc;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,2,0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[4];
		/// <summary>Pixel shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] preg = new Microsoft.Xna.Framework.Vector4[1];
		/// <summary>Instancing shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vireg = new Microsoft.Xna.Framework.Vector4[4];
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
				return new byte[] {4,188,240,11,207,131,0,1,32,152,0,8,254,255,9,1,0,0,15,152,135,0,1,3,131,0,1,1,131,0,1,96,135,0,1,4,131,0,1,4,131,0,1,1,195,0,6,6,95,118,115,95,99,134,0,1,3,131,0,1,1,131,0,1,152,135,0,1,1,131,0,1,4,131,0,1,1,147,0,0,1,6,1,95,1,112,1,115,1,95,1,99,134,0,0,1,3,131,0,0,1,1,1,0,1,0,1,14,1,64,135,0,0,1,216,131,0,0,1,4,131,0,0,1,1,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,153,0,0,1,7,1,95,1,118,1,115,1,98,1,95,1,99,133,0,0,1,3,131,0,0,1,1,1,0,1,0,1,14,1,168,135,0,0,1,4,131,0,0,1,4,131,0,0,1,1,195,0,0,1,7,1,95,1,118,1,115,1,105,1,95,1,99,133,0,0,1,12,131,0,0,1,4,1,0,1,0,1,14,1,204,143,0,0,1,7,1,95,1,112,1,115,1,95,1,115,1,48,133,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,3,131,0,0,1,16,131,0,0,1,4,143,0,0,1,4,131,0,0,1,15,131,0,0,1,4,143,0,0,1,9,1,66,1,108,1,101,1,110,1,100,1,105,1,110,1,103,135,0,0,1,5,131,0,0,1,16,131,0,0,1,4,143,0,0,1,6,131,0,0,1,15,131,0,0,1,4,143,0,0,1,11,1,73,1,110,1,115,1,116,1,97,1,110,1,99,1,105,1,110,1,103,133,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,5,131,0,0,1,1,131,0,0,1,10,131,0,0,1,7,131,0,0,1,4,131,0,0,1,32,139,0,0,1,108,131,0,0,1,136,139,0,0,1,164,131,0,0,1,192,138,0,0,1,14,1,76,1,0,1,0,1,14,1,104,138,0,0,1,14,1,180,1,0,1,0,1,14,1,200,138,0,0,1,15,1,140,135,0,0,1,3,1,0,1,0,1,15,1,8,135,0,0,1,2,131,0,0,1,92,134,0,0,1,14,1,220,1,0,1,0,1,14,1,216,131,0,0,1,93,134,0,0,1,14,1,244,1,0,1,0,1,14,1,240,1,0,1,0,1,15,1,60,135,0,0,1,2,131,0,0,1,92,134,0,0,1,15,1,16,1,0,1,0,1,15,1,12,131,0,0,1,93,134,0,0,1,15,1,40,1,0,1,0,1,15,1,36,1,0,1,0,1,15,1,124,135,0,0,1,2,131,0,0,1,92,134,0,0,1,15,1,80,1,0,1,0,1,15,1,76,131,0,0,1,93,134,0,0,1,15,1,104,1,0,1,0,1,15,1,100,135,0,0,1,6,135,0,0,1,2,132,255,0,131,0,0,1,1,134,0,0,1,1,1,164,1,16,1,42,1,17,131,0,0,1,1,1,16,131,0,0,1,148,135,0,0,1,36,131,0,0,1,192,131,0,0,1,232,139,0,0,1,152,131,0,0,1,28,131,0,0,1,139,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,135,0,0,1,132,131,0,0,1,68,1,0,1,2,131,0,0,1,1,133,0,0,1,76,131,0,0,1,92,131,0,0,1,108,1,0,1,3,131,0,0,1,1,133,0,0,1,116,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,1,150,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,84,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,16,1,66,1,0,1,3,1,0,1,3,131,0,0,1,33,1,0,1,0,1,48,1,80,1,0,1,0,1,49,1,81,193,0,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,0,1,33,1,31,1,31,1,254,1,255,1,0,1,0,1,64,1,0,1,184,1,64,1,0,1,0,1,1,1,0,1,0,1,66,1,194,131,0,0,1,76,1,64,131,0,0,1,108,1,198,1,177,1,121,1,255,1,0,1,0,1,200,1,143,1,192,1,0,1,0,1,198,1,108,1,0,1,225,150,0,0,1,2,132,255,0,138,0,0,1,2,1,16,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,20,131,0,0,1,252,135,0,0,1,36,135,0,0,1,196,139,0,0,1,156,131,0,0,1,28,131,0,0,1,143,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,136,131,0,0,1,48,1,0,1,2,1,0,1,4,1,0,1,4,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,105,1,95,1,99,1,0,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,4,198,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,252,1,0,1,17,1,0,1,5,138,0,0,1,16,1,66,131,0,0,1,1,131,0,0,1,6,131,0,0,1,2,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,0,1,80,1,4,1,0,1,12,1,0,1,5,1,0,1,13,1,0,1,6,1,0,1,14,1,0,1,7,1,0,1,63,1,0,1,8,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,0,1,16,1,19,1,0,1,0,1,16,1,18,1,245,1,85,1,96,1,3,1,0,1,0,1,18,1,3,1,194,133,0,0,1,96,1,9,1,48,1,15,1,18,1,0,1,18,135,0,0,1,32,1,18,1,196,1,0,1,34,131,0,0,1,5,1,248,1,48,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,15,1,200,132,0,0,1,5,1,248,1,32,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,64,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,80,131,0,0,1,6,1,136,132,0,0,1,5,1,248,132,0,0,1,6,1,136,132,0,0,1,200,1,15,131,0,0,1,27,1,0,1,0,1,225,1,3,1,0,1,0,1,200,1,15,131,0,0,1,198,1,0,1,0,1,235,1,3,1,5,1,0,1,200,1,15,131,0,0,1,177,1,148,1,148,1,235,1,3,1,4,1,0,1,200,1,15,1,0,1,2,1,0,1,108,1,248,1,148,1,235,1,3,1,2,1,0,1,200,1,8,131,0,0,1,233,1,167,1,0,1,175,1,2,1,7,1,0,1,200,1,4,131,0,0,1,233,1,167,1,0,1,175,1,2,1,6,1,0,1,200,1,2,131,0,0,1,233,1,167,1,0,1,175,1,2,1,5,1,0,1,200,1,1,131,0,0,1,233,1,167,1,0,1,175,1,2,1,4,1,0,1,200,1,15,1,128,1,62,132,0,0,1,226,131,0,0,1,200,1,3,1,128,1,1,1,0,1,176,1,176,1,0,1,226,1,1,1,1,1,0,1,200,1,3,1,128,1,0,1,0,1,26,1,26,1,0,1,226,150,0,0,1,1,132,255,0,131,0,0,1,1,134,0,0,1,1,1,164,1,16,1,42,1,17,131,0,0,1,1,1,16,131,0,0,1,148,135,0,0,1,36,131,0,0,1,192,131,0,0,1,232,139,0,0,1,152,131,0,0,1,28,131,0,0,1,139,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,135,0,0,1,132,131,0,0,1,68,1,0,1,2,131,0,0,1,1,133,0,0,1,76,131,0,0,1,92,131,0,0,1,108,1,0,1,3,131,0,0,1,1,133,0,0,1,116,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,1,150,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,84,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,16,1,66,1,0,1,3,1,0,1,3,131,0,0,1,33,1,0,1,0,1,48,1,80,1,0,1,0,1,49,1,81,193,0,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,0,1,33,1,31,1,31,1,254,1,255,1,0,1,0,1,64,1,0,1,184,1,64,1,0,1,0,1,1,1,0,1,0,1,66,1,194,131,0,0,1,76,1,64,131,0,0,1,108,1,198,1,177,1,121,1,255,1,0,1,0,1,200,1,143,1,192,1,0,1,0,1,198,1,108,1,0,1,225,150,0,0,1,1,132,255,0,138,0,0,1,16,1,232,1,16,1,42,1,17,1,1,1,0,1,0,1,14,1,224,1,0,1,0,1,2,1,8,135,0,0,1,36,1,0,1,0,1,14,1,112,1,0,1,0,1,14,1,152,138,0,0,1,14,1,72,131,0,0,1,28,1,0,1,0,1,14,1,59,1,255,1,254,1,3,132,0,0,1,2,131,0,0,1,28,134,0,0,1,14,1,52,131,0,0,1,68,1,0,1,2,131,0,0,1,4,133,0,0,1,76,131,0,0,1,92,131,0,0,1,156,1,0,1,2,1,0,1,4,1,0,1,216,133,0,0,1,164,131,0,0,1,180,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,4,198,0,0,1,95,1,118,1,115,1,98,1,95,1,99,1,0,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,216,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,156,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,1,0,1,0,1,1,1,200,1,0,1,17,1,0,1,6,138,0,0,1,16,1,66,131,0,0,1,1,131,0,0,1,4,131,0,0,1,2,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,5,1,0,1,0,1,80,1,6,1,0,1,0,1,16,1,7,1,0,1,48,1,32,1,8,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,0,1,16,1,36,1,0,1,0,1,16,1,35,180,0,0,1,63,1,128,1,0,1,0,1,64,1,64,134,0,0,1,240,1,85,1,64,1,5,1,0,1,0,1,18,1,0,1,194,133,0,0,1,96,1,9,1,96,1,15,1,18,1,0,1,18,133,0,0,1,96,1,21,1,96,1,27,1,18,1,0,1,18,133,0,0,1,32,1,33,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,35,1,0,1,0,1,34,133,0,0,1,5,1,248,1,48,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,15,1,200,132,0,0,1,5,1,248,1,32,131,0,0,1,2,1,208,132,0,0,1,5,1,248,132,0,0,1,6,1,136,132,0,0,1,200,1,15,1,0,1,6,1,0,1,0,1,198,1,0,1,161,1,0,1,255,1,0,1,92,134,0,0,1,27,1,226,1,0,1,0,1,6,1,200,1,15,1,0,1,0,1,160,1,198,1,136,1,0,1,161,1,2,1,4,1,0,1,200,1,15,1,0,1,5,1,160,1,198,1,136,1,0,1,161,1,2,1,5,1,0,1,92,1,15,1,0,1,4,1,160,1,198,1,136,1,198,1,161,1,2,1,6,1,6,1,200,1,15,1,0,1,4,1,160,1,177,1,136,1,0,1,171,1,2,1,6,1,4,1,200,1,15,1,0,1,5,1,160,1,177,1,136,1,0,1,171,1,2,1,5,1,5,1,200,1,15,1,0,1,0,1,160,1,177,1,136,1,0,1,171,1,2,1,4,1,0,1,92,1,2,1,0,1,2,1,0,1,27,1,27,1,177,1,161,1,3,1,0,1,6,1,200,1,15,1,0,1,0,1,160,1,27,1,52,1,148,1,171,1,2,1,4,1,0,1,200,1,15,1,0,1,5,1,160,1,27,1,52,1,148,1,171,1,2,1,5,1,5,1,200,1,15,1,0,1,4,1,160,1,27,1,52,1,148,1,171,1,2,1,6,1,4,1,92,1,8,1,0,1,2,1,0,1,27,1,27,1,108,1,161,1,3,1,1,1,6,1,200,1,15,1,0,1,4,1,160,1,108,1,208,1,148,1,171,1,2,1,6,1,4,1,200,1,15,1,0,1,5,1,160,1,108,1,208,1,148,1,171,1,2,1,5,1,5,1,200,1,15,1,0,1,0,1,160,1,108,1,208,1,148,1,171,1,2,1,4,1,0,1,200,1,1,131,0,0,1,170,1,167,1,0,1,239,1,0,1,3,1,0,1,200,1,2,131,0,0,1,170,1,167,1,0,1,239,1,5,1,3,1,0,1,200,1,4,131,0,0,1,170,1,167,1,0,1,239,1,4,1,3,1,0,1,200,1,1,1,0,1,2,1,0,1,190,1,190,1,0,1,176,131,0,0,1,200,1,4,1,0,1,2,1,0,1,190,1,190,1,0,1,176,1,0,1,1,1,0,1,200,1,15,1,0,1,3,1,0,1,176,1,27,1,166,1,108,1,255,1,3,1,0,1,200,1,8,131,0,0,1,85,1,62,1,0,1,175,1,3,1,3,1,0,1,200,1,4,131,0,0,1,85,1,62,1,0,1,175,1,3,1,2,1,0,1,200,1,3,131,0,0,1,196,1,25,1,0,1,224,1,2,1,2,1,0,1,200,1,15,1,128,1,62,132,0,0,1,226,131,0,0,1,200,1,3,1,128,1,1,1,0,1,176,1,176,1,0,1,226,1,1,1,1,1,0,1,200,1,3,1,128,1,0,1,0,1,26,1,26,1,0,1,226,151,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,164,1,16,1,42,1,17,131,0,0,1,1,1,16,131,0,0,1,148,135,0,0,1,36,131,0,0,1,192,131,0,0,1,232,139,0,0,1,152,131,0,0,1,28,131,0,0,1,139,1,255,1,255,1,3,132,0,0,1,2,131,0,0,1,28,135,0,0,1,132,131,0,0,1,68,1,0,1,2,131,0,0,1,1,133,0,0,1,76,131,0,0,1,92,131,0,0,1,108,1,0,1,3,131,0,0,1,1,133,0,0,1,116,132,0,0,1,95,1,112,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,1,150,0,0,1,95,1,112,1,115,1,95,1,115,1,48,1,0,1,171,1,0,1,4,1,0,1,12,1,0,1,1,1,0,1,1,1,0,1,1,134,0,0,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,84,1,16,1,0,1,1,132,0,0,1,4,134,0,0,1,16,1,66,1,0,1,3,1,0,1,3,131,0,0,1,33,1,0,1,0,1,48,1,80,1,0,1,0,1,49,1,81,193,0,0,1,1,1,16,1,2,1,0,1,0,1,18,1,0,1,196,133,0,0,1,48,1,3,1,0,1,0,1,34,133,0,0,1,16,1,8,1,0,1,33,1,31,1,31,1,254,1,255,1,0,1,0,1,64,1,0,1,184,1,64,1,0,1,0,1,1,1,0,1,0,1,66,1,194,131,0,0,1,76,1,64,131,0,0,1,108,1,198,1,177,1,121,1,255,1,0,1,0,1,200,1,143,1,192,1,0,1,0,1,198,1,108,1,0,1,225,151,0,0,132,255,0,138,0,0,1,1,1,160,1,16,1,42,1,17,1,1,1,0,1,0,1,1,1,4,131,0,0,1,156,135,0,0,1,36,135,0,0,1,196,139,0,0,1,156,131,0,0,1,28,131,0,0,1,143,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,135,0,0,1,136,131,0,0,1,48,1,0,1,2,131,0,0,1,4,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,4,198,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,156,1,0,1,17,1,0,1,2,138,0,0,1,16,1,66,131,0,0,1,1,131,0,0,1,2,131,0,0,1,2,1,0,1,0,1,2,1,144,1,0,1,16,1,0,1,3,1,0,1,32,1,80,1,4,1,0,1,0,1,48,1,80,1,0,1,1,1,49,1,81,1,0,1,0,1,16,1,11,1,0,1,0,1,16,1,10,1,48,1,5,1,32,1,3,1,0,1,0,1,18,1,0,1,194,133,0,0,1,80,1,5,1,0,1,0,1,18,1,0,1,196,133,0,0,1,32,1,10,1,0,1,0,1,34,133,0,0,1,5,1,248,1,32,131,0,0,1,6,1,136,132,0,0,1,5,1,248,1,16,131,0,0,1,15,1,200,132,0,0,1,200,1,8,131,0,0,1,167,1,167,1,0,1,175,1,2,1,3,1,0,1,200,1,4,131,0,0,1,167,1,167,1,0,1,175,1,2,1,2,1,0,1,200,1,2,131,0,0,1,167,1,167,1,0,1,175,1,2,1,1,1,0,1,200,1,1,131,0,0,1,167,1,167,1,0,1,175,1,2,1,0,1,0,1,200,1,15,1,128,1,62,132,0,0,1,226,131,0,0,1,200,1,3,1,128,1,1,1,0,1,176,1,176,1,0,1,226,1,1,1,1,1,0,1,200,1,3,1,128,1,0,1,0,1,26,1,26,1,0,1,226,142,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {68,41,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,210,95,99,248,249,53,127,236,255,254,191,254,188,223,84,126,255,181,241,55,253,255,15,208,239,126,29,253,255,175,169,127,127,200,243,235,209,255,127,255,203,230,247,159,254,26,174,159,63,75,191,195,239,67,253,240,123,171,240,189,223,243,55,145,239,254,173,13,239,253,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,159,157,231,215,255,53,56,190,155,32,80,51,113,218,95,165,113,218,55,25,71,106,63,5,250,249,141,126,13,129,251,207,105,63,65,27,138,25,155,29,233,243,55,211,118,254,243,107,209,255,17,246,118,63,23,28,255,239,255,27,99,248,205,34,223,227,239,216,123,63,70,255,127,82,230,203,89,177,188,192,223,191,238,175,17,127,31,241,108,236,253,223,144,254,127,182,108,218,108,57,101,8,24,195,235,121,54,203,235,95,99,12,88,192,43,249,53,228,115,188,155,122,239,150,244,255,63,202,251,251,47,163,255,255,67,222,223,207,137,62,115,143,70,127,27,253,254,79,121,127,255,73,94,252,255,27,232,239,160,207,159,162,223,255,123,212,246,223,162,255,255,169,250,247,47,167,223,255,39,250,255,239,25,105,251,91,209,103,191,217,111,234,218,142,232,247,45,250,255,31,20,105,251,134,62,123,233,181,45,233,247,185,182,251,245,228,7,183,255,191,233,49,188,243,123,227,151,95,251,255,254,191,255,175,255,123,231,215,56,121,115,252,228,119,162,63,255,216,95,67,62,67,219,223,137,91,253,26,233,31,70,255,60,213,247,127,77,250,247,57,253,252,125,127,13,161,213,175,173,159,181,10,83,242,11,127,237,95,251,107,210,55,191,38,81,119,136,79,149,167,254,218,95,135,56,239,215,228,255,228,161,143,239,253,254,59,191,198,23,197,180,174,154,234,188,77,183,94,221,73,191,253,252,245,243,84,102,48,61,169,22,171,162,164,95,30,142,247,62,29,63,188,191,55,222,59,216,223,255,53,126,130,216,228,215,252,53,126,211,191,136,168,243,123,248,253,252,46,132,54,205,249,31,68,152,254,73,242,251,175,249,7,253,154,250,59,125,253,39,209,36,253,69,79,120,24,191,41,125,254,159,209,223,255,217,95,244,107,233,223,68,45,250,255,175,241,23,31,51,203,252,166,127,208,175,199,239,252,154,244,217,87,127,210,175,139,54,191,193,175,143,239,241,255,63,233,215,196,119,191,193,111,64,48,126,141,191,232,255,254,191,165,111,67,111,131,203,255,198,244,254,191,136,222,187,150,222,127,226,175,33,159,225,43,67,239,63,138,254,217,225,247,127,29,230,207,3,250,255,183,127,13,43,171,150,182,93,190,255,186,207,229,215,164,185,208,240,55,250,131,136,54,74,207,223,152,198,111,126,255,77,254,160,95,203,254,254,155,254,65,191,182,253,253,215,248,131,126,29,253,29,243,242,235,122,159,83,187,255,200,155,35,253,253,215,32,56,191,246,127,244,107,42,156,95,135,230,8,180,255,53,121,126,190,226,57,35,58,252,58,242,55,96,252,103,52,23,255,217,31,228,62,251,107,232,125,247,153,153,215,95,91,63,251,49,158,235,95,243,63,250,53,248,251,255,236,47,146,191,127,45,254,251,215,213,191,137,214,128,75,243,111,254,254,13,248,239,95,159,254,22,188,126,163,255,8,176,240,187,224,250,63,240,239,24,3,193,248,147,12,7,96,142,125,249,155,225,151,95,43,34,127,191,214,207,169,252,237,125,29,94,216,36,127,191,134,200,223,223,100,126,255,53,221,239,93,249,251,155,110,41,127,127,19,203,223,175,97,229,239,111,146,121,16,249,19,89,196,251,255,217,31,212,165,189,193,235,79,251,205,232,159,95,11,178,248,231,253,218,134,246,135,191,137,124,230,211,126,255,55,113,180,255,117,60,218,255,121,252,217,175,195,121,211,191,140,254,255,183,253,26,38,39,235,104,255,77,201,167,250,2,22,238,191,117,227,27,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,190,137,231,71,48,126,244,252,232,249,209,99,158,203,15,136,149,255,61,142,149,127,207,223,211,135,231,229,65,108,126,196,228,83,126,77,206,131,200,239,212,136,243,41,191,174,198,201,200,101,252,123,20,247,142,37,71,242,55,73,238,66,114,29,191,22,229,170,126,157,244,63,251,139,16,51,255,58,156,74,165,28,10,255,253,107,252,77,191,22,226,111,201,139,120,159,255,53,244,249,95,19,249,252,255,166,207,255,111,254,92,115,38,127,16,114,45,166,175,95,83,251,250,117,189,190,240,217,175,235,245,165,57,24,239,115,233,171,255,185,244,229,242,51,191,22,247,245,107,118,250,250,245,58,125,253,122,3,125,253,122,3,125,253,122,209,190,126,29,219,151,201,41,80,155,63,73,199,252,15,97,188,191,134,203,19,241,223,191,102,39,79,244,107,117,242,68,191,182,203,19,253,67,14,110,152,39,250,53,189,60,17,158,31,229,137,126,110,242,68,120,252,60,209,115,166,125,36,103,251,107,13,229,108,37,79,116,240,107,216,156,237,207,74,78,232,235,234,158,33,29,227,241,247,159,212,225,239,63,201,240,183,145,13,195,223,34,27,255,217,159,228,243,247,175,225,241,247,175,17,225,239,255,39,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Single' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, float value)
		{
			if ((NonLinearDepthOutTextureAlphaClip.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == NonLinearDepthOutTextureAlphaClip.cid0))
			{
				this.ClipThreshold = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader sampler of type 'TextureSamplerState' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetSamplerStateImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Xen.Graphics.TextureSamplerState value)
		{
			if ((NonLinearDepthOutTextureAlphaClip.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == NonLinearDepthOutTextureAlphaClip.sid0))
			{
				this.AlphaTextureSampler = value;
				return true;
			}
			return false;
		}
		/// <summary>Set a shader texture of type 'Texture2D' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetTextureImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Graphics.Texture2D value)
		{
			if ((NonLinearDepthOutTextureAlphaClip.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == NonLinearDepthOutTextureAlphaClip.tid0))
			{
				this.AlphaTexture = value;
				return true;
			}
			return false;
		}
	}
}
