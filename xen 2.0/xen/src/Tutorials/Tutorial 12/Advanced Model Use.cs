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
using Xen.Ex;


/*
 * 
 * This is an advanced tutorial.
 * 
 * 
 * This sample extends from Tutorial_11 (Model animation)
 * This sample demonstrates:
 * 
 * Using the embedded bounding box data stored in a model
 * 
 */
namespace Tutorials.Tutorial_12
{
	//In this tutorial, the bounding boxes of the model will be extracted, and drawn
	//This is an advanced technique

	class Actor : IDraw, IContentOwner
	{
		private ModelInstance model;

		//NEW CODE
		//A list of bounding boxes
		private List<IDraw> boundingBoxes;

		public Actor(ContentRegister content)
		{
			//A ModelInstance can be created without any content...
			//However it cannot be used until the content is set

			model = new ModelInstance();

			content.Add(this);

			//play an animation
			model.GetAnimationController().PlayLoopingAnimation(3);

			//This method creates a Cube for each bone in the mesh
			BuildBoundingBoxGeometry();
		}

		public void Draw(DrawState state)
		{
			//ModelInstances automatically setup the default material shaders
			//Custom shaders can be used with model.SetShaderOverride(...)
			model.Draw(state);

			DrawBoundingBoxes(state);
		}

		//NEW CODE
		private void DrawBoundingBoxes(DrawState state)
		{
			//First, get the animated bone transforms of the model.
			//These transforms are in 'bone-space', not in world space.
			var boneAnimationTransforms = model.GetAnimationController().GetTransformedBones(state);


			//Get a simple shader from Xen.Ex that fills a solid colour
			var shader = state.GetShader<Xen.Ex.Shaders.FillSolidColour>();

			//set the fill colour
			shader.FillColour = Color.White.ToVector4();

			using (state + shader)
			using (state.RenderState.Push())
			{
				//disable back face culling
				state.RenderState.CurrentRasterState.CullMode = CullMode.None;
				//set to wireframe
				state.RenderState.CurrentRasterState.FillMode = FillMode.WireFrame;

				//loop through all the geometry data in the model..
				//(note, the sample model has only 1 geometry instance)


				var modelSkeleton = model.ModelData.Skeleton;
				int boxIndex = 0;

				foreach (var meshData in model.ModelData.Meshes)
				{
					foreach (var geometry in meshData.Geometry)
					{
						//now loop through all bones used by this geometry

						for (int geometryBone = 0; geometryBone < geometry.BoneIndices.Length; geometryBone++)
						{
							//index of the bone (a piece of geometry may not use all the bones in the model)
							int boneIndex = geometry.BoneIndices[geometryBone];

							//get the base transform of the bone (the transform when not animated)
							var boneTransform = modelSkeleton.BoneWorldTransforms[boneIndex];

							//multiply the transform with the animation bone-local transform

							//it would be better to use Transform.Multiply() here to save data copying on the xbox
							boneTransform *= boneAnimationTransforms[boneIndex];

							//push the transform
							using (state.WorldMatrix.PushMultiply(ref boneTransform))
							{
								//draw the box
								if (boundingBoxes[boxIndex].CullTest(state))
									boundingBoxes[boxIndex].Draw(state);

								boxIndex++;
							}
						}
					}
				}
			}
		}

		//this method iterates through the geometry and creates the cubes used to display the bounding boxes
		//this is run when the tutorial starts
		private void BuildBoundingBoxGeometry()
		{
			this.boundingBoxes = new List<IDraw>(); 

			foreach (var meshData in model.ModelData.Meshes)
			{
				foreach (var geometry in meshData.Geometry)
				{
					//now loop through all bones used by this geometry

					for (int geometryBone = 0; geometryBone < geometry.BoneIndices.Length; geometryBone++)
					{
						//index of the bone (a peice of geometry may not use all the bones in the model)
						int boneIndex = geometry.BoneIndices[geometryBone];

						//the bounds of the geometry for the given bone...
						var bounds = geometry.BoneLocalBounds[geometryBone];

						//create the cube.
						//it would probably be best to reuse the one cube for all bones...
						this.boundingBoxes.Add(new Xen.Ex.Geometry.Cube(bounds.Minimum, bounds.Maximum));
					}
				}
			}
		}

		//from here everything is the same as the previous example

		public bool CullTest(ICuller culler)
		{
			return model.CullTest(culler);
		}


		public void LoadContent(ContentState state)
		{
			//load the model data into the model instance
			model.ModelData = state.Load<Xen.Ex.Graphics.Content.ModelData>(@"tiny_4anim");
		}
	}

	[DisplayName(Name = "Tutorial 12: Advanced Mesh Bounding Boxes")]
	public class Tutorial : Application
	{
		//screen draw target
		private DrawTargetScreen drawToScreen;

		protected override void Initialise()
		{
			var camera = new Camera3D();
			camera.LookAt(new Vector3(0, 0, 4), new Vector3(3, 4, 4), new Vector3(0, 0, 1));

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;

			//NEW CODE
			//create the actor instance
			drawToScreen.Add(new Actor(this.Content));
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
