using System;
using UnityEngine;

public class AttackState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		InputController inputController = player.inputController;
		Weapon weapon = player.GetWeapon();
		bool flag = player.GetSkills().GetSkill(SkillsType.FLY) > 0f;
		if (player.IsLocal())
		{
			if (flag)
			{
				GameApp.GetInstance().GetGameWorld().GetCamera()
					.SetPivotOffset(Vector3.up * 0.25f);
			}
			if ((player.GetWeapon().GetWeaponType() != WeaponType.Sniper && player.GetWeapon().GetWeaponType() != WeaponType.AdvancedSniper && weapon.GetWeaponType() != WeaponType.RelectionSniper) || player.GetWeapon().CoolDown())
			{
				if (weapon.IsTypeOfLoopShootingWeapon() && weapon.GetWeaponType() != WeaponType.Sword && weapon.GetWeaponType() != WeaponType.AdvancedSword && weapon.GetWeaponType() != WeaponType.LaserGun)
				{
					GameApp.GetInstance().GetGameWorld().GetCamera()
						.ZoomIn(deltaTime);
				}
				else if (player.GetWeapon().GetWeaponType() == WeaponType.RocketLauncher || player.GetWeapon().GetWeaponType() == WeaponType.AutoRocketLauncher)
				{
					GameApp.GetInstance().GetGameWorld().GetCamera()
						.ZoomToRPG(deltaTime);
				}
				else if (player.GetWeapon().GetWeaponType() != WeaponType.Sniper && player.GetWeapon().GetWeaponType() != WeaponType.AdvancedSniper && player.GetWeapon().GetWeaponType() != WeaponType.RelectionSniper)
				{
					GameApp.GetInstance().GetGameWorld().GetCamera()
						.ZoomOut(deltaTime);
				}
			}
			player.Move(inputController.inputInfo.moveDirection);
			if (inputController.inputInfo.IsMoving())
			{
				if (flag)
				{
					AudioManager.GetInstance().StopSound("float_idle");
					AudioManager.GetInstance().PlaySoundSingleAt("Audio/float_move", player.GetTransform().position);
				}
				else
				{
					player.PlayWalkSound();
				}
			}
			else
			{
				player.ResetRunningPhase();
				if (flag)
				{
					AudioManager.GetInstance().StopSound("float_move");
					AudioManager.GetInstance().PlaySoundSingleAt("Audio/float_idle", player.GetTransform().position);
				}
				else
				{
					AudioManager.GetInstance().StopSound("walk_crement");
				}
			}
		}
		player.GetWeapon().PlaySound();
		player.GetWeapon().Loop(deltaTime);
		player.GetWeapon().AutoDestructEffect();
		WrapMode mode = WrapMode.Loop;
		if (!player.GetWeapon().IsTypeOfLoopShootingWeapon())
		{
			mode = WrapMode.ClampForever;
		}
		if (player.IsLocal() && (weapon.GetWeaponType() == WeaponType.Sniper || weapon.GetWeaponType() == WeaponType.AdvancedSniper || weapon.GetWeaponType() == WeaponType.RelectionSniper))
		{
			Sniper sniper = weapon as Sniper;
			if (!inputController.inputInfo.fire)
			{
				if (sniper.AttackDoneReadyZoomOut())
				{
					GameApp.GetInstance().GetGameWorld().GetCamera()
						.ZoomOut(deltaTime);
				}
			}
			else
			{
				player.SniperAttack = false;
			}
		}
		if (player.GetWeapon().CoolDown())
		{
			if (player.IsLocal())
			{
				if (weapon.GetWeaponType() == WeaponType.Sniper || weapon.GetWeaponType() == WeaponType.AdvancedSniper || weapon.GetWeaponType() == WeaponType.RelectionSniper)
				{
					Sniper sniper2 = weapon as Sniper;
					if (inputController.inputInfo.fire || player.SniperAttack)
					{
						sniper2.Aim(deltaTime);
						if (!inputController.inputInfo.IsMoving())
						{
							if (flag)
							{
								player.PlayAnimation(AnimationString.FlyIdle, WrapMode.Loop);
								player.PlayAnimationWithoutBlend(AnimationString.FlyIdle + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
							}
							else
							{
								player.PlayAnimationAllLayers(AnimationString.Idle + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
							}
						}
						else if (flag)
						{
							player.PlayAnimation(AnimationString.GetFlyAniString(inputController.inputInfo.dir), WrapMode.Loop);
							player.PlayAnimationWithoutBlend(AnimationString.Fly + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
						}
						else
						{
							player.PlayAnimationAllLayers(AnimationString.Run + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
						}
						return;
					}
					player.SniperAttack = true;
					player.Attack();
				}
				else
				{
					player.Attack();
				}
			}
			else if (weapon.GetWeaponType() == WeaponType.Sniper || weapon.GetWeaponType() == WeaponType.AdvancedSniper || weapon.GetWeaponType() == WeaponType.RelectionSniper)
			{
				if (inputController.inputInfo.fire)
				{
					if (!inputController.inputInfo.IsMoving())
					{
						if (flag)
						{
							player.PlayAnimation(AnimationString.FlyIdle, WrapMode.Loop);
							player.PlayAnimationWithoutBlend(AnimationString.FlyIdle + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
						}
						else
						{
							player.PlayAnimationAllLayers(AnimationString.Idle + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
						}
					}
					else if (flag)
					{
						player.PlayAnimation(AnimationString.GetFlyAniString(inputController.inputInfo.dir), WrapMode.Loop);
						player.PlayAnimationWithoutBlend(AnimationString.Fly + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
					}
					else
					{
						player.PlayAnimationAllLayers(AnimationString.Run + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
					}
					return;
				}
				float num = (weapon.Adjuster.angleOffsetH - 0.5f) * ((float)Math.PI / 180f);
				if (flag)
				{
					num += (float)Math.PI / 15f;
				}
				player.GetTransform().RotateAround(-Vector3.up, num);
				weapon.CreateTrajectory();
				player.GetTransform().RotateAround(Vector3.up, num);
			}
			else
			{
				float num2 = weapon.Adjuster.angleOffsetH * ((float)Math.PI / 180f);
				if (flag)
				{
					num2 -= (float)Math.PI / 180f;
				}
				player.GetTransform().RotateAround(-Vector3.up, num2);
				weapon.CreateTrajectory();
				player.GetTransform().RotateAround(Vector3.up, num2);
			}
			if (inputController.inputInfo.IsMoving())
			{
				if (flag)
				{
					player.PlayAnimation(AnimationString.GetFlyAniString(inputController.inputInfo.dir), WrapMode.Loop);
				}
				else if (player.GetWeapon().GetWeaponType() != WeaponType.MachineGun && player.GetWeapon().GetWeaponType() != WeaponType.AdvancedMachineGun && player.GetWeapon().GetWeaponType() != WeaponType.Sword && player.GetWeapon().GetWeaponType() != WeaponType.AdvancedSword)
				{
					player.PlayAnimation(AnimationString.Run + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
				}
				if (flag && (player.GetWeapon().GetWeaponType() == WeaponType.MachineGun || player.GetWeapon().GetWeaponType() == WeaponType.AdvancedMachineGun || player.GetWeapon().GetWeaponType() == WeaponType.Sword || player.GetWeapon().GetWeaponType() == WeaponType.AdvancedSword))
				{
					player.PlayAnimation(AnimationString.FlyRunAttack + player.GetWeaponAnimationSuffix(), mode);
				}
				else
				{
					player.PlayAnimation(AnimationString.RunAttack + player.GetWeaponAnimationSuffix(), mode);
				}
			}
			else if (flag)
			{
				if (weapon.GetWeaponType() == WeaponType.Sword || weapon.GetWeaponType() == WeaponType.AdvancedSword)
				{
					player.PlayAnimation("fly_stand_shoot_jian_lower", WrapMode.Loop);
				}
				else
				{
					player.PlayAnimation(AnimationString.FlyIdle, WrapMode.Loop);
				}
				player.PlayAnimation(AnimationString.FlyAttack + player.GetWeaponAnimationSuffix(), mode);
			}
			else
			{
				player.PlayAnimationAllLayers(AnimationString.Attack + player.GetWeaponAnimationSuffix(), mode);
			}
		}
		else if (!player.GetWeapon().IsTypeOfLoopShootingWeapon())
		{
			if (inputController.inputInfo.IsMoving())
			{
				if (!flag && player.GetWeapon().GetWeaponType() != WeaponType.MachineGun && player.GetWeapon().GetWeaponType() != WeaponType.AdvancedMachineGun && player.GetWeapon().GetWeaponType() != WeaponType.Sword && player.GetWeapon().GetWeaponType() != WeaponType.AdvancedSword)
				{
					player.PlayAnimation(AnimationString.Run + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
				}
				string text = AnimationString.RunAttack;
				if (flag && (player.GetWeapon().GetWeaponType() == WeaponType.Sword || player.GetWeapon().GetWeaponType() == WeaponType.AdvancedSword))
				{
					text = AnimationString.FlyRunAttack;
				}
				if (player.IsPlayingAnimation(text + player.GetWeaponAnimationSuffix()) && player.AnimationPlayed(text + player.GetWeaponAnimationSuffix(), 1f))
				{
					if (flag)
					{
						player.PlayAnimationWithoutBlend(AnimationString.Fly + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
					}
					else
					{
						player.PlayAnimationAllLayers(AnimationString.Run + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
					}
				}
			}
			else
			{
				string text2 = AnimationString.Attack;
				if (flag)
				{
					text2 = AnimationString.FlyAttack;
				}
				if (player.IsPlayingAnimation(text2 + player.GetWeaponAnimationSuffix()) && player.AnimationPlayed(text2 + player.GetWeaponAnimationSuffix(), 1f))
				{
					if (flag)
					{
						if (weapon.GetWeaponType() == WeaponType.Sword || weapon.GetWeaponType() == WeaponType.AdvancedSword)
						{
							player.PlayAnimationWithoutBlend(AnimationString.FlyIdle, WrapMode.Loop);
						}
						player.PlayAnimationWithoutBlend(AnimationString.FlyIdle + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
					}
					else
					{
						player.PlayAnimationAllLayers(AnimationString.Idle + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
					}
				}
			}
		}
		if ((!inputController.inputInfo.fire || !weapon.HaveBullets()) && (!player.GetWeapon().GetWaitForAttackAnimationStop() || ((!player.IsPlayingAnimation(AnimationString.Attack + player.GetWeaponAnimationSuffix()) || player.AnimationPlayed(AnimationString.Attack + player.GetWeaponAnimationSuffix(), 1f)) && (!player.IsPlayingAnimation(AnimationString.RunAttack + player.GetWeaponAnimationSuffix()) || player.AnimationPlayed(AnimationString.RunAttack + player.GetWeaponAnimationSuffix(), 1f)) && (!player.IsPlayingAnimation(AnimationString.FlyAttack + player.GetWeaponAnimationSuffix()) || player.AnimationPlayed(AnimationString.FlyAttack + player.GetWeaponAnimationSuffix(), 1f)) && (!player.IsPlayingAnimation(AnimationString.FlyRunAttack + player.GetWeaponAnimationSuffix()) || player.AnimationPlayed(AnimationString.FlyRunAttack + player.GetWeaponAnimationSuffix(), 1f)))))
		{
			player.SetState(Player.IDLE_STATE);
			if (flag)
			{
				player.PlayAnimation(AnimationString.FlyIdle, WrapMode.Loop);
				player.PlayAnimation(AnimationString.FlyIdle + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
			}
			else
			{
				player.PlayAnimationAllLayers(AnimationString.Idle + player.GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
			}
		}
	}
}
