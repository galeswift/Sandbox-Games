using System;
using System.Collections.Generic;
using System.Text;
using Xen.Graphics;
using Xen.Camera;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Xen.Ex.Graphics.Display;
using Xen.Ex.Graphics;
using Xen.Ex.Graphics2D;
using System.CodeDom;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Xen.Ex.Graphics.Content;

namespace Xen.Ex.Graphics.Processor
{
#if DEBUG

	//this class generates the .net assemblies used by the CPU particle processor
	static class CpuParticleLogicBuilder
	{
		static Dictionary<string, CodeExpression> memberTranslate;

		public static CodeMemberMethod BuildCpuLogic(string name, ReadOnlyArrayCollection<ParticleSystemLogicStep> steps, bool addMethod)
		{
			//generate the a runtime method...

			if (memberTranslate == null)
			{
				memberTranslate = new Dictionary<string, CodeExpression>();
				BuildTranslate(memberTranslate);
			}

			CodeMemberMethod method = new CodeMemberMethod();
			method.Attributes = MemberAttributes.Public | MemberAttributes.Static | MemberAttributes.Final;
			method.Name = name;

			//inputs
			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(uint), "count"));
			if (addMethod)
				method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(uint[]), "indices"));
			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(float), "time"));
			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Random), "rand"));
			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(float), "delta_time"));
			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(float[]), "global"));

			//fill the logic tree
			BuildLogic(method.Statements, steps, addMethod);

			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Vector2[]), "life"));

			//pass in all members even if unused
			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Vector4[]), "ps"));
			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Vector4[]), "vr"));
			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Vector4[]), "col"));
			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Vector4[]), "user"));

			return method;
		}

		public static byte[] GenerateAssembly(string namespaceName, string className, params CodeMemberMethod[] methods)
		{
			CodeCompileUnit unit = new CodeCompileUnit();
			CodeNamespace space = new CodeNamespace(namespaceName);
			CodeTypeDeclaration type = new CodeTypeDeclaration(className);

			type.Attributes = MemberAttributes.Static | MemberAttributes.Public;
			type.TypeAttributes = System.Reflection.TypeAttributes.Public | System.Reflection.TypeAttributes.Sealed;

			type.Members.AddRange(methods);
			space.Types.Add(type);
			unit.Namespaces.Add(space);

			Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider();

			System.CodeDom.Compiler.CompilerParameters options = new System.CodeDom.Compiler.CompilerParameters();
			options.IncludeDebugInformation = false;
			options.GenerateExecutable = false;

			options.ReferencedAssemblies.Add(typeof(int).Assembly.Location);
			options.ReferencedAssemblies.Add(typeof(Vector2).Assembly.Location);

			System.CodeDom.Compiler.CompilerResults results = provider.CompileAssemblyFromDom(options,unit);

			System.CodeDom.Compiler.CodeGeneratorOptions ops = new System.CodeDom.Compiler.CodeGeneratorOptions();
			ops.IndentString = "\t";
			ops.VerbatimOrder = true;
			ops.BracingStyle = "C";

			StringWriter text = new StringWriter();
			provider.GenerateCodeFromCompileUnit(unit, text, ops);
			string code = text.ToString();

			if (results.Errors.HasErrors)
			{
				string errors = "";
				foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
				{
					errors += error.ToString() + Environment.NewLine;
				}
				throw new InvalidOperationException(errors);
			}
			
			byte[] data = File.ReadAllBytes(results.PathToAssembly);
			File.Delete(results.PathToAssembly);
			return data;
		}

		private static void BuildTranslate(Dictionary<string, CodeExpression> translate)
		{
			string prefix = "";

			translate.Add("position.x",	Ex(prefix+"ps", 0));
			translate.Add("position.y",	Ex(prefix+"ps", 1));
			translate.Add("position.z",	Ex(prefix+"ps", 2));
			translate.Add("size",		Ex(prefix+"ps", 3));

			translate.Add("velocity.x",	Ex(prefix+"vr", 0));
			translate.Add("velocity.y",	Ex(prefix+"vr", 1));
			translate.Add("velocity.z",	Ex(prefix+"vr", 2));
			translate.Add("rotation",	Ex(prefix+"vr", 3));

			translate.Add("red",		Ex(prefix+"col", 0));
			translate.Add("green",		Ex(prefix+"col", 1));
			translate.Add("blue",		Ex(prefix+"col", 2));
			translate.Add("alpha",		Ex(prefix+"col", 3));

			translate.Add("life",		Ex("life", 0));
			translate.Add("age",		new CodeBinaryOperatorExpression(new CodeArgumentReferenceExpression("time"), CodeBinaryOperatorType.Subtract, Ex("life", 1)));

			translate.Add("delta_time", new CodeArgumentReferenceExpression("delta_time"));


			for (int i = 0; i < 4; i++)
			{
				translate.Add("user" + i, Ex(prefix+"user", i));
				translate.Add("local" + i, new CodeVariableReferenceExpression("local"+i));
			}
			for (int i = 0; i < 16; i++)
				translate.Add("global" + i, new CodeArrayIndexerExpression(new CodeArgumentReferenceExpression("global"), new CodePrimitiveExpression(i)));
		}

		private static CodeExpression Ex(string member, int index)
		{
			string swizzle = "X";
			switch (index)
			{
				case 1:
					swizzle = "Y";
					break;
				case 2:
					swizzle = "Z";
					break;
				case 3:
					swizzle = "W";
					break;
			}
			CodeExpression loop = new CodeVariableReferenceExpression("i");
			return new CodeFieldReferenceExpression(new CodeArrayIndexerExpression(new CodeArgumentReferenceExpression(member), loop), swizzle);
		}

		private static CodeExpression Arg(string input)
		{
			if (input == null)
				return null;
			CodeExpression ex;
			if (memberTranslate.TryGetValue(input, out ex))
				return ex;
			return new CodePrimitiveExpression(float.Parse(input, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
		}

		private static void BuildLogic(CodeStatementCollection statements, ReadOnlyArrayCollection<ParticleSystemLogicStep> steps, bool addMethod)
		{
			Dictionary<string, bool> argsUsed = new Dictionary<string, bool>();
			foreach (string arg in memberTranslate.Keys)
				argsUsed.Add(arg, false);
			argsUsed.Add("rand", false);

			CodeStatementCollection code = new CodeStatementCollection();
			BuildLogic(code, steps, argsUsed, 0);

			//rand requires a local variable
			if (argsUsed["rand"])
				statements.Add(
					new CodeVariableDeclarationStatement(
						typeof(float), "_tmp", new CodePrimitiveExpression(0.0f)));

			for (int i = 0; i < 4; i++)
			{
				if (argsUsed["local" + i])
				{
					code.Insert(0,
						new CodeVariableDeclarationStatement(
							typeof(float), "local"+i, new CodePrimitiveExpression(0.0f)));
				}
			}

			CodeVariableReferenceExpression ref_i = new CodeVariableReferenceExpression("i");

			if (addMethod) // add methods have indices to update...
				ref_i.VariableName = "_i";

			CodeIterationStatement loop = new CodeIterationStatement();
			loop.InitStatement = new CodeVariableDeclarationStatement(typeof(uint), ref_i.VariableName, new CodePrimitiveExpression(0));
			loop.IncrementStatement = new CodeAssignStatement(ref_i,new CodeBinaryOperatorExpression(ref_i, CodeBinaryOperatorType.Add, new CodePrimitiveExpression((uint)1)));
			loop.TestExpression = new CodeBinaryOperatorExpression(ref_i, CodeBinaryOperatorType.LessThan, new CodeArgumentReferenceExpression("count"));

			// uint i = indices[_i];
			if (addMethod)
				loop.Statements.Add(new CodeVariableDeclarationStatement(typeof(uint), "i", new CodeArrayIndexerExpression(new CodeArgumentReferenceExpression("indices"), ref_i)));
			else
			{
				//put in velocity logic

				loop.Statements.Add(
					new CodeAssignStatement(Arg("position.x"),
						new CodeBinaryOperatorExpression(Arg("position.x"), CodeBinaryOperatorType.Add,
							new CodeBinaryOperatorExpression(Arg("velocity.x"), CodeBinaryOperatorType.Multiply, Arg("delta_time")))));

				loop.Statements.Add(
					new CodeAssignStatement(Arg("position.y"),
						new CodeBinaryOperatorExpression(Arg("position.y"), CodeBinaryOperatorType.Add,
							new CodeBinaryOperatorExpression(Arg("velocity.y"), CodeBinaryOperatorType.Multiply, Arg("delta_time")))));

				loop.Statements.Add(
					new CodeAssignStatement(Arg("position.z"),
						new CodeBinaryOperatorExpression(Arg("position.z"), CodeBinaryOperatorType.Add,
							new CodeBinaryOperatorExpression(Arg("velocity.z"), CodeBinaryOperatorType.Multiply, Arg("delta_time")))));
			}

			loop.Statements.AddRange(code);

			statements.Add(loop);
		}

		private static void BuildLogic(CodeStatementCollection statements, ReadOnlyArrayCollection<ParticleSystemLogicStep> steps, Dictionary<string, bool> argUsed, int depth)
		{
			for (int i = 0; i < steps.Length; i++)
			{
				ParticleSystemLogicStep step = steps[i];

				CodeExpression arg0		= Arg(step.arg0);
				CodeExpression arg1		= Arg(step.arg1);
				CodeExpression argT		= Arg(step.target);
				CodeExpression target	= Arg(step.target);

				if (arg0 != null && arg0 is CodePrimitiveExpression == false)
					argUsed[step.arg0] = true;
				if (arg1 != null && arg1 is CodePrimitiveExpression == false)
					argUsed[step.arg1] = true;
				if (target != null && target is CodePrimitiveExpression == false)
					argUsed[step.target] = true;

				if (step.Children.Length > 0)
				{
					CodeVariableReferenceExpression local = new CodeVariableReferenceExpression("i" + depth);

					if (step.type == "loop")
					{
						CodeIterationStatement iteration = new CodeIterationStatement();

						iteration.InitStatement = new CodeVariableDeclarationStatement(typeof(uint), local.VariableName, new CodePrimitiveExpression((uint)0));
						iteration.TestExpression = new CodeBinaryOperatorExpression(local, CodeBinaryOperatorType.LessThan, arg0);
						iteration.IncrementStatement = new CodeAssignStatement(local, new CodeBinaryOperatorExpression(local, CodeBinaryOperatorType.Add, new CodePrimitiveExpression((uint)1)));

						BuildLogic(iteration.Statements, step.Children, argUsed, depth + 1);
						statements.Add(iteration);
					}
					else
					{
						CodeBinaryOperatorType op = CodeBinaryOperatorType.IdentityEquality;
						//if
						switch (step.type)
						{
							case "if_notequal":
								op = CodeBinaryOperatorType.IdentityInequality;
								break;
							case "if_lessequal":
								op = CodeBinaryOperatorType.LessThanOrEqual;
								break;
							case "if_greaterequal":
								op = CodeBinaryOperatorType.GreaterThanOrEqual;
								break;
							case "if_less":
								op = CodeBinaryOperatorType.LessThan;
								break;
							case "if_greater":
								op = CodeBinaryOperatorType.GreaterThan;
								break;
						}

						CodeConditionStatement condition = new CodeConditionStatement();
						condition.Condition = new CodeBinaryOperatorExpression(
							arg0, op, arg1);


						BuildLogic(condition.TrueStatements, step.Children, argUsed, depth + 1);

						statements.Add(condition);
					}
				}
				else
				{
					CodeExpression expression = null;

					switch (step.type)
					{
						case "set":
							expression = arg0;
							break;
						case "add":
							expression = Op(CodeBinaryOperatorType.Add, argT, arg0, arg1);
							break;
						case "sub":
							expression = Op(CodeBinaryOperatorType.Subtract, argT, arg0, arg1);
							break;
						case "mul":
							expression = Op(CodeBinaryOperatorType.Multiply, argT, arg0, arg1);
							break;
						case "div":
							expression = Op(CodeBinaryOperatorType.Divide, argT, arg0, arg1);
							break;
						case "abs":
						case "sign":
						case "cos":
						case "sin":
						case "acos":
						case "asin":
						case "tan":
						case "atan":
						case "sqrt":
							expression = MathOp1(step.type, arg0);
							break;
						case "atan2":
						case "min":
						case "max":
							expression = MathOp2(step.type, arg0, arg1);
							break;
						case "saturate":
							expression =
								MathOp2("max",
									new CodePrimitiveExpression(0.0f),
										MathOp2("min", new CodePrimitiveExpression(1.0f),
											arg0));
							break;
						case "rsqrt":
							expression =
									new CodeBinaryOperatorExpression(
										new CodePrimitiveExpression(1.0f), CodeBinaryOperatorType.Divide,
											MathOp1("sqrt", arg0));
							break;
						case "madd":
							expression =
									new CodeBinaryOperatorExpression(argT,
										CodeBinaryOperatorType.Add,
											new CodeBinaryOperatorExpression(arg0,
												CodeBinaryOperatorType.Multiply,
													arg1));
							break;
						case "rand":
							argUsed["rand"] = true;
							expression = Rand(arg0, arg1, statements);
							break;
						case "rand_smooth":
							argUsed["rand"] = true;
							expression = RandSmooth(arg0, arg1, statements);
							break;
					}

					if (expression == null)
						throw new ArgumentNullException("Unknown statement " + step.type);

					statements.Add(new CodeAssignStatement(target,
						expression));
				}
			}
		}

		private static CodeExpression rand = new CodeVariableReferenceExpression("_tmp");

		private static CodeExpression RandDouble()
		{
			return	new CodeCastExpression(typeof(float),
						new CodeMethodInvokeExpression(
							new CodeMethodReferenceExpression(
								new CodeArgumentReferenceExpression("rand"),
									"NextDouble")));
		}

		private static CodeExpression Rand(CodeExpression arg0, CodeExpression arg1, CodeStatementCollection statements)
		{
			statements.Add(new CodeAssignStatement(rand,RandDouble()));

			if (arg1 == null)
				return new CodeBinaryOperatorExpression(
					arg0, CodeBinaryOperatorType.Multiply, rand);

			//arg0 + rand * (arg1 - arg0);

			return new CodeBinaryOperatorExpression(
				arg0, CodeBinaryOperatorType.Add,
					new CodeBinaryOperatorExpression(
						rand, CodeBinaryOperatorType.Multiply,
							new CodeBinaryOperatorExpression(
								arg1, CodeBinaryOperatorType.Subtract, arg0)));
		}

		private static CodeExpression RandSmooth(CodeExpression arg0, CodeExpression arg1, CodeStatementCollection statements)
		{
//			_rand = ((float)random.NextDouble() - 0.5f);
//			_rand = _rand * Math.Abs(_rand) + (_rand + 1) * 0.5f;

			//_rand = ((float)r.NextDouble() - 0.5f);
			statements.Add(
				new CodeAssignStatement(rand,
					new CodeBinaryOperatorExpression(
						RandDouble(),
							CodeBinaryOperatorType.Subtract,
								new CodePrimitiveExpression(0.5f))));

//			_rand = _rand * Math.Abs(_rand) + (_rand + 1) * 0.5f;

			statements.Add(
				new CodeAssignStatement(rand,
					new CodeBinaryOperatorExpression(
						new CodeBinaryOperatorExpression(
							rand, CodeBinaryOperatorType.Multiply, 
								MathOp1("abs",rand)),
									CodeBinaryOperatorType.Add,
										new CodeBinaryOperatorExpression(
											new CodeBinaryOperatorExpression(
												rand, CodeBinaryOperatorType.Add,
													new CodePrimitiveExpression(1.0f)),
														CodeBinaryOperatorType.Multiply,
															new CodePrimitiveExpression(0.5f)))));
													

			if (arg1 == null)
				return new CodeBinaryOperatorExpression(
					arg0, CodeBinaryOperatorType.Multiply, rand);

			//arg0 + rand * (arg1 - arg0);

			return new CodeBinaryOperatorExpression(
				arg0, CodeBinaryOperatorType.Add,
					new CodeBinaryOperatorExpression(
						rand, CodeBinaryOperatorType.Multiply,
							new CodeBinaryOperatorExpression(
								arg1, CodeBinaryOperatorType.Subtract, arg0)));
		}

		private static CodeExpression MathOp1(string name, CodeExpression arg)
		{
			char[] chars = name.ToCharArray();
			if (chars.Length > 0)
				chars[0] = char.ToUpper(chars[0]);
			name = new string(chars);

			return	new CodeCastExpression(typeof(float),
						new CodeMethodInvokeExpression(
							new CodeMethodReferenceExpression(
								new CodeTypeReferenceExpression(typeof(Math)), name),
								arg));
		}

		private static CodeExpression MathOp2(string name, CodeExpression arg0, CodeExpression arg1)
		{
			char[] chars = name.ToCharArray();
			if (chars.Length > 0)
				chars[0] = char.ToUpper(chars[0]);
			name = new string(chars);

			return	new CodeCastExpression(typeof(float),
						new CodeMethodInvokeExpression(
							new CodeMethodReferenceExpression(
								new CodeTypeReferenceExpression(typeof(Math)), name),
								arg0,arg1));
		}

		private static CodeExpression Op(CodeBinaryOperatorType op, CodeExpression target, CodeExpression arg0, CodeExpression arg1)
		{
			if (arg1 == null)
				return new CodeBinaryOperatorExpression(target, op, arg0);
			else
				return new CodeBinaryOperatorExpression(arg0, op, arg1);
		}
	}

#endif
}
