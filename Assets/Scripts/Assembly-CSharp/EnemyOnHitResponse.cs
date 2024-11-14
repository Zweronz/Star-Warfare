using UnityEngine;

internal class EnemyOnHitResponse : Response
{
	protected short m_enemyID;

	protected int m_hp;

	protected byte m_dead;

	protected int m_killerID;

	protected bool m_criticalAttack;

	protected byte m_weaponType;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_enemyID = bytesBuffer.ReadShort();
		m_hp = bytesBuffer.ReadInt();
		m_dead = bytesBuffer.ReadByte();
		if (m_dead == 1)
		{
			m_killerID = bytesBuffer.ReadInt();
			m_criticalAttack = bytesBuffer.ReadBool();
		}
		else
		{
			m_weaponType = bytesBuffer.ReadByte();
		}
	}

	public override void ProcessLogic()
	{
		Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID("E_" + m_enemyID);
		if (enemyByID == null || enemyByID.GetState() == Enemy.DEAD_STATE)
		{
			return;
		}
		enemyByID.HP = m_hp;
		if (enemyByID.HP > 0)
		{
			enemyByID.OnHitResponse();
			if (m_weaponType == 18 || m_weaponType == 38)
			{
				enemyByID.SlowDownEffect();
			}
			if (m_weaponType == 39)
			{
				enemyByID.SetAimEffect();
			}
		}
		if (enemyByID.HP != 0)
		{
			return;
		}
		if (m_killerID == Lobby.GetInstance().GetChannelID())
		{
			Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
			player.RecoveryWhenMakeKills();
		}
		if (m_killerID == Lobby.GetInstance().GetChannelID() || enemyByID.IsBoss())
		{
			if (enemyByID.EnemyType != EnemyType.MainMantis && enemyByID.EnemyType != EnemyType.AssistMantis)
			{
				enemyByID.AddCashAndExpToPlayer();
			}
			else
			{
				CoopMantis coopMantis = enemyByID as CoopMantis;
				if (coopMantis != null && coopMantis.GetCoopMantis() != null && coopMantis.GetCoopMantis().HP == 0)
				{
					coopMantis.AddCashAndExp();
				}
			}
		}
		enemyByID.SetCriticalAttack(m_criticalAttack);
		enemyByID.OnDead();
		enemyByID.SetState(Enemy.DEAD_STATE);
	}

	public override void ProcessRobotLogic(Robot robot)
	{
		if (robot.lobby.IsMasterPlayer && m_hp <= 0)
		{
			robot.RemoveEnemy(m_enemyID);
			Debug.Log("enemy dead:" + m_enemyID);
		}
	}
}
