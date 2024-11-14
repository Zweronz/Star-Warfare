internal class SendVSTimeResponse : Response
{
	public float time;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		time = (float)bytesBuffer.ReadInt() * 1f / 100f;
	}

	public override void ProcessLogic()
	{
		VSClock vSClock = Lobby.GetInstance().GetVSClock();
		vSClock.SetCurrentTime(time);
	}
}
