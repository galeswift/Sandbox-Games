using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xen.Graphics.ShaderSystem.CustomTool;

namespace CmdXenFX
{
	class Program
	{
		private static void Main(string[] args)
		{
			if (args.Length != 3)
			{
				Console.Error.WriteLine("Usage: cmdxenfx inputFile outputNamespace outputFile");
				return;
			}

			string source = args[0];
			string dest = args[2];

			if (!File.Exists(source))
			{
				Console.Error.WriteLine(@"FileNotFound: '{0}'", source);
				return;
			}

			if (File.Exists(dest))
			{
				if (File.GetLastWriteTime(dest) > File.GetLastWriteTime(source) &&
					File.GetLastWriteTime(dest) > File.GetLastWriteTime(typeof(ShaderCodeGenerator).Assembly.Location))
				{
					Console.WriteLine("Skipping... (File has not changed)");
					return;
				}
			}

			byte[] code = new ShaderCodeGenerator().GenerateCode(Path.GetFullPath(args[0]), File.ReadAllText(args[0]), args[1], new Microsoft.CSharp.CSharpCodeProvider());

			using (Stream file = File.Create(args[2]))
				file.Write(code, 0, code.Length);
		}
	}
}