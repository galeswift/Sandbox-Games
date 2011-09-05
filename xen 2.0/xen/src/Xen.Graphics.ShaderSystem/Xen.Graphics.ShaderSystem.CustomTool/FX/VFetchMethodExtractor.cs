using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Xen.Graphics.ShaderSystem.CustomTool.FX
{
	//this is tricky...
	public sealed class VFetchXboxMethodExtractor
	{
		private readonly SourceShader source;
		private readonly byte[] psCode, vsCode;

		public VFetchXboxMethodExtractor(SourceShader shader, HlslTechnique technique)
		{
			this.source = shader;

			HlslMethod method = shader.GetMethod(technique.VertexShaderMethodName, Platform.Xbox);
			if (method != null && method.UsesVFetch)
			{
				vsCode = ProcessMethod(method, technique.VertexShaderArgs, ShaderProfile.VS_3_0);
			}

			method = shader.GetMethod(technique.PixelShaderVersion, Platform.Xbox); //pixel shader using vfetch?!
			if (method != null && method.UsesVFetch)
			{
				psCode = ProcessMethod(method, technique.PixelShaderArgs, ShaderProfile.PS_3_0);
			}
		}

		public byte[] GetPixelShaderCode()
		{
			if (psCode != null)
				return (byte[])psCode.Clone();
			return null;
		}
		public byte[] GetVertexShaderCode()
		{
			if (vsCode != null)
				return (byte[])vsCode.Clone();
			return null;
		}

		private byte[] ProcessMethod(HlslMethod method, IEnumerator<string> argsEnum, ShaderProfile shaderProfile)
		{
			//this is where things get silly... :-)

			//the method uses vfetch, and for various reasons, the shader code cannot be extracted from an Effect
			//the shader also cannot be decompiled.
			//However, a single shader can be compiled from HLSL. Which is great.... except...
			//the technique may define the method to use, and may pass in arguements..
			// like so:
			//
			// VertexShader = compile vs_2_0 zomg(true);
			//
			//In such a case, the method 'zomg' can't be directly compiled using ShaderCompiler.
			//When this is the case, things get stupid.
			//
			//The way to compile the method is to create a stub that calls the real method,
			//with values for the arguements. Modifying the method declaration with default arguements
			//wouldn't always work...

			//this is, of course, assuming the method has uniform arguements being passed in.
			//if it isn't, then no problem.

			List<string> rawArgs = new List<string>();
			while (argsEnum.MoveNext())
				rawArgs.Add(argsEnum.Current);

			CompiledShader compiledShader;
			string workingDirectory = null;

			if (rawArgs.Count == 0)
			{
				//just compile the method directly (easy)...
				
				try
				{
					//set the working directory, since it's using CompileFromFile
					workingDirectory = Directory.GetCurrentDirectory();

					string path = Path.GetDirectoryName(source.FileName);
					if (path != null && path.Length > 0)
						Directory.SetCurrentDirectory(Path.GetDirectoryName(source.FileName));

					compiledShader =
						ShaderCompiler.CompileFromFile(
							Path.GetFileName(source.FileName),
							DecompiledEffect.XboxCompileMacros,
							new VFetchIncludeHandler(source.FileName, false),
							source.CompilerOptions,
							method.Name,
							shaderProfile,
							Microsoft.Xna.Framework.TargetPlatform.Xbox360);
				}
				finally
				{
					if (workingDirectory != null)
						Directory.SetCurrentDirectory(workingDirectory);
				}
			}
			else
			{
				//this gets much trickier..
				//have to extract the args, and importantly, put them in the right place


				/*
				 * 
				 * eg, a method and it's technique may look like this:
				 * 
				 * float4 Test(float4 pos : POSITION, uniform float scale) : POSITION
				 * {
				 *   ... vfetch ...
				 * }
				 * 
				 * technique
				 * {
				 *   VertexShader = compile vs_2_0 Test(5.0);
				 *   ...
				 * }
				 * 
				 * 
				 * The stub that is generated must pull the '5.0' from the technique
				 * and pass it into the real method, like so:
				 * 
				 * 
				 * float4 Test_STUB(float4 pos : POSITION) : POSITION
				 * {
				 *	 return Test(pos, 5.0);
				 * }
				 * 
				 * 
				 * Note: the uniform was removed from the stub input declaration
				 * 
				 */


				//the actual arg values (passed into the real method)
				List<string> args = new List<string>();
				StringBuilder arg = new StringBuilder();

				int depth = 0;
				//break the args list up
				for (int i = 0; i < rawArgs.Count; i++)
				{
					if (rawArgs[i].Length == 1 && (rawArgs[i][0] == '(' || rawArgs[i][0] == '{' || rawArgs[i][0] == '['))
						depth++;
					if (rawArgs[i].Length == 1 && (rawArgs[i][0] == ')' || rawArgs[i][0] == '}' || rawArgs[i][0] == ']'))
						depth--;

					if (depth == 0 && rawArgs[i] == ",")
					{
						args.Add(arg.ToString());
						arg.Length = 0;
					}
					else
					{
						arg.Append(rawArgs[i]);
						arg.Append(' ');
					}
				}
				args.Add(arg.ToString());


				//the input args that are being replaced must be declared as 'uniform'

				//parse the method declaration...

				depth = 0;
				HlslStructure hs = method.HlslShader;
				bool parsingArgs = false;
				int argIndex = 0;

				StringBuilder stubMethodDeclaration = new StringBuilder();
				StringBuilder stubMethodInvoke = new StringBuilder();
				int stubMethodInvokeCount = 0;

				//random name
				string stubName = "_STUB" + Guid.NewGuid().ToString("N");

				bool includingArg = true;
				int replacedArgIndex = 0;
				int parseArgIndex = 0;

				int parseArgNameIndex = 0;
				bool parseCheckForName = true;

				bool stubReturnsValue = false;

				for (int i = 0; i < hs.Elements.Length; i++)
				{
					if (hs.Elements[i].Length == 1 && (hs.Elements[i][0] == ')' || hs.Elements[i][0] == '}' || hs.Elements[i][0] == ']'))
						depth--;
					if (hs.Elements[i].Length == 1 && (hs.Elements[i][0] == '(' || hs.Elements[i][0] == '{' || hs.Elements[i][0] == '['))
					{
						depth++;

						if (depth == 1 && hs.Elements[i][0] == '(' && !parsingArgs)
						{
							//about to begin the method args
							parsingArgs = true;

							//append the stub name, so to not conflict with the original method
							stubMethodDeclaration.Append(stubName);
							stubMethodDeclaration.Append('(');
							continue;
						}
					}

					//actually parsing the args within the (,,,,,) block
					if (parsingArgs)
					{
						if (argIndex == 0)
						{
							//check for uniform.
							if (hs.Elements[i] == "uniform" && replacedArgIndex != args.Count)
							{
								//replace this arg if possible


								//add technique value to the invoke
								if (stubMethodInvokeCount != 0)
									stubMethodInvoke.Append(',');

								stubMethodInvoke.Append(args[replacedArgIndex]);
								stubMethodInvokeCount++;

								replacedArgIndex++;

								//remove the last written character (a ,) from the stub method,
								//but only if it's not the first parsed arg
								if (parseArgIndex != 0 && includingArg)
									stubMethodDeclaration.Length--;

								//skip it in the stub declaration
								includingArg = false;
							}
							else
								includingArg = true;
						}

						if (depth == 1 && (hs.Elements[i].Length == 1 && (hs.Elements[i][0] == ',' || hs.Elements[i][0] == ')')))
						{
							argIndex = 0;
							parseArgIndex++;

							//write the element name into the arg invoke list
							if (includingArg)
							{
								if (stubMethodInvokeCount > 0)
									stubMethodInvoke.Append(',');

								stubMethodInvoke.Append(hs.Elements[parseArgNameIndex]);
								stubMethodInvokeCount++;
							}

							parseCheckForName = true;
							parseArgNameIndex = 0;
						}
						else
						{
							if (includingArg)
							{
								//want to include the name of the arg in the invoke list.
								if (hs.Elements[i].Length == 1 && (hs.Elements[i][0] == ':' || hs.Elements[i][0] == '='))
								{
									//the arg is declared, now it's being given a default or semantic
									parseCheckForName = false;
								}

								if (parseCheckForName && depth == 1)
								{
									//last value written should be the name of the arg
									parseArgNameIndex = i;
								}
							}

							argIndex++;
						}
					}

					if (includingArg || depth == 0)
					{
						if (stubMethodDeclaration.Length > 0 && i > 0 && Tokenizer.IsIdentifierToken(hs.Elements[i]) && Tokenizer.IsIdentifierToken(hs.Elements[i - 1]))
							stubMethodDeclaration.Append(' ');
						stubMethodDeclaration.Append(hs.Elements[i]);

						if (depth == 0 && hs.Elements[i].Length == 1 && hs.Elements[i][0] == ':')
						{
							//method returns a value that is important somehow...
							//ie,
							//float4 Method() : POSITION

							stubReturnsValue = true;
						}
					}
				}

				//yikes.
				//at this point,
				//stubMethodDeclaration will have the declaration of the method, without the uniform parametres
				//stubMethodInvoke will have the list of arguements to pass into the real method, from the stub method.

				//so construct the full stub


				string fullStub = string.Format("{0}{1}{5}{1}\t{2}{3}({4});{1}{6}",
					stubMethodDeclaration,
					Environment.NewLine,
					stubReturnsValue ? "return " : "",
					method.Name,
					stubMethodInvoke,
					"{","}");

				//append it to the end of the real shader
				StringBuilder fullShader = new StringBuilder();

				fullShader.Append(source.ShaderSource);
				fullShader.AppendLine();
				fullShader.Append(fullStub);

				//now compile the bugger...

				try
				{
					//set the working directory, since it's using CompileFromSource
					workingDirectory = Directory.GetCurrentDirectory();

					string path = Path.GetDirectoryName(source.FileName);
					if (path != null && path.Length > 0)
						Directory.SetCurrentDirectory(Path.GetDirectoryName(source.FileName));

					//not all compiler options apply when compiling a single shader (instead of an effect)
					CompilerOptions options = source.CompilerOptions &
						(CompilerOptions.AvoidFlowControl | CompilerOptions.PreferFlowControl);

					//compile it... finally...
					compiledShader =
						ShaderCompiler.CompileFromSource(
							fullShader.ToString(),
							DecompiledEffect.XboxCompileMacros,
							new VFetchIncludeHandler(source.FileName, false),
							options,
							method.Name + stubName,
							shaderProfile,
							Microsoft.Xna.Framework.TargetPlatform.Xbox360);
				}
				finally
				{
					if (workingDirectory != null)
						Directory.SetCurrentDirectory(workingDirectory);
				}
			}

			if (!compiledShader.Success)
				Common.ThrowError(compiledShader.ErrorsAndWarnings, source.ShaderSource);

			return compiledShader.GetShaderCode();
		}
	}
}
