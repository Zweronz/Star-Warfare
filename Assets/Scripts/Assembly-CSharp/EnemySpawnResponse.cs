using UnityEngine;

internal class EnemySpawnResponse : Response
{
	protected byte m_enemyType;

	protected short m_enemyID;

	protected short m_x;

	protected short m_y;

	protected short m_z;

	protected short m_round;

	protected byte m_index;

	protected bool m_elite;

	protected bool m_fromGrave;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_enemyType = bytesBuffer.ReadByte();
		m_enemyID = bytesBuffer.ReadShort();
		m_x = bytesBuffer.ReadShort();
		m_y = bytesBuffer.ReadShort();
		m_z = bytesBuffer.ReadShort();
		m_round = bytesBuffer.ReadShort();
		m_index = bytesBuffer.ReadByte();
		m_elite = bytesBuffer.ReadBool();
		m_fromGrave = bytesBuffer.ReadBool();
	}

	public override void ProcessLogic()
	{
		GameObject gameObject = GameObject.Find("Player");
		if (!(gameObject == null))
		{
			Vector3 spawnPosition = new Vector3((float)m_x / 10f, (float)m_y / 10f, (float)m_z / 10f);
			Enemy enemy = GameApp.GetInstance().GetGameWorld().SpawnEnemy((EnemyType)m_enemyType, m_enemyID, spawnPosition, m_elite);
			if (enemy != null && m_fromGrave)
			{
				enemy.SetInGrave(true);
			}
			if (!Lobby.GetInstance().IsMasterPlayer)
			{
				GameApp.GetInstance().GetGameWorld().EnemyID = m_enemyID;
			}
		}
	}

	public override void ProcessRobotLogic(Robot robot)
	{
		if (robot.lobby.IsMasterPlayer)
		{
			RobotEnemy robotEnemy = new RobotEnemy();
			robotEnemy.o = new GameObject();
			robotEnemy.o.transform.position = robotEnemy.targetPoses[0];
			robotEnemy.o.name = robot.userName + "_E" + m_enemyID;
			robotEnemy.EnemyID = m_enemyID;
			robot.enemyList.Add(m_enemyID, robotEnemy);
		}
	}
}
