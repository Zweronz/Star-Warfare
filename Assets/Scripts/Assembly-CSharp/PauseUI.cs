using UnityEngine;

public class PauseUI : UIHandler, IUIHandle, IUIPause
{
	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_HANDLE = 2;

	public UIStateManager stateMgr;

	protected NetworkManager networkMgr;

	private byte state;

	private MessageBoxUI msgUI;

	public PauseUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
		state = 0;
	}

	public void Init()
	{
		state = 0;
		Time.timeScale = 0f;
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
		Create();
	}

	public void Close()
	{
		stateMgr.m_UIPopupManager.RemoveAll();
	}

	public void Create()
	{
		stateMgr.m_UIPopupManager.RemoveAll();
		GameUIManager.GetInstance().LoadHUDPause(stateMgr, this);
		msgUI = new MessageBoxUI(stateMgr);
		msgUI.Create();
		msgUI.Hide();
		stateMgr.m_UIPopupManager.Add(msgUI);
	}

	public bool Update()
	{
		return false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control != msgUI)
		{
			return;
		}
		int eventID = msgUI.GetEventID();
		if (eventID == MessageBoxUI.EVENT_QUIT_GAME)
		{
			switch (command)
			{
			case 10:
				GameUIManager.GetInstance().RemoveAll();
				msgUI.Hide();
				Time.timeScale = 1f;
				stateMgr.FrGoToPhase(7, false, false, false);
				GameApp.GetInstance().GetGameWorld().SubmitBattleScores();
				break;
			case 9:
				msgUI.Hide();
				AudioManager.GetInstance().PlaySound(AudioName.PAUSE_BACK);
				GameUIManager.GetInstance().LoadHUDPause(stateMgr, this);
				break;
			}
		}
	}

	public void QuitGame()
	{
		msgUI.CreateQuery(UIConstant.GetMessage(0), MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_QUIT_GAME);
		msgUI.Show();
	}
}
