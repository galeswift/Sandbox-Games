using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xen.Graphics.ShaderSystem.CustomTool;
using System.CodeDom.Compiler;

namespace XenShaderUpdate
{
	public static class Program
	{
		//this program recurses through a directory structure, and tries to find
		//shaders that have been compiled using the custom tool plugin.
		//If the shaders are out of date, it will update them.

		private static string startDir;
		private static string filter;
		private static DateTime generateDate;
		private static ShaderCodeGenerator codeGenerator;
		private static CodeDomProvider codeProvider;
		private static bool forced;

		[STAThread]
		private static void Main(string[] args)
		{
			filter = "*.fx.cs";

			if (args.Length > 0)
				forced = args[0] == "forced";

			//the modified date of the generator
			string path = typeof(ShaderCodeGenerator).Assembly.Location;
			generateDate = File.GetLastWriteTime(path);

			startDir = Directory.GetCurrentDirectory();
			DirectoryInfo current = new DirectoryInfo(startDir);
			RecurseDirectories(current);

#if DEBUG
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
#endif
		}

		private static void RecurseDirectories(DirectoryInfo current)
		{
			//don't want to follow shortcuts, etc, so recurse manually
			FileInfo[] files = current.GetFiles(filter, SearchOption.TopDirectoryOnly);

			foreach (DirectoryInfo dir in current.GetDirectories())
				RecurseDirectories(dir);

			foreach (FileInfo file in files)
				ProcessFile(file);
		}

		private static void ProcessFile(FileInfo fileInfo)
		{
			//start reading the file. it should be in the following format:

			// XenFX
			// Assembly = Xen.Graphics.ShaderSystem.CustomTool, Version=6.0.0.1, Culture=neutral, PublicKeyToken=e706afd07878dfca
			// SourceFile = ...
			// Namespace = ...


			string expectedAssembly = "// Assembly = " + typeof(ShaderCodeGenerator).Assembly.FullName;

#if !DEBUG
			try
			{
#endif
				string sourceFile = null;
				string localNamespace = null;


				using (FileStream file = fileInfo.OpenRead())
				using (StreamReader reader = new StreamReader(file))
				{
					string xenHeader = reader.ReadLine();
					string asmHeader = reader.ReadLine();
					string srcHeader = reader.ReadLine();
					string namespaceHeader = reader.ReadLine();

					if (xenHeader != "// XenFX" ||
						asmHeader != expectedAssembly ||
						srcHeader == null ||
						namespaceHeader == null)
					{
						if (xenHeader == "// XenFX" && asmHeader != expectedAssembly)
							Console.Error.WriteLine(string.Format("Failed to read file, mismatched assembly: \"{0}\" ", Trim(fileInfo.FullName)));
						else
							Console.Error.WriteLine(string.Format("Failed to read file, Incorrect format: \"{0}\" ", Trim(fileInfo.FullName)));
						return;
					}

					if (srcHeader.StartsWith("// SourceFile = ") == false ||
						namespaceHeader.StartsWith("// Namespace = ") == false)
					{
						Console.Error.WriteLine(string.Format("Failed to read file, unexpected source or namespace declaration: \"{0}\" ", Trim(fileInfo.FullName)));
						return;
					}

					sourceFile = Path.Combine(fileInfo.Directory.FullName, srcHeader.Substring(16));
					localNamespace = namespaceHeader.Substring(15);

					if (File.Exists(sourceFile) == false)
					{
						Console.Error.WriteLine(string.Format("Failed to read shader source file: \"{0}\" for shader \"{1}\" ", Trim(sourceFile), Trim(fileInfo.FullName)));
						return;
					}
				}

				

				//right. Now, see if the file is even out of date!

				if (forced ||
					File.GetLastWriteTime(sourceFile) > fileInfo.LastWriteTime ||
					generateDate > fileInfo.LastWriteTime)
				{
					//great. need to update it!

					ConstructShader(fileInfo, sourceFile, localNamespace);
				}
#if !DEBUG
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(string.Format("Failed to process file: \"{0}\"", fileInfo.FullName));
				Console.Error.WriteLine(e.ToString());
			}
#endif

		}

		private static void ConstructShader(FileInfo fileInfo, string sourceFile, string localNamespace)
		{
			if (codeGenerator == null)
			{
				codeGenerator = new ShaderCodeGenerator();
				codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
			}

			byte[] shaderCode = codeGenerator.GenerateCode(sourceFile, File.ReadAllText(sourceFile), localNamespace, codeProvider);

			using (FileStream file = fileInfo.Create())
				file.Write(shaderCode, 0, shaderCode.Length);

			//done!
			Console.WriteLine(string.Format("Updated: \"{0}\"", Trim(fileInfo.FullName)));
		}

		private static string Trim(string path)
		{
			if (path.StartsWith(startDir))
				return path.Substring(startDir.Length);
			return path;
		}
	}
}
