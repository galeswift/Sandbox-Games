using System;
using System.Collections.Generic;
using System.Text;



using Xen;
using Xen.Camera;
using Xen.Graphics;
using Xen.Ex.Geometry;
using Xen.Ex.Material;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


/*
 * This sample modifies Tutorial_02 (Draw sphere)
 * This sample demonstrates:
 * 
 * creating a custom vertex and index buffer
 * 
 * see the 'NEW CODE' comments for code that has changed in this tutorial
 * 
 */
namespace Tutorials.Tutorial_04
{
	//NEW CODE
	//A custom vertex structure, storing a position and normal.
	//(An XNA vertex structure could also be used, such as VertexPositionNormalTexture)
	struct CustomVertex
	{
		public Vector3 Position;
		public Vector3 Normal;
		public Vector2 TexCoord;	//texture coord is unused in this example, but is required by MaterialShader
		
		//constructor
		public CustomVertex(Vector3 position, Vector3 normal)
		{
			this.Position = position;
			this.Normal = normal;
			this.TexCoord = new Vector2();
		}
	}


	//NEW CODE
	//a class that draws a quad using custom vertices.
	class QuadGeometry : IDraw
	{
		//vertex and index buffers
		private readonly IVertices vertices;
		private readonly IIndices indices;


		//setup and create the vertices/indices
		public QuadGeometry()
		{
			//create an array of custom vertices to form a quad
			//(this is just a normal C# array)
			CustomVertex[] verts =
			{
				new CustomVertex(new Vector3(-1,-1,0), Vector3.UnitZ), // bottom left
				new CustomVertex(new Vector3(-1, 1,0), Vector3.UnitZ), // top left
				new CustomVertex(new Vector3( 1,-1,0), Vector3.UnitZ), // bottom right
				new CustomVertex(new Vector3( 1, 1,0), Vector3.UnitZ), // top right
			};

			//create the indices data array
			ushort[] inds =
			{
				0,1,2, // first triangle	(bottom left -> top left -> bottom right)
				1,3,2  // second triangle	(top left -> top right -> bottom right)
			};

			//create the vertices/indices objects
			this.vertices = new Vertices<CustomVertex>(verts);
			this.indices = new Indices<ushort>(inds);
		}

		//draw the quad
		public void Draw(DrawState state)
		{
			//draw the vertices as a triangle list, with the indices
			this.vertices.Draw(state, this.indices, PrimitiveType.TriangleList);
		}

		//Fully implement CullTest
		public bool CullTest(ICuller culler)
		{
			//cull test with a bounding box...
			//the box is represented as 'min / max' positions.
			//the vertex positions range from -1,-1,0 to 1,1,0

			//If the camera were changed, and this quad were offscreen, the cull test
			//would return false, and it would not be drawn.
			return culler.TestBox(new Vector3(-1, -1, 0), new Vector3(1, 1, 0));
		}
	}
	


	//a simple class that draws the quad,
	//mostly unchanged from 'SphereDrawer' in tutorial 02
	class GeometryDrawer : IDraw
	{
		private QuadGeometry geometry;
		private Matrix worldMatrix;
		private IShader shader;

		public GeometryDrawer(Vector3 position)
		{
			//NEW CODE
			//create the quad
			this.geometry = new QuadGeometry();

			//setup the world matrix
			this.worldMatrix = Matrix.CreateTranslation(position);

			//create a lighting shader with some average looking lighting :-)
			var material = new MaterialShader();
			material.SpecularColour = Color.LightYellow.ToVector3();//with a nice sheen

			var lightDirection = new Vector3(0.5f,1,-0.5f); //a less dramatic direction

			var lights = new MaterialLightCollection();
			lights.AmbientLightColour = Color.DarkGoldenrod.ToVector3() * 0.5f;
			lights.CreateDirectionalLight(-lightDirection, Color.WhiteSmoke);

			material.LightCollection = lights;

			this.shader = material;
		}

		public void Draw(DrawState state)
		{
			//push the world matrix, multiplying by the current matrix if there is one
			using (state.WorldMatrix.PushMultiply(ref worldMatrix))
			{

				//cull test the custom geometry
				if (geometry.CullTest(state))
				{
					//bind the shader
					using (state.Shader.Push(shader))
					{
						//draw the custom geometry
						geometry.Draw(state);
					}
				}
			}
		}

		//always draw.. don't cull yet
		public bool CullTest(ICuller culler)
		{
			return true;
		}
	}


	//this class hasn't changed...

	//an application that draws geometry in the middle of the screen
	[DisplayName(Name = "Tutorial 04: Custom Geometry")]
	public class Tutorial : Application
	{
		//a DrawTargetScreen is a draw target that draws items directly to the screen.
		//in this case it will only draw a GeometryDrawer
		private DrawTargetScreen drawToScreen;

		protected override void Initialise()
		{
			//draw targets usually need a camera.
			Camera3D camera = new Camera3D();
			//look at the geometry, which will be at 0,0,0
			camera.LookAt(Vector3.Zero, new Vector3(0, 0, 4), Vector3.UnitY);

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;

			//create the geometry
			GeometryDrawer geometry = new GeometryDrawer(Vector3.Zero);

			//add it to be drawn to the screen
			drawToScreen.Add(geometry);
		}

		//main application draw method
		protected override void Frame(FrameState state)
		{
			//draw to the screen.
			drawToScreen.Draw(state);
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}
	}
}
