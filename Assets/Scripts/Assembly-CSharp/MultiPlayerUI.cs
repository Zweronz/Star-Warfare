using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MultiPlayerUI : UIHandler, IUIHandle
{
	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_HANDLE = 2;

	public UIStateManager stateMgr;

	private byte state;

	private static byte[] BG_IMG = new byte[2] { 0, 1 };

	private NavigationBarUI navigationBar;

	private NavigationMenuUI navigationMenu;

	private UIImage multiPlayerImg;

	private UITextButton createBtn;

	private UITextButton joinBtn;

	private UIClickButton refreshBtn;

	private static byte[] REFRESH_NORMAL = new byte[2] { 27, 29 };

	private static byte[] REFRESH_PRESSED = new byte[2] { 26, 28 };

	private UITextButton quickJoinBtn;

	private static byte[] SEARCH_NORMAL = new byte[2] { 17, 19 };

	private static byte[] SEARCH_ACTIVE = new byte[2] { 16, 18 };

	private UIImage searchImg;

	private Rect clearTextRect;

	private UIListRoom listRoom;

	private CreateRoomUI createRoom;

	private PasswordUI inputPassword;

	private RoomSearchUI roomSearch;

	private UISliderTab roomCategory;

	private byte lastCategoryIndex;

	private static byte BG_IMG_ROOM_CATEGORY = 36;

	private NetLoadingUI netLoadingUI;

	private string m_strSearch = string.Empty;

	private Rect m_searchRect;

	private string m_prevStrSearch = string.Empty;

	private UITextButton detailSearchBtn;

	private DateTime time;

	private DateTime refreshBtnTime;

	private bool bHideGUI;

	private MessageBoxUI msgUI;

	private bool m_advancedSearch;

	public MultiPlayerUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
		state = 0;
	}

	public void Init()
	{
		state = 0;
		bHideGUI = false;
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
	}

	public void Close()
	{
		if (listRoom != null)
		{
			listRoom.Clear();
		}
		if (createRoom != null)
		{
			createRoom.Clear();
		}
		if (inputPassword != null)
		{
			inputPassword.Clear();
		}
		if (roomSearch != null)
		{
			roomSearch.Clear();
		}
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
	}

	public bool Create()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		GameApp.GetInstance().GetGameMode().ModePlay = Mode.VS_CMI;
		UnitUI unitUI = Res2DManager.GetInstance().vUI[4];
		if (unitUI == null)
		{
			Debug.Log("this is error!");
			return false;
		}
		multiPlayerImg = new UIImage();
		multiPlayerImg.AddObject(unitUI, 0, BG_IMG);
		multiPlayerImg.Rect = multiPlayerImg.GetObjectRect();
		navigationBar = new NavigationBarUI(stateMgr);
		navigationBar.Create();
		navigationBar.SetTitle("MULTI-PLAYER");
		navigationBar.Show();
		createBtn = new UITextButton();
		createBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 3);
		createBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 2);
		createBtn.Rect = createBtn.GetObjectRect(UIButtonBase.State.Normal);
		createBtn.SetText("font2", "CREATE", UIConstant.fontColor_cyan);
		createBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		joinBtn = new UITextButton();
		joinBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 5);
		joinBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 4);
		joinBtn.Rect = joinBtn.GetObjectRect(UIButtonBase.State.Normal);
		joinBtn.SetText("font2", "JOIN", UIConstant.fontColor_cyan);
		joinBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		refreshBtn = new UIClickButton();
		refreshBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, REFRESH_NORMAL);
		refreshBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, REFRESH_PRESSED);
		refreshBtn.Rect = refreshBtn.GetObjectRect(UIButtonBase.State.Normal);
		m_advancedSearch = false;
		detailSearchBtn = new UITextButton();
		detailSearchBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 22);
		detailSearchBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 21);
		detailSearchBtn.Rect = detailSearchBtn.GetObjectRect(UIButtonBase.State.Normal);
		SetAdvancedBtnText();
		searchImg = new UIImage();
		searchImg.AddObject(unitUI, 0, 18);
		searchImg.Rect = searchImg.GetObjectRect();
		searchImg.Enable = true;
		clearTextRect = unitUI.GetModulePositionRect(0, 0, 17);
		clearTextRect.y = UIConstant.ScreenLocalHeight - clearTextRect.y - clearTextRect.height;
		clearTextRect = UIConstant.GetRectForScreenAdaptived(clearTextRect);
		quickJoinBtn = new UITextButton();
		quickJoinBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 7);
		quickJoinBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 6);
		quickJoinBtn.Rect = quickJoinBtn.GetObjectRect(UIButtonBase.State.Normal);
		quickJoinBtn.SetText("font2", "QUICK JOIN", UIConstant.fontColor_cyan, quickJoinBtn.Rect.width);
		quickJoinBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		listRoom = new UIListRoom();
		listRoom.Create(unitUI);
		ResetUIRoom(0);
		navigationMenu = new NavigationMenuUI(stateMgr);
		navigationMenu.Create();
		navigationMenu.Show();
		createRoom = new CreateRoomUI(stateMgr);
		createRoom.Create();
		createRoom.Hide();
		inputPassword = new PasswordUI(stateMgr);
		inputPassword.Create();
		inputPassword.Hide();
		roomSearch = new RoomSearchUI(stateMgr);
		roomSearch.Create();
		roomSearch.Hide();
		roomCategory = new UISliderTab();
		roomCategory.Create();
		InitRoomCategory();
		stateMgr.m_UIManager.Add(multiPlayerImg);
		stateMgr.m_UIManager.Add(navigationBar);
		stateMgr.m_UIManager.Add(roomCategory);
		stateMgr.m_UIManager.Add(createBtn);
		stateMgr.m_UIManager.Add(joinBtn);
		stateMgr.m_UIManager.Add(quickJoinBtn);
		stateMgr.m_UIManager.Add(listRoom);
		stateMgr.m_UIManager.Add(searchImg);
		stateMgr.m_UIManager.Add(refreshBtn);
		stateMgr.m_UIManager.Add(detailSearchBtn);
		stateMgr.m_UIPopupManager.Add(navigationMenu);
		stateMgr.m_UIPopupManager.Add(inputPassword);
		stateMgr.m_UIPopupManager.Add(createRoom);
		stateMgr.m_UIPopupManager.Add(roomSearch);
		msgUI = new MessageBoxUI(stateMgr);
		msgUI.Create();
		string msg = UIConstant.GetMessage(21).Replace("[n]", "\n");
		msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_RANK_DISMATCH_JOINROOM);
		msgUI.Hide();
		stateMgr.m_UIPopupManager.Add(msgUI);
		netLoadingUI = new NetLoadingUI(stateMgr);
		netLoadingUI.Create();
		netLoadingUI.Hide();
		stateMgr.m_UIPopupManager.Add(netLoadingUI);
		return true;
	}

	public void ResetUIRoom(byte recommendRoomNumber)
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[4];
		if (listRoom == null)
		{
			return;
		}
		listRoom.ClearRoomList();
		List<Room> roomList = Lobby.GetInstance().GetRoomList();
		for (int i = 0; i < 5; i++)
		{
			UIListRoom.UIRoomIcon uIRoomIcon = new UIListRoom.UIRoomIcon(listRoom);
			uIRoomIcon.m_background = new UIImage();
			uIRoomIcon.m_background.AddObject(ui, 0, 8 + i);
			uIRoomIcon.m_background.Rect = uIRoomIcon.m_background.GetObjectRect();
			uIRoomIcon.m_pingIcon = new UIImage();
			uIRoomIcon.m_mapIcon = new UIImage();
			uIRoomIcon.m_mapName = new FrUIText();
			uIRoomIcon.m_playerNum = new FrUIText();
			uIRoomIcon.m_pingValue = new FrUIText();
			uIRoomIcon.m_passwordIcon = new UIImage();
			uIRoomIcon.m_gameMode = new UIImage();
			uIRoomIcon.m_rankMaskIcon = new UIImage();
			uIRoomIcon.m_rankIcon = new List<UIImage>();
			uIRoomIcon.Id = i;
			uIRoomIcon.Rect = uIRoomIcon.m_background.Rect;
			if (i < recommendRoomNumber)
			{
				uIRoomIcon.m_highlightFrame = new UIImage();
				uIRoomIcon.m_highlightFrame.AddObject(ui, 0, 41);
				uIRoomIcon.m_highlightFrame.Rect = uIRoomIcon.Rect;
			}
			else
			{
				uIRoomIcon.m_highlightFrame = null;
			}
			listRoom.Add(uIRoomIcon);
			if (i < roomList.Count)
			{
				listRoom.SetRoomTexture(i, ui, roomList[i]);
			}
		}
	}

	public void RefreshRoomList(bool bNetLoad)
	{
		if (inputPassword != null && inputPassword.Visible)
		{
			return;
		}
		listRoom.SetSelection(-1);
		time = DateTime.Now;
		refreshBtnTime = DateTime.Now;
		string text = m_strSearch.ToLower().Trim();
		if (text == string.Empty)
		{
			if (m_advancedSearch)
			{
				SearchRoomAdvancedRequest request = new SearchRoomAdvancedRequest(roomSearch.GetOnlineMode(), GameApp.GetInstance().GetUserState().GetRank()
					.rankID, roomSearch.GetLowerRankID(), roomSearch.GetUpperRankID(), roomSearch.GetGameMode(), roomSearch.GetWinCondition(), roomSearch.GetWinValue(), roomSearch.GetIsBalance(), roomSearch.GetPlayerNumber());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			else
			{
				GetRoomListRequest request2 = new GetRoomListRequest(roomCategory.GetSelectIndex(), GameApp.GetInstance().GetUserState().GetRank()
					.rankID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
		else
		{
			SearchRoomRequest request3 = new SearchRoomRequest(text);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
		}
		if (bNetLoad)
		{
			netLoadingUI.Show(5);
		}
	}

	private void InitRoomCategory()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[4];
		roomCategory.Clear();
		roomCategory.SetBackground(ui, 0, BG_IMG_ROOM_CATEGORY);
		roomCategory.SetSlider(ui, 0, 40);
		roomCategory.SetClipRect(roomCategory.backgroundImg.Rect.x - 10f, roomCategory.backgroundImg.Rect.y, roomCategory.backgroundImg.Rect.width + 20f, roomCategory.backgroundImg.Rect.height);
		for (int i = 0; i < UIConstant.TOTAL_SEARCH_ROOM_CATEGORY; i++)
		{
			UITab uITab = new UITab();
			uITab.AddObject(UITab.State.Normal, ui, 0, 37 + i);
			uITab.AddObject(UITab.State.Selected, ui, 0, 37 + i);
			uITab.Rect = uITab.GetObjectRect(UITab.State.Normal);
			uITab.Enable = false;
			roomCategory.Add(uITab);
		}
		roomCategory.SetScroller(UIScroller.ScrollerDir.Vertical, 0f, 161f, 80f, roomCategory.backgroundImg.Rect);
		roomCategory.m_scroller.DeltaTime = 0.016f;
		roomCategory.SetSelection(0);
		lastCategoryIndex = 0;
	}

	public void SetAdvancedBtnText()
	{
		if (!m_advancedSearch)
		{
			detailSearchBtn.SetText("font2", "ADVANCED", UIConstant.fontColor_cyan);
			detailSearchBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		}
		else
		{
			detailSearchBtn.SetText("font2", "DEFAULT", UIConstant.fontColor_cyan);
			detailSearchBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		}
	}

	public void ShowPromotion()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		if (userState != null && userState.showPromotion && !msgUI.IsVisiable())
		{
			msgUI.CreateQuery(userState.m_promotion.m_msg, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_STORE_PROMOTION);
			msgUI.Show();
			userState.showPromotion = false;
		}
	}

	public bool Update()
	{
		switch (state)
		{
		case 0:
			if (Create())
			{
				RefreshRoomList(false);
				state = 2;
			}
			break;
		case 2:
		{
			UITouchInner[] array = iPhoneInputMgr.MockTouches();
			foreach (UITouchInner touch in array)
			{
				if (stateMgr.m_UIManager != null)
				{
					if (createRoom.Visible || inputPassword.Visible || roomSearch.Visible || navigationMenu.IsWorkingIAP())
					{
						break;
					}
					if (!stateMgr.m_UIManager.HandleInput(touch))
					{
					}
				}
			}
			if (!m_strSearch.Equals(m_prevStrSearch))
			{
				if (m_advancedSearch)
				{
					m_advancedSearch = false;
					roomSearch.ResetAllConditions();
					SetAdvancedBtnText();
				}
				else if (m_strSearch != string.Empty)
				{
					roomCategory.ShowSlider(false);
				}
				else
				{
					roomCategory.ShowSlider(true);
				}
				m_prevStrSearch = m_strSearch;
				RefreshRoomList(true);
			}
			if ((DateTime.Now - time).TotalSeconds >= 15.0)
			{
				RefreshRoomList(false);
			}
			ShowPromotion();
			break;
		}
		}
		return false;
	}

	public void OnGUI()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[4];
		if (unitUI == null || bHideGUI || (navigationMenu != null && (navigationMenu.m_state == 1 || navigationMenu.IsWorkingIAP())) || (netLoadingUI != null && netLoadingUI.IsVisiableMessage()))
		{
			return;
		}
		if (createRoom != null && createRoom.Visible)
		{
			createRoom.OnGUI();
		}
		else
		{
			if ((inputPassword != null && inputPassword.Visible) || (roomSearch != null && roomSearch.Visible) || (msgUI != null && msgUI.Visible) || listRoom == null || !listRoom.Visible || listRoom.m_roomLst == null)
			{
				return;
			}
			MultiMenuScript multiMenuScript = (MultiMenuScript)stateMgr;
			for (int i = 0; i < listRoom.m_roomLst.Count; i++)
			{
				UIListRoom.UIRoomIcon uIRoomIcon = listRoom.m_roomLst[i];
				if (!uIRoomIcon.m_userName.Equals(string.Empty))
				{
					multiMenuScript.playerNameStyle.alignment = TextAnchor.UpperLeft;
					GUI.Label(uIRoomIcon.m_userNamePos, uIRoomIcon.m_userName, multiMenuScript.playerNameStyle);
				}
			}
			multiMenuScript.searchRoomStyle.padding.left = UIConstant.GetWidthForScreenAdaptived(10);
			multiMenuScript.searchRoomStyle.padding.bottom = UIConstant.GetHeightForScreenAdaptived(10);
			m_strSearch = GUI.TextField(clearTextRect, m_strSearch, 20, multiMenuScript.searchRoomStyle);
			m_strSearch = Regex.Replace(m_strSearch, "[\n\r]", string.Empty);
		}
	}

	public void HideNetLoading()
	{
		netLoadingUI.Hide();
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == navigationBar)
		{
			((MultiMenuScript)stateMgr).Logout();
			((MultiMenuScript)stateMgr).FrFree();
			Application.LoadLevel("StartMenu");
		}
		else if (control == createBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			createRoom.Show();
		}
		else if (control == createRoom)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			if (command == 0)
			{
				netLoadingUI.Show(10);
				bHideGUI = true;
			}
			createRoom.Hide();
		}
		else if (control == joinBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			List<Room> roomList = Lobby.GetInstance().GetRoomList();
			if (listRoom.m_selectIndex != -1 && listRoom.m_selectIndex < roomList.Count)
			{
				byte rankID = GameApp.GetInstance().GetUserState().GetRank()
					.rankID;
				if (rankID < roomList[listRoom.m_selectIndex].getMinJoinRankID())
				{
					string msg = UIConstant.GetMessage(33).Replace("[n]", "\n");
					msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_RANK_DISMATCH_JOINROOM);
					msgUI.Show();
					return;
				}
				if (rankID > roomList[listRoom.m_selectIndex].getMaxJoinRankID())
				{
					string msg2 = UIConstant.GetMessage(34).Replace("[n]", "\n");
					msgUI.CreateConfirm(msg2, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_RANK_DISMATCH_JOINROOM);
					msgUI.Show();
					return;
				}
				if (roomList[listRoom.m_selectIndex].isHasPassword())
				{
					stateMgr.m_UIManager.SetEnable(false);
					inputPassword.Init();
					inputPassword.Show();
					return;
				}
				netLoadingUI.Show(5);
				UploadArmorAndBagRequest request = new UploadArmorAndBagRequest(GameApp.GetInstance().GetUserState());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				JoinRoomRequest request2 = new JoinRoomRequest(roomList[listRoom.m_selectIndex].getRoomID(), rankID, TimeManager.GetInstance().Ping);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				Lobby.GetInstance().SetCurrentRoomID(roomList[listRoom.m_selectIndex].getRoomID());
				Lobby.GetInstance().SetCurrentRoomMapID(roomList[listRoom.m_selectIndex].getMapID());
			}
		}
		else if (control == quickJoinBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			UploadArmorAndBagRequest request3 = new UploadArmorAndBagRequest(GameApp.GetInstance().GetUserState());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
			Rank rank = GameApp.GetInstance().GetUserState().GetRank();
			QuickJoinRequest request4 = new QuickJoinRequest(rank.rankID, TimeManager.GetInstance().Ping);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request4);
			netLoadingUI.Show(5);
		}
		else if (control == refreshBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			if ((DateTime.Now - refreshBtnTime).TotalSeconds >= 1.0)
			{
				RefreshRoomList(true);
			}
		}
		else if (control == listRoom)
		{
			int num = (int)wparam;
			List<Room> roomList2 = Lobby.GetInstance().GetRoomList();
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			if (num >= roomList2.Count)
			{
				return;
			}
			if (listRoom.m_selectIndex != num)
			{
				listRoom.SetSelection(num);
				return;
			}
			byte rankID2 = GameApp.GetInstance().GetUserState().GetRank()
				.rankID;
			if (rankID2 < roomList2[num].getMinJoinRankID())
			{
				string msg3 = UIConstant.GetMessage(33).Replace("[n]", "\n");
				msgUI.CreateConfirm(msg3, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_RANK_DISMATCH_JOINROOM);
				msgUI.Show();
				return;
			}
			if (rankID2 > roomList2[num].getMaxJoinRankID())
			{
				string msg4 = UIConstant.GetMessage(34).Replace("[n]", "\n");
				msgUI.CreateConfirm(msg4, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_RANK_DISMATCH_JOINROOM);
				msgUI.Show();
				return;
			}
			if (roomList2[num].isHasPassword())
			{
				stateMgr.m_UIManager.SetEnable(false);
				inputPassword.Init();
				inputPassword.Show();
				return;
			}
			netLoadingUI.Show(5);
			UploadArmorAndBagRequest request5 = new UploadArmorAndBagRequest(GameApp.GetInstance().GetUserState());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request5);
			JoinRoomRequest request6 = new JoinRoomRequest(roomList2[num].getRoomID(), rankID2, TimeManager.GetInstance().Ping);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request6);
			Lobby.GetInstance().SetCurrentRoomID(roomList2[num].getRoomID());
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
			}
			switch (command)
			{
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
		}
		else if (control == searchImg)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			m_strSearch = string.Empty;
			m_prevStrSearch = string.Empty;
		}
		else if (control == inputPassword)
		{
			stateMgr.m_UIManager.SetEnable(true);
			switch (command)
			{
			case 0:
			{
				List<Room> roomList3 = Lobby.GetInstance().GetRoomList();
				if (listRoom.m_selectIndex != -1 && listRoom.m_selectIndex < roomList3.Count && inputPassword.Verify(roomList3[listRoom.m_selectIndex].getRoomPassword()))
				{
					inputPassword.Hide();
					netLoadingUI.Show(5);
					UploadArmorAndBagRequest request7 = new UploadArmorAndBagRequest(GameApp.GetInstance().GetUserState());
					GameApp.GetInstance().GetNetworkManager().SendRequest(request7);
					byte rankID3 = GameApp.GetInstance().GetUserState().GetRank()
						.rankID;
					JoinRoomRequest request8 = new JoinRoomRequest(roomList3[listRoom.m_selectIndex].getRoomID(), rankID3, TimeManager.GetInstance().Ping);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request8);
					Lobby.GetInstance().SetCurrentRoomID(roomList3[listRoom.m_selectIndex].getRoomID());
				}
				else
				{
					inputPassword.ShowErrorMsg();
				}
				break;
			}
			case 1:
				inputPassword.Hide();
				break;
			}
		}
		else if (control == detailSearchBtn)
		{
			if (!m_advancedSearch)
			{
				roomSearch.Show();
			}
			else
			{
				m_advancedSearch = false;
				roomSearch.ResetAllConditions();
				SetAdvancedBtnText();
				roomCategory.ShowSlider(true);
				RefreshRoomList(true);
			}
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
		}
		else if (control == roomSearch)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			if (command == 0)
			{
				m_advancedSearch = true;
				SetAdvancedBtnText();
				m_prevStrSearch = string.Empty;
				m_strSearch = string.Empty;
				roomCategory.ShowSlider(false);
				RefreshRoomList(true);
			}
			roomSearch.Hide();
		}
		else if (control == roomCategory)
		{
			if (m_advancedSearch)
			{
				m_advancedSearch = false;
				roomSearch.ResetAllConditions();
				SetAdvancedBtnText();
				roomCategory.ShowSlider(true);
				RefreshRoomList(true);
			}
			else if (m_strSearch != string.Empty)
			{
				m_prevStrSearch = string.Empty;
				m_strSearch = string.Empty;
				roomCategory.ShowSlider(true);
				RefreshRoomList(true);
			}
			else if (roomCategory.GetSelectIndex() != lastCategoryIndex)
			{
				lastCategoryIndex = roomCategory.GetSelectIndex();
				RefreshRoomList(true);
			}
		}
		else
		{
			if (control != msgUI)
			{
				return;
			}
			int eventID = msgUI.GetEventID();
			if (eventID == MessageBoxUI.EVENT_RANK_DISMATCH_JOINROOM)
			{
				if (command == 9)
				{
					msgUI.Hide();
				}
			}
			else if (eventID == MessageBoxUI.EVENT_STORE_PROMOTION)
			{
				switch (command)
				{
				case 10:
					Debug.Log("Go to Store");
					msgUI.Hide();
					stateMgr.FrGoToPhase(4, false, false, false);
					break;
				case 9:
					msgUI.Hide();
					break;
				}
			}
		}
	}
}
