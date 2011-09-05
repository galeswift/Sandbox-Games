using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xen.Graphics;

namespace Xen.Ex.Scene
{
	/// <summary>
	/// <para>A DrawPredicate uses a <see cref="OcclusionQuery"/> to cull drawing of a 'complex' object based on the number of pixels a 'predicate' object draws</para>
	/// <para>If the predicate object (usually simple bounding geometry) draws more than <see cref="MinimumPixelCount"/>, the (usually more complex) object will be dawn</para>
	/// </summary>
	/// r<remarks>
	/// <para>An example use of this class would be drawing complex geometry, using a much simpler drawable (say, a bounding box) as the predicate.</para>
	/// <para>When rendering, the predicate will be drawn every few frames (colour and depth writing will be disabled). While drawing the predicate, an occlusion query will be active.</para>
	/// <para>The occlusion query results are returned several frames later. The result is the number of pixels drawn (the number of pixels drawn by the predicate).</para>
	/// <para>If the number of pixels drawn is greater than <see cref="MinimumPixelCount"/>, then the complex object will be drawn.</para>
	/// <para>This means that if the predicate is occluded while rendering (NOTE: closer objects must still be drawn before it) then the complex object won't be rendered at all.</para>
	/// <para>Note, there are several requirements for a DrawPredicate to improve performance:<br/>The predicate must be drawn in order such that potential occluders are drawn first (eg, front to back).<br/>The main item must be complex enough, and occluded often enough such that the overhead of drawing the predicate is actually worth while. (If you are fill rate limited, a DrawPredicate will probably not help at all)<br/>Finally, the potential delay between the oclusion query result being returned may produce a delay where the object isn't drawn.</para>
	/// <para>Finally, note that when the predicate is <i>off screen</i> (as reported by it's cull test) the query result gets reset. When the predicate is on screen again, the query is assumed to have passed. This can be disabled by setting <see cref="OffScreenPredicateQueryReset"/> to false. When false, the complex object may be invisible for the first few frames when appearing from off screen. When true, when comming back on screen, the complex object may be drawn even though occluded. (The default is false)</para>
	/// </remarks>
	public sealed class DrawPredicate : IDraw, IDisposable
	{
		private readonly IDraw item, predicate;
		private OcclusionQuery query;
		private int frameIndex;
		private ushort pixelCount = 1, queryPixelCount = ushort.MaxValue;
		private bool queryInProgress;
		private bool supported = true;
		private bool resetOnCullChange = false;

		/// <summary>
		/// Construct the DrawPredicate
		/// </summary>
		/// <param name="complex">The 'complex' object that is drawn based on the occlusion of the predicate</param>
		/// <param name="predicate">The 'simple' predicate object</param>
		public DrawPredicate(IDraw complex, IDraw predicate)
		{
			if (complex == null ||
				predicate == null)
				throw new ArgumentNullException();

			this.item = complex;
			this.predicate = predicate;

			this.supported = Application.GetApplicationInstance().IsHiDefDevice;
		}

		/// <summary>
		/// Construct the DrawPredicate
		/// </summary>
		/// <param name="complex">The 'complex' object that is drawn based on the occlusion of the predicate</param>
		/// <param name="predicate">The 'simple' predicate object</param>
		/// <param name="minmumPixelCount">The minimum predicate pixel count required to draw the 'complex' object</param>
		public DrawPredicate(IDraw complex, IDraw predicate, int minmumPixelCount) : this(complex,predicate)
		{
			this.MinimumPixelCount = minmumPixelCount;
		}

		/// <summary>
		/// Construct the DrawPredicate
		/// </summary>
		/// <param name="complex">The 'complex' object that is drawn based on the occlusion of the predicate</param>
		/// <param name="predicate">The 'simple' predicate object</param>
		/// <param name="minmumPixelCount">The minimum predicate pixel count required to draw the 'complex' object</param>
		/// <param name="initialQueryVisibility">If false, the predicate will treat the query as having failed, this will require a positive query result before the object becomes visible</param>
		public DrawPredicate(IDraw complex, IDraw predicate, int minmumPixelCount, bool initialQueryVisibility)
			: this(complex, predicate, minmumPixelCount)
		{
			if (!initialQueryVisibility)
				this.queryPixelCount = 0;
		}

		/// <summary>
		/// The minimum predicate pixel count required to draw the 'complex' object
		/// </summary>
		public int MinimumPixelCount
		{
			get { return pixelCount; }
			set 
			{ 
				if (value < 1) 
					throw new ArgumentException("Value must be 1 or greater"); 
				pixelCount = (ushort)Math.Min(ushort.MaxValue,value); 
			}
		}

		/// <summary>
		/// If true, the occlusion query result will be reset when the predicate is off screen
		/// </summary>
		public bool OffScreenPredicateQueryReset
		{
			get { return resetOnCullChange; }
			set { resetOnCullChange = value; }
		}

		/// <summary>
		/// Draw the predicate, and if the predicate draws at least <see cref="MinimumPixelCount"/> pixels, the 'complex' object will be drawn
		/// </summary>
		/// <param name="state"></param>
		public void Draw(DrawState state)
		{
			int frame = state.Properties.FrameIndex;

			if (!supported || frame == frameIndex)
			{
				if (item.CullTest(state))
					item.Draw(state);
				return;
			}

			if (resetOnCullChange && frameIndex != frame - 1)
			{
				//XNA requires IsComplete to be checked.
				if (query != null)
					queryInProgress = query.IsComplete;

				queryInProgress = false;//reset the query
				queryPixelCount = ushort.MaxValue;
			}

			if (queryInProgress && query.IsComplete)
			{
				queryInProgress = false;
				this.queryPixelCount = (ushort)query.PixelCount;
			}

			if (!queryInProgress)
			{
				GraphicsDevice device = (GraphicsDevice)state;

				//run the query
				if (query == null)
				{
					query = new OcclusionQuery(device);
				}

				query.Begin();

				state.RenderState.Push();

				state.RenderState.CurrentDepthState.DepthWriteEnabled = false;
				state.RenderState.CurrentRasterState.ColourWriteMask = ColorWriteChannels.None;
				state.RenderState.CurrentBlendState = new AlphaBlendState();

				predicate.Draw(state);

				state.RenderState.Pop();

				query.End();

				queryInProgress = true;
			}

			frameIndex = frame;

			if (queryPixelCount >= pixelCount && item.CullTest(state))
				item.Draw(state);
		}

		/// <summary>
		/// Cull test the predicate
		/// </summary>
		/// <param name="culler"></param>
		/// <returns></returns>
		public bool CullTest(ICuller culler)
		{
			return predicate.CullTest(culler);
		}

		/// <summary>
		/// Dispose the <see cref="OcclusionQuery"/>
		/// </summary>
		public void Dispose()
		{
			if (query != null)
				query.Dispose();
			query = null;
		}
	}
}
