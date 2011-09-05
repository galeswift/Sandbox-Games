using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace Xen.Ex.Compression
{
	//The following classes are used by the animation system to lossy compress animation data.
	//This was most useful in early versions of XNA, where ouput files were not compressed. This is less useful today.

	[Flags]
	enum TransformStorage : byte
	{
		ScaleChange = 1,
		TranslateChange = 2,
		RotationChange = 4,
		ScaleOne = 1,
		ScaleHalf = 8 | 1,
		TranslateDeltaNByte = 2,
		TranslateDeltaHalf = 16 | 2,
		RotationDeltaNByte = 4,
		RotationDeltaHalf = 32 | 4,
		Keyframe = 64,
		RepeatPrevious = 128
	}
	[Flags]
	enum TransformKeyframe : byte
	{
		ScaleChange = 1,
		TranslateChange = 2,
		RotationChange = 4,
	}

	/// <summary>
	/// Reads compressed <see cref="Transform"/> values from a stream written by a <see cref="CompressedTransformWriter"/>
	/// </summary>
	/// <remarks>
	/// <para>This object is not compatible with a <see cref="CompressedTransformAccelerationWriter"/> written stream</para>
	/// </remarks>
	public struct CompressedTransformReader
	{
		internal Transform value;
		private short repeats;
		private short initalised;

		/// <summary>
		/// Reset the reader to it's default state
		/// </summary>
		public void Reset()
		{
			this.value = new Transform();
			this.repeats = 0;
			this.initalised = 0;
		}

		/// <summary>
		/// Read the next value in the stream. Returns true if there is a new <see cref="Transform"/> avaliable
		/// </summary>
		/// <param name="reader"></param>
		/// <returns>Returns true if there is a new <see cref="Transform"/> avaliable</returns>
		/// <seealso cref="GetTransform"/>
		/// <remarks><para>Where possible, prefer using the byte[] version of this method</para></remarks>
		public bool MoveNext(BinaryReader reader)
		{
			//value hasn't changed
			if (repeats > 0)
			{
				repeats--;
				return true;
			}

			TransformStorage store = (TransformStorage)reader.ReadByte();

			if (store == 0)
				return false;

			if (initalised == 0)
			{
				initalised = 1;
				value = Transform.Identity;
			}

			//value hasn't changed
			if ((store & TransformStorage.RepeatPrevious) == TransformStorage.RepeatPrevious)
			{
				this.repeats = (short)((store & (~TransformStorage.RepeatPrevious)) - 1);
				return true;
			}

			//read compressed data/deltas

			//scale changed?
			if ((store & TransformStorage.ScaleChange) == TransformStorage.ScaleChange)
			{
				if ((store & TransformStorage.ScaleHalf) == TransformStorage.ScaleHalf)
				{
					//scale is stored as a HalfFloat
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle value =
						new Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle();

					value.PackedValue = reader.ReadUInt16();
					this.value.Scale = value.ToSingle();
				}
				else
				{
					//scale is 1
					this.value.Scale = 1;
				}
			}

			//transform delta?
			if ((store & TransformStorage.TranslateChange) == TransformStorage.TranslateChange)
			{
				Vector4 delta4;

				if ((store & TransformStorage.TranslateDeltaHalf) == TransformStorage.TranslateDeltaHalf)
				{
					//delta stored as half in 6 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4();

#if !XBOX360
					delta.PackedValue = (ulong)reader.ReadUInt32() | ((ulong)reader.ReadUInt16() << 32);
#else
					delta.PackedValue = (ulong)reader.ReadUInt16() | ((ulong)reader.ReadUInt32() << 16);
#endif
					delta4 = delta.ToVector4();
				}
				else
				{
					//delta stored normalised, in 3 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4();

#if !XBOX360
					delta.PackedValue = reader.ReadUInt16() | ((uint)reader.ReadByte() << 16);
#else
					delta.PackedValue = (uint)reader.ReadByte() | ((uint)reader.ReadUInt16() << 8);
#endif
					delta4 = delta.ToVector4();
				}

				this.value.Translation.X += delta4.X;
				this.value.Translation.Y += delta4.Y;
				this.value.Translation.Z += delta4.Z;
			}

			//rotation delta?
			if ((store & TransformStorage.RotationChange) == TransformStorage.RotationChange)
			{
				Quaternion rotation;
				Vector4 delta4;

				if ((store & TransformStorage.RotationDeltaHalf) == TransformStorage.RotationDeltaHalf)
				{
					//delta stored as normalised short in 8 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4();

					delta.PackedValue = reader.ReadUInt64();
					delta4 = delta.ToVector4();
				}
				else
				{
					//delta stored as normalised in 4 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4();

					delta.PackedValue = reader.ReadUInt32();
					delta4 = delta.ToVector4();
				}

				rotation = new Quaternion(delta4.X, delta4.Y, delta4.Z, delta4.W);
				Quaternion.Multiply(ref rotation, ref this.value.Rotation, out this.value.Rotation);
				float length =
					this.value.Rotation.X * this.value.Rotation.X +
					this.value.Rotation.Y * this.value.Rotation.Y +
					this.value.Rotation.Z * this.value.Rotation.Z +
					this.value.Rotation.W * this.value.Rotation.W;
				if (length > 1.0001f || length < 0.9999f)
				{
					//normalize
					length = 1.0f / (float)Math.Sqrt(length);
					this.value.Rotation.X *= length;
					this.value.Rotation.Y *= length;
					this.value.Rotation.Z *= length;
					this.value.Rotation.W *= length;
				}
			}

			if ((store & TransformStorage.Keyframe) == TransformStorage.Keyframe)
			{
				//read exact float data...
				TransformKeyframe keyframe = (TransformKeyframe)reader.ReadByte();

				if ((keyframe & TransformKeyframe.ScaleChange) == TransformKeyframe.ScaleChange)
				{
					this.value.Scale = reader.ReadSingle();
				}

				if ((keyframe & TransformKeyframe.TranslateChange) == TransformKeyframe.TranslateChange)
				{
					this.value.Translation.X = reader.ReadSingle();
					this.value.Translation.Y = reader.ReadSingle();
					this.value.Translation.Z = reader.ReadSingle();
				}

				if ((keyframe & TransformKeyframe.RotationChange) == TransformKeyframe.RotationChange)
				{
					this.value.Rotation.X = reader.ReadSingle();
					this.value.Rotation.Y = reader.ReadSingle();
					this.value.Rotation.Z = reader.ReadSingle();
					this.value.Rotation.W = reader.ReadSingle();
				}
			}

#if DEBUG
			this.value.Validate();
#endif
			return true;
		}

		/// <summary>
		/// Read the next value from a data array. Returns true if there is a new <see cref="Transform"/> avaliable
		/// </summary>
		/// <param name="sourceData"></param>
		/// <returns>Returns true if there is a new <see cref="Transform"/> avaliable</returns>
		/// <param name="index">index in the source data to begin reading (this value will be modified to indicated how many bytes were read)</param>
		/// <remarks><para>This method will most likely be more efficient than the <see cref="BinaryReader"/> version</para></remarks>
		/// <seealso cref="GetTransform"/>
		public bool MoveNext(byte[] sourceData, ref int index)
		{
			BitCast cast = new BitCast();

			//value hasn't changed
			if (repeats > 0)
			{
				repeats--;
				return true;
			}

			TransformStorage store = (TransformStorage)sourceData[index++];

			if (store == 0)
				return false;

			if (initalised == 0)
			{
				initalised = 1;
				value = Transform.Identity;
			}

			//value hasn't changed
			if ((store & TransformStorage.RepeatPrevious) == TransformStorage.RepeatPrevious)
			{
				this.repeats = (short)((store & (~TransformStorage.RepeatPrevious)) - 1);
				return true;
			}

			//read compressed data/deltas

			//scale changed?
			if ((store & TransformStorage.ScaleChange) == TransformStorage.ScaleChange)
			{
				if ((store & TransformStorage.ScaleHalf) == TransformStorage.ScaleHalf)
				{
					//scale is stored as a HalfFloat
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle value =
						new Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle();

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];

					value.PackedValue = cast.UInt16;
					this.value.Scale = value.ToSingle();
				}
				else
				{
					//scale is 1
					this.value.Scale = 1;
				}
			}

			//transform delta?
			if ((store & TransformStorage.TranslateChange) == TransformStorage.TranslateChange)
			{
				Vector4 delta4;

				if ((store & TransformStorage.TranslateDeltaHalf) == TransformStorage.TranslateDeltaHalf)
				{
					//delta stored as half in 6 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4();

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];
					cast.Byte4 = sourceData[index++];
					cast.Byte5 = sourceData[index++];
					cast.Byte6 = 0;
					cast.Byte7 = 0;

					delta.PackedValue = cast.UInt64;

					delta4 = delta.ToVector4();
				}
				else
				{
					//delta stored normalised, in 3 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4();

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = 0;

					delta.PackedValue = cast.UInt32;

					delta4 = delta.ToVector4();
				}

				this.value.Translation.X += delta4.X;
				this.value.Translation.Y += delta4.Y;
				this.value.Translation.Z += delta4.Z;
			}

			//rotation delta?
			if ((store & TransformStorage.RotationChange) == TransformStorage.RotationChange)
			{
				Quaternion rotation;
				Vector4 delta4;

				if ((store & TransformStorage.RotationDeltaHalf) == TransformStorage.RotationDeltaHalf)
				{
					//delta stored as normalised short in 8 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4();

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];
					cast.Byte4 = sourceData[index++];
					cast.Byte5 = sourceData[index++];
					cast.Byte6 = sourceData[index++];
					cast.Byte7 = sourceData[index++];

					delta.PackedValue = cast.UInt64;

					delta4 = delta.ToVector4();
				}
				else
				{
					//delta stored as normalised in 4 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4();

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					delta.PackedValue = cast.UInt32;

					delta4 = delta.ToVector4();
				}

				rotation = new Quaternion(delta4.X, delta4.Y, delta4.Z, delta4.W);
				Quaternion.Multiply(ref rotation, ref this.value.Rotation, out this.value.Rotation);
				float length =
					this.value.Rotation.X * this.value.Rotation.X +
					this.value.Rotation.Y * this.value.Rotation.Y +
					this.value.Rotation.Z * this.value.Rotation.Z +
					this.value.Rotation.W * this.value.Rotation.W;
				if (length > 1.0001f || length < 0.9999f)
				{
					//normalize
					length = 1.0f / (float)Math.Sqrt(length);
					this.value.Rotation.X *= length;
					this.value.Rotation.Y *= length;
					this.value.Rotation.Z *= length;
					this.value.Rotation.W *= length;
				}
			}

			if ((store & TransformStorage.Keyframe) == TransformStorage.Keyframe)
			{
				//read exact float data...
				TransformKeyframe keyframe = (TransformKeyframe)sourceData[index++];

				if ((keyframe & TransformKeyframe.ScaleChange) == TransformKeyframe.ScaleChange)
				{
					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Scale = cast.Single;
				}

				if ((keyframe & TransformKeyframe.TranslateChange) == TransformKeyframe.TranslateChange)
				{
					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Translation.X = cast.Single;

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Translation.Y = cast.Single;

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Translation.Z = cast.Single;
				}

				if ((keyframe & TransformKeyframe.RotationChange) == TransformKeyframe.RotationChange)
				{
					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Rotation.X = cast.Single;

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Rotation.Y = cast.Single;

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Rotation.Z = cast.Single;

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Rotation.W = cast.Single;
				}
			}
#if DEBUG
			this.value.Validate();
#endif
			return true;
		}
		/// <summary>
		/// Get the most recently read transform
		/// </summary>
		/// <param name="transform"></param>
		public void GetTransform(out Transform transform)
		{
			transform = value;
		}
	}

	/// <summary>
	/// Reads compressed <see cref="Transform"/> values from a stream written by a <see cref="CompressedTransformAccelerationWriter"/>
	/// </summary>
	/// <remarks>
	/// <para>This object is not compatible with a <see cref="CompressedTransformWriter"/> written stream</para>
	/// <para>The <see cref="CompressedTransformAccelerationWriter"/> and <see cref="CompressedTransformAccelerationReader"/> have higher overhead than the <see cref="CompressedTransformWriter"/> and <see cref="CompressedTransformReader"/></para>
	/// </remarks>
	public struct CompressedTransformAccelerationReader
	{
		internal Transform value;
		private Quaternion accelerationR;
		private Vector3 accelerationT;
		private short repeats;
		private short initalised;


		/// <summary>
		/// Reset the reader to it's default state
		/// </summary>
		public void Reset()
		{
			this.value = new Transform();
			this.repeats = 0;
			this.initalised = 0;
			this.accelerationT = new Vector3();
			this.accelerationR = new Quaternion();
		}
		/// <summary>
		/// Read the next value in the stream. Returns true if there is a new <see cref="Transform"/> avaliable
		/// </summary>
		/// <param name="reader"></param>
		/// <returns>Returns true if there is a new <see cref="Transform"/> avaliable</returns>
		/// <seealso cref="GetTransform"/>
		/// <remarks><para>Where possible, prefer using the byte[] version of this method</para></remarks>
		public bool MoveNext(BinaryReader reader)
		{
			//value hasn't changed
			if (repeats > 0)
			{
				repeats--;

				Quaternion.Multiply(ref this.accelerationR, ref this.value.Rotation, out this.value.Rotation);
				float length =
					this.value.Rotation.X * this.value.Rotation.X +
					this.value.Rotation.Y * this.value.Rotation.Y +
					this.value.Rotation.Z * this.value.Rotation.Z +
					this.value.Rotation.W * this.value.Rotation.W;
				if (length > 1.0001f || length < 0.9999f)
				{
					//normalize
					length = 1.0f / (float)Math.Sqrt(length);
					this.value.Rotation.X *= length;
					this.value.Rotation.Y *= length;
					this.value.Rotation.Z *= length;
					this.value.Rotation.W *= length;
				}
				this.value.Translation.X += this.accelerationT.X;
				this.value.Translation.Y += this.accelerationT.Y;
				this.value.Translation.Z += this.accelerationT.Z;

#if DEBUG
				this.value.Validate();
#endif
				return true;
			}

			TransformStorage store = (TransformStorage)reader.ReadByte();

			if (store == 0)
				return false;

			if (initalised == 0)
			{
				initalised = 1;
				value = Transform.Identity;
				accelerationR = Quaternion.Identity;
			}

			//value hasn't changed
			if ((store & TransformStorage.RepeatPrevious) == TransformStorage.RepeatPrevious)
			{
				this.repeats = (short)((store & (~TransformStorage.RepeatPrevious)) - 1);

				Quaternion.Multiply(ref this.accelerationR, ref this.value.Rotation, out this.value.Rotation);
				float length =
					this.value.Rotation.X * this.value.Rotation.X +
					this.value.Rotation.Y * this.value.Rotation.Y +
					this.value.Rotation.Z * this.value.Rotation.Z +
					this.value.Rotation.W * this.value.Rotation.W;
				if (length > 1.0001f || length < 0.9999f)
				{
					//normalize
					length = 1.0f / (float)Math.Sqrt(length);
					this.value.Rotation.X *= length;
					this.value.Rotation.Y *= length;
					this.value.Rotation.Z *= length;
					this.value.Rotation.W *= length;
				}
				this.value.Translation.X += this.accelerationT.X;
				this.value.Translation.Y += this.accelerationT.Y;
				this.value.Translation.Z += this.accelerationT.Z;

#if DEBUG
				this.value.Validate();
#endif
				return true;
			}

			//read compressed data/deltas

			//scale changed?
			if ((store & TransformStorage.ScaleChange) == TransformStorage.ScaleChange)
			{
				if ((store & TransformStorage.ScaleHalf) == TransformStorage.ScaleHalf)
				{
					//scale is stored as a HalfFloat
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle value =
						new Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle();

					value.PackedValue = reader.ReadUInt16();
					this.value.Scale = value.ToSingle();
				}
				else
				{
					//scale is 1
					this.value.Scale = 1;
				}
			}

			//transform delta?
			if ((store & TransformStorage.TranslateChange) == TransformStorage.TranslateChange)
			{
				Vector4 delta4;

				if ((store & TransformStorage.TranslateDeltaHalf) == TransformStorage.TranslateDeltaHalf)
				{
					//delta stored as half in 6 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4();

#if !XBOX360
					delta.PackedValue = (ulong)reader.ReadUInt32() | ((ulong)reader.ReadUInt16() << 32);
#else
					delta.PackedValue = (ulong)reader.ReadUInt16() | ((ulong)reader.ReadUInt32() << 16);
#endif
					delta4 = delta.ToVector4();
				}
				else
				{
					//delta stored normalised, in 3 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4();

#if !XBOX360
					delta.PackedValue = reader.ReadUInt16() | ((uint)reader.ReadByte() << 16);
#else
					delta.PackedValue = (uint)reader.ReadByte() | ((uint)reader.ReadUInt16() << 8);
#endif
					delta4 = delta.ToVector4();
				}

				this.accelerationT.X += delta4.X;
				this.accelerationT.Y += delta4.Y;
				this.accelerationT.Z += delta4.Z;
			}

			//rotation delta?
			if ((store & TransformStorage.RotationChange) == TransformStorage.RotationChange)
			{
				Quaternion rotation;
				Vector4 delta4;

				if ((store & TransformStorage.RotationDeltaHalf) == TransformStorage.RotationDeltaHalf)
				{
					//delta stored as normalised short in 8 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4();

					delta.PackedValue = reader.ReadUInt64();
					delta4 = delta.ToVector4();
				}
				else
				{
					//delta stored as normalised in 4 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4();

					delta.PackedValue = reader.ReadUInt32();
					delta4 = delta.ToVector4();
				}

				rotation = new Quaternion(delta4.X, delta4.Y, delta4.Z, delta4.W);
				Quaternion.Multiply(ref rotation, ref this.accelerationR, out this.accelerationR);
			}

			{
				Quaternion.Multiply(ref this.accelerationR, ref this.value.Rotation, out this.value.Rotation);
				float length =
					this.value.Rotation.X * this.value.Rotation.X +
					this.value.Rotation.Y * this.value.Rotation.Y +
					this.value.Rotation.Z * this.value.Rotation.Z +
					this.value.Rotation.W * this.value.Rotation.W;
				if (length > 1.0001f || length < 0.9999f)
				{
					//normalize
					length = 1.0f / (float)Math.Sqrt(length);
					this.value.Rotation.X *= length;
					this.value.Rotation.Y *= length;
					this.value.Rotation.Z *= length;
					this.value.Rotation.W *= length;
				}
				this.value.Translation.X += this.accelerationT.X;
				this.value.Translation.Y += this.accelerationT.Y;
				this.value.Translation.Z += this.accelerationT.Z;
			}

			if ((store & TransformStorage.Keyframe) == TransformStorage.Keyframe)
			{
				//read exact float data...
				TransformKeyframe keyframe = (TransformKeyframe)reader.ReadByte();

				if ((keyframe & TransformKeyframe.ScaleChange) == TransformKeyframe.ScaleChange)
				{
					this.value.Scale = reader.ReadSingle();
				}

				if ((keyframe & TransformKeyframe.TranslateChange) == TransformKeyframe.TranslateChange)
				{
					this.value.Translation.X = reader.ReadSingle();
					this.value.Translation.Y = reader.ReadSingle();
					this.value.Translation.Z = reader.ReadSingle();
				}

				if ((keyframe & TransformKeyframe.RotationChange) == TransformKeyframe.RotationChange)
				{
					this.value.Rotation.X = reader.ReadSingle();
					this.value.Rotation.Y = reader.ReadSingle();
					this.value.Rotation.Z = reader.ReadSingle();
					this.value.Rotation.W = reader.ReadSingle();
				}
			}
#if DEBUG
			this.value.Validate();
#endif
			return true;
		}

		/// <summary>
		/// Read the next value from a data array. Returns true if there is a new <see cref="Transform"/> avaliable
		/// </summary>
		/// <returns>Returns true if there is a new <see cref="Transform"/> avaliable</returns>
		/// <param name="index">index in the source data to begin reading (this value will be modified to indicated how many bytes were read)</param>
		/// <remarks><para>This method will most likely be more efficient than the <see cref="BinaryReader"/> version</para></remarks>
		/// <param name="sourceData"></param>
		/// <seealso cref="GetTransform"/>
		public bool MoveNext(byte[] sourceData, ref int index)
		{
			BitCast cast = new BitCast();

			//value hasn't changed
			if (repeats > 0)
			{
				repeats--;

				Quaternion.Multiply(ref this.accelerationR, ref this.value.Rotation, out this.value.Rotation);
				float length =
					this.value.Rotation.X * this.value.Rotation.X +
					this.value.Rotation.Y * this.value.Rotation.Y +
					this.value.Rotation.Z * this.value.Rotation.Z +
					this.value.Rotation.W * this.value.Rotation.W;
				if (length > 1.0001f || length < 0.9999f)
				{
					//normalize
					length = 1.0f / (float)Math.Sqrt(length);
					this.value.Rotation.X *= length;
					this.value.Rotation.Y *= length;
					this.value.Rotation.Z *= length;
					this.value.Rotation.W *= length;
				}
				this.value.Translation.X += this.accelerationT.X;
				this.value.Translation.Y += this.accelerationT.Y;
				this.value.Translation.Z += this.accelerationT.Z;

#if DEBUG
				this.value.Validate();
#endif
				return true;
			}

			TransformStorage store = (TransformStorage)sourceData[index++];

			if (store == 0)
				return false;

			if (initalised == 0)
			{
				initalised = 1;
				value = Transform.Identity;
				accelerationR = Quaternion.Identity;
			}

			//value hasn't changed
			if ((store & TransformStorage.RepeatPrevious) == TransformStorage.RepeatPrevious)
			{
				this.repeats = (short)((store & (~TransformStorage.RepeatPrevious)) - 1);

				Quaternion.Multiply(ref this.accelerationR, ref this.value.Rotation, out this.value.Rotation);
				float length =
					this.value.Rotation.X * this.value.Rotation.X +
					this.value.Rotation.Y * this.value.Rotation.Y +
					this.value.Rotation.Z * this.value.Rotation.Z +
					this.value.Rotation.W * this.value.Rotation.W;
				if (length > 1.0001f || length < 0.9999f)
				{
					//normalize
					length = 1.0f / (float)Math.Sqrt(length);
					this.value.Rotation.X *= length;
					this.value.Rotation.Y *= length;
					this.value.Rotation.Z *= length;
					this.value.Rotation.W *= length;
				}
				this.value.Translation.X += this.accelerationT.X;
				this.value.Translation.Y += this.accelerationT.Y;
				this.value.Translation.Z += this.accelerationT.Z;

#if DEBUG
				this.value.Validate();
#endif
				return true;
			}

			//read compressed data/deltas

			//scale changed?
			if ((store & TransformStorage.ScaleChange) == TransformStorage.ScaleChange)
			{
				if ((store & TransformStorage.ScaleHalf) == TransformStorage.ScaleHalf)
				{
					//scale is stored as a HalfFloat
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle value =
						new Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle();

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];

					value.PackedValue = cast.UInt16;
					this.value.Scale = value.ToSingle();
				}
				else
				{
					//scale is 1
					this.value.Scale = 1;
				}
			}

			//transform delta?
			if ((store & TransformStorage.TranslateChange) == TransformStorage.TranslateChange)
			{
				Vector4 delta4;

				if ((store & TransformStorage.TranslateDeltaHalf) == TransformStorage.TranslateDeltaHalf)
				{
					//delta stored as half in 6 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4();

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];
					cast.Byte4 = sourceData[index++];
					cast.Byte5 = sourceData[index++];
					cast.Byte6 = 0;
					cast.Byte7 = 0;

					delta.PackedValue = cast.UInt64;

					delta4 = delta.ToVector4();
				}
				else
				{
					//delta stored normalised, in 3 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4();

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = 0;

					delta.PackedValue = cast.UInt32;

					delta4 = delta.ToVector4();
				}

				this.accelerationT.X += delta4.X;
				this.accelerationT.Y += delta4.Y;
				this.accelerationT.Z += delta4.Z;
			}

			//rotation delta?
			if ((store & TransformStorage.RotationChange) == TransformStorage.RotationChange)
			{
				Quaternion rotation;
				Vector4 delta4;

				if ((store & TransformStorage.RotationDeltaHalf) == TransformStorage.RotationDeltaHalf)
				{
					//delta stored as normalised short in 8 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4();

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];
					cast.Byte4 = sourceData[index++];
					cast.Byte5 = sourceData[index++];
					cast.Byte6 = sourceData[index++];
					cast.Byte7 = sourceData[index++];

					delta.PackedValue = cast.UInt64;

					delta4 = delta.ToVector4();
				}
				else
				{
					//delta stored as normalised in 4 bytes
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 delta
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4();

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					delta.PackedValue = cast.UInt32;

					delta4 = delta.ToVector4();
				}

				rotation = new Quaternion(delta4.X, delta4.Y, delta4.Z, delta4.W);
				Quaternion.Multiply(ref rotation, ref this.accelerationR, out this.accelerationR);
			}

			{
				Quaternion.Multiply(ref this.accelerationR, ref this.value.Rotation, out this.value.Rotation);
				float length =
					this.value.Rotation.X * this.value.Rotation.X +
					this.value.Rotation.Y * this.value.Rotation.Y +
					this.value.Rotation.Z * this.value.Rotation.Z +
					this.value.Rotation.W * this.value.Rotation.W;
				if (length > 1.0001f || length < 0.9999f)
				{
					//normalize
					length = 1.0f / (float)Math.Sqrt(length);
					this.value.Rotation.X *= length;
					this.value.Rotation.Y *= length;
					this.value.Rotation.Z *= length;
					this.value.Rotation.W *= length;
				}
				this.value.Translation.X += this.accelerationT.X;
				this.value.Translation.Y += this.accelerationT.Y;
				this.value.Translation.Z += this.accelerationT.Z;
			}

			if ((store & TransformStorage.Keyframe) == TransformStorage.Keyframe)
			{
				//read exact float data...
				TransformKeyframe keyframe = (TransformKeyframe)sourceData[index++];

				if ((keyframe & TransformKeyframe.ScaleChange) == TransformKeyframe.ScaleChange)
				{
					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Scale = cast.Single;
				}

				if ((keyframe & TransformKeyframe.TranslateChange) == TransformKeyframe.TranslateChange)
				{
					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Translation.X = cast.Single;

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Translation.Y = cast.Single;

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Translation.Z = cast.Single;
				}

				if ((keyframe & TransformKeyframe.RotationChange) == TransformKeyframe.RotationChange)
				{
					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Rotation.X = cast.Single;

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Rotation.Y = cast.Single;

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Rotation.Z = cast.Single;

					cast.Byte0 = sourceData[index++];
					cast.Byte1 = sourceData[index++];
					cast.Byte2 = sourceData[index++];
					cast.Byte3 = sourceData[index++];

					this.value.Rotation.W = cast.Single;
				}
			}
#if DEBUG
			this.value.Validate();
#endif
			return true;
		}
		/// <summary>
		/// Get the most recently read transform
		/// </summary>
		/// <param name="transform"></param>
		public void GetTransform(out Transform transform)
		{
			transform = value;
		}
	}

	/// <summary>
	/// <para>Writes a series of <see cref="Transform"/> structures to a binary stream, using lossy/lossless data compression</para>
	/// <para>Prefer using this method of compression over <see cref="CompressedTransformAccelerationWriter"/> for input data with rapidly changing, less predictable movement (commonly for animation data)</para>
	/// <para>Redundant values are skipped and packed data types are used where possible</para>
	/// <para>Compression levels are based on specified compression tolerances</para>
	/// </summary>
	/// <remarks>
	/// <para>Tolerance values may be changed at will, this will not corrupt data already written/about to be written</para>
	/// </remarks>
	public sealed class CompressedTransformWriter
	{
		#region members

		private float scaleTolerance;
		private float translateTolerance;
		private float rotateTolerance;
		private float cos2RotateTolerance;
		private Transform previous;
		private int repeat;
		private readonly byte[] writeBuffer;

		#endregion

		#region tolerances

		/// <summary>
		/// Gets/Sets the scale tolerance. This is the amount the scale needs to change for a new value to be compressed to the stream. Set to 0 for lossless writing
		/// </summary>
		public float ScaleTolerance
		{
			get { return scaleTolerance; }
			set
			{
				if (value < 0)
					throw new ArgumentException();
				scaleTolerance = value;
			}
		}
		/// <summary>
		/// Gets/Sets the translate tolerance. This is the amount an translate x/y/z needs to change for a new value to be compressed to the stream. Set to 0 for lossless writing
		/// </summary>
		public float TranslateTolerance
		{
			get { return translateTolerance; }
			set
			{
				if (value < 0)
					throw new ArgumentException();
				translateTolerance = value;
			}
		}
		/// <summary>
		/// Gets/Sets the rotation tolerance. Rotations are stored as normalised vectors. This tolerance is for the change in a component of the vector. The maximum value is 0.5. However this would produce very infrequent rotations. 
		/// </summary>
		public float RotationTolerance
		{
			get { return rotateTolerance; }
			set
			{
				if (value < 0 || value > 0.5f)
					throw new ArgumentException();
				rotateTolerance = value;
				cos2RotateTolerance = (float)Math.Cos(value * 0.5f);
			}
		}

		#endregion

		/// <summary>
		/// <para>Construct the writer with default values of:</para>
		/// <para>ScaleTolerance = 0.025</para>
		/// <para>TranslateTolerance = 0.01</para>
		/// <para>RotationTolerance = 0.001</para>
		/// </summary>
		public CompressedTransformWriter()
		{
			ScaleTolerance = 0.025f;
			TranslateTolerance = 0.01f;
			RotationTolerance = 0.001f;
			this.previous = Transform.Identity;
			writeBuffer = new byte[16];
		}
		/// <summary>
		/// Construct the writer
		/// </summary>
		/// <param name="scaleTolerance"></param>
		/// <param name="translateTolerance"></param>
		/// <param name="rotationTolerance"></param>
		public CompressedTransformWriter(float scaleTolerance, float translateTolerance, float rotationTolerance)
		{
			this.ScaleTolerance = scaleTolerance;
			this.TranslateTolerance = translateTolerance;
			this.RotationTolerance = rotationTolerance;
			this.previous = Transform.Identity;
			writeBuffer = new byte[16];
		}

		/// <summary>
		/// Writes the entire transformation uncompressed. Normally the change in transform is written, relying on the previous value. (Writes 34 bytes)
		/// </summary>
		/// <remarks><para>This method can be used to 'reset' the compression stream.</para></remarks>
		/// <param name="transform"></param>
		/// <param name="writer"></param>
		/// <returns>Returns the number of bytes written</returns>
		public int WriteUncompressed(ref Transform transform, BinaryWriter writer)
		{
			writer.Write((byte)TransformStorage.Keyframe);
			writer.Write((byte)(TransformKeyframe.RotationChange | TransformKeyframe.ScaleChange | TransformKeyframe.TranslateChange));

			writer.Write(transform.Scale);
			writer.Write(transform.Translation.X);
			writer.Write(transform.Translation.Y);
			writer.Write(transform.Translation.Z);
			writer.Write(transform.Rotation.X);
			writer.Write(transform.Rotation.Y);
			writer.Write(transform.Rotation.Z);
			writer.Write(transform.Rotation.W);
			previous = transform;

			repeat = 0;
			return 34;
		}
		/// <summary>
		/// Writes the entire transformation uncompressed. Normally the change in transform is written, relying on the previous value. Returns the number of bytes written (which is always 34 for this method)
		/// </summary>
		/// <remarks><para>This method can be used to 'reset' the compression stream.</para></remarks>
		/// <param name="transform"></param>
		/// <param name="writer"></param>
		/// <returns>Returns the number of bytes written</returns>
		public int WriteUncompressed(Transform transform, BinaryWriter writer)
		{
			return WriteUncompressed(ref transform, writer);
		}

		/// <summary>
		/// <para>Write the final bytes that are needed to complete writing a series of transforms. Returns the number of bytes that were written</para>
		/// </summary>
		/// <param name="writer"></param>
		/// <returns>Returns the number of bytes written</returns>
		public int EndWriting(BinaryWriter writer)
		{
			this.previous = Transform.Identity;
			if (repeat != 0)
			{
				writer.Write((byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat));
				writer.Write((byte)0);
				repeat = 0;
				return 2;
			}
			writer.Write((byte)0);
			return 1;
		}

		/// <summary>
		/// <para>Write the final bytes that are needed to complete writing a series of transforms. Returns the number of bytes that were written</para>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="writeTarget"></param>
		/// <returns>Returns the number of bytes written</returns>
		public int EndWriting(byte[] writeTarget, ref int index)
		{
			this.previous = Transform.Identity;
			if (repeat != 0)
			{
				writeTarget[index++] = (byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat);
				writeTarget[index++] = (byte)0;
				repeat = 0;
				return 2;
			}
			writeTarget[index++] = (byte)0;
			return 1;
		}

		/// <summary>
		/// Write the transform to the stream. Returns the number of bytes, if any, that were written
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="writer"></param>
		/// <param name="forceWrite"><para>If true, the method call will always write at least 1 byte</para><para>When false, multiple identical transforms may be represented with a write of a single byte at the end of the sequence</para></param>
		/// <returns>Returns the number of bytes written</returns>
		public int Write(ref Transform transform, BinaryWriter writer, bool forceWrite)
		{
			int bufferIndex = 0;
			TransformStorage store = 0;
			TransformKeyframe keyframe = 0;

			//if the scale has changed a considerable amount...
			if (Math.Abs(previous.Scale - transform.Scale) > scaleTolerance)
			{
				if (transform.Scale >= 1 - scaleTolerance && transform.Scale <= 1 + scaleTolerance)
				{
					store |= TransformStorage.ScaleOne | TransformStorage.ScaleChange;

					previous.Scale = 1;
				}
				else
				{
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle value =
						new Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle(transform.Scale);
					float valuef = value.ToSingle();

					if (Math.Abs(transform.Scale - valuef) <= scaleTolerance)
					{
						ushort pack = value.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);

						store |= TransformStorage.ScaleHalf | TransformStorage.ScaleChange;

						previous.Scale = valuef;
					}
					else
					{
						//need full precision
						keyframe |= TransformKeyframe.ScaleChange;
						store |= TransformStorage.Keyframe;
					}
				}
			}


			{
				Vector3 deltaTranslation = new Vector3(
					(transform.Translation.X - previous.Translation.X),
					(transform.Translation.Y - previous.Translation.Y),
					(transform.Translation.Z - previous.Translation.Z));

				if (Math.Abs(deltaTranslation.X) > translateTolerance ||
					Math.Abs(deltaTranslation.Y) > translateTolerance ||
					Math.Abs(deltaTranslation.Z) > translateTolerance)
				{
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 dif8
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4(deltaTranslation.X, deltaTranslation.Y, deltaTranslation.Z, 0);

					Vector4 dif8v4 = dif8.ToVector4();

					if (Math.Abs(dif8v4.X - deltaTranslation.X) <= translateTolerance &&
						Math.Abs(dif8v4.Y - deltaTranslation.Y) <= translateTolerance &&
						Math.Abs(dif8v4.Z - deltaTranslation.Z) <= translateTolerance)
					{
						store |= TransformStorage.TranslateDeltaNByte | TransformStorage.TranslateChange;

						previous.Translation.X += dif8v4.X;
						previous.Translation.Y += dif8v4.Y;
						previous.Translation.Z += dif8v4.Z;

						uint pack = dif8.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
					}
					else
					{
						Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 difHalf =
							new Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4(deltaTranslation.X, deltaTranslation.Y, deltaTranslation.Z, 0);

						Vector4 difHalf4 = difHalf.ToVector4();

						if (Math.Abs(difHalf4.X - deltaTranslation.X) <= translateTolerance &&
							Math.Abs(difHalf4.Y - deltaTranslation.Y) <= translateTolerance &&
							Math.Abs(difHalf4.Z - deltaTranslation.Z) <= translateTolerance)
						{
							store |= TransformStorage.TranslateDeltaHalf | TransformStorage.TranslateChange;

							previous.Translation.X += difHalf4.X;
							previous.Translation.Y += difHalf4.Y;
							previous.Translation.Z += difHalf4.Z;

							ulong pack = difHalf.PackedValue;
							writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 32) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 40) & 0xFF);
						}
						else
						{
							//need full precision
							keyframe |= TransformKeyframe.TranslateChange;
							store |= TransformStorage.Keyframe;
						}
					}
				}
			}

			{
				Quaternion deltaRotation = previous.Rotation;
				deltaRotation.X = -deltaRotation.X;
				deltaRotation.Y = -deltaRotation.Y;
				deltaRotation.Z = -deltaRotation.Z;
				Quaternion.Multiply(ref transform.Rotation, ref deltaRotation, out deltaRotation);
				Quaternion.Normalize(ref deltaRotation, out deltaRotation);

				if (deltaRotation.W < cos2RotateTolerance)
				{
					Vector4 deltaRotV4 = new Vector4(deltaRotation.X, deltaRotation.Y, deltaRotation.Z, deltaRotation.W);

					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 dif8
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4(deltaRotation.X, deltaRotation.Y, deltaRotation.Z, deltaRotation.W);

					Vector4 dif8v4 = dif8.ToVector4();
					Quaternion quat = new Quaternion(dif8v4.X, dif8v4.Y, dif8v4.Z, dif8v4.W);
					Quaternion quatNorm;
					Quaternion.Normalize(ref quat, out quatNorm);
					//fast multiply, only care about W
					float quaternionW = (deltaRotation.W * quatNorm.W) - (((deltaRotation.X * -quatNorm.X) + (deltaRotation.Y * -quatNorm.Y)) + (deltaRotation.Z * -quatNorm.Z));

					if (cos2RotateTolerance <= quaternionW)
					{
						store |= TransformStorage.RotationDeltaNByte | TransformStorage.RotationChange;

						Quaternion.Multiply(ref quat, ref previous.Rotation, out previous.Rotation);

						uint pack = dif8.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
					}
					else
					{
						Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 difHalf =
							new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4(deltaRotV4);

						Vector4 difHalf4 = difHalf.ToVector4();

						quat.X = difHalf4.X;
						quat.Y = difHalf4.Y;
						quat.Z = difHalf4.Z;
						quat.W = difHalf4.W;
						Quaternion.Normalize(ref quat, out quatNorm);

						//fast multiply, only care about W
						quaternionW = (deltaRotation.W * quatNorm.W) - (((deltaRotation.X * -quatNorm.X) + (deltaRotation.Y * -quatNorm.Y)) + (deltaRotation.Z * -quatNorm.Z));

						if (quaternionW <= cos2RotateTolerance)
						{
							store |= TransformStorage.RotationDeltaHalf | TransformStorage.RotationChange;

							Quaternion.Multiply(ref quat, ref previous.Rotation, out previous.Rotation);

							ulong pack = difHalf.PackedValue;
							writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 32) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 40) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 48) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 56) & 0xFF);
						}
						else
						{
							//need full precision
							keyframe |= TransformKeyframe.RotationChange;
							store |= TransformStorage.Keyframe;
						}
					}
				}
			}

			//nothing significant changed in the frame
			if (store == 0 &&
				keyframe == 0)
			{
				repeat++;
				if (repeat == 127 || forceWrite)
				{
					writer.Write((byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat));
					repeat = 0;
					return 1;
				}
				return 0;
			}

			int written = 0;

			if (repeat != 0)
			{
				writer.Write((byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat));
				repeat = 0;
				written++;
			}
			//write in the data

			if (store != 0)
			{
				writer.Write((byte)store);
				written++;
			}
			if (bufferIndex != 0)
			{
				writer.Write(writeBuffer, 0, bufferIndex);
				written += bufferIndex;
			}

			if (keyframe != 0)
			{
				writer.Write((byte)keyframe);
				written++;

				if ((keyframe & TransformKeyframe.ScaleChange) == TransformKeyframe.ScaleChange)
				{
					writer.Write(transform.Scale);
					previous.Scale = transform.Scale;
					written += 4;
				}
				if ((keyframe & TransformKeyframe.TranslateChange) == TransformKeyframe.TranslateChange)
				{
					writer.Write(transform.Translation.X);
					writer.Write(transform.Translation.Y);
					writer.Write(transform.Translation.Z);
					previous.Translation = transform.Translation;
					written += 12;
				}
				if ((keyframe & TransformKeyframe.RotationChange) == TransformKeyframe.RotationChange)
				{
					writer.Write(transform.Rotation.X);
					writer.Write(transform.Rotation.Y);
					writer.Write(transform.Rotation.Z);
					writer.Write(transform.Rotation.W);
					previous.Rotation = transform.Rotation;
					written += 16;
				}
			}
			return written;
		}


		/// <summary>
		/// Write the transform to the stream. Returns the number of bytes, if any, that were written
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="writer"></param>
		/// <param name="forceWrite"><para>If true, the method call will always write at least 1 byte</para><para>When false, multiple identical transforms may be represented with a write of a single byte at the end of the sequence</para></param>
		/// <returns>Returns the number of bytes written</returns>
		public int Write(Transform transform, BinaryWriter writer, bool forceWrite)
		{
			return Write(ref transform, writer, forceWrite);
		}


		/// <summary>
		/// Write the transform to a byte[] buffer. Returns the number of bytes, if any, that were written
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="index">index to begin writing to the buffer</param>
		/// <param name="writeTarget">buffer that transform data will be written to (this buffer should be at least 34 bytes in length)</param>
		/// <param name="forceWrite"><para>If true, the method call will always write at least 1 byte</para><para>When false, multiple identical transforms may be represented with a write of a single byte at the end of the sequence</para></param>
		/// <returns>Returns the number of bytes written</returns>
		public int Write(ref Transform transform, byte[] writeTarget, ref int index, bool forceWrite)
		{
			int bufferIndex = 0;
			TransformStorage store = 0;
			TransformKeyframe keyframe = 0;

			//if the scale has changed a considerable amount...
			if (Math.Abs(previous.Scale - transform.Scale) > scaleTolerance)
			{
				if (transform.Scale >= 1 - scaleTolerance && transform.Scale <= 1 + scaleTolerance)
				{
					store |= TransformStorage.ScaleOne | TransformStorage.ScaleChange;

					previous.Scale = 1;
				}
				else
				{
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle value =
						new Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle(transform.Scale);
					float valuef = value.ToSingle();

					if (Math.Abs(transform.Scale - valuef) <= scaleTolerance)
					{
						ushort pack = value.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);

						store |= TransformStorage.ScaleHalf | TransformStorage.ScaleChange;

						previous.Scale = valuef;
					}
					else
					{
						//need full precision
						keyframe |= TransformKeyframe.ScaleChange;
						store |= TransformStorage.Keyframe;
					}
				}
			}


			{
				Vector3 deltaTranslation = new Vector3(
					(transform.Translation.X - previous.Translation.X),
					(transform.Translation.Y - previous.Translation.Y),
					(transform.Translation.Z - previous.Translation.Z));

				if (Math.Abs(deltaTranslation.X) > translateTolerance ||
					Math.Abs(deltaTranslation.Y) > translateTolerance ||
					Math.Abs(deltaTranslation.Z) > translateTolerance)
				{
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 dif8
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4(deltaTranslation.X, deltaTranslation.Y, deltaTranslation.Z, 0);

					Vector4 dif8v4 = dif8.ToVector4();

					if (Math.Abs(dif8v4.X - deltaTranslation.X) <= translateTolerance &&
						Math.Abs(dif8v4.Y - deltaTranslation.Y) <= translateTolerance &&
						Math.Abs(dif8v4.Z - deltaTranslation.Z) <= translateTolerance)
					{
						store |= TransformStorage.TranslateDeltaNByte | TransformStorage.TranslateChange;

						previous.Translation.X += dif8v4.X;
						previous.Translation.Y += dif8v4.Y;
						previous.Translation.Z += dif8v4.Z;

						uint pack = dif8.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
					}
					else
					{
						Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 difHalf =
							new Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4(deltaTranslation.X, deltaTranslation.Y, deltaTranslation.Z, 0);

						Vector4 difHalf4 = difHalf.ToVector4();

						if (Math.Abs(difHalf4.X - deltaTranslation.X) <= translateTolerance &&
							Math.Abs(difHalf4.Y - deltaTranslation.Y) <= translateTolerance &&
							Math.Abs(difHalf4.Z - deltaTranslation.Z) <= translateTolerance)
						{
							store |= TransformStorage.TranslateDeltaHalf | TransformStorage.TranslateChange;

							previous.Translation.X += difHalf4.X;
							previous.Translation.Y += difHalf4.Y;
							previous.Translation.Z += difHalf4.Z;

							ulong pack = difHalf.PackedValue;
							writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 32) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 40) & 0xFF);
						}
						else
						{
							//need full precision
							keyframe |= TransformKeyframe.TranslateChange;
							store |= TransformStorage.Keyframe;
						}
					}
				}
			}

			{
				Quaternion deltaRotation = previous.Rotation;
				deltaRotation.X = -deltaRotation.X;
				deltaRotation.Y = -deltaRotation.Y;
				deltaRotation.Z = -deltaRotation.Z;
				Quaternion.Multiply(ref transform.Rotation, ref deltaRotation, out deltaRotation);
				Quaternion.Normalize(ref deltaRotation, out deltaRotation);

				if (deltaRotation.W < cos2RotateTolerance)
				{
					Vector4 deltaRotV4 = new Vector4(deltaRotation.X, deltaRotation.Y, deltaRotation.Z, deltaRotation.W);

					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 dif8
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4(deltaRotation.X, deltaRotation.Y, deltaRotation.Z, deltaRotation.W);

					Vector4 dif8v4 = dif8.ToVector4();
					Quaternion quat = new Quaternion(dif8v4.X, dif8v4.Y, dif8v4.Z, dif8v4.W);
					Quaternion quatNorm;
					Quaternion.Normalize(ref quat, out quatNorm);

					//fast multiply, only care about W
					float quaternionW = (deltaRotation.W * quatNorm.W) - (((deltaRotation.X * -quatNorm.X) + (deltaRotation.Y * -quatNorm.Y)) + (deltaRotation.Z * -quatNorm.Z));

					if (cos2RotateTolerance <= quaternionW)
					{
						store |= TransformStorage.RotationDeltaNByte | TransformStorage.RotationChange;

						Quaternion.Multiply(ref quat, ref previous.Rotation, out previous.Rotation);

						uint pack = dif8.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
					}
					else
					{
						Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 difHalf =
							new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4(deltaRotV4);

						Vector4 difHalf4 = difHalf.ToVector4();

						quat.X = difHalf4.X;
						quat.Y = difHalf4.Y;
						quat.Z = difHalf4.Z;
						quat.W = difHalf4.W;
						Quaternion.Normalize(ref quat, out quatNorm);

						//fast multiply, only care about W
						quaternionW = (deltaRotation.W * quat.W) - (((deltaRotation.X * -quat.X) + (deltaRotation.Y * -quat.Y)) + (deltaRotation.Z * -quat.Z));

						if (quaternionW <= cos2RotateTolerance)
						{
							store |= TransformStorage.RotationDeltaHalf | TransformStorage.RotationChange;

							Quaternion.Multiply(ref quat, ref previous.Rotation, out previous.Rotation);

							ulong pack = difHalf.PackedValue;
							writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 32) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 40) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 48) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 56) & 0xFF);
						}
						else
						{
							//need full precision
							keyframe |= TransformKeyframe.RotationChange;
							store |= TransformStorage.Keyframe;
						}
					}
				}
			}

			//nothing significant changed in the frame
			if (store == 0 &&
				keyframe == 0)
			{
				repeat++;
				if (repeat == 127 || forceWrite)
				{
					writeTarget[index++] = (byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat);
					repeat = 0;
					return 1;
				}
				return 0;
			}

			int written = 0;

			if (repeat != 0)
			{
				writeTarget[index++] = (byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat);
				repeat = 0;
				written++;
			}
			//write in the data

			if (store != 0)
			{
				writeTarget[index++] = (byte)store;
				written++;
			}
			if (bufferIndex != 0)
			{
				for (int i = 0; i < bufferIndex; i++)
					writeTarget[index++] = writeBuffer[i];
				written += bufferIndex;
			}

			if (keyframe != 0)
			{
				writeTarget[index++] = (byte)keyframe;
				written++;
				BitCast cast = new BitCast();

				if ((keyframe & TransformKeyframe.ScaleChange) == TransformKeyframe.ScaleChange)
				{
					cast.Single = transform.Scale;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					previous.Scale = transform.Scale;
					written += 4;
				}
				if ((keyframe & TransformKeyframe.TranslateChange) == TransformKeyframe.TranslateChange)
				{
					cast.Single = transform.Translation.X;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					cast.Single = transform.Translation.Y;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					cast.Single = transform.Translation.Z;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					previous.Translation = transform.Translation;
					written += 12;
				}
				if ((keyframe & TransformKeyframe.RotationChange) == TransformKeyframe.RotationChange)
				{
					cast.Single = transform.Rotation.X;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					cast.Single = transform.Rotation.Y;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					cast.Single = transform.Rotation.Z;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					cast.Single = transform.Rotation.W;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					previous.Rotation = transform.Rotation;
					written += 16;
				}
			}
			return written;
		}

		/// <summary>
		/// Write the transform to a byte[] buffer. Returns the number of bytes, if any, that were written
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="index">index to begin writing to the buffer</param>
		/// <param name="writeTarget">buffer that transform data will be written to (this buffer should be at least 34 bytes in length)</param>
		/// <param name="forceWrite"><para>If true, the method call will always write at least 1 byte</para><para>When false, multiple identical transforms may be represented with a write of a single byte at the end of the sequence</para></param>
		/// <returns>Returns the number of bytes written</returns>
		public int Write(Transform transform, byte[] writeTarget, ref int index, bool forceWrite)
		{
			return Write(ref transform, writeTarget, ref index, forceWrite);
		}
	}

	/// <summary>
	/// <para>Writes a series of <see cref="Transform"/> structures to a binary stream as deltas, using lossy/lossless data compression</para>
	/// <para>Prefer using this method of compression over <see cref="CompressedTransformWriter"/> for input data with smooth consistent movement that doesn't change velocity much over many samples, such as player movement</para>
	/// <para>Redundant values are skipped and packed data types are used where possible</para>
	/// <para>Compression levels are based on specified compression tolerances</para>
	/// </summary>
	/// <remarks>
	/// <para>Tolerance values may be changed at will, this will not corrupt data already written/about to be written</para>
	/// <para>The <see cref="CompressedTransformAccelerationWriter"/> and <see cref="CompressedTransformAccelerationReader"/> have higher overhead than the <see cref="CompressedTransformWriter"/> and <see cref="CompressedTransformReader"/></para>
	/// </remarks>
	public sealed class CompressedTransformAccelerationWriter
	{
		#region members

		private float scaleTolerance;
		private float translateTolerance;
		private float rotateTolerance;
		private float cos2RotateTolerance;
		private Transform previous, acceleration;
		private int repeat;
		private readonly byte[] writeBuffer;

		#endregion

		#region tolerances

		/// <summary>
		/// Gets/Sets the scale tolerance. This is the amount the scale needs to change for a new value to be compressed to the stream. Set to 0 for lossless writing
		/// </summary>
		public float ScaleTolerance
		{
			get { return scaleTolerance; }
			set
			{
				if (value < 0)
					throw new ArgumentException();
				scaleTolerance = value;
			}
		}
		/// <summary>
		/// Gets/Sets the translate tolerance. This is the amount an translate x/y/z needs to change for a new value to be compressed to the stream. Set to 0 for lossless writing
		/// </summary>
		public float TranslateTolerance
		{
			get { return translateTolerance; }
			set
			{
				if (value < 0)
					throw new ArgumentException();
				translateTolerance = value;
			}
		}
		/// <summary>
		/// Gets/Sets the rotation tolerance. Rotations are stored as normalised vectors. This tolerance is for the change in a component of the vector. The maximum value is 0.5. However this would produce very infrequent rotations. 
		/// </summary>
		public float RotationTolerance
		{
			get { return rotateTolerance; }
			set
			{
				if (value < 0 || value > 0.5f)
					throw new ArgumentException();
				rotateTolerance = value;
				cos2RotateTolerance = (float)Math.Cos(value * 0.5f);
			}
		}

		#endregion

		/// <summary>
		/// <para>Construct the writer with default values of:</para>
		/// <para>ScaleTolerance = 0.025</para>
		/// <para>TranslateTolerance = 0.01</para>
		/// <para>RotationTolerance = 0.001</para>
		/// </summary>
		public CompressedTransformAccelerationWriter()
		{
			ScaleTolerance = 0.025f;
			TranslateTolerance = 0.01f;
			RotationTolerance = 0.001f;
			this.previous = Transform.Identity;
			this.acceleration = Transform.Identity;
			writeBuffer = new byte[16];
		}
		/// <summary>
		/// Construct the writer
		/// </summary>
		/// <param name="scaleTolerance"></param>
		/// <param name="translateTolerance"></param>
		/// <param name="rotationTolerance"></param>
		public CompressedTransformAccelerationWriter(float scaleTolerance, float translateTolerance, float rotationTolerance)
		{
			this.ScaleTolerance = scaleTolerance;
			this.TranslateTolerance = translateTolerance;
			this.RotationTolerance = rotationTolerance;
			this.previous = Transform.Identity;
			this.acceleration = Transform.Identity;
			writeBuffer = new byte[16];
		}

		/// <summary>
		/// Writes the entire transformation uncompressed. Normally the change in transform is written, relying on the previous value. (Writes 34 bytes)
		/// </summary>
		/// <remarks><para>This method can be used to 'reset' the compression stream.</para></remarks>
		/// <param name="transform"></param>
		/// <param name="writer"></param>
		/// <returns>Returns the number of bytes written</returns>
		public int WriteUncompressed(ref Transform transform, BinaryWriter writer)
		{
			writer.Write((byte)TransformStorage.Keyframe);
			writer.Write((byte)(TransformKeyframe.RotationChange | TransformKeyframe.ScaleChange | TransformKeyframe.TranslateChange));

			writer.Write(transform.Scale);
			writer.Write(transform.Translation.X);
			writer.Write(transform.Translation.Y);
			writer.Write(transform.Translation.Z);
			writer.Write(transform.Rotation.X);
			writer.Write(transform.Rotation.Y);
			writer.Write(transform.Rotation.Z);
			writer.Write(transform.Rotation.W);
			previous = transform;

			repeat = 0;
			return 34;
		}
		/// <summary>
		/// Writes the entire transformation uncompressed. Normally the change in transform is written, relying on the previous value. Returns the number of bytes written (which is always 34 for this method)
		/// </summary>
		/// <remarks><para>This method can be used to 'reset' the compression stream.</para></remarks>
		/// <param name="transform"></param>
		/// <param name="writer"></param>
		/// <returns>Returns the number of bytes written</returns>
		public int WriteUncompressed(Transform transform, BinaryWriter writer)
		{
			return WriteUncompressed(ref transform, writer);
		}

		/// <summary>
		/// <para>Write the final bytes that are needed to complete writing a series of transforms. Returns the number of bytes that were written</para>
		/// </summary>
		/// <param name="writer"></param>
		/// <returns>Returns the number of bytes written</returns>
		public int EndWriting(BinaryWriter writer)
		{
			this.previous = Transform.Identity;
			this.acceleration = Transform.Identity;
			if (repeat != 0)
			{
				writer.Write((byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat));
				writer.Write((byte)0);
				repeat = 0;
				return 2;
			}
			writer.Write((byte)0);
			return 1;
		}
		/// <summary>
		/// <para>Write the final bytes that are needed to complete writing a series of transforms. Returns the number of bytes that were written</para>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="writeTarget"></param>
		/// <returns>Returns the number of bytes written</returns>
		public int EndWriting(byte[] writeTarget, ref int index)
		{
			this.previous = Transform.Identity;
			this.acceleration = Transform.Identity;
			if (repeat != 0)
			{
				writeTarget[index++] = (byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat);
				writeTarget[index++] = (byte)0;
				repeat = 0;
				return 2;
			}
			writeTarget[index++] = (byte)0;
			return 1;
		}


		/// <summary>
		/// Write the transform to the stream. Returns the number of bytes, if any, that were written
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="writer"></param>
		/// <param name="forceWrite"><para>If true, the method call will always write at least 1 byte</para><para>When false, multiple identical transforms may be represented with a write of a single byte at the end of the sequence</para></param>
		/// <returns>Returns the number of bytes written</returns>
		public int Write(ref Transform transform, BinaryWriter writer, bool forceWrite)
		{
			int bufferIndex = 0;
			TransformStorage store = 0;
			TransformKeyframe keyframe = 0;

			//if the scale has changed a considerable amount...
			if (Math.Abs(previous.Scale - transform.Scale) > scaleTolerance)
			{
				if (transform.Scale >= 1 - scaleTolerance && transform.Scale <= 1 + scaleTolerance)
				{
					store |= TransformStorage.ScaleOne | TransformStorage.ScaleChange;

					previous.Scale = 1;
				}
				else
				{
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle value =
						new Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle(transform.Scale);
					float valuef = value.ToSingle();

					if (Math.Abs(transform.Scale - valuef) <= scaleTolerance)
					{
						ushort pack = value.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);

						store |= TransformStorage.ScaleHalf | TransformStorage.ScaleChange;

						previous.Scale = valuef;
					}
					else
					{
						//need full precision
						keyframe |= TransformKeyframe.ScaleChange;
						store |= TransformStorage.Keyframe;
					}
				}
			}


			{
				Vector3 deltaTranslation = new Vector3(
					(transform.Translation.X - previous.Translation.X) - acceleration.Translation.X,
					(transform.Translation.Y - previous.Translation.Y) - acceleration.Translation.Y,
					(transform.Translation.Z - previous.Translation.Z) - acceleration.Translation.Z);

				if (Math.Abs(deltaTranslation.X) > translateTolerance ||
					Math.Abs(deltaTranslation.Y) > translateTolerance ||
					Math.Abs(deltaTranslation.Z) > translateTolerance)
				{
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 dif8
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4(deltaTranslation.X, deltaTranslation.Y, deltaTranslation.Z, 0);

					Vector4 dif8v4 = dif8.ToVector4();

					if (Math.Abs(dif8v4.X - deltaTranslation.X) <= translateTolerance &&
						Math.Abs(dif8v4.Y - deltaTranslation.Y) <= translateTolerance &&
						Math.Abs(dif8v4.Z - deltaTranslation.Z) <= translateTolerance)
					{
						store |= TransformStorage.TranslateDeltaNByte | TransformStorage.TranslateChange;

						acceleration.Translation.X += dif8v4.X;
						acceleration.Translation.Y += dif8v4.Y;
						acceleration.Translation.Z += dif8v4.Z;

						uint pack = dif8.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
					}
					else
					{
						Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 difHalf =
							new Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4(deltaTranslation.X, deltaTranslation.Y, deltaTranslation.Z, 0);

						Vector4 difHalf4 = difHalf.ToVector4();

						if (Math.Abs(difHalf4.X - deltaTranslation.X) <= translateTolerance &&
							Math.Abs(difHalf4.Y - deltaTranslation.Y) <= translateTolerance &&
							Math.Abs(difHalf4.Z - deltaTranslation.Z) <= translateTolerance)
						{
							store |= TransformStorage.TranslateDeltaHalf | TransformStorage.TranslateChange;

							acceleration.Translation.X += difHalf4.X;
							acceleration.Translation.Y += difHalf4.Y;
							acceleration.Translation.Z += difHalf4.Z;

							ulong pack = difHalf.PackedValue;
							writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 32) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 40) & 0xFF);
						}
						else
						{
							//need full precision
							keyframe |= TransformKeyframe.TranslateChange;
							store |= TransformStorage.Keyframe;
						}
					}
				}
			}

			{
				Quaternion deltaRotation;
				Quaternion.Multiply(ref acceleration.Rotation, ref previous.Rotation, out deltaRotation);
				deltaRotation.X = -deltaRotation.X;
				deltaRotation.Y = -deltaRotation.Y;
				deltaRotation.Z = -deltaRotation.Z;
				Quaternion.Multiply(ref transform.Rotation, ref deltaRotation, out deltaRotation);
				Quaternion.Normalize(ref deltaRotation, out deltaRotation);

				if (deltaRotation.W < cos2RotateTolerance)
				{
					Vector4 deltaRotV4 = new Vector4(deltaRotation.X, deltaRotation.Y, deltaRotation.Z, deltaRotation.W);

					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 dif8
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4(deltaRotation.X, deltaRotation.Y, deltaRotation.Z, deltaRotation.W);

					Vector4 dif8v4 = dif8.ToVector4();
					Quaternion quat = new Quaternion(dif8v4.X, dif8v4.Y, dif8v4.Z, dif8v4.W);
					Quaternion quatNorm;
					Quaternion.Normalize(ref quat, out quatNorm);

					//fast multiply, only care about W
					float quaternionW = (deltaRotation.W * quatNorm.W) - (((deltaRotation.X * -quatNorm.X) + (deltaRotation.Y * -quatNorm.Y)) + (deltaRotation.Z * -quatNorm.Z));

					if (cos2RotateTolerance <= quaternionW)
					{
						store |= TransformStorage.RotationDeltaNByte | TransformStorage.RotationChange;

						Quaternion.Multiply(ref quat, ref acceleration.Rotation, out acceleration.Rotation);

						uint pack = dif8.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
					}
					else
					{
						Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 difHalf =
							new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4(deltaRotV4);

						Vector4 difHalf4 = difHalf.ToVector4();

						quat.X = difHalf4.X;
						quat.Y = difHalf4.Y;
						quat.Z = difHalf4.Z;
						quat.W = difHalf4.W;
						Quaternion.Normalize(ref quat, out quatNorm);

						//fast multiply, only care about W
						quaternionW = (deltaRotation.W * quat.W) - (((deltaRotation.X * -quat.X) + (deltaRotation.Y * -quat.Y)) + (deltaRotation.Z * -quat.Z));

						if (quaternionW <= cos2RotateTolerance)
						{
							store |= TransformStorage.RotationDeltaHalf | TransformStorage.RotationChange;

							Quaternion.Multiply(ref quat, ref acceleration.Rotation, out acceleration.Rotation);

							ulong pack = difHalf.PackedValue;
							writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 32) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 40) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 48) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 56) & 0xFF);
						}
						else
						{
							//need full precision
							keyframe |= TransformKeyframe.RotationChange;
							store |= TransformStorage.Keyframe;
						}
					}
				}
			}

			Quaternion.Multiply(ref acceleration.Rotation, ref previous.Rotation, out previous.Rotation);
			float length =
				this.previous.Rotation.X * this.previous.Rotation.X +
				this.previous.Rotation.Y * this.previous.Rotation.Y +
				this.previous.Rotation.Z * this.previous.Rotation.Z +
				this.previous.Rotation.W * this.previous.Rotation.W;
			if (length > 1.0001f || length < 0.9999f)
			{
				//normalize
				length = 1.0f / (float)Math.Sqrt(length);
				this.previous.Rotation.X *= length;
				this.previous.Rotation.Y *= length;
				this.previous.Rotation.Z *= length;
				this.previous.Rotation.W *= length;
			}
			previous.Translation.X += acceleration.Translation.X;
			previous.Translation.Y += acceleration.Translation.Y;
			previous.Translation.Z += acceleration.Translation.Z;

			//nothing significant changed in the frame
			if (store == 0 &&
				keyframe == 0)
			{
				repeat++;
				if (repeat == 127 || forceWrite)
				{
					writer.Write((byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat));
					repeat = 0;
					return 1;
				}
				return 0;
			}

			int written = 0;

			if (repeat != 0)
			{
				writer.Write((byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat));
				repeat = 0;
				written++;
			}
			//write in the data

			if (store != 0)
			{
				writer.Write((byte)store);
				written++;
			}
			if (bufferIndex != 0)
			{
				writer.Write(writeBuffer, 0, bufferIndex);
				written += bufferIndex;
			}

			if (keyframe != 0)
			{
				writer.Write((byte)keyframe);
				written++;

				if ((keyframe & TransformKeyframe.ScaleChange) == TransformKeyframe.ScaleChange)
				{
					writer.Write(transform.Scale);
					previous.Scale = transform.Scale;
					written += 4;
				}
				if ((keyframe & TransformKeyframe.TranslateChange) == TransformKeyframe.TranslateChange)
				{
					writer.Write(transform.Translation.X);
					writer.Write(transform.Translation.Y);
					writer.Write(transform.Translation.Z);
					previous.Translation = transform.Translation;
					written += 12;
				}
				if ((keyframe & TransformKeyframe.RotationChange) == TransformKeyframe.RotationChange)
				{
					writer.Write(transform.Rotation.X);
					writer.Write(transform.Rotation.Y);
					writer.Write(transform.Rotation.Z);
					writer.Write(transform.Rotation.W);
					previous.Rotation = transform.Rotation;
					written += 16;
				}
			}
			return written;
		}


		/// <summary>
		/// Write the transform to the stream. Returns the number of bytes, if any, that were written
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="writer"></param>
		/// <param name="forceWrite"><para>If true, the method call will always write at least 1 byte</para><para>When false, multiple identical transforms may be represented with a write of a single byte at the end of the sequence</para></param>
		/// <returns>Returns the number of bytes written</returns>
		public int Write(Transform transform, BinaryWriter writer, bool forceWrite)
		{
			return Write(ref transform, writer, forceWrite);
		}


		/// <summary>
		/// Write the transform to a byte[] buffer. Returns the number of bytes, if any, that were written
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="index">index to begin writing to the buffer</param>
		/// <param name="writeTarget">buffer that transform data will be written to (this buffer should be at least 34 bytes in length)</param>
		/// <param name="forceWrite"><para>If true, the method call will always write at least 1 byte</para><para>When false, multiple identical transforms may be represented with a write of a single byte at the end of the sequence</para></param>
		/// <returns>Returns the number of bytes written</returns>
		public int Write(ref Transform transform, byte[] writeTarget, ref int index, bool forceWrite)
		{
			int bufferIndex = 0;
			TransformStorage store = 0;
			TransformKeyframe keyframe = 0;

			//if the scale has changed a considerable amount...
			if (Math.Abs(previous.Scale - transform.Scale) > scaleTolerance)
			{
				if (transform.Scale >= 1 - scaleTolerance && transform.Scale <= 1 + scaleTolerance)
				{
					store |= TransformStorage.ScaleOne | TransformStorage.ScaleChange;

					previous.Scale = 1;
				}
				else
				{
					Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle value =
						new Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle(transform.Scale);
					float valuef = value.ToSingle();

					if (Math.Abs(transform.Scale - valuef) <= scaleTolerance)
					{
						ushort pack = value.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);

						store |= TransformStorage.ScaleHalf | TransformStorage.ScaleChange;

						previous.Scale = valuef;
					}
					else
					{
						//need full precision
						keyframe |= TransformKeyframe.ScaleChange;
						store |= TransformStorage.Keyframe;
					}
				}
			}


			{
				Vector3 deltaTranslation = new Vector3(
					(transform.Translation.X - previous.Translation.X) - acceleration.Translation.X,
					(transform.Translation.Y - previous.Translation.Y) - acceleration.Translation.Y,
					(transform.Translation.Z - previous.Translation.Z) - acceleration.Translation.Z);

				if (Math.Abs(deltaTranslation.X) > translateTolerance ||
					Math.Abs(deltaTranslation.Y) > translateTolerance ||
					Math.Abs(deltaTranslation.Z) > translateTolerance)
				{
					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 dif8
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4(deltaTranslation.X, deltaTranslation.Y, deltaTranslation.Z, 0);

					Vector4 dif8v4 = dif8.ToVector4();

					if (Math.Abs(dif8v4.X - deltaTranslation.X) <= translateTolerance &&
						Math.Abs(dif8v4.Y - deltaTranslation.Y) <= translateTolerance &&
						Math.Abs(dif8v4.Z - deltaTranslation.Z) <= translateTolerance)
					{
						store |= TransformStorage.TranslateDeltaNByte | TransformStorage.TranslateChange;

						acceleration.Translation.X += dif8v4.X;
						acceleration.Translation.Y += dif8v4.Y;
						acceleration.Translation.Z += dif8v4.Z;

						uint pack = dif8.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
					}
					else
					{
						Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 difHalf =
							new Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4(deltaTranslation.X, deltaTranslation.Y, deltaTranslation.Z, 0);

						Vector4 difHalf4 = difHalf.ToVector4();

						if (Math.Abs(difHalf4.X - deltaTranslation.X) <= translateTolerance &&
							Math.Abs(difHalf4.Y - deltaTranslation.Y) <= translateTolerance &&
							Math.Abs(difHalf4.Z - deltaTranslation.Z) <= translateTolerance)
						{
							store |= TransformStorage.TranslateDeltaHalf | TransformStorage.TranslateChange;

							acceleration.Translation.X += difHalf4.X;
							acceleration.Translation.Y += difHalf4.Y;
							acceleration.Translation.Z += difHalf4.Z;

							ulong pack = difHalf.PackedValue;
							writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 32) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 40) & 0xFF);
						}
						else
						{
							//need full precision
							keyframe |= TransformKeyframe.TranslateChange;
							store |= TransformStorage.Keyframe;
						}
					}
				}
			}

			{
				Quaternion deltaRotation;
				Quaternion.Multiply(ref acceleration.Rotation, ref previous.Rotation, out deltaRotation);
				deltaRotation.X = -deltaRotation.X;
				deltaRotation.Y = -deltaRotation.Y;
				deltaRotation.Z = -deltaRotation.Z;
				Quaternion.Multiply(ref transform.Rotation, ref deltaRotation, out deltaRotation);
				Quaternion.Normalize(ref deltaRotation, out deltaRotation);

				if (deltaRotation.W < cos2RotateTolerance)
				{
					Vector4 deltaRotV4 = new Vector4(deltaRotation.X, deltaRotation.Y, deltaRotation.Z, deltaRotation.W);

					Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 dif8
						= new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4(deltaRotation.X, deltaRotation.Y, deltaRotation.Z, deltaRotation.W);

					Vector4 dif8v4 = dif8.ToVector4();
					Quaternion quat = new Quaternion(dif8v4.X, dif8v4.Y, dif8v4.Z, dif8v4.W);
					Quaternion quatNorm;
					Quaternion.Normalize(ref quat, out quatNorm);

					//fast multiply, only care about W
					float quaternionW = (deltaRotation.W * quatNorm.W) - (((deltaRotation.X * -quatNorm.X) + (deltaRotation.Y * -quatNorm.Y)) + (deltaRotation.Z * -quatNorm.Z));

					if (cos2RotateTolerance <= quaternionW)
					{
						store |= TransformStorage.RotationDeltaNByte | TransformStorage.RotationChange;

						Quaternion.Multiply(ref quat, ref acceleration.Rotation, out acceleration.Rotation);

						uint pack = dif8.PackedValue;
						writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
						writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
					}
					else
					{
						Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 difHalf =
							new Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4(deltaRotV4);

						Vector4 difHalf4 = difHalf.ToVector4();

						quat.X = difHalf4.X;
						quat.Y = difHalf4.Y;
						quat.Z = difHalf4.Z;
						quat.W = difHalf4.W;
						Quaternion.Normalize(ref quat, out quatNorm);

						//fast multiply, only care about W
						quaternionW = (deltaRotation.W * quat.W) - (((deltaRotation.X * -quat.X) + (deltaRotation.Y * -quat.Y)) + (deltaRotation.Z * -quat.Z));

						if (quaternionW <= cos2RotateTolerance)
						{
							store |= TransformStorage.RotationDeltaHalf | TransformStorage.RotationChange;

							Quaternion.Multiply(ref quat, ref acceleration.Rotation, out acceleration.Rotation);

							ulong pack = difHalf.PackedValue;
							writeBuffer[bufferIndex++] = (byte)((pack >> 0) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 8) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 16) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 24) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 32) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 40) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 48) & 0xFF);
							writeBuffer[bufferIndex++] = (byte)((pack >> 56) & 0xFF);
						}
						else
						{
							//need full precision
							keyframe |= TransformKeyframe.RotationChange;
							store |= TransformStorage.Keyframe;
						}
					}
				}
			}

			Quaternion.Multiply(ref acceleration.Rotation, ref previous.Rotation, out previous.Rotation);
			float length =
				this.previous.Rotation.X * this.previous.Rotation.X +
				this.previous.Rotation.Y * this.previous.Rotation.Y +
				this.previous.Rotation.Z * this.previous.Rotation.Z +
				this.previous.Rotation.W * this.previous.Rotation.W;
			if (length > 1.0001f || length < 0.9999f)
			{
				//normalize
				length = 1.0f / (float)Math.Sqrt(length);
				this.previous.Rotation.X *= length;
				this.previous.Rotation.Y *= length;
				this.previous.Rotation.Z *= length;
				this.previous.Rotation.W *= length;
			}
			previous.Translation.X += acceleration.Translation.X;
			previous.Translation.Y += acceleration.Translation.Y;
			previous.Translation.Z += acceleration.Translation.Z;

			//nothing significant changed in the frame
			if (store == 0 &&
				keyframe == 0)
			{
				repeat++;
				if (repeat == 127 || forceWrite)
				{
					writeTarget[index++] = (byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat);
					repeat = 0;
					return 1;
				}
				return 0;
			}

			int written = 0;

			if (repeat != 0)
			{
				writeTarget[index++] = (byte)(TransformStorage.RepeatPrevious | (TransformStorage)repeat);
				repeat = 0;
				written++;
			}
			//write in the data

			if (store != 0)
			{
				writeTarget[index++] = (byte)store;
				written++;
			}
			if (bufferIndex != 0)
			{
				for (int i = 0; i < bufferIndex; i++)
					writeTarget[index++] = writeBuffer[i];
				written += bufferIndex;
			}

			if (keyframe != 0)
			{
				writeTarget[index++] = (byte)keyframe;
				written++;
				BitCast cast = new BitCast();

				if ((keyframe & TransformKeyframe.ScaleChange) == TransformKeyframe.ScaleChange)
				{
					cast.Single = transform.Scale;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					previous.Scale = transform.Scale;
					written += 4;
				}
				if ((keyframe & TransformKeyframe.TranslateChange) == TransformKeyframe.TranslateChange)
				{
					cast.Single = transform.Translation.X;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					cast.Single = transform.Translation.Y;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					cast.Single = transform.Translation.Z;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					previous.Translation = transform.Translation;
					written += 12;
				}
				if ((keyframe & TransformKeyframe.RotationChange) == TransformKeyframe.RotationChange)
				{
					cast.Single = transform.Rotation.X;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					cast.Single = transform.Rotation.Y;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					cast.Single = transform.Rotation.Z;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					cast.Single = transform.Rotation.W;
					writeTarget[index++] = cast.Byte0;
					writeTarget[index++] = cast.Byte1;
					writeTarget[index++] = cast.Byte2;
					writeTarget[index++] = cast.Byte3;
					previous.Rotation = transform.Rotation;
					written += 16;
				}
			}
			return written;
		}

		/// <summary>
		/// Write the transform to a byte[] buffer. Returns the number of bytes, if any, that were written
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="index">index to begin writing to the buffer</param>
		/// <param name="writeTarget">buffer that transform data will be written to (this buffer should be at least 34 bytes in length)</param>
		/// <param name="forceWrite"><para>If true, the method call will always write at least 1 byte</para><para>When false, multiple identical transforms may be represented with a write of a single byte at the end of the sequence</para></param>
		/// <returns>Returns the number of bytes written</returns>
		public int Write(Transform transform, byte[] writeTarget, ref int index, bool forceWrite)
		{
			return Write(ref transform, writeTarget, ref index, forceWrite);
		}
	}

}
