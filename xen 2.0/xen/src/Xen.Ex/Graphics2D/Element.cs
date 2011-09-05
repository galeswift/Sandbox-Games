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
	/// Element alignment
	/// </summary>
	public enum HorizontalAlignment : byte
	{
		/// <summary></summary>
		Left,
		/// <summary></summary>
		Right,
		/// <summary></summary>
		Centre,
	}
	/// <summary>
	/// Element alignment
	/// </summary>
	public enum VerticalAlignment : byte
	{
		/// <summary></summary>
		Bottom,
		/// <summary></summary>
		Top,
		/// <summary></summary>
		Centre,
	}
	/// <summary>
	/// Element scaling
	/// </summary>
	public enum ElementScaling : byte
	{
		/// <summary></summary>
		Normal,
		/// <summary>The element is scaled such that it's displayed size is the size of it's parent plus it's current size</summary>
		FillToParentPlusSize,
	}

	/// <summary>
	/// Base abstract class for a 2D element. <see cref="ElementRect"/> extends the class with size and scaling information
	/// </summary>
	public abstract class Element : IDraw
	{
		private bool enabled = true, clipTestActive;
		private HorizontalAlignment hAlign;
		private VerticalAlignment vAlign;
		private AlphaBlendState blend = AlphaBlendState.None;
		internal Element parent;
		private Vector2 position;
		private static Xen.Camera.Camera2D camera;
		private static string cameraID = typeof(Element).FullName + ".camera";

		/// <summary>
		/// Gets an optional list of children for the element
		/// </summary>
		protected virtual List<Element> Children
		{
			get { return null; }
		}

		/// <summary>
		/// Gets/Sets if this element should write to depth
		/// </summary>
		public bool WriteToDepth { get; set; }

		/// <summary>
		/// Gets/Sets the alpha blend mode for this element
		/// </summary>
		public AlphaBlendState AlphaBlendState
		{
			get { return blend; }
			set { blend = value; }
		}

		/// <summary>
		/// Gets/Sets the vertical alignment of this element
		/// </summary>
		public VerticalAlignment VerticalAlignment
		{
			get { return vAlign; }
			set { vAlign = value; }
		}

		/// <summary>
		/// Gets/Sets the horizontal alignment of this element
		/// </summary>
		public HorizontalAlignment HorizontalAlignment
		{
			get { return hAlign; }
			set { hAlign = value; }
		}

		/// <summary>
		/// Set the parent of <paramref name="element"/> to this
		/// </summary>
		/// <param name="element"></param>
		protected void SetParentToThis(Element element)
		{
			element.parent = this;
		}
		/// <summary>
		/// Set parent to null
		/// </summary>
		/// <param name="element"></param>
		protected void ResetParent(Element element)
		{
			element.parent = null;
		}

		/// <summary>
		/// Gets the world matrix used to display this element. Defalut implementation generates a 2D matrix based on size and scale
		/// </summary>
		/// <param name="matrix"></param>
		/// <param name="scale"></param>
		/// <param name="elementSize"></param>
		protected virtual void GetDisplayMatrix(out Matrix matrix, Vector2 scale, ref Vector2 elementSize)
		{
			if (this.IsNormalised)
				scale = new Vector2(1, 1);
			bool useSize = UseSize;

#if XBOX360
			matrix = new Matrix();
#endif

			if (useSize)
				matrix.M11 = elementSize.X / scale.X;
			else
				matrix.M11 = 1.0f / scale.X;
			matrix.M12 = 0;
			matrix.M13 = 0;
			matrix.M14 = 0;

			matrix.M21 = 0;
			if (useSize)
				matrix.M22 = elementSize.Y / scale.Y;
			else
				matrix.M22 = 1.0f / scale.Y;
			matrix.M23 = 0;
			matrix.M24 = 0;

			matrix.M31 = 0;
			matrix.M32 = 0;
			matrix.M33 = 1;
			matrix.M34 = 0;

			matrix.M41 = position.X;
			matrix.M42 = position.Y;
			matrix.M43 = 0;
			matrix.M44 = 1;

			switch (vAlign)
			{
				case VerticalAlignment.Centre:
					matrix.M42 += (scale.Y - elementSize.Y) * 0.5f;
					break;
				case VerticalAlignment.Top:
					matrix.M42 += scale.Y - elementSize.Y;
					break;
			}
			switch (hAlign)
			{
				case HorizontalAlignment.Centre:
					matrix.M41 += (scale.X - elementSize.X) * 0.5f;
					break;
				case HorizontalAlignment.Right:
					matrix.M41 += scale.X - elementSize.X;
					break;
			}

			matrix.M41 /= scale.X;
			matrix.M42 /= scale.Y;
		}

		/// <summary>
		/// Gets/Sets if this element is visible
		/// </summary>
		public bool Visible
		{
			get { return enabled; }
			set { enabled = value; }
		}

		/// <summary>
		/// Gets the parent of this element
		/// </summary>
		public Element Parent
		{
			get { return parent; }
		}

		/// <summary>
		/// <para>Gets/Sets the position of this element</para>
		/// <para>Measured in pixels, from the bottom left. Note: Normalized elements are measured in [0,1] range, not exact pixel count</para>
		/// </summary>
		public Vector2 Position
		{
			get { return position; }
			set { position = value; }
		}

		/// <summary>
		/// Writes the default texture coordinates for this element
		/// </summary>
		/// <param name="topLeft"></param>
		/// <param name="topRight"></param>
		/// <param name="bottomLeft"></param>
		/// <param name="bottomRight"></param>
		protected virtual void WriteTextureCoords(ref Vector2 topLeft, ref Vector2 topRight, ref Vector2 bottomLeft, ref Vector2 bottomRight)
		{
			topLeft = new Vector2(0, 0);
			topRight = new Vector2(1, 0);
			bottomLeft = new Vector2(0, 1);
			bottomRight = new Vector2(1, 1);
		}

		/// <summary>
		/// Writes the default colours for this element
		/// </summary>
		/// <param name="topLeft"></param>
		/// <param name="topRight"></param>
		/// <param name="bottomLeft"></param>
		/// <param name="bottomRight"></param>
		protected virtual void WriteColours(ref Color topLeft, ref Color topRight, ref Color bottomLeft, ref Color bottomRight)
		{
			topLeft = Color.White;
			topRight = Color.White;
			bottomLeft = Color.White;
			bottomRight = Color.White;
		}

		/// <summary>
		/// Draw the element
		/// </summary>
		/// <param name="state"></param>
		public void Draw(DrawState state)
		{
			if (this.enabled)
				Draw(state, state.DrawTarget.Size, 255);
		}

		private void Draw(DrawState state, Vector2 scale, byte clipDepth)
		{
			Element parent = this.parent;


			Matrix matrix;

			if (parent == null)
			{
				this.clipTestActive = false;

				state.RenderState.Push();
				state.RenderState.CurrentRasterState.MultiSampleAntiAlias = false;
				state.RenderState.CurrentDepthState.DepthWriteEnabled = WriteToDepth;
				state.RenderState.CurrentDepthState.DepthTestEnabled = WriteToDepth;

				if (camera == null)
				{
					camera = state.Application.UserValues[cameraID] as Xen.Camera.Camera2D;
					if (camera == null)
					{
						camera = new Xen.Camera.Camera2D(true);
						state.Application.UserValues[cameraID] = camera;
					}
				}

				state.Camera.Push(camera);
			}
			else
				this.clipTestActive = parent.clipTestActive | parent.ClipsChildren;

			StencilState stencilState = new StencilState();
			if (clipTestActive)
			{
				stencilState.Enabled = true;
				stencilState.ReferenceValue = clipDepth;
				stencilState.StencilFunction = CompareFunction.Equal;
				stencilState.StencilPassOperation = StencilOperation.Keep;
			}

			bool clearStencil = false;
			if (this.ClipsChildren)
			{
				clearStencil = clipDepth == 255;
				clipDepth--;

				if (!clipTestActive)
				{
					//check there actually is a stencil buffer
#if DEBUG
					DepthFormat format = state.DrawTarget.SurfaceDepthFormat;

					if (format != DepthFormat.Depth24Stencil8)
						throw new InvalidOperationException("ElementRect.ClipChildren requires the DrawTarget has a valid Depth Buffer with an 8bit Stencil Buffer");
#endif

					stencilState.Enabled = true;
					stencilState.ReferenceValue = clipDepth;
					stencilState.StencilPassOperation = StencilOperation.Replace;
				}
				else
					stencilState.StencilPassOperation = StencilOperation.Decrement;
			}

			if ((scale.X != 0 && scale.Y != 0))
			{
				Vector2 size = ElementSize;
				GetDisplayMatrix(out matrix, scale, ref size);

				using (state.WorldMatrix.PushMultiply(ref matrix))
				{

					IShader shader = BindShader(state, false);
					if (shader != null)
						state.Shader.Push(shader);

					state.RenderState.CurrentBlendState = blend;
					state.RenderState.CurrentStencilState = stencilState;


					if (!UseSize)
						size = new Vector2(1, 1);
					else
						if (IsNormalised)
							size *= scale;

					PreDraw(size);

					DrawElement(state);

					List<Element> children = Children;
					if (children != null)
						foreach (Element child in children)
							if (((IDraw)child).CullTest(state))
								child.Draw(state, size, clipDepth);

					if (shader != null)
						state.Shader.Pop();

					if (clearStencil)
					{
						shader = BindShader(state, true);
						if (shader != null)
							state.Shader.Push(shader);
						stencilState = new StencilState();
						stencilState.Enabled = true;
						stencilState.StencilFunction = CompareFunction.Never;
						stencilState.StencilFailOperation = StencilOperation.Zero;
						state.RenderState.CurrentStencilState = stencilState;

						DrawElement(state);

						if (shader != null)
							state.Shader.Pop();
					}

				}
			}


			if (parent == null)
			{
				state.RenderState.Pop();
				state.Camera.Pop();
			}
		}



		/// <summary>
		/// <para>Attempt to get the layout (position and size) of this element</para>
		/// <para>The draw target the element is draw to must be specified</para>
		/// <para>Note this operation can be quite expensive if performed multiple times per frame</para>
		/// </summary>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="drawTarget"></param>
		/// <returns></returns>
		public bool TryGetLayout(out Vector2 position, out Vector2 size, DrawTarget drawTarget)
		{
			return TryGetLayout(out position, out size, drawTarget.Size);
		}
		/// <summary>
		/// <para>Attempt to get the layout (position and size) of this element</para>
		/// <para>The size of the draw target the element is being drawn to must be specified</para>
		/// <para>Note this operation can be quite expensive if performed multiple times per frame</para>
		/// </summary>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="drawTargetSize"></param>
		/// <returns></returns>
		public bool TryGetLayout(out Vector2 position, out Vector2 size, Vector2 drawTargetSize)
		{
			position = new Vector2();
			size = new Vector2(1, 1);
			Matrix tmp = Matrix.Identity;
			Vector2 originalSize = drawTargetSize;

			if (TryApplyLayout(ref position, ref size, ref drawTargetSize, ref tmp))
			{
				position.X *= originalSize.X;
				position.Y *= originalSize.Y;
				size.X *= originalSize.X;
				size.Y *= originalSize.Y;
				return true;
			}
			else
			{
				position = new Vector2();
				size = new Vector2();
				return false;
			}
		}

		private bool TryApplyLayout(ref Vector2 position, ref Vector2 localScale, ref Vector2 scale, ref Matrix matrix)
		{
			if (parent != null)
			{
				if (!parent.TryApplyLayout(ref position, ref localScale, ref scale, ref matrix))
					return false;
			}

			if ((scale.X != 0 && scale.Y != 0))
			{
				Vector2 size = ElementSize;
				GetDisplayMatrix(out matrix, scale, ref size);

				if (!UseSize)
					size = new Vector2(1, 1);
				else
					if (IsNormalised)
						size *= scale;

				scale = size;

				Vector2.Transform(ref position, ref matrix, out position);
				Vector2.TransformNormal(ref localScale, ref matrix, out localScale);

				return true;
			}
			return false;
		}



		/// <summary>
		/// Override to implement draw logic
		/// </summary>
		/// <param name="state"></param>
		protected abstract void DrawElement(DrawState state);

		/// <summary>
		/// <para>Override to implement logic that occurs before the element is drawn, such as dirtying the element if it's size has changed</para>
		/// </summary>
		/// <param name="size"></param>
		protected virtual void PreDraw(Vector2 size) { }

		/// <summary>
		/// Override to bind the shader used by drawing. If <paramref name="maskOnly"/> is true, the element is being drawn as a stencil mask
		/// </summary>
		/// <param name="state"></param>
		/// <param name="maskOnly"></param>
		protected abstract IShader BindShader(DrawState state, bool maskOnly);


		bool ICullable.CullTest(ICuller culler)
		{
			return enabled;
		}

		/// <summary>
		/// Override and return true to clip children
		/// </summary>
		protected virtual bool ClipsChildren
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Override and return true to normalise the element
		/// </summary>
		protected virtual bool IsNormalised
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Override and return false if this element does not have a size (default is true)
		/// </summary>
		protected virtual bool UseSize
		{
			get { return true; }
		}
		/// <summary>
		/// Override to return the size of the element
		/// </summary>
		protected abstract Vector2 ElementSize
		{
			get;
		}
	}

	/// <summary>
	/// Abstract class for a 2D element storing position size and scaling information.
	/// </summary>
	public abstract class ElementRect : Element
	{
		private VertexPositionColorTexture[] vertexData = new VertexPositionColorTexture[4];
		private Vertices<VertexPositionColorTexture> vertices;
		private bool clipChildren = false, dirty = true, normalised;
		private Vector2 size;
		private ElementScaling hSize;
		private ElementScaling vSize;
		private List<Element> children;
		private bool enableDepthTest;

		/// <summary></summary>
		/// <param name="sizeInPixels"></param>
		protected ElementRect(Vector2 sizeInPixels)
			: this(sizeInPixels,false)
		{
		}

		/// <summary>
		/// Construct the rect
		/// </summary>
		/// <param name="size">If Normalised, the position and size are stored as [0,1] range. Otherwise the position and size are measured in pixels</param>
		/// <param name="normalised"></param>
		protected ElementRect(Vector2 size, bool normalised)
		{
			this.normalised = normalised;
			this.Size = size;
		}



		/// <summary></summary>
		protected sealed override List<Element> Children
		{
			get
			{
				return children;
			}
		}

		/// <summary>
		/// Add a child element to this element
		/// </summary>
		/// <param name="element"></param>
		public void Add(Element element)
		{
			if (element == null)
				throw new ArgumentNullException();
			if (element.parent != null)
				throw new InvalidOperationException("element.parent");
			
			SetParentToThis(element);

			if (children == null)
				children = new List<Element>();
			children.Add(element);
		}

		/// <summary>
		/// Remove a child element from this element
		/// </summary>
		/// <param name="element"></param>
		public void Remove(Element element)
		{
			if (children != null && element != null && element.parent == this)
			{
				children.Remove(element);
				ResetParent(element);
			}
		}

		/// <summary>
		/// Gets/Sets the vertical scaling logic of this element
		/// </summary>
		public ElementScaling VerticalScaling
		{
			get { return vSize; }
			set { vSize = value; }
		}

		/// <summary>
		/// Gets/Sets the horizontal scaling logic of this element
		/// </summary>
		public ElementScaling HorizontalScaling
		{
			get { return hSize; }
			set { hSize = value; }
		}

		/// <summary>
		/// <para>When true, the element will be displayed at a Z depth of 1.0 (maximum), with depth testing enabled</para>
		/// <para>When true, this element will be occluded by any 3D object that has been drawn</para>
		/// </summary>
		public bool DrawAtMaxZDepth
		{
			get { return enableDepthTest; }
			set 
			{
				if (enableDepthTest != value)
				{
					enableDepthTest = value;
					dirty = true;
				}
			}
		}

		/// <summary></summary>
		/// <param name="matrix"></param>
		/// <param name="scale"></param>
		/// <param name="elementSize"></param>
		protected override void GetDisplayMatrix(out Matrix matrix, Vector2 scale, ref Vector2 elementSize)
		{
			if (hSize == ElementScaling.FillToParentPlusSize)
				elementSize.X += normalised ? (1 - this.Position.X) : (scale.X - this.Position.X);
			if (vSize == ElementScaling.FillToParentPlusSize)
				elementSize.Y += normalised ? (1 - this.Position.Y) : (scale.Y - this.Position.Y);

			base.GetDisplayMatrix(out matrix, scale, ref elementSize);
		}

		/// <summary></summary>
		/// <param name="state"></param>
		protected override void DrawElement(DrawState state)
		{
			bool depthState = false;
			if (enableDepthTest)
			{
				depthState = state.RenderState.CurrentDepthState.DepthTestEnabled;
				state.RenderState.CurrentDepthState.DepthTestEnabled = true;
			}

			vertices.Draw(state, null, PrimitiveType.TriangleStrip);

			if (enableDepthTest)
				state.RenderState.CurrentDepthState.DepthTestEnabled = depthState;
		}

		/// <summary>
		/// If true, the stored coordinates are in the range [0,1]. When false, the coordinates are in pixels
		/// </summary>
		public bool NormalisedCoordinates
		{
			get { return normalised; }
			set { normalised = value; }
		}

		/// <summary>
		/// Updates vertex data when dirty
		/// </summary>
		/// <param name="size"></param>
		protected override void PreDraw(Vector2 size)
		{
			if (dirty)
			{
				vertexData[0].Position = new Vector3(1, 0, 1);
				vertexData[1].Position = new Vector3(0, 0, 1);
				vertexData[2].Position = new Vector3(1, 1, 1);
				vertexData[3].Position = new Vector3(0, 1, 1);

				WriteTextureCoords(ref vertexData[3].TextureCoordinate, ref vertexData[2].TextureCoordinate,
					ref vertexData[1].TextureCoordinate, ref vertexData[0].TextureCoordinate);
				WriteColours(ref vertexData[3].Color, ref vertexData[2].Color,
					ref vertexData[1].Color, ref vertexData[0].Color);

				if (vertices == null)
				{
					vertices = new Vertices<VertexPositionColorTexture>(vertexData);
					vertices.ResourceUsage = ResourceUsage.Dynamic;
				}
				else
					vertices.SetDirty();
				dirty = false;
			}
		}

		/// <summary>
		/// Set the element as dirty (vertex buffers will be updated)
		/// </summary>
		protected void SetDirty()
		{
			dirty = true;
		}

		/// <summary>
		/// Gets the size of the element
		/// </summary>
		protected override sealed Vector2 ElementSize
		{
			get { return size; }
		}

		/// <summary></summary>
		protected override sealed bool ClipsChildren
		{
			get
			{
				return clipChildren && this.children != null;
			}
		}

		/// <summary></summary>
		protected override sealed bool IsNormalised
		{
			get
			{
				return normalised;
			}
		}

		/// <summary>
		/// Visibly clip children to this element using the stencil buffer. NOTE: requires an 8bit stencil buffer, and assumes the stencil buffer is cleared before drawing root elements.
		/// </summary>
		/// <remarks>Stencil buffer is reset to zero once rendering is complete.</remarks>
		public bool ClipChildren
		{
			get { return clipChildren; }
			set { clipChildren = value; }
		}

		/// <summary>
		/// <para>Gets/Sets the size of this element.</para>
		/// <para>Measured in pixels, if <see cref="NormalisedCoordinates"/> is true then the size is measured in the range [0,1]</para>
		/// </summary>
		public Vector2 Size
		{
			get { return size; }
			set
			{
				if (size != value)
				{
					size = value;
					SizeChanged();
				}
			}
		}
		/// <summary></summary>
		protected virtual void SizeChanged()
		{
		}
	}
}
