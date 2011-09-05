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
	/// A simple class that constructs spherical geometry
	/// </summary>
	public sealed class Sphere : IDrawBatch, ICullableInstance
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

		/// <summary>
		/// Create spherical geometry
		/// </summary>
		/// <param name="size">Size of the sphere</param>
		/// <param name="tesselation">Tesselation of the sphere. Approx number of triangles will be tesselation*tesselation</param>
		/// <param name="storePositionOnly">Store only position, no normals or texture coordinates</param>
		/// <param name="hemisphere">Generate a hemisphere</param>
		/// <param name="sizeToInternalSphere">Expand the sphere to compensate for smaller internal size caused by low tesselation</param>
		public Sphere(Vector3 size, int tesselation, bool storePositionOnly, bool hemisphere, bool sizeToInternalSphere)
		{
			if (tesselation <= 1)
				throw new ArgumentException("tesselation is too small");
			this.radius = Math.Max(Math.Abs(size.X),Math.Max(Math.Abs(size.Y),Math.Abs(size.Z)));

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

			int y_count = tesselation / (hemisphere ? 2 : 1);
			for (int y = 0; y <= y_count; y++)
			{
				float ay = y / tes * (float)Math.PI;
				int div = (int)(tesselation * 2 * (float)Math.Sin(ay)) + 1;

				for (int x = 0; x <= div; x++)
				{
					float ax = x / (float)div * (float)Math.PI * 2.0f;

					float s = (float)Math.Sin(ay);
					Vector3 norm = new Vector3((float)Math.Cos(ax) * s, (float)Math.Sin(ax) * s, (float)Math.Cos(ay));

					if (storePositionOnly)
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
				startIndex += div + 1;
			}

			if (storePositionOnly)
				this.verts = new Vertices<Vector3>(vertsPos);
			else
				this.verts = new Vertices<VertexPositionNormalTexture>(verts);
			this.inds = new Indices<ushort>(inds);
		}

		/// <summary>
		/// Static method to generate the geometry data used by the sphere
		/// </summary>
		public static void GenerateGeometry(Vector3 size, int tesselation, bool storePositionOnly, bool hemisphere, bool sizeToInternalSphere, out IVertices vertices, out IIndices indices)
		{
			Sphere sphere = new Sphere(size, tesselation, storePositionOnly, hemisphere, sizeToInternalSphere);
			vertices = sphere.verts;
			indices = sphere.inds;
		}

		/// <summary>
		/// Create spherical geometry
		/// </summary>
		/// <param name="size">Size of the sphere</param>
		/// <param name="tesselation">Tesselation of the sphere. Approx number of triangles will be tesselation*tesselation</param>
		public Sphere(Vector3 size, int tesselation)
			: this(size, tesselation, false, false, false)
		{
		}

		/// <summary></summary>
		public bool CullTest(ICuller culler)
		{
			return culler.TestSphere(radius);
		}
		/// <summary></summary>
        public bool CullTest(ICuller culler, float radius)
        {
            return culler.TestSphere(radius);
        }

		/// <summary>Draw the sphere</summary>
		public void Draw(DrawState state)
		{
			verts.Draw(state, inds, PrimitiveType.TriangleList);
		}

		/// <summary>Draw this sphere as a batch</summary>
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
		/// Cull test this sphere, using a world matrix to provide the position of the sphere
		/// </summary>
		public bool CullTest(ICuller culler, ref Matrix instance)
		{
			return culler.TestSphere(this.radius, instance.Translation);
		}

		#endregion
	}
}
