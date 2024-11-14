using UnityEngine;

public class UIDraggableListItem : MonoBehaviour
{
	private Vector3 mTargetPosition;

	private Vector2 mMoveSpeed;

	private float mWidth;

	private float mHeight;

	public void SetSize(float width, float heigth)
	{
		mWidth = width;
		mHeight = heigth;
	}

	public void SetTargetPosition(Vector3 targetPos, Vector2 moveSpeed)
	{
		mTargetPosition = targetPos;
		mMoveSpeed = new Vector2(moveSpeed.x, moveSpeed.y);
	}

	public void UpdatePosition()
	{
		Vector3 localPosition = base.transform.localPosition;
		float deltaValue = GetDeltaValue(localPosition.x, mTargetPosition.x, mMoveSpeed.x);
		float deltaValue2 = GetDeltaValue(localPosition.y, mTargetPosition.y, mMoveSpeed.y);
		base.transform.localPosition += new Vector3(deltaValue, deltaValue2, 0f);
	}

	private float GetDeltaValue(float localValue, float targetValue, float stepValue)
	{
		if (localValue > targetValue)
		{
			return (!(stepValue > localValue - targetValue)) ? (0f - stepValue) : (targetValue - localValue);
		}
		if (localValue < targetValue)
		{
			return (!(stepValue > targetValue - localValue)) ? stepValue : (targetValue - localValue);
		}
		return 0f;
	}

	public void MoveToTarget()
	{
		base.transform.localPosition = mTargetPosition;
	}

	public bool IsHigherThan(float top)
	{
		return base.transform.localPosition.y - mHeight / 2f > top;
	}

	public bool IsLowerThan(float bottom)
	{
		return base.transform.localPosition.y + mHeight / 2f < bottom;
	}
}
