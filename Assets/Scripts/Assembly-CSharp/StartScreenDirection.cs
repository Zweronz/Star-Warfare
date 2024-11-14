using UnityEngine;

public class StartScreenDirection : MonoBehaviour
{
	protected float startTime;

	protected int state;

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		ResetDirection();
	}

	private void ResetDirection()
	{
		if (state == 0 && GameApp.GetInstance().PreviousOrientation == DeviceOrientation.Portrait)
		{
			Screen.orientation = ScreenOrientation.LandscapeRight;
			state = 1;
		}
		if (state == 1)
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			state = 2;
		}
		GameApp.GetInstance().PreviousOrientation = Input.deviceOrientation;
		if (state == 2 || GameApp.GetInstance().PreviousOrientation != DeviceOrientation.Portrait)
		{
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
	}
}
