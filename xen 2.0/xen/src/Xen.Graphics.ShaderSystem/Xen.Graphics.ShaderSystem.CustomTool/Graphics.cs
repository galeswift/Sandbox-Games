using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace Xen.Graphics.ShaderSystem.CustomTool
{
	//storage for a static graphics device, used for compiling shaders
	public static class Graphics
	{
		private static GraphicsDevice device;
		private static Form hiddenWindow;
		private readonly static List<Texture> textureCache;
		private readonly static Dictionary<Type, SurfaceFormat> textureFormatDict;

		static Graphics()
		{
			textureCache = new List<Texture>();
			textureFormatDict = new Dictionary<Type, SurfaceFormat>();
			
			hiddenWindow = new Form();
			PresentationParameters present = new PresentationParameters();
			present.IsFullScreen = false;
			present.DeviceWindowHandle = hiddenWindow.Handle;

			try
			{
				device = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, present);
			}
			catch
			{
				throw new CompileException("The xen shader compiler requires an XNA HiDef capable graphics device");
			}
		}

		public static GraphicsDevice GraphicsDevice
		{
			get
			{
				return device;
			}
		}

		public static void EndGetTempTexture(Texture texture)
		{
			textureCache.Add(texture);
		}
		public static Texture BeginGetTempTexture(Type type)
		{
			if (type == typeof(Texture))
				type = typeof(Texture2D);

			for (int i = 0; i < textureCache.Count; i++)
			{
				if (textureCache[i].GetType() == type)
				{
					Texture t = textureCache[i];
					textureCache.RemoveAt(i);
					return t;
				}
			}

			//create.
			SurfaceFormat format = SurfaceFormat.HalfSingle;

			Texture texture = null;

			if (type == typeof(Texture2D))
				texture = new Texture2D(GraphicsDevice, 2, 2, false, format);
			if (type == typeof(Texture3D))
				texture = new Texture3D(GraphicsDevice, 2, 2, 2, false, format);
			if (type == typeof(TextureCube))
				texture = new TextureCube(GraphicsDevice, 2, false, format);

			return texture;
		}

	}
}
