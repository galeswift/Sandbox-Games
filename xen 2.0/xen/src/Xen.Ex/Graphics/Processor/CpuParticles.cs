using System;
using System.Collections.Generic;
using System.Text;
using Xen.Graphics;
using Xen.Camera;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Xen.Ex.Graphics.Display;
using Xen.Ex.Graphics;
using Xen.Ex.Graphics2D;
using System.CodeDom;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Xen.Ex.Graphics.Content;

namespace Xen.Ex.Graphics.Processor
{
	/// <summary>
	/// <para>Stores data for the CPU particle processor.</para>
	/// <para>This data is stored as a dynamically loaded .net assembly.</para>
	/// </summary>
	public sealed class CpuParticleProcessorData
	{
		const string namespaceName = "Xen.Ex.Graphics.Content.Runtime";
		
#if DEBUG
		private byte[] runtimeAssemblyData;
#endif
		private readonly string runtimeClassName;
		private readonly List<CodeMemberMethod> methods;
		private readonly System.Reflection.Assembly assembly;

		/// <summary>
		/// Gets the assembly
		/// </summary>
		public System.Reflection.Assembly Assembly { get { return assembly; } }
		/// <summary>
		/// Gets the name of the class in the assembly that stores the particle logic
		/// </summary>
		public string RuntimeClassName { get { return runtimeClassName; } }

		internal CpuParticleProcessorData()
		{
			this.methods = new List<CodeMemberMethod>();

			runtimeClassName = "xen" + Guid.NewGuid().ToString("N");
		}

		internal CpuParticleProcessorData(ContentReader reader)
		{
			byte[] runtimeAssemblyData = reader.ReadBytes(reader.ReadInt32());
			this.runtimeClassName = namespaceName + "." + reader.ReadString();
			this.assembly = System.Reflection.Assembly.Load(runtimeAssemblyData);
		}

#if DEBUG

		internal void Write(BinaryWriter writer)
		{
			writer.Write(runtimeAssemblyData.Length);
			writer.Write(runtimeAssemblyData);
			writer.Write(runtimeClassName);
		}


		//all code for the particle types are stored in the same assmebly
		internal void AddParticleType(ParticleSystemTypeData typeData)
		{
			methods.Add(CpuParticleLogicBuilder.BuildCpuLogic(typeData.Name+"_frame",typeData.ParticleLogicData.Frame,false));
			methods.Add(CpuParticleLogicBuilder.BuildCpuLogic(typeData.Name+"_once",typeData.ParticleLogicData.Once,true));
		}

		internal void BuildAssembly()
		{
			this.runtimeAssemblyData = CpuParticleLogicBuilder.GenerateAssembly(namespaceName, runtimeClassName, methods.ToArray());
		}

#endif
	}

	/// <summary>
	/// Particle processor that runs particle logic on the CPU
	/// </summary>
	public sealed class CpuParticleProcessor : IParticleProcessor
	{
		//signatures for update methods

		//signature for default frame update:
		//uint count, int step, Random rand, float delta_time, float[] global, Vector2[] life, Vector4[] ps, Vector4[] vr, Vector4[] col, Vector4[] user
		delegate void UpdateParticleDelegate(uint count, float time, Random rand, float delta_time, float[] global, Vector2[] life, Vector4[] ps, Vector4[] vr, Vector4[] col, Vector4[] user);
		delegate void AddParticleDelegate(uint count, uint[] indices, float time, Random rand, float delta_time, float[] global, Vector2[] life, Vector4[] ps, Vector4[] vr, Vector4[] col, Vector4[] user);

		private UpdateParticleDelegate frameMethod;
		private AddParticleDelegate onceMethod;
		private readonly Random random = new Random();
		private uint previousFrameCount;

		private Vector4[] positions, velocity, colours, userdata;
		private Vector2[] lifeData;
		private uint countMask, maxCount, timeStepHz;
		private IParticleProcessor[] processors;
		private ParticleSystemTypeData particleTypeData;

		private uint[] addIndices;

		void IParticleProcessor.Initalise(ParticleSystemTypeData typeData, IParticleProcessor[] allProcessors, bool useColourValues, uint maxLifeTimeSteps, uint timeStepHz, uint maxExpectedCount)
		{
			CpuParticleProcessorData processor = typeData.RuntimeLogicData.CpuParticleProcessorData;

			this.particleTypeData = typeData;
			this.timeStepHz = timeStepHz;
			this.processors = allProcessors;
			this.addIndices = new uint[8];

			//max expected count is always a power of 2
			this.maxCount = maxExpectedCount;
			this.countMask = maxExpectedCount - 1;

			bool usesUserValues = typeData.RuntimeLogicData.SystemUsesUserValues;
			bool usesLifeOrAge = typeData.RuntimeLogicData.SystemUsesLifeOrAgeValues;

			positions = new Vector4[maxExpectedCount];
			velocity = new Vector4[maxExpectedCount];

			if (useColourValues)
				colours = new Vector4[maxExpectedCount];

			if (usesUserValues)
				userdata = new Vector4[maxExpectedCount];

			if (usesLifeOrAge)
				lifeData = new Vector2[maxExpectedCount];

			System.Reflection.Assembly asm = processor.Assembly;

			Type type = asm.GetType(processor.RuntimeClassName);

			System.Reflection.MethodInfo frameMethod = type.GetMethod(typeData.Name + "_frame", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
			System.Reflection.MethodInfo onceMethod = type.GetMethod(typeData.Name + "_once", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

			this.frameMethod = (UpdateParticleDelegate)Delegate.CreateDelegate(typeof(UpdateParticleDelegate), frameMethod);
			this.onceMethod = (AddParticleDelegate)Delegate.CreateDelegate(typeof(AddParticleDelegate), onceMethod);
		}

		void IParticleProcessor.Warm(Application application)
		{
		}

		void IParticleProcessor.Update(int particleCount, float particleCountF, float deltaTime, float[] globals, CopyAction[] copyActions, int copyActionCount, AddAction[] addActions, int addActionCount, int step)
		{
			float time = (float)(step/timeStepHz) + (float)(step%timeStepHz) * deltaTime;

			//first update all particles from the last frame...
			if (previousFrameCount > 0)
				frameMethod(previousFrameCount, time, random, deltaTime, globals, this.lifeData, this.positions, this.velocity, this.colours, this.userdata);

			//then, apply the move operations
			for (int i = 0; i < copyActionCount; i++)
			{
				CopyAction action = copyActions[i];

				action.indexFrom = action.indexFrom & countMask;
				action.indexTo = action.indexTo & countMask;

				positions[action.indexTo] = positions[action.indexFrom];
				velocity[action.indexTo] = velocity[action.indexFrom];

				if (userdata != null)	userdata[action.indexTo] = userdata[action.indexFrom];
				if (colours != null)	colours[action.indexTo] = colours[action.indexFrom];
				if (lifeData != null)	lifeData[action.indexTo] = lifeData[action.indexFrom];
			}

			if (addActionCount > 0)
			{
				//make sure the indices array is big enough
				if (addActionCount > addIndices.Length)
				{
					int count = addIndices.Length;
					while (addActionCount > count)
						count *= 2;
					Array.Resize(ref addIndices, count);
				}

				//finally, add the new particles
				for (int i = 0; i < addActionCount; i++)
				{
					AddAction action = addActions[i];

					// simply wrap if outside the expected range
					// this will produce similar glitches as the gpu processor
					action.index = action.index & countMask; 

					if (action.cloneTypeIndex == -1)
					{
						//set all particle values to their defaults
						positions[action.index] = action.spawnDetails.PositionSize;
						velocity[action.index] = action.spawnDetails.VelocityRotation;
						if (userdata != null)
							userdata[action.index] = action.spawnDetails.UserValues;
						if (colours != null)
							colours[action.index] = action.spawnDetails.Colour;
					}
					else
					{
						//copy in particle values from another particle system
						CpuParticleProcessor other = (CpuParticleProcessor)processors[action.cloneTypeIndex];
						uint fromIndex = (uint)action.cloneFromIndex & other.countMask;

						positions[action.index] = other.positions[fromIndex];
						velocity[action.index] = other.velocity[fromIndex];

						if (userdata != null)
						{
							if (other.userdata != null)
								userdata[action.index] = other.userdata[fromIndex];
							else
								userdata[action.index] = new Vector4();
						}
						if (colours != null)
						{
							if (other.colours != null)
								colours[action.index] = other.colours[fromIndex];
							else
								colours[action.index] = new Vector4();
						}
					}

					if (lifeData != null)
						this.lifeData[action.index] = new Vector2((float)action.lifeSteps * deltaTime, time);

					addIndices[i] = action.index;
				}

				//run the add logic
				onceMethod((uint)addActionCount, addIndices, time, random, deltaTime, globals, lifeData, positions, velocity, colours, userdata);
			}

			previousFrameCount = Math.Min((uint)particleCount, maxCount); // cannot process more 
		}

		void IParticleProcessor.DrawProcess(FrameState state, int step)
		{
		}

		void IParticleProcessor.BeginDrawPass(int pass, float deltaTime, int step)
		{
		}

		bool IParticleProcessor.RequiresDrawPass
		{
			get { return false; }
		}

		void IParticleProcessor.DrawCallback(DrawState state, IParticleSystemDrawer particleDrawer, uint particleCount)
		{
			if (particleCount > 0)
				particleDrawer.DrawCpuParticles(state, this.particleTypeData, particleCount, this.positions, this.velocity, this.colours, this.userdata);
		}

		void IDisposable.Dispose()
		{
		}
	}
}
