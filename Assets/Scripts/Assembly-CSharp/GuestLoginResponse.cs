using System;
using UnityEngine;

public class GuestLoginResponse : Response
{
	private bool loginSuccess;

	private int channelID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		channelID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		Debug.Log("GuestLoginResponse: " + channelID);
		TimeManager.GetInstance().LastSynTime = Time.time;
		Lobby.GetInstance().SetChannelID(channelID);
		TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
		long mTime = (long)timeSpan.TotalMilliseconds;
		TimeSynchronizeRequest request = new TimeSynchronizeRequest(0, mTime);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		GameObject gameObject = GameObject.Find("StartMenu");
		if (gameObject != null)
		{
			StartMenuScript component = gameObject.GetComponent<StartMenuScript>();
			if (component != null)
			{
				component.CorrectLogin();
				component.GotoMultiMenu();
			}
		}
	}

	public override void ProcessRobotLogic(Robot robot)
	{
	}
}
