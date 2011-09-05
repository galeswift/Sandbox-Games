// XenFX
// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=7.0.1.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
// SourceFile = GpuParticles.fx
// Namespace = Xen.Ex.Graphics.Processor

namespace Xen.Ex.Graphics.Processor
{
	
	/// <summary><para>Technique 'ParticleStoreLife128' generated from file 'GpuParticles.fx'</para><para>Vertex Shader: approximately 18 instruction slots used, 129 registers</para><para>Pixel Shader: approximately 3 instruction slots used, 0 registers</para></summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("Xen.Graphics.ShaderSystem.CustomTool.dll", "e03e5007-ea04-4dcc-8690-d7e2837ab13f")]
	internal sealed class ParticleStoreLife128 : Xen.Graphics.ShaderSystem.BaseShader
	{
		/// <summary>Construct an instance of the 'ParticleStoreLife128' shader</summary>
		public ParticleStoreLife128()
		{
		}
		/// <summary>Setup shader static values</summary><param name="state"/>
		private void gdInit(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// set the graphics ID
			ParticleStoreLife128.gd = state.DeviceUniqueIndex;
			this.GraphicsID = state.DeviceUniqueIndex;
			ParticleStoreLife128.cid0 = state.GetNameUniqueID("indices");
			ParticleStoreLife128.cid1 = state.GetNameUniqueID("invTargetSize");
		}
		/// <summary>Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.</summary><param name="state"/><param name="ic"/><param name="ec"/><param name="ext"/>
		protected override void BeginImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			// if the device changed, call Warm()
			if ((state.DeviceUniqueIndex != ParticleStoreLife128.gd))
			{
				this.WarmShader(state);
				ic = true;
			}
			// Force updating if the instance has changed
			this.vreg_change = (this.vreg_change | ic);
			if ((this.vreg_change == true))
			{
				ParticleStoreLife128.fx.vs_c.SetValue(this.vreg);
				this.vreg_change = false;
				ic = true;
			}
			// Finally, bind the effect
			if ((ic | ec))
			{
				state.SetEffect(this, ref ParticleStoreLife128.fx, ext);
			}
		}
		/// <summary>Warm (Preload) the shader</summary><param name="state"/>
		protected override void WarmShader(Xen.Graphics.ShaderSystem.ShaderSystemBase state)
		{
			// Shader is already warmed
			if ((ParticleStoreLife128.gd == state.DeviceUniqueIndex))
			{
				return;
			}
			// Setup the shader
			if ((ParticleStoreLife128.gd != state.DeviceUniqueIndex))
			{
				this.gdInit(state);
			}
			ParticleStoreLife128.fx.Dispose();
			// Create the effect instance
			state.CreateEffect(out ParticleStoreLife128.fx, ParticleStoreLife128.fxb, 20, 5);
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
			usage = ((Microsoft.Xna.Framework.Graphics.VertexElementUsage)(ParticleStoreLife128.vin[i]));
			index = ParticleStoreLife128.vin[(i + 1)];
		}
		/// <summary>Static graphics ID</summary>
		private static int gd;
		/// <summary>Static effect container instance</summary>
		private static Xen.Graphics.ShaderSystem.ShaderEffect fx;
		/// <summary/>
		private bool vreg_change;
		/// <summary>Name ID for 'indices'</summary>
		private static int cid0;
		/// <summary>Set the shader array value 'float3 indices[128]'</summary><param name="value"/><param name="readIndex"/><param name="writeIndex"/><param name="count"/>
		public void SetIndices(Microsoft.Xna.Framework.Vector3[] value, uint readIndex, uint writeIndex, uint count)
		{
			Microsoft.Xna.Framework.Vector3 val;
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
						> 128)))
			{
				throw new System.ArgumentException("Invalid range");
			}
			for (i = 0; ((i < count) 
						&& (wi < 128)); i = (i + 1))
			{
				val = value[ri];
				this.vreg[((wi * 1) 
							+ 0)] = new Microsoft.Xna.Framework.Vector4(val.X, val.Y, val.Z, 0F);
				ri = (ri + 1);
				wi = (wi + 1);
			}
			this.vreg_change = true;
		}
		/// <summary>Set and copy the array data for the shader value 'float3 indices[128]'</summary>
		public Microsoft.Xna.Framework.Vector3[] Indices
		{
			set
			{
				this.SetIndices(value, 0, 0, ((uint)(value.Length)));
			}
		}
		/// <summary>Name ID for 'invTargetSize'</summary>
		private static int cid1;
		/// <summary>Set the shader value 'float2 invTargetSize'</summary><param name="value"/>
		public void SetInvTargetSize(ref Microsoft.Xna.Framework.Vector2 value)
		{
			this.vreg[128] = new Microsoft.Xna.Framework.Vector4(value.X, value.Y, 0F, 0F);
			this.vreg_change = true;
		}
		/// <summary>Assign the shader value 'float2 invTargetSize'</summary>
		public Microsoft.Xna.Framework.Vector2 InvTargetSize
		{
			set
			{
				this.SetInvTargetSize(ref value);
			}
		}
		/// <summary>array storing vertex usages, and element indices</summary>
readonly 
		private static int[] vin = new int[] {0,0};
		/// <summary>Vertex shader register storage</summary>
readonly 
		private Microsoft.Xna.Framework.Vector4[] vreg = new Microsoft.Xna.Framework.Vector4[129];
#if XBOX360
		/// <summary>Static RLE compressed shader byte code (Xbox360)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {4,188,240,11,207,131,0,1,16,136,0,8,254,255,9,1,0,0,8,124,135,0,1,3,131,0,5,1,0,0,8,48,135,0,1,129,131,0,1,4,131,0,1,1,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,175,0,0,1,6,1,95,1,118,1,115,1,95,1,99,134,0,0,1,1,131,0,0,1,16,131,0,0,1,4,143,0,0,1,2,131,0,0,1,15,131,0,0,1,4,147,0,0,1,7,1,83,1,104,1,97,1,100,1,101,1,114,133,0,0,1,1,131,0,0,1,1,131,0,0,1,3,131,0,0,1,3,131,0,0,1,4,131,0,0,1,32,138,0,0,1,8,1,112,135,0,0,1,1,1,0,1,0,1,8,1,108,135,0,0,1,2,131,0,0,1,92,134,0,0,1,8,1,64,1,0,1,0,1,8,1,60,131,0,0,1,93,134,0,0,1,8,1,88,1,0,1,0,1,8,1,84,135,0,0,1,2,136,0,0,132,255,0,131,0,0,1,1,135,0,0,1,160,1,16,1,42,1,17,132,0,0,1,124,131,0,0,1,36,135,0,0,1,36,135,0,0,1,88,139,0,0,1,48,131,0,0,1,28,131,0,0,1,35,1,255,1,255,1,3,144,0,0,1,28,1,112,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,36,1,16,134,0,0,1,4,134,0,0,1,8,1,33,1,0,1,1,1,0,1,1,131,0,0,1,1,1,0,1,0,1,48,1,80,132,0,0,1,16,1,1,1,196,1,0,1,34,131,0,0,1,200,1,3,1,192,1,0,1,0,1,176,1,176,1,0,1,226,151,0,0,132,255,0,138,0,0,1,9,1,252,1,16,1,42,1,17,1,1,1,0,1,0,1,8,1,240,1,0,1,0,1,1,1,12,135,0,0,1,36,1,0,1,0,1,8,1,148,1,0,1,0,1,8,1,188,138,0,0,1,8,1,108,131,0,0,1,28,1,0,1,0,1,8,1,95,1,255,1,254,1,3,132,0,0,1,1,131,0,0,1,28,134,0,0,1,8,1,88,131,0,0,1,48,1,0,1,2,131,0,0,1,129,133,0,0,1,56,131,0,0,1,72,1,95,1,118,1,115,1,95,1,99,1,0,1,171,1,171,1,0,1,1,1,0,1,3,1,0,1,1,1,0,1,4,1,0,1,129,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,229,0,0,178,0,0,1,118,1,115,1,95,1,51,1,95,1,48,1,0,1,50,1,46,1,48,1,46,1,49,1,49,1,54,1,50,1,54,1,46,1,48,1,0,1,171,135,0,0,1,1,139,0,0,1,20,1,0,1,252,1,0,1,16,147,0,0,1,64,131,0,0,1,204,1,0,1,1,1,0,1,1,138,0,0,1,8,1,33,131,0,0,1,1,131,0,0,1,1,131,0,0,1,1,1,0,1,0,1,2,1,144,131,0,0,1,3,1,0,1,0,1,48,1,80,1,0,1,0,1,16,1,15,176,0,0,1,63,1,128,1,0,1,0,1,64,131,0,0,1,191,1,128,134,0,0,1,16,1,1,1,16,1,3,1,0,1,0,1,18,1,0,1,194,133,0,0,1,96,1,4,1,64,1,10,1,18,1,0,1,18,135,0,0,1,32,1,14,1,196,1,0,1,34,131,0,0,1,5,1,248,132,0,0,1,14,1,248,132,0,0,1,48,1,16,133,0,0,1,108,1,226,131,0,0,1,92,134,0,0,1,108,1,226,131,0,0,1,200,1,2,1,0,1,0,1,160,1,108,1,108,1,0,1,33,1,0,1,128,1,0,1,176,1,128,133,0,0,1,65,1,194,1,0,1,0,1,255,1,44,1,24,1,1,1,0,1,0,1,27,1,0,1,177,1,234,1,0,1,0,1,128,1,200,1,2,1,0,1,0,1,1,1,177,1,108,1,108,1,237,1,0,1,1,1,1,1,200,1,137,1,192,1,62,1,0,1,177,1,177,1,198,1,139,1,0,1,255,1,255,1,200,1,2,131,0,0,1,27,1,198,1,0,1,224,132,0,0,1,32,133,0,0,1,177,1,226,131,0,0,1,200,1,2,1,128,1,62,1,0,1,177,1,177,1,198,1,139,1,0,1,128,1,255,1,92,134,0,0,1,108,1,226,131,0,0,1,200,1,3,1,128,1,0,1,224,1,197,1,197,1,0,1,34,142,0,0,1,0};
			}
		}
#else
		/// <summary>Static Length+DeflateStream compressed shader byte code (Windows)</summary>
		private static byte[] fxb
		{
			get
			{
				return new byte[] {168,19,0,0,236,189,7,96,28,73,150,37,38,47,109,202,123,127,74,245,74,215,224,116,161,8,128,96,19,36,216,144,64,16,236,193,136,205,230,146,236,29,105,71,35,41,171,42,129,202,101,86,101,93,102,22,64,204,237,157,188,247,222,123,239,189,247,222,123,239,189,247,186,59,157,78,39,247,223,255,63,92,102,100,1,108,246,206,74,218,201,158,33,128,170,200,31,63,126,124,31,63,34,254,197,223,240,127,250,251,126,179,95,195,61,191,230,143,253,223,255,215,31,244,27,200,239,191,54,254,166,255,239,232,223,127,48,253,255,215,209,207,126,244,252,232,249,209,243,163,231,71,207,143,158,31,61,63,122,126,61,250,255,239,127,217,252,254,211,95,67,252,131,223,236,215,16,95,193,127,126,45,250,255,111,26,249,220,248,19,191,62,253,255,245,60,155,229,181,124,134,255,255,218,250,127,188,147,122,239,180,191,129,123,183,212,223,1,255,79,209,239,127,79,250,236,49,253,255,79,213,191,127,111,250,253,141,215,206,60,255,55,61,166,255,191,142,191,252,191,255,239,255,235,255,254,109,126,141,147,55,199,79,126,39,250,243,199,245,51,239,149,20,159,175,154,223,127,239,247,223,249,53,190,40,166,117,213,84,231,109,186,245,234,78,250,237,231,175,159,167,50,130,244,164,90,172,138,146,126,121,56,222,251,116,252,240,254,222,120,239,96,127,255,215,248,137,95,227,215,248,117,137,10,127,145,7,143,159,223,69,208,250,131,104,180,127,211,175,201,191,255,218,244,251,127,166,191,255,70,244,251,175,241,23,241,239,191,193,111,138,207,255,32,31,35,140,193,252,254,175,253,24,253,243,107,253,95,52,134,223,253,215,50,99,248,253,127,3,249,12,227,252,157,184,213,175,145,130,30,59,191,134,116,250,7,211,191,7,244,243,219,191,134,153,195,191,246,175,253,53,137,234,191,38,81,29,62,223,143,158,31,61,63,122,126,244,252,232,249,209,243,163,231,71,15,158,203,15,240,127,254,96,246,127,254,160,223,131,192,252,158,244,243,31,4,60,231,255,252,166,127,210,111,201,191,255,154,244,251,175,241,39,253,90,236,254,241,239,112,69,254,164,177,124,247,55,161,237,175,169,237,254,96,241,141,126,237,95,19,237,210,95,131,96,255,26,127,211,175,195,46,227,175,137,54,127,16,189,140,207,254,160,95,151,97,253,90,246,179,95,147,255,254,181,255,163,95,35,253,175,249,29,233,247,215,161,175,190,250,131,188,126,233,255,127,205,31,28,254,253,127,255,73,191,46,247,247,107,241,223,127,208,175,241,213,95,244,227,22,231,175,254,160,223,88,251,193,239,244,255,63,216,27,207,31,244,91,80,7,191,166,182,19,88,191,198,31,44,184,254,218,255,208,175,73,126,221,31,76,176,254,224,95,227,175,17,95,239,215,248,141,254,161,63,248,215,216,255,139,224,125,254,63,1,0,0,255,255};
			}
		}
#endif
		/// <summary>Set a shader attribute of type 'Vector2' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, ref Microsoft.Xna.Framework.Vector2 value)
		{
			if ((ParticleStoreLife128.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == ParticleStoreLife128.cid1))
			{
				this.SetInvTargetSize(ref value);
				return true;
			}
			return false;
		}
		/// <summary>Set a shader attribute of type 'Vector3[]' by global unique ID, see <see cref="Xen.Graphics.ShaderSystem.ShaderSystemBase.GetNameUniqueID"/> for details.</summary><param name="state"/><param name="id"/><param name="value"/>
		protected override bool SetAttributeImpl(Xen.Graphics.ShaderSystem.ShaderSystemBase state, int id, Microsoft.Xna.Framework.Vector3[] value)
		{
			if ((ParticleStoreLife128.gd != state.DeviceUniqueIndex))
			{
				this.WarmShader(state);
			}
			if ((id == ParticleStoreLife128.cid0))
			{
				this.SetIndices(value, 0, 0, ((uint)(value.Length)));
				return true;
			}
			return false;
		}
	}
}
