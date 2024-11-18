using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : UIHandler, IUIHandle
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

	private UIImage storeImg;

	private UIImage mGunIcon;

	private UIImage mTitleFrame;

	private UIImage mTitleBG;

	private FrUIText mTitleText;

	private FrUIText mLevelText;

	private FrUIText[] mPropertiesTexts;

	private UIImage[,] mSuitIcon;

	private UIAvatar3D avatarFrame;

	private UISliderTag swapTag;

	private UISliderAvatar swapAvatar;

	private UIImage[,] compareImg;

	private UITextImage[] compareTitleImg;

	private FrUIText[] compareTxt;

	public byte state;

	private byte curTag = 5;

	private byte nextTag = 5;

	private int[] m_equipSel = new int[6];

	protected PlayerSkill playerSkill;

	protected PlayerSkill playerSkillPrev;

	private RefuelUI refuel;

	private UITextButton refuelTxtBtn;

	private UITextButton propsTxtBtn;

	private UITextButton iapTxtBtn;

	private UIImage refuelFlagImg;

	private UIImage propsFlagImg;

	private UIImage iapFlagImg;

	private UIImage[] tagNavImg;

	private UIImage selectTagNavImg;

	private List<List<UIImage>> avatarNavListImg = new List<List<UIImage>>();

	private UIImage selectAvatarNavImg;

	private GameObject light;

	private UIDialog mDescription;

	private UINumeric priceNum;

	private string[] mWeaponDescriptions;

	private string[] mBonusDescriptions;

	private string[] mPackDescriptions;

	private bool mNeedUpdateDescrition = true;

	private UIClickButton twitterImg;

	private UIClickButton facebookImg;

	private MessageBoxUI msgUI;

	private IAPUI iapUI;

	private byte mRankID;

	private UIClickButton freeGotMithrilImg;

	private UIClickButton New;

	private AdsUI adsui;

	public StoreUI(UIStateManager stateMgr)
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
	}

	public void Create()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		CreateClone();
		mRankID = userState.GetRank().rankID;
		UnitUI unitUI = Res2DManager.GetInstance().vUI[11];
		storeImg = new UIImage();
		storeImg.AddObject(unitUI, 0, BG_IMG);
		storeImg.Rect = storeImg.GetObjectRect();
		navigationBar = new NavigationBarUI(stateMgr);
		navigationBar.Create();
		navigationBar.SetTitle("STORE");
		navigationBar.Show();
		propsTxtBtn = new UITextButton();
		propsTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 48);
		propsTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 48);
		propsTxtBtn.SetTextOffset(0f, -10f);
		propsTxtBtn.SetText("font2", "ITEMS", UIConstant.fontColor_cyan);
		propsTxtBtn.Rect = propsTxtBtn.GetObjectRect(UIButtonBase.State.Normal);
		propsTxtBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		propsFlagImg = new UIImage();
		propsFlagImg.AddObject(unitUI, 0, 75);
		propsFlagImg.Rect = propsFlagImg.GetObjectRect();
		refuelTxtBtn = new UITextButton();
		refuelTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 49);
		refuelTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 49);
		refuelTxtBtn.SetTextOffset(0f, -10f);
		refuelTxtBtn.SetText("font2", "AMMO", UIConstant.fontColor_cyan);
		refuelTxtBtn.Rect = refuelTxtBtn.GetObjectRect(UIButtonBase.State.Normal);
		refuelTxtBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		refuelFlagImg = new UIImage();
		refuelFlagImg.AddObject(unitUI, 0, 76);
		refuelFlagImg.Rect = refuelFlagImg.GetObjectRect();
		iapTxtBtn = new UITextButton();
		iapTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 81);
		iapTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 81);
		iapTxtBtn.SetTextOffset(0f, -10f);
		iapTxtBtn.SetText("font2", "GOLD", UIConstant.fontColor_cyan);
		iapTxtBtn.Rect = iapTxtBtn.GetObjectRect(UIButtonBase.State.Normal);
		iapTxtBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		iapFlagImg = new UIImage();
		iapFlagImg.AddObject(unitUI, 0, 82);
		iapFlagImg.Rect = iapFlagImg.GetObjectRect();
		compareImg = new UIImage[3, 4];
		compareImg[0, 0] = CreateImg(30);
		compareImg[2, 0] = CreateImg(38);
		compareImg[1, 0] = CreateImg(34);
		compareImg[0, 3] = CreateImg(31);
		compareImg[0, 3].Rect = compareImg[0, 3].Rect;
		compareImg[2, 3] = CreateImg(39);
		compareImg[2, 3].Rect = compareImg[2, 3].Rect;
		compareImg[1, 3] = CreateImg(35);
		compareImg[1, 3].Rect = compareImg[1, 3].Rect;
		compareImg[0, 1] = CreateImg(32);
		compareImg[0, 1].Rect = compareImg[0, 3].Rect;
		compareImg[2, 1] = CreateImg(40);
		compareImg[2, 1].Rect = compareImg[2, 3].Rect;
		compareImg[1, 1] = CreateImg(36);
		compareImg[1, 1].Rect = compareImg[1, 3].Rect;
		compareImg[0, 2] = CreateImg(33);
		compareImg[0, 2].Rect = compareImg[0, 3].Rect;
		compareImg[2, 2] = CreateImg(41);
		compareImg[2, 2].Rect = compareImg[2, 3].Rect;
		compareImg[1, 2] = CreateImg(37);
		compareImg[1, 2].Rect = compareImg[1, 3].Rect;
		compareTitleImg = new UITextImage[3];
		compareTitleImg[0] = new UITextImage();
		compareTitleImg[0].AddObject(unitUI, 0, 69);
		compareTitleImg[0].SetText("font3", string.Empty, UIConstant.PROPERTY_COLOR[0], FrUIText.enAlignStyle.TOP_RIGHT);
		Rect objectRect = compareTitleImg[0].GetObjectRect();
		compareTitleImg[0].SetTextOffset(-10f, -2f);
		compareTitleImg[0].Rect = new Rect(objectRect.x, objectRect.y - 10f, objectRect.width, objectRect.height + 20f);
		compareTitleImg[0].SetScaleWithInt(true);
		compareTitleImg[1] = new UITextImage();
		compareTitleImg[1].AddObject(unitUI, 0, 70);
		compareTitleImg[1].SetText("font3", string.Empty, UIConstant.PROPERTY_COLOR[1], FrUIText.enAlignStyle.TOP_RIGHT);
		objectRect = compareTitleImg[1].GetObjectRect();
		compareTitleImg[1].SetTextOffset(-10f, -2f);
		compareTitleImg[1].Rect = new Rect(objectRect.x, objectRect.y - 10f, objectRect.width, objectRect.height + 20f);
		compareTitleImg[1].SetScaleWithInt(true);
		compareTitleImg[2] = new UITextImage();
		compareTitleImg[2].AddObject(unitUI, 0, 71);
		compareTitleImg[2].SetText("font3", string.Empty, UIConstant.PROPERTY_COLOR[2], FrUIText.enAlignStyle.TOP_RIGHT);
		objectRect = compareTitleImg[2].GetObjectRect();
		compareTitleImg[2].SetTextOffset(-10f, -2f);
		compareTitleImg[2].Rect = new Rect(objectRect.x, objectRect.y - 10f, objectRect.width, objectRect.height + 20f);
		compareTitleImg[2].SetScaleWithInt(true);
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
		stateMgr.m_UIManager.Add(storeImg);
		stateMgr.m_UIManager.Add(navigationBar);
		stateMgr.m_UIManager.Add(propsTxtBtn);
		stateMgr.m_UIManager.Add(refuelTxtBtn);
		stateMgr.m_UIManager.Add(iapTxtBtn);
		stateMgr.m_UIManager.Add(propsFlagImg);
		stateMgr.m_UIManager.Add(refuelFlagImg);
		stateMgr.m_UIManager.Add(iapFlagImg);
		mTitleFrame = new UIImage();
		mTitleFrame.AddObject(unitUI, 0, TITLE_FRAME_BEGIN_IMG, TITLE_FRAME_COUNT_IMG);
		mTitleFrame.Rect = mTitleFrame.GetObjectRect();
		stateMgr.m_UIManager.Add(mTitleFrame);
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 3);
		mTitleBG = new UIImage();
		mTitleBG.AddObject(unitUI, 0, 4);
		mTitleBG.Rect = mTitleBG.GetObjectRect();
		mTitleBG.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 52);
		mTitleText = new FrUIText();
		mTitleText.Rect = modulePositionRect2;
		mTitleText.Set("font3", "Name", UIConstant.FONT_COLOR_EQUIP_NAME, modulePositionRect2.width);
		Rect modulePositionRect3 = unitUI.GetModulePositionRect(0, 0, 53);
		mLevelText = new FrUIText();
		mLevelText.Rect = modulePositionRect3;
		mLevelText.Set("font3", "Level", UIConstant.FONT_COLOR_WEAPON_LEVEL, modulePositionRect3.width);
		mPropertiesTexts = new FrUIText[4]
		{
			new FrUIText(),
			new FrUIText(),
			new FrUIText(),
			new FrUIText()
		};
		for (int i = 0; i < 4; i++)
		{
			Rect modulePositionRect4 = unitUI.GetModulePositionRect(0, 0, 54 + i);
			mPropertiesTexts[i].Rect = modulePositionRect4;
			mPropertiesTexts[i].Set("font3", string.Empty, UIConstant.fontColor_cyan, modulePositionRect4.width);
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
		mGunIcon = new UIImage();
		mGunIcon.AddObject(unitUI, 1, 0);
		mGunIcon.Rect = mGunIcon.GetObjectRect();
		mGunIcon.Visible = false;
		stateMgr.m_UIManager.Add(mGunIcon);
		Rect modulePositionRect5 = unitUI.GetModulePositionRect(0, 0, 18);
		avatarFrame = new UIAvatar3D(modulePositionRect5, new Vector3(-0.499798f, 0.0172753f, 3.620711f), new Vector3(1.2f, 1.2f, 1.2f), new Vector3(0f, 150f, 0f));
		avatarFrame.UpdateAnimation();
		avatarFrame.Show();
		InitNavList();
		swapAvatar = new UISliderAvatar();
		swapAvatar.Create(unitUI);
		tagNavImg = new UIImage[6];
		for (int m = 0; m < tagNavImg.Length; m++)
		{
			tagNavImg[m] = new UIImage();
			tagNavImg[m].AddObject(unitUI, 0, 24 + m);
			tagNavImg[m].Rect = tagNavImg[m].GetObjectRect();
			stateMgr.m_UIManager.Add(tagNavImg[m]);
		}
		selectTagNavImg = new UIImage();
		selectTagNavImg.AddObject(unitUI, 0, 23);
		selectTagNavImg.Rect = tagNavImg[curTag].Rect;
		stateMgr.m_UIManager.Add(selectTagNavImg);
		for (int n = 0; n < 3; n++)
		{
			for (int num = 0; num < 4; num++)
			{
				compareImg[n, num].SetClipOffs(0, new Vector2(10f, 0f));
				compareImg[n, num].SetClipOffs(1, new Vector2(0f, 0f));
				compareImg[n, num].SetClipOffs(2, new Vector2(-10f, 0f));
				compareImg[n, num].SetClipOffs(3, new Vector2(0f, 0f));
				stateMgr.m_UIManager.Add(compareImg[n, num]);
			}
		}
		for (int num2 = 0; num2 < 3; num2++)
		{
			stateMgr.m_UIManager.Add(compareTitleImg[num2]);
		}
		for (int num3 = 0; num3 < 3; num3++)
		{
			stateMgr.m_UIManager.Add(compareTxt[num3]);
		}
		stateMgr.m_UIManager.Add(avatarFrame);
		stateMgr.m_UIManager.Add(swapAvatar);
		mDescription = new UIDialog(stateMgr, 1);
		mDescription.Create();
		mDescription.Show();
		mDescription.AddBGFrame(unitUI, 0, BRIEF_BACKGROUND_BEGIN_IMG, BRIEF_BACKGROUND_COUNT_IMG);
		int rankID = GameApp.GetInstance().GetUserState().GetRank()
			.rankID;
		if (rankID >= 0 && rankID <= 3)
		{
			mDescription.AddDisImage(unitUI, 0, 85);
		}
		else
		{
			mDescription.AddDisImage(unitUI, 0, 86);
		}
		byte[] module = new byte[2] { 21, 72 };
		byte[] module2 = new byte[2] { 20, 73 };
		byte[] module3 = new byte[2] { 19, 74 };
		mDescription.AddButton(0, UIButtonBase.State.Normal, unitUI, 0, module);
		mDescription.AddButton(0, UIButtonBase.State.Pressed, unitUI, 0, module2);
		mDescription.AddButton(0, UIButtonBase.State.Disabled, unitUI, 0, module3);
		Rect modulePositionRect6 = unitUI.GetModulePositionRect(0, 0, 50);
		mDescription.SetTextShowRect(modulePositionRect6.x, modulePositionRect6.y, modulePositionRect6.width, modulePositionRect6.height);
		mDescription.SetText("font3", "Here is the description of the equipment.", UIConstant.FONT_COLOR_DESCRIPTION, FrUIText.enAlignStyle.TOP_LEFT);
		mDescription.SetBlock(false);
		stateMgr.m_UIManager.Add(mDescription);
		UnitUI unitUI2 = Res2DManager.GetInstance().vUI[17];
		priceNum = new UINumeric();
		priceNum.AlignStyle = UINumeric.enAlignStyle.center;
		priceNum.SpacingOffsetX = -7f;
		priceNum.Rect = unitUI.GetModulePositionRect(0, 0, 58);
		stateMgr.m_UIManager.Add(priceNum);
		swapTag = new UISliderTag();
		swapTag.Create(unitUI, 0);
		ResetUITag();
		stateMgr.m_UIPopupManager.Add(swapTag);
		facebookImg = new UIClickButton();
		facebookImg.AddObject(UIButtonBase.State.Normal, unitUI, 0, 78);
		facebookImg.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 80);
		facebookImg.Rect = facebookImg.GetObjectRect(UIButtonBase.State.Normal);
		if (GameApp.GetInstance().openfreemithril)
		{
			freeGotMithrilImg = new UIClickButton();
			freeGotMithrilImg.AddObject(UIButtonBase.State.Normal, unitUI, 0, 83);
			freeGotMithrilImg.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 83);
			freeGotMithrilImg.Rect = freeGotMithrilImg.GetObjectRect(UIButtonBase.State.Normal);
			stateMgr.m_UIManager.Add(freeGotMithrilImg);
			New = new UIClickButton();
			New.AddObject(UIButtonBase.State.Normal, unitUI, 0, 84);
			New.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 84);
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
		if (userState.bGotoIAP)
		{
			iapUI.Create();
			iapUI.SetSelection(0);
			iapUI.Show();
			userState.bGotoIAP = false;
		}
		UnityEngine.Object original = Resources.Load("UI/light");
		light = UnityEngine.Object.Instantiate(original, new Vector3(-0.55f, 0f, 2.5f), Quaternion.identity) as GameObject;
		light.transform.position = new Vector3(-0.55f, -0.2f, 3.5f);
		light.transform.Rotate(new Vector3(270f, 0f, 0f));
		light.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		GameObject gameObject = GameObject.Find("Main Camera");
		gameObject.transform.position = new Vector3(-0.5f, 1f, 0f);
		Rect rct = new Rect(0.117f, 0f, 0.62f, 1f);
		gameObject.GetComponent<Camera>().rect = UIConstant.GetRectForScreenAdaptived2(rct);
		if (userState.GetShowNotify() == 1 && userState.GetDiscountStatus() == 1)
		{
			userState.SetDiscountStatus(2);
			int discountWeapon = userState.GetDiscountWeapon();
			msgUI.CreateWeaponConfirm("We have a new gun on sale!", userState.GetDiscountWeapon(), MessageBoxUI.EVENT_SHOW_DISCOUNT);
			msgUI.Show();
		}
		if (userState.GetDiscountStatus() == 2)
		{
			m_equipSel[5] = userState.GetDiscountWeapon();
			avatarFrame.ChangeAvatar(m_equipSel);
			playerSkill = InitSkills(m_equipSel);
			mNeedUpdateDescrition = true;
		}
		else
		{
			if (userState.m_promotion.m_salesOff.Count <= 0)
			{
				return;
			}
			int num4 = m_equipSel[5];
			foreach (KeyValuePair<int, float> item in userState.m_promotion.m_salesOff)
			{
				Weapon weapon = userState.GetWeapons()[item.Key];
				if (weapon.Level == 0 || weapon.Level == 15)
				{
					num4 = item.Key;
					break;
				}
			}
			m_equipSel[5] = num4;
			avatarFrame.ChangeAvatar(m_equipSel);
			playerSkill = InitSkills(m_equipSel);
			mNeedUpdateDescrition = true;
		}
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
				list.Add(list2[j]);
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
			list4.Add(weapons[l]);
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
		stateMgr.m_UIPopupManager.Add(selectAvatarNavImg);
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
		UnitUI ui = Res2DManager.GetInstance().vUI[11];
		UIImage uIImage = new UIImage();
		uIImage.AddObject(ui, 0, index);
		uIImage.Rect = uIImage.GetObjectRect();
		return uIImage;
	}

	public void ResetUITag()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[11];
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
		Debug.Log("SetAvatars : " + type);
		((ShopAndCustomize)stateMgr).LoadAllAvatar(type, true, true);
		if (type == curTag)
		{
			swapAvatar.ClearWithoutDestroyObj();
		}
		else
		{
			swapAvatar.Clear();
		}
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
			List<List<Armor>> armor = userState.GetArmor();
			List<Armor> list = armor[type];
			zero2 = ((type != 4) ? avatarFrame.GetModel().transform.Find(UIConstant.SUB_AVATAR[type]).position : new Vector3(-0.7232f, 1.6f, 3.32f));
			zero3 = list[m_equipSel[type]].Center;
			zero = swapAvatar.ToPixelScreen(new Vector2(x, zero2.y + 1.2f * zero3.z));
			for (int k = 0; k < list.Count; k++)
			{
				int index = k % list.Count;
				GameObject equipment = list[index].Equipment;
				UISliderAvatar.Avatar3D avatar3D = new UISliderAvatar.Avatar3D();
				avatar3D.m_scale = UIConstant.AVATAR_SCALE[list[index].GetArmorID(), type];
				avatar3D.m_obj = new UI3DFrame(new Rect(0f, 0f, 500f, 600f), new Vector3(x, zero2.y, zero2.z), new Vector3(0f, 205f, 0f));
				avatar3D.m_obj.SetModel(equipment);
				if (list[index].GetArmorID() == m_equipSel[type])
				{
					selection = k;
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
				float num3 = (0f - (avatar3D.m_scale - 1.2f)) * list[index].Center.z;
				avatar3D.m_obj.SetPosition(new Vector3(x, zero2.y + num3, zero2.z));
				Transform transform2 = avatar3D.m_obj.GetModel().transform.Find("lock(Clone)");
				transform2.gameObject.transform.position = new Vector3(x - 0.02f, zero2.y + list[index].Center.z + 0.02f, 2.5f);
				transform2.gameObject.transform.LookAt(Camera.main.transform);
				Transform transform3 = avatar3D.m_obj.GetModel().transform.Find("unlock(Clone)");
				transform3.gameObject.transform.position = new Vector3(x - 0.02f, zero2.y + list[index].Center.z + 0.02f, 2.5f);
				transform3.gameObject.transform.LookAt(Camera.main.transform);
				Transform transform4 = avatar3D.m_obj.GetModel().transform.Find("unlock1(Clone)");
				transform4.gameObject.transform.position = new Vector3(x - 0.02f, zero2.y + list[index].Center.z + 0.02f, 2.5f);
				transform4.gameObject.transform.LookAt(Camera.main.transform);
				avatar3D.m_state = list[index].Level;
				if (list[index].Mithril == 0)
				{
					avatar3D.m_bMithril = false;
				}
				else
				{
					avatar3D.m_bMithril = true;
				}
				avatar3D.m_obj.Show();
				avatar3D.m_Lock = new UIImage();
				avatar3D.m_Lock.AddObject(ui, 0, 13);
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
			List<Weapon> weapons = userState.GetWeapons();
			zero2 = new Vector3(-0.7232f, 1.1f, 3.32f);
			zero3 = weapons[m_equipSel[type]].Center;
			zero = swapAvatar.ToPixelScreen(new Vector2(x, zero2.y + 0.120000005f));
			for (int l = 0; l < weapons.Count; l++)
			{
				int num4 = l % weapons.Count;
				GameObject gun = weapons[num4].Gun;
				UISliderAvatar.Avatar3D avatar3D2 = new UISliderAvatar.Avatar3D();
				avatar3D2.m_scale = UIConstant.WEAPON_SCALE[weapons[num4].GunID];
				avatar3D2.m_obj = new UI3DFrame(new Rect(0f, 0f, 500f, 600f), new Vector3(weapons[num4].Center.x, zero2.y, zero2.z), new Vector3(0f, 205f, 0f));
				avatar3D2.m_obj.SetModel(gun);
				if (weapons[num4].GunID == m_equipSel[type])
				{
					selection = l;
				}
				switch (l)
				{
				case 23:
					avatar3D2.m_obj.AddSubModel(weapons[num4].Gun.transform.GetChild(0).gameObject);
					avatar3D2.m_obj.AddSubModel(weapons[num4].Gun.transform.GetChild(1).gameObject);
					break;
				case 24:
				case 25:
				case 39:
					avatar3D2.m_obj.AddSubModel(weapons[num4].Gun.transform.GetChild(1).gameObject);
					break;
				case 22:
					avatar3D2.m_obj.AddSubModel(weapons[num4].Gun.transform.GetChild(0).gameObject);
					avatar3D2.m_obj.AddSubModel(weapons[num4].Gun.transform.GetChild(1).gameObject);
					avatar3D2.m_obj.AddSubModel(weapons[num4].Gun.transform.GetChild(2).gameObject);
					break;
				case 44:
					avatar3D2.m_obj.AddSubModel(weapons[num4].Gun.transform.GetChild(0).gameObject);
					break;
				default:
					avatar3D2.m_obj.AddSubModel(avatar3D2.m_obj.GetModel());
					break;
				case 26:
				case 27:
				case 28:
				case 29:
				case 30:
				case 31:
					break;
				}
				if (weapons[num4].GunID == 39)
				{
				}
				avatar3D2.m_obj.CenterOffset = new Vector3(weapons[num4].Center.y, weapons[num4].Center.z, weapons[num4].Center.y);
				avatar3D2.m_pos.x = UIConstant.WEAPON_OFFSET[weapons[num4].GunID, 0];
				avatar3D2.m_pos.y = UIConstant.WEAPON_OFFSET[weapons[num4].GunID, 1];
				avatar3D2.m_obj.SetScale(new Vector3(avatar3D2.m_scale, avatar3D2.m_scale, avatar3D2.m_scale));
				float num5 = (0f - (1.2f - avatar3D2.m_scale)) * weapons[num4].Center.y;
				avatar3D2.m_obj.SetPosition(new Vector3(x, zero2.y + num5, zero2.z));
				Transform transform5 = avatar3D2.m_obj.GetModel().transform.Find("lock(Clone)");
				transform5.gameObject.transform.localScale = new Vector3(UIConstant.PRICE_SCALE[num4], UIConstant.PRICE_SCALE[num4], UIConstant.PRICE_SCALE[num4]);
				transform5.gameObject.transform.position = new Vector3(x + weapons[num4].Center.y - 0.02f + UIConstant.PRICE_OFFSET[num4, 0], zero2.y + weapons[num4].Center.z + 0.02f + UIConstant.PRICE_OFFSET[num4, 1], 2.5f + UIConstant.PRICE_OFFSET[num4, 2]);
				transform5.gameObject.transform.LookAt(Camera.main.transform);
				Transform transform6 = avatar3D2.m_obj.GetModel().transform.Find("unlock(Clone)");
				transform6.gameObject.transform.localScale = new Vector3(UIConstant.PRICE_SCALE[num4], UIConstant.PRICE_SCALE[num4], UIConstant.PRICE_SCALE[num4]);
				transform6.gameObject.transform.position = new Vector3(x + weapons[num4].Center.y - 0.02f + UIConstant.PRICE_OFFSET[num4, 0], zero2.y + weapons[num4].Center.z + 0.02f + UIConstant.PRICE_OFFSET[num4, 1], 2.5f + UIConstant.PRICE_OFFSET[num4, 2]);
				transform6.gameObject.transform.LookAt(Camera.main.transform);
				Transform transform7 = avatar3D2.m_obj.GetModel().transform.Find("unlock1(Clone)");
				transform7.gameObject.transform.localScale = new Vector3(UIConstant.PRICE_SCALE[num4], UIConstant.PRICE_SCALE[num4], UIConstant.PRICE_SCALE[num4]);
				transform7.gameObject.transform.position = new Vector3(x + weapons[num4].Center.y - 0.02f + UIConstant.PRICE_OFFSET[num4, 0], zero2.y + weapons[num4].Center.z + 0.02f + UIConstant.PRICE_OFFSET[num4, 1], 2.5f + UIConstant.PRICE_OFFSET[num4, 2]);
				transform7.gameObject.transform.LookAt(Camera.main.transform);
				avatar3D2.m_state = weapons[num4].Level;
				if (weapons[num4].Mithril == 0)
				{
					avatar3D2.m_bMithril = false;
				}
				else
				{
					avatar3D2.m_bMithril = true;
				}
				avatar3D2.m_obj.Show();
				avatar3D2.m_Lock = new UIImage();
				avatar3D2.m_Lock.AddObject(ui, 0, 13);
				avatar3D2.m_Lock.Rect = avatar3D2.m_Lock.GetObjectRect();
				avatar3D2.Rect = new Rect(zero.x - 60f, zero.y - 67.5f, 120f, 135f);
				avatar3D2.Id = weapons[num4].GunID;
				avatar3D2.m_order = weapons[num4].DisplayOrder;
				avatar3D2.Add(avatar3D2.m_obj);
				avatar3D2.Add(avatar3D2.m_Lock);
				avatar3D2.Show();
				Sort(avatar3D2);
			}
			for (int m = 0; m < swapAvatar.m_avatar3D.Count; m++)
			{
				if (m_equipSel[type] == swapAvatar.m_avatar3D[m].Id)
				{
					selection = m;
					break;
				}
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
			float num = avatar.m_order;
			int num2 = 0;
			int num3 = swapAvatar.m_avatar3D.Count - 1;
			int num4 = (num2 + num3) / 2;
			if (num <= (float)swapAvatar.m_avatar3D[num2].m_order)
			{
				swapAvatar.Insert(num2, avatar);
			}
			else if (num >= (float)swapAvatar.m_avatar3D[num3].m_order)
			{
				swapAvatar.Insert(num3 + 1, avatar);
			}
			else
			{
				while (num3 - num2 > 1)
				{
					float num5 = swapAvatar.m_avatar3D[num4].m_order;
					if (num == num5)
					{
						num2 = num4;
						break;
					}
					if (num > num5)
					{
						num2 = num4;
					}
					else
					{
						num3 = num4;
					}
					num4 = (num2 + num3) / 2;
				}
				swapAvatar.Insert(num2 + 1, avatar);
			}
		}
		return true;
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
			if (swapAvatar != null)
			{
				if (swapAvatar.m_state == 3 && nextTag != curTag)
				{
					SetAvatars(nextTag);
					mNeedUpdateDescrition = true;
				}
				else if (mRankID != userState.GetRank().rankID)
				{
					SetAvatars(curTag);
					mNeedUpdateDescrition = true;
					mRankID = userState.GetRank().rankID;
				}
			}
			UpdateComparsion();
			UpdateAvatarNav();
			UpdateDescription();
			if (refuel.Visible)
			{
				refuel.UpdateEnergy();
			}
			ShowRewardMsg();
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

	public void UpdateComfirm()
	{
		if (curTag == 5)
		{
			Weapon weapon = userState.GetWeapons()[m_equipSel[5]];
			if (weapon.Level > 0 && weapon.Level < Global.MAX_LEVEL_WEAPONW)
			{
				mDescription.SetButtonText(0, "UPGRADE");
			}
			else if (weapon.Level == Global.MAX_LEVEL_WEAPONW)
			{
				mDescription.SetButtonText(0, "MAX");
			}
			else
			{
				mDescription.SetButtonText(0, "BUY");
			}
		}
		else
		{
			mDescription.SetButtonText(0, "BUY");
		}
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
		UnitUI ui = Res2DManager.GetInstance().vUI[10];
		if (curTag < 4)
		{
			int armorGroupID = userState.GetArmor(curTag, m_equipSel[curTag]).GetArmorGroupID();
			for (int i = 0; i < 4; i++)
			{
				if (userState.GetArmor(i, m_equipSel[i]).Level == 0)
				{
					flag = true;
				}
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
		string empty4 = string.Empty;
		bool flag3 = false;
		int num = m_equipSel[curTag];
		for (int l = 0; l < 4; l++)
		{
			mPropertiesTexts[l].SetText(string.Empty);
		}
		if (curTag < 5)
		{
			mDescription.m_DisImage.Visible = false;
			mGunIcon.Visible = false;
			Armor armor = userState.GetArmor(curTag, num);
			empty = armor.Name;
			if (armor.Level == 0)
			{
				flag = true;
				text = "Unlock:Rank " + (armor.UnlockLevel + 1);
				mLevelText.SetColor(UIConstant.FONT_COLOR_DESCRIPTION_LOCK);
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
			if (armor.Level != 0 && armor.Level != 15)
			{
				flag3 = true;
			}
			else if (armor.Level == 0)
			{
				flag2 = true;
			}
			empty4 = ((armor.Mithril != 0) ? string.Format("#{0:N0}", armor.Mithril) : string.Format("${0:N0}", armor.Price));
		}
		else
		{
			Weapon weapon = userState.GetWeapons()[num];
			empty = weapon.Name;
			if (weapon.Level == 0)
			{
				flag2 = true;
				flag = true;
				text = "Unlock:Rank " + (weapon.UnlockLevel + 1);
				mLevelText.SetColor(UIConstant.FONT_COLOR_DESCRIPTION_LOCK);
			}
			empty2 = "POW " + (int)weapon.SimpleDamage() + UIConstant.PROPERTY_POWER[weapon.GunID];
			mPropertiesTexts[0].Set("font3", empty2, UIConstant.PROPERTY_COLOR[1]);
			empty2 = "FIRERATE " + string.Format("{0:N2}", weapon.AttackFrequency);
			mPropertiesTexts[1].Set("font3", empty2, UIConstant.PROPERTY_COLOR[1]);
			empty2 = "ENG " + weapon.EnegyConsume;
			mPropertiesTexts[2].Set("font3", empty2, UIConstant.PROPERTY_COLOR[1]);
			mDescription.m_DisImage.Visible = false;
			if (weapon.GetSpeedDrag() != 0f)
			{
				empty2 = "SPD " + weapon.GetSpeedDrag();
				mPropertiesTexts[3].Set("font3", empty2, UIConstant.PROPERTY_COLOR[1]);
			}
			if (weapon.Mithril == 0)
			{
				empty4 = string.Format("${0:N0}", weapon.Price);
			}
			else if (userState.GetDiscountStatus() == 2 && userState.GetDiscountWeapon() == weapon.GunID)
			{
				mDescription.m_DisImage.Visible = true;
				int rankID = userState.GetRank().rankID;
				empty4 = string.Format("#{0:N0}", (int)((float)weapon.Mithril * UIConstant.GetRankDisCount(rankID)));
			}
			else
			{
				float promotion = GetPromotion(weapon.GunID);
				if (promotion != 1f)
				{
					empty4 = string.Format("#{0:N0}", (int)((float)weapon.Mithril * promotion));
					UnitUI ui2 = Res2DManager.GetInstance().vUI[11];
					if ((double)promotion <= 0.51)
					{
						mDescription.SetDisImage(ui2, 0, 85);
					}
					else if ((double)promotion <= 0.61)
					{
						mDescription.SetDisImage(ui2, 0, 88);
					}
					else if ((double)promotion <= 0.76)
					{
						mDescription.SetDisImage(ui2, 0, 86);
					}
					else
					{
						mDescription.SetDisImage(ui2, 0, 87);
					}
					mDescription.m_DisImage.Visible = true;
				}
				else
				{
					mDescription.m_DisImage.Visible = false;
					empty4 = string.Format("#{0:N0}", weapon.Mithril);
				}
			}
			if (weapon.Level != 0 && weapon.Level != 15)
			{
				flag3 = true;
			}
			mGunIcon.Visible = true;
			int num3 = 0;
			num3 = ((weapon.GetWeaponType() != WeaponType.PlasmaNeo) ? ((weapon.GetWeaponType() != WeaponType.LaserRifle && weapon.GetWeaponType() != WeaponType.LaserGun && weapon.GetWeaponType() != WeaponType.AdvancedShotGun) ? ((weapon.GetWeaponType() != WeaponType.RocketLauncher && weapon.GetWeaponType() != WeaponType.GrenadeLauncher && weapon.GetWeaponType() != WeaponType.AdvancedGrenadeLauncher && weapon.GetWeaponType() != WeaponType.LightBow && weapon.GetWeaponType() != WeaponType.LightFist && weapon.GetWeaponType() != WeaponType.AutoRocketLauncher && weapon.GetWeaponType() != WeaponType.AutoBow && weapon.GetWeaponType() != WeaponType.TheArrow && weapon.GetWeaponType() != WeaponType.FlyGrenadeLauncher) ? ((weapon.GetWeaponType() != WeaponType.Sword && weapon.GetWeaponType() != WeaponType.AdvancedSword) ? ((weapon.GetWeaponType() == WeaponType.Sniper || weapon.GetWeaponType() == WeaponType.AdvancedSniper || weapon.GetWeaponType() == WeaponType.RelectionSniper) ? 10 : ((weapon.GetWeaponType() == WeaponType.PingPongLauncher) ? 14 : ((weapon.GetWeaponType() != WeaponType.TrackingGun) ? 6 : 12))) : 8) : 4) : 2) : 0);
			if (!flag3)
			{
				num3++;
			}
			mGunIcon.SetTexture(ui, 1, num3);
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
					string text2 = string.Empty;
					if (playerSkill.HasSign(item2.skillType) && item2.data > 0f)
					{
						text2 += "+";
					}
					if (playerSkill.IsPercetage(item2.skillType))
					{
						float num4 = item2.data * 100f;
						text2 = text2 + num4 + "%";
					}
					else
					{
						text2 += item2.data;
					}
					empty3 = empty3.Replace(UIConstant.KEY_WORD[(int)item2.skillType], text2);
				}
			}
		}
		else
		{
			empty3 = "The description text is not ready.";
		}
		mTitleText.SetText(empty);
		mLevelText.SetText(text);
		mDescription.SetText("font3", empty3, (!flag) ? UIConstant.FONT_COLOR_DESCRIPTION : UIConstant.FONT_COLOR_DESCRIPTION_LOCK, FrUIText.enAlignStyle.TOP_LEFT);
		UnitUI ui3 = Res2DManager.GetInstance().vUI[17];
		if (flag3)
		{
			mDescription.m_ownBtn.SetEnable(0, false);
			priceNum.Visible = false;
		}
		else
		{
			mDescription.m_ownBtn.SetEnable(0, true);
			priceNum.Visible = true;
			priceNum.SetNumeric(ui3, 0, empty4);
		}
		mNeedUpdateDescrition = false;
	}

	public float GetPromotion(int gunId)
	{
		foreach (KeyValuePair<int, float> item in userState.m_promotion.m_salesOff)
		{
			if (item.Key == gunId)
			{
				return item.Value;
			}
		}
		return 1f;
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

	public void ResetAvatarState(int id, byte level)
	{
		for (int i = 0; i < swapAvatar.m_avatar3D.Count; i++)
		{
			if (swapAvatar.m_avatar3D[i].Id == id)
			{
				swapAvatar.m_avatar3D[i].m_state = level;
				break;
			}
		}
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

	public AdsUI GetAds()
	{
		return adsui;
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

	public void ClearDisCount()
	{
		userState.SetDiscountStatus(0);
		userState.SetDiscountTime("0000-00-00 00:00");
		userState.SetShowNotify(0);
		userState.SetDiscountWeapon(-1);
		userState.SetGameTime(0);
		userState.SetUseMithril(0);
		Debug.Log("clear");
	}

	public void ShowMovieMsg()
	{
		if (GameApp.GetInstance().GetUserState().showmovie)
		{
			string msg = "Watch video of our game!!";
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

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == freeGotMithrilImg)
		{
			adsui = new AdsUI(stateMgr);
			adsui.Create();
			stateMgr.m_UIPopupManager.Add(adsui);
			adsui.Show();
		}
		else if (control == navigationBar)
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
		else if (control == swapAvatar)
		{
			if (command == 2)
			{
				if (curTag == 5)
				{
					AudioManager.GetInstance().PlaySound(AudioName.MOUNT_WEAPON);
				}
				else
				{
					AudioManager.GetInstance().PlaySound(AudioName.MOUNT_GEARS);
				}
				if (curTag < 5)
				{
					Armor armor = userState.GetArmor(curTag, (int)wparam);
					m_equipSel[curTag] = (int)wparam;
				}
				else
				{
					Weapon weapon = userState.GetWeapons()[(int)wparam];
					int num = (int)wparam;
					m_equipSel[curTag] = num;
				}
				avatarFrame.ChangeAvatar(m_equipSel);
				playerSkill = InitSkills(m_equipSel);
				mNeedUpdateDescrition = true;
			}
		}
		else if (control == mDescription)
		{
			if (command != 9)
			{
				return;
			}
			if (curTag < 5)
			{
				for (int i = 0; i < Global.AVATAR_PART_NUM; i++)
				{
					if (curTag != i)
					{
						continue;
					}
					Armor armor2 = userState.GetArmor(i, m_equipSel[i]);
					if (armor2.Level == 15)
					{
						if (armor2.Mithril == 0)
						{
							if (userState.GetCash() >= armor2.Price)
							{
								userState.Buy(armor2.Price);
								armor2.Level = 1;
								ResetAvatarState(m_equipSel[i], armor2.Level);
								userState.Achievement.GotNewAvatar(userState.OwnedSuitCount());
								if (curTag == 4)
								{
									int num2 = userState.GetAvatar()[4];
									if (num2 != m_equipSel[4])
									{
										Armor armor3 = userState.GetArmor(4, num2);
										Armor armor4 = userState.GetArmor(4, m_equipSel[4]);
										ChangeBag(armor3, armor4);
									}
								}
								userState.SetAvatar((BodyType)i, m_equipSel[i]);
							}
							else
							{
								string text = UIConstant.GetMessage(12).Replace("[n]", "\n");
								if (AndroidConstant.version == AndroidConstant.Version.Kindle)
								{
									int unlockLevel = armor2.UnlockLevel;
									text = text.Replace("[RankX]", unlockLevel + 1 + string.Empty);
								}
								msgUI.CreateQuery(text, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_PURCHASE_CASH);
								msgUI.Show();
							}
						}
						else if (userState.GetMithril() >= armor2.Mithril)
						{
							userState.BuyWithMithril(armor2.Mithril);
							armor2.Level = 1;
							ResetAvatarState(m_equipSel[i], armor2.Level);
							userState.Achievement.GotNewAvatar(userState.OwnedSuitCount());
							if (curTag == 4)
							{
								int num3 = userState.GetAvatar()[4];
								if (num3 != m_equipSel[4])
								{
									Armor armor5 = userState.GetArmor(4, num3);
									Armor armor6 = userState.GetArmor(4, m_equipSel[4]);
									ChangeBag(armor5, armor6);
								}
							}
							userState.SetAvatar((BodyType)i, m_equipSel[i]);
						}
						else
						{
							string msg = UIConstant.GetMessage(11).Replace("[n]", "\n");
							msgUI.CreateQuery(msg, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_PURCHASE_MITHRIL);
							msgUI.Show();
						}
					}
					else
					{
						string text2 = UIConstant.GetMessage(17).Replace("[n]", "\n");
						if (AndroidConstant.version == AndroidConstant.Version.Kindle)
						{
							int unlockLevel2 = armor2.UnlockLevel;
							text2 = text2.Replace("[RankX]", unlockLevel2 + 1 + string.Empty);
						}
						msgUI.CreateQuery(text2, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_PURCHASE_RANK);
						msgUI.Show();
					}
				}
				playerSkillPrev = InitSkills(userState.GetAvatar());
			}
			else if (curTag == 5)
			{
				Weapon weapon2 = userState.GetWeapons()[m_equipSel[5]];
				if (weapon2.Level == 15)
				{
					if (weapon2.Mithril == 0)
					{
						if (userState.GetCash() >= weapon2.Price)
						{
							userState.Buy(weapon2.Price);
							weapon2.Level = 1;
							ResetAvatarState(m_equipSel[5], weapon2.Level);
							Weapon weapon3 = userState.GetBattleWeapons()[0];
							int weaponBagIndex = userState.GetWeaponBagIndex(weapon3);
							Weapon weapon4 = userState.GetWeapons()[m_equipSel[5]];
							userState.InsertPropsToStorage(weapon3.GunID + 1, 1);
							userState.SetBagPosition(weaponBagIndex, m_equipSel[5] + 1);
						}
						else
						{
							string msg2 = UIConstant.GetMessage(12).Replace("[n]", "\n");
							msgUI.CreateQuery(msg2, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_PURCHASE_CASH);
							msgUI.Show();
						}
					}
					else
					{
						int num4 = weapon2.Mithril;
						if (userState.GetDiscountStatus() == 2 && userState.GetDiscountWeapon() == weapon2.GunID)
						{
							int rankID = userState.GetRank().rankID;
							num4 = (int)((float)num4 * UIConstant.GetRankDisCount(rankID));
							Debug.Log("打折价格为:" + num4);
						}
						else
						{
							float promotion = GetPromotion(weapon2.GunID);
							if (promotion != 1f)
							{
								num4 = (int)((float)num4 * promotion);
								Debug.Log("promotion price: " + num4);
							}
						}
						if (userState.GetMithril() >= num4)
						{
							if (userState.GetDiscountStatus() == 2 && userState.GetDiscountWeapon() == weapon2.GunID)
							{
								userState.BuyWithMithril(num4);
								ClearDisCount();
							}
							else
							{
								userState.AddUseMithril(num4);
								userState.BuyWithMithril(num4);
								if (userState.GetDiscountStatus() == 1 && userState.GetDiscountWeapon() == weapon2.GunID)
								{
									ClearDisCount();
								}
							}
							weapon2.Level = 1;
							ResetAvatarState(m_equipSel[5], weapon2.Level);
							Weapon weapon5 = userState.GetBattleWeapons()[0];
							int weaponBagIndex2 = userState.GetWeaponBagIndex(weapon5);
							Weapon weapon6 = userState.GetWeapons()[m_equipSel[5]];
							userState.InsertPropsToStorage(weapon5.GunID + 1, 1);
							userState.SetBagPosition(weaponBagIndex2, m_equipSel[5] + 1);
						}
						else
						{
							string msg3 = UIConstant.GetMessage(11).Replace("[n]", "\n");
							msgUI.CreateQuery(msg3, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_PURCHASE_CASH);
							msgUI.Show();
						}
					}
				}
				else
				{
					string text3 = UIConstant.GetMessage(17).Replace("[n]", "\n");
					if (AndroidConstant.version == AndroidConstant.Version.Kindle)
					{
						int unlockLevel3 = weapon2.UnlockLevel;
						text3 = text3.Replace("[RankX]", unlockLevel3 + 1 + string.Empty);
					}
					msgUI.CreateQuery(text3, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_PURCHASE_RANK);
					msgUI.Show();
				}
			}
			GameApp.GetInstance().Save();
			mNeedUpdateDescrition = true;
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
		}
		else if (control == propsTxtBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			stateMgr.FrGoToPhase(10, false, false, true);
		}
		else if (control == refuelTxtBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			stateMgr.m_UIManager.SetEnable(false);
			refuel.Init();
			refuel.Show();
		}
		else if (control == iapTxtBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
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
			adsui = new AdsUI(stateMgr);
			adsui.Create();
			stateMgr.m_UIPopupManager.Add(adsui);
			adsui.Show();
		}
		else
		{
			if (control != msgUI)
			{
				return;
			}
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
			else if (eventID == MessageBoxUI.EVENT_PURCHASE_RANK)
			{
				switch (command)
				{
				case 10:
					msgUI.Hide();
					iapUI.Create();
					iapUI.SetSelection(0);
					iapUI.Show();
					break;
				case 9:
					msgUI.Hide();
					break;
				}
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
			else if (eventID == MessageBoxUI.EVENT_SHOW_DISCOUNT)
			{
				if (command == 9)
				{
					msgUI.Hide();
					userState.SetShowNotify(0);
					GameApp.GetInstance().Save();
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
					string msg4 = "Download Our New App NOW!";
					msgUI.CreateConfirmMovie(msg4, MessageBoxUI.MESSAGE_FLAG_MOVIE, MessageBoxUI.EVENT_STORE_LINK);
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
		}
	}
}
