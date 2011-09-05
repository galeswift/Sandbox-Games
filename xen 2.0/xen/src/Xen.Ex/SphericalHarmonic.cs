using System;
using Microsoft.Xna.Framework;

namespace Xen.Ex
{
	/// <summary>
	/// <para>A structure storing a 9 constant (L2) RGB sperhical harmonic. See remarks for details.</para>
	/// <para>An L2 SH is significantly more accurate than an L1 SH, however it is also significantly more complex to calculate and display.</para>
	/// </summary>
	/// <remarks><para>L1 and L2 Spherical harmonics are useful for storing approximate lighiting information.</para>
	/// <para>They work by storing the an approximation of of a lighting function over a sphere.<br/>
	/// Where as most approximations work in one or two dimensions (for example, approximations to sin/cos), a sherical harmonic is an approximation of an RGB (red/blue/green) function that maps to a sphere.<br/> 
	/// The most common use of an L2 spherical harmonic is to efficiently encode directional lighting information at a point. Another method to achieve this would be to use a Cube Map. 
	/// An SH works well as a substitute, as it typically will use much less data, and produces a smoother output.</para>
	/// <para>Spherical harmonics also have the advantage they can be efficiently added together, multiplied, lerped, etc.</para>
	/// <para>An example of how an SH can be used to store lighting information:<br/>
	/// In a game, it may be desirable that an object is lit by multiple nearby lights. However, it may be too inefficient to light the object with every light.<br/>
	/// An approximation to this lighting, would be to use a SH. Given the centre point of the object, if there are 8 lights nearby, then call <see cref="AddLight(Vector3, Vector3, float)"/> once for each light.
	/// This will store the approximate contribution of that light (for it's given direction) in the SH function. Sampling the SH (usually in the vertex shader) will produce an approximation for the input lights, however it will be much cheaper than sampling each light for every pixel.</para>
	/// <para>Consider the following code example:</para>
	/// <example>
	/// <code>
	///		SphericalHarmonicL2RGB sh = new SphericalHarmonicL2RGB();
	///		
	///		//Add light to the SH, coming from the positive X direction
	///		Vector3 light = new Vector3(1,1,1);
	///		Vector3 direction = new Vector3(1,0,0);
	///		sh.AddLight(light, direction, 1);
	///
	///		//Sample the SH function in three directions.
	///		Vector3 lightPosX = sh.SampleDirection(new Vector3(1, 0, 0));
	///		Vector3 lightNegX = sh.SampleDirection(new Vector3(-1, 0, 0));
	///		Vector3 lightPosY = sh.SampleDirection(new Vector3(0, 1, 0));
	/// </code></example>
	/// <para>In the above example, white RGB light in the positive X direction is added to an blank SH.<br/>
	/// The SH is then sampled three times, in the positive / negative X axis, and also the Y axis.<br/>
	/// The output of the SH will be very close to (1,1,1) in the positive X axis (as is desired, as this is the direction of the input light).<br/>
	/// The output of the SH in the negative X and positive Y axis will be close to zero (as no light is coming from these directions).<br/>
	/// Note, the outputs will not be <i>exactly</i> one or zero (as it's an approximation!).</para>
	/// <para>Note that AddLight can be called as often as desired, which is useful to accumulate ligth from multiple sources.</para>
	/// <para>
	/// The following Shader code can be used to sample an SH stored in GPU format (see <see cref="CopyToGpuArray(Vector4[])"/>):<br/>
	/// <code>
	/// 
	/// //storage for the Spherical Harmonic
	///	float4 SH[9];
	///
	///	float3 SampleSH(float3 normal)
	///	{
	///		//optional:
	///		normal = normalize(normal); // either make sure the input is normalised, or normalise here.
	///		
	///		float3 sample =
	///			SH[0].xyz + 
	///			SH[1].xyz * normal.x +  
	///			SH[2].xyz * normal.y + 
	///			SH[3].xyz * normal.z + 
	///			SH[4].xyz * (normal.x * normal.y) +
	///			SH[5].xyz * (normal.y * normal.z) + 
	///			SH[6].xyz * (normal.x * normal.z) + 
	///			SH[7].xyz * ((normal.z * normal.z) - (1.0f / 3.0f)) + 
	///			SH[8].xyz * ((normal.x * normal.x) - (normal.y * normal.y));
	///			
	///		//optional:
	///		sample = max(0,sample);		// clamp to zero, it's possible for the SH approximation to output negative values
	///		
	///		return sample;
	///	}
	/// </code></para></remarks>
	public struct SphericalHarmonicL2RGB
	{
		//Spherical harmonic storage.
		/// <summary>
		/// Spherical Harmonic RGB Constant
		/// </summary>
		public Vector3 SH0, SH1, SH2, SH3, SH4, SH5, SH6, SH7, SH8;
		/// <summary>
		/// <para>Accumulated weighting value. This value is provided as a helper to make averaging easier. It stores accumulated weighting values from calls to AddLight.</para>
		/// <para>See <see cref="AddLight(Vector3, Vector3, float)"/> for further details.</para>
		/// </summary>
		public float Weighting;

		/// <summary>
		/// <para>Store the SH constants in a Vector4 array (The vector array must have a length of 9)</para>
		/// <para>Note, the W values in the output array are not used (they are written as Zero). If desired, these values can be used for other data.</para>
		/// <para>See <see cref="SphericalHarmonicL2RGB"/> remarks for example shader code to sample the SH.</para>
		/// </summary>
		public void CopyToGpuArray(Vector4[] l2SphericalHarmonicGpuFormatArray)
		{
			if (l2SphericalHarmonicGpuFormatArray == null)
				throw new ArgumentNullException("l2SphericalHarmonicGpuFormatArray");
			if (l2SphericalHarmonicGpuFormatArray.Length < 9)
				throw new ArgumentException("Input array must be at least 9 elements long");

			Vector4 value = new Vector4();

			value.X = SH0.X; value.Y = SH0.Y; value.Z = SH0.Z;
			l2SphericalHarmonicGpuFormatArray[0] = value;

			value.X = SH1.X; value.Y = SH1.Y; value.Z = SH1.Z;
			l2SphericalHarmonicGpuFormatArray[1] = value;

			value.X = SH2.X; value.Y = SH2.Y; value.Z = SH2.Z;
			l2SphericalHarmonicGpuFormatArray[2] = value;

			value.X = SH3.X; value.Y = SH3.Y; value.Z = SH3.Z;
			l2SphericalHarmonicGpuFormatArray[3] = value;

			value.X = SH4.X; value.Y = SH4.Y; value.Z = SH4.Z;
			l2SphericalHarmonicGpuFormatArray[4] = value;

			value.X = SH5.X; value.Y = SH5.Y; value.Z = SH5.Z;
			l2SphericalHarmonicGpuFormatArray[5] = value;

			value.X = SH6.X; value.Y = SH6.Y; value.Z = SH6.Z;
			l2SphericalHarmonicGpuFormatArray[6] = value;

			value.X = SH7.X; value.Y = SH7.Y; value.Z = SH7.Z;
			l2SphericalHarmonicGpuFormatArray[7] = value;

			value.X = SH8.X; value.Y = SH8.Y; value.Z = SH8.Z;
			l2SphericalHarmonicGpuFormatArray[8] = value;
		}

		/// <summary>
		/// <para>Store the SH constants in a Vector4 array (The vector array must have a length of 9)</para>
		/// <para>Note, the W values in the output array are not used (they are written as Zero). If desired, these values can be used for other data.</para>
		/// <para>See <see cref="SphericalHarmonicL2RGB"/> remarks for example shader code to sample the SH.</para>
		/// </summary>
		/// <param name="l2SphericalHarmonicGpuFormatArray"></param>
		/// <param name="offset">Offset into the array to begin writing the SH</param>
		public void CopyToGpuArray(Vector4[] l2SphericalHarmonicGpuFormatArray, int offset)
		{
			if (l2SphericalHarmonicGpuFormatArray == null)
				throw new ArgumentNullException("l2SphericalHarmonicGpuFormatArray");
			if (l2SphericalHarmonicGpuFormatArray.Length < 9 + offset)
				throw new ArgumentException("Input array is too small");

			l2SphericalHarmonicGpuFormatArray[offset + 0] = new Vector4(SH0, 0);
			l2SphericalHarmonicGpuFormatArray[offset + 1] = new Vector4(SH1, 0);
			l2SphericalHarmonicGpuFormatArray[offset + 2] = new Vector4(SH2, 0);
			l2SphericalHarmonicGpuFormatArray[offset + 3] = new Vector4(SH3, 0);
			l2SphericalHarmonicGpuFormatArray[offset + 4] = new Vector4(SH4, 0);
			l2SphericalHarmonicGpuFormatArray[offset + 5] = new Vector4(SH5, 0);
			l2SphericalHarmonicGpuFormatArray[offset + 6] = new Vector4(SH6, 0);
			l2SphericalHarmonicGpuFormatArray[offset + 7] = new Vector4(SH7, 0);
			l2SphericalHarmonicGpuFormatArray[offset + 8] = new Vector4(SH8, 0);
		}

		/// <summary>
		/// Add two spherical harmonics together
		/// </summary>
		public static void Add(ref SphericalHarmonicL2RGB x, ref SphericalHarmonicL2RGB y, out SphericalHarmonicL2RGB result)
		{
			result = new SphericalHarmonicL2RGB();
			result.Weighting = x.Weighting + y.Weighting;

			result.SH0.X = x.SH0.X + y.SH0.X; result.SH0.Y = x.SH0.Y + y.SH0.Y; result.SH0.Z = x.SH0.Z + y.SH0.Z;
			result.SH1.X = x.SH1.X + y.SH1.X; result.SH1.Y = x.SH1.Y + y.SH1.Y; result.SH1.Z = x.SH1.Z + y.SH1.Z;
			result.SH2.X = x.SH2.X + y.SH2.X; result.SH2.Y = x.SH2.Y + y.SH2.Y; result.SH2.Z = x.SH2.Z + y.SH2.Z;
			result.SH3.X = x.SH3.X + y.SH3.X; result.SH3.Y = x.SH3.Y + y.SH3.Y; result.SH3.Z = x.SH3.Z + y.SH3.Z;
			result.SH4.X = x.SH4.X + y.SH4.X; result.SH4.Y = x.SH4.Y + y.SH4.Y; result.SH4.Z = x.SH4.Z + y.SH4.Z;
			result.SH5.X = x.SH5.X + y.SH5.X; result.SH5.Y = x.SH5.Y + y.SH5.Y; result.SH5.Z = x.SH5.Z + y.SH5.Z;
			result.SH6.X = x.SH6.X + y.SH6.X; result.SH6.Y = x.SH6.Y + y.SH6.Y; result.SH6.Z = x.SH6.Z + y.SH6.Z;
			result.SH7.X = x.SH7.X + y.SH7.X; result.SH7.Y = x.SH7.Y + y.SH7.Y; result.SH7.Z = x.SH7.Z + y.SH7.Z;
			result.SH8.X = x.SH8.X + y.SH8.X; result.SH8.Y = x.SH8.Y + y.SH8.Y; result.SH8.Z = x.SH8.Z + y.SH8.Z;
		}

		/// <summary>
		/// Multiply a spherical harmonic by a constant scale factor
		/// </summary>
		public static void Multiply(ref SphericalHarmonicL2RGB x, float scale, out SphericalHarmonicL2RGB result)
		{
			result = new SphericalHarmonicL2RGB();
			result.Weighting = x.Weighting * scale;

			result.SH0.X = x.SH0.X * scale; result.SH0.Y = x.SH0.Y * scale; result.SH0.Z = x.SH0.Z * scale;
			result.SH1.X = x.SH1.X * scale; result.SH1.Y = x.SH1.Y * scale; result.SH1.Z = x.SH1.Z * scale;
			result.SH2.X = x.SH2.X * scale; result.SH2.Y = x.SH2.Y * scale; result.SH2.Z = x.SH2.Z * scale;
			result.SH3.X = x.SH3.X * scale; result.SH3.Y = x.SH3.Y * scale; result.SH3.Z = x.SH3.Z * scale;
			result.SH4.X = x.SH4.X * scale; result.SH4.Y = x.SH4.Y * scale; result.SH4.Z = x.SH4.Z * scale;
			result.SH5.X = x.SH5.X * scale; result.SH5.Y = x.SH5.Y * scale; result.SH5.Z = x.SH5.Z * scale;
			result.SH6.X = x.SH6.X * scale; result.SH6.Y = x.SH6.Y * scale; result.SH6.Z = x.SH6.Z * scale;
			result.SH7.X = x.SH7.X * scale; result.SH7.Y = x.SH7.Y * scale; result.SH7.Z = x.SH7.Z * scale;
			result.SH8.X = x.SH8.X * scale; result.SH8.Y = x.SH8.Y * scale; result.SH8.Z = x.SH8.Z * scale;
		}

		/// <summary>
		/// Divide a spherical harmonic by a constant factor
		/// </summary>
		public static void Divide(ref SphericalHarmonicL2RGB x, float divider, out SphericalHarmonicL2RGB result)
		{
			result = new SphericalHarmonicL2RGB();

			float scale = 1.0f / divider;
			result.Weighting = x.Weighting * scale;

			result.SH0.X = x.SH0.X * scale; result.SH0.Y = x.SH0.Y * scale; result.SH0.Z = x.SH0.Z * scale;
			result.SH1.X = x.SH1.X * scale; result.SH1.Y = x.SH1.Y * scale; result.SH1.Z = x.SH1.Z * scale;
			result.SH2.X = x.SH2.X * scale; result.SH2.Y = x.SH2.Y * scale; result.SH2.Z = x.SH2.Z * scale;
			result.SH3.X = x.SH3.X * scale; result.SH3.Y = x.SH3.Y * scale; result.SH3.Z = x.SH3.Z * scale;
			result.SH4.X = x.SH4.X * scale; result.SH4.Y = x.SH4.Y * scale; result.SH4.Z = x.SH4.Z * scale;
			result.SH5.X = x.SH5.X * scale; result.SH5.Y = x.SH5.Y * scale; result.SH5.Z = x.SH5.Z * scale;
			result.SH6.X = x.SH6.X * scale; result.SH6.Y = x.SH6.Y * scale; result.SH6.Z = x.SH6.Z * scale;
			result.SH7.X = x.SH7.X * scale; result.SH7.Y = x.SH7.Y * scale; result.SH7.Z = x.SH7.Z * scale;
			result.SH8.X = x.SH8.X * scale; result.SH8.Y = x.SH8.Y * scale; result.SH8.Z = x.SH8.Z * scale;
		}

		/// <summary>
		/// Linear interpolate (Lerp) between two spherical harmonics based on a interpolation factor
		/// </summary>
		/// <param name="factor">Determines the interpolation point. When factor is 1.0, the output will be <paramref name="x"/>, when factor is 0.0, the output will be <paramref name="y"/></param>
		/// <param name="result"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void Lerp(ref SphericalHarmonicL2RGB x, ref SphericalHarmonicL2RGB y, float factor, out SphericalHarmonicL2RGB result)
		{
			result = new SphericalHarmonicL2RGB();

			float xs = factor;
			float ys = 1.0f - factor;

			result.Weighting = x.Weighting * xs + y.Weighting * ys;

			result.SH0.X = xs * x.SH0.X + ys * y.SH0.X; result.SH0.Y = xs * x.SH0.Y + ys * y.SH0.Y; result.SH0.Z = xs * x.SH0.Z + ys * y.SH0.Z;
			result.SH1.X = xs * x.SH1.X + ys * y.SH1.X; result.SH1.Y = xs * x.SH1.Y + ys * y.SH1.Y; result.SH1.Z = xs * x.SH1.Z + ys * y.SH1.Z;
			result.SH2.X = xs * x.SH2.X + ys * y.SH2.X; result.SH2.Y = xs * x.SH2.Y + ys * y.SH2.Y; result.SH2.Z = xs * x.SH2.Z + ys * y.SH2.Z;
			result.SH3.X = xs * x.SH3.X + ys * y.SH3.X; result.SH3.Y = xs * x.SH3.Y + ys * y.SH3.Y; result.SH3.Z = xs * x.SH3.Z + ys * y.SH3.Z;
			result.SH4.X = xs * x.SH4.X + ys * y.SH4.X; result.SH4.Y = xs * x.SH4.Y + ys * y.SH4.Y; result.SH4.Z = xs * x.SH4.Z + ys * y.SH4.Z;
			result.SH5.X = xs * x.SH5.X + ys * y.SH5.X; result.SH5.Y = xs * x.SH5.Y + ys * y.SH5.Y; result.SH5.Z = xs * x.SH5.Z + ys * y.SH5.Z;
			result.SH6.X = xs * x.SH6.X + ys * y.SH6.X; result.SH6.Y = xs * x.SH6.Y + ys * y.SH6.Y; result.SH6.Z = xs * x.SH6.Z + ys * y.SH6.Z;
			result.SH7.X = xs * x.SH7.X + ys * y.SH7.X; result.SH7.Y = xs * x.SH7.Y + ys * y.SH7.Y; result.SH7.Z = xs * x.SH7.Z + ys * y.SH7.Z;
			result.SH8.X = xs * x.SH8.X + ys * y.SH8.X; result.SH8.Y = xs * x.SH8.Y + ys * y.SH8.Y; result.SH8.Z = xs * x.SH8.Z + ys * y.SH8.Z;
		}

		/// <summary>
		/// Add two spherical harmonics together
		/// </summary>
		public static SphericalHarmonicL2RGB operator +(SphericalHarmonicL2RGB x, SphericalHarmonicL2RGB y)
		{
			SphericalHarmonicL2RGB o = new SphericalHarmonicL2RGB();
			o.Weighting = x.Weighting + y.Weighting;

			o.SH0.X = x.SH0.X + y.SH0.X; o.SH0.Y = x.SH0.Y + y.SH0.Y; o.SH0.Z = x.SH0.Z + y.SH0.Z;
			o.SH1.X = x.SH1.X + y.SH1.X; o.SH1.Y = x.SH1.Y + y.SH1.Y; o.SH1.Z = x.SH1.Z + y.SH1.Z;
			o.SH2.X = x.SH2.X + y.SH2.X; o.SH2.Y = x.SH2.Y + y.SH2.Y; o.SH2.Z = x.SH2.Z + y.SH2.Z;
			o.SH3.X = x.SH3.X + y.SH3.X; o.SH3.Y = x.SH3.Y + y.SH3.Y; o.SH3.Z = x.SH3.Z + y.SH3.Z;
			o.SH4.X = x.SH4.X + y.SH4.X; o.SH4.Y = x.SH4.Y + y.SH4.Y; o.SH4.Z = x.SH4.Z + y.SH4.Z;
			o.SH5.X = x.SH5.X + y.SH5.X; o.SH5.Y = x.SH5.Y + y.SH5.Y; o.SH5.Z = x.SH5.Z + y.SH5.Z;
			o.SH6.X = x.SH6.X + y.SH6.X; o.SH6.Y = x.SH6.Y + y.SH6.Y; o.SH6.Z = x.SH6.Z + y.SH6.Z;
			o.SH7.X = x.SH7.X + y.SH7.X; o.SH7.Y = x.SH7.Y + y.SH7.Y; o.SH7.Z = x.SH7.Z + y.SH7.Z;
			o.SH8.X = x.SH8.X + y.SH8.X; o.SH8.Y = x.SH8.Y + y.SH8.Y; o.SH8.Z = x.SH8.Z + y.SH8.Z;

			return o;
		}

		/// <summary>
		/// Multiply a spherical harmonic by a constant scale factor
		/// </summary>
		public static SphericalHarmonicL2RGB operator *(SphericalHarmonicL2RGB x, float scale)
		{
			SphericalHarmonicL2RGB o = new SphericalHarmonicL2RGB();
			o.Weighting = x.Weighting * scale;

			o.SH0.X = x.SH0.X * scale; o.SH0.Y = x.SH0.Y * scale; o.SH0.Z = x.SH0.Z * scale;
			o.SH1.X = x.SH1.X * scale; o.SH1.Y = x.SH1.Y * scale; o.SH1.Z = x.SH1.Z * scale;
			o.SH2.X = x.SH2.X * scale; o.SH2.Y = x.SH2.Y * scale; o.SH2.Z = x.SH2.Z * scale;
			o.SH3.X = x.SH3.X * scale; o.SH3.Y = x.SH3.Y * scale; o.SH3.Z = x.SH3.Z * scale;
			o.SH4.X = x.SH4.X * scale; o.SH4.Y = x.SH4.Y * scale; o.SH4.Z = x.SH4.Z * scale;
			o.SH5.X = x.SH5.X * scale; o.SH5.Y = x.SH5.Y * scale; o.SH5.Z = x.SH5.Z * scale;
			o.SH6.X = x.SH6.X * scale; o.SH6.Y = x.SH6.Y * scale; o.SH6.Z = x.SH6.Z * scale;
			o.SH7.X = x.SH7.X * scale; o.SH7.Y = x.SH7.Y * scale; o.SH7.Z = x.SH7.Z * scale;
			o.SH8.X = x.SH8.X * scale; o.SH8.Y = x.SH8.Y * scale; o.SH8.Z = x.SH8.Z * scale;

			return o;
		}

		/// <summary>
		/// Divide a spherical harmonic by a constant factor
		/// </summary>
		public static SphericalHarmonicL2RGB operator /(SphericalHarmonicL2RGB x, float divider)
		{
			SphericalHarmonicL2RGB o = new SphericalHarmonicL2RGB();

			float scale = 1.0f / divider;
			o.Weighting = x.Weighting * scale;

			o.SH0.X = x.SH0.X * scale; o.SH0.Y = x.SH0.Y * scale; o.SH0.Z = x.SH0.Z * scale;
			o.SH1.X = x.SH1.X * scale; o.SH1.Y = x.SH1.Y * scale; o.SH1.Z = x.SH1.Z * scale;
			o.SH2.X = x.SH2.X * scale; o.SH2.Y = x.SH2.Y * scale; o.SH2.Z = x.SH2.Z * scale;
			o.SH3.X = x.SH3.X * scale; o.SH3.Y = x.SH3.Y * scale; o.SH3.Z = x.SH3.Z * scale;
			o.SH4.X = x.SH4.X * scale; o.SH4.Y = x.SH4.Y * scale; o.SH4.Z = x.SH4.Z * scale;
			o.SH5.X = x.SH5.X * scale; o.SH5.Y = x.SH5.Y * scale; o.SH5.Z = x.SH5.Z * scale;
			o.SH6.X = x.SH6.X * scale; o.SH6.Y = x.SH6.Y * scale; o.SH6.Z = x.SH6.Z * scale;
			o.SH7.X = x.SH7.X * scale; o.SH7.Y = x.SH7.Y * scale; o.SH7.Z = x.SH7.Z * scale;
			o.SH8.X = x.SH8.X * scale; o.SH8.Y = x.SH8.Y * scale; o.SH8.Z = x.SH8.Z * scale;

			return o;
		}

		/// <summary>
		/// <para>Add light from a given direction to the SH function. See the <see cref="SphericalHarmonicL2RGB"/> remarks for further details on how an SH can be used to approximate lighting.</para>
		/// <para>Input light (<paramref name="inputRGB"/>) will be multiplied by <paramref name="weight"/>, and <paramref name="weight"/> will be added to <see cref="Weighting"/></para>
		/// </summary>
		/// <param name="inputRGB">Input light intensity in RGB (usually gamma space) format for the given direction</param>
		/// <param name="normalisedDirection">Normalised (length must be one) direction of the incoming light</param>
		/// <param name="weight">Weighting for this light, usually 1.0f. Use this value, and <see cref="Weighting"/> if averaging a large number of lighting samples (eg, when converting a cube map to an SH)</param>
		public void AddLight(Vector3 inputRGB, Vector3 normalisedDirection, float weight)
		{
			AddLight(ref inputRGB, ref normalisedDirection, weight);
		}

		/// <summary>
		/// <para>Add light from a given direction to the SH function. See the <see cref="SphericalHarmonicL2RGB"/> remarks for further details on how an SH can be used to approximate lighting.</para>
		/// <para>Input light (<paramref name="inputRGB"/>) will be multiplied by <paramref name="weight"/>, and <paramref name="weight"/> will be added to <see cref="Weighting"/></para>
		/// </summary>
		/// <param name="inputRGB">Input light intensity in RGB (usually gamma space) format for the given direction</param>
		/// <param name="normalisedDirection">Normalised (length must be one) direction of the incoming light</param>
		/// <param name="weight">Weighting for this light, usually 1.0f. Use this value, and <see cref="Weighting"/> if averaging a large number of lighting samples (eg, when converting a cube map to an SH)</param>
		public void AddLight(ref Vector3 inputRGB, ref Vector3 normalisedDirection, float weight)
		{
			float x = normalisedDirection.X;
			float y = normalisedDirection.Y;
			float z = normalisedDirection.Z;

			float r = inputRGB.X * weight;
			float g = inputRGB.Y * weight;
			float b = inputRGB.Z * weight;

			//Spherical Harmonic constants
			const float f0 = 0.25f;
			const float f1 = 0.5f;
			const float f2 = 0.937500143128f;
			const float f3 = 0.234375035782f;

			//Axis constants for SH input
			float c1 = (f1 * (x));
			float c2 = (f1 * (y));
			float c3 = (f1 * (z));
			float c4 = (f2 * (x * y));
			float c5 = (f2 * (y * z));
			float c6 = (f2 * (x * z));
			float c7 = (f3 * (z * z - (1.0f / 3.0f)));
			float c8 = (f3 * (x * x - y * y));

			//Red channel
			SH0.X += r * f0;
			SH1.X += r * c1;
			SH2.X += r * c2;
			SH3.X += r * c3;
			SH4.X += r * c4;
			SH5.X += r * c5;
			SH6.X += r * c6;
			SH7.X += r * c7;
			SH8.X += r * c8;
			//Green channel
			SH0.Y += g * f0;
			SH1.Y += g * c1;
			SH2.Y += g * c2;
			SH3.Y += g * c3;
			SH4.Y += g * c4;
			SH5.Y += g * c5;
			SH6.Y += g * c6;
			SH7.Y += g * c7;
			SH8.Y += g * c8;
			//Blue channel
			SH0.Z += b * f0;
			SH1.Z += b * c1;
			SH2.Z += b * c2;
			SH3.Z += b * c3;
			SH4.Z += b * c4;
			SH5.Z += b * c5;
			SH6.Z += b * c6;
			SH7.Z += b * c7;
			SH8.Z += b * c8;

			//Store the accumulated weighting, useful if averaging the light input is desired.
			Weighting += weight;
		}

		//Sample the spherical harmonic
		/// <summary>
		/// Sample the SH in the given normalised (length = 1) direction. See <see cref="SphericalHarmonicL2RGB"/> remarks for further details.
		/// </summary>
		public Vector3 SampleDirection(Vector3 normalisedDirection)
		{
			float x = normalisedDirection.X;
			float y = normalisedDirection.Y;
			float z = normalisedDirection.Z;

			float xy = x * y;
			float yz = y * z;
			float xz = x * z;

			float zz3 = (z * z) - (1.0f / 3.0f);
			float xxyy = (x * x) - (y * y);


			float r =	SH0.X +
						SH1.X * x +
						SH2.X * y +
						SH3.X * z +
						SH4.X * xy +
						SH5.X * yz +
						SH6.X * xz +
						SH7.X * zz3 +
						SH8.X * xxyy;

			float g =	SH0.Y +
						SH1.Y * x +
						SH2.Y * y +
						SH3.Y * z +
						SH4.Y * xy +
						SH5.Y * yz +
						SH6.Y * xz +
						SH7.Y * zz3 +
						SH8.Y * xxyy;

			float b =	SH0.Z +
						SH1.Z * x +
						SH2.Z * y +
						SH3.Z * z +
						SH4.Z * xy +
						SH5.Z * yz +
						SH6.Z * xz +
						SH7.Z * zz3 +
						SH8.Z * xxyy;

			return new Vector3(r, g, b);
		}

		/// <summary>
		/// <para>Use this function after accumulating multiple lights with the AddLight method.</para>
		/// <para>This method averages all the calls to AddLight() based on the accumulated weighting, assuming they were light input over a sphere.</para>
		/// <para>For example, if generating a spherical harmonic from a cube map, treat each pixel as a light source by calling AddLight(), then call this method to get the average for entire the sphere.</para>
		/// </summary>
		/// <returns></returns>
		private SphericalHarmonicL2RGB GetWeightedAverageLightInputFromSphere()
		{
			//Average out the entire spherical harmonic.
			//The 4 is because the SH lighting input is being sampled over a cosine weighted hemisphere.
			//The hemisphere halves the divider, the cosine weighting halves it again.
			if (Weighting > 0)
				return this * (4.0f / Weighting);
			return this;
		}
		
		/// <summary>
		/// Generate a spherical harmonic from the faces of a cubemap, treating each pixel as a light source and averaging the result.
		/// </summary>
		public static SphericalHarmonicL2RGB GenerateSphericalHarmonicFromCubeMap(Vector3[][] colourDataFaces)
		{
			if (colourDataFaces == null)
				throw new ArgumentNullException();
			if (colourDataFaces.Length != 6)
				throw new ArgumentException("colourDataFaces.Length != 6");
			return GenerateSphericalHarmonicFromCubeMap(colourDataFaces[0], colourDataFaces[1], colourDataFaces[2], colourDataFaces[3], colourDataFaces[4], colourDataFaces[5]);
		}

		/// <summary>
		/// Generate a spherical harmonic from the faces of a cubemap, treating each pixel as a light source and averaging the result.
		/// </summary>
		public static SphericalHarmonicL2RGB GenerateSphericalHarmonicFromCubeMap(
					Vector3[] colourDataPositiveX,
					Vector3[] colourDataNegativeX,
					Vector3[] colourDataPositiveY,
					Vector3[] colourDataNegativeY,
					Vector3[] colourDataPositiveZ,
					Vector3[] colourDataNegativeZ)
		{
			if (colourDataPositiveX == null ||
				colourDataNegativeX == null ||
				colourDataPositiveY == null ||
				colourDataNegativeY == null ||
				colourDataPositiveZ == null ||
				colourDataNegativeZ == null)
				throw new ArgumentNullException();

			SphericalHarmonicL2RGB sh = new SphericalHarmonicL2RGB();

			Vector3[][] source = {
				colourDataPositiveX,
				colourDataNegativeX,
				colourDataPositiveY,
				colourDataNegativeY,
				colourDataPositiveZ,
				colourDataNegativeZ,
			};

			//extract the 6 faces of the cubemap.
			for (int face = 0; face < 6; face++)
			{
				int size = (int)(Math.Sqrt(source[face].Length) + 0.5);

				if (size * size != source[face].Length)
					throw new ArgumentException("Cubemap face is an unexpected (non square) size");

				Microsoft.Xna.Framework.Graphics.CubeMapFace faceId = (Microsoft.Xna.Framework.Graphics.CubeMapFace)face;

				//get the transformation for this face,
				Matrix cubeFaceMatrix;
				Xen.Graphics.DrawTargetTextureCube.GetCubeMapFaceMatrix(faceId, out cubeFaceMatrix);

				//extract the spherical harmonic for this face and accumulate it.
				sh += ExtractSphericalHarmonicForCubeFace(cubeFaceMatrix, source[face], size);
			}

			//average out over the sphere
			return sh.GetWeightedAverageLightInputFromSphere();
		}

		private static Xen.Ex.SphericalHarmonicL2RGB ExtractSphericalHarmonicForCubeFace(Matrix faceTransform, Vector3[] colourDataRGB, int faceSize)
		{
			Xen.Ex.SphericalHarmonicL2RGB sh = new Xen.Ex.SphericalHarmonicL2RGB();

			//For each pixel in the face, generate it's SH contribution.
			//Treat each pixel in the cube as a light source, which gets added to the SH.
			//This is used to generate an indirect lighting SH for the scene.

			//See the remarks in SphericalHarmonicL2RGB for more detals.

			float directionStep = 2.0f / (faceSize - 1.0f);
			int pixelIndex = 0;

			float dirY = 1.0f;
			for (int y = 0; y < faceSize; y++)
			{
				Xen.Ex.SphericalHarmonicL2RGB lineSh = new Xen.Ex.SphericalHarmonicL2RGB();
				float dirX = -1.0f;

				for (int x = 0; x < faceSize; x++)
				{
					//the direction to the pixel in the cube
					Vector3 direction = new Vector3(dirX, dirY, 1);
					Vector3.TransformNormal(ref direction, ref faceTransform, out direction);

					//length of the direction vector
					float length = direction.Length();
					//approximate area of the pixel (pixels close to the cube edges appear smaller when projected)
					float weight = 1.0f / length;

					//normalise:
					direction.X *= weight;
					direction.Y *= weight;
					direction.Z *= weight;

					//decode the RGBM colour
					Vector3 rgb = colourDataRGB[pixelIndex++];

					//Add it to the SH
					lineSh.AddLight(ref rgb, ref direction, weight);

					dirX += directionStep;
				}

				//average the SH
				if (lineSh.Weighting > 0)
					lineSh /= lineSh.Weighting;

				//add the line to the full SH
				//(SH is generated line by line to ease problems with floating point accuracy loss)
				sh += lineSh;

				dirY -= directionStep;
			}

			if (sh.Weighting > 0)
				sh /= sh.Weighting;

			return sh;
		}
	}


	/// <summary>
	/// <para>A structure storing a 4 constant (L1) RGB sperhical harmonic. See remarks for details.</para>
	/// <para>An L1 SH is less accurate than an L2 SH, but significantly faster to calculate and display.</para>
	/// </summary>
	/// <remarks><para>L1 and L2 Spherical harmonics are useful for storing approximate lighiting information.</para>
	/// <para>They work by storing the an approximation of of a lighting function over a sphere.<br/>
	/// Where as most approximations work in one or two dimensions (for example, approximations to sin/cos), a sherical harmonic is an approximation of an RGB (red/blue/green) function that maps to a sphere.<br/> 
	/// The most common use of an L1 or L2 spherical harmonic is to efficiently encode directional lighting information at a point. Another method to achieve this would be to use a Cube Map. 
	/// An SH works well as a substitute, as it typically will use much less data, and produces a smoother output.</para>
	/// <para>Spherical harmonics also have the advantage they can be efficiently added together, multiplied, lerped, etc.</para>
	/// <para>An example of how an SH can be used to store lighting information:<br/>
	/// In a game, it may be desirable that an object is lit by multiple nearby lights. However, it may be too inefficient to light the object with every light.<br/>
	/// An approximation to this lighting, would be to use a SH. Given the centre point of the object, if there are 8 lights nearby, then call <see cref="AddLight(Vector3, Vector3, float)"/> once for each light.
	/// This will store the approximate contribution of that light (for it's given direction) in the SH function. Sampling the SH (usually in the vertex shader) will produce an approximation for the input lights, however it will be much cheaper than sampling each light for every pixel.</para>
	/// <para>There are two ways to decode the SH, either as an array or a matrix:</para>
	/// <para>
	/// The following Shader code can be used to sample an SH stored in GPU matrix format (see <see cref="GetMatrix4x3"/>):<br/>
	/// <code>
	/// //storage for the Spherical Harmonic
	/// float4x3 SH;
	/// 
	/// float3 SampleSH(float3 normal)
	/// {
	///		//optional:
	///		normal = normalize(normal); // either make sure the input is normalised, or normalise here.
	///		
	///		return mul(float4(normal,1),SH);
	/// }
	/// </code></para></remarks>
	public struct SphericalHarmonicL1RGB
	{
		//Spherical harmonic storage.
		/// <summary>
		/// Spherical Harmonic RGB Constants
		/// </summary>
		public Vector4 Red, Blue, Green;

		/// <summary>
		/// Gets the L1 SH in a float4x3 compatible format for use in a shader
		/// </summary>
		/// <param name="value"></param>
		public void GetMatrix4x3(out Matrix value)
		{
			value = new Matrix();
			value.M11 = Red.X;
			value.M21 = Red.Y;
			value.M31 = Red.Z;
			value.M41 = Red.W;
			value.M12 = Green.X;
			value.M22 = Green.Y;
			value.M32 = Green.Z;
			value.M42 = Green.W;
			value.M13 = Blue.X;
			value.M23 = Blue.Y;
			value.M33 = Blue.Z;
			value.M43 = Blue.W;
		}

		/// <summary>
		/// Add two spherical harmonics together
		/// </summary>
		public static void Add(ref SphericalHarmonicL1RGB x, ref SphericalHarmonicL1RGB y, out SphericalHarmonicL1RGB result)
		{
			result = new SphericalHarmonicL1RGB();

			result.Red.X = x.Red.X + y.Red.X; result.Red.Y = x.Red.Y + y.Red.Y; result.Red.Z = x.Red.Z + y.Red.Z;
			result.Blue.X = x.Blue.X + y.Blue.X; result.Blue.Y = x.Blue.Y + y.Blue.Y; result.Blue.Z = x.Blue.Z + y.Blue.Z;
			result.Green.X = x.Green.X + y.Green.X; result.Green.Y = x.Green.Y + y.Green.Y; result.Green.Z = x.Green.Z + y.Green.Z;
		}

		/// <summary>
		/// Multiply a spherical harmonic by a constant scale factor
		/// </summary>
		public static void Multiply(ref SphericalHarmonicL1RGB x, float scale, out SphericalHarmonicL1RGB result)
		{
			result = new SphericalHarmonicL1RGB();

			result.Red.X = x.Red.X * scale; result.Red.Y = x.Red.Y * scale; result.Red.Z = x.Red.Z * scale;
			result.Blue.X = x.Blue.X * scale; result.Blue.Y = x.Blue.Y * scale; result.Blue.Z = x.Blue.Z * scale;
			result.Green.X = x.Green.X * scale; result.Green.Y = x.Green.Y * scale; result.Green.Z = x.Green.Z * scale;
		}

		/// <summary>
		/// Divide a spherical harmonic by a constant factor
		/// </summary>
		public static void Divide(ref SphericalHarmonicL1RGB x, float divider, out SphericalHarmonicL1RGB result)
		{
			result = new SphericalHarmonicL1RGB();

			float scale = 1.0f / divider;

			result.Red.X = x.Red.X * scale; result.Red.Y = x.Red.Y * scale; result.Red.Z = x.Red.Z * scale;
			result.Blue.X = x.Blue.X * scale; result.Blue.Y = x.Blue.Y * scale; result.Blue.Z = x.Blue.Z * scale;
			result.Green.X = x.Green.X * scale; result.Green.Y = x.Green.Y * scale; result.Green.Z = x.Green.Z * scale;
		}

		/// <summary>
		/// Linear interpolate (Lerp) between two spherical harmonics based on a interpolation factor
		/// </summary>
		/// <param name="factor">Determines the interpolation point. When factor is 1.0, the output will be <paramref name="x"/>, when factor is 0.0, the output will be <paramref name="y"/></param>
		/// <param name="result"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void Lerp(ref SphericalHarmonicL1RGB x, ref SphericalHarmonicL1RGB y, float factor, out SphericalHarmonicL1RGB result)
		{
			result = new SphericalHarmonicL1RGB();

			float xs = factor;
			float ys = 1.0f - factor;

			result.Red.X = xs * x.Red.X + ys * y.Red.X; result.Red.Y = xs * x.Red.Y + ys * y.Red.Y; result.Red.Z = xs * x.Red.Z + ys * y.Red.Z;
			result.Blue.X = xs * x.Blue.X + ys * y.Blue.X; result.Blue.Y = xs * x.Blue.Y + ys * y.Blue.Y; result.Blue.Z = xs * x.Blue.Z + ys * y.Blue.Z;
			result.Green.X = xs * x.Green.X + ys * y.Green.X; result.Green.Y = xs * x.Green.Y + ys * y.Green.Y; result.Green.Z = xs * x.Green.Z + ys * y.Green.Z;
		}

		/// <summary>
		/// Add two spherical harmonics together
		/// </summary>
		public static SphericalHarmonicL1RGB operator +(SphericalHarmonicL1RGB x, SphericalHarmonicL1RGB y)
		{
			SphericalHarmonicL1RGB o = new SphericalHarmonicL1RGB();

			o.Red.X = x.Red.X + y.Red.X; o.Red.Y = x.Red.Y + y.Red.Y; o.Red.Z = x.Red.Z + y.Red.Z;
			o.Blue.X = x.Blue.X + y.Blue.X; o.Blue.Y = x.Blue.Y + y.Blue.Y; o.Blue.Z = x.Blue.Z + y.Blue.Z;
			o.Green.X = x.Green.X + y.Green.X; o.Green.Y = x.Green.Y + y.Green.Y; o.Green.Z = x.Green.Z + y.Green.Z;

			return o;
		}

		/// <summary>
		/// Multiply a spherical harmonic by a constant scale factor
		/// </summary>
		public static SphericalHarmonicL1RGB operator *(SphericalHarmonicL1RGB x, float scale)
		{
			SphericalHarmonicL1RGB o = new SphericalHarmonicL1RGB();

			o.Red.X = x.Red.X * scale; o.Red.Y = x.Red.Y * scale; o.Red.Z = x.Red.Z * scale;
			o.Blue.X = x.Blue.X * scale; o.Blue.Y = x.Blue.Y * scale; o.Blue.Z = x.Blue.Z * scale;
			o.Green.X = x.Green.X * scale; o.Green.Y = x.Green.Y * scale; o.Green.Z = x.Green.Z * scale;

			return o;
		}

		/// <summary>
		/// Divide a spherical harmonic by a constant factor
		/// </summary>
		public static SphericalHarmonicL1RGB operator /(SphericalHarmonicL1RGB x, float divider)
		{
			SphericalHarmonicL1RGB o = new SphericalHarmonicL1RGB();

			float scale = 1.0f / divider;

			o.Red.X = x.Red.X * scale; o.Red.Y = x.Red.Y * scale; o.Red.Z = x.Red.Z * scale;
			o.Blue.X = x.Blue.X * scale; o.Blue.Y = x.Blue.Y * scale; o.Blue.Z = x.Blue.Z * scale;
			o.Green.X = x.Green.X * scale; o.Green.Y = x.Green.Y * scale; o.Green.Z = x.Green.Z * scale;

			return o;
		}

		/// <summary>
		/// <para>Add light from a given direction to the SH function. See the <see cref="SphericalHarmonicL1RGB"/> remarks for further details on how an SH can be used to approximate lighting.</para>
		/// </summary>
		/// <param name="inputRGB">Input light intensity in RGB (usually gamma space) format for the given direction</param>
		/// <param name="normalisedDirection">Normalised (length must be one) direction of the incoming light</param>
		/// <param name="weight">Scaling factor for the input light</param>
		public void AddLight(Vector3 inputRGB, Vector3 normalisedDirection, float weight)
		{
			AddLight(ref inputRGB, ref normalisedDirection, weight);
		}

		/// <summary>
		/// <para>Add light from a given direction to the SH function. See the <see cref="SphericalHarmonicL1RGB"/> remarks for further details on how an SH can be used to approximate lighting.</para>
		/// <para>Input light (<paramref name="inputRGB"/>) will be multiplied by <paramref name="weight"/></para>
		/// </summary>
		/// <param name="inputRGB">Input light intensity in RGB (usually gamma space) format for the given direction</param>
		/// <param name="normalisedDirection">Normalised (length must be one) direction of the incoming light</param>
		/// <param name="weight">Scaling factor for the input light</param>
		public void AddLight(ref Vector3 inputRGB, ref Vector3 normalisedDirection, float weight)
		{
			float x = normalisedDirection.X;
			float y = normalisedDirection.Y;
			float z = normalisedDirection.Z;

			weight *= 0.5f;

			float r = inputRGB.X * weight;
			float g = inputRGB.Y * weight;
			float b = inputRGB.Z * weight;

			Red.X += x * r;
			Red.Y += y * r;
			Red.Z += z * r;
			Red.W += r;

			Green.X += x * g;
			Green.Y += y * g;
			Green.Z += z * g;
			Green.W += g;

			Blue.X += x * b;
			Blue.Y += y * b;
			Blue.Z += z * b;
			Blue.W += b;
		}

		//Sample the spherical harmonic
		/// <summary>
		/// Sample the SH in the given normalised (length = 1) direction. See <see cref="SphericalHarmonicL1RGB"/> remarks for further details.
		/// </summary>
		public Vector3 SampleDirection(Vector3 normalisedDirection)
		{
			float x = normalisedDirection.X;
			float y = normalisedDirection.Y;
			float z = normalisedDirection.Z;

			float r = x * Red.X + y * Red.Y + z * Red.Z + Red.W;
			float g = x * Green.X + y * Green.Y + z * Green.Z + Green.W;
			float b = x * Blue.X + y * Blue.Y + z * Blue.Z + Blue.W;

			return new Vector3(r, g, b);
		}


		//This code is identical to the L2 SH code:

		/// <summary>
		/// Generate a spherical harmonic from the faces of a cubemap, treating each pixel as a light source and averaging the result.
		/// </summary>
		public static SphericalHarmonicL1RGB GenerateSphericalHarmonicFromCubeMap(Vector3[][] colourDataFaces)
		{
			if (colourDataFaces == null)
				throw new ArgumentNullException();
			if (colourDataFaces.Length != 6)
				throw new ArgumentException("colourDataFaces.Length != 6");
			return GenerateSphericalHarmonicFromCubeMap(colourDataFaces[0], colourDataFaces[1], colourDataFaces[2], colourDataFaces[3], colourDataFaces[4], colourDataFaces[5]);
		}

		/// <summary>
		/// Generate a spherical harmonic from the faces of a cubemap, treating each pixel as a light source and averaging the result.
		/// </summary>
		public static SphericalHarmonicL1RGB GenerateSphericalHarmonicFromCubeMap(
					Vector3[] colourDataPositiveX,
					Vector3[] colourDataNegativeX,
					Vector3[] colourDataPositiveY,
					Vector3[] colourDataNegativeY,
					Vector3[] colourDataPositiveZ,
					Vector3[] colourDataNegativeZ)
		{
			if (colourDataPositiveX == null ||
				colourDataNegativeX == null ||
				colourDataPositiveY == null ||
				colourDataNegativeY == null ||
				colourDataPositiveZ == null ||
				colourDataNegativeZ == null)
				throw new ArgumentNullException();

			SphericalHarmonicL1RGB sh = new SphericalHarmonicL1RGB();

			Vector3[][] source = {
				colourDataPositiveX,
				colourDataNegativeX,
				colourDataPositiveY,
				colourDataNegativeY,
				colourDataPositiveZ,
				colourDataNegativeZ,
			};

			//extract the 6 faces of the cubemap.
			for (int face = 0; face < 6; face++)
			{
				int size = (int)(Math.Sqrt(source[face].Length) + 0.5);

				if (size * size != source[face].Length)
					throw new ArgumentException("Cubemap face is an unexpected (non square) size");

				Microsoft.Xna.Framework.Graphics.CubeMapFace faceId = (Microsoft.Xna.Framework.Graphics.CubeMapFace)face;

				//get the transformation for this face,
				Matrix cubeFaceMatrix;
				Xen.Graphics.DrawTargetTextureCube.GetCubeMapFaceMatrix(faceId, out cubeFaceMatrix);

				//extract the spherical harmonic for this face and accumulate it.
				sh += ExtractSphericalHarmonicForCubeFace(cubeFaceMatrix, source[face], size);
			}

			//average out over the sphere. account for cosine weighting which doubles the output - but it's also over a full sphere not a hemisphere
			return sh / 3;
		}

		private static Xen.Ex.SphericalHarmonicL1RGB ExtractSphericalHarmonicForCubeFace(Matrix faceTransform, Vector3[] colourDataRGB, int faceSize)
		{
			Xen.Ex.SphericalHarmonicL1RGB sh = new Xen.Ex.SphericalHarmonicL1RGB();

			//For each pixel in the face, generate it's SH contribution.
			//Treat each pixel in the cube as a light source, which gets added to the SH.
			//This is used to generate an indirect lighting SH for the scene.

			//See the remarks in SphericalHarmonicL1RGB for more detals.

			float directionStep = 2.0f / (faceSize - 1.0f);
			int pixelIndex = 0;

			float dirY = 1.0f;
			for (int y = 0; y < faceSize; y++)
			{
				Xen.Ex.SphericalHarmonicL1RGB lineSh = new Xen.Ex.SphericalHarmonicL1RGB();
				float dirX = -1.0f;

				for (int x = 0; x < faceSize; x++)
				{
					//the direction to the pixel in the cube
					Vector3 direction = new Vector3(dirX, dirY, 1);
					Vector3.TransformNormal(ref direction, ref faceTransform, out direction);

					//length of the direction vector
					float length = direction.Length();
					//approximate area of the pixel (pixels close to the cube edges appear smaller when projected)
					float weight = 1.0f / length;

					//normalise:
					direction.X *= weight;
					direction.Y *= weight;
					direction.Z *= weight;

					//decode the RGBM colour
					Vector3 rgb = colourDataRGB[pixelIndex++];

					//Add it to the SH
					lineSh.AddLight(ref rgb, ref direction, weight);

					dirX += directionStep;
				}

				//average the SH
				lineSh /= (float)faceSize;

				//add the line to the full SH
				//(SH is generated line by line to ease problems with floating point accuracy loss)
				sh += lineSh;

				dirY -= directionStep;
			}

			if (faceSize > 0)
				sh /= (float)faceSize;

			return sh;
		}
	}
}
