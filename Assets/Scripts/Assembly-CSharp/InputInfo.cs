using UnityEngine;

public class InputInfo
{
	public bool fire;

	public bool moving;

	public Vector3 moveDirection = Vector3.zero;

	public MoveDirection dir;

	protected TouchInfo lastMoveTouch = new TouchInfo();

	protected TouchInfo lastMoveTouch2 = new TouchInfo();

	public bool IsMoving()
	{
		if (moveDirection.x != 0f || moveDirection.z != 0f)
		{
			return true;
		}
		return false;
	}
}
