using System;
using UnityEngine;

public class TutorialUI : UIHandler, IUIHandle
{
	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_HANDLE = 2;

	public UIStateManager stateMgr;

	private byte state;

	private UIImage[] tutorialImg;

	private UIScroller m_scroller = new UIScroller();

	protected int m_IconWidth;

	protected int m_IconHeight;

	protected Rect m_showRect;

	protected int m_selectIndex;

	private UIImage[] pageNavImg;

	private UIImage selectStagePageImg;

	private FadeAnimationScript fade;

	public TutorialUI(UIStateManager stateMgr)
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
	}

	public void Create()
	{
		stateMgr.m_UIManager.RemoveAll();
		stateMgr.m_UIPopupManager.RemoveAll();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[25];
		fade.FadeOutBlack();
		tutorialImg = new UIImage[6];
		for (int i = 0; i < tutorialImg.Length; i++)
		{
			tutorialImg[i] = new UIImage();
			tutorialImg[i].AddObject(unitUI, i);
			tutorialImg[i].Rect = tutorialImg[i].GetObjectRect();
			stateMgr.m_UIManager.Add(tutorialImg[i]);
		}
		m_IconWidth = (int)tutorialImg[0].Rect.width;
		m_IconHeight = (int)tutorialImg[0].Rect.height;
		m_showRect = tutorialImg[0].Rect;
		SetScroller(0f, m_IconWidth * 5, m_IconWidth, m_showRect);
		stateMgr.m_UIManager.Add(m_scroller);
		pageNavImg = new UIImage[6];
		int num = 64;
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 6, 0);
		float num2 = modulePositionRect.x + modulePositionRect.width * 0.5f - (float)(5 * num) * 0.5f;
		for (int j = 0; j < pageNavImg.Length; j++)
		{
			pageNavImg[j] = new UIImage();
			pageNavImg[j].AddObject(unitUI, 6, 0);
			pageNavImg[j].Rect = new Rect(num2 - modulePositionRect.width * 0.5f, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
			num2 += (float)num;
			stateMgr.m_UIManager.Add(pageNavImg[j]);
		}
		selectStagePageImg = new UIImage();
		selectStagePageImg.AddObject(unitUI, 6, 1);
		selectStagePageImg.Rect = pageNavImg[m_selectIndex].Rect;
		stateMgr.m_UIManager.Add(selectStagePageImg);
		UnitUI ui = Res2DManager.GetInstance().vUI[22];
		float num3 = (float)Screen.width / (float)Screen.height - UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight;
		if (num3 > 0f)
		{
			float num4 = (int)Math.Abs((float)Screen.height * UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight - (float)Screen.width) / 2;
			num4 *= 1.5f;
			UIImage uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(num4, UIConstant.ScreenLocalHeight));
			uIImage.Rect = new Rect((0f - num4) / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			stateMgr.m_UIManager.Add(uIImage);
			uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(num4, UIConstant.ScreenLocalHeight));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth + num4 / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			stateMgr.m_UIManager.Add(uIImage);
		}
		else if (num3 < 0f)
		{
			float num5 = (int)Math.Abs((float)Screen.width * UIConstant.ScreenLocalHeight / UIConstant.ScreenLocalWidth - (float)Screen.height) / 2;
			num5 *= 1.5f;
			UIImage uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num5));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, (0f - num5) / 2f, 0f, 0f);
			stateMgr.m_UIManager.Add(uIImage);
			uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num5));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, UIConstant.ScreenLocalHeight + num5 / 2f, 0f, 0f);
			stateMgr.m_UIManager.Add(uIImage);
		}
		Debug.Log("create end!");
	}

	public void SetScroller(float min, float max, float spacing, Rect rct)
	{
		m_scroller.Loop = false;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Horizontal, min, max, spacing);
		m_scroller.Rect = rct;
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
			UpdateTutorial();
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

	private void UpdateTutorial()
	{
		float num = 0f;
		int num2 = (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
		float num3 = m_showRect.x + m_showRect.width * 0.5f;
		float num4 = m_showRect.y + m_showRect.height * 0.5f;
		int num5 = tutorialImg.Length;
		for (int i = 0; i < num5; i++)
		{
			UIImage uIImage = tutorialImg[i];
			num = num3 + m_scroller.Spacing * (float)i - m_scroller.ScrollerPos - 0.5f * (float)m_IconWidth;
			uIImage.Rect = new Rect(num, num4 - (float)m_IconHeight * 0.5f, m_IconWidth, m_IconHeight);
			uIImage.SetClip(m_showRect);
		}
	}

	private void GotoSolo()
	{
		GameApp.GetInstance().GetUserState().SetFirstLunchApp(false);
		stateMgr.FrFree();
		Application.LoadLevel("SoloMenu");
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control != m_scroller)
		{
			return;
		}
		switch (command)
		{
		case 1:
			if (m_selectIndex == 5 && wparam > 0f)
			{
				GotoSolo();
			}
			break;
		case 4:
			if (m_selectIndex == 5)
			{
				GotoSolo();
				break;
			}
			m_selectIndex++;
			m_selectIndex %= 6;
			m_scroller.ScrollerPos = (float)m_selectIndex * m_scroller.Spacing;
			selectStagePageImg.Rect = pageNavImg[m_selectIndex].Rect;
			AudioManager.GetInstance().PlaySound(AudioName.SWITCH_ITEM);
			break;
		case 3:
			m_selectIndex = (int)wparam;
			selectStagePageImg.Rect = pageNavImg[m_selectIndex].Rect;
			AudioManager.GetInstance().PlaySound(AudioName.SWITCH_ITEM);
			break;
		}
	}
}
