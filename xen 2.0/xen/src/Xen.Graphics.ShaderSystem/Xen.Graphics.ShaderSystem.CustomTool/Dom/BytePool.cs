using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xen.Graphics.ShaderSystem.CustomTool.FX;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.CodeDom;
using System.IO;
using System.CodeDom.Compiler;
using Xen.Graphics.ShaderSystem.Constants;

namespace Xen.Graphics.ShaderSystem.CustomTool.Dom
{
	struct ByteArray
	{
		public readonly byte[] Array;
		public readonly ulong Hash64;
		public readonly int Hash32;

		public ByteArray(byte[] array)
		{
			this.Array = array;

			byte[] hash = new byte[8];
			for (int i = 0; i < array.Length; i++)
				hash[i & 7] ^= array[i];

			this.Hash64 = BitConverter.ToUInt64(hash, 0);

			hash[0] ^= hash[4];
			hash[1] ^= hash[5];
			hash[2] ^= hash[6];
			hash[3] ^= hash[7];

			this.Hash32 = BitConverter.ToInt32(hash, 0) ^ array.Length;
		}


		public class Comparer : IEqualityComparer<ByteArray>
		{
			public bool Equals(ByteArray x, ByteArray y)
			{
				if (x.Array == y.Array)
					return true;
				if (x.Hash64 == y.Hash64 && x.Array.Length == y.Array.Length)
				{
					for (int i = 0; i < x.Array.Length; i++)
					{
						if (x.Array[i] != y.Array[i])
							return false;
					}
					return true;
				}
				return false;
			}

			public int GetHashCode(ByteArray array)
			{
				return array.Hash32;
			}
		}

	}





	public sealed class BytePool
	{
		private readonly Dictionary<ByteArray, CodeFieldReferenceExpression> pool;

		public BytePool()
		{
			pool = new Dictionary<ByteArray, CodeFieldReferenceExpression>(new ByteArray.Comparer());
		}

		public CodeExpression AddArray(byte[] data)
		{
			ByteArray array = new ByteArray(data);
			CodeFieldReferenceExpression expression;

			if (pool.TryGetValue(array, out expression))
				return expression;

			expression = new CodeFieldReferenceExpression();
			pool.Add(array, expression);

			return expression;
		}

		public void GeneratePool(CodeTypeDeclarationCollection typeList, CompileDirectives compileDirectives)
		{
			Guid nameExt = Guid.NewGuid();
			string name = "pool" + nameExt.ToString("N").ToUpper();

			CodeTypeDeclaration type = new CodeTypeDeclaration(name);

			type.IsClass = true;
			type.TypeAttributes = System.Reflection.TypeAttributes.Class | System.Reflection.TypeAttributes.Sealed;

			CodeExpression typeRef = new CodeTypeReferenceExpression(name);

			typeList.Add(type);

			int index = 0;
			//add all the pools to the type.
			foreach (KeyValuePair<ByteArray, CodeFieldReferenceExpression> item in this.pool)
			{
				//create the static member
				string itemName = "item" + index++;

				//decompressed in code
				CodeExpression dataCode = ShaderBytes.ToArray(item.Key.Array, compileDirectives);

				CodeMemberField field = new CodeMemberField(typeof(byte[]), itemName);
				field.Attributes = MemberAttributes.Static | MemberAttributes.Public | MemberAttributes.Final;

				//assign it inline
				//will be decompressed by the application at load time
				field.InitExpression = dataCode;

				type.Members.Add(field);

				item.Value.FieldName = itemName;
				item.Value.TargetObject = typeRef;
			}
		}
	}
}
