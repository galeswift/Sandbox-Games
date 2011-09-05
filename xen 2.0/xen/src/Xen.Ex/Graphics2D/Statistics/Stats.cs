using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Xen.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Xen.Ex.Graphics2D.Statistics
{
	sealed class GraphDrawer : ElementRect
	{
		private IVertices graph;
		private static string graphID = typeof(GraphDrawer).FullName + ".vertices";

		private readonly float[] values;
		private readonly Vector4[] graphData;
		private int index;
		private bool dirty;
		private float widthScale, maxValue, maxValueInv;
		private Color colour;
		private float minScale = 0;
		private readonly float goodValue;

		public const int MaxGraphSamples = 200;

		public GraphDrawer(int samples, Color colour, Vector2 size, float minScale, float goodValue) : base(size)
		{
			this.goodValue = goodValue;

			this.minScale = minScale;
			if (samples < 1 || samples > MaxGraphSamples)
				throw new ArgumentException("Samples must be 2 or greater and less than or equal to " + MaxGraphSamples);

			this.values = new float[samples];
			this.graphData = new Vector4[samples];

			this.widthScale = 1.0f / ((float)(samples));
			dirty = true;

			this.colour = colour;
			this.AlphaBlendState = AlphaBlendState.Additive;
		}
		
#if XBOX360
		static double log2 = Math.Log(2);
#endif


		public void SetValue(float value)
		{
			values[(index++) % values.Length] = value;
			dirty = true;

			float max = 0;
			for (int i = 0; i < values.Length; i++)
				max = Math.Max(max, values[i]);

			max = Math.Max(minScale, max);
#if XBOX360
			maxValue = (float)Math.Pow(2, Math.Ceiling(Math.Log(max * 1.25) / log2));
#else
			maxValue = (float)Math.Pow(2, Math.Ceiling(Math.Log(max * 1.25, 2)));
#endif
			maxValueInv = 1.0f / maxValue;
		}

		public float MaxValue
		{
			get { return maxValue; }
		}


		protected override IShader BindShader(DrawState state, bool maskOnly)
		{
			Xen.Ex.Graphics2D.Statistics.DrawGraphLine shader = state.GetShader<Xen.Ex.Graphics2D.Statistics.DrawGraphLine>();

			if (dirty)
			{
				float x = widthScale;
				for (int i = 0; i < graphData.Length; i++)
				{
					int index = (i + this.index) % graphData.Length;
					float good = 0;

					if (goodValue != 0)
					{
						good = values[index];
						good = (good - Math.Abs(goodValue)) / goodValue;
					}
					graphData[i] = new Vector4(x, values[index] * maxValueInv, 0, good);
					x += widthScale;
				}
				dirty = false;
			}

			shader.GraphLine = this.graphData;

			return shader;
		}

		protected override void DrawElement(DrawState state)
		{
			if (graph == null)
			{
				graph = state.Application.UserValues[graphID] as IVertices;

				if (graph == null)
				{
					float[] graphValues = new float[MaxGraphSamples];
					for (int i = 0; i < MaxGraphSamples; i++)
						graphValues[i] = (float)i;

					this.graph = Vertices<float>.CreateSingleElementVertices(graphValues, VertexElementUsage.Position, 0);
					state.Application.UserValues[graphID] = this.graph;
				}
			}

			graph.Draw(state, null, PrimitiveType.LineStrip, this.values.Length - 1,0,0);
		}

		public void ResetAllGraphValues()
		{
			for (int i = 0; i < values.Length; i++)
			{
				values[i] = 0;
			}
			dirty = true;
		}
	}

	/// <summary>
	/// An element that displays a graph with a title
	/// </summary>
	public class Graph : ElementRect
	{
		private GraphDrawer graph;
		private Vertices<VertexPositionColor> outline;
		private Vertices<VertexPositionColor> background;
		private TextElementRect title, value;
		private float previousValue;

		/// <summary>
		/// Gets/Sets the graph font
		/// </summary>
		public SpriteFont Font
		{
			get
			{
				return title.Font;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException();

				title.Font = value;
				this.value.Font = value;
			}
		}

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="samples"></param>
		/// <param name="fontHeight"></param>
		public Graph(string name, int width, int height, int samples, int fontHeight)
			: this(name, width, height, samples, fontHeight, 0, null, 0)
		{
		}
		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="samples"></param>
		/// <param name="fontHeight"></param>
		/// <param name="minScale"></param>
		public Graph(string name, int width, int height, int samples, int fontHeight, float minScale)
			: this(name, width, height, samples, fontHeight, minScale, null, 0)
		{
		}
		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="samples"></param>
		/// <param name="fontHeight"></param>
		/// <param name="font"></param>
		/// <param name="minScale"></param>
		/// <param name="goodValue">The value that determines the transition from green to red on the graph</param>
		public Graph(string name, int width, int height, int samples, int fontHeight, float minScale, SpriteFont font, float goodValue)
			: base(new Vector2((float)width, (float)height), false)
		{
			if (width < fontHeight ||
				height < fontHeight)
				throw new ArgumentException("size is too small for font size");

			graph = new GraphDrawer(samples, Color.Red, new Vector2(), minScale, goodValue);
			float edge = 0.25f;

			graph.VerticalScaling = ElementScaling.FillToParentPlusSize;
			graph.HorizontalScaling = ElementScaling.FillToParentPlusSize;
			graph.Position = new Vector2((float)(fontHeight) *edge, (float)(fontHeight));
			graph.Size = -graph.Position;

			title = new TextElementRect(Vector2.Zero, name,font);
			title.VerticalAlignment = VerticalAlignment.Top;
			title.HorizontalAlignment = HorizontalAlignment.Left;
			title.HorizontalScaling = ElementScaling.FillToParentPlusSize;
			title.VerticalScaling = ElementScaling.FillToParentPlusSize;
			title.TextHorizontalAlignment = TextHorizontalAlignment.Centre;
			title.TextVerticalAlignment = VerticalAlignment.Top;
			title.Size = new Vector2(0, 0);

			value = new TextElementRect(Vector2.Zero, "0",font);
			value.VerticalAlignment = VerticalAlignment.Bottom;
			value.HorizontalAlignment = HorizontalAlignment.Left;
			value.HorizontalScaling = ElementScaling.FillToParentPlusSize;
			value.VerticalScaling = ElementScaling.FillToParentPlusSize;
			value.TextHorizontalAlignment = TextHorizontalAlignment.Right;
			value.TextVerticalAlignment = VerticalAlignment.Bottom;
			value.Size = new Vector2(0, 0);
			value.Position = new Vector2(-10, 0);

			this.Add(title);
			this.Add(value);
			this.Add(graph);

			float x = fontHeight / ((float)width);
			float y = fontHeight / ((float)height);

			this.AlphaBlendState = AlphaBlendState.Alpha;

			VertexPositionColor[] verts = new VertexPositionColor[]
			{
				new VertexPositionColor(new Vector3(x*edge,y,0),Color.White),
				new VertexPositionColor(new Vector3(1-x*edge,y,0),Color.White),
				new VertexPositionColor(new Vector3(x*edge,y,0),Color.White),
				new VertexPositionColor(new Vector3(x*edge,1-y,0),Color.White),
				
				new VertexPositionColor(new Vector3(edge*x,y + 0.25f * (1-y*2),0),new Color(255,255,255,16)),
				new VertexPositionColor(new Vector3(1-x*edge,y + 0.25f * (1-y*2),0),new Color(255,255,255,16)),
				
				new VertexPositionColor(new Vector3(edge*x,y + 0.5f * (1-y*2),0),new Color(255,255,255,16)),
				new VertexPositionColor(new Vector3(1-x*edge,y + 0.5f * (1-y*2),0),new Color(255,255,255,16)),
				
				new VertexPositionColor(new Vector3(edge*x,y + 0.75f * (1-y*2),0),new Color(255,255,255,16)),
				new VertexPositionColor(new Vector3(1-x*edge,y + 0.75f * (1-y*2),0),new Color(255,255,255,16)),
				
				new VertexPositionColor(new Vector3(edge*x,y,0),new Color(255,255,255,16)),
				new VertexPositionColor(new Vector3(1-x*edge,y,0),new Color(255,255,255,16)),
				
			};

			outline = new Vertices<VertexPositionColor>(verts);

			background = new Vertices<VertexPositionColor>(new VertexPositionColor[]
				{
					new VertexPositionColor(new Vector3(0,0,0),new Color(0,0,0,200)),
					new VertexPositionColor(new Vector3(0,1,0),new Color(0,0,0,200)),
					new VertexPositionColor(new Vector3(1,0,0),new Color(0,0,0,200)),
					new VertexPositionColor(new Vector3(1,1,0),new Color(0,0,0,200)),
				});
		}

		/// <summary></summary>
		public void ResetAllGraphValues()
		{
			this.graph.ResetAllGraphValues();
		}

		/// <summary>Set the next value in the graph</summary>
		/// <param name="value"></param>
		public void SetGraphValue(float value)
		{
			if (value != previousValue)
			{
				this.value.Text.Clear();

				if (value == 0)
				{
					this.value.Text.Append('0');
				}
				else
				{
					//really don't want to allocate here, even though it's tiny, this graph may be used to show GC activity :P
				
					//display number as ###,###,### making it easier to read big numbers

					//unfortunately, this allocates:
					//this.value.Text.Append(((int)value).ToString("N0"));

					if (value < 0)
						this.value.Text.Append('-');
					
					int i = (int)value;

					int thousands = 1;
					while (Math.Abs(i) >= thousands * 1000 && thousands != 1000000000)
						thousands *= 1000;

					this.value.Text.Append(i / thousands);
					i = Math.Abs(i);

					while (thousands != 1)
					{
						this.value.Text.Append(',');
						int range = i / thousands;
						i -= range * thousands;

						thousands /= 1000;

						range = i / thousands;
						if (range < 10)
							this.value.Text.Append("00");
						else if (range < 100)
							this.value.Text.Append("0");
						this.value.Text.Append(range);
					}

					if (value <= 999 && value >= -999)
					{
						int remainder = (int)(value * 1000) - i * 1000;
						if (remainder != 0)
						{
							this.value.Text.Append('.');
							if (remainder < 10)
								this.value.Text.Append("00");
							else if (remainder < 100)
								this.value.Text.Append("0");
							this.value.Text.Append(remainder);
						}
					}
				}
			}
			this.graph.SetValue(value);
			previousValue = value;
		}

		/// <summary></summary>
		/// <param name="size"></param>
		protected override void PreDraw(Vector2 size)
		{
		}
		/// <summary></summary>
		/// <param name="state"></param>
		/// <param name="maskOnly"></param>
		protected override IShader BindShader(DrawState state, bool maskOnly)
		{
			return state.GetShader<Xen.Ex.Shaders.FillVertexColour>();
		}
		/// <summary></summary>
		/// <param name="state"></param>
		protected override void DrawElement(DrawState state)
		{
			background.Draw(state, null, PrimitiveType.TriangleStrip);
			outline.Draw(state, null, PrimitiveType.LineList);
		}
	}

	
#if XBOX360
	//totally hacky way of working out cpu usage
	sealed class ThreadActivity
	{
		private System.Threading.Thread thread;
		private float usage = 1;
		private bool active = false;
		System.Diagnostics.Stopwatch timer;
		System.Threading.ManualResetEvent block = new System.Threading.ManualResetEvent(false); 

		public float Usage
		{
			get { return usage; }
		}

		private static int[] threadIds = new int[] { 1, 3, 4, 5 };
		private int[] affinity;

		public ThreadActivity(int index)
		{
			this.affinity = new int[] { threadIds[index] };

			this.thread = new System.Threading.Thread(Process);
			this.thread.IsBackground = true;
			this.thread.Start();
		}

		public void Start()
		{
			block.Set();
			active = true;
			usage = 0;
		}

		public void Stop()
		{
			block.Reset();
			active = false;
		}

		void Process()
		{
			if (timer == null)
				timer = new System.Diagnostics.Stopwatch();
			this.thread.SetProcessorAffinity(affinity);

			timer.Reset();
			timer.Start();
			int iterations = 0;

			const int samples = 5000;
			TimeSpan sleep = new TimeSpan(TimeSpan.TicksPerSecond / samples);

			while (true)
			{
				block.WaitOne();

				while (active)
				{
					if (timer.ElapsedTicks > UpdateState.TicksInOneSecond)
					{
						timer.Reset();

						// 1 second passed. see how many iterations.
						// if the thread is not being used, it'll be close to 'samples'.
						// otherwise, it'll be a lot lower.
						float use = 1 - (float)(iterations) * (1.0f / (float)samples);
						if (use < 0.01f)
							use = 0;
						this.usage = use;

						iterations = 0;
						timer.Start();
					}
					else
					{
						iterations++;

						System.Threading.Thread.Sleep(sleep);	//sleep 1ms
					}
				}
			}
		}
	}
#endif


	/// <summary>
	/// <para>[Debug mode only] Displays various statistics gathered from the previous rendered frame</para>
	/// </summary>
	public sealed class DrawStatisticsDisplay : IDraw, IUpdate
	{
#if DEBUG
		private delegate float Call(ref DrawStatistics stats, DrawState state);
		private Graph[] graphs;
		private Call[] setGraphCalls;
		private DrawStatistics previousFrameOverhead;
		private bool enabled, keyState;
		private TextElement toggleText, toggleTextGamepad, toggleTextDisplay;
		private WeakReference garbageTracker = new WeakReference(null);
		
		private OcclusionQuery fillRateQuery;
		private bool fillRateQueryActive;
		private float pixelsFillled;

#if XBOX360
		ThreadActivity[] threads = new ThreadActivity[4]
			{
				new ThreadActivity(0),
				new ThreadActivity(1),
				new ThreadActivity(2),
				new ThreadActivity(3),
			};
#endif
#endif
		private SpriteFont font;
		private Keys debugKey = Keys.F12;
		
		/// <summary>
		/// Gets/Sets the key used to toggle the graphs
		/// </summary>
		public Keys DebugToggleKey
		{
			get { return debugKey; }
			set 
			{
				debugKey = value;
#if DEBUG
				toggleText.Text.SetText(string.Format("Press {0} to toggle debug graphs", debugKey.ToString()));
#endif
			}
		}

		/// <summary>Construct the statistics display</summary>
		/// <param name="manager"></param>
		public DrawStatisticsDisplay(UpdateManager manager) : this(manager,null)
		{
		}

		/// <summary>Construct the statistics display</summary>
		/// <param name="manager"></param>
		/// <param name="font"></param>
		public DrawStatisticsDisplay(UpdateManager manager, SpriteFont font)
		{
			this.font = font;

#if DEBUG
			if (manager != null)
				manager.Add(this);

			toggleText = new TextElement("", font);
			toggleText.HorizontalAlignment = HorizontalAlignment.Left;
			toggleText.VerticalAlignment = VerticalAlignment.Top;

			toggleTextGamepad = new TextElement("Hold both thumbsticks to toggle debug graphs", font);
			toggleTextGamepad.HorizontalAlignment = HorizontalAlignment.Left;
			toggleTextGamepad.VerticalAlignment = VerticalAlignment.Top;

			DebugToggleKey = Keys.F12;
#endif
		}

		/// <summary>
		/// Gets/Sets the font used by the display
		/// </summary>
		public SpriteFont Font
		{
			get { return font; }
			set 
			{
				if (value == null)
					throw new ArgumentNullException();

				if (this.font != value)
				{
					this.font = value;
					
#if DEBUG
					toggleText.Font = font;
					toggleTextGamepad.Font = font;

					if (graphs != null)
						for (int i = 0; i < graphs.Length; i++)
							graphs[i].Font = (value);
#endif
				}
			}
		}

		static float Avg(int total, int count)
		{
			if (count == 0)
				return 0;
			return ((float)total) / ((float)count);
		}

		delegate T Func<A, B, C, D, T>(A a, B b, C c, D d);

		/// <summary>
		/// Draw the statistics
		/// </summary>
		/// <param name="state"></param>
		public void Draw(DrawState state)
		{
#if DEBUG


			float fade = 8 - state.TotalTimeSeconds * 0.5f;
			if (fade > 1)
				fade = 1;
			if (fade < 0)
				fade = 0;


			if (!enabled)
			{
				if (toggleTextDisplay != null && fade > 0)
				{
					this.toggleTextDisplay.ColourFloat = new Vector4(1, 1, 1, fade);
					toggleTextDisplay.Draw(state);

					AlignElements(state);
				}
				return;
			}


			if (state.Application.IsHiDefDevice)
			{
				if (fillRateQuery == null)
				{
					GraphicsDevice device = (GraphicsDevice)state;
					fillRateQuery = new OcclusionQuery(device);

					fillRateQuery.Begin();
					fillRateQueryActive = true;
				}

				if (fillRateQueryActive)
				{
					fillRateQuery.End();
					fillRateQueryActive = false;
				}
			}

			DrawStatistics stats;
			state.GetPreviousFrameStatistics(out stats);

			stats -= previousFrameOverhead;

			if (graphs == null)
			{
				const int width = 210;
				const int height = 128;
				const int fontPix = 20;
				List<Call> calls = new List<Call>();

				Func<string, Call, float, Graph> add = delegate(string name, Call call, float good) { calls.Add(call); return new Graph(name, width, height, width - fontPix / 2, fontPix, 0, font, -good); };
				Func<string, Call, float, Graph> addHalf = delegate(string name, Call call, float good) { calls.Add(call); return new Graph(name, width / 2, height, width / 2 - fontPix / 2, fontPix, 0, font, -good); };
				Func<string, Call, float, Graph> addHalfMin1 = delegate(string name, Call call, float good) { calls.Add(call); return new Graph(name, width / 2, height, width / 2 - fontPix / 2, fontPix, 1, font, -good); };

				graphs = new Graph[]
				{
					add("Frame Rate (Approx)",delegate(ref DrawStatistics s, DrawState dstate)
						{return (float)dstate.ApproximateFrameRate;}, -20),
					
#if XBOX360
					addHalf("Primitives Drawn",delegate(ref DrawStatistics s, DrawState dstate)
					    {return (float)(s.TrianglesDrawn+s.LinesDrawn+s.PointsDrawn);}, 1000000),

					addHalf("Pixels Drawn\n(Approx)",delegate(ref DrawStatistics s, DrawState dstate)
					    {return Math.Max(0,pixelsFillled - (float)s.XboxPixelFillBias);}, 20000000), // not accurate
#else
			
					add("Primitives Drawn",delegate(ref DrawStatistics s, DrawState dstate)
					    {return (float)(s.TrianglesDrawn+s.LinesDrawn+s.PointsDrawn);}, 1000000),

					addHalf("Pixels Drawn",delegate(ref DrawStatistics s, DrawState dstate)
					    {return Math.Max(0,pixelsFillled);}, 18000000),
#endif
					
					addHalf("Draw Calls",delegate(ref DrawStatistics s, DrawState dstate)
					    {return (float)(s.DrawIndexedPrimitiveCallCount + s.DrawPrimitivesCallCount);}, 300),
					
					addHalf("Garbage Collected",delegate(ref DrawStatistics s, DrawState dstate)
					    {
							if (garbageTracker.Target == null) { garbageTracker.Target = new object(); return 1; }
							return 0;
						},0),
						
					addHalf("Allocated Bytes (Managed)",delegate(ref DrawStatistics s, DrawState dstate)
					    {
							return (int)GC.GetTotalMemory(false);
						},0),
					
#if XBOX360
					
					addHalfMin1("CPU Usage\n(Primary)",delegate(ref DrawStatistics s, DrawState dstate)
					    {return threads[0].Usage;},-0.5f),
					addHalfMin1("CPU Usage\n(Task Threads)",delegate(ref DrawStatistics s, DrawState dstate)
					    {return (threads[1].Usage+threads[2].Usage+threads[3].Usage) / 3.0f;},-0.25f),
#endif
						
				};

				this.setGraphCalls = calls.ToArray();
			}

			for (int i = 0; i < graphs.Length; i++)
			{
				graphs[i].SetGraphValue(setGraphCalls[i](ref stats, state));
				graphs[i].Visible = true;
			}

			AlignElements(state);


			DrawStatistics currentPreDraw;
			state.GetCurrentFrameStatistics(out currentPreDraw);


			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i].Visible)
					graphs[i].Draw(state);
			}


			DrawStatistics currentPostDraw;
			state.GetCurrentFrameStatistics(out currentPostDraw);

			previousFrameOverhead = currentPostDraw - currentPreDraw;


			if (state.Application.IsHiDefDevice)
			{
				if (fillRateQuery.IsComplete)
				{
					pixelsFillled = (float)fillRateQuery.PixelCount;

					fillRateQuery.Begin();
					fillRateQueryActive = true;
				}
			}
			else
				pixelsFillled = -1;
#endif
		}
		
#if DEBUG

		private void AlignElements(DrawState state)
		{
			if (state.DrawTarget == null)
				return;

			float overscanPixels = 0;

#if XBOX360
			overscanPixels = (float)state.DrawTarget.Width * 0.1f;
#else
			overscanPixels = (float)state.DrawTarget.Width * 0.05f;
#endif

			toggleText.Position = new Vector2((float)Math.Floor(overscanPixels), (float)Math.Floor(-overscanPixels));
			toggleTextGamepad.Position = new Vector2((float)Math.Floor(overscanPixels), (float)Math.Floor(-overscanPixels));

			if (!enabled)
				return;

			Vector2 size = state.DrawTarget.Size;

			size.X -= overscanPixels;

			float x = (float)Math.Floor(overscanPixels), y = (float)Math.Floor(-overscanPixels), height = 0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i].Visible == false)
					continue;

				if (x + graphs[i].Size.X > size.X)
				{
					y -= height+2;
					x = overscanPixels;
					height = 0;
				}

				graphs[i].HorizontalAlignment = HorizontalAlignment.Left;
				graphs[i].VerticalAlignment = VerticalAlignment.Top;
				graphs[i].Position = new Vector2(x, y);

				height = Math.Max(height, graphs[i].Size.Y);
				x += graphs[i].Size.X+2;
			}
		}
#endif

		bool ICullable.CullTest(ICuller culler)
		{
#if DEBUG
			return true;
#else
			return false;
#endif
		}

		/// <summary>
		/// Toggle the graphs on/off
		/// </summary>
		public void ToggleGraphs(Application application)
		{
#if DEBUG
			this.enabled = !enabled;
			if (enabled)
			{
				previousFrameOverhead = new DrawStatistics();
				if (graphs != null)
					foreach (Graph g in graphs)
						g.ResetAllGraphValues();
				garbageTracker.Target = new object();
			}

			if (application.IsHiDefDevice && fillRateQueryActive)
			{
				fillRateQuery.End();
				fillRateQueryActive = false;
			}

#if XBOX360

			if (enabled)
			{
				for (int i = 0; i < threads.Length; i++)
					threads[i].Start();
			}
			else
			{
				for (int i = 0; i < threads.Length; i++)
					threads[i].Stop();
			}

#endif
#endif
		}

		UpdateFrequency IUpdate.Update(UpdateState state)
		{
#if DEBUG
			bool kstate = false;
#if !XBOX
			if (state.PlayerInput[0].ControlInput == Xen.Input.ControlInput.KeyboardMouse)
			{
				kstate = state.KeyboardState.KeyState[debugKey];
				toggleTextDisplay = toggleText;
			}
			else
#endif
			{
				bool held = state.PlayerInput[0].InputState.Buttons.LeftStickClick.DownDuration > 0.5f && state.PlayerInput[0].InputState.Buttons.RightStickClick.DownDuration > 0.5f;

				kstate = !held;
				toggleTextDisplay = toggleTextGamepad;
			}

			if (!kstate && this.keyState)
			{
				ToggleGraphs(state.Application);
			}
			this.keyState = kstate;

			return UpdateFrequency.FullUpdate60hz;
#else
			return UpdateFrequency.Terminate;
#endif
		}
	}
}
