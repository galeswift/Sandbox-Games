using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Xen.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Xen.Camera;

namespace Xen.Ex.Scene
{
	/// <summary>
	/// <para>This is a very simple class that will draw wireframe outlines of cull tests that have been performed</para>
	/// <para>This class is intended to be used as a DrawTarget modifier (<see cref="IBeginEndDraw"/>). Add it to a draw target with <see cref="DrawTarget.AddModifier"/></para>
	/// </summary>
	/// <remarks>
	/// <para>This class works by acting as both a DrawTarget Modifier and a PostCuller cull test primitive</para>
	/// <para>A DrawTarget Modifier is setup/shutdown when the DrawTarget begins/finishes rendering. (see <see cref="IBeginEndDraw"/>)</para>
	/// <para>During the setup phase, this class adds itself as a PostCuller to the DrawTarget (see DrawState.Cullers.PushPostCuller, this allows the class to record the bounding boxes and spheres used for successful CullTests performed</para>
	/// <para>During the shutdown phase, this class draws the recorded sphers/cubes.</para>
	/// </remarks>
	public sealed class CullTestVisualizer : IBeginEndDraw, ICullPrimitive
	{
		struct MatrixPair
		{
			public Matrix Matrix;
			public ICamera Camera;
		}
		struct VectorPair
		{
			public Vector3 Position;
			public float Radius;
			public ICamera Camera;
		}



		private readonly List<VectorPair> spheres;
		private readonly List<MatrixPair> cubes;
		private IVertices cubeVS, sphereVS;
		private bool enabled, enabledBuffer;

		//Generally, this is not a good idea: This class stores a reference to the camera stack that is used by the DrawState.
		private Xen.Graphics.Stack.CameraStack cameras;

		private static string cubeVSid = typeof(CullTestVisualizer).FullName + ".cubeVS";
		private static string sphereVSid = typeof(CullTestVisualizer).FullName + ".sphereVS";

		private MatrixPair baseMatrix;

		/// <summary>
		/// <para>Construct the Visualizer. This class displays the cull tests that have been performed.</para>
		/// <para>Add the constructed instance of this class to a DrawTarget as a Modifier using <see cref="DrawTarget.AddModifier"/></para>
		/// </summary>
		public CullTestVisualizer()
		{
			this.enabled = true;
			this.spheres = new List<VectorPair>();
			this.cubes = new List<MatrixPair>();
			this.EnableDepthTesting = true;

			baseMatrix.Matrix = Matrix.Identity;
		}

		/// <summary>
		/// <para>Gets/Set if the visualization is enabled</para>
		/// </summary>
		public bool Enabled { get { return enabled; } set { enabled = value; } }

		/// <summary>
		/// Begin the modifier (This method is called by the DrawTarget)
		/// </summary>
		/// <param name="state"></param>
		public void Begin(DrawState state)
		{
			enabledBuffer = enabled;
			this.cameras = state.Camera;

			if (enabledBuffer)
				state.Cullers.PushPostCuller(this);
		}

		/// <summary>
		/// End the modifier (This method is called by the DrawTarget)
		/// </summary>
		/// <param name="state"></param>
		public void End(DrawState state)
		{
			if (enabledBuffer)
				state.Cullers.PopPostCuller();

			this.cameras = null;

			if (cubes.Count > 0 || spheres.Count > 0)
			{
				Xen.Ex.Shaders.FillSolidColour shader = state.GetShader<Xen.Ex.Shaders.FillSolidColour>();
				shader.FillColour = new Vector4(1,1,1,0.25f);

				using (state.RenderState.Push())
				using (state.Shader.Push(shader))
				using (state.Camera.Push())
				{
					ICamera camera = state.Camera.GetCamera();

					state.RenderState.CurrentDepthState.DepthWriteEnabled = false;
					state.RenderState.CurrentDepthState.DepthTestEnabled = EnableDepthTesting;
					state.RenderState.CurrentBlendState = AlphaBlendState.Alpha;

					GenCubeVS(state);
					GenSphereVS(state);

					Matrix mat;
					for (int i = 0; i < cubes.Count; i++)
					{
						mat = cubes[i].Matrix;
						if (camera != cubes[i].Camera)
						{
							camera = cubes[i].Camera;
							state.Camera.SetCamera(camera);
						}
						using (state.WorldMatrix.Push(ref mat))
						{
							cubeVS.Draw(state, null, PrimitiveType.LineList);
						}
					}

					mat = Matrix.Identity;

					for (int i = 0; i < spheres.Count; i++)
					{
						Vector3 v = spheres[i].Position;
						float scale = spheres[i].Radius;

						if (camera != spheres[i].Camera)
						{
							camera = spheres[i].Camera;
							state.Camera.SetCamera(camera);
						}

						mat.M11 = scale;
						mat.M22 = scale;
						mat.M33 = scale;
						mat.M41 = v.X;
						mat.M42 = v.Y;
						mat.M43 = v.Z;

						using (state.WorldMatrix.Push(ref mat))
						{
							sphereVS.Draw(state, null, PrimitiveType.LineList);
						}
					}
				}
			}

			spheres.Clear();
			cubes.Clear();
		}

		private void GenCubeVS(DrawState state)
		{
			if (cubeVS != null) return;
			cubeVS = state.Application.UserValues[cubeVSid] as IVertices;
			if (cubeVS != null) return;

			//cube outlines, between 0,0,0 and 1,1,1
			cubeVS = new Vertices<Vector3>(
				new Vector3(0,0,0),new Vector3(1,0,0),new Vector3(0,1,0),new Vector3(1,1,0),
				new Vector3(0,0,1),new Vector3(1,0,1),new Vector3(0,1,1),new Vector3(1,1,1),

				new Vector3(0,0,0),new Vector3(0,1,0),new Vector3(1,0,0),new Vector3(1,1,0),
				new Vector3(0,0,1),new Vector3(0,1,1),new Vector3(1,0,1),new Vector3(1,1,1),
				
				new Vector3(0,0,0),new Vector3(0,0,1),new Vector3(1,0,0),new Vector3(1,0,1),				
				new Vector3(0,1,0),new Vector3(0,1,1),new Vector3(1,1,0),new Vector3(1,1,1)
			);
			state.Application.UserValues[cubeVSid] = cubeVS;
		}

		private void GenSphereVS(DrawState state)
		{
			if (sphereVS != null) return;
			sphereVS = state.Application.UserValues[sphereVSid] as Vertices<Vector3>;
			if (sphereVS != null) return;

			const int subdiv = 64;

			int index = 0;
			Vector3[] verts = new Vector3[(subdiv + 1) * 6];

			for (int axis = 0; axis < 3; axis++)
			{
				for (int i = 0; i <= subdiv; i++)
				{
					float f = (((float)i) / ((float)subdiv)) * MathHelper.TwoPi;
					float fp = (((float)i-1) / ((float)subdiv)) * MathHelper.TwoPi;

					float x = (float)Math.Cos(f);
					float y = (float)Math.Sin(f);
					float xp = (float)Math.Cos(fp);
					float yp = (float)Math.Sin(fp);

					switch (axis)
					{
						case 0:
							verts[index++] = new Vector3(xp, yp, 0);
							verts[index++] = new Vector3(x, y, 0);
							break;
						case 1:
							verts[index++] = new Vector3(xp, 0, yp);
							verts[index++] = new Vector3(x, 0, y);
							break;
						case 2:
							verts[index++] = new Vector3(0, xp, yp);
							verts[index++] = new Vector3(0, x, y);
							break;
					}
				}
			}


			//sphere outlines, between -1,-1,-1 and 1,1,1
			sphereVS = new Vertices<Vector3>(verts);
			state.Application.UserValues[sphereVSid] = sphereVS;
		}


		#region ICullPrimitive Members

		bool ICullPrimitive.TestWorldBox(ref Vector3 min, ref Vector3 max, ref Matrix world)
		{
			lock (this)
			{
				MatrixPair baseMatrix = this.baseMatrix;
				baseMatrix.Camera = cameras.GetCamera();

				if (baseMatrix.Camera == null)
					return true;

				baseMatrix.Matrix.M41 = min.X;
				baseMatrix.Matrix.M42 = min.Y;
				baseMatrix.Matrix.M43 = min.Z;

				baseMatrix.Matrix.M11 = max.X - min.X;
				baseMatrix.Matrix.M22 = max.Y - min.Y;
				baseMatrix.Matrix.M33 = max.Z - min.Z;

				Matrix.Multiply(ref baseMatrix.Matrix, ref world, out baseMatrix.Matrix);

				cubes.Add(baseMatrix);
				return true;
			}
		}

		bool ICullPrimitive.TestWorldBox(Vector3 min, Vector3 max, ref Matrix world)
		{
			lock (this)
			{
				MatrixPair baseMatrix = this.baseMatrix;
				baseMatrix.Camera = cameras.GetCamera();

				if (baseMatrix.Camera == null)
					return true;

				baseMatrix.Matrix.M41 = min.X;
				baseMatrix.Matrix.M42 = min.Y;
				baseMatrix.Matrix.M43 = min.Z;

				baseMatrix.Matrix.M11 = max.X - min.X;
				baseMatrix.Matrix.M22 = max.Y - min.Y;
				baseMatrix.Matrix.M33 = max.Z - min.Z;

				Matrix.Multiply(ref baseMatrix.Matrix, ref world, out baseMatrix.Matrix);

				cubes.Add(baseMatrix);
				return true;
			}
		}

		bool ICullPrimitive.TestWorldBox(ref Vector3 min, ref Vector3 max)
		{
			lock (this)
			{
				baseMatrix.Camera = cameras.GetCamera();

				if (baseMatrix.Camera == null)
					return true;

				baseMatrix.Matrix.M41 = min.X;
				baseMatrix.Matrix.M42 = min.Y;
				baseMatrix.Matrix.M43 = min.Z;

				baseMatrix.Matrix.M11 = max.X - min.X;
				baseMatrix.Matrix.M22 = max.Y - min.Y;
				baseMatrix.Matrix.M33 = max.Z - min.Z;

				cubes.Add(baseMatrix);
				return true;
			}
		}

		bool ICullPrimitive.TestWorldBox(Vector3 min, Vector3 max)
		{
			lock (this)
			{
				baseMatrix.Camera = cameras.GetCamera();

				if (baseMatrix.Camera == null)
					return true;

				baseMatrix.Matrix.M41 = min.X;
				baseMatrix.Matrix.M42 = min.Y;
				baseMatrix.Matrix.M43 = min.Z;

				baseMatrix.Matrix.M11 = max.X - min.X;
				baseMatrix.Matrix.M22 = max.Y - min.Y;
				baseMatrix.Matrix.M33 = max.Z - min.Z;

				cubes.Add(baseMatrix);
				return true;
			}
		}

		bool ICullPrimitive.TestWorldSphere(float radius, ref Vector3 position)
		{
			lock (this)
			{
				VectorPair pair = new VectorPair();
				pair.Camera = cameras.GetCamera();

				if (pair.Camera == null)
					return true;

				pair.Position = position;
				pair.Radius = radius;
				spheres.Add(pair);
				return true;
			}
		}

		bool ICullPrimitive.TestWorldSphere(float radius, Vector3 position)
		{
			lock (this)
			{
				VectorPair pair = new VectorPair();
				pair.Camera = cameras.GetCamera();

				if (pair.Camera == null)
					return true;

				pair.Position = position;
				pair.Radius = radius;
				spheres.Add(pair);
				return true;
			}
		}

		ContainmentType ICullPrimitive.IntersectWorldBox(ref Vector3 min, ref Vector3 max, ref Matrix world)
		{
			lock (this)
			{
				MatrixPair baseMatrix = this.baseMatrix;
				baseMatrix.Camera = cameras.GetCamera();

				if (baseMatrix.Camera == null)
					return ContainmentType.Contains;

				baseMatrix.Matrix.M41 = min.X;
				baseMatrix.Matrix.M42 = min.Y;
				baseMatrix.Matrix.M43 = min.Z;

				baseMatrix.Matrix.M11 = max.X - min.X;
				baseMatrix.Matrix.M22 = max.Y - min.Y;
				baseMatrix.Matrix.M33 = max.Z - min.Z;

				Matrix.Multiply(ref baseMatrix.Matrix, ref world, out baseMatrix.Matrix);

				cubes.Add(baseMatrix);
				return ContainmentType.Contains;
			}
		}

		ContainmentType ICullPrimitive.IntersectWorldBox(Vector3 min, Vector3 max, ref Matrix world)
		{
			lock (this)
			{
				MatrixPair baseMatrix = this.baseMatrix;
				baseMatrix.Camera = cameras.GetCamera();

				if (baseMatrix.Camera == null)
					return ContainmentType.Contains;

				baseMatrix.Matrix.M41 = min.X;
				baseMatrix.Matrix.M42 = min.Y;
				baseMatrix.Matrix.M43 = min.Z;

				baseMatrix.Matrix.M11 = max.X - min.X;
				baseMatrix.Matrix.M22 = max.Y - min.Y;
				baseMatrix.Matrix.M33 = max.Z - min.Z;

				Matrix.Multiply(ref baseMatrix.Matrix, ref world, out baseMatrix.Matrix);

				cubes.Add(baseMatrix);
				return ContainmentType.Contains;
			}
		}

		ContainmentType ICullPrimitive.IntersectWorldBox(ref Vector3 min, ref Vector3 max)
		{
			lock (this)
			{
				baseMatrix.Camera = cameras.GetCamera();

				if (baseMatrix.Camera == null)
					return ContainmentType.Contains;

				baseMatrix.Matrix.M41 = min.X;
				baseMatrix.Matrix.M42 = min.Y;
				baseMatrix.Matrix.M43 = min.Z;

				baseMatrix.Matrix.M11 = max.X - min.X;
				baseMatrix.Matrix.M22 = max.Y - min.Y;
				baseMatrix.Matrix.M33 = max.Z - min.Z;

				cubes.Add(baseMatrix);
				return ContainmentType.Contains;
			}
		}

		ContainmentType ICullPrimitive.IntersectWorldBox(Vector3 min, Vector3 max)
		{
			lock (this)
			{
				baseMatrix.Camera = cameras.GetCamera();

				if (baseMatrix.Camera == null)
					return ContainmentType.Contains;

				baseMatrix.Matrix.M41 = min.X;
				baseMatrix.Matrix.M42 = min.Y;
				baseMatrix.Matrix.M43 = min.Z;

				baseMatrix.Matrix.M11 = max.X - min.X;
				baseMatrix.Matrix.M22 = max.Y - min.Y;
				baseMatrix.Matrix.M33 = max.Z - min.Z;

				cubes.Add(baseMatrix);
				return ContainmentType.Contains;
			}
		}

		ContainmentType ICullPrimitive.IntersectWorldSphere(float radius, ref Vector3 position)
		{
			lock (this)
			{
				VectorPair pair = new VectorPair();
				pair.Camera = cameras.GetCamera();

				if (pair.Camera == null)
					return ContainmentType.Contains;

				pair.Position = position;
				pair.Radius = radius;
				spheres.Add(pair);
				return ContainmentType.Contains;
			}
		}

		ContainmentType ICullPrimitive.IntersectWorldSphere(float radius, Vector3 position)
		{
			lock (this)
			{
				VectorPair pair = new VectorPair();
				pair.Camera = cameras.GetCamera();

				if (pair.Camera == null)
					return ContainmentType.Contains;

				pair.Position = position;
				pair.Radius = radius;
				spheres.Add(pair);
				return ContainmentType.Contains;
			}
		}

		#endregion

		/// <summary>
		/// When false, the displayed cull tests will have depth testing disabled - so they are always visible
		/// </summary>
		public bool EnableDepthTesting { get; set; }
	}
}
