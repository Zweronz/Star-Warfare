using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
	protected Timer sendingTimer = new Timer();

	protected Timer sendingShootDirTimer = new Timer();

	protected Timer sendingVerifyTimer = new Timer();

	protected NetworkManager networkMgr;

	protected NetworkTransform lastTrans = new NetworkTransform(Vector3.zero, 0f, 0, false);

	protected WeaponType lastWT = WeaponType.AssaultRifle;

	protected Timer hpRecoveryTimer = new Timer();

	protected Timer uploadScoreTimer = new Timer();

	protected Timer attackShiledTimer = new Timer();

	protected Enemy mAimEnemy;

	protected RemotePlayer mAimPlayer;

	public LocalPlayer()
	{
		userState = GameApp.GetInstance().GetUserState();
	}

	public override void Init()
	{
		InitSkills();
		base.Init();
		base.inputController.Init();
		sendingTimer.SetTimer(0.2f, false);
		sendingShootDirTimer.SetTimer(0.5f, false);
		sendingVerifyTimer.SetTimer(60f, true);
		networkMgr = GameApp.GetInstance().GetNetworkManager();
		SetUserID(Lobby.GetInstance().GetChannelID());
		PlayAnimation(AnimationString.Run + "_rifle", WrapMode.Loop);
		PlayAnimation(AnimationString.Idle + "_rifle", WrapMode.Loop);
		Transform transform = playerTransform.FindChild(BoneName.Bag).FindChild("Bag");
		if (transform != null)
		{
			transform.localPosition = Vector3.zero;
		}
		base.MaxHp += (int)playerSkill.GetSkill(SkillsType.HP_BOOTH);
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			base.MaxHp = VSMath.GetHpInVS(base.MaxHp);
		}
		base.Hp = base.MaxHp;
		speedBooth = playerSkill.GetSkill(SkillsType.SPEED_BOOTH);
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			GameApp.GetInstance().GetGameWorld().CreateTeamSkills();
		}
		hpRecoveryTimer.SetTimer(1f, false);
		uploadScoreTimer.SetTimer(30f, false);
		attackShiledTimer.SetTimer(0.25f, false);
		if (userState.Enegy < 500)
		{
			userState.Enegy = 500;
		}
	}

	public override void PowerUp(bool enable, int initialSkillIndex)
	{
		base.PowerUp(enable, initialSkillIndex);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			if (!enable)
			{
				return;
			}
			PlayerBuff playerBuff = PlayerBuff.PowerUp;
			switch (initialSkillIndex)
			{
			case 1:
				playerBuff = PlayerBuff.SpeedUp;
				break;
			case 2:
				playerBuff = PlayerBuff.DefenceUp;
				break;
			case 3:
				playerBuff = PlayerBuff.AndromedaUp;
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					RecoveHP(300f);
				}
				else
				{
					RecoveHP(10000f);
				}
				break;
			case 4:
				playerBuff = PlayerBuff.HealthSteal;
				break;
			case 5:
				playerBuff = PlayerBuff.AttackShiled;
				break;
			case 6:
				playerBuff = PlayerBuff.ImpactWave;
				ImpactWave();
				break;
			case 7:
				playerBuff = PlayerBuff.TrackWave;
				TrackWave();
				break;
			case 8:
				playerBuff = PlayerBuff.HurtHealth;
				break;
			case 9:
				playerBuff = PlayerBuff.GravityForce;
				GravityForce();
				break;
			}
			PlayerBuffRequest request = new PlayerBuffRequest((byte)playerBuff);
			networkMgr.SendRequest(request);
		}
		else if (enable)
		{
			switch (initialSkillIndex)
			{
			case 3:
				RecoveHP(10000f);
				break;
			case 6:
				ImpactWave();
				break;
			case 7:
				TrackWave();
				break;
			case 9:
				Debug.Log("GRAVITY_FORCE initialSkillIndex = " + initialSkillIndex);
				GravityForce();
				break;
			}
		}
	}

	public override void ChangeWeaponInBag(int bagIndex)
	{
		Weapon weaponInBag = userState.GetWeaponInBag(bagIndex);
		Weapon weapon = base.weapon;
		if (weaponInBag != weapon)
		{
			if (weaponInBag != null)
			{
				ChangeWeapon(weaponInBag);
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerChangeWeaponRequest request = new PlayerChangeWeaponRequest((byte)bagIndex);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected void CheckAttackShiled()
	{
		float num = 4f;
		short num2 = 100;
		if (!IsPowerUp(5) || !attackShiledTimer.Ready())
		{
			return;
		}
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			num2 = (short)VSMath.GetDamageInVS(num2);
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item != null && (!GameApp.GetInstance().GetGameMode().IsTeamMode() || !item.IsSameTeam(this)))
				{
					float num3 = Vector3.Distance(GetTransform().position, item.GetTransform().position);
					if (num3 < num)
					{
						PlayerHitPlayerRequest request = new PlayerHitPlayerRequest(num2, item.GetUserID(), false, 0);
						networkMgr.SendRequest(request);
					}
				}
			}
		}
		else
		{
			Collider[] array = Physics.OverlapSphere(GetTransform().position, num, 1 << PhysicsLayer.ENEMY);
			Collider[] array2 = array;
			foreach (Collider c in array2)
			{
				GameObject enemyByCollider = Enemy.GetEnemyByCollider(c);
				if (enemyByCollider.name.StartsWith("E_"))
				{
					Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyByCollider.name);
					if (enemyByID != null && Vector3.Distance(enemyByID.GetTransform().position, GetTransform().position) < 4f)
					{
						DamageProperty damageProperty = new DamageProperty();
						damageProperty.isLocal = true;
						damageProperty.damage = num2;
						enemyByID.HitEnemy(damageProperty);
					}
				}
			}
		}
		attackShiledTimer.Do();
	}

	public override void Loop(float deltaTime)
	{
		base.Loop(deltaTime);
		base.inputController.Process();
		if (weapon.GetWeaponType() != lastWT)
		{
			lastWT = weapon.GetWeaponType();
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			SendInput();
			SendVerifyData();
		}
		base.State.NextState(this, deltaTime);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && InPlayingState())
		{
			SendTransform();
		}
		CheckHPRecovery();
		if (uploadScoreTimer.Ready())
		{
			UploadStatistaic();
			uploadScoreTimer.Do();
		}
		CheckAttackShiled();
		if (weapon != null && (weapon.GetWeaponType() == WeaponType.AdvancedMachineGun || weapon.GetWeaponType() == WeaponType.AdvancedGrenadeLauncher || weapon.GetWeaponType() == WeaponType.FlyGrenadeLauncher))
		{
			weapon.Loop(deltaTime);
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			SendShootDir();
		}
		ThirdPersonStandardCameraScript camera = GameApp.GetInstance().GetGameWorld().GetCamera();
		if (camera != null)
		{
			base.AngleV = camera.AngelV;
		}
	}

	public void CheckHPRecovery()
	{
		if (hpRecoveryTimer.Ready() && InPlayingState())
		{
			float num = playerSkill.GetSkill(SkillsType.HP_AUTO_RECOVERY) + GameApp.GetInstance().GetGameWorld().TeamSkills.teamHpRecovery;
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				num = VSMath.GetAutoRecoverInVS(num);
			}
			RecoveHP(num);
			hpRecoveryTimer.Do();
		}
	}

	public void RecoveHP(float recoveryRate)
	{
		base.Hp += (int)recoveryRate;
		base.Hp = Mathf.Clamp(base.Hp, 0, base.MaxHp);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerHpRecoveryRequest request = new PlayerHpRecoveryRequest(0, (short)recoveryRate);
			networkMgr.SendRequest(request);
		}
	}

	public void SendVerifyData()
	{
		if (!sendingVerifyTimer.Ready())
		{
			return;
		}
		sendingVerifyTimer.Do();
		if (string.IsNullOrEmpty(GameApp.GetInstance().MacAddress))
		{
			return;
		}
		List<VerifyData> list = new List<VerifyData>();
		for (int i = 0; i < Global.AVATAR_PART_NUM; i++)
		{
			Armor armor = userState.GetArmor(i);
			List<Skill> skills = armor.GetSkills();
			ArmorVerify armorVerify = new ArmorVerify(armor.GetArmorType(), armor.GetArmorID());
			foreach (Skill item5 in skills)
			{
				if (item5.skillType != SkillsType.MONEY_BOOTH && item5.skillType != SkillsType.EXP_BOOTH && item5.skillType != SkillsType.SAVE_ENEGY && item5.skillType != SkillsType.UNLIMITED_ENEGY && item5.skillType != SkillsType.FLY && item5.skillType != SkillsType.MONEY_BOOTH && item5.skillType != SkillsType.MONEY_BOOTH && item5.skillType != SkillsType.MONEY_BOOTH)
				{
					Debug.Log("ArmorType/ID/SkillType/Origin/Verify: " + armor.GetArmorType().ToString() + "/" + armor.GetArmorID() + "/" + item5.skillType.ToString() + "/" + item5.data + "/" + armorVerify.GetSkill(item5.skillType));
					VerifyData item = new VerifyData(armor.GetArmorType(), armor.GetArmorID(), item5.skillType, item5.data, armorVerify.GetSkill(item5.skillType));
					list.Add(item);
				}
			}
		}
		SendUdidVerifyDataRequest request = new SendUdidVerifyDataRequest(list);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		List<VerifyData> list2 = new List<VerifyData>();
		if (userState.ArmorInOneCollection())
		{
			int armorGroupID = userState.GetArmor(0).GetArmorGroupID();
			ArmorRewards armorRewards = userState.GetArmorRewards()[armorGroupID];
			List<Skill> skills2 = armorRewards.GetSkills();
			ArmorRewardsVerify armorRewardsVerify = new ArmorRewardsVerify(armorRewards.GetArmorRewardsID());
			foreach (Skill item6 in skills2)
			{
				if (item6.skillType != SkillsType.MONEY_BOOTH && item6.skillType != SkillsType.EXP_BOOTH && item6.skillType != SkillsType.SAVE_ENEGY && item6.skillType != SkillsType.UNLIMITED_ENEGY && item6.skillType != SkillsType.FLY && item6.skillType != SkillsType.MONEY_BOOTH && item6.skillType != SkillsType.MONEY_BOOTH && item6.skillType != SkillsType.MONEY_BOOTH)
				{
					Debug.Log("GroupID/SkillType/Origin/Verify: " + armorRewards.GetArmorRewardsID() + "/" + item6.skillType.ToString() + "/" + item6.data + "/" + armorRewardsVerify.GetSkill(item6.skillType));
					VerifyData item2 = new VerifyData(armorRewards.GetArmorRewardsID(), item6.skillType, item6.data, armorRewardsVerify.GetSkill(item6.skillType));
					list2.Add(item2);
				}
			}
		}
		SendUdidVerifyDataRequest request2 = new SendUdidVerifyDataRequest(list2);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		List<VerifyData> list3 = new List<VerifyData>();
		if (weapon != null)
		{
			WeaponVerify weaponVerify = new WeaponVerify(weapon.GunID);
			Debug.Log("GunID: " + weapon.GunID);
			Debug.Log("range: " + weapon.GetRange() + "/" + weaponVerify.range);
			Debug.Log("damageInit: " + weapon.GetDamageInit() + "/" + weaponVerify.damageInit);
			Debug.Log("splashDamageInit: " + weapon.GetSplashDamageInit() + "/" + weaponVerify.splashDamageInit);
			Debug.Log("splashDuration: " + weapon.GetSplashDuration() + "/" + weaponVerify.splashDuration);
			Debug.Log("bombRangeInit: " + weapon.GetBombRangeInit() + "/" + weaponVerify.bombRangeInit);
			Debug.Log("attackFrenquencyInit: " + weapon.GetAttackFrenquencyInit() + "/" + weaponVerify.attackFrenquencyInit);
			Debug.Log("speedDrag: " + weapon.GetSpeedDrag() + "/" + weaponVerify.speedDrag);
			VerifyData item3 = new VerifyData(weapon.GunID, 0, weapon.GetRange(), weaponVerify.range);
			list3.Add(item3);
			item3 = new VerifyData(weapon.GunID, 1, weapon.GetDamageInit(), weaponVerify.damageInit);
			list3.Add(item3);
			item3 = new VerifyData(weapon.GunID, 2, weapon.GetSplashDamageInit(), weaponVerify.splashDamageInit);
			list3.Add(item3);
			item3 = new VerifyData(weapon.GunID, 3, weapon.GetSplashDuration(), weaponVerify.splashDuration);
			list3.Add(item3);
			item3 = new VerifyData(weapon.GunID, 4, weapon.GetBombRangeInit(), weaponVerify.bombRangeInit);
			list3.Add(item3);
			item3 = new VerifyData(weapon.GunID, 5, weapon.GetAttackFrenquencyInit(), weaponVerify.attackFrenquencyInit);
			list3.Add(item3);
			item3 = new VerifyData(weapon.GunID, 6, weapon.GetSpeedDrag(), weaponVerify.speedDrag);
			list3.Add(item3);
		}
		SendUdidVerifyDataRequest request3 = new SendUdidVerifyDataRequest(list3);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
		List<VerifyData> list4 = new List<VerifyData>();
		WeaponUpgrade weaponUpgrade = GameApp.GetInstance().GetUserState().weaponUpgrade;
		if (weaponUpgrade != null)
		{
			WeaponUpgradeVerify weaponUpgradeVerify = new WeaponUpgradeVerify();
			for (int j = 0; j < Global.MAX_LEVEL_WEAPONW; j++)
			{
				Debug.Log("Level/Origin/Verify: " + j + "/" + weaponUpgrade.GetDamage(j) + "/" + weaponUpgradeVerify.GetDamage(j));
				VerifyData item4 = new VerifyData(j, weaponUpgrade.GetDamage(j), weaponUpgradeVerify.GetDamage(j));
				list4.Add(item4);
			}
		}
		SendUdidVerifyDataRequest request4 = new SendUdidVerifyDataRequest(list4);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request4);
	}

	public void SendInput()
	{
		InputInfo inputInfo = base.inputController.inputInfo;
		InputInfo previousInputInfo = base.inputController.previousInputInfo;
		bool flag = inputInfo.fire;
		if (!weapon.CheckBullets())
		{
			flag = false;
		}
		if (weapon.GetWeaponType() == WeaponType.LaserGun)
		{
			if (weapon.GunID == 38)
			{
				SnowGun snowGun = (SnowGun)weapon;
				if (snowGun != null && snowGun.IsOverHeat)
				{
					flag = false;
				}
			}
			else
			{
				LaserCannon laserCannon = (LaserCannon)weapon;
				if (laserCannon != null && laserCannon.IsOverHeat)
				{
					flag = false;
				}
			}
		}
		if (inputInfo.IsMoving() != previousInputInfo.IsMoving() || flag != previousInputInfo.fire)
		{
			SendPlayerInputRequest request = new SendPlayerInputRequest(flag, inputInfo.IsMoving());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			previousInputInfo.moveDirection = inputInfo.moveDirection;
			previousInputInfo.fire = flag;
		}
	}

	public void SendTransform()
	{
		if (sendingTimer.Ready() && TimeManager.GetInstance().NetworkTimeSynchronized)
		{
			if (lastTrans.Pos != playerTransform.position || lastTrans.Angle != playerTransform.eulerAngles.y)
			{
				SendTransformStateRequest request = new SendTransformStateRequest(playerTransform.position, playerTransform.eulerAngles);
				networkMgr.SendRequest(request);
				lastTrans.Pos = playerTransform.position;
				lastTrans.Angle = playerTransform.eulerAngles.y;
			}
			sendingTimer.Do();
		}
	}

	public void SendShootDir()
	{
		if (sendingShootDirTimer.Ready())
		{
			InputInfo inputInfo = base.inputController.inputInfo;
			if (inputInfo.fire)
			{
				short angleV = (short)GameApp.GetInstance().GetGameWorld().GetCamera()
					.AngelV;
				SendPlayerShootAngleVRequest request = new SendPlayerShootAngleVRequest(angleV);
				networkMgr.SendRequest(request);
			}
			sendingShootDirTimer.Do();
		}
	}

	public override void PlayAnimation(string name, WrapMode mode)
	{
		base.PlayAnimation(name, mode);
	}

	public override bool IsLocal()
	{
		return true;
	}

	public override void OnDead()
	{
		base.OnDead();
		Transform transform = Camera.mainCamera.transform.Find("Screen_DeadBlood");
		transform.localScale = new Vector3(2.4f * (float)Screen.width / (float)Screen.height, 2.4f, 1f);
		transform.renderer.enabled = true;
		deadAnimationTimer.SetTimer(4f, false);
	}

	public override bool CheckLose()
	{
		return rebirthTimer.Ready();
	}

	public override bool StartWaitRebirth()
	{
		int[] bagPosition = userState.GetBagPosition();
		int[] array = bagPosition;
		foreach (int num in array)
		{
			if (num == 87 || num == 88)
			{
				rebirthTimer.SetTimer(5f, false);
				return true;
			}
		}
		return false;
	}

	public override bool DeadAnimationCompleted()
	{
		return deadAnimationTimer.Ready();
	}

	public override bool WinAnimationCompleted()
	{
		return winAnimationTimer.Ready();
	}

	public void SendRebirthRequest()
	{
		if (base.SendingRebirthRequest)
		{
			return;
		}
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		float num4 = 0f;
		float num5 = 0f;
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.RESPAWN);
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		int num6 = Random.Range(0, array.Length);
		List<GameObject> list = new List<GameObject>();
		for (int i = num6; i < array.Length; i++)
		{
			list.Add(array[i]);
		}
		for (int j = 0; j < num6; j++)
		{
			list.Add(array[j]);
		}
		for (int k = 0; k < list.Count; k++)
		{
			if (!list[k].name.StartsWith(ObjectNamePrefix.PLAYER_SPAWN_POINT))
			{
				continue;
			}
			Vector3 vector = list[k].transform.position + new Vector3(0f, 1f, 0f);
			float num7 = 9999f;
			bool flag = false;
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item == null || !item.InPlayingState() || IsSameTeam(item))
				{
					continue;
				}
				Vector3 vector2 = item.GetTransform().position + new Vector3(0f, 1f, 0f);
				Vector3 direction = vector2 - vector;
				float sqrMagnitude = new Vector3(direction.x, 0f, direction.z).sqrMagnitude;
				if (sqrMagnitude < num7)
				{
					num7 = sqrMagnitude;
				}
				if (!flag && sqrMagnitude < 2025f)
				{
					Ray ray = new Ray(vector, direction);
					float magnitude = direction.magnitude;
					RaycastHit hitInfo;
					if (!Physics.Raycast(ray, out hitInfo, magnitude, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				if (num7 > num4)
				{
					num2 = k;
					num4 = num7;
				}
				continue;
			}
			if (num7 < 225f)
			{
				if (num7 > num5)
				{
					num3 = k;
					num5 = num7;
				}
				continue;
			}
			num = k;
			break;
		}
		if (num == -1)
		{
			num = num3;
		}
		if (num == -1)
		{
			num = num2;
		}
		if (num == -1)
		{
			num = 0;
		}
		num = (num + num6) % array.Length;
		PlayerRebirthRequest request = new PlayerRebirthRequest((byte)num);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		base.SendingRebirthRequest = true;
		Debug.Log("send rebirth:" + base.State);
		GameObject gameObject = GameObject.Find("GameUI");
		if (gameObject != null)
		{
			InGameUIScript component = gameObject.GetComponent<InGameUIScript>();
			if (component != null)
			{
				component.WaitVSRebirthEnd();
			}
		}
	}

	private void ImpactWave()
	{
		AudioManager.GetInstance().PlaySoundAt("Audio/light_sword/windblade", GetTransform().position);
		ThirdPersonStandardCameraScript component = Camera.mainCamera.GetComponent<ThirdPersonStandardCameraScript>();
		Camera camera = component.camera;
		Ray ray = default(Ray);
		Vector3 vector = camera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 50f));
		Vector3 normalized = (vector - camera.transform.position).normalized;
		ray = new Ray(camera.transform.position + normalized * 1.8f, normalized);
		RaycastHit hitInfo;
		Vector3 vector2 = ((!Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER))) ? camera.transform.TransformPoint(0f, 0f, 1000f) : hitInfo.point);
		Vector3 vector3 = GetTransform().TransformPoint(new Vector3(0f, 1f, 0.5f));
		Vector3 normalized2 = (vector2 - vector3).normalized;
		GameObject original = Resources.Load("Effect/update_effect/effect_wave_002") as GameObject;
		GameObject gameObject = Object.Instantiate(original, vector3, Quaternion.LookRotation(normalized2)) as GameObject;
		ImpactWaveScript component2 = gameObject.GetComponent<ImpactWaveScript>();
		component2.dir = normalized2;
		component2.flySpeed = 10f;
		component2.explodeRadius = 3f;
		component2.life = 8f;
		float num = 22000f;
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			num = VSMath.GetDamageInVS(num) * VSMath.IMPACT_WAVE_DAMAGE_BOOTH;
		}
		component2.damage = (int)num;
		component2.gunType = WeaponType.ImpactWave;
		component2.isPenerating = true;
		component2.targetPos = vector2;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerImpactWaveRequest request = new PlayerImpactWaveRequest(22, vector3, normalized2);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	private void TrackWave()
	{
		AudioManager.GetInstance().PlaySoundAt("Audio/light_sword/windblade", GetTransform().position);
		ThirdPersonStandardCameraScript component = Camera.mainCamera.GetComponent<ThirdPersonStandardCameraScript>();
		Camera camera = component.camera;
		Transform transform = camera.transform;
		Ray ray = default(Ray);
		Vector3 vector = camera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 50f));
		Vector3 normalized = (vector - transform.position).normalized;
		ray = new Ray(transform.position + normalized * 1.8f, normalized);
		RaycastHit hitInfo;
		Vector3 vector2 = ((!Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER))) ? camera.transform.TransformPoint(0f, 0f, 1000f) : hitInfo.point);
		Vector3 vector3 = GetTransform().TransformPoint(new Vector3(0f, 1f, 0.5f));
		Vector3 normalized2 = (vector2 - vector3).normalized;
		GameObject original = Resources.Load("ZB_effect/black_hole_beam") as GameObject;
		GameObject gameObject = Object.Instantiate(original, vector3, Quaternion.LookRotation(normalized2)) as GameObject;
		TrackWaveScript component2 = gameObject.GetComponent<TrackWaveScript>();
		component2.dir = normalized2;
		component2.flySpeed = 20f;
		component2.explodeRadius = 3f;
		component2.life = 8f;
		component2.damage = 1;
		component2.gunType = WeaponType.TrackWave;
		component2.isPenerating = false;
		component2.targetPos = vector2;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerTrackWaveRequest request = new PlayerTrackWaveRequest(39, vector3, normalized2);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public void SetAimEnemy(Enemy enemy)
	{
		mAimEnemy = enemy;
	}

	public void SetAimPlayer(RemotePlayer rPlayer)
	{
		mAimPlayer = rPlayer;
	}

	private void GravityForce()
	{
		if (mAimEnemy != null && mAimEnemy.GetState() != Enemy.DEAD_STATE)
		{
			mAimEnemy.SetState(Enemy.GRAVITY_FORCE_STATE);
			mAimEnemy.SetStartGravityForceTimeNow();
			mAimEnemy.SetGravityForceTarget(GetTransform().position);
			mAimEnemy.StartGravityForceEffect();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(mAimEnemy.EnemyID, 5, mAimEnemy.GetTransform().position, GetTransform().position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		if (mAimPlayer != null && mAimPlayer.InPlayingState())
		{
			mAimPlayer.OnGravityForce(GetTransform().position);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerGravityForceRequest request2 = new PlayerGravityForceRequest(mAimPlayer.GetUserID(), GetTransform().position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}
}
