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
	/// A simple class that constructs cone geometry
	/// </summary>
	public sealed class Cone : IDrawBatch, ICullableInstance
	{
		IVertices verts;
		IIndices inds;
		float radius;
		float geometryRadius;

		/// <summary>
		/// Radius of the cone
		/// </summary>
		public float Radius
		{
			get { return radius; }
		}
		/// <summary>
		/// Radius of the cone mesh. This value will be larger than <see cref="Radius"/> if 'sizeToInternalCone' is specified as true in the extended constructor
		/// </summary>
		public float GeometricRadius
		{
			get { return geometryRadius; }
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="size"></param>
		/// <param name="tesselation"></param>
		public Cone(Vector3 size, int tesselation)
			: this(size, tesselation, false, false)
		{
		}


		/// <summary>
		/// Static method to generate the geometry data used by the sphere
		/// </summary>
		public static void GenerateGeometry(Vector3 size, int tesselation, bool positionOnly, bool sizeToInternalCone, out IVertices vertices, out IIndices indices)
		{
			Cone geom = new Cone(size, tesselation, positionOnly, sizeToInternalCone);
			vertices = geom.verts;
			indices = geom.inds;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="size"></param>
		/// <param name="tesselation"></param>
		/// <param name="positionOnly">vertex data will only store position</param>
		/// <param name="sizeToInternalCone">expand the cone geometry outwards so it encoses the desired size</param>
		public Cone(Vector3 size, int tesselation, bool positionOnly, bool sizeToInternalCone)
		{
			if (tesselation <= 2)
				throw new ArgumentException("tesselation is too small");
			if (size.Z == 0)
				throw new ArgumentException("size.Z == 0");

			this.radius = 0;
			//this.radius = Math.Max(Math.Abs(size.X), Math.Max(Math.Abs(size.Y), Math.Abs(size.Z)));

			//if true, then a cone of 'size' should not intersect if placed within this mesh.
			//since the resolution isn't limitless, the flat surfaces will make the internal size slightly smaller.
			//so expand to compensate
			if (sizeToInternalCone)
			{
				float s = 1.0f/(float)Math.Cos(Math.PI / tesselation);
				size.X *= s;
				size.Y *= s;
			}
			geometryRadius = Math.Max(Math.Abs(size.X), Math.Max(Math.Abs(size.Y), Math.Abs(size.Z)));

			float tes = tesselation;
			int prevCount = -1;
			int startIndex = 0;
			int prevStartIndex = 0;
			List<VertexPositionNormalTexture> verts = new List<VertexPositionNormalTexture>();
			List<Vector3> vertsPos = new List<Vector3>();
			List<ushort> inds = new List<ushort>();

			if (positionOnly)
				vertsPos.Add(Vector3.Zero);
			else
				verts.Add(new VertexPositionNormalTexture(Vector3.Zero, Vector3.Zero, new Vector2(0.5f, 0.5f)));

			int y_count = tesselation;
			for (int y = 0; y < y_count; y++)
			{
				float ay = y / tes * (float)Math.PI * 2.0f;
				int div = (int)(tesselation * 2 * (float)Math.Sin(ay)) + 1;


				Vector3 norm = new Vector3((float)Math.Cos(ay), (float)Math.Sin(ay), 1);
				Vector3 pos = norm * size;
				this.radius = Math.Max(this.radius, pos.LengthSquared());
				Vector2 tex = new Vector2(norm.X * 0.5f + 0.5f, norm.Y * 0.5f + 0.5f);

				if (positionOnly)
					vertsPos.Add(pos);
				else
					verts.Add(new VertexPositionNormalTexture(pos, new Vector3(0, 0, Math.Sign(size.Z)), tex));

				norm.X *= -size.Z/size.X;
				norm.Y *= -size.Z/size.Y;
				norm.Normalize();

				if (!positionOnly)
					verts.Add(new VertexPositionNormalTexture(pos, -norm, tex));

				if (positionOnly)
				{
					inds.Add(0);
					inds.Add((ushort)(y + 1));
					inds.Add((ushort)((y + 1) % y_count + 1));

					if (y != 0 && y != y_count - 1)
					{
						inds.Add(1);
						inds.Add((ushort)((y + 1) % y_count + 1));
						inds.Add((ushort)(y + 1));
					}
				}
				else
				{
					inds.Add(0);
					inds.Add((ushort)(y * 2 + 2));
					inds.Add((ushort)(((y + 1) % y_count) * 2 + 2));

					if (y != 0 && y != y_count-1)
					{
						inds.Add(1);
						inds.Add((ushort)(((y + 1) % y_count) * 2 + 1));
						inds.Add((ushort)(y*2+1));
					}
				}

				prevCount = div;
				prevStartIndex = startIndex;
				startIndex += div;
			}

			if (this.radius != 0)
				this.radius = (float)Math.Sqrt(this.radius);

			if (positionOnly)
				this.verts = new Vertices<Vector3>(vertsPos);
			else
				this.verts = new Vertices<VertexPositionNormalTexture>(verts);
			this.inds = new Indices<ushort>(inds);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="culler"></param>
		/// <returns></returns>
		public bool CullTest(ICuller culler)
		{
			return culler.TestSphere(radius);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="culler"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
        public bool CullTest(ICuller culler, float radius)
        {
            return culler.TestSphere(radius);
        }

		/// <summary></summary>
		/// <param name="state"></param>
		public void Draw(DrawState state)
		{
			verts.Draw(state, inds, PrimitiveType.TriangleList);
		}
		/// <summary></summary>
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
		/// Cull test an instance of this cone, using a local world matrix for the position
		/// </summary>
		public bool CullTest(ICuller culler, ref Matrix instance)
		{
			return culler.TestSphere(this.radius, instance.Translation);
		}

		#endregion
	}
}
