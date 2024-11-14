using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AdMobManager : MonoBehaviour
{
	[method: MethodImpl(32)]
	public static event Action adViewDidReceiveAdEvent;

	[method: MethodImpl(32)]
	public static event Action<string> adViewFailedToReceiveAdEvent;

	[method: MethodImpl(32)]
	public static event Action interstitialDidReceiveAdEvent;

	[method: MethodImpl(32)]
	public static event Action<string> interstitialFailedToReceiveAdEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void adViewDidReceiveAd(string empty)
	{
		if (AdMobManager.adViewDidReceiveAdEvent != null)
		{
			AdMobManager.adViewDidReceiveAdEvent();
		}
	}

	public void adViewFailedToReceiveAd(string error)
	{
		if (AdMobManager.adViewFailedToReceiveAdEvent != null)
		{
			AdMobManager.adViewFailedToReceiveAdEvent(error);
		}
	}

	public void interstitialDidReceiveAd(string empty)
	{
		if (AdMobManager.interstitialDidReceiveAdEvent != null)
		{
			AdMobManager.interstitialDidReceiveAdEvent();
		}
	}

	public void interstitialFailedToReceiveAd(string error)
	{
		if (AdMobManager.interstitialFailedToReceiveAdEvent != null)
		{
			AdMobManager.interstitialFailedToReceiveAdEvent(error);
		}
	}
}
