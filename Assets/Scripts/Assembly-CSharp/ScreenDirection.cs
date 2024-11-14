using UnityEngine;

public class ScreenDirection : MonoBehaviour
{
	protected float startTime;

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
	}

	private void ResetDirection()
	{
		if (GameApp.GetInstance().PreviousOrientation != Input.deviceOrientation)
		{
			GameApp.GetInstance().PreviousOrientation = Input.deviceOrientation;
			if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
			{
				Screen.orientation = ScreenOrientation.LandscapeRight;
				AdMobBinding.rotateToOrientation(DeviceOrientation.LandscapeRight);
			}
			else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
			{
				Screen.orientation = ScreenOrientation.LandscapeLeft;
				AdMobBinding.rotateToOrientation(DeviceOrientation.LandscapeLeft);
			}
		}
	}

	private void LateUpdate()
	{
		ResetDirection();
	}
}
