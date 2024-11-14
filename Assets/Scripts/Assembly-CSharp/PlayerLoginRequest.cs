public class PlayerLoginRequest : Request
{
	public string userName;

	public string passWord;

	public string version;

	public string udid;

	public int mithril;

	public string platform;

	public override byte[] GetBytes()
	{
		byte b = (byte)(BytesBuffer.GetStringByteLength(userName) + BytesBuffer.GetStringByteLength(passWord) + BytesBuffer.GetStringByteLength(version) + BytesBuffer.GetStringByteLength(udid) + BytesBuffer.GetIntByteLength(mithril) + BytesBuffer.GetStringByteLength(platform));
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(1);
		bytesBuffer.AddByte(b);
		bytesBuffer.AddString(userName);
		bytesBuffer.AddString(passWord);
		bytesBuffer.AddString(version);
		bytesBuffer.AddString(udid);
		bytesBuffer.AddInt(mithril);
		bytesBuffer.AddString(platform);
		return bytesBuffer.GetBytes();
	}
}
