public class SearchRoomRequest : Request
{
	public string mSearchText;

	public SearchRoomRequest(string text)
	{
		mSearchText = text;
	}

	public override byte[] GetBytes()
	{
		byte stringByteLength = BytesBuffer.GetStringByteLength(mSearchText);
		int num = 2 + stringByteLength;
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte(10);
		bytesBuffer.AddByte((byte)(num - 2));
		bytesBuffer.AddString(mSearchText);
		return bytesBuffer.GetBytes();
	}
}
