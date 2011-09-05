using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xen.Graphics.ShaderSystem.CustomTool.FX;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.CodeDom;
using System.IO;
using System.CodeDom.Compiler;
using Xen.Graphics.ShaderSystem.Constants;
using System.Reflection;

namespace Xen.Graphics.ShaderSystem.CustomTool.Dom
{
	public sealed class PreshaderSrc
	{
		private readonly List<CodeStatement> statements;
		private static Dictionary<string, MethodInfo> methodList;
		private int maxTempRegisters;
		private int maxConstantRegisterAccess; // a preshader may write to constants on the end of the main constant array
		private int maxBooleanRegistersWrite;

		public PreshaderSrc(AsmListing listing, CodeStatementCollection statementCollection)
		{
			statements = new List<CodeStatement>();

			object[] paramsArray = new object[1];
			StringBuilder sb = new StringBuilder();

			foreach (AsmCommand cmd in listing.Commands)
			{
				
				MethodInfo method;
				if (!methodList.TryGetValue(cmd.Target, out method))
				{
					if (cmd.Target == "preshader")
						continue;

					throw new CompileException(string.Format("Error decoding PreShader: Unexpected command '{0}'", cmd.Target));
				}

				string[] args = new string[cmd.OpCount];
				for (int i = 0; i < cmd.OpCount; i++)
				{
					cmd.GetOp(i).ToString(sb);
					args[i] = sb.ToString();
					sb.Length = 0;
				}
				paramsArray[0] = args;

				method.Invoke(this, paramsArray);
			}

			statementCollection.AddRange(this.statements.ToArray());
		}

		static PreshaderSrc()
		{
			methodList = new Dictionary<string, MethodInfo>();

			//extract the methods from this class, which will be used to generate preshader code

			MethodInfo[] methods = typeof(PreshaderSrc).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (MethodInfo method in methods)
			{
				if (char.IsLower(method.Name[0]))
					methodList.Add(method.Name, method);
			}
		}

		private CodeStatement Next 
		{
			set { statements.Add(value); } 
		}

		private CodeStatement[] NextArray
		{
			set { foreach (CodeStatement s in value) Next = s; }
		}

		public int MaxTempRegisters
		{
			get { return maxTempRegisters; }
		}
		public int MaxConstantRegisterAccess
		{
			get { return maxConstantRegisterAccess; }
		}
		public int MaxBooleanConstantRegisterWrite
		{
			get { return maxBooleanRegistersWrite; }
		}


		/*
		 * 
		 * 
		 *	The following methods are named after their preshader ASM equivalents
		 * 
		 * 
		 */

		/*
		 
mul r0, c6.w, c3 
add c3, r1, r0 
dot r0.xyz, (0.577, -0.577, -0.577), c4.xyz 
dot r0.yzw, (0.577, -0.577, -0.577), c5.xyz 
dot r0.zwx, (0.577, -0.577, -0.577), c6.xyz 
dot r1.xyz, r0.xyz, r0.xyz 
rsq r0.w, r1.x 
mul c0.xyz, r0.w, r0.xyz
mul c5.xyz, c7.xyz, 
		 
		 */

		private static CodeExpression One = new CodePrimitiveExpression(1.0f);
		private static CodeExpression Zero = new CodePrimitiveExpression(0.0f);

		CodeStatement Assign(CodeExpression dst, bool destIsBoolean, CodeExpression value)
		{
			if (destIsBoolean)
				return new CodeAssignStatement(dst, new CodeBinaryOperatorExpression(value, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(0F)));
			else
				return new CodeAssignStatement(dst, value);
		}
		CodeStatement[] AssignSingle(Reg dst, CodeExpression value)
		{
			CodeStatement[] output = new CodeStatement[dst.Length];
			output[0] = Assign(dst[0], dst.IsBoolean, value);// new CodeAssignStatement(dst[0], value);
			for (int i = 1; i < output.Length; i++)
				output[i] = new CodeAssignStatement(dst[i], dst[0]);
			return output;
		}
		CodeStatement[] Assign(Reg dst, Reg value)
		{
			if (value.Length == 1)
			{
				CodeStatement[] output = new CodeStatement[dst.Length];
				output[0] = Assign(dst[0], dst.IsBoolean, value[0]);//new CodeAssignStatement(dst[0], value[0]);
				for (int i = 1; i < output.Length; i++)
					output[i] = new CodeAssignStatement(dst[i], dst[0]);
				return output;
			}
			else
			{
				CodeStatement[] output = new CodeStatement[dst.Length];
				for (int i = 0; i < output.Length; i++)
					output[i] = Assign(dst[i], dst.IsBoolean, value[i]);//new CodeAssignStatement(dst[i], value[i]);
				return output;
			}
		}

		class Reg
		{
			private CodeExpression[] reference;
			private string swizzle;
			public readonly bool IsBoolean;

			public Reg(string reg, bool isDest, ref int maxTempRegisters, ref int maxConstantRegisterAccess, ref int maxBooleanRegistersWrite)
			{
				//parse the register

				//if the first character is a '(', then this is a constant value.
				//otherwise, it's a register.

				if (reg.Length > 0 && reg[0] == '(')
					ParseConstant(reg);
				else
					ParseValue(reg, isDest, ref maxTempRegisters, ref maxConstantRegisterAccess, ref maxBooleanRegistersWrite, out IsBoolean);
			}

			public CodeExpression this[int index]
			{
				get
				{
					if (reference.Length == 1)
						return reference[0];
					return reference[index];
				}
			}

			public int Length { get { return reference.Length; } }
			public string Swizzle { get { return swizzle; } }

			void ParseConstant(string reg)
			{
				reg = reg.Replace('(', ' ').Replace(')', ' ');//not the best...

				string[] values = reg.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

				reference = new CodeExpression[values.Length];
				for (int i = 0; i < values.Length; i++)
					reference[i] = new CodePrimitiveExpression(float.Parse(values[i]));
			}

			void ParseValue(string reg, bool isDest, ref int maxTempRegisters, ref int maxConstantRegisterAccess, ref int maxBooleanRegistersWrite, out bool isBoolean)
			{
				//first character is the regster set. Eg 'r' for temp, 'c' for constant, etc.
				//destination cosntants write directly to the main registers, not pre-registers.

				int start = 0;

				//if it starts with a '-', then it's negative
				bool negative = reg[0] == '-';
				if (negative)
					start = 1;

				isBoolean = false;
				if (reg.Length >= start + 1 &&
					reg[start] == 'o' &&
					reg[start + 1] == 'b')
				{
					//special case, writing to a boolean variable.
					start++;
					isBoolean = true;
				}

				string group = reg[start].ToString();

				//the index will be the next one, two or three characters
				int indexSize = 1;
				if ((reg.Length > 2+start && char.IsNumber(reg[start+2])))
				{
					indexSize++;
					if ((reg.Length > 3 + start && char.IsNumber(reg[start + 3])))
						indexSize++;
				}

				int index = int.Parse(reg.Substring(start + 1, indexSize));

				if (isBoolean)
				{
					if (!isDest)
						throw new ArgumentException("Unexpected error in preshader: Boolean value used as read type");

					maxBooleanRegistersWrite = Math.Max(maxBooleanRegistersWrite, index + 1);

					reference = new CodeExpression[] { BoolIndex(group, index) };
					return;
				}
				else
				{
					if (reg[start] == 'r') // temp reg
						maxTempRegisters = Math.Max(maxTempRegisters, index + 1);
					if (reg[start] == 'c' && isDest) // constant reg
						maxConstantRegisterAccess = Math.Max(maxConstantRegisterAccess, index + 1);
				}


				//finally, there may be a swizzle.

				swizzle = "xyzw";
				if (reg.Length > indexSize + 1 + start && reg[indexSize + 1 + start] == '.')
					swizzle = reg.Substring(indexSize + 2 + start);

				reference = new CodeExpression[swizzle.Length];

				if (reg[start] == 'c' && !isDest)
					group = "p";

				for (int i = 0; i < swizzle.Length; i++)
					reference[i] = Index(group, index, swizzle[i]);
			}

			CodeExpression Index(string group, int index, char swizzle)
			{
				CodeExpression variable;
				if (group == "r")//local temp.
					variable = new CodeVariableReferenceExpression(group + index);
				else
					variable = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression(group),new CodePrimitiveExpression(index));

				variable = new CodeFieldReferenceExpression(variable, char.ToUpper(swizzle).ToString());
				return variable;
			}
			CodeExpression BoolIndex(string group, int index)
			{
				return new CodeArrayIndexerExpression(new CodeVariableReferenceExpression(group), new CodePrimitiveExpression(index));
			}


		}


		private Reg DstOp(string op)
		{
			return new Reg(op, true, ref maxTempRegisters, ref maxConstantRegisterAccess, ref maxBooleanRegistersWrite);
		}
		private Reg SrcOp(string op)
		{
			return new Reg(op, false, ref maxTempRegisters, ref maxConstantRegisterAccess, ref maxBooleanRegistersWrite);
		}
			
		void cmp(string[] ops)
		{
			Reg dst = DstOp(ops[0]), 
				src0 = SrcOp(ops[1]), 
				src1 = SrcOp(ops[2]), 
				src2 = SrcOp(ops[3]);

			//		dst = src0 >= 0 ? src1 : src2;

			for (int i = 0; i < dst.Length; i++)
			{
				Next = new CodeConditionStatement(
						new CodeBinaryOperatorExpression(src0[i], CodeBinaryOperatorType.GreaterThanOrEqual, Zero),
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, src1[i]) },
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, src2[i]) });
			}
		}

		void max(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]),
				src1 = SrcOp(ops[2]);

			for (int i = 0; i < dst.Length; i++)
			{
				Next = new CodeConditionStatement(
						new CodeBinaryOperatorExpression(src0[i], CodeBinaryOperatorType.GreaterThanOrEqual, src1[i]),
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, src0[i]) },
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, src1[i]) });
			}
		}

		void min(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]),
				src1 = SrcOp(ops[2]);

			for (int i = 0; i < dst.Length; i++)
			{
				Next = new CodeConditionStatement(
						new CodeBinaryOperatorExpression(src0[i], CodeBinaryOperatorType.LessThan, src1[i]),
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, src0[i]) },
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, src1[i]) });
			}
		}

		//less than
		void lt(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]),
				src1 = SrcOp(ops[2]);

			for (int i = 0; i < dst.Length; i++)
			{
				Next = new CodeConditionStatement(
						new CodeBinaryOperatorExpression(src0[i], CodeBinaryOperatorType.LessThan, src1[i]),
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, Zero) },
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, One) });
			}
		}
		//greater than
		void gt(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]),
				src1 = SrcOp(ops[2]);

			for (int i = 0; i < dst.Length; i++)
			{
				Next = new CodeConditionStatement(
						new CodeBinaryOperatorExpression(src0[i], CodeBinaryOperatorType.GreaterThan, src1[i]),
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, Zero) },
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, One) });
			}
		}


		//less than or equal
		void le(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]),
				src1 = SrcOp(ops[2]);

			for (int i = 0; i < dst.Length; i++)
			{
				Next = new CodeConditionStatement(
						new CodeBinaryOperatorExpression(src0[i], CodeBinaryOperatorType.LessThanOrEqual, src1[i]),
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, Zero) },
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, One) });
			}
		}
		//greater than or equal
		void ge(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]),
				src1 = SrcOp(ops[2]);

			for (int i = 0; i < dst.Length; i++)
			{
				Next = new CodeConditionStatement(
						new CodeBinaryOperatorExpression(src0[i], CodeBinaryOperatorType.GreaterThanOrEqual, src1[i]),
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, Zero) },
							new CodeStatement[] { Assign(dst[i], dst.IsBoolean, One) });
			}
		}

		void abs(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Math)), "Abs"), src0[i]));
		}

		void neg(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeBinaryOperatorExpression(new CodePrimitiveExpression(0), CodeBinaryOperatorType.Subtract, src0[i]));
		}

		void add(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]),
				src1 = SrcOp(ops[2]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeBinaryOperatorExpression(src0[i], CodeBinaryOperatorType.Add, src1[i]));
		}

		void mul(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]),
				src1 = SrcOp(ops[2]);


			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeBinaryOperatorExpression(src0[i], CodeBinaryOperatorType.Multiply, src1[i]));
		}


		void rsq(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeBinaryOperatorExpression(new CodePrimitiveExpression(1.0f), CodeBinaryOperatorType.Divide, new CodeCastExpression(typeof(float), new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Math)), "Sqrt"), src0[i]))));
		}


		void cos(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeCastExpression(typeof(float), 
					new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression(typeof(System.Math)), "Cos"), src0[i])));
		}

		void sin(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeCastExpression(typeof(float), 
					new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression(typeof(System.Math)), "Sin"), src0[i])));
		}

		void tan(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeCastExpression(typeof(float), 
					new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression(typeof(System.Math)), "Tan"), src0[i])));
		}

		void atan(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeCastExpression(typeof(float), 
					new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression(typeof(System.Math)), "Atan"), src0[i])));
		}

		void acos(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeCastExpression(typeof(float), 
					new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression(typeof(System.Math)), "Acos"), src0[i])));
		}
		void asin(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeCastExpression(typeof(float), 
					new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression(typeof(System.Math)), "Asin"), src0[i])));
		}

		void cosh(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeCastExpression(typeof(float), 
					new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression(typeof(System.Math)), "Cosh"), src0[i])));
		}
		void sinh(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeCastExpression(typeof(float), 
					new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression(typeof(System.Math)), "Sinh"), src0[i])));
		}
		void tanh(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeCastExpression(typeof(float),
					new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
						new CodeTypeReferenceExpression(typeof(System.Math)), "Tanh"), src0[i])));
		}


		void crs(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]),
				src1 = SrcOp(ops[2]);

			if (dst.Length != 3 ||
				src0.Length != 3 ||
				src1.Length != 3)
				throw new NotSupportedException("Cross product on non-vector3");


			Next = Assign(dst[0], dst.IsBoolean, new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(src0[1], CodeBinaryOperatorType.Multiply, src1[2]), CodeBinaryOperatorType.Subtract, new CodeBinaryOperatorExpression(src0[2], CodeBinaryOperatorType.Multiply, src1[1])));
			Next = Assign(dst[1], dst.IsBoolean, new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(src0[2], CodeBinaryOperatorType.Multiply, src1[0]), CodeBinaryOperatorType.Subtract, new CodeBinaryOperatorExpression(src0[0], CodeBinaryOperatorType.Multiply, src1[2])));
			Next = Assign(dst[2], dst.IsBoolean, new CodeBinaryOperatorExpression(new CodeBinaryOperatorExpression(src0[0], CodeBinaryOperatorType.Multiply, src1[1]), CodeBinaryOperatorType.Subtract, new CodeBinaryOperatorExpression(src0[1], CodeBinaryOperatorType.Multiply, src1[0])));
		}

		void mov(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			NextArray = Assign(dst, src0);
		}


		void dot(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]),
				src1 = SrcOp(ops[2]);

			CodeExpression exp = null;

			switch (Math.Max(src0.Length,src1.Length))
			{
				case 4:
					exp = new CodeBinaryOperatorExpression(
						
							new CodeBinaryOperatorExpression(
									new CodeBinaryOperatorExpression(src0[0], CodeBinaryOperatorType.Multiply, src1[0]),
									CodeBinaryOperatorType.Add,
									new CodeBinaryOperatorExpression(src0[1], CodeBinaryOperatorType.Multiply, src1[1])), 
									
						CodeBinaryOperatorType.Add,
							
							new CodeBinaryOperatorExpression(
										new CodeBinaryOperatorExpression(src0[2], CodeBinaryOperatorType.Multiply, src1[2]),
										CodeBinaryOperatorType.Add,
										new CodeBinaryOperatorExpression(src0[3], CodeBinaryOperatorType.Multiply, src1[3])));
					break;
				case 3:
					exp = new CodeBinaryOperatorExpression(
						
							new CodeBinaryOperatorExpression(
									new CodeBinaryOperatorExpression(src0[0], CodeBinaryOperatorType.Multiply, src1[0]),
									CodeBinaryOperatorType.Add,
									new CodeBinaryOperatorExpression(src0[1], CodeBinaryOperatorType.Multiply, src1[1])), 
									
						CodeBinaryOperatorType.Add,
							
							new CodeBinaryOperatorExpression(src0[2], CodeBinaryOperatorType.Multiply, src1[2]));
					break;
				case 2:
					exp = new CodeBinaryOperatorExpression(
									new CodeBinaryOperatorExpression(src0[0], CodeBinaryOperatorType.Multiply, src1[0]),
									CodeBinaryOperatorType.Add,
									new CodeBinaryOperatorExpression(src0[1], CodeBinaryOperatorType.Multiply, src1[1]));
					break;
				case 1:
					exp = new CodeBinaryOperatorExpression(src0[0], CodeBinaryOperatorType.Multiply, src1[0]);
					break;
			}

			if (exp != null)
			{
				if (dst.Length > 1)
				{
					Next = Assign(dst[0], dst.IsBoolean, exp);
					for (int i = 1; i < dst.Length; i++)
						Next = Assign(dst[i], dst.IsBoolean, dst[0]);
				}
				else
				{
					NextArray = AssignSingle(dst, exp);
				}
			}
		}

		void sincos(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			switch (dst.Swizzle)
			{
				case "x":
					Next = Assign(dst[0], dst.IsBoolean, new CodeCastExpression(typeof(float), new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Math)), "Cos"), src0[0])));
					break;
				case "y":
					Next = Assign(dst[1], dst.IsBoolean, new CodeCastExpression(typeof(float), new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Math)), "Sin"), src0[0])));
					break;
				case "xy":
					Next = Assign(dst[0], dst.IsBoolean, new CodeCastExpression(typeof(float), new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Math)), "Cos"), src0[0])));
					Next = Assign(dst[1], dst.IsBoolean, new CodeCastExpression(typeof(float), new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Math)), "Sin"), src0[0])));
					break;
			}
		}

		void sub(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]),
				src1 = SrcOp(ops[2]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeBinaryOperatorExpression(src0[i], CodeBinaryOperatorType.Subtract, src1[i]));
		}

		void rcp(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeBinaryOperatorExpression(new CodePrimitiveExpression(1.0f), CodeBinaryOperatorType.Divide, src0[i]));

		}

		void exp(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				   src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeCastExpression(typeof(float), new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Math)), "Exp"), src0[i])));
	
		}

		void frc(string[] ops)
		{

			Reg dst = DstOp(ops[0]),
				   src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, 
					new CodeBinaryOperatorExpression(src0[i], CodeBinaryOperatorType.Subtract,
					new CodeCastExpression(typeof(float), new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Math)), "Floor"), src0[i]))));
	
		}

		void log(string[] ops)
		{
			Reg dst = DstOp(ops[0]),
				   src0 = SrcOp(ops[1]);

			for (int i = 0; i < dst.Length; i++)
				Next = Assign(dst[i], dst.IsBoolean, new CodeCastExpression(typeof(float), new CodeBinaryOperatorExpression(new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Math)), "Log"), new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(System.Math)), "Abs"), src0[i])), CodeBinaryOperatorType.Divide, new CodePrimitiveExpression(0.69314718055994529))));
	
		}
	}
}

