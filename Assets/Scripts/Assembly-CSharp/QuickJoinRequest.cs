public class QuickJoinRequest : Request
{
	private byte mRankId;

	private short ping;

	public QuickJoinRequest(byte rankId, short ping)
	{
		mRankId = rankId;
		this.ping = ping;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(5);
		bytesBuffer.AddByte(11);
		bytesBuffer.AddByte(3);
		bytesBuffer.AddByte(mRankId);
		bytesBuffer.AddShort(ping);
		return bytesBuffer.GetBytes();
	}
}
