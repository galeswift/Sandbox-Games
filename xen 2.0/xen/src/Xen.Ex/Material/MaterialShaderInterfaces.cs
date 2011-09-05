using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Xen.Graphics;
using Microsoft.Xna.Framework;

namespace Xen.Ex.Material
{
	internal interface IMS_Base : Xen.Graphics.IShader
	{
		Texture2D CustomTexture { set; }
		TextureSamplerState CustomTextureSampler { set; }
		Texture2D CustomEmissiveTexture { set; }
		TextureSamplerState CustomEmissiveTextureSampler { set; }

		void SetP_fogColourAndGamma(ref Vector4 colour);
		void SetP_EmissiveColour(ref Vector4 colour);
		void SetV_fogAndAlpha(ref Vector3 colour);
		void SetV_SH(ref Matrix sh);
	}
	internal interface IMS_PerPixel : IMS_Base
	{
		Texture2D CustomNormalMap { set; }
		TextureSamplerState CustomNormalMapSampler { set; }

		void SetP_lights(Microsoft.Xna.Framework.Vector4[] value, uint readIndex, uint writeIndex, uint count);
	}
	internal interface IMS_PerVertex : IMS_Base
	{
		void SetV_lights(Microsoft.Xna.Framework.Vector4[] value, uint readIndex, uint writeIndex, uint count);
	}

	//tagging interfaces
	internal interface IMS_VertexColour
	{
	}
}
