using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xen.Graphics.ShaderSystem.CustomTool.FX;
using System.CodeDom;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Xen.Graphics.ShaderSystem.CustomTool.Dom
{
	//this class manageds a CodeDom for a shader, such as:
	/*
	 
	public sealed class Technique : BaseShader
	{
		public override void Bind(IShaderSystem state)
		{
		}

		protected override bool Changed()
		{
		}

		protected override int[] GetShaderConstantHash(bool ps)
		{
		}

		protected override void GetVertexInput(int index, out Microsoft.Xna.Framework.Graphics.VertexElementUsage elementUsage, out int elementIndex)
		{
		}

		protected override int GetVertexInputCount()
		{
		}

		protected override void WarmShader(IShaderSystem state)
		{
		}
	}
	 

	 */


	public sealed class ShaderDom : DomBase, IShaderDom
	{
		private readonly CodeTypeDeclaration classDom;
		private readonly List<DomBase> domList;
		private readonly SourceShader source;
		private readonly string techniqueName;
		private readonly Platform platform;

		public ShaderDom(SourceShader source, string techniqueName, Platform platform, CompileDirectives directives)
		{
			this.domList = new List<DomBase>();
			this.domList.Add(this);

			//add custom dom's
			this.domList.Add(new ShaderBytes(source, techniqueName, platform));

			//needs to be passed to the registers as an interface provider
			ConstantSetup constantes = new ConstantSetup(source, techniqueName, platform);

			this.domList.Add(new ShaderRegisters(source, techniqueName, platform, constantes));
			this.domList.Add(constantes);
			this.domList.Add(new ShaderTextures(source, techniqueName, platform));

			foreach (DomBase dom in domList)
				dom.Setup(this);

			this.techniqueName = techniqueName;
			this.source = source;
			this.platform = platform;


			classDom = new CodeTypeDeclaration(techniqueName);
			classDom.IsClass = true;
			classDom.Attributes = MemberAttributes.Final | MemberAttributes.Public;
			classDom.TypeAttributes = TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.Class;


			//provide a useful comment to the class
			GenerateClassComment();


			if (source.GenerateInternalClass)
			{
				classDom.Attributes = MemberAttributes.Final | MemberAttributes.Assembly;
				classDom.TypeAttributes = TypeAttributes.NestedAssembly | TypeAttributes.Class | TypeAttributes.Sealed;
			}
			
			classDom.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(System.Diagnostics.DebuggerStepThroughAttribute))));
			classDom.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(System.CodeDom.Compiler.GeneratedCodeAttribute)),new CodeAttributeArgument(new CodePrimitiveExpression(GetType().Assembly.ManifestModule.Name)),new CodeAttributeArgument(new CodePrimitiveExpression(GetType().Assembly.ManifestModule.ModuleVersionId.ToString()))));

			classDom.BaseTypes.Add(new CodeTypeReference(typeof(BaseShader)));

			//add custom base types to the shader
			//these are defined in TechniqueExtraData
			AsmTechnique asmTechnique = this.source.GetAsmTechnique(this.techniqueName, this.platform);

			if (asmTechnique.TechniqueExtraData != null && asmTechnique.TechniqueExtraData.ClassBaseTypes != null)
			{
				foreach (string baseTypeName in asmTechnique.TechniqueExtraData.ClassBaseTypes)
					classDom.BaseTypes.Add(new CodeTypeReference(baseTypeName));
			}


			this.directives = directives;

			SetupMembers(techniqueName);
			foreach (DomBase dom in domList)
				dom.SetupMembers(this);

			CreateConstructor();
			CreateStaticGraphicsInitMethod();
			CreateBindMethod();

			CreateWarmShaderMethod();

			CreateChangedMethod();

			CreateVertexInputMethods();

			CodeTypeMemberCollection pcMembers = new CodeTypeMemberCollection();
			CodeTypeMemberCollection xboxMembers = new CodeTypeMemberCollection();

			foreach (DomBase dom in this.domList)
			{
				dom.AddMembers(this, delegate(CodeTypeMember s, string c) { Comment(s, c); classDom.Members.Add(s); }, Platform.Both);

				if (!source.DefinePlatform) // no need for specialization when the platform is constant
				{
					dom.AddMembers(this, delegate(CodeTypeMember s, string c) { Comment(s, c); pcMembers.Add(s); }, Platform.Windows);
					dom.AddMembers(this, delegate(CodeTypeMember s, string c) { Comment(s, c); xboxMembers.Add(s); }, Platform.Xbox);
				}
			}


			foreach (DomBase dom in this.domList)
			{
				dom.AddReadonlyMembers(this, 
					delegate(CodeTypeMember s, string c)
					{
						CodeTypeMember readonlySnip = directives.CreateReadOnlySnippet();
						Comment(readonlySnip ?? s, c);
						if (readonlySnip != null) classDom.Members.Add(readonlySnip);
						classDom.Members.Add(s); 
					}, Platform.Both);

				if (!source.DefinePlatform)
				{
					dom.AddReadonlyMembers(this,
						delegate(CodeTypeMember s, string c)
						{
							CodeTypeMember readonlySnip = directives.CreateReadOnlySnippet();
							Comment(readonlySnip ?? s, c);
							if (readonlySnip != null) pcMembers.Add(readonlySnip);
							pcMembers.Add(s);
						}, Platform.Windows);

					dom.AddReadonlyMembers(this,
						delegate(CodeTypeMember s, string c)
						{
							CodeTypeMember readonlySnip = directives.CreateReadOnlySnippet();
							Comment(readonlySnip ?? s, c);
							if (readonlySnip != null) xboxMembers.Add(readonlySnip);
							xboxMembers.Add(s);
						}, Platform.Xbox);
				}
			}

			if (pcMembers.Count > 0 || xboxMembers.Count > 0)
			{
				//add #if / else blocks

				classDom.Members.Add(directives.IfXboxStatement);

				foreach (CodeTypeMember type in xboxMembers)
					classDom.Members.Add(type);

				classDom.Members.Add(directives.ElseStatement);

				foreach (CodeTypeMember type in pcMembers)
					classDom.Members.Add(type);

				classDom.Members.Add(directives.EndifStatement);
			}

			//finally, create the attribute setters

			CreateSetAttributeMethod(typeof(float), "SetAttributeImpl", "attribute");
			CreateSetAttributeMethod(typeof(Vector2), "SetAttributeImpl", "attribute");
			CreateSetAttributeMethod(typeof(Vector3), "SetAttributeImpl", "attribute");
			CreateSetAttributeMethod(typeof(Vector4), "SetAttributeImpl", "attribute");
			CreateSetAttributeMethod(typeof(Matrix), "SetAttributeImpl", "attribute");
			CreateSetAttributeMethod(typeof(bool), "SetAttributeImpl", "attribute");

			CreateSetAttributeMethod(typeof(float[]), "SetAttributeImpl", "attribute");
			CreateSetAttributeMethod(typeof(Vector2[]), "SetAttributeImpl", "attribute");
			CreateSetAttributeMethod(typeof(Vector3[]), "SetAttributeImpl", "attribute");
			CreateSetAttributeMethod(typeof(Vector4[]), "SetAttributeImpl", "attribute");
			CreateSetAttributeMethod(typeof(Matrix[]), "SetAttributeImpl", "attribute");

			CreateSetAttributeMethod(typeof(Xen.Graphics.TextureSamplerState), "SetSamplerStateImpl", "sampler");
			CreateSetAttributeMethod(typeof(Texture), "SetTextureImpl", "texture");
			CreateSetAttributeMethod(typeof(Texture2D), "SetTextureImpl", "texture");
			CreateSetAttributeMethod(typeof(Texture3D), "SetTextureImpl", "texture");
			CreateSetAttributeMethod(typeof(TextureCube), "SetTextureImpl", "texture");

		}

		private void GenerateClassComment()
		{
			AsmTechnique technique = this.source.GetAsmTechnique(this.techniqueName, this.platform);

			//generate a comment for the class, detailing the shaders
			string vspre_comment = "";
			string pspre_comment = "";

			//if (technique.VertexPreShader != null)
			//    vspre_comment = string.Format("</para><para>Vertex Preshader: approximately {0} instruction slots used, {1} {2}", technique.VertexPreShader.GetCommandCount(), technique.VertexPreShader.RegisterSet.FloatRegisterCount, technique.VertexPreShader.RegisterSet.FloatRegisterCount == 1 ? "register" : "registers");
			//if (technique.PixelPreShader != null)
			//    pspre_comment = string.Format("</para><para>Pixel Preshader: approximately {0} instruction slots used, {1} {2}", technique.PixelPreShader.GetCommandCount(), technique.PixelPreShader.RegisterSet.FloatRegisterCount, technique.PixelPreShader.RegisterSet.FloatRegisterCount == 1 ? "register" : "registers");

			//comment the class
			string comment = string.Format(
				@"<para>Technique '{0}' generated from file '{1}'</para><para>Vertex Shader: {2}, {3} {4}{5}</para><para>Pixel Shader: {6}, {7} {8}{9}</para>",
				this.techniqueName,
				System.IO.Path.GetFileName(this.source.FileName),
				technique.VertexShaderComment,
				technique.VertexShader.RegisterSet.FloatRegisterCount,
				technique.VertexShader.RegisterSet.FloatRegisterCount == 1 ? "register" : "registers",
				vspre_comment,
				technique.PixelShaderComment,
				technique.PixelShader.RegisterSet.FloatRegisterCount,
				technique.PixelShader.RegisterSet.FloatRegisterCount == 1 ? "register" : "registers",
				pspre_comment);

			Comment(classDom, comment);
		}


		private void Comment(CodeStatementCollection statements, string comment)
		{
			if (comment != null)
				statements.Add(new CodeCommentStatement(comment));
		}
		private void Comment(CodeTypeMember member, string comment)
		{
			CodeCommentStatement statement;

			if (comment != null)
				statement = new CodeCommentStatement(string.Format("<summary>{0}</summary>", comment), true);
			else
				statement = new CodeCommentStatement("<summary/>", true);

			if (member is CodeMemberMethod)
			{
				foreach (CodeParameterDeclarationExpression param in (member as CodeMemberMethod).Parameters)
					statement.Comment.Text += string.Format("<param name=\"{0}\"/>", param.Name);
			}
			member.Comments.Add(statement);
		}

		#region creation methods

		private void CreateConstructor()
		{

			//create the constructor

			CodeConstructor constructor = new CodeConstructor();
			constructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;

			classDom.Members.Add(constructor);

			Comment(constructor, string.Format("Construct an instance of the '{0}' shader", this.techniqueName));

			foreach (DomBase dom in this.domList)
				dom.AddConstructor(this, delegate(CodeStatement s) { constructor.Statements.Add(s); });

			//create the static constructor

			CodeTypeConstructor staticConstructor = new CodeTypeConstructor();
			Comment(staticConstructor, string.Format("Static Constructor for '{0}'", this.techniqueName));
			staticConstructor.Attributes = MemberAttributes.Private | MemberAttributes.Final;

			foreach (DomBase dom in this.domList)
				dom.AddStaticConstructor(this, delegate(CodeStatement s) { staticConstructor.Statements.Add(s); });

			if (staticConstructor.Statements.Count > 0)
				classDom.Members.Add(staticConstructor);

		}



		private void CreateStaticGraphicsInitMethod()
		{
			//create the graphics method

			CodeMemberMethod graphInit = new CodeMemberMethod();
			graphInit.Name = graphicsIDInit.MethodName;
			graphInit.Attributes = MemberAttributes.Private | MemberAttributes.Final;
			graphInit.Parameters.Add(ShaderSystemArg);

			classDom.Members.Add(graphInit);

			Comment(graphInit, "Setup shader static values");

			foreach (DomBase dom in this.domList)
				dom.AddStaticGraphicsInit(this, delegate(CodeStatement s, string c) { Comment(graphInit.Statements, c); graphInit.Statements.Add(s); });
		}


		private void CreateChangedMethod()
		{
			//create the Changed() method

			CodeMemberMethod changed = new CodeMemberMethod();
			changed.Name = "Changed";
			changed.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			changed.ReturnType = new CodeTypeReference(typeof(bool));

			Comment(changed, "True if a shader constant has changed since the last Bind()");

			classDom.Members.Add(changed);

			CodeExpression combine = null;

			foreach (DomBase dom in this.domList)
			{
				dom.AddChangedCondition(this, delegate(CodeExpression ex) 
				{
					//build the expression...
					if (combine == null)
						combine = ex;
					else
						combine = new CodeBinaryOperatorExpression(combine, CodeBinaryOperatorType.BitwiseOr, ex);
				});
			}

			changed.Statements.Add(new CodeMethodReturnStatement(combine ?? new CodePrimitiveExpression(false)));
		}


		private void CreateBindMethod()
		{
			//create the bind() method

			CodeMemberMethod bind = new CodeMemberMethod();
			bind.Name = "BeginImpl";
			bind.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			bind.Parameters.Add(ShaderSystemArg);
			bind.Parameters.Add(new CodeParameterDeclarationExpression(typeof(bool), "ic"));
			//bind.Parameters.Add(new CodeParameterDeclarationExpression(typeof(bool), "tc"));
			bind.Parameters.Add(new CodeParameterDeclarationExpression(typeof(bool), "ec"));
			bind.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ShaderExtension), "ext"));

			Comment(bind, "Bind the shader, 'ic' indicates the shader instance has changed and 'ec' indicates the extension has changed.");

			classDom.Members.Add(bind);

			foreach (DomBase dom in this.domList)
				dom.AddBindBegin(this, delegate(CodeStatement s, string c) { Comment(bind.Statements, c); bind.Statements.Add(s); });
			foreach (DomBase dom in this.domList)
				dom.AddBind(this, delegate(CodeStatement s, string c) { Comment(bind.Statements, c); bind.Statements.Add(s); });
			foreach (DomBase dom in this.domList)
				dom.AddBindEnd(this, delegate(CodeStatement s, string c) { Comment(bind.Statements, c); bind.Statements.Add(s); });
			foreach (DomBase dom in this.domList)
				dom.AddBindFinal(this, delegate(CodeStatement s, string c) { Comment(bind.Statements, c); bind.Statements.Add(s); });
		}

		private void CreateVertexInputMethods()
		{
			AsmListing asmVS = this.source.GetAsmTechnique(this.techniqueName, this.platform).VertexShader;

			//create the GetVertexInputCount() and GetVertexInput() methods

			CodeMemberMethod count = new CodeMemberMethod();
			count.Name = "GetVertexInputCountImpl";
			count.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			count.ReturnType = new CodeTypeReference(typeof(int));

			Comment(count, "Returns the number of vertex inputs used by this shader");
			count.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(asmVS.InputCount)));
			classDom.Members.Add(count);

			//inputs are stored in a static array

			//create it...

			int[] arrayValues = new int[asmVS.InputCount * 2];
			for (int i = 0; i < asmVS.InputCount; i++)
			{
				arrayValues[i] = (int)asmVS.GetInput(i).Usage;
				arrayValues[i + asmVS.InputCount] = (int)asmVS.GetInput(i).Index;
			}

			this.vsInputField = new CodeMemberField(typeof(int[]), "vin");
			this.vsInputField.Attributes = MemberAttributes.Private | MemberAttributes.Static | MemberAttributes.Final;
			this.vsInputField.InitExpression = ShaderBytes.ToArray(arrayValues, this.directives);

			CodeFieldReferenceExpression vsInputRef = new CodeFieldReferenceExpression(ShaderClassEx, vsInputField.Name);

			//protected internal abstract void GetVertexInput(int index, out VertexElementUsage elementUsage, out int elementIndex);

			CodeMemberMethod getInput = new CodeMemberMethod();
			getInput.Name = "GetVertexInputImpl";
			getInput.Attributes = MemberAttributes.Family | MemberAttributes.Override;

			CodeParameterDeclarationExpression indexParam = new CodeParameterDeclarationExpression(typeof(int),"i");
			getInput.Parameters.Add(indexParam);

			CodeParameterDeclarationExpression param = new CodeParameterDeclarationExpression(typeof(VertexElementUsage), "usage");
			param.Direction = FieldDirection.Out;
			getInput.Parameters.Add(param);

			param = new CodeParameterDeclarationExpression(typeof(int), "index");
			param.Direction = FieldDirection.Out;
			getInput.Parameters.Add(param);

			CodeArgumentReferenceExpression indexRef = new CodeArgumentReferenceExpression(indexParam.Name);

			//the element index is stored at 'i + asmVS.InputCount'
			CodeExpression indexArray = new CodeArrayIndexerExpression(vsInputRef,
				new CodeBinaryOperatorExpression(indexRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(asmVS.InputCount)));

			//and the usage must be cast
			CodeExpression usageCast = new CodeCastExpression(typeof(VertexElementUsage), new CodeArrayIndexerExpression(vsInputRef,indexRef));

			getInput.Statements.Add(new CodeAssignStatement(new CodeArgumentReferenceExpression("usage"), usageCast));
			getInput.Statements.Add(new CodeAssignStatement(new CodeArgumentReferenceExpression("index"), indexArray));

			Comment(getInput, "Returns a vertex input used by this shader");

			classDom.Members.Add(getInput);
		}

		private void CreateShaderConstantHashMethod()
		{
			//create the GetShaderConstantHash() method
			//protected abstract int[] GetShaderConstantHash(bool ps);

			//this method is only used during shader merging (for validation).

			CodeMemberMethod hash = new CodeMemberMethod();
			hash.Name = "GetShaderConstantHash";
			hash.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			hash.Parameters.Add(new CodeParameterDeclarationExpression(typeof(bool),"ps"));
			hash.ReturnType = new CodeTypeReference(typeof(int[]));

			Comment(hash, "Gets an array containing approximate hash codes for the constants used by this shader (Used for validation when merging two shaders)");

			classDom.Members.Add(hash);

			int[] vsHashSet = this.source.GetAsmTechnique(this.techniqueName,this.platform).VertexShader.RegisterSet.GetHashSet();
			int[] psHashSet = this.source.GetAsmTechnique(this.techniqueName,this.platform).PixelShader.RegisterSet.GetHashSet();

			//the input 'ps' selects the set.

			CodeStatement valueOut = new CodeConditionStatement(new CodeArgumentReferenceExpression("ps"),
				new CodeStatement[]{ new CodeMethodReturnStatement(ShaderBytes.ToArray(psHashSet,this.directives)) }, //true, PS
				new CodeStatement[]{ new CodeMethodReturnStatement(ShaderBytes.ToArray(vsHashSet, this.directives))}  //false, VS
				);

			hash.Statements.Add(valueOut);
		}

		private void CreateWarmShaderMethod()
		{
			//create the WarmShader() method

			CodeMemberMethod warm = new CodeMemberMethod();
			warm.Name = "WarmShader";
			warm.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			warm.Parameters.Add(ShaderSystemArg);

			Comment(warm, "Warm (Preload) the shader");

			classDom.Members.Add(warm);

			foreach (DomBase dom in this.domList)
				dom.AddWarm(this, delegate(CodeStatement s, string c) { Comment(warm.Statements, c); warm.Statements.Add(s); });
		}


		//the method that creates SetAttribute methods

		private void CreateSetAttributeMethod(Type type, string method, string typename)
		{
			//create complex method...
			//which looks like:
			/*
					protected override bool SetAttribute(Xen.Graphics.ShaderSystem.IShaderSystem state, int name_uid, ref Microsoft.Xna.Framework.Matrix value)
					{
						if ((ParticleStoreLife128.init_gd != state.DeviceUniqueIndex))
						{
							ParticleStoreLife128._init(state);
						}
						if ((name_uid == ParticleStoreLife128.id_1))
						{
							this.SetTest(ref value);
							return true;
						}
						return false;
					}
			 */

			//first, try and grab the setter statements.
			CodeStatementCollection statements = new CodeStatementCollection();

			foreach (DomBase dom in this.domList)
				dom.AddSetAttribute(this, delegate(CodeStatement s) { statements.Add(s); }, type);

			if (statements.Count == 0)
				return; // this is most common. no need for the method.


			//ok, does need the method

			CodeMemberMethod attrib = new CodeMemberMethod();
			attrib.Name = method;
			attrib.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			attrib.ReturnType = new CodeTypeReference(typeof(bool));
			attrib.Parameters.Add(ShaderSystemArg);

			CodeParameterDeclarationExpression parm = new CodeParameterDeclarationExpression(typeof(int), setAttribIdRef.ParameterName);
			attrib.Parameters.Add(parm);

			parm = new CodeParameterDeclarationExpression(type, setAttribValueRef.ParameterName);
			if (type.IsClass == false && System.Runtime.InteropServices.Marshal.SizeOf(type) > 4)
				parm.Direction = FieldDirection.Ref;
			attrib.Parameters.Add(parm);

			//first, add a check to make sure the device is warmed
			attrib.Statements.Add(this.validateDeviceIdAndWarmShader);

			//add the assignment checkers
			attrib.Statements.AddRange(statements);

			//otherwise return false
			attrib.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));

			Comment(attrib, string.Format("Set a shader {2} of type '{0}' by global unique ID, see <see cref=\"{1}.GetNameUniqueID\"/> for details.", type.Name, typeof(ShaderSystemBase).FullName, typename));

			classDom.Members.Add(attrib);
		}



		#endregion

		public CodeTypeDeclaration CodeTypeDeclaration
		{
			get { return classDom; }
		}


		// member setup
		private CodeFieldReferenceExpression gdIndexRef;
		private CodeTypeReference shaderTypeRef;
		private CodeParameterDeclarationExpression shaderSystemDecl;
		private CodeArgumentReferenceExpression shaderSystemRef;
		private CodeStatement validateDeviceId;
		private CodeStatement validateDeviceIdAndWarmShader;
		private CodeMethodReferenceExpression graphicsIDInit;
		private CodeVariableReferenceExpression bindInstanceChange;
		//private CodeVariableReferenceExpression bindTypeChange;
		private CodeVariableReferenceExpression extensionModeChange;
		private CodeVariableReferenceExpression shaderExtensionMode;
		private CodeExpression shaderExtensionIsBlending, shaderExtensionIsInstancing;
		private CompileDirectives directives;
		private CodeFieldReferenceExpression effectRef, effectBytesRef;
		private CodeFieldReferenceExpression vsRegRef, psRegRef, vsRegChangeRef, psRegChangeRef, vsBooleanRegRef, psBooleanRegRef, vsBooleanRegChangeRef, psBooleanRegChangeRef;//, vsRegPreRef, psRegPreRef; // actual instances are created by the ShaderBytes class
		private CodeFieldReferenceExpression vsInstancingRegRef, vsBlendRegRef, vsInstancingRegChangeRef, vsBlendRegChangeRef;
		private CodeMemberField vsInputField;
		private CodeArgumentReferenceExpression setAttribIdRef, setAttribValueRef;


		//interface implementation
		public CodeFieldReferenceExpression GraphicsDeviceUID { get { return gdIndexRef; } }
		public CodeTypeReference ShaderClass { get { return shaderTypeRef; } }
		public CodeTypeReferenceExpression ShaderClassEx { get { return new CodeTypeReferenceExpression(shaderTypeRef); } }
		public CodeParameterDeclarationExpression ShaderSystemArg { get { return shaderSystemDecl; } }
		public CodeArgumentReferenceExpression ShaderSystemRef { get { return shaderSystemRef; } }

		public CodeFieldReferenceExpression EffectRef { get { return effectRef; } }
		public CodeFieldReferenceExpression EffectBytesRef { get { return effectBytesRef; } }

		public CodeVariableReferenceExpression BindShaderInstanceChange { get { return bindInstanceChange; } }
		//public CodeVariableReferenceExpression BindShaderTypeChange { get { return bindTypeChange; } }
		public CodeVariableReferenceExpression ExtensionModeChange { get { return extensionModeChange; } }
		public CodeVariableReferenceExpression ShaderExtensionMode { get { return shaderExtensionMode; } }

		public CodeExpression ShaderExtensionIsBlending { get { return shaderExtensionIsBlending; } }
		public CodeExpression ShaderExtensionIsInstancing { get { return shaderExtensionIsInstancing; } }

		public CodeExpression Instance { get { return new CodeThisReferenceExpression(); } }
		//expression to statement
		public CodeStatement ETS(CodeExpression expression) { return new CodeExpressionStatement(expression); }
		public CompileDirectives CompileDirectives { get { return directives; } }

		public CodeFieldReferenceExpression VertexShaderRegistersRef { get { return vsRegRef; } }
		public CodeFieldReferenceExpression PixelShaderRegistersRef { get { return psRegRef; } }

		public CodeFieldReferenceExpression BlendShaderRegistersRef { get { return vsBlendRegRef; } }
		public CodeFieldReferenceExpression InstancingShaderRegistersRef { get { return vsInstancingRegRef; } }

		public CodeFieldReferenceExpression BlendShaderRegistersChangedRef { get { return vsBlendRegChangeRef; } }
		public CodeFieldReferenceExpression InstancingShaderRegistersChangdRef { get { return vsInstancingRegChangeRef; } }

		public CodeFieldReferenceExpression VertexShaderRegistersChangedRef { get { return vsRegChangeRef; } }
		public CodeFieldReferenceExpression PixelShaderRegistersChangedRef { get { return psRegChangeRef; } }


		public CodeFieldReferenceExpression VertexShaderBooleanRegistersRef { get { return vsBooleanRegRef; } }
		public CodeFieldReferenceExpression PixelShaderBooleanRegistersRef { get { return psBooleanRegRef; } }

		public CodeFieldReferenceExpression VertexShaderBooleanRegistersChangedRef { get { return vsBooleanRegChangeRef; } }
		public CodeFieldReferenceExpression PixelShaderBooleanRegistersChangedRef { get { return psBooleanRegChangeRef; } }

		//setAttribIdRef, setAttribValueRef
		public CodeExpression AttributeAssignId { get { return setAttribIdRef; } }
		public CodeExpression AttributeAssignValue { get { return setAttribValueRef; } }

		public SourceShader SourceShader { get { return source; } }
		public string TechniqueName { get { return techniqueName; } }
		public Platform Platform { get { return platform; } }

		//implement the code generation methods

		private void SetupMembers(string name)
		{
			shaderTypeRef = new CodeTypeReference(name);
			gdIndexRef = new CodeFieldReferenceExpression(ShaderClassEx, "gd");

			shaderSystemDecl = new CodeParameterDeclarationExpression(typeof(ShaderSystemBase), "state");
			shaderSystemRef = new CodeArgumentReferenceExpression("state");

			graphicsIDInit = new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), "gdInit");

			validateDeviceId = new CodeConditionStatement(new CodeBinaryOperatorExpression(GraphicsDeviceUID, CodeBinaryOperatorType.IdentityInequality, new CodePropertyReferenceExpression(ShaderSystemRef, "DeviceUniqueIndex")),
				new CodeExpressionStatement(new CodeMethodInvokeExpression(graphicsIDInit, shaderSystemRef)));

			validateDeviceIdAndWarmShader = new CodeConditionStatement(new CodeBinaryOperatorExpression(GraphicsDeviceUID, CodeBinaryOperatorType.IdentityInequality, new CodePropertyReferenceExpression(ShaderSystemRef, "DeviceUniqueIndex")),
				new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "WarmShader", shaderSystemRef)));

			bindInstanceChange = new CodeVariableReferenceExpression("ic");
			//bindTypeChange = new CodeVariableReferenceExpression("tc");
			extensionModeChange = new CodeVariableReferenceExpression("ec");
			shaderExtensionMode = new CodeVariableReferenceExpression("ext");

			shaderExtensionIsBlending = new CodeBinaryOperatorExpression(shaderExtensionMode, CodeBinaryOperatorType.IdentityEquality, new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(ShaderExtension)), "Blending"));
			shaderExtensionIsInstancing = new CodeBinaryOperatorExpression(shaderExtensionMode, CodeBinaryOperatorType.IdentityEquality, new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(ShaderExtension)), "Instancing"));
	
			effectRef = new CodeFieldReferenceExpression(ShaderClassEx, "fx");
			effectBytesRef = new CodeFieldReferenceExpression(ShaderClassEx, "fxb");

			vsRegRef = new CodeFieldReferenceExpression(Instance, "vreg");
			psRegRef = new CodeFieldReferenceExpression(Instance, "preg");

			vsRegChangeRef = new CodeFieldReferenceExpression(Instance, "vreg_change");
			psRegChangeRef = new CodeFieldReferenceExpression(Instance, "preg_change");

			vsBlendRegRef = new CodeFieldReferenceExpression(Instance, "vbreg");
			vsInstancingRegRef = new CodeFieldReferenceExpression(Instance, "vireg");

			vsBlendRegChangeRef = new CodeFieldReferenceExpression(Instance, "vbreg_change");
			vsInstancingRegChangeRef = new CodeFieldReferenceExpression(Instance, "vireg_change");

			vsBooleanRegRef = new CodeFieldReferenceExpression(Instance, "vreg_bool");
			psBooleanRegRef = new CodeFieldReferenceExpression(Instance, "preg_bool");

			vsBooleanRegChangeRef = new CodeFieldReferenceExpression(Instance, "vreg_bool_change");
			psBooleanRegChangeRef = new CodeFieldReferenceExpression(Instance, "preg_bool_change");

			setAttribIdRef = new CodeArgumentReferenceExpression("id");
			setAttribValueRef = new CodeArgumentReferenceExpression("value");
		}

		public override void AddMembers(IShaderDom shader, Action<CodeTypeMember, string> add, Platform platform)
		{
			if (platform != Platform.Both)
				return;

			//static graphics ID
			CodeMemberField field = new CodeMemberField(typeof(int),gdIndexRef.FieldName);
			field.Attributes = MemberAttributes.Static | MemberAttributes.Final | MemberAttributes.Private;
			add(field,"Static graphics ID");
		}

		public override void AddReadonlyMembers(IShaderDom shader, Action<CodeTypeMember, string> add, Platform platform)
		{
			if (platform == Platform.Both)
				add(vsInputField, "array storing vertex usages, and element indices");
		}

		public override void AddStaticGraphicsInit(IShaderDom shader, Action<CodeStatement, string> add)
		{
			//assign the static graphics ID
			CodeExpression deviceId = new CodePropertyReferenceExpression(ShaderSystemRef, "DeviceUniqueIndex");
			CodeStatement assign = new CodeAssignStatement(gdIndexRef, deviceId);
			add(assign, "set the graphics ID");

			assign = new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "GraphicsID"), deviceId);
			add(assign, null);
		}

		public override void AddWarm(IShaderDom shader, Action<CodeStatement, string> add)
		{
			//return from the method if the device index hasn't changed

			CodeStatement returnCondition = 
				new CodeConditionStatement(
					new CodeBinaryOperatorExpression(GraphicsDeviceUID, CodeBinaryOperatorType.IdentityEquality, new CodePropertyReferenceExpression(ShaderSystemRef, "DeviceUniqueIndex")),
						new CodeMethodReturnStatement());

			add(returnCondition, "Shader is already warmed");

			add(validateDeviceId, "Setup the shader");
		}

		public override void AddBindBegin(IShaderDom shader, Action<CodeStatement, string> add)
		{
			CodeExpression devIndex = new CodeFieldReferenceExpression(shader.ShaderSystemRef, "DeviceUniqueIndex");

			//if extending in some way, then don't compare the devIndex to the class static, compare it to the virtual g uid
			CodeExpression classDevIndex = GraphicsDeviceUID;

			CodeExpression devChanged = new CodeBinaryOperatorExpression(devIndex, CodeBinaryOperatorType.IdentityInequality, classDevIndex);

			CodeStatement checkChanged =
				new CodeConditionStatement(devChanged,
					ETS(new CodeMethodInvokeExpression(Instance, "WarmShader", ShaderSystemRef)), // Call warm
					new CodeAssignStatement(BindShaderInstanceChange, new CodePrimitiveExpression(true)));

			add(checkChanged, "if the device changed, call Warm()");

			//bind the shaders if tc is true (type has changed)
			//and that 'owner' is true (owns the shaders)

			//if (((tc && this.owner)
			//            == true))
			//{
			//    state.SetShaders(ShadowShader.vs, ShadowShader.ps);
			//}

			//for classes that are either extendable or extended from another shader,
			//then the SetShaders call is put into a virtual method that is overridden.

		}

		public override void AddBindFinal(IShaderDom shader, Action<CodeStatement, string> add)
		{
			//at the end of the bind, set call SetEffect.
			CodeExpression setShaders = new CodeMethodInvokeExpression(
				ShaderSystemRef, "SetEffect", new CodeThisReferenceExpression(), new CodeDirectionExpression(FieldDirection.Ref, EffectRef), shader.ShaderExtensionMode);

			CodeExpression condition = new CodeBinaryOperatorExpression(shader.BindShaderInstanceChange, CodeBinaryOperatorType.BitwiseOr, shader.ExtensionModeChange);

			add(new CodeConditionStatement(condition, ETS(setShaders)), "Finally, bind the effect");
		}

		private int MaxSamplers(RegisterSet set)
		{
			int maxSampler = 0;

			for (int i = 0; i < set.RegisterCount; i++)
			{
				Register reg = set.GetRegister(i);
				if (reg.Category == RegisterCategory.Sampler)
					maxSampler = Math.Max(reg.Index, maxSampler);
			}
			return maxSampler;
		}



	}
}
