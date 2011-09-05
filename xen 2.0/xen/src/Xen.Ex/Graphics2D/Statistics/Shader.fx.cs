// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = Shader.fx
// Namespace = Xen.Ex.Graphics2D.Statistics

namespace Xen.Ex.Graphics2D.Statistics
{
	
	/// <summary><para>Technique 'DrawGraphLine' generated from file 'Shader.fx'</para><para>Vertex Shader: approximately 10 instruction slots used, 204 registers</para><para>Pixel Shader: approximately 9 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class DrawGraphLine : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'DrawGraphLine' shader</summary>
		public DrawGraphLine()
		{
			this.sc0 = -1;
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			DrawGraphLine.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			DrawGraphLine.cid0 = state.GetNameUniqueID("graphLine");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != DrawGraphLine.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			// Set the value for attribute 'worldViewProj'
			this.vreg_change = (this.vreg_change | state.SetWorldViewProjectionMatrix(ref this.vreg[200], ref this.vreg[201], ref this.vreg[202], ref this.vreg[203], ref this.sc0));
			if ((this.vreg_change == true))
			{
				DrawGraphLine.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref DrawGraphLine.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((DrawGraphLine.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((DrawGraphLine.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			DrawGraphLine.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out DrawGraphLine.fx, DrawGraphLine.fxb, 12, 13);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(DrawGraphLine.vin[i]));
			index = DrawGraphLine.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'graphLine'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float4 graphLine[200]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetGraphLine(Microsoft.Xna.Framework.Vector4[] value, uint readIndex, uint writeIndex, uint count)
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
						> 200)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 200)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = val;
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float4 graphLine[200]'</summary>
		public Microsoft.Xna.Framework.Vector4[] GraphLine
		{
			set
			{
				this.SetGraphLine(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Change ID for Semantic bound attribute 'worldViewProj'</summary>
		private int sc0;
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[204];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,13,44,135,0,1,3,131,0,5,1,0,0,12,224,135,0,1,204,131,0,1,4,131,0,1,1,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,163,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,1,131,0,0,1,1,131,0,0,1,3,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,138,0,0,1,13,1,32,135,0,0,1,1,1,0,1,0,1,13,1,28,135,0,0,1,2,131,0,0,1,92,134,0,0,1,12,1,240,1,0,1,0,1,12,1,236,131,0,0,1,93,134,0,0,1,13,1,8,1,0,1,0,1,13,1,4,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,134,0,0,1,1,1,56,1,16,1,42,1,17,132,0,0,1,164,131,0,0,1,148,135,0,0,1,36,131,0,0,1,88,131,0,0,1,128,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,1,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,84,1,16,141,0,0,1,4,1,33,131,0,0,1,1,131,0,0,1,1,1,0,1,0,1,16,1,160,176,0,0,1,63,1,128,146,0,0,1,80,1,1,1,196,1,0,1,34,131,0,0,1,200,1,4,1,0,1,0,1,4,1,108,1,108,1,0,1,160,1,0,1,255,1,0,1,200,1,2,131,0,0,1,198,1,108,1,0,1,226,131,0,0,1,76,1,32,133,0,0,1,177,1,226,131,0,0,1,200,1,137,1,192,1,0,1,0,1,198,1,177,1,0,1,225,131,0,0,1,200,1,2,1,128,1,0,1,0,1,177,1,108,1,0,1,225,151,0,0,132,255,0,138,0,0,1,14,1,44,1,16,1,42,1,17,1,1,1,0,1,0,1,13,1,120,131,0,0,1,180,135,0,0,1,36,134,0,0,1,13,1,68,138,0,0,1,13,1,28,131,0,0,1,28,1,0,1,0,1,13,1,15,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,134,0,0,1,13,1,8,131,0,0,1,48,1,0,1,2,131,0,0,1,204,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,204,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,166,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,180,1,0,1,1,1,0,1,1,138,0,0,1,4,1,33,131,0,0,1,1,131,0,0,1,1,131,0,0,1,1,1,0,1,0,1,2,1,144,131,0,0,1,3,1,0,1,0,1,16,1,160,1,0,1,0,1,16,1,13,1,16,1,1,1,16,1,3,1,0,1,0,1,18,1,0,1,194,133,0,0,1,96,1,4,1,32,1,10,1,18,1,0,1,18,135,0,0,1,32,1,12,1,196,1,0,1,34,131,0,0,1,5,1,248,132,0,0,1,15,1,248,132,0,0,1,48,1,64,133,0,0,1,108,1,226,131,0,0,1,92,134,0,0,1,198,1,226,131,0,0,1,200,1,8,1,0,1,0,1,160,1,190,1,190,1,0,1,48,1,0,1,203,1,0,1,200,1,1,1,0,1,0,1,160,1,190,1,190,1,0,1,48,1,0,1,202,1,0,1,200,1,1,1,0,1,1,1,160,1,190,1,190,1,0,1,48,1,0,1,201,1,0,1,200,1,2,1,0,1,0,1,160,1,190,1,190,1,0,1,48,1,0,1,200,1,0,1,176,1,18,1,128,1,62,1,0,1,108,1,27,1,1,1,128,1,1,1,201,1,200,1,176,1,72,1,128,1,62,1,0,1,27,1,27,1,0,1,128,1,0,1,203,1,202,1,92,134,0,0,1,198,1,226,131,0,0,1,20,1,16,1,128,1,0,1,160,1,0,1,0,1,27,1,194,142,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {252,28,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,206,111,44,191,255,218,248,155,254,255,31,253,70,242,247,63,71,255,255,117,244,179,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,255,202,243,235,209,255,127,255,203,230,247,159,254,26,18,207,252,102,191,134,196,54,254,243,107,209,255,127,211,200,231,18,255,252,223,255,247,175,79,255,190,158,103,179,188,150,207,240,255,95,91,255,143,119,82,239,157,223,77,99,42,180,249,157,244,119,192,255,83,244,251,255,137,98,172,255,142,254,255,167,234,223,191,1,181,249,117,188,118,230,249,191,233,49,241,215,111,133,95,126,173,255,251,255,254,191,254,239,223,230,215,56,121,115,252,228,119,162,63,127,252,215,144,207,188,87,82,124,190,106,126,255,189,223,127,231,215,248,162,152,214,85,83,157,183,233,214,171,59,233,183,159,191,126,158,202,8,210,147,106,177,42,74,250,229,225,120,239,211,241,195,251,123,227,189,131,253,253,95,227,39,126,141,95,227,215,37,42,252,69,191,198,175,241,7,253,30,244,255,127,80,126,254,26,252,249,175,105,63,55,207,31,244,123,252,46,130,238,31,68,35,253,147,126,45,38,197,111,64,191,255,26,127,50,253,255,47,250,13,233,239,95,147,255,254,191,241,217,159,244,235,113,219,95,243,15,250,53,233,111,26,202,175,245,107,254,26,191,54,253,252,207,254,162,95,135,201,247,235,113,155,95,131,254,254,53,127,141,127,229,15,250,117,25,214,175,141,207,232,255,255,52,183,255,53,126,141,223,136,126,254,43,127,17,255,254,27,252,166,244,249,127,246,7,249,35,7,173,204,239,191,217,111,66,255,252,90,255,23,209,234,39,126,109,67,171,223,244,55,150,207,120,78,164,89,10,186,239,252,26,50,136,127,142,254,61,160,159,223,254,53,12,175,252,181,127,45,225,72,115,248,235,112,44,252,163,231,71,207,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,255,95,122,46,63,32,46,252,231,122,241,223,175,241,107,184,248,239,55,253,147,126,75,141,239,232,111,141,5,249,247,63,24,127,143,229,187,191,9,109,37,150,251,181,255,160,127,78,98,63,196,155,127,208,175,145,254,110,4,251,215,160,239,127,79,122,231,183,70,27,188,255,47,253,26,233,255,205,159,255,152,252,253,15,33,230,251,167,232,61,249,251,215,226,191,255,105,251,247,175,195,127,255,51,246,239,223,128,255,254,103,233,111,68,200,255,79,0,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector4[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector4[] value)
		{
			if ((DrawGraphLine.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == DrawGraphLine.cid0))
			{
				this.SetGraphLine(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
	}
}
