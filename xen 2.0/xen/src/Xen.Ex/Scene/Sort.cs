using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Xen.Ex.Scene
{
	/// <summary>
	/// <para>This class will draw a list of objects in sorted order, drawing them in either back-to-front or front-to-back order</para>
	/// <para>NOTE: Sorting order is based on the CullTests performed by the items. Items that do not perform a CullTest through an <see cref="ICuller"/> will not be sorted</para>
	/// </summary>
	public sealed class DepthDrawSorter : IDraw
	{
		struct Entry
		{
			public float addIndex;
			public IDraw item;
		}

		private DepthSortMode sortMode;
		private int itemCount, previousFrameVisibleCount;
		private float addCount;
		private Entry[] items = new Entry[32];
		private float[] depths = new float[32];
		private uint sortEvery, sortFrameIndex;
		private bool visible = true;

		//actually used as a post-culler here
		private PositionCalculatingPreCuller postCuller = new PositionCalculatingPreCuller();

		/// <summary></summary>
		/// <param name="sortMode">Sorting mode, either front to back, or back to front</param>
		public DepthDrawSorter(DepthSortMode sortMode)
		{
			this.sortMode = sortMode;
		}

		/// <summary>
		/// Gets/Sets if this sorter, and it's children, shall be drawn
		/// </summary>
		public bool Visible
		{
			get { return visible; }
			set { visible = value; }
		}

		/// <summary>
		/// Gets/Sets the sorting mode for this drawer
		/// </summary>
		public DepthSortMode SortMode
		{
			get { return sortMode; }
			set { sortMode = value; }
		}

		/// <summary>
		/// <para>Set this value to 5 to perform the sort once every 6 frames (sort is delayed by 5 frames)</para>
		/// <para>Set this value to 0 to sort every frame (default), etc.</para>
		/// <para>This can be used to reduce the CPU load of the sorter</para>
		/// </summary>
		public uint SortDelayFrameCount
		{
			get { return sortEvery; }
			set 
			{
				if (sortEvery != value)
				{
					sortEvery = value;
					sortFrameIndex = sortEvery;
				}
			}
		}

		/// <summary>
		/// Add an item to the sorter
		/// </summary>
		/// <param name="item"></param>
		public void Add(IDraw item)
		{
			if (item == null)
				throw new ArgumentNullException();

			if (itemCount == items.Length)
			{
				Array.Resize(ref items, items.Length * 2);
				Array.Resize(ref depths, depths.Length * 2);
			}

			items[itemCount++] = new Entry() { item = item, addIndex = this.addCount++ };

			sortFrameIndex = sortEvery;
		}

		/// <summary>
		/// Removes all items from the sorter
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < itemCount; i++)
				items[i].item = null;
			itemCount = 0;
		}

		/// <summary>
		/// <para>Removes an item from the sorter (Performs a linear search of the sorter to find the item)</para>
		/// </summary>
		/// <param name="item"></param>
		public bool Remove(IDraw item)
		{
			for (int i = 0; i < itemCount; i++)
			{
				if (items[i].item == item)
				{
					items[i].item = null;

					itemCount--;

					if (i != itemCount)
					{
						//swap in the end item
						items[i] = items[itemCount];
						items[itemCount].item = null;
					}

					sortFrameIndex = sortEvery;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Draw the items in sorted order
		/// </summary>
		/// <param name="state"></param>
		public void Draw(DrawState state)
		{
			if (itemCount == 0)
				return;

			sortFrameIndex++;

			//sortEvery so many frames...
			if (sortFrameIndex > sortEvery)
				sortFrameIndex = 0; // ok, time to sort
			else
			{
				//otherwise,
				//draw using the previous frame sorting...

				if (sortMode == DepthSortMode.FrontToBack)
				{
					for (int i = 0; i < previousFrameVisibleCount; i++)
					{
						IDraw item = items[i].item;
						if (item.CullTest(state))
							item.Draw(state);
					}
				}
				else
				{
					for (int i = previousFrameVisibleCount - 1; i >= 0; i--)
					{
						IDraw item = items[i].item;
						if (item.CullTest(state))
							item.Draw(state);
					}
				}
				int drawn = 0;
				//draw the rest...
				for (int i = previousFrameVisibleCount; i < this.itemCount; i++)
				{
					IDraw item = items[i].item;
					if (item.CullTest(state))
					{
						item.Draw(state);
						drawn++;
					}
				}
				return;
			}

			//sort and draw...

			ICuller culler = state;

			state.Cullers.PushPostCuller(postCuller);

			postCuller.BeginPreCullItem(state);

			int index = 0;
			int visible = 0;
			int backIndex = itemCount - 1;
			int backIndexCulled = itemCount - 1;
			int unsortedItems = 0;

			Vector3 position, camera, direction;

			state.Camera.GetCameraPosition(out camera);
			state.Camera.GetCameraViewDirection(out direction);

			for (int i = 0; i < itemCount; i++)
			{
				Entry item = items[index];

				postCuller.ResetPreCullItem();

				bool cullTest = item.item.CullTest(culler);

				if (cullTest && postCuller.TryGetPosition(out position))
				{
					//keep item in the list
					items[index] = items[visible];

					//centre of cull tests
					position.X -= camera.X;
					position.Y -= camera.Y;
					position.Z -= camera.Z;

					float depth = direction.X * position.X + direction.Y * position.Y + direction.Z * position.Z;

					if (depth > 0)
						depth = position.X * position.X + position.Y * position.Y + position.Z * position.Z;

					depths[visible] = depth;
					items[visible] = item;

					visible++;
					index++;
				}
				else
				{
					//swap the back culled element to this one, don't increment index
					items[index] = items[backIndexCulled];
					items[backIndexCulled] = item;
					backIndexCulled--;

					if (cullTest)
					{
						//as the last step, put this item at the very back.
						items[backIndexCulled + 1] = items[backIndex];
						items[backIndex] = item;
						depths[backIndex] = item.addIndex;

						backIndex--;
						unsortedItems++;
					}
				}
			}

			state.Cullers.PopPostCuller();

			if (unsortedItems > 0)
			{
				backIndex++;

				//due to the way the algorithm works, the unsorted list is usually ordered like so:
				//1,2,3,4,5,6,7,0

				//so put the last element first, and check if they are out of order

				float lastD = this.depths[this.itemCount - 1];
				Entry lastE = this.items[this.itemCount - 1];
				bool outOfOrder = lastD > this.depths[backIndex];

				for (int i = this.itemCount - 2; i >= backIndex; i--)
				{
					if (i != this.itemCount - 2)
						outOfOrder |= this.depths[i] > this.depths[i + 1];

					this.items[i+1] = this.items[i];
					this.depths[i+1] = this.depths[i];
				}
				this.depths[backIndex] = lastD;
				this.items[backIndex] = lastE;

				//draw the unsorted items in their add order (which was written to depths)
				//this sort won't be all that efficient
				if (outOfOrder)
					Array.Sort(this.depths, this.items, backIndex, unsortedItems);

				for (int i = 0; i < unsortedItems; i++)
					items[backIndex++].item.Draw(state);
			}

			if (visible > 0)
			{
				//if the frame hasn't changed, the items should already be in sorted order,
				//so this sort should be quick

				//test if the values are already sorted...
				float depth = this.depths[0];
				bool outOfOrder = false;
				for (int i = 1; i < visible; i++)
				{
					outOfOrder |= depths[i] < depth;
					depth = depths[i];
				}

				if (outOfOrder)
					Array.Sort(this.depths, this.items, 0, visible);

				if (sortMode == DepthSortMode.FrontToBack)
				{
					for (int i = 0; i < visible; i++)
						items[i].item.Draw(state);
				}
				else
				{
					for (int i = visible - 1; i >= 0; i--)
						items[i].item.Draw(state);
				}
			}
			previousFrameVisibleCount = visible;
		}

		bool ICullable.CullTest(ICuller culler)
		{
			return itemCount > 0 && visible;
		}
	}

	/// <summary>
	/// Sorting mode for a <see cref="DepthDrawSorter"/>
	/// </summary>
	public enum DepthSortMode
	{
		/// <summary>
		/// <para>Items are drawn back first, with the closest items drawn last</para>
		/// <para>Use this mode for effects such as alpha blending, that can produce different results when drawn in different orders</para>
		/// </summary>
		BackToFront,
		/// <summary>
		/// <para>Items are drawn front first, with the furthest items drawn last</para>
		/// <para>Use this mode for reducing overdraw, to improve performance of high-fill rate objects</para>
		/// </summary>
		FrontToBack
	}
}
