using UnityEngine;

public class PingPongLauncher : Weapon
{
	protected const float shootLastingTime = 0.5f;

	protected float rocketFlySpeed = 12f;

	protected static int sbulletCount;

	public override WeaponType GetWeaponType()
	{
		return WeaponType.PingPongLauncher;
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
		int num = Random.Range(1, 3);
		AudioManager.GetInstance().PlaySoundAt("Audio/diablo/black_disk_fire0" + num, player.GetTransform().position);
		GameObject original = Resources.Load("Effect/update_effect/effect_ufo_gunfire_001") as GameObject;
		gunfireObj = Object.Instantiate(original, gunfire.position, Quaternion.identity) as GameObject;
		gunfireObj.transform.parent = gunfire;
	}

	public override void GunOn()
	{
		base.GunOn();
		SetWeaponShootSpeed(1.4f);
		base.ExplodeSoundName = "Audio/rpg/rpg-31_boom";
	}

	public override void Init(Player player)
	{
		base.Init(player);
	}

	public override void DamageBoothByArmor()
	{
		base.DamageBoothByArmor();
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.PINGPONG_BOOTH);
	}

	public override float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return base.GetDamageAfterBoothByArmor(skill) * (1f + skill.GetSkill(SkillsType.PINGPONG_BOOTH));
	}

	public Vector3 GetNetworkVector3(Vector3 v)
	{
		short num = (short)(v.x * 10f);
		short num2 = (short)(v.y * 10f);
		short num3 = (short)(v.z * 10f);
		Vector3 result = default(Vector3);
		result.x = (float)num * 1f / 10f;
		result.y = (float)num2 * 1f / 10f;
		result.z = (float)num3 * 1f / 10f;
		return result;
	}

	public override void Attack(float deltaTime)
	{
		lastAttackTime = Time.time;
		SetLastAttackTimeShared();
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
		Ray ray = default(Ray);
		Vector3 vector = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x, (float)Screen.height - gameCamera.ReticlePosition.y, 50f));
		Vector3 normalized = (vector - cameraTransform.position).normalized;
		ray = new Ray(cameraTransform.position + normalized * 1.8f, normalized);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.GIFT)))
		{
			aimTarget = hitInfo.point;
		}
		else
		{
			aimTarget = cameraTransform.TransformPoint(0f, 0f, 1000f);
		}
		Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
		GameObject original = Resources.Load("Effect/PingPongShot") as GameObject;
		Vector3 networkVector = GetNetworkVector3(gunfire.position);
		Vector3 networkVector2 = GetNetworkVector3(normalized2);
		GameObject gameObject = Object.Instantiate(original, networkVector, Quaternion.LookRotation(networkVector2)) as GameObject;
		ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
		component.dir = networkVector2;
		component.flySpeed = rocketFlySpeed;
		component.explodeRadius = bombRange;
		component.hitForce = hitForce;
		component.life = 8f;
		component.damage = (int)damage;
		component.GunType = WeaponType.PingPongLauncher;
		component.targetPos = aimTarget;
		component.isPenerating = true;
		component.isReflecting = true;
		component.bagIndex = (byte)GameApp.GetInstance().GetUserState().GetWeaponBagIndex(this);
		component.weapon = this;
		component.explodeRadius = 3f;
		int num = Random.Range(1, 3);
		AudioManager.GetInstance().PlaySoundAt("Audio/diablo/black_disk_fire0" + num, player.GetTransform().position);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerFireRocketRequest request = new PlayerFireRocketRequest(20, gunfire.position, normalized2);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		GameObject original2 = Resources.Load("Effect/update_effect/effect_ufo_gunfire_001") as GameObject;
		gunfireObj = Object.Instantiate(original2, gunfire.position, Quaternion.identity) as GameObject;
		gunfireObj.transform.parent = gunfire;
	}
}
