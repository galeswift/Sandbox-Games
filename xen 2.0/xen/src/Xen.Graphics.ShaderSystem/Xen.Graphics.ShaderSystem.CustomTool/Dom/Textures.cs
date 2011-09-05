using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xen.Graphics.ShaderSystem.CustomTool.FX;
using System.CodeDom;
using Microsoft.Xna.Framework;

namespace Xen.Graphics.ShaderSystem.CustomTool.Dom
{
	//how a texture is assigned to samplers
	class TextureAssociation
	{
		public readonly Register Texture;
		public readonly List<int> PsSamplers, VsSamplers;
		public bool IsGlobal { get { return Texture.Semantic != null ? Texture.Semantic.Equals("global", StringComparison.InvariantCultureIgnoreCase) : false; } }

		public TextureAssociation(Register reg)
		{
			this.Texture = reg;
			this.PsSamplers = new List<int>();
			this.VsSamplers = new List<int>();
			
		}
	}

	class SharedSampler
	{
		public int PsIndex = -1, VsIndex = -1;
		public Register SamplerDetails;
		public int Index;
		public bool IsGlobal { get { return SamplerDetails.Semantic != null ? SamplerDetails.Semantic.Equals("global", StringComparison.InvariantCultureIgnoreCase) : false; } }
		public TextureSamplerState DefaultState;
	}

	//generates the shader textures
	public sealed class ShaderTextures : DomBase
	{
		private readonly List<Register> psSamplers, vsSamplers;
		private readonly Dictionary<string, SharedSampler> allSamplers;
		private readonly TextureAssociation[] textures;

		public ShaderTextures(SourceShader source, string techniqueName, Platform platform)
		{
			this.psSamplers = new List<Register>();
			this.vsSamplers = new List<Register>();
			this.allSamplers = new Dictionary<string, SharedSampler>();

			AsmTechnique technique = source.GetAsmTechnique(techniqueName, platform);
			TechniqueExtraData extras = technique.TechniqueExtraData;

			//pull out the textures that will be used
			textures = new TextureAssociation[extras.TechniqueTextures.Length];
			for (int i = 0; i < extras.TechniqueTextures.Length; i++)
				textures[i] = new TextureAssociation(extras.TechniqueTextures[i]);

			//now do the samplers
			RegisterSet set = technique.PixelShader.RegisterSet;

			//pixel first
			foreach (Register reg in set)
			{
				if (reg.Category == RegisterCategory.Sampler)
				{
					psSamplers.Add(reg);

					int textureIndex = extras.PixelSamplerTextureIndex[reg.Index];

					if (textureIndex == -1)
						ThrowSamplerNoTextureException(reg);

					textures[textureIndex].PsSamplers.Add(reg.Index);

					//add the sampler to 'allSamplers'
					SharedSampler ss = new SharedSampler();
					ss.PsIndex = reg.Index;
					ss.SamplerDetails = reg;
					ss.Index = allSamplers.Count;
					ss.DefaultState = extras.PixelSamplerStates[reg.Index];
					allSamplers.Add(reg.Name, ss);
				}
			}

			set = technique.VertexShader.RegisterSet;

			//now vertex
			foreach (Register reg in set)
			{
				if (reg.Category == RegisterCategory.Sampler)
				{
					vsSamplers.Add(reg);

					int textureIndex = extras.VertexSamplerTextureIndex[reg.Index];
					if (textureIndex == -1)
						ThrowSamplerNoTextureException(reg);

					textures[textureIndex].VsSamplers.Add(reg.Index);

					//add the sampler to 'allSamplers'
					SharedSampler ss;
					if (!allSamplers.TryGetValue(reg.Name, out ss))
					{
						ss = new SharedSampler();
						ss.SamplerDetails = reg;
						ss.Index = allSamplers.Count;
						ss.DefaultState = extras.VertexSamplerStates[reg.Index];
						allSamplers.Add(reg.Name, ss);
					}
					ss.VsIndex = reg.Index;
				}
			}
		}

		private static void ThrowSamplerNoTextureException(Register reg)
		{
			throw new CompileException(string.Format("Texture Sampler '{0} {1}' is not bound to a named Texture{2}The sampler must define a default sampler state, in the example format:{2}{2}texture{3} {1}Texture;{2}{0} {1} = sampler_state{2}{4}{2}\tTexture = ({1}Texture);{2}{5}; ", reg.Type, reg.Name, Environment.NewLine, reg.Type.Length >= 7 ? reg.Type.Substring(7) : "", "{", "}"));
		}

		public override void AddMembers(IShaderDom shader, Action<CodeTypeMember, string> add, Platform platform)
		{
			if (platform != Platform.Both)
				return;

			//create the properties for the sampler states
			GenerateProperties(shader, add);

			GenerateNameIds(shader, add);



			int vsTextures, psTextures;
			ComputeTextureCount(out vsTextures, out psTextures);

			//create arrays for the textures, if needed
			if (vsTextures >= 0)
			{
				//change:
				CodeMemberField field = new CodeMemberField(typeof(bool), "vtc");
				field.Attributes = MemberAttributes.Private | MemberAttributes.Family;
				add(field, "Vertex samplers/textures changed");
			}
			if (psTextures >= 0)
			{
				//change:
				CodeMemberField field = new CodeMemberField(typeof(bool), "ptc");
				field.Attributes = MemberAttributes.Private | MemberAttributes.Family;
				add(field, "Pixel samplers/textures changed");
			}
		}

		public override void AddChangedCondition(IShaderDom shader, Action<CodeExpression> add)
		{
			int vsTextures, psTextures;
			ComputeTextureCount(out vsTextures, out psTextures);

			//create arrays for the textures, if needed
			if (vsTextures >= 0)
				add(new CodeFieldReferenceExpression(shader.Instance, "vtc"));
			if (psTextures >= 0)
				add(new CodeFieldReferenceExpression(shader.Instance, "ptc"));
		}

		public override void AddReadonlyMembers(IShaderDom shader, Action<CodeTypeMember, string> add, Platform platform)
		{
			if (platform != Platform.Both)
				return;


			//add the texture members
			int vsTextures,psTextures;
			ComputeTextureCount(out vsTextures, out psTextures);

			//create arrays for the textures, if needed
			if (vsTextures >= 0)
			{
				CodeMemberField field = new CodeMemberField(typeof(Microsoft.Xna.Framework.Graphics.Texture[]), "vtx");
				field.Attributes = MemberAttributes.Private | MemberAttributes.Family;
				field.InitExpression = new CodeArrayCreateExpression(typeof(Microsoft.Xna.Framework.Graphics.Texture), vsTextures + 1);
				add(field, "Bound vertex textures");

				field = new CodeMemberField(typeof(Xen.Graphics.TextureSamplerState[]), "vts");
				field.Attributes = MemberAttributes.Private | MemberAttributes.Family;
				field.InitExpression = new CodeArrayCreateExpression(typeof(Xen.Graphics.TextureSamplerState), vsTextures + 1);
				add(field, "Bound vertex samplers");
			}
			if (psTextures >= 0)
			{
				CodeMemberField field = new CodeMemberField(typeof(Microsoft.Xna.Framework.Graphics.Texture[]), "ptx");
				field.Attributes = MemberAttributes.Private | MemberAttributes.Family;
				field.InitExpression = new CodeArrayCreateExpression(typeof(Microsoft.Xna.Framework.Graphics.Texture), psTextures + 1);
				add(field, "Bound pixel textures");

				field = new CodeMemberField(typeof(Xen.Graphics.TextureSamplerState[]), "pts");
				field.Attributes = MemberAttributes.Private | MemberAttributes.Family;
				field.InitExpression = new CodeArrayCreateExpression(typeof(Xen.Graphics.TextureSamplerState), psTextures + 1);
				add(field, "Bound pixel samplers");
			}
		}

		private void ComputeTextureCount(out int vsTextures, out int psTextures)
		{
			vsTextures = -1;
			psTextures = -1;

			for (int i = 0; i < textures.Length; i++)
			{
				TextureAssociation tex = textures[i];
				foreach (int index in tex.PsSamplers)
					psTextures = Math.Max(psTextures, index);
				foreach (int index in tex.VsSamplers)
					vsTextures = Math.Max(vsTextures, index);
			}
		}

		private void GenerateNameIds(IShaderDom shader, Action<CodeTypeMember, string> add)
		{
			//each texture / sampler needs to be settable by ID

			foreach (SharedSampler ss in allSamplers.Values)
			{
				//create a uid for the sampler

				CodeMemberField field = new CodeMemberField(typeof(int), "sid" + ss.Index);
				field.Attributes = MemberAttributes.Private | MemberAttributes.Family | MemberAttributes.Static;

				add(field, string.Format("Name uid for sampler for '{0} {1}'", Common.ToUpper(ss.SamplerDetails.Type), ss.SamplerDetails.Name));
			}

			//create for each texture
			for (int i = 0; i < textures.Length; i++)
			{
				TextureAssociation tex = textures[i];

				if (tex.PsSamplers.Count > 0 || tex.VsSamplers.Count > 0)
				{
					//create a uid for the sampler

					CodeMemberField field = new CodeMemberField(typeof(int), "tid" + i);
					field.Attributes = MemberAttributes.Private | MemberAttributes.Family | MemberAttributes.Static;

					add(field, string.Format("Name uid for texture for '{0} {1}'", tex.Texture.Type, tex.Texture.Name));
				}
			}
		}

		public override void AddStaticGraphicsInit(IShaderDom shader, Action<CodeStatement, string> add)
		{
			//initalise the static UIDs

			foreach (SharedSampler ss in allSamplers.Values)
			{
				//set the uid for the sampler

				CodeExpression uid = new CodeFieldReferenceExpression(shader.ShaderClassEx, "sid" + ss.Index);

				CodeExpression call = new CodeMethodInvokeExpression(
					new CodeMethodReferenceExpression(shader.ShaderSystemRef, "GetNameUniqueID"),
					new CodePrimitiveExpression(ss.SamplerDetails.Name));

				if (ss.IsGlobal)
				{
					//slightly differnet for globals. Need to call generic getter for the global index
					call = new CodeMethodInvokeExpression(
						new CodeMethodReferenceExpression(shader.ShaderSystemRef, "GetGlobalUniqueID", new CodeTypeReference(typeof(Xen.Graphics.TextureSamplerState))),
						new CodePrimitiveExpression(ss.SamplerDetails.Name));
				}

				CodeStatement assign = new CodeAssignStatement(uid, call);

				add(assign, null);
			}

			//and all the textures
			for (int i = 0; i < textures.Length; i++)
			{
				TextureAssociation tex = textures[i];

				if (tex.PsSamplers.Count > 0 || tex.VsSamplers.Count > 0)
				{
					//set the uid for the sampler

					CodeExpression uid = new CodeFieldReferenceExpression(shader.ShaderClassEx, "tid" + i);

					CodeExpression call = new CodeMethodInvokeExpression(
						new CodeMethodReferenceExpression(shader.ShaderSystemRef, "GetNameUniqueID"),
						new CodePrimitiveExpression(tex.Texture.Name));

					if (tex.IsGlobal)
					{
						//slightly differnet for globals. Need to call generic getter for the global index
						call = new CodeMethodInvokeExpression(
							new CodeMethodReferenceExpression(shader.ShaderSystemRef, "GetGlobalUniqueID", new CodeTypeReference(Common.GetTextureType(tex.Texture.Type))),
							new CodePrimitiveExpression(tex.Texture.Name));
					}

					CodeStatement assign = new CodeAssignStatement(uid, call);

					add(assign, null);

				}
			}
		}


		//setup the ID based setters / getters
		public override void AddSetAttribute(IShaderDom shader, Action<CodeStatement> add, Type type)
		{

			//samplers first
			if (type == typeof(Xen.Graphics.TextureSamplerState))
			{
				foreach (SharedSampler ss in allSamplers.Values)
				{
					if (ss.IsGlobal)
						continue;

					//assign the sampler, if it matches.
					CodeExpression uid = new CodeFieldReferenceExpression(shader.ShaderClassEx, "sid" + ss.Index);
					CodeExpression sampler = new CodePropertyReferenceExpression(shader.Instance, Common.ToUpper(ss.SamplerDetails.Name));

					CodeExpression assignIdsMatch =
						new CodeBinaryOperatorExpression(shader.AttributeAssignId, CodeBinaryOperatorType.IdentityEquality, uid);

					CodeStatement assignAttribute = new CodeAssignStatement(sampler, shader.AttributeAssignValue);

					CodeConditionStatement performAssign =
						new CodeConditionStatement(assignIdsMatch,
							assignAttribute, //call the assignment code
							new CodeMethodReturnStatement(new CodePrimitiveExpression(true))); //return true, set correctly.

					add(performAssign);
				}
			}


			if (!typeof(Microsoft.Xna.Framework.Graphics.Texture).IsAssignableFrom(type))
				return;

			//now, all the non-global textures
			for (int i = 0; i < textures.Length; i++)
			{
				TextureAssociation tex = textures[i];

				if ((tex.PsSamplers.Count > 0 || tex.VsSamplers.Count > 0)
					&& tex.IsGlobal == false)
				{
					if (Common.GetTextureType(tex.Texture.Type) == type)
					{
						//assign
						CodeExpression uid = new CodeFieldReferenceExpression(shader.ShaderClassEx, "tid" + i);
						CodeExpression sampler = new CodePropertyReferenceExpression(shader.Instance, Common.ToUpper(tex.Texture.Name));

						CodeExpression assignIdsMatch =
							new CodeBinaryOperatorExpression(shader.AttributeAssignId, CodeBinaryOperatorType.IdentityEquality, uid);

						CodeStatement assignAttribute = new CodeAssignStatement(sampler, shader.AttributeAssignValue);

						CodeConditionStatement performAssign =
							new CodeConditionStatement(assignIdsMatch,
								assignAttribute, //call the assignment code
								new CodeMethodReturnStatement(new CodePrimitiveExpression(true))); //return true, set correctly.

						add(performAssign);
					}
				}
			}
		}


		public override void AddConstructor(IShaderDom shader, Action<CodeStatement> add)
		{
			//set the samplers to their defaults

			foreach (SharedSampler ss in allSamplers.Values)
			{
				//get the default as an int,
				//then cast it to a sampler state using explicit cast construction
				CodeExpression value = new CodeCastExpression(typeof(Xen.Graphics.TextureSamplerState), new CodePrimitiveExpression((int)ss.DefaultState));

				//find the coorisponding VS / PS samplers
				if (ss.PsIndex >= 0)
				{
					CodeStatement assign = new CodeAssignStatement(new CodeArrayIndexerExpression(new CodeFieldReferenceExpression(shader.Instance, "pts"), new CodePrimitiveExpression(ss.PsIndex)), value);
					add(assign);
				}
				if (ss.VsIndex >= 0)
				{
					CodeStatement assign = new CodeAssignStatement(new CodeArrayIndexerExpression(new CodeFieldReferenceExpression(shader.Instance, "vts"), new CodePrimitiveExpression(ss.VsIndex)), value);
					add(assign);
				}				
			}
		}


		private void GenerateProperties(IShaderDom shader, Action<CodeTypeMember, string> add)
		{

			foreach (SharedSampler ss in allSamplers.Values)
			{

				//simlar in style to:

				/*
							 
					public Xen.Graphics.TextureSamplerState ShadowSampler
					{
						get
						{
							return this.ps_0;
						}
						set
						{
							if ((value != this.ps_0))
							{
								this.ps_0 = value;
								this.ps_m = (this.ps_m | 1);
							}
						}
					}
				 * 
				 */
				CodeExpression value = new CodePropertySetValueReferenceExpression();
				CodeExpression pts = new CodeFieldReferenceExpression(shader.Instance, "pts");
				CodeExpression vts = new CodeFieldReferenceExpression(shader.Instance, "vts");

				//change:
				CodeExpression ptc = new CodeFieldReferenceExpression(shader.Instance, "ptc");
				CodeExpression vtc = new CodeFieldReferenceExpression(shader.Instance, "vtc");

				CodeMemberProperty prop = new CodeMemberProperty();
				prop.Type = new CodeTypeReference(typeof(Xen.Graphics.TextureSamplerState));
				prop.Attributes = MemberAttributes.Public | MemberAttributes.Final;

				if (ss.IsGlobal) //make the property private if it's global
					prop.Attributes = MemberAttributes.Private | MemberAttributes.Final;

				prop.Name = Common.ToUpper(ss.SamplerDetails.Name);

				prop.HasGet = true;
				prop.HasSet = true;

				//return the first value
				if (ss.PsIndex >= 0)
					prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeArrayIndexerExpression(pts, new CodePrimitiveExpression(ss.PsIndex))));
				else
					prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeArrayIndexerExpression(vts, new CodePrimitiveExpression(ss.VsIndex))));

				if (ss.PsIndex >= 0)
				{
					CodeExpression input = new CodeArrayIndexerExpression(pts, new CodePrimitiveExpression(ss.PsIndex));
					CodeExpression condition = new CodeBinaryOperatorExpression(value, CodeBinaryOperatorType.IdentityInequality, input);
					prop.SetStatements.Add(new CodeConditionStatement(condition, new CodeAssignStatement(input, value), new CodeAssignStatement(ptc,new CodePrimitiveExpression(true))));
				}
				if (ss.VsIndex >= 0)
				{
					CodeExpression input = new CodeArrayIndexerExpression(vts, new CodePrimitiveExpression(ss.VsIndex));
					CodeExpression condition = new CodeBinaryOperatorExpression(value, CodeBinaryOperatorType.IdentityInequality, input);
					prop.SetStatements.Add(new CodeConditionStatement(condition, new CodeAssignStatement(input, value), new CodeAssignStatement(vtc,new CodePrimitiveExpression(true))));
				}

				add(prop, string.Format("Get/Set the Texture Sampler State for '{0} {1}'", Common.ToUpper(ss.SamplerDetails.Type), ss.SamplerDetails.Name));
			}
			
			
			


			//add the texture properties
			for (int i = 0; i < textures.Length; i++)
			{
				TextureAssociation tex = textures[i];

				//only add if they are used
				if (tex.PsSamplers.Count > 0 || tex.VsSamplers.Count > 0)
				{
					//create the property,
					//very similar to the sampler property.

					CodeExpression value = new CodePropertySetValueReferenceExpression();

					CodeExpression ptx = new CodeFieldReferenceExpression(shader.Instance, "ptx");
					CodeExpression vtx = new CodeFieldReferenceExpression(shader.Instance, "vtx");

					//change:
					CodeExpression ptc = new CodeFieldReferenceExpression(shader.Instance, "ptc");
					CodeExpression vtc = new CodeFieldReferenceExpression(shader.Instance, "vtc");

					CodeMemberProperty prop = new CodeMemberProperty();
					prop.Type = new CodeTypeReference(Common.GetTextureType(tex.Texture.Type));
					prop.Attributes = MemberAttributes.Public | MemberAttributes.Final;

					if (tex.IsGlobal) //make the property private if it's global
						prop.Attributes = MemberAttributes.Private | MemberAttributes.Final;

					prop.Name = Common.ToUpper(tex.Texture.Name);

					prop.HasGet = true;
					prop.HasSet = true;

					//return the first value
					if (tex.PsSamplers.Count > 0)
						prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(prop.Type,new CodeArrayIndexerExpression(ptx, new CodePrimitiveExpression(tex.PsSamplers[0])))));
					else
						prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(prop.Type,new CodeArrayIndexerExpression(vtx, new CodePrimitiveExpression(tex.VsSamplers[0])))));

					//changes.. (similar to sampler property)
					CodeStatement[] updateOpP = new CodeStatement[tex.PsSamplers.Count+1];
					CodeStatement[] updateOpV = new CodeStatement[tex.VsSamplers.Count+1];
					CodeExpression conditionP = null;
					CodeExpression conditionV = null;

					for (int n = 0; n < tex.PsSamplers.Count; n++)
					{
						CodeExpression input = new CodeArrayIndexerExpression(ptx, new CodePrimitiveExpression(tex.PsSamplers[n]));
						conditionP = conditionP ?? new CodeBinaryOperatorExpression(value, CodeBinaryOperatorType.IdentityInequality, input);
						updateOpP[0] = updateOpP[0] ?? new CodeAssignStatement(ptc, new CodePrimitiveExpression(true));
						updateOpP[n+1] = new CodeAssignStatement(input, value);
					}
					for (int n = 0; n < tex.VsSamplers.Count; n++)
					{
						CodeExpression input = new CodeArrayIndexerExpression(vtx, new CodePrimitiveExpression(tex.VsSamplers[n]));
						conditionV = conditionV ?? new CodeBinaryOperatorExpression(value, CodeBinaryOperatorType.IdentityInequality, input);
						updateOpV[0] = updateOpV[0] ?? new CodeAssignStatement(vtc, new CodePrimitiveExpression(true));
						updateOpV[n + 1] = new CodeAssignStatement(input, value);
					}

					//pull it all together
					if (tex.PsSamplers.Count > 0)
						prop.SetStatements.Add(new CodeConditionStatement(conditionP, updateOpP));
					if (tex.VsSamplers.Count > 0)
						prop.SetStatements.Add(new CodeConditionStatement(conditionV, updateOpV));

					add(prop, string.Format("Get/Set the Bound texture for '{0} {1}'", tex.Texture.Type, tex.Texture.Name));
				}
			}
		}


		//bind the samplers / textures
		public override void AddBind(IShaderDom shader, Action<CodeStatement, string> add)
		{
			// assign global textures first...

			bool firstAssign = true;
			for (int i = 0; i < this.textures.Length; i++)
			{
				if ((this.textures[i].PsSamplers.Count > 0 ||
					this.textures[i].VsSamplers.Count > 0) &&
					this.textures[i].IsGlobal)
				{
					//assign the global

					//a bit like:
					//this.ShadowMap = state.GetGlobalTexture2D(ShadowShaderBlend.tid0);

					CodeExpression uid = new CodeFieldReferenceExpression(shader.ShaderClassEx, "tid" + i);
					CodeExpression prop = new CodePropertyReferenceExpression(shader.Instance, Common.ToUpper(this.textures[i].Texture.Name));

					CodeExpression call = new CodeMethodInvokeExpression(shader.ShaderSystemRef, "GetGlobal" + this.textures[i].Texture.Type, uid);

					CodeStatement assign = new CodeAssignStatement(prop, call);

					add(assign, firstAssign ? "Assign global textures" : null);
					firstAssign = false;
				}
			}


			// assign global samplers...

			firstAssign = true;
			foreach (KeyValuePair<string,SharedSampler> sampler in allSamplers)
			{
				if (sampler.Value.IsGlobal)
				{
					//assign the global
					int i = sampler.Value.Index;

					//a bit like:
					//this.ShadowMapSampler = state.GetGlobalTextureSamplerState(ShadowShaderBlend.sid0);

					CodeExpression uid = new CodeFieldReferenceExpression(shader.ShaderClassEx, "sid" + i);
					CodeExpression prop = new CodePropertyReferenceExpression(shader.Instance, sampler.Key);

					CodeExpression call = new CodeMethodInvokeExpression(shader.ShaderSystemRef, "GetGlobalTextureSamplerState", uid);

					CodeStatement assign = new CodeAssignStatement(prop, call);

					add(assign, firstAssign ? "Assign global sampler" : null);
					firstAssign = false;
				}
			}


			//bind the samplers / textures
	
			//change:
			CodeExpression ptc = new CodeFieldReferenceExpression(shader.Instance, "ptc");
			CodeExpression vtc = new CodeFieldReferenceExpression(shader.Instance, "vtc");

			//setup the pixel samplers
			if (psSamplers.Count > 0)
			{
				CodeExpression condition = new CodeBinaryOperatorExpression(shader.BindShaderInstanceChange, CodeBinaryOperatorType.BitwiseOr, ptc);

				CodeExpression textures = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "ptx");
				CodeExpression samplers = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "pts");
				CodeExpression assign = new CodeMethodInvokeExpression(shader.ShaderSystemRef, "SetPixelShaderSamplers",textures, samplers);

				CodeStatement combine = new CodeConditionStatement(condition,shader.ETS(assign), new CodeAssignStatement(ptc,new CodePrimitiveExpression(false)));
				add(combine, "Assign pixel shader textures and samplers");
			}

			//do it all again for VS
			if (vsSamplers.Count > 0)
			{
				CodeExpression condition = new CodeBinaryOperatorExpression(shader.BindShaderInstanceChange, CodeBinaryOperatorType.BitwiseOr, vtc);

				CodeExpression textures = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "vtx");
				CodeExpression samplers = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "vts");
				CodeExpression assign = new CodeMethodInvokeExpression(shader.ShaderSystemRef, "SetVertexShaderSamplers", textures, samplers);

				CodeStatement combine = new CodeConditionStatement(condition, shader.ETS(assign), new CodeAssignStatement(vtc, new CodePrimitiveExpression(false)));
				add(combine, "Assign pixel shader textures and samplers");
			}
		}

	}
}