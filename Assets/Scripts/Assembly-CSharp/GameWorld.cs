using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld
{
	private enum DirObjTeamName
	{
		Blue = 0,
		Red = 1,
		White = 2
	}

	protected LocalPlayer player;

	protected List<RemotePlayer> remotePlayerList = new List<RemotePlayer>();

	protected Hashtable enemyList = new Hashtable();

	protected HashSet<int> lostEnemyList = new HashSet<int>();

	protected int enemyRespawns;

	protected int enemyID;

	protected Timer switchBossLevelTimer = new Timer();

	protected EnemyType bossType;

	protected short bossID;

	protected float bossLeveStartFadeTime = -1f;

	protected ThirdPersonStandardCameraScript tpcs;

	protected ObjectPool[] enemyObjectPool = new ObjectPool[8];

	protected ObjectPool[] eliteEnemyObjectPool = new ObjectPool[8];

	protected ObjectPool[] enemyBodyPool = new ObjectPool[8];

	protected ObjectPool hitBloodPool = new ObjectPool();

	protected Enemy[] bossArray;

	protected List<WayPoint> wayPointList = new List<WayPoint>();

	protected VSClock vsClock;

	protected VSClock flagClock;

	protected List<CMIGift> cmiGifts = new List<CMIGift>();

	protected CMIGift cmiSpecialGift = new CMIGift();

	protected GameObject[] cmiGiftSpawnPoint;

	protected VSClock vipClock;

	private DirObjTeamName vipTeamDirObj = DirObjTeamName.White;

	public int BaseDifficultyLevel { get; set; }

	public int DifficultyLevel { get; set; }

	public TeamSkill TeamSkills { get; set; }

	public TeamSkill OtherTeamSkill { get; set; }

	public int TotalEnemyCount { get; set; }

	public int TotalWaves { get; set; }

	public GameState State { get; set; }

	public bool InBossBattle { get; set; }

	public MithrilDropInfo MithrilDrops { get; set; }

	public VSBattleInformation BattleInfo { get; set; }

	public PVPRewardInfo PVPReward { get; set; }

	public VSClock FlagClock
	{
		get
		{
			return flagClock;
		}
	}

	public VSClock VIPClock
	{
		get
		{
			return vipClock;
		}
	}

	public bool Exit { get; set; }

	public int FlagInPlayerID { get; set; }

	public int VIPInPlayerID { get; set; }

	public int LastFlagInPlayerID { get; set; }

	public GameObject FlagDirObj { get; set; }

	public GameObject VIPDirObj { get; set; }

	public GameObject CMIDirObj { get; set; }

	public GameObject VIPEffect { get; set; }

	public int EnemyID
	{
		get
		{
			return enemyID;
		}
		set
		{
			enemyID = value;
		}
	}

	public Enemy GetBoss(int index)
	{
		if (bossArray != null && index < bossArray.Length)
		{
			return bossArray[index];
		}
		return null;
	}

	public List<WayPoint> GetWayPointList()
	{
		return wayPointList;
	}

	public List<CMIGift> GetCMIGifts()
	{
		List<CMIGift> list = new List<CMIGift>();
		foreach (CMIGift cmiGift in cmiGifts)
		{
			list.Add(cmiGift);
		}
		list.Add(cmiSpecialGift);
		return list;
	}

	public void SetCMISpecialGift(CMIGiftType type, short id, bool isActive, byte pointId)
	{
		cmiSpecialGift.SetType(type);
		cmiSpecialGift.SetId(id);
		cmiSpecialGift.SetActive(isActive);
		cmiSpecialGift.SetPointId(pointId);
		cmiSpecialGift.SetPosition(cmiGiftSpawnPoint[pointId].transform.position);
		cmiSpecialGift.SetHp(CMIConfig.HP_RANDOM);
	}

	public void SetCMIGifts(List<CMIGift> gifts)
	{
		DestroyGifts();
		foreach (CMIGift gift in gifts)
		{
			CreateCMIGift(gift);
		}
	}

	public void CreateCMIGift(CMIGift gift)
	{
		GameObject gameObject = null;
		if (gift.GetType() == CMIGiftType.TYPE_LARGE)
		{
			gift.SetHp(CMIConfig.HP_LARGE);
			gameObject = Resources.Load("Effect/chris_box03") as GameObject;
		}
		else if (gift.GetType() == CMIGiftType.TYPE_MIDDLE)
		{
			gift.SetHp(CMIConfig.HP_MIDDLE);
			gameObject = Resources.Load("Effect/chris_box02") as GameObject;
		}
		else
		{
			gift.SetHp(CMIConfig.HP_SMALL);
			gameObject = Resources.Load("Effect/chris_box01") as GameObject;
		}
		gift.CreateObj(gameObject, cmiGiftSpawnPoint[gift.GetPointId()].transform.position, "G_" + gift.GetId());
		gift.SetActive(true);
		cmiGifts.Add(gift);
	}

	public void EndVSCMI()
	{
		DestroyGifts();
		cmiSpecialGift.SetActive(false);
	}

	public void DestroyGifts()
	{
		foreach (CMIGift cmiGift in cmiGifts)
		{
			cmiGift.DestroyObj();
		}
		cmiGifts.Clear();
	}

	public void DestroyGift(CMIGift g)
	{
		if (g.IsSpecialGift())
		{
			g.SetActive(false);
			return;
		}
		g.DestroyObj();
		if (cmiGifts.Contains(g))
		{
			cmiGifts.Remove(g);
		}
	}

	public void GetPositionCMI(byte pointId)
	{
	}

	public CMIGift GetGiftWithId(int id)
	{
		foreach (CMIGift cmiGift in cmiGifts)
		{
			if (cmiGift.GetId() == id)
			{
				return cmiGift;
			}
		}
		if (cmiSpecialGift.GetId() == id)
		{
			return cmiSpecialGift;
		}
		return null;
	}

	public void DropTheFlag(Vector3 pos)
	{
		GameObject gameObject = GameObject.Find("CathTheFlag");
		if (gameObject == null)
		{
			int loadedLevel = Application.loadedLevel;
			GameObject original = ((loadedLevel != 13) ? (Resources.Load("Loot/Flag") as GameObject) : (Resources.Load("Loot/Flag_2") as GameObject));
			gameObject = Object.Instantiate(original) as GameObject;
			gameObject.name = "CathTheFlag";
		}
		gameObject.transform.position = pos;
	}

	public void HideTheFlag()
	{
		GameObject gameObject = GameObject.Find("CathTheFlag");
		if (gameObject != null)
		{
			Object.Destroy(gameObject);
		}
	}

	public void CatchTheFlag()
	{
		GameObject gameObject = GameObject.Find("CathTheFlag");
		if (gameObject != null)
		{
			Object.Destroy(gameObject);
		}
	}

	public void Init()
	{
		Exit = false;
		PreCalculateWayPoints();
		player = new LocalPlayer();
		player.Init();
		if (GameApp.GetInstance().GetGameMode().IsCatchTheFlagMode() && Lobby.GetInstance().IsMasterPlayer)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.FLAG_RESPAWN);
			int num = Random.Range(0, array.Length);
			DropTheFlagRequest request = new DropTheFlagRequest(-1, array[num].transform.position + Vector3.up * 0f, true);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		tpcs = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
		tpcs.Init();
		State = GameState.Playing;
		vsClock = Lobby.GetInstance().GetVSClock();
		vsClock.Restart();
		if (GameApp.GetInstance().GetGameMode().IsCatchTheFlagMode())
		{
			flagClock = new VSClock();
			flagClock.SetTotalGameSeconds(40);
			flagClock.Restart();
			GameObject original = Resources.Load("UI/flagdir") as GameObject;
			FlagDirObj = Object.Instantiate(original) as GameObject;
			FlagDirObj.transform.localPosition = player.GetTransform().position;
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_VIP)
		{
			vipClock = new VSClock();
			vipClock.SetTotalGameSeconds(50);
			vipClock.Restart();
			GameObject original2 = Resources.Load("UI/flagdir") as GameObject;
			VIPDirObj = Object.Instantiate(original2) as GameObject;
			VIPDirObj.transform.localPosition = player.GetTransform().position;
			VIPDirObj.SetActive(false);
			GameObject original3 = Resources.Load("Effect/VSVIPPlayer") as GameObject;
			VIPEffect = Object.Instantiate(original3) as GameObject;
			VIPEffect.transform.localPosition = player.GetTransform().position;
			VIPEffect.SetActive(false);
			vipTeamDirObj = DirObjTeamName.White;
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
		{
			GameObject original4 = Resources.Load("UI/flagdir") as GameObject;
			CMIDirObj = Object.Instantiate(original4) as GameObject;
			CMIDirObj.transform.localPosition = player.GetTransform().position;
			CMIDirObj.SetActive(false);
			cmiGiftSpawnPoint = GameObject.FindGameObjectsWithTag(TagName.GIFT_SPAWN);
			GameObject prefab = Resources.Load("Effect/chris_box04") as GameObject;
			cmiSpecialGift.CreateObj(prefab, "G_0");
			cmiSpecialGift.SetActive(false);
			NumberManager.GetInstance().Clear();
			NumberManager.GetInstance().Init();
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			GameApp.GetInstance().GetUserState().Achievement.OnceCoop();
		}
		GameObject gameObject = null;
		if (!GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			for (int i = 0; i < 8; i++)
			{
				enemyObjectPool[i] = new ObjectPool();
				gameObject = Resources.Load("Enemy/bug0" + (i + 1)) as GameObject;
				enemyObjectPool[i].Init("enemy/bug" + (i + 1), gameObject, 8, 0.8f);
				enemyBodyPool[i] = new ObjectPool();
				gameObject = Resources.Load("DeadBody/Deadbody " + i) as GameObject;
				enemyBodyPool[i].Init("DeadBody/Deadbody" + (i + 1), gameObject, 8, 3f);
			}
			for (int j = 0; j < 6; j++)
			{
				eliteEnemyObjectPool[j] = new ObjectPool();
				gameObject = Resources.Load("Enemy/bug0" + (j + 1) + "_elite") as GameObject;
				eliteEnemyObjectPool[j].Init("enemy/bug" + (j + 1) + "_elite", gameObject, 2, 0.8f);
			}
		}
		GameObject prefab2 = Resources.Load("Effect/hitBlood") as GameObject;
		hitBloodPool.Init("effect/hitBlood", prefab2, 12, 0.8f);
		InBossBattle = false;
		BattleInfo = new VSBattleInformation();
		PVPReward = new PVPRewardInfo();
		FlagInPlayerID = -1;
		LastFlagInPlayerID = -1;
	}

	private void PreCalculateWayPoints()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.WAYPOINT);
		GameObject[] array2 = array;
		foreach (GameObject obj in array2)
		{
			WayPoint wayPoint = new WayPoint(obj);
			WayPointScript component = wayPoint.WayPointObject.GetComponent<WayPointScript>();
			if (null != component)
			{
				component.owner = wayPoint;
			}
			wayPointList.Add(wayPoint);
		}
		foreach (WayPoint wayPoint2 in wayPointList)
		{
			WayPointScript component2 = wayPoint2.WayPointObject.GetComponent<WayPointScript>();
			if (!(null != component2))
			{
				continue;
			}
			WayPointScript[] nodes = component2.nodes;
			foreach (WayPointScript wayPointScript in nodes)
			{
				if (null != wayPointScript && wayPointScript.owner != null)
				{
					wayPoint2.AddAdjacentWayPoint(wayPointScript.owner);
					wayPointScript.owner.AddAdjacentWayPoint(wayPoint2);
				}
			}
		}
		foreach (WayPoint wayPoint3 in wayPointList)
		{
			foreach (WayPoint wayPoint4 in wayPointList)
			{
				wayPoint3.AddWayPointDistance(wayPoint4);
			}
		}
	}

	public ObjectPool GetDeadBodyPool(EnemyType eType)
	{
		return enemyBodyPool[(int)eType];
	}

	public ObjectPool GetHitBloodPool()
	{
		return hitBloodPool;
	}

	public void AddLostEnemy(int enemyID)
	{
		lostEnemyList.Add(enemyID);
	}

	public int GetLostEnemyCount()
	{
		return lostEnemyList.Count;
	}

	public void AddRespawnedEnemy()
	{
		enemyRespawns++;
	}

	public int GetRespawnedEnemy()
	{
		return enemyRespawns;
	}

	public ThirdPersonStandardCameraScript GetCamera()
	{
		return tpcs;
	}

	public int GetEnemyTypeID(EnemyType eType)
	{
		int result = 0;
		switch (eType)
		{
		case EnemyType.Drone:
			result = 0;
			break;
		case EnemyType.Zergling:
			result = 1;
			break;
		case EnemyType.Scorpion:
			result = 2;
			break;
		case EnemyType.Mutalisk:
			result = 3;
			break;
		case EnemyType.Reaver:
			result = 4;
			break;
		case EnemyType.Worm:
			result = 5;
			break;
		case EnemyType.Boomer:
			result = 6;
			break;
		case EnemyType.Tank:
			result = 7;
			break;
		}
		return result;
	}

	public LocalPlayer GetPlayer()
	{
		return player;
	}

	public int GetPlayerCount()
	{
		return remotePlayerList.Count + 1;
	}

	public int GetInGamePlayerCount()
	{
		int num = 1;
		foreach (RemotePlayer remotePlayer in remotePlayerList)
		{
			if (remotePlayer != null && remotePlayer.State != Player.LOSE_STATE)
			{
				num++;
			}
		}
		return num;
	}

	public int GetPlayingPlayerCount()
	{
		int num = 0;
		if (player.InPlayingState())
		{
			num++;
		}
		foreach (RemotePlayer remotePlayer in remotePlayerList)
		{
			if (remotePlayer.InPlayingState())
			{
				num++;
			}
		}
		return num;
	}

	public Enemy SpawnEnemy(EnemyType eType, short enemyID, Vector3 spawnPosition, bool bElite)
	{
		Enemy enemy = null;
		GameObject gameObject = null;
		GameObject gameObject2 = null;
		string empty = string.Empty;
		GameObject gameObject3 = null;
		if (bElite)
		{
			empty = "_elite";
		}
		switch (eType)
		{
		case EnemyType.Drone:
			enemy = new Drone();
			break;
		case EnemyType.Zergling:
			enemy = new Zergling();
			break;
		case EnemyType.Scorpion:
			enemy = new Scorpion();
			break;
		case EnemyType.Mutalisk:
			enemy = new Mutalisk();
			break;
		case EnemyType.Reaver:
			enemy = new Reaver();
			break;
		case EnemyType.Worm:
			enemy = new Worm();
			break;
		case EnemyType.Boomer:
			enemy = new Boomer();
			break;
		case EnemyType.Tank:
			enemy = new Tank();
			break;
		case EnemyType.Dragon:
			if (InBossBattle)
			{
				break;
			}
			State = GameState.SwitchBossLevel;
			player.PlayTeleportAnimation();
			foreach (RemotePlayer remotePlayer in remotePlayerList)
			{
				if (remotePlayer != null && remotePlayer.InPlayingState())
				{
					remotePlayer.PlayTeleportAnimation();
				}
			}
			bossType = eType;
			bossID = enemyID;
			break;
		case EnemyType.Beetle:
			if (InBossBattle)
			{
				break;
			}
			State = GameState.SwitchBossLevel;
			player.PlayTeleportAnimation();
			foreach (RemotePlayer remotePlayer2 in remotePlayerList)
			{
				if (remotePlayer2 != null && remotePlayer2.InPlayingState())
				{
					remotePlayer2.PlayTeleportAnimation();
				}
			}
			bossType = eType;
			bossID = enemyID;
			break;
		case EnemyType.Mantis:
			if (InBossBattle)
			{
				break;
			}
			State = GameState.SwitchBossLevel;
			player.PlayTeleportAnimation();
			foreach (RemotePlayer remotePlayer3 in remotePlayerList)
			{
				if (remotePlayer3 != null && remotePlayer3.InPlayingState())
				{
					remotePlayer3.PlayTeleportAnimation();
				}
			}
			bossType = eType;
			bossID = enemyID;
			break;
		case EnemyType.MainMantis:
			if (InBossBattle)
			{
				break;
			}
			State = GameState.SwitchBossLevel;
			player.PlayTeleportAnimation();
			foreach (RemotePlayer remotePlayer4 in remotePlayerList)
			{
				if (remotePlayer4 != null && remotePlayer4.InPlayingState())
				{
					remotePlayer4.PlayTeleportAnimation();
				}
			}
			bossType = eType;
			bossID = enemyID;
			break;
		case EnemyType.Earthworm:
			if (InBossBattle)
			{
				break;
			}
			State = GameState.SwitchBossLevel;
			player.PlayTeleportAnimation();
			foreach (RemotePlayer remotePlayer5 in remotePlayerList)
			{
				if (remotePlayer5 != null && remotePlayer5.InPlayingState())
				{
					remotePlayer5.PlayTeleportAnimation();
				}
			}
			bossType = eType;
			bossID = enemyID;
			break;
		case EnemyType.SantaMachine:
			if (InBossBattle)
			{
				break;
			}
			State = GameState.SwitchBossLevel;
			player.PlayTeleportAnimation();
			foreach (RemotePlayer remotePlayer6 in remotePlayerList)
			{
				if (remotePlayer6 != null && remotePlayer6.InPlayingState())
				{
					remotePlayer6.PlayTeleportAnimation();
				}
			}
			bossType = eType;
			bossID = enemyID;
			break;
		}
		if (enemy != null && State != GameState.SwitchBossLevel)
		{
			enemy.EnemyType = eType;
			if (GameObject.Find("E_" + enemyID) == null)
			{
				gameObject3 = (bElite ? eliteEnemyObjectPool[GetEnemyTypeID(eType)].CreateObject(Vector3.zero, Vector3.zero, Quaternion.identity) : enemyObjectPool[GetEnemyTypeID(eType)].CreateObject(Vector3.zero, Vector3.zero, Quaternion.identity));
				gameObject3.layer = PhysicsLayer.ENEMY;
				enemy.IsElite = bElite;
				enemy.Init(gameObject3);
				enemy.EnemyID = enemyID;
				enemy.GetTransform().localScale = enemy.GetLocalScale();
				if (bElite)
				{
					enemy.GetTransform().localScale *= 1.3f;
				}
				gameObject3.transform.position = spawnPosition;
				gameObject3.name = "E_" + enemyID;
				enemy.EnemyName = gameObject3.name;
				enemyList.Add(gameObject3.name, enemy);
			}
		}
		return enemy;
	}

	public void AddRemotePlayer(RemotePlayer player)
	{
		remotePlayerList.Add(player);
	}

	public RemotePlayer GetRemotePlayerByUserID(int userID)
	{
		foreach (RemotePlayer remotePlayer in remotePlayerList)
		{
			if (remotePlayer.GetUserID() == userID)
			{
				return remotePlayer;
			}
		}
		return null;
	}

	public void RemoveRemotePlayer(int userID)
	{
		foreach (RemotePlayer remotePlayer in remotePlayerList)
		{
			if (remotePlayer.GetUserID() == userID)
			{
				if (GameApp.GetInstance().GetGameMode().IsCoopMode())
				{
					remotePlayer.State = Player.LOSE_STATE;
					remotePlayer.GetGameObject().SetActiveRecursively(false);
				}
				else
				{
					Object.Destroy(remotePlayer.GetGameObject());
					remotePlayer.GetTransform().position = new Vector3(0f, -10000f, 0f);
					remotePlayerList.Remove(remotePlayer);
				}
				GameApp.GetInstance().GetGameWorld().CreateTeamSkills();
				break;
			}
		}
	}

	public List<Player> GetPlayers()
	{
		List<Player> list = new List<Player>();
		list.Add(player);
		foreach (RemotePlayer remotePlayer in remotePlayerList)
		{
			list.Add(remotePlayer);
		}
		return list;
	}

	public List<RemotePlayer> GetRemotePlayers()
	{
		return remotePlayerList;
	}

	public Hashtable GetEnemies()
	{
		return enemyList;
	}

	public Enemy GetEnemyByID(string enemyID)
	{
		return (Enemy)enemyList[enemyID];
	}

	public void Loop(float deltaTime)
	{
		if (vsClock != null)
		{
			vsClock.Update();
		}
		if (flagClock != null)
		{
			flagClock.Update();
		}
		if (vipClock != null)
		{
			vipClock.Update();
		}
		if (State == GameState.SwitchBossLevel)
		{
			if (player.TeleportAnimationEnds())
			{
				CreateBossLevel(bossType);
			}
		}
		else
		{
			if (player != null)
			{
				player.Loop(deltaTime);
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				foreach (RemotePlayer remotePlayer in remotePlayerList)
				{
					remotePlayer.Loop(deltaTime);
				}
			}
			object[] array = new object[enemyList.Count];
			enemyList.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				Enemy enemy = enemyList[array[i]] as Enemy;
				enemy.DoLogic(deltaTime);
			}
		}
		if (bossLeveStartFadeTime > 0f)
		{
			if (Time.time - bossLeveStartFadeTime > 1f)
			{
				player.ActivatePlayer(true);
			}
			if (Time.time - bossLeveStartFadeTime > 2f)
			{
				player.EnableTeleportEffect(false);
				foreach (RemotePlayer remotePlayer2 in remotePlayerList)
				{
					if (remotePlayer2 != null)
					{
						remotePlayer2.EnableTeleportEffect(false);
					}
				}
				bossLeveStartFadeTime = -1f;
			}
		}
		for (int j = 0; j < 8; j++)
		{
			if (enemyBodyPool[j] != null)
			{
				enemyBodyPool[j].AutoDestruct();
			}
		}
		hitBloodPool.AutoDestruct();
		if (GameApp.GetInstance().GetGameMode().IsCatchTheFlagMode())
		{
			if (player == null)
			{
				return;
			}
			FlagDirObj.transform.localPosition = player.GetTransform().position + Vector3.up * 2.6f;
			GameObject gameObject = GameObject.Find("CathTheFlag");
			if (gameObject != null)
			{
				FlagDirObj.SetActiveRecursively(true);
				FlagDirObj.transform.LookAt(gameObject.transform.position + Vector3.up);
				return;
			}
			if (FlagInPlayerID == player.GetUserID())
			{
				FlagDirObj.SetActiveRecursively(false);
				return;
			}
			RemotePlayer remotePlayerByUserID = GetRemotePlayerByUserID(FlagInPlayerID);
			if (remotePlayerByUserID != null)
			{
				FlagDirObj.SetActiveRecursively(true);
				FlagDirObj.transform.LookAt(remotePlayerByUserID.GetTransform());
			}
			else
			{
				FlagDirObj.SetActiveRecursively(false);
			}
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_VIP)
		{
			if (player == null)
			{
				return;
			}
			VIPDirObj.transform.localPosition = player.GetTransform().position + Vector3.up * 2.6f;
			Player remotePlayerByUserID2 = GetRemotePlayerByUserID(VIPInPlayerID);
			if (remotePlayerByUserID2 != null)
			{
				VIPDirObj.SetActive(true);
				VIPDirObj.transform.LookAt(remotePlayerByUserID2.GetTransform().position + Vector3.up);
				if (vipTeamDirObj != (DirObjTeamName)remotePlayerByUserID2.Team)
				{
					Color color = UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)remotePlayerByUserID2.Team];
					color = new Color(color.r * 0.5f, color.g * 0.5f, color.b * 0.5f, color.a);
					VIPDirObj.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_TintColor", color);
					vipTeamDirObj = (DirObjTeamName)remotePlayerByUserID2.Team;
				}
			}
			else
			{
				VIPDirObj.SetActive(false);
			}
			Player playerByUserID = GetPlayerByUserID(VIPInPlayerID);
			if (playerByUserID != null)
			{
				VIPEffect.SetActive(true);
				VIPEffect.transform.position = playerByUserID.GetTransform().position;
			}
			else
			{
				VIPEffect.SetActive(false);
			}
		}
		else
		{
			if (GameApp.GetInstance().GetGameMode().ModePlay != Mode.VS_CMI)
			{
				return;
			}
			foreach (CMIGift cmiGift in cmiGifts)
			{
				cmiGift.Loop(deltaTime);
			}
			cmiSpecialGift.Loop(deltaTime);
			if (CMIDirObj != null)
			{
				CMIDirObj.transform.localPosition = player.GetTransform().position + Vector3.up * 2.6f;
				if (cmiSpecialGift.GetActive())
				{
					CMIDirObj.SetActive(true);
					CMIDirObj.transform.LookAt(cmiSpecialGift.GetPosition() + Vector3.up);
				}
				else
				{
					CMIDirObj.SetActive(false);
				}
			}
		}
	}

	public Enemy CreateBossLevel(EnemyType eType)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.ITEM);
		GameObject[] array2 = array;
		foreach (GameObject obj in array2)
		{
			Object.Destroy(obj);
		}
		Enemy enemy = null;
		GameObject gameObject = null;
		GameObject original = null;
		InBossBattle = true;
		State = GameState.Playing;
		Vector3 position = Vector3.zero;
		Quaternion rotation = Quaternion.identity;
		AudioManager.GetInstance().StopMusic();
		switch (eType)
		{
		case EnemyType.Dragon:
		{
			gameObject = GameObject.FindGameObjectWithTag(TagName.LEVEL);
			Object.Destroy(gameObject);
			GameObject gameObject2 = Resources.Load("Level/Boss1") as GameObject;
			GameObject gameObject3 = Object.Instantiate(gameObject2, Vector3.zero, Quaternion.identity) as GameObject;
			player.GetTransform().position = Vector3.zero + Vector3.up * 0.3f + Vector3.forward * -15f + Vector3.left * 15f;
			enemy = new Dragon();
			original = Resources.Load("Enemy/boss01") as GameObject;
			AudioManager.GetInstance().PlayMusic("Audio/boss_final");
			bossArray = new Enemy[1];
			bossArray[0] = enemy;
			position = gameObject2.transform.position + Vector3.up * 3f;
			break;
		}
		case EnemyType.Beetle:
		{
			gameObject = GameObject.FindGameObjectWithTag(TagName.LEVEL);
			Object.Destroy(gameObject);
			GameObject gameObject2 = Resources.Load("Level/Boss3") as GameObject;
			GameObject gameObject3 = Object.Instantiate(gameObject2, Vector3.zero, Quaternion.identity) as GameObject;
			player.GetTransform().position = Vector3.zero + Vector3.up * 0.3f + Vector3.forward * -3f + Vector3.left * 3f;
			enemy = new Spider();
			original = Resources.Load("Enemy/boss03") as GameObject;
			AudioManager.GetInstance().PlayMusic("Audio/boss_final");
			bossArray = new Enemy[1];
			bossArray[0] = enemy;
			position = gameObject2.transform.position + Vector3.up * 3f;
			break;
		}
		case EnemyType.Mantis:
		{
			Debug.Log("create mantis level");
			gameObject = GameObject.FindGameObjectWithTag(TagName.LEVEL);
			Object.Destroy(gameObject);
			GameObject gameObject2 = Resources.Load("Level/Boss2") as GameObject;
			GameObject gameObject3 = Object.Instantiate(gameObject2, Vector3.zero, Quaternion.identity) as GameObject;
			player.GetTransform().position = Vector3.zero + Vector3.up * 0.3f + Vector3.forward * -3f + Vector3.left * 3f;
			enemy = new Mantis();
			original = Resources.Load("Enemy/boss02") as GameObject;
			AudioManager.GetInstance().PlayMusic("Audio/boss_final");
			bossArray = new Enemy[1];
			bossArray[0] = enemy;
			break;
		}
		case EnemyType.MainMantis:
		{
			Debug.Log("create double mantis level");
			gameObject = GameObject.FindGameObjectWithTag(TagName.LEVEL);
			Object.Destroy(gameObject);
			GameObject gameObject2 = Resources.Load("Level/Boss4") as GameObject;
			GameObject gameObject3 = Object.Instantiate(gameObject2, Vector3.zero, Quaternion.identity) as GameObject;
			player.GetTransform().position = Vector3.zero + Vector3.up * 0.3f + Vector3.forward * -3f + Vector3.left * 3f;
			enemy = new MainMantis();
			original = Resources.Load("Enemy/boss02") as GameObject;
			AudioManager.GetInstance().PlayMusic("Audio/boss_final");
			bossArray = new Enemy[2];
			bossArray[0] = enemy;
			break;
		}
		case EnemyType.Earthworm:
		{
			gameObject = GameObject.FindGameObjectWithTag(TagName.LEVEL);
			Object.Destroy(gameObject);
			GameObject gameObject2 = Resources.Load("Level/Boss5") as GameObject;
			GameObject gameObject3 = Object.Instantiate(gameObject2, Vector3.zero, Quaternion.identity) as GameObject;
			player.GetTransform().position = Vector3.zero + Vector3.up * 0.3f + Vector3.forward * -3f + Vector3.left * 3f;
			enemy = new Earthworm();
			original = Resources.Load("Enemy/boss05") as GameObject;
			AudioManager.GetInstance().PlayMusic("Audio/boss_final");
			bossArray = new Enemy[1];
			bossArray[0] = enemy;
			position = gameObject2.transform.position + Vector3.up * 3f;
			break;
		}
		case EnemyType.SantaMachine:
		{
			gameObject = GameObject.FindGameObjectWithTag(TagName.LEVEL);
			Object.Destroy(gameObject);
			GameObject gameObject2 = Resources.Load("Level/Boss6") as GameObject;
			GameObject gameObject3 = Object.Instantiate(gameObject2, Vector3.zero, Quaternion.identity) as GameObject;
			player.GetTransform().position = Vector3.zero + Vector3.up * 0.3f + Vector3.forward * -3f + Vector3.left * 3f;
			enemy = new SatanMachine();
			original = Resources.Load("Enemy/boss06") as GameObject;
			AudioManager.GetInstance().PlayMusic("Audio/boss_final");
			bossArray = new Enemy[1];
			bossArray[0] = enemy;
			position = gameObject2.transform.position + Vector3.up * 3f;
			break;
		}
		}
		byte b = 0;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			b = player.GetSeatID();
		}
		GameObject gameObject4 = GameObject.FindGameObjectsWithTag(TagName.PLAYER_SPAWN_IN_BOSS)[b];
		if (null != gameObject4)
		{
			player.GetTransform().position = gameObject4.transform.position;
			player.GetTransform().rotation = gameObject4.transform.rotation;
			tpcs.AngelH = gameObject4.transform.rotation.eulerAngles.y;
		}
		GameObject gameObject5 = GameObject.FindGameObjectsWithTag(TagName.BOSS_SPAWN_POINT)[0];
		if (null != gameObject5)
		{
			position = gameObject5.transform.position;
			rotation = gameObject5.transform.rotation;
		}
		enemy.EnemyType = eType;
		if (GameObject.Find("E_" + enemyID) == null)
		{
			GameObject gameObject6 = Object.Instantiate(original) as GameObject;
			gameObject6.transform.position = position;
			gameObject6.transform.rotation = rotation;
			enemy.Init(gameObject6);
			enemy.EnemyID = bossID;
			gameObject6.name = "E_" + enemy.EnemyID;
			enemyList.Add(gameObject6.name, enemy);
		}
		if (eType == EnemyType.MainMantis)
		{
			Enemy enemy2 = new AssistMantis();
			bossArray[1] = enemy2;
			gameObject5 = GameObject.FindGameObjectsWithTag(TagName.BOSS_SPAWN_POINT)[1];
			if (null != gameObject5)
			{
				position = gameObject5.transform.position;
				rotation = gameObject5.transform.rotation;
			}
			GameObject original2 = Resources.Load("Enemy/boss02-1") as GameObject;
			enemy2.EnemyType = EnemyType.AssistMantis;
			GameObject gameObject7 = Object.Instantiate(original2) as GameObject;
			gameObject7.transform.position = position;
			gameObject7.transform.rotation = rotation;
			enemy2.Init(gameObject7);
			enemy2.EnemyID = (short)(bossID + 1);
			gameObject7.name = "E_" + enemy2.EnemyID;
			enemyList.Add(gameObject7.name, enemy2);
			(enemy as CoopMantis).SetCoopMantis(enemy2 as CoopMantis);
			(enemy2 as CoopMantis).SetCoopMantis(enemy as CoopMantis);
		}
		FadeAnimationScript.GetInstance().StartFade(Color.white, new Color(1f, 1f, 1f, 0f), 1f);
		player.PlayTeleportAnimation();
		bossLeveStartFadeTime = Time.time;
		return enemy;
	}

	public void LateLoop(float deltaTime)
	{
		if (player != null)
		{
			player.AdjustAnimationInLateUpdate(deltaTime);
		}
		foreach (RemotePlayer remotePlayer in remotePlayerList)
		{
			if (remotePlayer != null)
			{
				remotePlayer.AdjustAnimationInLateUpdate(deltaTime);
			}
		}
	}

	public void CreateTeamSkills()
	{
		TeamSkills = new TeamSkill();
		OtherTeamSkill = new TeamSkill();
		if (player == null)
		{
			Debug.Log("player kong");
		}
		else
		{
			PlayerSkill skills = player.GetSkills();
			if (skills == null)
			{
				Debug.Log("ps kong");
			}
		}
		TeamSkills.teamHpRecovery = player.GetSkills().GetSkill(SkillsType.TEAM_HP_RECOVERY);
		TeamSkills.teamAttackBooth = player.GetSkills().GetSkill(SkillsType.TEAM_ATTACK_BOOTH);
		TeamSkills.teamDamageReduce = player.GetSkills().GetSkill(SkillsType.TEAM_DAMAGE_REDUCE);
		foreach (RemotePlayer remotePlayer in remotePlayerList)
		{
			if (!remotePlayer.InPlayingState())
			{
				continue;
			}
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				if (!GameApp.GetInstance().GetGameMode().IsTeamMode())
				{
					continue;
				}
				if (!remotePlayer.IsSameTeam(player))
				{
					float skill = remotePlayer.GetSkills().GetSkill(SkillsType.TEAM_HP_RECOVERY);
					float skill2 = remotePlayer.GetSkills().GetSkill(SkillsType.TEAM_ATTACK_BOOTH);
					float skill3 = remotePlayer.GetSkills().GetSkill(SkillsType.TEAM_DAMAGE_REDUCE);
					if (skill > OtherTeamSkill.teamHpRecovery)
					{
						OtherTeamSkill.teamHpRecovery = skill;
					}
					if (skill2 > OtherTeamSkill.teamAttackBooth)
					{
						OtherTeamSkill.teamAttackBooth = skill2;
					}
					if (skill3 < OtherTeamSkill.teamDamageReduce)
					{
						OtherTeamSkill.teamDamageReduce = skill3;
					}
					continue;
				}
			}
			float skill4 = remotePlayer.GetSkills().GetSkill(SkillsType.TEAM_HP_RECOVERY);
			float skill5 = remotePlayer.GetSkills().GetSkill(SkillsType.TEAM_ATTACK_BOOTH);
			float skill6 = remotePlayer.GetSkills().GetSkill(SkillsType.TEAM_DAMAGE_REDUCE);
			if (skill4 > TeamSkills.teamHpRecovery)
			{
				TeamSkills.teamHpRecovery = skill4;
			}
			if (skill5 > TeamSkills.teamAttackBooth)
			{
				TeamSkills.teamAttackBooth = skill5;
			}
			if (skill6 < TeamSkills.teamDamageReduce)
			{
				TeamSkills.teamDamageReduce = skill6;
			}
			Debug.Log("team rc:" + TeamSkills.teamHpRecovery);
		}
		player.CreateWeaponDamageBooth();
	}

	public void SpawnItem(LootType lootType, Vector3 pos, int amount, short sequenceID)
	{
		float num = Global.FLOORHEIGHT;
		Ray ray = new Ray(pos + new Vector3(0f, 0.5f, 0f), Vector3.down);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 10f, 1 << PhysicsLayer.FLOOR))
		{
			num = hitInfo.point.y;
		}
		GameObject original = Resources.Load("Loot/Enegy") as GameObject;
		GameObject original2 = Resources.Load("Loot/Money") as GameObject;
		GameObject gameObject = null;
		float num2 = num;
		switch (lootType)
		{
		case LootType.Enegy:
			gameObject = Object.Instantiate(original, new Vector3(pos.x, num2 + 1f, pos.z), Quaternion.identity) as GameObject;
			gameObject.GetComponent<ItemScript>().itemType = lootType;
			gameObject.GetComponent<ItemScript>().LowPos = num2 + 1.2f;
			gameObject.GetComponent<ItemScript>().HighPos = num2 + 1.5f;
			gameObject.GetComponent<ItemScript>().Amount = amount;
			gameObject.GetComponent<ItemScript>().sequenceID = sequenceID;
			break;
		case LootType.Money:
			gameObject = Object.Instantiate(original2, new Vector3(pos.x, num2 + 1f, pos.z), Quaternion.identity) as GameObject;
			gameObject.GetComponent<ItemScript>().itemType = lootType;
			gameObject.GetComponent<ItemScript>().LowPos = num2 + 1.2f;
			gameObject.GetComponent<ItemScript>().HighPos = num2 + 1.5f;
			gameObject.GetComponent<ItemScript>().Amount = amount;
			gameObject.GetComponent<ItemScript>().sequenceID = sequenceID;
			break;
		}
		gameObject.name = "Loot_" + sequenceID;
		gameObject.tag = TagName.ITEM;
		GameObject original3 = Resources.Load("Loot/Halo") as GameObject;
		GameObject gameObject2 = Object.Instantiate(original3, gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
		gameObject2.AddComponent<LookAtCameraScript>();
		gameObject2.transform.localScale = Vector3.one * 0.4f;
		gameObject2.transform.parent = gameObject.transform;
	}

	public void DestroyLoot(short sequenceID)
	{
		GameObject gameObject = GameObject.Find("Loot_" + sequenceID);
		if (gameObject != null)
		{
			Object.DestroyObject(gameObject);
		}
	}

	public void VSTimeStopResume()
	{
		if (vsClock != null && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_FFA)
			{
				if (remotePlayerList.Count == 0)
				{
					vsClock.StopTime();
				}
				else
				{
					vsClock.ResumeTime();
				}
			}
			else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_TDM)
			{
				bool flag = false;
				foreach (RemotePlayer remotePlayer in remotePlayerList)
				{
					if (remotePlayer.Team != player.Team)
					{
						flag = true;
					}
				}
				if (flag)
				{
					vsClock.ResumeTime();
				}
				else
				{
					vsClock.StopTime();
				}
			}
		}
		if (flagClock == null || !GameApp.GetInstance().GetGameMode().IsCatchTheFlagMode())
		{
			return;
		}
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_FFA)
		{
			if (remotePlayerList.Count == 0)
			{
				flagClock.StopTime();
			}
			else if (FlagInPlayerID != -1)
			{
				flagClock.ResumeTime();
			}
		}
		else
		{
			if (GameApp.GetInstance().GetGameMode().ModePlay != Mode.VS_CTF_TDM)
			{
				return;
			}
			bool flag2 = false;
			foreach (RemotePlayer remotePlayer2 in remotePlayerList)
			{
				if (remotePlayer2.Team != player.Team)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				if (FlagInPlayerID != -1)
				{
					flagClock.ResumeTime();
				}
			}
			else
			{
				flagClock.StopTime();
			}
		}
	}

	public bool RemotePlayerExists(int channelID)
	{
		RemotePlayer remotePlayerByUserID = GetRemotePlayerByUserID(channelID);
		if (remotePlayerByUserID != null)
		{
			return true;
		}
		return false;
	}

	public void SubmitBattleScores()
	{
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			CoopState coopState = (CoopState)GameApp.GetInstance().GetUserState().GetBattleStates()[0];
			coopState.SetMaxKills(player.Kills);
			coopState.AddTotalKills(player.Kills);
		}
		else if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_TDM)
			{
				TDMState tDMState = (TDMState)GameApp.GetInstance().GetUserState().GetBattleStates()[1];
				tDMState.SetMaxKills(player.VSStatistics.Kills);
				tDMState.AddTotalKills(player.VSStatistics.Kills);
				tDMState.AtomicLoses();
			}
			else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_FFA)
			{
				FFAState fFAState = (FFAState)GameApp.GetInstance().GetUserState().GetBattleStates()[2];
				fFAState.SetMaxKills(player.VSStatistics.Kills);
				fFAState.AddTotalKills(player.VSStatistics.Kills);
				fFAState.AtomicLoses();
			}
			else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_VIP)
			{
				VIPState vIPState = (VIPState)GameApp.GetInstance().GetUserState().GetBattleStates()[3];
				vIPState.SetMaxKills(player.VSStatistics.Kills);
				vIPState.AddTotalKills(player.VSStatistics.Kills);
				vIPState.AtomicLoses();
			}
			else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
			{
				CMIState cMIState = (CMIState)GameApp.GetInstance().GetUserState().GetBattleStates()[4];
				cMIState.SetMaxKills(player.VSStatistics.Kills);
				cMIState.AddTotalKills(player.VSStatistics.Kills);
				cMIState.AtomicLoses();
			}
		}
		GameApp.GetInstance().GetUserState().Achievement.SubmitScore(player.Kills);
		GameApp.GetInstance().GetUserState().Achievement.SubmitAllToGameCenter();
	}

	public Player GetPlayerByUserID(int playerID)
	{
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID == channelID)
		{
			return player;
		}
		return GetRemotePlayerByUserID(playerID);
	}
}
