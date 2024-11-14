using System;
using System.IO;
using UnityEngine;

public abstract class UIStateManager : MonoBehaviour
{
	public const byte PHASE_INIT = 0;

	public const byte PHASE_LOADING = 1;

	public const byte PHASE_MAINMENU = 2;

	public const byte PHASE_CUSTOMIZE = 3;

	public const byte PHASE_STORE = 4;

	public const byte PHASE_MAKE_PACKAGE = 5;

	public const byte PHASE_GAME = 6;

	public const byte PHASE_STATISTICS = 7;

	public const byte PHASE_OPTIONS = 8;

	public const byte PHASE_MULTI_PLAYER = 9;

	public const byte PHASE_STORE_PROPS = 10;

	public const byte PHASE_EXTRA = 11;

	public const byte PHASE_READY_GAME = 12;

	public const byte PHASE_STAGE_CHOISE = 13;

	public const byte PHASE_PAUSE = 14;

	public const byte PHASE_OPTIONS_INGAME = 15;

	public const byte PHASE_NET_STATISTICS = 16;

	protected const byte PHASE_LOGO = 17;

	protected const byte PHASE_SPLASH = 18;

	protected const byte PHASE_GOTO_START_MENU = 19;

	protected const byte PHASE_TUTORIAL = 20;

	private const byte FR_EXCEPTION_RUN = 101;

	private int nPhasePrevious = -1;

	private int nPhaseCurrent = -1;

	private int nPhaseNext = -1;

	private bool bPhaseInit;

	private bool bSkipPhase;

	private bool bSkipInit;

	private bool bSkipClose;

	private bool bIgnorePrevious;

	private bool bChangePhase;

	public UIManager m_UIManager;

	public UIManager m_UIPopupManager;

	public GUIStyle txtStyle;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		try
		{
			if (bChangePhase)
			{
				bChangePhase = false;
				if (!bSkipClose)
				{
					FrClose(nPhaseCurrent);
				}
				if (!bIgnorePrevious)
				{
					nPhasePrevious = nPhaseCurrent;
				}
				nPhaseCurrent = nPhaseNext;
				if (!bSkipInit)
				{
					FrInit(nPhaseCurrent);
				}
				bPhaseInit = true;
				if (nPhaseNext != nPhaseCurrent)
				{
					bSkipPhase = true;
				}
			}
			else
			{
				bPhaseInit = false;
			}
			if (bSkipPhase)
			{
				bSkipPhase = false;
			}
			else
			{
				FrUpdate(nPhaseCurrent);
			}
		}
		catch (IOException e)
		{
			exceptionCaught(e, 101);
		}
		finally
		{
			NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
			if (networkManager != null && networkManager.IsDisconnected)
			{
				GameApp.GetInstance().GetNetworkManager().CloseConnection();
			}
		}
	}

	public UIManager GetPopup()
	{
		GameObject gameObject = GameObject.Find("Popup");
		if (gameObject != null)
		{
			PopupScript component = gameObject.GetComponent<PopupScript>();
			return component.m_UIManager;
		}
		return null;
	}

	public void FrGoToPhase(int phase)
	{
		FrGoToPhase(phase, false, false, false);
	}

	protected abstract void FrInit(int phase);

	protected abstract void FrClose(int phase);

	protected abstract void FrUpdate(int phase);

	public abstract void FrFree();

	public void FrGoToPhase(int phase, bool skipInit, bool skipClose, bool ignorePrevious)
	{
		bChangePhase = true;
		nPhaseNext = phase;
		bSkipInit = skipInit;
		bSkipClose = skipClose;
		bIgnorePrevious = ignorePrevious;
	}

	public bool FrIsInitPhase()
	{
		return bPhaseInit;
	}

	public int FrGetCurrentPhase()
	{
		return nPhaseCurrent;
	}

	public int FrGetPreviousPhase()
	{
		return nPhasePrevious;
	}

	public int FrGetNextPhase()
	{
		return nPhaseNext;
	}

	public static void exceptionCaught(Exception e, int location)
	{
		string message = location + ":" + e.ToString();
		Debug.Log(message);
	}

	public void OnApplicationPause()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			Debug.Log("OnApplicationPause");
			GameApp.GetInstance().Save();
		}
	}

	public void OnApplicationQuit()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
			if (networkManager != null)
			{
				networkManager.CloseConnection();
			}
		}
		NetworkManager networkManagerIAP = GameApp.GetInstance().GetNetworkManagerIAP();
		if (networkManagerIAP != null)
		{
			networkManagerIAP.CloseConnection();
		}
		Debug.Log("on application quit");
		GameApp.GetInstance().Save();
	}
}
