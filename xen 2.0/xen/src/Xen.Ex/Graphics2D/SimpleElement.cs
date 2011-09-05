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
	/// Element that displays a texture
	/// </summary>
	public class TexturedElement : ElementRect
	{
		private static TextureSamplerState pointFilter,bilinearFilter;
		private Rectangle? crop;
		private Vector2 pixelSize;
		private Texture2D texture;
		private DrawTargetTexture2D textureSource;


		/// <summary>
		/// Gets/Sets a cropping rectangle for this element
		/// </summary>
		public Rectangle? TextureCrop
		{
			get { return crop; }
			set
			{
				if (this.crop != value)
				{
					this.crop = value;
					SetDirty();
				}
			}
		}

		/// <summary>
		/// Construct the element
		/// </summary>
		/// <param name="sizeInPixels"></param>
		public TexturedElement(Vector2 sizeInPixels)
			: base(sizeInPixels)
		{
		}

		/// <summary>
		/// Construct the element
		/// </summary>
		/// <param name="sizeInPixels"></param>
		/// <param name="texture"></param>
		public TexturedElement(Texture2D texture, Vector2 sizeInPixels) : base(sizeInPixels)
		{
			this.texture = texture;
		}

		/// <summary>
		/// Construct the element
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="size">If Normalised, the position and size are stored as [0,1] range. Otherwise the position and size are measured in pixels</param>
		/// <param name="normalised"></param>
		public TexturedElement(Texture2D texture, Vector2 size, bool normalised) : base(size,normalised)
		{
			this.texture = texture;
		}


		/// <summary>
		/// Construct the element, using a <see cref="DrawTargetTexture2D"/> as the texture source
		/// </summary>
		/// <param name="sizeInPixels"></param>
		/// <param name="textureSource"></param>
		public TexturedElement(DrawTargetTexture2D textureSource, Vector2 sizeInPixels) : base(sizeInPixels)
		{
			if (textureSource == null)
				throw new ArgumentNullException();

			this.texture = textureSource.GetTexture();
			this.textureSource = textureSource;
		}

		/// <summary>
		/// Construct the element, using a <see cref="DrawTargetTexture2D"/> as the texture source
		/// </summary>
		/// <param name="textureSource"></param>
		/// <param name="size">If Normalised, the position and size are stored as [0,1] range. Otherwise the position and size are measured in pixels</param>
		/// <param name="normalised"></param>
		public TexturedElement(DrawTargetTexture2D textureSource, Vector2 size, bool normalised) : base(size,normalised)
		{
			if (textureSource == null)
				throw new ArgumentNullException();

			this.texture = textureSource.GetTexture();
			this.textureSource = textureSource;
		}

		static TexturedElement()
		{
			bilinearFilter = TextureSamplerState.BilinearFiltering;
			bilinearFilter.AddressUV = TextureAddressMode.Clamp;

			pointFilter = TextureSamplerState.PointFiltering;
			pointFilter.AddressUV = TextureAddressMode.Clamp;
		}

		/// <summary></summary>
		/// <param name="size"></param>
		protected override void PreDraw(Vector2 size)
		{
			if (size != pixelSize)
			{
				SetDirty();
				pixelSize = size;
			}
			base.PreDraw(size);
		}

		private bool usePointFilter = false;

		/// <summary>
		/// Gets/Sets if the texture should be displayed with point filtering
		/// </summary>
		public bool UsePointFiltering
		{
			get { return usePointFilter; }
			set { usePointFilter = value; }
		}

		/// <summary>
		/// Gets/Sets the texture being displayed
		/// </summary>
		public Texture2D Texture
		{
			get { if (textureSource != null) return textureSource.GetTexture(); return texture; }
			set 
			{
				if (value == null)
					throw new ArgumentNullException();

				if (textureSource != null && value != textureSource.GetTexture())
					textureSource = null;

				if (value != texture)
				{
					texture = value;
					SetDirty();
				}
			}
		}

		/// <summary></summary>
		/// <param name="topLeft"></param>
		/// <param name="topRight"></param>
		/// <param name="bottomLeft"></param>
		/// <param name="bottomRight"></param>
		protected override void WriteTextureCoords(ref Vector2 topLeft, ref Vector2 topRight, ref Vector2 bottomLeft, ref Vector2 bottomRight)
		{
			float x0 = 0, x1 = 1;
			float y0 = 0, y1 = 1;

			Texture2D texture = this.texture;

			if (texture == null &&
				textureSource != null)
				texture = textureSource.GetTexture();

			if (pixelSize.X != 0 && pixelSize.Y != 0)
			{
				x0 = 0.5f / pixelSize.X;
				x1 = 1 + x0;
				y0 = 0.5f / pixelSize.Y;
				y1 = 1 + y0;

				if (crop != null && texture != null)
				{
					Vector2 size = new Vector2(1.0f / texture.Width, 1.0f / texture.Height);
					Rectangle r = crop.Value;

					x0 *= r.Width * size.X;
					y0 *= r.Height * size.Y;

					x1 = (r.X + r.Width) * size.X + x0;
					y1 = (r.Y + r.Height) * size.Y + y0;

					x0 += r.X * size.X;
					y0 += r.Y * size.Y;
				}
			}


			topLeft = new Vector2(x0, y0);
			topRight = new Vector2(x1, y0);
			bottomLeft = new Vector2(x0, y1);
			bottomRight = new Vector2(x1, y1);
		}

		/// <summary></summary>
		/// <param name="state"></param>
		/// <param name="maskOnly"></param>
		protected override IShader BindShader(DrawState state, bool maskOnly)
		{
			if (this.texture == null && textureSource != null)
			{
				Texture2D tex = textureSource.GetTexture();
				if (tex == null)
					throw new InvalidOperationException("TexturedElement is trying to use a DrawTargetTexture2D that hasn't been drawn (DrawTargetTexture2D.GetTexture() is null)");
				this.Texture = tex;
			}

			FillCustomTexture shader = state.GetShader<FillCustomTexture>();

			shader.CustomTexture = texture;
			shader.CustomTextureSampler = usePointFilter ? pointFilter : bilinearFilter;

			return shader;
		}
	}

	/// <summary>
	/// An element that is displayed with a custom shader
	/// </summary>
	public class ShaderElement : ElementRect
	{
		private IShader shader;
		private int texSizeX, texSizeY;
		private Vector2 pixelSize;
		private Rectangle? crop;


		/// <summary>
		/// Construct the element
		/// </summary>
		/// <param name="shader"></param>
		/// <param name="sizeInPixels"></param>
		public ShaderElement(IShader shader, Vector2 sizeInPixels)
			: this(shader, 0, 0, sizeInPixels, false)
		{
		}

		/// <summary>
		/// Construct the element
		/// </summary>
		/// <param name="shader"></param>
		/// <param name="size">If Normalised, the position and size are stored as [0,1] range. Otherwise the position and size are measured in pixels</param>
		/// <param name="normalised"></param>
		public ShaderElement(IShader shader, Vector2 size, bool normalised) : this(shader,0,0,size,normalised)
		{
		}
		/// <summary>
		/// Construct the element, specifying the size of the texture that will be used
		/// </summary>
		/// <param name="shader"></param>
		/// <param name="textureWidth"></param>
		/// <param name="textureHeight"></param>
		/// <param name="sizeInPixels"></param>
		public ShaderElement(IShader shader, int textureWidth, int textureHeight, Vector2 sizeInPixels) : base(sizeInPixels)
		{
			if (shader == null)
				throw new ArgumentNullException();
			this.shader = shader;
			if (textureHeight != 0 || textureWidth != 0)
				this.SetTextureSize(textureWidth, textureHeight);
		}

		/// <summary>
		/// Gets/Sets an optional texture crop used by this element
		/// </summary>
		public Rectangle? TextureCrop
		{
			get { return crop; }
			set
			{
				if (this.crop.HasValue != value.HasValue || (this.crop.HasValue && this.crop.Value != value.Value))
				{
					this.crop = value;
					SetDirty();
				}
			}
		}

		/// <summary>
		/// Construct the element, specifying the size of the texture that will be used
		/// </summary>
		/// <param name="shader"></param>
		/// <param name="textureHeight"></param>
		/// <param name="textureWidth"></param>
		/// <param name="size">If Normalised, the position and size are stored as [0,1] range. Otherwise the position and size are measured in pixels</param>
		/// <param name="normalised"></param>
		public ShaderElement(IShader shader, int textureWidth, int textureHeight, Vector2 size, bool normalised)
			: base(size,normalised)
		{
			this.shader = shader;
			if (textureHeight != 0 || textureWidth != 0)
				this.SetTextureSize(textureWidth, textureHeight);
		}


		/// <summary></summary>
		/// <param name="size"></param>
		protected override void PreDraw(Vector2 size)
		{
			if (size != pixelSize)
			{
				pixelSize = size;
				SetDirty();
			}
			base.PreDraw(size);
		}

		/// <summary>
		/// <para>Set the size of a texture being displayed by this element.</para>
		/// <para>Setting this value only modifies the texture coordinates generated, to produce more accurate texture filtering</para>
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void SetTextureSize(int width, int height)
		{
			if (this.texSizeX != width ||
				this.texSizeY != height)
			{
				this.texSizeX = width;
				this.texSizeY = height;
				SetDirty();
			}
		}

		/// <summary>
		/// Gets/Sets the shader used by this element
		/// </summary>
		public IShader Shader
		{
			get { return shader; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();
				shader = value;
			}
		}

		/// <summary></summary>
		/// <param name="topLeft"></param>
		/// <param name="topRight"></param>
		/// <param name="bottomLeft"></param>
		/// <param name="bottomRight"></param>
		protected override void WriteTextureCoords(ref Vector2 topLeft, ref Vector2 topRight, ref Vector2 bottomLeft, ref Vector2 bottomRight)
		{
			float x0 = 0, x1 = 1;
			float y0 = 0, y1 = 1;

			if (pixelSize.X != 0 && pixelSize.Y != 0)
			{
				x0 = 0.5f / pixelSize.X;
				x1 = 1 + x0;
				y0 = 0.5f / pixelSize.Y;
				y1 = 1 + y0;

				if (crop != null &&
					texSizeX != 0 && texSizeY != 0)
				{
					Vector2 size = new Vector2(1.0f / texSizeX, 1.0f / texSizeY);
					Rectangle r = crop.Value;

					x0 *= r.Width * size.X;
					y0 *= r.Height * size.Y;

					x1 = (r.X + r.Width) * size.X + x0;
					y1 = (r.Y + r.Height) * size.Y + y0;
					x0 += r.X * size.X;
					y0 += r.Y * size.Y;
				}
			}

			topLeft = new Vector2(x0, y0);
			topRight = new Vector2(x1, y0);
			bottomLeft = new Vector2(x0, y1);
			bottomRight = new Vector2(x1, y1);
		}

		/// <summary></summary>
		/// <param name="state"></param>
		/// <param name="maskOnly"></param>
		protected override IShader BindShader(DrawState state, bool maskOnly)
		{
			if (shader == null)
				throw new InvalidOperationException("Shader == null");
			return shader;
		}
	}

	/// <summary>
	/// Creates an element that displays a solid colour
	/// </summary>
	public class SolidColourElement : ElementRect
	{
		private Color colour = Color.CornflowerBlue;

		/// <summary>
		/// Construct the element
		/// </summary>
		/// <param name="colour"></param>
		/// <param name="size">If Normalised, the position and size are stored as [0,1] range. Otherwise the position and size are measured in pixels</param>
		/// <param name="normalised"></param>
		public SolidColourElement(Color colour, Vector2 size, bool normalised)
			: base(size,normalised)
		{
			this.colour = colour;
		}
		/// <summary>
		/// Construct the element
		/// </summary>
		/// <param name="colour"></param>
		/// <param name="sizeInPixels"></param>
		public SolidColourElement(Color colour, Vector2 sizeInPixels)
			: base(sizeInPixels)
		{
			this.colour = colour;
		}

		/// <summary>
		/// Gets/Sets the colour used by this element
		/// </summary>
		public Color Colour
		{
			get { return colour; }
			set 
			{
				if (colour != value)
				{
					colour = value;
					SetDirty();
				}
			}
		}

		/// <summary></summary>
		/// <param name="state"></param>
		/// <param name="maskOnly"></param>
		protected override IShader BindShader(DrawState state, bool maskOnly)
		{
			return state.GetShader<Shaders.FillVertexColour>();
		}

		/// <summary></summary>
		/// <param name="topLeft"></param>
		/// <param name="topRight"></param>
		/// <param name="bottomLeft"></param>
		/// <param name="bottomRight"></param>
		protected override void WriteColours(ref Color topLeft, ref Color topRight, ref Color bottomLeft, ref Color bottomRight)
		{
			topLeft = colour;
			topRight = colour;
			bottomLeft = colour;
			bottomRight = colour;
		}
	}

}
