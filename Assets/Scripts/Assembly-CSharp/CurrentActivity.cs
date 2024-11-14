using UnityEngine;

public class CurrentActivity
{
	private static CurrentActivity activity;

	public readonly AndroidJavaObject JavaObject;

	private CurrentActivity()
	{
		if (activity == null)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			JavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
	}

	public static CurrentActivity getInstance()
	{
		if (activity == null)
		{
			activity = new CurrentActivity();
		}
		return activity;
	}
}
