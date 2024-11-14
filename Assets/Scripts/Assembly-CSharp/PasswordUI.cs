using UnityEngine;

public class PasswordUI : UIPanelX, UIHandler
{
	public enum Command
	{
		Confirm = 0,
		Back = 1
	}

	public UIStateManager stateMgr;

	public static byte PASSWORD_BEGIN_IMG = 3;

	public static byte PASSWORD_COUNT_IMG = 16;

	private UIBlock m_block;

	private UIImage shadowImg;

	private UIImage passwordBoxBGImg;

	private UIImage passwordBoxImg;

	private UITextButton backTxtBtn;

	private UITextButton confirmTxtBtn;

	private FrUIText passwordTxt;

	private UIImage passwordBGImg;

	private UIImage warningImg;

	private static byte[] PASSWORD_BACKGROUND = new byte[3] { 23, 24, 25 };

	private UISliderNum[] passwordNum;

	private UIImage[] passwordMaskImg;

	private bool bError;

	public PasswordUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public void Create()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[4];
		bError = false;
		m_block = new UIBlock();
		m_block.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
		Add(m_block);
		shadowImg = new UIImage();
		shadowImg.AddObject(unitUI, 1, 0);
		shadowImg.Rect = shadowImg.GetObjectRect();
		shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 1, 2);
		passwordBoxBGImg = new UIImage();
		passwordBoxBGImg.AddObject(unitUI, 1, 1);
		passwordBoxBGImg.Rect = passwordBoxBGImg.GetObjectRect();
		passwordBoxBGImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		passwordBoxImg = new UIImage();
		passwordBoxImg.AddObject(unitUI, 1, PASSWORD_BEGIN_IMG, PASSWORD_COUNT_IMG);
		passwordBoxImg.Rect = passwordBoxImg.GetObjectRect();
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 1, 22);
		backTxtBtn = new UITextButton();
		backTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 1, 22);
		backTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 1, 21);
		backTxtBtn.Rect = new Rect(modulePositionRect2.x - 15f, modulePositionRect2.y - 15f, modulePositionRect2.width + 30f, modulePositionRect2.height + 30f);
		backTxtBtn.SetText("font3", "CANCEL", UIConstant.fontColor_cyan);
		backTxtBtn.SetTextColor(Color.white, Color.white);
		Rect modulePositionRect3 = unitUI.GetModulePositionRect(0, 1, 20);
		confirmTxtBtn = new UITextButton();
		confirmTxtBtn.AddObject(UIButtonBase.State.Normal, unitUI, 1, 20);
		confirmTxtBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 1, 19);
		confirmTxtBtn.Rect = new Rect(modulePositionRect3.x - 15f, modulePositionRect3.y - 15f, modulePositionRect3.width + 30f, modulePositionRect3.height + 30f);
		confirmTxtBtn.SetText("font3", "OK", UIConstant.fontColor_cyan);
		confirmTxtBtn.SetTextColor(Color.white, Color.white);
		Rect modulePositionRect4 = unitUI.GetModulePositionRect(0, 1, 30);
		passwordTxt = new FrUIText();
		string text = UIConstant.GetMessage(35).Replace("[n]", "\n");
		passwordTxt.Set("font3", text, UIConstant.fontColor_cyan, modulePositionRect4.width);
		passwordTxt.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
		passwordTxt.Rect = modulePositionRect4;
		passwordBGImg = new UIImage();
		passwordBGImg.AddObject(unitUI, 1, PASSWORD_BACKGROUND);
		passwordBGImg.Rect = passwordBGImg.GetObjectRect();
		passwordMaskImg = new UIImage[4];
		for (int i = 0; i < 4; i++)
		{
			passwordMaskImg[i] = new UIImage();
			passwordMaskImg[i].AddObject(unitUI, 1, 26 + i);
			passwordMaskImg[i].Rect = passwordMaskImg[i].GetObjectRect();
		}
		passwordNum = new UISliderNum[4];
		Rect modulePositionRect5 = unitUI.GetModulePositionRect(0, 2, 0);
		for (int j = 0; j < 4; j++)
		{
			passwordNum[j] = new UISliderNum();
			passwordNum[j].Create(unitUI, 2, 0);
			for (int k = 0; k < 10; k++)
			{
				UISliderNum.UINumIcon uINumIcon = new UISliderNum.UINumIcon();
				uINumIcon.m_background = new UIImage();
				uINumIcon.m_background.AddObject(unitUI, 2, k);
				Rect objectRect = uINumIcon.m_background.GetObjectRect();
				uINumIcon.m_background.Rect = new Rect(passwordMaskImg[j].Rect.x + (passwordMaskImg[j].Rect.width - objectRect.width) * 0.5f, passwordMaskImg[j].Rect.y + (passwordMaskImg[j].Rect.height - objectRect.height) * 0.5f, objectRect.width, objectRect.height);
				uINumIcon.Visible = false;
				uINumIcon.Enable = false;
				uINumIcon.Rect = uINumIcon.m_background.Rect;
				passwordNum[j].Add(uINumIcon);
			}
			passwordNum[j].SetClipRect(passwordMaskImg[j].Rect.x, passwordMaskImg[j].Rect.y, passwordMaskImg[j].Rect.width, passwordMaskImg[j].Rect.height);
			float num = modulePositionRect5.height + 5f;
			Rect rct = new Rect(passwordNum[j].m_showRect.x - 10f, passwordNum[j].m_showRect.y - 10f, passwordNum[j].m_showRect.width + 20f, passwordNum[j].m_showRect.height + 20f);
			passwordNum[j].SetScroller(0f, 10f * num, num, rct, true);
			passwordNum[j].SetSelection(0);
		}
		warningImg = new UIImage();
		warningImg.AddObject(unitUI, 1, 31);
		warningImg.Rect = warningImg.GetObjectRect();
		Add(shadowImg);
		Add(passwordBoxImg);
		Add(passwordBoxBGImg);
		Add(passwordTxt);
		Add(warningImg);
		Add(passwordBGImg);
		UISliderNum[] array = passwordNum;
		foreach (UISliderNum control in array)
		{
			Add(control);
		}
		UIImage[] array2 = passwordMaskImg;
		foreach (UIImage control2 in array2)
		{
			Add(control2);
		}
		Add(backTxtBtn);
		Add(confirmTxtBtn);
		SetUIHandler(this);
	}

	public void Init()
	{
		bError = false;
		UnitUI unitUI = Res2DManager.GetInstance().vUI[4];
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 1, 30);
		string text = UIConstant.GetMessage(35).Replace("[n]", "\n");
		passwordTxt.Set("font3", text, UIConstant.fontColor_cyan, modulePositionRect.width);
		warningImg.SetTexture(unitUI, 1, 31);
		for (int i = 0; i < passwordNum.Length; i++)
		{
			passwordNum[i].SetSelection(0);
		}
	}

	public new void Clear()
	{
		UISliderNum[] array = passwordNum;
		foreach (UISliderNum uISliderNum in array)
		{
			uISliderNum.Clear();
		}
		base.Clear();
	}

	public void ShowErrorMsg()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[4];
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 1, 30);
		string text = UIConstant.GetMessage(32).Replace("[n]", "\n");
		passwordTxt.Set("font3", text, UIConstant.fontColor_cyan, modulePositionRect.width);
		warningImg.SetTexture(unitUI, 1, 32);
		bError = true;
	}

	public bool Verify(short roomPassword)
	{
		if (roomPassword == PasswordToShort())
		{
			return true;
		}
		return false;
	}

	private short PasswordToShort()
	{
		int num = 0;
		int num2 = 1;
		for (int num3 = passwordNum.Length - 1; num3 >= 0; num3--)
		{
			num += passwordNum[num3].GetSelectedIndex() * num2;
			num2 *= 10;
		}
		return (short)num;
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
		if (control == backTxtBtn)
		{
			m_Parent.SendEvent(this, 1, 0f, 0f);
			AudioManager.GetInstance().PlaySound(AudioName.CANCLE);
		}
		else if (control == confirmTxtBtn)
		{
			m_Parent.SendEvent(this, 0, 0f, 0f);
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
		}
		else
		{
			if (!bError)
			{
				return;
			}
			UISliderNum[] array = passwordNum;
			foreach (UISliderNum uISliderNum in array)
			{
				if (uISliderNum == control && (command == 1 || command == 0))
				{
					Init();
					break;
				}
			}
		}
	}
}
