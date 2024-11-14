using System;
using System.Collections.Generic;
using UnityEngine;

public class IAPUI : UIPanelX, UIHandler
{
	public enum Command
	{
		Exit = 0
	}

	private byte m_rank;

	private UISliderIAP.UIIAPIcon rankiap;

	private UISliderIAP.UIIAPIcon rookieiap;

	public UIStateManager stateMgr;

	private static byte[] BG_IMG = new byte[1] { 1 };

	private UIBlock m_block;

	private UIImage shadowImg;

	private UIImage IAPImg;

	private UIClickButton backBtn;

	private UIClickButton restoreBtn;

	private UIClickButton[] categoryTxtBtn;

	private UIImage selectCategoryImg;

	private List<List<UIImage>> pageNavImg = new List<List<UIImage>>();

	private UIImage[] selectPageImg = new UIImage[4];

	private List<UISliderIAP> swapIAPList = new List<UISliderIAP>();

	private UISliderIAP swapIAP;

	private static byte[] TAG_IAP_NORMAL = new byte[2] { 11, 19 };

	private static byte[] TAG_BANK_NORMAL = new byte[2] { 12, 21 };

	private static byte SELECT_TAG = 13;

	private static byte[] BACK_NORMAL = new byte[2] { 16, 23 };

	private static byte[] BACK_PRESSED = new byte[2] { 16, 24 };

	public byte curTag;

	private UINumeric mithrilNum;

	private UINumeric cashNum;

	private MessageBoxUI msgUI;

	private NetLoadingUI netLoadingUI;

	private static IAPName iapProcessing = IAPName.None;

	private List<IAPItem> itemList;

	private bool m_bGotMithril;

	public static bool m_bUpdate;

	private FrUIText tipsTxt;

	public static IAPName IapProcessing
	{
		get
		{
			return iapProcessing;
		}
	}

	public IAPUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public void Close()
	{
		GameApp.GetInstance().DestoryNetWorkIAP();
		Debug.Log("close networkIAP..");
		stateMgr.m_UIManager.SetEnable(true);
		swapIAP.Clear();
		ClearIAPList();
		Clear();
		base.Visible = false;
	}

	public void Create()
	{
		NetworkManager networkManagerIAP = GameApp.GetInstance().GetNetworkManagerIAP();
		if (GameApp.GetInstance().IsConnectedToInternet() && networkManagerIAP == null && AndroidConstant.version == AndroidConstant.Version.GooglePlay)
		{
			networkManagerIAP = GameApp.GetInstance().CreateNetworkIAP("sw1.freyrgames.com", 8095);
			GetMithrilPriceRequest request = new GetMithrilPriceRequest();
			networkManagerIAP.SendRequest(request);
			Debug.Log("create networkIAP..");
		}
		m_bGotMithril = false;
		m_bUpdate = false;
		UnitUI unitUI = Res2DManager.GetInstance().vUI[21];
		UnitUI ui = Res2DManager.GetInstance().vUI[17];
		curTag = 0;
		m_block = new UIBlock();
		m_block.Rect = new Rect(0f, 0f, UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight);
		Add(m_block);
		shadowImg = new UIImage();
		shadowImg.AddObject(unitUI, 0, 0);
		shadowImg.Rect = shadowImg.GetObjectRect();
		shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		IAPImg = new UIImage();
		IAPImg.AddObject(unitUI, 0, BG_IMG);
		IAPImg.Rect = IAPImg.GetObjectRect();
		backBtn = new UIClickButton();
		backBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, BACK_NORMAL);
		backBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, BACK_PRESSED);
		backBtn.Rect = backBtn.GetObjectRect(UIButtonBase.State.Normal);
		restoreBtn = new UIClickButton();
		restoreBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 26);
		restoreBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 25);
		restoreBtn.Rect = restoreBtn.GetObjectRect(UIButtonBase.State.Normal);
		restoreBtn.Visible = false;
		restoreBtn.Enable = false;
		string text = UIConstant.GetMessage(44).Replace("[n]", "\n");
		tipsTxt = new FrUIText();
		tipsTxt.AlignStyle = FrUIText.enAlignStyle.TOP_LEFT;
		tipsTxt.Set("font3", text, UIConstant.FONT_COLOR_TIPS, UIConstant.ScreenLocalWidth - 150f);
		tipsTxt.Rect = new Rect(backBtn.Rect.x + backBtn.Rect.width + 20f, backBtn.Rect.y, UIConstant.ScreenLocalWidth - 150f, backBtn.Rect.height);
		IniIAPList();
		Add(shadowImg);
		Add(IAPImg);
		Add(backBtn);
		Add(tipsTxt);
		Add(restoreBtn);
		categoryTxtBtn = new UIClickButton[Global.TOTAL_IAP_CATEGORY_NUM];
		categoryTxtBtn[0] = new UIClickButton();
		categoryTxtBtn[0].AddObject(UIButtonBase.State.Normal, unitUI, 0, TAG_IAP_NORMAL);
		categoryTxtBtn[0].AddObject(UIButtonBase.State.Pressed, unitUI, 0, TAG_IAP_NORMAL);
		categoryTxtBtn[0].Rect = categoryTxtBtn[Global.IAP_CATEGORY_PURCHASE].GetObjectRect(UIButtonBase.State.Normal);
		Add(categoryTxtBtn[0]);
		categoryTxtBtn[1] = new UIClickButton();
		categoryTxtBtn[1].AddObject(UIButtonBase.State.Normal, unitUI, 0, TAG_BANK_NORMAL);
		categoryTxtBtn[1].AddObject(UIButtonBase.State.Pressed, unitUI, 0, TAG_BANK_NORMAL);
		categoryTxtBtn[1].Rect = categoryTxtBtn[Global.IAP_CATEGORY_EXCHANGE].GetObjectRect(UIButtonBase.State.Normal);
		Add(categoryTxtBtn[1]);
		selectCategoryImg = new UIImage();
		selectCategoryImg.AddObject(unitUI, 0, SELECT_TAG);
		selectCategoryImg.Rect = categoryTxtBtn[curTag].Rect;
		Add(selectCategoryImg);
		mithrilNum = new UINumeric();
		int mithril = GameApp.GetInstance().GetUserState().GetMithril();
		mithrilNum.AlignStyle = UINumeric.enAlignStyle.right;
		mithrilNum.SpacingOffsetX = -8f;
		mithrilNum.SetNumeric(ui, 1, "#" + string.Format("{0:N0}", mithril));
		mithrilNum.Rect = unitUI.GetModulePositionRect(0, 0, 14);
		Add(mithrilNum);
		cashNum = new UINumeric();
		int cash = GameApp.GetInstance().GetUserState().GetCash();
		cashNum.AlignStyle = UINumeric.enAlignStyle.left;
		cashNum.SpacingOffsetX = -8f;
		cashNum.SetNumeric(ui, 1, "$" + string.Format("{0:N0}", cash));
		cashNum.Rect = unitUI.GetModulePositionRect(0, 0, 15);
		Add(cashNum);
		pageNavImg.Clear();
		int num = 32;
		pageNavImg = new List<List<UIImage>>();
		for (int i = 0; i < Global.TOTAL_IAP_CATEGORY_NUM; i++)
		{
			Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 17);
			float num2 = modulePositionRect.x + modulePositionRect.width * 0.5f - (float)((Global.TOTAL_IAP_CATEGORY[i] - 1) * num) * 0.5f;
			List<UIImage> list = new List<UIImage>();
			for (int j = 0; j < Global.TOTAL_IAP_CATEGORY[i]; j++)
			{
				UIImage uIImage = new UIImage();
				uIImage.AddObject(unitUI, 0, 17);
				uIImage.Rect = new Rect(num2 - modulePositionRect.width * 0.5f, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
				num2 += (float)num;
				list.Add(uIImage);
			}
			pageNavImg.Add(list);
		}
		for (int k = 0; k < pageNavImg.Count; k++)
		{
			for (int l = 0; l < pageNavImg[k].Count; l++)
			{
				Add(pageNavImg[k][l]);
			}
		}
		for (int m = 0; m < selectPageImg.Length; m++)
		{
			selectPageImg[m] = new UIImage();
			selectPageImg[m].AddObject(unitUI, 0, 18);
			selectPageImg[m].Rect = pageNavImg[curTag][m].Rect;
			Add(selectPageImg[m]);
		}
		swapIAP = new UISliderIAP();
		swapIAP.Create(unitUI);
		SetUIIAP(0);
		Add(swapIAP);
		netLoadingUI = new NetLoadingUI(stateMgr);
		netLoadingUI.Create();
		netLoadingUI.Hide();
		Add(netLoadingUI);
		tipsTxt.Visible = false;
		tipsTxt.Enable = false;
		msgUI = new MessageBoxUI(stateMgr);
		msgUI.Create();
		msgUI.Hide();
		Add(msgUI);
		SetUIHandler(this);
		stateMgr.m_UIManager.SetEnable(false);
		Shop shop = new Shop();
		shop.CreateIAPShopData();
		itemList = shop.GetIAPList();
	}

	private void ClearIAPList()
	{
		for (int i = 0; i < swapIAPList.Count; i++)
		{
			swapIAPList[i].Clear();
		}
		swapIAPList.Clear();
	}

	public void IniIAPList()
	{
		ClearIAPList();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[21];
		UnitUI ui = Res2DManager.GetInstance().vUI[17];
		UISliderIAP uISliderIAP = new UISliderIAP();
		for (int i = 0; i < Global.TOTAL_IAP_CATEGORY[0]; i++)
		{
			UISliderIAP.UIIAPIcon uIIAPIcon = new UISliderIAP.UIIAPIcon();
			if (AndroidConstant.version == AndroidConstant.Version.Kindle)
			{
				switch (i)
				{
				case 0:
				{
					rookieiap = new UISliderIAP.UIIAPIcon();
					rookieiap.SetBackground(unitUI, i + 2);
					rookieiap.SetBuyBtn(unitUI, 0, 9, 8);
					rookieiap.SetBuyBtnText(ui, 1, "$" + UIConstant.IAP_PRICE_KINDLE[i]);
					if (GameApp.GetInstance().GetUserState().bPurchaseRookie)
					{
						rookieiap.SetBuyBtnPress(unitUI, 0, 8);
					}
					Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 7);
					if (UIConstant.IAP_MITHRIL[i] > 0)
					{
						rookieiap.SetMithril(ui, 1, "#" + Convert.ToString(UIConstant.IAP_MITHRIL[i]), modulePositionRect2);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0 || i == 0)
					{
						rookieiap.SetDecorate(unitUI, 1, i);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0)
					{
						modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 6);
						rookieiap.SetOriginal(ui, 2, "$" + UIConstant.IAP_ORIGIONAL_PRICE[i], new Rect(modulePositionRect2.x, modulePositionRect2.y - 20f, modulePositionRect2.width, modulePositionRect2.height));
						modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 5);
						rookieiap.SetSaving(ui, 3, "$" + UIConstant.IAP_SAVING_PRICE[i], new Rect(modulePositionRect2.x, modulePositionRect2.y - 20f, modulePositionRect2.width, modulePositionRect2.height));
					}
					rookieiap.m_buyBtn.Id = i;
					rookieiap.Id = i;
					rookieiap.Enable = true;
					rookieiap.Rect = uIIAPIcon.m_background.Rect;
					uISliderIAP.Add(rookieiap);
					break;
				}
				case 1:
				{
					UnitUI unitUI2 = Res2DManager.GetInstance().vUI[14];
					Rank rank = GameApp.GetInstance().GetUserState().GetRank();
					m_rank = rank.rankID;
					rankiap = new UISliderIAP.UIIAPIcon();
					UserState userState = GameApp.GetInstance().GetUserState();
					List<Rank> rankList = userState.GetRankList();
					switch (AndroidConstant.version)
					{
					case AndroidConstant.Version.GooglePlay:
						rankiap.SetBackground(unitUI, i + 2);
						break;
					case AndroidConstant.Version.Kindle:
						rankiap.SetBackground(unitUI, i + 2, 0);
						break;
					}
					rankiap.SetBuyBtn(unitUI, 0, 9, 8);
					rankiap.SetRank(unitUI, 29 + m_rank);
					rankiap.SetExpBar(unitUI);
					rankiap.SetBuyBtnText(ui, 1, "$" + UIConstant.IAP_PRICE_KINDLE[i]);
					if (m_rank == 11)
					{
						rankiap.SetRankDes(unitUI, 42);
						rankiap.SetBuyBtnPress(unitUI, 0, 8);
					}
					else if (m_rank < 11)
					{
						int exp = rankList[m_rank + 1].exp;
						int num = userState.GetExp() - rankList[m_rank].exp;
						int width = (int)(rankiap.m_expBarImg.Rect.width * (float)num / (float)exp);
						rankiap.width = width;
						rankiap.SetRankDes(unitUI, 41);
					}
					Rect modulePositionRect3 = unitUI.GetModulePositionRect(0, 0, 7);
					if (UIConstant.IAP_MITHRIL[i] > 0)
					{
						rankiap.SetMithril(ui, 1, "#" + Convert.ToString(UIConstant.IAP_MITHRIL[i]), modulePositionRect3);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0 || i == 0)
					{
						rankiap.SetDecorate(unitUI, 1, i);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0)
					{
						modulePositionRect3 = unitUI.GetModulePositionRect(0, 0, 6);
						rankiap.SetOriginal(ui, 2, "$" + UIConstant.IAP_ORIGIONAL_PRICE[i], new Rect(modulePositionRect3.x, modulePositionRect3.y - 20f, modulePositionRect3.width, modulePositionRect3.height));
						modulePositionRect3 = unitUI.GetModulePositionRect(0, 0, 5);
						rankiap.SetSaving(ui, 3, "$" + UIConstant.IAP_SAVING_PRICE[i], new Rect(modulePositionRect3.x, modulePositionRect3.y - 20f, modulePositionRect3.width, modulePositionRect3.height));
					}
					rankiap.m_buyBtn.Id = i;
					rankiap.Id = i;
					rankiap.Enable = true;
					rankiap.Rect = uIIAPIcon.m_background.Rect;
					uISliderIAP.Add(rankiap);
					break;
				}
				default:
				{
					uIIAPIcon.SetBackground(unitUI, i + 2);
					uIIAPIcon.SetBuyBtn(unitUI, 0, 9, 8);
					uIIAPIcon.SetBuyBtnText(ui, 1, "$" + UIConstant.IAP_PRICE_KINDLE[i]);
					Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 7);
					if (UIConstant.IAP_MITHRIL[i] > 0)
					{
						uIIAPIcon.SetMithril(ui, 1, "#" + Convert.ToString(UIConstant.IAP_MITHRIL[i]), modulePositionRect);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0 || i == 0)
					{
						uIIAPIcon.SetDecorate(unitUI, 1, i);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0)
					{
						modulePositionRect = unitUI.GetModulePositionRect(0, 0, 6);
						uIIAPIcon.SetOriginal(ui, 2, "$" + UIConstant.IAP_ORIGIONAL_PRICE[i], new Rect(modulePositionRect.x, modulePositionRect.y - 20f, modulePositionRect.width, modulePositionRect.height));
						modulePositionRect = unitUI.GetModulePositionRect(0, 0, 5);
						uIIAPIcon.SetSaving(ui, 3, "$" + UIConstant.IAP_SAVING_PRICE[i], new Rect(modulePositionRect.x, modulePositionRect.y - 20f, modulePositionRect.width, modulePositionRect.height));
					}
					uIIAPIcon.m_buyBtn.Id = i;
					uIIAPIcon.Id = i;
					uIIAPIcon.Enable = true;
					uIIAPIcon.Rect = uIIAPIcon.m_background.Rect;
					uISliderIAP.Add(uIIAPIcon);
					break;
				}
				}
			}
			else
			{
				if (AndroidConstant.version != 0)
				{
					continue;
				}
				switch (i)
				{
				case 0:
				{
					rookieiap = new UISliderIAP.UIIAPIcon();
					rookieiap.SetBackground(unitUI, i + 2);
					rookieiap.SetBuyBtn(unitUI, 0, 9, 8);
					rookieiap.SetBuyBtnText(ui, 1, "$" + UIConstant.IAP_PRICE[i]);
					if (GameApp.GetInstance().GetUserState().bPurchaseRookie)
					{
						rookieiap.SetBuyBtnPress(unitUI, 0, 8);
					}
					Rect modulePositionRect5 = unitUI.GetModulePositionRect(0, 0, 7);
					if (UIConstant.IAP_MITHRIL[i] > 0)
					{
						rookieiap.SetMithril(ui, 1, "#" + Convert.ToString(UIConstant.IAP_MITHRIL[i]), modulePositionRect5);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0 || i == 0)
					{
						rookieiap.SetDecorate(unitUI, 1, i);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0)
					{
						modulePositionRect5 = unitUI.GetModulePositionRect(0, 0, 6);
						rookieiap.SetOriginal(ui, 2, "$" + UIConstant.IAP_ORIGIONAL_PRICE[i], new Rect(modulePositionRect5.x, modulePositionRect5.y - 20f, modulePositionRect5.width, modulePositionRect5.height));
						modulePositionRect5 = unitUI.GetModulePositionRect(0, 0, 5);
						rookieiap.SetSaving(ui, 3, "$" + UIConstant.IAP_SAVING_PRICE[i], new Rect(modulePositionRect5.x, modulePositionRect5.y - 20f, modulePositionRect5.width, modulePositionRect5.height));
					}
					rookieiap.m_buyBtn.Id = i;
					rookieiap.Id = i;
					rookieiap.Enable = true;
					rookieiap.Rect = uIIAPIcon.m_background.Rect;
					uISliderIAP.Add(rookieiap);
					break;
				}
				case 1:
				{
					rankiap = new UISliderIAP.UIIAPIcon();
					rankiap.SetBackground(unitUI, i + 2);
					rankiap.SetBuyBtn(unitUI, 0, 9, 8);
					rankiap.SetBuyBtnText(ui, 1, "$" + UIConstant.IAP_PRICE[i]);
					if (GameApp.GetInstance().GetUserState().bPurchaseSergeant)
					{
						rankiap.SetBuyBtnPress(unitUI, 0, 8);
					}
					Rect modulePositionRect6 = unitUI.GetModulePositionRect(0, 0, 7);
					if (UIConstant.IAP_MITHRIL[i] > 0)
					{
						rankiap.SetMithril(ui, 1, "#" + Convert.ToString(UIConstant.IAP_MITHRIL[i]), modulePositionRect6);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0 || i == 0)
					{
						rankiap.SetDecorate(unitUI, 1, i);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0)
					{
						modulePositionRect6 = unitUI.GetModulePositionRect(0, 0, 6);
						rankiap.SetOriginal(ui, 2, "$" + UIConstant.IAP_ORIGIONAL_PRICE[i], new Rect(modulePositionRect6.x, modulePositionRect6.y - 20f, modulePositionRect6.width, modulePositionRect6.height));
						modulePositionRect6 = unitUI.GetModulePositionRect(0, 0, 5);
						rankiap.SetSaving(ui, 3, "$" + UIConstant.IAP_SAVING_PRICE[i], new Rect(modulePositionRect6.x, modulePositionRect6.y - 20f, modulePositionRect6.width, modulePositionRect6.height));
					}
					rankiap.m_buyBtn.Id = i;
					rankiap.Id = i;
					rankiap.Enable = true;
					rankiap.Rect = uIIAPIcon.m_background.Rect;
					uISliderIAP.Add(rankiap);
					break;
				}
				default:
				{
					uIIAPIcon.SetBackground(unitUI, i + 2);
					uIIAPIcon.SetBuyBtn(unitUI, 0, 9, 8);
					uIIAPIcon.SetBuyBtnText(ui, 1, "$" + UIConstant.IAP_PRICE[i]);
					Rect modulePositionRect4 = unitUI.GetModulePositionRect(0, 0, 7);
					if (UIConstant.IAP_MITHRIL[i] > 0)
					{
						uIIAPIcon.SetMithril(ui, 1, "#" + Convert.ToString(UIConstant.IAP_MITHRIL[i]), modulePositionRect4);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0 || i == 0)
					{
						uIIAPIcon.SetDecorate(unitUI, 1, i);
					}
					if (UIConstant.IAP_SAVING_PRICE[i].Length > 0)
					{
						modulePositionRect4 = unitUI.GetModulePositionRect(0, 0, 6);
						uIIAPIcon.SetOriginal(ui, 2, "$" + UIConstant.IAP_ORIGIONAL_PRICE[i], new Rect(modulePositionRect4.x, modulePositionRect4.y - 20f, modulePositionRect4.width, modulePositionRect4.height));
						modulePositionRect4 = unitUI.GetModulePositionRect(0, 0, 5);
						uIIAPIcon.SetSaving(ui, 3, "$" + UIConstant.IAP_SAVING_PRICE[i], new Rect(modulePositionRect4.x, modulePositionRect4.y - 20f, modulePositionRect4.width, modulePositionRect4.height));
					}
					uIIAPIcon.m_buyBtn.Id = i;
					uIIAPIcon.Id = i;
					uIIAPIcon.Enable = true;
					uIIAPIcon.Rect = uIIAPIcon.m_background.Rect;
					uISliderIAP.Add(uIIAPIcon);
					break;
				}
				}
			}
		}
		swapIAPList.Add(uISliderIAP);
		UISliderIAP uISliderIAP2 = new UISliderIAP();
		for (int j = 0; j < Global.TOTAL_IAP_CATEGORY[1]; j++)
		{
			UISliderIAP.UIIAPIcon uIIAPIcon2 = new UISliderIAP.UIIAPIcon();
			uIIAPIcon2.SetBackground(unitUI, j + 11);
			uIIAPIcon2.SetBuyBtn(unitUI, 0, 9, 8);
			uIIAPIcon2.SetBuyBtnText(ui, 1, "#" + UIConstant.IAP_EXCHANGE_MITHRIL[j]);
			Rect modulePositionRect7 = unitUI.GetModulePositionRect(0, 0, 7);
			uIIAPIcon2.SetMithril(ui, 1, "$" + UIConstant.IAP_EXCHANGE_CASH[j] / 1000 + "k", modulePositionRect7);
			uIIAPIcon2.m_buyBtn.Id = j;
			uIIAPIcon2.Id = j;
			uIIAPIcon2.Enable = true;
			uIIAPIcon2.Rect = uIIAPIcon2.m_background.Rect;
			uISliderIAP2.Add(uIIAPIcon2);
		}
		swapIAPList.Add(uISliderIAP2);
	}

	public void UpdatePrice()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[21];
		UnitUI ui = Res2DManager.GetInstance().vUI[17];
		if (swapIAPList == null || swapIAPList.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < swapIAPList[0].m_IAPIcons.Count; i++)
		{
			UISliderIAP.UIIAPIcon uIIAPIcon = swapIAPList[0].m_IAPIcons[i];
			uIIAPIcon.ResetBackground(unitUI, i + 2);
			uIIAPIcon.ResetBuyBtn(unitUI, 0, 9, 8);
			if (AndroidConstant.version == AndroidConstant.Version.GooglePlay)
			{
				uIIAPIcon.SetBuyBtnText(ui, 1, "$" + UIConstant.IAP_PRICE[i]);
			}
			else if (AndroidConstant.version == AndroidConstant.Version.Kindle)
			{
				uIIAPIcon.SetBuyBtnText(ui, 1, "$" + UIConstant.IAP_PRICE_KINDLE[i]);
			}
			Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 7);
			if (UIConstant.IAP_MITHRIL[i] > 0)
			{
				uIIAPIcon.SetMithril(ui, 1, "#" + Convert.ToString(UIConstant.IAP_MITHRIL[i]), modulePositionRect);
			}
			if (UIConstant.IAP_SAVING_PRICE[i].Length > 0 || i == 0)
			{
				uIIAPIcon.ResetDecorate(unitUI, 1, i);
			}
			if (UIConstant.IAP_SAVING_PRICE[i].Length > 0)
			{
				modulePositionRect = unitUI.GetModulePositionRect(0, 0, 6);
				uIIAPIcon.SetOriginal(ui, 2, "$" + UIConstant.IAP_ORIGIONAL_PRICE[i], new Rect(modulePositionRect.x, modulePositionRect.y - 20f, modulePositionRect.width, modulePositionRect.height));
				modulePositionRect = unitUI.GetModulePositionRect(0, 0, 5);
				uIIAPIcon.SetSaving(ui, 3, "$" + UIConstant.IAP_SAVING_PRICE[i], new Rect(modulePositionRect.x, modulePositionRect.y - 20f, modulePositionRect.width, modulePositionRect.height));
			}
			uIIAPIcon.m_buyBtn.Id = i;
			uIIAPIcon.Id = i;
			uIIAPIcon.Enable = true;
			uIIAPIcon.Rect = uIIAPIcon.m_background.Rect;
		}
		m_bUpdate = false;
	}

	public void SetSelection(int index)
	{
		swapIAP.SetSelection(index);
	}

	public void SetUIIAP(byte tag)
	{
		curTag = tag;
		for (int i = 0; i < pageNavImg.Count; i++)
		{
			for (int j = 0; j < pageNavImg[i].Count; j++)
			{
				pageNavImg[i][j].Visible = false;
				if (tag == i)
				{
					pageNavImg[i][j].Visible = true;
				}
			}
		}
		for (int k = 0; k < selectPageImg.Length; k++)
		{
			int index = k % pageNavImg[tag].Count;
			selectPageImg[k].Rect = pageNavImg[tag][index].Rect;
		}
		swapIAP.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[21];
		for (int l = 0; l < swapIAPList[tag].m_IAPIcons.Count; l++)
		{
			swapIAP.Add(swapIAPList[tag].m_IAPIcons[l]);
		}
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 2);
		Rect rect = new Rect(0f, IAPImg.Rect.y + IAPImg.Rect.height * 0.5f - (modulePositionRect.height * 0.5f + 30f), UIConstant.ScreenLocalWidth, modulePositionRect.height + 60f);
		swapIAP.SetClipRect(rect);
		float num = modulePositionRect.width + 10f;
		swapIAP.SetScroller(0f, num * (float)swapIAP.m_IAPIcons.Count, num, rect);
		swapIAP.SetSelection(0);
	}

	public override void Draw()
	{
		base.Draw();
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (base.HandleInput(touch))
		{
			return true;
		}
		return false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == swapIAP)
		{
			switch (command)
			{
			case 1:
			{
				int num2 = (int)wparam;
				for (int i = 0; i < selectPageImg.Length; i++)
				{
					int index2 = (num2 + i) % pageNavImg[curTag].Count;
					selectPageImg[i].Rect = pageNavImg[curTag][index2].Rect;
				}
				AudioManager.GetInstance().PlaySound(AudioName.SWITCH_ITEM);
				break;
			}
			case 0:
				AudioManager.GetInstance().PlaySound(AudioName.CLICK);
				if (curTag == Global.IAP_CATEGORY_PURCHASE)
				{
					if (!GameApp.GetInstance().IsConnectedToInternet())
					{
						break;
					}
					backBtn.Enable = false;
					int index = (int)wparam;
					if (itemList[index].Name == IAPName.ROOKIE)
					{
						if (GameApp.GetInstance().GetUserState().bPurchaseRookie)
						{
							backBtn.Enable = true;
							break;
						}
						netLoadingUI.SetMessage(22, MessageBoxUI.MESSAGE_FLAG_ERROR, MessageBoxUI.EVENT_IAP_NET_TIMEOUT);
						netLoadingUI.Show(1800);
						AndroidIAPPluginScript.CallPurchaseProduct(itemList[index].ID);
						iapProcessing = (IAPName)index;
					}
					else if (itemList[index].Name == IAPName.SERGEANT)
					{
						bool flag = false;
						if (AndroidConstant.version == AndroidConstant.Version.GooglePlay)
						{
							flag = GameApp.GetInstance().GetUserState().bPurchaseSergeant;
						}
						else if (AndroidConstant.version == AndroidConstant.Version.Kindle)
						{
							flag = GameApp.GetInstance().GetUserState().GetExp() >= 20000000;
						}
						if (flag)
						{
							backBtn.Enable = true;
							break;
						}
						netLoadingUI.SetMessage(22, MessageBoxUI.MESSAGE_FLAG_ERROR, MessageBoxUI.EVENT_IAP_NET_TIMEOUT);
						netLoadingUI.Show(1800);
						AndroidIAPPluginScript.CallPurchaseProduct(itemList[index].ID);
						iapProcessing = (IAPName)index;
					}
					else
					{
						netLoadingUI.SetMessage(22, MessageBoxUI.MESSAGE_FLAG_ERROR, MessageBoxUI.EVENT_IAP_NET_TIMEOUT);
						netLoadingUI.Show(1800);
						AndroidIAPPluginScript.CallPurchaseProduct(itemList[index].ID);
						iapProcessing = (IAPName)index;
					}
				}
				else if (curTag == Global.IAP_CATEGORY_EXCHANGE)
				{
					int num = (int)wparam;
					UserState userState = GameApp.GetInstance().GetUserState();
					if (userState.GetMithril() >= UIConstant.IAP_EXCHANGE_MITHRIL[num] && userState.GetCash() + UIConstant.IAP_EXCHANGE_CASH[num] <= Global.MAX_CASH)
					{
						userState.BuyWithMithril(UIConstant.IAP_EXCHANGE_MITHRIL[num]);
						userState.AddCash(UIConstant.IAP_EXCHANGE_CASH[num]);
					}
				}
				break;
			}
			return;
		}
		if (control == backBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK_BACK);
			Close();
			m_Parent.SendEvent(this, 0, 0f, 0f);
			return;
		}
		if (control == restoreBtn)
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				msgUI.CreateQuery("Only Rookie package and Sergant Major can be restored", MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_RESTORE_DIALOG);
				msgUI.Show();
			}
			return;
		}
		if (control == msgUI)
		{
			int eventID = msgUI.GetEventID();
			if (eventID == MessageBoxUI.EVENT_RESTORE_DIALOG)
			{
				switch (command)
				{
				case 10:
					AudioManager.GetInstance().PlaySound(AudioName.CLICK_BACK);
					iapProcessing = IAPName.Restore;
					AndroidIAPPluginScript.GetRestorePurchse();
					break;
				}
			}
			msgUI.Hide();
			stateMgr.m_UIManager.SetEnable(false);
			return;
		}
		for (int j = 0; j < categoryTxtBtn.Length; j++)
		{
			if (categoryTxtBtn[j] == control)
			{
				SetUIIAP((byte)j);
				selectCategoryImg.Rect = categoryTxtBtn[j].Rect;
				break;
			}
		}
	}

	public override void Update()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[21];
		if (AndroidConstant.version == AndroidConstant.Version.Kindle)
		{
			UnitUI unitUI = Res2DManager.GetInstance().vUI[14];
			Rank rank = GameApp.GetInstance().GetUserState().GetRank();
			if (rank.rankID != m_rank && rankiap != null)
			{
				m_rank = rank.rankID;
				rankiap.ResetRank(ui, 29 + m_rank);
			}
			if (m_rank == 11 && rankiap != null)
			{
				rankiap.RestRankDes(ui, 42);
				rankiap.SetBuyBtnPress(ui, 0, 8);
			}
			else if (m_rank < 11 && rankiap != null)
			{
				UserState userState = GameApp.GetInstance().GetUserState();
				List<Rank> rankList = userState.GetRankList();
				int exp = rankList[m_rank + 1].exp;
				int num = userState.GetExp() - rankList[m_rank].exp;
				int width = (int)(rankiap.m_expBarImg.Rect.width * (float)num / (float)exp);
				rankiap.width = width;
			}
		}
		NetworkManager networkManagerIAP = GameApp.GetInstance().GetNetworkManagerIAP();
		if (networkManagerIAP != null)
		{
			networkManagerIAP.ProcessReceivedPackets();
		}
		if (m_bUpdate && networkManagerIAP != null)
		{
			UpdatePrice();
		}
		if (GameApp.GetInstance().GetUserState().bPurchaseRookie && rookieiap != null)
		{
			rookieiap.SetBuyBtnPress(ui, 0, 8);
		}
		if (GameApp.GetInstance().GetUserState().bPurchaseSergeant && rankiap != null)
		{
			rankiap.SetBuyBtnPress(ui, 0, 8);
		}
		base.Update();
		UnitUI ui2 = Res2DManager.GetInstance().vUI[17];
		int mithril = GameApp.GetInstance().GetUserState().GetMithril();
		int cash = GameApp.GetInstance().GetUserState().GetCash();
		if (cashNum != null)
		{
			cashNum.SetNumeric(ui2, 1, "$" + string.Format("{0:N0}", cash));
		}
		if (mithrilNum != null)
		{
			mithrilNum.SetNumeric(ui2, 1, "#" + string.Format("{0:N0}", mithril));
		}
		GetPurchaseStatus();
	}

	public void GetPurchaseStatus()
	{
		if (iapProcessing == IAPName.None)
		{
			return;
		}
		switch (AndroidIAPPluginScript.GetPurchaseStatus())
		{
		case 0:
			return;
		case 1:
			GameApp.GetInstance().GetUserState().DeliverIAPItem(iapProcessing);
			AndroidIAPPluginScript.InitPurchase();
			iapProcessing = IAPName.None;
			netLoadingUI.Hide();
			stateMgr.m_UIManager.SetEnable(false);
			backBtn.Enable = true;
			return;
		}
		if (AndroidConstant.version == AndroidConstant.Version.Kindle)
		{
			msgUI.Hide();
		}
		AndroidIAPPluginScript.InitPurchase();
		iapProcessing = IAPName.None;
		netLoadingUI.Hide();
		stateMgr.m_UIManager.SetEnable(false);
		backBtn.Enable = true;
	}
}
