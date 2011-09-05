using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Xen.Ex
{
	/// <summary>
	/// A structure that can be used to do bit casting between primitve types
	/// </summary>
	/// <remarks>
	/// <para>For example, to bitwise convert an int to a float:</para>
	/// <example>
	/// <code>
	/// BitCast cast = new BitCast();
	/// 
	/// //...
	/// 
	/// cast.Int32 = 12345;
	/// float value = cast.Single;
	/// </code>
	/// </example>
	/// <para>The same can be done with Bytes, using <see cref="Byte0"/> to <see cref="Byte7"/></para>
	/// </remarks>
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public struct BitCast
	{
		//big/little endian differences
#if XBOX360
		
		/// <summary></summary>
		[FieldOffset(4)]
		public int Int32;
		/// <summary></summary>
		[FieldOffset(4)]
		public uint UInt32;
		/// <summary></summary>
		[FieldOffset(6)]
		public ushort UInt16;
		/// <summary></summary>
		[FieldOffset(6)]
		public short Int16;
		/// <summary></summary>
		[FieldOffset(4)]
		public float Single;
		/// <summary></summary>
		[FieldOffset(0)]
		public double Double;
		/// <summary></summary>
		[FieldOffset(0)]
		public ulong UInt64;
		/// <summary></summary>
		[FieldOffset(0)]
		public long Int64;

		
		/// <summary></summary>
		[FieldOffset(7)]
		public byte Byte0;
		/// <summary></summary>
		[FieldOffset(6)]
		public byte Byte1;
		/// <summary></summary>
		[FieldOffset(5)]
		public byte Byte2;
		/// <summary></summary>
		[FieldOffset(4)]
		public byte Byte3;
		/// <summary></summary>
		[FieldOffset(3)]
		public byte Byte4;
		/// <summary></summary>
		[FieldOffset(2)]
		public byte Byte5;
		/// <summary></summary>
		[FieldOffset(1)]
		public byte Byte6;
		/// <summary></summary>
		[FieldOffset(0)]
		public byte Byte7;
#else

		/// <summary></summary>
		[FieldOffset(0)]
		public int Int32;
		/// <summary></summary>
		[FieldOffset(0)]
		public uint UInt32;
		/// <summary></summary>
		[FieldOffset(0)]
		public ushort UInt16;
		/// <summary></summary>
		[FieldOffset(0)]
		public short Int16;
		/// <summary></summary>
		[FieldOffset(0)]
		public float Single;
		/// <summary></summary>
		[FieldOffset(0)]
		public double Double;
		/// <summary></summary>
		[FieldOffset(0)]
		public ulong UInt64;
		/// <summary></summary>
		[FieldOffset(0)]
		public long Int64;


		/// <summary></summary>
		[FieldOffset(0)]
		public byte Byte0;
		/// <summary></summary>
		[FieldOffset(1)]
		public byte Byte1;
		/// <summary></summary>
		[FieldOffset(2)]
		public byte Byte2;
		/// <summary></summary>
		[FieldOffset(3)]
		public byte Byte3;
		/// <summary></summary>
		[FieldOffset(4)]
		public byte Byte4;
		/// <summary></summary>
		[FieldOffset(5)]
		public byte Byte5;
		/// <summary></summary>
		[FieldOffset(6)]
		public byte Byte6;
		/// <summary></summary>
		[FieldOffset(7)]
		public byte Byte7;
#endif
	}

	/// <summary>
	/// Wraps an array as a readonly IList collection, and provides a struct based enumerator (reduces garbage collection and need for 'xxxCollection' classes)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct ReadOnlyArrayCollection<T> : IList<T>
	{
		readonly T[] array;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="array"></param>
		public ReadOnlyArrayCollection(T[] array)
		{
			if (array == null)
				this = Empty;
			else
				this.array = array;
		}

		/// <summary>
		/// struct enumerator
		/// </summary>
		public struct ArrayEnumerator : IEnumerator<T>
		{
			readonly T[] array;
			int index;

			internal ArrayEnumerator(T[] array)
			{
				this.array = array;
				this.index = -1;
			}

			/// <summary></summary>
			public T Current
			{
				get { return array[index]; }
			}

			/// <summary></summary>
			public void Dispose()
			{
				index = -1;
			}

			object System.Collections.IEnumerator.Current
			{
				get { return array[index]; }
			}

			/// <summary></summary>
			public bool MoveNext()
			{
				return ++index != array.Length;
			}

			/// <summary></summary>
			public void Reset()
			{
				index = -1;
			}
		}

		/// <summary></summary>
		public int IndexOf(T item)
		{
			return ((IList<T>)array).IndexOf(item);
		}

		void IList<T>.Insert(int index, T item)
		{
			throw new NotSupportedException();
		}

		void IList<T>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		/// <summary></summary>
		public T this[int index]
		{
			get
			{
				return array[index];
			}
			set
			{
				throw new InvalidOperationException("readonly");
			}
		}


		void ICollection<T>.Add(T item)
		{
			throw new NotSupportedException();
		}

		void ICollection<T>.Clear()
		{
			throw new NotSupportedException();
		}

		/// <summary></summary>
		public bool Contains(T item)
		{
			return ((ICollection<T>)array).Contains(item);
		}

		/// <summary></summary>
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.array.CopyTo(array, arrayIndex);
		}

		int ICollection<T>.Count
		{
			get { return array.Length; }
		}
		/// <summary></summary>
		public int Length
		{
			get { return array.Length; }
		}

		/// <summary></summary>
		public bool IsReadOnly
		{
			get { return true; }
		}

		bool ICollection<T>.Remove(T item)
		{
			throw new NotSupportedException();
		}

		ArrayEnumerator GetEnumerator()
		{
			return new ArrayEnumerator(array);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary></summary>
		public T[] ToArray()
		{
			return (T[])array.Clone();
		}

		private static T[] emptyList;
		/// <summary>
		/// Gets an empty collection
		/// </summary>
		public static ReadOnlyArrayCollection<T> Empty
		{
			get
			{
				if (emptyList == null)
					emptyList = new T[0];
				return new ReadOnlyArrayCollection<T>(emptyList);
			}
		}

	}


	/// <summary>
	/// This class wraps a <see cref="StringBuilder"/>, providing an integer count that increments each time the string changes (A change index)
	/// </summary>
	public sealed class TextValue
	{
		internal StringBuilder value = new StringBuilder();
		private int changeIndex;

		/// <summary>
		/// True if the text has changed (change index mismatch)
		/// </summary>
		/// <param name="changeIndex"></param>
		/// <returns></returns>
		public bool HasChanged(ref int changeIndex)
		{
			if (this.changeIndex != changeIndex)
			{
				changeIndex = this.changeIndex;
				return true;
			}
			return false;
		}

		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static implicit operator string(TextValue value)
		{
			return value.value.ToString();
		}

		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static implicit operator StringBuilder(TextValue value)
		{
			return value.value;
		}

		/// <summary>
		/// Length of the string
		/// </summary>
		public int Length
		{
			get
			{
				return value.Length;
			}
		}

		/// <summary>
		/// Clears the string
		/// </summary>
		public void Clear()
		{
			if (value.Length != 0)
			{
				value.Length = 0;
				changeIndex++;
			}
		}

		/// <summary>
		/// Trim <paramref name="trimCharacters"/> number of characters from the end of the text
		/// </summary>
		/// <returns>the number of characters trimmed</returns>
		/// <param name="trimCharacters"></param>
		public int TrimEnd(int trimCharacters)
		{
			if (trimCharacters >= this.Length)
			{
				int len = this.Length;
				Clear();
				return len;
			}

			this.value.Length -= trimCharacters;
			changeIndex++;
			return trimCharacters;
		}

		/// <summary></summary>
		public void SetText(string value) 
		{
			this.value.Length = 0; this.value.Append(value); changeIndex++;
		}
		/// <summary>Set a text character by index</summary>
		public void SetCharacter(char value, int index) { if (this.value[index] != value) { this.value[index] = value; changeIndex++; } }
		/// <summary></summary>
		public void SetText(int value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(float value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(double value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(short value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(ushort value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(uint value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(long value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(ulong value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(byte value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(sbyte value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(bool value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(char value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(char[] value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void SetText(object value) { this.value.Length = 0; this.value.Append(value); changeIndex++; }

		/// <summary></summary>
		public void Append(string value) 
		{
			this.value.Append(value); changeIndex++;
		}
		/// <summary></summary>
		public void Append(int value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(float value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(double value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(short value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(ushort value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(uint value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(long value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(ulong value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(byte value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(sbyte value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(bool value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(char value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(char[] value) { this.value.Append(value); changeIndex++; }
		/// <summary></summary>
		public void Append(object value) { this.value.Append(value); changeIndex++; }

		/// <summary></summary>
		public void AppendFormat(string format, object arg0) { this.value.AppendFormat(format, arg0); changeIndex++; }
		/// <summary></summary>
		public void AppendFormat(string format, params object[] args) { this.value.AppendFormat(format, args); changeIndex++; }
		/// <summary></summary>
		public void AppendFormat(IFormatProvider provider, string format, params object[] args) { this.value.AppendFormat(provider, format, args); changeIndex++; }
		/// <summary></summary>
		public void AppendFormat(string format, object arg0, object arg1) { this.value.AppendFormat(format, arg0, arg1); changeIndex++; }
		/// <summary></summary>
		public void AppendFormat(string format, object arg0, object arg1, object arg2) { this.value.AppendFormat(format, arg0, arg1, arg2); changeIndex++; }

		/// <summary></summary>
		public void AppendFormatLine(string format, object arg0) { this.value.AppendFormat(format, arg0); this.value.AppendLine(); changeIndex++; }
		/// <summary></summary>
		public void AppendFormatLine(string format, params object[] args) { this.value.AppendFormat(format, args); this.value.AppendLine(); changeIndex++; }
		/// <summary></summary>
		public void AppendFormatLine(IFormatProvider provider, string format, params object[] args) { this.value.AppendFormat(provider, format, args); this.value.AppendLine(); changeIndex++; }
		/// <summary></summary>
		public void AppendFormatLine(string format, object arg0, object arg1) { this.value.AppendFormat(format, arg0, arg1); this.value.AppendLine(); changeIndex++; }
		/// <summary></summary>
		public void AppendFormatLine(string format, object arg0, object arg1, object arg2) { this.value.AppendFormat(format, arg0, arg1, arg2); this.value.AppendLine(); changeIndex++; }

		/// <summary></summary>
		public void AppendLine(string value) { this.value.AppendLine(value); changeIndex++; }
		/// <summary></summary>
		public void AppendLine() { this.value.AppendLine(); changeIndex++; }

		/// <summary></summary>
		public static TextValue operator +(TextValue text, string value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, int value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, float value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, double value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, short value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, ushort value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, uint value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, long value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, ulong value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, byte value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, sbyte value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, bool value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, char value) { text.value.Append(value); text.changeIndex++; return text; }
		/// <summary></summary>
		public static TextValue operator +(TextValue text, object value) { text.value.Append(value); text.changeIndex++; return text; }

		/// <summary></summary>
		public override string ToString()
		{
			return value.ToString();
		}
	}

}
