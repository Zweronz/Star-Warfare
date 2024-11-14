using System;
using System.Collections.Generic;
using UnityEngine;

public class PropsStoreUI : UIHandler, IUIHandle
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

	private UIImage propsStoreImg;

	private UITextButton[] categoryTxtBtn;

	private UIImage selectCategoryImg;

	private List<List<UIImage>> pageNavImg = new List<List<UIImage>>();

	private UIImage selectPropsPageImg;

	private UISliderProps swapProps;

	private byte curType;

	private string[] mPropsDescriptions;

	private MessageBoxUI msgUI;

	private IAPUI iapUI;

	public PropsStoreUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
		state = 0;
	}

	public void Init()
	{
		state = 0;
		userState = GameApp.GetInstance().GetUserState();
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
		string[] gameText = Res2DManager.GetInstance().GetGameText();
		if (gameText.Length > 3)
		{
			mPropsDescriptions = Res2DManager.GetInstance().SplitString(gameText[3]);
		}
	}

	public void Close()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		mPropsDescriptions = null;
	}

	public void Create()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[13];
		propsStoreImg = new UIImage();
		propsStoreImg.AddObject(unitUI, 0, BG_IMG);
		propsStoreImg.Rect = propsStoreImg.GetObjectRect();
		navigationBar = new NavigationBarUI(stateMgr);
		navigationBar.Create();
		navigationBar.SetTitle("STORE");
		navigationBar.Show();
		stateMgr.m_UIManager.Add(propsStoreImg);
		stateMgr.m_UIManager.Add(navigationBar);
		categoryTxtBtn = new UITextButton[Global.TOTAL_ITEM_CATEGORY_NUM];
		for (int i = 0; i < categoryTxtBtn.Length; i++)
		{
			categoryTxtBtn[i] = new UITextButton();
			categoryTxtBtn[i].AddObject(UIButtonBase.State.Normal, unitUI, 0, 3 + i);
			categoryTxtBtn[i].AddObject(UIButtonBase.State.Pressed, unitUI, 0, 3 + i);
			categoryTxtBtn[i].Rect = categoryTxtBtn[i].GetObjectRect(UIButtonBase.State.Normal);
			categoryTxtBtn[i].SetText("font3", UIConstant.ITEM_CATEGORY[i], UIConstant.fontColor_cyan);
			categoryTxtBtn[i].SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
			stateMgr.m_UIManager.Add(categoryTxtBtn[i]);
		}
		selectCategoryImg = new UIImage();
		selectCategoryImg.AddObject(unitUI, 0, 6);
		selectCategoryImg.Rect = categoryTxtBtn[curType].Rect;
		stateMgr.m_UIManager.Add(selectCategoryImg);
		pageNavImg.Clear();
		int num = 24;
		pageNavImg = new List<List<UIImage>>();
		for (int j = 0; j < Global.TOTAL_ITEM_CATEGORY_NUM; j++)
		{
			Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 15);
			float num2 = modulePositionRect.y + modulePositionRect.height * 0.5f + (float)((Global.TOTAL_ITEM_CATEGORY[j] - 1) * num) * 0.5f;
			List<UIImage> list = new List<UIImage>();
			for (int k = 0; k < Global.TOTAL_ITEM_CATEGORY[j]; k++)
			{
				UIImage uIImage = new UIImage();
				uIImage.AddObject(unitUI, 0, 15);
				uIImage.Rect = new Rect(modulePositionRect.x, num2 - modulePositionRect.height * 0.5f, modulePositionRect.width, modulePositionRect.height);
				num2 -= (float)num;
				list.Add(uIImage);
			}
			pageNavImg.Add(list);
		}
		for (int l = 0; l < pageNavImg.Count; l++)
		{
			for (int m = 0; m < pageNavImg[l].Count; m++)
			{
				stateMgr.m_UIManager.Add(pageNavImg[l][m]);
			}
		}
		swapProps = new UISliderProps();
		swapProps.Create(unitUI);
		SetProps(curType);
		stateMgr.m_UIManager.Add(swapProps);
		selectPropsPageImg = new UIImage();
		selectPropsPageImg.AddObject(unitUI, 0, 16);
		selectPropsPageImg.Rect = pageNavImg[curType][0].Rect;
		stateMgr.m_UIManager.Add(selectPropsPageImg);
		navigationMenu = new NavigationMenuUI(stateMgr);
		navigationMenu.Create();
		navigationMenu.Show();
		stateMgr.m_UIPopupManager.Add(navigationMenu);
		msgUI = new MessageBoxUI(stateMgr);
		msgUI.Create();
		msgUI.Hide();
		stateMgr.m_UIPopupManager.Add(msgUI);
		iapUI = new IAPUI(stateMgr);
		stateMgr.m_UIPopupManager.Add(iapUI);
	}

	public void SetProps(byte type)
	{
		curType = type;
		swapProps.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[13];
		UnitUI ui = Res2DManager.GetInstance().vUI[20];
		UnitUI ui2 = Res2DManager.GetInstance().vUI[17];
		for (int i = 0; i < pageNavImg.Count; i++)
		{
			for (int j = 0; j < pageNavImg[i].Count; j++)
			{
				pageNavImg[i][j].Visible = false;
				if (type == i)
				{
					pageNavImg[i][j].Visible = true;
				}
			}
		}
		int[] array = null;
		switch (type)
		{
		case 0:
			array = Global.ITEM_HP;
			break;
		case 1:
			array = Global.ITEM_REVIVAL;
			break;
		case 2:
			array = Global.ITEM_ASSIST;
			break;
		}
		for (int k = 0; k < array.Length; k++)
		{
			UISliderProps.UIPropsBoxIcon uIPropsBoxIcon = new UISliderProps.UIPropsBoxIcon(swapProps);
			int num = array[k];
			Item item = userState.GetItem()[num];
			uIPropsBoxIcon.m_background.AddObject(unitUI, 0, 7);
			uIPropsBoxIcon.m_background.Rect = uIPropsBoxIcon.m_background.GetObjectRect();
			uIPropsBoxIcon.m_plateImg.AddObject(unitUI, 0, 12);
			uIPropsBoxIcon.m_plateImg.Rect = uIPropsBoxIcon.m_plateImg.GetObjectRect();
			uIPropsBoxIcon.m_propsImg.AddObject(ui, 1, (int)(item.ItemID - 81));
			uIPropsBoxIcon.m_propsImg.Rect = uIPropsBoxIcon.m_plateImg.Rect;
			uIPropsBoxIcon.m_buyBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 11);
			uIPropsBoxIcon.m_buyBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 10);
			uIPropsBoxIcon.m_buyBtn.Rect = uIPropsBoxIcon.m_buyBtn.GetObjectRect(UIButtonBase.State.Normal);
			if (item.Mithril == 0)
			{
				uIPropsBoxIcon.m_buyBtn.SetNumeric(ui2, 1, "$" + item.Price, -5f);
			}
			else
			{
				uIPropsBoxIcon.m_buyBtn.SetNumeric(ui2, 1, "#" + item.Mithril, -5f);
			}
			uIPropsBoxIcon.m_buyBtn.SetTextColor(UIConstant.fontColor_white, UIConstant.fontColor_cyan);
			uIPropsBoxIcon.m_nameImg.AddObject(unitUI, 1, (int)(item.ItemID - 81));
			uIPropsBoxIcon.m_nameImg.Rect = uIPropsBoxIcon.m_nameImg.GetObjectRect();
			uIPropsBoxIcon.m_nameWidth = (int)uIPropsBoxIcon.m_nameImg.Rect.width;
			uIPropsBoxIcon.m_nameHeight = (int)uIPropsBoxIcon.m_nameImg.Rect.height;
			string text = mPropsDescriptions[(int)(item.ItemID - 81)];
			uIPropsBoxIcon.m_description.AlignStyle = FrUIText.enAlignStyle.TOP_LEFT;
			uIPropsBoxIcon.m_description.Rect = unitUI.GetModulePositionRect(0, 0, 13);
			List<Effect> effect = item.itemEffect.GetEffect();
			foreach (Effect item2 in effect)
			{
				string text2 = string.Empty;
				if (item.itemEffect.HasSign(item2.effectType) && item2.data > 0f)
				{
					text2 += "+";
				}
				if (item.itemEffect.IsPercetage(item2.effectType))
				{
					float num2 = item2.data * 100f;
					text2 = text2 + num2 + "%";
				}
				else
				{
					text2 += Convert.ToString(item2.data);
				}
				text = text.Replace(UIConstant.PROPS_KEY_WORD[(int)item2.effectType], text2);
			}
			text = text.Replace(UIConstant.KEEP_TIME_STRING, Convert.ToString(item.Duration));
			uIPropsBoxIcon.m_description.AlignStyle = FrUIText.enAlignStyle.TOP_LEFT;
			uIPropsBoxIcon.m_description.Set("font3", text, UIConstant.FONT_COLOR_DESCRIPTION, uIPropsBoxIcon.m_description.Rect.width);
			uIPropsBoxIcon.m_buyBtn.Id = num;
			uIPropsBoxIcon.Id = num;
			uIPropsBoxIcon.Rect = uIPropsBoxIcon.m_background.Rect;
			uIPropsBoxIcon.Show();
			swapProps.Add(uIPropsBoxIcon);
		}
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 2);
		swapProps.SetClipRect(modulePositionRect);
		float num3 = modulePositionRect.height / 3f;
		swapProps.SetScroller(0f, num3 * (float)(Global.TOTAL_ITEM_CATEGORY[type] - 1), num3, modulePositionRect);
		swapProps.SetSelection(0);
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
			break;
		}
		}
		return false;
	}

	public Item GetItem(int index)
	{
		return GameApp.GetInstance().GetUserState().GetItem()[index];
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == navigationBar)
		{
			stateMgr.FrGoToPhase(4, false, false, true);
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
				stateMgr.FrGoToPhase(4, false, false, true);
				break;
			case 3:
				stateMgr.FrGoToPhase(3, false, false, true);
				break;
			case 4:
				stateMgr.FrGoToPhase(8, false, false, true);
				break;
			case 5:
				stateMgr.FrGoToPhase(11, false, false, true);
				break;
			}
			return;
		}
		if (control == swapProps)
		{
			switch (command)
			{
			case 1:
			{
				int index2 = (int)wparam;
				selectPropsPageImg.Rect = pageNavImg[curType][index2].Rect;
				break;
			}
			case 0:
			{
				int index = (int)wparam;
				Item item = GetItem(index);
				if (item.Mithril == 0)
				{
					if (userState.GetCash() >= item.Price)
					{
						if (userState.GetNumFromStorage((int)(item.ItemID + 1)) + userState.GetPropsNumFromBag((int)item.ItemID) < 99)
						{
							userState.Buy(item.Price);
							userState.InsertPropsToStorage((int)(item.ItemID + 1), 1);
						}
						else
						{
							string msg = UIConstant.GetMessage(31).Replace("[n]", "\n");
							msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_PURCHASE_PROPS_TO_MANY);
							msgUI.Show();
						}
					}
					else
					{
						msgUI.CreateQuery(UIConstant.GetMessage(12), MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_PURCHASE_CASH);
						msgUI.Show();
					}
				}
				else if (userState.GetMithril() >= item.Mithril)
				{
					if (userState.GetNumFromStorage((int)(item.ItemID + 1)) + userState.GetPropsNumFromBag((int)item.ItemID) < 99)
					{
						userState.BuyWithMithril(item.Mithril);
						userState.InsertPropsToStorage((int)(item.ItemID + 1), 1);
					}
					else
					{
						string msg2 = UIConstant.GetMessage(31).Replace("[n]", "\n");
						msgUI.CreateConfirm(msg2, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_PURCHASE_PROPS_TO_MANY);
						msgUI.Show();
					}
				}
				else
				{
					msgUI.CreateQuery(UIConstant.GetMessage(11), MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_PURCHASE_MITHRIL);
					msgUI.Show();
				}
				AudioManager.GetInstance().PlaySound(AudioName.CLICK);
				break;
			}
			}
			return;
		}
		if (control == msgUI)
		{
			int eventID = msgUI.GetEventID();
			if (eventID == MessageBoxUI.EVENT_PURCHASE_MITHRIL || eventID == MessageBoxUI.EVENT_PURCHASE_CASH)
			{
				switch (command)
				{
				case 10:
					msgUI.Hide();
					iapUI.Create();
					iapUI.SetSelection(2);
					iapUI.Show();
					break;
				case 9:
					msgUI.Hide();
					break;
				}
			}
			else if (eventID == MessageBoxUI.EVENT_PURCHASE_PROPS_TO_MANY && command == 9)
			{
				msgUI.Hide();
			}
			return;
		}
		for (int i = 0; i < categoryTxtBtn.Length; i++)
		{
			if (categoryTxtBtn[i] == control)
			{
				selectCategoryImg.Rect = categoryTxtBtn[i].Rect;
				selectPropsPageImg.Rect = pageNavImg[i][0].Rect;
				SetProps((byte)i);
				break;
			}
		}
	}
}
