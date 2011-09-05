using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.CodeDom;

namespace Xen.Graphics.ShaderSystem.CustomTool.Dom
{
	//this class simply deals with #if / #endif blocks in a shader
	//which is a bit more involved than might be expected, as CodeDom doesn't support them...
	public sealed class CompileDirectives
	{

		//default, for C#, etc.
		private readonly string[] directives = new string[]
		{
			"#if {0}",
			"#else",
			"#endif"
		};
		private readonly CodeSnippetTypeMember readonlySnip;
		private readonly bool isCSharp;
		private readonly CodeDomProvider codeProvider;

		public CodeSnippetTypeMember GetIfStatement(string conditionalName)
		{
			return new CodeSnippetTypeMember(string.Format(directives[0], conditionalName));
		}
		public CodeSnippetTypeMember IfXboxStatement
		{
			get { return new CodeSnippetTypeMember(string.Format(directives[0], "XBOX360")); }
		}
		public CodeSnippetTypeMember ElseStatement
		{
			get { return new CodeSnippetTypeMember(directives[1]); }
		}
		public CodeSnippetTypeMember EndifStatement
		{
			get { return new CodeSnippetTypeMember(directives[2]); }
		}
		public bool IsCSharp
		{
			get { return isCSharp; }
		}
		public CodeDomProvider CodeDomProvider
		{
			get { return codeProvider; }
		}

		//may be null
		public CodeTypeMember CreateReadOnlySnippet()
		{
			if (readonlySnip == null)
				return null;
			return new CodeSnippetTypeMember(readonlySnip.Text);
		}


		//construct, given the code provider being used.
		public CompileDirectives(CodeDomProvider codeProvider)
		{
			this.codeProvider = codeProvider;

			if (codeProvider.FileExtension == "cs")
			{
				readonlySnip = new CodeSnippetTypeMember("readonly ");
				isCSharp = true;
			}
			if (codeProvider.FileExtension == "vb")
				readonlySnip = new CodeSnippetTypeMember("ReadOnly ");


			//try and load the directives from the user config.
			try
			{
				//GenericType
				string defaultDiretive = Properties.Settings.Default.DefaultCompilerDirectives.Trim();
				if (defaultDiretive.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Length == 3)
					directives = defaultDiretive.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

				System.Reflection.PropertyInfo[] properties = Properties.Settings.Default.GetType().GetProperties();

				foreach (System.Reflection.PropertyInfo property in properties)
				{
					if (property.PropertyType == typeof(string) && property.Name.EndsWith("CompilerDirectives", StringComparison.InvariantCultureIgnoreCase))
					{
						string directive = property.GetValue(Properties.Settings.Default, null) as string;

						if (directive != null)
						{
							string[] lines = directive.Trim().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
							if (lines.Length == 4)
							{
								if (("*." + codeProvider.FileExtension).Equals(lines[0].Trim(), StringComparison.InvariantCultureIgnoreCase))
								{
									for (int i = 0; i < 3; i++)
									{
										directives[i] = lines[i + 1];
									}
								}
							}
						}
					}
				}

				for (int i = 0; i < directives.Length; i++)
				{
					directives[i] = directives[i].Trim();
				}

				if (directives[0].Contains("{0}") == false)
					throw new Exception("Could not find {0} in directive 0");

			}
			catch (Exception e)
			{
				throw new Exception("Error in user.config file", e);
			}
		}

	}
}
