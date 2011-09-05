using System;
using System.Collections.Generic;
using System.Text;
using Xen;
using Xen.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xen.Camera;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Xen.Ex.Geometry
{
	/// <summary>
	/// Cube geometry
	/// </summary>
	public sealed class Cube : IDrawBatch, ICullableInstance
	{
		IVertices verts;
		IIndices inds;
		Vector3 min,max; 

		/// <summary>
		/// Construct the cube with the given size
		/// </summary>
		/// <param name="size"></param>
		public Cube(Vector3 size)
		{
			Vector3 absSize = new Vector3(Math.Abs(size.X), Math.Abs(size.Y), Math.Abs(size.Z));
			this.min = -absSize;
			this.max = absSize;

			//yeah ok
			verts = new Vertices<VertexPositionNormalTexture>(
				new VertexPositionNormalTexture(new Vector3(-1,-1,-1) * size,new Vector3(-1,0,0), new Vector2(0,0)),
				new VertexPositionNormalTexture(new Vector3(-1, 1,-1) * size,new Vector3(-1,0,0), new Vector2(1,0)),
				new VertexPositionNormalTexture(new Vector3(-1, 1, 1) * size,new Vector3(-1,0,0), new Vector2(1,1)),
				new VertexPositionNormalTexture(new Vector3(-1,-1, 1) * size,new Vector3(-1,0,0), new Vector2(0,1)),
				
				new VertexPositionNormalTexture(new Vector3(-1, -1,-1) * size,new Vector3(0,-1,0), new Vector2(1,0)),
				new VertexPositionNormalTexture(new Vector3( 1, -1,-1) * size,new Vector3(0,-1,0), new Vector2(0,0)),
				new VertexPositionNormalTexture(new Vector3( 1, -1, 1) * size,new Vector3(0,-1,0), new Vector2(0,1)),
				new VertexPositionNormalTexture(new Vector3(-1, -1, 1) * size,new Vector3(0,-1,0), new Vector2(1,1)),
				
				new VertexPositionNormalTexture(new Vector3(-1,-1,-1) * size,new Vector3(0,0,-1), new Vector2(0,0)),
				new VertexPositionNormalTexture(new Vector3( 1,-1,-1) * size,new Vector3(0,0,-1), new Vector2(1,0)),
				new VertexPositionNormalTexture(new Vector3( 1, 1,-1) * size,new Vector3(0,0,-1), new Vector2(1,1)),
				new VertexPositionNormalTexture(new Vector3(-1, 1,-1) * size,new Vector3(0,0,-1), new Vector2(0,1)),

				
				new VertexPositionNormalTexture(new Vector3(1,-1,-1) * size,new Vector3(1,0,0), new Vector2(1,1)),
				new VertexPositionNormalTexture(new Vector3(1, 1,-1) * size,new Vector3(1,0,0), new Vector2(0,1)),
				new VertexPositionNormalTexture(new Vector3(1, 1, 1) * size,new Vector3(1,0,0), new Vector2(0,0)),
				new VertexPositionNormalTexture(new Vector3(1,-1, 1) * size,new Vector3(1,0,0), new Vector2(1,0)),
				
				new VertexPositionNormalTexture(new Vector3(-1, 1,-1) * size,new Vector3(0,1,0), new Vector2(0,1)),
				new VertexPositionNormalTexture(new Vector3( 1, 1,-1) * size,new Vector3(0,1,0), new Vector2(1,1)),
				new VertexPositionNormalTexture(new Vector3( 1, 1, 1) * size,new Vector3(0,1,0), new Vector2(1,0)),
				new VertexPositionNormalTexture(new Vector3(-1, 1, 1) * size,new Vector3(0,1,0), new Vector2(0,0)),
				
				new VertexPositionNormalTexture(new Vector3(-1,-1,1) * size,new Vector3(0,0,1), new Vector2(1,1)),
				new VertexPositionNormalTexture(new Vector3( 1,-1,1) * size,new Vector3(0,0,1), new Vector2(0,1)),
				new VertexPositionNormalTexture(new Vector3( 1, 1,1) * size,new Vector3(0,0,1), new Vector2(0,0)),
				new VertexPositionNormalTexture(new Vector3(-1, 1,1) * size,new Vector3(0,0,1), new Vector2(1,0))
			);

			inds = new Indices<ushort>(
				0 + 0, 1 + 0, 2 + 0, 0 + 0, 2 + 0, 3 + 0,
				0 + 4, 2 + 4, 1 + 4, 0 + 4, 3 + 4, 2 + 4,
				0 + 8, 1 + 8, 2 + 8, 0 + 8, 2 + 8, 3 + 8,

				0 + 12, 2 + 12, 1 + 12, 0 + 12, 3 + 12, 2 + 12,
				0 + 16, 1 + 16, 2 + 16, 0 + 16, 2 + 16, 3 + 16,
				0 + 20, 2 + 20, 1 + 20, 0 + 20, 3 + 20, 2 + 20
			);
		}

		/// <summary>
		/// Construct the cube with the given minimum and maximum bound
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public Cube(Vector3 min, Vector3 max)
		{
			this.min = min;
			this.max = max;

			Vector3 size = max - min;

			//yeah ok
			verts = new Vertices<VertexPositionNormalTexture>(
				new VertexPositionNormalTexture(new Vector3(0, 0, 0) * size + min, new Vector3(-1, 0, 0), new Vector2(0, 0)),
				new VertexPositionNormalTexture(new Vector3(0, 1, 0) * size + min, new Vector3(-1, 0, 0), new Vector2(1, 0)),
				new VertexPositionNormalTexture(new Vector3(0, 1, 1) * size + min, new Vector3(-1, 0, 0), new Vector2(1, 1)),
				new VertexPositionNormalTexture(new Vector3(0, 0, 1) * size + min, new Vector3(-1, 0, 0), new Vector2(0, 1)),

				new VertexPositionNormalTexture(new Vector3(0, 0, 0) * size + min, new Vector3(0, -1, 0), new Vector2(1, 0)),
				new VertexPositionNormalTexture(new Vector3(1, 0, 0) * size + min, new Vector3(0, -1, 0), new Vector2(0, 0)),
				new VertexPositionNormalTexture(new Vector3(1, 0, 1) * size + min, new Vector3(0, -1, 0), new Vector2(0, 1)),
				new VertexPositionNormalTexture(new Vector3(0, 0, 1) * size + min, new Vector3(0, -1, 0), new Vector2(1, 1)),

				new VertexPositionNormalTexture(new Vector3(0, 0, 0) * size + min, new Vector3(0, 0, -1), new Vector2(0, 0)),
				new VertexPositionNormalTexture(new Vector3(1, 0, 0) * size + min, new Vector3(0, 0, -1), new Vector2(1, 0)),
				new VertexPositionNormalTexture(new Vector3(1, 1, 0) * size + min, new Vector3(0, 0, -1), new Vector2(1, 1)),
				new VertexPositionNormalTexture(new Vector3(0, 1, 0) * size + min, new Vector3(0, 0, -1), new Vector2(0, 1)),


				new VertexPositionNormalTexture(new Vector3(1, 0, 0) * size + min, new Vector3(1, 0, 0), new Vector2(1, 1)),
				new VertexPositionNormalTexture(new Vector3(1, 1, 0) * size + min, new Vector3(1, 0, 0), new Vector2(0, 1)),
				new VertexPositionNormalTexture(new Vector3(1, 1, 1) * size + min, new Vector3(1, 0, 0), new Vector2(0, 0)),
				new VertexPositionNormalTexture(new Vector3(1, 0, 1) * size + min, new Vector3(1, 0, 0), new Vector2(1, 0)),

				new VertexPositionNormalTexture(new Vector3(0, 1, 0) * size + min, new Vector3(0, 1, 0), new Vector2(0, 1)),
				new VertexPositionNormalTexture(new Vector3(1, 1, 0) * size + min, new Vector3(0, 1, 0), new Vector2(1, 1)),
				new VertexPositionNormalTexture(new Vector3(1, 1, 1) * size + min, new Vector3(0, 1, 0), new Vector2(1, 0)),
				new VertexPositionNormalTexture(new Vector3(0, 1, 1) * size + min, new Vector3(0, 1, 0), new Vector2(0, 0)),

				new VertexPositionNormalTexture(new Vector3(0, 0, 1) * size + min, new Vector3(0, 0, 1), new Vector2(1, 1)),
				new VertexPositionNormalTexture(new Vector3(1, 0, 1) * size + min, new Vector3(0, 0, 1), new Vector2(0, 1)),
				new VertexPositionNormalTexture(new Vector3(1, 1, 1) * size + min, new Vector3(0, 0, 1), new Vector2(0, 0)),
				new VertexPositionNormalTexture(new Vector3(0, 1, 1) * size + min, new Vector3(0, 0, 1), new Vector2(1, 0))
			);

			inds = new Indices<ushort>(
				0 + 0, 1 + 0, 2 + 0, 0 + 0, 2 + 0, 3 + 0,
				0 + 4, 2 + 4, 1 + 4, 0 + 4, 3 + 4, 2 + 4,
				0 + 8, 1 + 8, 2 + 8, 0 + 8, 2 + 8, 3 + 8,

				0 + 12, 2 + 12, 1 + 12, 0 + 12, 3 + 12, 2 + 12,
				0 + 16, 1 + 16, 2 + 16, 0 + 16, 2 + 16, 3 + 16,
				0 + 20, 2 + 20, 1 + 20, 0 + 20, 3 + 20, 2 + 20
			);
		}

		/// <summary>
		/// FrustumCull test the cube
		/// </summary>
		/// <param name="culler"></param>
		/// <returns></returns>
		public bool CullTest(ICuller culler) 
		{
			return culler.TestBox(ref min, ref max);
		}

		/// <summary>
		/// Draw the cube geometry
		/// </summary>
		/// <param name="state"></param>
		public void Draw(DrawState state)
		{
			verts.Draw(state, inds, PrimitiveType.TriangleList);
		}
		/// <summary>
		/// Draw multiple instances of this cube geometry
		/// </summary>
		public void DrawBatch(DrawState state, Xen.Graphics.InstanceBuffer instances)
		{
			verts.DrawInstances(state, inds, PrimitiveType.TriangleList, instances);
		}
		/// <summary>
		/// Draw multiple instances of this cube geometry</summary>
		public void DrawBatch(DrawState state, Matrix[] instances, int instanceCount)
		{
			verts.DrawInstances(state, inds, PrimitiveType.TriangleList, instances, instanceCount);
		}

		#region ICullableInstance Members

		/// <summary>
		/// Cull test this box using a local world matrix
		/// </summary>
		public bool CullTest(ICuller culler, ref Matrix instance)
		{
			return culler.TestBox(ref min, ref max, ref instance);
		}

		#endregion
	}
}
