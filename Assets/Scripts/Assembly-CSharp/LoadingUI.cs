using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoadingUI : UIHandler, IUIHandle
{
	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_HANDLE = 2;

	public UIStateManager stateMgr;

	protected NetworkManager networkMgr;

	private byte state;

	private UIImage shadowImg;

	private UIImage loadImg;

	private UIImage loadLightImg;

	private float rotate;

	private FrUIText tipsTxt;

	private FadeAnimationScript fade;

	private UIImage topLineImg;

	private UIImage bottomLineImg;

	public LoadingUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
		state = 0;
	}

	public void Init()
	{
		fade = FadeAnimationScript.GetInstance();
		state = 0;
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
	}

	public void Close()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
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
		stateMgr.m_UIPopupManager.RemoveAll();
		rotate = 0f;
		fade.FadeOutBlack();
		UnitUI ui = Res2DManager.GetInstance().vUI[22];
		shadowImg = new UIImage();
		shadowImg.AddObject(ui, 0, 0);
		shadowImg.Rect = shadowImg.GetObjectRect();
		shadowImg.SetColor(Color.black);
		shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		loadImg = new UIImage();
		loadImg.AddObject(ui, 0, 1);
		loadImg.Rect = loadImg.GetObjectRect();
		loadLightImg = new UIImage();
		loadLightImg.AddObject(ui, 0, 2);
		loadLightImg.Rect = loadLightImg.GetObjectRect();
		string[] gameText = Res2DManager.GetInstance().GetGameText();
		string[] array = Res2DManager.GetInstance().SplitString(gameText[6]);
		int num = Random.Range(0, array.Length);
		tipsTxt = new FrUIText();
		tipsTxt.AlignStyle = FrUIText.enAlignStyle.TOP_CENTER;
		tipsTxt.Set("font2", array[num], UIConstant.FONT_COLOR_TIPS, UIConstant.ScreenLocalWidth - 150f);
		tipsTxt.Rect = new Rect(75f, 50f, UIConstant.ScreenLocalWidth - 150f, loadLightImg.Rect.y - 150f);
		topLineImg = new UIImage();
		topLineImg.AddObject(ui, 1, 4, 2);
		topLineImg.Rect = new Rect(75f, loadLightImg.Rect.y - 150f + 50f, UIConstant.ScreenLocalWidth - 150f, 10f);
		float getTextHeight = tipsTxt.GetTextHeight;
		bottomLineImg = new UIImage();
		bottomLineImg.AddObject(ui, 1, 4, 2);
		bottomLineImg.Rect = new Rect(75f, loadLightImg.Rect.y - 150f + 50f - getTextHeight - 10f, UIConstant.ScreenLocalWidth - 150f, 10f);
		stateMgr.m_UIManager.Add(shadowImg);
		stateMgr.m_UIManager.Add(loadImg);
		stateMgr.m_UIManager.Add(loadLightImg);
		stateMgr.m_UIManager.Add(tipsTxt);
		stateMgr.m_UIManager.Add(bottomLineImg);
		stateMgr.m_UIManager.Add(topLineImg);
		UnitUI ui2 = Res2DManager.GetInstance().vUI[22];
		float num2 = (float)Screen.width / (float)Screen.height - UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight;
		if (num2 > 0f)
		{
			float num3 = (int)Math.Abs((float)Screen.height * UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight - (float)Screen.width) / 2;
			num3 *= 1.5f;
			UIImage uIImage = new UIImage();
			uIImage.AddObject(ui2, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(num3, UIConstant.ScreenLocalHeight));
			uIImage.Rect = new Rect((0f - num3) / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
			uIImage = new UIImage();
			uIImage.AddObject(ui2, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(num3, UIConstant.ScreenLocalHeight));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth + num3 / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
		}
		else if (num2 < 0f)
		{
			float num4 = (int)Math.Abs((float)Screen.width * UIConstant.ScreenLocalHeight / UIConstant.ScreenLocalWidth - (float)Screen.height) / 2;
			num4 *= 1.5f;
			UIImage uIImage = new UIImage();
			uIImage.AddObject(ui2, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num4));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, (0f - num4) / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
			uIImage = new UIImage();
			uIImage.AddObject(ui2, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num4));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, UIConstant.ScreenLocalHeight + num4 / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
		}
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
			rotate -= 0.2f;
			rotate %= 360f;
			loadLightImg.SetRotation(rotate);
			break;
		}
		return false;
	}

	public bool FadeOutComplete()
	{
		return fade.FadeOutComplete();
	}

	public void FadeOutBlack()
	{
		fade.FadeOutBlack();
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
	}
}
