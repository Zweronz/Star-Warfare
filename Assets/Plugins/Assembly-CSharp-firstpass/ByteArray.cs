using System.IO;

public class ByteArray
{
	private MemoryStream stream;

	private BinaryWriter writer;

	public ByteArray()
	{
		stream = new MemoryStream();
		writer = new BinaryWriter(stream);
	}

	public void writeByte(byte value)
	{
		writer.Write(value);
	}

	public byte[] GetAllBytes()
	{
		byte[] array = new byte[stream.Length];
		stream.Position = 0L;
		stream.Read(array, 0, array.Length);
		return array;
	}
}
