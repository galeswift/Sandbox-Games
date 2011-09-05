using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Xen.Graphics;
using Xen.Ex.Graphics.Content;
using Xen.Ex.Material;
using Xen.Ex.Compression;
using Xen.Threading;

namespace Xen.Ex.Graphics
{

	/// <summary>
	/// Draws an XNA Avatar, with animation using either built in XNA animations of animations loaded through the content pipeline using <see cref="AvatarAnimationData"/>
	/// </summary>
	public sealed class AvatarInstance : IDraw, ICullableInstance
	{
		private AvatarAnimationController controller;
		private Microsoft.Xna.Framework.GamerServices.AvatarRenderer avatarRenderer;
		private Microsoft.Xna.Framework.GamerServices.AvatarDescription avatarDescription;
		private MaterialLightCollection lights;
		private readonly bool useLoadingEffect;
		private float scale;

		internal readonly List<KeyValuePair<string,AnimationData>> animationList;
		internal AvatarAnimationData sourceData;
		internal readonly Transform[] bindPoseInverse, bindPoseWorld, bindPoseWorldInverse;
		
		/// <summary>
		/// Gets/Sets/Modifies the expression used by the avatar when drawing
		/// </summary>
		public Microsoft.Xna.Framework.GamerServices.AvatarExpression AvatarExpression;

#if XBOX360
		private bool bindPoseIsDirty;
#else
		private readonly Vector3[] boneVisLines;
		#region bind pose
		private readonly static Matrix[] staticBindPose = new Matrix[]{
new Matrix(-0.9999998f ,4.302114E-16f ,1.192093E-07f ,0f ,4.302114E-16f ,1f ,-1.94289E-16f ,0f ,-1.192093E-07f ,-1.942889E-16f ,-0.9999998f ,0f ,9.494152E-07f ,0.7552034f ,-0.008665388f ,1f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0f ,0f ,-2.910383E-11f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.09005711f ,-0.1072717f ,0.008671193f ,1f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,-0.09005711f ,-0.1072717f ,0.008671193f ,0.9999999f),
new Matrix(1.15f ,0f ,-5.376673E-25f ,0f ,0f ,1.15f ,-2.688337E-25f ,0f ,4.756288E-25f ,2.378144E-25f ,1.3f ,0f ,0f ,0.03059953f ,-2.910383E-11f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0f ,0.09179866f ,-0.007715893f ,0.9999999f),
new Matrix(1f ,0f ,0f ,0f ,0f ,1f ,0f ,0f ,0f ,0f ,1f ,0f ,-7.070368E-06f ,-0.2687335f ,-0.01300271f ,0.9999999f),
new Matrix(1.18f ,0f ,0f ,0f ,0f ,1f ,0f ,0f ,0f ,0f ,1.18f ,0f ,0f ,-0.1343725f ,-0.006499312f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,4.135903E-25f ,0f ,-4.135903E-25f ,1f ,0f ,0f ,-4.135903E-25f ,0f ,1f ,0f ,-7.070368E-06f ,-0.2687335f ,-0.01300271f ,0.9999999f),
new Matrix(1.18f ,4.135903E-25f ,4.880365E-25f ,0f ,-4.880365E-25f ,1f ,0f ,0f ,-4.880365E-25f ,0f ,1.18f ,0f ,0f ,-0.1343725f ,-0.006499312f ,0.9999999f),
new Matrix(1.18f ,0f ,-5.376673E-25f ,0f ,0f ,1f ,-2.688337E-25f ,0f ,4.880365E-25f ,2.067951E-25f ,1.3f ,0f ,0f ,0.06119912f ,-0.005143928f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.005812414f ,-0.2596922f ,-0.02537671f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.007403408f ,0.1224214f ,0.01426828f ,0.9999999f),
new Matrix(1.18f ,0f ,-4.880365E-25f ,0f ,0f ,1f ,-2.440183E-25f ,0f ,4.880365E-25f ,2.067951E-25f ,1.18f ,0f ,0.002913281f ,-0.1298461f ,-0.01268019f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0f ,0.1737709f ,-0.01882024f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.00579828f ,-0.2596922f ,-0.02537671f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,-0.007403407f ,0.1224214f ,0.01426828f ,0.9999999f),
new Matrix(1.18f ,4.135903E-25f ,0f ,0f ,-4.880365E-25f ,1f ,-2.440183E-25f ,0f ,0f ,2.067951E-25f ,1.18f ,0f ,-0.00289914f ,-0.1298461f ,-0.01268019f ,0.9999999f),
new Matrix(1.18f ,0f ,-4.880365E-25f ,0f ,0f ,1f ,-2.440183E-25f ,0f ,4.880365E-25f ,2.067951E-25f ,1.18f ,0f ,0f ,0.04343986f ,-0.004703019f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,-7.071068E-06f ,0.1183337f ,0.02284149f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.1176131f ,5.775504E-05f ,-0.0279201f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.007855958f ,-0.1010247f ,0.1342524f ,1),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.117613f ,5.775318E-05f ,-0.0279201f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.007855952f ,-0.1010247f ,0.1342524f ,0.9999999f),
new Matrix(1.3f ,0f ,-4.880365E-25f ,0f ,0f ,1f ,-2.440183E-25f ,0f ,5.376673E-25f ,2.067951E-25f ,1.18f ,0f ,-7.071068E-06f ,0.03944456f ,0.007605664f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.1696066f ,-0.003995299f ,0.0005878786f ,0.9999999f),
new Matrix(1f ,0f ,-4.880365E-25f ,0f ,0f ,1.18f ,-2.440183E-25f ,0f ,4.135903E-25f ,2.440183E-25f ,1.18f ,0f ,0.0848033f ,-0.00199765f ,0.0002898554f ,0.9999999f),
new Matrix(1f ,0f ,-4.880365E-25f ,0f ,0f ,1.18f ,-2.440183E-25f ,0f ,4.135903E-25f ,2.440183E-25f ,1.18f ,0f ,0f ,0f ,-5.820766E-11f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.1696066f ,-0.003995301f ,0.0005878787f ,0.9999999f),
new Matrix(1f ,4.880365E-25f ,0f ,0f ,-4.135903E-25f ,1.18f ,-2.440183E-25f ,0f ,0f ,2.440183E-25f ,1.18f ,0f ,-0.08480332f ,-0.001997652f ,0.0002898555f ,0.9999999f),
new Matrix(1f ,4.880365E-25f ,0f ,0f ,-4.135903E-25f ,1.18f ,-2.440183E-25f ,0f ,0f ,2.440183E-25f ,1.18f ,0f ,0f ,-1.862645E-09f ,0f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.1027992f ,-0.002424836f ,0.001567673f ,0.9999999f),
new Matrix(1f ,0f ,-4.880365E-25f ,0f ,0f ,1.18f ,-2.440183E-25f ,0f ,4.135903E-25f ,2.440183E-25f ,1.18f ,0f ,0.07710293f ,-0.001824439f ,0.001175754f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.1541988f ,-0.003637314f ,0.002347426f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.1027992f ,-0.002424838f ,0.001567673f ,0.9999999f),
new Matrix(1f ,4.880365E-25f ,0f ,0f ,-4.135903E-25f ,1.18f ,-2.440183E-25f ,0f ,0f ,2.440183E-25f ,1.18f ,0f ,-0.07710292f ,-0.00182444f ,0.001175754f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.1541988f ,-0.003637316f ,0.002347426f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.1083924f ,-0.02864808f ,0.04242924f ,1f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.1101885f ,-0.02752805f ,0.01115742f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.1034568f ,-0.03065729f ,-0.01861204f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.09029046f ,-0.03503358f ,-0.04665053f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.07399872f ,-0.1849946f ,4.082802E-06f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.1110087f ,-0.1110014f ,4.082802E-06f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0f ,0f ,0.009895937f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.1083924f ,-0.02864808f ,0.04242924f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.1101884f ,-0.02752805f ,0.01115742f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.1034568f ,-0.03065729f ,-0.01861204f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.09029045f ,-0.03503358f ,-0.04665053f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.07399871f ,-0.1849946f ,4.08286E-06f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.1110087f ,-0.1110014f ,4.08286E-06f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,0f ,-1.862645E-09f ,0.009895937f ,0.9999999f),
new Matrix(1f ,0f ,0f ,0f ,0f ,1f ,0f ,0f ,0f ,0f ,1f ,0f ,0.04690241f ,-1.862645E-09f ,0.0003796703f ,1f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.04973078f ,0f ,-1.224695E-05f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.04768014f ,0f ,-0.000355173f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.04028386f ,0f ,-0.0003551769f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.06541445f ,-0.03722751f ,0.04949193f ,1f),
new Matrix(1f ,4.135903E-25f ,4.135903E-25f ,0f ,-4.135903E-25f ,1f ,0f ,0f ,-4.135903E-25f ,0f ,1f ,0f ,-0.04690242f ,0f ,0.0003796704f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.04973083f ,-1.862645E-09f ,-1.224689E-05f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.04768026f ,-1.862645E-09f ,-0.0003551729f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.04028386f ,-1.862645E-09f ,-0.0003551766f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.06542858f ,-0.03722751f ,0.04949194f ,0.9999999f),
new Matrix(1f ,0f ,0f ,0f ,0f ,1f ,0f ,0f ,0f ,0f ,1f ,0f ,0.03380674f ,-1.862645E-09f ,1.224864E-05f ,1f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.03515738f ,0f ,-5.820766E-11f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.03452808f ,0f ,1.22448E-05f ,0.9999999f),
new Matrix(1f ,0f ,-4.135903E-25f ,0f ,0f ,1f ,-2.067951E-25f ,0f ,4.135903E-25f ,2.067951E-25f ,1f ,0f ,0.02958536f ,0f ,-2.328306E-10f ,0.9999999f),
new Matrix(1f ,0f ,0f ,0f ,0f ,1f ,0f ,0f ,0f ,0f ,1f ,0f ,0.03209555f ,-0.01738984f ,0.01951019f ,1f),
new Matrix(1f ,4.135903E-25f ,4.135903E-25f ,0f ,-4.135903E-25f ,1f ,0f ,0f ,-4.135903E-25f ,0f ,1f ,0f ,-0.03380674f ,0f ,1.22487E-05f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.03515732f ,-1.862645E-09f ,0f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.03452795f ,-1.862645E-09f ,1.224491E-05f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,0f ,0f ,-4.135903E-25f ,1f ,-2.067951E-25f ,0f ,0f ,2.067951E-25f ,1f ,0f ,-0.02958536f ,-1.862645E-09f ,0f ,0.9999999f),
new Matrix(1f ,4.135903E-25f ,4.135903E-25f ,0f ,-4.135903E-25f ,1f ,0f ,0f ,-4.135903E-25f ,0f ,1f ,0f ,-0.03209561f ,-0.01738983f ,0.01951019f ,0.9999999f),
	};
		#endregion

#endif

		/// <summary>
		/// Construct an avatar instance
		/// </summary>
		/// <param name="avatarDescription">The XNA avatar description. If null, a random description will be used</param>
		public AvatarInstance(Microsoft.Xna.Framework.GamerServices.AvatarDescription avatarDescription) : this(avatarDescription, false)
		{
		}

		/// <summary>
		/// Construct an avatar instance
		/// </summary>
		/// <param name="avatarDescription">The XNA avatar description. If null, a random description will be used</param>
		/// <param name="useLoadingEffect"></param>
		public AvatarInstance(Microsoft.Xna.Framework.GamerServices.AvatarDescription avatarDescription, bool useLoadingEffect)
		{
			if (avatarDescription == null)
				avatarDescription = Microsoft.Xna.Framework.GamerServices.AvatarDescription.CreateRandom();

			this.animationList = new List<KeyValuePair<string, AnimationData>>();

			this.bindPoseInverse = new Transform[Microsoft.Xna.Framework.GamerServices.AvatarRenderer.BoneCount];
			this.bindPoseWorld = new Transform[Microsoft.Xna.Framework.GamerServices.AvatarRenderer.BoneCount];
			this.bindPoseWorldInverse = new Transform[Microsoft.Xna.Framework.GamerServices.AvatarRenderer.BoneCount];

			this.useLoadingEffect = useLoadingEffect;
			this.AvatarDescription = avatarDescription;
			this.AvatarExpression = new Microsoft.Xna.Framework.GamerServices.AvatarExpression();

			for (int i = 0; i < bindPoseWorld.Length; i++)
			{
				bindPoseInverse[i] = Transform.Identity;
				bindPoseWorld[i] = Transform.Identity;
				bindPoseWorldInverse[i] = Transform.Identity;
			}

			this.scale = 1;
			
#if !XBOX360
			this.boneVisLines = new Vector3[this.bindPoseWorld.Length*2];
#else
			this.bindPoseIsDirty = true;
#endif
		}

		/// <summary>
		/// <para>Gets/Sets the lights collection used by to compute the lighting for the avatar</para>
		/// </summary>
		public MaterialLightCollection LightCollection
		{
			get { return lights; }
			set { lights = value; }
		}

		/// <summary>
		/// Gets/Sets the AvatarDescription
		/// </summary>
		public Microsoft.Xna.Framework.GamerServices.AvatarDescription AvatarDescription
		{
			get { return avatarDescription; }
			set
			{
				if (value != avatarDescription)
				{
					if (value != null
#if XBOX360
						&& value.IsValid)
#else
						)
#endif
					{
						avatarDescription = value;

						if (this.avatarRenderer != null)
							this.avatarRenderer.Dispose();
						this.avatarRenderer = null;
					}
					else
						throw new ArgumentNullException("AvatarDescription is null or not valid");
				}
			}
		}

		/// <summary>
		/// Gets/Creates an animation controller for this avatar instance
		/// </summary>
		/// <returns></returns>
		public AvatarAnimationController GetAnimationController()
		{
			if (controller == null)
				controller = new AvatarAnimationController(null, this);
			return controller;
		}

		/// <summary>
		/// <para>Gets/Creates an animation controller that runs as a thread task</para>
		/// <para>Async animations require adding to an <see cref="UpdateManager"/> because their processing is initalised at the end of the update loop</para>
		/// </summary>
		/// <param name="manager"></param>
		/// <returns></returns>
		public AvatarAnimationController GetAsyncAnimationController(UpdateManager manager)
		{
			if (manager == null)
				throw new ArgumentNullException();
			if (controller == null)
				controller = new AvatarAnimationController(manager, this);
			return controller;
		}

		/// <summary>
		/// Draw the avatar.
		/// </summary>
		/// <param name="state"></param>
		public void Draw(DrawState state)
		{
			if (controller != null)
			{
				controller.WaitForAsyncAnimation(state, state.Properties.FrameIndex, true);

				if (controller.IsDisposed)
					controller = null;
			}

			if (controller == null)
				throw new InvalidOperationException("Animation Controller is null");

			if (this.avatarRenderer == null)
				this.avatarRenderer = new Microsoft.Xna.Framework.GamerServices.AvatarRenderer(this.avatarDescription, this.useLoadingEffect);
			
#if XBOX360
			MaterialLightCollection lights = this.lights;

			MaterialLightCollectionFlag lightsFlag;

			state.DrawFlags.GetFlag(out lightsFlag);
			if (lightsFlag.OverrideLightCollection)
				lights = lightsFlag.LightCollection;

			if (lights != null && lights.LightingEnabled)
			{
				//compute the lighting for the avatar...
				//Do this by accumulating the lights in the collection.

				Vector3 centre = new Vector3(0, scale, 0);

				bool isIdentity;
				Matrix world;
				state.WorldMatrix.GetMatrix(out world, out isIdentity);
				if (!isIdentity)
				{
					Vector4 centreT = new Vector4(centre, 1);
					Vector4.Transform(ref centreT, ref world, out centreT);
					if (centreT.W != 0)
					{
						centreT.W = 1.0f / centreT.W;
						centreT.X *= centreT.W;
						centreT.Y *= centreT.W;
						centreT.Z *= centreT.W;
						centreT.W *= centreT.W;
					}
					centre = new Vector3(centreT.X, centreT.Y, centreT.Z);
				}

				Vector4 ambient = lights.ambient * lights.ambient;
				Vector4 directional = new Vector4();
				Vector3 lightDirection = new Vector3();

				//first must iterate all the lights, to get an approximate weighted direction for them
				AccumulateLightDirection(lights.lights, ref centre, ref lightDirection);

				//now the approximate direction of incomming light is known...
				float lightDirectionNormLen = lightDirection.Length();
				float directionalWeight = 0;
				if (lightDirectionNormLen > 0)
				{
					directionalWeight = lightDirectionNormLen * 50 / (scale * scale);
					if (directionalWeight > 1) directionalWeight = 1;

					lightDirectionNormLen = 1.0f / lightDirectionNormLen;
					lightDirection.X *= lightDirectionNormLen;
					lightDirection.Y *= lightDirectionNormLen;
					lightDirection.Z *= lightDirectionNormLen;
				}

				//loop the lights again, accumulating their input
				AccumulateLightContribution(lights.lights, ref centre, ref lightDirection, ref ambient, ref directional, directionalWeight);

				//finally, apply it to the model.
				avatarRenderer.AmbientLightColor = new Vector3((float)Math.Sqrt(ambient.X),(float)Math.Sqrt(ambient.Y),(float)Math.Sqrt(ambient.Z));
				avatarRenderer.LightColor = new Vector3((float)Math.Sqrt(directional.X),(float)Math.Sqrt(directional.Y),(float)Math.Sqrt(directional.Z));
				if (directionalWeight > 0)
					avatarRenderer.LightDirection = lightDirection;
			}
			else
			{
				avatarRenderer.AmbientLightColor = Vector3.One;
				avatarRenderer.LightColor = new Vector3();
			}

			Matrix matrix;
			state.Camera.GetProjectionMatrix(out matrix, state.DrawTarget.Size);
			avatarRenderer.Projection = matrix;

			state.Camera.GetViewMatrix(out matrix);
			avatarRenderer.View = matrix;

			state.WorldMatrix.GetMatrix(out matrix);

			if (scale != 1)
			{
				matrix.M11 *= scale;
				matrix.M12 *= scale;
				matrix.M13 *= scale;
				matrix.M21 *= scale;
				matrix.M22 *= scale;
				matrix.M23 *= scale;
				matrix.M31 *= scale;
				matrix.M32 *= scale;
				matrix.M33 *= scale;
			}

			avatarRenderer.World = matrix;

			if (this.bindPoseIsDirty && avatarRenderer.State == Microsoft.Xna.Framework.GamerServices.AvatarRendererState.Ready)
			{
				CalculateBindPose();
			}

			Microsoft.Xna.Framework.GamerServices.AvatarExpression expression = this.AvatarExpression;
			this.controller.GetExpressionOverride(ref expression);

			state.RenderState.InternalSyncToGraphicsDevice();

			avatarRenderer.Draw(controller.boneList, expression);
#else

			//draw the skeleton as lines... Not terribly efficient but who cares :-)
			//uses a bindpose extracted from the xbox at runtime (staticBindPose)
			if (this.sourceData != null)
			{
				for (int i = 0; i < controller.boneList.Length; i++)
				{
					controller.boneList[i] *= staticBindPose[i];
				}

				this.sourceData.skeleton.TransformHierarchy(controller.boneList);
				for (int i = 0; i < this.sourceData.skeleton.BoneCount; i++)
				{
					int parent = this.sourceData.skeleton.BoneData[i].Parent;
					if (parent != -1)
					{
						this.boneVisLines[i * 2 + 0] = controller.boneList[parent].Translation * scale;
						this.boneVisLines[i * 2 + 1] = controller.boneList[i].Translation * scale;
					}
				}

				Xen.Ex.Shaders.FillSolidColour solid = state.GetShader<Xen.Ex.Shaders.FillSolidColour>();
				solid.FillColour = new Vector4(1, 1, 1, 1);


				using (state.Shader.Push(solid))
				{
					state.DrawDynamicVertices(boneVisLines, PrimitiveType.LineList);
				}
			}
#endif
		}

		private void AccumulateLightDirection(List<ShaderLight> lights, ref Vector3 centre, ref Vector3 lightDirection)
		{
			float invScaleSq = 0.5f / (scale * scale);
			Vector4 rgbScalar = new Vector4(0.3f, 0.5f, 0.2f, 0);

			foreach (ShaderLight light in lights)
			{
				ShaderDirectionalLight directional = light as ShaderDirectionalLight;
				if (directional != null)
				{
					float dot;
					Vector4.Dot(ref rgbScalar, ref directional.colourLinear,out dot);
					lightDirection.X += directional.Direction.X * dot;
					lightDirection.Y += directional.Direction.Y * dot;
					lightDirection.Z += directional.Direction.Z * dot;
				}
				else //must be a point light
				{
					ShaderPointLight pointLight = light as ShaderPointLight;
					if (pointLight != null)
					{
						Vector3 direction = (centre - pointLight.Position);
						float distanceSq = direction.LengthSquared();
						float distance = (float)Math.Sqrt(distanceSq);


						//work out the falloff for the light...
						//formular is:
						//1.0f / (A + d*B + d^2*C);

						float attenuation = pointLight.SourceRadius + distanceSq * pointLight.LightIntensity;

						if (attenuation != 0)
							attenuation = 1.0f / attenuation;
						else
							attenuation = 1;

						//weight the light more as it gets closer to the avatar.
						float weighting = distanceSq * invScaleSq;
						if (weighting > 1) weighting = 1;

						float dot;
						Vector4.Dot(ref rgbScalar, ref pointLight.colourLinear, out dot);
						dot = (dot * weighting * attenuation);

						lightDirection.X += direction.X * dot;
						lightDirection.Y += direction.Y * dot;
						lightDirection.Z += direction.Z * dot;
					}
				}
			}
		}

		private void AccumulateLightContribution(List<ShaderLight> lights, ref Vector3 centre, 
			ref Vector3 lightDirection, ref Vector4 ambientLight, ref Vector4 directionalLight, float directionalWeighting)
		{
			float invScaleSq = 0.5f / (scale * scale);

			foreach (ShaderLight light in lights)
			{
				ShaderDirectionalLight directional = light as ShaderDirectionalLight;
				if (directional != null)
				{
					float weight = Vector3.Dot(lightDirection, directional.Direction) * directionalWeighting;
					if (weight < 0) weight = 0;

					directionalLight += directional.colourLinear * weight;
					ambientLight += directional.colourLinear * (1 - weight);
				}
				else //must be a point light
				{
					ShaderPointLight pointLight = light as ShaderPointLight;
					if (pointLight != null)
					{
						Vector3 direction = (centre - pointLight.Position);
						float distanceSq = direction.LengthSquared();
						float distance = (float)Math.Sqrt(distanceSq);

						//weight the light more as it gets closer to the avatar.
						float weighting = distanceSq * invScaleSq;
						if (weighting > 1) weighting = 1;
						weighting *= directionalWeighting;

						float weight = Vector3.Dot(lightDirection, direction) * weighting / distance;
						if (weight < 0) weight = 0;

						//work out the falloff for the light...
						//formular is:
						//1.0f / (A + d^2*C);

						float attenuation = pointLight.SourceRadius + distanceSq * pointLight.LightIntensity;

						if (attenuation != 0)
							attenuation = 1.0f / attenuation;
						else
							attenuation = 1;

						Vector4 colour = pointLight.colourLinear;

						colour.X *= attenuation;
						colour.Y *= attenuation;
						colour.Z *= attenuation;

						directionalLight += colour * weight;
						ambientLight += colour * ((1 - weight) * 0.45f); //normally the direction only lights half the model at ~varying brightness
					}
				}
			}
		}

		/// <summary>
		/// FrustumCull test the model
		/// </summary>
		/// <param name="culler"></param>
		/// <returns></returns>
		public bool CullTest(ICuller culler)
		{
			//a very approximate bounding box...
			return culler.TestBox(new Vector3(-0.85f * scale, -0.5f * scale, -0.85f * scale), new Vector3(0.85f * scale, 2.5f * scale, 0.85f * scale));
		}


		/// <summary>
		/// FrustumCull test the model at the given location
		/// </summary>
		public bool CullTest(ICuller culler, ref Matrix instance)
		{
			//a very approximate bounding box...
			return culler.TestBox(new Vector3(-0.85f * scale, -0.5f * scale, -0.85f * scale), new Vector3(0.85f * scale, 2.5f * scale, 0.85f * scale), ref instance);
		}

		/// <summary>
		/// Gets/Sets the scale of the drawn Avatar
		/// </summary>
		public float Scale { get { return scale; } set { if (value <= 0) throw new ArgumentException(); scale = value; } }

		/// <summary>
		/// Add an animation source, adding all the animations present in the source file.
		/// </summary>
		public void AddAnimationSource(AvatarAnimationData avatarModelData)
		{
			if (avatarModelData == null)
				throw new ArgumentNullException();

			if (this.sourceData == null)
			{
				this.sourceData = avatarModelData;
			}

			foreach (AnimationData animation in avatarModelData.animations)
			{
				bool added = false;
				for (int i = 0; i < this.animationList.Count; i++)
				{
					if (this.animationList[i].Key == animation.Name)
					{
						added = true;
						this.animationList[i] = new KeyValuePair<string,AnimationData>(animation.Name, animation);
						break;
					}
				}

				if (!added)
					this.animationList.Add(new KeyValuePair<string, AnimationData>(animation.Name, animation));
			}
		}

		/// <summary>
		/// Add a single animation with a specified name from an animation source file. 
		/// </summary>
		/// <returns>Returns the index of the added animation</returns>
		public int AddNamedAnimation(AvatarAnimationData singleAnimationSourceModelData, string animationName)
		{
			if (singleAnimationSourceModelData == null)
				throw new ArgumentNullException();

			if (singleAnimationSourceModelData.animations.Length != 1)
				throw new ArgumentException("singleAnimationSourceModelData must have exactly one animation");

			if (this.sourceData == null)
			{
				this.sourceData = singleAnimationSourceModelData;
			}

			AnimationData animation = singleAnimationSourceModelData.animations[0];

			for (int i = 0; i < this.animationList.Count; i++)
			{
				if (this.animationList[i].Key == animationName)
				{
					this.animationList[i] = new KeyValuePair<string,AnimationData>(animationName, animation);
					return i;
				}
			}

			animationList.Add(new KeyValuePair<string, AnimationData>(animationName, animation));
			return animationList.Count - 1;
		}

#if XBOX360
		private void CalculateBindPose()
		{
			//hijack the controllers bone list....
			Matrix[] pose = this.controller.boneList;

			for (int i = 0; i < Microsoft.Xna.Framework.GamerServices.AvatarRenderer.BoneCount; i++)
				pose[i] = avatarRenderer.BindPose[i];

			for (int i = 0; i < bindPoseWorld.Length; i++)
			{
				bindPoseInverse[i] = new Transform(Matrix.Invert(pose[i]), false);
			}

			if (this.sourceData != null)
				this.sourceData.skeleton.TransformHierarchy(pose);

			for (int i = 0; i < bindPoseWorld.Length; i++)
			{
				bindPoseWorld[i] = new Transform(pose[i], false);
				bindPoseWorldInverse[i] = new Transform(Matrix.Invert(pose[i]), false);
			}

			//recalc the bone list
			this.controller.ProcessAnimation(0);

			this.bindPoseIsDirty = false;
		}
#endif
	}



	/// <summary>
	/// <para>Interface to a class that may modify the transforms of an avatars animation bone hierarchy, through a <see cref="AvatarAnimationController"/></para>
	/// <para>Note: Methods implemented for this interface should be thread safe</para>
	/// </summary>
	public interface IAvatarAnimationBoneModifier
	{
		/// <summary>
		/// <para>Modify the bones of the animation before the animation is processed. Returning false will prevent the animation / blending process from starting.</para>
		/// <para>Use this method to replace the entire animation, and prevent the standard animations from being processed.</para>
		/// <para>Any modifications to <paramref name="nonAnimatedBones"/> will have no effect unless false is returned.</para>
		/// </summary>
		/// <param name="nonAnimatedBones"></param>
		/// <param name="avatarModelData"></param>
		/// <param name="boneWorldSpaceIdentityTransforms"><para>The world space transforms of the bones</para><para>If no animations are playing, all bones will be Identity transforms. These transforms are their world space default transforms</para></param>
		/// <param name="boneWorldSpaceIdentityInverseTransforms"></param>
		/// <returns></returns>
		bool PreProcessAvatarAnimation(Transform[] nonAnimatedBones, AvatarAnimationData avatarModelData, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityTransforms, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityInverseTransforms);
		/// <summary>
		/// <para>Modify the animation bones of a mesh</para>
		/// </summary>
		/// <param name="boneSpaceTransforms"></param>
		/// <param name="boneWorldSpaceIdentityTransforms"><para>The world space transforms of the bones</para><para>If no animations are playing, all bones will be Identity transforms. These transforms are their world space default transforms</para></param>
		/// <param name="boneWorldSpaceIdentityInverseTransforms"></param>
		/// <param name="avatarModelData"></param>
		void ProcessAvatarBones(Transform[] boneSpaceTransforms, AvatarAnimationData avatarModelData, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityTransforms, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityInverseTransforms);

		/// <summary>
		/// <para>When true, <see cref="ProcessReadonlyHierarchyTransformedBones"/> will be called after bones have been processed.</para>
		/// <para>Note: Before calling <see cref="ProcessReadonlyHierarchyTransformedBones"/>, the bones will be transformed by their hieracrchy. This process is not required otherwise. Modifying a bone will have no effect on the rendererd avatar.</para>
		/// <para>This method will be most useful for getting the world position of avatar bones, to place props, etc.</para>
		/// </summary>
		bool CallProcessReadonlyHierarchyTransformedBones { get; }

		/// <summary>
		/// <para>Modify the animation bones of a mesh, after they have been transformed into bone-world space hierarchy</para>
		/// <para>Note: Before calling <see cref="ProcessReadonlyHierarchyTransformedBones"/>, the bones will be transformed by their hieracrchy. This process is not required otherwise. Modifying a bone will have no effect on the rendererd avatar.</para>
		/// <para>This method will be most useful for getting the world position of avatar bones, to place props, etc.</para>
		/// </summary>
		void ProcessReadonlyHierarchyTransformedBones(Transform[] hierarchyTransformedBones, AvatarAnimationData avatarModelData, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityTransforms, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityInverseTransforms);
	}







	/// <summary>
	/// Controls animation streams and calulate transformed bone structures for a model
	/// </summary>
	public sealed class AvatarAnimationController : Processor.AnimationProcessor
	{
		internal readonly Matrix[] boneList;
		private IAvatarAnimationBoneModifier boneModifier, boneModifierBuffer;
		private readonly AvatarInstance parent;

		private bool overrideExpression;
		private Microsoft.Xna.Framework.GamerServices.AvatarExpression expression;

		internal AvatarAnimationController(UpdateManager manager, AvatarInstance parent)
			: base(manager, parent)
		{
			this.boneList = new Matrix[Microsoft.Xna.Framework.GamerServices.AvatarRenderer.BoneCount];

			if (parent == null)
				throw new ArgumentNullException();

			this.parent = parent;

			transformedBones = new Transform[Microsoft.Xna.Framework.GamerServices.AvatarRenderer.BoneCount];
			transformIdentity = new bool[Microsoft.Xna.Framework.GamerServices.AvatarRenderer.BoneCount];

			for (int i = 0; i < transformedBones.Length; i++)
			{
				transformedBones[i] = Transform.Identity;
				transformIdentity[i] = true;
				boneList[i] = Matrix.Identity;
			}

			this.transformOuput = new AnimationTransformArray(transformedBones);
		}

		/// <summary>
		/// Gets/Sets an animation modifier that can modify animation bones before and after the bones are transformed into a hierarchy
		/// </summary>
		public IAvatarAnimationBoneModifier AnimationBoneModifier
		{
			get { return boneModifier; }
			set { boneModifier = value; }
		}

		/// <summary>
		/// <para>Gets an animation index by animation string name. -1 if not found.</para>
		/// <para>Performs a linear search of animations in the animation list</para>
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public override int AnimationIndex(string name)
		{
			for (int i = 0; i < parent.animationList.Count; i++)
			{
				if (parent.animationList[i].Key == name)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Plays an animation that loops continuously, returning a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animationIndex"></param>
		/// <returns></returns>
		public AnimationInstance PlayLoopingAnimation(int animationIndex)
		{
			return PlayLoopingAnimation(animationIndex, 0);
		}


		/// <summary>
		/// Plays an animation that loops continuously, returning a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animationIndex"></param>
		/// <returns></returns>
		/// <param name="fadeInTime">Time, in seconds, to fade the animation in</param>
		public AnimationInstance PlayLoopingAnimation(int animationIndex, float fadeInTime)
		{
			if (animationIndex == -1)
				throw new ArgumentException();
			return this.PlayLoopingAnimationData(parent.animationList[animationIndex].Value, fadeInTime);
		}
		/// <summary>
		/// Plays an animation, returning a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animationIndex"></param>
		/// <returns></returns>
		public AnimationInstance PlayAnimation(int animationIndex)
		{
			return PlayAnimation(animationIndex, 0, 0);
		}
		/// <summary>
		/// Plays an animation, returning a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animationIndex"></param>
		/// <param name="fadeInTime">Time, in seconds, to fade the animation in</param>
		/// <param name="fadeOutTime">Time, in seconds, to fade the animation out</param>
		/// <returns></returns>
		public AnimationInstance PlayAnimation(int animationIndex, float fadeInTime, float fadeOutTime)
		{
			if (animationIndex == -1)
				throw new ArgumentException();
			return PlayAnimationData(parent.animationList[animationIndex].Value, fadeInTime, fadeOutTime);
		}

		/// <summary>
		/// Plays a preset animation provided by the XNA runtime, returning a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animation">The preset animation to play</param>
		/// <param name="useExpressions">When true, the avatar will display the expressions stored in the animation</param>
		/// <returns></returns>
		public AnimationInstance PlayPresetAnimation(Microsoft.Xna.Framework.GamerServices.AvatarAnimationPreset animation, bool useExpressions)
		{
			return PlayPresetAnimation(animation, useExpressions, 0, 0);
		}
		/// <summary>
		/// Plays a preset animation provided by the XNA runtime, returning a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animation">The preset animation to play</param>
		/// <param name="useExpressions">When true, the avatar will display the expressions stored in the animation</param>
		/// <param name="fadeInTime">Time, in seconds, to fade the animation in</param>
		/// <param name="fadeOutTime">Time, in seconds, to fade the animation out</param>
		/// <returns></returns>
		public AnimationInstance PlayPresetAnimation(Microsoft.Xna.Framework.GamerServices.AvatarAnimationPreset animation, bool useExpressions, float fadeInTime, float fadeOutTime)
		{
			if (fadeInTime < 0)
				throw new ArgumentException("fadeInTime");
			if (fadeOutTime < 0)
				throw new ArgumentException("fadeOutTime");
			if (IsDisposed)
				throw new ObjectDisposedException("this");
			AnimationStreamControl control = new AvatarAnimationStreamControl(animation,useExpressions);
			control.Initalise(false, fadeInTime, fadeOutTime);
			animations.Add(control);
			return new AnimationInstance(control);
		}

		/// <summary>
		/// Plays a preset animation provided by the XNA runtime, looping continuously until stopped. Returns a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animation">The preset animation to play</param>
		/// <param name="useExpressions">When true, the avatar will display the expressions stored in the animation</param>
		/// <param name="fadeInTime">Time, in seconds, to fade the animation in</param>
		/// <returns></returns>
		public AnimationInstance PlayPresetLoopingAnimation(Microsoft.Xna.Framework.GamerServices.AvatarAnimationPreset animation, bool useExpressions, float fadeInTime)
		{
			if (fadeInTime < 0)
				throw new ArgumentException("fadeInTime");
			if (IsDisposed)
				throw new ObjectDisposedException("this");
			AnimationStreamControl control = new AvatarAnimationStreamControl(animation, useExpressions);
			control.Initalise(true, fadeInTime, 0);
			animations.Add(control);
			return new AnimationInstance(control);
		}

		/// <summary>
		/// Plays a preset animation provided by the XNA runtime, looping continuously until stopped. Returns a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animation">The preset animation to play</param>
		/// <param name="useExpressions">When true, the avatar will display the expressions stored in the animation</param>
		/// <returns></returns>
		public AnimationInstance PlayPresetLoopingAnimation(Microsoft.Xna.Framework.GamerServices.AvatarAnimationPreset animation, bool useExpressions)
		{
			return PlayPresetLoopingAnimation(animation, useExpressions, 0);
		}

		#region animation processor implementations


		/// <summary></summary>
		protected override void ResetTransforms()
		{
			Transform idenity = Transform.Identity;
			for (int i = 0; i < transformedBones.Length; i++)
				transformedBones[i] = idenity;
		}

		/// <summary>Number of animations stored in this controller</summary>
		public override int AnimationCount
		{
			get
			{
				if (IsDisposed)
					throw new ObjectDisposedException("this");
				return parent.animationList.Count;
			}
		}

		/// <summary></summary>
		protected override void OnBufferAnimationValues()
		{
			this.boneModifierBuffer = boneModifier;
		}

		/// <summary>
		/// <para>Clears <i>all</i> global cached animations for this <see cref="ModelData"/>. The cache helps reduce allocation/garbage build up</para>
		/// <para>Note: This purge will effect all <see cref="AnimationController"/> instances for the current <see cref="ModelData"/> Content</para>
		/// </summary>
		public override void PurgeAnimationStreamCaches()
		{
			foreach (KeyValuePair<string,AnimationData> animation in parent.animationList)
				animation.Value.ClearAnimationStreamCache();
		}



		/// <summary></summary>
		public override AnimationData GetAnimationData(int index)
		{
			if (IsDisposed)
				throw new ObjectDisposedException("this");
			return parent.animationList[index].Value;
		}

		/// <summary></summary>
		protected override bool ContentLoaded
		{
			get { return parent.sourceData != null; }
		}


		/// <summary></summary>
		protected internal override void ComputeAnimationBounds()
		{
			//animation bounds aren't very useful, as the avatar itself can vary in size.
		}

		internal bool GetExpressionOverride(ref Microsoft.Xna.Framework.GamerServices.AvatarExpression expression)
		{
			if (overrideExpression)
				expression = this.expression;
			return overrideExpression;
		}

		/// <summary></summary>
		protected override bool OnBeginProcessAnimation()
		{
			if (boneModifierBuffer != null)
			{
				if (!boneModifierBuffer.PreProcessAvatarAnimation(transformedBones, parent.sourceData,
					new ReadOnlyArrayCollection<Transform>(parent.bindPoseWorld),
					new ReadOnlyArrayCollection<Transform>(parent.bindPoseWorldInverse)))
					return false;
			}
			return true;
		}

		/// <summary></summary>
		protected override void OnEndProcessAnimation()
		{
			Matrix identity = Matrix.Identity;

			overrideExpression = false;
			expression = new Microsoft.Xna.Framework.GamerServices.AvatarExpression();

			foreach (AnimationStreamControl animation in this.animations)
			{
				AvatarAnimationStreamControl anim = animation as AvatarAnimationStreamControl;
				if (anim != null)
				{
					overrideExpression |= anim.GetExpression(ref expression);

					float weight = anim.WeightedScale;

					if (weight == 0)
						continue;

					Matrix matrix;

					if (weight == 1)
					{
						//mult the bone matrices...

						for (int i = 0; i < this.boneList.Length; i++)
						{
							matrix = anim.BoneTransforms[i];
							Transform transform = new Transform(ref matrix);

							Transform.Multiply(ref this.transformedBones[i], ref transform, out this.transformedBones[i]);
						}
					}
					else
					{
						for (int i = 0; i < this.boneList.Length; i++)
						{
							//matrices are annoying to lerp...
							//so.. use a transform.
							matrix = anim.BoneTransforms[i];
							Transform transform = new Transform(ref matrix);
							transform.InterpolateToIdentity(weight);

							Transform.Multiply(ref this.transformedBones[i], ref transform, out this.transformedBones[i]);
						}
					}
				}
			}


			if (this.parent.sourceData != null && boneModifierBuffer != null)
			{
				boneModifierBuffer.ProcessAvatarBones(this.transformedBones, parent.sourceData,
					new ReadOnlyArrayCollection<Transform>(parent.bindPoseWorld),
					new ReadOnlyArrayCollection<Transform>(parent.bindPoseWorldInverse));
			}

			for (int i = 0; i < this.transformedBones.Length; i++)
			{
				this.transformedBones[i].GetMatrix(out this.boneList[i]);
			}

			if (this.parent.sourceData != null && boneModifierBuffer != null &&
				boneModifierBuffer.CallProcessReadonlyHierarchyTransformedBones)
			{
				this.parent.sourceData.skeleton.TransformHierarchy(this.transformedBones);

				boneModifierBuffer.ProcessReadonlyHierarchyTransformedBones(this.transformedBones, parent.sourceData,
					new ReadOnlyArrayCollection<Transform>(parent.bindPoseWorld),
					new ReadOnlyArrayCollection<Transform>(parent.bindPoseWorldInverse));
			}

		}

		#endregion
	}


	internal class AvatarAnimationStreamControl : AnimationStreamControl
	{
		private readonly Microsoft.Xna.Framework.GamerServices.AvatarAnimationPreset animationPreset;
		private readonly Microsoft.Xna.Framework.GamerServices.AvatarAnimation animation;
		private readonly bool useExpression;

		public AvatarAnimationStreamControl(Microsoft.Xna.Framework.GamerServices.AvatarAnimationPreset animation, bool useExpression)
		{
			this.animationPreset = animation;
			this.animation = new Microsoft.Xna.Framework.GamerServices.AvatarAnimation(animation);
			this.useExpression = useExpression;
		}

		public bool GetExpression(ref Microsoft.Xna.Framework.GamerServices.AvatarExpression expressionOut)
		{
			Microsoft.Xna.Framework.GamerServices.AvatarExpression defaultExp = default(Microsoft.Xna.Framework.GamerServices.AvatarExpression);
			Microsoft.Xna.Framework.GamerServices.AvatarExpression expression = this.animation.Expression;
			
			if (useExpression && (
				expression.LeftEye != defaultExp.LeftEye ||
				expression.RightEye != defaultExp.RightEye ||
				expression.Mouth != defaultExp.Mouth ||
				expression.LeftEyebrow != defaultExp.LeftEyebrow ||
				expression.RightEyebrow != defaultExp.RightEyebrow))
			{
				expressionOut = expression;
				return true;
			}
			return false;
		}

		public System.Collections.ObjectModel.ReadOnlyCollection<Matrix> BoneTransforms
		{
			get { return animation.BoneTransforms; }
		}

		public override string AnimationName
		{
			get { return this.animationPreset.ToString(); }
		}

		public override void Interpolate()
		{
			float frameTime = frameTimeBuffer;
			float weighting = weightingBuffer;
			bool looping = loopingBuffer;

			if (weighting == 0)
				return;

			this.animation.CurrentPosition = TimeSpan.FromSeconds(frameTime);
			this.animation.Update(TimeSpan.Zero, looping);
		}

		internal override float Duration()
		{
			return (float)this.animation.Length.TotalSeconds;
		}
	}

}
