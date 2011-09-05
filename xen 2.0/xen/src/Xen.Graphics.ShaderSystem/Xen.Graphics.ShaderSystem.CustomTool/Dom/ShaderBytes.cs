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
using System.IO.Compression;

namespace Xen.Graphics.ShaderSystem.CustomTool.Dom
{
	//this class stores the byte data for the shaders in use
	//it also creates and stores the vertex and pixel shader instances
	public sealed class ShaderBytes : DomBase
	{
		private byte[] effectBytesPC, effectBytesXbox;
		private readonly SourceShader source;
		private readonly AsmTechnique asmTechnique;
		private readonly HlslTechnique hlslTechnique;
		private readonly Platform platform;

		public ShaderBytes(SourceShader source, string techniqueName, Platform platform)
		{
			this.source = source;
			this.platform = platform;

			this.asmTechnique = source.GetAsmTechnique(techniqueName, platform);
			this.hlslTechnique = source.GetTechnique(techniqueName, platform);
		}

		public override void Setup(IShaderDom shader)
		{
			ExtractBytes();
		}

		private void ExtractBytes()
		{
			//generate the PC ASM
			this.effectBytesPC = TechniqueExtractor.Generate(source, hlslTechnique, Platform.Windows);
			this.effectBytesXbox = TechniqueExtractor.Generate(source, hlslTechnique, Platform.Xbox);
		}

		public override void AddMembers(IShaderDom shader, Action<CodeTypeMember, string> add, Platform platform)
		{
			//add pixel and static vertex shaders
			if (platform == Platform.Both)
			{
				//add pixel and static effect
				CodeMemberField field = new CodeMemberField(typeof(ShaderEffect), shader.EffectRef.FieldName);
				field.Attributes = MemberAttributes.Private | MemberAttributes.Final | MemberAttributes.Static;

				add(field, "Static effect container instance");
			}

			Platform? writePlatform = null;
			if (this.platform == Platform.Both)
			{
				//shader is being built for both PC and xbox
				if (platform != Platform.Both) // writing system specific shader
					writePlatform = platform;
			}
			else
			{
				//shader is being built for just one platform
				writePlatform = this.platform;
			}

			if (writePlatform == null)
				return;

			if (writePlatform.Value == Platform.Windows)
			{
				//write windows
				WriteBytes(shader, shader.EffectBytesRef.FieldName, this.effectBytesPC, add, shader.CompileDirectives, false);
			}
			else
			{
				//write xbox
				WriteBytes(shader, shader.EffectBytesRef.FieldName, this.effectBytesXbox, add, shader.CompileDirectives, true);
			}
		}


		public override void AddWarm(IShaderDom shader, Action<CodeStatement, string> add)
		{
			//dispose the shaders (if the exist)
			//dipose VS or effect
			CodeStatement disposer = shader.ETS(new CodeMethodInvokeExpression(shader.EffectRef, "Dispose"));
			add(disposer, null);
			
			//and then recreate them using the shader system

			//state.CreateShaders(out ShadowShader.vs, out ShadowShader.ps, ShadowShader.vsb, ShadowShader.psb, 23, 15, 3, 0);

			int vsInstructions = asmTechnique.VertexShader.GetCommandCount() - 1;
			int psInstructions = asmTechnique.PixelShader.GetCommandCount() - 1;


			CodeExpression create = new CodeMethodInvokeExpression(shader.ShaderSystemRef, "CreateEffect",
				new CodeDirectionExpression(FieldDirection.Out, shader.EffectRef),
				shader.EffectBytesRef,
				new CodePrimitiveExpression(vsInstructions),
				new CodePrimitiveExpression(psInstructions));

			add(shader.ETS(create), "Create the effect instance");
		}


		private void WriteBytes(IShaderDom shader, string name, byte[] data, Action<CodeTypeMember, string> add, CompileDirectives compileDirectives,bool isXbox)
		{
			//the byte array gets run through a simple compression scheme first...

			//generate the local byte array
			CodeMemberProperty property = new CodeMemberProperty();
			property.Type = new CodeTypeReference(typeof(byte[]));
			property.Name = name;
			property.Attributes = MemberAttributes.Final | MemberAttributes.Private | MemberAttributes.Static;
			property.HasGet = true;

			if (!isXbox)
			{
				using (MemoryStream stream = new MemoryStream())
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					writer.Write(data.Length);
					using (DeflateStream d = new DeflateStream(stream, CompressionMode.Compress))
					{
						d.Write(data, 0, data.Length);
						d.Flush();
					}
					data = stream.ToArray();
				}
			}
			else
			{
				//no support for system.io.compression on the xbox :-(
				data = ShaderSystemBase.SimpleCompress(data);
			}

			CodeExpression dataCode = ToArray(data, compileDirectives);

			//will be decompressed at load time
			property.GetStatements.Add(new CodeMethodReturnStatement(dataCode));

			add(property,string.Format("Static {1} compressed shader byte code ({0})", isXbox ? "Xbox360" : "Windows", isXbox ? "RLE" : "Length+DeflateStream"));
		}


		public static CodeExpression ToArray<T>(T[] data, CompileDirectives compileDirectives)
		{
			CodeExpression[] exp = new CodeExpression[data.Length];

			for (int i = 0; i < exp.Length; i++)
				exp[i] = new CodePrimitiveExpression(data[i]);

			CodeArrayCreateExpression array = new CodeArrayCreateExpression(typeof(T[]), exp);

			if (!compileDirectives.IsCSharp)
				return array;

			//optimize the array a bit.. so it's stored in a smaller space
			StringBuilder output = new StringBuilder(data.Length * 8);
			CodeGeneratorOptions options = new CodeGeneratorOptions();

			options.IndentString = "";

			using (TextWriter writer = new StringWriter(output))
				compileDirectives.CodeDomProvider.GenerateCodeFromExpression(array, writer, options);

			output.Replace(Environment.NewLine, "");

			return new CodeSnippetExpression(output.ToString());
		}
	}
}
