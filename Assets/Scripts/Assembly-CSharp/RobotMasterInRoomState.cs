using UnityEngine;

public class RobotMasterInRoomState : RobotState
{
	public override void NextState(Robot robot)
	{
		if (GetInStateTime() > 4f)
		{
			if (!robot.sendingStartGameRequest && robot.joinedPlayer == robot.maxPlayer)
			{
				robot.sendingStartGameRequest = true;
				StartGameRequest request = new StartGameRequest();
				robot.network.SendRequest(request);
				Debug.Log("Robot " + robot.userName + " start a game");
			}
			EnterStateTime = Time.time;
		}
	}
}
