
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xen.Graphics.ShaderSystem.CustomTool.FX
{
	public class ShaderExtensionGenerator : HlslStructure.ITokenTranslator
	{
		private readonly StringBuilder builtShader = new StringBuilder();
		private readonly StringBuilder header = new StringBuilder();
		private readonly StringBuilder footer = new StringBuilder();

		struct MatrixMapping
		{
			public string Name, RemapTo, BaseName;
		}

		private readonly Dictionary<string, MatrixMapping> matrixRemap = new Dictionary<string, MatrixMapping>();
		private readonly Dictionary<string, bool> methodBuilt = new Dictionary<string, bool>();
		private Register worldMatrix;
		private static readonly string worldMatrixName = ToInternalMatrixName("WORLD");
		public static readonly string blendMatricesName = ToInternalMatrixName("BLENDMATRICES");
		private readonly string methodSignatureAppend, methodCallAppend;
		private readonly Dictionary<string, bool> methodNames = new Dictionary<string, bool>();

		private readonly Dictionary<HlslStructure, HlslStructure.ShaderStatement[]> structureBackup;
		private readonly HlslMethod[] methodList;
		private readonly string rootVsMethodName;
		private readonly string result;
		private readonly string techniquePostfix;
		private ShaderExtension methodExtractionExtension;


		public static string GenerateShaderCode(SourceShader shader, string techniquePostfix, bool mixedModeOutput, out int startLine)
		{
			string noTechniques = TechniqueRemover.Process(shader);
			
			startLine = 1;
			for (int i = 0; i < noTechniques.Length; i++)
			{
				if (noTechniques[i] == '\n')
					startLine++;
			}


			if (shader.GenerateInternalClass ||
				shader.UseParentNamespace ||
				shader.DefinePlatform ||
				shader.ExposeRegisters)
			{
				//recreate the extensions flag, if needed
				StringBuilder newExtensons = new StringBuilder("//CompilerOptions = ");

				if (shader.GenerateInternalClass) newExtensons.Append("internalclass, ");
				if (shader.UseParentNamespace) newExtensons.Append("parentnamespace, ");
				if (shader.DefinePlatform) newExtensons.Append("defineplatform, ");
				if (shader.ExposeRegisters) newExtensons.Append("exposeregisters");
				newExtensons.AppendLine();
				
				if (noTechniques.Length > 1 && noTechniques.Substring(0, 2) == Environment.NewLine)
					noTechniques = noTechniques.Substring(2);

				newExtensons.Append(noTechniques);
				noTechniques = newExtensons.ToString();
			}

			if (!mixedModeOutput)
			{
				//separate platforms
				string xbox = new ShaderExtensionGenerator(shader, Platform.Xbox, techniquePostfix).result;
				string pc = new ShaderExtensionGenerator(shader, Platform.Windows, techniquePostfix).result;
				return string.Format(@"{0}

#ifdef XBOX360
{1}
#else
{2}
#endif", noTechniques, xbox, pc);
			}
			else
			{
				string both = new ShaderExtensionGenerator(shader, Platform.Both, techniquePostfix).result;

				return string.Format(@"{0}

{1}",noTechniques, both);
			}
		}
		
		public static void ThrowGeneratorError(string errors, string file)
		{
			file = new System.IO.FileInfo(file).Name;
			throw new CompileException(string.Format(@"The Shader System failed to generated Instancing / Animation methods for shader '{0}'
See the shader in Tutorial 16 for examples of manually implementing animation or instancing extensions.
To disable automatic generation of shader extensions, add the following line to the very top of the shader FX file:
//CompilerOptions = DisableGenerateExtensions

Internal Error:
{1}", file, errors));
		}

		private ShaderExtensionGenerator(SourceShader shader, Platform platform, string techniquePostfix)
		{
			this.techniquePostfix = techniquePostfix;

			//this method temporary modifies the HlslStructure stored in the source shader, so take a backup first.
			structureBackup = new Dictionary<HlslStructure, HlslStructure.ShaderStatement[]>();
			BackupStructure(shader.HlslShader);
			

			RegisterSet registers     = shader.GetDecompiledEffect(platform).EffectRegisters;

			List<string> addRegisters = new List<string>();

			methodList = shader.GetAllMethods(platform, true).ToArray();

			foreach (var method in methodList)
			{
				if (methodNames.ContainsKey(method.Name))
					continue;

				bool open = false;
				bool args = false;

				//work out if this method takes args
				foreach (var element in method.HlslShader.Elements)
				{
					if (open)
					{
						if (element == ")")
							open = false;
						else
							args = true;
					}
					if (element == "(")
						open = true;
				}

				methodNames.Add(method.Name,args);
			}

			//collect up the declared world matrices
			foreach (var reg in registers)
			{
				if (reg.Semantic != null && reg.Semantic.StartsWith("WORLD", StringComparison.InvariantCultureIgnoreCase))
				{
					string replaceWith = reg.Semantic.Substring(5);

					//see if the replacement exists.
					if (replaceWith.Length > 0)
					{
						bool createReg = true;

						foreach (var r2 in registers)
						{
							if (r2.Semantic != null && r2.Semantic.Equals(replaceWith, StringComparison.InvariantCultureIgnoreCase))
							{
								createReg = false;
								matrixRemap.Add(reg.Name, new MatrixMapping() { Name = reg.Name, RemapTo = r2.Name + worldMatrixName, BaseName  = r2.Name});
							}
						}

						if (createReg)
						{
							addRegisters.Add(replaceWith);
							matrixRemap.Add(reg.Name, new MatrixMapping() { Name = reg.Name, RemapTo = ToInternalMatrixName(reg.Name), BaseName = ToInternalMatrixName(replaceWith) }); 
						}
					}
					else
						worldMatrix = reg;
				}
			}

			header.AppendLine();
			header.AppendFormat("float4 {0}[216] : BLENDMATRICES; ", blendMatricesName);
			header.AppendLine();
			//add the new registers
			foreach (var reg in addRegisters)
			{
				header.AppendFormat("float4x4 {0} : {1}; ", ToInternalMatrixName(reg), reg);
				header.AppendLine();
			}

			methodSignatureAppend = "";
			methodCallAppend      = "";

			foreach (var remap in matrixRemap.Values)
			{
				methodSignatureAppend += string.Format("float4x4 {0}, ", remap.RemapTo);
				methodCallAppend      += string.Format("{0}, ", remap.RemapTo);
			}
			methodSignatureAppend += "float4x4 " + worldMatrixName;
			methodCallAppend      += worldMatrixName;

			if (worldMatrix.Name != null)
			{
				matrixRemap.Add(worldMatrix.Name, new MatrixMapping() { Name = worldMatrix.Name, RemapTo = worldMatrixName });
			}


			BuildTechniqueMethod(shader, ShaderExtension.Blending);
			BuildTechniqueMethod(shader, ShaderExtension.Instancing);

			foreach (var tech in shader.GetAllTechniques())
			{
				if (tech.Platform == platform || tech.Platform == Platform.Both)
				{
					AsmTechnique asmTechnique = shader.GetAsmTechnique(tech.Name, platform);
					rootVsMethodName = tech.VertexShaderMethodName;
					GenerateTechnique(shader, tech, asmTechnique, platform);
				}
			}

			header.Append(builtShader);
			header.Append(footer);

			result = header.ToString();
		}


		static string ToInternalMatrixName(string name)
		{
			return string.Format("__{0}__GENMATRIX", name);
		}
		static string ToInternalMethodName(string name, ShaderExtension extension)
		{
			return string.Format("__{0}__{1}_GENCALL", name, extension);
		}


		private void BuildTechniqueMethod(SourceShader shader, ShaderExtension extension)
		{
			methodExtractionExtension = extension;

			foreach (var method in methodList)
			{
				if (methodBuilt.ContainsKey(method.Name) == false)
					methodBuilt.Add(method.Name, false);
			}

			foreach (var tech in shader.GetAllTechniques())
			{
				BuildMethod(tech.VertexShaderMethodName, extension);
			}

			methodBuilt.Clear();

			UnwindBackup(shader.HlslShader);
		}

		private void BackupStructure(HlslStructure hlslStructure)
		{
			structureBackup.Add(hlslStructure, hlslStructure.Elements.Clone() as HlslStructure.ShaderStatement[]);
			foreach (var child in hlslStructure.Children)
				BackupStructure(child);
		}

		private void UnwindBackup(HlslStructure hlslStructure)
		{
			HlslStructure.ShaderStatement[] elements = structureBackup[hlslStructure];

			for (int i = 0; i < hlslStructure.Elements.Length; i++)
				hlslStructure.Elements[i] = elements[i];

			foreach (var child in hlslStructure.Children)
				UnwindBackup(child);
		}

		void BuildMethod(string methodName, ShaderExtension extension)
		{
			if (methodBuilt.ContainsKey(methodName) == false)
				return;
			if (methodBuilt[methodName])
				return;

			StringBuilder sb = new StringBuilder();

			foreach (var method in methodList)
			{
				if (method.Name == methodName)
				{
					methodBuilt[methodName] = true;
					method.HlslShader.ToString(sb, 0, this, null, false, false);
				}
			}

			this.builtShader.Append(sb);
		}


		string HlslStructure.ITokenTranslator.Translate(string token, bool isInBody, out bool replaceBracketWithParenthesis)
		{
			replaceBracketWithParenthesis = false;

			return token;
		}

		bool HlslStructure.ITokenTranslator.IncludeBlock(HlslStructure block, int depth)
		{
			TranslateMatrices(block.Elements, depth, methodExtractionExtension);
			TranslateMethodNames(block.Elements, depth, methodExtractionExtension);

			return true;
		}

		void TranslateMethodNames(HlslStructure.ShaderStatement[] elements, int callDepth, ShaderExtension extension)
		{
			for (int i = 0; i < elements.Length; i++)
			{
				string methodName = elements[i].Statement;

				bool multArg;
				if (methodNames.TryGetValue(methodName, out multArg))
				{
					BuildMethod(methodName, extension);

					//find the method start
					for (int n = i+1; n < elements.Length; n++)
					{
						if (elements[n] == "(")
						{
							string argAppend = multArg ? ", " : "";
							//append the method signatures
							if (callDepth == 0)
								elements[n] = new HlslStructure.ShaderStatement("(" + this.methodSignatureAppend + argAppend, elements[n].Line);
							else
								elements[n] = new HlslStructure.ShaderStatement("(" + this.methodCallAppend + argAppend, elements[n].Line);

							elements[i] = new HlslStructure.ShaderStatement(ToInternalMethodName(methodName,extension), elements[i].Line);
							break;
						}
					}
				}
			}
		}

		void TranslateMatrices(HlslStructure.ShaderStatement[] elements, int callDepth, ShaderExtension extension)
		{
			bool isInstancing = extension == ShaderExtension.Instancing;
			List<int> skipFinalRemappingIndices = new List<int>();

			int processingMul = 0, depth = 0, arg = 0;
			int argStart = 0, argEnd = 0;
			int remapArg = -1;
			int remapIndex = 0;
			MatrixMapping remapping = new MatrixMapping();

			Stack<int> mulStart = new Stack<int>();
			Stack<int> mulDepth = new Stack<int>();
			Stack<int> mulArg = new Stack<int>();
			Stack<int> mulArgStart = new Stack<int>();
			Stack<int> mulArgEnd = new Stack<int>();
			Stack<int> mulRemap = new Stack<int>();
			Stack<int> mulRemapIdx = new Stack<int>();
			Stack<MatrixMapping> mulRemapping = new Stack<MatrixMapping>();

			for (int i = 0; i < elements.Length; i++)
			{
				string line = elements[i].Statement;

				if (line == "mul")
				{
					processingMul++;
					mulStart.Push(i);
					mulDepth.Push(depth);
					mulArg.Push(arg);
					mulArgEnd.Push(argEnd);
					mulArgStart.Push(argStart);
					mulRemap.Push(remapArg);
					mulRemapping.Push(remapping);
					mulRemapIdx.Push(remapIndex);

					depth = 0;
					arg = 0;
					argStart = 0;
					argEnd = 0;
					remapArg = -1;
					remapIndex = 0;
					remapping = new MatrixMapping();
				}

				MatrixMapping mapping;
				if (processingMul > 0 && depth == 1 && matrixRemap.TryGetValue(line, out mapping) && (mapping.BaseName != null || !isInstancing))
				{
					remapArg = arg;
					remapping = mapping;
					remapIndex = i;
				}

				if (processingMul > 0 && line == "(")
				{
					if (depth == 0)
						argStart = i;
					depth++;
				}

				if (processingMul > 0 && depth == 1 && line == ",")
				{
					if (depth == 1)
						argEnd = i;
					arg++;
				}

				if (processingMul > 0 && line == ")")
				{
					depth--;
					if (depth == 0)
					{
						string baseMatrix = remapping.Name;
						if (isInstancing)
							baseMatrix = remapping.BaseName;

						string prefix = "";
						int startPrefix = -1;
						if (remapArg == 1)
							startPrefix = argEnd;
						if (remapArg == 0)
							startPrefix = argStart;

						if (remapIndex > startPrefix && startPrefix != -1)
						{
							for (int p = startPrefix + 1; p < remapIndex; p++)
								prefix += elements[p].Statement + " ";
						}

						//right, see if the args need modifying.
						if (remapArg == 1)
						{
							//a known remapping matrix was used.
							var element = elements[remapIndex];
							elements[remapIndex] = new HlslStructure.ShaderStatement(baseMatrix, element.Line);

							element = elements[argStart];
							elements[argStart] = new HlslStructure.ShaderStatement("(mul(", element.Line);

							element = elements[argEnd];
							elements[argEnd] = new HlslStructure.ShaderStatement("," + prefix + worldMatrixName + "),", element.Line);

							if (!isInstancing)
								skipFinalRemappingIndices.Add(remapIndex);
						}

						if (remapArg == 0) // transpose
						{
							//a known remapping matrix was used.
							var element = elements[remapIndex];
							elements[remapIndex] = new HlslStructure.ShaderStatement(baseMatrix, element.Line);

							element = elements[argEnd];
							elements[argEnd] = new HlslStructure.ShaderStatement(",mul(" + prefix + worldMatrixName + ",", element.Line);

							element = elements[i];
							elements[i] = new HlslStructure.ShaderStatement("))", element.Line);

							if (!isInstancing)
								skipFinalRemappingIndices.Add(remapIndex);
						}


						processingMul--;
						mulStart.Pop();
						depth = mulDepth.Pop();
						arg = mulArg.Pop();
						argEnd = mulArgEnd.Pop();
						argStart = mulArgStart.Pop();
						remapArg = mulRemap.Pop();
						remapping = mulRemapping.Pop();
						remapIndex = mulRemapIdx.Pop();
					}
				}
			}

			//find unremapped matrices
			if (callDepth > 0)
			{
				for (int i = 0; i < elements.Length; i++)
				{
					//it is intended that this matrix reference is not modified.
					if (!isInstancing && skipFinalRemappingIndices.Contains(i))
						continue;

					MatrixMapping mapping;
					if (matrixRemap.TryGetValue(elements[i].Statement, out mapping))
						elements[i] = new HlslStructure.ShaderStatement(mapping.RemapTo, elements[i].Line);
				}
			}
		}


		private void GenerateTechnique(SourceShader shader, HlslTechnique technique, AsmTechnique asmTechnique, Platform platform)
		{
			string blendVS = ProcessTechniqueVS(shader, technique, asmTechnique, ShaderExtension.Blending, platform);
			string instVS = ProcessTechniqueVS(shader, technique, asmTechnique, ShaderExtension.Instancing, platform);
			
			footer.AppendLine();

			////append the new technique
			string techniqueName = technique.Name + techniquePostfix;
			string code = @"
technique {0} {11}
{9}
	pass
	{9}
		VertexShader = compile {1} {5}({6});
		PixelShader = compile {2} {3}({4});
	{10}
	pass Blending
	{9}
		VertexShader = compile {1} {7}({6});
	{10}
	pass Instancing
	{9}
		VertexShader = compile {1} {8}({6});
	{10}
{10}
";

			var annotation = new StringBuilder();

			var asm = shader.GetAsmTechnique(technique.Name, platform);

			if (asm != null && asm.TechniqueExtraData != null &&
				asm.TechniqueExtraData.ClassBaseTypes != null && asm.TechniqueExtraData.ClassBaseTypes.Length > 0)
			{
				annotation.Append("< string  BaseTypes = \"");

				bool first = true;
				foreach (var baseType in asm.TechniqueExtraData.ClassBaseTypes)
				{
					if (!first)
						annotation.Append(", ");
					first = false;
					annotation.Append(baseType);
				}

				annotation.Append("\"; >");
			}
			
			footer.AppendFormat(
				code,
				techniqueName,
				technique.GetVertexShaderVersion(platform),
				technique.GetPixelShaderVersion(platform),
				technique.PixelShaderMethodName,
				CombineArgs(technique.PixelShaderArgs),
				technique.VertexShaderMethodName,
				CombineArgs(technique.VertexShaderArgs),
				blendVS,
				instVS,
				"{","}",
				annotation);

		}

		private string CombineArgs(IEnumerator<string> args)
		{
			StringBuilder sb = new StringBuilder();
			while (args.MoveNext())
				sb.Append(args.Current);
			return sb.ToString();
		}

		private string GetMatrixRankExpansion(string registerName, AsmTechnique asmTechnique, out string matrixType)
		{
			//the matrix being computed is not a 4x4, but internally it must be treated as such,
			//so work out the extension required to fill it out to a full 4x4
			Register reg;
			asmTechnique.CommonRegisters.TryGetRegister(registerName, out reg);
			switch (reg.Rank)
			{
				default:
					matrixType = "float4x4";
					return "";
				case RegisterRank.FloatNx3:
					matrixType = "float4x3";
					return ",float4(0,0,0,1)";
				case RegisterRank.FloatNx2:
					matrixType = "float4x2";
					return ",float4(0,0,0,0),float4(0,0,0,1)";
				case RegisterRank.FloatNx1:
					matrixType = "float4x1";
					return ",float4(0,0,0,0),float4(0,0,0,0),float4(0,0,0,1)";
			}
		}


		private string ProcessTechniqueVS(SourceShader shader, HlslTechnique technique, AsmTechnique asmTechnique, ShaderExtension extension, Platform platform)
		{
			HlslMethod vs = shader.GetMethod(technique.VertexShaderMethodName, platform);
			HlslMethod ps = shader.GetMethod(technique.PixelShaderMethodName, platform);

			if (vs == null) throw new CompileException(string.Format("Unabled to find declaration for vertex shader method '{0}'", technique.VertexShaderMethodName));
			if (ps == null) throw new CompileException(string.Format("Unabled to find declaration for pixel shader method '{0}'", technique.PixelShaderMethodName));

			string nameAppend = "_" + Guid.NewGuid().ToString("N") + "_" + extension.ToString();
			string blendName = "__blend_weights__GEN";
			string indicesName = "__blend_indices__GEN";

			//generated merged matrices
			StringBuilder matrixGen = new StringBuilder();
			foreach (var remap in matrixRemap.Values)
			{
				if (remap.BaseName != null) // this will be null for the world matrix
				{
					//find the matching register in the technique
					string matrixType;
					string matrixExpansion = GetMatrixRankExpansion(remap.Name, asmTechnique, out matrixType);

					if (extension == ShaderExtension.Instancing)
						matrixGen.AppendFormat("\tfloat4x4 {0} = float4x4(({4})mul({1},{2}){3});", remap.RemapTo, worldMatrixName, remap.BaseName, matrixExpansion, matrixType);
					if (extension == ShaderExtension.Blending)
						matrixGen.AppendFormat("\tfloat4x4 {0} = float4x4(({4})mul({1},{2}){3});", remap.RemapTo, worldMatrixName, remap.Name, matrixExpansion, matrixType);
					matrixGen.AppendLine();
				}
			}


			//create the method signatures
			footer.AppendLine();
			bool multiArg = false;
			StringBuilder argList = new StringBuilder();
			int argIndex = -1;
			bool transposeWorldMatrix = false;

			foreach (var element in vs.HlslShader.Elements)
			{
				if (element.Statement == vs.Name)
				{
					footer.Append(vs.Name);
					footer.Append(nameAppend);
				}
				else
				{
					if (element.Statement == "(" || element.Statement == ",")
						argIndex = 0;
					else
					{
						argIndex++;
						if (element.Statement == "out" ||
							element.Statement == "in" ||
							element.Statement == "inout" ||
							element.Statement == "uniform" ||
							element.Statement == "const")
							argIndex--;
						if (argIndex != -1 && argIndex == 2)
						{
							if (argList.Length > 0)
								argList.Append(", ");
							argList.Append(element.Statement);
						}
					}

					if (element.Statement == ")")
					{
						argIndex = -1;

						if (methodNames.TryGetValue(vs.Name, out multiArg) && multiArg)
							footer.Append(", ");

						if (extension == ShaderExtension.Instancing)
						{
							footer.Append("float4x4 " + worldMatrixName + "_transpose : POSITION12");
							transposeWorldMatrix = true;
						}
						if (extension == ShaderExtension.Blending)
						{
							footer.AppendFormat("float4 {0} : BLENDWEIGHT, int4 {1} : BLENDINDICES", blendName, indicesName);
						}
					}

					footer.Append(element);
					footer.Append(' ');
				}
			}
			footer.AppendLine();
			footer.AppendLine("{");

			if (transposeWorldMatrix)
			{
				footer.AppendLine("\tfloat4x4 " + worldMatrixName + " = transpose(" + worldMatrixName + "_transpose);");
			}

			if (extension == ShaderExtension.Blending)
			{
				string calculation =
@"	float4x4 {3}
		= transpose(float4x4(
			{0}[{1}.x * 3 + 0] * {2}.x + {0}[{1}.y * 3 + 0] * {2}.y + {0}[{1}.z * 3 + 0] * {2}.z + {0}[{1}.w * 3 + 0] * {2}.w,
			{0}[{1}.x * 3 + 1] * {2}.x + {0}[{1}.y * 3 + 1] * {2}.y + {0}[{1}.z * 3 + 1] * {2}.z + {0}[{1}.w * 3 + 1] * {2}.w,
			{0}[{1}.x * 3 + 2] * {2}.x + {0}[{1}.y * 3 + 2] * {2}.y + {0}[{1}.z * 3 + 2] * {2}.z + {0}[{1}.w * 3 + 2] * {2}.w,
			float4(0,0,0,1)));";

				footer.AppendFormat(calculation, blendMatricesName, indicesName, blendName, worldMatrixName);
				footer.AppendLine();
			}
			
			footer.Append(matrixGen);
			
			footer.AppendFormat("\t{4}{0}({1}{2}{3});", ToInternalMethodName(vs.Name, extension), methodCallAppend, multiArg ? ", " : "", argList, vs.HasReturnValue ? "return " : "");

			footer.AppendLine();
			footer.AppendLine("}");

			return vs.Name + nameAppend;
		}
	}

	class TechniqueRemover : HlslStructure.ITokenTranslator
	{
		private readonly HashSet<HlslStructure> techniques;
		private readonly StringBuilder result;

		public static string Process(SourceShader shader)
		{
			return new TechniqueRemover(shader).result.ToString();
		}

		private TechniqueRemover(SourceShader shader)
		{
			techniques = new HashSet<HlslStructure>();
			foreach (var tech in shader.GetAllTechniques())
				techniques.Add(tech.HlslShader);

			result = new StringBuilder();
			shader.HlslShader.ToString(result, 0, this, null, false, true);
		}

		public string Translate(string token, bool isInBody, out bool replaceBracketWithParenthesis)
		{
			replaceBracketWithParenthesis = false;
			return token;
		}

		public bool IncludeBlock(HlslStructure block, int depth)
		{
			return !techniques.Contains(block);
		}
	}
}
