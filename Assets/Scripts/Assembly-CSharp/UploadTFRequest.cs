public class UploadTFRequest : Request
{
	private bool bTwitter;

	private bool bFacebook;

	public UploadTFRequest(bool twitter, bool facebook)
	{
		bTwitter = twitter;
		bFacebook = facebook;
	}

	public override byte[] GetBytes()
	{
		byte b = 2;
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(26);
		bytesBuffer.AddByte(b);
		bytesBuffer.AddBool(bTwitter);
		bytesBuffer.AddBool(bFacebook);
		return bytesBuffer.GetBytes();
	}
}
