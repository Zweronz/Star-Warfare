using System;
using System.Collections.Generic;
using UnityEngine;

public class MakePackageUI : UIHandler, IUIHandle
{
	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_HANDLE = 2;

	private const int MAX_PROPERTY_COUNT = 4;

	public UIStateManager stateMgr;

	protected UserState userState;

	private static byte[] BG_IMG = new byte[2] { 0, 1 };

	private static byte BRIEF_BACKGROUND_BEGIN_IMG = 22;

	private static byte BRIEF_BACKGROUND_COUNT_IMG = 12;

	private NavigationBarUI navigationBar;

	private NavigationMenuUI navigationMenu;

	private UIImage makePackageImg;

	private UIImage briefBGImg;

	private UIImage briefImg;

	private UIDragGrid gridPackage;

	private UISliderStorage swapStorage;

	public UIImage cursorImg;

	private UIImage[] tagNavImg;

	private UIImage selectTagNavImg;

	public byte state;

	private int[] m_bagPosition;

	private byte[,] m_storagePosition;

	private FrUIText mTitleText;

	private FrUIText mLevelText;

	private FrUIText[] mPropertiesTexts;

	private UIImage mGunIcon;

	private FrUIText mPropText;

	private UIImage[,] mLevelIcon;

	private UIDialog mDescription;

	private bool mNeedUpdateDescrition = true;

	private int setectedIndex = -1;

	private string[] mWeaponDescriptions;

	private string[] mPropsDescriptions;

	private MessageBoxUI msgUI;

	public MakePackageUI(UIStateManager stateMgr)
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
		mWeaponDescriptions = null;
	}

	public void Create()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		CreateClone();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[12];
		makePackageImg = new UIImage();
		makePackageImg.AddObject(unitUI, 0, BG_IMG);
		makePackageImg.Rect = makePackageImg.GetObjectRect();
		navigationBar = new NavigationBarUI(stateMgr);
		navigationBar.Create();
		navigationBar.SetTitle("PACKAGE");
		navigationBar.Show();
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 3);
		briefBGImg = new UIImage();
		briefBGImg.AddObject(unitUI, 0, 21);
		briefBGImg.Rect = briefBGImg.GetObjectRect();
		briefBGImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		gridPackage = new UIDragGrid();
		ResetUIPackage();
		gridPackage.Show();
		swapStorage = new UISliderStorage();
		swapStorage.Create(unitUI, 0);
		ResetUIStorage();
		swapStorage.SetAllEnable(false);
		swapStorage.SetSelection(0);
		stateMgr.m_UIManager.Add(makePackageImg);
		stateMgr.m_UIManager.Add(navigationBar);
		tagNavImg = new UIImage[Global.STORAGE_MAX_PANEL];
		int num = 32;
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 19);
		float num2 = modulePositionRect2.x + modulePositionRect2.width * 0.5f - (float)((Global.STORAGE_MAX_PANEL - 1) * num) * 0.5f;
		for (int i = 0; i < tagNavImg.Length; i++)
		{
			tagNavImg[i] = new UIImage();
			tagNavImg[i].AddObject(unitUI, 0, 19);
			tagNavImg[i].Rect = new Rect(num2 - modulePositionRect2.width * 0.5f, modulePositionRect2.y, modulePositionRect2.width, modulePositionRect2.height);
			num2 += (float)num;
			stateMgr.m_UIManager.Add(tagNavImg[i]);
		}
		selectTagNavImg = new UIImage();
		selectTagNavImg.AddObject(unitUI, 0, 16);
		selectTagNavImg.Rect = tagNavImg[0].Rect;
		stateMgr.m_UIManager.Add(selectTagNavImg);
		cursorImg = new UIImage();
		cursorImg.Visible = false;
		mDescription = new UIDialog(stateMgr, 0);
		mDescription.Create();
		mDescription.Show();
		mDescription.AddBGFrame(unitUI, 0, BRIEF_BACKGROUND_BEGIN_IMG, BRIEF_BACKGROUND_COUNT_IMG);
		Rect modulePositionRect3 = unitUI.GetModulePositionRect(0, 0, 37);
		mDescription.SetTextShowRect(modulePositionRect3.x, modulePositionRect3.y, modulePositionRect3.width, modulePositionRect3.height);
		mDescription.SetText("font3", "Here is the description of the equipment.", UIConstant.FONT_COLOR_DESCRIPTION, FrUIText.enAlignStyle.TOP_LEFT);
		mDescription.SetBlock(false);
		Rect modulePositionRect4 = unitUI.GetModulePositionRect(0, 0, 38);
		mTitleText = new FrUIText();
		mTitleText.Rect = modulePositionRect4;
		mTitleText.Set("font3", "Name", UIConstant.FONT_COLOR_EQUIP_NAME, modulePositionRect4.width);
		Rect modulePositionRect5 = unitUI.GetModulePositionRect(0, 0, 39);
		mLevelText = new FrUIText();
		mLevelText.Rect = modulePositionRect5;
		mLevelText.Set("font3", "Level", UIConstant.FONT_COLOR_WEAPON_LEVEL, modulePositionRect5.width);
		mPropertiesTexts = new FrUIText[4]
		{
			new FrUIText(),
			new FrUIText(),
			new FrUIText(),
			new FrUIText()
		};
		for (int j = 0; j < 4; j++)
		{
			Rect modulePositionRect6 = unitUI.GetModulePositionRect(0, 0, 40 + j);
			mPropertiesTexts[j].Rect = modulePositionRect6;
			mPropertiesTexts[j].Set("font3", string.Empty, UIConstant.fontColor_cyan, modulePositionRect6.width);
		}
		mLevelIcon = new UIImage[2, 7];
		for (int k = 0; k < 2; k++)
		{
			for (int l = 0; l < 7; l++)
			{
				mLevelIcon[k, l] = new UIImage();
				mLevelIcon[k, l].AddObject(unitUI, 0, 44 + k);
				Rect objectRect = mLevelIcon[0, 0].GetObjectRect();
				mLevelIcon[k, l].Rect = new Rect(objectRect.left + (objectRect.width + 5f) * (float)l, objectRect.top, objectRect.width, objectRect.height);
				mLevelIcon[k, l].Visible = true;
			}
		}
		mGunIcon = new UIImage();
		mGunIcon.AddObject(unitUI, 1, 0);
		mGunIcon.Rect = mGunIcon.GetObjectRect();
		mGunIcon.Visible = false;
		stateMgr.m_UIManager.Add(briefBGImg);
		stateMgr.m_UIManager.Add(mDescription);
		stateMgr.m_UIManager.Add(mTitleText);
		stateMgr.m_UIManager.Add(mLevelText);
		FrUIText[] array = mPropertiesTexts;
		foreach (FrUIText control in array)
		{
			stateMgr.m_UIManager.Add(control);
		}
		for (int n = 0; n < mLevelIcon.GetLength(0); n++)
		{
			for (int num3 = 0; num3 < mLevelIcon.GetLength(1); num3++)
			{
				stateMgr.m_UIManager.Add(mLevelIcon[n, num3]);
			}
		}
		stateMgr.m_UIManager.Add(mGunIcon);
		stateMgr.m_UIManager.Add(gridPackage);
		stateMgr.m_UIManager.Add(swapStorage);
		stateMgr.m_UIManager.Add(cursorImg);
		navigationMenu = new NavigationMenuUI(stateMgr);
		navigationMenu.Create();
		navigationMenu.Show();
		stateMgr.m_UIPopupManager.Add(navigationMenu);
		msgUI = new MessageBoxUI(stateMgr);
		msgUI.Create();
		string msg = UIConstant.GetMessage(21).Replace("[n]", "\n");
		msgUI.CreateConfirm(msg, MessageBoxUI.MESSAGE_FLAG_WARNING, MessageBoxUI.EVENT_MAKE_PACKAGE_WARNING);
		msgUI.Hide();
		stateMgr.m_UIPopupManager.Add(msgUI);
	}

	public UIImage CreateImg(int index)
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[12];
		UIImage uIImage = new UIImage();
		uIImage.AddObject(ui, 0, index);
		uIImage.Rect = uIImage.GetObjectRect();
		return uIImage;
	}

	public void ResetUIPackage()
	{
		gridPackage.Clear();
		UnitUI ui = Res2DManager.GetInstance().vUI[12];
		gridPackage.SetSelected(ui, 0, 15);
		for (int i = 0; i < userState.GetBagNum(); i++)
		{
			byte[] module = new byte[1] { (byte)(7 + i) };
			UIDragGrid.UIDragIcon uIDragIcon = gridPackage.AddGrid(ui, 0, module);
			if (m_bagPosition[i] > 0)
			{
				SetPackageTexture(i, m_bagPosition[i]);
			}
		}
	}

	public void SetPackageTexture(int index, int val)
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[20];
		val--;
		if (val >= 0)
		{
			if (val > 80)
			{
				gridPackage.SetGridTexture(index, ui, 1, val - 81);
				gridPackage.SetGridTexture(index, 0.75f);
			}
			else
			{
				gridPackage.SetGridTexture(index, ui, 0, val);
				gridPackage.SetGridTexture(index, 0.75f);
			}
		}
		else
		{
			gridPackage.ClearGridTexture(index);
		}
	}

	public void ResetUIStorage()
	{
		swapStorage.Clear();
		UnitUI ui = Res2DManager.GetInstance().vUI[12];
		UnitUI ui2 = Res2DManager.GetInstance().vUI[17];
		for (int i = 0; i < Global.STORAGE_MAX_PANEL; i++)
		{
			UISliderStorage.UIDragPanel uIDragPanel = new UISliderStorage.UIDragPanel();
			uIDragPanel.m_dragGrid = new UIDragGrid();
			uIDragPanel.m_dragGrid.SetParent(uIDragPanel);
			uIDragPanel.m_dragGrid.SetSelected(ui, 0, 5);
			swapStorage.Add(uIDragPanel);
			for (int j = 0; j < 9; j++)
			{
				UIDragGrid.UIDragIcon uIDragIcon = uIDragPanel.m_dragGrid.AddGrid(ui, 0, 6);
				uIDragIcon.m_UIMove.MinY = 10f;
				uIDragIcon.m_UIMove.MinX = 10000f;
				uIDragPanel.m_dragGrid.AddShadow(j, ui, 0, 4);
				uIDragPanel.m_dragGrid.AddNum(j, ui2, 1, m_storagePosition[i * 9 + j, 1]);
				SetStorageNum(i * 9 + j, m_storagePosition[i * 9 + j, 0]);
				SetStorageTexture(i * 9 + j, m_storagePosition[i * 9 + j, 0]);
			}
			uIDragPanel.m_dragGrid.Show();
			uIDragPanel.Show();
		}
		swapStorage.Update();
	}

	public void SetStorageNum(int index, int val)
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[17];
		val--;
		if (m_storagePosition[index, 1] > 1)
		{
			swapStorage.m_PropsPanels[index / 9].m_dragGrid.GetElements()[index % 9].m_num.Visible = true;
			swapStorage.m_PropsPanels[index / 9].m_dragGrid.GetElements()[index % 9].m_num.SetNumeric(ui, 0, string.Format("{0:N0}", m_storagePosition[index, 1]));
		}
		else
		{
			swapStorage.m_PropsPanels[index / 9].m_dragGrid.GetElements()[index % 9].m_num.Visible = false;
		}
	}

	public void SetStorageTexture(int index, int val)
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[20];
		val--;
		if (val >= 0)
		{
			if (val > 80)
			{
				swapStorage.m_PropsPanels[index / 9].m_dragGrid.SetGridTexture(index % 9, ui, 1, val - 81);
			}
			else
			{
				swapStorage.m_PropsPanels[index / 9].m_dragGrid.SetGridTexture(index % 9, ui, 0, val);
			}
		}
		else
		{
			swapStorage.m_PropsPanels[index / 9].m_dragGrid.ClearGridTexture(index % 9);
		}
	}

	private void SetCursorTexture(int val)
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[20];
		val--;
		setectedIndex = val;
		mNeedUpdateDescrition = true;
		if (val >= 0)
		{
			if (val > 80)
			{
				cursorImg.SetTexture(ui, 1, val - 81);
			}
			else
			{
				cursorImg.SetTexture(ui, 0, val);
			}
		}
		else
		{
			cursorImg.Free();
		}
	}

	public void CreateClone()
	{
		m_bagPosition = userState.GetBagPosition();
		m_storagePosition = userState.GetStorageInfo();
	}

	public bool Update()
	{
		switch (state)
		{
		case 0:
			state = 2;
			userState = GameApp.GetInstance().GetUserState();
			Create();
			break;
		case 2:
		{
			UpdateDescription();
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

	public int GetStorageIDAvailable(int startID)
	{
		for (int i = 0; i < m_storagePosition.GetLength(0); i++)
		{
			int num = (startID + i) % m_storagePosition.GetLength(0);
			if (m_storagePosition[num, 0] == 0)
			{
				return num;
			}
		}
		return -1;
	}

	public int GetStorageIDExist(int propID)
	{
		if (propID == 0)
		{
			return -1;
		}
		for (int i = 0; i < m_storagePosition.GetLength(0); i++)
		{
			if (m_storagePosition[i, 0] == propID)
			{
				return i;
			}
		}
		return -1;
	}

	public bool Verify(int bagIndex, int storageIndex)
	{
		int num = 0;
		if (m_bagPosition[bagIndex] > 0 && m_bagPosition[bagIndex] < 80)
		{
			for (int i = 0; i < m_bagPosition.Length; i++)
			{
				if (m_bagPosition[i] > 0 && m_bagPosition[i] < 80)
				{
					num++;
				}
			}
			if (num == 1 && (m_storagePosition[storageIndex, 0] <= 0 || m_storagePosition[storageIndex, 0] > 80))
			{
				return false;
			}
		}
		return true;
	}

	public void SetSelectForStorage(int id)
	{
		gridPackage.m_Selected.Visible = false;
		gridPackage.m_Selected.Id = -1;
		swapStorage.SetSelected(id);
	}

	public void SetSelectForPack(int id)
	{
		swapStorage.ClearSelected();
		gridPackage.m_Selected.Visible = true;
		gridPackage.m_Selected.Id = id;
		gridPackage.m_Selected.Rect = gridPackage.GetElements()[id].m_Background.Rect;
	}

	private void UpdateDescription()
	{
		if (!mNeedUpdateDescrition)
		{
			return;
		}
		UnitUI unitUI = Res2DManager.GetInstance().vUI[12];
		string text = string.Empty;
		string text2 = string.Empty;
		string empty = string.Empty;
		string text3 = string.Empty;
		for (int i = 0; i < 4; i++)
		{
			mPropertiesTexts[i].SetText(string.Empty);
		}
		string[] array = null;
		int num = setectedIndex;
		if (setectedIndex > 80)
		{
			for (int j = 0; j < 2; j++)
			{
				for (int k = 0; k < 7; k++)
				{
					mLevelIcon[j, k].Visible = false;
				}
			}
			mGunIcon.Visible = false;
			Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 3);
			mDescription.SetTextShowRect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
			array = mPropsDescriptions;
			num = setectedIndex - 81;
			text3 = array[num];
			Item item = userState.GetItem()[num];
			text = item.Name;
			List<Effect> effect = item.itemEffect.GetEffect();
			foreach (Effect item2 in effect)
			{
				string text4 = string.Empty;
				if (item.itemEffect.HasSign(item2.effectType) && item2.data > 0f)
				{
					text4 += "+";
				}
				if (item.itemEffect.IsPercetage(item2.effectType))
				{
					float num2 = item2.data * 100f;
					text4 = text4 + num2 + "%";
				}
				else
				{
					text4 += Convert.ToString(item2.data);
				}
				text3 = text3.Replace(UIConstant.PROPS_KEY_WORD[(int)item2.effectType], text4);
			}
			text3 = text3.Replace(UIConstant.KEEP_TIME_STRING, Convert.ToString(item.Duration));
		}
		else if (setectedIndex >= 0)
		{
			Weapon weapon = userState.GetWeapons()[setectedIndex];
			text = weapon.Name;
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
			text2 = "Lv." + weapon.Level;
			mLevelText.SetColor(UIConstant.FONT_COLOR_WEAPON_LEVEL);
			for (int l = 0; l < weapon.Level - 1; l++)
			{
				mLevelIcon[0, l].Visible = false;
				mLevelIcon[1, l].Visible = true;
			}
			for (int m = weapon.Level - 1; m < 7; m++)
			{
				mLevelIcon[0, m].Visible = true;
				mLevelIcon[1, m].Visible = false;
			}
			string empty2 = string.Empty;
			int num3 = (int)weapon.SimpleDamage();
			empty2 = num3.ToString();
			if (weapon.Level < Global.MAX_LEVEL_WEAPONW)
			{
				string text5 = empty2;
				empty2 = text5 + "(+" + (int)(weapon.SimpleDamage(weapon.Level + 1) - (float)num3) + ")";
			}
			empty = "POW " + empty2 + UIConstant.PROPERTY_POWER[weapon.GunID];
			mPropertiesTexts[0].Set("font3", empty, UIConstant.PROPERTY_COLOR[1]);
			empty = "FIRERATE " + string.Format("{0:N2}", weapon.AttackFrequency);
			mPropertiesTexts[1].Set("font3", empty, UIConstant.PROPERTY_COLOR[1]);
			empty = "ENG " + weapon.EnegyConsume;
			mPropertiesTexts[2].Set("font3", empty, UIConstant.PROPERTY_COLOR[1]);
			if (weapon.GetSpeedDrag() != 0f)
			{
				empty = "SPD " + weapon.GetSpeedDrag();
				mPropertiesTexts[3].Set("font3", empty, UIConstant.PROPERTY_COLOR[1]);
			}
			Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 37);
			mDescription.SetTextShowRect(modulePositionRect2.x, modulePositionRect2.y, modulePositionRect2.width, modulePositionRect2.height);
			array = mWeaponDescriptions;
			text3 = array[num];
		}
		else
		{
			for (int n = 0; n < 2; n++)
			{
				for (int num4 = 0; num4 < 7; num4++)
				{
					mLevelIcon[n, num4].Visible = false;
				}
			}
			mGunIcon.Visible = false;
		}
		text3 = text3.Replace("[EMPTY]", string.Empty);
		text3 = text3.Replace("[n]", "\n");
		mTitleText.SetText(text);
		mLevelText.SetText(text2);
		mDescription.SetText("font3", text3, UIConstant.FONT_COLOR_DESCRIPTION, FrUIText.enAlignStyle.TOP_LEFT);
		mNeedUpdateDescrition = false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == navigationBar)
		{
			stateMgr.FrGoToPhase(3, false, false, true);
		}
		else if (control == gridPackage)
		{
			switch (command)
			{
			case 3:
			{
				int num4 = (int)wparam;
				int num5 = (int)lparam;
				int num6 = m_bagPosition[num4];
				m_bagPosition[num4] = m_bagPosition[num5];
				m_bagPosition[num5] = (byte)num6;
				List<UIDragGrid.UIDragIcon> elements4 = gridPackage.GetElements();
				SetPackageTexture(num4, m_bagPosition[num4]);
				SetPackageTexture(num5, m_bagPosition[num5]);
				SetSelectForPack(num5);
				break;
			}
			case 4:
			{
				int num2 = (int)wparam;
				List<UIDragGrid.UIDragIcon> elements2 = gridPackage.GetElements();
				Rect rect = elements2[num2].m_Image.Rect;
				Vector2 point = new Vector2(rect.x + rect.width * 0.5f, rect.y + rect.height * 0.5f);
				List<UIDragGrid.UIDragIcon> elements3 = swapStorage.m_PropsPanels[swapStorage.m_selectPanel].m_dragGrid.GetElements();
				bool flag = false;
				for (int i = 0; i < elements3.Count; i++)
				{
					UIImage background = elements3[i].m_Background;
					if (!background.Rect.Contains(point))
					{
						continue;
					}
					if (!Verify(num2, swapStorage.m_selectPanel * 9 + i))
					{
						msgUI.Show();
						break;
					}
					AudioManager.GetInstance().PlaySound(AudioName.SWITCH_ITEM);
					SetSelectForStorage(swapStorage.m_selectPanel * 9 + i);
					int num3 = m_bagPosition[num2];
					m_bagPosition[num2] = m_storagePosition[swapStorage.m_selectPanel * 9 + i, 0];
					if (num3 == m_storagePosition[swapStorage.m_selectPanel * 9 + i, 0])
					{
						m_storagePosition[swapStorage.m_selectPanel * 9 + i, 1]++;
						m_bagPosition[num2] = 0;
						SetStorageNum(swapStorage.m_selectPanel * 9 + i, m_storagePosition[swapStorage.m_selectPanel * 9 + i, 0]);
					}
					else
					{
						int storageIDExist = GetStorageIDExist(num3);
						if (m_storagePosition[swapStorage.m_selectPanel * 9 + i, 0] == 0 || (m_storagePosition[swapStorage.m_selectPanel * 9 + i, 0] > 0 && m_storagePosition[swapStorage.m_selectPanel * 9 + i, 1] == 1))
						{
							if (storageIDExist != -1)
							{
								m_storagePosition[storageIDExist, 1]++;
								SetStorageNum(storageIDExist, m_storagePosition[storageIDExist, 0]);
								if (m_storagePosition[swapStorage.m_selectPanel * 9 + i, 0] > 0)
								{
									m_storagePosition[swapStorage.m_selectPanel * 9 + i, 0] = 0;
									m_storagePosition[swapStorage.m_selectPanel * 9 + i, 1] = 0;
									SetStorageTexture(swapStorage.m_selectPanel * 9 + i, m_storagePosition[swapStorage.m_selectPanel * 9 + i, 0]);
								}
							}
							else
							{
								m_storagePosition[swapStorage.m_selectPanel * 9 + i, 0] = (byte)num3;
								m_storagePosition[swapStorage.m_selectPanel * 9 + i, 1] = 1;
								SetStorageTexture(swapStorage.m_selectPanel * 9 + i, m_storagePosition[swapStorage.m_selectPanel * 9 + i, 0]);
							}
						}
						else if (m_storagePosition[swapStorage.m_selectPanel * 9 + i, 1] > 1)
						{
							if (storageIDExist != -1)
							{
								m_storagePosition[storageIDExist, 1]++;
								SetStorageNum(storageIDExist, m_storagePosition[storageIDExist, 0]);
							}
							else
							{
								storageIDExist = GetStorageIDAvailable(swapStorage.m_selectPanel * 9 + i);
								m_storagePosition[storageIDExist, 0] = (byte)num3;
								m_storagePosition[storageIDExist, 1] = 1;
								SetStorageTexture(storageIDExist, m_storagePosition[storageIDExist, 0]);
							}
							m_storagePosition[swapStorage.m_selectPanel * 9 + i, 1]--;
							SetStorageNum(swapStorage.m_selectPanel * 9 + i, m_storagePosition[swapStorage.m_selectPanel * 9 + i, 0]);
						}
					}
					SetPackageTexture(num2, m_bagPosition[num2]);
					flag = true;
					break;
				}
				if (!flag)
				{
					elements2[num2].m_Image.Rect = elements2[num2].m_Background.Rect;
				}
				break;
			}
			case 1:
			{
				int index = (int)wparam;
				List<UIDragGrid.UIDragIcon> elements = gridPackage.GetElements();
				cursorImg.Rect = elements[index].m_Image.Rect;
				cursorImg.Visible = true;
				break;
			}
			case 2:
				cursorImg.Visible = false;
				break;
			case 0:
			{
				int num = (int)wparam;
				SetCursorTexture(m_bagPosition[num]);
				cursorImg.Visible = false;
				SetSelectForPack(num);
				break;
			}
			}
		}
		else if (control == swapStorage)
		{
			switch (command)
			{
			case 2:
			{
				int num11 = (int)wparam;
				int num12 = (int)lparam;
				int num13 = m_storagePosition[num11, 0];
				int num14 = m_storagePosition[num11, 1];
				m_storagePosition[num11, 0] = m_storagePosition[num12, 0];
				m_storagePosition[num11, 1] = m_storagePosition[num12, 1];
				m_storagePosition[num12, 0] = (byte)num13;
				m_storagePosition[num12, 1] = (byte)num14;
				SetStorageTexture(num11, m_storagePosition[num11, 0]);
				SetStorageTexture(num12, m_storagePosition[num12, 0]);
				SetStorageNum(num11, m_storagePosition[num11, 0]);
				SetStorageNum(num12, m_storagePosition[num12, 0]);
				SetSelectForStorage(num12);
				break;
			}
			case 3:
			{
				int num9 = (int)wparam;
				List<UIDragGrid.UIDragIcon> elements6 = swapStorage.m_PropsPanels[num9 / 9].m_dragGrid.GetElements();
				Rect rect2 = elements6[num9 % 9].m_Image.Rect;
				Vector2 point2 = new Vector2(rect2.x + rect2.width * 0.5f, rect2.y + rect2.height * 0.5f);
				List<UIDragGrid.UIDragIcon> elements7 = gridPackage.GetElements();
				bool flag2 = false;
				for (int j = 0; j < elements7.Count; j++)
				{
					UIImage background2 = elements7[j].m_Background;
					Rect rect3 = new Rect(background2.Rect.x - 10f, background2.Rect.y - 10f, background2.Rect.width + 20f, background2.Rect.height + 20f);
					if (!rect3.Contains(point2))
					{
						continue;
					}
					if (!Verify(j, num9))
					{
						msgUI.Show();
						break;
					}
					AudioManager.GetInstance().PlaySound(AudioName.SWITCH_ITEM);
					int num10 = m_bagPosition[j];
					m_bagPosition[j] = m_storagePosition[num9, 0];
					SetSelectForPack(j);
					int storageIDExist2 = GetStorageIDExist(num10);
					if (m_storagePosition[num9, 1] <= 1)
					{
						if (storageIDExist2 != -1)
						{
							m_storagePosition[num9, 0] = 0;
							m_storagePosition[num9, 1] = 0;
							SetStorageTexture(num9, m_storagePosition[num9, 0]);
							m_storagePosition[storageIDExist2, 0] = (byte)num10;
							m_storagePosition[storageIDExist2, 1]++;
							SetStorageTexture(storageIDExist2, m_storagePosition[storageIDExist2, 0]);
							SetStorageNum(storageIDExist2, m_storagePosition[storageIDExist2, 0]);
						}
						else
						{
							m_storagePosition[num9, 0] = (byte)num10;
							m_storagePosition[num9, 1] = 1;
							SetStorageTexture(num9, m_storagePosition[num9, 0]);
						}
					}
					else
					{
						if (storageIDExist2 != -1)
						{
							m_storagePosition[storageIDExist2, 1]++;
							SetStorageNum(storageIDExist2, m_storagePosition[storageIDExist2, 0]);
						}
						else
						{
							storageIDExist2 = GetStorageIDAvailable(num9);
							m_storagePosition[storageIDExist2, 0] = (byte)num10;
							m_storagePosition[storageIDExist2, 1] = 1;
							SetStorageTexture(storageIDExist2, m_storagePosition[storageIDExist2, 0]);
							SetStorageNum(storageIDExist2, m_storagePosition[storageIDExist2, 0]);
						}
						m_storagePosition[num9, 1]--;
						SetStorageNum(num9, m_storagePosition[num9, 0]);
					}
					SetPackageTexture(j, m_bagPosition[j]);
					flag2 = true;
					break;
				}
				if (!flag2)
				{
					elements6[num9 % 9].m_Image.Rect = elements6[num9 % 9].m_Background.Rect;
				}
				break;
			}
			case 4:
			{
				int num8 = (int)wparam;
				List<UIDragGrid.UIDragIcon> elements5 = swapStorage.m_PropsPanels[num8 / 9].m_dragGrid.GetElements();
				cursorImg.Rect = elements5[num8 % 9].m_Image.Rect;
				cursorImg.Visible = true;
				break;
			}
			case 5:
				cursorImg.Visible = false;
				break;
			case 6:
			{
				int num7 = (int)wparam;
				SetCursorTexture(m_storagePosition[num7, 0]);
				cursorImg.Visible = false;
				SetSelectForStorage(num7);
				break;
			}
			case 1:
				selectTagNavImg.Rect = tagNavImg[(int)wparam].Rect;
				break;
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
			case 5:
				stateMgr.FrGoToPhase(11, false, false, true);
				break;
			}
		}
		else if (control == msgUI)
		{
			int eventID = msgUI.GetEventID();
			if (eventID == MessageBoxUI.EVENT_MAKE_PACKAGE_WARNING && command == 9)
			{
				msgUI.Hide();
			}
		}
	}
}
