using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xen.Graphics.ShaderSystem.CustomTool.FX
{
	public sealed class AsmTechnique
	{
		private AsmListing vsListing, psListing;
		private AsmListing vsBlendOverride, vsInstancingOverride;
		private string vertexShaderComment, pixelShaderComment;
		private RegisterSet registers;
		private readonly string name;
		private readonly TechniqueExtraData defaultValues;

		public string Name
		{
			get { return name; }
		}

		public AsmListing VertexShader		{ get { return vsListing; } }
		public AsmListing BlendingShader	{ get { return vsBlendOverride; } }
		public AsmListing InstancingShader	{ get { return vsInstancingOverride; } }
		public AsmListing PixelShader		{ get { return psListing; } }
		public RegisterSet CommonRegisters	{ get { return registers; } }
		public TechniqueExtraData TechniqueExtraData { get { return defaultValues; } }
		public string VertexShaderComment { get { return vertexShaderComment; } }
		public string PixelShaderComment { get { return pixelShaderComment; } }

		//this is a bit of a hack.
		//it relies on the fact that the DirectX shader compiler
		//marks up the disasembled shader with comments detailing the shader inputs.
		public static AsmTechnique[] ExtractTechniques(SourceShader shader, Platform platform, out DecompiledEffect fx, string generatedPrefix)
		{
			//decompile the shader
			fx = new DecompiledEffect(shader, platform);

			//break it up into techniques
			Tokenizer assemblyTokens = new Tokenizer(fx.DecompiledAsm, false, true, true);

			List<AsmTechnique> techniqes = new List<AsmTechnique>();

			while (assemblyTokens.NextToken())
			{
				if (assemblyTokens.Token.Equals("technique", StringComparison.InvariantCultureIgnoreCase))
				{
					//should be format:
					//technique NAME
					//{
					//}

					assemblyTokens.NextToken();
					string name = assemblyTokens.Token;

					if (generatedPrefix != null)
					{
						//only include generated techniques
						if (name.EndsWith(generatedPrefix))
							name = name.Substring(0, name.Length - generatedPrefix.Length);
						else
							continue;
					}

					assemblyTokens.NextToken();

					//may be a line break
					if (assemblyTokens.Token.Trim().Length == 0)
						assemblyTokens.NextToken();

					//should be a {
					if (assemblyTokens.Token != "{")
						throw new CompileException("Unexpected token in assembly technique declaration, expected '{': " + assemblyTokens.Token);

					// read the entire technique {} block
					if (!assemblyTokens.ReadBlock())
						throw new CompileException("Unexpected end of string in assembly technique pass declaration");

					AsmTechnique asm = new AsmTechnique(name, assemblyTokens.Token, fx.GetTechniqueDefaultValues(name));

					if (!shader.SkipConstantValidation)
					{
						//do some extra validation to make sure pixel inputs match vertex outputs
						asm.ValidatePixleShaderInput(shader, platform);
					}

					techniqes.Add(asm);
				}
			}

			for (int i = 0; i < techniqes.Count; i++)
			{
				techniqes[i].MergeSemantics(fx.EffectRegisters);
			}

			return techniqes.ToArray();
		}

		private void MergeSemantics(RegisterSet fxRegisters)
		{
			if (psListing != null)
				this.psListing.RegisterSet.MergeSemantics(fxRegisters);
			if (vsListing != null)
				this.vsListing.RegisterSet.MergeSemantics(fxRegisters);

			this.registers.MergeSemantics(fxRegisters);
		}

		private void ValidatePixleShaderInput(SourceShader source, Platform platform)
		{
			if (psListing != null && vsListing != null)
			{
				for (int i = 0; i < psListing.InputCount; i++)
				{
					if (!vsListing.ContainsOutput(psListing.GetInput(i)))
					{
						throw new CompileException(string.Format(
							"Pixel Shader '{0}' for Technique '{1}' tries to access input '{2} {3}', which is not output by Vertex Shader '{4}'{5}(Use the CompilerOption 'SkipConstantValidation' to disable this check)",
							source.GetTechnique(name, platform).PixelShaderMethodName,
							name,
							psListing.GetInput(i).Usage,
							psListing.GetInput(i).Index,
							source.GetTechnique(name, platform).VertexShaderMethodName,
							Environment.NewLine));
					}
				}
			}
		}

		private AsmTechnique(string name, string source, TechniqueExtraData defaultValues)
		{
			Tokenizer tokenizer = new Tokenizer(source, false, true, true);
			this.name = name;
			this.defaultValues = defaultValues;

			//parse the asm, and extract the first pass.
			string pass = null;
			string firstPassName = null;
			string blendPass = null, instancingPass = null;

			while (tokenizer.NextToken())
			{
				if (tokenizer.Token.Equals("pass", StringComparison.InvariantCultureIgnoreCase))
				{
					//may have a name next...
					tokenizer.NextToken();
					string token = tokenizer.Token;
					string passName = null;

					if (token != "{")
					{
						//the name is specified
						passName = tokenizer.Token;
						tokenizer.NextToken();
						token = tokenizer.Token;
					}

					//may be a new line
					while (token.Trim().Length == 0)
					{
						tokenizer.NextToken();
						token = tokenizer.Token;
					}

					if (token != "{")
						throw new CompileException("Unexpected token in assembly technique pass declaration, expected '{': " + token);

					if (!tokenizer.ReadBlock())
						throw new CompileException("Unexpected end of string in assembly technique pass declaration");

					bool isAnimated, isInstancing;
					ExtractPassType(passName, out isAnimated, out isInstancing);

					string help = @"
For example:

technique TechniqueName
{
	//default pass:
	pass
	{
		VertexShader = compile vs_2_0 BaseVS();
		PixelShader = compile ps_2_0 BasePS();
	}
	pass Animated
	{
		VertexShader = compile vs_2_0 AnimatedVS();
	}
	pass Instancing
	{
		VertexShader = compile vs_2_0 InstancingVS();
	}
}

Note, the instancing or animation passes may only replace the vertex shader";

					if (pass != null)
					{
						if (blendPass == null && isAnimated)
						{
							blendPass = tokenizer.Token;
						}
						else if (instancingPass == null && isInstancing)
						{
							instancingPass = tokenizer.Token;
						}
						else
							throw new CompileException(@"A shader technique may only define a single Pass, or define a dedicated Animation and/or Instancing pass:" + help);
					}
					else
					{
						if (isAnimated || isInstancing)
							throw new CompileException(@"A shader technique must define a default pass before defining an instancing or animation pass:" + help);

						firstPassName = passName;
						pass = tokenizer.Token;
					}
				}
			}

			if (pass == null)
				throw new CompileException("Technique '" + name + "' does not define a pass");

			ProcessPass(pass);

			this.vsBlendOverride = ProcessSupplimentaryPass(blendPass);
			this.vsInstancingOverride = ProcessSupplimentaryPass(instancingPass);

			SetupCommonRegisters();

			if (this.vsBlendOverride != null)
				this.vsBlendOverride.RegisterSet.CompactDuplicateRegisters(this.vsListing.RegisterSet, "Animated", this.name);
			if (this.vsInstancingOverride != null)
				this.vsInstancingOverride.RegisterSet.CompactDuplicateRegisters(this.vsListing.RegisterSet, "Instancing", this.name);
		}

		internal static void ExtractPassType(string passName, out bool isAnimated, out bool isInstancing)
		{
			isAnimated = false;
			isInstancing = false;

			if (passName.Equals("animated", StringComparison.InvariantCultureIgnoreCase) ||
							   passName.Equals("animation", StringComparison.InvariantCultureIgnoreCase) ||
							   passName.Equals("blending", StringComparison.InvariantCultureIgnoreCase) ||
							   passName.Equals("blended", StringComparison.InvariantCultureIgnoreCase))
				isAnimated = true;

			if (passName.Equals("instances", StringComparison.InvariantCultureIgnoreCase) ||
				passName.Equals("instancing", StringComparison.InvariantCultureIgnoreCase) ||
				passName.Equals("instanced", StringComparison.InvariantCultureIgnoreCase) ||
				passName.Equals("instance", StringComparison.InvariantCultureIgnoreCase))
				isInstancing = true;
		}

		private void ProcessPass(string pass)
		{
			//vsListing, psListing

			bool definesPixelShader = false, definesVertexShader = false;

			//extract the shader code
			Tokenizer tokenizer = new Tokenizer(pass, false, true, true);

			while (tokenizer.NextToken())
			{
				if (tokenizer.Token == "vertexshader")
				{
					while (tokenizer.NextToken())
					{
						if (tokenizer.Token == "asm")
						{
							definesVertexShader = true;

							//extract the vertex shader
							tokenizer.NextToken();
							if (tokenizer.Token != "{")
								throw new CompileException("Expected token in asm vertexshader: '" + tokenizer.Token + "' - expected '{'");
							tokenizer.ReadBlock();
							ProcessShader(tokenizer.Token, out this.vsListing, out vertexShaderComment);
							break;
						}
					}
				}
				if (tokenizer.Token == "pixelshader")
				{
					while (tokenizer.NextToken())
					{
						if (tokenizer.Token == "asm")
						{
							definesPixelShader = true;

							//extract the pixel shader
							tokenizer.NextToken();
							if (tokenizer.Token != "{")
								throw new CompileException("Expected token in asm pixelshader: '" + tokenizer.Token + "' - expected '{'");
							tokenizer.ReadBlock();
							ProcessShader(tokenizer.Token, out this.psListing, out pixelShaderComment);
							break;
						}
					}
				}
			}

			if (!definesVertexShader)
				throw new CompileException(string.Format("Default pass in technique '{0}' does not define a vertex shader", this.name));

			if (!definesPixelShader)
				throw new CompileException(string.Format("Default pass in technique '{0}' does not define a pixel shader", this.name));
		}


		private AsmListing ProcessSupplimentaryPass(string pass)
		{
			if (pass == null)
				return null;

			//vsListing, psListing
			AsmListing listing = null;
			bool definesVertexShader = false;

			//extract the shader code
			Tokenizer tokenizer = new Tokenizer(pass, false, true, true);

			while (tokenizer.NextToken())
			{
				if (tokenizer.Token == "vertexshader")
				{
					while (tokenizer.NextToken())
					{
						if (tokenizer.Token == "asm")
						{
							definesVertexShader = true;
							//extract the vertex shader
							tokenizer.NextToken();
							if (tokenizer.Token != "{")
								throw new CompileException("Expected token in asm vertexshader: '" + tokenizer.Token + "' - expected '{'");
							tokenizer.ReadBlock();
							string comment;
							ProcessShader(tokenizer.Token, out listing, out comment);
							break;
						}
					}
				}
				if (tokenizer.Token == "pixelshader")
				{
					throw new CompileException("A technique pass defining Animation or Instancing may not specify a pixel shader");
				}
			}

			if (!definesVertexShader)
				throw new CompileException(string.Format("Extension pass in technique '{0}' does not define a vertex shader", this.name));

			return listing;
		}

		private void SetupCommonRegisters()
		{
			Dictionary<string, Register> common = new Dictionary<string, Register>();

			AsmListing[] listings = new AsmListing[] { vsListing, psListing, vsBlendOverride, vsInstancingOverride };

			foreach (AsmListing listing in listings)
			{
				if (listing == null)
					continue;

				for (int i = 0; i < listing.RegisterSet.RegisterCount; i++)
				{
					Register reg = listing.RegisterSet.GetRegister(i);

					if (!common.ContainsKey(reg.Name))
						common.Add(reg.Name, reg);
				}
			}

			Register[] registers = new Register[common.Count];
			int count = 0;

			foreach (Register reg in common.Values)
			{
				registers[count++] = reg;
			}

			this.registers = new RegisterSet(registers);
		}

		private void ProcessShader(string asm, out AsmListing shader, out string comment)
		{
			//format:

			//either,

			/*
			 * //header
			 * preshader
			 * //header
			 * shader
			 * //comment
			 */
			
			//or

			/*
			 * //header
			 * shader
			 * //comment
			 */

			string[] asmLinesSource = asm.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			//ignore first and last line (the { and })
			string[] asmLines = new string[Math.Max(0,asmLinesSource.Length - 2)];
			for (int i = 1; i < asmLinesSource.Length-1; i++)
				asmLines[i - 1] = asmLinesSource[i].Trim();

			comment = "";
			if (asmLines.Length > 0)
			{
				comment = asmLines[asmLines.Length - 1];
				if (comment.StartsWith("// "))
					comment = comment.Substring(3);
			}

			bool[] isComment = new bool[asmLines.Length];

			for (int i = 0; i < asmLines.Length - 1; i++)
				isComment[i] = asmLines[i].StartsWith("//");

			//top header
			int headerLength = 0;
			int start = 0;
			for (int i = 1; i < asmLines.Length; i++)
			{
				if (!isComment[i] && asmLines[i].Length != 0)
					break;

				headerLength = i+1;
				start = i+1;
			}

			int firstBlockLength = 0;
			int firstBlockStart = start;
			for (int i = start; i < asmLines.Length; i++)
			{
				firstBlockLength = i - headerLength + 1;
				start = i+1;

				if (isComment[i])
					break;
			}

			int secondHeaderLength = 0;
			int secondHeaderStart = start;
			for (int i = start; i < asmLines.Length; i++)
			{
				if (!isComment[i] && asmLines[i].Length != 0)
					break;

				secondHeaderLength = i - firstBlockLength + 1;
				start = i+1;
			}

			int secondBlockLength = 0;
			int secondBlockStart = start;
			for (int i = start; i < asmLines.Length; i++)
			{
				secondBlockLength = i - secondHeaderLength + 1;
				start = i+1;

				if (isComment[i])
					break;
			}

			if (secondHeaderLength > 0 && 
				secondBlockLength  > 0)
			{
				//preshader is used
//				preshader = new AsmListing(CombineLines(asmLines, firstBlockStart, firstBlockLength), new RegisterSet(CombineLines(asmLines, 1, headerLength)));
//				shader = new AsmListing(CombineLines(asmLines, secondBlockStart, secondBlockLength), new RegisterSet(CombineLines(asmLines, secondHeaderStart, secondHeaderLength)));
				throw new CompileException("Compile Error: Unexpected preshader encountered! Turn back to save yourself!");
			}
			else
			{
				//no preshader
				shader = new AsmListing(CombineLines(asmLines, firstBlockStart, firstBlockLength), new RegisterSet(CombineLines(asmLines, 1, headerLength)));
			}
		}

		string CombineLines(string[] lines, int start, int length)
		{
			StringBuilder builder = new StringBuilder(32 * length);
			for (int i = 0; i < length; i++)
			{
				if (start + i < lines.Length)
					builder.AppendLine(lines[start + i]);
			}
			return builder.ToString();
		}

		public void ConstructTechniqueConstants(StringBuilder sb, bool includeSamplers)
		{

			if (VertexShader.RegisterSet.FloatRegisterCount > 0)
				sb.AppendFormat("uniform float4 _vs_c[{0}] : register(vs, c0);{1}", VertexShader.RegisterSet.FloatRegisterCount, Environment.NewLine);
			if (VertexShader.RegisterSet.BooleanRegisterCount > 0)
				sb.AppendFormat("uniform bool _vs_b[{0}] : register(vs, b0);{1}", VertexShader.RegisterSet.BooleanRegisterCount, Environment.NewLine);

			if (PixelShader.RegisterSet.FloatRegisterCount > 0)
				sb.AppendFormat("uniform float4 _ps_c[{0}] : register(ps, c0);{1}", PixelShader.RegisterSet.FloatRegisterCount, Environment.NewLine);
			if (PixelShader.RegisterSet.BooleanRegisterCount > 0)
				sb.AppendFormat("uniform bool _ps_b[{0}] : register(ps, b0);{1}", PixelShader.RegisterSet.BooleanRegisterCount, Environment.NewLine);

			if (BlendingShader != null && BlendingShader.RegisterSet.FloatRegisterCount > 0)
				sb.AppendFormat("uniform float4 _vsb_c[{0}];{1}", BlendingShader.RegisterSet.FloatRegisterCount, Environment.NewLine);
			if (InstancingShader != null && InstancingShader.RegisterSet.FloatRegisterCount > 0)
				sb.AppendFormat("uniform float4 _vsi_c[{0}];{1}", InstancingShader.RegisterSet.FloatRegisterCount, Environment.NewLine);

			if (includeSamplers)
			{
				//even with a register semantic, a vertex shader sampler doesn't always respect it's set order.
				SortedList<int, string> samplers = new SortedList<int, string>();
				foreach (Register reg in VertexShader.RegisterSet)
				{
					if (reg.Category == RegisterCategory.Sampler)
						samplers.Add(reg.Index, string.Format("{0} _vs_s{1} : register(vs, s{1});", reg.Type, reg.Index));
				}
				//so write them in the expected order too
				foreach (var line in samplers.Values)
					sb.AppendLine(line);

				//do it for PS samplers too (even though they seem to work OK)
				samplers.Clear();
				foreach (Register reg in PixelShader.RegisterSet)
				{
					if (reg.Category == RegisterCategory.Sampler)
						samplers.Add(reg.Index, string.Format("{0} _ps_s{1} : register(ps, s{1});", reg.Type, reg.Index));
				}
				foreach (var line in samplers.Values)
					sb.AppendLine(line);
			}
		}
	}

	public sealed class HlslTechnique
	{
		private readonly HlslStructure hs;
		private readonly string name, pixelShaderMethodName, vertexShaderMethodName, psVersion, vsVersion;
		private readonly string blendingShaderMethodName, instancingShaderMethodName;
		private readonly string[] psArgs, vsArgs, blendArgs, instancingArgs;
		private readonly Platform platform;
		public readonly bool IsGenerated;

		//extracts the techniques
		internal HlslTechnique(HlslStructure hs, Platform platform, string generatedPrefix)
		{
			this.hs = hs;
			this.platform = platform;
			this.name = hs.Elements[1];
			bool primaryPassSet = false;

			if (generatedPrefix != null)
			{
				if (name.EndsWith(generatedPrefix))
				{
					name = name.Substring(0, name.Length - generatedPrefix.Length);
					IsGenerated = true;
				}
			}

			//first child should be the first pass
			for (int i = 0; i < hs.Children.Length; i++)
			{
				if (hs.Children[i].Elements.Length > 0 && 
					hs.Children[i].Elements[0] == "pass")
				{
					//good.

					//this could be an animation or blending pass
					bool isAnimation = false, isInstancing = false;
					if (hs.Children[i].Elements.Length > 1)
						AsmTechnique.ExtractPassType(hs.Children[i].Elements[1], out isAnimation, out isInstancing);

					if (isAnimation || isInstancing)
					{
						string ignored, vs_version, name;
						string[] args_ignored, args;
						ExtractPass(hs.Children[i], out ignored, out name, out ignored, out vs_version, out args_ignored, out args);
					//	if (vs_version != vsVersion)
					//		throw new CompileException(string.Format("Technique Extension Pass '{0}' cannot use a different vertex shader version", hs.Children[i].Elements[1]));
						if (isInstancing)
						{
							instancingShaderMethodName = name;
							instancingArgs = args;
						}
						else
						{
							blendingShaderMethodName = name;
							blendArgs = args;
						}
					}
					else if (!primaryPassSet)
					{
						ExtractPass(hs.Children[i], out pixelShaderMethodName, out vertexShaderMethodName, out psVersion, out vsVersion, out psArgs, out vsArgs);
						primaryPassSet = true;
					}
				}
			}
		}

		private static void ExtractPass(HlslStructure pass, out string ps, out string vs, out string psVersion, out string vsVersion, out string[] psArgs, out string[] vsArgs)
		{
			ps = vs = vsVersion = psVersion = null;
			psArgs = vsArgs = null;

			//a bit nasty...
			foreach (HlslStructure hs in pass.Children)
			{
				//should be:
				//
				//VertexShader = compile vs_2_0 Zomg(true!);
				//
				//or similar

				int type = -1;
				string target = null, method = null;
				List<string> args = new List<string>();
				int paranethDepth = 0;

				for (int i = 0; i < hs.Elements.Length; i++)
				{
					if (hs.Elements[i].Statement.Equals("VertexShader", StringComparison.InvariantCultureIgnoreCase))
						type = 1;
					if (hs.Elements[i].Statement.Equals("PixelShader", StringComparison.InvariantCultureIgnoreCase))
						type = 2;

					if (hs.Elements[i].Statement.Equals("compile", StringComparison.InvariantCultureIgnoreCase))
						target = hs.Elements[i + 1];

					if (hs.Elements[i] == ")")
					{
						paranethDepth--;
					}

					if (paranethDepth > 0)
						args.Add(hs.Elements[i]);

					if (hs.Elements[i] == "(")
					{
						if (paranethDepth == 0)
							method = hs.Elements[i - 1];
						paranethDepth++;
					}
				}

				if (type == 1)
				{
					vs = method;
					vsArgs = args.ToArray();
					vsVersion = target;
				}
				if (type == 2)
				{
					ps = method;
					psArgs = args.ToArray();
					psVersion = target;
				}
			}
		}

		public HlslStructure HlslShader { get { return hs; } }
		public Platform Platform { get { return platform; } }
		public string Name { get { return name; } }
		public string PixelShaderMethodName { get { return pixelShaderMethodName; } }
		public string VertexShaderMethodName { get { return vertexShaderMethodName; } }
		public string BlendingShaderMethodName { get { return blendingShaderMethodName; } }
		public string InstancingShaderMethodName { get { return instancingShaderMethodName; } }
		public string GetPixelShaderVersion(Platform platform) { if (platform == Platform.Xbox) return "ps_3_0"; return psVersion; }
		public string GetVertexShaderVersion(Platform platform) { if (platform == Platform.Xbox) return "vs_3_0"; return vsVersion; }
		public IEnumerator<string> PixelShaderArgs { get { return ((IList<string>)psArgs).GetEnumerator(); } }
		public IEnumerator<string> VertexShaderArgs { get { return ((IList<string>)vsArgs).GetEnumerator(); } }
		public IEnumerator<string> BlendingShaderArgs { get { return ((IList<string>)blendArgs).GetEnumerator(); } }
		public IEnumerator<string> InstancingShaderArgs { get { return ((IList<string>)instancingArgs).GetEnumerator(); } }
	}

	public sealed class HlslMethod
	{
		private readonly HlslStructure hs;
		private readonly string name;
		private readonly Platform platform;

		//extracts the techniques
		internal HlslMethod(HlslStructure hs, Platform platform)
		{
			this.hs = hs;
			this.platform = platform;

			for (int i = 1; i < hs.Elements.Length; i++)
			{
				if (hs.Elements[i] == "(")
					name = hs.Elements[i - 1];
			}
		}

		public bool HasReturnValue { get { return hs.Elements.Length >= 1 && hs.Elements[0] != "void"; } }

		public Platform Platform { get { return platform; } }
		public HlslStructure HlslShader { get { return hs; } }
		public string Name { get { return name; } }
	}
}
