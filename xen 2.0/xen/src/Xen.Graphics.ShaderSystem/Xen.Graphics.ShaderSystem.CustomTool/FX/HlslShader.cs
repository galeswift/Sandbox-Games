using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xen.Graphics.ShaderSystem.CustomTool.FX
{
	//creates a hirachical representation of the shader
	public sealed class HlslStructure
	{
		public struct ShaderStatement
		{
			public ShaderStatement(string value, int line)
			{
				this.Statement = value;
				this.Insert = string.Format(" /*{0}*/ ",line);
				this.Line = line;
			}
			public readonly string Statement;
			public readonly string Insert;
			public readonly int Line;

			public static bool operator ==(ShaderStatement s, string v)
			{
				return s.Statement == v;
			}
			public static bool operator !=(ShaderStatement s, string v)
			{
				return s.Statement != v;
			}
			public static implicit operator string(ShaderStatement s)
			{
				return s.Statement;
			}
			public override int GetHashCode()
			{
				return Statement.GetHashCode();
			}
			public override bool Equals(object obj)
			{
				if (obj is ShaderStatement)
					return Statement.Equals(((ShaderStatement)obj).Statement);
				return Statement.Equals(obj);
			}
		}

		public readonly ShaderStatement[] Elements;
		public readonly HlslStructure[] Children;
		public readonly bool BraceEnclosedChildren;
		public readonly bool BracketEnclosedChildren;

		public HlslStructure(string source)
		{
			List<HlslStructure> nodes = new List<HlslStructure>();
			Tokenizer tokenizer = new Tokenizer(source, true, false, true);

			while (tokenizer.Index < tokenizer.Length)
				Parse(tokenizer, nodes);

			this.Elements = new ShaderStatement[0];
			this.Children = nodes.ToArray();
			this.BraceEnclosedChildren = false;
			this.BracketEnclosedChildren = false;
		}

		private HlslStructure(ShaderStatement[] el, HlslStructure[] ch, bool braceEnclosed, bool bracketEnclosedChildren)
		{
			this.Elements = el;
			this.Children = ch;
			this.BraceEnclosedChildren = braceEnclosed;
			this.BracketEnclosedChildren = bracketEnclosedChildren;
		}

		public IEnumerable<HlslStructure> GetEnumerator()
		{
			yield return this;
			foreach (HlslStructure hs in this.Children)
			{
				foreach (HlslStructure child in hs.GetEnumerator())
				{
					yield return child;
				}
			}
		}



		private static void Parse(Tokenizer tokenizer, List<HlslStructure> list)
		{
			int braceDepth = tokenizer.BraceDepth;
			int bracketDepth = tokenizer.BracketDepth;
			List<ShaderStatement> buffer = new List<ShaderStatement>();

			bool breakOnNewLine = false;
			bool isNewLine = false;

			//parsing a top level type declaration, eg, float4 test = ...;
			//will be set to false when hitting the '=' or a brace.
			//this is used to detect annotations on types (and ignore them!)
			bool processingTypeDeclaration = braceDepth == 0;

			while (tokenizer.NextToken())
			{
				if (isNewLine)
				{
					if (tokenizer.Token.Length == 1 &&
						tokenizer.Token[0] == '#') //#include, #if, etc.
					{
						breakOnNewLine = true;
						processingTypeDeclaration = false;
					}
				}

				if (processingTypeDeclaration && (tokenizer.Token == "=" || tokenizer.BraceDepth > 0))
					processingTypeDeclaration = false;


				//detect annotations (in a <> block)
				if (processingTypeDeclaration && tokenizer.Token == "<")
				{
					//skip the annotation entirely.

					int blockDepth = 1;
					//parse until the matching '>'
					while (tokenizer.NextToken() && blockDepth != 0)
					{
						if (tokenizer.Token.Length == 1)
						{
							if (tokenizer.Token[0] == '<')
								blockDepth++;
							else
							if (tokenizer.Token[0] == '>')
								blockDepth--;
						}
					}
				}


				if (tokenizer.TokenIsNewLine)
					isNewLine = true;
				else
					isNewLine = false;

				if (bracketDepth < tokenizer.BracketDepth && !processingTypeDeclaration)
				{
					List<HlslStructure> nodes = new List<HlslStructure>();

					while (bracketDepth < tokenizer.BracketDepth)
						Parse(tokenizer, nodes);

					list.Add(new HlslStructure(buffer.ToArray(), nodes.ToArray(), false, true));

					buffer.Clear();
					continue;
				}

				if (braceDepth < tokenizer.BraceDepth)
				{
					List<HlslStructure> nodes = new List<HlslStructure>();

					while (braceDepth < tokenizer.BraceDepth)
						Parse(tokenizer, nodes);

					list.Add(new HlslStructure(buffer.ToArray(), nodes.ToArray(), true, false));

					buffer.Clear();
					processingTypeDeclaration = tokenizer.BraceDepth == 0;

					continue;
				}

				if (braceDepth > tokenizer.BraceDepth)
					break;
				
				if (bracketDepth > tokenizer.BracketDepth && !processingTypeDeclaration)
					break;

				if (!tokenizer.TokenIsNewLine)
					buffer.Add(new ShaderStatement(tokenizer.Token, tokenizer.Line));

				if (tokenizer.Token == ";" || (breakOnNewLine && tokenizer.TokenIsNewLine))
					break;
			}

			if (buffer.Count > 0)
				list.Add(new HlslStructure(buffer.ToArray(), new HlslStructure[0], false, false));
		}


		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			ToString(sb, 0, null, null, false, false);
			return sb.ToString();
		}

		public interface ITokenTranslator
		{
			string Translate(string token, bool isInBody, out bool replaceBracketWithParenthesis);
			bool IncludeBlock(HlslStructure block, int depth);
		}
		private class DefaultITokenTranslator : ITokenTranslator
		{
			public string Translate(string token, bool isInBody, out bool replaceBracketWithParenthesis) { replaceBracketWithParenthesis = false; return token; }
			public bool IncludeBlock(HlslStructure block, int depth) { return true; }
		}

		public void ToString(StringBuilder sb, int depth, ITokenTranslator translator, string methodPrefix, bool appendLineTags, bool retainLines)
		{
			ToString(sb, depth, translator, methodPrefix, true, true, appendLineTags, retainLines);
		}

		//returns true if line tags were written
		private bool ToString(StringBuilder sb, int depth, ITokenTranslator translator, string methodPrefix, bool rootCall, bool lineBreak, bool appendLineTags, bool retainLineIndices)
		{
			if (translator == null)
				translator = new DefaultITokenTranslator();

			bool tagsWritten = false;

			if (translator.IncludeBlock(this,depth))
			{
				if (lineBreak)
				{
					sb.AppendLine();
					for (int i = 0; i < depth; i++)
						sb.Append('\t');
				}

				bool replaceBracketWithParenthesis = false;

				string lastTagInset = null;

				for (int i = 0; i < Elements.Length; i++)
				{
					if (i != 0 && Tokenizer.IsIdentifierToken(Elements[i]) && Tokenizer.IsIdentifierToken(Elements[i - 1]))
					{
						sb.Append(' ');

						if (appendLineTags)
						{
							sb.Append(Elements[i].Insert);
							tagsWritten = true;
						}
					}

					if (retainLineIndices)
					{
						int line = 0;
						foreach (char c in sb.ToString())
						{
							if (c == '\n')
								line++;
						}

						while (retainLineIndices && Elements[i].Line > line++)
						{
							sb.AppendLine();
						}
					}

					sb.Append(translator.Translate(Elements[i], rootCall == false, out replaceBracketWithParenthesis));
					lastTagInset = Elements[i].Insert;
				}

				if (BraceEnclosedChildren)
				{
					if (lastTagInset != null && appendLineTags)
					{
						sb.Append(lastTagInset);
						tagsWritten = true;
					}

					sb.AppendLine();
					for (int i = 0; i < depth; i++)
						sb.Append('\t');

					sb.Append('{');
					depth++;

					//add the prefix..
					if (methodPrefix != null)
					{
						foreach (string line in methodPrefix.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
						{
							sb.AppendLine();
							for (int i = 0; i < depth; i++)
								sb.Append('\t');
							sb.Append(line);
						}
					}
				}

				if (BracketEnclosedChildren)
				{
					sb.Append(replaceBracketWithParenthesis ? '(' : '[');
				}

				bool childTagsWritten = false;
				foreach (HlslStructure node in Children)
					childTagsWritten |= node.ToString(sb, depth, translator, null, false, lineBreak && !BracketEnclosedChildren, appendLineTags, retainLineIndices);

				if (BraceEnclosedChildren)
				{
					sb.AppendLine();
					for (int i = 1; i < depth; i++)
						sb.Append('\t');

					sb.Append('}');

					if (Children.Length > 0 && !retainLineIndices)
						sb.AppendLine();
				}

				if (BracketEnclosedChildren)
				{
					sb.Append(replaceBracketWithParenthesis ? ')' : ']');
				}

				if (!BraceEnclosedChildren && !childTagsWritten && lastTagInset != null && appendLineTags)
				{
					sb.Append(lastTagInset);
					tagsWritten = true;
				}

				tagsWritten |= childTagsWritten;
			}
			return tagsWritten;
		}
	}
}
