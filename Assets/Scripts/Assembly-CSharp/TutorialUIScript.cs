using System;
using UnityEngine;

public class TutorialUIScript : UIStateManager, UIHandler
{
	private TutorialUI tutorialUI;

	private float startTime;

	protected Timer fadeTimer = new Timer();

	private void Awake()
	{
		tutorialUI = new TutorialUI(this);
	}

	private void Start()
	{
		InitUIManager();
		FrGoToPhase(0, false, true, true);
	}

	private void InitUIManager()
	{
		Res2DManager.GetInstance().Init(22);
		m_UIManager = base.gameObject.AddComponent("UIManager") as UIManager;
		m_UIManager.SetParameter(24, 1, false);
		m_UIManager.SetUIHandler(this);
		m_UIPopupManager = GetPopup();
		m_UIPopupManager.SetParameter(24, 3, false);
		UnitUI ui = Res2DManager.GetInstance().vUI[22];
		UIImage uIImage = new UIImage();
		uIImage.AddObject(ui, 0, 0);
		uIImage.Rect = uIImage.GetObjectRect();
		uIImage.SetColor(Color.black);
		uIImage.SetSize(new Vector2(Screen.width, Screen.height));
		m_UIManager.Add(uIImage);
		GameObject gameObject = GameObject.Find("MenuMusic");
		if (gameObject == null)
		{
			GameObject original = Resources.Load("Audio/MenuMusic") as GameObject;
			gameObject = UnityEngine.Object.Instantiate(original, new Vector3(-0.5f, 1f, 0f), Quaternion.identity) as GameObject;
			gameObject.name = "MenuMusic";
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}

	protected override void FrInit(int phase)
	{
		switch (phase)
		{
		case 0:
			Res2DManager.GetInstance().SetResUI(25);
			StartLoading(1, -1);
			break;
		case 20:
			tutorialUI.Init();
			break;
		}
	}

	protected override void FrClose(int phase)
	{
		if (phase != 0 && phase == 20)
		{
			tutorialUI.Close();
		}
	}

	protected override void FrUpdate(int phase)
	{
		switch (phase)
		{
		case 0:
			FrGoToPhase(20, false, false, false);
			break;
		case 1:
			if (Res2DManager.GetInstance().LoadResPro())
			{
				FrGoToPhase(FrGetPreviousPhase(), true, false, false);
			}
			break;
		case 20:
			tutorialUI.Update();
			break;
		}
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
	}

	public void StartLoading(byte mode, int nMax)
	{
		Res2DManager.GetInstance().LoadResInit(mode, nMax);
		FrGoToPhase(1, false, true, false);
	}

	public override void FrFree()
	{
		Res2DManager.GetInstance().FreeUI(25, true);
		m_UIManager.Destory();
		m_UIManager.RemoveAll();
		m_UIPopupManager.Destory();
		m_UIPopupManager.RemoveAll();
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
		m_UIManager.Add(uIImage);
	}
}
