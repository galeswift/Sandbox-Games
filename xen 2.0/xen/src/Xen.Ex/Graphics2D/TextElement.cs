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
	/// Interface for a <see cref="TextElement"/> or a <see cref="TextElementRect"/>
	/// </summary>
	public interface ITextElement
	{
		/// <summary>
		/// Gets/Sets the colour of the text
		/// </summary>
		Color Colour { get; set; }
		/// <summary>
		/// Gets/Sets the colour of the text (as a <see cref="Vector4"/>)
		/// </summary>
		Vector4 ColourFloat { get; set; }
		/// <summary>
		/// <para>Gets/Sets the XNA <see cref="SpriteFont"/> used by ths element</para>
		/// <para>Note: The font must be set before drawing the text</para>
		/// </summary>
		SpriteFont Font { get; set; }
		/// <summary>
		/// Adds a child element that is embedded inline at the given text index
		/// </summary>
		/// <param name="element"></param>
		/// <param name="textIndex"></param>
		void AddInline(Element element, uint textIndex);
		/// <summary>
		/// Adds a child element that is embedded inline at the given text index
		/// </summary>
		/// <param name="element"></param>
		/// <param name="textIndex"></param>
		/// <param name="elementOffset"></param>
		void AddInline(Element element, uint textIndex, Vector2 elementOffset);
		/// <summary>
		/// Adds a child element that is embedded inline at the given text index
		/// </summary>
		/// <param name="element"></param>
		/// <param name="textIndex"></param>
		/// <param name="elementOffset"></param>
		void AddInline(Element element, uint textIndex, ref Vector2 elementOffset);
		/// <summary>
		/// Tries to remove an element added with <see cref="AddInline(Element, uint)"/>
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		bool RemoveInline(Element element);
		/// <summary>
		/// Tries to remove an element added with <see cref="AddInline(Element, uint)"/>
		/// </summary>
		bool RemoveInlineAt(uint textIndex);
		/// <summary>
		/// Remove all elements added with <see cref="AddInline(Element, uint)"/>
		/// </summary>
		void RemoveInlineChildren();
		/// <summary>
		/// <para>Gets the <see cref="TextValue"/> of this element (See implementation remarks for assignment details)</para>
		/// </summary>
		TextValue Text { get; set; }
	}

	/// <summary>
	/// Text alignment enumeration used by <see cref="TextElementRect"/>
	/// </summary>
	public enum TextHorizontalAlignment : byte
	{
		//useful comments!
		/// <summary>
		/// Left alignment
		/// </summary>
		Left,
		/// <summary>
		/// Right alignment
		/// </summary>
		Right,
		/// <summary>
		/// Centre alignment
		/// </summary>
		Centre,
		/// <summary>
		/// Justified alignment
		/// </summary>
		Justified,
	}

	//class that wraps up the child elements
	//deals with elements that are inserted into a text string
	class TextChildren
	{
		//this list must be manually sorted, as the xbox no longer supports sorted collections (!!!!)
		struct ElementOffset : IComparable<ElementOffset>
		{
			public uint Index;
			public Element Element;
			public Vector2 Offset;

			public int CompareTo(ElementOffset other)
			{
				return Index > other.Index ? 1 : -1;
			}
		}

		private readonly List<Element> children;
		private List<ElementOffset> insertedElements;
		private bool insertedElementsSorted = true;
		private int insertIndex;


		public TextChildren(SpriteElement rootSprite)
		{
			this.children = new List<Element>(1);
			this.children.Add(rootSprite);
		}
		public TextChildren(List<Element> children)
		{
			this.children = children;
		}



		public List<Element> Children { get { return children; } }
		private Element TextElement { get { return children[0]; } }

		//add / remove
		public void AddElement(Element element, uint textIndex, ref Vector2 elementOffset)
		{
			if (insertedElements == null)
				insertedElements = new List<ElementOffset>();

			this.children.Add(element);

			ElementOffset offset = new ElementOffset();
			offset.Element = element;
			offset.Offset = elementOffset;
			offset.Index = textIndex;

			this.insertedElements.Add(offset);
			insertedElementsSorted = false;
		}
		public bool RemoveElementAt(uint textIndex, out Element element)
		{
			ElementOffset find = new ElementOffset();
			find.Index = textIndex;
			element = null;

			if (this.insertedElements != null)
			{
				if (!insertedElementsSorted)
				{
					insertedElements.Sort();
					insertedElementsSorted = true;
				}

				int index = this.insertedElements.BinarySearch(find);
				if (index != -1)
				{
					element = this.insertedElements[index].Element;
					this.children.Remove(element);
					this.insertedElements.RemoveAt(index);
					insertedElementsSorted = false;
					return true;
				}
			}
			return false;
		}
		public bool RemoveElement(Element element)
		{
			if (this.insertedElements != null)
			{
				for (int i = 0; i < this.insertedElements.Count; i++)
				{
					if (this.insertedElements[i].Element == element)
					{
						this.insertedElements.RemoveAt(i);
						this.children.Remove(element);
						insertedElementsSorted = false;
						return true;
					}
				}
			}
			return false;
		}
		public void RemoveAll()
		{
			if (this.insertedElements != null)
			{
				insertedElements.Clear();
				this.children.RemoveRange(1, this.children.Count - 1);
			}
		}

		//running through the text string...
		public uint ResetInsertIndex()
		{
			if (insertedElements == null || insertedElements.Count == 0)
				return uint.MaxValue;
			insertIndex = 0;
			if (insertedElementsSorted == false)
			{
				this.insertedElements.Sort();
				this.insertedElementsSorted = true;
			}
			return insertedElements[0].Index;
		}

		public uint GetNextInsertedIndex()
		{
			insertIndex++;
			if (insertedElements == null ||
				insertIndex >= insertedElements.Count)
				return uint.MaxValue;
			if (insertedElementsSorted == false)
			{
				this.insertedElements.Sort();
				this.insertedElementsSorted = true;
			}
			return insertedElements[insertIndex].Index;
		}

		public uint GetInsertedCount()
		{
			return (uint)insertIndex;
		}

		public void MoveRange(ref Vector2 offset, uint start)
		{
			if (insertedElementsSorted == false)
			{
				this.insertedElements.Sort();
				this.insertedElementsSorted = true;
			}

			for (int i = (int)start; i < insertIndex; i++)
			{
				insertedElements[i].Element.Position += offset;
			}
		}
		public void MoveRangeUptoIndex(ref Vector2 offset, ref uint start, int maxTextIndex)
		{
			if (insertedElementsSorted == false)
			{
				this.insertedElements.Sort();
				this.insertedElementsSorted = true;
			}
			while (start < insertIndex)
			{
				if (insertedElements[(int)start].Index > maxTextIndex)
					return;
				insertedElements[(int)start].Element.Position += offset;
				start++;
			}
		}

		public void SetInsertedElementLayout(ref Vector2 position, out float insertedWidth)
		{
			if (insertedElementsSorted == false)
			{
				this.insertedElements.Sort();
				this.insertedElementsSorted = true;
			}

			Element text = TextElement;
			ElementOffset offset = insertedElements[insertIndex];
			Element element = offset.Element;

			insertedWidth = 0;
			if (element is ElementRect)
				insertedWidth = (element as ElementRect).Size.X;

			offset.Offset.X += position.X;
			offset.Offset.Y += position.Y;

			element.Position = offset.Offset;
			element.VerticalAlignment = text.VerticalAlignment;
			element.HorizontalAlignment = text.HorizontalAlignment;
		}
	}

	/// <summary>
	/// <para>Element that displays a text string at a position</para>
	/// <para>Use <see cref="TextElementRect"/> to display text in a rectangle</para>
	/// </summary>
	public class TextElement : Element, ITextElement
	{
		private ElementFont font;
		private SpriteFont spriteFont;
		private SpriteElement sprite;
		private TextValue text;
		private int textChangeIndex = -1;
		private TextChildren children;
		private Vector4 colour = Vector4.One;
		private bool dirty = false;

		/// <summary>
		/// <para>Construct the text element</para>
		/// <para>Note: the <see cref="Font"/> must be set before drawing</para>
		/// </summary>
		public TextElement()
			: this((string)null, null)
		{
		}
		/// <summary>
		/// <para>Construct the text element</para>
		/// <para>Note: the <see cref="Font"/> must be set before drawing</para>
		/// </summary>
		/// <param name="text"></param>
		public TextElement(string text)
			: this(text, null)
		{
		}

		private TextElement(SpriteFont font)
		{
			if (this.text == null)
				this.text = new TextValue();

			Texture2D texture = null;

			if (font != null)
			{
				this.font = font;
				this.spriteFont = font;
				texture = this.font.textureValue;
			}

			sprite = new SpriteElement(texture);
			sprite.VerticalAlignment = VerticalAlignment.Top;
			this.VerticalAlignment = VerticalAlignment.Top;

			children = new TextChildren(sprite);
			SetParentToThis(sprite);

			sprite.AlphaBlendState = AlphaBlendState.Alpha;
		}

		/// <summary>
		/// <para>Construct the text element</para>
		/// </summary>
		/// <param name="font"></param>
		/// <param name="text"></param>
		public TextElement(TextValue text, SpriteFont font)
			: this(font)
		{
			if (text == null)
				throw new ArgumentNullException();
			this.text = text;
		}
		/// <summary>
		/// <para>Construct the text element</para>
		/// </summary>
		/// <param name="font"></param>
		/// <param name="text"></param>
		public TextElement(string text, SpriteFont font)
			: this(font)
		{
			this.text = new TextValue();
			if (text != null)
				this.text.Append(text);
		}

		/// <summary>
		/// Gets/Sets the colour of the text
		/// </summary>
		public Color Colour
		{
			get { return new Color(colour); }
			set
			{
				if (new Color(colour) != value)
				{
					this.colour = value.ToVector4();
					dirty = true;
				}
			}
		}
		/// <summary>
		/// Gets/Sets the colour of the text (as a <see cref="Vector4"/>)
		/// </summary>
		public Vector4 ColourFloat
		{
			get { return colour; }
			set
			{
				if (colour != value)
				{
					this.colour = value;
					dirty = true;
				}
			}
		}

		/// <summary>
		/// <para>Gets/Sets the XNA <see cref="SpriteFont"/> used by ths element</para>
		/// <para>Note: The font must be set before drawing the text</para>
		/// </summary>
		public SpriteFont Font
		{
			get { return spriteFont; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("font");
				if (this.spriteFont != value)
				{
					this.font = value;
					this.spriteFont = value;
					this.textChangeIndex = -1;
				}
			}
		}

		/// <summary></summary>
		protected sealed override List<Element> Children
		{
			get
			{
				return children.Children;
			}
		}

		/// <summary>
		/// Adds a child element that is embedded inline at the given text index
		/// </summary>
		/// <param name="element"></param>
		/// <param name="textIndex"></param>
		public void AddInline(Element element, uint textIndex)
		{
			Vector2 zero = new Vector2();
			AddInline(element, textIndex, ref zero);
		}
		/// <summary>
		/// Adds a child element that is embedded inline at the given text index
		/// </summary>
		/// <param name="element"></param>
		/// <param name="textIndex"></param>
		/// <param name="elementOffset"></param>
		public void AddInline(Element element, uint textIndex, Vector2 elementOffset)
		{
			AddInline(element, textIndex, ref elementOffset);
		}
		/// <summary>
		/// Adds a child element that is embedded inline at the given text index
		/// </summary>
		/// <param name="element"></param>
		/// <param name="textIndex"></param>
		/// <param name="elementOffset"></param>
		public void AddInline(Element element, uint textIndex, ref Vector2 elementOffset)
		{
			if (textIndex >= this.text.Length)
				throw new ArgumentException("textIndex >= this.Text.Length");

			SetParentToThis(element);
			this.children.AddElement(element, textIndex, ref elementOffset);

			this.dirty = true;
		}
		/// <summary>
		/// Tries to remove an element added with <see cref="AddInline(Element,uint)"/>
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public bool RemoveInline(Element element)
		{
			if (this.children.RemoveElement(element))
			{
				this.ResetParent(element);

				this.dirty = true;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Tries to remove an element added with <see cref="AddInline(Element,uint)"/>
		/// </summary>
		public bool RemoveInlineAt(uint textIndex)
		{
			Element element;
			if (this.children.RemoveElementAt(textIndex, out element))
			{
				this.ResetParent(element);

				this.dirty = true;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Remove all elements added with <see cref="AddInline(Element,uint)"/>
		/// </summary>
		public void RemoveInlineChildren()
		{
			if (this.children.Children.Count > 1)
				this.dirty = true;

			for (int i = 1; i < this.children.Children.Count; i++)
				this.ResetParent(this.children.Children[i]);
			this.children.RemoveAll();
		}

		/// <summary>
		/// <para>Gets the <see cref="TextValue"/> of this element (See remarks for assignment details)</para>
		/// </summary>
		/// <remarks>
		/// <para>The <see cref="TextValue"/> object allows appending of strings, integers, etc. It does not allow direct asignment.</para>
		/// <para>Therefore, it is safe to do the following:</para>
		/// <code>textElement.Text += "...";</code>
		/// <para>But you cannot do:</para>
		/// <code>textElement.Text = "...";</code>
		/// <para>To set the text value, call SetText():</para>
		/// <code>textElement.Text.SetText("...");</code>
		/// <para>Appending will not allocate memory like normal string appending, as the internal <see cref="TextValue"/> is appending a <see cref="StringBuilder"/></para>
		/// </remarks>
		public TextValue Text
		{
			get { return text; }
			set { if (value != text) throw new ArgumentException("Cannot reassign text value"); }
		}

		private void Build()
		{
			if (this.font == null)
			{
				throw new InvalidOperationException("TextElement.Font == null");
			}

			sprite.SetTextureAndClear(font.textureValue);

			Vector2 position = new Vector2();
			bool flag = true;
			int index;

			uint insertedElementIndex = children.ResetInsertIndex();

			StringBuilder text = this.text.value;

			if (text.Length > 0)
			{
				for (int i = 0; i < text.Length; i++)
				{

					if (i == insertedElementIndex)
					{
						float width;

						//update the inserted element
						children.SetInsertedElementLayout(ref position, out width);
						position.X += width;
						insertedElementIndex = children.GetNextInsertedIndex();
					}


					char c = text[i];

					switch (c)
					{
						case '\r':
							break;

						case '\n':
							flag = true;
							position.X = 0;
							position.Y -= font.lineSpacingF;
							break;

						case '\t':
							if (font.TryGetCharacterIndex(' ', out index))
							{
								Vector3 kerning = font.kerning[index];
								float width = ((kerning.Y + kerning.Z)) * 4;

								position.X = (float)(Math.Floor(position.X / width) + 1) * width;
							}
							break;

						default:
							if (font.TryGetCharacterIndex(c, out index))
							{
								Vector3 kerning = font.kerning[index];
								if (flag)
								{
									kerning.X = Math.Max(kerning.X, 0f);
								}
								position.X += (kerning.X);

								Vector4 crop = font.croppingDataV4[index];

								Vector2 pos = position;
								pos.X += crop.X;
								pos.Y -= crop.Y + font.glyphSize[index].Y;


								sprite.AddSprite(ref pos, ref font.glyphSize[index], ref colour, ref font.glyphDataV4[index]);

								flag = false;
								position.X += ((kerning.Y + kerning.Z));
							}
							break;
					}
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
		/// <param name="state"></param>
		protected sealed override void DrawElement(DrawState state)
		{
		}
		/// <summary></summary>
		/// <param name="size"></param>
		protected sealed override void PreDraw(Vector2 size)
		{
			if (dirty || text.HasChanged(ref textChangeIndex))
			{
				Build();
				dirty = false;
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
			get
			{
				return false;
			}
		}
	}


	/// <summary>
	/// <para>Element that displays a text string within a rectangle</para>
	/// </summary>
	public class TextElementRect : ElementRect, ITextElement
	{
		private ElementFont font;
		private SpriteFont spriteFont;
		private SpriteElement sprite;
		private TextChildren children;
		private readonly TextValue text;
		private int textChangeIndex = -1;
		private Vector2 pixelSize;
		private TextHorizontalAlignment textAlign = TextHorizontalAlignment.Left;
		private VerticalAlignment textVAlign = VerticalAlignment.Top;
		private bool sizeToVertical;
		private List<int> spaces = null;
		private Vector4 colour = Vector4.One;

		/// <summary>
		/// When true, this element will be vertically sized to fit the text content
		/// </summary>
		public bool VerticalSizeToContent
		{
			get { return sizeToVertical; }
			set
			{
				if (sizeToVertical != value)
				{
					sizeToVertical = value;
					textChangeIndex = -1;
				}
			}
		}

		/// <summary>
		/// Gets/Sets the colour of the text
		/// </summary>
		public Color Colour
		{
			get { return new Color(colour); }
			set
			{
				ColourFloat = value.ToVector4();
			}
		}
		/// <summary>
		/// Gets/Sets the colour of the text (as a <see cref="Vector4"/>)
		/// </summary>
		public Vector4 ColourFloat
		{
			get { return colour; }
			set
			{
				if (colour != value)
				{
					this.colour = value;
					if (this.sprite != null)
					{
						Vector4 v = value;
						for (int i = 0; i < this.sprite.InstanceCount; i++)
							this.sprite.SetSpriteColour(i, ref v);
					}
					this.SetDirty();
				}
			}
		}

		/// <summary></summary>
		protected override void SizeChanged()
		{
			textChangeIndex = -1;
		}

		/// <summary>
		/// Gets/Sets the vertical alignment of the text
		/// </summary>
		public VerticalAlignment TextVerticalAlignment
		{
			get { return textVAlign; }
			set { textVAlign = value; }
		}

		/// <summary>
		/// Gets/Sets the horizontal alignment of the text
		/// </summary>
		public TextHorizontalAlignment TextHorizontalAlignment
		{
			get { return textAlign; }
			set
			{
				if (textAlign != value)
				{
					textAlign = value;
					textChangeIndex = -1;
				}
			}
		}

		/// <summary>
		/// Construct the text element
		/// </summary>
		/// <param name="sizeInPixels"></param>
		public TextElementRect(Vector2 sizeInPixels)
			: this(sizeInPixels, (string)null, null)
		{
			this.text = new TextValue();
		}

		/// <summary>
		/// Construct the text element
		/// </summary>
		/// <param name="sizeInPixels"></param>
		/// <param name="font"></param>
		/// <param name="text"></param>
		public TextElementRect(Vector2 sizeInPixels, string text, SpriteFont font)
			: this(sizeInPixels, font)
		{
			this.text = new TextValue();

			if (text != null)
				this.text.Append(text);
		}

		/// <summary>
		/// Construct the text element
		/// </summary>
		/// <param name="sizeInPixels"></param>
		/// <param name="font"></param>
		/// <param name="text"></param>
		public TextElementRect(Vector2 sizeInPixels, TextValue text, SpriteFont font)
			: this(sizeInPixels, font)
		{
			this.text = text ?? new TextValue();
		}

		/// <summary>
		/// Construct the text element
		/// </summary>
		/// <param name="sizeInPixels"></param>
		/// <param name="text"></param>
		public TextElementRect(Vector2 sizeInPixels, string text)
			: this(sizeInPixels, text, null)
		{
		}
		/// <summary>
		/// Construct the text element
		/// </summary>
		/// <param name="sizeInPixels"></param>
		/// <param name="text"></param>
		public TextElementRect(Vector2 sizeInPixels, TextValue text)
			: this(sizeInPixels, text, null)
		{
		}

		private TextElementRect(Vector2 sizeInPixels, SpriteFont font)
			: base(sizeInPixels)
		{
			if (font != null)
				this.font = font;

			this.spriteFont = font;

			sprite = new SpriteElement(font == null ? null : this.font.textureValue);
			sprite.VerticalAlignment = VerticalAlignment.Top;
			this.VerticalAlignment = VerticalAlignment.Top;

			Add(sprite);

			this.children = new TextChildren(this.Children);

			sprite.AlphaBlendState = AlphaBlendState.Alpha;
		}


		/// <summary>
		/// <para>Gets/Sets the XNA <see cref="SpriteFont"/> used by ths element</para>
		/// <para>Note: The font must be set before drawing the text</para>
		/// </summary>
		public SpriteFont Font
		{
			get { return spriteFont; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("font");
				if (this.spriteFont != value)
				{
					this.font = value;
					this.spriteFont = value;
					this.textChangeIndex = -1;
				}
			}
		}

		/// <summary>
		/// Adds a child element that is embedded inline at the given text index
		/// </summary>
		/// <param name="element"></param>
		/// <param name="textIndex"></param>
		public void AddInline(Element element, uint textIndex)
		{
			Vector2 zero = new Vector2();
			AddInline(element, textIndex, ref zero);
		}
		/// <summary>
		/// Adds a child element that is embedded inline at the given text index
		/// </summary>
		public void AddInline(Element element, uint textIndex, Vector2 elementOffset)
		{
			AddInline(element, textIndex, ref elementOffset);
		}
		/// <summary>
		/// Adds a child element that is embedded inline at the given text index
		/// </summary>
		public void AddInline(Element element, uint textIndex, ref Vector2 elementOffset)
		{
			if (textIndex >= this.text.Length)
				throw new ArgumentException("textIndex >= this.Text.Length");

			SetParentToThis(element);
			this.children.AddElement(element, textIndex, ref elementOffset);

			this.SetDirty();
		}
		/// <summary>
		/// Tries to remove an element added with <see cref="AddInline(Element,uint)"/>
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public bool RemoveInline(Element element)
		{
			if (this.children.RemoveElement(element))
			{
				this.ResetParent(element);

				this.SetDirty();
				return true;
			}
			return false;
		}
		/// <summary>
		/// Tries to remove an element added with <see cref="AddInline(Element,uint)"/>
		/// </summary>
		public bool RemoveInlineAt(uint textIndex)
		{
			Element element;
			if (this.children.RemoveElementAt(textIndex, out element))
			{
				this.ResetParent(element);

				this.SetDirty();
				return true;
			}
			return false;
		}
		/// <summary>
		/// Remove all elements added with <see cref="AddInline(Element,uint)"/>
		/// </summary>
		public void RemoveInlineChildren()
		{
			if (this.children.Children.Count > 1)
				this.SetDirty();

			for (int i = 1; i < this.children.Children.Count; i++)
				this.ResetParent(this.children.Children[i]);
			this.children.RemoveAll();
		}


		/// <summary>
		/// <para>Gets the <see cref="TextValue"/> of this element (See remarks for assignment details)</para>
		/// </summary>
		/// <remarks>
		/// <para>The <see cref="TextValue"/> object allows appending of strings, integers, etc. It does not allow direct asignment.</para>
		/// <para>Therefore, it is safe to do the following:</para>
		/// <code>textElement.Text += "...";</code>
		/// <para>But you cannot do:</para>
		/// <code>textElement.Text = "...";</code>
		/// <para>To set the text value, call SetText():</para>
		/// <code>textElement.Text.SetText("...");</code>
		/// <para>Appending will not allocate memory like normal string appending, as the internal <see cref="TextValue"/> is appending a <see cref="StringBuilder"/></para>
		/// </remarks>
		public TextValue Text
		{
			get { return text; }
			set { if (value != text) throw new ArgumentException("Cannot reassign text value"); }
		}

		//this is rather complex :-)
		private void Build(Vector2 pixelArea)
		{
			if (font == null)
				throw new InvalidOperationException("TextElementRect.Font");

			sprite.SetTextureAndClear(font.textureValue);

			Vector2 position = new Vector2();
			bool flag = true;
			int index;
			int periodIndex;
			int beginWord = 0;
			int beginLine = 0;
			int beginWordIndex = 0;
			int beginWordLength = 0;
			int spriceCount = 0;
			int lastIndex = -1;
			float beginX = 0;
			if (!font.TryGetCharacterIndex('.', out periodIndex))
				periodIndex = -1;
			if (textAlign == TextHorizontalAlignment.Justified && spaces == null)
				spaces = new List<int>();

			StringBuilder text = this.text.value;

			uint insertedElementIndex = children.ResetInsertIndex();
			uint lineInsertedElementCount = 0;


			if (text.Length > 0 && pixelArea.X > 1)
			{
				for (int i = 0; i < text.Length; i++)
				{
					if (i == insertedElementIndex)
					{
						float width;

						//update the inserted element
						children.SetInsertedElementLayout(ref position, out width);
						position.X += width;
						insertedElementIndex = children.GetNextInsertedIndex();
					}

					char c = text[i];

					switch (c)
					{
						case '\r':
							break;

						case '\n':

							if (textAlign == TextHorizontalAlignment.Right ||
								textAlign == TextHorizontalAlignment.Centre)
							{
								Vector2 delta = new Vector2(pixelArea.X - position.X, 0);
								if (textAlign == TextHorizontalAlignment.Centre)
									delta.X *= 0.5f;

								delta.X = (float)Math.Floor(delta.X);

								for (int n = beginLine; n < sprite.InstanceCount; n++)
								{
									sprite.MoveSprite(n, ref delta);
								}
								children.MoveRange(ref delta, lineInsertedElementCount);
							}

							if (textAlign == TextHorizontalAlignment.Justified)
								spaces.Clear();

							flag = true;
							position.X = 0;
							position.Y -= font.lineSpacingF;
							beginWord = -1;
							beginX = 0;

							beginLine = sprite.InstanceCount;
							lineInsertedElementCount = children.GetInsertedCount();
							break;

						case '\t':
							beginWord = -1;
							if (font.TryGetCharacterIndex(' ', out index))
							{
								Vector3 kerning = font.kerning[index];
								float width = ((kerning.Y + kerning.Z)) * 4;

								position.X = (float)(Math.Floor(position.X / width) + 1) * width;

								if (textAlign == TextHorizontalAlignment.Justified)
									spaces.Add(sprite.InstanceCount);
							}
							break;

						default:
							if (c == ' ' && textAlign == TextHorizontalAlignment.Justified)
								spaces.Add(sprite.InstanceCount);

							if (beginWord == -1 || !(char.IsLetterOrDigit(c) || c == '_'))
							{
								beginWord = lastIndex;
								beginWordIndex = i;
								beginWordLength = spriceCount;
								beginX = position.X;
							}

							if (font.TryGetCharacterIndex(c, out index))
							{
								Vector3 kerning = font.kerning[index];
								if (flag)
								{
									kerning.X = Math.Max(kerning.X, 0f);
								}
								position.X += (kerning.X);

								Vector4 crop = font.croppingDataV4[index];

								if (!char.IsWhiteSpace(c) && position.X + font.glyphWidth[index] > pixelArea.X)
								{

									//wrap to new line
									bool tooLong = false;
									if (beginWord != -1)
									{
										//find out how long this word is...

										float length = 0;
										for (int n = i; n < text.Length; n++)
										{
											if (!(char.IsLetterOrDigit(text[n]) || text[n] == '_'))
												break;

											int nindex = 0;
											if (font.TryGetCharacterIndex(text[n], out nindex))
											{
												length += font.glyphWidth[nindex];

												if ((position.X - beginX + length) > pixelArea.X)
												{
													//word is too long to be broken onto multiple lines
													tooLong = true;
													break;
												}
											}
										}


										if (!tooLong)
										{
											sprite.RemoveLastSprite(spriceCount - beginWordLength);
											i = beginWordIndex;
											spriceCount = beginWordLength;
											position.X = beginX;
											if (textAlign == TextHorizontalAlignment.Justified &&
												spaces.Count > 0)
												spaces.RemoveAt(spaces.Count - 1);
										}
									}

									if (textAlign == TextHorizontalAlignment.Right ||
										textAlign == TextHorizontalAlignment.Centre)
									{
										Vector2 delta = new Vector2(pixelArea.X - position.X, 0);
										if (textAlign == TextHorizontalAlignment.Centre)
											delta.X *= 0.5f;

										delta.X = (float)Math.Floor(delta.X);
										delta.Y = (float)Math.Floor(delta.Y);

										for (int n = beginLine; n < sprite.InstanceCount; n++)
										{
											sprite.MoveSprite(n, ref delta);
										}
										children.MoveRange(ref delta, lineInsertedElementCount);
									}

									if (textAlign == TextHorizontalAlignment.Justified && spaces.Count > 0)
									{
										float gap = (pixelArea.X - position.X) / (spaces.Count);
										float deltaf = 0;
										Vector2 delta = new Vector2(0, 0);

										int spaceIndex;
										if (font.TryGetCharacterIndex(' ', out spaceIndex))
										{
											Vector3 spaceSize = font.kerning[spaceIndex];
											if (gap > ((spaceSize.Y + spaceSize.Z)) * 40)
												gap = 0;
										}

										spaceIndex = 0;
										for (int n = beginLine; n < sprite.InstanceCount; n++)
										{
											if (spaceIndex != spaces.Count &&
												n == spaces[spaceIndex])
											{
												spaceIndex++;
												deltaf += gap;
												delta.X = (float)Math.Floor(deltaf);
											}
											sprite.MoveSprite(n, ref delta);
											children.MoveRangeUptoIndex(ref delta, ref lineInsertedElementCount, n);
										}
										children.MoveRangeUptoIndex(ref delta, ref lineInsertedElementCount, i + 1);
										spaces.Clear();
									}

									beginLine = sprite.InstanceCount;
									position.X = Math.Max(kerning.X, 0f);
									position.Y -= font.lineSpacingF;
									beginX = 0;
									if (!char.IsWhiteSpace(text[i]))
										i--;
									lineInsertedElementCount = children.GetInsertedCount();
									continue;
								}

								Vector2 pos = position;

								if (!sizeToVertical && periodIndex != -1 &&
									-position.Y > pixelArea.Y - font.lineSpacingF * 2 &&
									position.X + font.glyphWidth[index] > pixelArea.X - 3 * font.glyphWidth[periodIndex])
								{
									if (pixelArea.X > 3 * font.glyphWidth[periodIndex])
									{
										//add an elipsis ...
										crop = font.croppingDataV4[periodIndex];
										kerning = font.kerning[periodIndex];
										pos.Y -= crop.Y + font.glyphSize[periodIndex].Y;
										pos.X += crop.X;

										for (int n = 0; n < 3; n++)
										{
											sprite.AddSprite(ref pos, ref font.glyphSize[periodIndex], ref colour, ref font.glyphDataV4[periodIndex]);
											spriceCount++;

											pos.X += kerning.X + kerning.Y + kerning.Z;
										}

										i = text.Length;
										break;
									}
								}

								pos.X += crop.X;
								pos.Y -= crop.Y + font.glyphSize[index].Y;


								lastIndex = sprite.AddSprite(ref pos, ref font.glyphSize[index], ref colour, ref font.glyphDataV4[index]);
								spriceCount++;

								flag = false;
								position.X += ((kerning.Y + kerning.Z));
							}
							break;
					}
				}
			}

			if (textAlign == TextHorizontalAlignment.Right ||
				textAlign == TextHorizontalAlignment.Centre)
			{
				Vector2 delta = new Vector2(pixelArea.X - position.X, 0);
				if (textAlign == TextHorizontalAlignment.Centre)
					delta.X = (float)Math.Floor(delta.X * 0.5f);

				delta.X = (float)Math.Floor(delta.X);

				for (int n = beginLine; n < sprite.InstanceCount; n++)
				{
					sprite.MoveSprite(n, ref delta);
				}
				children.MoveRange(ref delta, lineInsertedElementCount);
			}
			if (sizeToVertical)
			{
				this.Size = new Vector2(this.Size.X, font.lineSpacingF - position.Y);
			}
			else
			{
				if (textVAlign == VerticalAlignment.Centre ||
					textVAlign == VerticalAlignment.Bottom)
				{
					float height = pixelArea.Y - (font.lineSpacingF - position.Y);
					if (height > 0)
					{

						if (textVAlign == VerticalAlignment.Centre)
							height *= 0.5f;

						height = (float)Math.Floor(height);
						Vector2 delta = new Vector2(0, -height);

						delta.Y = (float)Math.Floor(delta.Y);

						for (int n = 0; n < sprite.InstanceCount; n++)
							sprite.MoveSprite(n, ref delta);
						children.MoveRange(ref delta, 0);
					}
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
		/// <param name="state"></param>
		protected sealed override void DrawElement(DrawState state)
		{
		}

		/// <summary></summary>
		/// <param name="size"></param>
		protected sealed override void PreDraw(Vector2 size)
		{
			if (size != this.pixelSize)
			{
				textChangeIndex = -1;
				this.pixelSize = size;
			}

			if (text.HasChanged(ref this.textChangeIndex))
				Build(pixelSize);
		}
	}

	/// <summary>
	/// wrapper on SpriteFont private members, also does some useful pre-calculations to make things faster later
	/// </summary>
	class ElementFont
	{
		public readonly int[] characterIndex;
		public readonly char[] characterMap;
		public readonly Rectangle[] croppingData;
		public readonly Vector4[] croppingDataV4;
		public readonly Rectangle[] glyphData;
		public readonly Vector4[] glyphDataV4;
		public readonly Vector2[] glyphSize;
		public readonly float[] glyphWidth;
		public readonly Vector3[] kerning;
		public readonly int lineSpacing;
		public readonly float lineSpacingF;
		public readonly Texture2D textureValue;

		static Dictionary<SpriteFont, ElementFont> mapping
			= new Dictionary<SpriteFont, ElementFont>();

		public static implicit operator ElementFont(SpriteFont font)
		{
			ElementFont val;
			if (!mapping.TryGetValue(font, out val))
			{
				val = new ElementFont(font);
				mapping.Add(font, val);
			}
			return val;
		}

		ElementFont(SpriteFont font)
		{
			if (font == null)
				throw new ArgumentNullException();

			//time to rip out all the useful bits

			List<char> _characterMap;
			List<Rectangle> _croppingData;
			List<Rectangle> _glyphData;
			List<Vector3> _kerning;
			Get(font, "characterMap", out _characterMap);
			Get(font, "croppingData", out _croppingData);
			Get(font, "glyphData", out _glyphData);
			Get(font, "kerning", out _kerning);
			Get(font, "lineSpacing", out lineSpacing);
			Get(font, "textureValue", out textureValue);

			this.characterMap = _characterMap.ToArray();
			this.croppingData = _croppingData.ToArray();
			this.glyphData = _glyphData.ToArray();
			this.kerning = _kerning.ToArray();

			glyphSize = new Vector2[glyphData.Length];
			glyphDataV4 = new Vector4[glyphData.Length];
			croppingDataV4 = new Vector4[croppingData.Length];
			glyphWidth = new float[kerning.Length];

			lineSpacingF = (float)lineSpacing;

			int maxCharacter = 0;
			for (int i = 0; i < this.characterMap.Length; i++)
				maxCharacter = Math.Max(maxCharacter, (int)characterMap[i]);

			characterIndex = new int[maxCharacter + 1];
			for (int i = 0; i < characterIndex.Length; i++)
			{
				characterIndex[i] = -1;
			}
			for (int i = 0; i < this.characterMap.Length; i++)
			{
				characterIndex[(int)characterMap[i]] = i;
				glyphDataV4[i] = new Vector4((float)glyphData[i].X, (float)glyphData[i].Y, (float)glyphData[i].Width, (float)glyphData[i].Height);
				croppingDataV4[i] = new Vector4((float)croppingData[i].X, (float)croppingData[i].Y, (float)croppingData[i].Width, (float)croppingData[i].Height);
				glyphSize[i] = new Vector2((float)glyphData[i].Width, (float)glyphData[i].Height);
				glyphWidth[i] = kerning[i].X + kerning[i].Y + kerning[i].Z;
			}
		}

		public bool TryGetCharacterIndex(char character, out int index)
		{
			index = (int)character;
			if (index >= 0 && index < characterIndex.Length)
			{
				index = characterIndex[index];
				return index >= 0;
			}
			else
			{
				index = 0;
				return false;
			}
		}

		static void Get<T>(SpriteFont font, string name, out T value)
		{
			value = (T)typeof(SpriteFont).GetField(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(font);
		}
	}
}
