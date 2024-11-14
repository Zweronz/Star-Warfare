using System;

public class MessageStream
{
	private byte[] _buffer;

	private int _position;

	private int _length;

	private int _capacity;

	public MessageStream()
	{
		_buffer = new byte[0];
		_position = 0;
		_length = 0;
		_capacity = 0;
	}

	private byte ReadByte()
	{
		if (_position >= _length)
		{
			return 0;
		}
		return _buffer[_position++];
	}

	private int ReadInt()
	{
		int num = (_position += 4);
		if (num > _length)
		{
			_position = _length;
			return -1;
		}
		return _buffer[num - 4] | (_buffer[num - 3] << 8) | (_buffer[num - 2] << 16) | (_buffer[num - 1] << 24);
	}

	private byte[] ReadBytes(int count)
	{
		int num = _length - _position;
		if (num > count)
		{
			num = count;
		}
		if (num <= 0)
		{
			return null;
		}
		byte[] array = new byte[num];
		if (num <= 8)
		{
			int num2 = num;
			while (--num2 >= 0)
			{
				array[num2] = _buffer[_position + num2];
			}
		}
		else
		{
			Buffer.BlockCopy(_buffer, _position, array, 0, num);
		}
		_position += num;
		return array;
	}

	public bool Read(out Message message)
	{
		message = null;
		_position = 0;
		if (_length > 2)
		{
			message = new Message();
			message.Class = ReadByte();
			message.Size = ReadByte();
			if (message.Size <= 0 || message.Size <= _length - _position)
			{
				if (message.Size > 0)
				{
					message.Content = ReadBytes(message.Size);
				}
				Remove(message.Size + 2);
				return true;
			}
			message = null;
			return false;
		}
		return false;
	}

	private void EnsureCapacity(int value)
	{
		if (value > _capacity)
		{
			int num = value;
			if (num < 256)
			{
				num = 256;
			}
			if (num < _capacity * 2)
			{
				num = _capacity * 2;
			}
			byte[] array = new byte[num];
			if (_length > 0)
			{
				Buffer.BlockCopy(_buffer, 0, array, 0, _length);
			}
			_buffer = array;
			_capacity = num;
		}
	}

	public void Write(byte[] buffer, int offset, int count)
	{
		if (buffer.Length - offset < count)
		{
			count = buffer.Length - offset;
		}
		EnsureCapacity(buffer.Length + count);
		Array.Clear(_buffer, _length, _capacity - _length);
		Buffer.BlockCopy(buffer, offset, _buffer, _length, count);
		_length += count;
	}

	private void Remove(int count)
	{
		if (_length >= count)
		{
			Buffer.BlockCopy(_buffer, count, _buffer, 0, _length - count);
			_length -= count;
			Array.Clear(_buffer, _length, _capacity - _length);
		}
		else
		{
			_length = 0;
			Array.Clear(_buffer, 0, _capacity);
		}
	}
}
