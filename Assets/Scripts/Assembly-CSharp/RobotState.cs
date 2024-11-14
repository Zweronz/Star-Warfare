using UnityEngine;

public abstract class RobotState
{
	public float EnterStateTime;

	public float GetInStateTime()
	{
		return Time.time - EnterStateTime;
	}

	public abstract void NextState(Robot robot);
}
