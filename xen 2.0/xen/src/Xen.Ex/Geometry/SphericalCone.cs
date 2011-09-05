using System;
using System.Collections.Generic;
using System.Text;
using Xen;
using Xen.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xen.Camera;

namespace Xen.Ex.Geometry
{
	/// <summary>
	/// A simple class that constructs spherical-cone geometry
	/// </summary>
	public sealed class SphericalCone : IDrawBatch, ICullableInstance
	{
		IVertices verts;
		IIndices inds;
		float radius;
		float geometryRadius;

		/// <summary>
		/// Radius of the sphere
		/// </summary>
		public float Radius
		{
			get { return radius; }
		}
		/// <summary>
		/// Radius of the sphere mesh. This value will be larger than <see cref="Radius"/> if 'sizeToInternalSphere' is specified as true in the extended constructor
		/// </summary>
		public float GeometricRadius
		{
			get { return geometryRadius; }
		}

		/// <summary></summary>
		/// <param name="size"></param>
		/// <param name="maxAngle"></param>
		/// <param name="tesselation"></param>
		public SphericalCone(Vector3 size, float maxAngle, int tesselation)
			: this(size,maxAngle, tesselation, false, false)
		{
		}
		/// <summary></summary>
		/// <param name="size"></param>
		/// <param name="maxAngle"></param>
		/// <param name="tesselation"></param>
		/// <param name="positionOnly"></param>
		/// <param name="sizeToInternalSphere"></param>
		public SphericalCone(Vector3 size, float maxAngle, int tesselation, bool positionOnly, bool sizeToInternalSphere)
		{
			if (tesselation <= 1)
				throw new ArgumentException("tesselation is too small");
			this.radius = Math.Max(Math.Abs(size.X),Math.Max(Math.Abs(size.Y),Math.Abs(size.Z)));

			if (maxAngle > MathHelper.TwoPi)
				throw new ArgumentException();

			//if true, then a sphere of 'size' should not intersect if placed within this mesh.
			//since the resolution isn't limitless, the flat surfaces will make the internal size slightly smaller.
			//so expand to compensate
			if (sizeToInternalSphere)
			{
				size /= (float)Math.Cos(Math.PI / tesselation);
			}
			geometryRadius = Math.Max(Math.Abs(size.X), Math.Max(Math.Abs(size.Y), Math.Abs(size.Z)));

			float tes = tesselation;
			int prevCount = -1;
			int startIndex = 0;
			int prevStartIndex = 0;
			List<VertexPositionNormalTexture> verts = new List<VertexPositionNormalTexture>();
			List<Vector3> vertsPos = new List<Vector3>();
			List<ushort> inds = new List<ushort>();

			float ay;
			int div;

			int y_count = (int)Math.Ceiling(tesselation * maxAngle / (Math.PI * 2));
			if (y_count > tesselation)
				y_count = tesselation;

			if (sizeToInternalSphere)
			{
				maxAngle /= (float)Math.Cos(Math.PI / tesselation);
			}

			for (int y = 0; y <= y_count; y++)
			{
				ay = y / tes * (float)Math.PI;

				if (ay > maxAngle * 0.5f)
					ay = maxAngle * 0.5f;

				div = (int)(tesselation * 2 * (float)Math.Sin(ay)) + 1;

				for (int x = 0; x < div; x++)
				{
					float ax = x / (float)div * (float)Math.PI * 2.0f;

					float s = (float)Math.Sin(ay);
					Vector3 norm = new Vector3((float)Math.Cos(ax) * s, (float)Math.Sin(ax) * s, (float)Math.Cos(ay));

					if (positionOnly)
						vertsPos.Add(norm * size);
					else
						verts.Add(new VertexPositionNormalTexture(norm * size, norm, new Vector2(ax / (float)Math.PI * 0.5f, ay / (float)Math.PI)));
				}
				
				if (prevCount != -1)
				{
					int p = 0, c = 0;
					while (p != prevCount || c != div)
					{
						if (p / (float)prevCount > c / (float)div)
						{
							inds.Add((ushort)(c % div + startIndex));
							inds.Add((ushort)(p % prevCount + prevStartIndex));
							inds.Add((ushort)((++c) % div + startIndex));
						}
						else
						{
							inds.Add((ushort)(c%div + startIndex));
							inds.Add((ushort)(p % prevCount + prevStartIndex));
							inds.Add((ushort)((++p) % prevCount + prevStartIndex));
						}
					}
				}

				prevCount = div;
				prevStartIndex = startIndex;
				startIndex += div;
			}

			//not a sphere
			if (y_count != tesselation)
			{
				ay = maxAngle * 0.5f;

				div = (int)(tesselation * 2 * (float)Math.Sin(ay)) + 1;
				int edgeBaseIndex = positionOnly ? vertsPos.Count : verts.Count;

				if (positionOnly)
					vertsPos.Add(Vector3.Zero);
				else
					verts.Add(new VertexPositionNormalTexture(Vector3.Zero, Vector3.Zero, new Vector2(0.0f, 0.0f)));

				for (int x = 0; x < div; x++)
				{
					float ax = x / (float)div * (float)Math.PI * 2.0f;

					float s = (float)Math.Sin(ay);
					Vector3 norm = new Vector3((float)Math.Cos(ax) * (float)Math.Cos(ay), (float)Math.Sin(ax) * (float)Math.Cos(ay), -(float)Math.Sin(ay));
					Vector3 pos = new Vector3((float)Math.Cos(ax) * s, (float)Math.Sin(ax) * s, (float)Math.Cos(ay)) * size;



					if (positionOnly)
						vertsPos.Add(pos);
					else
						verts.Add(new VertexPositionNormalTexture(pos, norm, new Vector2(ax / (float)Math.PI * 0.5f, ay / (float)Math.PI)));

					inds.Add((ushort)(edgeBaseIndex));
					inds.Add((ushort)(edgeBaseIndex + 1 + x));
					inds.Add((ushort)(edgeBaseIndex + 1 + (x + 1) % div));
				}
			}



			if (positionOnly)
				this.verts = new Vertices<Vector3>(vertsPos);
			else
				this.verts = new Vertices<VertexPositionNormalTexture>(verts);
			this.inds = new Indices<ushort>(inds);
		}

		/// <summary>Culltest the cone</summary>
		public bool CullTest(ICuller culler)
		{
			return culler.TestSphere(radius);
		}
		/// <summary>Culltest the cone with a specified radius</summary>
        public bool CullTest(ICuller culler, float radius)
        {
            return culler.TestSphere(radius);
        }

		/// <summary>Draw the cone</summary>
		public void Draw(DrawState state)
		{
			verts.Draw(state, inds, PrimitiveType.TriangleList);
		}

		/// <summary>Draw the cone as a batch</summary>
		public void DrawBatch(DrawState state, Xen.Graphics.InstanceBuffer instances)
		{
			verts.DrawInstances(state, inds, PrimitiveType.TriangleList, instances);
		}
		/// <summary></summary>
		public void DrawBatch(DrawState state, Matrix[] instances, int instanceCount)
		{
			verts.DrawInstances(state, inds, PrimitiveType.TriangleList, instances, instanceCount);
		}

		#region ICullableInstance Members

		/// <summary>
		/// Culltest the cone with a instance world matrix providing the postion of the cone
		/// </summary>
		public bool CullTest(ICuller culler, ref Matrix instance)
		{
			return culler.TestSphere(this.radius, instance.Translation);
		}

		#endregion
	}
}
