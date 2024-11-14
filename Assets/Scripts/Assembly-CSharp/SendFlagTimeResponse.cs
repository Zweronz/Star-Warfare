internal class SendFlagTimeResponse : Response
{
	public float time;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		time = (float)bytesBuffer.ReadInt() * 1f / 100f;
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			VSClock flagClock = gameWorld.FlagClock;
			if (flagClock != null)
			{
				flagClock.SetCurrentTime(time);
			}
		}
	}
}
