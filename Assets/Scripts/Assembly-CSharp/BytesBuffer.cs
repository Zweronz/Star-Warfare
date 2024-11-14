using System;
using System.Text;
using UnityEngine;

public class BytesBuffer
{
	protected byte[] bytes;

	protected int offset;

	public BytesBuffer(int length)
	{
		bytes = new byte[length];
	}

	public BytesBuffer(byte[] fromBytes)
	{
		bytes = fromBytes;
	}

	public void AddFloat(float f)
	{
		byte[] array = BitConverter.GetBytes(f);
		Array.Reverse(array);
		array.CopyTo(bytes, offset);
		offset += 4;
	}

	public void AddBool(bool b)
	{
		if (b)
		{
			AddByte(1);
		}
		else
		{
			AddByte(0);
		}
	}

	public void AddByte(byte b)
	{
		bytes[offset] = b;
		offset++;
	}

	public void AddShort(short s)
	{
		byte[] array = BitConverter.GetBytes(s);
		Array.Reverse(array);
		array.CopyTo(bytes, offset);
		offset += 2;
	}

	public void AddInt(int i)
	{
		byte[] array = BitConverter.GetBytes(i);
		Array.Reverse(array);
		array.CopyTo(bytes, offset);
		offset += 4;
	}

	public void AddLong(long i)
	{
		byte[] array = BitConverter.GetBytes(i);
		Array.Reverse(array);
		array.CopyTo(bytes, offset);
		offset += 8;
	}

	public void AddVector3(Vector3 v)
	{
		AddFloat(v.x);
		AddFloat(v.y);
		AddFloat(v.z);
	}

	public void AddBytes(byte[] b)
	{
		b.CopyTo(bytes, offset);
		offset += b.Length;
	}

	public void AddString(string s)
	{
		byte[] array = Encoding.UTF8.GetBytes(s);
		AddByte((byte)array.Length);
		AddBytes(array);
	}

	public static byte GetStringByteLength(string s)
	{
		byte[] array = Encoding.UTF8.GetBytes(s);
		return (byte)(array.Length + 1);
	}

	public static byte GetIntByteLength(int s)
	{
		byte[] array = BitConverter.GetBytes(s);
		return (byte)(array.Length + 1);
	}

	public byte[] GetBytes()
	{
		return bytes;
	}

	public byte ReadByte()
	{
		byte result = bytes[offset];
		offset++;
		return result;
	}

	public bool ReadBool()
	{
		byte b = ReadByte();
		if (b == 1)
		{
			return true;
		}
		return false;
	}

	public short ReadShort()
	{
		byte[] array = new byte[2];
		Array.Copy(bytes, offset, array, 0, 2);
		Array.Reverse(array);
		offset += 2;
		return BitConverter.ToInt16(array, 0);
	}

	public int ReadInt()
	{
		byte[] array = new byte[4];
		Array.Copy(bytes, offset, array, 0, 4);
		Array.Reverse(array);
		offset += 4;
		return BitConverter.ToInt32(array, 0);
	}

	public long ReadLong()
	{
		byte[] array = new byte[8];
		Array.Copy(bytes, offset, array, 0, 8);
		Array.Reverse(array);
		offset += 8;
		return BitConverter.ToInt64(array, 0);
	}

	public string ReadString()
	{
		byte b = ReadByte();
		if (b == 0)
		{
			return null;
		}
		byte[] destinationArray = new byte[b];
		Array.Copy(bytes, offset, destinationArray, 0, b);
		offset += b;
		return Encoding.UTF8.GetString(destinationArray);
	}

	public string ReadStringShortLength()
	{
		short num = ReadShort();
		if (num == 0)
		{
			return null;
		}
		byte[] destinationArray = new byte[num];
		Array.Copy(bytes, offset, destinationArray, 0, num);
		offset += num;
		return Encoding.UTF8.GetString(destinationArray);
	}
}
