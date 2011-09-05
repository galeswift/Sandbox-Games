using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Xen.Graphics.ShaderSystem.CustomTool.FX
{
	//this is tricky...
	//Pull a modified Technique out of the shader, using only single constant buffers
	public static class TechniqueExtractor
	{
		public static byte[] Generate(SourceShader source, HlslTechnique technique, Platform platform)
		{

		//	ShaderExtensionGenerator exGen = new ShaderExtensionGenerator(source, platform);

			TokenRename vsRename, psRename;
			TokenRename vsBlendRename, vsInstanceRename;

			vsRename = new TokenRename(source, platform, "VS");
			psRename = new TokenRename(source, platform, "PS");

			vsBlendRename = new TokenRename(source, platform, "VS_B");
			vsInstanceRename = new TokenRename(source, platform, "VS_I");

			HlslMethod methodVS = source.GetMethod(technique.VertexShaderMethodName, platform);
			HlslMethod methodPS = source.GetMethod(technique.PixelShaderMethodName, platform);
			
			HlslMethod methodBlending = source.GetMethod(technique.BlendingShaderMethodName, platform);
			HlslMethod methodInstancing = source.GetMethod(technique.InstancingShaderMethodName, platform);

			if (methodPS == null || methodVS == null)
				return null;

			string vsPrefix, psPrefix;

			AsmTechnique asmTechnique = source.GetAsmTechnique(technique.Name, platform);
			vsPrefix = BuildMapping(asmTechnique, "vs", asmTechnique.VertexShader.RegisterSet);
			psPrefix = BuildMapping(asmTechnique, "ps", asmTechnique.PixelShader.RegisterSet);

			string blendingPrefix = null, instancingPrefix = null;
			if (asmTechnique.BlendingShader != null)
				blendingPrefix = BuildMapping(asmTechnique, "vsb", asmTechnique.BlendingShader.RegisterSet);
			if (asmTechnique.InstancingShader != null)
				instancingPrefix = BuildMapping(asmTechnique, "vsi", asmTechnique.InstancingShader.RegisterSet);

			//build the method bodies for the technique

			//process through all the methods used.
			vsRename.methodList.Add(methodVS);
			psRename.methodList.Add(methodPS);

			if (methodBlending != null) vsBlendRename.methodList.Add(methodBlending);
			if (methodInstancing != null) vsInstanceRename.methodList.Add(methodInstancing);

			//setup remapping for registers used
			MapRegistersToRenamer(vsRename, asmTechnique.VertexShader.RegisterSet, "vs");
			MapRegistersToRenamer(psRename, asmTechnique.PixelShader.RegisterSet, "ps");

			if (asmTechnique.BlendingShader != null)
			{
				MapRegistersToRenamer(vsBlendRename, asmTechnique.VertexShader.RegisterSet, "vs");
				MapRegistersToRenamer(vsBlendRename, asmTechnique.BlendingShader.RegisterSet, "vsb");
			}
			if (asmTechnique.InstancingShader != null)
			{
				MapRegistersToRenamer(vsInstanceRename, asmTechnique.VertexShader.RegisterSet, "vs");
				MapRegistersToRenamer(vsInstanceRename, asmTechnique.InstancingShader.RegisterSet, "vsi");
			}

			//finally, parse all the registers used by the Effect. Dump them in as 'unused'
			RegisterSet decompiledEffectRegisters = source.GetDecompiledEffect(platform).EffectRegisters;

			string unusedSamplerName = "__unused_sampler";
			string unusedFloatName = "__unused_";
			string unusedMatrixName = "__unused4x4_";
			string unusedArrayName = "__unused_array";

			foreach (Register reg in decompiledEffectRegisters)
			{
				if (reg.Name == null || reg.Category == RegisterCategory.Texture)
					continue;
				if (reg.Category == RegisterCategory.Sampler)
				{
					vsRename.unusedRemapping.Add(reg.Name, unusedSamplerName);
					psRename.unusedRemapping.Add(reg.Name, unusedSamplerName);
					vsBlendRename.unusedRemapping.Add(reg.Name, unusedSamplerName);
					vsInstanceRename.unusedRemapping.Add(reg.Name, unusedSamplerName);
				}
				else
				{
					vsRename.unusedRemapping.Add(reg.Name, reg.ArraySize > 0 ? unusedArrayName : (reg.Size > 1 ? unusedMatrixName : unusedFloatName));
					psRename.unusedRemapping.Add(reg.Name, reg.ArraySize > 0 ? unusedArrayName : (reg.Size > 1 ? unusedMatrixName : unusedFloatName));
					vsBlendRename.unusedRemapping.Add(reg.Name, reg.ArraySize > 0 ? unusedArrayName : (reg.Size > 1 ? unusedMatrixName : unusedFloatName));
					vsInstanceRename.unusedRemapping.Add(reg.Name, reg.ArraySize > 0 ? unusedArrayName : (reg.Size > 1 ? unusedMatrixName : unusedFloatName));
				}
			}
			//will write booleans down if the values are used.
			vsRename.unusedAccessed = new Dictionary<string, bool>();
			psRename.unusedAccessed = vsRename.unusedAccessed;
			vsBlendRename.unusedAccessed = vsRename.unusedAccessed;
			vsInstanceRename.unusedAccessed = vsRename.unusedAccessed;


			//note, the method list will be added to as new methods are needed by the base method.
			StringBuilder sb = new StringBuilder();

			//extract PS methods
			ExtractMethodBodies(sb, psRename);

			//extract VS methods
			ExtractMethodBodies(sb, vsRename);

			ExtractMethodBodies(sb, vsBlendRename);
			ExtractMethodBodies(sb, vsInstanceRename);

			string techniqueVsName = GetTechniqueInvoke(vsRename, sb, methodVS, technique.VertexShaderArgs);
			string techniquePsName = GetTechniqueInvoke(psRename, sb, methodPS, technique.PixelShaderArgs);

			string techniqueBlendingName = null, techniqueInstancingName = null;

			if (asmTechnique.BlendingShader != null)
				techniqueBlendingName = GetTechniqueInvoke(vsBlendRename, sb, methodBlending, technique.BlendingShaderArgs);
			if (asmTechnique.InstancingShader != null)
				techniqueInstancingName = GetTechniqueInvoke(vsInstanceRename, sb, methodInstancing, technique.InstancingShaderArgs);

			StringBuilder constants = new StringBuilder();

			//work out if any unused values were touched...
			//if so, add them.
			bool addUnusedSampler = false;
			bool addUnusedFloat = false;

			foreach (KeyValuePair<string,bool> item in vsRename.unusedAccessed)
			{
				if (item.Value)
				{
					//something has been used unexpectadly.
					Register reg;
					if (decompiledEffectRegisters.TryGetRegister(item.Key,out reg))
					{
						if (reg.Category == RegisterCategory.Sampler)
							addUnusedSampler = true;
						else
							addUnusedFloat = true;
					}
				}
			}
			if (addUnusedSampler)
				constants.AppendLine("sampler " + unusedSamplerName + ";");
			if (addUnusedFloat)
			{
				constants.AppendLine("const static float4 " + unusedFloatName + " = 0;");
				constants.AppendLine("const static float4x4 " + unusedMatrixName + " = 0;");
				constants.AppendLine("#define " + unusedArrayName + "(__INDEX__) " + unusedFloatName);
			}


			//setup the VS/PS constants
			asmTechnique.ConstructTechniqueConstants(constants, true);

			constants.Append(vsPrefix);
			constants.Append(psPrefix);
			if (blendingPrefix != null) constants.Append(blendingPrefix);
			if (instancingPrefix != null) constants.Append(instancingPrefix);

			constants.AppendLine(ExtractFixedDeclarations(source.HlslShader));

			//finally, build the technique delcaration
			string techniqueStructure =
@"{2}
{3}
technique Shader
{0}
	pass
	{0}
		VertexShader = compile {4} {5};
		PixelShader  = compile {6} {7};
	{1}
{8}{9}{1}";

			string extensionPass =
@"	pass {2}
	{0}
		VertexShader = compile {3} {4};
		PixelShader  = compile {5} {6};
	{1}
";
			string blendExtension = "", instanceExtension = "";

			if (asmTechnique.BlendingShader != null)
			{
				blendExtension = string.Format(extensionPass, "{", "}", "Blending", technique.GetVertexShaderVersion(platform), techniqueBlendingName, technique.GetPixelShaderVersion(platform), techniquePsName);
			}
			if (asmTechnique.InstancingShader != null)
			{
				instanceExtension = string.Format(extensionPass, "{", "}", "Instancing", "vs_3_0", techniqueInstancingName, "ps_3_0", techniquePsName);
			}

			string techniqueSource = string.Format(techniqueStructure, "{", "}", constants, sb,
				technique.GetVertexShaderVersion(platform), techniqueVsName,
				technique.GetPixelShaderVersion(platform), techniquePsName,
				blendExtension,instanceExtension);

			var types = typeof(EffectCompiler).Assembly.GetType("Xen.Graphics.ShaderSystem.EffectCompiler");
			var method = types.GetMethod("CompileEffect");

			CompileEffect effect = Delegate.CreateDelegate(typeof(CompileEffect), types.GetMethod("CompileEffect")) as CompileEffect;

			string compileErrors;
			byte[] compiledEffect = EffectCompiler.CompileEffect(techniqueSource, source.FileName, platform == Platform.Xbox, out compileErrors);

			if (compileErrors != null)
			{
				string[] errors = SanitizeConversionErrorMessage(compileErrors, techniqueSource, source.ShaderSource, source);

				if (source.ManualExtensions == false)
				{
					string errorsLine = "";
					foreach (var error in errors)
						errorsLine += error + Environment.NewLine;

					//error occured in generated code!
					ShaderExtensionGenerator.ThrowGeneratorError(errorsLine.Trim(), source.FileName);
				}
				else
				{
					string header = "XenFX Platform Technique Extraction Failed:";

					if (errors == null)
						Common.ThrowError(header, compileErrors);
					else
						Common.ThrowError(header, errors);
				}
			}

			return compiledEffect;
		}
		delegate byte[] CompileEffect(string source, string filename, bool xbox, out string errors);

		private static void ExtractMethodBodies(StringBuilder sb, TokenRename rename)
		{
			StringBuilder methodBody = new StringBuilder();
			for (int i = 0; i < rename.methodList.Count; i++)
			{
				ExtractMethodBody(rename, methodBody, rename.methodList[i], null);
				sb.Insert(0, methodBody);
				methodBody.Length = 0;
			}
		}

		private static void MapRegistersToRenamer(TokenRename rename, RegisterSet set, string extension)
		{
			foreach (Register reg in set)
			{
				if (reg.Category == RegisterCategory.Sampler)
					rename.samplerRemapping.Add(reg.Name, string.Format("_" + extension + "_s{0}", reg.Index));
				else
				{
					if (reg.ArraySize > 0)
						rename.arrayRemapping.Add(reg.Name, TranslateRegisterName(reg.Name, extension));
					else
						rename.registerRemapping.Add(reg.Name, TranslateRegisterName(reg.Name, extension));
				}
			}
		}

		private static string ExtractFixedDeclarations(HlslStructure shader)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < shader.Children.Length; i++)
			{
				if (shader.Children[i].Elements.Length > 0 &&
					(shader.Children[i].Elements[0] == "struct" ||
					shader.Children[i].Elements[0] == "static"))
				{
					sb.Append(shader.Children[i].ToString());
					sb.AppendLine(";");
				}
			}
			return sb.ToString();
		}

		private static void MapToValue(StringBuilder sb, Register reg, int index, string source, string arrayOffset)
		{
			if (reg.Category == RegisterCategory.Boolean)
				reg.Rank = RegisterRank.Bool;

			switch (reg.Rank)
			{
				case RegisterRank.Unknown:
					throw new ArgumentException("Unknown Register Type");

				case RegisterRank.Bool:
				case RegisterRank.FloatNx1:
				case RegisterRank.IntNx1:
					{
						sb.Append(source);
						sb.Append('[');
						sb.Append(arrayOffset);
						sb.Append("+");
						sb.Append(index++);
						sb.Append(']');

						switch (reg.Type)
						{
							case "float":
							case "bool":
							case "int":
								sb.Append(".x");
								break;
							case "int2":
							case "float2":
								sb.Append(".xy");
								break;
							case "int3":
							case "float3":
								sb.Append(".xyz");
								break;
						}
					}
					break;
				default:
					{
						sb.Append("transpose(");
						int count = 0;
						switch (reg.Rank)
						{
							case RegisterRank.FloatNx2:
							case RegisterRank.IntNx2:
								sb.Append("float2x4(");
								count = 2;
								break;
							case RegisterRank.FloatNx3:
							case RegisterRank.IntNx3:
								sb.Append("float3x4(");
								count = 3;
								break;
							case RegisterRank.FloatNx4:
							case RegisterRank.IntNx4:
								sb.Append("float4x4(");
								count = 4;
								break;
							default:
								return; //shouldn't be possible.
						}
						for (int i = 0; i < count; i++)
						{
							if (i != 0) sb.Append(',');
							sb.Append(source);
							sb.Append('[');
							if (arrayOffset.Length > 0)
							{
								sb.Append("((int)(");
								sb.Append(arrayOffset);
								sb.Append("))*");
								sb.Append(count);
							}
							sb.Append("+");
							sb.Append(index++);
							sb.Append(']');
						}
						sb.Append("))");
					}
					break;
			}
		}

		private static string BuildMapping(AsmTechnique asmTechnique, string extension, RegisterSet set)
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < set.RegisterCount; i++)
			{
				Register reg = set.GetRegister(i);
				string type = reg.Type;

				string source = "_" + extension + "_";

				switch (reg.Category)
				{
					case RegisterCategory.Boolean:
						source += "b";
						break;
					case RegisterCategory.Float4:
						source += "c";
						switch (reg.Rank)
						{
							case RegisterRank.FloatNx2:
							case RegisterRank.IntNx2:
								type = "float4x2";
								break;
							case RegisterRank.FloatNx3:
							case RegisterRank.IntNx3:
								type = "float4x3";
								break;
							case RegisterRank.FloatNx4:
							case RegisterRank.IntNx4:
								type = "float4x4";
								break;
						}
						break;
					default:
						continue;
				}


				sb.Append("#define ");
				sb.Append(TranslateRegisterName(reg.Name, extension));
				string index = "";
				if (reg.ArraySize > 0)
				{
					index = "(__INDEX__)";
					sb.Append(index);
				}

				sb.Append(" (");

				MapToValue(sb, reg, reg.Index, source, index);

				sb.AppendLine(")");
			}

			return sb.ToString();
		}


		private static string TranslateRegisterName(string name, string extension)
		{
			return "__" + name + "_" + extension + "reg";
		}


		class TokenRename : HlslStructure.ITokenTranslator
		{
			public TokenRename(SourceShader shader, Platform platform, string extension)
			{
				this.shader = shader;
				this.platform = platform;
				this.extension = extension;
			}

			public SourceShader shader;
			public string extension;
			public Platform platform;
			public readonly List<HlslMethod> methodList = new List<HlslMethod>();
			public readonly Dictionary<string, string> samplerRemapping = new Dictionary<string, string>();
			public readonly Dictionary<string, string> registerRemapping = new Dictionary<string, string>();
			public readonly Dictionary<string, string> arrayRemapping = new Dictionary<string, string>();
			public readonly Dictionary<string, string> unusedRemapping = new Dictionary<string, string>();
			public Dictionary<string, bool> unusedAccessed;

			string HlslStructure.ITokenTranslator.Translate(string token, bool isInBody, out bool replaceBracketWithParenthesis)
			{
				replaceBracketWithParenthesis = false;
				HlslMethod method = shader.GetMethod(token, platform);
				if (method != null)
				{
					if (isInBody)
					{
						if (methodList.Contains(method) == false)
							methodList.Add(method);
					}
					return TranslateMethodName(token);
				}
				string value;
				if (samplerRemapping.TryGetValue(token, out value))
					return value;
				if (registerRemapping.TryGetValue(token, out value))
					return value;
				if (arrayRemapping.TryGetValue(token, out value))
				{
					replaceBracketWithParenthesis = true;
					return value;
				}
				if (isInBody && unusedRemapping.TryGetValue(token, out value))
				{
					unusedAccessed[token] = true;
					replaceBracketWithParenthesis = true;
					return value;
				}
				return token;
			}

			public string TranslateMethodName(string name)
			{
				return "_" + name + "__" + extension;
			}

			bool HlslStructure.ITokenTranslator.IncludeBlock(HlslStructure block, int depth)
			{
				return true;
			}
		}

		private static void ExtractMethodBody(TokenRename rename, StringBuilder sb, HlslMethod method, string prefix)
		{
			method.HlslShader.ToString(sb, 0, rename, prefix, true, false);
		}

		private static string GetTechniqueInvoke(TokenRename rename, StringBuilder sb, HlslMethod method, IEnumerator<string> argsEnum)
		{
			string methodCall = rename.TranslateMethodName(method.Name);

			StringBuilder call = new StringBuilder();

			call.Append(methodCall);
			call.Append("(");

			while (argsEnum.MoveNext())
			{
				call.Append(argsEnum.Current);
			}
			call.Append(")");

			return call.ToString();
		}





		private static string[] SanitizeConversionErrorMessage(string errors, string source, string original, SourceShader shader)
		{
			CompileException compileException = null;
			try
			{
				List<string> errorList = new List<string>();
				string[] lines = errors.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
				StringBuilder output = new StringBuilder(errors.Length);

				string[] sourceLines = source.Split('\n');
				string[] originalLines = original.Split('\n');

				foreach (string line in lines)
				{
					if (line.Length > 1 && line[0] == '(')
					{
						//write the previous error out, if it exists
						if (output.Length > 0)
						{
							errorList.Add(output.ToString());
							output.Length = 0;
						}

						//ok, this might be a line error.
						string number = "";
						string lineNumberStr = null;
						for (int i = 1; i < line.Length - 1; i++)
						{
							if (line[i] == ')')
							{
								if (line[i + 1] != ':')
									number = "";// invalid
								break;
							}
							if (char.IsNumber(line[i]))
								number += line[i];
							else
							{
								if (line[i] == ',')
								{
									lineNumberStr = number;
									number = "";
								}
								else
								{
									number = ""; // invalid
									break;
								}
							}
						}
						if (lineNumberStr == null)
						{
							lineNumberStr = number;
							number = "0";
						}

						int lineNumber;
						if (lineNumberStr.Length > 0 && int.TryParse(lineNumberStr, out lineNumber) && lineNumber > 0 && lineNumber <= sourceLines.Length)
						{
							//find an error line reindex,
							int remapLine = -1;
							int reps = 4;	//look back at most 4 lines
							while (lineNumber-- > 0 && remapLine == -1 && reps-- > 0)
							{
								string errorLine = sourceLines[lineNumber];

								//find any '/*NUMBER*/' blocks, which remap to source line
								for (int i = 0; i < errorLine.Length - 1; i++)
								{
									if (errorLine[i] == '/' && errorLine[i + 1] == '*')
									{
										//this could be it.
										string remapLineStr = "";
										for (int n = i + 2; n < errorLine.Length - 2; n++)
										{
											if (char.IsNumber(errorLine[n]))
												remapLineStr += errorLine[n];
											else
											{
												if (!(errorLine[n] == '*' && errorLine[n + 1] == '/'))
													remapLineStr = "";	//invalid
												break;
											}
										}
										int lineIdx;
										if (remapLineStr.Length > 0 && int.TryParse(remapLineStr, out lineIdx))
											remapLine = lineIdx;
									}
									if (remapLine != -1)
										break;
								}
							}

							//write the new line index at the start of the line
							output.Append(string.Format("({0},", remapLine + 1));
						}
						else
							output.Append("(0,");

						output.AppendLine(line.Substring(number.Length + 3));
					}
					else
						output.Append(line);
				}

				//write the previous error out, if it exists
				if (output.Length > 0)
					errorList.Add(output.ToString());

				return errorList.ToArray();
			}
			catch (CompileException ex)
			{
				compileException = ex;
			}
			catch
			{
				return null;
			}

			if (compileException != null)
				throw compileException;
			return null;
		}
	}
}
