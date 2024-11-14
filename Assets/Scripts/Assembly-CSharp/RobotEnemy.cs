using UnityEngine;

public class RobotEnemy
{
	public GameObject o;

	public Vector3[] targetPoses = new Vector3[2]
	{
		new Vector3(11.4f, 0f, 14.4f),
		new Vector3(5.5f, 0f, -2.3f)
	};

	public Vector3 targetPos = Vector3.zero;

	protected Timer changeTargetTimer = new Timer();

	protected Timer moveTimer = new Timer();

	protected int currentTargetIndex;

	protected Vector3 lastTarget;

	protected Vector3 dir;

	public short EnemyID { get; set; }

	public RobotEnemy()
	{
		changeTargetTimer.SetTimer(5f, true);
		moveTimer.SetTimer(0.5f, false);
	}

	public void Loop(Robot robot)
	{
		if (changeTargetTimer.Ready())
		{
			currentTargetIndex++;
			if (currentTargetIndex == targetPoses.Length)
			{
				currentTargetIndex = 0;
			}
			lastTarget = targetPoses[currentTargetIndex];
			dir = (lastTarget - o.transform.position).normalized;
			EnemyMoveRequest request = new EnemyMoveRequest(o.transform.position, lastTarget, EnemyID, robot.lobby.GetChannelID(), false);
			robot.network.SendRequest(request);
			changeTargetTimer.Do();
		}
		if (moveTimer.Ready())
		{
			EnemyMoveRequest request2 = new EnemyMoveRequest(o.transform.position, lastTarget, EnemyID, 0, false);
			robot.network.SendRequest(request2);
			moveTimer.Do();
		}
		o.transform.Translate(dir * 4f * Time.deltaTime, Space.World);
	}
}
