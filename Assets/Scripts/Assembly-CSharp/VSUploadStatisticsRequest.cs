public class VSUploadStatisticsRequest : Request
{
	protected short kills;

	protected short death;

	protected short assist;

	protected int score;

	protected int bonus;

	protected int cashRewards;

	protected short secureFlags;

	protected short assistFlags;

	protected short assistVIP;

	protected short giftHitCMI;

	public VSUploadStatisticsRequest(short k, short d, short a, int s, int b, int c, short f, short af, short av, short gh)
	{
		kills = k;
		death = d;
		assist = a;
		score = s;
		bonus = b;
		cashRewards = c;
		secureFlags = f;
		assistFlags = af;
		assistVIP = av;
		giftHitCMI = gh;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(28);
		bytesBuffer.AddByte(130);
		bytesBuffer.AddByte(26);
		bytesBuffer.AddShort(kills);
		bytesBuffer.AddShort(death);
		bytesBuffer.AddShort(assist);
		bytesBuffer.AddInt(score);
		bytesBuffer.AddInt(bonus);
		bytesBuffer.AddInt(cashRewards);
		bytesBuffer.AddShort(secureFlags);
		bytesBuffer.AddShort(assistFlags);
		bytesBuffer.AddShort(assistVIP);
		bytesBuffer.AddShort(giftHitCMI);
		return bytesBuffer.GetBytes();
	}
}
