using UnityEngine;

public class RobotIdleState : RobotState
{
	public override void NextState(Robot robot)
	{
		float num = Random.Range(10f, 15f);
		if (robot.id % 2 != 0)
		{
			num = 1f;
		}
		if (!(GetInStateTime() > num))
		{
			return;
		}
		if (robot.id % 2 == 0)
		{
			robot.sendingCreatingRoomRequest = true;
			CreateRoomRequest request = new CreateRoomRequest(robot.userName + "'s room", -1, 2, false, 0, 0, 1, 500, 0, 11);
			robot.network.SendRequest(request);
		}
		else
		{
			robot.sendingJoingRequest = true;
			RobotRoom robotRoom = robot.uiScript.FindIdleRoom();
			if (robotRoom != null)
			{
				byte rankID = 1;
				JoinRoomRequest request2 = new JoinRoomRequest(robotRoom.roomID, rankID, 500);
				robot.network.SendRequest(request2);
			}
		}
		EnterStateTime = Time.time;
	}
}
