using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Xen.Ex.Scene
{
	/// <summary>
	/// <para>Stores a list of IDraw instances in a scene partitioning binary tree, this clas can be used to improve CullTest performance for large numbers of objects.</para>
	/// <para>NOTE: Instances are assumed to not be moving (the must be static)</para>
	/// </summary>
	public sealed class StaticBinaryTreePartition : StaticPartition
	{
		//bit shift stored...
		//so 3 == 8
		const int ChildCountShift = 3;
		const int ChildCount = 1 << ChildCountShift;

		private readonly float[] minBuffer = new float[4], maxBuffer = new float[4];
		private Vector3 boundsMin, boundsMax;
		private int count = 0;
		private int childIndex = 0;
		private IDraw[] allChildren = new IDraw[4*ChildCount];
		private BinaryNode[] allNodes = new BinaryNode[4];
		//stores current node index and desired node index when reordering data
		private ushort[] nodeChildIndex, nodeChildSwapIndex;
		private ushort nodeCount = 1;

		private ThreadDrawer[] threads;
		private int threadLevel;

		/// <summary></summary>
		/// <param name="item"></param>
		/// <param name="minBounds"></param>
		/// <param name="maxBounds"></param>
		protected override void AddItem(IDraw item, ref Vector3 minBounds, ref Vector3 maxBounds)
		{
			minBuffer[0] = minBounds.X;
			minBuffer[1] = minBounds.Y;
			minBuffer[2] = minBounds.Z;
			
			maxBuffer[0] = maxBounds.X;
			maxBuffer[1] = maxBounds.Y;
			maxBuffer[2] = maxBounds.Z;

			boundsMin.X = Math.Min(boundsMin.X, minBounds.X);
			boundsMin.Y = Math.Min(boundsMin.Y, minBounds.Y);
			boundsMin.Z = Math.Min(boundsMin.Z, minBounds.Z);

			boundsMax.X = Math.Max(boundsMax.X, maxBounds.X);
			boundsMax.Y = Math.Max(boundsMax.Y, maxBounds.Y);
			boundsMax.Z = Math.Max(boundsMax.Z, maxBounds.Z);

			count++;
			allNodes[0].Add(item, minBuffer, maxBuffer, 0, this, 0);
		}

		/// <summary></summary>
		/// <param name="state"></param>
		protected override void DrawItems(DrawState state)
		{
			//draw all items in the tree
			minBuffer[0] = boundsMin.X;
			minBuffer[1] = boundsMin.Y;
			minBuffer[2] = boundsMin.Z;

			maxBuffer[0] = boundsMax.X;
			maxBuffer[1] = boundsMax.Y;
			maxBuffer[2] = boundsMax.Z;

#if !XBOX360
			if (this.count > 64 && state.Application.ThreadPool.ThreadCount > 0)
			{
				//if there are lots of items and free threads, then traverse the tree on multiple threads
				DrawItemsThread(state);
				return;
			}
#endif

			Matrix world;
			state.WorldMatrix.GetMatrix(out world);

			allNodes[0].Draw(state, minBuffer, maxBuffer, 0, allChildren, allNodes, world == Matrix.Identity, this.IsOptimizedState);
		}


		//traverse the tree on multiple threads...
		private void DrawItemsThread(DrawState state)
		{
			minBuffer[0] = boundsMin.X;
			minBuffer[1] = boundsMin.Y;
			minBuffer[2] = boundsMin.Z;

			maxBuffer[0] = boundsMax.X;
			maxBuffer[1] = boundsMax.Y;
			maxBuffer[2] = boundsMax.Z;

			Threading.ThreadPool pool = state.Application.ThreadPool;

			if (this.threads == null)
			{
				//create the threads.
				//must be a power-of-two number of thread tasks
				threadLevel = 0;
				int threadCount = 2;
				while (pool.ThreadCount >= threadCount)
				{
					threadCount *= 2;
					threadLevel++;
				}

				this.threads = new ThreadDrawer[threadCount];
				for (int i = 0; i < threadCount; i++)
				{
					this.threads[i] = new ThreadDrawer();
					this.threads[i].axis = threadLevel % 3;
				}
			}
			
			Matrix world;
			state.WorldMatrix.GetMatrix(out world);
			bool isIdentity = world == Matrix.Identity;

			for (int i = 0; i < this.threads.Length; i++)
			{
				this.threads[i].children = this.allChildren;
				this.threads[i].nodes = this.allNodes;
				this.threads[i].idenityMatrix = isIdentity;

				this.threads[i].cullTestInstanceCount = 0;
				this.threads[i].instanceCount = 0;

				this.threads[i].treeIsOptimized = this.IsOptimizedState;
			}

			int index = 0;

			//traverse the tree, when getting to 'threadLevel' child depth, process on a thread
			DrawItemsThread(state, 0, 0, ref index,minBuffer,maxBuffer);

			//now wait for everything to finish...
			for (int i = 0; i < this.threads.Length; i++)
				threads[i].callback.WaitForCompletion();

			//iterate through the items to draw
			for (int t = 0; t < this.threads.Length; t++)
			{
				//some do not require cull tests
				if (this.IsOptimizedState)
				{
					for (int i = 0; i < threads[t].instanceCount; i++)
					{
						ThreadDrawnInstance inst = threads[t].instances[i];

						uint child = inst.firstChild;
						child <<= ChildCountShift;

						for (ushort c = 0; c < inst.childCount; c++)
						{
							IDraw item = allChildren[child++];
							if (item != null)
								item.Draw(state);
						}
					}
				}
				else
				{
					for (int i = 0; i < threads[t].instanceCount; i++)
					{
						ThreadDrawnInstance inst = threads[t].instances[i];

						uint child = inst.firstChild;
						child <<= ChildCountShift;

						for (ushort c = 0; c < inst.childCount; c++)
							allChildren[child++].Draw(state);
					}
				}

				//some do.
				for (int i = 0; i < threads[t].cullTestInstanceCount; i++)
				{
					ThreadDrawnInstance inst = threads[t].cullTestInstances[i];

					uint child = inst.firstChild;
					child <<= ChildCountShift;

					for (ushort c = 0; c < inst.childCount; c++)
					{
						if (allChildren[child].CullTest(state))
							allChildren[child].Draw(state);
						child++;
					}
				}
			}
		}

		//similar to a normal draw, but stops when depth == threadLevel, and then runs a thread process from there
		private void DrawItemsThread(DrawState state, ushort node, int depth, ref int index, float[] boundsMin, float[] boundsMax)
		{
			boundsMin[depth % 3] = allNodes[node].min;
			boundsMax[depth % 3] = allNodes[node].max;

			Vector3 localMin = new Vector3(), localMax = new Vector3();

			localMin.X = boundsMin[0];
			localMin.Y = boundsMin[1];
			localMin.Z = boundsMin[2];

			localMax.X = boundsMax[0];
			localMax.Y = boundsMax[1];
			localMax.Z = boundsMax[2];
		
			ContainmentType type;

			if (!threads[0].idenityMatrix)
				type = (state as ICuller).IntersectBox(ref localMin, ref localMax);
			else
				type = (state as ICuller).IntersectWorldBox(ref localMin, ref localMax);

			switch (type)
			{
			case ContainmentType.Contains:
				allNodes[node].Draw(state, allChildren, allNodes, IsOptimizedState);
				break;
			case ContainmentType.Intersects:
				if (allNodes[node].left == 0 || depth == threadLevel)
				{
					ThreadDrawer thread = this.threads[index++];
					for (int i = 0; i < 3; i++)
					{
						thread.minBuffer[i] = boundsMin[i];
						thread.maxBuffer[i] = boundsMax[i];
					}

					//carry on from here on a thread task
					thread.startIndex = node;
					thread.callback = state.Application.ThreadPool.QueueTask(thread, state);
				}
				else
				{

					DrawItemsThread(state, allNodes[node].left, depth + 1, ref index, boundsMin, boundsMax);

					boundsMin[0] = localMin.X;
					boundsMin[1] = localMin.Y;
					boundsMin[2] = localMin.Z;

					boundsMax[0] = localMax.X;
					boundsMax[1] = localMax.Y;
					boundsMax[2] = localMax.Z;


					DrawItemsThread(state, allNodes[node].right, depth + 1, ref index, boundsMin, boundsMax);

					boundsMin[0] = localMin.X;
					boundsMin[1] = localMin.Y;
					boundsMin[2] = localMin.Z;

					boundsMax[0] = localMax.X;
					boundsMax[1] = localMax.Y;
					boundsMax[2] = localMax.Z;
				}
				break;
			}
		}

		/// <summary></summary>
		protected override bool CullTestItems(ICuller culler)
		{
			return count > 0 && culler.TestBox(ref boundsMin, ref boundsMax);
		}

		/// <summary>
		/// Query the partition, returning all instances that intersect the primitive
		/// </summary>
		/// <param name="queryShape"></param>
		/// <param name="resultCallback">Callback for results. Return true to continue the query</param>
		protected override bool RunQuery(ICullPrimitive queryShape, Func<IDraw, bool> resultCallback)
		{
			minBuffer[0] = this.boundsMin.X;
			minBuffer[1] = this.boundsMin.Y;
			minBuffer[2] = this.boundsMin.Z;

			maxBuffer[0] = this.boundsMax.X;
			maxBuffer[1] = this.boundsMax.Y;
			maxBuffer[2] = this.boundsMax.Z;

			return allNodes[0].Query(queryShape, resultCallback, minBuffer, maxBuffer, 0, this.allChildren, this.allNodes);
		}


		//remove children from a node, then add them back to the main list (to be readded next frame)
		private void ReAddChildren(ushort firstChild)
		{
			uint child = firstChild;
			child <<= ChildCountShift;

			for (int i = 0; i < ChildCount; i++)
			{
				if (allChildren[child] != null)
				{
					Add(allChildren[child]);
					allChildren[child] = null;
					count--;
				}
				child++;
			}
		}

		private ushort GetNewChildArrayBaseIndex()
		{
			childIndex += ChildCount;

			if (childIndex + ChildCount > this.allChildren.Length)
				Array.Resize(ref this.allChildren, this.allChildren.Length * 2);

			return (ushort)(childIndex>>ChildCountShift);
		}

		private ushort GetNewNodeIndex(ushort firstChild)
		{
			if (nodeCount == this.allNodes.Length)
				Array.Resize(ref this.allNodes, this.allNodes.Length * 2);

			this.allNodes[nodeCount] = new BinaryNode(firstChild);

			if (nodeCount == ushort.MaxValue)
				throw new OverflowException();

			return nodeCount++;
		}

		/// <summary></summary>
		protected override void OptimizeContents()
		{
			//each node leaf stores an index into the allChildren, and a child count
			//this method runs through the tree, and orders the children stored in allChildren
			//so they are in the same order the tree would logically be traversed.
			//this way, when a non-leaf node is fully visible, it can loop through all it's 
			//children's drawable objects - without having to recurse into the tree.

			int allocCount = 2;
			while (this.nodeCount >= allocCount)
				allocCount *= 2;
			allocCount /= 2;

			if (nodeChildIndex == null ||
				nodeChildIndex.Length < allocCount)
			{
				Array.Resize(ref nodeChildIndex, allocCount);
				Array.Resize(ref nodeChildSwapIndex, allocCount);
			}

			for (ushort i = 0; i < this.nodeCount; i++)
			{
				if (this.allNodes[i].left == 0)
					nodeChildIndex[allNodes[i].firstChild] = i;
			}
			
			int order = 0;
			ushort min = ushort.MaxValue, max = 0;
			StoreLogicalOrder(0, ref order, ref min, ref max);

			//every second value in nodeChildIndex will now store the logical order, the first is the leaf order

			for (int i = 0; i < order; i++)
			{
				//swap into the correct ordering

				ushort target = nodeChildSwapIndex[i];

				if (allNodes[target].firstChild != i && target != 0)
				{
					ushort actual = nodeChildIndex[i];

					//swap the actual contents with the target.
					int start = allNodes[target].firstChild << ChildCountShift;
					int from = allNodes[actual].firstChild << ChildCountShift;

					ushort fc = allNodes[target].firstChild;
					nodeChildIndex[i] = target;
					nodeChildIndex[fc] = actual;
					//if (allNodes[actual].firstChild != i)
					//    throw new ArgumentException();

					for (int n = 0; n < ChildCount; n++)
					{
						Swap(ref allChildren[from + n], ref allChildren[start + n]);
					}

					//swap the pointers
					Swap(ref allNodes[target].firstChild, ref allNodes[actual].firstChild);
				}
			}
		}
		void Swap<T>(ref T a, ref T b)
		{
			T c = a;
			a = b;
			b = c;
		}
		void StoreLogicalOrder(ushort node, ref int index, ref ushort min, ref ushort max)
		{
			if (node != 0 && allNodes[node].left == 0)
			{
				ushort child = (ushort)index;
				this.nodeChildSwapIndex[index++] = node;

				min = Math.Min(min, child);
				max = Math.Max(max, (ushort)(child+1));
				return;
			}


			{
				ushort localMin = ushort.MaxValue, localMax = 0;
				StoreLogicalOrder(allNodes[node].left, ref index, ref localMin, ref localMax);

				if (localMin != ushort.MaxValue && localMax != 0)
				{
					min = Math.Min(min, localMin);
					max = Math.Max(max, localMax);
				}
			}
			{
				ushort localMin = ushort.MaxValue, localMax = 0;
				StoreLogicalOrder(allNodes[node].right, ref index, ref localMin, ref localMax);

				if (localMin != ushort.MaxValue && localMax != 0)
				{
					min = Math.Min(min, localMin);
					max = Math.Max(max, localMax);
				}
			}

			if (min != ushort.MaxValue && max != 0)
			{
				allNodes[node].firstChild = min;
				allNodes[node].childCount = max;
			}
			else
			{
				allNodes[node].firstChild = 0;
				allNodes[node].childCount = 0;
			}
		}


		struct ThreadDrawnInstance
		{
			public ushort firstChild, childCount;
		}

		class ThreadDrawer : Threading.IAction
		{
			public ushort startIndex;
			public int instanceCount;
			public ThreadDrawnInstance[] instances = new ThreadDrawnInstance[16];
			public int cullTestInstanceCount;
			public ThreadDrawnInstance[] cullTestInstances = new ThreadDrawnInstance[8];
			public readonly float[] minBuffer = new float[4], maxBuffer = new float[4];
			public BinaryNode[] nodes;
			public IDraw[] children;
			public int axis;
			public bool idenityMatrix;
			public Threading.WaitCallback callback;
			public bool treeIsOptimized;

			public void PerformAction(object data)
			{
				nodes[startIndex].DrawOnThread(this, (DrawState)data, minBuffer, maxBuffer, axis, children, nodes, idenityMatrix, treeIsOptimized);
			}
		}

		struct BinaryNode
		{
			public BinaryNode(ushort firstChild)
			{
				this.firstChild = firstChild;
				this.left = 0;
				this.right = 0;
				this.childCount = 0;
				this.min = 0;
				this.max = 0;
			}

			//index of the child nodes
			public ushort left, right;

			//bitshifted start index of children
			public ushort firstChild;
			public ushort childCount;

			//axis bounds for this node
			//each node is on the next axis
			//eg, if this node is bound on x-axis, it's children are bound on y-axis
			public float min, max;

			//draw without culling
			public void Draw(DrawState state, IDraw[] children, BinaryNode[] nodes, bool treeIsOptimized)
			{
				if (left == 0)
				{
					uint child = firstChild;
					child <<= ChildCountShift;

					for (ushort i = 0; i < childCount; i++)
						children[child++].Draw(state);
				}
				else
				{
					if (treeIsOptimized)
					{
						uint firstChild = this.firstChild;
						firstChild <<= ChildCountShift;
						uint lastChild = this.childCount;
						lastChild <<= ChildCountShift; // actually lastChild + 1

						for (uint i = firstChild; i < lastChild; i++)
						{
							IDraw item = children[i];
							if (item != null)
								item.Draw(state);
						}
					}
					else
					{
						nodes[left].Draw(state, children, nodes, treeIsOptimized);
						nodes[right].Draw(state, children, nodes, treeIsOptimized);
					}
				}
			}

			//draw with culling
			public void Draw(DrawState state, float[] boundsMin, float[] boundsMax, int axis, IDraw[] children, BinaryNode[] nodes, bool isIdentityMatrix, bool treeIsOptimized)
			{
				boundsMin[axis] = min;
				boundsMax[axis] = max;

				Vector3 localMin = new Vector3(), localMax = new Vector3();

				localMin.X = boundsMin[0];
				localMin.Y = boundsMin[1];
				localMin.Z = boundsMin[2];

				localMax.X = boundsMax[0];
				localMax.Y = boundsMax[1];
				localMax.Z = boundsMax[2];

				ContainmentType type;

				if (!isIdentityMatrix)
					type = (state as ICuller).IntersectBox(ref localMin, ref localMax);
				else
					type = (state as ICuller).IntersectWorldBox(ref localMin, ref localMax);

				switch (type)
				{
				case ContainmentType.Contains:
					Draw(state, children, nodes, treeIsOptimized);
					return;

				case ContainmentType.Intersects:
					if (left == 0)
					{
						uint child = firstChild;
						child <<= ChildCountShift;

						for (ushort i = 0; i < childCount; i++)
						{
							if (children[child].CullTest(state))
								children[child].Draw(state);
							child++;
						}
					}
					else
					{
						nodes[left].Draw(state, boundsMin, boundsMax, axis == 2 ? 0 : axis + 1, children, nodes, isIdentityMatrix, treeIsOptimized);

						boundsMin[0] = localMin.X;
						boundsMin[1] = localMin.Y;
						boundsMin[2] = localMin.Z;

						boundsMax[0] = localMax.X;
						boundsMax[1] = localMax.Y;
						boundsMax[2] = localMax.Z;

						nodes[right].Draw(state, boundsMin, boundsMax, axis == 2 ? 0 : axis + 1, children, nodes, isIdentityMatrix, treeIsOptimized);

						boundsMin[0] = localMin.X;
						boundsMin[1] = localMin.Y;
						boundsMin[2] = localMin.Z;

						boundsMax[0] = localMax.X;
						boundsMax[1] = localMax.Y;
						boundsMax[2] = localMax.Z;
					}
					break;
				}
			}


			//traverses the tree, but on a thread task
			void DrawOnThread(ThreadDrawer thread, DrawState state, IDraw[] children, BinaryNode[] nodes, bool treeIsOptimized)
			{
				if (left == 0)
				{
					ThreadDrawnInstance inst;
					inst.childCount = this.childCount;
					inst.firstChild = this.firstChild;
					if (thread.instanceCount == thread.instances.Length)
						Array.Resize(ref thread.instances, thread.instances.Length * 2);
					thread.instances[thread.instanceCount] = inst;
					thread.instanceCount++;
				}
				else
				{
					if (treeIsOptimized)
					{
						uint firstChild = this.firstChild;
						firstChild <<= ChildCountShift;
						uint lastChild = this.childCount;
						lastChild <<= ChildCountShift; // actually lastChild + 1

						uint count = lastChild - firstChild;

						ushort firstChildIndex = this.firstChild;
						ThreadDrawnInstance inst;

						while (count > ushort.MaxValue)
						{
							if (thread.instanceCount == thread.instances.Length)
								Array.Resize(ref thread.instances, thread.instances.Length * 2);

							firstChildIndex += ushort.MaxValue / ChildCount;
							count -= ushort.MaxValue;

							inst.childCount = ushort.MaxValue;
							inst.firstChild = firstChildIndex;

							thread.instances[thread.instanceCount++] = inst;
						}

						if (thread.instanceCount == thread.instances.Length)
							Array.Resize(ref thread.instances, thread.instances.Length * 2);

						inst.childCount = (ushort)count;
						inst.firstChild = firstChildIndex;

						thread.instances[thread.instanceCount++] = inst;
					}
					else
					{
						nodes[left].DrawOnThread(thread, state, children, nodes, treeIsOptimized);
						nodes[right].DrawOnThread(thread, state, children, nodes, treeIsOptimized);
					}
				}
			}

			//traverses the tree, but on a thread task
			public void DrawOnThread(ThreadDrawer thread, DrawState state, float[] boundsMin, float[] boundsMax, int axis, IDraw[] children, BinaryNode[] nodes, bool isIdentityMatrix, bool treeIsOptimized)
			{
				boundsMin[axis] = min;
				boundsMax[axis] = max;

				Vector3 localMin = new Vector3(), localMax = new Vector3();

				localMin.X = boundsMin[0];
				localMin.Y = boundsMin[1];
				localMin.Z = boundsMin[2];

				localMax.X = boundsMax[0];
				localMax.Y = boundsMax[1];
				localMax.Z = boundsMax[2];

				ContainmentType type;

				if (!isIdentityMatrix)
					type = (state as ICuller).IntersectBox(ref localMin, ref localMax);
				else
					type = (state as ICuller).IntersectWorldBox(ref localMin, ref localMax);

				switch (type)
				{
					case ContainmentType.Contains:
						DrawOnThread(thread, state, children, nodes, treeIsOptimized);
						break;

					case ContainmentType.Intersects:
						if (left == 0)
						{
							ThreadDrawnInstance inst;
							inst.childCount = this.childCount;
							inst.firstChild = this.firstChild;
							if (thread.cullTestInstanceCount == thread.cullTestInstances.Length)
								Array.Resize(ref thread.cullTestInstances, thread.cullTestInstances.Length * 2);
							thread.cullTestInstances[thread.cullTestInstanceCount] = inst;
							thread.cullTestInstanceCount++;
						}
						else
						{
							nodes[left].DrawOnThread(thread,state, boundsMin, boundsMax, axis == 2 ? 0 : axis + 1, children, nodes, isIdentityMatrix, treeIsOptimized);

							boundsMin[0] = localMin.X;
							boundsMin[1] = localMin.Y;
							boundsMin[2] = localMin.Z;

							boundsMax[0] = localMax.X;
							boundsMax[1] = localMax.Y;
							boundsMax[2] = localMax.Z;

							nodes[right].DrawOnThread(thread, state, boundsMin, boundsMax, axis == 2 ? 0 : axis + 1, children, nodes, isIdentityMatrix, treeIsOptimized);

							boundsMin[0] = localMin.X;
							boundsMin[1] = localMin.Y;
							boundsMin[2] = localMin.Z;

							boundsMax[0] = localMax.X;
							boundsMax[1] = localMax.Y;
							boundsMax[2] = localMax.Z;
						}
						break;
				}
			}

			//query without culling
			public bool Query(Func<IDraw, bool> callback, IDraw[] children, BinaryNode[] nodes)
			{
				if (left == 0)
				{
					uint child = firstChild;
					child <<= ChildCountShift;

					for (ushort i = 0; i < childCount; i++)
						if (!callback(children[child++]))
							return false;
				}
				else
				{
					if (!nodes[left].Query(callback, children, nodes))
						return false;
					if (!nodes[right].Query(callback, children, nodes))
						return false;
				}
				return true;
			}

			//query with culling
			public bool Query(ICullPrimitive cull, Func<IDraw, bool> callback, float[] boundsMin, float[] boundsMax, int axis, IDraw[] children, BinaryNode[] nodes)
			{
				boundsMin[axis] = min;
				boundsMax[axis] = max;

				Vector3 localMin = new Vector3(), localMax = new Vector3();

				localMin.X = boundsMin[0];
				localMin.Y = boundsMin[1];
				localMin.Z = boundsMin[2];

				localMax.X = boundsMax[0];
				localMax.Y = boundsMax[1];
				localMax.Z = boundsMax[2];

				switch (cull.IntersectWorldBox(ref localMin, ref localMax))
				{
				case ContainmentType.Contains:
					return Query(callback, children, nodes);

				case ContainmentType.Intersects:
					if (left == 0)
					{
						uint child = firstChild;
						child <<= ChildCountShift;

						for (ushort i = 0; i < childCount; i++)
						{
							if (!callback(children[child++]))
								return false;
						}
					}
					else
					{
						if (!nodes[left].Query(cull, callback, boundsMin, boundsMax, axis == 2 ? 0 : axis + 1, children, nodes))
							return false;

						boundsMin[0] = localMin.X;
						boundsMin[1] = localMin.Y;
						boundsMin[2] = localMin.Z;

						boundsMax[0] = localMax.X;
						boundsMax[1] = localMax.Y;
						boundsMax[2] = localMax.Z;

						if (!nodes[right].Query(cull, callback, boundsMin, boundsMax, axis == 2 ? 0 : axis + 1, children, nodes))
							return false;

						boundsMin[0] = localMin.X;
						boundsMin[1] = localMin.Y;
						boundsMin[2] = localMin.Z;

						boundsMax[0] = localMax.X;
						boundsMax[1] = localMax.Y;
						boundsMax[2] = localMax.Z;
					}
					break;
				}
				return true;
			}

			//add an item to this node
			public void Add(IDraw item, float[] itemMin, float[] itemMax, int axis, StaticBinaryTreePartition parent, ushort thisIndex)
			{
				if (this.min == 0 && this.max == 0)
				{
					this.min = itemMin[axis];
					this.max = itemMax[axis];
				}
				else
				{
					this.min = Math.Min(itemMin[axis], this.min);
					this.max = Math.Max(itemMax[axis], this.max);
				}

				if (left == 0)
				{
					if (childCount != ChildCount)
					{
						parent.allChildren[(((uint)firstChild) << ChildCountShift) + childCount] = item;
						childCount++;
						return;
					}
					Split(parent, thisIndex);
				}

				float itemCentre = itemMin[axis] + itemMax[axis];
				float nodeSplit = this.min + this.max;

				ushort goToNode = itemCentre > nodeSplit ? left : right;

				//if itemCentre and nodeSplit are equal, then itemCentre > nodeSplit is always false, which
				//will mean goToNode == right, so the item will always be added to the right...
				//so... If there are more than 'ChildCount' items in the same place, then this can 
				//cause a stack overflow, because right will keep expanding.
				//the solution isn't fast.. but is should keep things ballanced.
				if (itemCentre == nodeSplit)
				{
					//go to the node with fewer children
					goToNode =
						parent.allNodes[left].CountChildren(parent.allNodes) >
						parent.allNodes[right].CountChildren(parent.allNodes) ?
						right : left;
				}
				
				parent.allNodes[goToNode].Add(item, itemMin, itemMax, axis == 2 ? 0 : axis + 1, parent, goToNode);
			}

			int CountChildren(BinaryNode[] nodes)
			{
				if (left == 0)
					return childCount;
				return nodes[left].CountChildren(nodes) + nodes[right].CountChildren(nodes);
			}

			void Split(StaticBinaryTreePartition parent, ushort index)
			{
				ushort left = parent.GetNewNodeIndex(this.firstChild);
				ushort right = parent.GetNewNodeIndex(parent.GetNewChildArrayBaseIndex());

				parent.allNodes[index].left = left;
				parent.allNodes[index].right = right;

				//parent.allNodes may be reallocated in GetNewNodeIndex...
				this = parent.allNodes[index];

				parent.ReAddChildren(this.firstChild);
			}
		}
	}

}
