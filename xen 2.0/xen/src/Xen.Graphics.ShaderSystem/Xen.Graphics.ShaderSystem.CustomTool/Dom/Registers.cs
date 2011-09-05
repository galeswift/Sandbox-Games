using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xen.Graphics.ShaderSystem.CustomTool.FX;
using System.CodeDom;
using Microsoft.Xna.Framework;

namespace Xen.Graphics.ShaderSystem.CustomTool.Dom
{
	//stores the shader register classes (vreg, etc)
	public sealed class ShaderRegisters : DomBase
	{
		private readonly RegisterSet vsReg, psReg, vsInstancingReg, vsBlendingReg;
		private readonly Vector4[] vsDefault, psDefault;
		private readonly bool[] vsBooleanDefault, psBooleanDefault;
		private readonly TechniqueExtraData techniqueData;
		private readonly IExtensionStatementProvider extensionStatementProvider;

		public ShaderRegisters(SourceShader source, string techniqueName, Platform platform, IExtensionStatementProvider extensionStatementProvider)
		{
			AsmTechnique technique = source.GetAsmTechnique(techniqueName, platform);
			this.extensionStatementProvider = extensionStatementProvider;

			vsReg = technique.VertexShader.RegisterSet;
			psReg = technique.PixelShader.RegisterSet;

			if (technique.InstancingShader != null)
				vsInstancingReg = technique.InstancingShader.RegisterSet;
			if (technique.BlendingShader != null)
				vsBlendingReg = technique.BlendingShader.RegisterSet;

			if (technique.TechniqueExtraData != null)
			{
				this.techniqueData = technique.TechniqueExtraData;

				psDefault = technique.TechniqueExtraData.PixelShaderConstants;
				vsDefault = technique.TechniqueExtraData.VertexShaderConstants;

				psBooleanDefault = technique.TechniqueExtraData.PixelShaderBooleanConstants;
				vsBooleanDefault = technique.TechniqueExtraData.VertexShaderBooleanConstants;
			}
		}

		public override void AddReadonlyMembers(IShaderDom shader, Action<CodeTypeMember, string> add, Platform platform)
		{
			if (platform != Platform.Both)
				return;
		
			if (vsReg.FloatRegisterCount > 0)
			{
				CodeMemberField field = CreateShaderBufferField(shader, vsReg, shader.VertexShaderRegistersRef.FieldName);

				add(field, "Vertex shader register storage");
			}

			//and the PS

			if (psReg.FloatRegisterCount > 0)
			{
				CodeMemberField field = CreateShaderBufferField(shader, psReg, shader.PixelShaderRegistersRef.FieldName);

				add(field, "Pixel shader register storage");
			}


			//now do the boolean registers
			if (vsReg.BooleanRegisterCount > 0)
			{
				//create the vertex boolean registers
				CodeMemberField field = new CodeMemberField(typeof(bool[]), shader.VertexShaderBooleanRegistersRef.FieldName);
				field.Attributes = MemberAttributes.Private | MemberAttributes.Final;

				add(field, "Vertex shader boolean register storage");
			}
			//now the PS
			if (psReg.BooleanRegisterCount > 0)
			{
				//create the vertex boolean registers
				CodeMemberField field = new CodeMemberField(typeof(bool[]), shader.PixelShaderBooleanRegistersRef.FieldName);
				field.Attributes = MemberAttributes.Private | MemberAttributes.Final;

				add(field, "Pixel shader boolean register storage");
			}

			//blending
			if (vsBlendingReg != null && vsBlendingReg.FloatRegisterCount > 0 && shader.SourceShader.ManualExtensions)
			{
				CodeMemberField field = CreateShaderBufferField(shader, vsBlendingReg, shader.BlendShaderRegistersRef.FieldName);

				add(field, "Blend shader register storage");
			}
			//instancing
			if (vsInstancingReg != null && vsInstancingReg.FloatRegisterCount > 0)
			{
				CodeMemberField field = CreateShaderBufferField(shader, vsInstancingReg, shader.InstancingShaderRegistersRef.FieldName);

				add(field, "Instancing shader register storage");
			}
		}

		private CodeMemberField CreateShaderBufferField(IShaderDom shader, RegisterSet regSet, string name)
		{
			CodeExpression create = new CodeArrayCreateExpression(typeof(Vector4),
							new CodePrimitiveExpression(regSet.FloatRegisterCount)); //create with the number of registers

			//create the vertex registers
			CodeMemberField field = new CodeMemberField(typeof(Vector4[]), name);
			field.InitExpression = create;
			if (shader.SourceShader.ExposeRegisters)
				field.Attributes = MemberAttributes.Assembly | MemberAttributes.Final;
			else
				field.Attributes = MemberAttributes.Private | MemberAttributes.Final;
			return field;
		}

		public override void AddMembers(IShaderDom shader, Action<CodeTypeMember, string> add, Platform platform)
		{
			if (platform != Platform.Both)
				return;

			MemberAttributes attribs = MemberAttributes.Private | MemberAttributes.Final;
			if (shader.SourceShader.ExposeRegisters)
				attribs = MemberAttributes.Assembly | MemberAttributes.Final;

			//add change values for float registers
			if (vsReg.FloatRegisterCount > 0)
			{
				CodeMemberField field = new CodeMemberField(typeof(bool), shader.VertexShaderRegistersChangedRef.FieldName);
				field.Attributes = attribs;
				add(field, null);
			}

			//now the PS
			if (psReg.FloatRegisterCount > 0)
			{
				CodeMemberField field = new CodeMemberField(typeof(bool), shader.PixelShaderRegistersChangedRef.FieldName);
				field.Attributes = attribs;
				add(field, null);
			}

			//add change values for boolean registers
			if (vsReg.BooleanRegisterCount > 0)
			{
				CodeMemberField field = new CodeMemberField(typeof(bool), shader.VertexShaderBooleanRegistersChangedRef.FieldName);
				field.Attributes = attribs;
				add(field, null);
			}

			//now the PS
			if (psReg.BooleanRegisterCount > 0)
			{
				CodeMemberField field = new CodeMemberField(typeof(bool), shader.PixelShaderBooleanRegistersChangedRef.FieldName);
				field.Attributes = attribs;
				add(field, null);
			}


			//add change values for blending or instancing registers
			if (vsBlendingReg != null && vsBlendingReg.FloatRegisterCount > 0)
			{
				CodeMemberField field = new CodeMemberField(typeof(bool), shader.BlendShaderRegistersChangedRef.FieldName);
				field.Attributes = attribs;
				add(field, null);
			}
			if (vsInstancingReg != null && vsInstancingReg.FloatRegisterCount > 0)
			{
				CodeMemberField field = new CodeMemberField(typeof(bool), shader.InstancingShaderRegistersChangdRef.FieldName);
				field.Attributes = attribs;
				add(field, null);
			}

			//finally, add the method:
			//protected virtual void GetExtensionSupport(out bool blendingSupport, out bool instancingSupport)

			//only bother if the values are non-null.
			if (this.vsInstancingReg != null ||
				this.vsBlendingReg != null)
			{
				CodeMemberMethod extMethod = new CodeMemberMethod();
				extMethod.Name = "GetExtensionSupportImpl";
				extMethod.Attributes = MemberAttributes.Family | MemberAttributes.Override;
				CodeParameterDeclarationExpression param0 = new CodeParameterDeclarationExpression(typeof(bool), "blendingSupport");
				param0.Direction = FieldDirection.Out;
				CodeParameterDeclarationExpression param1 = new CodeParameterDeclarationExpression(typeof(bool), "instancingSupport");
				param1.Direction = FieldDirection.Out;
				extMethod.Parameters.Add(param0);
				extMethod.Parameters.Add(param1);

				//assign them
				extMethod.Statements.Add(new CodeAssignStatement(new CodeArgumentReferenceExpression(param0.Name), new CodePrimitiveExpression(this.vsBlendingReg != null)));
				extMethod.Statements.Add(new CodeAssignStatement(new CodeArgumentReferenceExpression(param1.Name), new CodePrimitiveExpression(this.vsInstancingReg != null)));

				add(extMethod, "Return the supported modes for this shader");
			}

			if (shader.SourceShader.ExposeRegisters)
			{
				CreateRegisterOffsetList(vsReg, "VertexShader", "Offset", add);
				CreateRegisterOffsetList(psReg, "VertexShader", "Offset", add);
			}
		}

		void CreateRegisterOffsetList(RegisterSet set, string prefix, string postfix, Action<CodeTypeMember, string> add)
		{
			if (set == null)
				return;
			foreach (Register reg in set)
			{
				if (reg.Category == RegisterCategory.Float4 ||
					reg.Category == RegisterCategory.Integer4)
				{
					CodeMemberField field = new CodeMemberField(typeof(int), prefix + Common.ToUpper(reg.Name) + postfix);
					field.Attributes = MemberAttributes.Const | MemberAttributes.Assembly;
					field.InitExpression = new CodePrimitiveExpression(reg.Index);
					add(field, null);
				}
			}
		}

		public override void AddConstructor(IShaderDom shader, Action<CodeStatement> add)
		{
			//setup registers

			//initalise vsReg defaults if non-zero
			for (int i = 0; i < vsReg.FloatRegisterCount && i < vsDefault.Length; i++)
			{
				if (vsDefault[i] != Vector4.Zero)
				{
					//copy the values into the array
					CodeStatement set = new CodeAssignStatement(
						new CodeArrayIndexerExpression(shader.VertexShaderRegistersRef, new CodePrimitiveExpression(i)),
						new CodeObjectCreateExpression(typeof(Vector4),
							new CodePrimitiveExpression(vsDefault[i].X),
							new CodePrimitiveExpression(vsDefault[i].Y),
							new CodePrimitiveExpression(vsDefault[i].Z),
							new CodePrimitiveExpression(vsDefault[i].W)));
					add(set);
				}
			}

			//and the PS

			for (int i = 0; i < psReg.FloatRegisterCount && i < psDefault.Length; i++)
			{
				if (psDefault[i] != Vector4.Zero)
				{
					//copy the values into the array
					CodeStatement set = new CodeAssignStatement(
						new CodeArrayIndexerExpression(shader.PixelShaderRegistersRef, new CodePrimitiveExpression(i)),
						new CodeObjectCreateExpression(typeof(Vector4),
							new CodePrimitiveExpression(psDefault[i].X),
							new CodePrimitiveExpression(psDefault[i].Y),
							new CodePrimitiveExpression(psDefault[i].Z),
							new CodePrimitiveExpression(psDefault[i].W)));
					add(set);
				}
			}

			//boolean registers
			if (vsReg.BooleanRegisterCount > 0)
			{
				//assign
				CodeExpression create = new CodeArrayCreateExpression(typeof(bool),
					new CodePrimitiveExpression(vsReg.BooleanRegisterCount)); //create with the number of registers

				add(new CodeAssignStatement(shader.VertexShaderBooleanRegistersRef, create));
				add(new CodeAssignStatement(shader.VertexShaderBooleanRegistersChangedRef, new CodePrimitiveExpression(true)));

				//if any are default non-zero, assign them.
				if (vsBooleanDefault != null)
				{
					for (int i = 0; i < this.vsBooleanDefault.Length && i < vsReg.BooleanRegisterCount; i++)
					{
						if (this.vsBooleanDefault[i])
							add(new CodeAssignStatement(new CodeArrayIndexerExpression(shader.VertexShaderBooleanRegistersRef, new CodePrimitiveExpression(i)), new CodePrimitiveExpression(true)));
					}
				}
			}
			//and the PS...
			if (psReg.BooleanRegisterCount > 0)
			{
				//assign
				CodeExpression create = new CodeArrayCreateExpression(typeof(bool),
					new CodePrimitiveExpression(psReg.BooleanRegisterCount)); //create with the number of registers

				add(new CodeAssignStatement(shader.PixelShaderBooleanRegistersRef, create));
				add(new CodeAssignStatement(shader.PixelShaderBooleanRegistersChangedRef, new CodePrimitiveExpression(true)));

				//if any are default non-zero, assign them.
				if (psBooleanDefault != null)
				{
					for (int i = 0; i < this.psBooleanDefault.Length && i < psReg.BooleanRegisterCount; i++)
					{
						if (this.psBooleanDefault[i])
							add(new CodeAssignStatement(new CodeArrayIndexerExpression(shader.PixelShaderBooleanRegistersRef, new CodePrimitiveExpression(i)), new CodePrimitiveExpression(true)));
					}
				}
			}
		}

		public override void AddBind(IShaderDom shader, Action<CodeStatement, string> add)
		{
			//if the instance has changed, set the change bools to true

			List<CodeStatement> setToTrue = new List<CodeStatement>();
			
			if (vsReg.FloatRegisterCount > 0)
				setToTrue.Add(new CodeAssignStatement(shader.VertexShaderRegistersChangedRef, new CodeBinaryOperatorExpression(shader.VertexShaderRegistersChangedRef, CodeBinaryOperatorType.BitwiseOr, shader.BindShaderInstanceChange)));
			if (psReg.FloatRegisterCount > 0)
				setToTrue.Add(new CodeAssignStatement(shader.PixelShaderRegistersChangedRef, new CodeBinaryOperatorExpression(shader.PixelShaderRegistersChangedRef, CodeBinaryOperatorType.BitwiseOr, shader.BindShaderInstanceChange)));
			if (vsReg.BooleanRegisterCount > 0)
				setToTrue.Add(new CodeAssignStatement(shader.VertexShaderBooleanRegistersChangedRef, new CodeBinaryOperatorExpression(shader.VertexShaderBooleanRegistersChangedRef, CodeBinaryOperatorType.BitwiseOr, shader.BindShaderInstanceChange)));
			if (psReg.BooleanRegisterCount > 0)
				setToTrue.Add(new CodeAssignStatement(shader.PixelShaderBooleanRegistersChangedRef, new CodeBinaryOperatorExpression(shader.PixelShaderBooleanRegistersChangedRef, CodeBinaryOperatorType.BitwiseOr, shader.BindShaderInstanceChange)));
			if (vsBlendingReg != null && vsBlendingReg.FloatRegisterCount > 0)
				setToTrue.Add(new CodeAssignStatement(shader.BlendShaderRegistersChangedRef, new CodeBinaryOperatorExpression(shader.BlendShaderRegistersChangedRef, CodeBinaryOperatorType.BitwiseOr, shader.BindShaderInstanceChange)));
			if (vsInstancingReg != null && vsInstancingReg.FloatRegisterCount > 0)
				setToTrue.Add(new CodeAssignStatement(shader.InstancingShaderRegistersChangdRef, new CodeBinaryOperatorExpression(shader.InstancingShaderRegistersChangdRef, CodeBinaryOperatorType.BitwiseOr, shader.BindShaderInstanceChange)));

			//if the instance has changed, set the values to true.
			for (int i = 0; i < setToTrue.Count; i++)
			{
				add(setToTrue[i], i== 0 ? "Force updating if the instance has changed" : null);
			}
		}
		
		public override void AddBindEnd(IShaderDom shader, Action<CodeStatement, string> add)
		{
			//set the constants
			//eg:
			/*
			 * 
			if (((this.vreg.change == true)
						|| (ic == true)))
			{
				state.SetShaderConstants(this.vreg.array, null);
				this.vreg.change = false;
			}
			 */

			CodeExpression vreg = new CodePrimitiveExpression(null);
			CodeExpression preg = vreg;
			
			CodeStatement setDirty = new CodeAssignStatement(shader.BindShaderInstanceChange, new CodePrimitiveExpression(true));

			if (vsReg.FloatRegisterCount > 0)
			{
				//assign if changed
				CodeConditionStatement assign = GenerateAssignConstantsInvoke(shader, setDirty, shader.VertexShaderRegistersRef, shader.VertexShaderRegistersChangedRef, "vs_c");

				add(assign, null);
			}

			//again for PS
			if (psReg.FloatRegisterCount > 0)
			{
				//assign if changed
				CodeConditionStatement assign = GenerateAssignConstantsInvoke(shader, setDirty, shader.PixelShaderRegistersRef, shader.PixelShaderRegistersChangedRef, "ps_c");

				add(assign, null);
			}

			//assing for blending, if it's there
			if (vsBlendingReg != null && vsBlendingReg.FloatRegisterCount > 0)
			{
				CodeConditionStatement condition = new CodeConditionStatement(shader.ShaderExtensionIsBlending);

				if (extensionStatementProvider.IsBlendingBufferNotShared)
				{
					condition.TrueStatements.Add(extensionStatementProvider.GetBlendDirectAssignment(shader));
				}
				else
				{
					//assign if changed
					CodeConditionStatement assign = GenerateAssignConstantsInvoke(shader, setDirty, shader.BlendShaderRegistersRef, shader.BlendShaderRegistersChangedRef, "vsb_c");

					//write in a mode statement
					condition.TrueStatements.AddRange(extensionStatementProvider.GetBlendExtensionAssignments());
					condition.TrueStatements.Add(assign);
				}

				add(condition, null);
			}

			//assing for instancing, if it's there
			if (vsInstancingReg != null && vsInstancingReg.FloatRegisterCount > 0)
			{
				//assign if changed
				CodeConditionStatement assign = GenerateAssignConstantsInvoke(shader, setDirty, shader.InstancingShaderRegistersRef, shader.InstancingShaderRegistersChangdRef, "vsi_c");

				//write in a mode statement
				CodeConditionStatement condition = new CodeConditionStatement(shader.ShaderExtensionIsInstancing, extensionStatementProvider.GetInstancingExtensionAssignments());
				condition.TrueStatements.Add(assign);

				add(condition, null);
			}

			//set the boolean registers
			if (vsReg.BooleanRegisterCount > 0)
			{
				//assign
				CodeExpression assign = new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(shader.EffectRef, "vs_b"), "SetValue", shader.VertexShaderBooleanRegistersRef);

				//assign the effect params directly
				CodeConditionStatement changed = new CodeConditionStatement(shader.VertexShaderBooleanRegistersRef,
										shader.ETS(assign), setDirty,
										new CodeAssignStatement(shader.VertexShaderBooleanRegistersChangedRef, new CodePrimitiveExpression(false)));

				add(changed, null);
			}
			if (vsReg.BooleanRegisterCount > 0)
			{
				//assign
				CodeExpression assign = new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(shader.EffectRef, "ps_b"), "SetValue", shader.PixelShaderBooleanRegistersRef);

				//assign the effect params directly
				CodeConditionStatement changed = new CodeConditionStatement(shader.PixelShaderBooleanRegistersChangedRef,
										shader.ETS(assign), setDirty,
										new CodeAssignStatement(shader.PixelShaderBooleanRegistersChangedRef, new CodePrimitiveExpression(false)));

				add(changed, null);
			}
		}
		
		private static CodeConditionStatement GenerateAssignConstantsInvoke(IShaderDom shader, CodeStatement setDirty, CodeExpression bufferRef, CodeExpression changeRef, string name)
		{
			CodeBinaryOperatorExpression assignCondition = new CodeBinaryOperatorExpression(
							changeRef, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(true));

			//assign the effect params directly

			//assign
			CodeConditionStatement assign = new CodeConditionStatement(assignCondition,
				shader.ETS(new CodeMethodInvokeExpression(new CodeFieldReferenceExpression(shader.EffectRef, name), "SetValue", bufferRef)),
				new CodeAssignStatement(changeRef, new CodePrimitiveExpression(false)),
				setDirty);
			return assign;
		}

		public override void AddChangedCondition(IShaderDom shader, Action<CodeExpression> add)
		{
			if (vsReg.FloatRegisterCount > 0)
				add(shader.VertexShaderRegistersChangedRef);
			if (psReg.FloatRegisterCount > 0)
				add(shader.PixelShaderRegistersChangedRef);
		}
	}
}
