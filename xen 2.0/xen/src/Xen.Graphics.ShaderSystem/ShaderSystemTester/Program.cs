using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xen.Graphics.ShaderSystem.CustomTool;
using Microsoft.CSharp;

namespace ShaderSystemTester
{
	class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			ShaderCodeGenerator codeGenerator = new ShaderCodeGenerator();

			codeGenerator.GenerateDebug(System.IO.File.ReadAllText("test.fx"), "test.fx");
		}
	}
}
