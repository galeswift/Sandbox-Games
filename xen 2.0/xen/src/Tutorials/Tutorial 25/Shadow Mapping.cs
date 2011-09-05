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

using Xen.Ex.Graphics.Content;
using Xen.Ex.Material;


/*
 * This sample extends from Tutorial_11 (Model Animation)
 * This sample demonstrates:
 * 
 * How certain Xen features can be used to implement a complex effect
 * 
 */
namespace Tutorials.Tutorial_25
{

	//
	// Please Note:
	//
	// This tutorial does not teach how shadow mapping works.
	// This tutorial shows how xen can be used to structure an effect such as shadow mapping.
	// Xen has no pre-built support for any shadow mapping effects.
	//
	// Shadow mapping requires an advanced understanding of many aspects of computer graphics
	// that goes well beyond the scope of these tutorials. There are many subtlties involved.
	// This tutorial does not attempt to explain any of these advanced concepts.
	//
	// Shadow mapping, in it's simplest form, is an extremely simple technique conceptually. 
	// The complications come from the understanding of these subtle aspects of computer graphics.
	//



	//
	// This tutorial relies heavily on Xen Draw Flags.
	// Draw Flags are intended to simplify managing a scene or object that is rendered multiple times.
	//
	// A Draw Flag can be any Structure or Enum type. The DrawState object stores a stack of DrawFlags,
	// which can be Pushed(), Popped() and set. Draw Flags are very fast to access.
	//
	// For example, the ModelInstance class has a property 'ShaderProvider'. This allows a model
	// to be displayed with a custom shader, using the ModelInstanceShaderProvider class.
	// However, if the scene has a large number of ModelInstance objects, and it is desired to draw
	// all of them with the same shader, it can be tricky or ugly to assign the shader provider for
	// every model, and then revert the setting when drawing is complete.
	//
	// For example, it may be desirable to render the scene with a custom shader that outputs a fog effect.
	// In such a case, traversing the scene, assigning the shader providers, drawing the scene, then finally
	// reverting the shader providers is not only tedious, but it's ugly and potentially error prone.
	//
	// This is where Draw Flags can significantly simplify the process. Draw flags are stored in the
	// DrawState, as a stack, so a Flag can be Pushed() before rendering the fog effect, then Popped()
	// once rendering is complete. The model can check the Flag when it draws (which is very fast), and 
	// adjust it's rendering logic accordingly.
	//
	// In the example above, the ModelShaderProviderFlag structure can be used. This flag simply
	// stores a ShaderProvider instance. When rendering, the ModelInstance class will check the Flag, if it
	// is set, it will use the ShaderProvider stored in the flag, otherwise, it will use its own.
	//
	// Eg:
	//
	// state.DrawFlags.Push(new ModelShaderProviderFlag(...));
	//
	// scene.Draw(state);
	//
	// state.DrawFlags.Pop<ModelShaderProviderFlag>();
	//
	// The MaterialLightCollection class also provides the 'MaterialLightCollectionFlag' structure, which is used
	// by ModelInstance and ModelBatch.
	//
	// (as with other xen stacks, the using() block is also supported)
	//


	//here, a custom Enum is created, which will flag the rendering mode of the tutorial.
	//This will be used as a DrawFlag
	enum TutorialRenderMode
	{
		Default,
		DepthOutput,	// depth will be drawn into the shadow map
		DrawShadow		// the shadow effect will be drawn
	}


	//This class is a ShaderProvider, it overrides the shaders used by a ModelInstance.
	//This class will query the TutorialRenderMode, and bind the required shader.
	//
	//DepthOutput mode will draw the models using the Xen.Ex.Shaders.NonLinearDepthOut shader,
	//these shaders ouput non-linear depth to RGBA.
	//Non linear is easier to manage, as it's the natural output of the shadow map projection matrix.
	//
	//(The linear versions of these shaders output linear depth to Red, and linear depth squared to Green)
	//
	sealed class ShadowOutputShaderProvider : Xen.Ex.Graphics.IModelShaderProvider
	{
		//no change to the shader:
		public IShader BeginModel(DrawState state, MaterialLightCollection lights)
		{
			return null;
		}
		public void EndModel(DrawState state)
		{
		}

		//return the shader to use:
		public IShader BeginGeometry(DrawState state, GeometryData geometry)
		{	
			//query the draw flag, 
			switch (state.DrawFlags.GetFlag<TutorialRenderMode>())
			{
				case TutorialRenderMode.DrawShadow:
				{
					//return the shader that draws the shadow
					var shader = state.GetShader<Shader.ShadowShader>();
					shader.TextureMap = geometry.MaterialData.Texture;
					
					return shader;
				}

				//return the shader that outpus depth
				case TutorialRenderMode.DepthOutput:
					return state.GetShader<Xen.Ex.Shaders.NonLinearDepthOut>();

				default:	//no flag is set, or it is in it's default state
					return null;	//do not change the shader
			}
		}
	}


	//this class creates a very simple disk, which is drawn below the actors
	//this class queries the TutorialRenderMode draw flag as well
	class GroundDisk : IDraw, IContentOwner
	{
		private IVertices vertices;
		private IIndices indices;
		private MaterialShader material;

		public GroundDisk(ContentRegister content, float radius, MaterialLightCollection lights)
		{
			//build the disk
			var vertexData = new VertexPositionNormalTexture[256];
			var indices = new List<int>();

			for (int i = 1; i < vertexData.Length; i++)
			{
				//a bunch of vertices, in a circle!
				float angle = (float)(i-1) / (float)(vertexData.Length-2) * MathHelper.TwoPi;
				Vector3 position = new Vector3((float)Math.Sin(angle), (float)Math.Cos(angle), 0);

				vertexData[i] = new VertexPositionNormalTexture(position * radius, new Vector3(0, 0, 1), new Vector2(position.X, position.Y));
				if (i > 1)
				{
					indices.Add(0);
					indices.Add(i - 1);
					indices.Add(i);
				}
			}
			vertexData[0] = new VertexPositionNormalTexture(new Vector3(), new Vector3(0, 0, 1), new Vector2());

			this.vertices = new Vertices<VertexPositionNormalTexture>(vertexData);
			this.indices = new Indices<int>(indices);

			//create the material, and add to content
			this.material = new MaterialShader();
			this.material.LightCollection = lights;
			this.material.Textures = new MaterialTextures();

			content.Add(this);
		}

		public void Draw(DrawState state)
		{
			//switch rendering mode based on the TutorialRenderMode flag
			switch (state.DrawFlags.GetFlag<TutorialRenderMode>())
			{
				case TutorialRenderMode.DepthOutput:
					//bind the depth output shader
					state.Shader.Push(state.GetShader<Xen.Ex.Shaders.NonLinearDepthOut>());
					break;
				case TutorialRenderMode.DrawShadow:
					//bind the shadow rendering shader
					Shader.ShadowShader shader = state.GetShader<Shader.ShadowShader>();
					shader.TextureMap = material.Textures.TextureMap;
					shader.TextureSampler = material.Textures.TextureMapSampler;
					state.Shader.Push(shader);
					break;
				default:
					//no flag known specified
					state.Shader.Push(material);
					break;
			}

			//draw the ground
			vertices.Draw(state, indices, PrimitiveType.TriangleList);

			state.Shader.Pop();
		}

		public bool CullTest(ICuller culler)
		{
			return true;
		}

		public void LoadContent(ContentState state)
		{
			material.Textures.TextureMap = state.Load<Texture2D>(@"box");
		}
	}

	//similar to the Tutorial 11 actor:
	//note, this class takes no special actions to deal with shadow rendering
	class Actor : IDraw, IContentOwner
	{
		private ModelInstance model;
		private Matrix worldMatrix;

		private AnimationController animationController;
		private AnimationInstance animation;

		public Actor(ContentRegister content, Vector3 position, float animationSpeed, int animationIndex)
		{
			Matrix.CreateRotationZ(1-(float)animationIndex, out this.worldMatrix);
			this.worldMatrix.Translation = position;

			model = new ModelInstance();
			this.animationController = model.GetAnimationController();

			content.Add(this);

			this.animation = this.animationController.PlayLoopingAnimation(animationIndex);
			this.animation.PlaybackSpeed = animationSpeed;
		}

		public void Draw(DrawState state)
		{
			using (state.WorldMatrix.PushMultiply(ref this.worldMatrix))
			{
				model.Draw(state);
			}
		}

		public bool CullTest(ICuller culler)
		{
			return model.CullTest(culler, ref worldMatrix);
		}

		public void LoadContent(ContentState state)
		{
			//load the model data into the model instance
			model.ModelData = state.Load<ModelData>(@"tiny_4anim");
		}
	}


	// This is the object that draws to the shadow map texture.
	// This class controls the rendering of the scene,
	// it sets up the draw flags that make the scene render Depth values
	class ShadowMapDrawer : IDraw
	{
		private ShadowOutputShaderProvider shaderProvider;
		private IDraw scene;

		public ShadowMapDrawer(IDraw scene, ShadowOutputShaderProvider shaderProvider)
		{
			this.scene = scene;
			this.shaderProvider = shaderProvider;
		}

		//get/set the scene
		public IDraw Scene
		{
			get { return scene; }
			set { if (value == null) throw new ArgumentNullException(); scene = value; }
		}

		public void Draw(DrawState state)
		{
			//set the draw flags up, which will control rendering
			//this will make the models render depth
			using (state.DrawFlags.Push(TutorialRenderMode.DepthOutput))					//set the flag to depth output
			using (state.DrawFlags.Push(new ModelShaderProviderFlag(this.shaderProvider)))	//make sure the provider is used
			{
				//draw the scene
				scene.Draw(state);

			}
		}

		public bool CullTest(ICuller culler)
		{
			return scene.CullTest(culler);
		}
	}

	//This class draws the shadow map into the scene.
	//It has the complex job of setting up the shadow shaders.
	class ShadowedSceneDrawer : IDraw
	{
		private ShadowOutputShaderProvider shaderProvider;
		private IDraw scene;
		private DrawTargetTexture2D shadowMapTarget;
		private Vector3 lightColour;

		public ShadowedSceneDrawer(IDraw scene, ShadowOutputShaderProvider shaderProvider, DrawTargetTexture2D shadowMapTarget, Vector3 lightColour)
		{
			this.scene = scene;
			this.shaderProvider = shaderProvider;
			this.shadowMapTarget = shadowMapTarget;
			this.lightColour = lightColour;
		}

			
		public void Draw(DrawState state)
		{
			SetupShadowShader(state);

			//set render mode to shadow map
			using (state.DrawFlags.Push(TutorialRenderMode.DrawShadow))						//set the flag to draw the shadowed light
			using (state.DrawFlags.Push(new ModelShaderProviderFlag(this.shaderProvider)))	//make sure the provider is used
			{

				//Push the shadow map camera as a post-culler.
				//This way, anything not within the frustum of the shadow map
				//camera will not be drawn with the shadow shader
				//if it's not in the view of the shadow map camera, it can't be lit.
				using (state.Cullers.PushPostCuller(this.shadowMapTarget.Camera))
				using (state.RenderState.Push())
				{
					//set an additive blending mode
					state.RenderState.CurrentBlendState = AlphaBlendState.AdditiveSaturate;
					state.RenderState.CurrentDepthState.DepthWriteEnabled = false;

					//draw the shadowed scene
					scene.Draw(state);
				}

			}
		}


		private void SetupShadowShader(DrawState state)
		{
			var shadowCamera = this.shadowMapTarget.Camera;

			//compute the view*projection matrix for the shadow map camera...

			Matrix view, projection, viewProjection;
			shadowCamera.GetViewMatrix(out view);
			shadowCamera.GetProjectionMatrix(out projection, this.shadowMapTarget.Size);

			Matrix.Multiply(ref view, ref projection, out viewProjection);

			//and the view direction
			Vector3 viewDirection;
			shadowCamera.GetCameraViewDirection(out viewDirection);


			//set the matrix and other constants in the shadow mapping shader instances
			var shader = state.GetShader<Shader.ShadowShader>();

			//non-blending shader
			shader.LightColour = lightColour;
			shader.ShadowMap = this.shadowMapTarget.GetTexture();
			shader.SetShadowMapProjection(ref viewProjection);
			shader.SetShadowViewDirection(ref viewDirection);
		}

		public bool CullTest(ICuller culler)
		{
			return true;
		}
	}



	[DisplayName(Name = "Tutorial 25: Shadow Mapping")]
	public class Tutorial : Application
	{
		//screen draw target
		private DrawTargetScreen drawToScreen;

		//a draw target which will have the shadow depth drawn
		private DrawTargetTexture2D drawShadowDepth;

		//a blur filter to blur the shadow depth texture
		private Xen.Ex.Filters.BlurFilter shadowDepthBlurFilter;
		
		//ambient lighting for the scene
		private MaterialLightCollection ambientLight;


		protected override void Initialise()
		{
			//setup ambient lighting
			this.ambientLight = new MaterialLightCollection();
			ambientLight.LightingEnabled = true;
			ambientLight.AmbientLightColour = new Vector3(0.4f, 0.2f, 0.1f);
			ambientLight.CreateDirectionalLight(new Vector3(-1, -1, 0), new Vector3(3,2,1)); // add some backlighting
			ambientLight.SphericalHarmonic.AddLight(new Vector3(2, 0.5f, 0.25f), new Vector3(0, 0, 1), 0.2f);

			//the camera for the shadows point of view, represents the direction of the light.
			Camera3D shadowCamera = new Camera3D();
			shadowCamera.LookAt(new Vector3(1, 1, 3), new Vector3(-15, 20, 20), new Vector3(0, 0, 1));

			//set the clip plane distances
			shadowCamera.Projection.FarClip = 40;
			shadowCamera.Projection.NearClip = 20;
			shadowCamera.Projection.FieldOfView *= 0.25f;


			//8bit is actually enough accuracy for this sample (given the limited range of the shadow)
			var textureFormat = SurfaceFormat.Color;
			const int resolution = 256;

			//create the shadow map texture:
			drawShadowDepth = new DrawTargetTexture2D(shadowCamera, resolution, resolution, textureFormat, DepthFormat.Depth24);
			drawShadowDepth.ClearBuffer.ClearColour = Color.White;

			//for the shadow technique used, the shadow buffer is blurred.
			//this requires an intermediate render target on the PC
			DrawTargetTexture2D blurIntermediate = null;

			//technically not required on the xbox if the render target is small enough to fit in EDRAM in one tile, but xna insists
			blurIntermediate = new DrawTargetTexture2D(shadowCamera, resolution, resolution, textureFormat, DepthFormat.None);

			//create a blur filter
			shadowDepthBlurFilter = new Xen.Ex.Filters.BlurFilter(Xen.Ex.Filters.BlurFilterFormat.SevenSampleBlur,1.0f, drawShadowDepth, blurIntermediate);

			//create the scene camera
			var camera = new Camera3D();
			camera.LookAt(new Vector3(0, 0, 3), new Vector3(10, 10, 6), new Vector3(0, 0, 1));
			camera.Projection.FieldOfView *= 0.55f;

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.Black;

			//the 'scene'
			//A DrawList from Tutorial 23 is used here, this stores the 'scene', 
			//which is just a set of actors and the ground
			Tutorials.Tutorial_23.DrawList scene = new Tutorials.Tutorial_23.DrawList();

			for (int x = 0; x < 2; x++)
			for (int y = 0; y < 2; y++)
			{
				//create the actor instances
				if (x != 0 || y != 0)
					scene.Add(new Actor(this.Content, new Vector3(x*6-3, y*6-3, 0), (x + y*2 + 1) * 0.2f, 4-x*2-y));
			}

			//add the ground
			var ground = new GroundDisk(this.Content, 10, ambientLight);
			scene.Add(ground);


			//setup the draw targets...


			//create the shader provider
			var shadowOutputShaderProvider = new ShadowOutputShaderProvider();

			//add a ShadowMapDrawer to the shadow map texture
			drawShadowDepth.Add(new ShadowMapDrawer(scene, shadowOutputShaderProvider));

			//setup the scene to be drawn to the screen
			//draw the scene normally (no shadow, just ambient)
			drawToScreen.Add(scene);

			Vector3 lightColour = new Vector3(2, 1.5f, 1);

			//then draw the scene with a shadow (blended on top)
			drawToScreen.Add(new ShadowedSceneDrawer(scene, shadowOutputShaderProvider, drawShadowDepth, lightColour));

			//add a nice faded background
			Tutorial_20.BackgroundGradient background = new Tutorial_20.BackgroundGradient(new Color(1, 0.5f, 0.3f), new Color(0.2f, 0.1f, 0.2f));
			background.DrawAtMaxZDepth = true;
			drawToScreen.Add(background);


			//create a textured element that will display the shadow map texture
			var shadowDepthDisplay = new TexturedElement(drawShadowDepth, new Vector2(256, 256));
			shadowDepthDisplay.VerticalAlignment = VerticalAlignment.Top;
			this.drawToScreen.Add(shadowDepthDisplay);
		}

		protected override void Frame(FrameState state)
		{
			//draw the shadow map texture first,
			drawShadowDepth.Draw(state);

			//apply the blur filter to the shadow texture
			shadowDepthBlurFilter.Draw(state);

			//set a global light collection (that all models will use)
			using (state.DrawFlags.Push(new MaterialLightCollectionFlag(ambientLight)))
			{
				//draw the scene to the screen
				drawToScreen.Draw(state);
			}
		}

		protected override void SetupGraphicsDeviceManager(GraphicsDeviceManager graphics, ref RenderTargetUsage presentation)
		{
			if (graphics != null) // graphics is null when starting within a WinForms host
			{
				graphics.PreferredBackBufferWidth = 1280;
				graphics.PreferredBackBufferHeight = 720;
				graphics.PreferMultiSampling = true;
			}
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}
	}
}
