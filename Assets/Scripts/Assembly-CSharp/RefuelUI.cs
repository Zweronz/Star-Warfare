using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RefuelUI : UIPanelX, UIHandler
{
	public enum Command
	{
		Confirm = 0,
		Back = 1
	}

	private const byte COMBO_BOX_GUN_COST = 2;

	public UIStateManager stateMgr;

	public static byte REFUEL_BEGIN_IMG = 3;

	public static byte REFUEL_COUNT_IMG = 16;

	private UIBlock m_block;

	private UIImage shadowImg;

	private UIImage refuelBGImg;

	private UIImage refuelImg;

	private UITextButton backTxtBtn;

	private UITextButton confirmTxtBtn;

	private UITextImage costTxtImg;

	private UIImage costImg;

	private UISliderCustomize energySlider;

	private UIImage cashImg;

	private FrUIText cashTxt;

	private int ENERGY_BEGIN_WIDTH = 54;

	private int ENERGY_END_WIDTH = 24;

	public int energy;

	public int cash;

	private float energyFactor;

	private string strEnergy = "0";

	private Rect energyTxtRect;

	private static byte[] DECORATE_ICONS = new byte[2] { 29, 30 };

	private UIImage decorateImg;

	private int gunId;

	private int gunIconWidth;

	public RefuelUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public void Create()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[18];
		UnitUI ui = Res2DManager.GetInstance().vUI[20];
		m_block = new UIBlock();
		m_block.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
		Add(m_block);
		shadowImg = new UIImage();
		shadowImg.AddObject(unitUI, 0, 0);
		shadowImg.Rect = shadowImg.GetObjectRect();
		shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 2);
		refuelBGImg = new UIImage();
		refuelBGImg.AddObject(unitUI, 0, 1);
		refuelBGImg.Rect = refuelBGImg.GetObjectRect();
		refuelBGImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		refuelImg = new UIImage();
		refuelImg.AddObject(unitUI, 0, REFUEL_BEGIN_IMG, REFUEL_COUNT_IMG);
		refuelImg.Rect = refuelImg.GetObjectRect();
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 28);
		backTxtBtn = new UITextButton();
		backTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 28);
		backTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 27);
		backTxtBtn.Rect = new Rect(modulePositionRect2.x - 15f, modulePositionRect2.y - 15f, modulePositionRect2.width + 30f, modulePositionRect2.height + 30f);
		backTxtBtn.SetText("font3", "CANCEL", UIConstant.fontColor_cyan);
		backTxtBtn.SetTextColor(Color.white, Color.white);
		Rect modulePositionRect3 = unitUI.GetModulePositionRect(0, 0, 26);
		confirmTxtBtn = new UITextButton();
		confirmTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, 26);
		confirmTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, 25);
		confirmTxtBtn.Rect = new Rect(modulePositionRect3.x - 15f, modulePositionRect3.y - 15f, modulePositionRect3.width + 30f, modulePositionRect3.height + 30f);
		confirmTxtBtn.SetText("font3", "OK", UIConstant.fontColor_cyan);
		confirmTxtBtn.SetTextColor(Color.white, Color.white);
		modulePositionRect = unitUI.GetModulePositionRect(0, 0, 20);
		gunIconWidth = (int)(modulePositionRect.width * 0.3f);
		List<Weapon> battleWeapons = GameApp.GetInstance().GetUserState().GetBattleWeapons();
		Weapon weapon = battleWeapons[0];
		gunId = weapon.GunID;
		costTxtImg = new UITextImage();
		costTxtImg.AddObject(unitUI, 0, 20);
		string text = "unlimited";
		if (weapon.EnegyConsume != 0)
		{
			text = Convert.ToString(GameApp.GetInstance().GetUserState().Enegy / weapon.EnegyConsume);
		}
		costTxtImg.SetText("font2", UIConstant.GUN_COST + weapon.EnegyConsume + "\n" + UIConstant.GUN_AMMO + text, UIConstant.fontColor_white, FrUIText.enAlignStyle.TOP_LEFT);
		costTxtImg.SetTextOffset(gunIconWidth + 5, 0f);
		costTxtImg.SetTextLineSpacing(-2f);
		costTxtImg.Rect = new Rect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
		costImg = new UIImage();
		costImg.AddObject(ui, 0, weapon.GunID);
		costImg.SetSize(new Vector2(gunIconWidth, costTxtImg.Rect.height));
		costImg.Rect = new Rect(costTxtImg.Rect.x + 5f, costTxtImg.Rect.y, gunIconWidth, costTxtImg.Rect.height);
		energyTxtRect = unitUI.GetModulePositionRect(0, 0, 21);
		energyTxtRect.y = UIConstant.ScreenLocalHeight - energyTxtRect.y - energyTxtRect.height;
		energyTxtRect = UIConstant.GetRectForScreenAdaptived(energyTxtRect);
		energySlider = new UISliderCustomize();
		UISliderCustomize.UISliderStruct sliderStruct = new UISliderCustomize.UISliderStruct(0, 22, 0, 23, 0, 24);
		energySlider.Create(unitUI, sliderStruct);
		Rect modulePositionRect4 = unitUI.GetModulePositionRect(0, 0, 22);
		energySlider.SetScroller(0f, 400f, 1f, 46f, modulePositionRect4);
		energySlider.SetScrollerMinSpeed(1);
		energySlider.SetSelection(0f);
		cashImg = new UIImage();
		cashImg.AddObject(unitUI, 0, 19);
		cashImg.Rect = cashImg.GetObjectRect();
		cashTxt = new FrUIText();
		cashTxt.Set("font3", GameApp.GetInstance().GetUserState().GetCash()
			.ToString(), UIConstant.fontColor_cyan, cashImg.Rect.width);
		cashTxt.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
		cashTxt.Rect = cashImg.Rect;
		decorateImg = new UIImage();
		decorateImg.AddObject(unitUI, 0, DECORATE_ICONS);
		decorateImg.Rect = decorateImg.GetObjectRect();
		Add(shadowImg);
		Add(refuelImg);
		Add(refuelBGImg);
		Add(backTxtBtn);
		Add(confirmTxtBtn);
		Add(costTxtImg);
		Add(costImg);
		Add(cashImg);
		Add(cashTxt);
		Add(energySlider);
		Add(decorateImg);
		SetUIHandler(this);
	}

	public void Init()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[18];
		energy = 0;
		cash = 0;
		strEnergy = Convert.ToString(energy);
		energyFactor = (float)GameApp.GetInstance().GetUserState().GetCash() / 400f;
		float a = 400f / (float)GameApp.GetInstance().GetUserState().GetCash();
		a = Mathf.Max(a, 1f);
	}

	public void UpdateEnergy()
	{
		if (energySlider.GetScrollerMoving())
		{
			cash = (energy = (int)(energySlider.GetScrollerPos() * energyFactor));
			strEnergy = FormatNum(energy);
		}
		cashTxt.SetText(Convert.ToString(GameApp.GetInstance().GetUserState().GetCash() - cash));
		List<Weapon> battleWeapons = GameApp.GetInstance().GetUserState().GetBattleWeapons();
		Weapon weapon = battleWeapons[0];
		if (gunId != weapon.GunID)
		{
			gunId = weapon.GunID;
			ChangeWeapon(gunId);
		}
		string text = "unlimited";
		if (weapon.EnegyConsume != 0)
		{
			text = Convert.ToString((GameApp.GetInstance().GetUserState().Enegy + energy) / weapon.EnegyConsume);
		}
		costTxtImg.SetText(UIConstant.GUN_COST + weapon.EnegyConsume + "\n" + UIConstant.GUN_AMMO + text);
	}

	public void ChangeWeapon(int gunID)
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[20];
		if (unitUI != null)
		{
			costImg.SetTexture(unitUI, 0, gunID);
			costImg.SetSize(new Vector2(gunIconWidth, costTxtImg.Rect.height));
			costImg.Rect = new Rect(costTxtImg.Rect.x + 5f, costTxtImg.Rect.y, gunIconWidth, costTxtImg.Rect.height);
		}
	}

	private string FormatNum(int val)
	{
		return string.Format("{0:N0}", val);
	}

	public void OnGUI()
	{
		stateMgr.txtStyle.padding.right = UIConstant.GetWidthForScreenAdaptived(28);
		stateMgr.txtStyle.padding.bottom = UIConstant.GetWidthForScreenAdaptived(30);
		strEnergy = GUI.TextField(energyTxtRect, strEnergy, 11, stateMgr.txtStyle);
		strEnergy = Regex.Replace(strEnergy, "[^0-9]", string.Empty);
		if (strEnergy.Length <= 0)
		{
			return;
		}
		energy = int.Parse(strEnergy.Replace(",", " "));
		energy = Mathf.Clamp(energy, 0, GameApp.GetInstance().GetUserState().GetCash());
		strEnergy = FormatNum(energy);
		if (!energySlider.GetScrollerMoving())
		{
			if (energyFactor != 0f)
			{
				energySlider.SetSelection((float)energy / energyFactor);
			}
			else
			{
				energySlider.SetSelection(0f);
			}
			energy = energy;
			cash = energy;
		}
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
		if (control == energySlider)
		{
			cash = (energy = (int)(energySlider.GetScrollerPos() * energyFactor));
			strEnergy = FormatNum(energy);
		}
		else if (control == backTxtBtn)
		{
			m_Parent.SendEvent(this, 1, 0f, 0f);
			AudioManager.GetInstance().PlaySound(AudioName.CANCLE);
		}
		else if (control == confirmTxtBtn)
		{
			m_Parent.SendEvent(this, 0, 0f, 0f);
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
		}
	}
}
