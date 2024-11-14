using UnityEngine;

public class RobotPlayingState : RobotState
{
	public override void NextState(Robot robot)
	{
		if (robot.sendingTimer.Ready())
		{
			robot.SendInput();
			SendTransformStateRequest request = new SendTransformStateRequest(robot.o.transform.position, robot.o.transform.rotation.ToEuler(), robot.timeMgr.NetworkTime);
			robot.network.SendRequest(request);
			robot.SpawnEnemy();
			robot.sendingTimer.Do();
		}
		robot.Move();
		robot.timeMgr.RobotLoop(robot.network);
		robot.UpdateEnemy();
		float num = Random.Range(40f, 80f);
	}
}
