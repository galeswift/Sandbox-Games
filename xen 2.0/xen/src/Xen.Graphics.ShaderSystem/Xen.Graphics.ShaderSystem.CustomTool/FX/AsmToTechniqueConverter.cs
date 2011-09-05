using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Xen.Graphics.ShaderSystem.CustomTool.FX
{
#if USING_XNA_FX

    public static class AsmToEffectConverter
    {

		public static byte[] Generate(AsmTechnique technique, Platform platform)
		{
			string techniqueCode;
		
			AsmToHlslConverter vsConvert = new AsmToHlslConverter(technique.VertexShader, "vs", platform, technique.VertexShader.RegisterSet.FloatRegisterCount, technique.VertexShader.RegisterSet.BooleanRegisterCount);
			AsmToHlslConverter psConvert = new AsmToHlslConverter(technique.PixelShader, "ps", platform, technique.PixelShader.RegisterSet.FloatRegisterCount, technique.PixelShader.RegisterSet.BooleanRegisterCount);

			if (platform == Platform.Windows)
			{
				StringBuilder constantSetup = new StringBuilder();
			//	technique.ConstructTechniqueConstants(constantSetup, false);

				string vsAsm = technique.VertexShader.ToString();
				vsAsm = vsAsm.Replace(Environment.NewLine, Environment.NewLine + "\t\t\t\t");

				string psAsm = technique.PixelShader.ToString();
				psAsm = psAsm.Replace(Environment.NewLine, Environment.NewLine + "\t\t\t\t");

				techniqueCode = string.Format(
@"
uniform float4 _vs_c[11] : register(vs, c0);
{4}
technique Shader
{0}
	pass
	{0}
		VertexShader = 
			asm 
			{0}
				{2}
			{1};
		PixelShader  = 
			asm 
			{0}
				{3}
			{1};
	{1}
{1}",
				"{", "}", vsAsm, psAsm, constantSetup);

			}
			else
			{
				string vsSource = vsConvert.GetSource();
				string psSource = psConvert.GetSource();

				//setup the technique.
				techniqueCode = string.Format(
	@"
{2}

{3}

technique Shader
{0}
	pass
	{0}
		VertexShader = compile {4} vsMain();
		PixelShader  = compile {5} psMain();
	{1}
{1}",
				"{", "}", vsSource, psSource, vsConvert.GetProfile().ToString().ToLower(), psConvert.GetProfile().ToString().ToLower());

			}
			
			TargetPlatform target = TargetPlatform.Unknown;
			switch (platform)
			{
				case Platform.Both:
					throw new ArgumentException();
				case Platform.Windows:
					target = TargetPlatform.Windows;
					break;
				case Platform.Xbox:
					target = TargetPlatform.Xbox360;
					break;
			}

			CompiledEffect effectSource = Effect.CompileEffectFromSource(techniqueCode, null, null, CompilerOptions.None, target);

			if (effectSource.Success == false)
				Common.ThrowError(effectSource.ErrorsAndWarnings, techniqueCode);

			byte[] code = effectSource.GetEffectCode();

			Effect effect = new Effect(Graphics.GraphicsDevice, code, CompilerOptions.None, new EffectPool());

			Vector4[] valuesV = new Vector4[11];
			for (int i = 0; i < valuesV.Length; i++)
			{
				valuesV[i] = new Vector4(i,0,0,0);
			}
			//Vector4[] valuesP = new Vector4[24];
			//for (int i = 0; i < valuesP.Length; i++)
			//{
			//    valuesP[i] = new Vector4(0, i, 0, 0);
			//}
			effect.Parameters[0].SetValue(valuesV);
			Vector4[] test = effect.Parameters[0].GetValueVector4Array(11);
			//effect.Parameters[1].SetValue(valuesP);
			//effect.Parameters[2].SetValue(new bool[] { false, false });

			GraphicsDevice gd = Graphics.GraphicsDevice;

			effect.CurrentTechnique = effect.Techniques[0];
			effect.Begin();
			effect.Techniques[0].Passes[0].Begin();


			Vector4[] outputV = gd.GetVertexShaderVector4ArrayConstant(0, 255);
			Vector4[] outputP = gd.GetPixelShaderVector4ArrayConstant(0, 24);
			//bool[] outputPB = gd.GetPixelShaderBooleanConstant(0, 2);

			effect.Techniques[0].Passes[0].End();
			effect.End();

			string asm = effect.Disassemble(false);

			return code;
        }
    }

	class AsmToHlslConverter
	{
		//wrapper for a shader input or output
		struct InputOutput
		{
			public InputOutput(string name, string mapping)
			{
				this.name = name;
				this.mapping = mapping;
				this.index = 0;
				this.size = 4;
			}
			public InputOutput(string name, string mapping, int index)
			{
				this.name = name;
				this.mapping = mapping;
				this.index = index;
				this.size = 4;
			}
			public InputOutput(string name, string mapping, int index, int size)
			{
				this.name = name;
				this.mapping = mapping;
				this.index = index;
				this.size = size;
			}
			public string name;
			public string mapping;
			//may be -1 if can't be appended
			public int index;
			public int size;
		}

		private AsmListing listing;
		private int maxConstant, maxRegister, maxInteger, maxBoolean;
		private bool useTemp;
		private int loopIndex;

		//shaders sometimes assign constant array values, 
		//if this happens, convert them to temps
        private readonly Dictionary<int, bool> assignedConstants;

        private readonly List<InputOutput> samplers;
        private readonly List<InputOutput> inputs, outputs;
        private readonly StringBuilder source;
        private readonly Dictionary<int, string> localConstants;
        private readonly Dictionary<int, string> localIntegerConstants;
        private readonly Dictionary<int, string> localBooleanConstants;

        private static readonly string[] VSprofiles = new string[] { "vs_1_1", "vs_2_0", "vs_2_a", "vs_3_0" };
        private static readonly string[] PSprofiles = new string[] { "ps_2_0", "ps_2_a", "ps_2_b", "ps_3_0" };

        private static readonly ShaderProfile[] VSprofileValues = new ShaderProfile[] { ShaderProfile.VS_1_1, ShaderProfile.VS_2_0, ShaderProfile.VS_2_A, ShaderProfile.VS_3_0 };
        private static readonly ShaderProfile[] PSprofileValues = new ShaderProfile[] { ShaderProfile.PS_2_0, ShaderProfile.PS_2_A, ShaderProfile.PS_2_B, ShaderProfile.PS_3_0 };

        private ShaderProfile profile;
        private readonly Platform platform;
        private readonly string prefix;


        public AsmToHlslConverter(AsmListing asmSource, string prefix, Platform platform, int shaderMaxConstants, int shaderMaxBooleanConstants)
		{
            this.platform = platform;
            this.prefix = prefix;

			maxConstant = -1;
			if (shaderMaxConstants != 0)
				maxConstant = shaderMaxConstants;
			maxBoolean = -1;
			if (shaderMaxBooleanConstants != 0)
				maxBoolean = shaderMaxBooleanConstants;
			maxRegister = -1;

			this.samplers = new List<InputOutput>();
			this.inputs = new List<InputOutput>();
			this.outputs = new List<InputOutput>();
			this.assignedConstants = new Dictionary<int, bool>();
			this.localConstants = new Dictionary<int, string>();
			this.localBooleanConstants = new Dictionary<int, string>();
			this.localIntegerConstants = new Dictionary<int, string>();

			for (int i = 0; i < 256; i++)
				assignedConstants.Add(i, false);

			this.source = new StringBuilder();

			this.listing = asmSource;

			if (listing.GetCommandCount() > 0)
			{
				//attempt to decode the shader
				if (DetectProfile())
				{
					List<Command> commands = new List<Command>();
					for (int i = 1; i < listing.GetCommandCount(); i++)
					{
						//extract the commands
						Command cmd = new Command(listing.GetCommand(i));
						if (cmd.name != null)
							commands.Add(cmd);
					}

					List<Command> allCommands = new List<Command>();

					List<Command> newCommands = new List<Command>();
					foreach (Command command in commands)
					{
						newCommands.Clear();
						GenerateCode(command, newCommands);
						allCommands.AddRange(newCommands);
					}
					commands = allCommands;

					bool isAsm = false;

					foreach (Command cmd in commands)
					{
						if (cmd.isAsm != isAsm)
						{
							if (isAsm)
							{
								this.source.Append("};");
								this.source.AppendLine();
							}
							else
							{
								this.source.Append("asm{");
								this.source.AppendLine();
							}
							isAsm = cmd.isAsm;
						}

						this.source.Append(cmd.name);
						for (int i = 0; i < cmd.args.Length; i++)
						{
							this.source.Append(' ');
							if (i != 0)
								this.source.Append(',');
							for (int a = 0; a < cmd.args[i].Length; a++)
								this.source.Append(cmd.args[i][a]);
						}
						this.source.AppendLine();
					}
					if (isAsm)
						this.source.Append("};");
				}
			}

			BuildMethod();

            /*
			CompiledShader shader =
				ShaderCompiler.CompileFromSource(
					this.source.ToString(),
					null, null, CompilerOptions.AvoidFlowControl, "Main",
					profile, platform);

			if (throwOnError && !shader.Success)
				Common.ThrowError("An error occured running the Xbox shader HLSL/ASM preprocessor", shader.ErrorsAndWarnings, asmSource.ToString());

			if (!shader.Success)
			{
				//tried the best.. if it failed, ohh well, go back to AssembleFromSource
				//probably used complex flow control?
				string rawAsm = asmSource.ToString();

				shader = ShaderCompiler.AssembleFromSource(rawAsm, null, null, CompilerOptions.None, platform);

				if (!shader.Success)
					Common.ThrowError(shader.ErrorsAndWarnings, rawAsm);
			}

			output = shader.GetShaderCode();
            */
		}

        public string GetSource()
        {
            return this.source.ToString();
        }

		public ShaderProfile GetProfile()
		{
			return profile;
		}

		private void BuildMethod()
		{
			string[] sourceLines = source.ToString().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

			source.Length = 0;

			if (maxConstant > 0)
			{
				source.Append("float4 _");
                source.Append(prefix);
				source.Append("_c[");
				source.Append(maxConstant);
				source.AppendLine("] : register(c0);");
			}
			if (maxBoolean > 0)
			{
				source.Append("bool _");
                source.Append(prefix);
                source.Append("_b[");
				source.Append(maxBoolean);
				source.AppendLine("] : register(b0);");
			}

			foreach (InputOutput sampler in samplers)
			{
				source.Append("sampler");
				source.Append(sampler.mapping);
				source.Append(" _");
				source.Append(prefix);
				source.Append("_");
				source.Append(sampler.name);
				source.Append(" : register(s");
				source.Append(sampler.index);
				source.AppendLine(");");
			}

			source.Append("void ");
            source.Append(prefix);
			source.AppendLine("Main(");
			bool first = true;
			foreach (InputOutput input in inputs)
			{
				if (!first)
					source.AppendLine(",");
				first = false;

				source.Append("\t\tin\tfloat");
				if (input.size > 1)
					source.Append(input.size);
				source.Append("\t_");
				source.Append(input.name);
				source.Append(" : ");
				source.Append(input.mapping);
				if (input.index != -1)
					source.Append(input.index);
			}

			foreach (InputOutput output in outputs)
			{
				if (!first)
					source.AppendLine(",");
				first = false;

				source.Append("\t\tout\tfloat");
				if (output.size > 1)
					source.Append(output.size);
				source.Append("\t_");
				source.Append(output.name);
				source.Append(" : ");
				source.Append(output.mapping);
				if (output.index != -1)
					source.Append(output.index);
			}
			source.AppendLine(")");
			source.AppendLine("{");

			foreach (InputOutput output in outputs)
			{
				if (output.index != -1)
				{
					source.Append("\t_");
					source.Append(output.name);
					source.AppendLine(" = 0;");
				}
			}

			foreach (KeyValuePair<int, string> kvp in localConstants)
			{
				source.Append("\tfloat4 _");
				source.Append(prefix);
				source.Append("_c");
				source.Append(kvp.Key);
				source.Append(" = float4(");
				source.Append(kvp.Value);
				source.AppendLine(");");
			}
			foreach (KeyValuePair<int, string> kvp in localBooleanConstants)
			{
				source.Append("\tbool _");
				source.Append(prefix);
				source.Append("_b");
				source.Append(kvp.Key);
				source.Append(" = (bool)(");
				source.Append(kvp.Value);
				source.AppendLine(");");
			}
			foreach (KeyValuePair<int, string> kvp in localIntegerConstants)
			{
				source.Append("\tint4 _i");
				source.Append(kvp.Key);
				source.Append(" = int4(");
				source.Append(kvp.Value);
				source.AppendLine(");");
			}

			if (maxRegister > 0)
			{
				for (int i = 0; i < maxRegister; i++)
				{
					source.Append("\tfloat4 _r");
					source.Append(i);
					source.AppendLine(" = 0;");
				}
			}
			if (maxInteger > 0)
			{
				for (int i = 0; i < maxInteger; i++)
				{
					source.Append("\tint4 _a");
					source.Append(i);
					source.AppendLine(" = 0;");
				}
			}
			if (useTemp)
				source.AppendLine("\tfloat4 _tmp = 0;");

			foreach (string line in sourceLines)
			{
				source.Append('\t');
				source.AppendLine(line);
			}
			source.AppendLine("}");
		}

		private void GenerateCode(Command command, List<Command> insertList)
		{
			switch (command.name)
			{
				case "dcl_2d":
				case "dcl_cube":
				case "dcl_volume":
					//sampler ahoy

					string name = command.args[0][0];

					InputOutput sampler = new InputOutput();
					sampler.index = int.Parse(name.Substring(1));
					sampler.mapping = command.name.Substring(4).ToUpper();
					sampler.name = name;

					this.samplers.Add(sampler);
					return;
			}

			if (command.name == "dcl")
			{
				string type = "TEXCOORD";

				string name = command.args[0][0];

				if (name.EndsWith("_pp")) //xbox doens't support partial precision
					name = name.Substring(0, name.Length - 3);

				if ((profile == ShaderProfile.PS_2_0 ||
					profile == ShaderProfile.PS_2_A ||
					profile == ShaderProfile.PS_2_B) &&
					name.Length > 0 && name[0] == 'v')
				{
					type = "COLOR";
				}

				//pixel shader tex coord input
				inputs.Add(new InputOutput(name, type, int.Parse(name.Substring(1))));
				return;
			}

			if (command.name.StartsWith("dcl_"))
			{
				//vertex shader declare

				string name = command.name.Substring(4);

				if (name.EndsWith("_pp")) //xbox doens't support partial precision
					name = name.Substring(0, name.Length - 3);

				//last two digits may be a number
				int index = 0;
				if (name.Length > 0 && char.IsNumber(name[name.Length - 1]))
				{
					if (name.Length > 1 && char.IsNumber(name[name.Length - 2]))
					{
						index = int.Parse(name.Substring(name.Length - 2));
						name = name.Substring(0, name.Length - 2);
					}
					else
					{
						index = int.Parse(name[name.Length - 1].ToString());
						name = name.Substring(0, name.Length - 1);
					}
				}

				//if (command.args.Length != 1 || command.args[0].Length != 1)
				//    throw new Exception(); //will be caught, and shader will not be replaced

				InputOutput input = new InputOutput();
				input.mapping = name.ToUpper();
				input.index = index;
				input.size = 4;
				input.name = command.args[0][0];

				if (input.name.Length > 0 && input.name[0] == 'o')
					outputs.Add(input);
				else
					inputs.Add(input);
				return;
			}

			//everything else should be some form of shader op.
			//so run it through the ASM converter...


			if (ProcessCommand(ref command, insertList))
				insertList.Add(command);
		}

		private string GetSamplerType(string[] sampler)
		{
			//remove non-numbers from the sampler string
			string smp = sampler[0];
			int number = 0;
			for (int i = 0; i < smp.Length; i++)
			{
				if (char.IsNumber(smp[i]))
				{
					number *= 10;
					number += int.Parse(smp[i].ToString());
				}
			}
			return this.samplers[number].mapping;
		}

		class Command
		{
			public string name;
			public string[][] args;
			public readonly bool isAsm = true;
			public Command Clone(string cmd)
			{
				Command clone = new Command();
				clone.name = cmd;
				clone.args = new string[this.args.Length][];
				for (int i = 0; i < this.args.Length; i++)
					clone.args[i] = (string[])this.args[i].Clone();
				return clone;
			}
			public Command Clone()
			{
				return (Command)MemberwiseClone();
			}
			private Command()
			{
			}
			public Command(AsmCommand command)
			{
				this.name = command.Target;
				args = new string[command.OpCount][];
				for (int i = 0; i < command.OpCount; i++)
				{
					args[i] = new string[command.GetOp(i).TokenCount];
					for (int t = 0; t < command.GetOp(i).TokenCount; t++)
					{
						string token = command.GetOp(i).GetToken(t);
						args[i][t] = token;
					}
				}
			}
			public Command(string hlsl, params string[] elements)
			{
				if (elements.Length > 0)
					hlsl = string.Format(hlsl, elements);
				this.name = hlsl;
				this.args = new string[0][];
				isAsm = false;
			}
			public string Arg(int index)
			{
				if (args[index].Length == 0)
					return "";
				string str = args[index][0];
				for (int i = 1; i < args[index].Length; i++)
					str += args[index][i];
				return str;
			}
			public string ArgNoSwizzle(int index)
			{
				if (args[index].Length == 0)
					return "";
				string str = args[index][0];
				for (int i = 1; i < args[index].Length; i++)
				{
					if (args[index][i] == ".")
						break;
					str += args[index][i];
				}
				return str;
			}
			public int Index(int index)
			{
				return int.Parse(args[index][0].Substring(1));
			}
			public string Swizzle(int index)
			{
				if (args[index].Length == 0)
					return "";
				string str = "";
				bool swiz = false;
				for (int i = 1; i < args[index].Length; i++)
				{
					if (args[index][i] == ".")
						swiz = true;
					if (swiz)
						str += args[index][i];
				}
				return str;
			}
			public string ArrayIndex(int index)
			{
				string array = null;

				foreach (string str in args[index])
				{
					if (str == "]")
						return array;
					if (array != null)
					{
						if (str.Length > 1 && !char.IsNumber(str[0]) && char.IsNumber(str[1]))
							array += '_';
						array += str;
					}
					if (str == "[")
						array = "";
				}
				return null;
			}
			public void StripArray(int index)
			{
				List<string> values = new List<string>();
				bool inArray = false;
				foreach (string str in args[index])
				{
					if (str == "[")
						inArray = true;
					if (!inArray)
						values.Add(str);
					if (str == "]")
						inArray = false;
				}
				args[index] = values.ToArray();
			}
		}


		bool DetectProfile()
		{
			AsmCommand command = listing.GetCommand(0);

			for (int i = 0; i < VSprofiles.Length; i++)
			{
				if (VSprofiles[i] == command.Target)
				{
					profile = VSprofileValues[i];
					return true;
				}
			}

			for (int i = 0; i < PSprofiles.Length; i++)
			{
				if (PSprofiles[i] == command.Target)
				{
					profile = PSprofileValues[i];
					return true;
				}
			}
			return false;
		}



		private void ExtractArg(ref string arg, string indexer)
		{
			if (arg.Length == 0)
				return;

			char start = arg[0];

			//constant
			if (start == 'c')
			{
				if (arg.Length > 1 && char.IsNumber(arg[1]))
				{
					int number = int.Parse(arg.Substring(1));
					maxConstant = Math.Max(maxConstant, number + 1);

					if (indexer == null)
					{
						if (!localConstants.ContainsKey(number))
							arg = "c[" + arg.Substring(1) + "]";
					}
					else
					{
						arg = "c[" + arg.Substring(1) + " + " + indexer + "]";
					}
				}
			}
			else
			{
				//temp register
				if (start == 'r')
				{
					int number = int.Parse(arg.Substring(1)) + 1;
					maxRegister = Math.Max(maxRegister, number);
				}
				//integer index
				if (start == 'a')
				{
					int number = int.Parse(arg.Substring(1)) + 1;
					maxInteger = Math.Max(maxInteger, number);

					if (indexer == null)
					{
						if (!localIntegerConstants.ContainsKey(number))
							arg = "a[" + arg.Substring(1) + "]";
					}
					else
					{
						arg = "a[" + arg.Substring(1) + " + " + indexer + "]";
					}
				}
				//integer index
				if (start == 'b')
				{
					int number = int.Parse(arg.Substring(1)) + 1;
					maxBoolean = Math.Max(maxBoolean, number);

					if (indexer == null)
					{
						if (!localBooleanConstants.ContainsKey(number))
							arg = "b[" + arg.Substring(1) + "]";
					}
					else
					{
						arg = "b[" + arg.Substring(1) + " + " + indexer + "]";
					}
				}

				//output
				if (start == 'o')
				{
					//this varies between shader models...
					//

					bool addOutput = true;

					for (int i = 0; i < outputs.Count; i++)
					{
						if (outputs[i].name == arg)
							addOutput = false;
					}

					if (addOutput)
					{
						switch (arg)
						{
							case "oDepth":
								outputs.Add(new InputOutput(arg, "DEPTH", -1, 1)); // may not have a index appended, must be float.
								break;
							case "oPos":
								outputs.Add(new InputOutput(arg, "POSITION"));
								break;
							case "oFog":
								outputs.Add(new InputOutput(arg, "FOG"));
								break;
							case "oPts":
								outputs.Add(new InputOutput(arg, "PSIZE"));
								break;
							case "oD0":
							case "oC0":
								outputs.Add(new InputOutput(arg, "COLOR", 0));
								break;
							case "oD1":
							case "oC1":
								outputs.Add(new InputOutput(arg, "COLOR", 1));
								break;
							case "oT0":
							case "oT1":
							case "oT2":
							case "oT3":
							case "oT4":
							case "oT5":
							case "oT6":
							case "oT7":
								outputs.Add(new InputOutput(arg, "TEXCOORD", int.Parse(arg[2].ToString())));
								break;
						}
					}
				}
			}

			if (start == 'c' || start == 'b' || start == 's')
			{
				arg = prefix + "_" + arg;
			}

		}





		bool ProcessCommand(ref Command cmd, List<Command> insertBefore)
		{
			bool isSaturated = cmd.name.EndsWith("_sat");

			if (isSaturated)
				cmd.name = cmd.name.Substring(0, cmd.name.Length - 4);

            if (this.platform == Platform.Xbox)
            {
                //modify the commands to work with local variables,
                //and deal with asm commands not supported on the xbox

                if (cmd.name.EndsWith("_pp")) // no partial precision on the xbox
                    cmd.name = cmd.name.Substring(0, cmd.name.Length - 3);

                if (cmd.name == "def")
                {
                    //special case, local constants

                    if (cmd.args.Length == 5 &&
                        cmd.args[0][0].Length > 1 &&
                        cmd.args[0][0][0] == 'c')
                    {
                        int index = int.Parse(cmd.args[0][0].Substring(1));

                        string value = cmd.Arg(1) + "," + cmd.Arg(2) + "," + cmd.Arg(3) + "," + cmd.Arg(4);

                        localConstants.Add(index, value);
                        return false;
                    }
                }
                if (cmd.name == "defi")
                {
                    //integer constant variation

                    if (cmd.args.Length == 5 &&
                        cmd.args[0][0].Length > 1 &&
                        cmd.args[0][0][0] == 'i')
                    {
                        int index = int.Parse(cmd.args[0][0].Substring(1));
                        string value = cmd.Arg(1) + "," + cmd.Arg(2) + "," + cmd.Arg(3) + "," + cmd.Arg(4);

                        localIntegerConstants.Add(index, value);
                        return false;
                    }
                }
                if (cmd.name == "defb")
                {
                    //boolean constant variation

                    if (cmd.args.Length == 2 &&
                        cmd.args[0][0].Length > 1 &&
                        cmd.args[0][0][0] == 'b')
                    {
                        int index = int.Parse(cmd.args[0][0].Substring(1));
                        string value = cmd.Arg(1);

                        localBooleanConstants.Add(index, value);
                        return false;
                    }
                }

                for (int i = 0; i < cmd.args.Length; i++)
                {
                    int firstIndex = 0;
                    for (int a = 0; a < cmd.args[i].Length; a++)
                    {
                        //some command inputs specify absolute value, which is a bit tricky here
                        string arg = cmd.args[i][a];

                        if (arg == "-")
                            firstIndex = 1;

                        if (a == firstIndex && arg.Length > 0 && char.IsNumber(arg[0]) == false)
                        {
                            if (arg.EndsWith("_abs"))
                            {
                                arg = arg.Substring(0, arg.Length - 4);

                                insertBefore.Add(new Command("_tmp = (float4)abs(_{0});", arg));

                                arg = "tmp";
                                useTemp = true;
							}

                            string indexer = cmd.ArrayIndex(i);
                            ExtractArg(ref arg, indexer);
                            if (indexer != null)
                                cmd.StripArray(i);

                            arg = "_" + arg;
                        }

                        cmd.args[i][a] = arg;
                    }
                }

                //These are the commands that do not exist in Xbox assembly (XPS / XVS)
                //They may have equivalents, however for the most part, their HLSL version is used.
                switch (cmd.name)
                {
                    //It seems that the xbox doesn't accept swizzles in parts of some operators. Yet XNA doesn't report an error. Great.
                    case "mad":
                        //mad dst, src0, src1, src2
                        cmd = new Command("{0} = ((float4)({1} * {2} + {3})){4};", cmd.Arg(0), cmd.Arg(1), cmd.Arg(2), cmd.Arg(3), cmd.Swizzle(0));
                        break;
                    case "dp4":
                        cmd = new Command("{0} = ((float4)dot({1},{2})){3};", cmd.Arg(0), cmd.Arg(1), cmd.Arg(2), cmd.Swizzle(0));
                        break;
                    case "dp3":
                        cmd = new Command("{0} = ((float4)dot((float3){1},(float3){2})){3};", cmd.Arg(0), cmd.Arg(1), cmd.Arg(2), cmd.Swizzle(0));
                        break;
                    case "mul":
                        cmd = new Command("{0} = ((float4)({1} * {2})){3};", cmd.Arg(0), cmd.Arg(1), cmd.Arg(2), cmd.Swizzle(0));
                        break;
                    case "add":
                        cmd = new Command("{0} = ((float4)({1} + {2})){3};", cmd.Arg(0), cmd.Arg(1), cmd.Arg(2), cmd.Swizzle(0));
                        break;
                    case "sub":
                        cmd = new Command("{0} = ((float4)({1} - {2})){3};", cmd.Arg(0), cmd.Arg(1), cmd.Arg(2), cmd.Swizzle(0));
                        break;

                    case "rep":
                        //a loop, on an integer constant.
                        cmd = new Command("for (int rep_i{0}=0; rep_i{0}<{1}.x; rep_i{0}++) {2}", (loopIndex++).ToString(), cmd.Arg(0), "{");
                        break;
                    case "loop":
                        //a loop, using a local constant.
                        cmd = new Command("for (int {0}=0; {0}<{1}.x; {0}++) {2}", cmd.Arg(0), cmd.Arg(1), "{");
                        break;

                    //if blocks...
                    //if_gt if_lt if_ge if_le if_eq if_ne

                    case "if"://if boolean
                        cmd = new Command("if ({0}) {1}", cmd.Arg(0), "{");
                        break;
                    case "if_eq"://equal
                        cmd = new Command("if ({0} == {1}) {2}", cmd.Arg(0), cmd.Arg(1), "{");
                        break;
                    case "if_ne"://not equal
                        cmd = new Command("if ({0} != {1}) {2}", cmd.Arg(0), cmd.Arg(1), "{");
                        break;
                    case "if_gt":
                        cmd = new Command("if ({0} > {1}) {2}", cmd.Arg(0), cmd.Arg(1), "{");
                        break;
                    case "if_ge":
                        cmd = new Command("if ({0} >= {1}) {2}", cmd.Arg(0), cmd.Arg(1), "{");
                        break;
                    case "if_lt":
                        cmd = new Command("if ({0} < {1}) {2}", cmd.Arg(0), cmd.Arg(1), "{");
                        break;
                    case "if_le":
                        cmd = new Command("if ({0} <= {1}) {2}", cmd.Arg(0), cmd.Arg(1), "{");
                        break;

                    case "else":
                        cmd = new Command("{0}else{1}", "}", "{");
                        break;

                    case "endif":
                    case "endrep":
                    case "endloop":
                        //end of a loop
                        cmd = new Command("{0}", "}");
                        break;

                    case "texkill":
                        //use Clip() instead
                        cmd = new Command("clip({0});", cmd.Arg(0));
                        break;

                    case "nrm":
                        //normalize command is not supported on the xbox.
                        // nrm _r0.xyz ,_r1
                        //becomes
                        // _r0.mask = (_r1 / length(_r1.xyz)).mask

                        cmd = new Command("{0} = ({1} / length({1}.xyz)){2};", cmd.Arg(0), cmd.Arg(1), cmd.Swizzle(0));
                        break;

                    case "mova":
                        //assign the integer register to a float register
                        cmd = new Command("_a0 = ({0});", cmd.Arg(1));
                        break;

                    // 1 arg
                    case "abs":

                        cmd = new Command("{0} = ((float4){1}({2})){3};", cmd.Arg(0), cmd.name, cmd.Arg(1), cmd.Swizzle(0));
                        break;

                    // 2 arg
                    case "pow":

                        cmd = new Command("{0} = ((float4){1}({2},{3})){4};", cmd.Arg(0), cmd.name, cmd.Arg(1), cmd.Arg(2), cmd.Swizzle(0));
                        break;

                    // 3 arg
                    case "lrp":
                        //dest = src2 + src0 * (src1 - src2)

                        cmd = new Command("{0} = (((float4)({3}))+((float4)({1})) * (((float4)({2}))-((float4)({3})))){4};", cmd.Arg(0), cmd.Arg(1), cmd.Arg(2), cmd.Arg(3), cmd.Swizzle(0));
                        break;

                    case "sincos":
                        string swizzle = cmd.Swizzle(0);
                        if (swizzle.Contains('x'))
                            insertBefore.Add(new Command("{0}.x = (cos({1}));", cmd.ArgNoSwizzle(0), cmd.Arg(1)));
                        if (swizzle.Contains('y'))
                            insertBefore.Add(new Command("{0}.y = (sin({1}));", cmd.ArgNoSwizzle(0), cmd.Arg(1)));

                        return false;

                    case "slt":
                        //less than
                        cmd = new Command("{0} = ((float4)({1} < {2} ? 1.0 : 0.0)){3};", cmd.Arg(0), cmd.Arg(1), cmd.Arg(2), cmd.Swizzle(0));
                        break;

                    case "cmp":
                        cmd = new Command("{0} = ((float4)({1} >= 0.0 ? {2} : {3})){4};", cmd.Arg(0), cmd.Arg(1), cmd.Arg(2), cmd.Arg(3), cmd.Swizzle(0));
                        break;

                    case "texld":
                        cmd = new Command("{0} = (tex{1}({2},{3})){4};", cmd.Arg(0), GetSamplerType(cmd.args[2]), cmd.Arg(2), cmd.Arg(1), cmd.Swizzle(0));
                        break;
                    case "texldl":
                        cmd = new Command("{0} = (tex{1}lod({2},{3})){4};", cmd.Arg(0), GetSamplerType(cmd.args[2]), cmd.Arg(2), cmd.Arg(1), cmd.Swizzle(0));
                        break;
                    case "texldb":
                        cmd = new Command("{0} = (tex{1}bias({2},{3})){4};", cmd.Arg(0), GetSamplerType(cmd.args[2]), cmd.Arg(2), cmd.Arg(1), cmd.Swizzle(0));
                        break;
                    case "texldp":
                        cmd = new Command("{0} = (tex{1}proj({2},{3})){4};", cmd.Arg(0), GetSamplerType(cmd.args[2]), cmd.Arg(2), cmd.Arg(1), cmd.Swizzle(0));
                        break;
                    case "texldd":
                        cmd = new Command("{0} = (tex{1}grad({2},{3},{4},{5})){6};", cmd.Arg(0), GetSamplerType(cmd.args[2]), cmd.Arg(2), cmd.Arg(1), cmd.Arg(3), cmd.Arg(4), cmd.Swizzle(0));
                        break;

                    case "mov":
                        //special case, cannot directly 'mov' to DEPTH value
                        if (cmd.Arg(0) == "_oDepth")
                            cmd = new Command("{0} = ({1}){2};", cmd.Arg(0), cmd.Arg(1), cmd.Swizzle(0));

                        break;
                }
            }

			if (isSaturated)
				cmd.name = cmd.name.Replace(" = ", " = saturate");

			return true;
		}
	}

#endif
}
