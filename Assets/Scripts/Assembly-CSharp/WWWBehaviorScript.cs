using UnityEngine;

public class WWWBehaviorScript : MonoBehaviour
{
	protected WWWManager mWWWManager;

	private void Start()
	{
		mWWWManager = GameApp.GetInstance().GetWWWManager();
	}

	private void Update()
	{
		mWWWManager.ProcessReceivedPackets();
	}

	public void SendRequest(WWWRequest request)
	{
		if (GameApp.GetInstance().IsConnectedToInternet())
		{
			StartCoroutine(request.Send());
		}
	}
}
