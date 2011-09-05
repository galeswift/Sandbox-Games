using System;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Xen.Ex.Graphics.Content
{

#if DEBUG && !XBOX360

	/// <summary>
	/// <para>This class provides Hotloading support to particle systems</para>
	/// <para>Hotloading will automatically reload the particle system if the source file changes (at runtime)</para>
	/// <para>This allows runtime editing/tweaking of particle system effects</para>
	/// <para>Call <see cref="Monitor"/> to begin monitoring the system</para>
	/// </summary>
	public sealed class ParticleSystemHotLoader : IUpdate
	{
		private readonly WeakReference system;
		private readonly string filename;
		private readonly string pathToShaderSystem;

		/// <summary>
		/// Begin monitoring the particle system for hotloading
		/// <para>Note: The path to the xen shader system .dlls must be specified. 
		/// The shader compiler has to be loaded dynamically for hotloading, and only works with a full installation of XNA GameStudio</para>
		/// </summary>
		/// <param name="updateManager"></param>
		/// <param name="system"></param>
		/// <param name="pathToShaderSystem">File path to the compiled xen shader system .dlls</param>
		/// <returns></returns>
		public static bool Monitor(UpdateManager updateManager, ParticleSystem system, string pathToShaderSystem)
		{
			new ParticleSystemHotLoader(updateManager, system, pathToShaderSystem);
			return true;
		}

		private ParticleSystemHotLoader(UpdateManager updateManager, ParticleSystem system, string pathToShaderSystem)
		{
			if (system == null || updateManager == null)
				throw new ArgumentNullException();

			if (system.ParticleSystemData == null)
				throw new ArgumentNullException("ParticleSystem.ParticleSystemData");
			
			this.system = new WeakReference(system);
			this.filename = system.ParticleSystemData.FileName;
			this.pathToShaderSystem = pathToShaderSystem;

			this.sourceFileModifiedDate = File.GetLastWriteTime(this.filename);

			updateManager.Add(this);
		}

		private DateTime sourceFileModifiedDate;
		private ParticleSystemData loadedSystemData;

		class LocalContentLoader : IContentOwner
		{
			private ParticleSystemData system;
			public LocalContentLoader(ParticleSystemData system)
			{
				this.system = system;
			}

			public void LoadContent(ContentState state)
			{
				system.UpdateTextures(state.ContentRegister, null);
				state.ContentRegister.Remove(this);
			}
		}

		private void CheckChanged(Application application)
		{

			//has the file changed?
			DateTime modifiedTime = File.GetLastWriteTime(this.filename);

			ParticleSystem system = this.system.Target as ParticleSystem;

			if (system == null)
			{
				//particle system is no longer
				return;
			}

			if (sourceFileModifiedDate < modifiedTime)
			{
				sourceFileModifiedDate = modifiedTime;
				//particle system source has changed...

				//attempt to reload

				ParticleSystemData previousData = loadedSystemData;


				try
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(this.filename);
					XmlElement root = doc.SelectSingleNode("particlesystem") as XmlElement;
					if (root == null)
						throw new ArgumentException("Missing XML root element \'particlesystem\'");

					loadedSystemData = new ParticleSystemData(this.filename, root, ContentTargetPlatform.Windows, false, null, pathToShaderSystem);

					LocalContentLoader content = new LocalContentLoader(loadedSystemData);
					application.Content.Add(content);

					system.SetParticleSystemHotloadedData(loadedSystemData);

					if (previousData != null)
					{
						//previous hot load has existing data still hanging around
						foreach (ParticleSystemTypeData typeData in previousData.ParticleTypeData)
						{
							//this is nasty
							//eeek! this texture was loaded with FromFile()....
							if (typeData.Texture != null)
								typeData.Texture.Dispose();
						}
					}
				}
				catch (Exception ex)
				{
					loadedSystemData = null;

					try
					{
						//show a message
						//the difficult way...
						string assembly = @"System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

						Type type = System.Reflection.Assembly.Load(assembly).GetType("System.Windows.Forms.MessageBox");

						System.Reflection.MethodInfo method = type.GetMethod("Show", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, new Type[] { typeof(string), typeof(string) }, null);

						method.Invoke(null, new string[] { ex.ToString(), "Error importing updated particle system" });
					}
					catch
					{
						throw ex;
					}
				}
			}
		}

		#region IUpdate Members

		UpdateFrequency IUpdate.Update(UpdateState state)
		{
			if (system.IsAlive == false)
				return UpdateFrequency.Terminate;
			
			CheckChanged(state.Application);

			return UpdateFrequency.PartialUpdate1hz;
		}

		#endregion
	}

#else
	
	/// <summary>
	/// ParticleSystemHotLoader is an empty shell class in Release and Xbox builds
	/// </summary>
	public sealed class ParticleSystemHotLoader
	{
		/// <summary>
		/// ParticleSystemHotLoader is an empty shell class in Release and Xbox builds
		/// </summary>
		public static bool Monitor(UpdateManager updateManager, ParticleSystem system, string pathToShaderSystem)
		{
			return false;
		}

		private ParticleSystemHotLoader()
		{
		}
	}
#endif
}
