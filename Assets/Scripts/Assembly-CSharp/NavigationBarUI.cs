using UnityEngine;

public class NavigationBarUI : UIPanelX, UIHandler
{
	public enum Command
	{
		Back = 0
	}

	public UIStateManager stateMgr;

	private static byte[] BG_IMG = new byte[4] { 0, 1, 2, 3 };

	private static byte[] TITLE_IMG = new byte[3] { 4, 5, 6 };

	private UIImage navigationBarImg;

	private UITextButton backTxtBtn;

	private UIImage titleImg;

	private FrUIText titleTxt;

	private UIImage cashImg;

	private UIImage energyImg;

	private UIImage mithrilImg;

	private UINumeric energyNum;

	private UINumeric mithrilNum;

	private UINumeric cashNum;

	private int cash;

	private int energy;

	private int mithril;

	public NavigationBarUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
	}

	public void Create()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[19];
		UnitUI ui2 = Res2DManager.GetInstance().vUI[17];
		navigationBarImg = new UIImage();
		navigationBarImg.AddObject(ui, 0, BG_IMG);
		navigationBarImg.Rect = navigationBarImg.GetObjectRect();
		titleImg = new UIImage();
		titleImg.AddObject(ui, 0, TITLE_IMG);
		titleImg.Rect = titleImg.GetObjectRect();
		titleTxt = new FrUIText();
		titleTxt.Set("font1", "STAR WARFARE", UIConstant.fontColor_cyan, titleImg.Rect.width);
		titleTxt.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
		titleTxt.Rect = titleImg.Rect;
		backTxtBtn = new UITextButton();
		backTxtBtn.AddObject(UIButtonBase.State.Normal, ui, 0, 7, 2);
		backTxtBtn.AddObject(UIButtonBase.State.Pressed, ui, 0, 9, 2);
		backTxtBtn.AddObject(UIButtonBase.State.Disabled, ui, 0, 9, 2);
		backTxtBtn.Rect = backTxtBtn.GetObjectRect(UIButtonBase.State.Normal);
		backTxtBtn.SetTextColor(UIConstant.fontColor_cyan, UIConstant.fontColor_cyan);
		cashImg = new UIImage();
		cashImg.AddObject(ui, 0, 12);
		cashImg.Rect = cashImg.GetObjectRect();
		energyImg = new UIImage();
		energyImg.AddObject(ui, 0, 13);
		energyImg.Rect = energyImg.GetObjectRect();
		mithrilImg = new UIImage();
		mithrilImg.AddObject(ui, 0, 11);
		mithrilImg.Rect = mithrilImg.GetObjectRect();
		cashNum = new UINumeric();
		cash = GameApp.GetInstance().GetUserState().GetCash();
		cashNum.AlignStyle = UINumeric.enAlignStyle.left;
		cashNum.SpacingOffsetX = -8f;
		cashNum.SetNumeric(ui2, 0, string.Format("{0:N0}", cash));
		cashNum.Rect = new Rect(cashImg.Rect.x + cashImg.Rect.width + 3f, cashImg.Rect.y, cashImg.Rect.width, cashImg.Rect.height + 2f);
		energyNum = new UINumeric();
		energy = GameApp.GetInstance().GetUserState().Enegy;
		energyNum.AlignStyle = UINumeric.enAlignStyle.left;
		energyNum.SpacingOffsetX = -8f;
		energyNum.SetNumeric(ui2, 0, string.Format("{0:N0}", energy));
		energyNum.Rect = new Rect(energyImg.Rect.x + energyImg.Rect.width + 3f, energyImg.Rect.y, energyImg.Rect.width, energyImg.Rect.height + 2f);
		mithrilNum = new UINumeric();
		mithril = GameApp.GetInstance().GetUserState().GetMithril();
		mithrilNum.AlignStyle = UINumeric.enAlignStyle.left;
		mithrilNum.SpacingOffsetX = -8f;
		mithrilNum.SetNumeric(ui2, 0, string.Format("{0:N0}", mithril));
		mithrilNum.Rect = new Rect(mithrilImg.Rect.x + mithrilImg.Rect.width + 3f, mithrilImg.Rect.y, mithrilImg.Rect.width, mithrilImg.Rect.height + 2f);
		Add(navigationBarImg);
		Add(titleImg);
		Add(titleTxt);
		Add(backTxtBtn);
		Add(cashImg);
		Add(energyImg);
		Add(mithrilImg);
		Add(cashNum);
		Add(energyNum);
		Add(mithrilNum);
		SetUIHandler(this);
	}

	public override void Update()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[19];
		UnitUI ui = Res2DManager.GetInstance().vUI[17];
		if (cash != GameApp.GetInstance().GetUserState().GetCash())
		{
			cash = GameApp.GetInstance().GetUserState().GetCash();
			cashNum.SetNumeric(ui, 0, string.Format("{0:N0}", cash));
		}
		if (energy != GameApp.GetInstance().GetUserState().Enegy)
		{
			energy = GameApp.GetInstance().GetUserState().Enegy;
			energyNum.SetNumeric(ui, 0, string.Format("{0:N0}", energy));
		}
		if (mithril != GameApp.GetInstance().GetUserState().GetMithril())
		{
			mithril = GameApp.GetInstance().GetUserState().GetMithril();
			mithrilNum.SetNumeric(ui, 0, string.Format("{0:N0}", mithril));
		}
	}

	public void SetTitle(string strTitle)
	{
		titleTxt.Set("font1", strTitle, UIConstant.fontColor_cyan, titleImg.Rect.width);
	}

	public void SetDisabledForBackBtn()
	{
		backTxtBtn.Enable = false;
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
			AudioManager.GetInstance().PlaySound(AudioName.CLICK_BACK);
			GameApp.GetInstance().Save();
			m_Parent.SendEvent(this, 0, 0f, 0f);
		}
	}
}
