using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Xen.Graphics.ShaderSystem.CustomTool.FX
{
	//output from a vertex shader, or input to a pixel shader...
	//eg, POSITION0
	public struct AsmDcl : IComparable<AsmDcl>
	{
		public AsmDcl(VertexElementUsage usage, byte index)
		{
			this.Usage = usage;
			this.Index = index;
		}

		//read from a dcl_ string
		public static bool Parse(string dcl, out AsmDcl dclOut)
		{
			dclOut = new AsmDcl();
			byte Index = 0;

			//last two characters *might* be a number
			if (dcl.Length > 0 && char.IsNumber(dcl[dcl.Length - 1]))
			{
				string index = dcl[dcl.Length - 1].ToString();

				if (dcl.Length > 1 && char.IsNumber(dcl[dcl.Length - 2]))
				{
					index = dcl[dcl.Length - 2] + index;
					dcl = dcl.Substring(0, dcl.Length - 2);
				}
				else
					dcl = dcl.Substring(0, dcl.Length - 1);

				Index = (byte)int.Parse(index);
			}

			VertexElementUsage Usage = VertexElementUsage.Position;

			switch (dcl)
			{
				case "position":
					Usage = VertexElementUsage.Position;
					break;
				case "blendweight":
					Usage = VertexElementUsage.BlendWeight;
					break;
				case "blendindices":
					Usage = VertexElementUsage.BlendIndices;
					break;
				case "normal":
					Usage = VertexElementUsage.Normal;
					break;
				case "psize":
					Usage = VertexElementUsage.PointSize;
					break;
				case "texcoord":
					Usage = VertexElementUsage.TextureCoordinate;
					break;
				case "tangent":
					Usage = VertexElementUsage.Tangent;
					break;
				case "binormal":
					Usage = VertexElementUsage.Binormal;
					break;
				case "color":
					Usage = VertexElementUsage.Color;
					break;
				case "fog":
					Usage = VertexElementUsage.Fog;
					break;
				case "depth":
					Usage = VertexElementUsage.Depth;
					break;
				case "tessfactor":
					Usage = VertexElementUsage.TessellateFactor;
					break;
				default:
					return false;
			}
			dclOut = new AsmDcl(Usage, Index);
			return true;
		}

		public readonly VertexElementUsage Usage;
		public readonly byte Index;


		public int CompareTo(AsmDcl other)
		{
			if (Usage != other.Usage)
				return Usage.CompareTo(other.Usage);
			if (Index != other.Index)
				return Index.CompareTo(other.Index);
			return 0;
		}
	}

	public sealed class AsmOp
	{
		private readonly string[] ops;

		public int TokenCount { get { return ops.Length; } }
		public string GetToken(int index) { return ops[index]; }

		public AsmOp(string[] ops)
		{
			this.ops = ops;
		}

		public void ToString(StringBuilder sb)
		{
			for (int i = 0; i < ops.Length; i++)
				sb.Append(ops[i]);
		}
	}

	public sealed class AsmCommand
	{
		private readonly string target;
		private readonly AsmOp[] ops;

		public string Target { get { return target; } }
		public int OpCount { get { return ops.Length; } }
		public AsmOp GetOp(int index) { return ops[index]; }

		public AsmCommand(string target, AsmOp[] ops)
		{
			this.target = target;
			this.ops = ops;
		}

		public void ToString(StringBuilder sb)
		{
			sb.Append(target);
			for (int i = 0; i < ops.Length; i++)
			{
				sb.Append(i == 0 ? " " : ", ");
				ops[i].ToString(sb);
			}
		}
	}

	public sealed class AsmListing
	{
		private readonly AsmCommand[] commands;
		private readonly RegisterSet registers;
		private readonly AsmDcl[] dclIn, dclOut;

		public int GetCommandCount() { return commands.Length; }
		public AsmCommand GetCommand(int index) { return commands[index]; }
		public IEnumerable<AsmCommand> Commands { get { foreach (AsmCommand cmd in commands) yield return cmd; } }

		public RegisterSet RegisterSet { get { return registers; } }

		public int InputCount { get { return dclIn.Length; } }
		public AsmDcl GetInput(int index) { return dclIn[index]; }
		public bool ContainsInput(AsmDcl dcl) { return Array.BinarySearch(this.dclIn, dcl) >= 0; }

		public int OutputCount { get { return dclOut.Length; } }
		public AsmDcl GetOutput(int index) { return dclOut[index]; }
		public bool ContainsOutput(AsmDcl dcl) { return Array.BinarySearch(this.dclOut, dcl) >= 0; }

		public AsmListing(string listing, RegisterSet registers)
		{
			this.registers = registers;

			Tokenizer tokens = new Tokenizer(listing, true, false, true);

			List<string> op = new List<string>();
			List<AsmCommand> commands = new List<AsmCommand>();
			List<AsmOp> ops = new List<AsmOp>();
			string target = null;

			while (tokens.NextToken())
			{
				string token = tokens.Token;

				switch (token)
				{
					case "(":	//this only occurs in preshaders
						tokens.ReadBlock();
						ops.Add(new AsmOp(new string[]{tokens.Token}));
						break;

					case ",":
						if (op.Count > 0)
							ops.Add(new AsmOp(op.ToArray()));
						op.Clear();
						break;

					case "\n":
					case "\r":
						if (op.Count > 0)
							ops.Add(new AsmOp(op.ToArray()));
						op.Clear();

						if (target != null)
							commands.Add(new AsmCommand(target,ops.ToArray()));
						
						target = null;
						ops.Clear();
						break;

					default:
						if (target == null)
							target = token;
						else
							op.Add(token);
						break;
				}
			}

			this.commands = commands.ToArray();

			this.dclIn = ExtractInputs();
			this.dclOut = ExtractOuputs();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < commands.Length; i++)
			{
				if (i != 0)
					sb.AppendLine();
				commands[i].ToString(sb);
			}
			return sb.ToString();
		}


		private AsmDcl[] ExtractOuputs()
		{
			List<AsmDcl> dclList = new List<AsmDcl>();

			if (commands.Length > 0 &&
				commands[0].Target.Length == 6 &&
				commands[0].Target.StartsWith("vs_3_"))
			{
				//vs_3_0 is the odd one out here, using dcl_position0.. style declarations

				for (int i = 0; i < this.commands.Length; i++)
				{
					if (this.commands[i].Target.Length > 4 &&
						this.commands[i].Target.Substring(0, 4) == "dcl_")
					{
						//it's an output, or an input..
						//if it's an output, it'll have one arg, going to an oX register

						if (this.commands[i].OpCount == 1)
						{
							AsmOp op = this.commands[i].GetOp(0);
							if (op.TokenCount >= 1)
							{
								string token = op.GetToken(0);
								if (token.Length > 1 &&
									token[0] == 'o' &&
									char.IsNumber(token[1]))
								{
									//it's an output!
									AsmDcl output;
									if (AsmDcl.Parse(this.commands[i].Target.Substring(4), out output))
										dclList.Add(output);
								}
							}
						}
					}
				}
			}
			else
			{
				//all others just use registers with funky names.
				//oPos			- VS position out
				//oTXX			- VS tex out
				//oD0, oD1		- VS colour out
				//oCX			- PS colour out
				//oDepth		- PS depth out

				//loop through all the commands,
				for (int c = 0; c < commands.Length; c++)
				{
					//loop their args
					for (int i = 0; i < commands[c].OpCount; i++)
					{
						AsmOp op = commands[c].GetOp(i);

						//find any that point to the special 'o' args
						string reg = null;

						if (op.TokenCount > 0)
						{
							reg = op.GetToken(0);

							if (reg.Length == 1 && reg[0] == '-' && op.TokenCount > 1)
								reg = op.GetToken(1);

							if (reg.Length > 1 &&
								reg[0] == 'o')
							{
								AsmDcl? dcl = null;
								//here we go. found an output.

								if (reg.Length == 4 && reg == "oPos")
									dcl = new AsmDcl(VertexElementUsage.Position, 0);
								if (reg.Length == 6 && reg == "oDepth")
									dcl = new AsmDcl(VertexElementUsage.Depth, 0);

								if (reg.Length == 3 && (reg[1] == 'D' || reg[1] == 'C') && char.IsNumber(reg[2]))
									dcl = new AsmDcl(VertexElementUsage.Color, byte.Parse(reg[2].ToString()));

								if (reg.Length == 3 && reg[1] == 'T' && char.IsNumber(reg[2]))
									dcl = new AsmDcl(VertexElementUsage.TextureCoordinate, byte.Parse(reg[2].ToString()));
								if (reg.Length == 4 && reg[1] == 'T' && char.IsNumber(reg[2]) && char.IsNumber(reg[3]))
									dcl = new AsmDcl(VertexElementUsage.TextureCoordinate, byte.Parse(reg.Substring(2)));
									
								if (dcl != null && dclList.Contains(dcl.Value) == false)
								{
									dclList.Add(dcl.Value);
								}
							}
						}
					}
				}
			}

			dclList.Sort();
			return dclList.ToArray();
		}


		private AsmDcl[] ExtractInputs()
		{
			List<AsmDcl> inputs = new List<AsmDcl>();

			if (commands.Length > 0 &&
				commands[0].Target.Length == 6 &&
				commands[0].Target.StartsWith("ps_2_"))
			{
				//logic is quite different for ps_2_x
				//instead of dcl_texcoord0 v0
				//you get:
				//dcl t0
				//or
				//dcl v0
				//for colour

				for (int i = 0; i < this.commands.Length; i++)
				{
					if (this.commands[i].Target.Length == 3 &&
						this.commands[i].Target == "dcl")
					{
						//if it's an output, it'll have one arg, going to a tX or vX register

						if (this.commands[i].OpCount == 1)
						{
							AsmOp op = this.commands[i].GetOp(0);
							if (op.TokenCount >= 1)
							{
								byte index;

								string token = op.GetToken(0);
								if (token.Length > 1 &&
									(token[0] == 'v' || token[0] == 't') &&
									char.IsNumber(token[1]) &&
									byte.TryParse(token.Substring(1), out index))
								{
									if (token[0] == 'v') // colour
										inputs.Add(new AsmDcl(VertexElementUsage.Color, index));
									else
										inputs.Add(new AsmDcl(VertexElementUsage.TextureCoordinate, index));
								}
							}
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < this.commands.Length; i++)
				{
					if (this.commands[i].Target.Length > 4 &&
						this.commands[i].Target.Substring(0, 4) == "dcl_")
					{
						//it's an output, or an input..
						//if it's an input, it'll have one arg, going to an vX register

						if (this.commands[i].OpCount == 1)
						{
							AsmOp op = this.commands[i].GetOp(0);
							if (op.TokenCount >= 1)
							{
								string token = op.GetToken(0);
								if (token.Length > 1 &&
									token[0] == 'v' &&
									char.IsNumber(token[1]))
								{
									//it's an input!
									AsmDcl input;
									if (AsmDcl.Parse(this.commands[i].Target.Substring(4), out input))
										inputs.Add(input);
								}
							}
						}
					}
				}
			}

			inputs.Sort();
			return inputs.ToArray();
		}
	}
}
