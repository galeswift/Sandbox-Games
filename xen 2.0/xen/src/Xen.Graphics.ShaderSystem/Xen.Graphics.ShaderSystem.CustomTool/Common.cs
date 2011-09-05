using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xen.Graphics.ShaderSystem.CustomTool.FX;

namespace Xen.Graphics.ShaderSystem.CustomTool
{
	public static class Common
	{
		//converts the first char to upper
		public static string ToUpper(string name)
		{
			if (name.Length == 0 || char.IsUpper(name[0]))
				return name;
			char[] nameC = name.ToCharArray();
			nameC[0] = char.ToUpper(nameC[0]);
			return new string(nameC);
		}

		public static void ThrowError(string errorList)
		{

			ThrowError(null, errorList);
		}

		public static void ThrowError(string header, string errorList)
		{

			ThrowError(header, errorList.Split(new string[] { Environment.NewLine + "(" }, StringSplitOptions.RemoveEmptyEntries));
		}

		public static void ThrowError(string[] errors)
		{
			ThrowError(null, errors);
		}

		public static void ThrowError(string header, string[] errors)
		{
			//pull out the errors...
			List<CompileException> exceptions = new List<CompileException>();

			if (header != null)
				exceptions.Add(new CompileException(header));

			if (errors != null)
			{
				foreach (string err in errors)
				{
					string e = err;
					if (e == null)
						continue;

					string memPath = System.IO.Directory.GetCurrentDirectory() + "\\memory";
					e = e.Replace(memPath, "");

					string error = e.Trim();

					string lineS = null, colS = "";
					string str = "";
					if (error.Length > 0 && error[0] == '(')
					{
						for (int i = 1; i < error.Length - 1; i++)
						{
							if (error[i] == ')' && error[i + 1] == ':')
								break;
							if (error[i] == ',' && lineS == null)
							{
								lineS = str;
								str = "";
							}
							else if (char.IsNumber(error[i]))
								str += error[i];
							else
							{
								str = "";
								break;
							}
						}
					}
					if (lineS == null)
						lineS = str;
					else
						colS = str;

					int line = 0, col = 0;
					bool knownLine = int.TryParse(lineS, out line);
					int.TryParse(colS, out col);
					line--;
					col--;

					if (line < 0)
						line = 0;
					if (col < 0)
						col = 0;

					exceptions.Add(new CompileException(line, col, error));
				}
			}

			if (exceptions.Count == 1)
				throw exceptions[0];
			else
				throw new CompileExceptionCollection(exceptions.ToArray());
		}

		public static Type GetTextureType(string textureTypeName)
		{
			switch (textureTypeName)
			{
				case "Texture1D":
				case "Texture2D":
					return typeof(Microsoft.Xna.Framework.Graphics.Texture2D);
				case "Texture3D":
					return typeof(Microsoft.Xna.Framework.Graphics.Texture3D);
				case "TextureCube":
					return typeof(Microsoft.Xna.Framework.Graphics.TextureCube);
				default:
					return typeof(Microsoft.Xna.Framework.Graphics.Texture);
			}
		}
	}


	[Flags]
	public enum Platform
	{
		Windows = 1,
		Xbox = 2,
		Both = 3
	}


	#region exceptions

	public sealed class CompileException : Exception
	{
		public CompileException(string text)
		{
			this.line = 0;
			this.col = 0;
			this.text = text;
		}
		public CompileException(int line, int col, string text)
		{
			this.line = line;
			this.col = col;
			this.text = text;
		}
		private readonly int line;
		private readonly int col;
		private readonly string text;

		//getters
		public int Coloumn
		{
			get { return col; }
		}

		public int Line
		{
			get { return line; }
		}

		public string Text
		{
			get { return text; }
		}

		public override string ToString()
		{
			return string.Format("({0}:{1}) {2}", Line, Coloumn, Text);
		}
	}

	public sealed class CompileExceptionCollection : Exception
	{
		private readonly CompileException[] exceptions;

		public CompileExceptionCollection(params CompileException[] exceptions)
		{
			this.exceptions = exceptions;
		}

		public CompileException GetException(int index)
		{
			return exceptions[index];
		}
		public int Count { get { return exceptions.Length; } }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (var line in exceptions)
			{
				sb.AppendLine(line.ToString());
			}
			return sb.ToString();
		}
	}

	#endregion
}
