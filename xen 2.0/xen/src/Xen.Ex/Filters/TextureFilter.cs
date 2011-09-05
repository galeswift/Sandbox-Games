using System;
using System.Collections.Generic;
using System.Text;
using Xen.Graphics;
using Xen.Ex.Graphics2D;
using Microsoft.Xna.Framework;
using Xen.Graphics.Modifier;

namespace Xen.Ex.Filters
{
	/// <summary>
	/// An interface that must be implemented by custom downsample shaders
	/// </summary>
	public interface IDownsampleShader : IShader
	{
		/// <summary>
		/// Gets / Sets the texture to be downsampled
		/// </summary>
		Microsoft.Xna.Framework.Graphics.Texture2D Texture { get; set; }
		/// <summary>
		/// Sets the direction downsampling occurs in
		/// </summary>
		/// <param name="direction"></param>
		void SetSampleDirection(ref Vector2 direction);
	}

	/// <summary>
	/// Interface to a class that provides downsampling shaders
	/// </summary>
	public interface IDownsampleShaderProvider
	{
		/// <summary>
		/// Two sample shader
		/// </summary>
		IDownsampleShader DownsampleShader2 { get; }
		/// <summary>
		/// Four sample shader
		/// </summary>
		IDownsampleShader DownsampleShader4 { get; }
		/// <summary>
		/// Eight sample shader
		/// </summary>
		IDownsampleShader DownsampleShader8 { get; }
	}

	/// <summary>
	/// The number of samples in a blur filter
	/// </summary>
	public enum BlurFilterFormat
	{
		/// <summary>
		/// 3x3 sample blur filter
		/// </summary>
		ThreeSampleBlur = 2,
		/// <summary>
		/// 5x5 sample blur
		/// </summary>
		FiveSampleBlur = 4,
		/// <summary>
		/// 7x7 sample blur filter
		/// </summary>
		SevenSampleBlur = 6,
		/// <summary>
		/// 15x15 sample blur filter
		/// </summary>
		FifteenSampleBlur = 8,
		/// <summary>
		/// 31x31 sample blur, assumes the graphics device supports texture filtering for the render target format
		/// </summary>
		ThirtyOneSampleBlur_FilteredTextureFormat = 9
	}

	static class FilterGenerator
	{
		private static float[] weights  = new float[16], offset  = new float[16];
		private static Vector2[] offsetsV = new Vector2[16];

		//generates weight and offset value for bell curve filters
		private static int GenerateFilter(BlurFilterFormat format, bool filtered, float bellExponent)
		{
			int samples = 0;
			switch (format)
			{
				case BlurFilterFormat.ThreeSampleBlur:
					samples = 3;
					break;
				case BlurFilterFormat.FiveSampleBlur:
					samples = 5;
					break;
				case BlurFilterFormat.SevenSampleBlur:
					samples = 7;
					break;
				case BlurFilterFormat.FifteenSampleBlur:
					samples = 15;
					break;
				case BlurFilterFormat.ThirtyOneSampleBlur_FilteredTextureFormat:
					if (!filtered)
						throw new ArgumentException();
					samples = 31;
					break;
			}

			int actualSamples = samples;
			if (filtered)
				actualSamples = samples / 2 + 1;

			if (!filtered)
			{
				//centre sample samples the 0 offset pixel
				int centre = actualSamples / 2;
				weights[centre] = 1;
				offset[centre] = 0;

				double total = 1;

				for (int i = 1; i <= centre; i++)
				{
					//bell curve
					//~ exp(- x^2 )
					double x = (i / (double)(centre + 1));

					double bell = Math.Exp(-(x * x) * 3 * bellExponent);
					total += bell * 2;

					offset[centre - i] = -i;
					offset[centre + i] = i;
					weights[centre - i] = (float)bell;
					weights[centre + i] = (float)bell;
				}

				float inv = 1.0f / (float)total;
				for (int i = 0; i < actualSamples; i++)
					weights[i] *= inv;
			}
			else
			{
				bool isOdd = (actualSamples & 1) == 1;

				//if it's odd, there is a centre sample

				int count = actualSamples/2;

				double total = 0;

				if (isOdd)
				{
					weights[count] = 2;
					offset[count] = 0;
					total = 2;
				}


				for (int i = 0; i < count; i++)
				{
					//bell curve
					//~ exp(- x^2 )
					//each is made up of two combined samples (filter)
					double x1,x2;

					if (isOdd)
					{
						x1 = ((i * 2 + 1) / (double)(actualSamples));
						x2 = ((i * 2 + 2) / (double)(actualSamples));
					}
					else
					{
						x1 = ((i * 2) / (double)(actualSamples));
						x2 = ((i * 2 + 1) / (double)(actualSamples));
					}

					double bell1 = Math.Exp(-(x1 * x1) * 3 * bellExponent);
					double bell2 = Math.Exp(-(x2 * x2) * 3 * bellExponent);

					if (!isOdd && i == 0)
						bell1 *= 0.5; //two samples on the centre

					double bias;
					if (isOdd)
						bias = ((i*2 + 1) * bell1 + (i*2 + 2) * bell2) / (bell1 + bell2);
					else
						bias = (i*2 * bell1 + (i*2 + 1) * bell2) / (bell1 + bell2);
					double weight = bell1 + bell2;

					total += weight * 2;

					int p = isOdd ? 1 : 0;

					offset[count - i - 1] = -(float)bias;
					offset[count + i + p] = (float)bias;
					weights[count - i - 1] = (float)weight;
					weights[count + i + p] = (float)weight;

				}

				float inv = 1.0f / (float)total;
				for (int i = 0; i < actualSamples; i++)
					weights[i] *= inv;
			}
			return actualSamples;
		}


		public static void SetFilter(BlurFilterFormat format, bool xAxis, SinglePassTextureFilter target, bool supportsFiltering, float bellExponent)
		{
			lock (weights)
			{
				int kernel = GenerateFilter(format, supportsFiltering, bellExponent);

				Vector2 axis = new Vector2(xAxis ? 1 : 0, xAxis ? 0 : 1);

				for (int i = 0; i < 16; i++)
					offsetsV[i] = axis * offset[i];

				target.SetFilter(offsetsV, weights, kernel);
			}
		}
	}

	/// <summary>
	/// Stores an 16 sample filter
	/// </summary>
	public struct Filter16Sample
	{
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset0;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset1;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset2;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset3;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset4;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset5;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset6;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset7;

		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset8;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset9;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset10;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset11;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset12;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset13;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset14;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset15;

		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight0;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight1;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight2;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight3;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight4;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight5;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight6;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight7;

		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight8;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight9;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight10;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight11;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight12;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight13;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight14;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight15;
	}

	/// <summary>
	/// Stores an 8 sample filter
	/// </summary>
	public struct Filter8Sample
	{
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset0;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset1;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset2;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset3;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset4;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset5;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset6;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset7;

		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight0;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight1;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight2;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight3;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight4;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight5;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight6;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight7;
	}

	/// <summary>
	/// Stores an 4 sample filter
	/// </summary>
	public struct Filter4Sample
	{
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset0;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset1;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset2;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset3;

		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight0;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight1;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight2;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight3;
	}

	/// <summary>
	/// Stores an 2 sample filter
	/// </summary>
	public struct Filter2Sample
	{
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset0;
		/// <summary>Pixel offset from the sampling centre point</summary>
		public Vector2 PixelOffset1;

		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight0;
		/// <summary>Pixel weighting for the sample (all weights should usually add to 1)</summary>
		public float Weight1;
	}

	/// <summary>
	/// Applies a single pass texture filter (up to 16 sample) to a draw target
	/// </summary>
	public sealed class SinglePassTextureFilter : IFrameDraw
	{
		private readonly DrawTargetTexture2D source, targetClone;
		private readonly ShaderElement element;
		private readonly Vector3[] filter = new Vector3[16];
		private int kernelSize;


		/// <summary>
		/// Creates a single pass 16 sample filter
		/// </summary>
		/// <param name="source">soure texture to filter</param>
		/// <param name="target">target to filter</param>
		/// <param name="filter">Filter to apply</param>
		public SinglePassTextureFilter(DrawTargetTexture2D source, DrawTargetTexture2D target, ref Filter16Sample filter)
			: this(source,target)
		{
			SetFilter(ref filter);
		}
		/// <summary>
		/// Creates a single pass 16 sample filter
		/// </summary>
		/// <param name="source">soure texture to filter</param>
		/// <param name="target">target to filter</param>
		/// <param name="filter">Filter to apply</param>
		public SinglePassTextureFilter(DrawTargetTexture2D source, DrawTargetTexture2D target, Filter16Sample filter)
			: this(source, target)
		{
			SetFilter(ref filter);
		}


		/// <summary>
		/// Creates a single pass 8 sample filter
		/// </summary>
		/// <param name="source">soure texture to filter</param>
		/// <param name="target">target to filter</param>
		/// <param name="filter">Filter to apply</param>
		public SinglePassTextureFilter(DrawTargetTexture2D source, DrawTargetTexture2D target, ref Filter8Sample filter)
			: this(source, target)
		{
			SetFilter(ref filter);
		}
		/// <summary>
		/// Creates a single pass 8 sample filter
		/// </summary>
		/// <param name="source">soure texture to filter</param>
		/// <param name="target">target to filter</param>
		/// <param name="filter">Filter to apply</param>
		public SinglePassTextureFilter(DrawTargetTexture2D source, DrawTargetTexture2D target, Filter8Sample filter)
			: this(source, target)
		{
			SetFilter(ref filter);
		}


		/// <summary>
		/// Creates a single pass 4 sample filter
		/// </summary>
		/// <param name="source">soure texture to filter</param>
		/// <param name="target">target to filter</param>
		/// <param name="filter">Filter to apply</param>
		public SinglePassTextureFilter(DrawTargetTexture2D source, DrawTargetTexture2D target, ref Filter4Sample filter)
			: this(source, target)
		{
			SetFilter(ref filter);
		}
		/// <summary>
		/// Creates a single pass 4 sample filter
		/// </summary>
		/// <param name="source">soure texture to filter</param>
		/// <param name="target">target to filter</param>
		/// <param name="filter">Filter to apply</param>
		public SinglePassTextureFilter(DrawTargetTexture2D source, DrawTargetTexture2D target, Filter4Sample filter)
			: this(source, target)
		{
			SetFilter(ref filter);
		}

		/// <summary>
		/// Creates a single pass 2 sample filter
		/// </summary>
		/// <param name="source">soure texture to filter</param>
		/// <param name="target">target to filter</param>
		/// <param name="filter">Filter to apply</param>
		public SinglePassTextureFilter(DrawTargetTexture2D source, DrawTargetTexture2D target, ref Filter2Sample filter)
			: this(source, target)
		{
			SetFilter(ref filter);
		}
		/// <summary>
		/// Creates a single pass 2 sample filter
		/// </summary>
		/// <param name="source">soure texture to filter</param>
		/// <param name="target">target to filter</param>
		/// <param name="filter">Filter to apply</param>
		public SinglePassTextureFilter(DrawTargetTexture2D source, DrawTargetTexture2D target, Filter2Sample filter)
			: this(source, target)
		{
			SetFilter(ref filter);
		}

		internal SinglePassTextureFilter(DrawTargetTexture2D source, DrawTargetTexture2D target)
		{
			if (source == null || target == null)
				throw new ArgumentNullException();
			if (source.SurfaceFormat != target.SurfaceFormat)
				throw new ArgumentException("source.SurfaceFormat != target.SurfaceFormat");
			if (source == target)
				throw new ArgumentException("source == target is invalid");
			if (source.Width > target.Width ||
				source.Height > target.Height)
				throw new ArgumentException("source is larger than target");

			this.source = source;
			this.targetClone = target.Clone(false,false,false);

			this.targetClone.ClearBuffer.Enabled = false;

			this.element = new ShaderElement(null, source.Width, source.Height,new Vector2(1,1),true);
			this.targetClone.Add(element);

			if (target.Width != source.Width ||
				target.Height != source.Height)
			{
				this.targetClone.AddModifier(new Xen.Graphics.Modifier.ScissorModifier(0, 0, Math.Min(1, (source.Width) / target.Width), Math.Min(1, (source.Height) / target.Height)));
				this.element.TextureCrop = new Rectangle(0, 0, Math.Min(this.source.Width, this.targetClone.Width), Math.Min(this.source.Height, this.targetClone.Height));
			}
		}

		/// <summary>
		/// Set a 16 sample filter
		/// </summary>
		/// <param name="filter"></param>
		public void SetFilter(ref Filter16Sample filter)
		{
			this.filter[0] = new Vector3(filter.PixelOffset0, filter.Weight0);
			this.filter[1] = new Vector3(filter.PixelOffset1, filter.Weight1);
			this.filter[2] = new Vector3(filter.PixelOffset2, filter.Weight2);
			this.filter[3] = new Vector3(filter.PixelOffset3, filter.Weight3);
			this.filter[4] = new Vector3(filter.PixelOffset4, filter.Weight4);
			this.filter[5] = new Vector3(filter.PixelOffset5, filter.Weight5);
			this.filter[6] = new Vector3(filter.PixelOffset6, filter.Weight6);
			this.filter[7] = new Vector3(filter.PixelOffset7, filter.Weight7);

			this.filter[8] = new Vector3(filter.PixelOffset8, filter.Weight8);
			this.filter[9] = new Vector3(filter.PixelOffset9, filter.Weight9);
			this.filter[10] = new Vector3(filter.PixelOffset10, filter.Weight10);
			this.filter[11] = new Vector3(filter.PixelOffset11, filter.Weight11);
			this.filter[12] = new Vector3(filter.PixelOffset12, filter.Weight12);
			this.filter[13] = new Vector3(filter.PixelOffset13, filter.Weight13);
			this.filter[14] = new Vector3(filter.PixelOffset14, filter.Weight14);
			this.filter[15] = new Vector3(filter.PixelOffset15, filter.Weight15);

			kernelSize = 16;
		}

		/// <summary>
		/// Set an 8 sample filter
		/// </summary>
		/// <param name="filter"></param>
		public void SetFilter(ref Filter8Sample filter)
		{
			this.filter[0] = new Vector3(filter.PixelOffset0, filter.Weight0);
			this.filter[1] = new Vector3(filter.PixelOffset1, filter.Weight1);
			this.filter[2] = new Vector3(filter.PixelOffset2, filter.Weight2);
			this.filter[3] = new Vector3(filter.PixelOffset3, filter.Weight3);
			this.filter[4] = new Vector3(filter.PixelOffset4, filter.Weight4);
			this.filter[5] = new Vector3(filter.PixelOffset5, filter.Weight5);
			this.filter[6] = new Vector3(filter.PixelOffset6, filter.Weight6);
			this.filter[7] = new Vector3(filter.PixelOffset7, filter.Weight7);

			kernelSize = 8;
		}

		/// <summary>
		/// Set a 4 sample filter
		/// </summary>
		/// <param name="filter"></param>
		public void SetFilter(ref Filter4Sample filter)
		{
			this.filter[0] = new Vector3(filter.PixelOffset0, filter.Weight0);
			this.filter[1] = new Vector3(filter.PixelOffset1, filter.Weight1);
			this.filter[2] = new Vector3(filter.PixelOffset2, filter.Weight2);
			this.filter[3] = new Vector3(filter.PixelOffset3, filter.Weight3);

			kernelSize = 4;
		}

		/// <summary>
		/// Set a 2 sample filter
		/// </summary>
		/// <param name="filter"></param>
		public void SetFilter(ref Filter2Sample filter)
		{
			this.filter[0] = new Vector3(filter.PixelOffset0, filter.Weight0);
			this.filter[1] = new Vector3(filter.PixelOffset1, filter.Weight1);

			kernelSize = 2;
		}

		internal void SetFilter(Vector2[] offsets, float[] weights, int kernelSize)
		{
			for (int i = 0; i < 16; i++)
				this.filter[i] = new Vector3(offsets[i], weights[i]);

			this.kernelSize = kernelSize;
		}

		/// <summary>
		/// Apply the filter
		/// </summary>
		/// <param name="state"></param>
		public void Draw(FrameState state)
		{
			switch (kernelSize)
			{
				case 16:
					Kernel16 shader16 = state.GetShader<Kernel16>();
					shader16.Texture = source.GetTexture();
					shader16.TextureSize = new Vector2(1.0f / this.source.Size.X, 1.0f / this.source.Size.Y);
					shader16.Kernel = (this.filter);
					element.Shader = shader16;
					break;
				case 15:
					Kernel15 shader15 = state.GetShader<Kernel15>();
					shader15.Texture = source.GetTexture();
					shader15.TextureSize = new Vector2(1.0f / this.source.Size.X, 1.0f / this.source.Size.Y);
					shader15.Kernel = (this.filter);
					element.Shader = shader15;
					break;
				case 8:
					Kernel8 shader8 = state.GetShader<Kernel8>();
					shader8.Texture = source.GetTexture();
					shader8.TextureSize = new Vector2(1.0f / this.source.Size.X, 1.0f / this.source.Size.Y);
					shader8.Kernel = (this.filter);
					element.Shader = shader8;
					break;
				case 7:
					Kernel7 shader7 = state.GetShader<Kernel7>();
					shader7.Texture = source.GetTexture();
					shader7.TextureSize = new Vector2(1.0f / this.source.Size.X, 1.0f / this.source.Size.Y);
					shader7.Kernel = (this.filter);
					element.Shader = shader7;
					break;
				case 4:
					Kernel4 shader4 = state.GetShader<Kernel4>();
					shader4.Texture = source.GetTexture();
					shader4.TextureSize = new Vector2(1.0f / this.source.Size.X, 1.0f / this.source.Size.Y);
					shader4.Kernel = (this.filter);
					element.Shader = shader4;
					break;
				case 3:
					Kernel3 shader3 = state.GetShader<Kernel3>();
					shader3.Texture = source.GetTexture();
					shader3.TextureSize = new Vector2(1.0f / this.source.Size.X, 1.0f / this.source.Size.Y);
					shader3.Kernel = (this.filter);
					element.Shader = shader3;
					break;
				case 2:
					Kernel2 shader2 = state.GetShader<Kernel2>();
					shader2.Texture = source.GetTexture();
					shader2.TextureSize = new Vector2(1.0f / this.source.Size.X, 1.0f / this.source.Size.Y);
					shader2.Kernel = (this.filter);
					element.Shader = shader2;
					break;
			}
			targetClone.Draw(state);
		}
	}

	/// <summary>
	/// Applies a two pass, 16 sample vertical and horizontal texture blur filter to a draw target
	/// </summary>
	public sealed class BlurFilter : IFrameDraw
	{
		private readonly SinglePassTextureFilter filterV, filterH;
		private readonly DrawTargetTexture2D source;

		/// <summary>
		/// Blur the source horizontally to the <paramref name="intermediate"/> target, then blur vertically back to <paramref name="source"/>.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="filterFormat">format of the blur filter</param>
		/// <param name="intermediate">draw target to use as a temporary, intermediate target for blurring</param>
		/// <param name="bellCurveExponent">
		/// <para>A scale value to infulence the bell curve used to generate the filter kernel.</para>
		/// <para>A value of 1.0 generates a standard blur filter kernels. Larger values will produce a tighter curve, and less blur.</para>
		/// <para>Smaller values will produce a wider curve, and a larger blur - but may produce a visible edge as the curve more rapidly ends.</para>
		/// </param>
		public BlurFilter(BlurFilterFormat filterFormat, float bellCurveExponent, DrawTargetTexture2D source, DrawTargetTexture2D intermediate) :
			this(filterFormat, bellCurveExponent, source, intermediate, source)
		{
		}

		/// <summary>
		/// Blur the source horizontally to the <paramref name="intermediate"/> target, then blur vertically to <paramref name="target"/>.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="filterFormat">format of the blur filter</param>
		/// <param name="intermediate">draw target to use as a temporary, intermediate target for blurring</param>
		/// <param name="target"></param>
		/// <param name="bellCurveExponent">
		/// <para>A scale value to infulence the bell curve used to generate the filter kernel.</para>
		/// <para>A value of 1.0 generates a standard blur filter kernels. Larger values will produce a tighter curve, and less blur.</para>
		/// <para>Smaller values will produce a wider curve, and a larger blur - but may produce a visible edge as the curve more rapidly ends.</para>
		/// </param>
		public BlurFilter(BlurFilterFormat filterFormat, float bellCurveExponent, DrawTargetTexture2D source, DrawTargetTexture2D intermediate, DrawTargetTexture2D target)
		{
			if (target == null || source == null)
				throw new ArgumentNullException();

			if (intermediate != null && source.SurfaceFormat != intermediate.SurfaceFormat)
				throw new ArgumentException("source.SurfaceFormat != intermediate.SurfaceFormat");
			if (intermediate != null && target.SurfaceFormat != intermediate.SurfaceFormat)
				throw new ArgumentException("target.SurfaceFormat != intermediate.SurfaceFormat");

			this.source = source;

			this.filterV = new SinglePassTextureFilter(source, intermediate);
			this.filterH = new SinglePassTextureFilter(intermediate, target);

			SetFilterFormat(filterFormat, bellCurveExponent);
		}

		/// <summary>
		/// Apply the filter
		/// </summary>
		/// <param name="state"></param>
		public void Draw(FrameState state)
		{
			filterV.Draw(state);
			filterH.Draw(state);
		}

		/// <summary>
		/// <para>Set the filter format of the blur filter.</para>
		/// <para>Note: Some filter formats require the graphics device support texture filtering for the given format</para>
		/// </summary>
		/// <param name="filterFormat"></param>
		/// <param name="bellCurveExponent">
		/// <para>A scale value to infulence the bell curve used to generate the filter kernel.</para>
		/// <para>A value of 1.0 generates a standard blur filter kernels. Larger values will produce a tighter curve, and less blur.</para>
		/// <para>Smaller values will produce a wider curve, and a larger blur - but may produce a visible edge as the curve more rapidly ends.</para>
		/// </param>
		public void SetFilterFormat(BlurFilterFormat filterFormat, float bellCurveExponent)
		{
			//IsFilterFormat
			bool filteringSupported = 
				
				DrawTargetTexture2D.SupportsFormatFiltering(source.SurfaceFormat);

			if (filteringSupported == false && filterFormat == BlurFilterFormat.ThirtyOneSampleBlur_FilteredTextureFormat)
				throw new ArgumentException("BlurFilterFormat.ThirtyOneSampleBlurFiltered is not supported, Hardware does not support texture filter for " + source.SurfaceFormat.ToString());

			FilterGenerator.SetFilter(filterFormat, true, this.filterH, filteringSupported, bellCurveExponent);
			FilterGenerator.SetFilter(filterFormat, false, this.filterV, filteringSupported, bellCurveExponent);
		}
	}

	/// <summary>
	/// Performs a number of passes to downsample a draw target to a desired size
	/// </summary>
	public sealed class TextureDownsample : IFrameDraw, IDownsampleShaderProvider
	{
		private List<IFrameDraw> passes = new List<IFrameDraw>();
		
		/// <summary>
		/// Construct the texture downsampler
		/// </summary>
		/// <param name="source">Source texture to read</param>
		/// <param name="target">Target texture to write to</param>
		/// <param name="intermediate">Intermediate texture (if null, will be created as required)</param>
		/// <param name="intermediate2">Second intermediate texture (if null, will be created as required)</param>
		/// <param name="targetWidth">target width to downsample to</param>
		/// <param name="targetHeight">target height to downsample to</param>
		public TextureDownsample(DrawTargetTexture2D source, DrawTargetTexture2D target, ref DrawTargetTexture2D intermediate, ref DrawTargetTexture2D intermediate2, int targetWidth, int targetHeight)
			: this(source, target, ref intermediate, ref intermediate2, targetWidth, targetHeight, null)
		{
		}
		/// <summary>
		/// Construct the texture downsampler
		/// </summary>
		/// <param name="source">Source texture to read</param>
		/// <param name="target">Target texture to write to</param>
		/// <param name="intermediate">Intermediate texture (if null, will be created as required)</param>
		/// <param name="intermediate2">Second intermediate texture (if null, will be created as required)</param>
		/// <param name="targetWidth">target width to downsample to</param>
		/// <param name="targetHeight">target height to downsample to</param>
		/// <param name="shaderProvider">Optional provider for downsample shaders</param>
		public TextureDownsample(DrawTargetTexture2D source, DrawTargetTexture2D target, ref DrawTargetTexture2D intermediate, ref DrawTargetTexture2D intermediate2, int targetWidth, int targetHeight,
					IDownsampleShaderProvider shaderProvider)
		{
			if (source == null ||
				target == null)
				throw new ArgumentNullException();

			if (targetWidth <= 0 ||
				targetHeight <= 0)
				throw new ArgumentException("Invalid target size");

			if (DrawTarget.FormatChannels(source.SurfaceFormat) != DrawTarget.FormatChannels(target.SurfaceFormat))
			    throw new ArgumentException("source.SurfaceFormat has a different number of channels than target.SurfaceFormat");

			if (targetWidth > target.Width ||
				targetHeight > target.Height)
				throw new ArgumentException("Size is larger than target");

			if (targetWidth > source.Width ||
				targetHeight > source.Height)
				throw new ArgumentException("Size is larger than source");

			if (intermediate != null)
			{
				if (target.SurfaceFormat != intermediate.SurfaceFormat)
					throw new ArgumentException("target.SurfaceFormat != intermediate.SurfaceFormat");
				if (intermediate == intermediate2)
					throw new ArgumentException("intermediate == intermediate2");
			}
			if (intermediate2 != null)
			{
				if (target.SurfaceFormat != intermediate2.SurfaceFormat)
					throw new ArgumentException("target.SurfaceFormat != intermediate2.SurfaceFormat");
			}

			int w = source.Width;
			int h = source.Height;

			int targetMultipleWidth = targetWidth;
			int targetMultipleHeight = targetHeight;

			while (targetMultipleWidth * 2 <= w)
				targetMultipleWidth *= 2;
			while (targetMultipleHeight * 2 <= h)
				targetMultipleHeight *= 2;

			DrawTargetTexture2D current = null;
			Rectangle sRegion = new Rectangle(0,0,0,0);

			//first pass may require that the source is sized down to a multiple of the target size

			if ((double)targetWidth / (double)w <= 0.5 &&
				(double)targetHeight / (double)h <= 0.5 &&
				(targetMultipleWidth != w || targetMultipleHeight != h))
			{
				DrawTargetTexture2D go = this.PickRT(ref intermediate, ref intermediate2, source, targetMultipleWidth, targetMultipleHeight,target.SurfaceFormat);

				Vector2 size = new Vector2((float)targetMultipleWidth, (float)targetMultipleHeight);

				TexturedElement te = new TexturedElement(source, size, false);
				te.TextureCrop = new Rectangle(0,0,w,h);

				go.Add(te);
				passes.Add(go);
				current = go;
				w = targetMultipleWidth;
				h = targetMultipleHeight;
			}

			//downsample on the larger axis, either 2x, 4x or 8x downsampling, until reached the target size

			while (target.Equals(current) == false)
			{
				DrawTargetTexture2D localSource = current ?? source;

				double difW = (double)targetWidth / (double)w;
				double difH = (double)targetHeight / (double)h;

				sRegion.Width = w;
				sRegion.Height = h;
				sRegion.Y = localSource.Height - h;

				//both width/height difference are less than 50% smaller, so a linear interpolation will do fine
				if (difW > 0.5 &&
					difH > 0.5)
				{
					//write directly to the target
					DrawTargetTexture2D go = target.Clone(false, false, false);
					Vector2 te_size = new Vector2((float)targetWidth, (float)targetHeight);
					TexturedElement te = new TexturedElement(localSource, te_size, false);

					go.AddModifier(new ScissorModifier(0, go.Height - targetHeight, targetWidth, go.Height, go));
					te.TextureCrop = sRegion;

					go.Add(te);
					passes.Add(go);
					current = go;

					continue;
				}

				bool horizontal = difW < difH;
				double dif = Math.Min(difW, difH);
				int size = horizontal ? w : h;

				Vector2 dir = new Vector2(0,0);
				if (horizontal)
					dir.X = 1.0f / localSource.Width;
				else
					dir.Y = 1.0f / localSource.Height;

				if (dif > 0.25) // cutoff for using 2 samples
				{
					DrawTargetTexture2D go;
					int new_width = w;
					int new_height = h;
					if (horizontal)
						new_width /= 2;
					else
						new_height /= 2;

					if (new_width == targetWidth && new_height == targetHeight)
						go = target.Clone(false, false, false);
					else
						go = PickRT(ref intermediate, ref intermediate2, localSource, new_width, new_height, target.SurfaceFormat);

					Vector2 se_size = new Vector2((float)new_width, (float)new_height);
					ShaderElement se = new ShaderElement((shaderProvider ?? this).DownsampleShader2, se_size, false);

					go.AddModifier(new ScissorModifier(0, go.Height - new_height, new_width, go.Height, go));

					se.TextureCrop = sRegion;

					go.Add(new Drawer(dir,se,localSource));
					passes.Add(go);

					w = new_width;
					h = new_height;

					current = go;
					continue;
				}

				if (dif > 0.125) // cutoff for using 4 samples
				{
					DrawTargetTexture2D go;
					int new_width = w;
					int new_height = h;
					if (horizontal)
						new_width /= 4;
					else
						new_height /= 4;

					if (new_width == targetWidth && new_height == targetHeight)
						go = target.Clone(false, false, false);
					else
						go = PickRT(ref intermediate, ref intermediate2, localSource, new_width, new_height, target.SurfaceFormat);

					Vector2 se_size = new Vector2((float)new_width, (float)new_height);
					ShaderElement se = new ShaderElement((shaderProvider ?? this).DownsampleShader4, se_size, false);

					go.AddModifier(new ScissorModifier(0, go.Height - new_height, new_width, go.Height, go));

					se.TextureCrop = sRegion;

					go.Add(new Drawer(dir, se, localSource));
					passes.Add(go);

					w = new_width;
					h = new_height;

					current = go;
					continue;
				}

				// cutoff for using 8 samples
				{
					DrawTargetTexture2D go;
					int new_width = w;
					int new_height = h;
					if (horizontal)
						new_width /= 8;
					else
						new_height /= 8;
					
					if (new_width == targetWidth && new_height == targetHeight)
						go = target.Clone(false, false, false);
					else
						go = PickRT(ref intermediate, ref intermediate2, localSource, new_width, new_height, target.SurfaceFormat);

					Vector2 se_size = new Vector2((float)new_width, (float)new_height);
					ShaderElement se = new ShaderElement((shaderProvider ?? this).DownsampleShader8, se_size, false);

					go.AddModifier(new ScissorModifier(0, go.Height - new_height, new_width, go.Height, go));

					se.TextureCrop = sRegion;

					go.Add(new Drawer(dir, se, localSource));
					passes.Add(go);

					w = new_width;
					h = new_height;

					current = go;
					continue;
				}
			}
		}

		DrawTargetTexture2D PickRT(ref DrawTargetTexture2D int1, ref DrawTargetTexture2D int2, DrawTargetTexture2D source, int w, int h, Microsoft.Xna.Framework.Graphics.SurfaceFormat targetFormat)
		{
			DrawTargetTexture2D target = null;
			if (int1 != null && int1.Equals(source))
				target = int2;
			else
				if (int2 != null && int2.Equals(source))
					target = int1;

			if (target == null)
			{
				int tw = ((w + 15) / 16) * 16;
				int th = ((h + 15) / 16) * 16;

				target = new DrawTargetTexture2D(source.Camera, tw, th, targetFormat, Microsoft.Xna.Framework.Graphics.DepthFormat.None);
			}
			else
			{
				if (target.Width < w ||
					target.Height < h)
				{
					string from = "intermediate";
					if (target == int2)
						from += "2";

					throw new ArgumentException(string.Format("'{0}' draw target is too small, minimum required size for '{1}' in the current context is {2}x{3}", from, from, w, h));
				}
			}

			if (int1 == null)
			{
				int1 = target;
			}
			else
			{
				if (int2 == null  &&
					target != int1)
				{
					int2 = target;
				}
			}

			return target.Clone(false, false, false);
		}

		class Drawer : IDraw
		{
			private static int directionId = -1;
			private static int texutreId = -1;

			private Vector2 direction;
			private ShaderElement drawable;
			private DrawTargetTexture2D source;

			public Drawer(Vector2 direction, ShaderElement drawable, DrawTargetTexture2D source)
			{
				drawable.SetTextureSize(source.Width, source.Height);
				this.drawable = drawable;
				this.source = source;
				this.direction = direction;
			}

			public void Draw(DrawState state)
			{
				if (directionId == -1)
				{
					directionId = state.GetShaderAttributeNameUniqueID("sampleDirection");
					texutreId = state.GetShaderAttributeNameUniqueID("Texture");
				}

				drawable.Shader.SetAttribute(state, directionId, ref direction);
				drawable.Shader.SetTexture(state, texutreId, source.GetTexture());

				drawable.Draw(state);
			}

			bool ICullable.CullTest(ICuller culler)
			{
				return true;
			}
		}

		/// <summary>
		/// Perform the texture downsample filter
		/// </summary>
		/// <param name="state"></param>
		public void Draw(FrameState state)
		{
			foreach (IFrameDraw draw in passes)
				draw.Draw(state);
		}

		private readonly static string downsampleShaderName2 = typeof(TextureDownsample).FullName + ".DownsampleShader2";
		private readonly static string downsampleShaderName4 = typeof(TextureDownsample).FullName + ".DownsampleShader4";
		private readonly static string downsampleShaderName8 = typeof(TextureDownsample).FullName + ".DownsampleShader8";

		IDownsampleShader IDownsampleShaderProvider.DownsampleShader2
		{
			get 
			{
				var app = Application.GetApplicationInstance();
				var shader = app.UserValues[downsampleShaderName2] as IDownsampleShader;
				if (shader == null)
				{
					shader = new Downsample2();
					app.UserValues[downsampleShaderName2] = shader;
				}
				return shader; 
			}
		}
		static Downsample4 s = new Downsample4();
		IDownsampleShader IDownsampleShaderProvider.DownsampleShader4
		{
			get
			{
				var app = Application.GetApplicationInstance();
				var shader = app.UserValues[downsampleShaderName4] as IDownsampleShader;
				if (shader == null)
				{
					shader = new Downsample4();
					app.UserValues[downsampleShaderName4] = shader;
				}
				return shader;
			}
		}

		IDownsampleShader IDownsampleShaderProvider.DownsampleShader8
		{
			get
			{
				var app = Application.GetApplicationInstance();
				var shader = app.UserValues[downsampleShaderName8] as IDownsampleShader;
				if (shader == null)
				{
					shader = new Downsample8();
					app.UserValues[downsampleShaderName8] = shader;
				}
				return shader;
			}
		}
	}
}
