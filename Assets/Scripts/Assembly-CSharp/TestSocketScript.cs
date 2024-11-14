using UnityEngine;

public class TestSocketScript : MonoBehaviour
{
	public Timer sendTimer = new Timer();

	private NetworkManager nm;

	private void Start()
	{
		TimeManager.GetInstance().Init();
		TimeManager.GetInstance().setPeriod(1f);
		nm = GameApp.GetInstance().CreateNetwork();
		sendTimer.SetTimer(0.2f, false);
	}

	private void Update()
	{
		if (sendTimer.Ready())
		{
			sendTimer.Do();
		}
		TimeManager.GetInstance().Loop();
		nm.ProcessReceivedPackets();
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 10f, 200f, 100f), TimeManager.GetInstance().Ping + string.Empty);
	}

	public void OnApplicationQuit()
	{
		nm.CloseConnection();
	}
}
