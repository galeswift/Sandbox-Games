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
 * This sample modifies Tutorial_02 (Draw Sphere)
 * This sample demonstrates:
 * 
 * creating and using a custom shader
 * 
 * ---------------------------------------------------------------
 * Before reading further, please read the comments in 'shader.fx'
 * ---------------------------------------------------------------
 * 
 * 
 * see the 'NEW CODE' comments for code that has changed in this tutorial
 * 
 */
namespace Tutorials.Tutorial_03
{
	//this class is mostly the same as the Draw Sphere tutorial,
	//except the shader is hard coded to the custom shader
	class SphereDrawer : IDraw
	{
		private Xen.Ex.Geometry.Sphere sphereGeometry;
		private Matrix worldMatrix;

		//NEW CODE

		//the shader class has been generated in the namespace 'Shader', because the filename is 'shader.fx'.
		//The only technique in the file is named 'Tutorial03Technique'.
		//The class that was generated is Shader.Tutorial03Technique:
		private Shader.Tutorial03Technique shader;

		public SphereDrawer(Vector3 position)
		{
			//setup the sphere
			Vector3 size = new Vector3(1,1,1);
			this.sphereGeometry = new Sphere(size, 32);

			//setup the world matrix
			this.worldMatrix = Matrix.CreateTranslation(position);

			//NEW CODE
			//create an instance of the shader:
			this.shader = new Shader.Tutorial03Technique();

			//Note: All shaders implement the 'IShader' interface
		}

		public void Draw(DrawState state)
		{
			using (state.WorldMatrix.PushMultiply(ref worldMatrix))
			{

				//cull test the sphere (note, the cull test uses the current world matrix)
				if (sphereGeometry.CullTest(state))
				{
					//NEW CODE
					//In this sample, the shader instance is defined in this class, however
					//the draw state can be used to get a shared static instance of a shader. 
					//Getting shader instances in this way can reduce memory usage. Eg:
					//
					// var shader = state.GetShader<Shader.Tutorial03Technique>();
					//

					//compute a scale value that follows a sin wave
					float scaleValue = (float)Math.Sin(state.TotalTimeSeconds) * 0.5f + 1.0f;

					//Set the scale value (scale is declared in the shader source)
					shader.Scale = scaleValue;
					
					//Bind the custom shader instance
					using (state.Shader.Push(shader))
					{
						//draw the sphere geometry
						sphereGeometry.Draw(state);
					}
				}

				//xen will reset the world matrix when the using statement finishes:
			}
		}

		//always draw.. don't cull yet
		public bool CullTest(ICuller culler)
		{
			return true;
		}
	}

	//an application that draws a sphere in the middle of the screen
	[DisplayName(Name = "Tutorial 03: Custom Shader")]
	public class Tutorial : Application
	{
		//a DrawTargetScreen is a draw target that draws items directly to the screen.
		//in this case it will only draw a SphereDrawer
		private DrawTargetScreen drawToScreen;

		protected override void Initialise()
		{
			Camera3D camera = new Camera3D();
			camera.LookAt(Vector3.Zero, new Vector3(0, 0, 4), Vector3.UnitY);

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;

			//create the sphere
			SphereDrawer sphere = new SphereDrawer(Vector3.Zero);

			//add it to be drawn to the screen
			drawToScreen.Add(sphere);
		}

		//main application draw method
		protected override void Frame(FrameState state)
		{
			//NEW CODE
			//set the global float4 attribute named 'colour'  to 1,0,0,1, which is bright red with an alpha of 1.
			state.ShaderGlobals.SetShaderGlobal("colour", new Vector4(1, 0, 0, 1));

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
