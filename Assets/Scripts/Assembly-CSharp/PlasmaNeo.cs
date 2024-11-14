using System;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaNeo : Weapon
{
	protected ObjectPool firelineObjectPool;

	protected ObjectPool sparksObjectPool;

	protected List<PlasmaHitInfo> hitPosition = new List<PlasmaHitInfo>();

	public override WeaponType GetWeaponType()
	{
		return WeaponType.PlasmaNeo;
	}

	public override string GetAnimationSuffix()
	{
		return "_rifle";
	}

	public override string GetAnimationSuffixAlter()
	{
		return "_rifle";
	}

	public override void Init(Player player)
	{
		base.Init(player);
		firelineObjectPool = new ObjectPool();
		GameObject prefab = Resources.Load("Effect/update_effect/effect_elc_gun_001") as GameObject;
		firelineObjectPool.Init("PlasmoFirelines", prefab, 18, 0.8f);
		sparksObjectPool = new ObjectPool();
		GameObject prefab2 = Resources.Load("Effect/update_effect/effect_elc_gun_002") as GameObject;
		sparksObjectPool.Init("Sparks", prefab2, 20, 0.5f);
		UnityEngine.Object original = Resources.Load("Effect/update_effect/effect_elc_gun_000");
		gunfireObj = (GameObject)UnityEngine.Object.Instantiate(original, gunfire.position, Quaternion.identity);
		gunfireObj.transform.parent = gunfire;
		playSoundTimer.SetTimer(attackFrenquency / 2f, true);
		StopFire();
	}

	public override void DamageBoothByArmor()
	{
		base.DamageBoothByArmor();
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.PLASMA_BOOTH);
	}

	public override float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return base.GetDamageAfterBoothByArmor(skill) * (1f + skill.GetSkill(SkillsType.PLASMA_BOOTH));
	}

	public override void Loop(float deltaTime)
	{
		for (int num = hitPosition.Count - 1; num >= 0; num--)
		{
			if (Time.time - hitPosition[num].lastExplodeTime > 0.2f)
			{
				Vector3 point = hitPosition[num].pos + new Vector3(Random.Range(-3, 3), Random.Range(-1, 1), Random.Range(-3, 3));
				hitPosition[num].count++;
				ExplodeAtPoint(point, (int)splashDamage, hitPosition[num].count);
				hitPosition[num].lastExplodeTime = Time.time;
				if (Time.time - hitPosition[num].time > splashDuration)
				{
					hitPosition.RemoveAt(num);
				}
			}
		}
	}

	public override void AutoDestructEffect()
	{
		if (firelineObjectPool != null)
		{
			firelineObjectPool.AutoDestruct();
		}
		if (sparksObjectPool != null)
		{
			sparksObjectPool.AutoDestruct();
		}
	}

	public override void StopFire()
	{
		if (gunfireObj != null)
		{
			gunfireObj.SetActiveRecursively(false);
		}
		if (firelineObjectPool != null)
		{
			firelineObjectPool.DestructAll();
		}
		if (sparksObjectPool != null)
		{
			sparksObjectPool.DestructAll();
		}
	}

	public override void PlaySound()
	{
		if (playSoundTimer.Ready())
		{
			AudioManager.GetInstance().PlaySoundAt("Audio/specialweapon/plasma_gun", player.GetTransform().position);
			playSoundTimer.Do();
		}
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
		Ray ray = new Ray(gunfire.transform.position, player.GetTransform().forward);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR)))
		{
			aimTarget = hitInfo.point;
		}
		else
		{
			aimTarget = cameraTransform.TransformPoint(0f, 0f, 1000f);
		}
		PlasmaHitInfo plasmaHitInfo = new PlasmaHitInfo();
		plasmaHitInfo.pos = aimTarget;
		plasmaHitInfo.time = Time.time;
		plasmaHitInfo.lastExplodeTime = Time.time;
		plasmaHitInfo.count = 0;
		hitPosition.Add(plasmaHitInfo);
		ExplodeAtPoint(aimTarget, (int)splashDamage, plasmaHitInfo.count);
		sparksObjectPool.CreateObject(aimTarget, Vector3.one, Quaternion.identity);
		Vector3 normalized = player.GetTransform().forward.normalized;
		gunfire.transform.rotation = player.GetTransform().rotation;
		gunfire.transform.rotation = Quaternion.AngleAxis(player.AngleV, -gunfire.transform.right) * gunfire.transform.rotation;
		normalized = gunfire.transform.forward.normalized;
		GameObject gameObject = firelineObjectPool.CreateObject(gunfire.transform.position + normalized * 2f, normalized, Quaternion.identity);
		gameObject.transform.Rotate(180f, 0f, 0f);
		if (!(gameObject == null))
		{
			FireLineScript component = gameObject.GetComponent<FireLineScript>();
			component.transform.Rotate(90f, 0f, 0f);
			component.beginPos = gunfire.position;
			component.endPos = gunfire.position + normalized * 100f;
		}
		if (gunfireObj != null)
		{
			gunfireObj.SetActiveRecursively(true);
		}
	}

	public void ExplodeAtPoint(Vector3 point, int exDamage, int count)
	{
		HashSet<Enemy> hashSet = new HashSet<Enemy>();
		if (count <= 2)
		{
			sparksObjectPool.CreateObject(point, Vector3.one, Quaternion.identity);
		}
		float num = 3f;
		bool flag = player.IsLocal();
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.damage = exDamage;
		bool criticalAttack = false;
		int num2 = Random.Range(0, 100);
		if (num2 < 40)
		{
			criticalAttack = true;
		}
		damageProperty.criticalAttack = criticalAttack;
		damageProperty.hitpoint = point;
		damageProperty.isLocal = flag;
		damageProperty.wType = WeaponType.PlasmaNeo;
		Collider[] array = Physics.OverlapSphere(point, num, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.GIFT));
		Collider[] array2 = array;
		foreach (Collider c in array2)
		{
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(c);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
				if (!hashSet.Contains(enemyByID))
				{
					enemyByID.HitEnemy(damageProperty);
					hashSet.Add(enemyByID);
				}
			}
			else if (enemyByCollider.layer == PhysicsLayer.GIFT && GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI && flag)
			{
				int idByName = CMIGift.GetIdByName(enemyByCollider.name);
				PlayerHitItemRequest request = new PlayerHitItemRequest((short)damage, idByName, false, (byte)GetWeaponType());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		if (!GameApp.GetInstance().GetGameMode().IsVSMode() || !flag)
		{
			return;
		}
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (GameApp.GetInstance().GetGameMode().IsTeamMode())
			{
				Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(item.GetUserID());
				if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
				{
					continue;
				}
			}
			float sqrMagnitude = (item.GetTransform().position - point).sqrMagnitude;
			float num3 = num * num;
			if (sqrMagnitude < num3)
			{
				PlayerHitPlayerRequest request2 = new PlayerHitPlayerRequest((short)splashDamage, item.GetUserID(), false, (byte)GetWeaponType());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	public override void Attack(float deltaTime)
	{
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
		if (gunfireObj != null)
		{
			gunfireObj.SetActiveRecursively(true);
		}
		HashSet<Enemy> hashSet = new HashSet<Enemy>();
		Camera mainCamera = Camera.mainCamera;
		Transform transform = mainCamera.transform;
		ThirdPersonStandardCameraScript component = Camera.mainCamera.GetComponent<ThirdPersonStandardCameraScript>();
		Ray ray = default(Ray);
		Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 0.1f));
		Vector3 normalized = (vector - transform.position).normalized;
		ray = new Ray(transform.position + normalized * 1.8f, normalized);
		RaycastHit raycastHit = default(RaycastHit);
		raycastHit.point = transform.position + (vector - transform.position).normalized * 80f;
		float num = Mathf.Tan((float)Math.PI / 3f);
		RaycastHit[] array = Physics.RaycastAll(ray, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.GIFT));
		float num2 = 1000000f;
		for (int i = 0; i < array.Length; i++)
		{
			if (GameApp.GetInstance().GetGameMode().IsVSMode() && GameApp.GetInstance().GetGameMode().IsTeamMode() && array[i].collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				int userID = int.Parse(array[i].collider.gameObject.name);
				Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(userID);
				if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
				{
					continue;
				}
			}
			Vector3 zero = Vector3.zero;
			if (((array[i].collider.gameObject.layer != PhysicsLayer.ENEMY) ? gunfire.transform.InverseTransformPoint(array[i].point) : gunfire.transform.InverseTransformPoint(array[i].collider.transform.position)).x < 2f)
			{
				float sqrMagnitude = (array[i].point - gunfire.transform.position).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					raycastHit = array[i];
					num2 = sqrMagnitude;
				}
			}
		}
		aimTarget = raycastHit.point;
		Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
		if (raycastHit.collider != null)
		{
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(raycastHit.collider);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
				if (enemyByID.GetState() != Enemy.DEAD_STATE && !hashSet.Contains(enemyByID))
				{
					hashSet.Add(enemyByID);
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.damage = (int)damage;
					bool criticalAttack = false;
					int num3 = Random.Range(0, 100);
					if (num3 < 80)
					{
						criticalAttack = true;
					}
					damageProperty.criticalAttack = criticalAttack;
					damageProperty.hitpoint = raycastHit.point;
					damageProperty.isLocal = true;
					damageProperty.wType = WeaponType.PlasmaNeo;
					enemyByID.HitEnemy(damageProperty);
				}
			}
			else if (enemyByCollider.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					PlayerHitPlayerRequest request = new PlayerHitPlayerRequest((short)damage, int.Parse(enemyByCollider.name), false, (byte)GetWeaponType());
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else if (enemyByCollider.layer == PhysicsLayer.GIFT && GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
			{
				int idByName = CMIGift.GetIdByName(enemyByCollider.name);
				PlayerHitItemRequest request2 = new PlayerHitItemRequest((short)damage, idByName, false, (byte)GetWeaponType());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
			PlasmaHitInfo plasmaHitInfo = new PlasmaHitInfo();
			plasmaHitInfo.pos = raycastHit.point;
			plasmaHitInfo.time = Time.time;
			plasmaHitInfo.lastExplodeTime = Time.time;
			plasmaHitInfo.count = 0;
			hitPosition.Add(plasmaHitInfo);
			ExplodeAtPoint(raycastHit.point, (int)splashDamage, plasmaHitInfo.count);
		}
		GameObject gameObject = firelineObjectPool.CreateObject(gunfire.transform.position + normalized2 * 2f, normalized2, Quaternion.identity);
		gameObject.transform.Rotate(180f, 0f, 0f);
		if (!(gameObject == null))
		{
			FireLineScript component2 = gameObject.GetComponent<FireLineScript>();
			component2.transform.Rotate(90f, 0f, 0f);
			component2.beginPos = gunfire.position;
			component2.endPos = raycastHit.point;
		}
		attacked = false;
		lastAttackTime = Time.time;
		SetLastAttackTimeShared();
	}
}
