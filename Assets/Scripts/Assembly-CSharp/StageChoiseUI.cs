using UnityEngine;

public class StageChoiseUI : UIHandler, IUIHandle
{
	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_HANDLE = 2;

	public UIStateManager stateMgr;

	private byte state;

	public static byte[] BG_IMG = new byte[2] { 0, 1 };

	private NavigationBarUI navigationBar;

	private NavigationMenuUI navigationMenu;

	private UIImage stageChoiseImg;

	private UIClickButton[] subStageBtn;

	private UIImage selectSubStageImg;

	private UIImage[] pageNavImg;

	private UIImage selectStagePageImg;

	private UISliderStage swapStage;

	private UIClickButton confirmBtn;

	private UIImage bgStageImg;

	public byte stageIdx;

	public byte subStageIdx;

	private static byte[] SUB_STAGE_LOCK_IMG = new byte[2] { 4, 5 };

	private static byte[] SUB_STAGE_UNLOCK_IMG = new byte[2] { 2, 3 };

	private static byte[] SUB_STAGE_SELECT_IMG = new byte[2] { 0, 1 };

	private static byte[] CONFIRM_NORMAL = new byte[2] { 26, 30 };

	private static byte[] CONFIRM_PRESSED = new byte[2] { 24, 29 };

	public StageChoiseUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
		state = 0;
	}

	public void Init()
	{
		state = 0;
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
	}

	public void Close()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
	}

	public void Create()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[3];
		stageIdx = GameApp.GetInstance().GetUserState().GetStage();
		subStageIdx = GameApp.GetInstance().GetUserState().GetSubStage();
		stageChoiseImg = new UIImage();
		stageChoiseImg.AddObject(unitUI, 0, BG_IMG);
		stageChoiseImg.Rect = stageChoiseImg.GetObjectRect();
		navigationBar = new NavigationBarUI(stateMgr);
		navigationBar.Create();
		navigationBar.SetTitle("LEVEL SELECT");
		navigationBar.Show();
		bgStageImg = new UIImage();
		bgStageImg.AddObject(unitUI, 9, stageIdx);
		bgStageImg.Rect = bgStageImg.GetObjectRect();
		bgStageImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth * 1.5f, UIConstant.ScreenLocalHeight * 1.5f));
		bgStageImg.SetColor(new Color(0.8f, 0.8f, 0.8f, 0.3f));
		swapStage = new UISliderStage();
		swapStage.Create(unitUI);
		ResetUIStage();
		stateMgr.m_UIManager.Add(stageChoiseImg);
		stateMgr.m_UIManager.Add(bgStageImg);
		stateMgr.m_UIManager.Add(navigationBar);
		stateMgr.m_UIManager.Add(swapStage);
		subStageBtn = new UIClickButton[Global.TOTAL_SUB_STAGE];
		for (int i = 0; i < subStageBtn.Length; i++)
		{
			subStageBtn[i] = new UIClickButton();
			stateMgr.m_UIManager.Add(subStageBtn[i]);
		}
		selectSubStageImg = new UIImage();
		stateMgr.m_UIManager.Add(selectSubStageImg);
		ResetUISubstage();
		pageNavImg = new UIImage[Global.TOTAL_SOLO_STAGE];
		int num = 32;
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 20);
		float num2 = modulePositionRect.x + modulePositionRect.width * 0.5f - (float)((Global.TOTAL_SOLO_STAGE - 1) * num) * 0.5f;
		for (int j = 0; j < pageNavImg.Length; j++)
		{
			pageNavImg[j] = new UIImage();
			pageNavImg[j].AddObject(unitUI, 0, 20);
			pageNavImg[j].Rect = new Rect(num2 - modulePositionRect.width * 0.5f, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
			num2 += (float)num;
			stateMgr.m_UIManager.Add(pageNavImg[j]);
		}
		selectStagePageImg = new UIImage();
		selectStagePageImg.AddObject(unitUI, 0, 23);
		selectStagePageImg.Rect = pageNavImg[stageIdx].Rect;
		stateMgr.m_UIManager.Add(selectStagePageImg);
		confirmBtn = new UIClickButton();
		confirmBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, CONFIRM_NORMAL);
		confirmBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, CONFIRM_PRESSED);
		confirmBtn.Rect = confirmBtn.GetObjectRect(UIButtonBase.State.Normal);
		stateMgr.m_UIManager.Add(confirmBtn);
		navigationMenu = new NavigationMenuUI(stateMgr);
		navigationMenu.Create();
		navigationMenu.Show();
		stateMgr.m_UIPopupManager.Add(navigationMenu);
	}

	public void ResetBGStage()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[3];
		bgStageImg.SetTexture(ui, 9, stageIdx);
		bgStageImg.Rect = bgStageImg.GetObjectRect();
		bgStageImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth * 1.5f, UIConstant.ScreenLocalHeight * 1.5f));
		bgStageImg.SetColor(new Color(0.8f, 0.8f, 0.8f, 0.3f));
	}

	public void ResetUIStage()
	{
		swapStage.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[3];
		for (int i = 0; i < Global.TOTAL_SOLO_STAGE; i++)
		{
			UISliderStage.UIStageIcon uIStageIcon = new UISliderStage.UIStageIcon();
			uIStageIcon.m_background = new UIClickButton();
			uIStageIcon.m_background.AddObject(UIButtonBase.State.Normal, unitUI, 8, i);
			uIStageIcon.m_background.AddObject(UIButtonBase.State.Pressed, unitUI, 8, i);
			uIStageIcon.m_background.Rect = unitUI.GetModulePositionRect(0, 0, 2);
			uIStageIcon.m_Lock = new UIImage();
			if (i < Global.TOTAL_STAGE && GameApp.GetInstance().GetUserState().GetStageState()[i, 0] == 0)
			{
				uIStageIcon.m_Lock.AddObject(unitUI, 0, 28);
				uIStageIcon.m_Lock.Rect = uIStageIcon.m_Lock.GetObjectRect();
				uIStageIcon.m_bLock = true;
			}
			uIStageIcon.m_level = new UIImage();
			uIStageIcon.m_bLevel = false;
			if (i >= Global.TOTAL_STAGE)
			{
				uIStageIcon.m_bLevel = true;
				uIStageIcon.m_level = new UIImage();
				uIStageIcon.m_level.AddObject(unitUI, 0, 31);
				uIStageIcon.m_level.Rect = uIStageIcon.m_level.GetObjectRect();
			}
			uIStageIcon.Id = i;
			uIStageIcon.Rect = uIStageIcon.m_background.Rect;
			uIStageIcon.Add(uIStageIcon.m_background);
			uIStageIcon.Add(uIStageIcon.m_Lock);
			uIStageIcon.Add(uIStageIcon.m_level);
			uIStageIcon.Show();
			swapStage.Add(uIStageIcon);
		}
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 2);
		Rect rect = new Rect(0f, modulePositionRect.y, UIConstant.ScreenLocalWidth, modulePositionRect.height);
		swapStage.SetClipRect(rect);
		float num = modulePositionRect.width - 120f;
		swapStage.SetScroller(0f, num * (float)Global.TOTAL_SOLO_STAGE, num, rect);
		swapStage.SetSelection(stageIdx);
	}

	public void ResetUISubstage()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[3];
		bool flag = false;
		if (stageIdx < Global.TOTAL_STAGE)
		{
			for (int i = 0; i < subStageBtn.Length; i++)
			{
				Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 12 + i);
				byte[] array = new byte[2];
				flag = GameApp.GetInstance().GetUserState().GetStageState(stageIdx, i) == 1;
				if (i == Global.TOTAL_SUB_STAGE - 1)
				{
					subStageBtn[i].SetTexture(UIButtonBase.State.Normal, unitUI, i + 1, 1);
					subStageBtn[i].SetTexture(UIButtonBase.State.Pressed, unitUI, i + 1, 0);
					subStageBtn[i].SetTexture(UIButtonBase.State.Disabled, unitUI, i + 1, 2);
					subStageBtn[i].Rect = modulePositionRect;
				}
				else
				{
					subStageBtn[i].SetTexture(UIButtonBase.State.Normal, unitUI, i + 1, SUB_STAGE_UNLOCK_IMG);
					subStageBtn[i].SetTexture(UIButtonBase.State.Pressed, unitUI, i + 1, SUB_STAGE_SELECT_IMG);
					subStageBtn[i].SetTexture(UIButtonBase.State.Disabled, unitUI, i + 1, SUB_STAGE_LOCK_IMG);
					subStageBtn[i].Rect = modulePositionRect;
				}
				if (!flag)
				{
					subStageBtn[i].Enable = false;
				}
				else
				{
					subStageBtn[i].Enable = true;
				}
				subStageBtn[i].Visible = true;
			}
			if (subStageIdx == Global.TOTAL_SUB_STAGE - 1)
			{
				selectSubStageImg.SetTexture(unitUI, subStageIdx + 1, 0);
			}
			else
			{
				selectSubStageImg.SetTexture(unitUI, subStageIdx + 1, SUB_STAGE_SELECT_IMG);
			}
			selectSubStageImg.Rect = subStageBtn[subStageIdx].Rect;
			selectSubStageImg.Visible = true;
		}
		else
		{
			for (int j = 0; j < subStageBtn.Length; j++)
			{
				subStageBtn[j].Visible = false;
			}
			selectSubStageImg.Visible = false;
		}
	}

	public bool Update()
	{
		switch (state)
		{
		case 0:
			Create();
			state = 2;
			break;
		case 2:
		{
			UITouchInner[] array = iPhoneInputMgr.MockTouches();
			foreach (UITouchInner touch in array)
			{
				if (!(stateMgr.m_UIManager != null) || stateMgr.m_UIManager.HandleInput(touch))
				{
				}
			}
			if (swapStage.GetScrollerState())
			{
				for (int j = 0; j < subStageBtn.Length; j++)
				{
					subStageBtn[j].Visible = false;
				}
				selectSubStageImg.Visible = false;
			}
			else if (stageIdx < Global.TOTAL_STAGE)
			{
				for (int k = 0; k < subStageBtn.Length; k++)
				{
					subStageBtn[k].Visible = true;
				}
				if (GameApp.GetInstance().GetUserState().GetStageState()[stageIdx, subStageIdx] == 0)
				{
					selectSubStageImg.Visible = false;
				}
				else
				{
					selectSubStageImg.Visible = true;
				}
			}
			break;
		}
		}
		return false;
	}

	public void StartGame(int mapId)
	{
		stateMgr.FrFree();
		if (mapId < Global.TOTAL_STAGE)
		{
			if (subStageIdx == Global.TOTAL_SUB_STAGE - 1)
			{
				GameApp.GetInstance().GetGameMode().ModePlay = Mode.Survival;
			}
			else
			{
				GameApp.GetInstance().GetGameMode().ModePlay = Mode.CamPain;
			}
		}
		else
		{
			GameApp.GetInstance().GetGameMode().ModePlay = Mode.Boss;
		}
		if (mapId == Global.TOTAL_STAGE)
		{
			mapId = 2;
		}
		else if (mapId == Global.TOTAL_STAGE + 1)
		{
			mapId = 4;
		}
		else if (mapId == Global.TOTAL_STAGE + 2)
		{
			mapId = 3;
		}
		else if (mapId == Global.TOTAL_STAGE + 3)
		{
			mapId = 5;
		}
		GameObject gameObject = GameObject.Find("MenuMusic");
		if (gameObject != null)
		{
			Object.DestroyObject(gameObject);
		}
		Application.LoadLevel("Level" + (mapId + 1));
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		bool flag = true;
		if (control == navigationBar)
		{
			((SoloMenuScript)stateMgr).FrFree();
			Application.LoadLevel("StartMenu");
			return;
		}
		if (control == navigationMenu)
		{
			switch (command)
			{
			case 1:
				stateMgr.m_UIManager.SetEnable(true);
				break;
			case 0:
				stateMgr.m_UIManager.SetEnable(false);
				break;
			case 2:
				stateMgr.FrGoToPhase(4, false, false, false);
				break;
			case 3:
				stateMgr.FrGoToPhase(3, false, false, false);
				break;
			case 4:
				stateMgr.FrGoToPhase(8, false, false, false);
				break;
			case 5:
				stateMgr.FrGoToPhase(11, false, false, false);
				break;
			}
			return;
		}
		if (control == swapStage)
		{
			switch (command)
			{
			case 1:
				AudioManager.GetInstance().PlaySound(AudioName.SWITCH_ITEM);
				stageIdx = (byte)wparam;
				ResetUISubstage();
				selectStagePageImg.Rect = pageNavImg[stageIdx].Rect;
				ResetBGStage();
				break;
			case 0:
				if (stageIdx >= Global.TOTAL_STAGE || GameApp.GetInstance().GetUserState().GetStageState()[stageIdx, subStageIdx] == 1 || !flag)
				{
					AudioManager.GetInstance().PlaySound(AudioName.CLICK);
					GameApp.GetInstance().GetUserState().SetStage(stageIdx);
					GameApp.GetInstance().GetUserState().SetSubStage(subStageIdx);
					GameApp.GetInstance().Save();
					StartGame(stageIdx);
				}
				break;
			}
			return;
		}
		if (control == confirmBtn)
		{
			if (stageIdx >= Global.TOTAL_STAGE || GameApp.GetInstance().GetUserState().GetStageState()[stageIdx, subStageIdx] == 1 || !flag)
			{
				AudioManager.GetInstance().PlaySound(AudioName.CLICK);
				GameApp.GetInstance().GetUserState().SetStage(stageIdx);
				GameApp.GetInstance().GetUserState().SetSubStage(subStageIdx);
				GameApp.GetInstance().Save();
				StartGame(stageIdx);
			}
			return;
		}
		for (int i = 0; i < subStageBtn.Length; i++)
		{
			if (subStageBtn[i] != control)
			{
				continue;
			}
			if (stageIdx < Global.TOTAL_STAGE && GameApp.GetInstance().GetUserState().GetStageState()[stageIdx, i] != 1 && flag)
			{
				break;
			}
			if (i == subStageIdx)
			{
				AudioManager.GetInstance().PlaySound(AudioName.CLICK);
				GameApp.GetInstance().GetUserState().SetStage(stageIdx);
				GameApp.GetInstance().GetUserState().SetSubStage(subStageIdx);
				StartGame(stageIdx);
				break;
			}
			UnitUI ui = Res2DManager.GetInstance().vUI[3];
			subStageIdx = (byte)i;
			if (subStageIdx == Global.TOTAL_SUB_STAGE - 1)
			{
				selectSubStageImg.SetTexture(ui, subStageIdx + 1, 0);
			}
			else
			{
				selectSubStageImg.SetTexture(ui, subStageIdx + 1, SUB_STAGE_SELECT_IMG);
			}
			selectSubStageImg.Rect = subStageBtn[subStageIdx].Rect;
			break;
		}
	}
}
