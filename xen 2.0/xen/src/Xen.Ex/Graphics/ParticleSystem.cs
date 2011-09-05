using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Xen.Graphics;
using Xen.Threading;
using Microsoft.Xna.Framework.Graphics;
using Xen.Ex.Graphics.Content;

namespace Xen.Ex.Graphics
{


	/// <summary>
	/// <para>Loads <see cref="ParticleSystemData"/> content and processes logic required to update a particle system.</para>
	/// <para>Per-Particle logic is performed by a particle processor class (either a CPU or GPU particle processor)</para>
	/// <para>Note: This class does not directly Draw a particle system, it only runs the logic to update a particle system</para>
	/// </summary>
	public sealed class ParticleSystem : IUpdate, IDisposable
	{
		private ParticleSystemData systemData;
		private ParticleSystemLogicData systemLogic;
		private ParticleStore[] particleStore;
		
		private ParticleSystemTrigger[] triggers;
		private ParticleSystemToggleTrigger[] toggles;

		//drawing the particles (for GPU drawing) must be done in
		//dependancy order
		private List<ParticleStore>[] particleStoreSortedDrawOrder;
		//each render step contains a sorted list of particle stores
		private int renderStepCount;
		
		private readonly SystemThreadAction threadAction;
		private readonly SystemDrawProc drawProc;

		internal readonly Random systemRandom = new Random();
		private int timeStep;
		private WaitCallback threadWaitCallback;
		private int[] typeDependancyList;

		private readonly float[] globalValues;
		private bool enabled;
		private bool disposed;

		private ParticleSpawnValues activeSpawnDetails, userSpawnDetails, userSpawnDetailsBuffer;
		private bool pauseUpdatesWhileCulled;

		/// <summary>
		/// Construct a particle system with data
		/// </summary>
		/// <param name="systemData"></param>
		/// <param name="update"></param>
		public ParticleSystem(ParticleSystemData systemData, UpdateManager update)
			: this()
		{
			if (update == null)
				throw new ArgumentNullException();

			Initalise(systemData, DefaultProcessorType);

			update.Add(this);
		}

		/// <summary>
		/// Construct the particle system. Before the particle system is used, the <see cref="ParticleSystemData"/> must be set
		/// </summary>
		/// <param name="update"></param>
		public ParticleSystem(UpdateManager update)
			: this()
		{
			if (update == null)
				throw new ArgumentNullException();

			update.Add(this);
		}

		//internal
		private ParticleSystem(ParticleSystemData systemData, Type processorType)
			: this()
		{
			Initalise(systemData, processorType);
		}
		private ParticleSystem()
		{
			activeSpawnDetails = ParticleSpawnValues.Default;
			userSpawnDetails = ParticleSpawnValues.Default;
			userSpawnDetailsBuffer = ParticleSpawnValues.Default;

			this.threadAction = new SystemThreadAction();
			this.threadAction.system = this;
			this.drawProc = new SystemDrawProc();
			this.globalValues = new float[16];
		}

		//type of the particle processor
		static Type DefaultProcessorType
		{
			get
			{
#if XBOX360
				return typeof(Processor.GpuParticleProcessor);
#else
				if (SystemSupportsGpuParticles && !ForceUseCpuParticleSystem)
					return typeof(Processor.GpuParticleProcessor);
				else
					return typeof(Processor.CpuParticleProcessor);
#endif
			}
		}

		internal void GetSpawnDetails(out ParticleSpawnValues details)
		{
			details = activeSpawnDetails;
		}

		/// <summary>
		/// <para>If true, the ParticleSystem will pause updating when nothing is currently drawing the particle system (default false)</para>
		/// <para>Setting this to true is recommended for looping effects that may not always have their displayers on screen</para>
		/// <para>Note: 3D Displays require thier <see cref="Display.ParticleDrawer3D.CullProxy"/> member to be set to perform culling, or to be manually culled by the application</para>
		/// </summary>
		public bool PauseUpdatingWhileCulled
		{
			get { return pauseUpdatesWhileCulled; }
			set { pauseUpdatesWhileCulled = value; }
		}

		#region spawn defaults

		/// <summary>
		/// Gets/Sets the default position of particle emitted by the particle system (particles emitted in the 'system' xml element)
		/// </summary>
		public Microsoft.Xna.Framework.Vector3 DefaultParticleEmitPosition
		{
			get 
			{
				return new Microsoft.Xna.Framework.Vector3(
					this.userSpawnDetails.PositionSize.X,
					this.userSpawnDetails.PositionSize.Y,
					this.userSpawnDetails.PositionSize.Z); 
			}
			set 
			{
				this.userSpawnDetails.PositionSize.X = value.X;
				this.userSpawnDetails.PositionSize.Y = value.Y;
				this.userSpawnDetails.PositionSize.Z = value.Z; 
			}
		}
		/// <summary>
		/// Gets/Sets the default velocity of particle emitted by the particle system (particles emitted in the 'system' xml element)
		/// </summary>
		public Microsoft.Xna.Framework.Vector3 DefaultParticleEmitVelocity
		{
			get
			{
				return new Microsoft.Xna.Framework.Vector3(
					this.userSpawnDetails.VelocityRotation.X,
					this.userSpawnDetails.VelocityRotation.Y,
					this.userSpawnDetails.VelocityRotation.Z);
			}
			set
			{
				this.userSpawnDetails.VelocityRotation.X = value.X;
				this.userSpawnDetails.VelocityRotation.Y = value.Y;
				this.userSpawnDetails.VelocityRotation.Z = value.Z;
			}
		}
		/// <summary>
		/// <para>Gets/Sets the default colour of particle emitted by the particle system (particles emitted in the 'system' xml element)</para>
		/// <para>Note: Setting a default colour for particle systems that do not access colour values will have no effect (the colour values are optimized out)</para>
		/// </summary>
		public Microsoft.Xna.Framework.Vector4 DefaultParticleEmitColour
		{
			get
			{
				return this.userSpawnDetails.Colour;
			}
			set
			{
				this.userSpawnDetails.Colour = value;
			}
		}
		/// <summary>
		/// Gets/Sets the default user values of particle emitted by the particle system (particles emitted in the 'system' xml element)
		/// <para>Note: Setting default user values for particle systems that do not access user values will have no effect (the user values are optimized out)</para>
		/// </summary>
		public Microsoft.Xna.Framework.Vector4 DefaultParticleEmitUserValues
		{
			get
			{
				return this.userSpawnDetails.UserValues;
			}
			set
			{
				this.userSpawnDetails.UserValues = value;
			}
		}
		/// <summary>
		/// Gets/Sets the default size of particle emitted by the particle system (particles emitted in the 'system' xml element)
		/// </summary>
		public float DefaultParticleEmitSize
		{
			get
			{
				return this.userSpawnDetails.PositionSize.W;
			}
			set
			{
				this.userSpawnDetails.PositionSize.W = value;
			}
		}
		/// <summary>
		/// Gets/Sets the default rotation of particle emitted by the particle system (particles emitted in the 'system' xml element)
		/// </summary>
		public float DefaultParticleEmitRotation
		{
			get
			{
				return this.userSpawnDetails.VelocityRotation.W;
			}
			set
			{
				this.userSpawnDetails.VelocityRotation.W = value;
			}
		}

		#endregion

		/// <summary>
		/// <para>Gets/Sets the <see cref="ParticleSystemData"/> to be used by this particle system instance.</para>
		/// <para>This value must be assigned before the particle system can be used.</para>
		/// </summary>
		public ParticleSystemData ParticleSystemData
		{
			get { return systemData; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();
				if (value == systemData)
					return;
				if (systemData != null)
					throw new InvalidOperationException("ParticleSystemData is already assigned");

				Initalise(value, DefaultProcessorType);
			}
		}


//any time this define is used, the code is only needed to build the 
//particle system data, or for hotloading
#if DEBUG && !XBOX360

		//used by the hotloader
		internal void SetParticleSystemHotloadedData(ParticleSystemData data)
		{
			//hackity hack
			Dispose();
			this.disposed = false;

			Initalise(data, DefaultProcessorType);
		}

		//a profiler is used to compute how much memory the particle system will use
		internal static ParticleSystem CreateProfiler(ParticleSystemData systemData)
		{
			return new ParticleSystem(systemData, (Type)null);
		}

#endif

		//creates the particle system storage classes (store all particle life data)
		private void Initalise(ParticleSystemData systemData, Type processorType)
		{
			if (systemData == null)
				throw new ArgumentNullException();

			this.systemData = systemData;
			this.enabled = true;

			//build the triggers
			this.triggers = new ParticleSystemTrigger[systemData.SystemLogicData.Triggers.Length];
			for (int i = 0; i < this.triggers.Length; i++)
				this.triggers[i] = new ParticleSystemTrigger(this, i);

			this.toggles = new ParticleSystemToggleTrigger[systemData.SystemLogicData.ToggleTriggers.Length];
			for (int i = 0; i < this.toggles.Length; i++)
				this.toggles[i] = new ParticleSystemToggleTrigger(this, i);


			this.systemLogic = systemData.SystemLogicData;

			//create the storage for the particle instances
			if (processorType == null)
				this.particleStore = systemData.CreateParticleProfilerStore();
			else
				this.particleStore = systemData.CreateParticleStore(processorType);

			this.particleStoreSortedDrawOrder = new List<ParticleStore>[4];
			//fill with a few empty entires
			for (int i = 0; i < particleStoreSortedDrawOrder.Length; i++)
				particleStoreSortedDrawOrder[i] = new List<ParticleStore>(particleStore.Length);

			typeDependancyList = new int[this.particleStore.Length * 2];


			activeSpawnDetails = this.userSpawnDetails;

			//run the 'once' emitter
			if (this.systemLogic.OnceEmitter != null)
				this.systemLogic.OnceEmitter.Run(this, 0);
		}

		/// <summary>
		/// Gets/Sets if this particle system performs updating
		/// </summary>
		public bool Enabled { get { return enabled; } set { enabled = value; } }

		//internal logic data
		internal ParticleSystemLogicData SystemData { get { return systemLogic; } }

		/// <summary>
		/// <para>Gets the particle system triggers defined by the particle system</para>
		/// <para>Triggers can be fired by the application to spawn particles on demand</para>
		/// </summary>
		public ReadOnlyArrayCollection<ParticleSystemTrigger> Triggers
		{
			get
			{
				if (disposed)
					throw new ObjectDisposedException("this");
				return new ReadOnlyArrayCollection<ParticleSystemTrigger>(this.triggers);
			}
		}
		/// <summary>
		/// <para>Gets the particle system toggle triggers defined by the particle system</para>
		/// <para>Toggle Triggers can be turned on and off by the application to spawn particles on demand</para>
		/// </summary>
		public ReadOnlyArrayCollection<ParticleSystemToggleTrigger> ToggleTriggers
		{
			get
			{
				if (disposed)
					throw new ObjectDisposedException("this");
				return new ReadOnlyArrayCollection<ParticleSystemToggleTrigger>(this.toggles);
			}
		}
		/// <summary>
		/// <para>Gets a particle system trigger by name, as defined in the particle system</para>
		/// <para>Triggers can be fired by the application to spawn particles on demand</para>
		/// </summary>
		public ParticleSystemTrigger GetTriggerByName(string name)
		{
			if (disposed)
				throw new ObjectDisposedException("this");
			foreach (ParticleSystemTrigger trigger in this.triggers)
				if (trigger.Name == name)
					return trigger;
			return null;
		}
		/// <summary>
		/// <para>Gets a particle system toggle trigger by name, as defined in the particle system</para>
		/// <para>Toggle Triggers can be turned on and off by the application to spawn particles on demand</para>
		/// </summary>
		public ParticleSystemToggleTrigger GetToggleTriggerByName(string name)
		{
			if (disposed)
				throw new ObjectDisposedException("this");
			foreach (ParticleSystemToggleTrigger toggle in this.toggles)
				if (toggle.Name == name)
					return toggle;
			return null;
		}
		/// <summary>
		/// Performs a linear search to find the particle system type data with the given name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ParticleSystemTypeData GetParticleTypeByName(string name)
		{
			if (this.systemData == null)
				throw new InvalidOperationException("ParticleSystemData == null");

			for (int i = 0; i < this.systemData.ParticleTypeData.Length; i++)
			{
				if (this.systemData.ParticleTypeData[i].Name == name)
					return this.systemData.ParticleTypeData[i];
			}
			return null;
		}

		//internal update method
		UpdateFrequency IUpdate.Update(UpdateState state)
		{
			if (disposed)
				return UpdateFrequency.Terminate;

			if (systemData == null)
				return UpdateFrequency.FullUpdate60hz;

			//draw proc only makes it's callback if system isn't null
			drawProc.system = null;

			if (enabled)
			{
				//system logic is performed as a thread task
				threadWaitCallback.WaitForCompletion();

				if (!pauseUpdatesWhileCulled ||
					this.drawProc.AnyChildDrawnLastFrame)
				{
					//particle system is enabled, and isn't offscreen
					BufferSpawnData();


					//start the task
					threadWaitCallback = state.Application.ThreadPool.QueueTask(threadAction, null);

					//draw proc only makes it's callback if system isn't null
					drawProc.system = this;
				}
			}

			//asks the application to call Draw() on the drawProc when the frame begins
			//as currnetly in update()
			state.PreFrameDraw(drawProc);

			return systemLogic.UpdateFrequency;
		}

		internal void BufferSpawnData()
		{

			//thread buffer the spawn location data...
			userSpawnDetailsBuffer = userSpawnDetails;

			foreach (ParticleSystemTrigger trigger in this.triggers)
				trigger.Buffer();
			foreach (ParticleSystemToggleTrigger toggle in this.toggles)
				toggle.Buffer();
		}

		//called on the task thread
		internal void UpdateParticles()
		{
			List<ParticleStore> sortedList = ComputeDependantOrderForFrame();

			//first time updating?
			if (timeStep == 0 && this.systemLogic.OnceEmitter != null)
			{
				uint totalParticles = 0;
				for (int i = 0; i < particleStore.Length; i++)
					totalParticles += particleStore[i].Count;

				if (totalParticles > 0) // special case, the OnceEmitter has created particles, need to run a processor pass first
					RunProcessorLogic(sortedList);
			}

			//update the particle types
			for (int i = 0; i < sortedList.Count; i++)
				sortedList[i].Update(this, timeStep);

			//spawn from here
			activeSpawnDetails = this.userSpawnDetailsBuffer;

			if (systemLogic.FrameEmitter != null)
				systemLogic.FrameEmitter.Run(this, timeStep);

			foreach (ParticleSystemTrigger trigger in this.triggers)
				trigger.Run(timeStep, ref activeSpawnDetails);

			foreach (ParticleSystemToggleTrigger toggle in this.toggles)
				toggle.Run(timeStep, ref activeSpawnDetails);

			//run the processor
			RunProcessorLogic(sortedList);

			for (int i = 0; i < this.particleStore.Length; i++)
				this.particleStore[i].ClearTypeEmitDependance();

			renderStepCount++;
			timeStep++;
		}

		private List<ParticleStore> ComputeDependantOrderForFrame()
		{

			//check for recursive particle emitting
			//(which doesn't work with the way rendering is performed on the xbox)

			for (int i = 0; i < this.particleStore.Length; i++)
				CheckEmitDependancy(i, 0);


			//store the particle stores into update dependancy ordering, for drawing
			//

			if (renderStepCount == particleStoreSortedDrawOrder.Length)
			{
				Array.Resize(ref particleStoreSortedDrawOrder, particleStoreSortedDrawOrder.Length * 2);
				for (int i = 0; i < particleStoreSortedDrawOrder.Length; i++)
					if (particleStoreSortedDrawOrder[i] == null)
						particleStoreSortedDrawOrder[i] = new List<ParticleStore>(particleStore.Length);
			}

			List<ParticleStore> sortedList = particleStoreSortedDrawOrder[renderStepCount];
			sortedList.Clear();


			//sort into dependancy order
			//if particle A emits particle B,
			//then particle B must render first,
			//because B reads the previous frame of particle A
			//because particle A may be being removed or moved during it's update

			//this is the reverse of the 'Emit Dependant' order

			foreach (ParticleStore store in particleStore)
				SortIntoEmitDependantOrder(store, sortedList);
			sortedList.Reverse();
			//sorted....

			return sortedList;
		}

		//run the per-particle processor (eg, the CPU or GPU processor)
		private void RunProcessorLogic(List<ParticleStore> sortedList)
		{
			float deltaTime = 1.0f / systemLogic.Frequency;

			//perform the update in dependant order
			for (int i = 0; i < sortedList.Count; i++)
				sortedList[i].UpdateProcessor(timeStep, globalValues, deltaTime);
		}

		//can't have particle emit a->b->a in a single frame
		private void CheckEmitDependancy(int index, int step)
		{
			for (int i = 0; i < step; i++)
			{
				if (typeDependancyList[i] == index)
				{
					//dependancy error, recursive emitting...
					string error = "Particle System recursive emit detected: ";
					for (int e = 0; e < step; e++)
					{
						error += this.particleStore[e].Name;
						error += " -> ";
					}
					error += this.particleStore[index].Name;

					throw new InvalidOperationException(error);
				}
			}

			typeDependancyList[step++] = index;

			int types = this.particleStore.Length;
			for (int i = 0; i < types; i++)
			{
				if (this.particleStore[index].GetEmitDependantOnType(i))
					CheckEmitDependancy(i, step);
			}
		}


		//called by a particle store (particle type) to emit a particle in a different particle store
		internal void Emit(ParticleSystemEmitData emit, int sourceIndex, float sourceIndexF)
		{
			particleStore[emit.typeIndex].Emit(this, emit, sourceIndex, sourceIndexF, timeStep);
		}

		internal ParticleStore GetStore(int index)
		{
			return particleStore[index];
		}

		/// <summary>
		/// Gets an array of 16 global values accessible by the entire particle system
		/// </summary>
		public float[] GlobalValues { get { return globalValues; } }

		/// <summary>
		/// Gets the number of particle types in use by this particle system
		/// </summary>
		public int ParticleTypeCount { get { if (particleStore == null) return 0; return particleStore.Length; } }
		/// <summary>
		/// Gets the number of individual particles in use by a particle type
		/// </summary>
		/// <param name="particleType"></param>
		/// <returns></returns>
		public int GetParticleCount(int particleType) { return (int)particleStore[particleType].Count; }

		//callback by the drawproc,
		//this will be called at the very start of a rendered frame (before the application)
		internal void Draw(FrameState state)
		{
			if (disposed)
				return;

			threadWaitCallback.WaitForCompletion();

			int step = this.timeStep - renderStepCount;
			float deltaTime = 1.0f / systemLogic.Frequency;

			if (step == 0)
			{
				//first time rendering, must warm the systems
				foreach (ParticleStore store in this.particleStore)
					store.ParticleProcessor.Warm(state.Application);
			}

			//an update may happen more than once per frame
			for (int i = 0; i < renderStepCount; i++)
			{
				//draw each particle type, one by one (in dependancy sorted order)
				foreach (ParticleStore store in particleStoreSortedDrawOrder[i])
					store.ParticleProcessor.BeginDrawPass(i, deltaTime, step);
				foreach (ParticleStore store in particleStoreSortedDrawOrder[i])
					store.ParticleProcessor.DrawProcess(state, step);

				step++;
			}

			renderStepCount = 0;
		}

		/// <summary>
		/// Warms the particle system (preloads any resources used)
		/// </summary>
		/// <param name="state"></param>
		public void Warm(DrawState state)
		{
			if (this.systemData == null)
				throw new InvalidOperationException("ParticleSystemData == null");
			foreach (ParticleStore store in this.particleStore)
				store.ParticleProcessor.Warm(state.Application);
		}
		/// <summary>
		/// Warms the particle system (preloads any resources used)
		/// </summary>
		public void Warm(Application application)
		{
			if (this.systemData == null)
				throw new InvalidOperationException("ParticleSystemData == null");
			foreach (ParticleStore store in this.particleStore)
				store.ParticleProcessor.Warm(application);
		}

		//call made by a particle drawer
		internal void DrawCallback(DrawState state, IParticleSystemDrawer particleDrawer)
		{
			if (systemLogic == null)
				throw new InvalidOperationException("Attempting to draw a ParticleSystem with unassigned ParticleSystemData Content");
			
			if (disposed)
				return;

			this.drawProc.MarkAsDrawn();

			//loop all the particle stores, and draw them all
			foreach (ParticleStore store in this.particleStore)
				store.ParticleProcessor.DrawCallback(state, particleDrawer, store.Count);
		}

		//particle processor logic (drawing on the gpu) must be sorted into dependancy order based on emitting
		private void SortIntoEmitDependantOrder(ParticleStore store, List<ParticleStore> sortedList)
		{
			if (sortedList.Contains(store))
				return;
			for (int i = 0; i < particleStore.Length; i++)
				if (store.GetEmitDependantOnType(i))
					SortIntoEmitDependantOrder(particleStore[i], sortedList);
			sortedList.Add(store);
		}

		/// <summary>
		/// True if this particle system has been disposed
		/// </summary>
		public bool IsDisposed
		{
			get { return disposed; }
		}

		/// <summary>
		/// Dispose this particle system, unloading resources created by it's processor and storage children
		/// </summary>
		public void Dispose()
		{
			if (disposed)
				return;


			disposed = true;
			this.threadWaitCallback.WaitForCompletion();

			this.timeStep = 0;
			this.renderStepCount = 0;

			this.systemData = null;
			this.systemLogic = null;

			for (int i = 0; i < this.particleStore.Length; i++)
			{
				this.particleStore[i].Dispose();
				this.particleStore[i] = null;
			}
			for (int i = 0; i < this.triggers.Length; i++)
			{
				this.triggers[i] = null;
			}
			for (int i = 0; i < this.toggles.Length; i++)
			{
				this.toggles[i] = null;
			}
			for (int i = 0; i < particleStoreSortedDrawOrder.Length; i++)
			{
				if (particleStoreSortedDrawOrder[i] != null)
					particleStoreSortedDrawOrder[i].Clear();
			}
			particleStoreSortedDrawOrder = null;
		}

#if !XBOX360
		private static bool forceUseCpuParticleSystem;
		/// <summary>
		/// <para>Gets/Sets if particle systems should use the CPU particle processor by default. (Windows Only)</para>
		/// </summary>
		public static bool ForceUseCpuParticleSystem
		{
			get { return forceUseCpuParticleSystem; }
			set { forceUseCpuParticleSystem = value; }
		}
#endif
		/// <summary>
		/// Returns true if this system supports GPU particle processing (This value is always true on the xbox)
		/// </summary>
		public static bool SystemSupportsGpuParticles
		{
			get
			{
				return Application.GetApplicationInstance().IsHiDefDevice;
			}
		}
	}


	/*
	 * 
	 * 
	 * Various helper and runtime classes used by the particle system class
	 * 
	 * These classes do not represent particle system content
	 * 
	 * 
	 */

	//thread callback
	class SystemThreadAction : IAction
	{
		public ParticleSystem system;

		public void PerformAction(object data)
		{
			system.UpdateParticles();
		}
	}
	//draw callback,
	//also stores booleans to record if the particle system was drawn during the frame
	class SystemDrawProc : IFrameDraw
	{
		private bool childDrawn = true;
		private bool anyChildDrawn = true;
		public ParticleSystem system;
		private int frameIndex = -1;

		public void MarkAsDrawn()
		{
			childDrawn = true;
		}
		public bool AnyChildDrawnLastFrame
		{
			get { return anyChildDrawn; }
		}

		public void Draw(FrameState state)
		{
			if (state.FrameIndex != frameIndex)
			{
				anyChildDrawn = childDrawn;
				childDrawn = false;
				frameIndex = state.FrameIndex;
			}

			if (system != null)
				system.Draw(state);
		}
	}

	//to keep from doing too many int->float casts, keep certain indices as float and int

	//note, internally, all particle timing is done with integers

	//particles are stored in a in place linked list.
	//each time step in the future stores a such a linked list of the particles that must be removed during that timestep

	//stores the number and first particle index to be removed during a timestep
	struct SystemTimeStep
	{
		public uint count;
		public uint firstParticleIndex;
	}

	//data for tracking the life of a single particle
	struct Particle
	{
		//linked list
		public uint nextParticleIndexForTimeStep;
		public uint previousParticleIndexForTimeStep;

		public ushort timeStepIndex, addActionIndex;
		public float index;
	}

	struct RemoveIndex
	{
		public uint index;
		public float indexF;
	}

	//this class manages particle adding and removing
	sealed class ParticleStore : IDisposable
	{
		//a timestep stores a integer based linked list of particle indices
		//for particles that will be removed at the given timestep.
		private readonly SystemTimeStep[] timeSteps;

		//there are enough timesteps stored to keep the longest possible life of a particle (then they loop back to zero)
		private readonly uint timeStepCount; //always a power of 2
		private readonly int timeStepMask; // timeStepCount - 1

		//the particles
		private Particle[] particles;
		private uint particleCapacity; // total count
		private uint maxParticleCapacity; // stores the max count encountered (for profiling the system)
		private float particleCapacityF; // as float

		//data that gets sent to a processor
		private AddAction[] addActions;
		//adding more than 32k particles in a frame isn't supported
		private ushort addActionCount;
		private CopyAction[] copyActions;
		private uint copyActionCount;
		private RemoveIndex[] removeIndices;

		//array of bools, one for each type in the particle system (indexed from 1, zero is 'no system')
		//used to detect recursive particle emits during a frame (and determine render order)
		//particles that don't emit from another particle are index 0. particle type 0 is index 1, etc.
		private readonly bool[] typeEmitDependancy;
		private readonly int particleTypeIndex;

		//the actual processor
		private readonly IParticleProcessor processor;

		private readonly ParticleSystemTypeData particleType;
		//duplicated for convienience
		private readonly ParticleSystemEmitterLogic frameEmitter, removeEmitter;

		//build it
		public ParticleStore(uint maxTimeSteps, ParticleSystemTypeData type, IParticleProcessor processor, int particleTypeCount, int particleTypeIndex)
		{
			if (type == null)
				throw new ArgumentNullException();
			
			this.typeEmitDependancy = new bool[particleTypeCount + 1];
			this.particleTypeIndex = particleTypeIndex;

			this.particleType = type;
			this.processor = processor;

			this.frameEmitter = type.FrameEmitter;
			this.removeEmitter = type.RemoveEmitter;

			this.timeStepCount = 1;
			while (maxTimeSteps > this.timeStepCount)
				this.timeStepCount *= 2;

			this.timeStepMask = (int)this.timeStepCount - 1;

			this.timeSteps = new SystemTimeStep[timeStepCount];
			this.particles = new Particle[Math.Max(4,type.ExpectedMaxCapacity)];

			this.addActions = new AddAction[8];
			this.copyActions = new CopyAction[8];
		}

		public IParticleProcessor ParticleProcessor
		{
			get { return processor; }
		}
		public uint MaxTimeSteps
		{
			get { return timeStepCount; }
		}
		public string Name
		{
			get { return particleType.Name; } 
		}
		public uint Count
		{
			get { return this.particleCapacity; }
		}
		public uint MaxCount
		{
			get { return this.maxParticleCapacity; }
		}
		public bool RequiresDrawPass
		{
			get { return processor != null && processor.RequiresDrawPass; }
		}
		public ParticleSystemTypeData ParticleTypeData
		{
			get { return particleType; }
		}

		public bool GetEmitDependantOnType(int typeIndex) 
		{
			return typeEmitDependancy[typeIndex + 1]; 
		}

		public void ClearTypeEmitDependance()
		{
			for (int i = 0; i < typeEmitDependancy.Length; i++)
				typeEmitDependancy[i] = false;
		}

		//the logic to emit a single particle
		public void Emit(ParticleSystem system, ParticleSystemEmitData emit, int sourceIndex, float sourceIndexF, int step)
		{
			//need more capacity?
			if (particleCapacity == particles.Length)
				Array.Resize(ref this.particles, this.particles.Length * 2);
			uint index = particleCapacity++;

			//for profiling
			maxParticleCapacity = Math.Max(maxParticleCapacity, index);

			//find the step the particle will be removed on
			int stepIndex = emit.baseLifeTimeStep;
			if (emit.varianceTimeStep != 0)
				stepIndex += system.systemRandom.Next(emit.varianceTimeStep);
			stepIndex = Math.Max(1, stepIndex);

			//lifespan in steps
			int life = stepIndex;

			//remove step
			stepIndex = (stepIndex + step) & timeStepMask;

			//create the particle
			Particle particle = new Particle();
			particle.index = particleCapacityF++;

			particle.previousParticleIndexForTimeStep = uint.MaxValue;
			particle.nextParticleIndexForTimeStep = uint.MaxValue;
			particle.addActionIndex = addActionCount;

			if (timeSteps[stepIndex].count++ != 0)
			{
				//step isn't empty?
				uint previousFirst = timeSteps[stepIndex].firstParticleIndex;

				particle.nextParticleIndexForTimeStep = previousFirst;
				particles[previousFirst].previousParticleIndexForTimeStep = index;
			}

			//start of the linked list
			timeSteps[stepIndex].firstParticleIndex = index;

			particle.timeStepIndex = (ushort)stepIndex;
			//store
			this.particles[index] = particle;

			//create the addAction
			AddAction action = new AddAction();
			action.index = index;
			action.lifeSteps = (uint)life;
			action.indexF = particle.index;

			action.cloneFromIndex = sourceIndex;
			action.cloneFromIndexF = sourceIndexF;
			action.cloneTypeIndex = emit.emitFromTypeIndex;

			//if sourceIndex is -1, then this is a root level particle,
			//so it's positin is set by the application.

			if (sourceIndex == -1)
			{
				system.GetSpawnDetails(out action.spawnDetails);
			}

			typeEmitDependancy[emit.emitFromTypeIndex + 1] = true;
		
			//cannot spawn more than 32k particles in a single frame
			if (addActionCount != (ushort)short.MaxValue)
			{
				addActions[addActionCount] = action;

				if (++addActionCount == addActions.Length)
					Array.Resize(ref addActions, addActions.Length * 2);
			}
		}

		//emit a particle from every particle
		public void EmitEach(ParticleSystem system, ParticleSystemEmitData emit, int step)
		{
			//emits one particle from every particle.
			//don't want to emit from a particle being emitted... (recursive emit)
			int cap = (int)particleCapacity;

			float f = 0;
			for (int i = 0; i < cap; i++)
				system.Emit(emit,i,f++);
		}

		//run particle logic one particle at a time
		internal void RunParticleTypeOneByOne(ParticleSystem system, int step, ref ParticleSystemActionData child)
		{
			uint cap = particleCapacity;
			float f = 0;
			for (uint i = 0; i < cap; i++)
				child.RunParticle(system, this, i,f++, step);
		}

		//run particle logic every so 'value' steps
		internal void RunParticleTypeEvery(ParticleSystem system, int step, int value, ref ParticleSystemActionData child)
		{
			//int stepBase = step + timeStepCount;
				
			if (value > 3)
			{
				//step is getting pretty big...
				//so...
				//don't loop all the particles, loop the timesteps that match
				//then loop all their particles.
				//this accesses less data but jumps around a lot.

				SystemTimeStep timestep;
				for (int t = 0; t < this.timeStepCount; t += value)
				{
					//this step is affected, so iterate all it's children.
					timestep = this.timeSteps[(step + t) & timeStepMask];
					uint index = timestep.firstParticleIndex;

					for (int i = 0; i < timestep.count; i++)
					{
						child.RunParticleChildren(system, this, index, this.particles[index].index, step);

						index = particles[index].nextParticleIndexForTimeStep;
					}
				}
			}
			else
			{
				uint cap =particleCapacity;
				float f = 0;
				for (uint i = 0; i < cap; i++)
				{
					if (((step + particles[i].timeStepIndex) % value) == 0)
						child.RunParticleChildren(system, this, i, f, step);
					f++;
				}
			}
		}


		//void ValidateSteps()
		//{
		//    for (int i = 0; i < this.timeSteps.Length; i++)
		//    {
		//        uint prev = uint.MaxValue;
		//        uint index = this.timeSteps[i].firstParticleIndex;
		//
		//        for (int p = 0; p < this.timeSteps[i].count; p++)
		//        {
		//            if (this.particles[index].previousParticleIndexForTimeStep != prev)
		//                throw new ArgumentException();
		//            prev = index;
		//            index = this.particles[index].nextParticleIndexForTimeStep;
		//        }
		//
		//        if (this.timeSteps[i].count > 0 && index != uint.MaxValue)
		//            throw new ArgumentException();
		//    }
		//}


		//the meat of the system
		public void Update(ParticleSystem system, int step)
		{
			int stepIndex = step & timeStepMask;

			SystemTimeStep timestep = timeSteps[stepIndex];
			timeSteps[stepIndex] = new SystemTimeStep();
			uint particleIndex = timestep.firstParticleIndex;



			//run per frame logic for the type
			if (frameEmitter != null)
				frameEmitter.RunParticleType(system, this, step);


			//iterate twice.
			//first pass, generate the operations to remove the particles
			//in the second pass, run the remove emitter, if there is one.
			//this way, a particle created in the removed emitter will not be rearranged
			//by another removed particle later

			//this is mostly for the benfit of the GPU system
			//
			uint copyCount = copyActionCount;

			if (timestep.count > 0)
			{
				Particle particle, replace;

				//first, collect the indices for removal, if they exist
				if (removeEmitter != null)
				{
					uint removeIndex = particleIndex;

					int count = 2;
					while (timestep.count > count)
						count *= 2;
					if (removeIndices == null || count > removeIndices.Length)
						Array.Resize(ref removeIndices, count);
					RemoveIndex indexEntry;

					for (int i = 0; i < timestep.count; i++)
					{
						particle = this.particles[removeIndex];

						indexEntry.index = removeIndex;
						indexEntry.indexF = particle.index;
						removeIndices[i] = indexEntry;

						removeIndex = particle.nextParticleIndexForTimeStep;
					}
				}


				//now remove them
				for (int i = 0; i < timestep.count; i++)
				{
					particle = this.particles[particleIndex];


					--particleCapacityF;
					uint last = --particleCapacity;


					//move last particle into this one's position
					replace = this.particles[last];
					
					//unless it's being removed anyway
					if (particleIndex != last)
					{
						//update the linked list indexing of the particles...
						if (replace.previousParticleIndexForTimeStep != uint.MaxValue)
							this.particles[replace.previousParticleIndexForTimeStep].nextParticleIndexForTimeStep = particleIndex;
						else
						{
							if (replace.timeStepIndex != stepIndex)
								this.timeSteps[replace.timeStepIndex].firstParticleIndex = particleIndex;
						}

						//particle has been moved once already in this update? (rare)
						if (replace.nextParticleIndexForTimeStep != uint.MaxValue)
							this.particles[replace.nextParticleIndexForTimeStep].previousParticleIndexForTimeStep = particleIndex;

						//may be moving the next particle in the list...
						if (particle.nextParticleIndexForTimeStep == last)
							particle.nextParticleIndexForTimeStep = particleIndex;

						//store the copy action...
						//but first...
						if (replace.addActionIndex < addActionCount &&
							this.addActions[replace.addActionIndex].index == last)
						{
							//this particle was just added in this frame, now it's being moved
							//so modify the add action instead to directly write to the position
							this.addActions[replace.addActionIndex].index = particleIndex;
							this.addActions[replace.addActionIndex].indexF = particle.index;
						}
						else
						{

							if (replace.addActionIndex >= short.MaxValue) // add action indices above 32k are for copy indices
							{
								//the particle is already being copied...
								//so modify the existing copy op
								copyActions[replace.addActionIndex - short.MaxValue].indexTo = particleIndex;
								copyActions[replace.addActionIndex - short.MaxValue].indexToF = particle.index;
							}
							else
							{

								//otherwise do a copy
								CopyAction copy = new CopyAction();

								copy.indexTo = particleIndex;
								copy.indexToF = particle.index;

								copy.indexFrom = last;
								copy.indexFromF = replace.index;

								replace.addActionIndex = (ushort)(short.MaxValue + copyActionCount);

								copyActions[copyActionCount] = copy;
								if (++copyActionCount == copyActions.Length)
									Array.Resize(ref copyActions, copyActions.Length * 2);
							}
						}


						replace.index = particle.index;
						this.particles[particleIndex] = replace;
					}

					particleIndex = particle.nextParticleIndexForTimeStep;
				}

				//now finally run the remove emitters.
				if (removeEmitter != null)
				{
					RemoveIndex index;

					for (int i = 0; i < timestep.count; i++)
					{
						index = removeIndices[i];

						if (removeEmitter != null)
							removeEmitter.RunParticle(system, this, index.index, index.indexF, step);
					}
				}
			}

			for (uint i = copyCount; i < copyActionCount; i++)
			{
				this.particles[copyActions[i].indexTo].addActionIndex = 0;
			}
		}

		//update the processor
		public void UpdateProcessor(int step, float[] globals, float deltaTime)
		{
			if (processor != null)
				processor.Update((int)this.particleCapacity, this.particleCapacityF, deltaTime, globals, this.copyActions, (int)this.copyActionCount, this.addActions, (int)this.addActionCount, step);

			this.addActionCount = 0;
			this.copyActionCount = 0;
		}

		//boom.
		public void Dispose()
		{
			processor.Dispose();

			this.particles = null;
			this.addActions = null;
			this.copyActions = null;
			this.removeIndices = null;
		}
	}
}
