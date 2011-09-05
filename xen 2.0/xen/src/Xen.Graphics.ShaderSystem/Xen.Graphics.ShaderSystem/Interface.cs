using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xen.Graphics;
using Xen.Graphics.ShaderSystem;


namespace Xen.Graphics
{
	//******************************************************************//
	// Do not modify this file from within the main Xen solution files, //
	// Only modify it from within the main Shader System solution!      //
	//******************************************************************//

	/// <summary>
	/// Allows for dynamic use of the shader
	/// </summary>
	public interface IShader
	{
		/// <summary>This method is called by the shader system, do not call this method directly.</summary>
		void Begin(ShaderSystemBase shaderSystem, bool instanceChanged, bool extensionChanged, ShaderExtension extension);
 
		/// <summary>
		/// True if a non-global has changed since this shader was last bound
		/// </summary>
		/// <returns></returns>
		bool HasChanged { get; }

		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		bool SetAttribute(ShaderSystemBase state, int name_uid, bool value);
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		bool SetAttribute(ShaderSystemBase state, int name_uid, ref Matrix value);
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		bool SetAttribute(ShaderSystemBase state, int name_uid, ref Vector4 value);
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		bool SetAttribute(ShaderSystemBase state, int name_uid, ref Vector3 value);
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		bool SetAttribute(ShaderSystemBase state, int name_uid, ref Vector2 value);
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		bool SetAttribute(ShaderSystemBase state, int name_uid, float value);
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		bool SetAttribute(ShaderSystemBase state, int name_uid, Matrix[] value);
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		bool SetAttribute(ShaderSystemBase state, int name_uid, Vector4[] value);
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		bool SetAttribute(ShaderSystemBase state, int name_uid, Vector3[] value);
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		bool SetAttribute(ShaderSystemBase state, int name_uid, Vector2[] value);
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		bool SetAttribute(ShaderSystemBase state, int name_uid, float[] value);
		/// <summary>
		/// Set a shader texture
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the texture name</param>
		/// <param name="texture">texture to set</param>
		/// <returns>true if the texture was set</returns>
		bool SetTexture(ShaderSystemBase state, int name_uid, Texture texture);
		/// <summary>
		/// Set a shader texture
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the texture name</param>
		/// <param name="texture">texture to set</param>
		/// <returns>true if the texture was set</returns>
		bool SetTexture(ShaderSystemBase state, int name_uid, Texture2D texture);
		/// <summary>
		/// Set a shader texture
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the texture name</param>
		/// <param name="texture">texture to set</param>
		/// <returns>true if the texture was set</returns>
		bool SetTexture(ShaderSystemBase state, int name_uid, Texture3D texture);
		/// <summary>
		/// Set a shader texture
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the texture name</param>
		/// <param name="texture">texture to set</param>
		/// <returns>true if the texture was set</returns>
		bool SetTexture(ShaderSystemBase state, int name_uid, TextureCube texture);
		/// <summary>
		/// Set a shader texture sampler state
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the sampler state name</param>
		/// <param name="sampler">sampler state to set</param>
		/// <returns>true if the sampler state was set</returns>
		bool SetSamplerState(ShaderSystemBase state, int name_uid, TextureSamplerState sampler);
		/// <summary>
		/// Get the number of vertex inputs required by the shader
		/// </summary>
		/// <returns>Returns the number of vertex inputs required by the shader</returns>
		int GetVertexInputCount();
		/// <summary>
		/// Gets a vertex input required by the shader, by index. <see cref="GetVertexInputCount"/> to get the number of inputs required.
		/// </summary>
		/// <param name="index">Index of the element</param>
		/// <param name="elementUsage">Gets the usage type of the vertex element (eg, <see cref="VertexElementUsage.Position"/>)</param>
		/// <param name="elementIndex">Gets the index of the vertex element (eg, there may be more than one <see cref="VertexElementUsage.TextureCoordinate"/>)</param>
		/// <remarks><para>Implementations should return elements in logical order, first all Position (0) elements in elementIndex order, all BlendWeights (1), etc.</para></remarks>
		void GetVertexInput(int index, out VertexElementUsage elementUsage, out int elementIndex);
		
		/// <summary>Returns value to indicate if this shader instance supports animation blending or instancing</summary>
		void GetExtensionSupport(out bool blendingSupport, out bool instancingSupport);
	}
}

namespace Xen.Graphics.ShaderSystem
{
	/// <summary>
	/// Base shader class, provides empty implementations of the IShader methods, to reduce generated code
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough]
	public abstract class BaseShader : IShader
	{
		//the 'Impl' extension is short for Implementation.
		//in this case, there is a bug on the Xbox whereby sometimes calling a protected method
		//from a hidden interface method can throw an access exception. No idea why, probably a bug in the .net CF.

		/// <summary>This method is called by the shader system, do not call this method directly.</summary>
		protected abstract void BeginImpl(ShaderSystemBase shaderSystem, bool instanceChanged, bool extensionChanged, ShaderExtension extension);

		/// <summary>This method is called by the shader system, do not call this method directly.</summary>
		void IShader.Begin(ShaderSystemBase shaderSystem, bool instanceChanged, bool extensionChanged, ShaderExtension extension)
		{
			this.BeginImpl(shaderSystem, instanceChanged, extensionChanged, extension);
		}

		/// <summary>Returns value to indicate if this shader instance supports animation blending or instancing</summary>
		protected virtual void GetExtensionSupportImpl(out bool blendingSupport, out bool instancingSupport)
		{
			blendingSupport = false;
			instancingSupport = false;
		}
		void IShader.GetExtensionSupport(out bool blendingSupport, out bool instancingSupport)
		{
			GetExtensionSupportImpl(out blendingSupport, out instancingSupport);
		}

#if DEBUG
		int GraphicsDeviceID = -1;
#endif

		/// <summary>
		/// Gets the graphics ID associated with this instance (debug only)
		/// </summary>
		protected int GraphicsID
		{
			get
			{
#if DEBUG
				return GraphicsDeviceID;
#else
				return -1;
#endif
			}
			set
			{
#if DEBUG
				if (GraphicsDeviceID != value && GraphicsDeviceID != -1)
					throw new InvalidOperationException(string.Format("Attempting to use a shader instance of type '{0}' that was created in another application! This could crash the XNA runtime!", GetType().FullName));
				GraphicsDeviceID = value;
#endif
			}
		}

		/// <summary>
		/// True if a non-global has changed since this shader was last bound
		/// </summary>
		/// <returns></returns>
		protected abstract bool Changed();
		bool IShader.HasChanged { get { return Changed(); } }

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, bool value) { return SetAttributeImpl(state, name_uid, value); }
		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, ref Matrix value) { return SetAttributeImpl(state, name_uid, ref value); }
		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, ref Vector4 value) { return SetAttributeImpl(state, name_uid, ref value); }
		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, ref Vector3 value) { return SetAttributeImpl(state, name_uid, ref value); }
		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, ref Vector2 value) { return SetAttributeImpl(state, name_uid, ref value); }
		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, float value) { return SetAttributeImpl(state, name_uid, value); }
		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, Matrix[] value) { return SetAttributeImpl(state, name_uid, value); }
		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, Vector4[] value) { return SetAttributeImpl(state, name_uid, value); }
		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, Vector3[] value) { return SetAttributeImpl(state, name_uid, value); }
		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, Vector2[] value) { return SetAttributeImpl(state, name_uid, value); }
		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, float[] value) { return SetAttributeImpl(state, name_uid, value); }
		bool IShader.SetTexture(ShaderSystemBase state, int name_uid, Texture texture) { return SetTextureImpl(state, name_uid, texture); }
		bool IShader.SetTexture(ShaderSystemBase state, int name_uid, Texture2D texture) { return SetTextureImpl(state, name_uid, texture); }
		bool IShader.SetTexture(ShaderSystemBase state, int name_uid, Texture3D texture) { return SetTextureImpl(state, name_uid, texture); }
		bool IShader.SetTexture(ShaderSystemBase state, int name_uid, TextureCube texture) { return SetTextureImpl(state, name_uid, texture); }
		bool IShader.SetSamplerState(ShaderSystemBase state, int name_uid, TextureSamplerState sampler) { return SetSamplerStateImpl(state, name_uid, sampler); }

		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		protected virtual bool SetAttributeImpl(ShaderSystemBase state, int name_uid, bool value) { return false; }
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		protected virtual bool SetAttributeImpl(ShaderSystemBase state, int name_uid, ref Matrix value) { return false; }
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		protected virtual bool SetAttributeImpl(ShaderSystemBase state, int name_uid, ref Vector4 value) { return false; }
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		protected virtual bool SetAttributeImpl(ShaderSystemBase state, int name_uid, ref Vector3 value) { return false; }
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		protected virtual bool SetAttributeImpl(ShaderSystemBase state, int name_uid, ref Vector2 value) { return false; }
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		protected virtual bool SetAttributeImpl(ShaderSystemBase state, int name_uid, float value) { return false; }
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		protected virtual bool SetAttributeImpl(ShaderSystemBase state, int name_uid, Matrix[] value) { return false; }
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		protected virtual bool SetAttributeImpl(ShaderSystemBase state, int name_uid, Vector4[] value) { return false; }
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		protected virtual bool SetAttributeImpl(ShaderSystemBase state, int name_uid, Vector3[] value) { return false; }
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		protected virtual bool SetAttributeImpl(ShaderSystemBase state, int name_uid, Vector2[] value) { return false; }
		/// <summary>
		/// Set a shader attribute
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the attribute name</param>
		/// <param name="value">value to set</param>
		/// <returns>true if the value was set</returns>
		protected virtual bool SetAttributeImpl(ShaderSystemBase state, int name_uid, float[] value) { return false; }
		/// <summary>
		/// Set a shader texture
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the texture name</param>
		/// <param name="texture">texture to set</param>
		/// <returns>true if the texture was set</returns>
		protected virtual bool SetTextureImpl(ShaderSystemBase state, int name_uid, Texture texture) { return false; }
		/// <summary>
		/// Set a shader texture
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the texture name</param>
		/// <param name="texture">texture to set</param>
		/// <returns>true if the texture was set</returns>
		protected virtual bool SetTextureImpl(ShaderSystemBase state, int name_uid, Texture2D texture) { return false; }
		/// <summary>
		/// Set a shader texture
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the texture name</param>
		/// <param name="texture">texture to set</param>
		/// <returns>true if the texture was set</returns>
		protected virtual bool SetTextureImpl(ShaderSystemBase state, int name_uid, Texture3D texture) { return false; }
		/// <summary>
		/// Set a shader texture
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the texture name</param>
		/// <param name="texture">texture to set</param>
		/// <returns>true if the texture was set</returns>
		protected virtual bool SetTextureImpl(ShaderSystemBase state, int name_uid, TextureCube texture) { return false; }
		/// <summary>
		/// Set a shader texture sampler state
		/// </summary>
		/// <param name="state"></param>
		/// <param name="name_uid">unique id of the sampler state name</param>
		/// <param name="sampler">sampler state to set</param>
		/// <returns>true if the sampler state was set</returns>
		protected virtual bool SetSamplerStateImpl(ShaderSystemBase state, int name_uid, TextureSamplerState sampler) { return false; }

		/// <summary>
		/// Get the number of vertex inputs required by the shader
		/// </summary>
		/// <returns>Returns the number of vertex inputs required by the shader</returns>
		internal protected abstract int GetVertexInputCountImpl();
		/// <summary>
		/// Gets a vertex input required by the shader, by index. <see cref="IShader.GetVertexInputCount"/> to get the number of inputs required.
		/// </summary>
		/// <param name="index">Index of the element</param>
		/// <param name="elementUsage">Gets the usage type of the vertex element (eg, <see cref="VertexElementUsage.Position"/>)</param>
		/// <param name="elementIndex">Gets the index of the vertex element (eg, there may be more than one <see cref="VertexElementUsage.TextureCoordinate"/>)</param>
		internal protected abstract void GetVertexInputImpl(int index, out VertexElementUsage elementUsage, out int elementIndex);


		/// <summary>
		/// Get the number of vertex inputs required by the shader
		/// </summary>
		/// <returns>Returns the number of vertex inputs required by the shader</returns>
		int IShader.GetVertexInputCount() { return GetVertexInputCountImpl(); }
		/// <summary>
		/// Gets a vertex input required by the shader, by index. <see cref="IShader.GetVertexInputCount"/> to get the number of inputs required.
		/// </summary>
		/// <param name="index">Index of the element</param>
		/// <param name="elementUsage">Gets the usage type of the vertex element (eg, <see cref="VertexElementUsage.Position"/>)</param>
		/// <param name="elementIndex">Gets the index of the vertex element (eg, there may be more than one <see cref="VertexElementUsage.TextureCoordinate"/>)</param>
		void IShader.GetVertexInput(int index, out VertexElementUsage elementUsage, out int elementIndex)
		{
			GetVertexInputImpl(index, out elementUsage, out elementIndex);
		}

		/// <summary>
		/// Preload shader resources
		/// </summary>
		/// <param name="state"></param>
		public void Warm(ShaderSystemBase state)
		{
			WarmShader(state);
		}
		/// <summary>
		/// Warm shader implementation
		/// </summary>
		/// <param name="state"></param>
		protected abstract void WarmShader(ShaderSystemBase state);
	}
	
	/// <summary>
	/// Storage for XNA Effect mapped shader
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough]
	public struct ShaderEffect
	{
		/// <summary>XNA Effect</summary>
		public Effect Effect;
		/// <summary>XNA Effect Passes</summary>
		public EffectPass Pass, BlendPass, InstancePass;
		/// <summary>XNA Effect Parameter for shader constants or booleans</summary>
		public EffectParameter vs_c, ps_c, vs_b, ps_b, vsb_c, vsi_c;

		/// <summary>
		/// Dispose of the Effect
		/// </summary>
		public void Dispose()
		{
			if (Effect != null)
				Effect.Dispose();
			Effect = null;
			vs_c = null;
			ps_c = null;
			vs_b = null;
			ps_b = null;
			vsb_c = null;
			vsi_c = null;
			Pass = null;
			BlendPass = null;
			InstancePass = null;
		}
	}

	/// <summary>
	/// Extension mode the shader is using
	/// </summary>
	public enum ShaderExtension : byte
	{
		/// <summary>
		/// Shader runs with no extension
		/// </summary>
		None,
		/// <summary>
		/// Shader runs as a animation blending shader
		/// </summary>
		Blending,
		/// <summary>
		/// Shader runs as an instancing shader
		/// </summary>
		Instancing
	}

	/// <summary>
	/// Interface for setting shader constants with common semantic such as 'WORLDVIEWPROJECTION' and the special 'GLOBAL' semantic. All methods are called back by the shader
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough]
	public abstract class ShaderSystemBase
	{
		/// <summary>System-wide unique non-zero index for the current application graphics device (if this index changes, the shader will recreate itself automatically)</summary>
		public int DeviceUniqueIndex;

		/// <summary>
		/// Returns true if the specified shader is current bound
		/// </summary>
		public abstract bool IsShaderBound(IShader shader);

		/// <summary>
		/// Create effects
		/// </summary>
		public abstract void CreateEffect(out ShaderEffect effect, byte[] compressedEffectBytes, int vsInstructionCount, int psInstructionCount);

		/// <summary>
		/// Set the effect. If the current effect is already set, the it's values are dirty and requires CommitChanges is called.
		/// </summary>
		public abstract void SetEffect(IShader owner, ref ShaderEffect effect, ShaderExtension extension);
		
		//the following methods are auto-bound by the plugin,
		//ie : WORLDVIEWPROJECTION on a matrix will translate to SET+WORLDVIEWPROJECTION+MATRIX
		//which will auto reference the method below.
		//adding more methods in this format will automatically get used
		//make sure the type is matched correctly, otherwise the tool will fail
		//note: only standard types in Microsoft.Xna are supported (no custom structures)

		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldViewProjectionMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldViewProjectionInverseMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldViewProjectionTransposeMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);

		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldProjectionMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldProjectionInverseMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldProjectionTransposeMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);

		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetViewProjectionMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetViewProjectionInverseMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetViewProjectionTransposeMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);

		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldViewMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldViewInverseMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldViewTransposeMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);

		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldInverseMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWorldTransposeMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetViewMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetViewInverseMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetViewTransposeMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetProjectionMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetProjectionInverseMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetProjectionTransposeMatrix(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, ref int changeIndex);

		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetCameraNearFarVector2(ref Vector4 value, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetCameraFovVector2(ref Vector4 value, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetCameraFovTangentVector2(ref Vector4 value, ref int changeIndex);

		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetViewDirectionVector3(ref Vector4 value, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetViewDirectionVector4(ref Vector4 value, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetViewPointVector3(ref Vector4 value, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetViewPointVector4(ref Vector4 value, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetWindowSizeVector2(ref Vector4 value, ref int changeIndex);

		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetVertexCountSingle(ref Vector4 value, ref int changeIndex);

		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetBlendMatricesVector4Array(Vector4[] array, int index, int count, ref int changeIndex);

		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetBlendMatricesDirect(EffectParameter param, ref int changeIndex);

		/// <summary>
		/// Get the unique id for the name of a global type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public abstract int GetGlobalUniqueID<T>(string name);
		/// <summary>
		/// Get the unique id for the name of an attibute, texture or sampler
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public abstract int GetNameUniqueID(string name);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalBool(bool[] array, int index, int gloabl_uid);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalVector4(ref Vector4 value, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalVector3(ref Vector4 value, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalVector2(ref Vector4 value, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalSingle(ref Vector4 value, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalMatrix1(ref Vector4 X, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalMatrix2(ref Vector4 X, ref Vector4 Y, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalMatrix3(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalMatrix4(ref Vector4 X, ref Vector4 Y, ref Vector4 Z, ref Vector4 W, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalVector4(Vector4[] array, int start, int end, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalVector3(Vector4[] array, int start, int end, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalVector2(Vector4[] array, int start, int end, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalSingle(Vector4[] array, int start, int end, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalMatrix4(Vector4[] array, int start, int end, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalMatrix3(Vector4[] array, int start, int end, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalMatrix2(Vector4[] array, int start, int end, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract bool SetGlobalMatrix1(Vector4[] array, int start, int end, int gloabl_uid, ref int changeIndex);
		/// <summary>Method used by a generated shader</summary>
		public abstract Texture GetGlobalTexture(int gloabl_uid);
		/// <summary>Method used by a generated shader</summary>
		public abstract Texture2D GetGlobalTexture2D(int gloabl_uid);
		/// <summary>Method used by a generated shader</summary>
		public abstract Texture3D GetGlobalTexture3D(int gloabl_uid);
		/// <summary>Method used by a generated shader</summary>
		public abstract TextureCube GetGlobalTextureCube(int gloabl_uid);
		/// <summary>Method used by a generated shader</summary>
		public abstract TextureSamplerState GetGlobalTextureSamplerState(int gloabl_uid);

		/// <summary>Method used by a generated shader</summary>
		public abstract void SetPixelShaderSamplers(Texture[] textures, TextureSamplerState[] samplers);
		/// <summary>Method used by a generated shader</summary>
		public abstract void SetVertexShaderSamplers(Texture[] textures, TextureSamplerState[] samplers);

		/// <summary>Method used by a generated shader</summary>
		public abstract void SetPixelShaderSampler(int index, Texture texture, TextureSamplerState sampler);
		/// <summary>Method used by a generated shader</summary>
		public abstract void SetVertexShaderSampler(int index, Texture texture, TextureSamplerState sampler);


		//compiled shader code typically has a lot of zeros, or runs of repeating numbers:

		/// <summary>
		/// Decompress a byte array with very simple run-length encoding based compression
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] SimpleDecompress(byte[] data)
		{
			List<byte> output = new List<byte>(data.Length);

			int i = 0;
			while (i < data.Length)
			{
				if (data[i] > 127)
				{
					for (int n = 128; n < data[i]; n++)
						output.Add(data[i + 1]);
					i += 2;
				}
				else
				{
					int lenght = data[i++];
					for (int n = 0; n < lenght; n++)
						output.Add(data[i++]);
				}
			}
			return output.ToArray();
		}

		/// <summary>
		/// Compress a byte array with very simple run-length encoding based compression
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] SimpleCompress(byte[] data)
		{
			List<byte> output = new List<byte>(data.Length);
			output.Add(0);
			int counter = 0;

			for (int i = 0; i < data.Length; )
			{
				int n = i + 1;
				while (n < data.Length)
				{
					if (data[i] != data[n] || n - i > 100)
						break;
					n++;
				}
				if (n == data.Length)
					n--;

				if (n - i > 2)
				{
					output.Add((byte)(n - i + 128));
					output.Add(data[i]);
					while (i < n)
					{
						i++;
					}
					counter = output.Count;
					output.Add(0);
				}
				else
				{
					if (i - counter > 100)
					{
						counter = output.Count;
						output.Add(0);
					}

					output[counter]++;
					output.Add(data[i++]);
				}
			}
			return output.ToArray();
		}
	}
}

namespace Xen.Graphics
{
	/// <summary>
	/// Packed representation of common Texture Sampler states. 4 bytes
	/// </summary>
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = 4)]
	[System.Diagnostics.DebuggerStepThrough]
	public struct TextureSamplerState
	{
		[System.Runtime.InteropServices.FieldOffset(0)]
		internal int mode;

		internal void ResetState(SamplerState sampler, ref TextureSamplerState current)
		{
			sampler.AddressU = AddressU;
			sampler.AddressV = AddressV;
			sampler.AddressW = AddressW;
			sampler.Filter = Filter;
			sampler.MaxAnisotropy = MaxAnisotropy;
			sampler.MaxMipLevel = MaxMipmapLevel;

			current = this;
		}


		internal TextureSamplerState(TextureAddressMode uv, TextureFilter filter, int maxAni)
		{
			mode = 0;
			this.AddressUV = uv;
			this.Filter = filter;
			this.MaxAnisotropy = maxAni;
		}
		internal TextureSamplerState(int mode)
		{
			this.mode = mode;
		}

		/// <summary></summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(TextureSamplerState a, TextureSamplerState b)
		{
			return a.mode == b.mode;
		}
		/// <summary></summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(TextureSamplerState a, TextureSamplerState b)
		{
			return a.mode != b.mode;
		}
		/// <summary></summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is TextureSamplerState)
				return ((TextureSamplerState)obj).mode == this.mode;
			return base.Equals(obj);
		}
		/// <summary>
		/// Gets the hash code for this sampler state. Returns the internal bitfield value
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return mode;
		}

		/// <summary>
		/// Cast this sampler to it's internal bitfield representation
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public static implicit operator int(TextureSamplerState state)
		{
			return state.mode;
		}
		/// <summary>
		/// Explicit case from an integer bitfield representation
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public static explicit operator TextureSamplerState(int state)
		{
			return new TextureSamplerState(state);
		}

		private static TextureSamplerState point = new TextureSamplerState(TextureAddressMode.Wrap, TextureFilter.Point, 0);
		private static TextureSamplerState bilinear = new TextureSamplerState(TextureAddressMode.Wrap, TextureFilter.LinearMipPoint, 0);
		private static TextureSamplerState trilinear = new TextureSamplerState(TextureAddressMode.Wrap, TextureFilter.Linear, 0);
		private static TextureSamplerState aniLow = new TextureSamplerState(TextureAddressMode.Wrap, TextureFilter.Anisotropic, 2);
		private static TextureSamplerState aniMed = new TextureSamplerState(TextureAddressMode.Wrap, TextureFilter.Anisotropic, 4);
		private static TextureSamplerState aniHigh = new TextureSamplerState(TextureAddressMode.Wrap, TextureFilter.Anisotropic, 8);

		/// <summary>
		/// Gets a texture sampler that applies simple pointer filtering
		/// </summary>
		public static TextureSamplerState PointFiltering { get { return point; } }
		/// <summary>
		/// Gets a texture sampler that applies bilinear filtering (linear UV filtering with point mipmap filtering)
		/// </summary>
		public static TextureSamplerState BilinearFiltering { get { return bilinear; } }
		/// <summary>
		/// Gets a texture sampler that applies trilinear filtering (linear UV filtering mipmap filtering)
		/// </summary>
		public static TextureSamplerState TrilinearFiltering { get { return trilinear; } }
		/// <summary>
		/// Gets a texture sampler that applies anisotropic filtering with a low max anisotropic value
		/// </summary>
		public static TextureSamplerState AnisotropicLowFiltering { get { return aniLow; } }
		/// <summary>
		/// Gets a texture sampler that applies anisotropic filtering with a medium max anisotropic value
		/// </summary>
		public static TextureSamplerState AnisotropicMediumFiltering { get { return aniMed; } }
		/// <summary>
		/// Gets a texture sampler that applies anisotropic filtering with a high max anisotropic value
		/// </summary>
		public static TextureSamplerState AnisotropicHighFiltering { get { return aniHigh; } }


		/// <summary>
		/// Allows setting of both the <see cref="AddressU"/> and <see cref="AddressV"/> coordinate address modes at the same time
		/// </summary>
		/// <remarks>The safest values to use are <see cref="TextureAddressMode.Wrap"/> for repeating textures, and <see cref="TextureAddressMode.Clamp"/>.</remarks>
		public TextureAddressMode AddressUV
		{
			set
			{
				{
					const int offset = 0; mode = ((mode & ~(3 << offset)) | (3 & ((int)value)) << offset);
				}
				{
					const int offset = 2; mode = ((mode & ~(3 << offset)) | (3 & ((int)value)) << offset);
				}
			}
		}

		/// <summary>
		/// Controls texture address behaviour for the U coordinate (The U coordinate is the x-axis in texture coordinate space)
		/// </summary>
		/// <remarks>The safest values to use are <see cref="TextureAddressMode.Wrap"/> for repeating textures, and <see cref="TextureAddressMode.Clamp"/>.</remarks>
		public TextureAddressMode AddressU
		{
			get { const int offset = 0; return (TextureAddressMode)(((mode >> offset) & 3)); }
			set { const int offset = 0; mode = ((mode & ~(3 << offset)) | (3 & ((int)value)) << offset); }
		}
		/// <summary>
		/// Controls texture address behaviour for the V coordinate (The V coordinate is the y-axis in texture coordinate space)
		/// </summary>
		/// <remarks>The safest values to use are <see cref="TextureAddressMode.Wrap"/> for repeating textures, and <see cref="TextureAddressMode.Clamp"/>.</remarks>
		public TextureAddressMode AddressV
		{
			get { const int offset = 2; return (TextureAddressMode)(((mode >> offset) & 3)); }
			set { const int offset = 2; mode = ((mode & ~(3 << offset)) | (3 & ((int)value)) << offset); }
		}
		/// <summary>
		/// Controls texture address behaviour for the W coordinate (The W coordinate is the z-axis in texture coordinate space). This filtering mode only applies to 3D textures.
		/// </summary>
		/// <remarks>The safest values to use are <see cref="TextureAddressMode.Wrap"/> for repeating textures, and <see cref="TextureAddressMode.Clamp"/>.</remarks>
		public TextureAddressMode AddressW
		{
			get { const int offset = 4; return (TextureAddressMode)(((mode >> offset) & 3)); }
			set { const int offset = 4; mode = ((mode & ~(3 << offset)) | (3 & ((int)value)) << offset); }
		}
		/// <summary>
		/// Controls texture filtering, such as mipmap interpolation and smoothing algorithm
		/// </summary>
		public TextureFilter Filter
		{
			get { const int offset = 6; return (TextureFilter)(((mode >> offset) & 15)); }
			set { const int offset = 6; mode = ((mode & ~(15 << offset)) | (15 & ((int)value)) << offset); }
		}
		/// <summary>
		/// Set the maximum number of samples used when <see cref="Filter"/> is set to <see cref="TextureFilter.Anisotropic"/> filtering. Range of [1-16], usually limited to values that are a power of two.
		/// </summary>
		public int MaxAnisotropy
		{
			get { const int offset = 10; return (((mode >> offset) & 15) + 1); }
			set { const int offset = 10; mode = ((mode & ~(15 << offset)) | (15 & (Math.Max(0, Math.Min(16, value) - 1))) << offset); }
		}
		/// <summary>
		/// Set the maximum mipmap level the video card will sample, where 0 is the largest map (and the default value). Set to 1 to prevent the highest mipmap level being sampled (this will effectivly half the resolution of the texture displayed).
		/// </summary>
		public int MaxMipmapLevel
		{
			get { const int offset = 14; return ((((mode >> offset)) & 255)); }
			set { const int offset = 14; mode = ((mode & ~(255 << offset)) | (255 & ((Math.Min(255, value)))) << offset); }
		}
	}
}
