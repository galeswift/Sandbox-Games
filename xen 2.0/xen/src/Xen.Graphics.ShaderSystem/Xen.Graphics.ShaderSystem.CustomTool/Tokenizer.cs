using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xen.Graphics.ShaderSystem.CustomTool
{
	public sealed class Tokenizer
	{
		private readonly StringBuilder token;
		private readonly string source;
		private int index, line, bracketDepth, braceDepth, parenthesisDepth, totalDepth;
		private readonly bool skipComments, periodAsCharacter, retainNewLines;

		public Tokenizer(string source) : this(source, true, true, false)
		{
		}

		public Tokenizer(string source, bool skipComments, bool periodAsCharacter, bool retainNewLines)
		{
			this.source = source;
			this.skipComments = skipComments;
			this.retainNewLines = retainNewLines;
			this.periodAsCharacter = periodAsCharacter;
			this.token = new StringBuilder();
		}

		public string Token
		{
			get { return token.ToString(); }
		}
		public bool TokenIsNewLine
		{
			get { return token.Length == 1 && (token[0] == '\n' || token[0] == '\r'); }
		}

		public int Index
		{
			get { return index; }
		}
		public int Length
		{
			get { return source.Length; }
		}
		public int Line
		{
			get { return line; }
		}
		public string Source
		{
			get { return source; }
		}
		public int BracketDepth
		{
			get { return bracketDepth; }
		}
		public int BraceDepth
		{
			get { return braceDepth; }
		}
		public int ParenthesisDepth
		{
			get { return parenthesisDepth; }
		}
		public int TotalDepth
		{
			get { return totalDepth; }
		}

		public bool NextToken()
		{
			return NextToken(true, true);
		}
		private bool NextToken(bool clearToken, bool skipWhitespace)
		{
			if (clearToken)
				token.Length = 0;
			int loop = 0;

			while (index < source.Length)
			{
				char character = source[index];

				if (character == '\n')
					line++;

				//extract comments...
				if (character == '/')
				{
					//conditional check next character
					if (index+1 < source.Length)
					{
						if (source[index+1] == '/') // line comment
						{
							index++;
							if (skipComments)
							{
								for (; index < source.Length; index++)
								{
									if (source[index] == '\n' || source[index] == '\r')
									{
										//token.Append('\n');
										break;
									}
								}
								continue;
							}
							else
								token.Append(source[index]);
						}
						if (source[index+1] == '*') /* begin block comment */
						{
							index++;
							if (skipComments)
							{
								int lineCount = line;
								if (source[index] == '\n')
									line++;

								char previous = '\0';
								index++;
								for (; index < source.Length; index++)
								{
									if (source[index] == '\n')
										line++;
									if (previous == '*' && source[index] == '/')
										break;
									previous = source[index];
								}
								index++;

								lineCount = line - lineCount;

								//while (lineCount-- > 0)
								//    token.Append('\n');

								continue;
							}
							else
								token.Append(source[index]);
						}
					}
				}

				if (loop > 0 && 
					!char.IsLetterOrDigit(character) &&
					character != '_' && !IsSpecialCharacter(character, true))
					return true;

				index++;
				if (!skipWhitespace)
					token.Append(character);

				if (char.IsWhiteSpace(character) && !IsSpecialCharacter(character, false))
				{
					if (loop == 0)
						continue;
					return true;
				}

				if (skipWhitespace)
					token.Append(character);

				if (loop == 0)
				{
					switch (character)
					{
						case '[':
							bracketDepth++;
							totalDepth++;
							return true;
						case ']':
							bracketDepth--;
							totalDepth--;
							return true;
						case '{':
							braceDepth++;
							totalDepth++;
							return true;
						case '}':
							braceDepth--;
							totalDepth--;
							return true;
						case '(':
							parenthesisDepth++;
							totalDepth++;
							return true;
						case ')':
							parenthesisDepth--;
							totalDepth--;
							return true;
					}
					if (!char.IsLetterOrDigit(character) && character != '_' && !IsSpecialCharacter(character, true))
						return true;
				}

				loop++;
			}
			return loop > 0 || index < source.Length;
		}

		public bool ReadBlock()
		{
			if (token.Length == 1)
			{
				switch (token[0])
				{
					case '{':
						return ReadBraceBlock();
					case '(':
						return ReadParenthesisBlock();
					case '[':
						return ReadBracketBlock();
				}
			}
			return false;
		}

		private bool IsSpecialCharacter(char character, bool wordCharacter)
		{
			return
				(periodAsCharacter && character == '.') ||
				(!wordCharacter && (retainNewLines && (character == '\n' || character == '\r')));
		}
		public static bool IsNameCharacter(char character)
		{
			return char.IsLetterOrDigit(character) || character == '_';
		}
		public static bool IsIdentifierToken(string token)
		{
			return token.Length > 1 || (token.Length == 1 && (char.IsLetterOrDigit(token[0]) || token[0] == '_'));
		}

		private bool ReadBracketBlock()
		{
			int bracket = this.bracketDepth;
			do
			{
				if (this.bracketDepth == bracket - 1)
					return true;
			}
			while (NextToken(false, false));
			return false;
		}


		private bool ReadBraceBlock()
		{
			int brace = this.braceDepth;
			do
			{
				if (this.braceDepth == brace - 1)
					return true;
			}
			while (NextToken(false, false));
			return false;
		}
		private bool ReadParenthesisBlock()
		{
			int paren = this.parenthesisDepth;
			do
			{
				if (this.parenthesisDepth == paren - 1)
					return true;
			}
			while (NextToken(false, false));
			return false;
		}
	}
}
