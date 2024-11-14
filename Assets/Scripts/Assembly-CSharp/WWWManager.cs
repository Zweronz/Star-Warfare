using UnityEngine;

public class WWWManager
{
	private WWWBehaviorScript mBehavior;

	private ReceivedPacketCache mReceivedPacketCache;

	public WWWManager()
	{
		mReceivedPacketCache = new ReceivedPacketCache();
		GameObject gameObject = GameObject.Find("WWWManager");
		if (gameObject == null)
		{
			gameObject = new GameObject("WWWManager");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<WWWBehaviorScript>();
		}
		mBehavior = gameObject.GetComponent<WWWBehaviorScript>();
	}

	public ReceivedPacketCache getReceivedPacketCache()
	{
		return mReceivedPacketCache;
	}

	public void ProcessReceivedPackets()
	{
		while (!mReceivedPacketCache.isEmpty())
		{
			WWWRequest wWWRequest = (WWWRequest)mReceivedPacketCache.RetrivePacket();
			if (wWWRequest != null)
			{
				wWWRequest.ProcessLogic();
			}
		}
	}

	public void SendRequest(WWWRequest request)
	{
		if (GameApp.GetInstance().IsConnectedToInternet() && null != mBehavior)
		{
			mBehavior.SendRequest(request);
		}
	}
}
