using UnityEngine;

internal class EnemyStateResponse : Response
{
	protected short m_enemyID;

	protected byte state;

	protected short m_x;

	protected short m_y;

	protected short m_z;

	protected short m_sx;

	protected short m_sy;

	protected short m_sz;

	protected int m_targetID;

	protected byte m_targetPointID;

	protected int m_StateIndex;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		state = bytesBuffer.ReadByte();
		m_enemyID = bytesBuffer.ReadShort();
		if (state == 93)
		{
			m_StateIndex = bytesBuffer.ReadInt();
			m_targetID = bytesBuffer.ReadInt();
			return;
		}
		m_x = bytesBuffer.ReadShort();
		m_y = bytesBuffer.ReadShort();
		m_z = bytesBuffer.ReadShort();
		if (state == 3 || state == 5)
		{
			m_sx = bytesBuffer.ReadShort();
			m_sy = bytesBuffer.ReadShort();
			m_sz = bytesBuffer.ReadShort();
		}
		if (state == 45 || state == 46 || state == 47 || state == 48 || state == 49 || state == 50 || state == 51 || state == 52 || state == 53 || state == 54 || state == 55 || state == 56 || state == 10 || state == 11 || state == 12 || state == 13 || state == 15 || state == 18 || state == 16 || state == 20 || state == 19 || state == 21 || state == 17 || state == 30 || state == 31 || state == 32 || state == 33 || state == 34 || state == 35 || state == 36 || state == 61 || state == 62 || state == 63 || state == 80 || state == 81 || state == 82 || state == 83 || state == 84 || state == 85 || state == 86 || state == 87 || state == 88 || state == 89 || state == 90 || state == 91 || state == 92 || state == 94 || state == 95 || state == 96 || state == 97 || state == 98)
		{
			m_targetID = bytesBuffer.ReadInt();
		}
		if (state == 70 || state == 71 || state == 72 || state == 73 || state == 74 || state == 75)
		{
			m_targetPointID = bytesBuffer.ReadByte();
		}
	}

	public override void ProcessLogic()
	{
		if (Lobby.GetInstance().IsMasterPlayer && state != 5)
		{
			return;
		}
		Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID("E_" + m_enemyID);
		if (enemyByID == null)
		{
			return;
		}
		Vector3 position = new Vector3((float)m_x / 10f, (float)m_y / 10f, (float)m_z / 10f);
		Vector3 vector = new Vector3((float)m_sx / 10f, (float)m_sy / 10f, (float)m_sz / 10f);
		if (state == 86)
		{
			enemyByID.GetTransform().position = position;
		}
		else if (state != 93)
		{
			enemyByID.UpdatePosition(position);
		}
		if (state == 1)
		{
			enemyByID.SetState(Enemy.ATTACK_STATE);
		}
		else if (state == 2)
		{
			enemyByID.SetState(Zergling.LOOKAROUND_STATE);
		}
		else if (state == 3)
		{
			Zergling zergling = enemyByID as Zergling;
			if (zergling != null)
			{
				zergling.PlayAnimation(AnimationString.ENEMY_JUMP, WrapMode.ClampForever);
				zergling.speed = vector;
				zergling.SetState(Zergling.JUMP_STATE);
			}
		}
		else if (state == 5)
		{
			enemyByID.SetState(Enemy.GRAVITY_FORCE_STATE);
			enemyByID.SetStartGravityForceTimeNow();
			enemyByID.SetGravityForceTarget(vector);
			enemyByID.StartGravityForceEffect();
		}
		else if (state == 4)
		{
			Tank tank = enemyByID as Tank;
			if (tank != null)
			{
				tank.SetState(Tank.STARTRUSH_STATE);
				tank.OnStartRush();
			}
		}
		else if (state == 45)
		{
			Dragon dragon = enemyByID as Dragon;
			dragon.SetState(Dragon.START_FLY_STATE);
			dragon.ChangeTargetPlayer(m_targetID);
			dragon.LookAtTarget();
			dragon.SetFlyTimeNow();
			dragon.EnableGravity(false);
		}
		else if (state == 46)
		{
			Dragon dragon2 = enemyByID as Dragon;
			dragon2.SetState(Enemy.CATCHING_STATE);
			dragon2.SetCatchingTimeNow();
			dragon2.ChangeTargetPlayer(m_targetID);
			dragon2.LookAtTarget();
		}
		else if (state == 50)
		{
			Dragon dragon3 = enemyByID as Dragon;
			dragon3.SetState(Dragon.START_RUSH_STATE);
			dragon3.ChangeTargetPlayer(m_targetID);
			dragon3.LookAtTarget();
			dragon3.EnableGravity(false);
		}
		else if (state == 47)
		{
			Dragon dragon4 = enemyByID as Dragon;
			dragon4.SetState(Dragon.FLY_STATE);
			dragon4.SetCatchingTimeNow();
			dragon4.ChangeTargetPlayer(m_targetID);
			dragon4.LookAtTarget();
		}
		else if (state == 49)
		{
			Dragon dragon5 = enemyByID as Dragon;
			dragon5.SetState(Dragon.FLY_DIVE_STATE);
			dragon5.ChangeTargetPlayer(m_targetID);
			dragon5.LookAtTarget();
			dragon5.OnDive();
		}
		else if (state == 48)
		{
			Dragon dragon6 = enemyByID as Dragon;
			dragon6.SetState(Dragon.FLY_SHOT_STATE);
			dragon6.ChangeTargetPlayer(m_targetID);
			dragon6.LookAtTarget();
			dragon6.StartFlyShot();
		}
		else if (state == 51)
		{
			Dragon dragon7 = enemyByID as Dragon;
			dragon7.SetState(Dragon.SHOT_STATE);
			dragon7.ChangeTargetPlayer(m_targetID);
			dragon7.LookAtTarget();
			dragon7.StartShot();
		}
		else if (state == 52)
		{
			Dragon dragon8 = enemyByID as Dragon;
			dragon8.SetState(Dragon.LANDING_STATE);
			dragon8.ChangeTargetPlayer(m_targetID);
			dragon8.LookAtTarget();
			dragon8.EnableGravity(true);
		}
		else if (state == 53)
		{
			Dragon dragon9 = enemyByID as Dragon;
			dragon9.SetState(Dragon.PU_STATE);
			dragon9.ChangeTargetPlayer(m_targetID);
			dragon9.LookAtTarget();
			dragon9.StartPu();
		}
		else if (state == 54)
		{
			Dragon dragon10 = enemyByID as Dragon;
			dragon10.SetState(Dragon.FLAME_STATE);
			dragon10.ChangeTargetPlayer(m_targetID);
			dragon10.LookAtTarget();
		}
		else if (state == 55)
		{
			Dragon dragon11 = enemyByID as Dragon;
			dragon11.SetState(Dragon.FLY_FLAME_STATE);
			dragon11.ChangeTargetPlayer(m_targetID);
			dragon11.LookAtTarget();
		}
		else if (state == 56)
		{
			Dragon dragon12 = enemyByID as Dragon;
			dragon12.SetState(Dragon.NORMAL_ATTACK_STATE);
			dragon12.ChangeTargetPlayer(m_targetID);
			dragon12.LookAtTarget();
			dragon12.StartNormalAttack();
		}
		else if (state == 10)
		{
			Mantis mantis = enemyByID as Mantis;
			mantis.SetState(Enemy.CATCHING_STATE);
			mantis.SetCatchingTimeNow();
			mantis.ChangeTargetPlayer(m_targetID);
			mantis.LookAtTarget();
		}
		else if (state == 11)
		{
			Mantis mantis2 = enemyByID as Mantis;
			mantis2.SetState(Mantis.NORMAL_ATTACK_STATE);
			mantis2.ChangeTargetPlayer(m_targetID);
			mantis2.LookAtTarget();
		}
		else if (state == 12)
		{
			Mantis mantis3 = enemyByID as Mantis;
			mantis3.SetState(Mantis.CRITICAL_ATTACK_STATE);
			mantis3.ChangeTargetPlayer(m_targetID);
			mantis3.LookAtTarget();
		}
		else if (state == 13)
		{
			Mantis mantis4 = enemyByID as Mantis;
			mantis4.SetState(Mantis.START_FLY_STATE);
			mantis4.ChangeTargetPlayer(m_targetID);
			mantis4.LookAtTarget();
			mantis4.EnableGravity(false);
		}
		else if (state == 14)
		{
			Mantis mantis5 = enemyByID as Mantis;
			mantis5.SetState(Mantis.FLY_IDLE_STATE);
			mantis5.SetFlyIdleTimeNow();
			mantis5.SetFlyTimeNow();
		}
		else if (state == 15)
		{
			Mantis mantis6 = enemyByID as Mantis;
			mantis6.SetState(Mantis.FLY_STATE);
			mantis6.SetCatchingTimeNow();
			mantis6.ChangeTargetPlayer(m_targetID);
			mantis6.LookAtTarget();
		}
		else if (state == 16)
		{
			Mantis mantis7 = enemyByID as Mantis;
			mantis7.SetState(Mantis.FLY_SHOT_STATE);
			mantis7.ChangeTargetPlayer(m_targetID);
			mantis7.LookAtTarget();
			mantis7.StartFlyShot();
		}
		else if (state == 17)
		{
			Mantis mantis8 = enemyByID as Mantis;
			mantis8.SetState(Mantis.FLY_ATTACK_STATE);
			mantis8.ChangeTargetPlayer(m_targetID);
			mantis8.LookAtTarget();
		}
		else if (state == 18)
		{
			Mantis mantis9 = enemyByID as Mantis;
			mantis9.SetState(Mantis.LANDING_STATE);
			mantis9.ChangeTargetPlayer(m_targetID);
			mantis9.LookAtTarget();
		}
		else if (state == 19)
		{
			Mantis mantis10 = enemyByID as Mantis;
			mantis10.SetState(Mantis.FLY_BOOMERANG_STATE);
			mantis10.ChangeTargetPlayer(m_targetID);
			mantis10.LookAtTarget();
			mantis10.StartFlyBoomerang();
		}
		else if (state == 20)
		{
			Mantis mantis11 = enemyByID as Mantis;
			mantis11.SetState(Mantis.FLY_LASER_STATE);
			mantis11.LaserFireStart();
			mantis11.ChangeTargetPlayer(m_targetID);
			mantis11.LookAtTarget();
		}
		else if (state == 21)
		{
			Mantis mantis12 = enemyByID as Mantis;
			mantis12.SetState(Mantis.FLY_DIVESTART_STATE);
			mantis12.ChangeTargetPlayer(m_targetID);
			mantis12.LookAtTarget();
		}
		else if (state == 30)
		{
			Spider spider = enemyByID as Spider;
			spider.SetState(Enemy.CATCHING_STATE);
			spider.SetCatchingTimeNow();
			spider.ChangeTargetPlayer(m_targetID);
			spider.LookAtTarget();
		}
		else if (state == 31)
		{
			Spider spider2 = enemyByID as Spider;
			spider2.SetState(Spider.NORMAL_ATTACK_STATE);
			spider2.ChangeTargetPlayer(m_targetID);
			spider2.LookAtTarget();
		}
		else if (state == 32)
		{
			Spider spider3 = enemyByID as Spider;
			spider3.SetState(Spider.DOUBLE_ATTACK_STATE);
			spider3.ChangeTargetPlayer(m_targetID);
			spider3.LookAtTarget();
		}
		else if (state == 33)
		{
			Spider spider4 = enemyByID as Spider;
			spider4.SetState(Spider.START_JUMP_STATE);
			spider4.ChangeTargetPlayer(m_targetID);
			spider4.StartJump();
			spider4.LookAtTarget();
		}
		else if (state == 34)
		{
			Spider spider5 = enemyByID as Spider;
			spider5.SetState(Spider.START_RUSH_STATE);
			spider5.ChangeTargetPlayer(m_targetID);
			spider5.StartRush();
			spider5.LookAtTarget();
		}
		else if (state == 35)
		{
			Spider spider6 = enemyByID as Spider;
			spider6.SetState(Spider.SHOT_STATE);
			spider6.ChangeTargetPlayer(m_targetID);
			spider6.LookAtTarget();
		}
		else if (state == 36)
		{
			Spider spider7 = enemyByID as Spider;
			spider7.SetState(Spider.CONTINUOUS_SHOT_STATE);
			spider7.ChangeTargetPlayer(m_targetID);
			spider7.LookAtTarget();
		}
		else if (state == 60)
		{
			AssistMantis assistMantis = enemyByID as AssistMantis;
			assistMantis.SetState(AssistMantis.WATCH_STATE);
		}
		else if (state == 61)
		{
			AssistMantis assistMantis2 = enemyByID as AssistMantis;
			assistMantis2.SetState(AssistMantis.SHOT_STATE);
			assistMantis2.ChangeTargetPlayer(m_targetID);
			assistMantis2.LookAtTarget();
			assistMantis2.StartShot();
		}
		else if (state == 62)
		{
			AssistMantis assistMantis3 = enemyByID as AssistMantis;
			assistMantis3.SetState(AssistMantis.BOOMERANG_STATE);
			assistMantis3.ChangeTargetPlayer(m_targetID);
			assistMantis3.LookAtTarget();
			assistMantis3.StartBoomerang();
		}
		else if (state == 63)
		{
			AssistMantis assistMantis4 = enemyByID as AssistMantis;
			assistMantis4.SetState(AssistMantis.LASER_STATE);
			assistMantis4.ChangeTargetPlayer(m_targetID);
			assistMantis4.LookAtTarget();
			assistMantis4.LaserFireStart();
		}
		else if (state == 70)
		{
			CoopMantis coopMantis = enemyByID as CoopMantis;
			coopMantis.SetState(CoopMantis.JOIN_START_FLY_STATE);
			coopMantis.SetTargetPoint((CoopMantis.TargetPointType)m_targetPointID);
			coopMantis.NeedChangeToCoop = false;
			coopMantis.DisableWallDefent();
			coopMantis.InitCoopAttackCount();
		}
		else if (state == 71)
		{
			CoopMantis coopMantis2 = enemyByID as CoopMantis;
			coopMantis2.SetState(CoopMantis.COOP_START_FLY_STATE);
			coopMantis2.SetTargetPoint((CoopMantis.TargetPointType)m_targetPointID);
			coopMantis2.NeedChangeToCoop = false;
			coopMantis2.InitCoopAttackCount();
		}
		else if (state == 72)
		{
			CoopMantis coopMantis3 = enemyByID as CoopMantis;
			coopMantis3.SetState(CoopMantis.COOP_FLY_STATE);
			coopMantis3.SetTargetPoint((CoopMantis.TargetPointType)m_targetPointID);
			coopMantis3.NeedChangeToCoop = false;
			coopMantis3.SetCoopFlyTimeNow();
			coopMantis3.InitCoopAttackCount();
		}
		else if (state == 73)
		{
			CoopMantis coopMantis4 = enemyByID as CoopMantis;
			coopMantis4.SetState(CoopMantis.COOP_FLY_LASER_STATE);
			coopMantis4.SetTargetPoint((CoopMantis.TargetPointType)m_targetPointID);
			coopMantis4.LaserFireStart();
		}
		else if (state == 74)
		{
			CoopMantis coopMantis5 = enemyByID as CoopMantis;
			coopMantis5.SetState(CoopMantis.COOP_FLY_SHOT_STATE);
			coopMantis5.SetTargetPoint((CoopMantis.TargetPointType)m_targetPointID);
			coopMantis5.StartFlyShot();
		}
		else if (state == 75)
		{
			CoopMantis coopMantis6 = enemyByID as CoopMantis;
			coopMantis6.SetState(CoopMantis.COOP_FLY_START_DIVE_STATE);
			coopMantis6.SetTargetPoint((CoopMantis.TargetPointType)m_targetPointID);
		}
		else if (state == 80)
		{
			Earthworm earthworm = enemyByID as Earthworm;
			earthworm.SetState(Enemy.CATCHING_STATE);
			earthworm.SetCatchingTimeNow();
			earthworm.ChangeTargetPlayer(m_targetID);
			earthworm.LookAtTarget();
		}
		else if (state == 81)
		{
			Earthworm earthworm2 = enemyByID as Earthworm;
			earthworm2.SetState(Earthworm.NORMAL_ATTACK_STATE);
			earthworm2.ChangeTargetPlayer(m_targetID);
			earthworm2.LookAtTargetInNormalAttack();
		}
		else if (state == 85)
		{
			Earthworm earthworm3 = enemyByID as Earthworm;
			earthworm3.SetState(Earthworm.DRILLIN_STATE);
			earthworm3.ChangeTargetPlayer(m_targetID);
			earthworm3.LookAtTarget();
		}
		else if (state == 82)
		{
			Earthworm earthworm4 = enemyByID as Earthworm;
			earthworm4.SetState(Earthworm.START_RUSH_STATE);
			earthworm4.ChangeTargetPlayer(m_targetID);
			earthworm4.StartRush();
			earthworm4.LookAtTarget();
		}
		else if (state == 86)
		{
			Earthworm earthworm5 = enemyByID as Earthworm;
			earthworm5.SetState(Earthworm.START_SUPER_RUSH_STATE);
			earthworm5.ChangeTargetPlayer(m_targetID);
			earthworm5.StartSuperRush();
			earthworm5.LookAtTarget();
		}
		else if (state == 83)
		{
			Earthworm earthworm6 = enemyByID as Earthworm;
			earthworm6.SetState(Earthworm.SHOT_STATE);
			earthworm6.ChangeTargetPlayer(m_targetID);
			earthworm6.LookAtTarget();
		}
		else if (state == 84)
		{
			Earthworm earthworm7 = enemyByID as Earthworm;
			earthworm7.SetState(Earthworm.CONTINUOUS_SHOT_STATE);
			earthworm7.ChangeTargetPlayer(m_targetID);
			earthworm7.LookAtTarget();
		}
		else if (state == 87)
		{
			SatanMachine satanMachine = enemyByID as SatanMachine;
			satanMachine.SetState(SatanMachine.SATANMACHINE_CATCHING_STATE);
			satanMachine.ChangeTargetPlayer(m_targetID);
			satanMachine.LookAtTarget();
		}
		else if (state == 88)
		{
			SatanMachine satanMachine2 = enemyByID as SatanMachine;
			satanMachine2.ChangeTargetPlayer(m_targetID);
			satanMachine2.ReadyToStomp();
		}
		else if (state == 89)
		{
			SatanMachine satanMachine3 = enemyByID as SatanMachine;
			satanMachine3.ChangeTargetPlayer(m_targetID);
			satanMachine3.ReadyToLaunchMissileA();
		}
		else if (state == 90)
		{
			SatanMachine satanMachine4 = enemyByID as SatanMachine;
			satanMachine4.ChangeTargetPlayer(m_targetID);
			satanMachine4.ReadyToLaunchMissileB();
		}
		else if (state == 91)
		{
			SatanMachine satanMachine5 = enemyByID as SatanMachine;
			satanMachine5.ChangeTargetPlayer(m_targetID);
			satanMachine5.ReadyToShotLaser();
		}
		else if (state == 92)
		{
			SatanMachine satanMachine6 = enemyByID as SatanMachine;
			satanMachine6.ChangeTargetPlayer(m_targetID);
			satanMachine6.ReadyToShotGiftBomb();
		}
		else if (state == 93)
		{
			SatanMachine satanMachine7 = enemyByID as SatanMachine;
			satanMachine7.LaunchMissile(m_StateIndex, m_targetID);
		}
		else if (state == 94)
		{
			SatanMachine satanMachine8 = enemyByID as SatanMachine;
			satanMachine8.ChangeTargetPlayer(m_targetID);
			satanMachine8.ReadyToStartRage();
		}
		else if (state == 95)
		{
			SatanMachine satanMachine9 = enemyByID as SatanMachine;
			satanMachine9.ReadyToEndRage();
		}
		else if (state == 96)
		{
			SatanMachine satanMachine10 = enemyByID as SatanMachine;
			satanMachine10.SetState(SatanMachine.IDLE_A_STATE);
		}
		else if (state == 97)
		{
			SatanMachine satanMachine11 = enemyByID as SatanMachine;
			satanMachine11.SetState(SatanMachine.IDLE_B_STATE);
			satanMachine11.IsDoPlayReloadSound = false;
		}
		else if (state == 98)
		{
			SatanMachine satanMachine12 = enemyByID as SatanMachine;
			satanMachine12.ChangeTargetPlayer(m_targetID);
		}
	}
}
