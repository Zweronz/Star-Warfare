using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
	protected Player player;

	protected int waveNum;

	protected GameWorld gameWorld;

	protected GameObject[] doors;

	protected float lastUpdateTime = -1000f;

	protected int currentSpawnIndex;

	protected float spawnSpeed;

	protected float timeBetweenWaves;

	protected float waveStartTime;

	protected float waveEndTime;

	protected int currentDoorIndex;

	protected int doorCount;

	protected bool levelClear;

	private SpawnConfig spawnConfigInfo;

	protected EnemyType bossType;

	private int playerIndex;

	public bool survival;

	protected List<EnemySpawnItem> spawnItems;

	public int WaveNum
	{
		get
		{
			return waveNum;
		}
	}

	public Player GetNextPlayer()
	{
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		int count = remotePlayers.Count;
		Player result = gameWorld.GetPlayer();
		if (playerIndex < count)
		{
			result = remotePlayers[playerIndex];
		}
		if (playerIndex > count)
		{
			playerIndex = 0;
		}
		else
		{
			playerIndex++;
		}
		return result;
	}

	private IEnumerator Start()
	{
		while (!InGameUIScript.bInited)
		{
			yield return 0;
		}
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			yield break;
		}
		for (int j = 0; j < 10; j++)
		{
			yield return 0;
		}
		int stage = GameApp.GetInstance().GetUserState().GetStage();
		int subStage = GameApp.GetInstance().GetUserState().GetSubStage();
		int netStage = GameApp.GetInstance().GetUserState().GetNetStage();
		int dataID = stage * Global.TOTAL_SUB_STAGE + subStage;
		int stageD = stage;
		int subStageD = subStage;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			dataID = ((netStage < Global.TOTAL_STAGE) ? (netStage * Global.TOTAL_SUB_STAGE + (Global.TOTAL_SUB_STAGE - 1)) : (Global.TOTAL_STAGE * Global.TOTAL_SUB_STAGE + netStage - Global.TOTAL_STAGE));
			if (netStage < Global.TOTAL_STAGE)
			{
				survival = true;
			}
			else
			{
				survival = false;
			}
			stageD = netStage;
		}
		else if (stage >= Global.TOTAL_STAGE)
		{
			dataID = Global.TOTAL_STAGE * Global.TOTAL_SUB_STAGE + stage - Global.TOTAL_STAGE;
		}
		else if (subStage == Global.TOTAL_SUB_STAGE - 1)
		{
			survival = true;
		}
		GameApp.GetInstance().GetGameWorld().DifficultyLevel = stageD * 3;
		if (!survival)
		{
			GameApp.GetInstance().GetGameWorld().DifficultyLevel += subStageD / 2;
		}
		if (stageD >= Global.TOTAL_STAGE)
		{
			switch (stageD)
			{
			case 7:
				GameApp.GetInstance().GetGameWorld().DifficultyLevel = 5;
				break;
			case 8:
				GameApp.GetInstance().GetGameWorld().DifficultyLevel = 11;
				break;
			case 9:
				GameApp.GetInstance().GetGameWorld().DifficultyLevel = 17;
				break;
			case 10:
				GameApp.GetInstance().GetGameWorld().DifficultyLevel = 17;
				break;
			case 11:
				GameApp.GetInstance().GetGameWorld().DifficultyLevel = 20;
				break;
			case 12:
				GameApp.GetInstance().GetGameWorld().DifficultyLevel = 20;
				break;
			case 13:
				GameApp.GetInstance().GetGameWorld().DifficultyLevel = 20;
				break;
			}
		}
		int baseDifficultyLevel = GameApp.GetInstance().GetGameWorld().DifficultyLevel;
		GameApp.GetInstance().GetGameWorld().BaseDifficultyLevel = baseDifficultyLevel;
		spawnSpeed = 2f - (float)waveNum * 0.1f;
		timeBetweenWaves = 1f;
		gameWorld = GameApp.GetInstance().GetGameWorld();
		doors = GameObject.FindGameObjectsWithTag(TagName.ENEMY_SPAWN_POINT);
		doorCount = doors.Length;
		waveStartTime = Time.time;
		player = gameWorld.GetPlayer();
		spawnConfigInfo = new SpawnConfig();
		spawnConfigInfo.Rounds = new List<Round>();
		UnitDataTable dataTable = Res2DManager.GetInstance().vDataTable[19 + dataID];
		for (int i = 0; i < dataTable.sRows; i++)
		{
			Round newRound = new Round
			{
				intermission = dataTable.GetData(i, 0, 0, false),
				EnemyInfos = new List<EnemySpawnInfo>()
			};
			for (int k = 0; k < 3; k++)
			{
				EnemySpawnInfo info = new EnemySpawnInfo
				{
					EType = (EnemyType)dataTable.GetData(i, 1 + k * 2, 0, false),
					From = (SpawnFromType)dataTable.GetData(i, 7, 0, false),
					Count = dataTable.GetData(i, 2 + k * 2, 0, false)
				};
				newRound.EnemyInfos.Add(info);
			}
			spawnConfigInfo.Rounds.Add(newRound);
		}
		Round newRound2 = new Round
		{
			EnemyInfos = new List<EnemySpawnInfo>()
		};
		EnemySpawnInfo info2 = new EnemySpawnInfo
		{
			EType = EnemyType.MainMantis,
			From = SpawnFromType.Door,
			Count = 1
		};
		newRound2.EnemyInfos.Add(info2);
		Round newRound3 = new Round
		{
			intermission = 1f,
			EnemyInfos = new List<EnemySpawnInfo>()
		};
		EnemySpawnInfo info3 = new EnemySpawnInfo
		{
			EType = EnemyType.Mantis,
			From = SpawnFromType.Door,
			Count = 1
		};
		newRound3.EnemyInfos.Add(info3);
		EnemySpawnInfo info4 = new EnemySpawnInfo
		{
			EType = EnemyType.Mutalisk,
			From = SpawnFromType.Grave,
			Count = 100
		};
		EnemySpawnInfo info5 = new EnemySpawnInfo
		{
			EType = EnemyType.Mutalisk,
			From = SpawnFromType.Grave,
			Count = 20
		};
		EnemySpawnInfo info6 = new EnemySpawnInfo
		{
			EType = EnemyType.Worm,
			From = SpawnFromType.Grave,
			Count = 20
		};
		EnemySpawnInfo info7 = new EnemySpawnInfo
		{
			EType = EnemyType.Reaver,
			From = SpawnFromType.Door,
			Count = 10
		};
		EnemySpawnInfo info8 = new EnemySpawnInfo
		{
			EType = EnemyType.Tank,
			From = SpawnFromType.Door,
			Count = 6
		};
		EnemySpawnInfo info9 = new EnemySpawnInfo
		{
			EType = EnemyType.Dragon,
			From = SpawnFromType.Door,
			Count = 1
		};
		if (!survival)
		{
			GameApp.GetInstance().GetGameWorld().TotalWaves = spawnConfigInfo.Rounds.Count;
		}
		else
		{
			GameApp.GetInstance().GetGameWorld().TotalEnemyCount = 999999;
		}
		spawnItems = new List<EnemySpawnItem>();
		int totalRound = 0;
		for (int r = 0; r < spawnConfigInfo.Rounds.Count; r++)
		{
			Round round = spawnConfigInfo.Rounds[r];
			for (int l = 0; l < round.EnemyInfos.Count; l++)
			{
				EnemySpawnInfo enemyInfo = round.EnemyInfos[l];
				for (int m = 0; m < enemyInfo.Count; m++)
				{
					EnemySpawnItem esItem = new EnemySpawnItem
					{
						EType = enemyInfo.EType,
						From = enemyInfo.From,
						Round = r,
						intermission = 0f
					};
					if (l == 0 && m == 0)
					{
						esItem.intermission = round.intermission;
					}
					spawnItems.Add(esItem);
				}
			}
			totalRound++;
		}
		int limit = 8;
		int repeatIndex = 0;
		int roundNum = 0;
		int enemyLeft4 = 0;
		int totalCount = spawnItems.Count;
		GameApp.GetInstance().GetGameWorld().TotalEnemyCount = totalCount;
		while (true)
		{
			if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
			{
				int enemyID2 = GameApp.GetInstance().GetGameWorld().EnemyID;
				int indexID = enemyID2;
				if (survival)
				{
					indexID = enemyID2 % totalCount;
					repeatIndex = enemyID2 / totalCount;
				}
				if (indexID < totalCount)
				{
					GameObject enemyPrefab = null;
					Transform grave = null;
					EnemySpawnItem spawnItem = spawnItems[indexID];
					if (survival)
					{
						roundNum = repeatIndex * totalRound + spawnItem.Round;
						GameApp.GetInstance().GetGameWorld().DifficultyLevel = baseDifficultyLevel + roundNum / Global.SURVIVAL_DIFFICULTY_UP_EVERY_ROUND;
					}
					for (enemyLeft4 = GameApp.GetInstance().GetGameWorld().GetEnemies()
						.Count; enemyLeft4 >= limit; enemyLeft4 = GameApp.GetInstance().GetGameWorld().GetEnemies()
						.Count)
					{
						yield return new WaitForSeconds(0.5f);
					}
					EnemyType enemyType = spawnItem.EType;
					if (spawnItem.intermission > 0f)
					{
						yield return new WaitForSeconds(spawnItem.intermission);
					}
					else
					{
						yield return new WaitForSeconds(0.5f);
					}
					if (enemyType == EnemyType.Dragon || enemyType == EnemyType.Beetle || enemyType == EnemyType.Mantis || enemyType == EnemyType.MainMantis || enemyType == EnemyType.Earthworm || enemyType == EnemyType.SantaMachine)
					{
						bossType = enemyType;
						GameApp.GetInstance().GetGameWorld().EnemyID = GameApp.GetInstance().GetGameWorld().EnemyID + 1;
						continue;
					}
					SpawnFromType from = spawnItem.From;
					if (from == SpawnFromType.Grave)
					{
						grave = CalculateGravePosition(GetNextPlayer().GetTransform());
					}
					enemyLeft4 = GameApp.GetInstance().GetGameWorld().GetEnemies()
						.Count;
					Vector3 spawnPosition2 = Vector3.zero;
					GameObject smokeObj = null;
					GameObject graveObj2 = null;
					switch (from)
					{
					case SpawnFromType.Door:
						spawnPosition2 = doors[currentDoorIndex].transform.position;
						currentDoorIndex++;
						if (currentDoorIndex == doorCount)
						{
							currentDoorIndex = 0;
						}
						break;
					case SpawnFromType.Grave:
					{
						float rndX = Random.Range((0f - grave.localScale.x) / 2f, grave.localScale.x / 2f);
						float rndZ = Random.Range((0f - grave.localScale.z) / 2f, grave.localScale.z / 2f);
						spawnPosition2 = grave.position + new Vector3(rndX, 0f, rndZ);
						if (Lobby.GetInstance().IsMasterPlayer)
						{
							GameObject yanPrefab = Resources.Load("Effect/Yan") as GameObject;
							Object.Instantiate(yanPrefab, spawnPosition2 + Vector3.up * 1f, Quaternion.identity);
						}
						GameObject rock1 = Resources.Load("Effect/GraveRock1") as GameObject;
						Object.Instantiate(rock1, spawnPosition2 + Vector3.down * 0f, Quaternion.identity);
						GameObject smoke = Resources.Load("Effect/grave_smoke") as GameObject;
						smokeObj = Object.Instantiate(smoke, spawnPosition2 + Vector3.up * 1f, Quaternion.identity) as GameObject;
						GameObject ground = Resources.Load("Effect/Grave_Ground") as GameObject;
						graveObj2 = Object.Instantiate(ground, spawnPosition2 + Vector3.up * 0.1f, Quaternion.Euler(270f, 0f, 0f)) as GameObject;
						break;
					}
					}
					GameObject currentEnemy2 = null;
					Enemy enemy3 = null;
					bool newPlayer = false;
					if (stageD == 0 && subStageD < 3)
					{
						newPlayer = true;
					}
					bool bElite = false;
					if (!newPlayer)
					{
						bElite = EliteSpawn(enemyType, 0, 0);
					}
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						if (Lobby.GetInstance().IsMasterPlayer)
						{
							EnemySpawnRequest esRequest3 = new EnemySpawnRequest(enemyType, spawnPosition2, (short)roundNum, 0, bElite, from == SpawnFromType.Grave);
							GameApp.GetInstance().GetNetworkManager().SendRequest(esRequest3);
						}
					}
					else if (GameApp.GetInstance().GetGameMode().IsSingle())
					{
						enemy3 = GameApp.GetInstance().GetGameWorld().SpawnEnemy(enemyType, (short)enemyID2, spawnPosition2, bElite);
					}
					if (GameApp.GetInstance().GetGameMode().IsSingle() && from == SpawnFromType.Grave)
					{
						if (smokeObj != null)
						{
							smokeObj.transform.parent = enemy3.GetTransform();
						}
						enemy3.SetInGrave(true);
					}
					GameApp.GetInstance().GetGameWorld().EnemyID = GameApp.GetInstance().GetGameWorld().EnemyID + 1;
				}
				else if (GameApp.GetInstance().GetGameWorld().GetEnemies()
					.Count == 0)
				{
					break;
				}
			}
			yield return 0;
		}
		if (bossType == EnemyType.Dragon || bossType == EnemyType.Beetle || bossType == EnemyType.Mantis || bossType == EnemyType.MainMantis || bossType == EnemyType.Earthworm || bossType == EnemyType.SantaMachine)
		{
			Vector3 spawnPosition = GameObject.FindGameObjectsWithTag(TagName.ENEMY_SPAWN_POINT)[0].transform.position;
			GameObject currentEnemy = null;
			Enemy enemy2 = null;
			short enemyID = (short)GameApp.GetInstance().GetGameWorld().EnemyID;
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				if (Lobby.GetInstance().IsMasterPlayer)
				{
					while (!GameApp.GetInstance().GetGameWorld().GetPlayer()
						.InPlayingState())
					{
						yield return 0;
					}
					GameApp.GetInstance().GetGameWorld().State = GameState.SwitchBossLevel;
					EnemySpawnRequest esRequest2 = new EnemySpawnRequest(bossType, spawnPosition, (short)roundNum, 0, false, false);
					GameApp.GetInstance().GetNetworkManager().SendRequest(esRequest2);
					if (bossType == EnemyType.MainMantis)
					{
						esRequest2 = new EnemySpawnRequest(EnemyType.AssistMantis, spawnPosition, (short)roundNum, 0, false, false);
						GameApp.GetInstance().GetNetworkManager().SendRequest(esRequest2);
					}
				}
			}
			else if (GameApp.GetInstance().GetGameMode().IsSingle())
			{
				enemy2 = GameApp.GetInstance().GetGameWorld().SpawnEnemy(bossType, enemyID, spawnPosition, false);
			}
		}
		yield return new WaitForSeconds(1f);
		enemyLeft4 = GameApp.GetInstance().GetGameWorld().GetEnemies()
			.Count;
		while (enemyLeft4 > 0 || GameApp.GetInstance().GetGameWorld().State == GameState.SwitchBossLevel)
		{
			yield return 0;
			Hashtable enemyList = GameApp.GetInstance().GetGameWorld().GetEnemies();
			enemyLeft4 = enemyList.Count;
		}
		player.OnWin();
		player.SetState(Player.WIN_STATE);
		yield return new WaitForSeconds(1f);
		if (player.NeverGotHit)
		{
			GameApp.GetInstance().GetUserState().Achievement.CheckAchievement_Ghost();
		}
	}

	public void SpawnEnemy(EnemySpawnItem esItem)
	{
	}

	public bool EliteSpawn(EnemyType eType, int spawnNum, int index)
	{
		bool result = false;
		int num = Random.Range(0, 100);
		if (num < 5)
		{
			result = true;
		}
		if (eType == EnemyType.Boomer || eType == EnemyType.Tank)
		{
			result = false;
		}
		return result;
	}

	public Transform CalculateGravePosition(Transform playerTrans)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.GRAVE);
		GameObject gameObject = null;
		float num = 99999f;
		GameObject[] array2 = array;
		foreach (GameObject gameObject2 in array2)
		{
			float sqrMagnitude = (playerTrans.position - gameObject2.transform.position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				gameObject = gameObject2;
				num = sqrMagnitude;
			}
		}
		return gameObject.transform;
	}

	private void Update()
	{
		if (!(Time.time - lastUpdateTime < spawnSpeed))
		{
			lastUpdateTime = Time.time;
		}
	}

	private void OnDrawGizmos()
	{
	}
}
