internal class RoomTimeSynchronizeResponse : Response
{
	public short id;

	public int time;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		id = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		TimeManager.GetInstance().Synchronize(0, id, 0L);
	}
}
