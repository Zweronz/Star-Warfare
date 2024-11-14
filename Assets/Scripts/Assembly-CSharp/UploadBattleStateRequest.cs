public class UploadBattleStateRequest : Request
{
	protected UserState userState;

	public UploadBattleStateRequest(UserState userState)
	{
		this.userState = userState;
	}

	public override byte[] GetBytes()
	{
		byte b = 88;
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(22);
		bytesBuffer.AddByte(b);
		userState.WriteBattleState(bytesBuffer);
		return bytesBuffer.GetBytes();
	}
}
