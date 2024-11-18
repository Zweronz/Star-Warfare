using System;
using System.IO;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameApp
{
	public const byte RECORD_CORRECT = 0;

	public const byte RECORD_ERROR = 1;

	public const byte RECORD_MISMATCH = 2;

	public const byte RECORD_NOT_EXIST = 3;

	protected static GameApp instance;

	protected GameWorld gameWorld;

	protected NetworkManager networkMgr;

	protected NetworkManager networkMgrIAP;

	protected WWWManager mWWWManager;

	protected GameConfig gameConfig = new GameConfig();

	protected UserState userState = new UserState();

	protected GameMode gameMode = new GameMode(NetworkType.Single, Mode.CamPain);

	public float AverageSendPacket;

	public float AverageSendByte;

	public float AverageReceivePacket;

	public float AverageReceiveByte;

	public bool httpRequestSent;

	public int Base64MinLength = 50;

	public int PressBackCount;

	private int MithrilFromTapjoy;

	public bool GameStart;

	public bool openfreemithril = true;

	public bool UIInit;

	public bool isChangeName;

	public bool AdsInit;

	public static int SpringBulletCount;

	public static bool bShowAds;

	public static bool isGoogleServiceReady;

	public DeviceOrientation PreviousOrientation { get; set; }

	public bool LogoFirstPop { get; set; }

	public bool AdsPoped { get; set; }

	public string UUID { get; set; }

	public string MacAddress { get; set; }

	public string DataPath { get; set; }

	public byte AppStatus { get; set; }

	public byte AppSNSStatus { get; set; }

	public static GameApp GetInstance()
	{
		if (instance == null)
		{
			instance = new GameApp();
			instance.PreviousOrientation = DeviceOrientation.Portrait;
			instance.LogoFirstPop = true;
			instance.AdsPoped = false;
			Application.targetFrameRate = 60;
		}
		return instance;
	}

	public void ShowAdMobBanner()
	{
	}

	public int GetMithrilFromTapjoy()
	{
		return MithrilFromTapjoy;
	}

	public void SetMithrilFromTapjoy(int mithril)
	{
		MithrilFromTapjoy = mithril;
	}

	public bool isGameStart()
	{
		return GameStart;
	}

	public void SetGameStart()
	{
		GameStart = true;
		AndroidPluginScript.isStart();
	}

	public bool IsConnectedToInternet()
	{
		bool result = false;
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
		{
			result = true;
		}
		return result;
	}

	public void SetGameMode(GameMode mode)
	{
		gameMode = mode;
	}

	public GameMode GetGameMode()
	{
		return gameMode;
	}

	public GameWorld GetGameWorld()
	{
		return gameWorld;
	}

	public void CreateGameWorld()
	{
		gameWorld = new GameWorld();
		Lobby.GetInstance().GetVSClock().Reset();
		if (GetInstance().GetGameMode().IsMultiPlayer())
		{
			Player player = new LocalPlayer();
			player.InitSkills();
			int hp = (int)player.GetSkills().GetSkill(SkillsType.HP_BOOTH);
			if (GetInstance().GetGameMode().IsVSMode())
			{
				hp = VSMath.GetHpInVS(hp);
			}
			GetSceneStateRequest request = new GetSceneStateRequest(hp);
			networkMgr.SendRequest(request);
		}
		else
		{
			DateTime soloSuccBossTime = userState.GetSoloSuccBossTime();
			if ((DateTime.Now - soloSuccBossTime).TotalDays >= 1.0)
			{
				userState.SetSoloSuccBossTime(DateTime.Now);
				for (int i = 0; i < Global.TOTAL_SOLO_BOSS_NUM; i++)
				{
					userState.SetSuccSoloBoss(i, 0);
				}
			}
			gameWorld.Init();
			gameWorld.GetPlayer().SetSeatID((byte)Random.Range(0, 4));
			gameWorld.GetPlayer().DropAtSpawnPosition();
		}
		if (gameMode.ModePlay == Mode.VS_CMI)
		{
			AudioManager.GetInstance().PlayMusic("Audio/wish");
		}
		else
		{
			AudioManager.GetInstance().PlayMusic("Audio/normal");
		}
	}

	public void StartHttpRequestThread()
	{
		if (IsConnectedToInternet() && !httpRequestSent)
		{
			HttpRequestThread @object = new HttpRequestThread();
			Thread thread = new Thread(@object.DoWork);
			thread.Start();
			httpRequestSent = true;
		}
	}

	public void ClearGameWorld()
	{
		gameWorld = null;
	}

	public void EnterBossLevel()
	{
	}

	public void LoadConfig()
	{
		if (gameConfig.monsterConfTable.Count == 0)
		{
			if (Application.isEditor || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				gameConfig.LoadFromXML(null);
			}
			else
			{
				gameConfig.LoadFromXML("/");
			}
		}
	}

	public NetworkManager CreateNetwork()
	{
		networkMgr = new NetworkManager();
		networkMgr.StartNetwork(networkMgr.strIP, networkMgr.port);
		return networkMgr;
	}

	public NetworkManager CreateNetwork(string strIP, int port)
	{
		networkMgr = new NetworkManager();
		networkMgr.StartNetwork(strIP, port);
		return networkMgr;
	}

	public void DestoryNetWork()
	{
		if (networkMgr != null)
		{
			networkMgr.CloseConnection();
			networkMgr = null;
		}
	}

	public NetworkManager GetNetworkManager()
	{
		return networkMgr;
	}

	public NetworkManager CreateNetworkIAP(string strIP, int port)
	{
		networkMgrIAP = new NetworkManager();
		networkMgrIAP.StartNetwork(strIP, port);
		return networkMgrIAP;
	}

	public void DestoryNetWorkIAP()
	{
		if (networkMgrIAP != null)
		{
			networkMgrIAP.CloseConnection();
			networkMgrIAP = null;
		}
	}

	public NetworkManager GetNetworkManagerIAP()
	{
		return networkMgrIAP;
	}

	public WWWManager GetWWWManager()
	{
		if (mWWWManager == null)
		{
			mWWWManager = new WWWManager();
		}
		return mWWWManager;
	}

	public UserState GetUserState()
	{
		return userState;
	}

	public void Loop(float deltaTime)
	{
		gameWorld.Loop(deltaTime);
	}

	public void LateLoop(float deltaTime)
	{
		gameWorld.LateLoop(deltaTime);
	}

	public GameConfig GetGameConfig()
	{
		return gameConfig;
	}

	public void Save()
	{
		if (userState.bInit)
		{
			string text = Application.dataPath + "/../../Documents/";
			if (Application.platform == RuntimePlatform.Android)
			{
				text = Application.persistentDataPath + "/";
			}
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			userState.SaveData(binaryWriter);
			byte[] data = memoryStream.ToArray();
			memoryStream.Close();
			binaryWriter.Close();
			data = userState.CryptBuffer(data);
			Stream stream = File.Open(text + "starWarfareMap_01", FileMode.Create);
			stream.Write(data, 0, data.Length);
			stream.Flush();
			stream.Close();
			byte[] array = new byte[data.Length];
			data.CopyTo(array, 0);
			array = userState.CryptMD5Buffer(array);
			Stream stream2 = File.Open(text + "starWarfareMap_02", FileMode.Create);
			stream2.Write(array, 0, array.Length);
			stream2.Flush();
			stream2.Close();
		}
	}

	public byte Load()
	{
		string text = Application.dataPath + "/../../Documents/";
		if (Application.platform == RuntimePlatform.Android)
		{
			text = Application.persistentDataPath + "/";
		}
		if (File.Exists(text + "starWarfareMap_01") && File.Exists(text + "starWarfareMap_02"))
		{
			Stream stream = File.Open(text + "starWarfareMap_01", FileMode.Open);
			Stream stream2 = File.Open(text + "starWarfareMap_02", FileMode.Open);
			byte[] bytesFromStream = userState.GetBytesFromStream(stream);
			byte[] bytesFromStream2 = userState.GetBytesFromStream(stream2);
			if (!userState.VerifyMD5(bytesFromStream, bytesFromStream2))
			{
				return 2;
			}
			byte[] buffer = userState.DecryptBuffer(bytesFromStream);
			stream.Close();
			stream2.Close();
			MemoryStream memoryStream = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			string text2;
			try
			{
				text2 = userState.LoadData(binaryReader);
			}
			catch (Exception)
			{
				return 1;
			}
			finally
			{
				binaryReader.Close();
				memoryStream.Close();
			}
			if (!text2.Equals(userState.version))
			{
				Save();
			}
			return 0;
		}
		return 3;
	}
}
