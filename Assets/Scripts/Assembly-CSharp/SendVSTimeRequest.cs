public class SendVSTimeRequest : Request
{
	protected int channelID;

	protected float time;

	public SendVSTimeRequest(int channelID, float time)
	{
		this.channelID = channelID;
		this.time = time;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(10);
		bytesBuffer.AddByte(133);
		bytesBuffer.AddByte(8);
		bytesBuffer.AddInt(channelID);
		bytesBuffer.AddFloat(time);
		return bytesBuffer.GetBytes();
	}
}
