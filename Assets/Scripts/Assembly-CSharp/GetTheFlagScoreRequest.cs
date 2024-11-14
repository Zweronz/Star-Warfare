public class GetTheFlagScoreRequest : Request
{
	protected int playerID;

	public GetTheFlagScoreRequest(int playerID)
	{
		this.playerID = playerID;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(6);
		bytesBuffer.AddByte(136);
		bytesBuffer.AddByte(4);
		bytesBuffer.AddInt(playerID);
		return bytesBuffer.GetBytes();
	}
}
