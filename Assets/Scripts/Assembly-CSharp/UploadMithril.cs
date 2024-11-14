public class UploadMithril : Request
{
	protected UserState userState;

	public UploadMithril(UserState userState)
	{
		this.userState = userState;
	}

	public override byte[] GetBytes()
	{
		byte b = 4;
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(16);
		bytesBuffer.AddByte(b);
		userState.WriteMithril(bytesBuffer);
		return bytesBuffer.GetBytes();
	}
}
