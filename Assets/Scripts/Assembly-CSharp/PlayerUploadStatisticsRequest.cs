public class PlayerUploadStatisticsRequest : Request
{
	protected int killCash;

	protected int pickupCash;

	protected int pickupEnegy;

	protected int comboCash;

	protected int bossCash;

	protected int bossMithril;

	public PlayerUploadStatisticsRequest(int kc, int pc, int pe, int cc, int bc, int bm)
	{
		killCash = kc;
		pickupCash = pc;
		pickupEnegy = pe;
		comboCash = cc;
		bossCash = bc;
		bossMithril = bm;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(26);
		bytesBuffer.AddByte(122);
		bytesBuffer.AddByte(24);
		bytesBuffer.AddInt(killCash);
		bytesBuffer.AddInt(pickupCash);
		bytesBuffer.AddInt(pickupEnegy);
		bytesBuffer.AddInt(comboCash);
		bytesBuffer.AddInt(bossCash);
		bytesBuffer.AddInt(bossMithril);
		return bytesBuffer.GetBytes();
	}
}
