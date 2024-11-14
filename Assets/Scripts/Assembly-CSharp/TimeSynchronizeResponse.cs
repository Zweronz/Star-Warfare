internal class TimeSynchronizeResponse : Response
{
	public short id;

	public int time;

	public long timeAtSend;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		id = bytesBuffer.ReadByte();
		time = bytesBuffer.ReadInt();
		timeAtSend = bytesBuffer.ReadLong();
	}

	public override void ProcessLogic()
	{
		TimeManager.GetInstance().Synchronize(time, id, timeAtSend);
	}

	public override void ProcessRobotLogic(Robot robot)
	{
		robot.timeMgr.Synchronize(time, id, 0L);
	}
}
