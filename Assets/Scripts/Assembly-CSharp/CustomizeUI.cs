using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeUI : UIHandler, IUIHandle
{
	private const int MAX_PROPERTY_COUNT = 4;

	private const byte IMG_HP = 0;

	private const byte IMG_POWER = 1;

	private const byte IMG_SPEED = 2;

	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_HANDLE = 2;

	public UIStateManager stateMgr;

	protected UserState userState;

	private static byte[] BG_IMG = new byte[2] { 0, 1 };

	private static byte TITLE_FRAME_BEGIN_IMG = 5;

	private static byte TITLE_FRAME_COUNT_IMG = 5;

	private static byte BRIEF_BACKGROUND_BEGIN_IMG = 10;

	private static byte BRIEF_BACKGROUND_COUNT_IMG = 7;

	private NavigationBarUI navigationBar;

	private NavigationMenuUI navigationMenu;

	private UIImage customizeImg;

	private UITextButton packTxtBtn;

	private UITextButton refuelTxtBtn;

	private UITextButton iapTxtBtn;

	private UIImage packFlagImg;

	private UIImage refuelFlagImg;

	private UIImage iapFlagImg;

	private UIImage equipBGImg;

	private UIClickButton equipBtn;

	private UIImage equipedImg;

	private UIImage mTitleFrame;

	private UIImage mTitleBG;

	private FrUIText mTitleText;

	private FrUIText mLevelText;

	private FrUIText[] mPropertiesTexts;

	private UIImage[,] mSuitIcon;

	private UIImage[,] mLevelIcon;

	private UIImage mGunIcon;

	private UIAvatar3D avatarFrame;

	private UISliderTag swapTag;

	private UISliderAvatar swapAvatar;

	private UIImage[,] compareImg;

	private UITextImage[] compareTitleImg;

	private FrUIText[] compareTxt;

	public byte state;

	private byte curTag = 5;

	private byte nextTag;

	private int[] m_equipSel = new int[6];

	protected PlayerSkill playerSkill;

	protected PlayerSkill playerSkillPrev;

	private RefuelUI refuel;

	private UIImage[] tagNavImg;

	private UIImage selectTagNavImg;

	private List<List<UIImage>> avatarNavListImg = new List<List<UIImage>>();

	private UIImage selectAvatarNavImg;

	private GameObject light;

	private UINumeric upPriceNum;

	private UIDialog mDescription;

	private string[] mWeaponDescriptions;

	private string[] mBonusDescriptions;

	private string[] mPackDescriptions;

	private bool mNeedUpdateDescrition = true;

	private UIClickButton twitterImg;

	private UIClickButton facebookImg;

	private MessageBoxUI msgUI;

	private IAPUI iapUI;

	private UIClickButton freeGotMithrilImg;

	private UIClickButton New;

	private AdsUI adsui;

	public CustomizeUI(UIStateManager stateMgr)
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
		mNeedUpdateDescrition = true;
		string[] gameText = Res2DManager.GetInstance().GetGameText();
		if (gameText.Length > 0)
		{
			mWeaponDescriptions = Res2DManager.GetInstance().SplitString(gameText[0]);
		}
		if (gameText.Length > 1)
		{
			mBonusDescriptions = Res2DManager.GetInstance().SplitString(gameText[1]);
		}
		if (gameText.Length > 2)
		{
			mPackDescriptions = Res2DManager.GetInstance().SplitString(gameText[2]);
		}
	}

	public void Close()
	{
		avatarFrame.Clear();
		swapAvatar.Clear();
		UnityEngine.Object.Destroy(light);
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		mWeaponDescriptions = null;
		mBonusDescriptions = null;
		mPackDescriptions = null;
		UnitUI ui = Res2DManager.GetInstance().vUI[22];
		UIImage uIImage = new UIImage();
		uIImage.AddObject(ui, 0, 0);
		uIImage.Rect = uIImage.GetObjectRect();
		uIImage.SetColor(Color.black);
		float num = (float)Screen.width / (float)Screen.height - UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight;
		if (num > 0f)
		{
			float num2 = (int)Math.Abs((float)Screen.height * UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight - (float)Screen.width);
			num2 *= 1.5f;
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth + num2, UIConstant.ScreenLocalHeight));
		}
		else if (num < 0f)
		{
			float num3 = (int)Math.Abs((float)Screen.width * UIConstant.ScreenLocalHeight / UIConstant.ScreenLocalWidth - (float)Screen.height);
			num3 *= 1.5f;
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight + num3));
		}
		else
		{
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		}
		stateMgr.m_UIManager.Add(uIImage);
	}

	public void Create()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		CreateClone();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[10];
		customizeImg = new UIImage();
		customizeImg.AddObject(unitUI, 0, BG_IMG);
		customizeImg.Rect = customizeImg.GetObjectRect();
		navigationBar = new NavigationBarUI(stateMgr);
		navigationBar.Create();
		navigationBar.SetTitle("CUSTOMIZE");
		navigationBar.Show();
		refuelTxtBtn = new UITextButton();
		refuelTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 49);
		refuelTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 49);
		refuelTxtBtn.SetText("font2", "AMMO", UIConstant.fontColor_cyan);
		refuelTxtBtn.SetTextOffset(0f, -10f);
		refuelTxtBtn.Rect = refuelTxtBtn.GetObjectRect(UIButtonBase.State.Normal);
		refuelTxtBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		refuelFlagImg = new UIImage();
		refuelFlagImg.AddObject(unitUI, 0, 83);
		refuelFlagImg.Rect = refuelFlagImg.GetObjectRect();
		equipBGImg = new UIImage();
		equipBGImg.AddObject(unitUI, 0, 75);
		equipBGImg.Rect = equipBGImg.GetObjectRect();
		byte[] module = new byte[2] { 79, 80 };
		byte[] module2 = new byte[2] { 77, 80 };
		byte[] module3 = new byte[2] { 78, 81 };
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 79);
		equipBtn = new UIClickButton();
		equipBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, module);
		equipBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, module2);
		equipBtn.AddObject(UIButtonBase.State.Disabled, unitUI, 0, module3);
		equipBtn.Rect = new Rect(modulePositionRect.x - 40f, modulePositionRect.y - 16f, modulePositionRect.width + 80f, modulePositionRect.height + 32f);
		packTxtBtn = new UITextButton();
		packTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 48);
		packTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 48);
		packTxtBtn.SetTextOffset(0f, -10f);
		packTxtBtn.SetText("font2", "PACK.", UIConstant.fontColor_cyan);
		packTxtBtn.Rect = packTxtBtn.GetObjectRect(UIButtonBase.State.Normal);
		packTxtBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		packFlagImg = new UIImage();
		packFlagImg.AddObject(unitUI, 0, 82);
		packFlagImg.Rect = packFlagImg.GetObjectRect();
		iapTxtBtn = new UITextButton();
		iapTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 88);
		iapTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 88);
		iapTxtBtn.SetTextOffset(0f, -10f);
		iapTxtBtn.SetText("font2", "GOLD", UIConstant.fontColor_cyan);
		iapTxtBtn.Rect = iapTxtBtn.GetObjectRect(UIButtonBase.State.Normal);
		iapTxtBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		iapFlagImg = new UIImage();
		iapFlagImg.AddObject(unitUI, 0, 89);
		iapFlagImg.Rect = iapFlagImg.GetObjectRect();
		compareImg = new UIImage[3, 4];
		compareImg[0, 0] = CreateImg(30);
		compareImg[1, 0] = CreateImg(34);
		compareImg[2, 0] = CreateImg(38);
		compareImg[0, 3] = CreateImg(31);
		compareImg[0, 3].Rect = compareImg[0, 3].Rect;
		compareImg[1, 3] = CreateImg(35);
		compareImg[1, 3].Rect = compareImg[1, 3].Rect;
		compareImg[2, 3] = CreateImg(39);
		compareImg[2, 3].Rect = compareImg[2, 3].Rect;
		compareImg[0, 1] = CreateImg(32);
		compareImg[0, 1].Rect = compareImg[0, 3].Rect;
		compareImg[1, 1] = CreateImg(36);
		compareImg[1, 1].Rect = compareImg[1, 3].Rect;
		compareImg[2, 1] = CreateImg(40);
		compareImg[2, 1].Rect = compareImg[2, 3].Rect;
		compareImg[0, 2] = CreateImg(33);
		compareImg[0, 2].Rect = compareImg[0, 3].Rect;
		compareImg[1, 2] = CreateImg(37);
		compareImg[1, 2].Rect = compareImg[1, 3].Rect;
		compareImg[2, 2] = CreateImg(41);
		compareImg[2, 2].Rect = compareImg[2, 3].Rect;
		compareTitleImg = new UITextImage[3];
		compareTitleImg[0] = new UITextImage();
		compareTitleImg[0].AddObject(unitUI, 0, 70);
		compareTitleImg[0].SetText("font3", string.Empty, UIConstant.PROPERTY_COLOR[0], FrUIText.enAlignStyle.TOP_RIGHT);
		Rect objectRect = compareTitleImg[0].GetObjectRect();
		compareTitleImg[0].SetTextOffset(-10f, -2f);
		compareTitleImg[0].Rect = new Rect(objectRect.x, objectRect.y - 10f, objectRect.width, objectRect.height + 20f);
		compareTitleImg[1] = new UITextImage();
		compareTitleImg[1].AddObject(unitUI, 0, 71);
		compareTitleImg[1].SetText("font3", string.Empty, UIConstant.PROPERTY_COLOR[1], FrUIText.enAlignStyle.TOP_RIGHT);
		objectRect = compareTitleImg[1].GetObjectRect();
		compareTitleImg[1].SetTextOffset(-10f, -2f);
		compareTitleImg[1].Rect = new Rect(objectRect.x, objectRect.y - 10f, objectRect.width, objectRect.height + 20f);
		compareTitleImg[2] = new UITextImage();
		compareTitleImg[2].AddObject(unitUI, 0, 72);
		compareTitleImg[2].SetText("font3", string.Empty, UIConstant.PROPERTY_COLOR[2], FrUIText.enAlignStyle.TOP_RIGHT);
		objectRect = compareTitleImg[2].GetObjectRect();
		compareTitleImg[2].SetTextOffset(-10f, -2f);
		compareTitleImg[2].Rect = new Rect(objectRect.x, objectRect.y - 10f, objectRect.width, objectRect.height + 20f);
		compareTxt = new FrUIText[3];
		compareTxt[0] = new FrUIText();
		compareTxt[0].AlignStyle = FrUIText.enAlignStyle.TOP_LEFT;
		compareTxt[0].Set("font3", "0,000", UIConstant.FONT_COLOR_DESCRIPTION, 200f);
		compareTxt[0].Rect = new Rect(compareTitleImg[0].Rect.x + 64f, compareTitleImg[0].Rect.y - 2f, 200f, compareTitleImg[0].Rect.height);
		compareTxt[1] = new FrUIText();
		compareTxt[1].AlignStyle = FrUIText.enAlignStyle.TOP_LEFT;
		compareTxt[1].Set("font3", "0,000", UIConstant.FONT_COLOR_DESCRIPTION, 200f);
		compareTxt[1].Rect = new Rect(compareTitleImg[1].Rect.x + 64f, compareTitleImg[1].Rect.y - 2f, 200f, compareTitleImg[1].Rect.height);
		compareTxt[2] = new FrUIText();
		compareTxt[2].AlignStyle = FrUIText.enAlignStyle.TOP_LEFT;
		compareTxt[2].Set("font3", "0,000", UIConstant.FONT_COLOR_DESCRIPTION, 200f);
		compareTxt[2].Rect = new Rect(compareTitleImg[2].Rect.x + 64f, compareTitleImg[2].Rect.y - 2f, 200f, compareTitleImg[2].Rect.height);
		stateMgr.m_UIManager.Add(customizeImg);
		stateMgr.m_UIManager.Add(navigationBar);
		mTitleFrame = new UIImage();
		mTitleFrame.AddObject(unitUI, 0, TITLE_FRAME_BEGIN_IMG, TITLE_FRAME_COUNT_IMG);
		mTitleFrame.Rect = mTitleFrame.GetObjectRect();
		stateMgr.m_UIManager.Add(mTitleFrame);
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 3);
		mTitleBG = new UIImage();
		mTitleBG.AddObject(unitUI, 0, 4);
		mTitleBG.Rect = mTitleBG.GetObjectRect();
		mTitleBG.SetSize(new Vector2(modulePositionRect2.width, modulePositionRect2.height));
		Rect modulePositionRect3 = unitUI.GetModulePositionRect(0, 0, 52);
		mTitleText = new FrUIText();
		mTitleText.Rect = modulePositionRect3;
		mTitleText.Set("font3", "Name", UIConstant.FONT_COLOR_EQUIP_NAME, modulePositionRect3.width);
		Rect modulePositionRect4 = unitUI.GetModulePositionRect(0, 0, 53);
		mLevelText = new FrUIText();
		mLevelText.Rect = modulePositionRect4;
		mLevelText.Set("font3", "Level", UIConstant.FONT_COLOR_WEAPON_LEVEL, modulePositionRect4.width);
		mPropertiesTexts = new FrUIText[4]
		{
			new FrUIText(),
			new FrUIText(),
			new FrUIText(),
			new FrUIText()
		};
		for (int i = 0; i < 4; i++)
		{
			Rect modulePositionRect5 = unitUI.GetModulePositionRect(0, 0, 54 + i);
			mPropertiesTexts[i].Rect = modulePositionRect5;
			mPropertiesTexts[i].Set("font3", string.Empty, UIConstant.fontColor_cyan, modulePositionRect5.width);
		}
		stateMgr.m_UIManager.Add(mTitleBG);
		stateMgr.m_UIManager.Add(mTitleText);
		stateMgr.m_UIManager.Add(mLevelText);
		FrUIText[] array = mPropertiesTexts;
		foreach (FrUIText control in array)
		{
			stateMgr.m_UIManager.Add(control);
		}
		mSuitIcon = new UIImage[2, 4];
		for (int k = 0; k < 2; k++)
		{
			for (int l = 0; l < 4; l++)
			{
				mSuitIcon[k, l] = CreateImg(59 + k * 4 + l);
				stateMgr.m_UIManager.Add(mSuitIcon[k, l]);
				mSuitIcon[k, l].Visible = false;
			}
		}
		mLevelIcon = new UIImage[2, 7];
		for (int m = 0; m < 2; m++)
		{
			for (int n = 0; n < 7; n++)
			{
				mLevelIcon[m, n] = new UIImage();
				mLevelIcon[m, n].AddObject(unitUI, 0, 67 + m);
				Rect objectRect2 = mLevelIcon[0, 0].GetObjectRect();
				mLevelIcon[m, n].Rect = new Rect(objectRect2.left + (objectRect2.width + 5f) * (float)n, objectRect2.top, objectRect2.width, objectRect2.height);
				stateMgr.m_UIManager.Add(mLevelIcon[m, n]);
				mLevelIcon[m, n].Visible = true;
			}
		}
		mGunIcon = new UIImage();
		mGunIcon.AddObject(unitUI, 1, 0);
		mGunIcon.Rect = mGunIcon.GetObjectRect();
		mGunIcon.Visible = false;
		stateMgr.m_UIManager.Add(mGunIcon);
		Rect modulePositionRect6 = unitUI.GetModulePositionRect(0, 0, 18);
		avatarFrame = new UIAvatar3D(modulePositionRect6, new Vector3(-0.499798f, 0.0172753f, 3.620711f), new Vector3(1.2f, 1.2f, 1.2f), new Vector3(0f, 150f, 0f));
		avatarFrame.UpdateAnimation();
		avatarFrame.Show();
		InitNavList();
		swapAvatar = new UISliderAvatar();
		swapAvatar.Create(unitUI);
		tagNavImg = new UIImage[6];
		for (int num = 0; num < tagNavImg.Length; num++)
		{
			tagNavImg[num] = new UIImage();
			tagNavImg[num].AddObject(unitUI, 0, 24 + num);
			tagNavImg[num].Rect = tagNavImg[num].GetObjectRect();
			stateMgr.m_UIManager.Add(tagNavImg[num]);
		}
		selectTagNavImg = new UIImage();
		selectTagNavImg.AddObject(unitUI, 0, 23);
		selectTagNavImg.Rect = tagNavImg[curTag].Rect;
		stateMgr.m_UIManager.Add(selectTagNavImg);
		stateMgr.m_UIManager.Add(packTxtBtn);
		stateMgr.m_UIManager.Add(packFlagImg);
		//stateMgr.m_UIManager.Add(iapTxtBtn);
		//stateMgr.m_UIManager.Add(iapFlagImg);
		for (int num2 = 0; num2 < 3; num2++)
		{
			for (int num3 = 0; num3 < 4; num3++)
			{
				compareImg[num2, num3].SetClipOffs(0, new Vector2(10f, 0f));
				compareImg[num2, num3].SetClipOffs(1, new Vector2(0f, 0f));
				compareImg[num2, num3].SetClipOffs(2, new Vector2(-10f, 0f));
				compareImg[num2, num3].SetClipOffs(3, new Vector2(0f, 0f));
				stateMgr.m_UIManager.Add(compareImg[num2, num3]);
			}
		}
		for (int num4 = 0; num4 < 3; num4++)
		{
			stateMgr.m_UIManager.Add(compareTitleImg[num4]);
		}
		for (int num5 = 0; num5 < 3; num5++)
		{
			stateMgr.m_UIManager.Add(compareTxt[num5]);
		}
		stateMgr.m_UIManager.Add(avatarFrame);
		stateMgr.m_UIManager.Add(swapAvatar);
		stateMgr.m_UIManager.Add(refuelTxtBtn);
		stateMgr.m_UIManager.Add(refuelFlagImg);
		mDescription = new UIDialog(stateMgr, 1);
		mDescription.Create();
		mDescription.Show();
		mDescription.AddBGFrame(unitUI, 0, BRIEF_BACKGROUND_BEGIN_IMG, BRIEF_BACKGROUND_COUNT_IMG);
		byte[] module4 = new byte[2] { 21, 73 };
		byte[] module5 = new byte[2] { 20, 74 };
		byte[] module6 = new byte[2] { 19, 69 };
		mDescription.AddButton(0, UIButtonBase.State.Normal, unitUI, 0, module4);
		mDescription.AddButton(0, UIButtonBase.State.Pressed, unitUI, 0, module5);
		mDescription.AddButton(0, UIButtonBase.State.Disabled, unitUI, 0, module6);
		Rect modulePositionRect7 = unitUI.GetModulePositionRect(0, 0, 50);
		mDescription.SetTextShowRect(modulePositionRect7.x, modulePositionRect7.y, modulePositionRect7.width, modulePositionRect7.height);
		mDescription.SetText("font3", "Here is the description of the equipment.", UIConstant.FONT_COLOR_DESCRIPTION, FrUIText.enAlignStyle.TOP_LEFT);
		mDescription.SetBlock(false);
		stateMgr.m_UIManager.Add(mDescription);
		UnitUI unitUI2 = Res2DManager.GetInstance().vUI[17];
		upPriceNum = new UINumeric();
		upPriceNum.AlignStyle = UINumeric.enAlignStyle.center;
		upPriceNum.SpacingOffsetX = -7f;
		upPriceNum.Rect = unitUI.GetModulePositionRect(0, 0, 58);
		stateMgr.m_UIManager.Add(upPriceNum);
		stateMgr.m_UIManager.Add(equipBGImg);
		stateMgr.m_UIManager.Add(equipBtn);
		swapTag = new UISliderTag();
		swapTag.Create(unitUI, 0);
		ResetUITag();
		stateMgr.m_UIPopupManager.Add(swapTag);
		twitterImg = new UIClickButton();
		twitterImg.AddObject(UIButtonBase.State.Normal, unitUI, 0, 84);
		twitterImg.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 86);
		twitterImg.Rect = twitterImg.GetObjectRect(UIButtonBase.State.Normal);
		facebookImg = new UIClickButton();
		facebookImg.AddObject(UIButtonBase.State.Normal, unitUI, 0, 85);
		facebookImg.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 87);
		facebookImg.Rect = facebookImg.GetObjectRect(UIButtonBase.State.Normal);
		if (GameApp.GetInstance().openfreemithril)
		{
			freeGotMithrilImg = new UIClickButton();
			freeGotMithrilImg.AddObject(UIButtonBase.State.Normal, unitUI, 0, 90);
			freeGotMithrilImg.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 90);
			freeGotMithrilImg.Rect = freeGotMithrilImg.GetObjectRect(UIButtonBase.State.Normal);
			stateMgr.m_UIManager.Add(freeGotMithrilImg);
			New = new UIClickButton();
			New.AddObject(UIButtonBase.State.Normal, unitUI, 0, 91);
			New.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 91);
			New.Rect = New.GetObjectRect(UIButtonBase.State.Normal);
			stateMgr.m_UIManager.Add(New);
			if (GameApp.GetInstance().AppStatus == 1)
			{
				freeGotMithrilImg.Visible = true;
				freeGotMithrilImg.Enable = true;
				New.Visible = true;
				New.Enable = true;
			}
			else
			{
				freeGotMithrilImg.Visible = false;
				freeGotMithrilImg.Enable = false;
				New.Visible = false;
				New.Enable = false;
			}
			if (GameApp.GetInstance().GetUserState().GetRewardStatus() == 2)
			{
				New.Visible = false;
			}
		}
		navigationMenu = new NavigationMenuUI(stateMgr);
		navigationMenu.Create();
		navigationMenu.Show();
		stateMgr.m_UIPopupManager.Add(navigationMenu);
		refuel = new RefuelUI(stateMgr);
		refuel.Create();
		refuel.Hide();
		stateMgr.m_UIPopupManager.Add(refuel);
		msgUI = new MessageBoxUI(stateMgr);
		msgUI.Create();
		msgUI.Hide();
		stateMgr.m_UIPopupManager.Add(msgUI);
		iapUI = new IAPUI(stateMgr);
		stateMgr.m_UIPopupManager.Add(iapUI);
		UnityEngine.Object original = Resources.Load("UI/light");
		light = UnityEngine.Object.Instantiate(original, new Vector3(-0.55f, 0f, 2.5f), Quaternion.identity) as GameObject;
		light.transform.position = new Vector3(-0.55f, -0.2f, 3.5f);
		light.transform.Rotate(new Vector3(270f, 0f, 0f));
		light.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		GameObject gameObject = GameObject.Find("Main Camera");
		gameObject.transform.position = new Vector3(-0.5f, 1f, 0f);
		Rect rct = new Rect(0.117f, 0f, 0.62f, 1f);
		gameObject.GetComponent<Camera>().rect = UIConstant.GetRectForScreenAdaptived2(rct);
	}

	public void InitNavList()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[10];
		avatarNavListImg.Clear();
		List<List<Armor>> armor = userState.GetArmor();
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 22);
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 2);
		float num = 0f;
		float num2 = 0f;
		avatarNavListImg = new List<List<UIImage>>();
		List<UIImage> list3;
		for (int i = 0; i < armor.Count; i++)
		{
			List<Armor> list = new List<Armor>();
			List<Armor> list2 = armor[i];
			for (int j = 0; j < list2.Count; j++)
			{
				if (list2[j].Level != 0 && list2[j].Level != 15)
				{
					list.Add(list2[j]);
				}
			}
			list3 = new List<UIImage>();
			num = modulePositionRect2.width / (float)list.Count;
			num = Mathf.Min(30f, num);
			num2 = modulePositionRect.x + modulePositionRect.width * 0.5f - (float)(list.Count - 1) * num * 0.5f;
			for (int k = 0; k < list.Count; k++)
			{
				UIImage uIImage = new UIImage();
				uIImage.AddObject(unitUI, 0, 22);
				uIImage.Rect = new Rect(num2 - modulePositionRect.width * 0.5f, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
				num2 += num;
				list3.Add(uIImage);
			}
			avatarNavListImg.Add(list3);
		}
		list3 = new List<UIImage>();
		List<Weapon> weapons = userState.GetWeapons();
		num = modulePositionRect2.width / (float)weapons.Count;
		num = Mathf.Min(30f, num);
		List<Weapon> list4 = new List<Weapon>();
		for (int l = 0; l < weapons.Count; l++)
		{
			if (weapons[l].Level != 0 && weapons[l].Level != 15)
			{
				list4.Add(weapons[l]);
			}
		}
		num2 = modulePositionRect.x + modulePositionRect.width * 0.5f - (float)(list4.Count - 1) * num * 0.5f;
		for (int m = 0; m < list4.Count; m++)
		{
			UIImage uIImage2 = new UIImage();
			uIImage2.AddObject(unitUI, 0, 22);
			uIImage2.Rect = new Rect(num2 - modulePositionRect.width * 0.5f, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
			num2 += num;
			list3.Add(uIImage2);
		}
		avatarNavListImg.Add(list3);
		for (int n = 0; n < avatarNavListImg.Count; n++)
		{
			for (int num3 = 0; num3 < avatarNavListImg[n].Count; num3++)
			{
				stateMgr.m_UIManager.Add(avatarNavListImg[n][num3]);
			}
		}
		selectAvatarNavImg = new UIImage();
		selectAvatarNavImg.AddObject(unitUI, 0, 23);
		selectAvatarNavImg.Rect = avatarNavListImg[curTag][0].Rect;
		stateMgr.m_UIManager.Add(selectAvatarNavImg);
	}

	public PlayerSkill InitSkills(int[] armors)
	{
		PlayerSkill playerSkill = new PlayerSkill();
		playerSkill.CreateSkills();
		for (int i = 0; i < Global.AVATAR_PART_NUM; i++)
		{
			Armor armor = userState.GetArmor(i, armors[i]);
			List<Skill> skills = armor.GetSkills();
			foreach (Skill item in skills)
			{
				playerSkill.AddSkill(item);
			}
		}
		if (userState.ArmorInOneCollection(armors))
		{
			int armorGroupID = userState.GetArmor(0, armors[0]).GetArmorGroupID();
			ArmorRewards armorRewards = userState.GetArmorRewards()[armorGroupID];
			List<Skill> skills2 = armorRewards.GetSkills();
			foreach (Skill item2 in skills2)
			{
				playerSkill.AddSkill(item2);
			}
		}
		return playerSkill;
	}

	public UIImage CreateImg(int index)
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[10];
		UIImage uIImage = new UIImage();
		uIImage.AddObject(ui, 0, index);
		uIImage.Rect = uIImage.GetObjectRect();
		return uIImage;
	}

	public void ResetUITag()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[10];
		for (int i = 0; i < UIConstant.avatar.Length; i++)
		{
			UISliderTag.UITagIcon uITagIcon = new UISliderTag.UITagIcon();
			uITagIcon.m_background = new UIImage();
			uITagIcon.m_background.AddObject(ui, 0, 17);
			uITagIcon.m_background.Rect = uITagIcon.m_background.GetObjectRect();
			uITagIcon.m_tagIcon = new UIImage();
			uITagIcon.m_tagIcon.AddObject(ui, 0, 42 + i);
			uITagIcon.m_tagIcon.Rect = uITagIcon.m_tagIcon.GetObjectRect();
			uITagIcon.Rect = uITagIcon.m_background.Rect;
			uITagIcon.Visible = true;
			uITagIcon.Id = i;
			swapTag.Add(uITagIcon);
		}
		swapTag.SetSelection(curTag);
	}

	public void SetNextTag(byte tag)
	{
		nextTag = tag;
	}

	public void SetAvatars(byte type)
	{
		for (int i = 0; i < avatarNavListImg.Count; i++)
		{
			for (int j = 0; j < avatarNavListImg[i].Count; j++)
			{
				avatarNavListImg[i][j].Visible = false;
				if (type == i)
				{
					avatarNavListImg[i][j].Visible = true;
				}
			}
		}
		((ShopAndCustomize)stateMgr).LoadAllAvatar(type, false, false);
		swapAvatar.Clear();
		swapAvatar.m_num = 0;
		curTag = type;
		UnitUI ui = Res2DManager.GetInstance().vUI[10];
		Vector2 zero = Vector2.zero;
		Vector3 zero2 = Vector3.zero;
		int selection = 0;
		float num = 120f;
		float num2 = 135f;
		float x = avatarFrame.GetModel().transform.Find(UIConstant.SUB_AVATAR[1]).position.x;
		Vector3 zero3 = Vector3.zero;
		if (type < 5)
		{
			List<Armor> list = new List<Armor>();
			List<List<Armor>> armor = userState.GetArmor();
			List<Armor> list2 = armor[type];
			for (int k = 0; k < list2.Count; k++)
			{
				if (list2[k].Level != 0 && list2[k].Level != 15)
				{
					list.Add(list2[k]);
				}
			}
			int dispCount = GetDispCount(list.Count);
			zero2 = ((type != 4) ? avatarFrame.GetModel().transform.Find(UIConstant.SUB_AVATAR[type]).position : new Vector3(-0.7232f, 1.6f, 3.32f));
			zero3 = list2[m_equipSel[type]].Center;
			int num3 = dispCount / 2;
			int num4 = dispCount;
			zero = swapAvatar.ToPixelScreen(new Vector2(x, zero2.y + 1.2f * zero3.z));
			for (int l = 0; l < dispCount; l++)
			{
				int index = l % list.Count;
				GameObject model = (GameObject)UnityEngine.Object.Instantiate(list[index].Equipment);
				UISliderAvatar.Avatar3D avatar3D = new UISliderAvatar.Avatar3D();
				avatar3D.m_scale = UIConstant.AVATAR_SCALE[list[index].GetArmorID(), type];
				avatar3D.m_obj = new UI3DFrame(new Rect(0f, 0f, 500f, 600f), new Vector3(x, zero2.y, zero2.z), new Vector3(0f, 205f, 0f));
				avatar3D.m_obj.SetModel(model);
				if (list[index].GetArmorID() == m_equipSel[type] && l - num3 < num4)
				{
					selection = l;
					num4 = l - num3;
				}
				Transform transform = avatar3D.m_obj.GetModel().transform.Find(UIConstant.SUB_AVATAR[type]);
				if (transform != null)
				{
					avatar3D.m_obj.AddSubModel(transform.gameObject);
				}
				else
				{
					avatar3D.m_obj.AddSubModel(avatar3D.m_obj.GetModel());
				}
				avatar3D.m_obj.CenterOffset = new Vector3(list[index].Center.x, list[index].Center.z, list[index].Center.y);
				if (type == 4)
				{
					if (list[index].GetArmorID() == 15 || list[index].GetArmorID() == 23)
					{
						avatar3D.m_obj.SetRotate(new Vector3(0f, 330f, 0f));
					}
					else if (list[index].GetArmorID() == 21)
					{
						avatar3D.m_obj.SetRotate(new Vector3(-10f, 330f, 30f));
					}
					else if (list[index].GetArmorID() == 24)
					{
						avatar3D.m_obj.SetRotate(new Vector3(0f, 150f, 0f));
					}
					else
					{
						avatar3D.m_obj.SetRotate(new Vector3(270f, 0f, 330f));
					}
					avatar3D.m_pos.x = UIConstant.BAG_OFFSET[list[index].GetArmorID(), 0];
					avatar3D.m_pos.y = UIConstant.BAG_OFFSET[list[index].GetArmorID(), 1];
				}
				else
				{
					avatar3D.m_obj.SetRotate(new Vector3(0f, 205f, 0f));
				}
				avatar3D.m_obj.SetScale(new Vector3(avatar3D.m_scale, avatar3D.m_scale, avatar3D.m_scale));
				float num5 = (0f - (avatar3D.m_scale - 1.2f)) * list[index].Center.z;
				avatar3D.m_obj.SetPosition(new Vector3(x, zero2.y + num5, zero2.z));
				Transform transform2 = avatar3D.m_obj.GetModel().transform.Find("lock(Clone)");
				transform2.gameObject.transform.position = new Vector3(x, zero2.y + list[index].Center.z, 2.5f);
				transform2.gameObject.transform.LookAt(Camera.main.transform);
				transform2.gameObject.SetActiveRecursively(false);
				Transform transform3 = avatar3D.m_obj.GetModel().transform.Find("unlock(Clone)");
				transform3.gameObject.transform.position = new Vector3(x, zero2.y + list[index].Center.z, 2.5f);
				transform3.gameObject.transform.LookAt(Camera.main.transform);
				transform3.gameObject.SetActiveRecursively(false);
				avatar3D.m_state = list[index].Level;
				avatar3D.m_obj.Show();
				avatar3D.m_Lock = new UIImage();
				avatar3D.m_Lock.AddObject(ui, 0, 17);
				avatar3D.m_Lock.Rect = avatar3D.m_Lock.GetObjectRect();
				avatar3D.Rect = new Rect(zero.x - num * 0.5f, zero.y - num2 * 0.5f, num, num2);
				avatar3D.Id = list[index].GetArmorID();
				avatar3D.Add(avatar3D.m_obj);
				avatar3D.Add(avatar3D.m_Lock);
				avatar3D.Show();
				swapAvatar.Add(avatar3D);
			}
		}
		else
		{
			List<Weapon> list3 = new List<Weapon>();
			List<Weapon> weapons = userState.GetWeapons();
			for (int m = 0; m < weapons.Count; m++)
			{
				if (weapons[m].Level != 0 && weapons[m].Level != 15)
				{
					list3.Add(weapons[m]);
				}
			}
			int dispCount2 = GetDispCount(list3.Count);
			int num6 = dispCount2 / 2;
			int num7 = dispCount2;
			zero2 = new Vector3(-0.7232f, 1.1f, 3.32f);
			zero3 = weapons[m_equipSel[type]].Center;
			zero = swapAvatar.ToPixelScreen(new Vector2(x, zero2.y + 0.120000005f));
			for (int n = 0; n < dispCount2; n++)
			{
				int index2 = n % list3.Count;
				GameObject model2 = (GameObject)UnityEngine.Object.Instantiate(list3[index2].Gun);
				UISliderAvatar.Avatar3D avatar3D2 = new UISliderAvatar.Avatar3D();
				avatar3D2.m_scale = UIConstant.WEAPON_SCALE[list3[index2].GunID];
				avatar3D2.m_obj = new UI3DFrame(new Rect(0f, 0f, 500f, 600f), new Vector3(list3[index2].Center.x, zero2.y, zero2.z), new Vector3(0f, 205f, 0f));
				avatar3D2.m_obj.SetModel(model2);
				if (list3[index2].GunID == m_equipSel[type] && n - num6 < num7)
				{
					selection = n;
					num7 = n - num6;
				}
				if (list3[index2].GunID == 23)
				{
					avatar3D2.m_obj.AddSubModel(list3[index2].Gun.transform.GetChild(0).gameObject);
					avatar3D2.m_obj.AddSubModel(list3[index2].Gun.transform.GetChild(1).gameObject);
				}
				else if (list3[index2].GunID == 24 || list3[index2].GunID == 25)
				{
					avatar3D2.m_obj.AddSubModel(list3[index2].Gun.transform.GetChild(1).gameObject);
				}
				else if (list3[index2].GunID == 22)
				{
					avatar3D2.m_obj.AddSubModel(list3[index2].Gun.transform.GetChild(0).gameObject);
					avatar3D2.m_obj.AddSubModel(list3[index2].Gun.transform.GetChild(1).gameObject);
					avatar3D2.m_obj.AddSubModel(list3[index2].Gun.transform.GetChild(2).gameObject);
				}
				else if (n != 26 && n != 27 && n != 28 && n != 29 && n != 30 && n != 31)
				{
					if (list3[index2].GunID == 44)
					{
						avatar3D2.m_obj.AddSubModel(list3[index2].Gun.transform.GetChild(0).gameObject);
						avatar3D2.m_obj.AddSubModel(list3[index2].Gun.transform.GetChild(1).gameObject);
						avatar3D2.m_obj.AddSubModel(list3[index2].Gun.transform.GetChild(2).gameObject);
					}
					else
					{
						avatar3D2.m_obj.AddSubModel(avatar3D2.m_obj.GetModel());
					}
				}
				avatar3D2.m_obj.CenterOffset = new Vector3(list3[index2].Center.y, list3[index2].Center.z, list3[index2].Center.y);
				avatar3D2.m_pos.x = UIConstant.WEAPON_OFFSET[list3[index2].GunID, 0];
				avatar3D2.m_pos.y = UIConstant.WEAPON_OFFSET[list3[index2].GunID, 1];
				avatar3D2.m_obj.SetScale(new Vector3(avatar3D2.m_scale, avatar3D2.m_scale, avatar3D2.m_scale));
				float num8 = (0f - (1.2f - avatar3D2.m_scale)) * list3[index2].Center.y;
				avatar3D2.m_obj.SetPosition(new Vector3(x, zero2.y + num8, zero2.z));
				Transform transform4 = avatar3D2.m_obj.GetModel().transform.Find("lock(Clone)");
				transform4.gameObject.transform.position = new Vector3(x + list3[index2].Center.y, zero2.y + list3[index2].Center.z, 2.5f);
				transform4.gameObject.transform.LookAt(Camera.main.transform);
				transform4.gameObject.SetActiveRecursively(false);
				Transform transform5 = avatar3D2.m_obj.GetModel().transform.Find("unlock(Clone)");
				transform5.gameObject.transform.position = new Vector3(x + list3[index2].Center.y, zero2.y + list3[index2].Center.z, 2.5f);
				transform5.gameObject.transform.LookAt(Camera.main.transform);
				transform5.gameObject.SetActiveRecursively(false);
				avatar3D2.m_state = list3[index2].Level;
				avatar3D2.m_obj.Show();
				avatar3D2.m_Lock = new UIImage();
				avatar3D2.m_Lock.AddObject(ui, 0, 17);
				avatar3D2.m_Lock.Rect = avatar3D2.m_Lock.GetObjectRect();
				avatar3D2.Rect = new Rect(zero.x - 60f, zero.y - 67.5f, 120f, 135f);
				avatar3D2.Id = list3[index2].GunID;
				avatar3D2.m_order = list3[index2].DisplayOrder;
				avatar3D2.Add(avatar3D2.m_obj);
				avatar3D2.Add(avatar3D2.m_Lock);
				avatar3D2.Show();
				Sort(avatar3D2);
			}
		}
		swapAvatar.SetInfos(zero, num, num2);
		swapAvatar.SetClipRect(zero.x - 5f * num * 0.5f, zero.y - num2 * 0.5f, 5f * num, num2);
		swapAvatar.SetScroller(0f, swapAvatar.m_avatar3D.Count * 120, 120f, new Rect(zero.x - 5f * num * 0.5f, zero.y - num2 * 0.5f, 5f * num, num2));
		swapAvatar.SetSelection(selection);
		swapAvatar.SetState(1);
	}

	private bool Sort(UISliderAvatar.Avatar3D avatar)
	{
		if (swapAvatar.m_avatar3D.Count == 0)
		{
			swapAvatar.Add(avatar);
		}
		else
		{
			int num = avatar.m_order;
			for (int i = 0; i < swapAvatar.m_avatar3D.Count; i++)
			{
				UISliderAvatar.Avatar3D avatar3D = swapAvatar.m_avatar3D[i];
				if (avatar.Id == avatar3D.Id && num <= avatar3D.m_order)
				{
					num = avatar3D.m_order + 100;
				}
			}
			avatar.m_order = num;
			float num2 = avatar.m_order;
			int num3 = 0;
			int num4 = swapAvatar.m_avatar3D.Count - 1;
			int num5 = (num3 + num4) / 2;
			if (num2 <= (float)swapAvatar.m_avatar3D[num3].m_order)
			{
				swapAvatar.Insert(num3, avatar);
			}
			else if (num2 >= (float)swapAvatar.m_avatar3D[num4].m_order)
			{
				swapAvatar.Insert(num4 + 1, avatar);
			}
			else
			{
				while (num4 - num3 > 1)
				{
					float num6 = swapAvatar.m_avatar3D[num5].m_order;
					if (num2 == num6)
					{
						num3 = num5;
						break;
					}
					if (num2 > num6)
					{
						num3 = num5;
					}
					else
					{
						num4 = num5;
					}
					num5 = (num3 + num4) / 2;
				}
				swapAvatar.Insert(num3 + 1, avatar);
			}
		}
		return true;
	}

	private int GetDispCount(int count)
	{
		int result = count;
		switch (count)
		{
		case 1:
			result = 6;
			break;
		case 2:
			result = 6;
			break;
		case 3:
			result = 6;
			break;
		case 4:
			result = 8;
			break;
		case 5:
			result = 10;
			break;
		}
		return result;
	}

	public void CreateClone()
	{
		int[] avatar = userState.GetAvatar();
		for (int i = 0; i < avatar.Length; i++)
		{
			m_equipSel[i] = avatar[i];
		}
		Weapon weapon = GameApp.GetInstance().GetUserState().GetBattleWeapons()[0];
		m_equipSel[5] = weapon.GetGunID();
		playerSkillPrev = InitSkills(avatar);
		playerSkill = InitSkills(m_equipSel);
	}

	public void SetWeapons()
	{
		swapAvatar.m_avatar3D.Clear();
	}

	public bool Update()
	{
		switch (state)
		{
		case 0:
			state = 1;
			userState = GameApp.GetInstance().GetUserState();
			Create();
			break;
		case 1:
			state = 2;
			SetAvatars(curTag);
			UpdateDescription();
			break;
		case 2:
		{
			if (avatarFrame != null)
			{
				avatarFrame.Update();
			}
			if (swapAvatar != null && swapAvatar.m_state == 3 && nextTag != curTag)
			{
				SetAvatars(nextTag);
				mNeedUpdateDescrition = true;
			}
			UpdateComparsion();
			UpdateAvatarNav();
			UpdateDescription();
			if (refuel.Visible)
			{
				refuel.UpdateEnergy();
			}
			ShowRewardMsg();
			ShowPromotion();
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
		ShowMovieMsg();
		return false;
	}

	public void ShowMovieMsg()
	{
		if (GameApp.GetInstance().GetUserState().showmovie)
		{
			string msg = "Watch video of our game!! ";
			msgUI.CreateConfirmMovie(msg, MessageBoxUI.MESSAGE_FLAG_MOVIE, MessageBoxUI.EVENT_SHOW_MOVIE);
			msgUI.Show();
			GameApp.GetInstance().GetUserState().showmovie = false;
		}
	}

	public void ShowMovie()
	{
		Debug.Log("showmovie");
		userState.SetShowMovieDate(DateTime.Now.ToString("yyyy-MM-dd"));
		userState.AddShowMovieTime();
		GameApp.GetInstance().Save();
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

	public void UpdateAvatar()
	{
		SetAvatars(curTag);
		mNeedUpdateDescrition = true;
	}

	private void UpdateAvatarNav()
	{
		for (int i = 0; i < swapAvatar.m_avatar3D.Count; i++)
		{
			if (swapAvatar.m_avatar3D[i].Id == m_equipSel[curTag])
			{
				selectAvatarNavImg.Rect = avatarNavListImg[curTag][i % avatarNavListImg[curTag].Count].Rect;
				break;
			}
		}
	}

	private void UpdateDescription()
	{
		if (!mNeedUpdateDescrition)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		UnitUI unitUI = Res2DManager.GetInstance().vUI[10];
		if (curTag < 4)
		{
			int armorGroupID = userState.GetArmor(curTag, m_equipSel[curTag]).GetArmorGroupID();
			for (int i = 0; i < 4; i++)
			{
				int armorGroupID2 = userState.GetArmor(i, m_equipSel[i]).GetArmorGroupID();
				if (armorGroupID2 == armorGroupID)
				{
					mSuitIcon[0, i].Visible = false;
					mSuitIcon[1, i].Visible = true;
				}
				else
				{
					mSuitIcon[0, i].Visible = true;
					mSuitIcon[1, i].Visible = false;
					flag = true;
				}
			}
		}
		else
		{
			for (int j = 0; j < 2; j++)
			{
				for (int k = 0; k < 4; k++)
				{
					mSuitIcon[j, k].Visible = false;
				}
			}
		}
		string empty = string.Empty;
		string text = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string text2 = string.Empty;
		int num = m_equipSel[curTag];
		for (int l = 0; l < 4; l++)
		{
			mPropertiesTexts[l].SetText(string.Empty);
		}
		if (curTag < 5)
		{
			Armor armor = userState.GetArmor(curTag, num);
			empty = armor.Name;
			if (userState.GetAvatar()[curTag] == num)
			{
				flag2 = true;
			}
			List<Skill> skills = armor.GetSkills();
			int num2 = 0;
			foreach (Skill item in skills)
			{
				empty2 = string.Empty;
				if (item.skillType <= SkillsType.MONEY_BOOTH && item.data != 0f)
				{
					if (playerSkill.HasSign(item.skillType) && item.data > 0f)
					{
						empty2 += "+";
					}
					empty2 = ((!playerSkill.IsPercetage(item.skillType)) ? (empty2 + item.data) : (empty2 + string.Format("{0:P0}", item.data)));
					empty2 += " ";
					empty2 += UIConstant.PROPERTY_NAME[(int)item.skillType];
					empty2 += "\n";
					mPropertiesTexts[num2].Set("font3", empty2, UIConstant.PROPERTY_COLOR[(int)item.skillType]);
					num2++;
				}
			}
			for (int m = 0; m < 2; m++)
			{
				for (int n = 0; n < 7; n++)
				{
					mLevelIcon[m, n].Visible = false;
				}
			}
			mGunIcon.Visible = false;
		}
		else
		{
			Weapon weapon = userState.GetWeapons()[num];
			empty = weapon.Name;
			mGunIcon.Visible = true;
			if (weapon.GetWeaponType() == WeaponType.PlasmaNeo)
			{
				mGunIcon.SetTexture(unitUI, 1, 0);
			}
			else if (weapon.GetWeaponType() == WeaponType.LaserRifle || weapon.GetWeaponType() == WeaponType.LaserGun || weapon.GetWeaponType() == WeaponType.AdvancedShotGun)
			{
				mGunIcon.SetTexture(unitUI, 1, 2);
			}
			else if (weapon.GetWeaponType() == WeaponType.RocketLauncher || weapon.GetWeaponType() == WeaponType.GrenadeLauncher || weapon.GetWeaponType() == WeaponType.AdvancedGrenadeLauncher || weapon.GetWeaponType() == WeaponType.LightBow || weapon.GetWeaponType() == WeaponType.LightFist || weapon.GetWeaponType() == WeaponType.AutoRocketLauncher || weapon.GetWeaponType() == WeaponType.AutoBow || weapon.GetWeaponType() == WeaponType.TheArrow || weapon.GetWeaponType() == WeaponType.FlyGrenadeLauncher)
			{
				mGunIcon.SetTexture(unitUI, 1, 4);
			}
			else if (weapon.GetWeaponType() == WeaponType.Sword || weapon.GetWeaponType() == WeaponType.AdvancedSword)
			{
				mGunIcon.SetTexture(unitUI, 1, 8);
			}
			else if (weapon.GetWeaponType() == WeaponType.Sniper || weapon.GetWeaponType() == WeaponType.AdvancedSniper || weapon.GetWeaponType() == WeaponType.RelectionSniper)
			{
				mGunIcon.SetTexture(unitUI, 1, 10);
			}
			else if (weapon.GetWeaponType() == WeaponType.PingPongLauncher)
			{
				mGunIcon.SetTexture(unitUI, 1, 14);
			}
			else if (weapon.GetWeaponType() == WeaponType.TrackingGun)
			{
				mGunIcon.SetTexture(unitUI, 1, 12);
			}
			else
			{
				mGunIcon.SetTexture(unitUI, 1, 6);
			}
			Weapon weapon2 = GameApp.GetInstance().GetUserState().GetBattleWeapons()[0];
			if (weapon2.GunID == weapon.GunID)
			{
				flag2 = true;
			}
			text = "Lv." + weapon.Level;
			mLevelText.SetColor(UIConstant.FONT_COLOR_WEAPON_LEVEL);
			for (int num3 = 0; num3 < weapon.Level - 1; num3++)
			{
				mLevelIcon[0, num3].Visible = false;
				mLevelIcon[1, num3].Visible = true;
			}
			for (int num4 = weapon.Level - 1; num4 < 7; num4++)
			{
				mLevelIcon[0, num4].Visible = true;
				mLevelIcon[1, num4].Visible = false;
			}
			string empty4 = string.Empty;
			int num5 = (int)weapon.SimpleDamage();
			empty4 = num5.ToString();
			if (weapon.Level < Global.MAX_LEVEL_WEAPONW)
			{
				flag3 = true;
				text2 = "$" + string.Format("{0:N0}", weapon.GetDamageUpgradePrice());
				string text3 = empty4;
				empty4 = text3 + "(+" + (int)(weapon.SimpleDamage(weapon.Level + 1) - (float)num5) + ")";
			}
			empty2 = "POW " + empty4;
			mPropertiesTexts[0].Set("font3", empty2, UIConstant.PROPERTY_COLOR[1]);
			empty2 = "FIRERATE " + string.Format("{0:N2}", weapon.AttackFrequency);
			mPropertiesTexts[1].Set("font3", empty2, UIConstant.PROPERTY_COLOR[1]);
			empty2 = "ENG " + weapon.EnegyConsume;
			mPropertiesTexts[2].Set("font3", empty2, UIConstant.PROPERTY_COLOR[1]);
			if (weapon.GetSpeedDrag() != 0f)
			{
				empty2 = "SPD " + weapon.GetSpeedDrag();
				mPropertiesTexts[3].Set("font3", empty2, UIConstant.PROPERTY_COLOR[1]);
			}
		}
		string[] array = null;
		switch (curTag)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			array = mBonusDescriptions;
			break;
		case 4:
			array = mPackDescriptions;
			break;
		case 5:
			array = mWeaponDescriptions;
			break;
		}
		if (array != null && num < array.Length)
		{
			empty3 += array[num];
			empty3 = empty3.Replace("[EMPTY]", string.Empty);
			empty3 = empty3.Replace("[n]", "\n");
			if (curTag < 5)
			{
				List<Skill> skills2;
				if (curTag < 4)
				{
					skills2 = userState.GetArmorRewards()[num].GetSkills();
				}
				else
				{
					Armor armor2 = userState.GetArmor(curTag, num);
					empty3 = empty3.Replace(UIConstant.BAG_SLOT_STRING, Convert.ToString(armor2.BagNum));
					skills2 = userState.GetArmor(curTag, num).GetSkills();
				}
				foreach (Skill item2 in skills2)
				{
					string text4 = string.Empty;
					if (playerSkill.HasSign(item2.skillType) && item2.data > 0f)
					{
						text4 += "+";
					}
					if (playerSkill.IsPercetage(item2.skillType))
					{
						float num6 = item2.data * 100f;
						text4 = text4 + num6 + "%";
					}
					else
					{
						text4 += item2.data;
					}
					empty3 = empty3.Replace(UIConstant.KEY_WORD[(int)item2.skillType], text4);
				}
			}
		}
		else
		{
			empty3 = "The description text is not ready.";
		}
		mTitleText.SetText(empty);
		mLevelText.SetText(text);
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 50);
		if (curTag < 5)
		{
			mDescription.SetTextShowRect(modulePositionRect.x, modulePositionRect.y - 58f, modulePositionRect.width, modulePositionRect.height + 58f);
			mDescription.m_ownBtn.SetVisible(0, false);
		}
		else
		{
			mDescription.SetTextShowRect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
			mDescription.m_ownBtn.SetVisible(0, true);
		}
		mDescription.SetText("font3", empty3, (!flag) ? UIConstant.FONT_COLOR_DESCRIPTION : UIConstant.FONT_COLOR_DESCRIPTION_LOCK, FrUIText.enAlignStyle.TOP_LEFT);
		UnitUI ui = Res2DManager.GetInstance().vUI[17];
		if (text2.Length > 0)
		{
			mDescription.m_ownBtn.SetEnable(0, true);
			upPriceNum.Visible = true;
			upPriceNum.SetNumeric(ui, 0, text2);
		}
		else
		{
			upPriceNum.Visible = false;
			mDescription.m_ownBtn.SetEnable(0, false);
		}
		if (flag2)
		{
			equipBtn.Enable = false;
			equipBGImg.SetTexture(unitUI, 0, 76);
			equipBGImg.Rect = equipBGImg.GetObjectRect();
		}
		else
		{
			equipBtn.Enable = true;
			equipBGImg.SetTexture(unitUI, 0, 75);
			equipBGImg.Rect = equipBGImg.GetObjectRect();
		}
		mNeedUpdateDescrition = false;
	}

	private void UpdateComparsion()
	{
		float num = compareImg[0, 1].Rect.width - 10f;
		float skill = playerSkill.GetSkill(SkillsType.HP_BOOTH);
		float skill2 = playerSkillPrev.GetSkill(SkillsType.HP_BOOTH);
		Weapon weapon = userState.GetWeapons()[m_equipSel[5]];
		float damageAfterBoothByArmor = weapon.GetDamageAfterBoothByArmor(playerSkill);
		Weapon weapon2 = userState.GetBattleWeapons()[0];
		float damageAfterBoothByArmor2 = weapon2.GetDamageAfterBoothByArmor(playerSkillPrev);
		float num2 = Mathf.Max(3.5f, playerSkill.GetSkill(SkillsType.SPEED_BOOTH) + 7f + weapon.GetSpeedDrag());
		float num3 = Mathf.Max(3.5f, playerSkillPrev.GetSkill(SkillsType.SPEED_BOOTH) + 7f + weapon2.GetSpeedDrag());
		float num4 = num3 * num / 15f;
		float num5 = num2 * num / 15f;
		float num6 = 45f * Mathf.Pow(skill2 / 10f, 0.2f) - 73f;
		float num7 = 45f * Mathf.Pow(skill / 10f, 0.2f) - 73f;
		float num8 = 45f * Mathf.Pow(damageAfterBoothByArmor2, 0.2f) - 73f;
		float num9 = 45f * Mathf.Pow(damageAfterBoothByArmor, 0.2f) - 73f;
		int num10 = (int)(skill2 - skill);
		if (num10 > 0)
		{
			compareTitleImg[0].SetTextColor(Color.red);
			compareTitleImg[0].SetText("-" + num10);
		}
		else if (num10 < 0)
		{
			compareTitleImg[0].SetTextColor(Color.green);
			compareTitleImg[0].SetText("+" + -num10);
		}
		else
		{
			compareTitleImg[0].SetTextColor(UIConstant.FONT_COLOR_DESCRIPTION);
			compareTitleImg[0].SetText(string.Empty);
		}
		int num11 = (int)(damageAfterBoothByArmor2 - damageAfterBoothByArmor);
		if (num11 > 0)
		{
			compareTitleImg[1].SetTextColor(Color.red);
			compareTitleImg[1].SetText("-" + num11);
		}
		else if (num11 < 0)
		{
			compareTitleImg[1].SetTextColor(Color.green);
			compareTitleImg[1].SetText("+" + -num11);
		}
		else
		{
			compareTitleImg[1].SetTextColor(UIConstant.FONT_COLOR_DESCRIPTION);
			compareTitleImg[1].SetText(string.Empty);
		}
		int num12 = (int)(num3 - num2);
		if (num12 > 0)
		{
			compareTitleImg[2].SetTextColor(Color.red);
			compareTitleImg[2].SetText("-" + num12);
		}
		else if (num12 < 0)
		{
			compareTitleImg[2].SetTextColor(Color.green);
			compareTitleImg[2].SetText("+" + -num12);
		}
		else
		{
			compareTitleImg[2].SetTextColor(UIConstant.FONT_COLOR_DESCRIPTION);
			compareTitleImg[2].SetText(string.Empty);
		}
		compareTxt[0].SetText(UIConstant.FormatNum((int)skill));
		compareTxt[1].SetText(UIConstant.FormatNum((int)damageAfterBoothByArmor));
		compareTxt[2].SetText(UIConstant.FormatNum((int)num2));
		DrawComparsion(0, num6 + 10f, num7 + 10f);
		DrawComparsion(1, num8 + 10f, num9 + 10f);
		DrawComparsion(2, num4 + 10f, num5 + 10f);
	}

	public void DrawComparsion(int type, float oldValue, float newValue)
	{
		compareImg[type, 1].Visible = false;
		compareImg[type, 2].Visible = false;
		compareImg[type, 3].Visible = false;
		float x = compareImg[type, 1].Rect.x;
		if (newValue > oldValue)
		{
			compareImg[type, 3].Visible = true;
			compareImg[type, 1].Visible = true;
			compareImg[type, 3].SetClip(new Rect(x, compareImg[type, 3].Rect.y, oldValue, compareImg[type, 3].Rect.height));
			compareImg[type, 1].SetClip(new Rect(x, compareImg[type, 3].Rect.y, newValue, compareImg[type, 3].Rect.height));
		}
		else if (newValue < oldValue)
		{
			compareImg[type, 2].Visible = true;
			compareImg[type, 3].Visible = true;
			compareImg[type, 2].SetClip(new Rect(x, compareImg[type, 3].Rect.y, oldValue, compareImg[type, 3].Rect.height));
			compareImg[type, 3].SetClip(new Rect(x, compareImg[type, 3].Rect.y, newValue, compareImg[type, 3].Rect.height));
		}
		else
		{
			compareImg[type, 3].Visible = true;
			compareImg[type, 3].SetClip(new Rect(x, compareImg[type, 3].Rect.y, oldValue, compareImg[type, 3].Rect.height));
		}
	}

	public void ChangeBag(Armor src, Armor dst)
	{
		int num = src.BagNum - dst.BagNum;
		int bagNum = userState.GetBagNum();
		userState.SetBagNum(dst.BagNum);
		byte[] array = new byte[Global.BAG_MAX_NUM];
		if (num <= 0)
		{
			return;
		}
		int[] propsInBag = userState.GetPropsInBag();
		array[0] = (byte)(userState.GetBattleWeapons()[0].GunID + 1);
		int num2 = 1;
		for (int i = 0; i < propsInBag.Length; i++)
		{
			int num3 = propsInBag[i] + 1;
			if (num2 < dst.BagNum)
			{
				if (num3 != array[0])
				{
					array[num2] = (byte)num3;
					num2++;
				}
			}
			else
			{
				userState.InsertPropsToStorage(num3, 1);
			}
		}
		userState.SetBagPosition(array);
	}

	public void ShowMsgTwitterPosted()
	{
		if (msgUI != null)
		{
			string msg = UIConstant.GetMessage(41).Replace("[n]", "\n");
			msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_SEND_TWITTER_CONFIRM);
			msgUI.Show();
		}
	}

	public void ShowMsgFacebookPosted()
	{
		if (msgUI != null)
		{
			string msg = UIConstant.GetMessage(38).Replace("[n]", "\n");
			msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_SEND_FACEBOOK_CONFIRM);
			msgUI.Show();
		}
	}

	public void ShowRewardMsg()
	{
		if (GameApp.GetInstance().GetUserState().showRewardMsg && !msgUI.IsVisiable())
		{
			string msg = "Congratulations, you have earned " + GameApp.GetInstance().GetUserState().rewardNumber + " mithril from " + GameApp.GetInstance().GetUserState().rewardAdsName + ".";
			msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_MITHRIL, MessageBoxUI.EVENT_AD_REWARD);
			msgUI.Show();
			GameApp.GetInstance().GetUserState().showRewardMsg = false;
		}
	}

	public AdsUI GetAds()
	{
		return adsui;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == freeGotMithrilImg)
		{
			adsui = new AdsUI(stateMgr);
			adsui.Create();
			stateMgr.m_UIPopupManager.Add(adsui);
			adsui.Show();
		}
		if (control == navigationBar)
		{
			avatarFrame.GetModel().SetActiveRecursively(false);
			light.SetActiveRecursively(false);
			foreach (UISliderAvatar.Avatar3D item in swapAvatar.m_avatar3D)
			{
				item.m_obj.GetModel().SetActiveRecursively(false);
			}
			stateMgr.FrFree();
			UIConstant.ExitShopAndCustomize();
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
			case 4:
				stateMgr.FrGoToPhase(8, false, false, true);
				break;
			case 5:
				stateMgr.FrGoToPhase(11, false, false, true);
				break;
			}
		}
		else if (control == swapTag)
		{
			if (command == 1 && (float)(int)curTag != wparam)
			{
				selectTagNavImg.Rect = tagNavImg[(byte)wparam].Rect;
				SetNextTag((byte)wparam);
				swapAvatar.SetState(2);
				AudioManager.GetInstance().PlaySound(AudioName.SWITCH_ITEM);
			}
		}
		else if (control == packTxtBtn)
		{
			stateMgr.FrGoToPhase(5, false, false, true);
		}
		else if (control == swapAvatar)
		{
			if (command == 2)
			{
				m_equipSel[curTag] = (int)wparam;
				avatarFrame.ChangeAvatar(m_equipSel);
				playerSkill = InitSkills(m_equipSel);
				mNeedUpdateDescrition = true;
				if (curTag == 5)
				{
					AudioManager.GetInstance().PlaySound(AudioName.MOUNT_WEAPON);
				}
				else
				{
					AudioManager.GetInstance().PlaySound(AudioName.MOUNT_GEARS);
				}
			}
		}
		else if (control == mDescription)
		{
			if (curTag == 5 && command == 9)
			{
				Weapon weapon = userState.GetWeapons()[m_equipSel[5]];
				if (weapon.Level != 0 && weapon.Level < Global.MAX_LEVEL_WEAPONW)
				{
					int num = (int)(weapon.SimpleDamage(weapon.Level + 1) - weapon.SimpleDamage());
					string text = UIConstant.GetMessage(15).Replace("[n]", "\n");
					text = text.Replace(UIConstant.UPGRADE_STRING, "+" + num);
					msgUI.CreateQuery(text, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_UPGRADE);
					msgUI.Show();
				}
				AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			}
		}
		else if (control == refuelTxtBtn)
		{
			stateMgr.m_UIManager.SetEnable(false);
			refuel.Init();
			refuel.Show();
		}
		else if (control == iapTxtBtn)
		{
			iapUI.Create();
			iapUI.SetSelection(0);
			iapUI.Show();
		}
		else if (control == refuel)
		{
			if (command == 0)
			{
				userState.Buy(refuel.cash);
				userState.Enegy += refuel.energy;
				refuel.Hide();
				stateMgr.m_UIManager.SetEnable(true);
				GameApp.GetInstance().Save();
			}
			else
			{
				refuel.Hide();
				stateMgr.m_UIManager.SetEnable(true);
			}
		}
		else if (control == equipBtn)
		{
			if (curTag == 5)
			{
				AudioManager.GetInstance().PlaySound(AudioName.MOUNT_WEAPON);
			}
			else
			{
				AudioManager.GetInstance().PlaySound(AudioName.MOUNT_GEARS);
			}
			Weapon weapon2 = userState.GetBattleWeapons()[0];
			int weaponBagIndex = userState.GetWeaponBagIndex(weapon2);
			int indexFromStorage = userState.GetIndexFromStorage(m_equipSel[5] + 1);
			if (indexFromStorage != -1)
			{
				userState.SetPropsToStorage(indexFromStorage, weapon2.GunID + 1, 1);
			}
			else
			{
				indexFromStorage = userState.GetWeaponBagIndex(m_equipSel[5]);
				userState.SetBagPosition(indexFromStorage, weapon2.GunID + 1);
			}
			userState.SetBagPosition(weaponBagIndex, m_equipSel[5] + 1);
			int num2 = userState.GetAvatar()[4];
			if (num2 != m_equipSel[4])
			{
				Armor armor = userState.GetArmor(4, num2);
				Armor armor2 = userState.GetArmor(4, m_equipSel[4]);
				ChangeBag(armor, armor2);
			}
			for (int i = 0; i < Global.AVATAR_PART_NUM; i++)
			{
				userState.SetAvatar((BodyType)i, m_equipSel[i]);
			}
			playerSkillPrev = InitSkills(userState.GetAvatar());
			mNeedUpdateDescrition = true;
			equipBtn.Enable = false;
		}
		else if (control == twitterImg)
		{
			string update = UIConstant.HEY + AndroidSwPluginScript.GetVersionUrl();
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				if (!TwitterAndroid.isLoggedIn())
				{
					Lobby.GetInstance().IsPostingScoreToSocialNetwork = false;
					TwitterAndroid.showLoginDialog();
					return;
				}
				ScreenCapture.CaptureScreenshot("tempscreens.png");
				string url = Application.persistentDataPath + "/tempscreens.png";
				WWW wWW = new WWW(url);
				TwitterAndroid.postUpdateWithImage(update, wWW.bytes);
				wWW = null;
			}
		}
		else if (control == facebookImg)
		{
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				ScreenCapture.CaptureScreenshot("tempscreens.png");
				if (!FacebookAndroid.isLoggedIn())
				{
					Lobby.GetInstance().IsPostingScoreToSocialNetwork = false;
					FacebookAndroid.login();
					return;
				}
				string url2 = Application.persistentDataPath + "/tempscreens.png";
				WWW wWW2 = new WWW(url2);
				Facebook.instance.postMessageWithLinkAndLinkToImage(UIConstant.SHOW_EQUIP_STR, AndroidSwPluginScript.GetVersionUrl(), "Star Warfare:Aliens Invasion", "http://125.141.149.48/iTunesArtwork.png", "Join us!", null);
				Facebook.instance.postImage(wWW2.bytes, "Hey! I am playing Star Warfare!", null);
				wWW2 = null;
			}
		}
		else
		{
			if (control != msgUI)
			{
				return;
			}
			int eventID = msgUI.GetEventID();
			if (eventID == MessageBoxUI.EVENT_UPGRADE)
			{
				switch (command)
				{
				case 10:
				{
					Weapon weapon3 = userState.GetWeapons()[m_equipSel[5]];
					if (weapon3.Level != 0 && weapon3.Level < Global.MAX_LEVEL_WEAPONW)
					{
						if (userState.GetCash() >= weapon3.GetDamageUpgradePrice())
						{
							userState.Buy(weapon3.GetDamageUpgradePrice());
							weapon3.Upgrade();
							mNeedUpdateDescrition = true;
							if (weapon3.Level == Global.MAX_LEVEL_WEAPONW)
							{
								userState.Achievement.FullUpgrades();
							}
							GameApp.GetInstance().Save();
							msgUI.Hide();
						}
						else
						{
							string msg = UIConstant.GetMessage(12).Replace("[n]", "\n");
							msgUI.CreateQueryNoCancel("Not enough gold.", MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_PURCHASE_CASH);
							msgUI.Show();
						}
					}
					else
					{
						msgUI.Hide();
					}
					break;
				}
				case 9:
					msgUI.Hide();
					break;
				}
			}
			else if (eventID == MessageBoxUI.EVENT_PURCHASE_CASH)
			{
				msgUI.Hide();
			}
			else if (eventID == MessageBoxUI.EVENT_SEND_TWITTER_CONFIRM)
			{
				if (command == 9)
				{
					msgUI.Hide();
				}
			}
			else if (eventID == MessageBoxUI.EVENT_SEND_FACEBOOK_CONFIRM)
			{
				if (command == 9)
				{
					msgUI.Hide();
				}
			}
			else if (eventID == MessageBoxUI.EVENT_AD_REWARD)
			{
				if (command == 9)
				{
					msgUI.Hide();
				}
			}
			else if (eventID == MessageBoxUI.EVENT_SHOW_MOVIE)
			{
				switch (command)
				{
				case 9:
					msgUI.Hide();
					break;
				case 10:
				{
					ShowMovie();
					string msg2 = "Download Our New App NOW!";
					msgUI.CreateConfirmMovie(msg2, MessageBoxUI.MESSAGE_FLAG_MOVIE, MessageBoxUI.EVENT_STORE_LINK);
					msgUI.Show();
					break;
				}
				}
			}
			else if (eventID == MessageBoxUI.EVENT_STORE_LINK)
			{
				switch (command)
				{
				case 9:
					msgUI.Hide();
					break;
				case 10:
					msgUI.Hide();
					Application.OpenURL(UIConstant.RUSH_VERSION_URL);
					break;
				}
			}
			else if (eventID == MessageBoxUI.EVENT_STORE_PROMOTION)
			{
				switch (command)
				{
				case 10:
					Debug.Log("Go to Store");
					msgUI.Hide();
					stateMgr.FrGoToPhase(4, false, false, true);
					break;
				case 9:
					msgUI.Hide();
					break;
				}
			}
		}
	}
}
