using UnityEngine;

public class AdMobEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		AdMobManager.adViewDidReceiveAdEvent += adViewDidReceiveAdEvent;
		AdMobManager.adViewFailedToReceiveAdEvent += adViewFailedToReceiveAdEvent;
		AdMobManager.interstitialDidReceiveAdEvent += interstitialDidReceiveAdEvent;
		AdMobManager.interstitialFailedToReceiveAdEvent += interstitialFailedToReceiveAdEvent;
	}

	private void OnDisable()
	{
		AdMobManager.adViewDidReceiveAdEvent -= adViewDidReceiveAdEvent;
		AdMobManager.adViewFailedToReceiveAdEvent -= adViewFailedToReceiveAdEvent;
		AdMobManager.interstitialDidReceiveAdEvent -= interstitialDidReceiveAdEvent;
		AdMobManager.interstitialFailedToReceiveAdEvent -= interstitialFailedToReceiveAdEvent;
	}

	private void adViewDidReceiveAdEvent()
	{
		Debug.Log("adViewDidReceiveAdEvent");
	}

	private void adViewFailedToReceiveAdEvent(string error)
	{
		Debug.Log("adViewFailedToReceiveAdEvent: " + error);
	}

	private void interstitialDidReceiveAdEvent()
	{
		Debug.Log("interstitialDidReceiveAdEvent");
	}

	private void interstitialFailedToReceiveAdEvent(string error)
	{
		Debug.Log("interstitialFailedToReceiveAdEvent: " + error);
	}
}
