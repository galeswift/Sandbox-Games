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
 * Drawing a mesh with instancing (if supported)
 * using a Draw proxy to draw instances and perform state batching
 * Using a Space Partitioning class (StaticBinaryTreePartition)
 * 
 * This example will run quite slow on systems without hardware instancing support
 */
namespace Tutorials.Tutorial_16
{

	//in this example, many instances of the same mesh will be drawn.
	//however, to make things more flexible, a proxy will be used for each instance.

	//There will be one 'instance mesh drawer', and many instances.

	//the instances will store their world matrix - and will then notify the mesh drawer
	//if they are visible.

	//After all the instances have been culled or accumulated, the instance mesh drawer
	//will draw all the visible instances in one batch.

	//This way, even if the mesh instances are drawn in an unexpected order,
	//they will still be drawn as a batch.
	//(provided the parent is drawn *after* all instances)

	//this class builds a list of visible instances.
	class DynamicInstancedMeshGeometry : IDraw
	{
		//instance matrices, this will be written every frame
		private Matrix[] instanceMatrices;
		private int instanceCount;
		private IDrawBatch geometry;

		public DynamicInstancedMeshGeometry(int maxInstances, IDrawBatch geometry)
		{
			instanceMatrices = new Matrix[maxInstances];

			//create a sphere for the geometry, as it implements IDrawBatch already
			this.geometry = geometry;
		}

		//each instance will call this culling method
		public bool CullTestInstance(ICuller culler, ref Vector3 translation)
		{
			return culler.TestSphere(1,ref translation);
		}

		//when drawing, the instances will call this method
		public void AddDynamicInstance(DrawState state)
		{
			//store the instance matrix
			state.WorldMatrix.GetMatrix(out instanceMatrices[instanceCount]);

			instanceCount++;
		}

		//draw all the instances
		//Note this class must be drawn after all the instances, to keep drawing in
		//the correct order. Otherwise instance culling may appear a frame delayed.
		public void Draw(DrawState state)
		{
			//get the instancing shader and bind it
			var shader = state.GetShader<Shader.Tutorial16>();

			using (state + shader)
			{
				//in this case, Xen.Ex.Geometry.Sphere implements IDrawBatch - allowing drawing a batch easily
				//otherwise, a call to:
				//
				//  vertices.DrawInstances(...)
				//
				//can be made (internally, the Sphere class does exactly this)

				//Note that all prebuilt shaders in Xen supports hardware instancing, and could be used in place of the custom shader

				//Get a dynamic instance buffer from the DrawState.
				//Pass in the matrix array to fill the buffer with the matrices
				//(Alternatively, get an empty instance buffer and write each matrix one at a time)
				var buffer = state.GetDynamicInstanceBuffer(this.instanceMatrices, this.instanceCount);

				//draw the dynamic instances
				geometry.DrawBatch(state, buffer);

				//reset the counter for the next frame
				instanceCount = 0;
			}
		}

		public bool CullTest(ICuller culler)
		{
			return instanceCount > 0;
		}
	}


	//this class draws a static list of instances,
	//it does no per-instance culling whatsoever - it draws everything in one batch!
	//
	//it has the minimum CPU overhead but a constant GPU load
	//it also does not need to copy the matrix data every frame
	//
	//because of this, it is *significantly* faster on the Xbox than the dynamic culler,
	//however, if even one instance is visible, then they are all drawn (all 25,000!)
	//
	// Be aware; static instancing is not always appropriate. It should typically only
	// be used for very simple geometry (such as this example)
	//
	class StaticInstancedMeshGeometry : IDraw
	{
		private readonly IDrawBatch geometry;
		private readonly InstanceBuffer staticInstanceBuffer;

		private BoundingBox bounds;

		public StaticInstancedMeshGeometry(int maxInstances, IDrawBatch geometry)
		{
			this.geometry = geometry;

			//setup the data for static instancing
			//create the static instance buffer
			this.staticInstanceBuffer = new InstanceBuffer(maxInstances);
		}

		//add a static instance
		public void AddStaticInstance(Matrix matrix, ref BoundingBox worldBounds)
		{
			//update the bounding box for the group of instances
			if (staticInstanceBuffer.InstanceCount == 0)
				this.bounds = worldBounds;
			else
				BoundingBox.CreateMerged(ref worldBounds, ref this.bounds, out this.bounds);

			staticInstanceBuffer.AddInstance(ref matrix);
		}

		//add a static instance
		public void AddStaticInstance(Matrix matrix, float radius)
		{
			//work out the bounding box
			var centre = matrix.Translation;
			var ext = new Vector3(radius, radius, radius);
			var bb = new BoundingBox(centre - ext, centre + ext);

			AddStaticInstance(matrix, ref bb);
		}

		//draw all the instances
		public void Draw(DrawState state)
		{
			//get the instancing shader and bind it
			var shader = state.GetShader<Shader.Tutorial16>();

			using (state + shader)
			{
				//draw everything in one go!
				this.geometry.DrawBatch(state, this.staticInstanceBuffer);
			}
		}

		bool ICullable.CullTest(ICuller culler)
		{
			//cull test on a single bounding box
			return culler.TestBox(ref bounds.Min, ref bounds.Max);
		}
	}

	// This is a simple class that can be toggled on an off, and all children will either draw or not draw
	// This is used to swap static or dynamic drawing of instances
	class ToggleDrawList : IDraw
	{
		public readonly List<IDraw> Children;
		public bool Enabled;

		public ToggleDrawList()
		{
			this.Children = new List<IDraw>();
			this.Enabled = true;
		}

		public void Draw(DrawState state)
		{
			foreach (var child in Children)
			{
				if (child.CullTest(state))
					child.Draw(state);
			}
		}

		bool ICullable.CullTest(ICuller culler)
		{
			return Enabled;	//only draw when enabled
		}
	}




	//this is the proxy that adds an instance of the mesh to the dynamic drawer
	class DynamicMeshInstance : IDraw
	{
		//the parent..
		private readonly DynamicInstancedMeshGeometry parent;
		//the position of the instance (a matrix could also be stored)
		private Vector3 translation;


		public DynamicMeshInstance(DynamicInstancedMeshGeometry parent, Vector3 translation)
		{
			if (parent == null)
				throw new ArgumentNullException();

			this.parent = parent;
			this.translation = translation;
		}

		public void Draw(DrawState state)
		{
			using (state.WorldMatrix.PushTranslateMultiply(ref translation))
			{
				//add the instance
				parent.AddDynamicInstance(state);
			}
		}

		public bool CullTest(ICuller culler)
		{
			return parent.CullTestInstance(culler,ref translation);
		}
	}


	[DisplayName(Name = "Tutorial 16: Instancing and Partitioning")]
	public class Tutorial : Application
	{
		private Camera3D camera;
		private DrawTargetScreen drawToScreen;
		private TextElement statusText;

		//the two lists of drawable objects - based on if dynamic or static instances are being drawn
		private ToggleDrawList dynamicDrawList;
		private ToggleDrawList staticDrawList;

		//this class visually shows the cull tests being performed
		private Xen.Ex.Scene.CullTestVisualizer cullVis;

		protected override void Initialise()
		{
			//create the camera
			Xen.Camera.FirstPersonControlledCamera3D camera = 
				new Xen.Camera.FirstPersonControlledCamera3D(this.UpdateManager,Vector3.Zero);
			camera.Projection.FarClip *= 10;

			this.camera = camera;


			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);

			//25,000 instances
			const int instanceCount = 25000;
			const float areaRadius = 500;

			//setup the two draw lists
			this.staticDrawList = new ToggleDrawList();
			this.dynamicDrawList = new ToggleDrawList();

			//geometry that will be drawn
			var geometry = new Xen.Ex.Geometry.Sphere(Vector3.One, 2, true, false, false);

			//create the mesh instance drawer, (but add it to the screen later)
			var meshDrawer = new DynamicInstancedMeshGeometry(instanceCount, geometry);
			var staticMeshDrawer = new StaticInstancedMeshGeometry(instanceCount, geometry);

			//the dynamicly culled instances are added to a StaticBinaryTreePartition, which 
			//sorts the items into a binary tree, for more efficient culling.
			//This class assumes it's children do not move (ie they are static)

			var sceneTree = new Xen.Ex.Scene.StaticBinaryTreePartition();

			//add it to the dynamic list
			dynamicDrawList.Children.Add(sceneTree);



			//create the instances
			Random random = new Random();

			for (int i = 0; i < instanceCount; i++)
			{
				//create a random position in a sphere
				Vector3 position = new Vector3(	(float)(random.NextDouble()-.5),
												(float)(random.NextDouble()-.5),
												(float)(random.NextDouble()-.5));
				position.Normalize();
				position *= (float)Math.Sqrt(random.NextDouble()) * areaRadius;


				//create the instance
				var instance = new DynamicMeshInstance(meshDrawer, position);

				//add the instance to the StaticBinaryTreePartition
				sceneTree.Add(instance);


				//add the details of this instance to the static drawer
				staticMeshDrawer.AddStaticInstance(Matrix.CreateTranslation(position), 1);
			}

			//now add the drawer (instances will be drawn by the StaticBinaryPartition, before the drawer)
			dynamicDrawList.Children.Add(meshDrawer);

			//now add the static mesh drawer
			staticDrawList.Children.Add(staticMeshDrawer);

			//finally, add them both to the screen
			this.drawToScreen.Add(dynamicDrawList);
			this.drawToScreen.Add(staticDrawList);

			//Note that if the StaticBinaryTreePartition was not used, then 
			//in each frame, every single instance would perform a CullTest to the screen
			//CullTests, despite their simplicity can be very costly in large numbers.
			//The StaticBinaryTreePartition will usually perform a maximum number of CullTests
			//that is approximately ~30% the number of children. (in this case, ~8000 tests)
			//At it's best, when it's entirely off or on screen, it will perform only 1 or 2 CullTests.
			
			//The number of cull tests performed will be displayed in debug builds of this tutorial:

			//add some statusText to display on screen to show the stats
			statusText = new TextElement();
			statusText.Position = new Vector2(50, -50);
			drawToScreen.Add(statusText);

			//add the cull test visualiser
			this.cullVis = new Xen.Ex.Scene.CullTestVisualizer();
			drawToScreen.AddModifier(cullVis);
		}

		//text for what buttons can be pressed
		private string buttonText;

		protected override void InitialisePlayerInput(Xen.Input.PlayerInputCollection playerInput)
		{
			//A and B if using a gamepad..
			string cullButton = "A";
			string staticButton = "B";
			string visButton = "X";

			//get the keys that A and B map to when using the keyboard / mouse
			if (playerInput[PlayerIndex.One].ControlInput == Xen.Input.ControlInput.KeyboardMouse)
			{
				cullButton = playerInput[PlayerIndex.One].KeyboardMouseControlMapping.A.ToString();
				staticButton = playerInput[PlayerIndex.One].KeyboardMouseControlMapping.B.ToString();
				visButton = playerInput[PlayerIndex.One].KeyboardMouseControlMapping.X.ToString();
				playerInput[PlayerIndex.One].InputMapper.CentreMouseToWindow = true;
			}

			//generate the string
			buttonText = string.Format("Hold '{0}' to pause culling\nHold '{1}' to use Static Instance Drawing\nHold '{2}' to show the cull tests being performed", cullButton, staticButton, visButton);
		}

		//load the font used by the status text
		protected override void LoadContent(ContentState state)
		{
			statusText.Font = state.Load<SpriteFont>("Arial");
		}

		protected override void Frame(FrameState state)
		{
			//store the global colour
			state.ShaderGlobals.SetShaderGlobal("colour", Color.Red.ToVector4());

			//set the on screen text
			statusText.Text.Clear();
			//framerate
			statusText.Text += (int)state.ApproximateFrameRate;
			statusText.Text += " fps";

#if DEBUG
			//display some statistics about the render
			DrawStatistics stats;
			state.GetPreviousFrameStatistics(out stats);
			
			statusText.Text += ", ";
			if (state.SupportsHardwareInstancing)
			{
				statusText.Text += stats.InstancesDrawn;
				statusText.Text += stats.InstancesDrawn == 1 ? " instance" : " instances";
				statusText.Text += " drawn (hardware instancing)";
			}
			else
			{
				statusText.Text += stats.DrawIndexedPrimitiveCallCount;
				statusText.Text += stats.DrawIndexedPrimitiveCallCount == 1 ? " instance" : " instances";
				statusText.Text += " drawn";
			}
#endif
			statusText.Text.AppendLine();
			statusText.Text += buttonText;
			
			//draw everything
			drawToScreen.Draw(state);
		}


		protected override void SetupGraphicsDeviceManager(GraphicsDeviceManager graphics, ref RenderTargetUsage presentation)
		{
			if (graphics != null) // graphics is null when starting within a WinForms host
			{
				graphics.PreferredBackBufferWidth = 1280;
				graphics.PreferredBackBufferHeight = 720;
			}
		}

		protected override void Update(UpdateState state)
		{
			//this code is a scene debugging 'hack' that is very useful.
			//when 'PauseFrustumCullPlaneUpdates' is true, the Projection class will not update it's
			//cull planes.
			//However you can still move the camera, and the matrices will update.
			//This allows you to visually debug the culling of the sceneTree.
			//While holding the button down, the culling will not change. Moving the camera around
			//will show any objects 'off screen' that are not being correctly culled
			camera.Projection.PauseFrustumCullPlaneUpdates = (state.PlayerInput[PlayerIndex.One].InputState.Buttons.A);

			//enable or disable static/dynamic instance drawing based on the B button
			staticDrawList.Enabled = state.PlayerInput[0].InputState.Buttons.B;
			dynamicDrawList.Enabled = !state.PlayerInput[0].InputState.Buttons.B;

			cullVis.Enabled = state.PlayerInput[0].InputState.Buttons.X;

			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}
	}
}
