using UnityEngine;

public class ExtraUI : UIHandler, IUIHandle
{
	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_HANDLE = 2;

	public UIStateManager stateMgr;

	private UserState userState;

	private byte state;

	public static byte[] BG_IMG = new byte[2] { 0, 1 };

	private NavigationBarUI navigationBar;

	private NavigationMenuUI navigationMenu;

	private UIImage extraImg;

	private static byte[] ACHIEVEMENT_NORMAL = new byte[4] { 5, 6, 7, 8 };

	private static byte[] ACHIEVEMENT_PRESSED = new byte[4] { 2, 3, 4, 8 };

	private static byte[] LEADBOARD_NORMAL = new byte[4] { 12, 13, 14, 15 };

	private static byte[] LEADBOARD_PRESSED = new byte[4] { 9, 10, 11, 15 };

	private UIClickButton achievementBtn;

	private UIClickButton leadBoardBtn;

	public ExtraUI(UIStateManager stateMgr)
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
		userState = GameApp.GetInstance().GetUserState();
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
		UnitUI ui = Res2DManager.GetInstance().vUI[7];
		extraImg = new UIImage();
		extraImg.AddObject(ui, 0, BG_IMG);
		extraImg.Rect = extraImg.GetObjectRect();
		achievementBtn = new UIClickButton();
		achievementBtn.AddObject(UIButtonBase.State.Normal, ui, 0, ACHIEVEMENT_NORMAL);
		achievementBtn.AddObject(UIButtonBase.State.Pressed, ui, 0, ACHIEVEMENT_PRESSED);
		achievementBtn.Rect = achievementBtn.GetObjectRect(UIButtonBase.State.Normal);
		leadBoardBtn = new UIClickButton();
		leadBoardBtn.AddObject(UIButtonBase.State.Normal, ui, 0, LEADBOARD_NORMAL);
		leadBoardBtn.AddObject(UIButtonBase.State.Pressed, ui, 0, LEADBOARD_PRESSED);
		leadBoardBtn.Rect = leadBoardBtn.GetObjectRect(UIButtonBase.State.Normal);
		navigationBar = new NavigationBarUI(stateMgr);
		navigationBar.Create();
		navigationBar.SetTitle("EXTRA");
		navigationBar.Show();
		stateMgr.m_UIManager.Add(extraImg);
		stateMgr.m_UIManager.Add(navigationBar);
		stateMgr.m_UIManager.Add(achievementBtn);
		stateMgr.m_UIManager.Add(leadBoardBtn);
		navigationMenu = new NavigationMenuUI(stateMgr);
		navigationMenu.Create();
		navigationMenu.Show();
		stateMgr.m_UIPopupManager.Add(navigationMenu);
	}

	public bool Update()
	{
		switch (state)
		{
		case 0:
			state = 2;
			Create();
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
			break;
		}
		}
		return false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == navigationBar)
		{
			if (Application.loadedLevelName.Equals("ShopAndCustomize"))
			{
				stateMgr.FrFree();
				UIConstant.ExitShopAndCustomize();
			}
			else
			{
				stateMgr.FrGoToPhase(stateMgr.FrGetPreviousPhase(), false, false, false);
			}
		}
		else if (control == navigationMenu)
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
				stateMgr.FrGoToPhase(4, false, false, true);
				break;
			case 3:
				stateMgr.FrGoToPhase(3, false, false, true);
				break;
			case 4:
				stateMgr.FrGoToPhase(8, false, false, true);
				break;
			}
		}
	}
}
