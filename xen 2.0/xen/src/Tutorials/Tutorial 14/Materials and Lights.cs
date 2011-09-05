using System;
using System.Collections.Generic;
using System.Text;



using Xen;
using Xen.Camera;
using Xen.Graphics;
using Xen.Ex.Graphics2D;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Xen.Ex.Material;
using Xen.Ex.Graphics;
using Xen.Ex.Graphics.Content;


/*
 * This sample demonstrates:
 * 
 * Drawing a circular disk using custom geometry and a complex material
 * 
 * placing two lights in the scene
 * 
 * changing the resolution
 * 
 */
namespace Tutorials.Tutorial_14
{

	//in this tutorial a circular disk is drawn to represent the ground.
	//this disk will use normal mapping, so it's vertices require
	//normals, binormals and tangents.

	//tangents, binormals and normals store the x/y/z directions of the surface
	//at the given vertex. The tangents/binormals usually need to match the
	//way textures are applied for normal mapping to work correctly.

	//custom vertex for the disk
	struct DiskVertex
	{
		public Vector3 position;
		public Vector3 normal, binormal, tangent;
		public Vector2 tex;

		//creates a vertex based on angle/size
		public DiskVertex(float angle, float size, float texScale)
		{
			position = new Vector3((float)Math.Sin(angle) * size, (float)Math.Cos(angle) * size, 0);
			//since it's a flat plane, the normals etc are constant
			normal = new Vector3(0, 0, 1);
			binormal = new Vector3(0, 1, 0);
			tangent = new Vector3(1, 0, 0);

			tex = new Vector2(position.X, position.Y) * texScale;
		}
	}

	//this class draws the ground disk
	class GroundDisk : IDraw, IContentOwner
	{
		//vertices containing the DiskVertex's
		private IVertices vertices;
		private IIndices indices;

		//custom material for this geometry
		private readonly MaterialShader Material;

		private float radius;

		public GroundDisk(ContentRegister content, MaterialLightCollection lights, float radius)
		{
			this.radius = radius;

			int vertexCount = 256;
			var indices = new List<int>();

			//create the vertices. Note the DiskVertex() constructor takes an angle/size
			var verts = new DiskVertex[vertexCount];

			for (int i = 0; i < vertexCount; i++)
			{
				verts[i] = new DiskVertex((i / (float)(vertexCount - 1)) * MathHelper.TwoPi, radius, 0.05f);
				
				if (i != 0)	//add the tirangle indices
				{
					indices.Add(0);
					indices.Add(i - 1);
					indices.Add(i);
				}
			}

			//create the vertex buffer
			this.vertices = new Vertices<DiskVertex>(verts);
			this.indices = new Indices<int>(indices);


			//create the custom material for this geometry
			//the light collection has been passed into the constructor, although it
			//could easily be changed later (by changing material.Lights)
			this.Material = new MaterialShader(lights);
			
			//give the disk really bright specular for effect
			Material.SpecularColour = new Vector3(1,1,1);
			Material.DiffuseColour = new Vector3(0.6f, 0.6f, 0.6f);
			Material.SpecularPower = 64;

			//setup the texture samples to use high quality anisotropic filtering
			//the textures are assigned in LoadContent
			Material.Textures = new MaterialTextures();
			Material.Textures.TextureMapSampler = TextureSamplerState.AnisotropicHighFiltering;
			Material.Textures.NormalMapSampler = TextureSamplerState.AnisotropicLowFiltering;

			//load the textures for this material
			content.Add(this);
		}

		public void LoadContent(ContentState state)
		{
			//load the box texture, and it's normal map.
			Material.Textures.TextureMap = state.Load<Texture2D>(@"box");
			Material.Textures.NormalMap = state.Load<Texture2D>(@"box_normal");
		}

		//draw the ground plane..
		public void Draw(DrawState state)
		{
			//first bind the material shader
			using (state + Material)
			{
				//then draw the vertices
				vertices.Draw(state, indices, PrimitiveType.TriangleList);
			}
		}

		public bool CullTest(ICuller culler)
		{
			return culler.TestBox(new Vector3(-radius, -radius, 0), new Vector3(radius, radius, 0));
		}
	}

	//this application will create a light collection with two lights,
	//it will also create the ground plane, and it creates spheres
	//that visually show where the lights are.
	[DisplayName(Name = "Tutorial 14: Materials and lights")]
	public class Tutorial : Application
	{
		private const float diskRadius = 50;

		private DrawTargetScreen drawToScreen;
		private Camera3D camera;
		private MaterialLightCollection lights;

		protected override void Initialise()
		{
			camera = new Camera3D();

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			//clear to dark blue
			drawToScreen.ClearBuffer.ClearColour = new Color(20, 20, 40);

			//create the light collection
			lights = new MaterialLightCollection();
			//set a dark blue ambient colour
			lights.AmbientLightColour = new Color(40, 40, 80).ToVector3();

			//positions for two lights
			Vector3[] lightPositions = new Vector3[] 
			{ 
				new Vector3(0, 30, 4), 
				new Vector3(0, -30, 4) 
			};

			//geometry for a light (shared for each light)
			IDraw lightGeometry = null;

			for (int i = 0; i < lightPositions.Length; i++)
			{
				float intensity = 2;
				Color lightColor = Color.LightYellow;
				Color lightSpecularColour = Color.WhiteSmoke;

				//interface to the light about to be created
				IMaterialPointLight light = null;

				//create the point light
				light = lights.CreatePointLight(lightPositions[i], intensity, lightColor, lightSpecularColour);
				
				//Adjusting this value controls how quickly the light falloff occurs.
				//A larger value will produce a slower falloff, and result in a softer, brighter light.
				//A smaller value will produce a darker, but sharper light source.
				//Generally, if you reduce this value, increase the intensity to compensate.
				light.SourceRadius = 4;

				//create the light geometry (a sphere)
				if (lightGeometry == null)
					lightGeometry = new Xen.Ex.Geometry.Sphere(Vector3.One, 8, true, false, false);

				//visually show the light with a light drawer
				var lightSourceDrawer = new LightSourceDrawer(lightPositions[i], lightGeometry,lightColor);

				//add the light geometry to the screen
				drawToScreen.Add(lightSourceDrawer);
			}

			//create the ground disk
			var ground = new GroundDisk(this.Content, lights, diskRadius);

			//then add it to the screen
			drawToScreen.Add(ground);
		}

		protected override void Frame(FrameState state)
		{
			//rotate the camera around the ground plane
			RotateCamera(state);

			//draw everything
			drawToScreen.Draw(state);
		}

		//Override this method to setup the graphics device manager before the application starts.
		//This method is called before Initalise()
		protected override void SetupGraphicsDeviceManager(GraphicsDeviceManager graphics, ref RenderTargetUsage presentation)
		{
			if (graphics != null) // graphics is null when starting within a WinForms host
			{
				graphics.PreferredBackBufferWidth = 1280;
				graphics.PreferredBackBufferHeight = 720;
			}
		}


		private void RotateCamera(IState state)
		{
			var lookAt = new Vector3(0, 0, 0);

			float angle = state.TotalTimeSeconds * 0.15f;

			var lookFrom = new Vector3((float)Math.Sin(angle) * diskRadius, (float)Math.Cos(angle) * diskRadius, 10);
			lookFrom.Z += (float)Math.Sin(angle) * 5;

			camera.LookAt(lookAt, lookFrom, new Vector3(0, 0, 1));
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}

	}


	//this class simply draws the sphere representing the lights
	class LightSourceDrawer : IDraw
	{
		private IDraw geometry;
		private Vector3 position;
		private Color lightColour;

		public LightSourceDrawer(Vector3 position, IDraw geometry, Color lightColour)
		{
			this.position = position;
			this.geometry = geometry;
			this.lightColour = lightColour;
		}

		public void Draw(DrawState state)
		{
			using (state.WorldMatrix.PushTranslateMultiply(ref position))
			{
				DrawSphere(state);
			}
		}

		private void DrawSphere(DrawState state)
		{
			//draw the geometry with a solid colour shader
			if (geometry.CullTest(state))
			{
				var shader = state.GetShader<Xen.Ex.Shaders.FillSolidColour>();

				shader.FillColour = lightColour.ToVector4();

				using (state.Shader.Push(shader))
					geometry.Draw(state);
			}
		}

		public bool CullTest(ICuller culler)
		{
			return true;
		}
	}
}
