public class PlayerUseItemRequest : Request
{
	protected byte bagIndex;

	protected byte itemID;

	protected byte buffValue;

	public PlayerUseItemRequest(byte bagIndex, byte itemID, byte buffValue)
	{
		this.bagIndex = bagIndex;
		this.itemID = itemID;
		this.buffValue = buffValue;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(5);
		bytesBuffer.AddByte(117);
		bytesBuffer.AddByte(3);
		bytesBuffer.AddByte(bagIndex);
		bytesBuffer.AddByte(itemID);
		bytesBuffer.AddByte(buffValue);
		return bytesBuffer.GetBytes();
	}
}
