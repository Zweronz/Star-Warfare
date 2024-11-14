using UnityEngine;

internal class EnemyMoveResponse : Response
{
	protected short m_enemyID;

	protected short m_x;

	protected short m_y;

	protected short m_z;

	protected short m_tx;

	protected short m_ty;

	protected short m_tz;

	protected int m_targetID;

	protected bool m_fly;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_enemyID = bytesBuffer.ReadShort();
		m_x = bytesBuffer.ReadShort();
		m_y = bytesBuffer.ReadShort();
		m_z = bytesBuffer.ReadShort();
		m_tx = bytesBuffer.ReadShort();
		m_ty = bytesBuffer.ReadShort();
		m_tz = bytesBuffer.ReadShort();
		m_targetID = bytesBuffer.ReadInt();
		m_fly = bytesBuffer.ReadBool();
	}

	public override void ProcessLogic()
	{
		if (Lobby.GetInstance().IsMasterPlayer)
		{
			return;
		}
		Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID("E_" + m_enemyID);
		if (enemyByID != null)
		{
			Vector3 position = new Vector3((float)m_x / 10f, (float)m_y / 10f, (float)m_z / 10f);
			Vector3 target = new Vector3((float)m_tx / 10f, (float)m_ty / 10f, (float)m_tz / 10f);
			enemyByID.GetTransform().position = position;
			enemyByID.ChangeTarget(target, m_targetID, false);
			if (enemyByID.GetState() == Enemy.GRAVEBORN_STATE)
			{
				enemyByID.SetInGrave(false);
			}
			if (!m_fly)
			{
				enemyByID.SetState(Enemy.CATCHING_STATE);
			}
		}
	}
}
