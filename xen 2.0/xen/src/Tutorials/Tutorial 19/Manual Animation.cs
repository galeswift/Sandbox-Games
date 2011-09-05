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
using Xen.Ex.Graphics.Content;

/*
 * This sample extends from Tutorial_11 (Model Animation)
 * This sample demonstrates:
 * 
 * Using an animation override to manually modify animation bones
 * 
 */
namespace Tutorials.Tutorial_19
{

	//NEW CODE
	//this class implements the IAnimationBoneModifier interface.
	//This interface allows this class to modify bone transforms in three methods.
	//
	//Each method allows the bone transforms to be modified. The first two methods
	//may return a boolean which indicates if the animation processor should continue
	//processing the animation. (returning false means 'the animation is complete')
	//
	//It allows the bones to be set before any animation calculations take place at all.
	//Also, after the model animations have been generated, it allows the bones to be
	//modified before and after the bones are transformed into a hierarchy.
	//
	//The bone hierarchy transform multiplies each bone by it's parent transform,
	//For example, changing the transform of the elbow bone would have different results depending if it
	//was changed before or after the hierarchy transform. Changing it before would mean any child bones
	//it has (such as a hand, fingers, etc) will be affected. Changing it after will not change the children.
	//
	//Be aware that for an Async animation controller, these methods may be called on a task thread, so
	//they should be thread safe with any draw or update code
	//
	class AnimationModifier : Xen.Ex.Graphics.IAnimationBoneModifier
	{
		//
		// This sample uses all three methods of the IAnimationBoneModifier interface
		//
		// In this example, there are two features demonstrated.
		//
		// The first type, a single bone is modified before the heirachy transform.
		// The 'spine' bone is rotated. Because this is done before the heirachy transform,
		// this causes the entire upper body to rotate.
		//
		// The second example is a bit silly.
		// The second example uses the two other methods.
		// It uses the third method, the post heirachy transform, to store the positions
		// of all the transformed bones. These positions are stored in a verlet inetegrator
		// In the first method, when enabled, the verlet integrator will generate entirely new
		// bone transforms, replacing the entire animation. This way, the normal model
		// animations need not be generated, and are skipped entirely.
		//

		//a flag to enable modifying the spine bone (first mode)
		private bool enableModifySingleBone;	
		//a flag to enable the verlet surprise (second mode)
		private bool enableVerlet;				
		//rotation value for the spine bone spinner mode
		private float rotationValue;
		// the bone index for the bone to spin
		private readonly int spinBoneIndex; 

		//the nasty hacky verlet solver
		public readonly SimpleVerletSolver verletSolver;

		//construct the modifier
		public AnimationModifier(int spinBoneIndex, ModelData modelData)
		{
			this.spinBoneIndex = spinBoneIndex;
			this.verletSolver = new SimpleVerletSolver(modelData);
		}
		
		//set the first mode enabled (spin the spine)
		public void SetRotateBoneEnabled(bool enabled, float rotationDelta)
		{
			this.enableModifySingleBone = enabled;
			if (enabled)
				this.rotationValue += rotationDelta;
			else
				this.rotationValue = 0;
		}
		//set the verlet model enabled
		public void SetVerletEnabled(bool enabled)
		{
			enableVerlet = enabled;
		}


		//methods of the interface

		//this first method is called before any normal animation calculation takes place
		//the nonAnimatedBones Transforms are usualy still storing the previous frame's animation,
		//however, this is not guarenteed. They should be considered write-only.
		//returning true from this method stops the animation process early
		public bool PreProcessAnimation(Transform[] nonAnimatedBones, ModelData modelData, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityTransforms, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityInverseTransforms)
		{
			//is the hacky verlet example enabled?
			if (enableVerlet)
			{
				//if so, let the verlet integrator generate the bone transforms itself
				verletSolver.IntegrateBones(nonAnimatedBones, boneWorldSpaceIdentityTransforms, boneWorldSpaceIdentityInverseTransforms);

				//the bones are now entirely generated, so return false.

				//return false because the animation is complete (in this example, the animation has been transformed manually)
				return false;
			}

			//returning true continues the animation process,
			//so the animations are generated, interpolated and ProcessBonesPreTransform is called.
			return true;
		}

		//the second method in the animation process.
		//At this point, the normal model animations have been generated.
		//However, the bones have not been transformed into a hierarchy yet.
		public bool ProcessBonesPreTransform(Transform[] boneSpaceTransforms, ModelData modelData, 
			ReadOnlyArrayCollection<Transform> boneWorldSpaceTransforms, ReadOnlyArrayCollection<Transform> boneWorldSpaceInverseTransforms)
		{
			//spin the spine..?
			if (enableModifySingleBone)
			{
				//make a transform to rotate the bone
				Transform transform = Transform.Identity;

				//create a rotation
				Quaternion.CreateFromYawPitchRoll(0, this.rotationValue, 0, out transform.Rotation);

				//modify the bone transform (or you could replace it entirely)
				boneSpaceTransforms[spinBoneIndex] *= transform;
			}

			//returning true continues the animation process, so the hierarchy is transformed and ProcessBonesPostTransform is called
			//return false if animation is complete (for example, the animation has been transformed manually)
			return true;
		}

		//the final method in the interface,
		//when this method is called, the bones have been transformed into their final hierarchy.
		//modifying a bone here will only modify the transform of that bone (and not it's children).
		public void ProcessBonesPostTransform(Transform[] worldSpaceTransforms, ModelData modelData)
		{
			//however, here, simply copy out the positions for each of the bones, and store them in the hacky verlet integrator
			//note: when the verlet system is active, neither this method or the second interface method will be called,
			//as it returns false in the first method.
			verletSolver.UpdateTransforms(worldSpaceTransforms);
		}
	}

	//this class is mostly the same as tutorial 12, except the animation controller has the animation modifier property set
	class Actor : IDraw, IContentOwner
	{
		private ModelInstance model;
		private AnimationController animationController;
		private AnimationInstance animation;
		//NEW CODE
		private AnimationModifier animationModifer;

		public Actor(ContentRegister content, UpdateManager updateManager)
		{
			//load the model
			model = new ModelInstance();
			content.Add(this);

			//create the animation controller
			animationController = model.GetAsyncAnimationController(updateManager);

			//NEW CODE
			//create the animation modifier

			int rotateBoneIndex = model.ModelData.Skeleton.GetBoneIndexByName("Bip01_Spine2");
			this.animationModifer = new AnimationModifier(rotateBoneIndex, model.ModelData);

			//set the modifier on the animation controller
			this.animationController.AnimationBoneModifier = this.animationModifer;

			//play the run animation
			int animationIndex = animationController.AnimationIndex("Jog");
			this.animation = animationController.PlayLoopingAnimation(animationIndex);
		}

		public void Draw(DrawState state)
		{
			model.Draw(state);
		}

		public bool CullTest(ICuller culler)
		{
			return model.CullTest(culler);
		}

		public void LoadContent(ContentState state)
		{
			//load the model data into the model instance
			model.ModelData = state.Load<Xen.Ex.Graphics.Content.ModelData>(@"tiny_4anim");
		}

		public void SetUserInputState(UpdateState state, bool preTransformButtonIsPressed, bool postTransformButtonIsPressed)
		{
			//NEW CODE
			//called from the application, 
			//update the Animation Modifer

			animationModifer.SetRotateBoneEnabled(preTransformButtonIsPressed, state.DeltaTimeSeconds * 4);
			animationModifer.SetVerletEnabled(postTransformButtonIsPressed);
		}
	}

	[DisplayName(Name = "Tutorial 19: Animation Bone Modification")]
	public class Tutorial : Application
	{
		private DrawTargetScreen drawToScreen;
		private Actor actor;
		private TextElement text;

		protected override void Initialise()
		{
			Camera3D camera = new Camera3D();
			camera.LookAt(new Vector3(0, 0, 4), new Vector3(3, 4, 4), new Vector3(0, 0, 1));

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;

			//create the actor
			actor = new Actor(this.Content, this.UpdateManager);

			drawToScreen.Add(actor);

			text = new TextElement();
			text.Position = new Vector2(30, -30); // text is aligned to the top left by default

			drawToScreen.Add(text);
		}

		protected override void InitialisePlayerInput(Xen.Input.PlayerInputCollection playerInput)
		{
			//setup text
			//generate a string to indicate the buttons to hold to adjust the animation
			string button1, button2;
			if (playerInput[PlayerIndex.One].ControlInput == Xen.Input.ControlInput.KeyboardMouse)
			{
				button1 = playerInput[PlayerIndex.One].KeyboardMouseControlMapping.A.ToString();
				button2 = playerInput[PlayerIndex.One].KeyboardMouseControlMapping.B.ToString();
			}
			else
			{
				button1 = "A";
				button2 = "B";
			}

			text.Text.Clear();
			text.Text.AppendFormatLine("Hold '{0}' to spin Tiny's upper body", button1);
			text.Text.AppendFormatLine("Hold '{0}' and Tiny will have an unfortunate accident", button2);
		}

		protected override void LoadContent(ContentState state)
		{
			text.Font = state.Load<SpriteFont>("Arial");
		}

		protected override void Frame(FrameState state)
		{
			drawToScreen.Draw(state);
		}

		protected override void Update(UpdateState state)
		{
			//simple logic to control the actor
			//holding A or B toggles the effects
			actor.SetUserInputState(state,
				state.PlayerInput[PlayerIndex.One].InputState.Buttons.A,
				state.PlayerInput[PlayerIndex.One].InputState.Buttons.B);

			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}
	}









	/// <summary>
	/// <para>A really simple verlet integration solver</para>
	/// <para>see http://www.gamasutra.com/resource_guide/20030121/jacobson_01.shtml for an example</para>
	/// </summary>
	class SimpleVerletSolver
	{
		/*
		 * 
		 * 
		 * This code is here just to provide a fun demonstration of animation modification,
		 * It's not intented to teach anything :-)
		 * 
		 * In other words, Please don't use this class as a base for more complex effects and expect it to work :-)
		 * 
		 */

		//in verlet systems, the position and previous position
		//of a set of points are stored
		private readonly Vector3[] positions, previousPositions;

		//a distance constraint between two points
		private struct Constraint
		{
			public int from, to;
			public float distance;
		}

		//Each bone is represented by 4 position values in the system,
		//one located at the bone centre, and 3 offset from the bone.
		//
		//There is one offset point for each axis, so the rotation of the bone can
		//be calculated. (All these points are connected by distance constraints)
		private const int BonePointCount = 4;
		private static readonly Vector3[] BoneOffsets = new Vector3[BonePointCount]
		{
			new Vector3(0,0,0), //bone centre
			new Vector3(1,0,0), //x-axis
			new Vector3(0,1,0), //y-axis
			new Vector3(0,0,1)  //z-axis
		};

		private readonly Constraint[] constraints;
		private readonly bool[] activeBones;

		private const float gravity = -0.01f;
		private const float groundHeight = -1;


		public SimpleVerletSolver(ModelData modelData)
		{
			var skeleton = modelData.Skeleton;

			//work out the bones used by the model
			this.activeBones = new bool[skeleton.BoneCount];

			foreach (var mesh in modelData.Meshes)
			{
				foreach (var geometry in mesh.Geometry)
				{
					foreach (int bone in geometry.BoneIndices)
						activeBones[bone] = true;
				}
			}


			this.positions = new Vector3[skeleton.BoneCount * BonePointCount];
			this.previousPositions = new Vector3[skeleton.BoneCount * BonePointCount];
			
			//setup the constraints
			var constraintList = new List<Constraint>();

			for (int i = 0; i < skeleton.BoneCount; i++)
			{
				if (activeBones[i]) // create the constraints for reach active bone
					BuildConstraints(i, skeleton, constraintList);
			}

			this.constraints = constraintList.ToArray();
		}

		private void BuildConstraints(int boneIndex, SkeletonData skeleton, List<Constraint> constraintList)
		{
			var bone = skeleton.BoneData[boneIndex];
			Matrix matrix;
			Transform transform;

			//for the child bones connected to this one..
			foreach (int child in bone.Children)
			{
				if (activeBones[child]) // that are in use...
				{
					//create a constraint from each of the 4 positions created for this bone,
					//and connect it to the child's first position
					for (int p = 0; p < BonePointCount; p++)
					{
						transform = skeleton.BoneWorldTransforms[boneIndex];
						transform.GetMatrix(out matrix);

						//work out the world position of this bone (given it has an offset)
						//(so the distance can be calculated)
						Vector3 position;
						Vector3.Transform(ref BoneOffsets[p], ref matrix, out position);


						var constraint = new Constraint();
						
						constraint.to = child * BonePointCount; // to the first child
						constraint.from = boneIndex * BonePointCount + p; //from each offset bone

						constraint.distance = (position - skeleton.BoneWorldTransforms[child].Translation).Length();

						//add the constraint
						constraintList.Add(constraint);
					}
				}
			}

			//now, link up the 4 offset positions to each other.
			for (int p1 = 0; p1 < BonePointCount; p1++)
			{
				for (int p2 = p1 + 1; p2 < BonePointCount; p2++)
				{
					Constraint constraint = new Constraint();
					constraint.to = boneIndex * BonePointCount + p2;
					constraint.from = boneIndex * BonePointCount + p1;

					constraint.distance = (BoneOffsets[p1] - BoneOffsets[p2]).Length();

					constraintList.Add(constraint);
				}
			}
		}


		//read in the positions of the bones
		public void UpdateTransforms(Transform[] boneTransforms)
		{
			//update previous positions
			for (int i = 0; i < previousPositions.Length; i++)
			{
				this.previousPositions[i] = this.positions[i];
			}

			Matrix matrix;
			for (int i = 0; i < boneTransforms.Length; i++)
			{
				//read each bone
				if (activeBones[i])
				{
					boneTransforms[i].GetMatrix(out matrix); //as a matrix.

					//work out the position each offset point from each bone should be, and output to the position value.
					for (int p = 0; p < BonePointCount; p++)
						Vector3.Transform(ref BoneOffsets[p], ref matrix, out this.positions[i * BonePointCount + p]);
				}
			}
		}

		//the actual integration step
		public void IntegrateBones(Transform[] boneTransforms, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityTransforms, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityInverseTransforms)
		{
			//perform the integration (constraints, verlet, etc)
			PerformIntegration();

			//new bone transfomrs will be written to boneTransforms

			Matrix matrix = Matrix.Identity;
			Transform transform = Transform.Identity;

			for (int i = 0; i < boneTransforms.Length; i++)
			{
				if (activeBones[i])
				{
					//for each bone..
					//the centre point (first position)
					Vector3 basis = this.positions[i * BonePointCount];
					//the three positions offset from the centre
					Vector3 x = this.positions[i * BonePointCount + 1];
					Vector3 y = this.positions[i * BonePointCount + 2];
					Vector3 z = this.positions[i * BonePointCount + 3];

					//get the differences to the centre point... 
					x -= basis;
					y -= basis;
					z -= basis;

					//these form the axis of the bone's rotation
					//x-axis of the matrix
					matrix.M11 = x.X;
					matrix.M12 = x.Y;
					matrix.M13 = x.Z;
					//y-axis of the matrix
					matrix.M21 = y.X;
					matrix.M22 = y.Y;
					matrix.M23 = y.Z;
					//z-axis of the matrix
					matrix.M31 = z.X;
					matrix.M32 = z.Y;
					matrix.M33 = z.Z;

					//position
					matrix.M41 = basis.X;
					matrix.M42 = basis.Y;
					matrix.M43 = basis.Z;

					//work out the transform from the matrix...

					//do not perform a validity check in the transform constrctor,
					//as who cares if the matrix isn't orthoganal :-)
					//otherwise it can throw an exception if the matrix is skewed

					transform = new Transform(ref matrix, false);

					//set the transform (multiplied by the inverse of the bone->world transform)
					//as the bones are in world space, needs to be in bone local space
					boneTransforms[i] = (boneWorldSpaceIdentityInverseTransforms[i] * transform);
				}
			}
		}



		private void PerformIntegration()
		{
			//verlet integrate
			for (int i = 0; i < positions.Length; i++)
			{
				Vector3 dif = this.positions[i] - this.previousPositions[i];

				this.previousPositions[i] = this.positions[i];
				this.positions[i] += dif;

				//gravity
				positions[i].Z += gravity;
			}

			//iterate the constraints 10 times (5 or more is usually enough)
			for (int iteration = 0; iteration < 10; iteration++)
			{
				//keep the points above the ground
				for (int i = 0; i < positions.Length; i++)
				{
					if (positions[i].Z < groundHeight)
					{
						previousPositions[i] = positions[i]; // kill their momentum if they are below
						positions[i].Z = groundHeight;
					}
				}

				//solve distance constraints
				for (int i = 0; i < this.constraints.Length; i++)
				{
					Vector3 start = this.positions[this.constraints[i].from];
					Vector3 end = this.positions[this.constraints[i].to];
					Vector3 dif = start - end;

					float len = dif.Length();
					len = (this.constraints[i].distance - len) / len * 0.5f;

					this.positions[this.constraints[i].from] += dif * len;
					this.positions[this.constraints[i].to] -= dif * len;

					//positions will now be the correct distance apart.
					//after a few iterations this usually ballances out over the entire system
				}
			}
		}
	}
}
