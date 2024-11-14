public class PlayerOnHitRequest : Request
{
	protected short damage;

	protected bool hasPlayerID;

	protected int playerID;

	public PlayerOnHitRequest(short damage, bool hasPlayerID, int playerID)
	{
		this.damage = damage;
		this.hasPlayerID = hasPlayerID;
		this.playerID = playerID;
	}

	public override byte[] GetBytes()
	{
		byte b = 3;
		if (hasPlayerID)
		{
			b += 4;
		}
		BytesBuffer bytesBuffer = new BytesBuffer(b + 2);
		bytesBuffer.AddByte(110);
		bytesBuffer.AddByte(b);
		bytesBuffer.AddBool(hasPlayerID);
		bytesBuffer.AddShort(damage);
		if (hasPlayerID)
		{
			bytesBuffer.AddInt(playerID);
		}
		return bytesBuffer.GetBytes();
	}
}
