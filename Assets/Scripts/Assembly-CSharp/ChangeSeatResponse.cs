using UnityEngine;

internal class ChangeSeatResponse : Response
{
	public byte success;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		success = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameObject gameObject = GameObject.Find("MultiMenu");
		if (gameObject != null)
		{
			MultiMenuScript component = gameObject.GetComponent<MultiMenuScript>();
			if (component != null)
			{
				component.SetChangeSeat();
			}
		}
	}

	public override void ProcessRobotLogic(Robot robot)
	{
	}
}
