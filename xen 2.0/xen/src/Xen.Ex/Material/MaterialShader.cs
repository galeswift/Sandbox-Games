using System;
using System.Collections.Generic;
using System.Text;
using Xen.Graphics;
using Xen.Graphics.ShaderSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xen.Ex.Material
{
	#region Shader Lights

	//internal representation of a light
	class ShaderLight : IMaterialLight
	{
		internal Vector4 position;
		internal Vector4 specular, spcularLinear;
		internal Vector4 colour, colourLinear;
		internal Vector4 attenuation;
		internal float invImportance;
		private float priorityAdjust = 1;
		internal readonly bool isDirectional;

		public ShaderLight()
		{
			this.isDirectional = this is ShaderDirectionalLight;
			this.invImportance = -1;
		}

		internal void CalculateImportance()
		{
			float colMax = Math.Max(Math.Max(colour.X * 50, colour.Y * 60), colour.Z * 35);
			invImportance = colMax * colMax + colourLinear.X * 4 + colourLinear.Y * 6 + colourLinear.Z * 2;
			invImportance += spcularLinear.X * 2 + spcularLinear.Y * 4 + spcularLinear.Z;
			invImportance = 10.0f / (invImportance * priorityAdjust);
		}

		public float PriorityMultiplier
		{
			get { return priorityAdjust; }
			set 
			{
				if (value != priorityAdjust)
				{
					priorityAdjust = value;
					invImportance = -1;
				}
			}
		}

		public Vector3 Colour
		{
			get { return new Vector3(colour.X, colour.Y, colour.Z); }
			set
			{
				if (colour.X != value.X ||
					colour.Y != value.Y ||
					colour.Z != value.Z)
				{
					colour.X = value.X; colour.Y = value.Y; colour.Z = value.Z;
					//mul by 3 to get roughly equivalent results to old style MaterialShader
					colourLinear.X = value.X * value.X * 3;
					colourLinear.Y = value.Y * value.Y * 3;
					colourLinear.Z = value.Z * value.Z * 3;
					invImportance = -1;
				}
			}
		}

		public Vector3 SpecularColour
		{
			get { return new Vector3(specular.X, specular.Y, specular.Z); }
			set
			{
				if (specular.X != value.X ||
					specular.Y != value.Y ||
					specular.Z != value.Z)
				{
					specular.X = value.X; specular.Y = value.Y; specular.Z = value.Z;
					spcularLinear.X = value.X * value.X * 3;
					spcularLinear.Y = value.Y * value.Y * 3;
					spcularLinear.Z = value.Z * value.Z * 3;
					invImportance = -1;
				}
			}
		}

		public float SpecularPowerScaler
		{
			get { return specular.W; }
			set 
			{
				if (specular.W != value)
				{
					specular.W = value;
					spcularLinear.W = value;
					invImportance = -1;
				}
			}
		}
	}

	/// <summary>
	/// Interface to a light created in a <see cref="MaterialLightCollection"/>
	/// </summary>
	public interface IMaterialLight
	{
		/// <summary>
		/// Gets/Sets the diffuse colour of the light (Vector3)
		/// </summary>
		Vector3 Colour
		{
			get;
			set;
		}
		/// <summary>
		/// Gets/Sets the specular colour of the light (Vector3)
		/// </summary>
		Vector3 SpecularColour
		{
			get;
			set;
		}
		/// <summary>
		/// Gets/Sets a scale factor for the material specular power (default is 1, no change)
		/// </summary>
		float SpecularPowerScaler
		{
			get;
			set;
		}

		/// <summary>
		/// <para>(Advanced) Gets/Sets a floating point value that can be used to adjust the rendering priority of a light.</para>
		/// <para>When a material shader cannot display all lights directly, lights it deems low priority will be displayed as ambient lighting</para>
		/// <para>This priority is calculated by the brightness and distance of the light. Adjust this value to bias the priority of this specific light.</para>
		/// <para>This value defaults to 1</para>
		/// </summary>
		float PriorityMultiplier
		{
			get;
			set;
		}
	}
	/// <summary>
	/// Interface to a point light stored in a <see cref="MaterialLightCollection"/>
	/// </summary>
	public interface IMaterialPointLight : IMaterialLight
	{
		/// <summary>
		/// Gets/Sets the position of the point light
		/// </summary>
		Vector3 Position { get; set; }
		/// <summary>
		/// Gets/Sets the light anttenuation (falloff) source radius. The light is simulated as a spherical source.
		/// <para>The larger the radius, the brighter the light and softer the falloff will be</para>
		/// <para>This value must be greater than 0</para>
		/// </summary>
		float SourceRadius { get; set; }
		/// <summary>
		/// Gets/Sets the light intensity
		/// </summary>
		float LightIntensity { get; set; }
	}
	/// <summary>
	/// Interface to a directional light stored in a <see cref="MaterialLightCollection"/>
	/// </summary>
	public interface IMaterialDirectionalLight : IMaterialLight
	{
		/// <summary>
		/// Gets/Sets the direction of the light
		/// </summary>
		Vector3 Direction { get; set; }
	}

	#region interface internal implementations

	sealed class ShaderPointLight : ShaderLight, IMaterialPointLight
	{
		public ShaderPointLight(Vector3 position, Vector3 colour, float intensity)
		{
			this.Position = position;
			this.Colour = colour;
			this.SpecularColour = colour;
			this.SpecularPowerScaler = 1;
			this.position.W = 1;

			this.attenuation = new Vector4(1, 1, 1, 1);
			this.LightIntensity = intensity;
		}

		public float SourceRadius
		{
			get { return attenuation.X; }
			set 
			{
				if (value <= 0)
					throw new ArgumentException();
				if (attenuation.X != value)
				{
					attenuation.X = value;
					attenuation.Z = 1.0f / value;
				}
			}
		}

		public float LightIntensity
		{
			get { return attenuation.Y; }
			set 
			{
				if (attenuation.Y != value)
				{
					attenuation.Y = value;
					attenuation.W = value > 0.00001f ? 1.0f / value : 100000f;
				}
			}
		}

		public Vector3 Position
		{
			get { return new Vector3(position.X, position.Y, position.Z); }
			set
			{
				if (position.X != value.X ||
					position.Y != value.Y ||
					position.Z != value.Z)
				{
					position.X = value.X; position.Y = value.Y; position.Z = value.Z;
				}
			}
		}
	}

	sealed class ShaderDirectionalLight : ShaderLight, IMaterialDirectionalLight
	{
		public ShaderDirectionalLight(Vector3 direction, Vector3 colour)
		{
			this.Direction = direction;
			this.Colour = colour;
			this.SpecularColour = colour;
			this.SpecularPowerScaler = 1;
			this.position.W = 0;
			this.attenuation = new Vector4(1, 1, 1, 1);
		}
		public Vector3 Direction
		{
			get { return new Vector3(position.X, position.Y, position.Z); }
			set
			{
				value.Normalize();
				if (position.X != value.X ||
					position.Y != value.Y ||
					position.Z != value.Z)
				{
					position.X = value.X; position.Y = value.Y; position.Z = value.Z;
				}
			}
		}

	}

	#endregion

	/// <summary>
	/// <para>A structure that can be used as a Draw Flag to have compatible classes (MaterialShader) to use a specific <see cref="MaterialLightCollection"/></para>
	/// </summary>
	public struct MaterialLightCollectionFlag
	{
		/// <summary>
		/// <para>Force drawn objects to use <see cref="LightCollection"/></para>
		/// </summary>
		public bool OverrideLightCollection;
		/// <summary>
		/// Get/Set the light collection to use
		/// </summary>
		public MaterialLightCollection LightCollection;

		/// <summary></summary>
		public MaterialLightCollectionFlag(MaterialLightCollection lightCollection)
		{
			this.OverrideLightCollection = true;
			this.LightCollection = lightCollection;
		}
	}

	/// <summary>
	/// Stores a collection of lights used by one or more <see cref="MaterialShader"/> instance
	/// </summary>
	public sealed class MaterialLightCollection
	{
		internal readonly List<ShaderLight> lights = new List<ShaderLight>();
		/// <summary>
		/// Gets/Sets the ambient lighting spherical harmonic used by a <see cref="MaterialShader"/>
		/// </summary>
		public SphericalHarmonicL1RGB SphericalHarmonic;
		internal Vector4 ambient;
		internal bool enabled;

		/// <summary>
		/// Construct a shader lights collection
		/// </summary>
		public MaterialLightCollection() : this(true)
		{
		}

		/// <summary>
		/// Construct a shader lights collection
		/// </summary>
		/// <param name="enabled"></param>
		public MaterialLightCollection(bool enabled)
		{
			this.enabled = enabled;
		}

		/// <summary>
		/// Gets/Sets if lighting is enabled
		/// </summary>
		public bool LightingEnabled
		{
			get { return enabled; }
			set { enabled = value; }
		}

		/// <summary>
		/// Gets the number of lights
		/// </summary>
		public int LightCount
		{
			get { return lights.Count; }
		}

		/// <summary>
		/// Gets/Sets the ambient light colour (Vector3)
		/// </summary>
		public Vector3 AmbientLightColour
		{
			get { return new Vector3(ambient.X,ambient.Y,ambient.Z); }
			set
			{
				ambient.X = value.X;
				ambient.Y = value.Y;
				ambient.Z = value.Z;
			}
		}

		#region internal list management
		bool AddLight(ShaderLight light, bool validate)
		{
			if (validate)
			{
				if (lights.Contains(light))
					return false;
			}

			lights.Add(light);

			return true;
		}

		bool RemoveLightIndex(ShaderLight light)
		{
			return lights.Remove(light);
		}
		#endregion

		/// <summary>
		/// Creates and adds a point light source to the light collection
		/// </summary>
		/// <param name="intensity">The intensity of the light source</param>
		/// <param name="position">Position of the lightsource</param>
		/// <param name="colour">Colour of the lightsource</param>
		/// <returns></returns>
		public IMaterialPointLight CreatePointLight(Vector3 position, float intensity, Vector3 colour)
		{
			IMaterialPointLight light = new ShaderPointLight(position, colour, intensity);
			AddLight((ShaderLight)light, false);
			return light;
		}
		/// <summary>
		/// Creates and adds a point light source to the light collection
		/// </summary>
		/// <param name="position">Position of the lightsource</param>
		/// <param name="intensity">The intensity of the light source</param>
		/// <param name="colour">Colour of the lightsource</param>
		/// <returns></returns>
		public IMaterialPointLight CreatePointLight(Vector3 position, float intensity, Color colour)
		{
			IMaterialPointLight light = new ShaderPointLight(position, colour.ToVector3(), intensity);
			AddLight((ShaderLight)light, false);
			return light;
		}

		/// <summary>
		/// Creates and adds a directional (infinite) light source to the light collection
		/// </summary>
		/// <param name="direction">Direction of the lightsource</param>
		/// <param name="colour">Colour of the lightsource</param>
		/// <returns></returns>
		public IMaterialDirectionalLight CreateDirectionalLight(Vector3 direction, Vector3 colour)
		{
			IMaterialDirectionalLight light = new ShaderDirectionalLight(direction, colour);
			AddLight((ShaderLight)light, false);
			return light;
		}
		/// <summary>
		/// Creates and adds a directional (infinite) light source to the light collection
		/// </summary>
		/// <param name="direction">Direction of the lightsource</param>
		/// <param name="colour">Colour of the lightsource</param>
		/// <returns></returns>
		public IMaterialDirectionalLight CreateDirectionalLight(Vector3 direction, Color colour)
		{
			IMaterialDirectionalLight light = new ShaderDirectionalLight(direction, colour.ToVector3());
			AddLight((ShaderLight)light, false);
			return light;
		}


		/// <summary>
		/// Creates and adds a point light source to the light collection
		/// </summary>
		/// <param name="position">Position of the lightsource</param>
		/// <param name="intensity">The intensity of the light source</param>
		/// <param name="colour">Colour of the lightsource</param>
		/// <param name="specularColour">Specular colour of the lightsource (Specular is direct light reflection)</param>
		/// <returns></returns>
		public IMaterialPointLight CreatePointLight(Vector3 position, float intensity, Vector3 colour, Vector3 specularColour)
		{
			IMaterialPointLight light = new ShaderPointLight(position, colour, intensity);
			AddLight((ShaderLight)light, false);
			light.SpecularColour = specularColour;
			return light;
		}
		/// <summary>
		/// Creates and adds a point light source to the light collection
		/// </summary>
		/// <param name="position">Position of the lightsource</param>
		/// <param name="intensity">The intensity of the light source</param>
		/// <param name="colour">Colour of the lightsource</param>
		/// <param name="specularColour">Specular colour of the lightsource (Specular is direct light reflection)</param>
		/// <returns></returns>
		public IMaterialPointLight CreatePointLight(Vector3 position, float intensity, Color colour, Color specularColour)
		{
			IMaterialPointLight light = new ShaderPointLight(position, colour.ToVector3(), intensity);
			AddLight((ShaderLight)light, false);
			light.SpecularColour = specularColour.ToVector3();
			return light;
		}

		/// <summary>
		/// Creates and adds a directional (infinite) light source to the light collection
		/// </summary>
		/// <param name="direction">Direction of the lightsource</param>
		/// <param name="colour">Colour of the lightsource</param>
		/// <param name="specularColour">Specular colour of the lightsource (Specular is direct light reflection)</param>
		/// <returns></returns>
		public IMaterialDirectionalLight CreateDirectionalLight(Vector3 direction, Vector3 colour, Vector3 specularColour)
		{
			IMaterialDirectionalLight light = new ShaderDirectionalLight(direction, colour);
			AddLight((ShaderLight)light, false);
			light.SpecularColour = specularColour;
			return light;
		}
		/// <summary>
		/// Creates and adds a directional (infinite) light source to the light collection
		/// </summary>
		/// <param name="direction">Direction of the lightsource</param>
		/// <param name="colour">Colour of the lightsource</param>
		/// <param name="specularColour">Specular colour of the lightsource (Specular is direct light reflection)</param>
		/// <returns></returns>
		public IMaterialDirectionalLight CreateDirectionalLight(Vector3 direction, Color colour, Color specularColour)
		{
			IMaterialDirectionalLight light = new ShaderDirectionalLight(direction, colour.ToVector3());
			AddLight((ShaderLight)light, false);
			light.SpecularColour = specularColour.ToVector3();
			return light;
		}

		/// <summary>
		/// Add an existing light to the shader, returns false if the light was already added
		/// </summary>
		/// <param name="light"></param>
		public bool AddLight(IMaterialLight light)
		{
			if (light is ShaderLight == false)
				throw new ArgumentNullException("light is null or not created by a call to CreatePointLight or CreateDirectionalLight");
			return AddLight((ShaderLight)light, true);
		}
		/// <summary>
		/// Remove a light from the shader
		/// </summary>
		public bool RemoveLight(IMaterialLight light)
		{
			if (light is ShaderLight)
				return false;
			return RemoveLightIndex(light as ShaderLight);
		}
		/// <summary>
		/// Removes all lights from this shader
		/// </summary>
		public void RemoveAllLights()
		{
			this.lights.Clear();
		}
	}

	#endregion

	#region shader cache

	sealed class ShaderCache
	{
		private readonly IMS_Base[] Shaders = new IMS_Base[32];
		private readonly ShaderInstance[] ShaderInstances = new ShaderInstance[32];
		public static readonly string TypeName = typeof(ShaderCache).FullName;

		public readonly static int MaxBones = 72;

		//pre-store the draw flags
		public readonly Xen.Graphics.Stack.DrawFlagValue<MaterialLightCollectionFlag>	MaterialLightCollectionFlag;
		public readonly Xen.Graphics.Stack.DrawFlagValue<MaterialTexturesFlag>			MaterialTexturesFlag;
		public readonly Xen.Graphics.Stack.DrawFlagValue<MaterialFogStateFlag>			MaterialFogStateFlag;
		public readonly Xen.Graphics.Stack.DrawFlagValue<LightingDisplayModelFlag>		LightingDisplayModelFlag;

		//default textures to use if the textures are null
		public readonly MaterialTextures defaultTextures;

		public ShaderCache(DrawState state)
		{

			this.MaterialLightCollectionFlag = state.DrawFlags.GetFlagReference<MaterialLightCollectionFlag>();
			this.MaterialTexturesFlag = state.DrawFlags.GetFlagReference<MaterialTexturesFlag>();
			this.MaterialFogStateFlag = state.DrawFlags.GetFlagReference<MaterialFogStateFlag>();
			this.LightingDisplayModelFlag = state.DrawFlags.GetFlagReference<LightingDisplayModelFlag>();

			defaultTextures = new MaterialTextures();
			defaultTextures.texture = state.Properties.WhiteTexture;
			defaultTextures.normalmap = state.Properties.FlatNormalMapTexture;
			defaultTextures.emissiveTexture = state.Properties.WhiteTexture;

		}

		public ShaderInstance GetShader(DrawState state, int lightCount, bool perPixel, bool vertexCol)
		{
			if (lightCount == 0)
				perPixel = false;

			bool smooth = false;
			if (perPixel && lightCount == 3)
				smooth = true;
			if (!perPixel && lightCount == 4)
				smooth = true;

			int index = lightCount | (perPixel ? 8 : 0) | (vertexCol ? 16 : 0);

			ShaderInstance instance = ShaderInstances[index];

			if (instance == null)
			{
				//right, find the shader and create it
				if (perPixel)
				{
					if (vertexCol)
					{
						if (lightCount == 1) Shaders[index] = new ps1nc();
						if (lightCount >= 2) Shaders[index] = new ps2nc();
					}
					else
					{
						if (lightCount == 1) Shaders[index] = new ps1n();
						if (lightCount >= 2) Shaders[index] = new ps2n();
					}
				}
				else
				{
					//per-vertex:

					if (vertexCol)
					{
						if (lightCount == 0) Shaders[index] = new vs0c();
						if (lightCount == 1) Shaders[index] = new vs1c();
						if (lightCount >= 2) Shaders[index] = new vs3c();
					}
					else
					{
						if (lightCount == 0) Shaders[index] = new vs0();
						if (lightCount == 1) Shaders[index] = new vs1();
						if (lightCount >= 2) Shaders[index] = new vs3();
					}
				}
				IMS_Base shader = Shaders[index];
				if (shader == null) throw new ArgumentNullException();

				ShaderInstances[index] = new ShaderInstance(this, shader, (uint)lightCount, smooth);
			}

			return ShaderInstances[index];
		}
	}

	#endregion



	sealed class ShaderInstance
	{
		//internally used structure to compute the most important lights
		struct PriorityLight : IComparable<PriorityLight>
		{
			public ShaderLight light;
			public float invPriority;

			public int CompareTo(PriorityLight other)
			{
#if XBOX360
				//not sure why, but the line below can occasionally cause Array.Sort to crash on the xbox (?!)
				return invPriority.CompareTo(other.invPriority);
#else
				return other.invPriority > invPriority ? -1 : 1;
#endif
			}
		}

		//internal shader interfaces (all will be point to the same shader instance)
		public readonly IMS_Base instance;
		private readonly IMS_PerPixel pixel;
		private readonly IMS_PerVertex vertex;
#if XBOX360
		public readonly BaseShader instanceBase;
#endif

		private readonly ShaderCache parent;

		//number of direct lights the shader supports
		private readonly uint lightCount;

		//buffer for storing light information to pass to a shader
		private readonly Vector4[] buffer;

		//high priority 'close' lights
		private readonly PriorityLight[] closeLights;
		
		//true if temporal smoothing is applied
		private readonly bool smooth;

		//state of PS constants, to avoid uneeded changes.
		private Vector4 currentFogGamma, currentEmissive;
		private float currentAlpha;

		//current bound
		private MaterialShader currentParent;
		private MaterialFogState currentFog;
		private int fogIndex;

		public ShaderInstance(ShaderCache parent, IShader shader, uint lightCount, bool smooth)
		{
			this.parent = parent;
			this.instance = shader as IMS_Base;
#if XBOX360
			this.instanceBase = shader as BaseShader;
#endif
			this.pixel = lightCount > 0 ? shader as IMS_PerPixel : null;
			this.vertex = lightCount > 0 ? shader as IMS_PerVertex : null;
			this.lightCount = smooth ? lightCount - 1 : lightCount;
			this.buffer = new Vector4[9];
			this.smooth = smooth;
			if (smooth)
				this.closeLights = new PriorityLight[lightCount];
		}

		public void Begin(DrawState state, MaterialShader parent, MaterialTextures textures, Dirty dirty, bool extChanged, ShaderExtension ext)
		{
			MaterialLightCollection lights = parent.lights;
			ShaderSystemBase shaderSystem = state;

			if (currentParent != parent)
			{
				dirty = Dirty.All;
				currentParent = parent;
			}

			MaterialFogState fog = parent.fogState;
			{
				MaterialFogStateFlag flag = this.parent.MaterialFogStateFlag.Value;
				if (flag.OverrideFogState)
					fog = flag.FogState;
			}

			//dirty by fog
			if (currentFog != fog)
			{
				currentFog = fog;
				dirty |= Dirty.FogNarFar;
				dirty |= Dirty.FogGamma;
			}
			if (currentFog != null && currentFog.changeIndex != fogIndex)
			{
				fogIndex = currentFog.changeIndex;
				dirty |= Dirty.FogNarFar;
				dirty |= Dirty.FogGamma;
			}

			textures = textures ?? this.parent.defaultTextures;

			this.instance.CustomTexture = textures.texture ?? state.Properties.WhiteTexture;
			this.instance.CustomTextureSampler = textures.textureSampler;

			this.instance.CustomEmissiveTexture = textures.emissiveTexture ?? state.Properties.WhiteTexture;
			this.instance.CustomEmissiveTextureSampler = textures.emissiveSampler;

			if (this.pixel != null)
			{
				this.pixel.CustomNormalMap = textures.normalmap ?? state.Properties.FlatNormalMapTexture;
				this.pixel.CustomNormalMapSampler = textures.normalSampler;
			}

			if ((dirty & Dirty.FogNarFar) == Dirty.FogNarFar || currentAlpha != parent.emissive.W)
			{
				Vector3 fogAlpha = new Vector3();
				if (fog != null && fog.fogEnabled)
				{
					float invFogEndMinusFogStart = Math.Max(0, fog.fogFar - fog.fogNear);
					if (invFogEndMinusFogStart > 0) invFogEndMinusFogStart = 1.0f / invFogEndMinusFogStart;
					fogAlpha.X = fog.fogNear;
					fogAlpha.Y = -invFogEndMinusFogStart;
				}
				fogAlpha.Z = parent.emissive.W;
				currentAlpha = parent.emissive.W;
				instance.SetV_fogAndAlpha(ref fogAlpha);
			}

			if ((dirty & Dirty.FogGamma) == Dirty.FogGamma)
			{
				Vector4 fogGamma = new Vector4();
				if (fog != null)
				{
					fogGamma.X = fog.fogColour.X * fog.fogColour.X;
					fogGamma.Y = fog.fogColour.Y * fog.fogColour.Y;
					fogGamma.Z = fog.fogColour.Z * fog.fogColour.Z;
				}
				fogGamma.W = parent.outputLinear ? 1.0f : 0.5f;

				if (currentFogGamma.X != fogGamma.X ||
					currentFogGamma.Y != fogGamma.Y ||
					currentFogGamma.Z != fogGamma.Z ||
					currentFogGamma.W != fogGamma.W)
				{
					//try to avoid uneeded changes to PS constants.
					//This can save a call to set the PS constants on the graphics device
					this.instance.SetP_fogColourAndGamma(ref fogGamma);
					currentFogGamma = fogGamma;
				}
			}

			if ((dirty & Dirty.Emissive) == Dirty.Emissive)
			{
				Vector4 emissive = parent.emissive;
				emissive.X *= emissive.X;
				emissive.Y *= emissive.Y;
				emissive.Z *= emissive.Z;

				if (currentEmissive.X != emissive.X ||
					currentEmissive.Y != emissive.Y ||
					currentEmissive.Z != emissive.Z)
				{
					//try to avoid uneeded changes to PS constants.
					this.instance.SetP_EmissiveColour(ref emissive);
					currentEmissive = emissive;
				}
			}

			//find and setup the most important lights
			if (lights != null && lights.enabled)
			{
				//common properties
				Vector3 diffuse = parent.diffuseColour;
				Vector3 specular = parent.specularColour;

				//convert to linear
				diffuse.X *= diffuse.X;
				diffuse.Y *= diffuse.Y;
				diffuse.Z *= diffuse.Z;

				specular.X *= specular.X;
				specular.Y *= specular.Y;
				specular.Z *= specular.Z;

				SphericalHarmonicL1RGB sphericalHarmonic = lights.SphericalHarmonic;

				float lastLightScale = 1;

				//there are more lights than can be displayed, so smoothing must be done
				if (lights.lights.Count > lightCount)
				{
					Vector4 shR = new Vector4(), shG = new Vector4(), shB = new Vector4();

					float radiusSq = parent.lightModelRadius * parent.lightModelRadius;
					radiusSq = Math.Max(radiusSq, 0.000001f);

					Vector3 position;
					state.WorldMatrix.GetPosition(out position);

					if (smooth)
					{
						//prepare for priority calculations..

						Vector3 view;

						state.Camera.GetCameraPosition(out view);
						float invRadiusSq = 1.0f / radiusSq;

						//bias priority calculation position to lights closer to the viewer
						Vector3 biasedPosition;

						biasedPosition.X = view.X - position.X;
						biasedPosition.Y = view.Y - position.Y;
						biasedPosition.Z = view.Z - position.Z;

						float biasLength = biasedPosition.X * biasedPosition.X + biasedPosition.Y * biasedPosition.Y + biasedPosition.Z * biasedPosition.Z;
						biasLength = (float)Math.Sqrt(biasLength);
						float offset = Math.Min(parent.lightModelRadius, biasLength);
						biasLength = biasLength > 0 ? 0.5f / biasLength : 0;

						biasedPosition.X = position.X + biasedPosition.X * biasLength * offset;
						biasedPosition.Y = position.Y + biasedPosition.Y * biasLength * offset;
						biasedPosition.Z = position.Z + biasedPosition.Z * biasLength * offset;

						//if the biased position is outside the view frustum of the camera, then move it to the edge
						Xen.Camera.ICamera cam = state.Camera.GetCamera();
						Plane[] cullPlanes = cam.GetCullingPlanes();

						for (int i = 0; i < cullPlanes.Length; i++)
						{
							Plane plane = cullPlanes[i];

							float distance =
								biasedPosition.X * plane.Normal.X +
								biasedPosition.Y * plane.Normal.Y +
								biasedPosition.Z * plane.Normal.Z +
								plane.D;

							if (distance > 0)
							{
								//bring it back to the plane
								biasedPosition.X -= plane.Normal.X * distance;
								biasedPosition.Y -= plane.Normal.Y * distance;
								biasedPosition.Z -= plane.Normal.Z * distance;
							}
						}

						//iterate the lights, 
						int written = 0;
						float maxPriority = 0;
						foreach (ShaderLight light in lights.lights)
						{
							//work out the importance of this light
							if (light.invImportance == -1)
								light.CalculateImportance();

							float invPriority = light.invImportance;

							if (light.isDirectional == false)
							{
								//intensity is stored in attenuation.y, radius is stored in attenuation.x, w and z are the respective inverses
								invPriority *= light.attenuation.Z * light.attenuation.W;

								//attenuation function is:
								// intensity / (1 + attenuation * len2)
								//where 'attenuation' is light.attenuation.Z (1.0 / atten.X)
								//the divide will be ignored

								float x = light.position.X;
								float y = light.position.Y;
								float z = light.position.Z;

								x -= biasedPosition.X;
								y -= biasedPosition.Y;
								z -= biasedPosition.Z;

								float distSq = x * x + y * y + z * z;

								float atten = 1 + light.attenuation.Z * distSq;

								invPriority *= atten + light.attenuation.Z;
							}

							//larger invPriroty, the less important the light

							PriorityLight newLight;
							newLight.light = light;
							newLight.invPriority = invPriority;

							if (written == closeLights.Length)
							{
								//all lights to display directly are written to already

								//see if there is a light this one can replace in the written lights
								//run through the written list

								if (newLight.invPriority < maxPriority)
								{
									maxPriority = newLight.invPriority;
									for (int i = 0; i < closeLights.Length; i++)
									{
										if (newLight.invPriority < closeLights[i].invPriority)
										{
											PriorityLight newAdd = closeLights[i];
											closeLights[i] = newLight;
											newLight = newAdd;
										}

										maxPriority = closeLights[i].invPriority > maxPriority ? closeLights[i].invPriority : maxPriority;
									}
								}

								//add the light to the SH (it may be a shuffled lower priorty light that got bumped from the close list)
								AddLightToSH(radiusSq, ref position, ref shR, ref shG, ref shB, newLight.light, 1);
							}
							else
							{
								maxPriority = invPriority > maxPriority ? invPriority : maxPriority;

								closeLights[written++] = newLight;
							}
						}


						//sort closeLights, by priority.
						//this list is at most 4 elements

						Array.Sort(closeLights);

						//the very last light in 'closeLights' is not displayed.
						//work out how close the last two lights are.
						//if they are very close, they may be about to swap. So fade out the second to last light.

						int lightIndex = (int)lightCount - 1;

						//interpolate the last two lights, depending on their priority.
						float ratio = closeLights[lightCount].invPriority / closeLights[lightCount - 1].invPriority;

						//interpolate when within 50% priotiy
						ratio = (ratio - 1) * 2;

						//if it's zero, then the last two lights are identical priority.
						//in which case, put them both in the SH,
						//if it's 1 or more, then they are distinct, so include the second to last fully, and put only the last in the SH

						if (ratio < 1)
						{
							//the last light to be drawn is scaled down
							lastLightScale = ratio;

							//add it into the SH as well
							AddLightToSH(radiusSq, ref position, ref shR, ref shG, ref shB, closeLights[lightCount - 1].light, 1 - ratio);
						}

						//always add the final light to the SH, as it's never drawn directly.
						AddLightToSH(radiusSq, ref position, ref shR, ref shG, ref shB, closeLights[lightCount].light, 1);
					}
					else
					{
						//add all the lights directly to the SH
						foreach (ShaderLight light in lights.lights)
						{
							AddLightToSH(radiusSq, ref position, ref shR, ref shG, ref shB, light, 1);
						}
					}

					sphericalHarmonic.Red.X += shR.X * diffuse.X;
					sphericalHarmonic.Red.Y += shR.Y * diffuse.X;
					sphericalHarmonic.Red.Z += shR.Z * diffuse.X;
					sphericalHarmonic.Red.W += shR.W * diffuse.X;

					sphericalHarmonic.Green.X += shG.X * diffuse.Y;
					sphericalHarmonic.Green.Y += shG.Y * diffuse.Y;
					sphericalHarmonic.Green.Z += shG.Z * diffuse.Y;
					sphericalHarmonic.Green.W += shG.W * diffuse.Y;

					sphericalHarmonic.Blue.X += shB.X * diffuse.Z;
					sphericalHarmonic.Blue.Y += shB.Y * diffuse.Z;
					sphericalHarmonic.Blue.Z += shB.Z * diffuse.Z;
					sphericalHarmonic.Blue.W += shB.W * diffuse.Z;
				}

				//generate the SH ambient matrix
				Matrix sh = new Matrix();

				sh.M11 = sphericalHarmonic.Red.X;
				sh.M21 = sphericalHarmonic.Red.Y;
				sh.M31 = sphericalHarmonic.Red.Z;
				sh.M41 = sphericalHarmonic.Red.W + lights.ambient.X * lights.ambient.X;
				sh.M12 = sphericalHarmonic.Green.X;
				sh.M22 = sphericalHarmonic.Green.Y;
				sh.M32 = sphericalHarmonic.Green.Z;
				sh.M42 = sphericalHarmonic.Green.W + lights.ambient.Y * lights.ambient.Y;
				sh.M13 = sphericalHarmonic.Blue.X;
				sh.M23 = sphericalHarmonic.Blue.Y;
				sh.M33 = sphericalHarmonic.Blue.Z;
				sh.M43 = sphericalHarmonic.Blue.W + lights.ambient.Z * lights.ambient.Z;

				this.instance.SetV_SH(ref sh);

				if (lightCount > 0)
				{
					//write the direct lights to the buffer
					int index = 0;
					for (int i = 0; i < lightCount; i++)
					{
						//shader format:
						//float4 lightPosition	= v_lights[n*3];
						//float4 specularColour	= v_lights[n*3+1];
						//float4 diffuseColour	= v_lights[n*3+2];
						//float attenuation		= diffuseColour.w;
						ShaderLight light = null;
						float scale = 1;

						//smoothing is active, so get the close lights and scale them by thier temporal scaling value 
						if (smooth)
						{
							//the last light to be drawn can be faded out
							if (i == lightCount-1)
								scale *= lastLightScale;

							light = closeLights[i].light;
						}
						else
						{
							light = lights.lights[i];
						}

						scale *= light.attenuation.Y * light.attenuation.X;


						Vector4 spec = light.spcularLinear;
						spec.X = spec.X * scale * specular.X;
						spec.Y = spec.Y * scale * specular.Y;
						spec.Z = spec.Z * scale * specular.Z;
						spec.W = spec.W * parent.specularPower;

						Vector4 colour = light.colourLinear;
						colour.X = colour.X * scale * diffuse.X;
						colour.Y = colour.Y * scale * diffuse.Y;
						colour.Z = colour.Z * scale * diffuse.Z;
						colour.W = light.attenuation.Z;

						buffer[index++] = light.position;
						buffer[index++] = spec;
						buffer[index++] = colour;
					}

					//and finally set the PS or VS constants
					if (this.pixel != null)
						this.pixel.SetP_lights(buffer, 0, 0, lightCount * 3);
					if (this.vertex != null)
						this.vertex.SetV_lights(buffer, 0, 0, lightCount * 3);
				}
			}
			else
			{
				//lighting is disabled
				Matrix sh = new Matrix();

				//ambient of 1.
				sh.M41 = 1;
				sh.M42 = 1;
				sh.M43 = 1;

				this.instance.SetV_SH(ref sh);
			}

			bool isBound = !shaderSystem.IsShaderBound(instance);

			this.instance.Begin(shaderSystem, isBound, extChanged, ext);
		}

		private static void AddLightToSH(float radiusSq, ref Vector3 position, ref Vector4 shR, ref Vector4 shG, ref Vector4 shB, ShaderLight addLight, float weight)
		{

			float x = addLight.position.X;
			float y = addLight.position.Y;
			float z = addLight.position.Z;

			float atten = 0.25f;
			float w = 1;

			if (addLight.isDirectional == false)
			{
				x -= position.X;
				y -= position.Y;
				z -= position.Z;

				float distSq = x * x + y * y + z * z;

				atten = 1 + addLight.attenuation.Z * distSq;

				atten = 0.25f / atten;

				float len = 1.0f / (float)Math.Sqrt(Math.Max(distSq, radiusSq));

				x *= len;
				y *= len;
				z *= len;

				atten *= addLight.attenuation.Y * addLight.attenuation.X * weight;
			}

			float r = addLight.colourLinear.X * atten;
			float g = addLight.colourLinear.Y * atten;
			float b = addLight.colourLinear.Z * atten;

			shR.X += r * x;
			shR.Y += r * y;
			shR.Z += r * z;
			shR.W += r * w;

			shG.X += g * x;
			shG.Y += g * y;
			shG.Z += g * z;
			shG.W += g * w;

			shB.X += b * x;
			shB.Y += b * y;
			shB.Z += b * z;
			shB.W += b * w;
		}
	}









	/// <summary>
	/// <para>A structure that can be used as a Draw Flag to have compatible classes (MaterialShader) use a specific <see cref="LightingDisplayModel"/></para>
	/// </summary>
	public struct LightingDisplayModelFlag
	{
		/// <summary>
		/// <para>Force drawn objects to use <see cref="LightingDisplayModel"/></para>
		/// </summary>
		public bool OverrideDisplayModel;
		/// <summary>
		/// Get/Set the light fog state to use
		/// </summary>
		public LightingDisplayModel DisplayModel;

		/// <summary></summary>
		public LightingDisplayModelFlag(LightingDisplayModel displayModel)
		{
			this.OverrideDisplayModel = true;
			this.DisplayModel = displayModel;
		}
	}

	/// <summary>
	/// A flag to control how lighitng is displayed on a model
	/// </summary>
	public enum LightingDisplayModel : byte
	{
		/// <summary>
		/// The model will be displayed with per-pixel lighting if it has a normal map, otherwise it will use per-vertex lighting
		/// </summary>
		Default,
		/// <summary>
		/// The same as Default, however the model will be limited to a single direct light source to improve performance
		/// </summary>
		SingleLight,
		/// <summary>
		/// <para>The model will be displayed with per-pixel lighting</para>
		/// <para>This is generally the slowest rendering mode</para>
		/// </summary>
		ForcePerPixel,
		/// <summary>
		/// <para>The model will be displayed with per-vertex lighting</para>
		/// <para>This is generally faster than the Per-Pixel rendering mode, unless the model has a very high polygon count, or is very small on screen</para>
		/// </summary>
		ForcePerVertex,
		/// <summary>
		/// <para>The model will be displayed with only ambient spherical harmonic lighting</para>
		/// <para>This is the fastest way to light a model, however it is generally low accuracy</para>
		/// </summary>
		ForceSphericalHarmonic
	}


	[Flags]
	enum Dirty : byte
	{
		FogGamma = 1,
		FogNarFar = 2,
		Diffuse = 4,
		Specular = 8,
		Texture = 16,
		NormalMap = 32,
		Emissive = 64,
		EmissiveTexture = 128,
		All = 255
	}

	/// <summary>
	/// <para>A structure that can be used as a Draw Flag to have compatible classes (MaterialShader) use a specific <see cref="MaterialFogState"/></para>
	/// </summary>
	public struct MaterialFogStateFlag
	{
		/// <summary>
		/// <para>Force drawn objects to use <see cref="FogState"/></para>
		/// </summary>
		public bool OverrideFogState;
		/// <summary>
		/// Get/Set the light fog state to use
		/// </summary>
		public MaterialFogState FogState;

		/// <summary></summary>
		public MaterialFogStateFlag(MaterialFogState fogState)
		{
			this.OverrideFogState = true;
			this.FogState = fogState;
		}
	}

	/// <summary>
	/// Storage for the fogging state used by a <see cref="MaterialShader"/>
	/// </summary>
	public sealed class MaterialFogState
	{
		//fog
		internal Vector3 fogColour;
		internal float fogNear, fogFar;
		internal bool fogEnabled = true;
		internal int changeIndex;

		/// <summary>
		/// Gets/Sets the colour of the fog
		/// </summary>
		public Vector3 FogColour
		{
			get { return fogColour; }
			set
			{
				changeIndex += (fogColour.X != value.X | fogColour.Y != value.Y | fogColour.Z != value.Z) ? 1 : 0;
				fogColour = value;
			}
		}

		/// <summary>
		/// Gets/Sets if fogging is enabled
		/// </summary>
		public bool FogEnabled
		{
			get { return fogEnabled; }
			set
			{
				changeIndex += (fogEnabled != value) ? 1 : 0;
				fogEnabled = value;
			}
		}

		/// <summary>
		/// Gets/Sets the near distance where fogging begins
		/// </summary>
		public float FogNearDistance
		{
			get { return fogNear; }
			set
			{
				changeIndex += (fogNear != value) ? 1 : 0;
				fogNear = value;
			}
		}

		/// <summary>
		/// Gets/Sets the far distance where fogging is at it's thickest
		/// </summary>
		public float FogFarDistance
		{
			get { return fogFar; }
			set
			{
				changeIndex += (fogFar != value) ? 1 : 0;
				fogFar = value;
			}
		}
	}


	/// <summary>
	/// <para>A structure that can be used as a Draw Flag to have compatible classes (MaterialShader) use a specific <see cref="MaterialTextures"/> object</para>
	/// </summary>
	public struct MaterialTexturesFlag
	{
		/// <summary>
		/// <para>Force drawn objects to use <see cref="Textures"/></para>
		/// </summary>
		public bool OverrideTextures;
		/// <summary>
		/// Get/Set the light fog state to use
		/// </summary>
		public MaterialTextures Textures;

		/// <summary></summary>
		public MaterialTexturesFlag(MaterialTextures textures)
		{
			this.OverrideTextures = true;
			this.Textures = textures;
		}
	}

	/// <summary>
	/// Storage for the textures used by a <see cref="MaterialShader"/>
	/// </summary>
	public sealed class MaterialTextures
	{
		//current textures and samplers
		internal Texture2D texture, normalmap, emissiveTexture;
		internal TextureSamplerState
			textureSampler = TextureSamplerState.AnisotropicMediumFiltering,
			normalSampler = TextureSamplerState.BilinearFiltering,
			emissiveSampler = TextureSamplerState.BilinearFiltering;
		internal int textureIndex, normalIndex, emissiveIndex;

		/// <summary>
		/// Gets/Sets an optional texture map used during shading
		/// </summary>
		public Texture2D TextureMap
		{
			get { return texture; }
			set
			{
				textureIndex += (texture != value) ? 1 : 0;
				texture = value;
			}
		}
		/// <summary>
		/// Gets/Sets the texture sampler used for the optional texture map
		/// </summary>
		public TextureSamplerState TextureMapSampler
		{
			get { return textureSampler; }
			set
			{
				textureIndex += (textureSampler != value) ? 1 : 0;
				textureSampler = value;
			}
		}
		/// <summary>
		/// Gets/Sets the optional normal map used for lighting. Alpha of the normal map modulates specular reflection. Using a normal map requires the geometry has normals, tangents and binormals.
		/// </summary>
		public Texture2D NormalMap
		{
			get { return normalmap; }
			set
			{
				normalIndex += (normalmap != value) ? 1 : 0;
				normalmap = value;
			}
		}
		/// <summary>
		/// Gets/Sets the texture sampler used for the optional normal map
		/// </summary>
		public TextureSamplerState NormalMapSampler
		{
			get { return normalSampler; }
			set
			{
				normalIndex += (normalSampler != value) ? 1 : 0;
				normalSampler = value;
			}
		}

		/// <summary>
		/// <para>Gets/Sets an optional emissive texture map used during shading</para>
		/// <para>NOTE: The emissive texture (unlike the emissive colour) is assumed to be stored in Linear Light space, not gamma corrected light space</para>
		/// </summary>
		public Texture2D EmissiveTextureMap
		{
			get { return texture; }
			set
			{
				emissiveIndex += (emissiveTexture != value) ? 1 : 0;
				emissiveTexture = value;
			}
		}
		/// <summary>
		/// Gets/Sets the texture sampler used for the optional emissive texture
		/// </summary>
		public TextureSamplerState EmissiveTextureMapSampler
		{
			get { return emissiveSampler; }
			set
			{
				emissiveIndex += (emissiveSampler != value) ? 1 : 0;
				emissiveSampler = value;
			}
		}
	}

	/// <summary>
	/// <para>A shader that implements a simple lighting model that supporting a large number of point and directional lights.</para>
	/// <para>MaterialShader also allows for textures, normal maps, vertex colours, skinning and hardware instancing to be used</para>
	/// </summary>
	public sealed class MaterialShader : IShader
	{
		//instance of the current shader (determined by the number of lights in use)
		private ShaderInstance shader;
		//cache object for getting shader instances
		private ShaderCache shaderCache;

		//dirty state for shader properties
		private Dirty dirty = Dirty.All;

		//display mode
		private LightingDisplayModel displayModel = LightingDisplayModel.Default;

		//approximate radius of the object being drawn
		internal float lightModelRadius = 0;

		//number of lights the current shader can handle
		private int lightCount;

		internal MaterialLightCollection lights;

		//colour and specular properties
		internal float specularPower = 16;
		internal Vector3 specularColour;
		internal Vector3 diffuseColour = Vector3.One;
		internal Vector4 emissive = new Vector4(0, 0, 0, 1);

		//state
		internal bool useVertexColour, outputLinear, perPixel;

		internal MaterialFogState fogState;

		internal MaterialTextures textures;

		/// <summary>
		/// Creates a MaterialShader that implements similar lighting characteristics to an XNA BasicEffect (The <see cref="LightCollection"/> collection will be populated if the basic effect has lighting enabled)
		/// </summary>
		/// <param name="effect"></param>
		/// <param name="shader"></param>
		/// <remarks><para>Not all features of the BasicEffect are implement by the created MaterialShader (eg Fog is not implemented)</para></remarks>
		/// <returns></returns>
		public static void FromBasicEffect(BasicEffect effect, out MaterialShader shader)
		{
			if (effect == null)
				throw new ArgumentNullException();

			shader = new MaterialShader();
			shader.Alpha = effect.Alpha;
			shader.EmissiveColour = effect.EmissiveColor;
			shader.UseVertexColour = effect.VertexColorEnabled;
			if (effect.TextureEnabled && effect.Texture != null)
			{
				shader.textures = new MaterialTextures();
				shader.textures.TextureMap = effect.TextureEnabled ? effect.Texture : null;
			}
			shader.DiffuseColour = effect.DiffuseColor;
			shader.SpecularColour = effect.SpecularColor;
			shader.SpecularPower = effect.SpecularPower;
			if (effect.FogEnabled)
			{
				shader.fogState = new MaterialFogState();
				shader.fogState.FogColour = effect.FogColor;
				shader.fogState.FogNearDistance = effect.FogStart;
				shader.fogState.FogFarDistance = effect.FogEnd;
			}

			if (effect.LightingEnabled)
			{
				MaterialLightCollection lights = new MaterialLightCollection();
				shader.lights = lights;
				lights.AmbientLightColour = effect.AmbientLightColor;

				if (effect.DirectionalLight0.Enabled)
				{
					lights.CreateDirectionalLight(effect.DirectionalLight0.Direction, effect.DirectionalLight0.DiffuseColor, effect.DirectionalLight0.SpecularColor);
				}
				if (effect.DirectionalLight1.Enabled)
				{
					lights.CreateDirectionalLight(effect.DirectionalLight1.Direction, effect.DirectionalLight1.DiffuseColor, effect.DirectionalLight1.SpecularColor);
				}
				if (effect.DirectionalLight2.Enabled)
				{
					lights.CreateDirectionalLight(effect.DirectionalLight2.Direction, effect.DirectionalLight2.DiffuseColor, effect.DirectionalLight2.SpecularColor);
				}
			}
		}

		/// <summary>
		/// Construct the material shader
		/// </summary>
		public MaterialShader(MaterialLightCollection lights)
		{
			this.lights = lights;
		}

		/// <summary>
		/// Construct the material shader
		/// </summary>
		public MaterialShader()
		{
		}

		/// <summary>
		/// Gets/Sets the fogging properties used by this shader
		/// </summary>
		public MaterialFogState FogState
		{
			get { return fogState; }
			set { fogState = value; }
		}

		/// <summary>
		/// Gets/Sets the textures used by this shader
		/// </summary>
		public MaterialTextures Textures
		{
			get { return textures; }
			set { textures = value; }
		}

		/// <summary>
		/// <para>Output linear light from the shader. Defaults to false.</para>
		/// <para>By default, the output from the shader is gamma corrected.</para>
		/// <para>Set this parameter to true if you are are accumulating or blending multiple shaders before post processing and gamma correction</para>
		/// </summary>
		public bool OutputLinearLight
		{
			get { return outputLinear; }
			set
			{
				this.dirty |= value != outputLinear ? Dirty.FogGamma : 0;
				outputLinear = value;
			}
		}

		/// <summary>
		/// Gets/Sets the lighting display mode, this mode controls how lighting is applied to the object being rendered
		/// </summary>
		public LightingDisplayModel LightingDisplayModel
		{
			get { return displayModel; }
			set
			{
				if (value != displayModel)
				{
					displayModel = value;
					this.shader = null;
				}
			}
		}

		/// <summary>
		/// Gets/Sets a radius used during lighting calculation.
		/// <para>For improved results, set this radius to approximately the radius of the model</para>
		/// </summary>
		public float LightingDisplayModelRadius
		{
			get { return lightModelRadius; }
			set
			{
				if (lightModelRadius != value)
				{
					if (value < 0)
						throw new ArgumentException();
					lightModelRadius = value;
				}
			}
		}

		/// <summary>
		/// Gets/Sets the light collection used by this shader
		/// </summary>
		public MaterialLightCollection LightCollection
		{
			get { return lights; }
			set { lights = value; }
		}

		/// <summary>
		/// Gets/Sets the diffuse colour of this material, (Default value is White (1,1,1))
		/// </summary>
		public Vector3 DiffuseColour
		{
			get { return diffuseColour; }
			set
			{
				dirty |= (diffuseColour.X != value.X | diffuseColour.Y != value.Y | diffuseColour.Z != value.Z) ? Dirty.Diffuse : 0;
				diffuseColour = value; 
			}
		}

		/// <summary>
		/// Gets/Sets the specular colour of this material, (Default value is Black (0,0,0))
		/// </summary>
		public Vector3 SpecularColour
		{	
			get { return specularColour; }
			set
			{
				dirty |= (specularColour.X != value.X | specularColour.Y != value.Y | specularColour.Z != value.Z) ? Dirty.Specular : 0;
				specularColour = value; 
			}
		}


		/// <summary>
		/// Specular power of the material (larger values produce a more focused specular reflection. Values between 5 and 32 are common). Default value is 16. Note the default specular colour is black (no specular)
		/// </summary>
		/// <remarks><para>Each light source can also modify the specular power through a scaler (see <see cref="IMaterialLight.SpecularPowerScaler"/>)</para></remarks>
		public float SpecularPower
		{
			get { return specularPower; }
			set
			{
				dirty |= (specularPower != value) ? Dirty.Specular : 0;
				specularPower = value;
			}
		}

		/// <summary>
		/// Gets/Sets if this shader should use vertex colours from the geometry. All geometry drawn with this shader will require COLOR0 elements in their vertex geometry
		/// </summary>
		public bool UseVertexColour
		{
			get { return useVertexColour; }
			set 
			{
				if (value != useVertexColour)
				{
					this.shader = null;
					useVertexColour = value;
				}
			}
		}

		/// <summary>
		/// Gets/Sets the base alpha value (default is 1)
		/// </summary>
		public float Alpha
		{
			get { return emissive.W; }
			set
			{
				dirty |= (emissive.W != value) ? Dirty.Emissive : 0;
				emissive.W = value; 
			}
		}

		/// <summary>
		/// Gets/Sets the emissive lighting value
		/// </summary>
		public Vector3 EmissiveColour
		{
			get { return new Vector3(emissive.X, emissive.Y, emissive.Z); }
			set
			{
				dirty |= (emissive.X != value.X | emissive.Y != value.Y | emissive.Z != value.Z) ? Dirty.Emissive : 0;
				emissive.X = value.X;
				emissive.Y = value.Y;
				emissive.Z = value.Z;
			}
		}

		void IShader.Begin(Xen.Graphics.ShaderSystem.ShaderSystemBase shaderSystem, bool ic, bool ec, Xen.Graphics.ShaderSystem.ShaderExtension ext)
		{
			DrawState state = (DrawState)shaderSystem;

			Xen.Graphics.Stack.DrawFlagStack flags = state.DrawFlags;

			if (shaderCache == null)
			{
				shaderCache = state.Application.UserValues[ShaderCache.TypeName] as ShaderCache;
				if (shaderCache == null)
				{
					shaderCache = new ShaderCache(state);
					state.Application.UserValues[ShaderCache.TypeName] = shaderCache;
				}
			}

			MaterialLightCollection lights = this.lights;
			{
				MaterialLightCollectionFlag flag = shaderCache.MaterialLightCollectionFlag.Value;
				if (flag.OverrideLightCollection)
					lights = flag.LightCollection;
			}

			MaterialTextures textures = this.textures;
			{
				MaterialTexturesFlag flag = shaderCache.MaterialTexturesFlag.Value;
				if (flag.OverrideTextures)
					textures = flag.Textures;
			}

			LightingDisplayModel displayModel = this.displayModel;
			{
				LightingDisplayModelFlag flag = shaderCache.LightingDisplayModelFlag.Value;
				if (flag.OverrideDisplayModel)
					displayModel = flag.DisplayModel;
			}

			bool perPixel = displayModel == LightingDisplayModel.ForcePerPixel || (displayModel == LightingDisplayModel.Default && textures != null && textures.normalmap != null);
			
			int count = 0;
			if (lights != null && lights.enabled && displayModel != LightingDisplayModel.ForceSphericalHarmonic)
				count = Math.Min(perPixel ? 3 : 4, lights.lights.Count);
			if (displayModel == LightingDisplayModel.SingleLight && count > 1)
				count = 1;

			if (count != this.lightCount || perPixel != this.perPixel)
				shader = null;

			this.perPixel = perPixel;

			this.lightCount = count;

			//update the shader?
			if (shader == null)
			{
				this.shader = shaderCache.GetShader(state, count, perPixel, this.useVertexColour);
			}

			this.shader.Begin(state, this, textures, dirty, ec, ext);
		}

		#region IShader internal implementation

		bool IShader.HasChanged
		{
			get { return true; }
		}

		void IShader.GetExtensionSupport(out bool blending, out bool instancing)
		{
			blending = true;
			instancing = true;
		}

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, bool value)
		{
			return false;
		}

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, float[] value)
		{
			return false;
		}

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, Vector2[] value)
		{
			return false;
		}

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, Vector3[] value)
		{
			return false;
		}

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, Vector4[] value)
		{
			return false;
		}

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, Matrix[] value)
		{
			return false;
		}

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, float value)
		{
			return false;
		}

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, ref Vector2 value)
		{
			return false;
		}

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, ref Vector3 value)
		{
			return false;
		}

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, ref Vector4 value)
		{
			return false;
		}

		bool IShader.SetAttribute(ShaderSystemBase state, int name_uid, ref Matrix value)
		{
			return false;
		}

		bool IShader.SetSamplerState(ShaderSystemBase state, int name_uid, Xen.Graphics.TextureSamplerState sampler)
		{
			return false;
		}

		bool IShader.SetTexture(ShaderSystemBase state, int name_uid, TextureCube texture)
		{
			return false;
		}

		bool IShader.SetTexture(ShaderSystemBase state, int name_uid, Texture3D texture)
		{
			return false;
		}

		bool IShader.SetTexture(ShaderSystemBase state, int name_uid, Texture2D texture)
		{
			return false;
		}

		bool IShader.SetTexture(ShaderSystemBase state, int name_uid, Texture texture)
		{
			return false;
		}

		void IShader.GetVertexInput(int index, out VertexElementUsage elementUsage, out int elementIndex)
		{
			this.shader.instance.GetVertexInput(index, out elementUsage, out elementIndex);
		}

		int IShader.GetVertexInputCount()
		{
			return this.shader.instance.GetVertexInputCount();
		}

		#endregion
	}
}
