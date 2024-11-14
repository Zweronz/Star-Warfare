using UnityEngine;

public class SatanMachineWhirlingAttackState : EnemyState
{
	private float startTime;

	private GameObject effect;

	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		SatanMachine satanMachine = enemy as SatanMachine;
		if (satanMachine == null)
		{
			return;
		}
		if (!satanMachine.IsDoWhirlingAttackBegin)
		{
			string sATANMACHINE_WHIRLING_ATTACK_BEGIN = AnimationString.SATANMACHINE_WHIRLING_ATTACK_BEGIN;
			satanMachine.PlayAnimation(sATANMACHINE_WHIRLING_ATTACK_BEGIN, WrapMode.ClampForever, 1f);
			if (satanMachine.AnimationPlayed(sATANMACHINE_WHIRLING_ATTACK_BEGIN, 1f))
			{
				satanMachine.IsDoWhirlingAttackBegin = true;
				startTime = Time.time;
				GameObject original = Resources.Load("Effect/SatanMachine/xmas_rotate") as GameObject;
				effect = Object.Instantiate(original, satanMachine.whirlingAttackCollider.gameObject.transform.position, Quaternion.identity) as GameObject;
				effect.transform.parent = satanMachine.whirlingAttackCollider.gameObject.transform;
			}
		}
		else if (!satanMachine.IsDoWhirlingAttackProcess)
		{
			string sATANMACHINE_WHIRLING_ATTACK_PROCESS = AnimationString.SATANMACHINE_WHIRLING_ATTACK_PROCESS;
			satanMachine.PlayAnimation(sATANMACHINE_WHIRLING_ATTACK_PROCESS, WrapMode.Loop, 1f);
			if (Time.time - startTime > 10f)
			{
				satanMachine.IsDoWhirlingAttackProcess = true;
				Object.Destroy(effect);
				effect = null;
				return;
			}
			enemy.DoMove(deltaTime);
			if (satanMachine.CheckWhirlingAttackHit() && GameApp.GetInstance().GetGameWorld().GetRemotePlayers()
				.Count == 0)
			{
				satanMachine.IsDoWhirlingAttackProcess = true;
				Object.Destroy(effect);
				effect = null;
			}
			if (!AudioManager.GetInstance().IsPlaying("xiaochouxiaosheng"))
			{
				satanMachine.PlaySound("Audio/enemy/SatanMachine/xiaochouxiaosheng");
			}
		}
		else
		{
			if (satanMachine.IsDoWhirlingAttackEnd)
			{
				return;
			}
			string sATANMACHINE_WHIRLING_ATTACK_END = AnimationString.SATANMACHINE_WHIRLING_ATTACK_END;
			satanMachine.PlayAnimation(sATANMACHINE_WHIRLING_ATTACK_END, WrapMode.ClampForever, 1f);
			if (satanMachine.AnimationPlayed(sATANMACHINE_WHIRLING_ATTACK_END, 1f))
			{
				satanMachine.IsDoWhirlingAttackEnd = true;
				if (satanMachine.IsRageEnd())
				{
					satanMachine.ReadyToEndRage();
				}
				else
				{
					satanMachine.ReadyToThrowBall();
				}
			}
		}
	}
}
