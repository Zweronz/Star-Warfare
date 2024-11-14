using UnityEngine;

public class MainScript : MonoBehaviour
{
	protected NetworkManager networkMgr;

	public bool bInit;

	private void Start()
	{
		bInit = false;
	}

	public void Init()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			networkMgr = GameApp.GetInstance().GetNetworkManager();
		}
		GameApp.GetInstance().CreateGameWorld();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
		}
		bInit = true;
	}

	private void Update()
	{
		if (bInit)
		{
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				TimeManager.GetInstance().Loop();
				networkMgr.ProcessReceivedPackets();
			}
			GameApp.GetInstance().Loop(Time.deltaTime);
		}
	}

	private void LateUpdate()
	{
		if (bInit)
		{
			GameApp.GetInstance().LateLoop(Time.deltaTime);
		}
	}
}
