using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xen.Graphics.ShaderSystem.CustomTool.FX;
using System.CodeDom;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace Xen.Graphics.ShaderSystem.CustomTool.Dom
{

	struct SemanticType
	{
		public Type Type;
		public string Mapping;
		public bool IsArray;
	}

	struct SemanticMapping
	{
		public SemanticType Type;
		public CodeFieldReferenceExpression[] ChangeRefs; //may be more than one register set (pixel/vertex/preshader) using this semantic
		public Register Register;
	}

	struct GlobalAttribute
	{
		public Register Register;
		public Type Type;
		public CodeFieldReferenceExpression[] ChangeRefs;
		public CodeFieldReferenceExpression GlobalIdRef;
	}

	public interface IExtensionStatementProvider
	{
		bool IsBlendingBufferNotShared { get; }
		CodeStatement GetBlendDirectAssignment(IShaderDom dom);
		CodeStatement[] GetBlendExtensionAssignments();
		CodeStatement[] GetInstancingExtensionAssignments();
	}

	public sealed class ConstantSetup : DomBase, IExtensionStatementProvider
	{
		private readonly SourceShader source;
		private readonly string techniqueName;
		private readonly AsmTechnique asm;
		private readonly List<string> attributeNames;
		private readonly List<CodeFieldReferenceExpression> attributeFields;
		private readonly List<CodeFieldReferenceExpression> attributeArrayFields;
		private readonly Dictionary<Type, List<CodeStatement>> attributeAssignment;
		private SemanticType[] semanticTypes;
		private readonly List<SemanticMapping> semanticMapping;
		private int semanticMappingRefCount;
		//listings, and their registers (eg, VertexShader and vreg)
		private KeyValuePair<AsmListing, CodeExpression>[] listingRegisters;
		private readonly List<GlobalAttribute> globals;
		private int globalRefCount;

		private readonly List<CodeStatement> blendExtensionAssignments = new List<CodeStatement>();
		private readonly List<CodeStatement> instancingExtensionAssignments = new List<CodeStatement>();

		public ConstantSetup(SourceShader source, string techniqueName, Platform platform)
		{
			this.source = source;
			this.techniqueName = techniqueName;

			this.attributeNames = new List<string>();
			this.attributeFields = new List<CodeFieldReferenceExpression>();
			this.attributeArrayFields = new List<CodeFieldReferenceExpression>();
			this.attributeAssignment = new Dictionary<Type, List<CodeStatement>>();
			this.semanticMapping = new List<SemanticMapping>();
			this.globals = new List<GlobalAttribute>();

			this.asm = source.GetAsmTechnique(techniqueName, platform);

			ComputeAllValidSemantics();
		}

		private void ProcessConstants(IShaderDom shader)
		{
			RegisterSet registers = asm.CommonRegisters;

			for (int i = 0; i < registers.RegisterCount; i++)
			{
				Register reg = registers.GetRegister(i);
				if (reg.Category == RegisterCategory.Float4 ||
					reg.Category == RegisterCategory.Boolean)
				{
					if (reg.Semantic == null)
						this.attributeNames.Add(reg.Name);
					else
						ExtractSemantic(shader, reg);
				}
				else if (reg.Semantic != null && reg.Category != RegisterCategory.Texture && reg.Category != RegisterCategory.Sampler)
				{
					throw new CompileException(string.Format("Error parsing semantic for '{1} {0}'. Semantic bound types may only be processed as Float4, Texture or Sampler registers", reg.Name, reg.Type));
				}
			}
		}

		private void ComputeAllValidSemantics()
		{
			//the methods in IShaderSystem are parsed.
			//anything in the format Set...Type is considered a semantic setter method.
			//eg, SetWorldMatrix is semantic 'WORLD' for Matrix.
			
			//valid types are Matrix, VectorX and Single
			Type[] types = new Type[] { typeof(Matrix), typeof(Vector2), typeof(Vector3), typeof(Vector4), typeof(Single) };
			MethodInfo[] methods = typeof(ShaderSystemBase).GetMethods();

			List<SemanticType> semantics = new List<SemanticType>();

			foreach (MethodInfo method in methods)
			{
				string name = method.Name;

				if (name.Length > 8 &&
					name.StartsWith("Set"))
				{
					//see if it ends with a type

					bool isArray = false;

					if (name.EndsWith("Array", StringComparison.InvariantCultureIgnoreCase))
					{
						isArray = true;
						name = name.Substring(0, name.Length - 5);
					}

					for (int i = 0; i < types.Length; i++)
					{
						if (name.EndsWith(types[i].Name))
						{
							//got it.

							string mapping = name.Substring(3, name.Length - 3 - types[i].Name.Length);

							SemanticType semantic = new SemanticType();
							semantic.Mapping = mapping;
							semantic.Type = types[i];
							semantic.IsArray = isArray;

							semantics.Add(semantic);
							break;
						}
					}
				}
			}

			this.semanticTypes = semantics.ToArray();
		}

		//pull a semantic bound register
		private void ExtractSemantic(IShaderDom shader, Register reg)
		{
			string semantic = reg.Semantic;

			Type dataType = null;
			switch (reg.Rank)
			{
				case RegisterRank.FloatNx1:
				{
					switch (reg.Type)
					{
						case "float":
						case "float1"://?
							dataType = typeof(Single);
							break;
						case "float2":
							dataType = typeof(Vector2);
							break;
						case "float3":
							dataType = typeof(Vector3);
							break;
						case "float4":
							dataType = typeof(Vector4);
							break;
					}
				}
				break;
				case RegisterRank.FloatNx2:
				case RegisterRank.FloatNx3:
				case RegisterRank.FloatNx4:
					dataType = typeof(Matrix);
				break;
				case RegisterRank.IntNx1:
				case RegisterRank.IntNx2:
				case RegisterRank.IntNx3:
				case RegisterRank.IntNx4:
				{
					//ints are almost always mapped to floats for semantic bound types (EG vertex count)
					//since the register category has been validated to Float4, this is the case here
					switch (reg.Type)
					{
						case "int":
						case "int1"://?
							dataType = typeof(Single);
							break;
						case "int2":
							dataType = typeof(Vector2);
							break;
						case "int3":
							dataType = typeof(Vector3);
							break;
						case "int4":
							dataType = typeof(Vector4);
							break;
					}
				}
				break;
				case RegisterRank.Bool:
				dataType = typeof(Single);
				break;
			}

			if (reg.Category == RegisterCategory.Boolean)
				dataType = typeof(bool);
			

			if (semantic.Length == 6 && semantic.Equals("global", StringComparison.InvariantCultureIgnoreCase))
			{
				//special case global value.

				if (dataType == null)
					throw new CompileException(string.Format("Error parsing semantic for '{0}'. Global values of type '{1}' are not supported.",reg.Name, reg.Type));
				
				GlobalAttribute global = new GlobalAttribute();
				global.Register = reg;
				global.Type = dataType;

				global.GlobalIdRef = new CodeFieldReferenceExpression(shader.ShaderClassEx, string.Format("gid{0}", globals.Count));

				List<CodeFieldReferenceExpression> globalRefs = new List<CodeFieldReferenceExpression>();
				List<CodeFieldReferenceExpression> arrayRefs = new List<CodeFieldReferenceExpression>();

				foreach (KeyValuePair<AsmListing, CodeExpression> listing in listingRegisters)
				{
					Register sreg;
					RegisterSet registers = listing.Key.RegisterSet;
					CodeExpression registersRef = listing.Value;

					if (registers.TryGetRegister(reg.Name, out sreg))
					{
						if (sreg.Category != RegisterCategory.Boolean)
						{
							string refId = string.Format("gc{0}", globalRefCount);
							globalRefs.Add(new CodeFieldReferenceExpression(shader.Instance, refId));
							globalRefCount++;
						}
					}
				}

				global.ChangeRefs = globalRefs.ToArray();

				globals.Add(global);
				return;
			}

			//special case
			bool isBlendMatrices = semantic.Equals("BLENDMATRICES", StringComparison.InvariantCultureIgnoreCase);

			if (reg.ArraySize != -1 && !isBlendMatrices)
			{
				//INVALID. EXTERMINATE.
				throw new CompileException(string.Format("Shader attribute '{0}' is defined as an array and has a semantic '{1}'. Semantics other than 'BLENDMATRICES' and 'GLOBAL' are invalid for Array types.", reg.Name, reg.Semantic));
			}

			bool isTranspose = semantic.Length > 9 && semantic.EndsWith("transpose", StringComparison.InvariantCultureIgnoreCase);

			if (isTranspose)
				semantic = semantic.Substring(0, semantic.Length - 9);

			SemanticType? dataSemanticType = null;

			foreach (SemanticType semanticType in semanticTypes)
			{
				if (semanticType.Type == dataType &&
					semanticType.Mapping.Equals(semantic, StringComparison.InvariantCultureIgnoreCase))
				{
					dataSemanticType = semanticType;
					break;
				}
			}

			if (dataSemanticType == null)
			{
				//INVALID. EXTERMINATE.
				throw new CompileException(string.Format("Shader attribute '{0}' has unrecognised semantic '{1}'.", reg.Name, reg.Semantic));
			}

			//create the mapping...
			SemanticMapping mapping = new SemanticMapping();
			mapping.Register = reg;
			mapping.Type = dataSemanticType.Value;

			//figure out how often this semantic is used..
			List<CodeFieldReferenceExpression> changeRefs = new List<CodeFieldReferenceExpression>();

			foreach (KeyValuePair<AsmListing, CodeExpression> listing in listingRegisters)
			{
				Register sreg;
				RegisterSet registers = listing.Key.RegisterSet;
				CodeExpression registersRef = listing.Value;

				if (registers.TryGetRegister(reg.Name, out sreg))
				{
					string changeId = string.Format("sc{0}", semanticMappingRefCount++);
					changeRefs.Add(new CodeFieldReferenceExpression(shader.Instance, changeId));
				}
			}

			mapping.ChangeRefs = changeRefs.ToArray();

			this.semanticMapping.Add(mapping);
		}

		//extract pairs of asmlistings / code registers
		private void ComputeListings(IShaderDom shader)
		{
			List<KeyValuePair<AsmListing, CodeExpression>> listings = new List<KeyValuePair<AsmListing, CodeExpression>>();

			if (asm.VertexShader != null)
				listings.Add(new KeyValuePair<AsmListing,CodeExpression>(asm.VertexShader, shader.VertexShaderRegistersRef));
			
			if (asm.PixelShader != null)
				listings.Add(new KeyValuePair<AsmListing,CodeExpression>(asm.PixelShader, shader.PixelShaderRegistersRef));

			if (asm.BlendingShader != null)
				listings.Add(new KeyValuePair<AsmListing, CodeExpression>(asm.BlendingShader, shader.BlendShaderRegistersRef));

			if (asm.InstancingShader != null)
				listings.Add(new KeyValuePair<AsmListing, CodeExpression>(asm.InstancingShader, shader.InstancingShaderRegistersRef));
			
			this.listingRegisters = listings.ToArray();
		}

		//add the member fields
		public override void SetupMembers(IShaderDom shader)
		{
			ComputeListings(shader);

			ProcessConstants(shader);

			for (int i = 0; i < attributeNames.Count; i++)
			{
				//create a field ref for the static that will be created
				//these are used to assign the value using 'SetAttribute'
				CodeFieldReferenceExpression field = new CodeFieldReferenceExpression(shader.ShaderClassEx, string.Format("cid{0}",i));
				this.attributeFields.Add(field);

				//if it's an array, an array object needs to be created too. Create the ref to it now

				field = null;

				Register reg;
				Type type;//unused
				
				if (ExtractRegType(attributeNames[i], out reg, out type) && reg.ArraySize != -1)
					field = new CodeFieldReferenceExpression(shader.Instance, string.Format("ca{0}", i));

				attributeArrayFields.Add(field);//may be null.
			}
		}

		public override void AddMembers(IShaderDom shader, Action<CodeTypeMember, string> add, Platform platform)
		{
			//create static ID's for the registers
			if (platform != Platform.Both)
				return;

			//static id's for named attributes
			for (int i = 0; i < attributeNames.Count; i++)
			{
				//static id of this attribute name
				CodeMemberField field = new CodeMemberField(typeof(int), attributeFields[i].FieldName);
				field.Attributes = MemberAttributes.Static | MemberAttributes.Private | MemberAttributes.Final;

				add(field, string.Format("Name ID for '{0}'", attributeNames[i]));


				CreateConstantSetters(shader, add, attributeNames[i], attributeFields[i], attributeArrayFields[i]);
			}

			//add the semantic change IDs
			foreach (SemanticMapping mapping in semanticMapping)
			{
				for (int i = 0; i < mapping.ChangeRefs.Length; i++)
				{
					CodeMemberField field = new CodeMemberField(typeof(int), mapping.ChangeRefs[i].FieldName);
					field.Attributes = MemberAttributes.Private | MemberAttributes.Final;

					add(field, i != 0 ? null : string.Format("Change ID for Semantic bound attribute '{0}'", mapping.Register.Name));
				}
			}

			//add the global refs and ids


			//add the global change IDs
			foreach (GlobalAttribute global in globals)
			{
				//global ID staics
				CodeMemberField field = new CodeMemberField(typeof(int), global.GlobalIdRef.FieldName);
				field.Attributes = MemberAttributes.Private | MemberAttributes.Final | MemberAttributes.Static;

				add(field, string.Format("TypeID for global attribute '{0} {1}'", global.Register.Type, global.Register.Name));

				for (int i = 0; i < global.ChangeRefs.Length; i++)
				{
					field = new CodeMemberField(typeof(int), global.ChangeRefs[i].FieldName);
					field.Attributes = MemberAttributes.Private | MemberAttributes.Final;

					add(field, i != 0 ? null : string.Format("Change ID for global attribute '{0} {1}'", global.Register.Type, global.Register.Name));
				}
			}
		}

		public override void AddReadonlyMembers(IShaderDom shader, Action<CodeTypeMember, string> add, Platform platform)
		{
			if (platform != Platform.Both)
				return;
			/*
			for (int i = 0; i < attributeNames.Count; i++)
			{
				//if it's an array, it needs an array object...

				Register reg;
				Type dataType;

				if (ExtractRegType(attributeNames[i], out reg, out dataType) && reg.ArraySize != -1)
				{
					//get the interface type for the field.
					Type interfaceType = typeof(Xen.Graphics.ShaderSystem.Constants.IArray<float>).GetGenericTypeDefinition();
					interfaceType = interfaceType.MakeGenericType(dataType);

					CodeMemberField field = new CodeMemberField(interfaceType, this.attributeArrayFields[i].FieldName);
					field.Attributes = MemberAttributes.Private | MemberAttributes.Final;

					add(field, string.Format("Array wrapper for '{0} {1}[{2}]'", reg.Type, attributeNames[i], reg.ArraySize));
				}
			}

			//add the global array IDs (if there are any)
			foreach (GlobalAttribute global in globals)
			{
				for (int i = 0; i < global.ChangeRefs.Length; i++)
				{
					//if it's an array, also add the array objects
					if (global.Register.ArraySize != -1)
					{
						Type interfaceType = typeof(Xen.Graphics.ShaderSystem.Constants.IArray<float>).GetGenericTypeDefinition();
						interfaceType = interfaceType.MakeGenericType(global.Type);

						//arrays are stored differently.. eg as IArray<Matrix>
						CodeMemberField field = new CodeMemberField(interfaceType, global.ArrayRefs[i].FieldName);
						field.Attributes = MemberAttributes.Private | MemberAttributes.Final;

						add(field, i != 0 ? null : string.Format("Array access for global attribute '{0} {1}[{2}]'", global.Register.Type, global.Register.Name, global.Register.ArraySize));
					}
				}
			}
			*/
		}

		public override void AddConstructor(IShaderDom shader, Action<CodeStatement> add)
		{
			//set the semantic change IDs to -1
			foreach (SemanticMapping mapping in semanticMapping)
			{
				for (int i = 0; i < mapping.ChangeRefs.Length; i++)
				{
					CodeAssignStatement assign = new CodeAssignStatement(mapping.ChangeRefs[i], new CodePrimitiveExpression(-1));

					add(assign);
				}
			}

			//set the global change IDs to -1
			foreach (GlobalAttribute global in globals)
			{
				int changeRefIndex = 0;
				foreach (KeyValuePair<AsmListing, CodeExpression> listing in listingRegisters)
				{
					Register sreg;
					RegisterSet registers = listing.Key.RegisterSet;
					CodeExpression registersRef = listing.Value;

					if (registers.TryGetRegister(global.Register.Name, out sreg) && sreg.Category != RegisterCategory.Boolean)
					{
						CodeAssignStatement assign = new CodeAssignStatement(global.ChangeRefs[changeRefIndex], new CodePrimitiveExpression(-1));

						add(assign);

						changeRefIndex++;
					}
				}
			}
		}

		public override void AddBind(IShaderDom shader, Action<CodeStatement, string> add)
		{
			bool requiresUnused = false;

			//bind the semantics bound attributes
			foreach (SemanticMapping mapping in semanticMapping)
			{
				//eg:
				//state.SetWorldMatrix(this.vreg.Matrix4Transpose(8), ref this.v_8);

				string method = string.Format("Set{0}{1}", mapping.Type.Mapping, mapping.Type.Type.Name);

				if (mapping.Type.IsArray)
					method += "Array";

				//for each register set, see if it uses this mapping

				int changeRefIndex = 0;
				foreach (KeyValuePair<AsmListing, CodeExpression> listing in listingRegisters)
				{
					Register sreg;
					RegisterSet registers = listing.Key.RegisterSet;
					CodeExpression registersRef = listing.Value;

					if (registers.TryGetRegister(mapping.Register.Name, out sreg))
					{
						//it does.. so the constants need setting..
						//changed |= state.SetWorldMatrix(ref this.vreg[8], ref this.vreg[9], ref this.vreg[9], ref unused, ref this.v_8);

						CodeExpression changeRef = Ref(mapping.ChangeRefs[changeRefIndex]);

						CodeExpression getRegisterX =	//this.vreg[8]
							new CodeArrayIndexerExpression(registersRef, new CodePrimitiveExpression(sreg.Index));

						//invoke
						CodeExpression invokeSet = null;

						if (mapping.Type.IsArray)
						{
							invokeSet = new CodeMethodInvokeExpression(shader.ShaderSystemRef, method, registersRef, new CodePrimitiveExpression(sreg.Index), new CodePrimitiveExpression(sreg.Size), changeRef);
						}
						else
						{
							if (mapping.Type.Type == typeof(Matrix))
							{
								int rank = (int)mapping.Register.Rank;
								if (rank != 4 && requiresUnused == false) // an 'unused' variable is required
								{
									//add a temporary. A matrix being set may not use all 4 rows. Need a temp write target
									add(new CodeVariableDeclarationStatement(typeof(Vector4), "unused", new CodeObjectCreateExpression(typeof(Vector4))), null);
									requiresUnused = true;
								}

								//setter takes in X,Y,Z,W registers
								CodeExpression unused = new CodeVariableReferenceExpression("unused");
								CodeExpression Y = rank >= 2 ? new CodeArrayIndexerExpression(registersRef, new CodePrimitiveExpression(sreg.Index + 1)) : unused;
								CodeExpression Z = rank >= 3 ? new CodeArrayIndexerExpression(registersRef, new CodePrimitiveExpression(sreg.Index + 2)) : unused;
								CodeExpression W = rank >= 4 ? new CodeArrayIndexerExpression(registersRef, new CodePrimitiveExpression(sreg.Index + 3)) : unused;

								invokeSet = new CodeMethodInvokeExpression(shader.ShaderSystemRef, method, Ref(getRegisterX), Ref(Y), Ref(Z), Ref(W), changeRef);
							}
							else
								invokeSet = new CodeMethodInvokeExpression(shader.ShaderSystemRef, method, Ref(getRegisterX), changeRef);
						}

						//update the change value appropriately
						CodeExpression changeValue = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), (registersRef as CodeFieldReferenceExpression).FieldName + "_change");

						//change |= ...;
						CodeStatement assign = new CodeAssignStatement(changeValue, 
							new CodeBinaryOperatorExpression(changeValue, CodeBinaryOperatorType.BitwiseOr,
								invokeSet));

						if (ExtractAssignmentForExtenionBlock(assign, shader, listing.Key))
							continue;

						add(assign, changeRefIndex != 0 ? null : string.Format("Set the value for attribute '{0}'", mapping.Register.Name));

						changeRefIndex++;
					}
				}
			}

			//bind the shader globals

			foreach (GlobalAttribute global in globals)
			{
				string registerTypeName = global.Type.Name;
				if (global.Type == typeof(Matrix))
				{
					registerTypeName += (int)global.Register.Rank;
				}
				
				int changeRefIndex = 0;
				foreach (KeyValuePair<AsmListing, CodeExpression> listing in listingRegisters)
				{
					Register sreg;
					RegisterSet registers = listing.Key.RegisterSet;
					CodeExpression registersRef = listing.Value;

					bool isMatrix = global.Type == typeof(Matrix);

					if (registers.TryGetRegister(global.Register.Name, out sreg))
					{
						//special case the booleans, as they have different logic to set globally.
						if (sreg.Category == RegisterCategory.Boolean)
						{
							if (global.Register.ArraySize != -1)
								throw new CompileException("'GLOBAL' Boolean Arrays are not supported");


							//this is a bit of a hack :-/
							//need to figure out if this is a vertex or pixel boolean constant.
							if (listing.Key == asm.VertexShader)
							{
								CodeExpression setValue = new CodeMethodInvokeExpression(shader.ShaderSystemRef, "SetGlobalBool", shader.VertexShaderBooleanRegistersRef, new CodePrimitiveExpression(sreg.Index), global.GlobalIdRef);

								//update the change flag
								CodeExpression invoke = new CodeBinaryOperatorExpression(
									shader.VertexShaderBooleanRegistersChangedRef, CodeBinaryOperatorType.BitwiseOr,
									setValue);

								add(shader.ETS(invoke),
									string.Format("Set the value for global 'bool {0}'", global.Register.Name));
							}
							if (listing.Key == asm.PixelShader)
							{
								CodeExpression setValue = new CodeMethodInvokeExpression(shader.ShaderSystemRef, "SetGlobalBool", shader.PixelShaderBooleanRegistersRef, new CodePrimitiveExpression(sreg.Index), global.GlobalIdRef);
								
								//update the change flag
								CodeExpression invoke = new CodeBinaryOperatorExpression(
									shader.PixelShaderBooleanRegistersChangedRef, CodeBinaryOperatorType.BitwiseOr,
									setValue);

								add(shader.ETS(invoke),
									string.Format("Set the value for global 'bool {0}'", global.Register.Name));
							}
						}
						else
						{
							//eg:
							//changed |= state.SetGlobalMatrix3(ref this.vreg[8], ref this.vreg[9], ref this.vreg[10], ShadowShaderBlend.g_id0, ref this.g_0);

							CodeExpression getRegisterX =	//this.vreg[8]
								new CodeArrayIndexerExpression(registersRef, new CodePrimitiveExpression(sreg.Index));

							string methodName = "SetGlobal" + global.Type.Name;
							int rank = (int)global.Register.Rank;

							if (isMatrix)
								methodName += rank;

							//eg, SetGlobalMatrix3

							CodeExpression changeRef = Ref(global.ChangeRefs[changeRefIndex]);
							CodeExpression invokeSet;

							//logic changes for arrays

							if (global.Register.ArraySize != -1)
							{
								//SetGlobalMatrix3 for Arrays takes in the array itself as the first arg.
								invokeSet =
									new CodeMethodInvokeExpression(shader.ShaderSystemRef, methodName, registersRef, new CodePrimitiveExpression(global.Register.Index), new CodePrimitiveExpression(global.Register.Size + global.Register.Index), global.GlobalIdRef, changeRef);
								//state.SetGlobal(this.ga0, ShadowShaderBlend.g_id0, ref this.g_0);
							}
							else
							{
								//SetGlobalMatrix3 for non-arays takes a variable number of arguements
								if (isMatrix)
								{
									List<CodeExpression> args = new List<CodeExpression>();
									//add X
									args.Add(Ref(getRegisterX));
									if (rank >= 2) args.Add(Ref(new CodeArrayIndexerExpression(registersRef, new CodePrimitiveExpression(sreg.Index + 1))));
									if (rank >= 3) args.Add(Ref(new CodeArrayIndexerExpression(registersRef, new CodePrimitiveExpression(sreg.Index + 2))));
									if (rank >= 4) args.Add(Ref(new CodeArrayIndexerExpression(registersRef, new CodePrimitiveExpression(sreg.Index + 3))));

									args.Add(global.GlobalIdRef);
									args.Add(changeRef);
									
									invokeSet =
										new CodeMethodInvokeExpression(shader.ShaderSystemRef, methodName, args.ToArray());
								}
								else
									invokeSet =
										new CodeMethodInvokeExpression(shader.ShaderSystemRef, methodName, Ref(getRegisterX), global.GlobalIdRef, changeRef);
							}

								
							//update the change value appropriately
							CodeExpression changeValue = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), (registersRef as CodeFieldReferenceExpression).FieldName + "_change");

							//change |= ...;
							CodeStatement assign = new CodeAssignStatement(changeValue, 
								new CodeBinaryOperatorExpression(changeValue, CodeBinaryOperatorType.BitwiseOr,
									invokeSet));

							if (ExtractAssignmentForExtenionBlock(assign, shader, listing.Key))
								continue;

							add(assign, changeRefIndex != 0 ? null : string.Format("Set the value for global '{0}'", global.Register.Name));

							changeRefIndex++;
						}
					}
				}
			}
		}

		private bool ExtractAssignmentForExtenionBlock(CodeStatement assign, IShaderDom shader, AsmListing asmListing)
		{
			//special case, blending or instancing assignments need to go in an if block.

			if (asmListing == asm.BlendingShader)
			{
				blendExtensionAssignments.Add(assign);
				return true;
			}

			if (asmListing == asm.InstancingShader)
			{
				instancingExtensionAssignments.Add(assign);
				return true;
			}

			return false;
		}

		//implement the provider interface, as used by ShaderRegisters class
		public CodeStatement[] GetBlendExtensionAssignments()
		{
			return blendExtensionAssignments.ToArray();
		}
		public CodeStatement[] GetInstancingExtensionAssignments()
		{
			return instancingExtensionAssignments.ToArray();
		}

		//true if the blend matrices are guarenteed to be the only values stored in the blending specific constants
		public bool IsBlendingBufferNotShared
		{
			get { return source.ManualExtensions == false; }
		}

		public CodeStatement GetBlendDirectAssignment(IShaderDom dom)
		{
			foreach (SemanticMapping mapping in semanticMapping)
			{
				if (mapping.Register.Name == ShaderExtensionGenerator.blendMatricesName)
				{
					//call the direct blend matrix setter
					var invokeSet = new CodeMethodInvokeExpression(dom.ShaderSystemRef, "SetBlendMatricesDirect", new CodeFieldReferenceExpression(dom.EffectRef, "vsb_c"), Ref(mapping.ChangeRefs[0]));

					//change |= ...;
					return new CodeAssignStatement(dom.BindShaderInstanceChange,
						new CodeBinaryOperatorExpression(dom.BindShaderInstanceChange, CodeBinaryOperatorType.BitwiseOr,
							invokeSet));
				}
			}
			return null;
		}


		//adds 'ref' to an expression
		private static CodeExpression Ref(CodeExpression expression)
		{
			return new CodeDirectionExpression(FieldDirection.Ref, expression);
		}

		public override void AddStaticGraphicsInit(IShaderDom shader, Action<CodeStatement, string> add)
		{
			//initalise the static ID's

			for (int i = 0; i < attributeNames.Count; i++)
			{
				//call state.GetNameUniqueID()

				CodeExpression call = new CodeMethodInvokeExpression(
					new CodeMethodReferenceExpression(shader.ShaderSystemRef, "GetNameUniqueID"),
					new CodePrimitiveExpression(attributeNames[i]));

				CodeStatement assign = new CodeAssignStatement(attributeFields[i], call);

				add(assign, null);
			}

			//setup the TypeIDs for global attributes
			//eg:
			//ShadowShaderBlend.g_id0 = state.GetGlobalUniqueID<Microsoft.Xna.Framework.Matrix[]>("shadowMapProjection");

			foreach (GlobalAttribute global in globals)
			{
				//call state.GetGlobalUniqueID()
				Type type = global.Type;
				if (global.Register.ArraySize != -1)
					type = type.MakeArrayType();
				
				CodeExpression call = new CodeMethodInvokeExpression(
					new CodeMethodReferenceExpression(shader.ShaderSystemRef, "GetGlobalUniqueID", new CodeTypeReference(type)),
					new CodePrimitiveExpression(global.Register.Name));

				CodeStatement assign = new CodeAssignStatement(global.GlobalIdRef, call);

				add(assign, null);
			}
		}


		private bool ExtractRegType(string name, out Register reg, out Type dataType)
		{
			bool hasSetMethod;
			int stride;
			return ExtractRegType(name, out reg, out dataType, out hasSetMethod, out stride);
		}
		private bool ExtractRegType(string name, out Register reg, out Type dataType, out bool hasSetMethod, out int stride)
		{
			dataType = null;
			hasSetMethod = true;
			stride = 1;
			reg = new Register();

			if (!this.asm.CommonRegisters.TryGetRegister(name, out reg))
				return false;
			
			if (reg.Category == RegisterCategory.Boolean)
			{
				dataType = typeof(bool);
				hasSetMethod = false;
				return true;
			}

			if (reg.Category != RegisterCategory.Float4)
				return false;

			switch (reg.Type)
			{
				case "float":
				case "float1"://?
				case "int": // integers or bools may be processed as floats by the FX compiler
				case "int1":
				case "bool":
					dataType = typeof(float);
					hasSetMethod = false;
					break;
				case "float2":
				case "int2":
					dataType = typeof(Vector2);
					break;
				case "float3":
				case "int3":
					dataType = typeof(Vector3);
					break;
				case "float4":
				case "int4":
					dataType = typeof(Vector4);
					break;

				default:
					if (reg.Rank >= RegisterRank.IntNx1)
						return false;
					dataType = typeof(Matrix);
					stride = (int)reg.Rank;
					break;
			}
			return true;
		}

		private void CreateConstantSetters(IShaderDom shader, Action<CodeTypeMember, string> add, string name, CodeExpression assignmentField, CodeExpression assignmentArrayField)
		{
			/*
			 * Something like:

			public void SetInvTargetSize(ref Microsoft.Xna.Framework.Vector2 value)
			{
				this.vreg.SetVector2(130, ref value);
			}

			public Microsoft.Xna.Framework.Vector2 InvTargetSize
			{
				set
				{
					this.SetInvTargetSize(ref value);
				}
			}*/

			Register reg;
			Type dataType;
			bool hasSetMethod;
			int stride;

			if (!ExtractRegType(name, out reg, out dataType, out hasSetMethod, out stride))
				return;

			Type arrayOrSingleType = dataType;

			//right...

			//create the method of the given type.


			//public void SetInvTargetSize(ref Microsoft.Xna.Framework.Vector2 value)
			CodeStatementCollection methodStatements = new CodeStatementCollection();

			CodeParameterDeclarationExpression param = new CodeParameterDeclarationExpression(dataType, "value");
			List<CodeParameterDeclarationExpression> additionalParams = new List<CodeParameterDeclarationExpression>();

			if (reg.ArraySize == -1)
				param.Direction = FieldDirection.Ref;
			else
			{
				arrayOrSingleType = dataType.MakeArrayType();
				param.Type = new CodeTypeReference(arrayOrSingleType);

				//add array params, readIndex, writeIndex, count
				additionalParams.Add(new CodeParameterDeclarationExpression(typeof(uint), "readIndex"));
				additionalParams.Add(new CodeParameterDeclarationExpression(typeof(uint), "writeIndex"));
				additionalParams.Add(new CodeParameterDeclarationExpression(typeof(uint), "count"));
			}

			CodeExpression valueRef = new CodeArgumentReferenceExpression(param.Name);

			//when there isn't a set method, there is just a set property
			if (!hasSetMethod)
				valueRef = new CodePropertySetValueReferenceExpression();

			//create the guts
			//depends on what constants use it...

			//eg:
			//this.vreg.SetVector2(130, ref value);

			Register sreg;

			if (dataType == typeof(bool))
			{
				//special case for booleans, assign the array directly.
				//looks like:
				// 
				// if (preg_bool[index] != value)
				// {
				//  preg_bool[index] = value;
				//  preg_bool_changed = true;
				// }

				foreach (KeyValuePair<AsmListing, CodeExpression> listing in listingRegisters)
				{
					RegisterSet registers = listing.Key.RegisterSet;
					CodeExpression registersRef = listing.Value;

					if (registers.TryGetRegister(name, out sreg))
					{
						if (listing.Key == asm.PixelShader)
						{
							CodeExpression arrayIndex = new CodeArrayIndexerExpression(shader.PixelShaderBooleanRegistersRef, new CodePrimitiveExpression(sreg.Index));

							CodeStatement assign = new CodeAssignStatement(arrayIndex, new CodePropertySetValueReferenceExpression());
							CodeStatement change = new CodeAssignStatement(shader.PixelShaderBooleanRegistersChangedRef, new CodePrimitiveExpression(true));

							CodeStatement condition = new CodeConditionStatement(
								new CodeBinaryOperatorExpression(arrayIndex, CodeBinaryOperatorType.IdentityInequality, new CodePropertySetValueReferenceExpression()),
								new CodeStatement[]{assign, change});

							methodStatements.Add(condition);
						}
						if (listing.Key == asm.VertexShader)
						{
							CodeExpression arrayIndex = new CodeArrayIndexerExpression(shader.VertexShaderBooleanRegistersRef, new CodePrimitiveExpression(sreg.Index));

							CodeStatement assign = new CodeAssignStatement(arrayIndex, new CodePropertySetValueReferenceExpression());
							CodeStatement change = new CodeAssignStatement(shader.VertexShaderBooleanRegistersChangedRef, new CodePrimitiveExpression(true));

							CodeStatement condition = new CodeConditionStatement(
								new CodeBinaryOperatorExpression(arrayIndex, CodeBinaryOperatorType.IdentityInequality, new CodePropertySetValueReferenceExpression()),
								new CodeStatement[] { assign, change });

							methodStatements.Add(condition);
						}
					}
				}
			}
			else
			{
				//some array set methods require temporary values, but should only be created once.
				bool tempValuesCreated = false;

				foreach (KeyValuePair<AsmListing, CodeExpression> listing in listingRegisters)
				{
					RegisterSet registers = listing.Key.RegisterSet;
					CodeExpression registersRef = listing.Value;

					if (registers.TryGetRegister(name, out sreg))
					{
						//Assign the register array data
						AssignRegister(dataType, sreg, reg, listing.Value, valueRef, methodStatements, ref tempValuesCreated);

						//set changed
						CodeExpression changeValue = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), (registersRef as CodeFieldReferenceExpression).FieldName + "_change");

						methodStatements.Add(new CodeAssignStatement(changeValue, new CodePrimitiveExpression(true)));
					}
				}
			}

			string upperName = Common.ToUpper(name);

			//there is always a setable property
			CodeMemberProperty property = new CodeMemberProperty();
			property.Name = upperName;
			property.Type = param.Type;
			property.Attributes = MemberAttributes.Final | MemberAttributes.Public;
			property.HasSet = true;
			property.HasGet = false;

			//there isn't always a set method
			CodeMemberMethod method = null;

			CodeStatement assignAttribute = null;

			if (hasSetMethod || reg.ArraySize != -1)
			{
				//create the method to set the value
				string methodName = "Set" + upperName;

				method = new CodeMemberMethod();
				method.Name = methodName;
				method.Attributes = MemberAttributes.Final | MemberAttributes.Public;

				method.Parameters.Add(param);
				method.Parameters.AddRange(additionalParams.ToArray());
				method.Statements.AddRange(methodStatements);


				//create a property that calls the Set method

				//is not an array
				CodeMethodInvokeExpression invokeSetter =
					new CodeMethodInvokeExpression(
						shader.Instance, method.Name,
						new CodeDirectionExpression(reg.ArraySize == -1 ? FieldDirection.Ref : FieldDirection.In, new CodePropertySetValueReferenceExpression()));

				if (reg.ArraySize != -1)
				{
					//add method ops (readIndex, writeIndex, count)
					invokeSetter.Parameters.Add(new CodePrimitiveExpression(0));
					invokeSetter.Parameters.Add(new CodePrimitiveExpression(0));
					invokeSetter.Parameters.Add(new CodeCastExpression(typeof(uint), new CodePropertyReferenceExpression(valueRef, "Length")));
				}

				property.SetStatements.Add(invokeSetter);



				//call the method as well for attribute assign
				CodeMethodInvokeExpression assignSetter = 
					new CodeMethodInvokeExpression(
						shader.Instance, method.Name,
						new CodeDirectionExpression(param.Direction, shader.AttributeAssignValue));

				if (reg.ArraySize != -1)
				{
					//add method ops (readIndex, writeIndex, count)
					assignSetter.Parameters.Add(new CodePrimitiveExpression(0));
					assignSetter.Parameters.Add(new CodePrimitiveExpression(0));
					assignSetter.Parameters.Add(new CodeCastExpression(typeof(uint), new CodePropertyReferenceExpression(valueRef, "Length")));
				}

				assignAttribute = shader.ETS(assignSetter);
			}
			else
			{
				//create a property to directly set the value

				property.SetStatements.AddRange(methodStatements);

				//attribute assign sets the property
				assignAttribute = new CodeAssignStatement(
					new CodePropertyReferenceExpression(shader.Instance, property.Name),
						shader.AttributeAssignValue);
			}
			

			if (reg.ArraySize > 0)
			{
				if (method != null)
					add(method, string.Format("Set the shader array value '{0} {1}[{2}]'", reg.Type, reg.Name, reg.ArraySize));
				add(property, string.Format("Set and copy the array data for the shader value '{0} {1}[{2}]'", reg.Type, reg.Name, reg.ArraySize));
			}
			else
			{
				if (method != null)
					add(method, string.Format("Set the shader value '{0} {1}'", reg.Type, reg.Name));
				add(property, string.Format("Assign the shader value '{0} {1}'", reg.Type, reg.Name));
			}

			//create the attribute assignment value statement.

			List<CodeStatement> assignList;
			if (!attributeAssignment.TryGetValue(arrayOrSingleType, out assignList))
			{
				assignList = new List<CodeStatement>();
				attributeAssignment.Add(arrayOrSingleType, assignList);
			}

			//create the statement...
			
			CodeExpression assignIdsMatch =
				new CodeBinaryOperatorExpression(shader.AttributeAssignId,  CodeBinaryOperatorType.IdentityEquality, assignmentField);

			CodeConditionStatement performAssign =
				new CodeConditionStatement(assignIdsMatch,
					assignAttribute, //call the assignment code
					new CodeMethodReturnStatement(new CodePrimitiveExpression(true))); //return true, set correctly.

			assignList.Add(performAssign);
		}

		private void AssignRegister(Type dataType, Register targetRegister, Register sourceRegister, CodeExpression codeExpression, CodeExpression valueRef, CodeStatementCollection statements, ref bool tempValuesCreated)
		{
			int arrayLength = Math.Max(1, targetRegister.ArraySize);
			bool isArray = targetRegister.ArraySize > 0;

			CodeExpression temp = null, loopIndex = null;
			if (isArray)
			{
				if (!tempValuesCreated)
				{
					statements.Add(new CodeVariableDeclarationStatement(dataType, "val"));
					statements.Add(new CodeVariableDeclarationStatement(typeof(int), "i"));

					//copies of readindex/writeindex
					statements.Add(new CodeVariableDeclarationStatement(typeof(uint), "ri"));
					statements.Add(new CodeVariableDeclarationStatement(typeof(uint), "wi"));
					tempValuesCreated = true;
				}

				loopIndex = new CodeVariableReferenceExpression("i");
				temp = new CodeVariableReferenceExpression("val");

				statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("ri"), new CodeArgumentReferenceExpression("readIndex")));
				statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("wi"), new CodeArgumentReferenceExpression("writeIndex")));
			}
			else
				loopIndex = new CodePrimitiveExpression(0);

			bool isMatrix = dataType == typeof(Matrix);
			bool isFloat = targetRegister.Rank >= RegisterRank.FloatNx1 && targetRegister.Rank <= RegisterRank.FloatNx4;
		
			int count = 1;
			switch (targetRegister.Rank)
			{
				case RegisterRank.FloatNx2:
				case RegisterRank.IntNx2:
					count = 2;
					break;
				case RegisterRank.FloatNx3:
				case RegisterRank.IntNx3:
					count = 3;
					break;
				case RegisterRank.FloatNx4:
				case RegisterRank.IntNx4:
					count = 4;
					break;
			}

			CodeExpression countRef = new CodeArgumentReferenceExpression("count");
			CodeExpression readIndexRef = new CodeArgumentReferenceExpression("ri");
			CodeExpression writeIndexRef = new CodeArgumentReferenceExpression("wi");
			CodeExpression valueLength = new CodePropertyReferenceExpression(valueRef, "Length");

			//check the incomming array
			if (isArray)
			{
				arrayLength = targetRegister.Size / count;

				//validate the input is not null
				CodeConditionStatement check = new CodeConditionStatement(
					new CodeBinaryOperatorExpression(valueRef, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null)),
					new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(ArgumentNullException),new CodePrimitiveExpression("value"))));
					
				statements.Add(check);

				//validate the array args are good.
				/*
			if (readIndex + count > value.Length ||
				writeIndex + count > 10)
				throw new System.ArgumentException("Input array is either null or too short");
*/
				CodeExpression readCompare = new CodeBinaryOperatorExpression(
					new CodeBinaryOperatorExpression(readIndexRef, CodeBinaryOperatorType.Add, countRef),
					 CodeBinaryOperatorType.GreaterThan, valueLength);
				CodeExpression writeCompare = new CodeBinaryOperatorExpression(
					new CodeBinaryOperatorExpression(writeIndexRef, CodeBinaryOperatorType.Add, countRef),
					 CodeBinaryOperatorType.GreaterThan, new CodePrimitiveExpression(sourceRegister.ArraySize));

				check = new CodeConditionStatement(
					new CodeBinaryOperatorExpression(readCompare, CodeBinaryOperatorType.BooleanOr, writeCompare),
					new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(ArgumentException),
						new CodePrimitiveExpression("Invalid range"))));

				statements.Add(check);
			}
			List<CodeStatement> innerStatements = new List<CodeStatement>();

			//genrate the code to set the registers
			{
				CodeExpression valueTarget = valueRef;

				if (targetRegister.ArraySize > 0)
				{
					innerStatements.Add(new CodeAssignStatement(temp, new CodeArrayIndexerExpression(valueRef, readIndexRef)));
					valueTarget = temp;
				}

				for (int i = 0; i < count; i++)
				{
					//register index to write:
					CodeExpression arrayIndex = new CodePrimitiveExpression(i + targetRegister.Index);

					//loop is a bit more complex: loopIndex * count + i + reg.Index
					if (isArray)
						arrayIndex = 
							new CodeBinaryOperatorExpression(
								new CodeBinaryOperatorExpression(writeIndexRef, CodeBinaryOperatorType.Multiply, new CodePrimitiveExpression(count)),
								CodeBinaryOperatorType.Add,
								arrayIndex);

					CodeExpression assignTarget = new CodeArrayIndexerExpression(codeExpression,
						arrayIndex);

					//float3x2 is rank 2, so 2 vectors are written.
					//each vector is 3 elements, so work out how many to write in XYZW per vector:
					
					//default the elements to zero.
					CodeExpression x = new CodePrimitiveExpression(0.0f), y = x, z = x, w = x;

					//get the first number of the '3x2' part of the type, eg from float3x2
					int number = 1;

					//find the first number in the type name, if there is one.
					foreach (char c in targetRegister.Type)
					{
						if (char.IsNumber(c))
						{
							number = int.Parse(c.ToString());
							break;
						}
					}
					
					//simple assignment:
					if (number == 4 && count == 1) // float4 or int4
					{
						//directly assign the value
						innerStatements.Add(new CodeAssignStatement(assignTarget, valueTarget));
						continue;
					}
					
					//set the x,y,z,w coords
					if (number >= 4)
						w = new CodeFieldReferenceExpression(valueTarget, isMatrix ? "M4" + (i+1) : "W");
					if (number >= 3)
						z = new CodeFieldReferenceExpression(valueTarget, isMatrix ? "M3" + (i+1) : "Z");
					if (number >= 2)
						y = new CodeFieldReferenceExpression(valueTarget, isMatrix ? "M2" + (i+1) : "Y");
					if (!isMatrix && number == 1)
						x = valueTarget; // float doesn't have a member
					else
						x = new CodeFieldReferenceExpression(valueTarget, isMatrix ? "M1" + (i+1) : "X");

					//and assign..
					innerStatements.Add(new CodeAssignStatement(assignTarget, new CodeObjectCreateExpression(typeof(Vector4), x,y,z,w)));
				}
			}

			if (isArray)
			{
				//generate the assignment loop
					
			//	for (int i = 0; i < count; i++)
			//	{
			//	...
			//	readIndex++, writeIndex++
			//	}

				//add readIndex & writeIndex increment
				innerStatements.Add(new CodeAssignStatement(readIndexRef, new CodeBinaryOperatorExpression(readIndexRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))));
				innerStatements.Add(new CodeAssignStatement(writeIndexRef, new CodeBinaryOperatorExpression(writeIndexRef, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))));

				CodeIterationStatement loop = new CodeIterationStatement(
					new CodeAssignStatement(loopIndex,new CodePrimitiveExpression(0)),
					new CodeBinaryOperatorExpression(
						new CodeBinaryOperatorExpression(loopIndex, CodeBinaryOperatorType.LessThan, countRef),
						CodeBinaryOperatorType.BooleanAnd,
						new CodeBinaryOperatorExpression(writeIndexRef, CodeBinaryOperatorType.LessThan, new CodePrimitiveExpression(arrayLength))),
					new CodeAssignStatement(loopIndex,new CodeBinaryOperatorExpression(loopIndex, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
					innerStatements.ToArray());
				statements.Add(loop);
			}
			else
				statements.AddRange(innerStatements.ToArray());
		}

		public override void AddSetAttribute(IShaderDom shader, Action<CodeStatement> add, Type type)
		{
			List<CodeStatement> statements;

			if (attributeAssignment.TryGetValue(type, out statements))
			{
				foreach (CodeStatement statement in statements)
				{
					add(statement);
				}
			}
		}
	}
}
