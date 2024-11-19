using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player
{
	protected const float SLOW_DOWN_EFFECT = 0.7f;

	public static PlayerState IDLE_STATE = new IdleState();

	public static PlayerState RUN_STATE = new RunState();

	public static PlayerState GETHURT_STATE = new GetHurtState();

	public static PlayerState ATTACK_STATE = new AttackState();

	public static PlayerState DEAD_STATE = new PlayerDeadState();

	public static PlayerState KNOCKED_STATE = new PlayerKnockedState();

	public static PlayerState GRAVITY_FORCE_STATE = new PlayerGravityForceState();

	public static PlayerState WIN_STATE = new WinState();

	public static PlayerState LOSE_STATE = new PlayerLoseState();

	public static PlayerState WAIT_REBIRTH_STATE = new PlayerWaitRebirthState();

	public static PlayerState WAIT_VS_REBIRTH_STATE = new PlayerWaitVSRebirthState();

	protected GameObject playerObj;

	protected Transform playerTransform;

	protected CharacterController cc;

	protected int userID;

	protected byte seatID;

	protected Weapon weapon;

	protected List<Weapon> weaponList;

	protected Timer lastUpdateNearestPointTimer = new Timer();

	protected UserState userState;

	protected float speed = 7f;

	public byte bagNum;

	protected int currentBagIndex;

	protected GameObject defentObj;

	protected FadeInAlphaAnimationScript[] defentScript = new FadeInAlphaAnimationScript[3];

	protected DefentOneByOneScript[] defentOnebyOneScript = new DefentOneByOneScript[3];

	protected Timer showDefentTimer = new Timer();

	protected PlayerSkill playerSkill = new PlayerSkill();

	protected float speedBooth;

	protected Transform mSpine;

	protected Transform mHead;

	protected Transform mWeapon;

	protected AimArm[] mArms;

	protected IK1JointAnalytic ikSolver = new IK1JointAnalytic();

	protected GameObject[] powerObj = new GameObject[10];

	protected Timer[] powerTimer = new Timer[10];

	protected Timer[] powerCDTimer = new Timer[10];

	protected GameObject itemAssistEffectObj;

	protected int monsterCash;

	protected int bonusCash;

	protected int pickupCash;

	protected int bossCash;

	protected int bossMithril;

	protected int exp;

	protected int combo;

	protected Timer comboTimer = new Timer();

	protected float teleportStartFadeTime = -1f;

	protected bool bStartFade;

	protected int initEnegy;

	protected int pickupEnegy;

	protected int maxCombo;

	protected float knockedSpeed;

	protected Vector3 mGravityForceTarget;

	protected float mStartGravityForceTime;

	protected List<AssistItem> assistItems = new List<AssistItem>();

	protected GameObject chuangsongObj;

	protected string name;

	protected bool isSpecialWinIdle = true;

	protected bool winSpecialFinish = true;

	protected int lastAssistItemCount = -1;

	protected Timer rebirthTimer = new Timer();

	protected Timer deadAnimationTimer = new Timer();

	protected Timer winAnimationTimer = new Timer();

	protected Timer vsRebirthTimer = new Timer();

	protected Timer lastBloodEffectTimer = new Timer();

	protected int runningPhase = -1;

	protected bool mIsSlowDown;

	protected Timer mSlowDownTimer = new Timer();

	protected Timer mAimTimer = new Timer();

	protected bool mAim;

	public bool isAiming;

	protected GameObject mTrackEffectObj;

	protected GameObject mSlowDownEffectObj;

	protected AlphaAnimationScript bagAlphaAnimationScript;

	protected UVOffsetScript bagUVOffsetScript;

	protected Dictionary<byte, TrackingGrenadeScript> trackingGrenadeDic = new Dictionary<byte, TrackingGrenadeScript>();

	protected Dictionary<byte, FlyGrenadeShotScript> flyGrenadeDic = new Dictionary<byte, FlyGrenadeShotScript>();

	protected GameObject mGravityForceBallObj;

	protected GameObject mGravityForceBeamObj;

	public PlayerState State { get; set; }

	public InputController inputController { get; set; }

	public WayPoint NearestPoint { get; set; }

	public int Hp { get; set; }

	public int MaxHp { get; set; }

	public bool NeverGotHit { get; set; }

	public int Kills { get; set; }

	public bool JustMakeAShoot { get; set; }

	public float AngleV { get; set; }

	public float TargetAngleV { get; set; }

	public bool SendingRebirthRequest { get; set; }

	public TeamName Team { get; set; }

	public PlayerVSStatistics VSStatistics { get; set; }

	public float SlowEffect { get; set; }

	public float LastAttackTimeShared { get; set; }

	public bool SniperAttack { get; set; }

	public float Speed
	{
		get
		{
			float num = 0f;
			if (IsPowerUp(0))
			{
				num += 2f;
			}
			if (IsPowerUp(1))
			{
				num += 2f;
			}
			if (IsPowerUp(3))
			{
				num += 1f;
			}
			if (State == ATTACK_STATE)
			{
				return Mathf.Max(2.45f, (speed + weapon.GetSpeedDrag() + speedBooth + num + GetSpeedItemAssist()) * 0.7f) * SlowEffect;
			}
			return Mathf.Max(3.5f, speed + weapon.GetSpeedDrag() + speedBooth + num + GetSpeedItemAssist()) * SlowEffect;
		}
		set
		{
			speed = value;
		}
	}

	public InputController InputController
	{
		get
		{
			return inputController;
		}
	}

	public Dictionary<byte, TrackingGrenadeScript> TrackingGrenadeDic
	{
		get
		{
			return trackingGrenadeDic;
		}
	}

	public Dictionary<byte, FlyGrenadeShotScript> FlyGrenadeDic
	{
		get
		{
			return flyGrenadeDic;
		}
	}

	public Timer GetLastBloodEffectTimer()
	{
		return lastBloodEffectTimer;
	}

	public void ComboClear()
	{
		combo = 0;
	}

	public void MakeCombo()
	{
		combo++;
		comboTimer.Do();
		if (combo > maxCombo)
		{
			maxCombo = combo;
		}
		userState.Achievement.MaxCombo(combo);
	}

	public string GetName()
	{
		return name;
	}

	public int GetInitEnegy()
	{
		return initEnegy;
	}

	public void SetName(string name)
	{
		this.name = name;
	}

	public int GetCombo()
	{
		return combo;
	}

	public int GetMaxCombo()
	{
		return maxCombo;
	}

	public int GetPickupEnegy()
	{
		return pickupEnegy;
	}

	public int GetMonsterCash()
	{
		return monsterCash;
	}

	public void SetMonsterCash(int cash)
	{
		monsterCash = cash;
	}

	public void AddMonsterCash(int cash)
	{
		monsterCash += cash;
	}

	public GameObject GetPlayerObject()
	{
		return playerObj;
	}

	public void SetBossCash(int cash)
	{
		bossCash = cash;
	}

	public int GetBossCash()
	{
		return bossCash;
	}

	public void SetBossMithril(int mithril)
	{
		bossMithril = mithril;
	}

	public int GetBossMithril()
	{
		return bossMithril;
	}

	public void SetPickupEnegy(int enegy)
	{
		pickupEnegy = enegy;
	}

	public int GetBounsCash()
	{
		return bonusCash;
	}

	public void SetBounsCash(int cash)
	{
		bonusCash = cash;
	}

	public void AddBounsCash(int cash)
	{
		bonusCash += cash;
	}

	public int GetPickupCash()
	{
		return pickupCash;
	}

	public void SetPickupCash(int cash)
	{
		pickupCash = cash;
	}

	public void AddPickupCash(int cash)
	{
		pickupCash += cash;
	}

	public int GetExp()
	{
		return exp;
	}

	public void SetExp(int exp)
	{
		this.exp = exp;
	}

	public void AddExp(int exp)
	{
		this.exp += exp;
	}

	public List<Weapon> GetWeaponList()
	{
		return weaponList;
	}

	public Weapon GetWeapon(int id)
	{
		for (int i = 0; i < weaponList.Count; i++)
		{
			if (weaponList[i].GunID == id)
			{
				return weaponList[i];
			}
		}
		return null;
	}

	public string GetWeaponAnimationSuffix()
	{
		return weapon.GetAnimationSuffix();
	}

	public string GetWeaponAnimationSuffixAlter()
	{
		return weapon.GetAnimationSuffixAlter();
	}

	public PlayerSkill GetSkills()
	{
		return playerSkill;
	}

	public virtual bool IsLocal()
	{
		return false;
	}

	public GameObject GetGameObject()
	{
		return playerObj;
	}

	public Transform GetTransform()
	{
		return playerTransform;
	}

	public void SetUserID(int userID)
	{
		this.userID = userID;
	}

	public int GetUserID()
	{
		return userID;
	}

	public void SetSeatID(byte seatID)
	{
		this.seatID = seatID;
	}

	public byte GetSeatID()
	{
		return seatID;
	}

	public virtual void SetState(PlayerState state)
	{
		State = state;
	}

	public Weapon GetWeapon()
	{
		return weapon;
	}

	public Collider GetCollider()
	{
		return cc;
	}

	public bool InPlayingState()
	{
		if (State == DEAD_STATE || State == WAIT_REBIRTH_STATE || State == WAIT_VS_REBIRTH_STATE || State == WIN_STATE || State == LOSE_STATE)
		{
			return false;
		}
		return true;
	}

	public bool IsSameTeam(Player player)
	{
		if (GameApp.GetInstance().GetGameMode().IsTeamMode())
		{
			return Team == player.Team;
		}
		return false;
	}

	public virtual void Init()
	{
		VSStatistics = new PlayerVSStatistics();
		GameObject gameObject = AvatarBuilder.GetInstance().ReBuildPlayerAvatar(userState, this);
		SetGameObject(gameObject);
		GameObject gameObject2 = GameObject.FindGameObjectWithTag(TagName.RESPAWN);
		gameObject.transform.position = gameObject2.transform.position;
		gameObject.name = "Player";
		gameObject.layer = PhysicsLayer.PLAYER;
		weaponList = userState.GetBattleWeapons();
		foreach (Weapon weapon in weaponList)
		{
			weapon.Init(this);
		}
		if (IsLocal())
		{
			ChangeWeaponInBag(userState.GetWeaponBagIndex(weaponList[0]));
		}
		else
		{
			ChangeWeapon(weaponList[0]);
		}
		monsterCash = 0;
		bonusCash = 0;
		pickupCash = 0;
		exp = 0;
		bossCash = 0;
		bossMithril = 0;
		State = IDLE_STATE;
		MaxHp = 0;
		Hp = MaxHp;
		bagNum = userState.GetBagNum();
		inputController = new InputController();
		State = IDLE_STATE;
		UpdateNearestPoint();
		lastUpdateNearestPointTimer.SetTimer(1f, false);
		if (playerSkill.GetSkill(SkillsType.BLOCK_AT_A_RATE) > 0f)
		{
			CreateDefent();
			showDefentTimer.SetTimer(0.8f, false);
		}
		GameObject original = Resources.Load("Effect/power2") as GameObject;
		if (playerSkill.GetSkill(SkillsType.POWER_UP) > 0f)
		{
			powerObj[0] = UnityEngine.Object.Instantiate(original, playerTransform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
			powerObj[0].transform.parent = playerTransform;
			powerObj[0].SetActiveRecursively(false);
		}
		if (playerSkill.GetSkill(SkillsType.SPEED_UP) > 0f)
		{
			powerObj[1] = UnityEngine.Object.Instantiate(original, playerTransform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
			powerObj[1].transform.parent = playerTransform;
			powerObj[1].SetActiveRecursively(false);
			Color color = new Color(0f, 0f, 0f, 1f);
			color.b = 1f;
			powerObj[1].GetComponent<Renderer>().material.SetColor("_TintColor", color);
		}
		if (playerSkill.GetSkill(SkillsType.DEFENCE_UP) > 0f)
		{
			powerObj[2] = UnityEngine.Object.Instantiate(original, playerTransform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
			powerObj[2].transform.parent = playerTransform;
			powerObj[2].SetActiveRecursively(false);
			Color color2 = new Color(0f, 0f, 0f, 1f);
			color2.g = 1f;
			powerObj[2].GetComponent<Renderer>().material.SetColor("_TintColor", color2);
		}
		if (playerSkill.GetSkill(SkillsType.ANDROMEDA_UP) > 0f)
		{
			powerObj[3] = UnityEngine.Object.Instantiate(original, playerTransform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
			powerObj[3].transform.parent = playerTransform;
			powerObj[3].SetActiveRecursively(false);
			Color color3 = new Color(0f, 0f, 0f, 1f);
			color3.r = 1f;
			color3.b = 1f;
			powerObj[3].GetComponent<Renderer>().material.SetColor("_TintColor", color3);
		}
		if (playerSkill.GetSkill(SkillsType.HEALTH_STEAL) > 0f)
		{
			powerObj[4] = UnityEngine.Object.Instantiate(original, playerTransform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
			powerObj[4].transform.parent = playerTransform;
			powerObj[4].SetActiveRecursively(false);
			Color color4 = new Color(0f, 0f, 0f, 1f);
			color4.r = 1f;
			color4.b = 1f;
			powerObj[4].GetComponent<Renderer>().material.SetColor("_TintColor", color4);
		}
		if (playerSkill.GetSkill(SkillsType.ATTACK_SHIELD) > 0f)
		{
			GameObject original2 = Resources.Load("Effect/update_effect/effect_fk_001") as GameObject;
			powerObj[5] = UnityEngine.Object.Instantiate(original2, playerTransform.position + Vector3.up * 1.2f, Quaternion.identity) as GameObject;
			powerObj[5].transform.parent = playerTransform;
			powerObj[5].SetActiveRecursively(false);
		}
		if (playerSkill.GetSkill(SkillsType.HURT_HEALTH) > 0f)
		{
			GameObject original3 = Resources.Load("SW2_Effect/Shield Sphere") as GameObject;
			powerObj[8] = UnityEngine.Object.Instantiate(original3, playerTransform.position + Vector3.up * 0.5f + Vector3.right * 0.07f, Quaternion.identity) as GameObject;
			powerObj[8].transform.parent = playerTransform;
			powerObj[8].SetActiveRecursively(false);
		}
		GameObject original4 = Resources.Load("Effect/update_effect/eff_speeddown_001") as GameObject;
		mSlowDownEffectObj = UnityEngine.Object.Instantiate(original4, playerTransform.position, Quaternion.identity) as GameObject;
		mSlowDownEffectObj.transform.parent = playerTransform;
		mSlowDownEffectObj.SetActiveRecursively(false);
		GameObject original5 = Resources.Load("ZB_effect/black_hole") as GameObject;
		mTrackEffectObj = UnityEngine.Object.Instantiate(original5, playerTransform.position + Vector3.up * 1f, Quaternion.identity) as GameObject;
		mTrackEffectObj.transform.parent = playerTransform;
		mTrackEffectObj.SetActive(false);
		itemAssistEffectObj = UnityEngine.Object.Instantiate(original, playerTransform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
		itemAssistEffectObj.transform.parent = playerTransform;
		itemAssistEffectObj.SetActiveRecursively(false);
		for (int i = 0; i < 10; i++)
		{
			powerTimer[i] = new Timer();
			powerCDTimer[i] = new Timer();
		}
		powerTimer[0].SetTimer(10f, false);
		powerCDTimer[0].SetTimer(30f, true);
		powerTimer[1].SetTimer(30f, false);
		powerCDTimer[1].SetTimer(90f, true);
		powerTimer[2].SetTimer(5f, false);
		powerCDTimer[2].SetTimer(40f, true);
		powerTimer[3].SetTimer(15f, false);
		powerCDTimer[3].SetTimer(100f, true);
		powerTimer[4].SetTimer(10f, false);
		powerCDTimer[4].SetTimer(120f, true);
		powerTimer[5].SetTimer(10f, false);
		powerCDTimer[5].SetTimer(55f, true);
		powerTimer[6].SetTimer(1f, false);
		powerCDTimer[6].SetTimer(60f, true);
		powerTimer[7].SetTimer(1f, false);
		powerCDTimer[7].SetTimer(30f, true);
		powerTimer[8].SetTimer(10f, false);
		powerCDTimer[8].SetTimer(60f, true);
		powerTimer[9].SetTimer(1f, false);
		powerCDTimer[9].SetTimer(90f, true);
		comboTimer.SetTimer(3f, false);
		Transform transform = (Transform)gameObject.GetComponent("Transform");
		mSpine = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
		mHead = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head");
		mWeapon = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/r hand gun");
		mArms = new AimArm[2]
		{
			new AimArm(transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm"), transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm"), transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand")),
			new AimArm(transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm"), transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm"), transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand"))
		};
		GameObject original6 = Resources.Load("Effect/Level/chuangsong") as GameObject;
		chuangsongObj = UnityEngine.Object.Instantiate(original6, playerTransform.position, Quaternion.identity) as GameObject;
		chuangsongObj.transform.position = playerTransform.position + Vector3.up;
		chuangsongObj.transform.parent = playerTransform;
		chuangsongObj.SetActiveRecursively(false);
		Transform transform2 = playerTransform.Find(BoneName.Bag).Find("Bag");
		FlyBagAnimationScript component = transform2.GetComponent<FlyBagAnimationScript>();
		if (component != null)
		{
			component.player = this;
		}
		bagUVOffsetScript = transform2.GetComponent<UVOffsetScript>();
		initEnegy = userState.Enegy;
		NeverGotHit = true;
		JustMakeAShoot = false;
		SendingRebirthRequest = false;
		lastBloodEffectTimer.SetTimer(0.3f, false);
		SlowEffect = 1f;
		mIsSlowDown = false;
		mSlowDownTimer.SetTimer(3f, true);
		mAimTimer.SetTimer(5f, true);
		trackingGrenadeDic.Clear();
		flyGrenadeDic.Clear();
		SniperAttack = false;
	}

	public void CreateWeaponDamageBooth()
	{
		foreach (Weapon weapon in weaponList)
		{
			weapon.DamageBoothByArmor();
		}
	}

	public void InitSkills()
	{
		playerSkill.CreateSkills();
		for (int i = 0; i < Global.AVATAR_PART_NUM; i++)
		{
			Armor armor = userState.GetArmor(i);
			List<Skill> skills = armor.GetSkills();
			foreach (Skill item in skills)
			{
				playerSkill.AddSkill(item);
			}
		}
		Skill skill = new Skill();
		skill.skillType = SkillsType.TEAM_DAMAGE_REDUCE;
		skill.data = -0.9f;
		if (!userState.ArmorInOneCollection())
		{
			return;
		}
		int armorGroupID = userState.GetArmor(0).GetArmorGroupID();
		ArmorRewards armorRewards = userState.GetArmorRewards()[armorGroupID];
		List<Skill> skills2 = armorRewards.GetSkills();
		foreach (Skill item2 in skills2)
		{
			playerSkill.AddSkill(item2);
		}
	}

	public virtual void Loop(float deltaTime)
	{
		if (lastUpdateNearestPointTimer.Ready())
		{
			UpdateNearestPoint();
			lastUpdateNearestPointTimer.Do();
		}
		for (int i = 0; i < 10; i++)
		{
			if (powerTimer[i].Ready())
			{
				PowerUp(false, i);
			}
		}
		if (comboTimer.Ready())
		{
			ComboClear();
			comboTimer.Do();
		}
		if (mAim && mAimTimer.Ready())
		{
			mAim = false;
			isAiming = false;
			SlowEffect = 1f;
			if (mTrackEffectObj != null)
			{
				mTrackEffectObj.SetActive(false);
			}
		}
		if (mIsSlowDown && mSlowDownTimer.Ready())
		{
			mIsSlowDown = false;
			SlowEffect = 1f;
			if (mSlowDownEffectObj != null)
			{
				mSlowDownEffectObj.SetActiveRecursively(false);
			}
		}
		if (assistItems.Count > 0)
		{
			if (!itemAssistEffectObj.active || lastAssistItemCount != assistItems.Count)
			{
				itemAssistEffectObj.SetActiveRecursively(true);
				Color color = new Color(0f, 0f, 0f, 1f);
				if (GetSpeedItemAssist() > 0f)
				{
					color.b = 1f;
				}
				if (GetAttackItemAssist() > 0f)
				{
					color.r = 1f;
				}
				if (GetDamageReduceItemAssist() != 0f)
				{
					color.g = 1f;
				}
				itemAssistEffectObj.GetComponent<Renderer>().material.SetColor("_TintColor", color);
				lastAssistItemCount = assistItems.Count;
			}
		}
		else
		{
			itemAssistEffectObj.SetActiveRecursively(false);
		}
		for (int num = assistItems.Count - 1; num >= 0; num--)
		{
			if (assistItems[num].TimeOut())
			{
				assistItems.Remove(assistItems[num]);
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			VSStatistics.UpdateCombo();
		}
		if (userState.GetAvatar()[4] == 20)
		{
			if (IsPowerUp(4))
			{
				PlayBagUVAnimation(true);
			}
			else
			{
				PlayBagUVAnimation(false);
			}
		}
	}

	public void CreateDeadBlood()
	{
		GameObject gameObject = null;
		GameObject gameObject2 = null;
		int num = Random.Range(0, 100);
		FloorInfo ground = GetGround();
		gameObject = ((num >= 50) ? (Resources.Load("Effect/BugDeadBlood2") as GameObject) : (Resources.Load("Effect/BugDeadBlood") as GameObject));
		int num2 = num % 3 + 1;
		gameObject2 = Resources.Load("Effect/Blood_Ground" + num2) as GameObject;
		UnityEngine.Object.Instantiate(gameObject, playerTransform.position + Vector3.up * 1.2f, Quaternion.identity);
		GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, new Vector3(playerTransform.position.x, ground.height + 0.1f, playerTransform.position.z), Quaternion.Euler(270f, 0f, 0f)) as GameObject;
		Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, ground.normal);
		gameObject3.transform.rotation = quaternion * gameObject3.transform.rotation;
	}

	public void CreatePlayerSign()
	{
		Transform transform = playerTransform.Find("teamsign");
		if (transform != null)
		{
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		GameObject gameObject = null;
		bool flag = false;
		GameObject gameObject2 = null;
		Vector3 position = playerTransform.position + Vector3.up * 2.6f;
		if (GetUserID() == GameApp.GetInstance().GetGameWorld().FlagInPlayerID)
		{
			int loadedLevel = Application.loadedLevel;
			gameObject = ((loadedLevel != 13) ? (Resources.Load("Loot/FlagOnPlayer") as GameObject) : (Resources.Load("Loot/FlagOnPlayer_2") as GameObject));
			position = playerTransform.position + Vector3.up * 2.06f;
			flag = true;
			gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
			gameObject2.name = "teamsign";
			gameObject2.transform.position = position;
			gameObject2.transform.parent = playerTransform;
		}
		else
		{
			if (!flag && IsLocal())
			{
				return;
			}
			gameObject = Resources.Load("Avatar/TeamSign/P" + (seatID + 1)) as GameObject;
			gameObject2 = UnityEngine.Object.Instantiate(gameObject, position, Quaternion.identity) as GameObject;
			gameObject2.name = "teamsign";
			gameObject2.transform.parent = playerTransform;
		}
		Renderer renderer = gameObject2.GetComponent<Renderer>();
		Color white = Color.white;
		if (GameApp.GetInstance().GetGameMode().IsTeamMode())
		{
			white = UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)Team];
		}
		else
		{
			white = UIConstant.COLOR_PLAYER_ICONS[seatID];
			white = new Color(white.r * 0.5f, white.g * 0.5f, white.b * 0.5f, white.a);
		}
		if (renderer != null)
		{
			renderer.material.SetColor("_TintColor", white);
		}
	}

	public void PlayTeleportAnimation()
	{
		chuangsongObj.GetComponent<Animation>().Stop();
		chuangsongObj.SetActiveRecursively(true);
		chuangsongObj.GetComponent<Animation>()["Take 001"].wrapMode = WrapMode.ClampForever;
		chuangsongObj.GetComponent<Animation>().Play("Take 001");
	}

	public bool TeleportAnimationEnds()
	{
		if (GameApp.GetInstance().GetGameWorld().State == GameState.SwitchBossLevel)
		{
			if (chuangsongObj.GetComponent<Animation>()["Take 001"].time >= chuangsongObj.GetComponent<Animation>()["Take 001"].clip.length * 0.5f && teleportStartFadeTime < 0f)
			{
				ActivatePlayer(false);
				EnableTeleportEffect(true);
				teleportStartFadeTime = Time.time;
			}
			if (!bStartFade && teleportStartFadeTime > 0f && Time.time - teleportStartFadeTime > 0.5f)
			{
				FadeAnimationScript.GetInstance().StartFade(new Color(1f, 1f, 1f, 0f), Color.white, 1f);
				bStartFade = true;
			}
			if (teleportStartFadeTime > 0f && Time.time - teleportStartFadeTime > 1.5f)
			{
				return true;
			}
		}
		return false;
	}

	public void EnableTeleportEffect(bool bEnable)
	{
		if (!bEnable)
		{
			chuangsongObj.SetActiveRecursively(false);
		}
		else
		{
			chuangsongObj.SetActiveRecursively(true);
		}
	}

	public void ActivatePlayer(bool bEnable)
	{
		if (!bEnable)
		{
			playerTransform.Find("body").GetComponent<Renderer>().enabled = false;
			playerTransform.Find("head").GetComponent<Renderer>().enabled = false;
			playerTransform.Find("hand").GetComponent<Renderer>().enabled = false;
			playerTransform.Find("foot").GetComponent<Renderer>().enabled = false;
			Transform transform = playerTransform.Find(BoneName.Bag + "/Bag");
			if (transform.GetComponent<Renderer>() != null)
			{
				transform.GetComponent<Renderer>().enabled = false;
			}
			else
			{
				int childCount = transform.GetChildCount();
				for (int i = 0; i < childCount; i++)
				{
					transform.GetChild(i).gameObject.SetActiveRecursively(false);
				}
			}
			weapon.EnableGunObject(false);
			return;
		}
		playerTransform.Find("body").GetComponent<Renderer>().enabled = true;
		playerTransform.Find("head").GetComponent<Renderer>().enabled = true;
		playerTransform.Find("hand").GetComponent<Renderer>().enabled = true;
		playerTransform.Find("foot").GetComponent<Renderer>().enabled = true;
		Transform transform2 = playerTransform.Find(BoneName.Bag + "/Bag");
		if (transform2.GetComponent<Renderer>() != null)
		{
			transform2.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			int childCount2 = transform2.GetChildCount();
			for (int j = 0; j < childCount2; j++)
			{
				transform2.GetChild(j).gameObject.SetActiveRecursively(true);
			}
		}
		weapon.EnableGunObject(true);
	}

	public bool UseItem(byte bagIndex)
	{
		Item item = GameApp.GetInstance().GetUserState().GetItem(bagIndex);
		if (item != null && item.Use(this, bagIndex))
		{
			GameApp.GetInstance().GetUserState().ClearItem(bagIndex);
			return true;
		}
		return false;
	}

	public void UseItemByItemID(byte itemID, float hpRate)
	{
		Item item = ItemFactory.GetInstance().CreateItem(itemID);
		item.LoadConfig();
		item.TakeEffect(this, hpRate);
	}

	public int GetItemsCount()
	{
		int num = 0;
		int[] bagPosition = GameApp.GetInstance().GetUserState().GetBagPosition();
		for (int i = 0; i < bagPosition.Length; i++)
		{
			if (bagPosition[i] > 80)
			{
				num++;
			}
		}
		return num;
	}

	public void ItemAssist(AssistItem item)
	{
		assistItems.Add(item);
	}

	public float GetSpeedItemAssist()
	{
		float num = 0f;
		float num2 = 0f;
		foreach (AssistItem assistItem in assistItems)
		{
			num = Mathf.Max(num, assistItem.GetSpeedBooth());
			num2 = Mathf.Max(num2, assistItem.GetSpeedBoothWhenGotHit());
		}
		return num + num2;
	}

	public float GetAttackItemAssist()
	{
		float num = 0f;
		float num2 = 0f;
		foreach (AssistItem assistItem in assistItems)
		{
			num = Mathf.Max(num, assistItem.GetAttackBooth());
			num2 = Mathf.Max(num2, assistItem.GetAttackBoothWhenSecureFlag());
		}
		return num + num2;
	}

	public float GetDamageReduceItemAssist()
	{
		float num = 0f;
		foreach (AssistItem assistItem in assistItems)
		{
			num = Mathf.Min(num, assistItem.GetDamageReduce());
		}
		return num;
	}

	public bool IsPowerUpCoolDown(int initiativeSkillIndex)
	{
		return powerCDTimer[initiativeSkillIndex].Ready();
	}

	public Timer GetPowerTimer(int initiativeSkillIndex)
	{
		return powerTimer[initiativeSkillIndex];
	}

	public Timer GetPowerCDTimer(int initiativeSkillIndex)
	{
		return powerCDTimer[initiativeSkillIndex];
	}

	public void AddTempBuff(EffectsType effectType, int duration, float data)
	{
		Item item = new AssistItem((ItemID)0);
		item.Time = duration;
		item.CreateEmptyItem();
		item.AddEffect(effectType, data);
		item.TakeEffect(this, 0f);
	}

	public void PlayBagAlphaAnimation()
	{
		if (bagAlphaAnimationScript != null)
		{
			bagAlphaAnimationScript.StartAnimation();
		}
	}

	public void PlayBagUVAnimation(bool bPlay)
	{
		if (bagUVOffsetScript != null)
		{
			if (bPlay)
			{
				bagUVOffsetScript.enabled = true;
				return;
			}
			bagUVOffsetScript.enabled = false;
			bagUVOffsetScript.GetComponent<Renderer>().material.SetTextureOffset("_tex2", new Vector2(0.66f, 0.66f));
		}
	}

	public virtual void PowerUp(bool enable, int powerUpSkillIndex)
	{
		if (powerObj[powerUpSkillIndex] != null)
		{
			powerObj[powerUpSkillIndex].SetActiveRecursively(enable);
		}
		if (enable)
		{
			powerTimer[powerUpSkillIndex].Do();
			powerCDTimer[powerUpSkillIndex].Do();
		}
	}

	public bool IsPowerUp(int powerUpSkillIndex)
	{
		if (powerObj[powerUpSkillIndex] != null)
		{
			if (powerObj[powerUpSkillIndex].active)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public void CreateDefent()
	{
		GameObject original = Resources.Load("Effect/defent") as GameObject;
		defentObj = UnityEngine.Object.Instantiate(original, playerTransform.position + Vector3.up * 0.5f + Vector3.forward * 0.6f, playerTransform.rotation) as GameObject;
		defentObj.transform.parent = playerTransform;
		for (int i = 0; i < 3; i++)
		{
			defentScript[i] = defentObj.transform.GetChild(i).GetComponent<FadeInAlphaAnimationScript>();
			defentOnebyOneScript[i] = defentObj.transform.GetChild(i).GetComponent<DefentOneByOneScript>();
		}
		showDefentTimer.SetTimer(0.8f, false);
	}

	public void ShowDefent()
	{
		if (showDefentTimer.Ready())
		{
			int num = Random.Range(-80, 80);
			defentObj.transform.rotation = playerTransform.rotation;
			defentObj.transform.rotation = Quaternion.AngleAxis(num, Vector3.up) * playerTransform.rotation;
			Vector3 normalized = defentObj.transform.TransformDirection(Vector3.forward).normalized;
			defentObj.transform.position = playerTransform.position + normalized * 0.6f + Vector3.up * 1.4f;
			for (int i = 0; i < 3; i++)
			{
				defentOnebyOneScript[i].appearTime = Time.time + (float)i * 0.05f;
				defentScript[i].FadeIn();
			}
			showDefentTimer.Do();
		}
	}

	public void RecoveryWhenMakeKills()
	{
		float num = GetSkills().GetSkill(SkillsType.HP_RECOVERY_WHEN_MAKE_KILL);
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			num = VSMath.GetKillRecoverInVS(num);
		}
		(this as LocalPlayer).RecoveHP(num);
	}

	public float GetDefenceSkillByWeaponType(WeaponType wType)
	{
		float result = 0f;
		switch (wType)
		{
		case WeaponType.AssaultRifle:
		case WeaponType.AdvancedAssaultRifle:
			result = playerSkill.GetSkill(SkillsType.ASSAULT_DEFENCE);
			break;
		case WeaponType.ShotGun:
		case WeaponType.AdvancedShotGun:
			result = playerSkill.GetSkill(SkillsType.SHOTGUN_DEFENCE);
			break;
		case WeaponType.RocketLauncher:
		case WeaponType.AutoRocketLauncher:
			result = playerSkill.GetSkill(SkillsType.RPG_DEFENCE);
			break;
		case WeaponType.LightBow:
		case WeaponType.AutoBow:
		case WeaponType.TheArrow:
			result = playerSkill.GetSkill(SkillsType.BOW_DEFENCE);
			break;
		case WeaponType.GrenadeLauncher:
		case WeaponType.AdvancedGrenadeLauncher:
		case WeaponType.FlyGrenadeLauncher:
			result = playerSkill.GetSkill(SkillsType.GRENADE_DEFENCE);
			break;
		case WeaponType.LaserGun:
			result = playerSkill.GetSkill(SkillsType.LASER_CANNON_DEFENCE);
			break;
		case WeaponType.LaserRifle:
			result = playerSkill.GetSkill(SkillsType.LASER_DEFENCE);
			break;
		case WeaponType.MachineGun:
		case WeaponType.AdvancedMachineGun:
			result = playerSkill.GetSkill(SkillsType.MACHINE_DEFENCE);
			break;
		case WeaponType.PlasmaNeo:
			result = playerSkill.GetSkill(SkillsType.PLASMA_DEFENCE);
			break;
		case WeaponType.LightFist:
		case WeaponType.Spring:
			result = playerSkill.GetSkill(SkillsType.GLOVE_DEFENCE);
			break;
		case WeaponType.Sword:
		case WeaponType.AdvancedSword:
			result = playerSkill.GetSkill(SkillsType.SWORD_DEFENCE);
			break;
		case WeaponType.Sniper:
		case WeaponType.AdvancedSniper:
		case WeaponType.RelectionSniper:
			result = playerSkill.GetSkill(SkillsType.SNIPER_DEFENCE);
			break;
		case WeaponType.TrackingGun:
			result = playerSkill.GetSkill(SkillsType.TRACKINGGUN_DEFENCE);
			break;
		case WeaponType.PingPongLauncher:
			result = playerSkill.GetSkill(SkillsType.PINGPONG_DEFENCE);
			break;
		}
		return result;
	}

	public virtual bool IsPlayingAnimation(string name)
	{
		return playerObj.GetComponent<Animation>().IsPlaying(name);
	}

	public virtual void PlayAnimation(string name, WrapMode mode)
	{
		if (!playerObj.GetComponent<Animation>().IsPlaying(name) || mode != WrapMode.ClampForever || AnimationPlayed(name, 1f))
		{
			playerObj.GetComponent<Animation>()[name].wrapMode = mode;
			playerObj.GetComponent<Animation>().CrossFade(name, 0.2f);
		}
	}

	public virtual void PlayAnimationWithoutBlend(string name, WrapMode mode)
	{
		if (!playerObj.GetComponent<Animation>().IsPlaying(name) || mode != WrapMode.ClampForever || AnimationPlayed(name, 1f))
		{
			playerObj.GetComponent<Animation>()[name].wrapMode = mode;
			playerObj.GetComponent<Animation>().Play(name);
		}
	}

	public virtual void PlayAnimationAllLayers(string name, WrapMode mode)
	{
		if (!playerObj.GetComponent<Animation>().IsPlaying(name) || mode != WrapMode.ClampForever)
		{
			playerObj.GetComponent<Animation>()[name].wrapMode = mode;
			playerObj.GetComponent<Animation>().CrossFade(name, 0.1f, PlayMode.StopAll);
		}
	}

	public virtual void StopAnimation(string name)
	{
		playerObj.GetComponent<Animation>().Stop(name);
	}

	public bool AnimationPlayed(string name, float percent)
	{
		if (playerObj.GetComponent<Animation>()[name] == null)
		{
			return false;
		}
		if (playerObj.GetComponent<Animation>()[name].speed >= 0f)
		{
			if (playerObj.GetComponent<Animation>()[name].time >= playerObj.GetComponent<Animation>()[name].clip.length * percent)
			{
				return true;
			}
			return false;
		}
		if (playerObj.GetComponent<Animation>()[name].time <= playerObj.GetComponent<Animation>()[name].clip.length * (1f - percent))
		{
			return true;
		}
		return false;
	}

	public bool AnimationPlayedLoop(string name, float percent)
	{
		int num = (int)(playerObj.GetComponent<Animation>()[name].time / playerObj.GetComponent<Animation>()[name].clip.length);
		float num2 = playerObj.GetComponent<Animation>()[name].time - playerObj.GetComponent<Animation>()[name].clip.length * (float)num;
		if (playerObj.GetComponent<Animation>()[name] == null)
		{
			return false;
		}
		if (playerObj.GetComponent<Animation>()[name].speed >= 0f)
		{
			if (num2 >= playerObj.GetComponent<Animation>()[name].clip.length * percent)
			{
				return true;
			}
			return false;
		}
		if (num2 <= playerObj.GetComponent<Animation>()[name].clip.length * (1f - percent))
		{
			return true;
		}
		return false;
	}

	public void AdjustAnimationInLateUpdate(float deltaTime)
	{
		if (State != ATTACK_STATE)
		{
			return;
		}
		Weapon weapon = GetWeapon();
		if (weapon != null && weapon.GetWeaponType() == WeaponType.LaserGun)
		{
			LaserCannon laserCannon = weapon as LaserCannon;
			if (laserCannon != null && laserCannon.IsOverHeat)
			{
				return;
			}
		}
		float num = (AngleV * weapon.Adjuster.angleScaleV + weapon.Adjuster.angleOffsetV) * ((float)Math.PI / 180f);
		Matrix4x4 identity = Matrix4x4.identity;
		identity[0, 0] = Mathf.Cos(num / 2f);
		identity[0, 1] = 0f - Mathf.Sin(num / 2f);
		identity[1, 0] = Mathf.Sin(num / 2f);
		identity[1, 1] = Mathf.Cos(num / 2f);
		Matrix4x4 matrix4x = Util.CreateMatrixPosition(new Vector3(0f, weapon.Adjuster.pivotOffset, 0f));
		Vector3 position = (mWeapon.localToWorldMatrix * matrix4x).GetColumn(3);
		Matrix4x4 matrix4x2 = ((weapon.GetWeaponType() == WeaponType.LightFist || weapon.GetWeaponType() == WeaponType.LightBow || weapon.GetWeaponType() == WeaponType.AutoBow || weapon.GetWeaponType() == WeaponType.Spring || weapon.GetWeaponType() == WeaponType.TheArrow) ? Util.CreateMatrix(-mWeapon.forward, mWeapon.right, -mWeapon.up, position) : Util.CreateMatrix(mWeapon.right, mWeapon.forward, -mWeapon.up, position));
		Matrix4x4 matrix4x3 = matrix4x2.inverse * mWeapon.localToWorldMatrix;
		Matrix4x4 identity2 = Matrix4x4.identity;
		identity2[1, 1] = Mathf.Cos(num);
		identity2[1, 2] = Mathf.Sin(num);
		identity2[2, 1] = 0f - Mathf.Sin(num);
		identity2[2, 2] = Mathf.Cos(num);
		Matrix4x4 matrix4x4 = matrix4x2 * identity2 * matrix4x3;
		if (null != mSpine)
		{
			Matrix4x4 matrix4x5 = Util.RelativeMatrix(mSpine, playerTransform);
			Matrix4x4 matrix4x6 = matrix4x5 * identity;
			Matrix4x4 m = playerTransform.localToWorldMatrix * matrix4x6;
			mSpine.rotation = Util.QuaternionFromMatrix(m);
		}
		if (null != mHead)
		{
			Matrix4x4 matrix4x7 = Util.RelativeMatrix(mHead, playerTransform);
			Matrix4x4 matrix4x8 = matrix4x7 * identity;
			Matrix4x4 m2 = playerTransform.localToWorldMatrix * matrix4x8;
			mHead.rotation = Util.QuaternionFromMatrix(m2);
		}
		for (int i = 0; i < mArms.Length; i++)
		{
			if (null != mArms[i].mShoulder && null != mArms[i].mElbow && null != mArms[i].mShoulder)
			{
				mArms[i].mHandRelativeToWeapon = Util.RelativeMatrix(mArms[i].mHand, mWeapon);
				Vector3 target = (matrix4x4 * mArms[i].mHandRelativeToWeapon).MultiplyPoint3x4(Vector3.zero);
				ikSolver.Solve(new Transform[3]
				{
					mArms[i].mShoulder,
					mArms[i].mElbow,
					mArms[i].mHand
				}, target);
				mArms[i].mHand.rotation = Util.QuaternionFromMatrix(matrix4x4 * mArms[i].mHandRelativeToWeapon);
			}
		}
	}

	public void AddMixingTransformAnimation(string weaponSuffix)
	{
		if (playerObj.GetComponent<Animation>()[AnimationString.RunAttack + weaponSuffix] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.RunAttack + weaponSuffix].AddMixingTransform(playerTransform.Find(BoneName.UpperBody));
		}
		if (playerObj.GetComponent<Animation>()[AnimationString.FlyAttack + weaponSuffix] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.FlyAttack + weaponSuffix].AddMixingTransform(playerTransform.Find(BoneName.UpperBody));
		}
		if (playerObj.GetComponent<Animation>()[AnimationString.FlyIdle + weaponSuffix] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.FlyIdle + weaponSuffix].AddMixingTransform(playerTransform.Find(BoneName.UpperBody));
		}
		if (playerObj.GetComponent<Animation>()[AnimationString.Fly + weaponSuffix] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.Fly + weaponSuffix].AddMixingTransform(playerTransform.Find(BoneName.UpperBody));
		}
	}

	public void SetLowerBodyAnimation(string weaponSuffix, int layer)
	{
		if (playerObj.GetComponent<Animation>()[AnimationString.Run + weaponSuffix] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.Run + weaponSuffix].layer = layer;
		}
		if (playerObj.GetComponent<Animation>()[AnimationString.Idle + weaponSuffix] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.Idle + weaponSuffix].layer = layer;
		}
		if (playerObj.GetComponent<Animation>()[AnimationString.Attack + weaponSuffix] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.Attack + weaponSuffix].layer = layer;
		}
	}

	public void SetGameObject(GameObject obj)
	{
		playerObj = obj;
		playerTransform = obj.transform;
		cc = playerObj.GetComponent<Collider>() as CharacterController;
		if (playerObj.GetComponent<Animation>()[AnimationString.FlyIdle] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.FlyIdle].layer = -1;
		}
		if (playerObj.GetComponent<Animation>()["fly_stand_shoot_jian_lower"] != null)
		{
			playerObj.GetComponent<Animation>()["fly_stand_shoot_jian_lower"].layer = -1;
		}
		if (playerObj.GetComponent<Animation>()[AnimationString.FlyForward] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.FlyForward].layer = -1;
		}
		if (playerObj.GetComponent<Animation>()[AnimationString.FlyBack] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.FlyBack].layer = -1;
		}
		if (playerObj.GetComponent<Animation>()[AnimationString.FlyLeft] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.FlyLeft].layer = -1;
		}
		if (playerObj.GetComponent<Animation>()[AnimationString.FlyRight] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.FlyRight].layer = -1;
		}
		SetLowerBodyAnimation("_rifle", -1);
		SetLowerBodyAnimation("_shotgun", -1);
		SetLowerBodyAnimation("_bazinga", -1);
		SetLowerBodyAnimation("_grenade_launcher", -1);
		SetLowerBodyAnimation("_BLACKSTARS", -1);
		SetLowerBodyAnimation("_laser", -1);
		SetLowerBodyAnimation("_bow", -1);
		SetLowerBodyAnimation("_fist", -1);
		SetLowerBodyAnimation("_Sniper", -1);
		AddMixingTransformAnimation("_rifle");
		AddMixingTransformAnimation("_shotgun");
		AddMixingTransformAnimation("_bazinga");
		AddMixingTransformAnimation("_grenade_launcher");
		AddMixingTransformAnimation("_BLACKSTARS");
		AddMixingTransformAnimation("_laser");
		AddMixingTransformAnimation("_bow");
		AddMixingTransformAnimation("_fist");
		AddMixingTransformAnimation("_Sniper");
		if (playerObj.GetComponent<Animation>()[AnimationString.FlyRunAttack + "_machinegun"] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.FlyRunAttack + "_machinegun"].AddMixingTransform(playerTransform.Find(BoneName.UpperBody));
		}
		if (playerObj.GetComponent<Animation>()[AnimationString.FlyRunAttack + "_jian"] != null)
		{
			playerObj.GetComponent<Animation>()[AnimationString.FlyRunAttack + "_jian"].AddMixingTransform(playerTransform.Find(BoneName.UpperBody));
		}
		if (GetSkills().GetSkill(SkillsType.FLY) > 0f)
		{
			AddMixingTransformAnimation("_machinegun");
			AddMixingTransformAnimation("_jian");
		}
	}

	public void PlayWalkSound()
	{
		if (runningPhase == -1 && IsPlayingAnimation(AnimationString.Run + GetWeaponAnimationSuffixAlter()) && !AnimationPlayedLoop(AnimationString.Run + GetWeaponAnimationSuffixAlter(), 0.5f))
		{
			runningPhase = 0;
		}
		else if (runningPhase == 0 && AnimationPlayedLoop(AnimationString.Run + GetWeaponAnimationSuffixAlter(), 0.5f))
		{
			runningPhase = 1;
			AudioManager.GetInstance().PlaySoundAt("Audio/walk_crement", GetTransform().position);
		}
		else if (runningPhase == 1 && AnimationPlayedLoop(AnimationString.Run + GetWeaponAnimationSuffixAlter(), 0.96f))
		{
			runningPhase = -1;
			AudioManager.GetInstance().PlaySoundAt("Audio/walk_crement", GetTransform().position);
		}
	}

	public void ResetRunningPhase()
	{
		runningPhase = -1;
	}

	public void DropAtSpawnPosition()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.RESPAWN);
		GetTransform().position = array[seatID].transform.position;
		GetTransform().rotation = array[seatID].transform.rotation;
		if (IsLocal())
		{
			ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
			if (null != component)
			{
				component.AngelH = array[seatID].transform.rotation.eulerAngles.y;
			}
		}
		GameObject original = Resources.Load("Loot/item_gold") as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original, playerTransform.position + Vector3.up, Quaternion.identity) as GameObject;
	}

	public void DropAtSpawnPositionVS()
	{
		GameObject gameObject = GameObject.Find(ObjectNamePrefix.PLAYER_SPAWN_POINT_FULL_PATH + seatID);
		GetTransform().position = gameObject.transform.position;
		GetTransform().rotation = gameObject.transform.rotation;
		if (IsLocal())
		{
			ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
			if (null != component)
			{
				component.AngelH = gameObject.transform.rotation.eulerAngles.y;
			}
		}
		GameObject original = Resources.Load("Loot/item_gold") as GameObject;
		GameObject gameObject2 = UnityEngine.Object.Instantiate(original, playerTransform.position + Vector3.up, Quaternion.identity) as GameObject;
	}

	public void ReSpawnAtPoint(int index)
	{
		playerObj.GetComponent<Animation>().Stop();
		if (IsLocal())
		{
			Transform transform = Camera.main.transform.Find("Screen_DeadBlood");
			transform.GetComponent<Renderer>().enabled = false;
			GameApp.GetInstance().GetGameWorld().State = GameState.Playing;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.RESPAWN);
		GetTransform().position = array[index].transform.position;
		GetTransform().rotation = array[index].transform.rotation;
		SetState(IDLE_STATE);
		if (!Application.isMobilePlatform)
		{
			Screen.lockCursor = true;
		}
		Hp = MaxHp;
		SendingRebirthRequest = false;
		if (IsLocal())
		{
			ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
			if (null != component)
			{
				component.AngelH = array[index].transform.rotation.eulerAngles.y;
			}
		}
		GameObject original = Resources.Load("Loot/item_gold") as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original, playerTransform.position + Vector3.up, Quaternion.identity) as GameObject;
	}

	public bool DoWaitVSRebirth()
	{
		return vsRebirthTimer.Ready();
	}

	public void OnWaitVSRebirth()
	{
		vsRebirthTimer = new Timer();
		vsRebirthTimer.SetTimer(11f, false);
		GameObject gameObject = GameObject.Find("GameUI");
		if (gameObject != null)
		{
			InGameUIScript component = gameObject.GetComponent<InGameUIScript>();
			if (component != null)
			{
				component.WaitVSRebirthStart();
			}
		}
	}

	public int GetVSRebirthRemainingTime()
	{
		return (int)(11f - vsRebirthTimer.GetTimeSpan());
	}

	public virtual bool StartWaitRebirth()
	{
		return true;
	}

	public Timer GetRebirthTimer()
	{
		return rebirthTimer;
	}

	public void Attack()
	{
		JustMakeAShoot = true;
		weapon.Attack(Time.deltaTime);
	}

	public void Move(Vector3 motion)
	{
		cc.Move(motion * Speed * Time.deltaTime);
	}

	public virtual void ChangeWeaponInBag(int bagIndex)
	{
	}

	protected void ChangeWeapon(Weapon w)
	{
		if (weapon != null)
		{
			weapon.GunOff();
		}
		weapon = w;
		weapon.GunOn();
	}

	public void GetKnocked()
	{
		string text = AnimationString.Attacked;
		if (knockedSpeed < 0f)
		{
			text = AnimationString.AttackedBack;
		}
		PlayAnimation(text, WrapMode.ClampForever);
		cc.Move(-playerTransform.forward * knockedSpeed);
		if (AnimationPlayed(text, 1f))
		{
			SetState(IDLE_STATE);
			if (!(GetSkills().GetSkill(SkillsType.FLY) > 0f) && 0 == 0)
			{
				PlayAnimationAllLayers(AnimationString.Idle + GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
			}
		}
	}

	public void DoGravityForce(float deltaTime)
	{
		PlayAnimationAllLayers(AnimationString.Idle + GetWeaponAnimationSuffixAlter(), WrapMode.Loop);
		Vector3 vector = mGravityForceTarget - playerTransform.position;
		if (vector.sqrMagnitude > 4f)
		{
			cc.Move(vector.normalized * 20f * deltaTime);
			if (mGravityForceBeamObj != null)
			{
				mGravityForceBeamObj.transform.LookAt(mGravityForceTarget + Vector3.up);
				mGravityForceBeamObj.transform.localScale = new Vector3(1f / mGravityForceBeamObj.transform.parent.lossyScale.x, 1f / mGravityForceBeamObj.transform.parent.lossyScale.y, (mGravityForceTarget - playerTransform.position).magnitude / mGravityForceBeamObj.transform.parent.lossyScale.z);
			}
		}
		else if (mGravityForceBeamObj != null)
		{
			mGravityForceBeamObj.SetActive(false);
		}
		if (Time.time - mStartGravityForceTime > 2f)
		{
			SetState(IDLE_STATE);
			StopGravityForceEffect();
		}
	}

	public void UnderAttackSetHP(int hp)
	{
		if (InPlayingState())
		{
			if (IsLocal())
			{
				Transform transform = Camera.main.transform.Find("Screen_Blood");
				if (transform != null)
				{
					transform.localScale = new Vector3(2.4f * (float)Screen.width / (float)Screen.height, 2.4f, 1f);
					ScreenBloodScript component = transform.GetComponent<ScreenBloodScript>();
					component.NewBlood(Hp - hp);
				}
			}
			else if (Hp == hp)
			{
				ShowDefent();
				return;
			}
			Hp = hp;
			if (hp == 0)
			{
				SetState(DEAD_STATE);
				OnDead();
			}
			else
			{
				AudioManager.GetInstance().PlaySound("Audio/playerGotHit");
			}
		}
		NeverGotHit = false;
	}

	public void UnderAttack(int damage)
	{
		if (InPlayingState())
		{
			Hp -= damage;
			Hp = Mathf.Clamp(Hp, 0, MaxHp);
			if (Hp <= 0)
			{
				OnDead();
				SetState(DEAD_STATE);
			}
			else
			{
				AudioManager.GetInstance().PlaySound("Audio/playerGotHit");
			}
			Transform transform = Camera.main.transform.Find("Screen_Blood");
			if (transform != null)
			{
				transform.localScale = new Vector3(2.4f * (float)Screen.width / (float)Screen.height, 2.4f, 1f);
				ScreenBloodScript component = transform.GetComponent<ScreenBloodScript>();
				component.NewBlood(damage);
			}
		}
		NeverGotHit = false;
	}

	public void GetHit(int damage)
	{
		Hp -= damage;
		Hp = Mathf.Clamp(Hp, 0, MaxHp);
	}

	public void Block()
	{
		if (!(defentObj != null))
		{
			return;
		}
		float num = Random.Range(0f, 1f);
		if (num < playerSkill.GetSkill(SkillsType.BLOCK_AT_A_RATE))
		{
			ShowDefent();
			int num2 = Random.Range(0, 100);
			if (num2 < 50)
			{
				AudioManager.GetInstance().PlaySoundSingleAt("Audio/force_shield01", playerTransform.position);
			}
			else
			{
				AudioManager.GetInstance().PlaySoundSingleAt("Audio/force_shield02", playerTransform.position);
			}
		}
	}

	public bool DoWin()
	{
		if (!winSpecialFinish)
		{
			PlayAnimation(AnimationString.WinSpecial, WrapMode.ClampForever);
			if (AnimationPlayed(AnimationString.WinSpecial, 1f))
			{
				winSpecialFinish = true;
			}
		}
		else if (GetWeapon().GetWeaponType() == WeaponType.MachineGun || GetWeapon().GetWeaponType() == WeaponType.AdvancedMachineGun)
		{
			PlayAnimation(AnimationString.WinIdleMachineGun, WrapMode.Loop);
		}
		else if (isSpecialWinIdle)
		{
			PlayAnimation(AnimationString.WinIdleSpecial, WrapMode.Loop);
		}
		else
		{
			PlayAnimationWithoutBlend(AnimationString.WinIdle, WrapMode.Loop);
		}
		return winAnimationTimer.Ready();
	}

	public bool DoVSLose()
	{
		return winAnimationTimer.Ready();
	}

	public virtual bool CheckLose()
	{
		return false;
	}

	public virtual bool DeadAnimationCompleted()
	{
		return false;
	}

	public virtual bool WinAnimationCompleted()
	{
		return false;
	}

	public void UploadStatistaic()
	{
		GameMode gameMode = GameApp.GetInstance().GetGameMode();
		if (gameMode.IsMultiPlayer())
		{
			if (gameMode.IsCoopMode())
			{
				PlayerUploadStatisticsRequest request = new PlayerUploadStatisticsRequest(GetMonsterCash(), GetPickupCash(), GetPickupEnegy(), GetBounsCash(), GetBossCash(), GetBossMithril());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			else if (gameMode.IsVSMode())
			{
				VSUploadStatisticsRequest request2 = new VSUploadStatisticsRequest((short)VSStatistics.Kills, (short)VSStatistics.Death, (short)VSStatistics.Assist, VSStatistics.Score, VSStatistics.Bonus, VSStatistics.CashReward, (short)VSStatistics.SecureFlags, (short)VSStatistics.AssistFlags, (short)VSStatistics.VIPAssist, (short)VSStatistics.CMIGiftHit);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	public void OnHit(int attackDamage)
	{
		if (defentObj != null)
		{
			float num = Random.Range(0f, 1f);
			if (num < playerSkill.GetSkill(SkillsType.BLOCK_AT_A_RATE))
			{
				ShowDefent();
				int num2 = Random.Range(0, 100);
				if (num2 < 50)
				{
					AudioManager.GetInstance().PlaySoundSingleAt("Audio/force_shield01", playerTransform.position);
				}
				else
				{
					AudioManager.GetInstance().PlaySoundSingleAt("Audio/force_shield02", playerTransform.position);
				}
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					PlayerOnHitRequest request = new PlayerOnHitRequest(0, false, 0);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				return;
			}
		}
		float num3 = (1f + playerSkill.GetSkill(SkillsType.DAMAGE_REDUCE)) * (1f + GetDamageReduceItemAssist()) * (1f + playerSkill.GetSkill(SkillsType.TEAM_DAMAGE_REDUCE));
		if (IsPowerUp(2))
		{
			num3 *= 0.14999998f;
		}
		if (IsPowerUp(3))
		{
			num3 *= 0.7f;
		}
		if (IsPowerUp(8))
		{
			num3 *= -0.6f;
		}
		attackDamage = (int)((float)attackDamage * num3);
		if (!InPlayingState())
		{
			return;
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerOnHitRequest request2 = new PlayerOnHitRequest((short)attackDamage, false, 0);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			return;
		}
		float skill = GetSkills().GetSkill(SkillsType.SPEEDUP_WHEN_GOT_HIT);
		if (skill > 0f)
		{
			AddTempBuff(EffectsType.SPEED_BOOTH_WHEN_GOT_HIT, 2, skill);
		}
		UnderAttack(attackDamage);
	}

	public void OnKnocked(float speed)
	{
		if (InPlayingState())
		{
			knockedSpeed = speed;
			GetWeapon().StopFire();
			GetWeapon().AutoDestructEffect();
			SetState(KNOCKED_STATE);
		}
	}

	public void OnGravityForce(Vector3 targetPos)
	{
		if (InPlayingState())
		{
			mGravityForceTarget = targetPos;
			mStartGravityForceTime = Time.time;
			GetWeapon().StopFire();
			GetWeapon().AutoDestructEffect();
			SetState(GRAVITY_FORCE_STATE);
			StartGravityForceEffect();
		}
	}

	public virtual void OnDead()
	{
		GetWeapon().StopFire();
		if (IsLocal())
		{
			UploadStatistaic();
		}
		else
		{
			RemotePlayer remotePlayer = this as RemotePlayer;
			if (remotePlayer != null)
			{
				remotePlayer.GetInterpolation().Clear();
			}
		}
		isAiming = false;
		PlayAnimation(AnimationString.Dead, WrapMode.ClampForever);
		AudioManager.GetInstance().PlaySound("Audio/player_killed_1");
		if (!IsLocal())
		{
			GameApp.GetInstance().GetGameWorld().CreateTeamSkills();
		}
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			CreateDeadBlood();
		}
		if (IsLocal() && GameApp.GetInstance().GetGameMode().IsCatchTheFlagMode() && GameApp.GetInstance().GetGameWorld().FlagInPlayerID == userID)
		{
			DropTheFlagRequest request = new DropTheFlagRequest(GetUserID(), GetTransform().position + Vector3.up * 0f, false);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		StopGravityForceEffect();
	}

	public void OnWin()
	{
		if (IsLocal())
		{
			UploadStatistaic();
		}
		if (GetWeapon().GetWeaponType() == WeaponType.MachineGun || GetWeapon().GetWeaponType() == WeaponType.AdvancedMachineGun)
		{
			winSpecialFinish = true;
			isSpecialWinIdle = false;
		}
		else
		{
			int num = Random.Range(0, 100);
			if (num < 50)
			{
				winSpecialFinish = false;
				isSpecialWinIdle = false;
			}
			else
			{
				winSpecialFinish = true;
				isSpecialWinIdle = true;
			}
		}
		GetWeapon().StopFire();
		winAnimationTimer.SetTimer(4f, false);
	}

	public void OnLose()
	{
		if (IsLocal())
		{
			UploadStatistaic();
		}
		GetWeapon().StopFire();
		winAnimationTimer.SetTimer(4f, false);
	}

	public void OnPickUp(LootType lootType, int amount)
	{
		switch (lootType)
		{
		case LootType.Enegy:
		{
			userState.Enegy += amount;
			pickupEnegy += amount;
			AudioManager.GetInstance().PlaySound("Audio/pickup/pickup_energy");
			GameObject original2 = Resources.Load("Effect/update_effect/effect_pick_energy_001") as GameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(original2, playerTransform.position + Vector3.up, Quaternion.identity) as GameObject;
			gameObject2.transform.parent = playerTransform;
			break;
		}
		case LootType.Money:
		{
			AddPickupCash(amount);
			int num = Random.Range(0, 100);
			if (num < 50)
			{
				AudioManager.GetInstance().PlaySound("Audio/pickup/pickup_money01");
			}
			else
			{
				AudioManager.GetInstance().PlaySound("Audio/pickup/pickup_money02");
			}
			GameObject original = Resources.Load("Effect/update_effect/effect_pick_gold_001") as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate(original, playerTransform.position + Vector3.up, Quaternion.identity) as GameObject;
			gameObject.transform.parent = playerTransform;
			break;
		}
		}
	}

	public void OnPickUp(short sequenceID)
	{
		GameObject gameObject = GameObject.Find("Loot_" + sequenceID);
		if (gameObject != null)
		{
			ItemScript component = gameObject.GetComponent<ItemScript>();
			OnPickUp(component.itemType, component.Amount);
		}
	}

	public FloorInfo GetGround()
	{
		FloorInfo floorInfo = new FloorInfo();
		floorInfo.height = Global.FLOORHEIGHT;
		Vector3 up = Vector3.up;
		Ray ray = new Ray(playerTransform.position + new Vector3(0f, 0.5f, 0f), Vector3.down);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 10f, 1 << PhysicsLayer.FLOOR))
		{
			floorInfo.height = hitInfo.point.y;
			floorInfo.normal = hitInfo.normal;
		}
		if (floorInfo.normal.y < 0f)
		{
			floorInfo.normal = -floorInfo.normal;
		}
		return floorInfo;
	}

	public void UpdateNearestPoint()
	{
		float num = 999999f;
		foreach (WayPoint wayPoint in GameApp.GetInstance().GetGameWorld().GetWayPointList())
		{
			Vector3 vector = playerTransform.position - wayPoint.Position;
			float magnitude = vector.magnitude;
			if (vector.y < 3f && magnitude < num)
			{
				Ray ray = new Ray(playerTransform.position + new Vector3(0f, 0.5f, 0f), wayPoint.Position - playerTransform.position);
				RaycastHit hitInfo;
				if (!Physics.Raycast(ray, out hitInfo, magnitude, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PATH_FINDING_WALL)))
				{
					NearestPoint = wayPoint;
					num = magnitude;
				}
			}
		}
	}

	public void SlowDownEffect()
	{
		mSlowDownTimer.Do();
		if (!mIsSlowDown)
		{
			mIsSlowDown = true;
			SlowEffect = 0.7f;
			if (mSlowDownEffectObj != null)
			{
				mSlowDownEffectObj.SetActiveRecursively(true);
			}
		}
	}

	public void SetAimEffect()
	{
		mAimTimer.Do();
		if (!mAim)
		{
			mAim = true;
			isAiming = true;
			SlowEffect = 0.7f;
			if (mTrackEffectObj != null)
			{
				mTrackEffectObj.SetActive(true);
			}
		}
	}

	public void StartGravityForceEffect()
	{
		if (mGravityForceBallObj == null)
		{
			Transform transform = playerTransform.Find("GravityForceBall");
			if (transform != null)
			{
				mGravityForceBallObj = transform.gameObject;
			}
			else
			{
				GameObject original = Resources.Load("SW2_Effect/C12_Low_qiu") as GameObject;
				mGravityForceBallObj = UnityEngine.Object.Instantiate(original) as GameObject;
				mGravityForceBallObj.name = "GravityForceBall";
				mGravityForceBallObj.transform.parent = playerTransform;
				mGravityForceBallObj.transform.localPosition = Vector3.up;
				mGravityForceBallObj.transform.localRotation = Quaternion.identity;
			}
		}
		mGravityForceBallObj.transform.localScale = 2f * Vector3.one / mGravityForceBallObj.transform.parent.lossyScale.x;
		mGravityForceBallObj.SetActive(true);
		if (mGravityForceBeamObj == null)
		{
			Transform transform2 = playerTransform.Find("GravityForceBeam");
			if (transform2 != null)
			{
				mGravityForceBeamObj = transform2.gameObject;
			}
			else
			{
				GameObject original2 = Resources.Load("SW2_Effect/C12_Low_gpizi") as GameObject;
				mGravityForceBeamObj = UnityEngine.Object.Instantiate(original2) as GameObject;
				mGravityForceBeamObj.name = "GravityForceBeam";
				mGravityForceBeamObj.transform.parent = playerTransform;
				mGravityForceBeamObj.transform.localPosition = Vector3.up;
			}
		}
		mGravityForceBeamObj.transform.LookAt(mGravityForceTarget + Vector3.up);
		mGravityForceBeamObj.transform.localScale = new Vector3(1f / mGravityForceBeamObj.transform.parent.lossyScale.x, 1f / mGravityForceBeamObj.transform.parent.lossyScale.y, (mGravityForceTarget - playerTransform.position).magnitude / mGravityForceBeamObj.transform.parent.lossyScale.z);
		mGravityForceBeamObj.SetActive(true);
		AudioManager.GetInstance().PlaySoundAt("Audio/specialweapon/lightbow_hum", mGravityForceBeamObj.transform.position);
	}

	public void StopGravityForceEffect()
	{
		if (mGravityForceBallObj == null)
		{
			Transform transform = playerTransform.Find("GravityForceBall");
			if (transform != null)
			{
				mGravityForceBallObj = transform.gameObject;
			}
		}
		if (mGravityForceBallObj != null)
		{
			mGravityForceBallObj.SetActive(false);
		}
		if (mGravityForceBeamObj == null)
		{
			Transform transform2 = playerTransform.Find("GravityForceBeam");
			if (transform2 != null)
			{
				mGravityForceBeamObj = transform2.gameObject;
			}
		}
		if (mGravityForceBeamObj != null)
		{
			mGravityForceBeamObj.SetActive(false);
		}
	}
}
