using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
	private static GameUIManager Instance;

	private Dictionary<GameUIStatus, GameUI> mUiDic;

	private List<GameUIStatus> mNextStatusList = new List<GameUIStatus>();

	private List<GameUIStatus> mRemovedStatusList = new List<GameUIStatus>();

	private List<GameUIStatus> mHiddenStatusList = new List<GameUIStatus>();

	private List<GameUIBundle> mNextGameUIBundleList = new List<GameUIBundle>();

	private List<GameUIListener> mNextGameUIListenerList = new List<GameUIListener>();

	private void Awake()
	{
		if (Instance == null)
		{
			mUiDic = new Dictionary<GameUIStatus, GameUI>();
			Instance = this;
			Object.DontDestroyOnLoad(this);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public static GameUIManager GetInstance()
	{
		return Instance;
	}

	private void Update()
	{
		foreach (KeyValuePair<GameUIStatus, GameUI> item in mUiDic)
		{
			if (item.Value == null)
			{
				RemoveUI(item.Key);
			}
		}
		foreach (GameUIStatus mRemovedStatus in mRemovedStatusList)
		{
			if (mRemovedStatus == GameUIStatus.None)
			{
				continue;
			}
			Debug.Log("removedStatus : " + mRemovedStatus);
			if (mUiDic.ContainsKey(mRemovedStatus))
			{
				GameUI gameUI = mUiDic[mRemovedStatus];
				mUiDic.Remove(mRemovedStatus);
				if (gameUI != null)
				{
					Debug.Log("Destroy : " + mRemovedStatus);
					Object.Destroy(gameUI.gameObject);
				}
			}
		}
		mRemovedStatusList.Clear();
		foreach (GameUIStatus mHiddenStatus in mHiddenStatusList)
		{
			if (mHiddenStatus != 0 && mUiDic.ContainsKey(mHiddenStatus))
			{
				GameUI gameUI2 = mUiDic[mHiddenStatus];
				if (gameUI2 != null && !gameUI2.IsHide())
				{
					gameUI2.Hide();
				}
			}
		}
		mHiddenStatusList.Clear();
		for (int i = 0; i < mNextStatusList.Count; i++)
		{
			GameUIStatus gameUIStatus = mNextStatusList[i];
			GameUIBundle gameUIBundle = mNextGameUIBundleList[i];
			GameUIListener listener = mNextGameUIListenerList[i];
			if (mNextStatusList[i] == GameUIStatus.None)
			{
				continue;
			}
			if (mUiDic.ContainsKey(gameUIStatus))
			{
				GameUI gameUI3 = mUiDic[gameUIStatus];
				if (gameUI3 != null)
				{
					mUiDic.Remove(gameUIStatus);
					gameUI3.SetGameUIBundle(gameUIBundle);
					if (gameUI3.IsHide())
					{
						gameUI3.Show();
					}
					mUiDic.Add(gameUIStatus, gameUI3);
				}
			}
			else
			{
				GameUI gameUI4 = CreateUI(gameUIStatus);
				gameUI4.InitUI(gameUIStatus);
				gameUI4.SetListener(listener);
				gameUI4.SetGameUIBundle(gameUIBundle);
				mUiDic.Add(gameUIStatus, gameUI4);
			}
		}
		mNextStatusList.Clear();
		mNextGameUIBundleList.Clear();
	}

	private GameUI CreateUI(GameUIStatus gameUIStatus)
	{
		GameObject gameObject = null;
		GameObject gameObject2 = null;
		switch (gameUIStatus)
		{
		case GameUIStatus.HUD:
			gameObject = Resources.Load("NGUI/HUD/UI/HUD") as GameObject;
			gameObject2 = Object.Instantiate(gameObject) as GameObject;
			gameObject2.transform.position = new Vector3(10000f, 10000f, 10000f);
			break;
		case GameUIStatus.HUD_PAUSE:
			gameObject = Resources.Load("NGUI/HUD/UI/HUDPause") as GameObject;
			gameObject2 = Object.Instantiate(gameObject) as GameObject;
			gameObject2.transform.position = new Vector3(10000f, 10000f, 20000f);
			break;
		case GameUIStatus.BOUNTYHUNTER:
			gameObject = Resources.Load("NGUI/Ads/UI/BountyHunterUI") as GameObject;
			gameObject2 = Object.Instantiate(gameObject) as GameObject;
			gameObject2.transform.position = new Vector3(10000f, 20000f, 20000f);
			break;
		case GameUIStatus.CALLOFARENA:
			gameObject = Resources.Load("NGUI/Ads/UI/CallOfArenaUI") as GameObject;
			gameObject2 = Object.Instantiate(gameObject) as GameObject;
			gameObject2.transform.position = new Vector3(10000f, 20000f, 20000f);
			break;
		}
		if (gameObject2 == null)
		{
			Debug.LogError(string.Concat("Can't find UI '", gameUIStatus, "'"));
			return null;
		}
		return gameObject2.GetComponent<GameUI>();
	}

	public void LoadUI(GameUIStatus status, GameUIBundle bundle, GameUIListener listener)
	{
		mNextStatusList.Add(status);
		mNextGameUIBundleList.Add(bundle);
		mNextGameUIListenerList.Add(listener);
	}

	public void LoadUI(GameUIStatus status, GameUIBundle bundle)
	{
		LoadUI(status, bundle, null);
	}

	public void LoadUI(GameUIStatus status)
	{
		LoadUI(status, null, null);
	}

	public void LoadUI(GameUIStatus status, GameUIListener listener)
	{
		LoadUI(status, null, listener);
	}

	public void RemoveUI(GameUIStatus status)
	{
		mRemovedStatusList.Add(status);
	}

	public void RemoveUI(GameUI gameUI)
	{
		RemoveUI(gameUI.UIStatus);
	}

	public void HideUI(GameUIStatus status)
	{
		mHiddenStatusList.Add(status);
	}

	public void HideUI(GameUI gameUI)
	{
		HideUI(gameUI.UIStatus);
	}

	public GameUI GetGameUI(GameUIStatus status)
	{
		if (mUiDic.ContainsKey(status))
		{
			return mUiDic[status];
		}
		return null;
	}

	public void LoadHUD(HUDBattle.HUDType hudType, UIStateManager uiStateManager, IExitRoom exitroom)
	{
		GameUIBundle gameUIBundle = new GameUIBundle();
		gameUIBundle.Add("key_hud_type", hudType);
		gameUIBundle.Add("key_hud_UIStateManager", uiStateManager);
		gameUIBundle.Add("key_hud_exitroom", exitroom);
		LoadUI(GameUIStatus.HUD, gameUIBundle);
	}

	public void LoadHUDPause(UIStateManager uiStateManager, IUIPause uipause)
	{
		GameUIBundle gameUIBundle = new GameUIBundle();
		gameUIBundle.Add("key_hud_pause_UIStateManager", uiStateManager);
		gameUIBundle.Add("key_hud_pause_quitgame", uipause);
		LoadUI(GameUIStatus.HUD_PAUSE, gameUIBundle);
	}

	public void LoadBountyHunter()
	{
	}

	public void LoadCallOfArena()
	{
	}

	public void RemoveAll()
	{
		Debug.Log("RemoveAll");
		RemoveUI(GameUIStatus.HUD);
		RemoveUI(GameUIStatus.HUD_PAUSE);
	}
}
