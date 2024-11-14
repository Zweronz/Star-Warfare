using UnityEngine;

public class iPhoneInputMgr
{
	private static UITouchInner[] touches = new UITouchInner[0];

	private static bool buttonDown = false;

	private static Vector2 lastPosition = new Vector2(0f, 0f);

	private static float lastTime = 0f;

	private static int lastFrameCounter = -1;

	public static UITouchInner[] MockTouches()
	{
		return DoMockTouches();
	}

	public static UITouchInner[] DoMockTouches()
	{
		if (Time.frameCount == lastFrameCounter)
		{
			return touches;
		}
		lastFrameCounter = Time.frameCount;
		if (Input.GetMouseButton(0))
		{
			if (buttonDown)
			{
				if (Input.mousePosition.x != lastPosition.x || Input.mousePosition.y != lastPosition.y)
				{
					touches = new UITouchInner[1];
					touches[0].fingerId = 0;
					touches[0].position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					touches[0].deltaPosition = touches[0].position - lastPosition;
					touches[0].deltaTime = Time.time - lastTime;
					touches[0].tapCount = 0;
					touches[0].phase = TouchPhase.Moved;
					lastPosition = touches[0].position;
					lastTime = Time.time;
				}
				else
				{
					touches = new UITouchInner[0];
				}
			}
			else
			{
				touches = new UITouchInner[1];
				touches[0].fingerId = 0;
				touches[0].position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				touches[0].deltaPosition = new Vector2(0f, 0f);
				touches[0].deltaTime = 0f;
				touches[0].tapCount = 0;
				touches[0].phase = TouchPhase.Began;
				buttonDown = true;
				lastPosition = touches[0].position;
				lastTime = Time.time;
			}
		}
		else if (buttonDown)
		{
			touches = new UITouchInner[1];
			touches[0].fingerId = 0;
			touches[0].position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			touches[0].deltaPosition = new Vector2(0f, 0f);
			touches[0].deltaTime = 0f;
			touches[0].tapCount = 0;
			touches[0].phase = TouchPhase.Ended;
			buttonDown = false;
		}
		else
		{
			touches = new UITouchInner[0];
		}
		return touches;
	}
}
