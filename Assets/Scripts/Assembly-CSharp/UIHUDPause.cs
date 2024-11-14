using UnityEngine;

public class UIHUDPause : GameUI, GameUIListener
{
	public const string BUNDLE_KEY_HUD_PAUSE_UISTATEMANAGER = "key_hud_pause_UIStateManager";

	public const string BUNDLE_KEY_HUD_PAUSE_QUITGAME = "key_hud_pause_quitgame";

	[SerializeField]
	private GameObject buttonContainer;

	private UIStateManager mUIStateManager;

	private IUIPause mIUIPause;

	protected override void OnInit(GameUIBundle bundle)
	{
		base.OnInit(bundle);
		SetListener(this);
		if (bundle != null)
		{
			mIUIPause = (IUIPause)bundle.Get("key_hud_pause_quitgame");
			mUIStateManager = (UIStateManager)bundle.Get("key_hud_pause_UIStateManager");
		}
	}

	protected override void OnUIUpdate()
	{
		base.OnUIUpdate();
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameUIManager.GetInstance().LoadBountyHunter();
		}
	}

	public void OnTouch(GameUITouchEvent touchEvent)
	{
		switch (touchEvent.EventID)
		{
		case TouchEventID.HUD_Pause_Resume:
			mUIStateManager.FrGoToPhase(6, true, false, false);
			Time.timeScale = 1f;
			GameUIManager.GetInstance().RemoveUI(base.UIStatus);
			break;
		case TouchEventID.HUD_Pause_Restart:
		{
			Time.timeScale = 1f;
			Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
			if (player != null)
			{
				GameApp.GetInstance().GetUserState().Enegy = player.GetInitEnegy();
			}
			GameUIManager.GetInstance().RemoveAll();
			Application.LoadLevel(Application.loadedLevelName);
			break;
		}
		case TouchEventID.HUD_Pause_Option:
			GameUIManager.GetInstance().HideUI(this);
			mUIStateManager.FrGoToPhase(15, false, false, false);
			break;
		case TouchEventID.HUD_Pause_Exit:
			GameUIManager.GetInstance().HideUI(this);
			mIUIPause.QuitGame();
			break;
		}
	}

	protected override void OnHide()
	{
		base.OnHide();
		buttonContainer.SetActive(false);
	}

	protected override void OnShow()
	{
		base.OnShow();
		buttonContainer.SetActive(true);
	}
}
