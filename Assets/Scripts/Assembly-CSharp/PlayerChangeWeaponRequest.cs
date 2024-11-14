public class PlayerChangeWeaponRequest : Request
{
	protected byte bagIndex;

	public PlayerChangeWeaponRequest(byte bagIndex)
	{
		this.bagIndex = bagIndex;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(3);
		bytesBuffer.AddByte(111);
		bytesBuffer.AddByte(1);
		bytesBuffer.AddByte(bagIndex);
		return bytesBuffer.GetBytes();
	}
}
