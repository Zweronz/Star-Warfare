public class PlayerJoinTeamStartGameRequest : Request
{
	public override byte[] GetBytes()
	{
		byte b = 0;
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(19);
		bytesBuffer.AddByte(b);
		return bytesBuffer.GetBytes();
	}
}
