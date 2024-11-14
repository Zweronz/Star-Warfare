using UnityEngine;

public class IdleState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		InputController inputController = player.inputController;
		player.GetWeapon().StopFire();
		player.GetWeapon().AutoDestructEffect();
		bool flag = player.GetSkills().GetSkill(SkillsType.FLY) > 0f;
		WeaponType weaponType = player.GetWeapon().GetWeaponType();
		if (player.IsLocal())
		{
			if (flag)
			{
				GameApp.GetInstance().GetGameWorld().GetCamera()
					.SetPivotOffset(Vector3.up * 0.25f);
			}
			Sniper sniper = null;
			if (player.GetWeapon().GetWeaponType() == WeaponType.Sniper || player.GetWeapon().GetWeaponType() == WeaponType.AdvancedSniper || player.GetWeapon().GetWeaponType() == WeaponType.RelectionSniper)
			{
				sniper = player.GetWeapon() as Sniper;
			}
			if (sniper == null || sniper.AttackDoneReadyZoomOut())
			{
				if (player.GetWeapon().GetWeaponType() == WeaponType.RocketLauncher || player.GetWeapon().GetWeaponType() == WeaponType.AutoRocketLauncher)
				{
					GameApp.GetInstance().GetGameWorld().GetCamera()
						.ZoomToRPG(deltaTime);
				}
				else
				{
					GameApp.GetInstance().GetGameWorld().GetCamera()
						.ZoomOut(deltaTime);
				}
			}
			player.Move(inputController.inputInfo.moveDirection);
		}
		if (inputController.inputInfo.IsMoving())
		{
			if (flag)
			{
				player.PlayAnimation(AnimationString.GetFlyAniString(inputController.inputInfo.dir), WrapMode.Loop);
				player.PlayAnimation(AnimationString.Fly + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
				AudioManager.GetInstance().StopSound("float_idle");
				AudioManager.GetInstance().PlaySoundSingleAt("Audio/float_move", player.GetTransform().position);
			}
			else
			{
				player.PlayAnimation(AnimationString.Run + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
				player.PlayWalkSound();
			}
		}
		else
		{
			player.ResetRunningPhase();
			if (flag)
			{
				player.PlayAnimation(AnimationString.FlyIdle, WrapMode.Loop);
				player.PlayAnimation(AnimationString.FlyIdle + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
				AudioManager.GetInstance().StopSound("float_move");
				AudioManager.GetInstance().PlaySoundSingleAt("Audio/float_idle", player.GetTransform().position);
			}
			else
			{
				AudioManager.GetInstance().StopSound("walk_crement");
				player.PlayAnimation(AnimationString.Idle + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
			}
		}
		if (inputController.inputInfo.fire && player.GetWeapon().HaveBullets())
		{
			player.SetState(Player.ATTACK_STATE);
		}
	}
}
