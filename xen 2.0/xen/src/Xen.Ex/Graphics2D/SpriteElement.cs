using System;
using System.Collections.Generic;
using System.Text;
using Xen.Graphics;
using Xen.Graphics.Modifier;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Xen.Ex.Graphics2D
{
	/// <summary>
	/// <para>An element that displays many smaller 2D sprites</para>
	/// <para>Sprite instances can be added with <see cref="AddSprite(ref Vector2,ref Vector2)"/>, modified with <see cref="SetSprite(int,ref Vector2,ref Vector2)"/></para>
	/// </summary>
	public sealed class SpriteElement : Element
	{
		#region members

		private static readonly Matrix defaultValue = new Matrix(0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0.5f, 0.5f, 0);

		private static readonly int NonInstancingRenderCount = 60;
		//how many sprites are needed before hardware instancing kicks in instead of shader instancing
		private static readonly int HardwareInstancingMinimum = NonInstancingRenderCount*4;

		struct InstanceVertex
		{
			public InstanceVertex(Vector3 v, float t)
			{
				this.pos = v;
				this.tex = t;
			}
			public Vector3 pos;
			public float tex;
		}

		private Matrix[] instances = new Matrix[32];
		private Stack<int> clearedIndices = new Stack<int>();
		private int instanceCount;

		//vertex and index buffer are shared through the app using UserValues global
		private IVertices vertices;
		private Indices<ushort> indices;
		//shader instancing
		private IVertices verticesSI;
		private Indices<ushort> indicesSI;

		private Texture2D texture;
		private Vector2 texInvPixelSize;

		#endregion

		/// <summary>
		/// <para>Gets the texture used to display the sprite</para>
		/// <para>Use <see cref="SetTextureAndClear"/> to change the texture</para>
		/// </summary>
		public Texture2D Texture
		{
			get { return texture;}
		}

		/// <summary>
		/// Sets the position and size of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the sprite</param>
		/// <param name="position"></param>
		/// <param name="size"></param>
		public void SetSprite(int index, ref Vector2 position, ref Vector2 size)
		{
			if (index >= instanceCount)
				throw new IndexOutOfRangeException();

			Matrix value = instances[index];
			value.M11 = position.X; value.M12 = position.Y; value.M13 = size.X; value.M14 = size.Y;
			instances[index] = value;
		}
		/// <summary>
		/// Sets the position, size and crop rect of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the sprite</param>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="textureCropRect"></param>
		public void SetSprite(int index, ref Vector2 position, ref Vector2 size, ref Vector4 textureCropRect)
		{
			if (index >= instanceCount)
				throw new IndexOutOfRangeException();

			Matrix value = defaultValue;
			value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
			value.M21 = texInvPixelSize.X * (0.5f + textureCropRect.X); value.M22 = texInvPixelSize.Y * (0.5f + textureCropRect.Y); value.M23 = texInvPixelSize.X * (textureCropRect.Z); value.M24 = texInvPixelSize.Y * (textureCropRect.W);
			instances[index] = value;
		}
		/// <summary>
		/// Sets the position, size and crop rect of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the sprite</param>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="textureCropSize"></param>
		/// <param name="textureCropTopLeft"></param>
		public void SetSprite(int index, ref Vector2 position, ref Vector2 size, ref Vector2 textureCropTopLeft, ref Vector2 textureCropSize)
		{
			if (index >= instanceCount)
				throw new IndexOutOfRangeException();

			float tx0 = textureCropTopLeft.X;
			float ty0 = textureCropTopLeft.Y;
			float tx1 = (textureCropTopLeft.X + textureCropSize.X);
			float ty1 = (textureCropTopLeft.Y + textureCropSize.Y);

			Matrix value = defaultValue;
			value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
			value.M21 = texInvPixelSize.X * (0.5f) + tx0; value.M22 = texInvPixelSize.Y * (0.5f) + ty0; value.M23 = texInvPixelSize.X * (0.5f) + tx1; value.M24 = texInvPixelSize.Y * (0.5f) + ty1;
			instances[index] = value;
		}
		/// <summary>
		/// Sets the position, size and crop rect of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the sprite</param>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="textureCrop"></param>
		public void SetSprite(int index,ref Vector2 position, ref Vector2 size, ref Rectangle textureCrop)
		{
			if (index >= instanceCount)
				throw new IndexOutOfRangeException();

			float tx0 = (float)textureCrop.X;
			float ty0 = (float)textureCrop.Y;
			float tx1 = (float)textureCrop.Width;
			float ty1 = (float)textureCrop.Height;

			Matrix value = defaultValue;
			value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
			value.M21 = texInvPixelSize.X * (0.5f + tx0); value.M22 = texInvPixelSize.Y * (0.5f + ty0); value.M23 = texInvPixelSize.X * (tx1); value.M24 = texInvPixelSize.Y * (ty1);
			instances[index] = value;
		}
		/// <summary>
		/// Sets the position and of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the sprite</param>
		/// <param name="position"></param>
		/// <param name="size"></param>
		public void SetSprite(int index, Vector2 position, Vector2 size)
		{
			SetSprite(index, ref position, ref size);
		}
		/// <summary>
		/// Sets the position, size and crop rect of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the sprite</param>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="textureCropRect"></param>
		public void SetSprite(int index, Vector2 position, Vector2 size, Vector4 textureCropRect)
		{
			SetSprite(index, ref position, ref size, ref textureCropRect);
		}
		/// <summary>
		/// Sets the position, size and crop rect of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the sprite</param>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="textureCropSize"></param>
		/// <param name="textureCropTopLeft"></param>
		public void SetSprite(int index, Vector2 position, Vector2 size, Vector2 textureCropTopLeft, Vector2 textureCropSize)
		{
			SetSprite(index, ref position, ref size, ref textureCropTopLeft, ref textureCropSize);
		}
		/// <summary>
		/// Sets the position, size and crop rect of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the sprite</param>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="textureCrop"></param>
		public void SetSprite(int index, Vector2 position, Vector2 size, Rectangle textureCrop)
		{
			SetSprite(index, ref position, ref size, ref textureCrop);
		}

		/// <summary>
		/// <para>Add a sprite to this element.</para>
		/// <para>Returns the index of the added sprite (which can be used to change the sprite later)</para>
		/// </summary>
		/// <param name="position">position of the sprite</param>
		/// <param name="size">size of the sprite</param>
		/// <param name="colour">colour of the sprite</param>
		/// <param name="textureCropTopLeft"></param>
		/// <param name="textureCropSize"></param>
		/// <returns>Index of the added sprite (Index can be used to change the sprite later)</returns>
		public int AddSprite(ref Vector2 position, ref Vector2 size, ref Vector4 colour, ref Vector2 textureCropTopLeft, ref Vector2 textureCropSize)
		{
			float tx0 = textureCropTopLeft.X;
			float ty0 = textureCropTopLeft.Y;
			float tx1 = (textureCropTopLeft.X + textureCropSize.X);
			float ty1 = (textureCropTopLeft.Y + textureCropSize.Y);

			if (clearedIndices.Count > 0)
			{
				int i = clearedIndices.Pop();

				Matrix value = defaultValue;
				value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
				value.M21 = texInvPixelSize.X * (0.5f) + tx0; value.M22 = texInvPixelSize.Y * (0.5f) + ty0; value.M23 = texInvPixelSize.X * (0.5f) + tx1; value.M24 = texInvPixelSize.Y * (0.5f) + ty1;
				value.M31 = colour.X; value.M32 = colour.Y; value.M33 = colour.Z; value.M34 = colour.W;
				instances[i] = value;

				return i;
			}
			else
			{
				if (this.instanceCount == this.instances.Length)
					Array.Resize(ref this.instances, this.instances.Length * 2);

				int i = instanceCount;
				Matrix value = defaultValue;
				value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
				value.M21 = texInvPixelSize.X * (0.5f) + tx0; value.M22 =  texInvPixelSize.Y * (0.5f) + ty0; value.M23 =  texInvPixelSize.X * (0.5f) + tx1; value.M24 =  texInvPixelSize.Y * (0.5f) + ty1;
				value.M31 = colour.X; value.M32 = colour.Y; value.M33 = colour.Z; value.M34 = colour.W;
				instances[i] = value;

				return instanceCount++;
			}
		}

		/// <summary>
		/// <para>Add a sprite to this element.</para>
		/// <para>Returns the index of the added sprite (which can be used to change the sprite later)</para>
		/// </summary>
		/// <param name="position">position of the sprite</param>
		/// <param name="size">size of the sprite</param>
		/// <param name="colour">colour of the sprite</param>
		/// <param name="textureCropTopLeft"></param>
		/// <param name="textureCropSize"></param>
		/// <returns>Index of the added sprite (Index can be used to change the sprite later)</returns>
		public int AddSprite(Vector2 position, Vector2 size, Vector4 colour, Vector2 textureCropTopLeft, Vector2 textureCropSize)
		{
			return AddSprite(ref position, ref size, ref colour, ref textureCropTopLeft, ref textureCropSize);
		}


		/// <summary>
		/// <para>Add a sprite to this element.</para>
		/// <para>Returns the index of the added sprite (which can be used to change the sprite later)</para>
		/// </summary>
		/// <param name="position">position of the sprite</param>
		/// <param name="size">size of the sprite</param>
		/// <param name="colour">colour of the sprite</param>
		/// <param name="textureCropRect">A rectangle as Vector4, in the form (X,Y,Width,Height)</param>
		/// <returns>Index of the added sprite (Index can be used to change the sprite later)</returns>
		public int AddSprite(ref Vector2 position, ref Vector2 size, ref Vector4 colour, ref Vector4 textureCropRect)
		{
			float tx0 = textureCropRect.X;
			float ty0 = textureCropRect.Y;
			float tx1 = textureCropRect.Z;
			float ty1 = textureCropRect.W;

			if (clearedIndices.Count > 0)
			{
				int i = clearedIndices.Pop();

				Matrix value = defaultValue;
				value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
				value.M21 = texInvPixelSize.X * (0.5f + tx0); value.M22 =  texInvPixelSize.Y * (0.5f + ty0); value.M23 =  texInvPixelSize.X * (tx1); value.M24 =  texInvPixelSize.Y * (ty1);
				value.M31 = colour.X; value.M32 = colour.Y; value.M33 = colour.Z; value.M34 = colour.W;
				instances[i] = value;

				return i;
			}
			else
			{
				if (this.instanceCount == this.instances.Length)
					Array.Resize(ref this.instances, this.instances.Length * 2);

				int i = instanceCount;
				Matrix value = defaultValue;
				value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
				value.M21 = texInvPixelSize.X * (0.5f + tx0); value.M22 =  texInvPixelSize.Y * (0.5f + ty0); value.M23 =  texInvPixelSize.X * (tx1); value.M24 =  texInvPixelSize.Y * (ty1);
				value.M31 = colour.X; value.M32 = colour.Y; value.M33 = colour.Z; value.M34 = colour.W;
				instances[i] = value;

				return instanceCount++;
			}
		}
		/// <summary>
		/// <para>Add a sprite to this element.</para>
		/// <para>Returns the index of the added sprite (which can be used to change the sprite later)</para>
		/// </summary>
		/// <param name="position">position of the sprite</param>
		/// <param name="size">size of the sprite</param>
		/// <param name="colour">colour of the sprite</param>
		/// <param name="textureCropRect">A rectangle as Vector4, in the form (X,Y,Width,Height)</param>
		/// <returns>Index of the added sprite (Index can be used to change the sprite later)</returns>
		public int AddSprite(Vector2 position, Vector2 size, Vector4 colour, Vector4 textureCropRect)
		{
			return AddSprite(ref position, ref size, ref colour, ref textureCropRect);
		}

		/// <summary>
		/// <para>Add a sprite to this element.</para>
		/// <para>Returns the index of the added sprite (which can be used to change the sprite later)</para>
		/// </summary>
		/// <param name="position">position of the sprite</param>
		/// <param name="size">size of the sprite</param>
		/// <param name="colour">colour of the sprite</param>
		/// <param name="textureCrop"></param>
		/// <returns>Index of the added sprite (Index can be used to change the sprite later)</returns>
		public int AddSprite(ref Vector2 position, ref Vector2 size, ref Vector4 colour, ref Rectangle textureCrop)
		{
			float tx0 = (float)textureCrop.X;
			float ty0 = (float)textureCrop.Y;
			float tx1 = (float)textureCrop.Width;
			float ty1 = (float)textureCrop.Height;

			if (clearedIndices.Count > 0)
			{
				int i = clearedIndices.Pop();

				Matrix value = defaultValue;
				value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
				value.M21 = texInvPixelSize.X * (0.5f + tx0); value.M22 =  texInvPixelSize.Y * (0.5f + ty0); value.M23 =  texInvPixelSize.X * (tx1); value.M24 =  texInvPixelSize.Y * (ty1);
				value.M31 = colour.X; value.M32 = colour.Y; value.M33 = colour.Z; value.M34 = colour.W;
				instances[i] = value;

				return i;
			}
			else
			{
				if (this.instanceCount == this.instances.Length)
					Array.Resize(ref this.instances, this.instances.Length * 2);

				int i = instanceCount;
				Matrix value = defaultValue;
				value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
				value.M21 = texInvPixelSize.X * (0.5f + tx0); value.M22 =  texInvPixelSize.Y * (0.5f + ty0); value.M23 =  texInvPixelSize.X * (tx1); value.M24 =  texInvPixelSize.Y * (ty1);
				value.M31 = colour.X; value.M32 = colour.Y; value.M33 = colour.Z; value.M34 = colour.W;
				instances[i] = value;

				return instanceCount++;
			}
		}

		/// <summary>
		/// <para>Add a sprite to this element.</para>
		/// <para>Returns the index of the added sprite (which can be used to change the sprite later)</para>
		/// </summary>
		/// <param name="position">position of the sprite</param>
		/// <param name="size">size of the sprite</param>
		/// <param name="colour">colour of the sprite</param>
		/// <param name="textureCrop"></param>
		/// <returns>Index of the added sprite (Index can be used to change the sprite later)</returns>
		public int AddSprite(Vector2 position, Vector2 size, Vector4 colour, Rectangle textureCrop)
		{
			return AddSprite(ref position, ref size, ref colour, ref textureCrop);
		}

		/// <summary>
		/// <para>Add a sprite to this element.</para>
		/// <para>Returns the index of the added sprite (which can be used to change the sprite later)</para>
		/// </summary>
		/// <param name="position">position of the sprite</param>
		/// <param name="size">size of the sprite</param>
		/// <returns>Index of the added sprite (Index can be used to change the sprite later)</returns>
		public int AddSprite(ref Vector2 position, ref Vector2 size)
		{
			if (clearedIndices.Count > 0)
			{
				int i = clearedIndices.Pop();

				Matrix value = defaultValue;
				value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
				value.M21 = texInvPixelSize.X * (0.5f); value.M22 =  texInvPixelSize.Y * (0.5f); value.M23 =  texInvPixelSize.X * (0.5f) + 1; value.M24 =  texInvPixelSize.Y * (0.5f) + 1;
				instances[i] = value;

				return i;
			}
			else
			{
				if (this.instanceCount == this.instances.Length)
					Array.Resize(ref this.instances, this.instances.Length * 2);

				int i = instanceCount;
				Matrix value = defaultValue;
				value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
				value.M21 = texInvPixelSize.X * (0.5f); value.M22 =  texInvPixelSize.Y * (0.5f); value.M23 =  texInvPixelSize.X * (0.5f) + 1; value.M24 =  texInvPixelSize.Y * (0.5f) + 1;
				instances[i] = value;

				return instanceCount++;
			}
		}

		/// <summary>
		/// <para>Add a sprite to this element.</para>
		/// <para>Returns the index of the added sprite (which can be used to change the sprite later)</para>
		/// </summary>
		/// <param name="position">position of the sprite</param>
		/// <param name="size">size of the sprite</param>
		/// <param name="colour">colour of the sprite</param>
		/// <returns>Index of the added sprite (Index can be used to change the sprite later)</returns>
		public int AddSprite(ref Vector2 position, ref Vector2 size, ref Vector4 colour)
		{
			if (clearedIndices.Count > 0)
			{
				int i = clearedIndices.Pop();

				Matrix value = defaultValue;
				value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
				value.M21 = texInvPixelSize.X * (0.5f); value.M22 =  texInvPixelSize.Y * (0.5f); value.M23 =  texInvPixelSize.X * (0.5f) + 1; value.M24 =  texInvPixelSize.Y * (0.5f) + 1;
				value.M31 = colour.X; value.M32 = colour.Y; value.M33 = colour.Z; value.M34 = colour.W;
				instances[i] = value;

				return i;
			}
			else
			{
				if (this.instanceCount == this.instances.Length)
					Array.Resize(ref this.instances, this.instances.Length * 2);

				int i = instanceCount;
				Matrix value = defaultValue;
				value.M11 = position.X; value.M12 =  position.Y; value.M13 =  size.X; value.M14 =  size.Y;
				value.M21 = texInvPixelSize.X * (0.5f); value.M22 =  texInvPixelSize.Y * (0.5f); value.M23 =  texInvPixelSize.X * (0.5f) + 1; value.M24 =  texInvPixelSize.Y * (0.5f) + 1;
				value.M31 = colour.X; value.M32 = colour.Y; value.M33 = colour.Z; value.M34 = colour.W;
				instances[i] = value;

				return instanceCount++;
			}
		}

		/// <summary>
		/// <para>Add a sprite to this element.</para>
		/// <para>Returns the index of the added sprite (which can be used to change the sprite later)</para>
		/// </summary>
		/// <param name="position">position of the sprite</param>
		/// <param name="size">size of the sprite</param>
		/// <returns>Index of the added sprite (Index can be used to change the sprite later)</returns>
		public int AddSprite(Vector2 position, Vector2 size)
		{
			return AddSprite(ref position, ref size);
		}

		/// <summary>
		/// <para>Add a sprite to this element.</para>
		/// <para>Returns the index of the added sprite (which can be used to change the sprite later)</para>
		/// </summary>
		/// <param name="position">position of the sprite</param>
		/// <param name="size">size of the sprite</param>
		/// <param name="colour">colour of the sprite</param>
		/// <returns>Index of the added sprite (Index can be used to change the sprite later)</returns>
		public int AddSprite(Vector2 position, Vector2 size, Vector4 colour)
		{
			return AddSprite(ref position, ref size, ref colour);
		}
		/// <summary>
		/// Set the position of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the added sprite</param>
		/// <param name="position"></param>
		public void SetSpritePosition(int index, ref Vector2 position)
		{
			if (index > instanceCount)
				throw new IndexOutOfRangeException();

			instances[index].M11 = position.X;
			instances[index].M12 = position.Y;
		}
		/// <summary>
		/// Set the position of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the added sprite</param>
		/// <param name="position"></param>
		public void SetSpritePosition(int index, Vector2 position)
		{
			if (index > instanceCount)
				throw new IndexOutOfRangeException();

			instances[index].M11 = position.X;
			instances[index].M12 = position.Y;
		}
		/// <summary>
		/// Copies in position data for all sprites, using an array
		/// </summary>
		public void SetSpritePositions(Vector2[] positions)
		{
			for (int n = 0; n < positions.Length && n < instances.Length; n++)
			{
				instances[n].M11 = positions[n].X;
				instances[n].M12 = positions[n].Y;
			}
		}
		/// <summary>
		/// Get the position of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the added sprite</param>
		/// <param name="position"></param>
		public void GetSpritePosition(int index, out Vector2 position)
		{
			if (index > instanceCount)
				throw new IndexOutOfRangeException();

			position = new Vector2(instances[index].M11, instances[index].M12);
		}
		/// <summary>
		/// Set the colour of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the added sprite</param>
		/// <param name="colour"></param>
		public void SetSpriteColour(int index, ref Vector4 colour)
		{
			if (index > instanceCount)
				throw new IndexOutOfRangeException();

			instances[index].M31 = colour.X;
			instances[index].M32 = colour.Y;
			instances[index].M33 = colour.Z;
			instances[index].M34 = colour.W;
		}
		/// <summary>
		/// Set the rotation of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the added sprite</param>
		/// <param name="rotation">Rotation in radians</param>
		/// <param name="rotationCentre"></param>
		public void SetSpriteRotation(int index, float rotation, ref Vector2 rotationCentre)
		{
			if (index > instanceCount)
				throw new IndexOutOfRangeException();

			instances[index].M41 = rotation;
			instances[index].M42 = rotationCentre.X;
			instances[index].M43 = rotationCentre.Y;
		}
		/// <summary>
		/// Set the rotation of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the added sprite</param>
		/// <param name="rotation">Rotation in radians</param>
		/// <param name="rotationCentre"></param>
		public void SetSpriteRotation(int index, float rotation, Vector2 rotationCentre)
		{
			if (index > instanceCount)
				throw new IndexOutOfRangeException();

			instances[index].M41 = rotation;
			instances[index].M42 = rotationCentre.X;
			instances[index].M43 = rotationCentre.Y;
		}
		/// <summary>
		/// Set the rotation of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the added sprite</param>
		/// <param name="rotation">Rotation in radians</param>
		public void SetSpriteRotation(int index, float rotation)
		{
			if (index > instanceCount)
				throw new IndexOutOfRangeException();

			instances[index].M41 = rotation;
			instances[index].M42 = 0.5f;
			instances[index].M43 = 0.5f;
		}
		/// <summary>
		/// Set the colour of a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the added sprite</param>
		/// <param name="colour"></param>
		public void SetSpriteColour(int index, Vector4 colour)
		{
			if (index > instanceCount)
				throw new IndexOutOfRangeException();

			instances[index].M31 = colour.X;
			instances[index].M32 = colour.Y;
			instances[index].M33 = colour.Z;
			instances[index].M34 = colour.W;
		}
		/// <summary>
		/// Move a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the added sprite</param>
		/// <param name="deltaPosition">position change</param>
		public void MoveSprite(int index, ref Vector2 deltaPosition)
		{
			if (index > instanceCount)
				throw new IndexOutOfRangeException();

			instances[index].M11 += deltaPosition.X;
			instances[index].M12 += deltaPosition.Y;
		}
		/// <summary>
		/// Move a sprite that has been added to this element
		/// </summary>
		/// <param name="index">index of the added sprite</param>
		/// <param name="deltaPosition">position change</param>
		public void MoveSprite(int index, Vector2 deltaPosition)
		{
			if (index > instanceCount)
				throw new IndexOutOfRangeException();

			instances[index].M11 += deltaPosition.X;
			instances[index].M12 += deltaPosition.Y;
		}

		/// <summary>
		/// <para>Change the texture used to display sprites</para>
		/// <para>This operation will also clear all added sprites</para>
		/// </summary>
		/// <param name="texture"></param>
		public void SetTextureAndClear(Texture2D texture)
		{
			if (texture != this.texture)
			{
				if (texture == null)
					texInvPixelSize = Vector2.Zero;
				else
					texInvPixelSize = new Vector2(1.0f / texture.Width, 1.0f / texture.Height);
				this.texture = texture;
			}

			this.instanceCount = 0;
			this.clearedIndices.Clear();
		}
		/// <summary>
		/// <para>Clear all sprites added to this element</para>
		/// </summary>
		public void ClearAllSprites()
		{
			this.instanceCount = 0;
			this.clearedIndices.Clear();
		}
		/// <summary>
		/// <para>Removes a single sprite by index</para>
		/// <para>This method prevents the sprite from drawing, but there is sill cpu setup overhead involved in drawing until <see cref="ClearAllSprites"/> is called</para>
		/// </summary>
		/// <param name="index"></param>
		public void RemoveSprite(int index)
		{
			if (index >= instanceCount)
				throw new IndexOutOfRangeException();

			if (instances[index].M13 != 0 ||
				instances[index].M14 != 0)
			{
				instances[index] = defaultValue;
				clearedIndices.Push(index);
			}
		}
		/// <summary>
		/// <para>Removes a range of sprites by index</para>
		/// <para>This method prevents the sprites from drawing, but there is sill cpu setup overhead involved in drawing until <see cref="ClearAllSprites"/> is called</para>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public void RemoveSpriteRange(int index, int count)
		{
			if (index + count >= instanceCount)
				throw new IndexOutOfRangeException();

			for (int i = index; i < index+count; i++)
			{
				if (instances[i].M13 != 0 ||
					instances[i].M14 != 0)
				{
					instances[i] = defaultValue;
					clearedIndices.Push(i);
				}
			}
		}
		/// <summary>
		/// Removes the last sprite from this element
		/// </summary>
		/// <param name="count"></param>
		public void RemoveLastSprite(int count)
		{
			if (count > instanceCount || count < 0)
				throw new IndexOutOfRangeException();
			instanceCount -= count;
		}

		/// <summary>
		/// Gets the number of instances that are drawn (this may be different from the sprite count)
		/// </summary>
		public int InstanceCount
		{
			get { return instanceCount; }
		}

		/// <summary>
		/// Construct the sprite element
		/// </summary>
		/// <param name="texture"></param>
		public SpriteElement(Texture2D texture)
		{
			this.texture = texture;

			if (texture != null)
				texInvPixelSize = new Vector2(1.0f / texture.Width, 1.0f / texture.Height);
		}

		/// <summary>
		/// Construct the sprite element
		/// </summary>
		public SpriteElement()
		{
		}

		/// <summary></summary>
		/// <param name="state"></param>
		protected sealed override void DrawElement(DrawState state)
		{
			if (state.Properties.SupportsHardwareInstancing && instanceCount > HardwareInstancingMinimum)
			{
				using (state.WorldMatrix.PushIdentity())
				{
					vertices.DrawInstances(state, indices, PrimitiveType.TriangleList, state.GetDynamicInstanceBuffer(this.instances, this.instanceCount));
				}
			}
			else
			{
				Graphics2D.NonInstancingSprite shader = state.GetShader<Graphics2D.NonInstancingSprite>();

				for (int i = 0; i < instanceCount; i += NonInstancingRenderCount)
				{
					int count = Math.Min(NonInstancingRenderCount, (instanceCount - i));

					shader.SetInstances(this.instances, (uint)i, 0, (uint)count);

					this.verticesSI.Draw(state, this.indicesSI, PrimitiveType.TriangleList, 2 * count, 0, 0);
				}
			}
		}

		/// <summary></summary>
		/// <param name="state"></param>
		/// <param name="maskOnly"></param>
		protected override sealed IShader BindShader(DrawState state, bool maskOnly)
		{
			if (this.vertices == null)
			{
				this.vertices = state.Application.UserValues[GetType().FullName + ".vertices"] as IVertices;
				this.indices = state.Application.UserValues[GetType().FullName + ".indices"] as Indices<ushort>;

				this.verticesSI = state.Application.UserValues[GetType().FullName + ".verticesSI"] as IVertices;
				this.indicesSI = state.Application.UserValues[GetType().FullName + ".indicesSI"] as Indices<ushort>;

				if (this.vertices == null)
				{
					//still null, create the global vertices
					this.vertices = new Vertices<Vector4>(
						new Vector4(0, 0, 0, 1),
						new Vector4(1, 0, 0, 1),
						new Vector4(1, 1, 0, 1),
						new Vector4(0, 1, 0, 1));

					this.indices = new Indices<ushort>(0, 2, 1, 0, 3, 2);

					//shader instancing..
					List<InstanceVertex> verts = new List<InstanceVertex>();
					List<ushort> inds = new List<ushort>();

					for (int i = 0; i < NonInstancingRenderCount; i++)
					{
						verts.Add(new InstanceVertex(new Vector3(0, 0, 0), (float)i));
						verts.Add(new InstanceVertex(new Vector3(1, 0, 0), (float)i));
						verts.Add(new InstanceVertex(new Vector3(1, 1, 0), (float)i));
						verts.Add(new InstanceVertex(new Vector3(0, 1, 0), (float)i));

						inds.Add((ushort)(0 + i * 4));
						inds.Add((ushort)(2 + i * 4));
						inds.Add((ushort)(1 + i * 4));
						inds.Add((ushort)(0 + i * 4));
						inds.Add((ushort)(3 + i * 4));
						inds.Add((ushort)(2 + i * 4));
					}

					this.verticesSI = new Vertices<InstanceVertex>(verts.ToArray());
					this.indicesSI = new Indices<ushort>(inds.ToArray());

					state.Application.UserValues[GetType().FullName + ".vertices"] = vertices;
					state.Application.UserValues[GetType().FullName + ".indices"] = indices;
					state.Application.UserValues[GetType().FullName + ".verticesSI"] = verticesSI;
					state.Application.UserValues[GetType().FullName + ".indicesSI"] = indicesSI;
				}
			}

			if (state.Properties.SupportsHardwareInstancing && instanceCount > HardwareInstancingMinimum)
			{
				Graphics2D.InstancingSprite shader = state.GetShader<Graphics2D.InstancingSprite>();
				Matrix world;
				state.WorldMatrix.GetMatrix(out world);
				shader.SetSpriteWorldMatrix(ref world);
				shader.CustomTexture = texture ?? state.Properties.WhiteTexture;
				return shader;
			}
			else
			{
				Graphics2D.NonInstancingSprite shader = state.GetShader<Graphics2D.NonInstancingSprite>();

				shader.CustomTexture = texture ?? state.Properties.WhiteTexture;
				return shader;
			}
		}

		/// <summary></summary>
		protected override sealed Vector2 ElementSize
		{
			get { return Vector2.Zero; }
		}
		/// <summary></summary>
		protected override sealed bool UseSize
		{
			get { return false; }
		}
	}
}
