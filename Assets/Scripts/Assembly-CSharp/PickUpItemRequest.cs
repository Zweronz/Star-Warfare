public class PickUpItemRequest : Request
{
	protected short sequenceID;

	public PickUpItemRequest(short sequenceID)
	{
		this.sequenceID = sequenceID;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		bytesBuffer.AddByte(120);
		bytesBuffer.AddByte(2);
		bytesBuffer.AddShort(sequenceID);
		return bytesBuffer.GetBytes();
	}
}
