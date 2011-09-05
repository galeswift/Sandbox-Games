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


namespace Tutorials.Tutorial_26
{

	//
	// Some notes about the Xen thread pool:
	// The Xen ThreadPool will create a task thread for every
	// free hardware thread that is available.
	//
	// On a dual core PC, this means 1 task thread is created.
	// On a quad core PC, this means 3 task threads are created.
	//
	// On the xbox, 3 task threads are created.
	// (XNA gives access to 4 of the 6 threads on the xbox)
	//
	// NOTE: On a single core PC no task threads are created.
	// Any queued task will be performed on the calling thread.
	//
	// (However, Background tasks will have a thread created)
	//

	//This is the class that performs the threaded loading.
	//The only difference between this class and a normal ContentRegister
	//is that BeginLoad() must be called to start the content loading.
	public class ThreadedContentRegister : Xen.Threading.IAction, IContentRegister
	{
		//store the content register, and a list of items to load.
		private ContentRegister content;
		private readonly List<IContentOwner> contentToLoad;

		//store a reference to the application.
		private readonly Application application;

		//store the loading statistics
		private float loadPercentage;
		private bool loadComplete, loadingInProgress;


		public ThreadedContentRegister(Application application)
		{
			//store a reference to the application for later..
			this.application = application;
			this.contentToLoad = new List<IContentOwner>();
		}

		//use this property to check if the load is done.
		public bool LoadingComplete
		{
			get { lock (this) { return loadComplete; } }
		}
		public float LoadingPercentage
		{
			get { lock (this) { return loadPercentage; } }
		}

		//Use this method to add an item that needs it's content loaded.
		public void Add(IContentOwner owner)
		{
			if (loadingInProgress)
				throw new InvalidOperationException("Loading has already started...");

			//store it in the list
			this.contentToLoad.Add(owner);
		}

		void IContentRegister.Remove(IContentOwner owner)
		{
			throw new NotSupportedException();
		}

		//this must be called to begin the loading process
		public void BeginLoad()
		{
			if (loadingInProgress)
				throw new InvalidOperationException("Loading has already started...");
			loadingInProgress = true;

			//queue or create a background thread task to load the content
			
			//ThreadPool.QueueTask() could be used here, however, on a
			//single core PC, the task would be run and QueueTask would
			//block until it completes. (As no tasks threads are available)

			//Using 'RunBackgroundTask' forces a thread to be created in 
			//the case the machine is single core.
			//This should only be used for *large* complex tasks that
			//take several frames to complete. 
			//All other tasks should *always* use ThreadPool.QueueTask.

			this.application.ThreadPool.RunBackgroundTask(this, null);

			//For most other cases where Threading is desired, using
			//ThreadPool.QueueTask() is recommended. 
			//For example, internally, async model animation and particle 
			//processing uses ThreadPool.QueueTask.

			//QueueTask() and RunBackgroundTask() will return a
			//WaitCallback structure. This structure stores a handle
			//to the task in process. It can be used to wait for the task
			//to complete, or check if it has completed.

			//The ThreadPool will call the method below: (on another thread)
		}

		//this method is called by a Background Thread.
		void Xen.Threading.IAction.PerformAction(object data)
		{
			//here is where the loading is initalised.
			//this is on another thread

			//create the ContentRegister
			//This should usually be done on the thread
			//where the register will be used

			this.content = new ContentRegister(this.application);


			float invItemCount100 = 0, index = 0;

			if (this.contentToLoad.Count > 0)
				invItemCount100 = 100F / this.contentToLoad.Count;

			//now, loop the content list, and load each item
			for (int i = 0; i < this.contentToLoad.Count; i++)
			{
				this.content.Add(this.contentToLoad[i]);

				//update the progress
				index += 1;
				lock (this)
				{
					this.loadPercentage = index * invItemCount100;
				}
			}

			//done!
			lock (this)
			{
				this.loadComplete = true;
			}
		}

		//call this when you are done with the level, and want to clean it's content up.
		//just make sure the content isn't in use anymore when you call this!
		public void Dispose()
		{
			//dispose the content register. (Otherwise the content will stay in memory)

			if (this.content != null)
				this.content.Dispose();

			this.content = null;
		}
	}
}
