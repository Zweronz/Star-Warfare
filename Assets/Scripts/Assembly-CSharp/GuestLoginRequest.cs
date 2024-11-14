public class GuestLoginRequest : Request
{
	protected string userName;

	protected int userID;

	protected byte userType;

	protected long bossDate;

	protected short bossKillTime;

	protected short bossDropMithrilTime;

	public GuestLoginRequest(string userName, int userID, byte userType, long bossDate, short bossKillTime, short bossDropMithrilTime)
	{
		this.userName = userName;
		this.userID = userID;
		this.userType = userType;
		this.bossDate = bossDate;
		this.bossKillTime = bossKillTime;
		this.bossDropMithrilTime = bossDropMithrilTime;
	}

	public override byte[] GetBytes()
	{
		byte b = (byte)(BytesBuffer.GetStringByteLength(userName) + 17);
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(25);
		bytesBuffer.AddByte(b);
		bytesBuffer.AddInt(userID);
		bytesBuffer.AddString(userName);
		bytesBuffer.AddByte(userType);
		bytesBuffer.AddLong(bossDate);
		bytesBuffer.AddShort(bossKillTime);
		bytesBuffer.AddShort(bossDropMithrilTime);
		return bytesBuffer.GetBytes();
	}
}
