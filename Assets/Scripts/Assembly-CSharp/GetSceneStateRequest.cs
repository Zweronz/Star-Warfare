public class GetSceneStateRequest : Request
{
	public int playerHp;

	public GetSceneStateRequest(int hp)
	{
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			playerHp = hp;
		}
		else
		{
			playerHp = hp;
		}
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(6);
		bytesBuffer.AddByte(101);
		bytesBuffer.AddByte(4);
		bytesBuffer.AddInt(playerHp);
		return bytesBuffer.GetBytes();
	}
}
