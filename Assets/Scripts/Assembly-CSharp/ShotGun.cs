using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Weapon
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

	public ShotGun()
	{
		maxCapacity = 9999;
		base.IsSelectedForBattle = false;
		shotgunFireTimer = new Timer();
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.ShotGun;
	}

	public override string GetAnimationSuffix()
	{
		return "_shotgun";
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
		AudioManager.GetInstance().PlaySoundAt("Audio/shotgun/" + base.Name + "_FiringSound", player.GetTransform().position);
		GameObject original = Resources.Load("Effect/bug_shotgun_fire") as GameObject;
		gunfireObj = Object.Instantiate(original, gunfire.position, Quaternion.identity) as GameObject;
		gunfireObj.transform.parent = gunfire;
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
		AudioManager.GetInstance().PlaySoundAt("Audio/shotgun/" + base.Name + "_FiringSound", player.GetTransform().position);
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
		GameObject original = Resources.Load("Effect/bug_shotgun_fire") as GameObject;
		gunfireObj = Object.Instantiate(original, gunfire.position, Quaternion.identity) as GameObject;
		gunfireObj.transform.parent = gunfire;
		SetLastAttackTimeShared();
		lastAttackTime = Time.time;
		readyforCock = true;
		shotgunFireTimer = new Timer();
		shotgunFireTimer.SetTimer(attackFrenquency * 0.7f, false);
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		for (int i = 0; i < 8; i++)
		{
			HashSet<Enemy> hashSet = new HashSet<Enemy>();
			int num = Random.Range(0, reticleWidth) - reticleWidth / 2;
			int num2 = Random.Range(0, reticleHeight) - reticleHeight / 2;
			Vector3 vector = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x + (float)num, (float)Screen.height - gameCamera.ReticlePosition.y + (float)num2, 0.1f));
			Vector3 normalized = (vector - cameraTransform.position).normalized;
			Ray ray = new Ray(cameraTransform.position + normalized * 1.8f, normalized);
			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.GIFT)))
			{
				continue;
			}
			if (GameApp.GetInstance().GetGameMode().IsVSMode() && GameApp.GetInstance().GetGameMode().IsTeamMode() && hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				int userID = int.Parse(hitInfo.collider.gameObject.name);
				Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(userID);
				if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
				{
					continue;
				}
			}
			if (!(hitInfo.collider != null))
			{
				continue;
			}
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(hitInfo.collider);
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
				float num3 = Vector3.Distance(enemyByID.GetTransform().position, player.GetTransform().position);
				if (num3 > range)
				{
					damageProperty.damage = (int)((float)(int)damage * (1f - (num3 - range) * damageDeductRate) * 0.125f);
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
					int num4 = int.Parse(enemyByCollider.name);
					if (hashtable2.Contains(num4))
					{
						hashtable2[num4] = (int)hashtable2[num4] + (int)damage;
					}
					else
					{
						hashtable2.Add(num4, (int)damage);
					}
				}
			}
			else if (enemyByCollider.layer == PhysicsLayer.GIFT && GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
			{
				int idByName = CMIGift.GetIdByName(enemyByCollider.name);
				PlayerHitItemRequest request = new PlayerHitItemRequest((short)damage, idByName, false, (byte)GetWeaponType());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		foreach (short key in hashtable.Keys)
		{
			Enemy enemyByID2 = GameApp.GetInstance().GetGameWorld().GetEnemyByID("E_" + key);
			DamageProperty damageProperty2 = new DamageProperty();
			damageProperty2.hitForce = Vector3.zero;
			damageProperty2.damage = (int)hashtable[key];
			bool criticalAttack = false;
			int num6 = Random.Range(0, 100);
			if (num6 < 60)
			{
				criticalAttack = true;
			}
			damageProperty2.wType = WeaponType.ShotGun;
			damageProperty2.hitpoint = enemyByID2.GetTransform().position + Vector3.up;
			damageProperty2.criticalAttack = criticalAttack;
			damageProperty2.isLocal = true;
			enemyByID2.HitEnemy(damageProperty2);
		}
		foreach (int key2 in hashtable2.Keys)
		{
			int num8 = (int)((float)(int)hashtable2[key2] * 0.35f);
			PlayerHitPlayerRequest request2 = new PlayerHitPlayerRequest((short)num8, key2, false, (byte)GetWeaponType());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
	}

	public override void GunOff()
	{
		base.GunOff();
		readyforCock = false;
	}
}
