using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Xen.Graphics;
using Xen.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xen.Ex.Graphics;
using Microsoft.Xna.Framework.Content;
using Xen.Ex.Graphics.Processor;

namespace Xen.Ex.Graphics.Content
{
	/// <summary>
	/// Used internally for content pipeline helpers
	/// </summary>
	public enum ContentTargetPlatform
	{
		/// <summary></summary>
		Windows,
		/// <summary></summary>
		Xbox360
	}

	/// <summary>
	/// Interface to a class than can display particles
	/// </summary>
	public interface IParticleSystemDrawer : IDraw
	{
		/// <summary>
		/// Draw GPU processed particles (this method is called internally)
		/// </summary>
		/// <param name="state"></param><param name="particleType"></param><param name="count"></param>
		/// <param name="colour"></param><param name="positionSize"></param><param name="userValues"></param><param name="velocityRotation"></param>
		void DrawGpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint count, Texture2D positionSize, Texture2D velocityRotation, Texture2D colour, Texture2D userValues);
#if !XBOX360
		/// <summary>
		/// Draw CPU processed particles (this method is called internally)
		/// </summary>
		/// <param name="state"></param><param name="particleType"></param><param name="count"></param><param name="positionSize"></param>
		/// <param name="velocityRotation"></param><param name="colour"></param><param name="userValues"></param>
		void DrawCpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint count, Vector4[] positionSize, Vector4[] velocityRotation, Vector4[] colour, Vector4[] userValues);
#endif
		/// <summary>
		/// Set a mask bit that will enable/disable drawing a specific particle type
		/// </summary>
		/// <param name="particleType"></param>
		/// <param name="draw"></param>
		void SetParticleTypeDrawMask(Xen.Ex.Graphics.Content.ParticleSystemTypeData particleType, bool draw);
		/// <summary>
		/// Set a mask bit that will enable/disable drawing a specific particle type
		/// </summary>
		/// <param name="particleTypeIndex"></param>
		/// <param name="draw"></param>
		void SetParticleTypeDrawMask(int particleTypeIndex, bool draw);
		/// <summary>
		/// Set a mask bit that will enable/disable drawing a specific particle type by name
		/// </summary>
		/// <param name="particleTypeName"></param>
		/// <param name="draw"></param>
		void SetParticleTypeDrawMask(string particleTypeName, bool draw);
		/// <summary>
		/// Set a mask bit that will enable/disable drawing of all particle types
		/// </summary>
		/// <param name="draw"></param>
		void SetParticleTypeDrawMaskAllTypes(bool draw);
		/// <summary>
		/// Gets the particle system for this drawer
		/// </summary>
		ParticleSystem ParticleSystem { get; }
		/// <summary>
		/// Gets/Sets if this drawer is enabled
		/// </summary>
		bool Enabled { get; set; }
	}

	//a particle processor can do many wonderous things
	internal interface IParticleProcessor : IDisposable
	{
		void Initalise(ParticleSystemTypeData typeData, IParticleProcessor[] allProcessors, bool useColourValues, uint maxLifeTimeSteps, uint timeStepHz, uint maxExpectedCount);
		//Note: Update may be called on a task thread
		void Update(int particleCount, float particleCountF, float delatTime, float[] globals, CopyAction[] copyActions, int copyActionCount, AddAction[] addActions, int addActionCount, int step);
		void DrawProcess(FrameState state, int step);
		void BeginDrawPass(int pass, float deltaTime, int step);
		bool RequiresDrawPass { get; }
		void DrawCallback(DrawState state, IParticleSystemDrawer particleDrawer, uint particleCount);
		void Warm(Application application);
	}

	//details for a spawning particle
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
	internal struct ParticleSpawnValues
	{
		[System.Runtime.InteropServices.FieldOffset(0)]
		public Vector4 PositionSize;
		[System.Runtime.InteropServices.FieldOffset(16)]
		public Vector4 VelocityRotation;
		[System.Runtime.InteropServices.FieldOffset(32)]
		public Vector4 Colour;
		[System.Runtime.InteropServices.FieldOffset(48)]
		public Vector4 UserValues;

		//union the float values with ints
		[System.Runtime.InteropServices.FieldOffset(0)]
		private int i0;
		[System.Runtime.InteropServices.FieldOffset(4)]
		private int i1;
		[System.Runtime.InteropServices.FieldOffset(8)]
		private int i2;
		[System.Runtime.InteropServices.FieldOffset(12)]
		private int i3;
		[System.Runtime.InteropServices.FieldOffset(16)]
		private int i4;
		[System.Runtime.InteropServices.FieldOffset(20)]
		private int i5;
		[System.Runtime.InteropServices.FieldOffset(24)]
		private int i6;
		[System.Runtime.InteropServices.FieldOffset(28)]
		private int i7;
		[System.Runtime.InteropServices.FieldOffset(32)]
		private int i8;
		[System.Runtime.InteropServices.FieldOffset(36)]
		private int i9;
		[System.Runtime.InteropServices.FieldOffset(40)]
		private int i10;
		[System.Runtime.InteropServices.FieldOffset(44)]
		private int i11;
		[System.Runtime.InteropServices.FieldOffset(48)]
		private int i12;
		[System.Runtime.InteropServices.FieldOffset(52)]
		private int i13;
		[System.Runtime.InteropServices.FieldOffset(56)]
		private int i14;
		[System.Runtime.InteropServices.FieldOffset(60)]
		private int i15;

		public static readonly ParticleSpawnValues Default;

		static ParticleSpawnValues()
		{
			Default = new ParticleSpawnValues();
			Default.Colour = Vector4.One;
			Default.PositionSize.W = 1;
		}

		// using this class is *critical* to prevent garbage build up when ParticleSpawnValues is used in a dictionary
		public class ParticleSpawnValuesComparer : IEqualityComparer<ParticleSpawnValues>
		{
			public bool Equals(ParticleSpawnValues x, ParticleSpawnValues y)
			{
				return 
					x.i0 == y.i0 && x.i1 == y.i1 && x.i2 == y.i2 && x.i3 == y.i3 && x.i4 == y.i4 && 
					x.i5 == y.i5 && x.i6 == y.i6 && x.i7 == y.i7 && x.i8 == y.i8 && x.i9 == y.i9 &&
					x.i10 == y.i10 && x.i11 == y.i11 && x.i12 == y.i12 && x.i13 == y.i13 && x.i14 == y.i14 && x.i15 == y.i15;
			}

			public int GetHashCode(ParticleSpawnValues obj)
			{
				return obj.i0 ^ obj.i1 ^ obj.i2 ^ obj.i3 ^ obj.i4 ^ obj.i5 ^ obj.i6 ^ obj.i7 ^ obj.i8 ^ obj.i9 ^
					obj.i10 ^ obj.i11 ^ obj.i12 ^ obj.i13 ^ obj.i14 ^ obj.i15;
			}
		}
	}



	//data representing a particle to be added
	internal struct AddAction
	{
		public uint index, lifeSteps;
		public int cloneFromIndex, cloneTypeIndex;
		public float indexF, cloneFromIndexF;

		public ParticleSpawnValues spawnDetails;
	}

	//data representing a particle to be copied
	internal struct CopyAction
	{
		public uint indexFrom, indexTo;
		public float indexFromF, indexToF;
	}

	/// <summary>
	/// Storage class for the per frame and one time logic performed on individual particles
	/// </summary>
	public sealed class ParticleSystemTypeLogicData
	{
		private readonly ParticleSystemLogicStep[] once, frame;

		/// <summary>
		/// Gets the array of logic steps performed when the particle type is created
		/// </summary>
		public ReadOnlyArrayCollection<ParticleSystemLogicStep> Once
		{
			get
			{
				if (frame == null) return ReadOnlyArrayCollection<ParticleSystemLogicStep>.Empty;
				return new ReadOnlyArrayCollection<ParticleSystemLogicStep>(once); 
			}
		}
		/// <summary>
		/// Gets the array of logic steps performed per frame for the particle type
		/// </summary>
		public ReadOnlyArrayCollection<ParticleSystemLogicStep> Frame
		{
			get 
			{
				if (frame == null) return ReadOnlyArrayCollection<ParticleSystemLogicStep>.Empty;
				return new ReadOnlyArrayCollection<ParticleSystemLogicStep>(frame); 
			}
		}
		
#if DEBUG && !XBOX360

		internal ParticleSystemTypeLogicData(XmlElement xml)
		{
			foreach (XmlNode logicNode in xml.ChildNodes)
			{
				if (logicNode is XmlElement && logicNode.Name == "logic")
				{
					foreach (XmlNode node in logicNode.ChildNodes)
					{
						if (node is XmlElement && (node.Name == "once" || node.Name == "frame"))
						{
							List<ParticleSystemLogicStep> steps = new List<ParticleSystemLogicStep>();
							foreach (XmlNode step in node.ChildNodes)
							{
								if (step is XmlElement)
									steps.Add(new ParticleSystemLogicStep(step as XmlElement));
							}

							if (node.Name == "once")
								this.once = steps.ToArray();
							if (node.Name == "frame")
								this.frame = steps.ToArray();
						}
					}
				}
			}

			if (this.frame == null)
				this.frame = new ParticleSystemLogicStep[0];
			if (this.once == null)
				this.once = new ParticleSystemLogicStep[0];
		}
		
		internal void Write(BinaryWriter writer)
		{
			int count = -1;
			if (once != null) count = once.Length;

			writer.Write(count);
			if (once != null)
			{
				for (int i = 0; i < once.Length; i++)
					once[i].Write(writer);
			}


			count = -1;
			if (frame != null) count = frame.Length;

			writer.Write(count);
			if (frame != null)
			{
				for (int i = 0; i < frame.Length; i++)
					frame[i].Write(writer);
			}
		}
#endif

		internal ParticleSystemTypeLogicData(ContentReader reader)
		{
			int count = reader.ReadInt32();
			if (count != -1)
			{
				this.once = new ParticleSystemLogicStep[count];
				for (int i = 0; i < count; i++)
					this.once[i] = new ParticleSystemLogicStep(reader);
			}

			count = reader.ReadInt32();
			if (count != -1)
			{
				this.frame = new ParticleSystemLogicStep[count];
				for (int i = 0; i < count; i++)
					this.frame[i] = new ParticleSystemLogicStep(reader);
			}
		}
	}

	/// <summary>
	/// Raw representation of a operation (step) performed on a particle, eg, as a value assignment
	/// </summary>
	public struct ParticleSystemLogicStep
	{
		internal readonly string target, arg0, arg1, type;
		internal readonly ParticleSystemLogicStep[] children;

		/// <summary>
		/// The target of the operation (eg, 'position.x')
		/// </summary>
		public string Target { get { return target; } }
		/// <summary>
		/// The first arguement of the operation (eg, '0.5' or 'position.x')
		/// </summary>
		public string Arg0 { get { return arg0; } }
		/// <summary>
		/// The optional second arguement of the operation (eg, '0.5' or 'position.x')
		/// </summary>
		public string Arg1 { get { return arg1; } }
		/// <summary>
		/// The name of the logic method to perform (eg, 'set' to assign a value)
		/// </summary>
		public string Method { get { return type; } }

		/// <summary>
		/// Gets a list of children for this operation. Children will only be present on branching operations, such as 'loop'
		/// </summary>
		public ReadOnlyArrayCollection<ParticleSystemLogicStep> Children 
		{
			get 
			{
				if (children == null) return ReadOnlyArrayCollection<ParticleSystemLogicStep>.Empty;
				return new ReadOnlyArrayCollection<ParticleSystemLogicStep>(children); 
			} 
		}

#if DEBUG && !XBOX360

		internal ParticleSystemLogicStep(XmlElement xml)
		{
			this.type = xml.Name;
			this.arg1 = XmlHelper.GetAttributeOpt(xml, "arg1");
			bool hasChildren = type.StartsWith("if_");

			switch (type)
			{
				case "every":
					this.arg0 = XmlHelper.GetAttributeOpt(xml, "interval");
					this.target = null;
					hasChildren = true;
					break;
				case "chance":
					this.arg0 = XmlHelper.GetAttributeOpt(xml, "percent");
					this.target = null;
					hasChildren = true;
					break;
				case "loop":
					this.arg0 = XmlHelper.GetAttributeOpt(xml, "count");
					this.target = null;
					hasChildren = true;
					break;
				default:
					this.target = hasChildren ? XmlHelper.GetAttributeOpt(xml, "target") : XmlHelper.GetAttribute(xml, "target");
					this.arg0 = XmlHelper.GetAttribute(xml, "arg0");
					break;
			}

			if (hasChildren)
			{
				List<ParticleSystemLogicStep> steps = new List<ParticleSystemLogicStep>();
				foreach (XmlNode node in xml.ChildNodes)
				{
					if (node is XmlElement)
						steps.Add(new ParticleSystemLogicStep(node as XmlElement));
				}
				this.children = steps.ToArray();
			}
			else
				this.children = null;
		}
		
		internal void Write(BinaryWriter writer)
		{
			writer.Write(target != null);
			if (target != null) writer.Write(target);

			writer.Write(arg0 != null);
			if (arg0 != null) writer.Write(arg0);

			writer.Write(arg1 != null);
			if (arg1 != null) writer.Write(arg1);

			writer.Write(type != null);
			if (type != null) writer.Write(type);

			int count = -1;
			if (children != null) count = children.Length;
			writer.Write(count);
			if (children != null)
			{
				for (int i = 0; i < children.Length; i++)
					children[i].Write(writer);
			}
		}
#endif

		internal ParticleSystemLogicStep(ContentReader reader)
		{
			this.target = null;
			this.arg0 = null;
			this.arg1 = null;
			this.type = null;
			this.children = null;

			if (reader.ReadBoolean())
				target = reader.ReadString();

			if (reader.ReadBoolean())
				arg0 = reader.ReadString();

			if (reader.ReadBoolean())
				arg1 = reader.ReadString();

			if (reader.ReadBoolean())
				type = reader.ReadString();

			int count = reader.ReadInt32();
			if (count != -1)
			{
				this.children = new ParticleSystemLogicStep[count];
				for (int i = 0; i < count; i++)
					this.children[i] = new ParticleSystemLogicStep(reader);
			}
		}
	}

	/// <summary>
	/// Stores the data required by a runtime particle processor (for both CPU and GPU processors)
	/// </summary>
	public sealed class ParticleSystemRuntimeLogicData
	{
		private readonly GpuParticleProcessorData gpuSystemData;

		/// <summary>
		/// Get the processor data for a GPU particle processor
		/// </summary>
		public GpuParticleProcessorData GpuParticleProcessorData { get { return gpuSystemData; } }

		private readonly bool usesUserValues, usesLifeOrAge;

		/// <summary>
		/// True if this processor data uses 'user' values
		/// </summary>
		public bool SystemUsesUserValues { get { return usesUserValues; } }
		/// <summary>
		/// True if this processor data uses 'life' or 'age' values
		/// </summary>
		public bool SystemUsesLifeOrAgeValues { get { return usesLifeOrAge; } }
		
#if !XBOX360
		private readonly CpuParticleProcessorData cpuSystemData;

		/// <summary>
		/// Get the processor data for a CPU particle processor (Windows Only)
		/// </summary>
		public CpuParticleProcessorData CpuParticleProcessorData { get { return cpuSystemData; } }
#endif

		/// <summary>
		/// Construct the runtime data
		/// </summary>
		/// <param name="reader"></param>
		internal ParticleSystemRuntimeLogicData(ContentReader reader)
		{
			usesLifeOrAge = reader.ReadBoolean();
			usesUserValues = reader.ReadBoolean();

			gpuSystemData = new GpuParticleProcessorData(reader);
#if !XBOX360
			cpuSystemData = new CpuParticleProcessorData(reader);
#endif
		}

#if DEBUG && !XBOX360

		internal ParticleSystemRuntimeLogicData(ParticleSystemTypeData typeData, bool usesColour, CpuParticleProcessorData cpuData, ContentTargetPlatform targetPlatform, string pathToShaderSystem)
		{
			this.usesUserValues = false;
			this.usesLifeOrAge = false;

			ParticleSystemData.ComputeUsesUserValues(typeData.ParticleLogicData.Once, ref usesUserValues, ref usesLifeOrAge, typeData.GpuBufferPosition);
			ParticleSystemData.ComputeUsesUserValues(typeData.ParticleLogicData.Frame, ref usesUserValues, ref usesLifeOrAge, typeData.GpuBufferPosition);

			this.gpuSystemData = new GpuParticleProcessorData(typeData, usesColour, usesUserValues, usesLifeOrAge, targetPlatform, pathToShaderSystem);
			this.cpuSystemData = cpuData;
		}

		internal void Write(BinaryWriter writer, ContentTargetPlatform taretPlatform)
		{
			writer.Write(usesLifeOrAge);
			writer.Write(usesUserValues);

			gpuSystemData.Write(writer);
			if (taretPlatform != ContentTargetPlatform.Xbox360)
				cpuSystemData.Write(writer);
		}

#endif
	}

	/// <summary>
	/// Stores the data for a <see cref="ParticleSystem"/>. Load instances of this class using <see cref="IContentOwner"/>.
	/// </summary>
	public sealed class ParticleSystemData
	{
		private readonly ParticleSystemTypeData[] particleTypes;
		private readonly ParticleSystemLogicData system;
		private readonly bool systemUsesColourValues;
		private readonly string filename;
#if DEBUG && !XBOX360
		private readonly bool includeLogicTypeData;
		internal string FileName { get { return filename; } }
#endif

		/// <summary>
		/// Gets the logic data for this system
		/// </summary>
		public ParticleSystemLogicData SystemLogicData
		{
			get { return system; }
		}

		/// <summary>
		/// Gets the type data for the particle types defined in this system
		/// </summary>
		public ReadOnlyArrayCollection<ParticleSystemTypeData> ParticleTypeData
		{
			get { return new ReadOnlyArrayCollection<ParticleSystemTypeData>(this.particleTypes); }
		}

#if DEBUG && !XBOX360

		/// <summary>
		/// <para>Load particle system data from a file (Using this method directly is not recommended - use the content pipeline or hotloading)</para>
		/// <para>This constructor is only available on DEBUG Windows builds</para>
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="root"></param>
		/// <param name="targetPlatform"></param>
		/// <param name="includeLogicTypeData"></param>
		/// <param name="BuildTextureCallback"></param>
		/// <param name="pathToShaderSystem">the path to the xen shader system binaries</param>
		public ParticleSystemData(string filename, XmlElement root, ContentTargetPlatform targetPlatform, bool includeLogicTypeData, Converter<string, string> BuildTextureCallback, string pathToShaderSystem)
		{
			//push the culture, set an default culture to enforce number formats
			System.Globalization.CultureInfo storedCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

			System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			try
			{
				this.filename = filename;
				this.includeLogicTypeData = includeLogicTypeData;

				//parse the xml document
				List<XmlElement> particleNodes = new List<XmlElement>();

				XmlElement systemNode = null;

				foreach (XmlNode node in root.ChildNodes)
				{
					if (node is XmlElement)
					{
						if (node.Name == "particle")
							particleNodes.Add(node as XmlElement);
						if (node.Name == "system")
							systemNode = node as XmlElement;
					}
				}

				if (systemNode == null)
					throw new FormatException("XML data does not contain a \'system\' element");

				this.system = new ParticleSystemLogicData(filename, systemNode, particleNodes.ToArray(), out this.particleTypes, BuildTextureCallback);

				//determine if colours have been used in any of the particle types

				bool coloursUsed = false;

				//not the most efficient way, but it's only done once
				for (int i = 0; i < particleTypes.Length; i++)
				{
					foreach (ParticleSystemLogicStep step in particleTypes[i].ParticleLogicData.Frame)
						ComputeColoursUsed(step, ref coloursUsed);

					foreach (ParticleSystemLogicStep step in particleTypes[i].ParticleLogicData.Once)
						ComputeColoursUsed(step, ref coloursUsed);
				}
				this.systemUsesColourValues = coloursUsed;

				//calculate the maximum expected number of each particle type (using a test run of the particle system)
				ComputeExpectedCapacity();

				//create the CPU runtime logic
				CpuParticleProcessorData cpuData = null;

				if (targetPlatform != ContentTargetPlatform.Xbox360)
				{
					cpuData = new CpuParticleProcessorData();
					for (int i = 0; i < particleTypes.Length; i++)
						cpuData.AddParticleType(particleTypes[i]);
					cpuData.BuildAssembly();
				}

				//generate the runtime particle data (eg, GPU shaders, CPU code)
				for (int i = 0; i < particleTypes.Length; i++)
					particleTypes[i].CreateRuntimeLogic(coloursUsed, cpuData, targetPlatform, pathToShaderSystem);
			}
			finally
			{
				//reset the culture
				System.Threading.Thread.CurrentThread.CurrentCulture = storedCulture;
			}
		}

		/// <summary>
		/// Write the particle system data to a file (this method is only available on DEBUG Windows builds)
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="targetPlatform"></param>
		public void Write(BinaryWriter writer, ContentTargetPlatform targetPlatform)
		{
			writer.Write(filename);
			writer.Write(particleTypes.Length);
			for (int i = 0; i < particleTypes.Length; i++)
				particleTypes[i].Write(writer, targetPlatform, includeLogicTypeData);

			system.Write(writer);

			writer.Write(systemUsesColourValues);
		}

#endif

		//load from a content reader
		internal ParticleSystemData(ContentReader reader)
		{
			this.filename = reader.ReadString();
			int count = reader.ReadInt32();
			this.particleTypes = new ParticleSystemTypeData[count];

			string basePath = Path.GetDirectoryName(reader.AssetName);
			if (basePath.Length > 0)
				basePath += '\\';

			for (int i = 0; i < count; i++)
				this.particleTypes[i] = new ParticleSystemTypeData(reader,i, basePath);

			this.system = new ParticleSystemLogicData(reader);

			this.systemUsesColourValues = reader.ReadBoolean();
		}


		#region content loader internal wrapper

		//internal data used by the content pipeline for runtime loading
		internal class RuntimeReader : ContentTypeReader<ParticleSystemData>
		{
			protected override ParticleSystemData Read(ContentReader input, ParticleSystemData existingInstance)
			{
				if (existingInstance != null)
					existingInstance.UpdateTextures(input.ContentManager, Path.GetDirectoryName(input.AssetName));

				return existingInstance ?? new ParticleSystemData(input);
			}
		}

		//reload the textures
		internal void UpdateTextures(ContentManager content, string basePath)
		{
			for (int i = 0; i < this.particleTypes.Length; i++)
				this.particleTypes[i].UpdateTextures(content, basePath);
		}

		internal static string RuntimeReaderType
		{
			get { return typeof(RuntimeReader).AssemblyQualifiedName; }
		}

		#endregion

		//used by the particle system to create storage for particle types
		internal ParticleStore[] CreateParticleStore(Type processorType)
		{
			if (processorType == null)
				throw new ArgumentNullException();
			if (!typeof(IParticleProcessor).IsAssignableFrom(processorType))
				throw new ArgumentException("Type does not implement IParticleProcessor");

			//ParticleSystemData
			ParticleStore[] particleStore = new ParticleStore[this.particleTypes.Length];
			IParticleProcessor[] allProcessors = new IParticleProcessor[this.particleTypes.Length];

			//IParticleProcessor are the objects that actually run the particle logic code,
			//eg, the GPU processor

			for (int i = 0; i < particleStore.Length; i++)
			{
				//work out how many timesteps may be taken (based on max life of a particle)
				uint maxTimeSteps = (uint)Math.Ceiling(system.Frequency * system.GetEmitterMaximumLifespan(particleTypes[i].Name));

				allProcessors[i] = Activator.CreateInstance(processorType) as IParticleProcessor;

				particleStore[i] = new ParticleStore(maxTimeSteps, particleTypes[i], allProcessors[i], particleStore.Length, i);
			}

			bool coloursUsed = systemUsesColourValues;

			//initalise the processors
			for (int i = 0; i < particleStore.Length; i++)
				allProcessors[i].Initalise(particleTypes[i], allProcessors, coloursUsed, particleStore[i].MaxTimeSteps, this.system.Frequency,this.particleTypes[i].ExpectedMaxCapacity);

			return particleStore;
		}

		//used by the particle system to create profile particle storage.
		//profile mode works out how much memory a particle system will require
		internal ParticleStore[] CreateParticleProfilerStore()
		{
			//ParticleSystemData
			ParticleStore[] particleStore = new ParticleStore[this.particleTypes.Length];

			
			for (int i = 0; i < particleStore.Length; i++)
			{
				//work out how many timesteps may be taken (based on max life of a particle)
				uint maxTimeSteps = (uint)Math.Ceiling(system.Frequency * system.GetEmitterMaximumLifespan(particleTypes[i].Name));

				particleStore[i] = new ParticleStore(maxTimeSteps, particleTypes[i], null, particleStore.Length, i);
			}

			return particleStore;
		}

#if DEBUG && !XBOX360

		//used by the content pipeline and hotloading to work out memory requirements
		internal void ComputeExpectedCapacity()
		{
			//create a profiler system
			ParticleSystem system = ParticleSystem.CreateProfiler(this);

			//enable any toggles
			for (int i = 0; i < system.ToggleTriggers.Length; i++)
				system.ToggleTriggers[i].Enabled = true;

			system.BufferSpawnData();

			uint stepsPerIteration = 0;
			for (int i = 0; i < this.particleTypes.Length; i++)
				stepsPerIteration = Math.Max(stepsPerIteration, system.GetStore(i).MaxTimeSteps);

			//run the system for 5 iterations
			for (uint iter = 0; iter < 5; iter++)
			{
				for (uint step = 0; step < stepsPerIteration; step++)
					system.UpdateParticles();
			}

			//fire the triggers
			for (int i = 0; i < system.Triggers.Length; i++)
			{
				uint count = system.SystemData.Triggers[i].ParticleCapacityMultipler;
				for (uint c = 0; c < count; c++)
					system.Triggers[i].FireTrigger();
			}

			system.BufferSpawnData();

			//run for 5 more iterations
			for (uint iter = 0; iter < 5; iter++)
			{
				for (uint step = 0; step < stepsPerIteration; step++)
					system.UpdateParticles();
			}

			//measure the max particle counts
			for (int i = 0; i < system.ParticleTypeCount; i++)
				this.particleTypes[i].SetExpectedMaxCapacity(system.GetStore(i).MaxCount);
		}

		//work out if colours are used? (otherwise they aren't stored)
		private static void ComputeColoursUsed(ParticleSystemLogicStep step, ref bool coloursUsed)
		{
			//yeah, this is ugly :P
			if (!coloursUsed)
				coloursUsed =
					(step.target != null && (step.target == "red" || step.target == "blue" || step.target == "green" || step.target == "alpha")) ||
					(step.arg0 != null && (step.arg0 == "red" || step.arg0 == "blue" || step.arg0 == "green" || step.arg0 == "alpha")) ||
					(step.arg1 != null && (step.arg1 == "red" || step.arg1 == "blue" || step.arg1 == "green" || step.arg1 == "alpha"));

			if (!coloursUsed && step.children != null)
				foreach (ParticleSystemLogicStep child in step.children)
					ComputeColoursUsed(child, ref coloursUsed);
		}


		//work out if 'user' values are used? (otherwise they aren't stored)
		internal static void ComputeUsesUserValues(ReadOnlyArrayCollection<ParticleSystemLogicStep> steps, ref bool usesUserValues, ref bool usesLifeOrAge, bool validateUserGpuBuffer)
		{
			usesUserValues |= validateUserGpuBuffer;

			foreach (ParticleSystemLogicStep step in steps)
				ComputeUsesUserValues(step, ref usesUserValues, ref usesLifeOrAge, validateUserGpuBuffer);
		}

		static void ComputeUsesUserValues(ParticleSystemLogicStep step, ref bool usesUserValues, ref bool usesLifeOrAge, bool validateUserGpuBuffer)
		{
			//and more ugly. but it works
			if (!usesUserValues || validateUserGpuBuffer)
			{
				bool usesUserData = 
					(step.target != null && step.target.StartsWith("user")) ||
					(step.arg0 != null && step.arg0.StartsWith("user")) ||
					(step.arg1 != null && step.arg1.StartsWith("user"));
				usesUserValues |= usesUserData;

				if (validateUserGpuBuffer && usesUserData)
				{
					bool error = step.target != null && step.target != "user0" && step.target.StartsWith("user");

					if (error)
						throw new ArgumentException("Invalid XML: Particle system that declare 'gpu_buffer_position = true' cannot assign 'user1', 'user2' or 'user3'");
				}
			}

			if (!usesLifeOrAge)
				usesLifeOrAge =
					(step.arg0 != null && (step.arg0 == "life" || step.arg0 == "age")) ||
					(step.arg1 != null && (step.arg1 == "life" || step.arg1 == "age"));

			if (step.children != null && (!usesUserValues || !usesLifeOrAge))
			{
				foreach (ParticleSystemLogicStep child in step.children)
					ComputeUsesUserValues(child, ref usesUserValues, ref usesLifeOrAge, validateUserGpuBuffer);
			}
		}

#endif
	}

	/// <summary>
	/// <para>A trigger can be defined in a particle system</para>
	/// <para>Triggers can be fired by the application to generate particles on demand</para>
	/// </summary>
	public sealed class ParticleSystemTrigger
	{
		internal ParticleSystemTrigger(ParticleSystem system, int index)
		{
			this.parent = system;
			this.index = index;
			this.fireDetails = new List<ParticleSpawnValues>();
			this.fireDetailsBuffer = new List<ParticleSpawnValues>();
		}

		private readonly ParticleSystem parent;
		private readonly int index;
		private List<ParticleSpawnValues> fireDetails, fireDetailsBuffer;

		/// <summary>
		/// Name of this trigger
		/// </summary>
		public string Name
		{
			get { return parent.SystemData.Triggers[index].Name; }
		}

		//thread buffer
		internal void Buffer()
		{
			List<ParticleSpawnValues> temp = fireDetails;
			//swap the lists
			fireDetails = fireDetailsBuffer;
			fireDetailsBuffer = temp;
		}

		#region fire trigger

		/// <summary>
		/// Fire this trigger once, at a given location
		/// </summary>
		/// <param name="particleEmitPositon"></param>
		public void FireTrigger(ref Vector3 particleEmitPositon)
		{
			ParticleSpawnValues values = ParticleSpawnValues.Default;
			values.PositionSize.X = particleEmitPositon.X;
			values.PositionSize.Y = particleEmitPositon.Y;
			values.PositionSize.Z = particleEmitPositon.Z;
			fireDetails.Add(values);
		}
		/// <summary>
		/// Fire this trigger once, at a given location
		/// </summary>
		/// <param name="particleEmitPositon"></param>
		public void FireTrigger(Vector3 particleEmitPositon)
		{
			ParticleSpawnValues values = ParticleSpawnValues.Default;
			values.PositionSize.X = particleEmitPositon.X;
			values.PositionSize.Y = particleEmitPositon.Y;
			values.PositionSize.Z = particleEmitPositon.Z;
			fireDetails.Add(values);
		}
		/// <summary>
		/// Fire this trigger once, at a given location and size
		/// </summary>
		/// <param name="particleEmitPositon"></param>
		/// <param name="size"></param>
		public void FireTrigger(ref Vector3 particleEmitPositon, float size)
		{
			ParticleSpawnValues values = ParticleSpawnValues.Default;
			values.PositionSize.X = particleEmitPositon.X;
			values.PositionSize.Y = particleEmitPositon.Y;
			values.PositionSize.Z = particleEmitPositon.Z;
			values.PositionSize.W = size;
			fireDetails.Add(values);
		}
		/// <summary>
		/// Fire this trigger once, at a given location and size
		/// </summary>
		/// <param name="particleEmitPositon"></param>
		/// <param name="size"></param>
		public void FireTrigger(Vector3 particleEmitPositon, float size)
		{
			ParticleSpawnValues values = ParticleSpawnValues.Default;
			values.PositionSize.X = particleEmitPositon.X;
			values.PositionSize.Y = particleEmitPositon.Y;
			values.PositionSize.Z = particleEmitPositon.Z;
			values.PositionSize.W = size;
			fireDetails.Add(values);
		}

		/// <summary>
		/// Fire this trigger once, at a given location and size, velocity and rotation
		/// </summary>
		/// <param name="particleEmitPositon"></param>
		/// <param name="rotation"></param>
		/// <param name="size"></param>
		/// <param name="velocity"></param>
		public void FireTrigger(ref Vector3 particleEmitPositon, float size, ref Vector3 velocity, float rotation)
		{
			ParticleSpawnValues values = ParticleSpawnValues.Default;
			values.PositionSize.X = particleEmitPositon.X;
			values.PositionSize.Y = particleEmitPositon.Y;
			values.PositionSize.Z = particleEmitPositon.Z;
			values.PositionSize.W = size;
			values.VelocityRotation.X = velocity.X;
			values.VelocityRotation.Y = velocity.Y;
			values.VelocityRotation.Z = velocity.Z;
			values.VelocityRotation.W = rotation;
			fireDetails.Add(values);
		}
		/// <summary>
		/// Fire this trigger once, at a given location and size, velocity and rotation
		/// </summary>
		/// <param name="particleEmitPositon"></param>
		/// <param name="rotation"></param>
		/// <param name="size"></param>
		/// <param name="velocity"></param>
		public void FireTrigger(Vector3 particleEmitPositon, float size, Vector3 velocity, float rotation)
		{
			ParticleSpawnValues values = ParticleSpawnValues.Default;
			values.PositionSize.X = particleEmitPositon.X;
			values.PositionSize.Y = particleEmitPositon.Y;
			values.PositionSize.Z = particleEmitPositon.Z;
			values.PositionSize.W = size;
			values.VelocityRotation.X = velocity.X;
			values.VelocityRotation.Y = velocity.Y;
			values.VelocityRotation.Z = velocity.Z;
			values.VelocityRotation.W = rotation;
			fireDetails.Add(values);
		}


		/// <summary>
		/// <para>Fire this trigger once, at a given location and size, velocity and rotation</para>
		/// <para>Note: Setting a default colour for particle systems that do not access colour values will have no effect (the colour values are optimized out)</para>
		/// </summary>
		/// <param name="particleEmitPositon"></param>
		/// <param name="rotation"></param>
		/// <param name="size"></param>
		/// <param name="velocity"></param>
		/// <param name="colour"></param>
		public void FireTrigger(ref Vector3 particleEmitPositon, float size, ref Vector3 velocity, float rotation, ref Vector4 colour)
		{
			ParticleSpawnValues values = ParticleSpawnValues.Default;
			values.PositionSize.X = particleEmitPositon.X;
			values.PositionSize.Y = particleEmitPositon.Y;
			values.PositionSize.Z = particleEmitPositon.Z;
			values.PositionSize.W = size;
			values.VelocityRotation.X = velocity.X;
			values.VelocityRotation.Y = velocity.Y;
			values.VelocityRotation.Z = velocity.Z;
			values.VelocityRotation.W = rotation;
			values.Colour = colour;
			fireDetails.Add(values);
		}
		/// <summary>
		/// <para>Fire this trigger once, at a given location and size, velocity and rotation</para>
		/// <para>Note: Setting a default colour for particle systems that do not access colour values will have no effect (the colour values are optimized out)</para>
		/// </summary>
		/// <param name="particleEmitPositon"></param>
		/// <param name="rotation"></param>
		/// <param name="size"></param>
		/// <param name="velocity"></param>
		/// <param name="colour"></param>
		public void FireTrigger(Vector3 particleEmitPositon, float size, Vector3 velocity, float rotation, Vector4 colour)
		{
			ParticleSpawnValues values = ParticleSpawnValues.Default;
			values.PositionSize.X = particleEmitPositon.X;
			values.PositionSize.Y = particleEmitPositon.Y;
			values.PositionSize.Z = particleEmitPositon.Z;
			values.PositionSize.W = size;
			values.VelocityRotation.X = velocity.X;
			values.VelocityRotation.Y = velocity.Y;
			values.VelocityRotation.Z = velocity.Z;
			values.VelocityRotation.W = rotation;
			values.Colour = colour;
			fireDetails.Add(values);
		}


		/// <summary>
		/// <para>Fire this trigger once, at a given location and size, velocity and rotation</para>
		/// <para>Note: Setting a default colour for particle systems that do not access colour values will have no effect (the colour values are optimized out)</para>
		/// <para>Note: Setting default user values for particle systems that do not access user values will have no effect (the user values are optimized out)</para>
		/// </summary>
		/// <param name="particleEmitPositon"></param>
		/// <param name="rotation"></param>
		/// <param name="size"></param>
		/// <param name="velocity"></param>
		/// <param name="colour"></param>
		/// <param name="userValues"></param>
		public void FireTrigger(ref Vector3 particleEmitPositon, float size, ref Vector3 velocity, float rotation, ref Vector4 colour, ref Vector4 userValues)
		{
			ParticleSpawnValues values = new ParticleSpawnValues();
			values.PositionSize.X = particleEmitPositon.X;
			values.PositionSize.Y = particleEmitPositon.Y;
			values.PositionSize.Z = particleEmitPositon.Z;
			values.PositionSize.W = size;
			values.VelocityRotation.X = velocity.X;
			values.VelocityRotation.Y = velocity.Y;
			values.VelocityRotation.Z = velocity.Z;
			values.VelocityRotation.W = rotation;
			values.Colour = colour;
			values.UserValues = userValues;
			fireDetails.Add(values);
		}
		/// <summary>
		/// <para>Fire this trigger once, at a given location and size, velocity and rotation</para>
		/// <para>Note: Setting a default colour for particle systems that do not access colour values will have no effect (the colour values are optimized out)</para>
		/// <para>Note: Setting default user values for particle systems that do not access user values will have no effect (the user values are optimized out)</para>
		/// </summary>
		/// <param name="particleEmitPositon"></param>
		/// <param name="rotation"></param>
		/// <param name="size"></param>
		/// <param name="velocity"></param>
		/// <param name="colour"></param>
		/// <param name="userValues"></param>
		public void FireTrigger(Vector3 particleEmitPositon, float size, Vector3 velocity, float rotation, Vector4 colour, ref Vector4 userValues)
		{
			ParticleSpawnValues values = new ParticleSpawnValues();
			values.PositionSize.X = particleEmitPositon.X;
			values.PositionSize.Y = particleEmitPositon.Y;
			values.PositionSize.Z = particleEmitPositon.Z;
			values.PositionSize.W = size;
			values.VelocityRotation.X = velocity.X;
			values.VelocityRotation.Y = velocity.Y;
			values.VelocityRotation.Z = velocity.Z;
			values.VelocityRotation.W = rotation;
			values.Colour = colour;
			values.UserValues = userValues;
			fireDetails.Add(values);
		}

		#endregion

		/// <summary>
		/// Fire this trigger once at 0,0,0
		/// </summary>
		public void FireTrigger()
		{
			fireDetails.Add(ParticleSpawnValues.Default);
		}

		//run the trigger logic
		internal void Run(int step, ref ParticleSpawnValues fireDetails)
		{
			if (fireDetailsBuffer.Count > 0)
			{
				ParticleSystemEmitterLogic data = parent.SystemData.Triggers[index];

				if (data != null)
				{
					foreach (ParticleSpawnValues location in fireDetailsBuffer)
					{
						fireDetails = location;
						data.Run(parent, step);
					}
				}

				fireDetailsBuffer.Clear();
			}
		}
	}


	/// <summary>
	/// <para>A toggle trigger can be defined in a particle system</para>
	/// <para>Toggle Triggers can be enabled / disabled by the application to generate particles on demand</para>
	/// </summary>
	public sealed class ParticleSystemToggleTrigger
	{
		internal ParticleSystemToggleTrigger(ParticleSystem system, int index)
		{
			this.parent = system;
			this.index = index;
		}

		private readonly ParticleSystem parent;
		private readonly int index;
		private bool enabled, enabledBuffer;
		private ParticleSpawnValues spawnDetails, spawnDetailsBuffer;

		/// <summary>
		/// Name of this toggle trigger
		/// </summary>
		public string Name
		{
			get { return parent.SystemData.Triggers[index].Name; }
		}

		//thread buffer
		internal void Buffer()
		{
			spawnDetailsBuffer = spawnDetails;
			enabledBuffer = enabled;
		}

		/// <summary>
		/// Gets/Sets if this toggle trigger is enabled
		/// </summary>
		public bool Enabled
		{
			get { return enabled; }
			set { enabled = value; }
		}

		#region spawn defaults

		/// <summary>
		/// Gets/Sets the default position of particle emitted by this toggle trigger
		/// </summary>
		public Vector3 DefaultParticleEmitPosition
		{
			get
			{
				return new Vector3(
					this.spawnDetails.PositionSize.X,
					this.spawnDetails.PositionSize.Y,
					this.spawnDetails.PositionSize.Z);
			}
			set
			{
				this.spawnDetails.PositionSize.X = value.X;
				this.spawnDetails.PositionSize.Y = value.Y;
				this.spawnDetails.PositionSize.Z = value.Z;
			}
		}
		/// <summary>
		/// Gets/Sets the default velocity of particle emitted by this toggle trigger
		/// </summary>
		public Vector3 DefaultParticleEmitVelocity
		{
			get
			{
				return new Vector3(
					this.spawnDetails.VelocityRotation.X,
					this.spawnDetails.VelocityRotation.Y,
					this.spawnDetails.VelocityRotation.Z);
			}
			set
			{
				this.spawnDetails.VelocityRotation.X = value.X;
				this.spawnDetails.VelocityRotation.Y = value.Y;
				this.spawnDetails.VelocityRotation.Z = value.Z;
			}
		}
		/// <summary>
		/// <para>Gets/Sets the default colour of particle emitted by this toggle trigger</para>
		/// <para>Note: Setting a default colour for particle systems that do not access colour values will have no effect (the colour values are optimized out)</para>
		/// </summary>
		public Vector4 DefaultParticleEmitColour
		{
			get
			{
				return this.spawnDetails.Colour;
			}
			set
			{
				this.spawnDetails.Colour = value;
			}
		}
		/// <summary>
		/// Gets/Sets the default user values of particle emitted by this toggle trigger
		/// <para>Note: Setting default user values for particle systems that do not access user values will have no effect (the user values are optimized out)</para>
		/// </summary>
		public Vector4 DefaultParticleEmitUserValues
		{
			get
			{
				return this.spawnDetails.UserValues;
			}
			set
			{
				this.spawnDetails.UserValues = value;
			}
		}
		/// <summary>
		/// Gets/Sets the default size of particle emitted by this toggle trigger
		/// </summary>
		public float DefaultParticleEmitSize
		{
			get
			{
				return this.spawnDetails.PositionSize.W;
			}
			set
			{
				this.spawnDetails.PositionSize.W = value;
			}
		}
		/// <summary>
		/// Gets/Sets the default rotation of particle emitted this toggle trigger
		/// </summary>
		public float DefaultParticleEmitRotation
		{
			get
			{
				return this.spawnDetails.VelocityRotation.W;
			}
			set
			{
				this.spawnDetails.VelocityRotation.W = value;
			}
		}

		#endregion

		//run the logic
		internal void Run(int step, ref ParticleSpawnValues fireDetails)
		{
			if (enabledBuffer)
			{
				fireDetails = this.spawnDetailsBuffer;
				ParticleSystemEmitterLogic data = parent.SystemData.Triggers[index];
				if (data != null)
					data.Run(parent, step);
			}
		}
	}

	/// <summary>
	/// <para>Stores information about a particle system, such as Emitters and Triggers</para>
	/// </summary>
	public sealed class ParticleSystemLogicData
	{
		private readonly ParticleSystemEmitterLogic once, frame;
		private readonly ParticleSystemEmitterLogic[] triggers, toggles;
		private readonly Dictionary<string, float> particleMaximumLife;
		
		private readonly UpdateFrequency frameRate;
		private readonly uint frequency;

		/// <summary>
		/// Gets the frequency the particle system is processed at
		/// </summary>
		public uint Frequency { get { return frequency; } }
		/// <summary>
		/// Gets the logic data for the emitter run when the particle system is created
		/// </summary>
		public ParticleSystemEmitterLogic OnceEmitter { get { return once; } }
		/// <summary>
		/// Gets the logic data for the emitter run every frame by the particle system
		/// </summary>
		public ParticleSystemEmitterLogic FrameEmitter { get { return frame; } }

		internal UpdateFrequency UpdateFrequency { get { return frameRate; } }

		/// <summary>
		/// Gets the definitons for the triggers used by this particle system data
		/// </summary>
		public ReadOnlyArrayCollection<ParticleSystemEmitterLogic> Triggers 
		{ 
			get { return new ReadOnlyArrayCollection<ParticleSystemEmitterLogic>(this.triggers); }
		}
		/// <summary>
		/// Gets the definitons for the toggle triggers used by this particle system data
		/// </summary>
		public ReadOnlyArrayCollection<ParticleSystemEmitterLogic> ToggleTriggers
		{
			get { return new ReadOnlyArrayCollection<ParticleSystemEmitterLogic>(this.toggles); }
		}
		
#if DEBUG && !XBOX360

		//parse an the xml data...
		internal ParticleSystemLogicData(string baseFileName, XmlElement xml, XmlElement[] particleTypeDefs, out ParticleSystemTypeData[] particleTypes, Converter<string, string> BuildTextureCallback)
		{
			//maximum life of a particle type by name
			particleMaximumLife = new Dictionary<string,float>();

			List<ParticleSystemEmitterLogic> triggers = new List<ParticleSystemEmitterLogic>();
			List<ParticleSystemEmitterLogic> toggles = new List<ParticleSystemEmitterLogic>();

			switch (XmlHelper.GetAttribute(xml, "frame_rate"))
			{
				case "60hz":
					this.frameRate = UpdateFrequency.FullUpdate60hz;
					this.frequency = 60;
					break;
				case "30hz":
					this.frameRate = UpdateFrequency.HalfUpdate30hz;
					this.frequency = 30;
					break;
				case "15hz":
					this.frameRate = UpdateFrequency.PartialUpdate15hz;
					this.frequency = 30;
					break;
				default:
					throw new ArgumentException("XML element \'system\' has invalid attribute value \'frame_rate\'");
			}


			foreach (XmlNode node in xml.ChildNodes)
			{
				if (node is XmlElement)
				{
					switch (node.Name)
					{
						case "once":
							once = new ParticleSystemEmitterLogic(node as XmlElement, particleMaximumLife, this.frequency, null);
							break;
						case "frame":
							frame = new ParticleSystemEmitterLogic(node as XmlElement, particleMaximumLife, this.frequency, null);
							break;
						case "triggers":
						case "toggles":
							foreach (XmlNode subNode in node.ChildNodes)
							{
								if (subNode is XmlElement)
								{
									if (subNode.Name == "trigger")
									{
										ParticleSystemEmitterLogic trigger = new ParticleSystemEmitterLogic(subNode as XmlElement, particleMaximumLife, this.frequency, null, true);
										triggers.Add(trigger);
									}
									if (subNode.Name == "toggle")
									{
										ParticleSystemEmitterLogic toggle = new ParticleSystemEmitterLogic(subNode as XmlElement, particleMaximumLife, this.frequency, null, true);
										toggles.Add(toggle);
									}
								}
							}
							break;
					}
				}
			}


			this.toggles = toggles.ToArray();
			this.triggers = triggers.ToArray();

			particleTypes = new ParticleSystemTypeData[particleTypeDefs.Length];

			for (int i = 0; i < particleTypeDefs.Length; i++)
				particleTypes[i] = new ParticleSystemTypeData(baseFileName,particleTypeDefs[i],i, particleMaximumLife, frequency, BuildTextureCallback);
			

			if (once != null)
				once.UpdateEmitters(particleTypes);
			if (frame != null)
				frame.UpdateEmitters(particleTypes);

			for (int i = 0; i < particleTypes.Length; i++)
				particleTypes[i].UpdateEmitters(particleTypes);

			for (int i = 0; i < this.toggles.Length; i++)
				this.toggles[i].UpdateEmitters(particleTypes);
			for (int i = 0; i < this.triggers.Length; i++)
				this.triggers[i].UpdateEmitters(particleTypes);
		}

		internal void Write(BinaryWriter writer)
		{
			writer.Write(once != null);
			if (once != null)
				once.Write(writer);

			writer.Write(frame != null);
			if (frame != null)
				frame.Write(writer);

			writer.Write(triggers.Length);
			for (int i = 0; i < triggers.Length; i++)
				triggers[i].Write(writer);

			writer.Write(toggles.Length);
			for (int i = 0; i < toggles.Length; i++)
				toggles[i].Write(writer);


			writer.Write(particleMaximumLife.Count);

			foreach (KeyValuePair<string,float> kvp in particleMaximumLife)
			{
				writer.Write(kvp.Key);
				writer.Write(kvp.Value);
			}

			writer.Write((int)this.frameRate);
			writer.Write(this.frequency);
		}
#endif
		/// <summary>
		/// Load particle system data from a content stream
		/// </summary>
		/// <param name="reader"></param>
		public ParticleSystemLogicData(ContentReader reader)
		{
			bool isNotNull = reader.ReadBoolean();
			if (isNotNull)
				once = new ParticleSystemEmitterLogic(reader);

			isNotNull = reader.ReadBoolean();
			if (isNotNull)
				frame = new ParticleSystemEmitterLogic(reader);

			int count = reader.ReadInt32();
			this.triggers = new ParticleSystemEmitterLogic[count];
			for (int i = 0; i < count; i++)
				this.triggers[i] = new ParticleSystemEmitterLogic(reader);

			count = reader.ReadInt32();
			this.toggles = new ParticleSystemEmitterLogic[count];
			for (int i = 0; i < count; i++)
				this.toggles[i] = new ParticleSystemEmitterLogic(reader);

			count = reader.ReadInt32();

			particleMaximumLife = new Dictionary<string, float>();
			for (int i = 0; i < count; i++)
				particleMaximumLife.Add(reader.ReadString(), reader.ReadSingle());

			frameRate = (UpdateFrequency)reader.ReadInt32();
			frequency = reader.ReadUInt32();
		}

		/// <summary>
		/// Gets the maximum lifespan of a particle type
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public float GetEmitterMaximumLifespan(string name)
		{
			if (particleMaximumLife.ContainsKey(name) == false) return 0;
			return particleMaximumLife[name];
		}
	}

	/// <summary>
	/// Stores the data for a specific particle type declared in a particle system
	/// </summary>
	public sealed class ParticleSystemTypeData
	{
		private readonly string name;
		private AlphaBlendState blend;
		private readonly string textureName;
		private Texture2D texture;
		private readonly ParticleSystemTypeLogicData particleLogic;
		private uint expectedMaxCapacity;
		private readonly int index;
		private readonly bool gpuBufferPosition;

		private readonly ParticleSystemEmitterLogic frameEmitter, removeEmitter;
		private ParticleSystemRuntimeLogicData runtimeLogic;

		/// <summary>
		/// Gets the name of the particle type
		/// </summary>
		public string Name { get { return name; } }
		/// <summary>
		/// Gets the index of the particle type (as declared in the particle system)
		/// </summary>
		public int TypeIndex { get { return index; } }
		/// <summary>
		/// Gets the name of the texture used by this particle type
		/// </summary>
		public string TextureName { get { return textureName; } }
		/// <summary>
		/// Gets the texture used by this particle type (may be null)
		/// </summary>
		public Texture2D Texture { get { return texture; } }
		/// <summary>
		/// Gets / Sets the blend mode used by this particle type
		/// </summary>
		public AlphaBlendState BlendMode { get { return blend; } set { blend = value; } }
		/// <summary>
		/// Gets the logic data for this particle type
		/// </summary>
		public ParticleSystemTypeLogicData ParticleLogicData { get { return particleLogic; } }

		/// <summary>
		/// Gets the per-frame emitter logic for this particle type
		/// </summary>
		public ParticleSystemEmitterLogic FrameEmitter { get { return frameEmitter; } }
		/// <summary>
		/// Gets the one time emitter logic for this particle type
		/// </summary>
		public ParticleSystemEmitterLogic RemoveEmitter { get { return removeEmitter; } }

		/// <summary>
		/// Gets the maximum expected number of particles of this type that will be created
		/// </summary>
		public uint ExpectedMaxCapacity { get { return expectedMaxCapacity; } }
		/// <summary>
		/// Gets the runtime logic data for this particle type
		/// </summary>
		public ParticleSystemRuntimeLogicData RuntimeLogicData { get { return runtimeLogic; } }

		/// <summary>
		/// True if the GPU particle processor buffers initial particle position XYZ in user1,user2,user3
		/// </summary>
		public bool GpuBufferPosition { get { return gpuBufferPosition; } }

#if DEBUG && !XBOX360

		internal ParticleSystemTypeData(string baseFileName, XmlElement xml, int index, Dictionary<string, float> maxLife, float frequency, Converter<string, string> BuildTextureCallback)
		{
			this.index = index;
			this.name = XmlHelper.GetAttribute(xml, "name");

			//reloading the texture at runtime
			if (BuildTextureCallback != null)
				this.textureName = BuildTextureCallback(XmlHelper.GetAttribute(xml, "texture"));
			else
			{
				string texture = XmlHelper.GetAttribute(xml, "texture");
				if (texture != null && texture.Length > 0)
					this.textureName = new FileInfo(baseFileName).Directory.FullName + @"/" + XmlHelper.GetAttribute(xml, "texture");
				else
					this.textureName = "";
			}

			this.particleLogic = new ParticleSystemTypeLogicData(xml);

			string gpuBufferPositionValue = XmlHelper.GetAttributeOpt(xml, "gpu_buffer_position");
			this.gpuBufferPosition = gpuBufferPositionValue == "true" || gpuBufferPositionValue == "1";

			switch (XmlHelper.GetAttributeOpt(xml, "blend"))
			{
				case "additive":
					this.blend = AlphaBlendState.Additive;
					break;
				case "additive_saturate":
					this.blend = AlphaBlendState.AdditiveSaturate;
					break;
				case "subtract":
					this.blend = AlphaBlendState.Additive;
					this.blend.BlendOperation = BlendFunction.ReverseSubtract;
					break;
				case "subtract_saturate":
					this.blend = AlphaBlendState.Additive;
					this.blend.SourceBlend = Blend.DestinationColor;
					this.blend.BlendOperation = BlendFunction.ReverseSubtract;
					break;
				default:
					this.blend = AlphaBlendState.PremodulatedAlpha;
					break;
			}

			foreach (XmlNode node in xml.ChildNodes)
			{
				if (node is XmlElement && node.Name == "emitter")
				{
					foreach (XmlNode emitter in node.ChildNodes)
					{
						if (emitter is XmlElement)
						{
							switch (emitter.Name)
							{
								default:
									throw new ArgumentException(string.Format("Unexpected emitter type \'{0}\'", emitter.Name));
								case "frame":
									this.frameEmitter = new ParticleSystemEmitterLogic(emitter as XmlElement, maxLife, frequency, name);
									break;
								case "remove":
									this.removeEmitter = new ParticleSystemEmitterLogic(emitter as XmlElement, maxLife, frequency, name);
									break;
							}
						}
					}
				}
			}
		}

		internal void CreateRuntimeLogic(bool coloursUsed, CpuParticleProcessorData cpuData, ContentTargetPlatform targetPlatform, string pathToShaderSystem)
		{
			this.runtimeLogic = new ParticleSystemRuntimeLogicData(this, coloursUsed, cpuData, targetPlatform, pathToShaderSystem);
		}

		internal void UpdateEmitters(ParticleSystemTypeData[] particleTypes)
		{
			if (frameEmitter != null)
				frameEmitter.UpdateEmitters(particleTypes);
			if (removeEmitter != null)
				removeEmitter.UpdateEmitters(particleTypes);
		}

		internal void Write(BinaryWriter writer, ContentTargetPlatform targetPlatform, bool includeLogicTypeData)
		{
			writer.Write(name);
			writer.Write((uint)blend);
			writer.Write(textureName);
			writer.Write(expectedMaxCapacity);
			writer.Write(gpuBufferPosition);

			writer.Write(includeLogicTypeData);
			if (includeLogicTypeData)
				particleLogic.Write(writer);

			writer.Write(frameEmitter != null);
			if (frameEmitter != null)
				frameEmitter.Write(writer);

			writer.Write(removeEmitter != null);
			if (removeEmitter != null)
				removeEmitter.Write(writer);

			this.runtimeLogic.Write(writer, targetPlatform);
		}
#endif

		internal ParticleSystemTypeData(ContentReader reader, int index, string basePath)
		{
			this.index = index;
			this.name = reader.ReadString();
			this.blend = (AlphaBlendState)reader.ReadUInt32();
			this.textureName = reader.ReadString();
			this.expectedMaxCapacity = reader.ReadUInt32();
			this.gpuBufferPosition = reader.ReadBoolean();

			bool includeLogic = reader.ReadBoolean();
			if (includeLogic)
				this.particleLogic = new ParticleSystemTypeLogicData(reader);

			bool isNotNull = reader.ReadBoolean();
			if (isNotNull)
				this.frameEmitter = new ParticleSystemEmitterLogic(reader);

			isNotNull = reader.ReadBoolean();
			if (isNotNull)
				this.removeEmitter = new ParticleSystemEmitterLogic(reader);

			this.runtimeLogic = new ParticleSystemRuntimeLogicData(reader);

			UpdateTextures(reader.ContentManager, basePath);
		}

		internal void SetExpectedMaxCapacity(uint expectedMaxCapacity)
		{
			//add ~20% and round to the next highest power of two

			expectedMaxCapacity += expectedMaxCapacity / 5;

			this.expectedMaxCapacity = Math.Max(this.expectedMaxCapacity, 4);
			while (expectedMaxCapacity > this.expectedMaxCapacity)
				this.expectedMaxCapacity *= 2;
		}

		//reload textures
		internal void UpdateTextures(ContentManager content, string basePath)
		{
			if (this.textureName == null || this.textureName.Length == 0)
				return;
#if DEBUG && !XBOX360
			if (basePath == null) // hotloading?
			{
				//don't try this at home...
				//load from disk directly
				GraphicsDevice device = (content.ServiceProvider.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService).GraphicsDevice;
				using (Stream fileStream = File.OpenRead(this.textureName))
					this.texture = Texture2D.FromStream(device, fileStream);
			}
			else
#endif
				this.texture = content.Load<Texture2D>(basePath.Length == 0 ? textureName : basePath + textureName);
		}
	}

	/// <summary>
	/// Logic action type for an emitter
	/// </summary>
	public enum ParticleSystemActionType
	{
		/// <summary>A chance based action</summary>
		Chance,
		/// <summary>An emitter action</summary>
		Emit,
		/// <summary>An interval based action</summary>
		Every,
		/// <summary>A looped action</summary>
		Loop
	}

	/// <summary>
	/// Data for a particle Emit operation
	/// </summary>
	public sealed class ParticleSystemEmitData
	{
		internal readonly int baseLifeTimeStep, varianceTimeStep;
#if DEBUG && !XBOX360
		private readonly string typeName, emitFromType;
#endif
		internal int typeIndex, emitFromTypeIndex;

		/// <summary>
		/// Minimum life of the particle, in time steps
		/// </summary>
		public int BaseLifeTimeStep { get { return baseLifeTimeStep; } }
		/// <summary>
		/// Random variance of the life of the particle, in time steps
		/// </summary>
		public int LifeVarianceTimeStep { get { return varianceTimeStep; } }

#if DEBUG && !XBOX360

		/// <summary>
		/// (DEBUG Windows builds only)
		/// </summary>
		public string EmitTypeName { get { return typeName; } }
		/// <summary>
		/// (DEBUG Windows builds only)
		/// </summary>
		public string EmitFromTypeName { get { return emitFromType; } }


		internal ParticleSystemEmitData(int life, int variance, string type, string emitFromType)
		{
			this.baseLifeTimeStep = (life - variance);
			this.varianceTimeStep = (variance * 2);
			this.typeName = type;
			this.emitFromType = emitFromType;
			this.typeIndex = -1;
			this.emitFromTypeIndex = -1;
		}

		//get integer id's for the emitter names
		internal void UpdateEmitters(ParticleSystemTypeData[] particleTypes)
		{
			for (int i = 0; i < particleTypes.Length; i++)
			{
				if (particleTypes[i].Name == this.typeName)
				{
					this.typeIndex = i;
				}
				if (particleTypes[i].Name == this.emitFromType)
				{
					this.emitFromTypeIndex = i;
				}
			}
			if (this.typeIndex == -1)
				throw new ArgumentException(string.Format("Invalid XML \'emit name\': Definition for particle of type \'{0}\' was not found",typeName));

			if (emitFromTypeIndex != -1)
			{
				if (!particleTypes[this.typeIndex].GpuBufferPosition && particleTypes[this.emitFromTypeIndex].GpuBufferPosition)
					throw new ArgumentException(string.Format("Invalid XML gpu_buffer_position mismatch: Particle type '{0}' with 'gpu_buffer_position = {1}' emits type '{2}' with 'gpu_buffer_position = {3}'", emitFromType, particleTypes[emitFromTypeIndex].GpuBufferPosition, typeName, particleTypes[typeIndex].GpuBufferPosition));
			}
		}

		internal void Write(BinaryWriter writer)
		{
			writer.Write(this.baseLifeTimeStep);
			writer.Write(this.varianceTimeStep);

			writer.Write(this.typeIndex);
			writer.Write(this.emitFromTypeIndex);
		}

#endif
		internal ParticleSystemEmitData(ContentReader reader)
		{
			this.baseLifeTimeStep = reader.ReadInt32();
			this.varianceTimeStep = reader.ReadInt32();

			this.typeIndex = reader.ReadInt32();
			this.emitFromTypeIndex = reader.ReadInt32();
		}

	}
	
	//all internal particle timing logic uses integers, not floats
	/// <summary>
	/// Stores the action to perform in a particle system Emitter
	/// </summary>
	public struct ParticleSystemActionData
	{
		private readonly ParticleSystemActionType action;
		private readonly int value;
		private readonly ParticleSystemEmitData emitData;
		private readonly ParticleSystemActionData[] children;

		/// <summary>
		/// Type for this action
		/// </summary>
		public ParticleSystemActionType ActionType { get { return action; } }
		/// <summary>
		/// Value for this action (eg, loop count)
		/// </summary>
		public int Value { get { return value; } }
		/// <summary>
		/// EmitData for this action, if the action is an Emit action
		/// </summary>
		public ParticleSystemEmitData EmitData { get { return emitData; } }
		/// <summary>
		/// Child actions (eg, for a loop action)
		/// </summary>
		public ReadOnlyArrayCollection<ParticleSystemActionData> Children 
		{
			get 
			{
				if (children == null) return ReadOnlyArrayCollection<ParticleSystemActionData>.Empty;
				return new ReadOnlyArrayCollection<ParticleSystemActionData>(children); 
			}
		}

		internal ParticleSystemActionData(ParticleSystemActionType action, int value, ParticleSystemEmitData emitData, ParticleSystemActionData[] children)
		{
			this.value = value;
			this.action = action;
			this.emitData = emitData;
			this.children = children;
		}
#if DEBUG && !XBOX360

		internal void Write(BinaryWriter writer)
		{
			writer.Write((int)action);
			writer.Write(value);
			if (action == ParticleSystemActionType.Emit)
				emitData.Write(writer);
			int count = -1;

			if (children != null)
				count = children.Length;
			writer.Write(count);

			if (children != null)
				for (int i = 0; i < children.Length; i++)
					children[i].Write(writer);
		}

#endif

		internal ParticleSystemActionData(ContentReader reader)
		{
			this.emitData = null;
			this.children = null;

			this.action = (ParticleSystemActionType)reader.ReadInt32();
			this.value = reader.ReadInt32();
			if (this.action == ParticleSystemActionType.Emit)
				this.emitData = new ParticleSystemEmitData(reader);
			int count = reader.ReadInt32();
			if (count != -1)
			{
				this.children = new ParticleSystemActionData[count];
				for (int i = 0; i < count; i++)
					this.children[i] = new ParticleSystemActionData(reader);
			}
		}

		//run the action
		internal void Run(ParticleSystem system, int step)
		{
			switch (action)
			{
				case ParticleSystemActionType.Chance:
					{
						if (system.systemRandom.Next(65535) < value)
						{
							foreach (ParticleSystemActionData child in children)
								child.Run(system, step);
						}
					}
					break;
				case ParticleSystemActionType.Emit:
					{
						system.Emit((ParticleSystemEmitData)this.emitData,-1,-1);
					}
					break;
				case ParticleSystemActionType.Every:
					{
						if ((step % value) == 0)
						{
							foreach (ParticleSystemActionData child in children)
								child.Run(system, step);
						}
					}
					break;
				case ParticleSystemActionType.Loop:
					{
						for (int i = 0; i < value; i++)
						{
							foreach (ParticleSystemActionData child in children)
								child.Run(system, step);
						}
					}
					break;
			}
		}

		//run the action on a particle type
		internal void RunParticleType(ParticleSystem system, ParticleStore type, int step)
		{
			switch (action)
			{
				case ParticleSystemActionType.Chance:
					{
						type.RunParticleTypeOneByOne(system,step,ref this);
					}
					break;
				case ParticleSystemActionType.Emit:
					{
						type.EmitEach(system, (ParticleSystemEmitData)this.emitData, step);
					}
					break;
				case ParticleSystemActionType.Every:
					{
						if (value == 1)
						{
							foreach (ParticleSystemActionData child in children)
								child.RunParticleType(system, type, step);
						}
						else
						{
							type.RunParticleTypeEvery(system, step, value, ref this);
						}
					}
					break;
				case ParticleSystemActionType.Loop:
					{
						for (int i = 0; i < value; i++)
						{
							foreach (ParticleSystemActionData child in children)
								child.RunParticleType(system, type, step);
						}
					}
					break;
			}
		}

		//run the children of an action on a particle
		internal void RunParticleChildren(ParticleSystem system, ParticleStore type, uint index, float indexF, int step)
		{
			foreach (ParticleSystemActionData child in children)
				child.RunParticle(system, type, index, indexF, step);
		}

		//run the action on a specific particle
		internal void RunParticle(ParticleSystem system, ParticleStore type, uint index, float indexF, int step)
		{
			switch (action)
			{
				case ParticleSystemActionType.Chance:
					{
						if (system.systemRandom.Next(65535) < value)
						{
							foreach (ParticleSystemActionData child in children)
								child.RunParticle(system, type, index, indexF, step);
						}
					}
					break;
				case ParticleSystemActionType.Emit:
					{
						system.Emit((ParticleSystemEmitData)this.emitData, (int)index, indexF);
					}
					break;
				case ParticleSystemActionType.Every:
					{
						if ((step % value) == 0)
						{
							foreach (ParticleSystemActionData child in children)
								child.RunParticle(system, type, index, indexF, step);
						}
					}
					break;
				case ParticleSystemActionType.Loop:
					{
						for (int i = 0; i < value; i++)
						{
							foreach (ParticleSystemActionData child in children)
								child.RunParticle(system, type, index, indexF, step);
						}
					}
					break;
			}
		}

#if DEBUG && !XBOX360

		internal void UpdateEmitters(ParticleSystemTypeData[] particleTypes)
		{
			if (this.action == ParticleSystemActionType.Emit)
				(this.emitData as ParticleSystemEmitData).UpdateEmitters(particleTypes);

			if (children != null)
				foreach (ParticleSystemActionData child in children)
					child.UpdateEmitters(particleTypes);
		}

#endif
	}

	//all internal particle timing logic uses integers, not floats
	/// <summary>
	/// Stores the data for a emitter (or a trigger)
	/// </summary>
	public sealed class ParticleSystemEmitterLogic
	{
		//(name only applies to triggers)
		private readonly string name;
		private readonly uint particleCapacityMultiplier;
		private readonly ParticleSystemActionData[] actions;

		/// <summary>
		/// Name of the emitter, if it's a trigger or toggle trigger
		/// </summary>
		public string Name { get { return name ?? ""; } }
		/// <summary>
		/// Capacity multipler for Triggers
		/// </summary>
		public uint ParticleCapacityMultipler { get { return particleCapacityMultiplier; } }
		/// <summary>
		/// Actions performed by the emitter
		/// </summary>
		public ReadOnlyArrayCollection<ParticleSystemActionData> Actions { get { return new ReadOnlyArrayCollection<ParticleSystemActionData>(actions); } }
		
#if DEBUG && !XBOX360

		internal ParticleSystemEmitterLogic(XmlElement xml, Dictionary<string, float> maxLife, float frequency, string particleTypeName)
		{
			this.particleCapacityMultiplier = 1;

			if (xml == null)
				this.actions = new ParticleSystemActionData[0];
			else
				this.actions = BuildActions(xml, maxLife, frequency, particleTypeName);
		}

		internal ParticleSystemEmitterLogic(XmlElement xml, Dictionary<string, float> maxLife, float frequency, string particleTypeName, bool isTrigger)
			: this(xml, maxLife, frequency, particleTypeName)
		{
			if (isTrigger)
			{
				XmlAttribute att = xml.Attributes["name"];
				if (att != null)
					name = att.Value;

				att = xml.Attributes["allocation_multiplier"];
				if (att != null)
					uint.TryParse(att.Value, System.Globalization.NumberStyles.Integer | System.Globalization.NumberStyles.AllowThousands,System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out this.particleCapacityMultiplier);

				if (name == null || name.Length == 0)
					throw new ArgumentException("Invalid XML data. Trigger.Name is null or empty");
			}
		}


		private ParticleSystemActionData[] BuildActions(XmlElement xml, Dictionary<string, float> maxLife, float frequency, string particleTypeName)
		{
			List<ParticleSystemActionData> actions = new List<ParticleSystemActionData>();
			foreach (XmlNode node in xml)
			{
				if (node is XmlElement)
					actions.Add(BuildAction(node as XmlElement, maxLife, frequency, particleTypeName));
			}

			return actions.ToArray();
		}

		private ParticleSystemActionData BuildAction(XmlElement xml, Dictionary<string,float> maxLife, float frequency, string particleTypeName)
		{
			switch (xml.Name)
			{
				case "chance":
					{
						//integer chance between 0 and 65536
						int chance = Convert.ToInt32(XmlHelper.Parse(xml, "percent", 0, 100) * 65536) / 100;
						return new ParticleSystemActionData(ParticleSystemActionType.Chance, chance, null, BuildActions(xml, maxLife, frequency, particleTypeName));
					}
				case "emit":
					{
						//for sanity sake, keep the particles to a reasonable lifespan
						float life = XmlHelper.Parse(xml, "life", 0, 600);
						float variance = 0;
						if (xml.Attributes["life_variance"] != null)
							variance = XmlHelper.Parse(xml, "life_variance", 0, life);

						string typename = XmlHelper.GetAttribute(xml, "type");

						//store the maximum lifespan for this particle type
						float currentMaxLife;
						maxLife.TryGetValue(typename, out currentMaxLife);
						currentMaxLife = Math.Max(life + variance, currentMaxLife);
						maxLife[typename] = currentMaxLife;


						int lifeTimeStep = Convert.ToInt32(life * frequency);
						int varianceTimeStep = Convert.ToInt32(variance * frequency);

						return new ParticleSystemActionData(ParticleSystemActionType.Emit, 0, new ParticleSystemEmitData(lifeTimeStep, varianceTimeStep, typename, particleTypeName), null);
					}
				case "loop":
					return new ParticleSystemActionData(ParticleSystemActionType.Loop, Convert.ToInt32(XmlHelper.Parse(xml, "count", 1, null)), null, BuildActions(xml, maxLife, frequency, particleTypeName));
				case "every":
					{
						int interval = Math.Max(1,(int)Math.Ceiling(XmlHelper.Parse(xml, "interval", 0, null) * frequency));
						return new ParticleSystemActionData(ParticleSystemActionType.Every, interval, null, BuildActions(xml, maxLife, frequency, particleTypeName));
					}
				default:
					throw new ArgumentException("Invalid XML in emitter: " + xml.Name);
			}
		}

		internal void Write(BinaryWriter writer)
		{
			writer.Write(name != null);
			if (name != null)
				writer.Write(name);

			writer.Write(particleCapacityMultiplier);

			writer.Write(actions.Length);
			for (int i = 0; i < actions.Length; i++)
				actions[i].Write(writer);
			

		}
#endif

		internal ParticleSystemEmitterLogic(ContentReader reader)
		{
			bool isNotNull = reader.ReadBoolean();
			if (isNotNull)
				this.name = reader.ReadString();

			particleCapacityMultiplier = reader.ReadUInt32();

			int count = reader.ReadInt32();

			this.actions = new ParticleSystemActionData[count];
			for (int i = 0; i < count; i++)
				this.actions[i] = new ParticleSystemActionData(reader);
		}


		//run the emitter
		internal void Run(ParticleSystem systemLogic, int step)
		{
			for (int i = 0; i < actions.Length; i++)
				actions[i].Run(systemLogic, step);
		}

		//run for a specific particle
		internal void RunParticle(ParticleSystem systemLogic, ParticleStore store, uint index, float indexF, int step)
		{
			for (int i = 0; i < actions.Length; i++)
				actions[i].RunParticle(systemLogic, store, index, indexF, step);
		}

		//run for a particle type
		internal void RunParticleType(ParticleSystem systemLogic, ParticleStore store, int step)
		{
			for (int i = 0; i < actions.Length; i++)
				actions[i].RunParticleType(systemLogic, store, step);
		}
		
#if DEBUG && !XBOX360
		
		internal void UpdateEmitters(ParticleSystemTypeData[] particleTypes)
		{
			for (int i = 0; i < actions.Length; i++)
				actions[i].UpdateEmitters(particleTypes);
		}

#endif
	}
	
#if DEBUG && !XBOX360

	//general helper for various xml things
	static class XmlHelper
	{
		//get an optional attribute
		public static string GetAttributeOpt(XmlElement xml, string attributeName)
		{
			if (xml.Attributes[attributeName] == null)
				return null;
			return GetAttribute(xml, attributeName);
		}

		//get an attribute
		public static string GetAttribute(XmlElement xml, string attributeName)
		{
			XmlAttribute att = xml.Attributes[attributeName];
			if (att == null)
				throw new ArgumentException(string.Format("Invalid XML in emitter: Unable to find attribute \'{0}\' in element \'{1}\'", attributeName, xml.Name));
			return att.Value;
		}

		//parse a float
		public static float Parse(XmlElement xml, string attributeName, float? min, float? max)
		{
			string attValue = GetAttribute(xml, attributeName);
			float value;
			if (!float.TryParse(attValue, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out value))
				throw new ArgumentException(string.Format("Invalid XML in emitter: Unable to read decimal value \'{2}\' attribute \'{0}\' in element \'{1}\'", attributeName, xml.Name, attValue));

			if ((min != null && min.Value > value) ||
				(max != null && max.Value < value))
				throw new ArgumentException(string.Format("Invalid XML in emitter: Decimal value \'{2}\' attribute \'{0}\' in element \'{1}\' is outside the expected range ({3} to {4})", attributeName, xml.Name, attValue, min ?? float.NegativeInfinity, max ?? float.PositiveInfinity));

			return value;
		}
	}

#endif
}
