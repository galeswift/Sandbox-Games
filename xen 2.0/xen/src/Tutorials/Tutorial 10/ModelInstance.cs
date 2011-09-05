using System;
using System.Text;
using System.Collections.Generic;



using Xen;
using Xen.Camera;
using Xen.Graphics;
using Xen.Ex.Graphics;
using Xen.Ex.Graphics2D;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


/*
 * This sample extends from Tutorial_01 (Application class)
 * This sample demonstrates:
 * 
 * Loading ModelData and displaying it with a ModelInstance
 * 
 */
namespace Tutorials.Tutorial_10
{
	//NEW CODE
	//Models in Xen.Ex are in two parts, the ModelInstance class and the ModelData class.
	//ModelData is loaded through the XNA Content Pipeline.
	//ModelInstances draw and animate the ModelData

	//The mesh file used here is 'tiny_4anim.x'. This mesh is in the Content project.
	//NOTE: the mesh uses the 'Model - Xen' Content processor!
	//
	//XNA also supports the adobe FBX model format.
	//To export a compatible .X file from 3D Studio max, users have reported the
	//'Panda-X' exporter works the best.
	//
	//To get access to this content processor, the Content project must reference
	//Xen.Ex.ModelImporter.
	//This .dll can be found in ../bin/Xen.Ex.ModelImporter/Xen.Ex.ModelImporter.dll
	//If this .dll is missing, make sure the 'build' batch file has been run

	class Actor : IDraw, IContentOwner
	{
		private ModelInstance model;
		private Matrix worldMatrix;

		public Actor(ContentRegister content, Vector3 position)
		{
			//set the world matrix
			Matrix.CreateTranslation(ref position, out this.worldMatrix);

			//A ModelInstance can be created without any content...
			//However it cannot be used until the content is set

			this.model = new ModelInstance();

			//Note:
			// When a model is drawn, by default it will bind and use a MaterialShader.
			// This shader uses the material properties stored in the original mesh.
			// If you don't want to use the MaterialShader, simply set the property
			// model.ShaderProvider to null.
			

			//add to the content register
			content.Add(this);
		}

		public void Draw(DrawState state)
		{
			using (state.WorldMatrix.Push(ref worldMatrix))
			{
				//ModelData stores accurate bounding box information
				//the ModelInstance uses this to cull the model.

				if (model.CullTest(state))
					model.Draw(state);
			}
		}

		public void LoadContent(ContentState state)
		{
			//load the model data into the model instance
			model.ModelData = state.Load<Xen.Ex.Graphics.Content.ModelData>(@"tiny_4anim");
		}

		public bool CullTest(ICuller culler)
		{
			return true;
		}
	}



	[DisplayName(Name = "Tutorial 10: Model Instance")]
	public class Tutorial : Application
	{
		//screen draw target
		private DrawTargetScreen drawToScreen;

		protected override void Initialise()
		{
			var camera = new Camera3D();
			//Look at the mesh
			camera.LookAt(new Vector3(0, 0, 0), new Vector3(4, 6, 2), new Vector3(0, 0, 1));

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;

			//NEW CODE
			//create the actor instance
			var actor = new Actor(this.Content, Vector3.Zero);

			//add it to the screen
			drawToScreen.Add(actor);
		}

		protected override void Frame(FrameState state)
		{
			drawToScreen.Draw(state);
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}
	}
}
