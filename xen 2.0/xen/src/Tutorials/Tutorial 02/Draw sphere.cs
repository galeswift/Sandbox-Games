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
 * This sample extends from Tutorial_01 (Application class)
 * This sample demonstrates:
 * 
 * drawing objects using the IDraw interface,
 * using a world matrix
 * creating and binding a prebuilt shader
 * using a culler to do on-screen culling
 * using an prebuilt geometry class to draw spherical geometry
 * 
 */
namespace Tutorials.Tutorial_02
{
	//This is a class that draws a sphere. (It uses a prebuilt Sphere geometry in Xen.Ex)
	//It stores a world matrix for the sphere it will draw, a shader used to draw it and the 
	//geometry for the sphere.
	//
	//This class implements the IDraw interface. This means the object will draw geometry.
	//Anything in xen that can be drawn typically implements the IDraw interface.
	//
	//Almost every drawing class in xen implements IDraw.
	//The major exception are vertex buffers (More on that in tutorial 4) and DrawTargets.
	//DrawTargets implement 'IFrameDraw', which provides a method that can only be called
	//at a higher level.
	//
	//Each DrawTarget stores a list of user created IDraw objects. (such as game entities, effects, etc).
	//These objects get drawn to the target during the DrawTarget.Draw() call.
	//If you wish to have an object drawn to the screen, Add() it to the DrawTargetScreen object.
	//This is shown later in this tutorial...

	class SphereDrawer : IDraw
	{
		//This object stores the geometry of a sphere, using a pre-built class located in Xen.Ex.
		//(Xen.Ex.Geometry.Sphere implements IDraw too)
		//Note however that the Sphere class *only* draws the geometry. (It doesn't setup a shader, for example)
		private Xen.Ex.Geometry.Sphere sphereGeometry;

		//world matrix (position, scale, rotation, etc) of the sphere
		private Matrix worldMatrix;

		//shader instance used to display the sphere.
		//all geometry must be drawn with a shader.
		private MaterialShader shader;


		//constructor
		public SphereDrawer(Vector3 position)
		{
			//setup the sphere geometry
			var size = new Vector3(1,1,1);
			//Use the prebuilt sphere geometry class
			this.sphereGeometry = new Sphere(size, 32);

			//Setup the world matrix
			this.worldMatrix = Matrix.CreateTranslation(position);

			//Create a lighting shader with some nice looking lighting
			//'MaterialShader' is a prebuilt class in Xen.Ex. It is similar to the XNA BasicEffect
			//This class implements the IShader interface. All Xen shaders implement IShader.
			this.shader = new MaterialShader();
			this.shader.SpecularColour = Color.LightYellow.ToVector3();						//give the material a nice sheen

			var lightDirection = new Vector3(0.5f,1,-0.5f); //a dramatic direction

			//create a light collection and add a couple of lights to it
			var lights = new MaterialLightCollection();

			lights.AmbientLightColour = Color.CornflowerBlue.ToVector3() * 0.5f;	//set the ambient
			lights.CreateDirectionalLight(lightDirection, Color.Gray);				//add the first of two light sources
			lights.CreateDirectionalLight(-lightDirection, Color.DarkSlateBlue);

			//set the light collection used by the material shader
			this.shader.LightCollection = lights;
		}

		//draw the sphere (This is the method declared in the IDraw interface)
		public void Draw(DrawState state)
		{
			//the DrawState object controls current drawing state for the application.

			//The DrawState uses a number of stacks, it is important to understand how pushing/popping a stack works.

			//
			// In xen, many aspects of rendering are stored in a Stack within the DrawState object.
			// Shaders, for example, are set in state.Shaders.
			//
			// In XNA, when you set a state (such as a render state, shader, texture, etc) then 
			// the previous value is overwritten. By using a stack, you can 'save' the state by
			// calling Push(). This means the current state is pushed to the top of the stack.
			//
			// After rendering, calling Pop() will reset the state back to how it was when Push()
			// was called. This means you can modify the state however you like, and it won't
			// provided you use Push()/Pop() it will not affect the next object being drawn.
			//
			// Xen provides a number of shortcuts to make this even easier,
			// The simplest is the using() statment is setup to atomatically call 'Pop()' for you.
			// This will be shown below:
			//

			//First, push the world matrix, multiplying by the current matrix (if there is one).
			//(This is very similar to using openGL glPushMatrix() and then glMultMatrix())
			//The DrawState object maintains the world matrix stack, pushing and popping this stack is very fast.
			//
			//If the worldMatrix was previous 'A', then ater this call it will be 'A * this.worldMatrix'
			//When this using statement completes, Pop() will be called automatically, and the matrix
			//will be reset to 'A'.
			using (state.WorldMatrix.PushMultiply(ref this.worldMatrix))
			{
				//the 'using' statement is used here, because it is setup to atuomatically call
				//state.WorldMatrix.Pop() when complete.


				//The next line frustum cull tests the sphere (ie, it tests if the sphere is on screen)
				//Culltest will return false if the test fails (in this case false would mean the sphere is off screen)
				//The CullTest method requirs an ICuller to be passed in. Here the state object is used because the 
				//DrawState object implements the ICuller interface (DrawState's culler performs screen culling)
				//The cull test uses the current world matrix, so make sure you perform the CullTest after applying any
				//transformations.
				//The CullTest method is defined by the ICullable interface. 
				//Any IDraw object must also implements ICullable!
				if (sphereGeometry.CullTest(state))
				{
					//the sphere is on screen...

					//Now the shader is setup, it's time to start using it.


					//set the shader that will be used.
					//Shaders are stored in a Stack in the DrawState object.
					using (state.Shader.Push(shader))
					{
						//now everthing is setup draw the sphere geometry!
						sphereGeometry.Draw(state);
					}
				}
			}

			//at this point, both using statements have finished, so the world matrix
			//and shader will be reset back to what they were at the start of this method call.
		}


		//Anything that implements IDraw (such as this class) also implements ICullable.
		//this requires an object implement the CullTest method declared below.
		//CullTest returns true or false.
		//
		//For the majority of cases, CullTest will perform frustum culling ('on-screen culling').
		//A return value of true means on-screen, false means off screen.
		//
		//The 'culler' object passed in is smart. It typically knows the current world matrix.
		//
		//In the example above, the Sphere geometry class ('this.sphereGeometry') internally
		//implements CullTest as:
		//
		// return culler.TestSphere(this.radius);
		//
		//The sphere need not know where it is being drawn (ie, it's current world matrix)
		//because the ICuller already knows this.
		public bool CullTest(ICuller culler)
		{
			//in this case, however, simply return true.
			//this means 'always draw'.

			//This is because in this case, a world-matrix will be applied during the
			//draw() method, which will potentially change the result of the sphereGeometry
			//cull test.
			
			//What this means is that:

			//return sphereGeometry.CullTest(culler);

			//would be invalid here.

			//However,
			//culler.TestSphere(this.sphereGeometry.Radius, this.worldMatrix.Translation);
			//would be correct, assuming the world matrix was not scaling/rotating
			
			return true;
		}
	}


	//This class is an application that draws the sphere in the middle of the screen
	[DisplayName(Name = "Tutorial 02: Draw a Sphere")]
	public class Tutorial : Application
	{
		DrawTargetScreen drawToScreen;

		protected override void Initialise()
		{
			var camera = new Camera3D();

			//look at the sphere, which will be at 0,0,0. Look from 0,0,4. 
			camera.LookAt(Vector3.Zero, new Vector3(0, 0, 4), Vector3.UnitY);

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;

			//create the sphere at 0,0,0
			var sphere = new SphereDrawer(Vector3.Zero);

			//add it to be drawn to the screen
			drawToScreen.Add(sphere);
		}

		//main application draw method
		protected override void Frame(FrameState state)
		{
			//draw to the screen.
			//This causes the sphere to be drawn (because it was added to the screen)
			drawToScreen.Draw(state);
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}
	}
}
