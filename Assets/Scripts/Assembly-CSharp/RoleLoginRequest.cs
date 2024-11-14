public class RoleLoginRequest : Request
{
	protected string userName;

	protected int userID;

	protected byte userType;

	public RoleLoginRequest(string userName, int userID, byte userType)
	{
		this.userName = userName;
		this.userID = userID;
		this.userType = userType;
	}

	public override byte[] GetBytes()
	{
		byte b = (byte)(BytesBuffer.GetStringByteLength(userName) + 5);
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(12);
		bytesBuffer.AddByte(b);
		bytesBuffer.AddInt(userID);
		bytesBuffer.AddString(userName);
		bytesBuffer.AddByte(userType);
		return bytesBuffer.GetBytes();
	}
}
