internal class TestRespones : Response
{
	public int test1;

	public int test2;

	public int test3;

	public int test4;

	public int test5;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		for (int i = 0; i < 5; i++)
		{
			test1 = bytesBuffer.ReadInt();
		}
	}

	public override void ProcessLogic()
	{
	}

	public override void ProcessRobotLogic(Robot robot)
	{
	}
}
