using UnityEngine;

internal class EnemyShotResponse : Response
{
	protected byte m_enemyType;

	protected short m_x;

	protected short m_y;

	protected short m_z;

	protected short m_sx;

	protected short m_sy;

	protected short m_sz;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_enemyType = bytesBuffer.ReadByte();
		m_x = bytesBuffer.ReadShort();
		m_y = bytesBuffer.ReadShort();
		m_z = bytesBuffer.ReadShort();
		m_sx = bytesBuffer.ReadShort();
		m_sy = bytesBuffer.ReadShort();
		m_sz = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		Vector3 position = new Vector3((float)m_x / 10f, (float)m_y / 10f, (float)m_z / 10f);
		Vector3 vector = new Vector3((float)m_sx / 10f, (float)m_sy / 10f, (float)m_sz / 10f);
		string path = "Effect/Scorpion_Shot";
		switch ((EnemyType)m_enemyType)
		{
		case EnemyType.Scorpion:
			path = "Effect/Scorpion_Shot";
			break;
		case EnemyType.Mutalisk:
			path = "Effect/Mutalisk/Mutalisk_Shot";
			break;
		case EnemyType.Reaver:
			path = "Effect/Reaver/Reaver_Shot";
			break;
		case EnemyType.Dragon:
			path = "Effect/Dragonball";
			break;
		}
		GameObject original = Resources.Load(path) as GameObject;
		GameObject gameObject = Object.Instantiate(original, position, Quaternion.LookRotation(-vector)) as GameObject;
		EnemyShotScript component = gameObject.GetComponent<EnemyShotScript>();
		component.enemyType = (EnemyType)m_enemyType;
		switch ((EnemyType)m_enemyType)
		{
		case EnemyType.Scorpion:
			component.trType = TrajectoryType.Parabola;
			component.damageType = DamageType.Normal;
			component.attackDamage = Scorpion.ShotAttackDamage;
			break;
		case EnemyType.Mutalisk:
			component.trType = TrajectoryType.Straight;
			component.damageType = DamageType.Sputtering;
			component.attackDamage = Mutalisk.ShotAttackDamage;
			component.areaDamage = Mutalisk.ShotAreaDamage;
			component.explodeRadius = Mutalisk.ShotExplodeRadius;
			break;
		case EnemyType.Reaver:
			component.trType = TrajectoryType.Parabola;
			component.damageType = DamageType.Explosion;
			component.attackDamage = Reaver.ShotAttackDamage;
			component.areaDamage = Reaver.ShotAreaDamage;
			component.explodeRadius = Reaver.ShotExplodeRadius;
			break;
		}
		component.speed = vector;
	}
}
