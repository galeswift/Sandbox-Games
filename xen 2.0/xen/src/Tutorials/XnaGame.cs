using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Xen;

namespace Tutorials
{
	/// <summary>
	/// This is an example of an XNA game that creates a Xen application host
	/// </summary>
	public class XnaGame : Microsoft.Xna.Framework.Game
	{
		private readonly GraphicsDeviceManager graphics;

		public XnaGame(Application application)
		{
			//create the graphics device manager
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;

			Content.RootDirectory = "Content";

			//create the xen host
			GameComponent component = new Xen.GameComponentHost(this, application, graphics);

			//add it to the xna component list
			this.Components.Add(component);
		}
	}
}
