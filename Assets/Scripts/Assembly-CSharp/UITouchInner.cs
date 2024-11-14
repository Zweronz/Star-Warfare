using UnityEngine;

public struct UITouchInner
{
	public int fingerId;

	public Vector2 position;

	public Vector2 deltaPosition;

	public float deltaTime;

	public int tapCount;

	public TouchPhase phase;
}
