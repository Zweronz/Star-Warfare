public class CreateVSRoomRequest : Request
{
	public string roomName;

	public short passWord;

	public byte maxPlayer;

	public bool hasPass;

	public byte mode;

	public byte mapID;

	public byte rankID;

	public short ping;

	public byte minJoinRankID;

	public byte maxJoinRankID;

	public bool winCondition;

	public short winValue;

	public bool isAutoBalance;

	public CreateVSRoomRequest(string name, short pass, byte playerNum, bool hasPassword, byte mode, byte mapID, byte rankID, short ping, byte minJoinRankID, byte maxJoinRankID, bool winCondition, short winValue, bool isAutoBalance)
	{
		roomName = name;
		passWord = -1;
		passWord = pass;
		maxPlayer = playerNum;
		hasPass = hasPassword;
		this.mode = mode;
		this.mapID = mapID;
		this.rankID = rankID;
		this.ping = ping;
		this.minJoinRankID = minJoinRankID;
		this.maxJoinRankID = maxJoinRankID;
		this.winCondition = winCondition;
		this.winValue = winValue;
		this.isAutoBalance = isAutoBalance;
	}

	public override byte[] GetBytes()
	{
		byte stringByteLength = BytesBuffer.GetStringByteLength(roomName);
		int num = 15 + stringByteLength;
		if (hasPass)
		{
			num += 2;
		}
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte(18);
		bytesBuffer.AddByte((byte)(num - 2));
		bytesBuffer.AddBool(hasPass);
		bytesBuffer.AddByte(maxPlayer);
		bytesBuffer.AddByte(mode);
		bytesBuffer.AddByte(mapID);
		bytesBuffer.AddByte(rankID);
		bytesBuffer.AddShort(ping);
		bytesBuffer.AddByte(minJoinRankID);
		bytesBuffer.AddByte(maxJoinRankID);
		bytesBuffer.AddBool(winCondition);
		bytesBuffer.AddShort(winValue);
		bytesBuffer.AddBool(isAutoBalance);
		bytesBuffer.AddString(roomName);
		if (hasPass)
		{
			bytesBuffer.AddShort(passWord);
		}
		return bytesBuffer.GetBytes();
	}
}
