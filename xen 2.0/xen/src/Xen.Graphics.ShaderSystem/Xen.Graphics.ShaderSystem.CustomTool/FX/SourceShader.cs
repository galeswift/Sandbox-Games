
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Xen.Graphics.ShaderSystem.CustomTool.FX
{

	//stores an FX file, and extracts user options
	public sealed class SourceShader
	{
		private readonly string shaderSource;
		private readonly string[] sourceLines;
	
		private readonly bool mixedMode;
		private readonly bool debugHlslProcessXboxShader;
		private readonly bool internalClass;
		private readonly bool useParentNamespace;
		private readonly bool skipConstantValidation;
		private readonly bool exposeRegisters;
		private readonly bool manualExtensions;

		private int generatedLineStart = -1;
		private readonly HlslStructure hlslShader;
		private readonly string filename;
		private readonly List<SourceShader> includedSource;

		private readonly List<HlslTechnique> techniques;
		private readonly List<HlslMethod> methods;

		private readonly List<AsmTechnique> asmTechniques, xboxAsmTechniques;
		private DecompiledEffect decompiledEffectPC, decompiledEffectXbox;


		public static SourceShader Create(string shaderSource, string filename)
		{
			SourceShader ss = null;
			string dir = Directory.GetCurrentDirectory();
			try
			{
				Directory.SetCurrentDirectory(new FileInfo(filename).Directory.FullName);

				ss = new SourceShader(shaderSource, filename, null);
				ss.ExtractAsmTechniques(null);

				if (!ss.manualExtensions)
				{
					try
					{
						int line;
						shaderSource = ShaderExtensionGenerator.GenerateShaderCode(ss, "", ss.mixedMode, out line);
						ss = new SourceShader(shaderSource, filename, null);
						ss.generatedLineStart = line;
						ss.ExtractAsmTechniques(null);
					}
					catch (Exception ex)
					{
						ShaderExtensionGenerator.ThrowGeneratorError(ex.ToString(), filename);
					}
				}
			}
			finally
			{
				Directory.SetCurrentDirectory(dir);
			}
			return ss;
		}

		private SourceShader(string shaderSource, string filename, string generatedAppend)
		{
			this.sourceLines = shaderSource.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

			if (sourceLines.Length > 0)
			{
				ComputeCompilerOps(sourceLines[0], out internalClass, out useParentNamespace, out mixedMode, out debugHlslProcessXboxShader, out skipConstantValidation, out exposeRegisters, out manualExtensions);
			}

			this.shaderSource = shaderSource;
			this.filename = filename;

			this.techniques = new List<HlslTechnique>();
			this.methods = new List<HlslMethod>();
			this.hlslShader = new HlslStructure(shaderSource);
			this.includedSource = new List<SourceShader>();
		
			this.ExtractIncludeSource();
			this.ExtractMethods(generatedAppend);

			asmTechniques = new List<AsmTechnique>();
			if (!mixedMode) xboxAsmTechniques = new List<AsmTechnique>();
		}

		public DecompiledEffect GetDecompiledEffect(Platform platform)
		{
			switch (platform)
			{
				case Platform.Xbox:
					return decompiledEffectXbox;
				default:
					return decompiledEffectPC;
			}
		}

		private void ExtractAsmTechniques(string generatedPrefix)
		{
			if (!mixedMode)
			{
				//both platforms compile their own effects

				//pull the actual asm data out..
				this.asmTechniques.AddRange(AsmTechnique.ExtractTechniques(this, Platform.Windows, out decompiledEffectPC, generatedPrefix));
				this.xboxAsmTechniques.AddRange(AsmTechnique.ExtractTechniques(this, Platform.Xbox, out decompiledEffectXbox, generatedPrefix));
			}
			else
			{
				//one set of techniques for both windows and xbox

				//pull the actual asm data out..
				this.asmTechniques.AddRange(AsmTechnique.ExtractTechniques(this, Platform.Both, out decompiledEffectPC, generatedPrefix));
				decompiledEffectXbox = decompiledEffectPC;
			}
		}

		private void ExtractIncludeSource()
		{
			if (filename != null)
			{
				//parse the hs, look for #include's in the root level only

				foreach (HlslStructure hs in this.hlslShader.Children)
				{
					if (hs.Elements.Length > 4)
					{
						if (hs.Elements[0] == "#" &&
							hs.Elements[1] == "include")
						{
							bool global = hs.Elements[2] == "<";
							StringBuilder file = new StringBuilder();

							for (int i = 3; i < hs.Elements.Length-1; i++)
							{
								if ((global && hs.Elements[i] == ">") || (!global && hs.Elements[i] == "\""))
									break;

								file.Append(hs.Elements[i]);
							}

							string includeName = file.ToString();

							//find the file
							string path = Path.IsPathRooted(file.ToString()) ? file.ToString() : Path.Combine(Path.GetDirectoryName(filename), file.ToString());

							if (File.Exists(path))
							{
								//load the file and parse it as well
								SourceShader include = new SourceShader(File.ReadAllText(path), path, null);
								includedSource.Add(include);
							}
						}
					}
				}
			}
		}


		private void ExtractMethods(string generatedPrefix)
		{
			string str = this.hlslShader.ToString();

			List<Platform> platformStack = new List<Platform>();

			//pull out methods and techniques, but only from the root level...
			foreach (HlslStructure hs in this.hlslShader.Children)
			{
				//could be an #if, #else, #etc
				if (hs.Children.Length == 0 &&
					hs.Elements.Length > 1 &&
					hs.Elements[0].Statement.Length == 1 &&
					hs.Elements[0].Statement[0] == '#')
				{
					//need to account for #if XBOX360 blocks
					switch (hs.Elements[1].Statement)
					{
						case "if":
						case "ifdef":
							if (hs.Elements.Length > 2)
							{
								if (hs.Elements[2] == "XBOX360" ||
									hs.Elements[2] == "XBOX")
									platformStack.Add(Platform.Xbox);
								else
								if (hs.Elements[2] == "!XBOX360" ||
									hs.Elements[2] == "!XBOX")
									platformStack.Add(Platform.Windows);
								else
									platformStack.Add(Platform.Both);
							}
							break;

						case "ifndef":
							if (hs.Elements.Length > 2)
							{
								if (hs.Elements[2] == "XBOX360" ||
									hs.Elements[2] == "XBOX")
									platformStack.Add(Platform.Windows);
								else
								if (hs.Elements[2] == "!XBOX360" ||
									hs.Elements[2] == "!XBOX")
									platformStack.Add(Platform.Xbox);
								else
									platformStack.Add(Platform.Both);
							}
							break;

						case "else":
							if (platformStack.Count > 0)
							{
								Platform peek = platformStack[platformStack.Count - 1];
								platformStack.RemoveAt(platformStack.Count - 1);

								if (peek == Platform.Xbox)
									platformStack.Add(Platform.Windows);
								if (peek == Platform.Windows)
									platformStack.Add(Platform.Xbox);
							}
							break;
						case "endif":
							if (platformStack.Count > 0)
								platformStack.RemoveAt(platformStack.Count - 1);
							break;
					}
				}


				if (hs.BraceEnclosedChildren) 
				{
					if (hs.Elements.Length > 0 && hs.Elements[0].Statement.Equals("technique", StringComparison.InvariantCultureIgnoreCase))
					{
						//figure out the platform, based on #if blocks stack.
						Platform platform = Platform.Both;
						for (int i = 0; i < platformStack.Count; i++)
							platform &= platformStack[i];

						var technique = new HlslTechnique(hs, platform, generatedPrefix);
						if (technique.IsGenerated || generatedPrefix == null)
							this.techniques.Add(technique);
					}

					//finding a method is a bit trickier

					if (hs.Elements.Length > 2)
					{
						//should have a (...) block in it to be a method...
						int openDepth = 0;
						for (int i = 2; i < hs.Elements.Length; i++)
						{
							if (hs.Elements[i] == "(")
								openDepth++;

							if (hs.Elements[i] == ")")
							{
								if (--openDepth == 0)
								{
									//figure out the platform, based on #if blocks stack.
									Platform platform = Platform.Both;
									for (int p = 0; p < platformStack.Count; p++)
										platform &= platformStack[p];

									//found the method.
									this.methods.Add(new HlslMethod(hs, platform));
									break;
								}
							}
						}
					}
				}
			}

			foreach (SourceShader child in this.includedSource)
				child.ExtractMethods(generatedPrefix);
		}

		public HlslMethod GetMethod(string name, Platform platform)
		{
			if (name == null)
				return null;
			for (int i = 0; i < this.methods.Count; i++)
			{
				if (this.methods[i].Name == name)
				{
					if ((this.methods[i].Platform & platform) == platform)
						return this.methods[i];
				}
			}
			for (int i = 0; i < this.includedSource.Count; i++)
			{
				HlslMethod method = this.includedSource[i].GetMethod(name, platform);
				if (method != null)
					return method;
			}
			return null;
		}


		public IEnumerable<HlslMethod> GetAllMethods(Platform platform, bool useIncludeFiles)
		{
			for (int i = 0; i < this.methods.Count; i++)
			{
				if ((this.methods[i].Platform & platform) == platform)
					yield return this.methods[i];
			}
			if (useIncludeFiles)
			{
				for (int i = 0; i < this.includedSource.Count; i++)
				{
					foreach (HlslMethod method in this.includedSource[i].GetAllMethods(platform, useIncludeFiles))
						yield return method;
				}
			}
		}

		public HlslTechnique GetTechnique(string name, Platform platform)
		{
			for (int i = 0; i < this.techniques.Count; i++)
			{
				if (this.techniques[i].Name == name &&
					(this.techniques[i].Platform & platform) == platform)
					return this.techniques[i];
			}
			return null;
		}
		public AsmTechnique GetAsmTechnique(string name, Platform platform)
		{
			List<AsmTechnique> techniques = this.asmTechniques;

			if (platform == Platform.Xbox)
				techniques = xboxAsmTechniques ?? techniques;

			for (int i = 0; i < techniques.Count; i++)
			{
				if (techniques[i].Name == name)
					return techniques[i];
			}
			return null;
		}


		//expose the data


		public string ShaderSource { get { return shaderSource; } }
		public string GetShaderLine(int line) { return sourceLines[line]; }
		public int ShaderLines { get { return sourceLines.Length; } }
		public string FileName { get { return filename; } }

		public HlslStructure HlslShader { get { return hlslShader; } }
		public int GeneratedCodeStartLine { get { return generatedLineStart; } }

		public bool DefinePlatform { get { return !mixedMode; } }
		public bool DebugHlslProcessXboxShader { get { return debugHlslProcessXboxShader; } }
		public bool GenerateInternalClass { get { return internalClass; } }
		public bool UseParentNamespace { get { return useParentNamespace; } }
		public bool SkipConstantValidation { get { return skipConstantValidation; } }
		public bool ExposeRegisters { get { return exposeRegisters; } }
		public bool ManualExtensions { get { return manualExtensions; } }

		public HlslTechnique[] GetAllTechniques() { return techniques.ToArray(); }


		#region data extraction

		private static void ComputeCompilerOps(string firstLine, out bool internalClass, out bool useParentNamespace, out bool mixedMode, out bool debugHlslProcessXboxShader, out bool skipConstantValidation, out bool exposeRegisters, out bool manualExtensions)
		{
			string cop = "compileroptions";

			/*
				looking for
			
			// CompilerOptions = ...

				on the firstLine line
			 */

			mixedMode = true;
			debugHlslProcessXboxShader = false;
			internalClass = false;
			useParentNamespace = false;
			skipConstantValidation = false;
			exposeRegisters = false;
			manualExtensions = false;

			if (firstLine.Length < cop.Length + 2)
				return;

			firstLine = firstLine.ToLower();

			//yes this could be done better...
			//remove white space and comment slashes
			firstLine = firstLine.Replace(" ", "").Replace("/", "").Replace("\t", "");

			if (firstLine.Length < cop.Length + 2)
				return;

			//first part needs to be compileroptions
			if (firstLine.Substring(0, cop.Length) != cop)
				return;

			//
			firstLine = firstLine.Substring(cop.Length);

			//then =
			if (firstLine.Length == 0)
				return;
			if (firstLine[0] != '=')
				return;

			//parse the options!

			string[] ops = firstLine.Substring(1).Split(',');

			foreach (string op in ops)
			{
				switch (op)
				{
					//case "avoidflowcontrol":
					//    options |= CompilerOptions.AvoidFlowControl;
					//    break;
					//case "partialprecision":
					//    options |= CompilerOptions.PartialPrecision;
					//    break;
					//case "preferflowcontrol":
					//    options |= CompilerOptions.PreferFlowControl;
					//    break;
					case "disablegenerateextensions":
						manualExtensions = true;
						break;
					case "internalclass":
						internalClass = true;
						break;
					case "parentnamespace":
						useParentNamespace = true;
						break;
					case "defineplatform":
						mixedMode = false;
						break;
					case "exposeregisters":
						exposeRegisters = true;
						break;
				}
			}
			return;
		}

		#endregion
	}
}
