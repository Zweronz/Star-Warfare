using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedShotGun : Weapon
{
	protected static int sbulletCount;

	protected Timer shotgunFireTimer;

	protected bool readyforCock;

	private float damageDeductRate = 0.08f;

	protected int reticleWidth;

	protected int reticleHeight;

	public override int BulletCount
	{
		get
		{
			return sbulletCount;
		}
		set
		{
			sbulletCount = value;
		}
	}

	public AdvancedShotGun()
	{
		maxCapacity = 9999;
		base.IsSelectedForBattle = false;
		shotgunFireTimer = new Timer();
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.AdvancedShotGun;
	}

	public override string GetAnimationSuffix()
	{
		return "_grenade_launcher";
	}

	public override string GetAnimationSuffixAlter()
	{
		return "_shotgun";
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
		PlayGunAnimation();
		AudioManager.GetInstance().PlaySoundAt("Audio/shotgun/MORPHEUS", player.GetTransform().position);
		GameObject original = Resources.Load("Effect/update_effect/effect_shot_gun_001") as GameObject;
		gunfireObj = Object.Instantiate(original, gunfire.position, Quaternion.identity) as GameObject;
		gunfireObj.transform.parent = gunfire;
		gunfireObj.transform.rotation = player.GetTransform().rotation;
		gunfireObj.transform.localRotation *= Quaternion.Euler(new Vector3(0f - player.AngleV, 0f, 0f));
	}

	protected void PlayGunAnimation()
	{
		gun.GetComponent<Animation>()["fire"].wrapMode = WrapMode.Once;
		gun.GetComponent<Animation>()["fire"].speed = 1f;
		gun.GetComponent<Animation>().Play("fire");
	}

	public override void GunOn()
	{
		base.GunOn();
		SetWeaponShootSpeed(1f);
	}

	public override void Init(Player player)
	{
		base.Init(player);
		hitForce = 20f;
		damageDeductRate = 50f / range * 0.01f;
		if (base.Name == "TSG-03")
		{
			reticleWidth = 100;
			reticleHeight = 70;
		}
		else if (base.Name == "SD58")
		{
			reticleWidth = 130;
			reticleHeight = 100;
		}
		else if (base.Name == "WD03S")
		{
			reticleWidth = 60;
			reticleHeight = 40;
		}
		else if (base.Name == "S92M")
		{
			reticleWidth = 160;
			reticleHeight = 120;
		}
		else if (base.Name == "T740")
		{
			reticleWidth = 100;
			reticleHeight = 70;
		}
		else
		{
			Debug.Log("Can not find reticle size for this shotgun");
			reticleWidth = 100;
			reticleHeight = 70;
		}
	}

	public override void DamageBoothByArmor()
	{
		base.DamageBoothByArmor();
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.SHOTGUN_BOOTH);
	}

	public override float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return base.GetDamageAfterBoothByArmor(skill) * (1f + skill.GetSkill(SkillsType.SHOTGUN_BOOTH));
	}

	public void PlayPumpAnimation()
	{
	}

	public override void PlayBlankShootSound()
	{
		AudioManager.GetInstance().PlaySoundSingle("Audio/blank/blank_shot02");
	}

	public override void Loop(float deltaTime)
	{
		base.Loop(deltaTime);
	}

	public override void Attack(float deltaTime)
	{
		AudioManager.GetInstance().PlaySoundAt("Audio/shotgun/MORPHEUS", player.GetTransform().position);
		PlayGunAnimation();
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
		GameObject original = Resources.Load("Effect/update_effect/effect_shot_gun_001") as GameObject;
		gunfireObj = Object.Instantiate(original, gunfire.position, Quaternion.identity) as GameObject;
		gunfireObj.transform.parent = gunfire;
		gunfireObj.transform.rotation = player.GetTransform().rotation;
		gunfireObj.transform.localRotation *= Quaternion.Euler(new Vector3(0f - player.AngleV, 0f, 0f));
		lastAttackTime = Time.time;
		SetLastAttackTimeShared();
		readyforCock = true;
		shotgunFireTimer = new Timer();
		shotgunFireTimer.SetTimer(attackFrenquency * 0.7f, false);
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		Hashtable hashtable3 = new Hashtable();
		for (int i = 0; i < 8; i++)
		{
			HashSet<Enemy> hashSet = new HashSet<Enemy>();
			int num = Random.Range(0, reticleWidth) - reticleWidth / 2;
			int num2 = Random.Range(0, reticleHeight) - reticleHeight / 2;
			Vector3 vector = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x + (float)num, (float)Screen.height - gameCamera.ReticlePosition.y + (float)num2, 0.1f));
			Vector3 normalized = (vector - cameraTransform.position).normalized;
			Ray ray = new Ray(cameraTransform.position + normalized * 1.8f, normalized);
			RaycastHit raycastHit = default(RaycastHit);
			RaycastHit[] array = Physics.RaycastAll(ray, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.GIFT));
			float num3 = 1000000f;
			if (array.Length > 0)
			{
				aimTarget = array[array.Length - 1].point;
				raycastHit = array[array.Length - 1];
			}
			else
			{
				aimTarget = raycastHit.point;
			}
			Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
			Vector3 vector2 = gunfire.InverseTransformPoint(aimTarget);
			float num4 = 1000f;
			for (int j = 0; j < array.Length; j++)
			{
				if ((array[j].collider.gameObject.layer == PhysicsLayer.WALL || array[j].collider.gameObject.layer == PhysicsLayer.TRANSPARENT_WALL) && array[j].distance < num4)
				{
					num4 = array[j].distance;
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				Vector3 zero = Vector3.zero;
				if (array[k].distance > num4)
				{
					continue;
				}
				raycastHit = array[k];
				if (GameApp.GetInstance().GetGameMode().IsVSMode() && GameApp.GetInstance().GetGameMode().IsTeamMode() && raycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
				{
					int userID = int.Parse(raycastHit.collider.gameObject.name);
					Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(userID);
					if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
					{
						continue;
					}
				}
				if (!(raycastHit.collider != null))
				{
					continue;
				}
				GameObject enemyByCollider = Enemy.GetEnemyByCollider(raycastHit.collider);
				if (enemyByCollider.name.StartsWith("E_"))
				{
					Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
					if (enemyByID.GetState() == Enemy.DEAD_STATE || hashSet.Contains(enemyByID))
					{
						continue;
					}
					hashSet.Add(enemyByID);
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.hitForce = ray.direction * 2f;
					float num5 = Vector3.Distance(enemyByID.GetTransform().position, player.GetTransform().position);
					if (num5 > range)
					{
						damageProperty.damage = (int)((float)(int)damage * (1f - (num5 - range) * damageDeductRate) * 0.125f);
					}
					else
					{
						damageProperty.damage = (int)(damage * 0.125f);
					}
					if (damageProperty.damage > 0)
					{
						if (hashtable.Contains(enemyByID.EnemyID))
						{
							hashtable[enemyByID.EnemyID] = (int)hashtable[enemyByID.EnemyID] + damageProperty.damage;
						}
						else
						{
							hashtable.Add(enemyByID.EnemyID, damageProperty.damage);
						}
					}
				}
				else if (enemyByCollider.layer == PhysicsLayer.REMOTE_PLAYER)
				{
					if (GameApp.GetInstance().GetGameMode().IsVSMode())
					{
						int num6 = int.Parse(enemyByCollider.name);
						if (hashtable2.Contains(num6))
						{
							hashtable2[num6] = (int)hashtable2[num6] + (int)damage;
						}
						else
						{
							hashtable2.Add(num6, (int)damage);
						}
					}
				}
				else if (enemyByCollider.layer == PhysicsLayer.GIFT)
				{
					int idByName = CMIGift.GetIdByName(enemyByCollider.name);
					if (hashtable3.Contains(idByName))
					{
						hashtable3[idByName] = (int)hashtable3[idByName] + (int)damage;
					}
					else
					{
						hashtable3.Add(idByName, (int)damage);
					}
				}
			}
		}
		foreach (short key in hashtable.Keys)
		{
			Enemy enemyByID2 = GameApp.GetInstance().GetGameWorld().GetEnemyByID("E_" + key);
			bool criticalAttack = false;
			int num8 = Random.Range(0, 100);
			if (num8 < 60)
			{
				criticalAttack = true;
			}
			DamageProperty damageProperty2 = new DamageProperty();
			damageProperty2.hitForce = Vector3.zero;
			damageProperty2.damage = (int)hashtable[key];
			damageProperty2.wType = WeaponType.AdvancedShotGun;
			damageProperty2.hitpoint = enemyByID2.GetTransform().position + Vector3.up;
			damageProperty2.criticalAttack = criticalAttack;
			damageProperty2.isLocal = true;
			enemyByID2.HitEnemy(damageProperty2);
		}
		foreach (int key2 in hashtable3.Keys)
		{
			if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
			{
				int num10 = (int)((float)(int)hashtable3[key2] * 0.35f);
				PlayerHitItemRequest request = new PlayerHitItemRequest((short)num10, key2, false, (byte)GetWeaponType());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		foreach (int key3 in hashtable2.Keys)
		{
			int num12 = (int)((float)(int)hashtable2[key3] * 0.35f);
			PlayerHitPlayerRequest request2 = new PlayerHitPlayerRequest((short)num12, key3, false, (byte)GetWeaponType());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
	}

	public override void GunOff()
	{
		base.GunOff();
		readyforCock = false;
	}
}
