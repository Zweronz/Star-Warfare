public class PlayerRebirthRequest : Request
{
	protected byte spawnPointIndex;

	public PlayerRebirthRequest(byte spawnPointIndex)
	{
		this.spawnPointIndex = spawnPointIndex;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(3);
		bytesBuffer.AddByte(125);
		bytesBuffer.AddByte(1);
		bytesBuffer.AddByte(spawnPointIndex);
		return bytesBuffer.GetBytes();
	}
}
