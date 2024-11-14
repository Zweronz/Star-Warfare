using UnityEngine;

public class OptionsUI : UIHandler, IUIHandle
{
	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_HANDLE = 2;

	private const byte TYPE_SOUND = 0;

	private const byte TYPE_CONTROLS = 1;

	private const byte TYPE_ADVANCED = 2;

	public UIStateManager stateMgr;

	private UserState userState;

	private byte state;

	public static byte[] BG_IMG = new byte[2] { 0, 1 };

	private NavigationBarUI navigationBar;

	private NavigationMenuUI navigationMenu;

	private UIImage optionImg;

	private UIImage frameBGImg;

	private UIImage frameImg;

	private UISliderCustomize musicVolumeSlider;

	private UISliderCustomize soundVolumeSlider;

	private UISliderCustomize sensitivitySlider;

	private int ENERGY_BEGIN_WIDTH = 54;

	private int ENERGY_END_WIDTH = 24;

	private static byte FRAME_BEGIN_IMG = 4;

	private static byte FRAME_COUNT_IMG = 30;

	private UIClickButton[] categoryBtn;

	private UIImage selectCategoryImg;

	private FrUIText BGMTitle;

	private FrUIText SFXTitle;

	private FrUIText ConTitle;

	private FrUIText BladePadTitle;

	private FrUIText UDIDText;

	private UIClickButton confirmBtn;

	private static byte[] CONFIRM_NORMAL = new byte[2] { 38, 54 };

	private static byte[] CONFIRM_PRESSED = new byte[2] { 37, 53 };

	private byte curType;

	private UISliderSwitch music;

	private UISliderSwitch sound;

	private UISliderSwitch bladepad;

	private bool bPlayMusic = true;

	private bool bPlaySound = true;

	private bool bBladePade;

	private float musicVolume = 1f;

	private float soundVolume = 1f;

	private byte sensitivity = 1;

	private static byte[] categorySound = new byte[2] { 34, 49 };

	private static byte[] categoryControl = new byte[2] { 35, 50 };

	private static byte[] DECORATE_ICONS = new byte[4] { 55, 56, 57, 58 };

	private UIImage decorateImg;

	public OptionsUI(UIStateManager stateMgr)
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
		userState = GameApp.GetInstance().GetUserState();
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
		UnitUI unitUI = Res2DManager.GetInstance().vUI[8];
		optionImg = new UIImage();
		optionImg.AddObject(unitUI, 0, BG_IMG);
		optionImg.Rect = optionImg.GetObjectRect();
		navigationBar = new NavigationBarUI(stateMgr);
		navigationBar.Create();
		navigationBar.SetTitle("OPTIONS");
		navigationBar.Show();
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 3);
		frameBGImg = new UIImage();
		frameBGImg.AddObject(unitUI, 0, 2);
		frameBGImg.Rect = frameBGImg.GetObjectRect();
		frameBGImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		frameImg = new UIImage();
		frameImg.AddObject(unitUI, 0, FRAME_BEGIN_IMG, FRAME_COUNT_IMG);
		frameImg.Rect = frameImg.GetObjectRect();
		stateMgr.m_UIManager.Add(optionImg);
		stateMgr.m_UIManager.Add(navigationBar);
		stateMgr.m_UIManager.Add(frameImg);
		stateMgr.m_UIManager.Add(frameBGImg);
		categoryBtn = new UIClickButton[2];
		for (int i = 0; i < categoryBtn.Length; i++)
		{
			categoryBtn[i] = new UIClickButton();
			if (i == 0)
			{
				categoryBtn[i].AddObject(UIButtonBase.State.Normal, unitUI, 0, categorySound);
				categoryBtn[i].AddObject(UIButtonBase.State.Pressed, unitUI, 0, categorySound);
			}
			else
			{
				categoryBtn[i].AddObject(UIButtonBase.State.Normal, unitUI, 0, categoryControl);
				categoryBtn[i].AddObject(UIButtonBase.State.Pressed, unitUI, 0, categoryControl);
			}
			categoryBtn[i].Rect = categoryBtn[i].GetObjectRect(UIButtonBase.State.Normal);
			stateMgr.m_UIManager.Add(categoryBtn[i]);
		}
		selectCategoryImg = new UIImage();
		selectCategoryImg.AddObject(unitUI, 0, 36);
		selectCategoryImg.Rect = categoryBtn[curType].Rect;
		stateMgr.m_UIManager.Add(selectCategoryImg);
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 51);
		BGMTitle = new FrUIText();
		BGMTitle.Rect = modulePositionRect2;
		BGMTitle.AlignStyle = FrUIText.enAlignStyle.TOP_RIGHT;
		BGMTitle.Set("font2", "MUSIC", UIConstant.FONT_COLOR_EQUIP_NAME, modulePositionRect2.width);
		stateMgr.m_UIManager.Add(BGMTitle);
		modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 52);
		SFXTitle = new FrUIText();
		SFXTitle.Rect = modulePositionRect2;
		SFXTitle.AlignStyle = FrUIText.enAlignStyle.TOP_RIGHT;
		SFXTitle.Set("font2", "SOUND FX", UIConstant.FONT_COLOR_EQUIP_NAME, modulePositionRect2.width);
		stateMgr.m_UIManager.Add(SFXTitle);
		modulePositionRect2 = new Rect(modulePositionRect2.x - 100f, modulePositionRect2.y - 150f, 100f, 100f);
		UDIDText = new FrUIText();
		UDIDText.Rect = modulePositionRect2;
		UDIDText.AlignStyle = FrUIText.enAlignStyle.TOP_LEFT;
		if (AndroidConstant.version == AndroidConstant.Version.GooglePlay)
		{
			UDIDText.Set("font2", "P:" + GameApp.GetInstance().UUID, UIConstant.FONT_COLOR_EQUIP_NAME, modulePositionRect2.width);
		}
		else if (AndroidConstant.version == AndroidConstant.Version.Kindle)
		{
			UDIDText.Set("font2", "K:" + GameApp.GetInstance().UUID, UIConstant.FONT_COLOR_EQUIP_NAME, modulePositionRect2.width);
		}
		stateMgr.m_UIManager.Add(UDIDText);
		modulePositionRect2 = unitUI.GetModulePositionRect(0, 1, 3);
		ConTitle = new FrUIText();
		ConTitle.Rect = modulePositionRect2;
		ConTitle.AlignStyle = FrUIText.enAlignStyle.TOP_LEFT;
		ConTitle.Set("font2", "SENSITIVITY", UIConstant.FONT_COLOR_EQUIP_NAME, modulePositionRect2.width);
		stateMgr.m_UIManager.Add(ConTitle);
		musicVolume = userState.GetMusicVolume();
		musicVolumeSlider = new UISliderCustomize();
		UISliderCustomize.UISliderStruct sliderStruct = new UISliderCustomize.UISliderStruct(0, 46, 0, 47, 0, 48);
		musicVolumeSlider.Create(unitUI, sliderStruct);
		Rect modulePositionRect3 = unitUI.GetModulePositionRect(0, 0, 46);
		musicVolumeSlider.SetScroller(0f, 400f, 4f, 46f, modulePositionRect3);
		musicVolumeSlider.SetSelection((int)(musicVolume * 100f));
		stateMgr.m_UIManager.Add(musicVolumeSlider);
		soundVolume = userState.GetSoundVolume();
		soundVolumeSlider = new UISliderCustomize();
		UISliderCustomize.UISliderStruct sliderStruct2 = new UISliderCustomize.UISliderStruct(0, 41, 0, 42, 0, 43);
		soundVolumeSlider.Create(unitUI, sliderStruct2);
		modulePositionRect3 = unitUI.GetModulePositionRect(0, 0, 41);
		soundVolumeSlider.SetScroller(0f, 400f, 4f, 46f, modulePositionRect3);
		soundVolumeSlider.SetSelection((int)(soundVolume * 100f));
		stateMgr.m_UIManager.Add(soundVolumeSlider);
		music = new UISliderSwitch();
		music.Create(unitUI);
		InitMusic();
		stateMgr.m_UIManager.Add(music);
		sound = new UISliderSwitch();
		sound.Create(unitUI);
		InitSound();
		stateMgr.m_UIManager.Add(sound);
		sensitivity = (byte)userState.TouchInputSensitivity;
		sensitivitySlider = new UISliderCustomize();
		UISliderCustomize.UISliderStruct sliderStruct3 = new UISliderCustomize.UISliderStruct(1, 0, 1, 1, 1, 2);
		sensitivitySlider.Create(unitUI, sliderStruct3);
		modulePositionRect3 = unitUI.GetModulePositionRect(0, 1, 0);
		sensitivitySlider.SetScroller(0f, 464f, 232f, 17f, modulePositionRect3);
		sensitivitySlider.SetSelection((int)sensitivity);
		sensitivitySlider.SetWidthFactor(0.92f);
		stateMgr.m_UIManager.Add(sensitivitySlider);
		confirmBtn = new UIClickButton();
		confirmBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, CONFIRM_NORMAL);
		confirmBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, CONFIRM_PRESSED);
		confirmBtn.Rect = confirmBtn.GetObjectRect(UIButtonBase.State.Normal);
		decorateImg = new UIImage();
		decorateImg.AddObject(unitUI, 0, DECORATE_ICONS);
		decorateImg.Rect = decorateImg.GetObjectRect();
		SetOption(curType);
		stateMgr.m_UIManager.Add(confirmBtn);
		stateMgr.m_UIManager.Add(decorateImg);
		navigationMenu = new NavigationMenuUI(stateMgr);
		navigationMenu.Create();
		navigationMenu.Show();
		stateMgr.m_UIPopupManager.Add(navigationMenu);
	}

	private void SetEnableForSoundPanel(bool enable)
	{
		if (enable)
		{
			musicVolumeSlider.Show();
			soundVolumeSlider.Show();
			music.Show();
			sound.Show();
			SFXTitle.Visible = enable;
			BGMTitle.Visible = enable;
			decorateImg.Visible = enable;
		}
		else
		{
			musicVolumeSlider.Hide();
			soundVolumeSlider.Hide();
			music.Hide();
			sound.Hide();
			SFXTitle.Visible = enable;
			BGMTitle.Visible = enable;
			decorateImg.Visible = enable;
		}
	}

	private void SetEnableForControlPanel(bool enable)
	{
		if (enable)
		{
			sensitivitySlider.Show();
			ConTitle.Visible = enable;
		}
		else
		{
			sensitivitySlider.Hide();
			ConTitle.Visible = enable;
		}
	}

	public void InitSound()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[8];
		sound.SetBackground(ui, 0, 40);
		sound.SetSlider(ui, 0, 39);
		sound.SetClipRect(sound.backgroundImg.Rect.x + 1f, sound.backgroundImg.Rect.y, sound.backgroundImg.Rect.width - 2f, sound.backgroundImg.Rect.height);
		sound.SetScroller(0f, 67f, 67f, sound.backgroundImg.Rect);
		bPlaySound = userState.GetPlaySound();
		sound.SetSelection((!bPlaySound) ? 1 : 0);
	}

	public void InitMusic()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[8];
		music.SetBackground(ui, 0, 45);
		music.SetSlider(ui, 0, 44);
		music.SetClipRect(music.backgroundImg.Rect.x + 1f, music.backgroundImg.Rect.y, music.backgroundImg.Rect.width - 2f, music.backgroundImg.Rect.height);
		music.SetScroller(0f, 67f, 67f, music.backgroundImg.Rect);
		bPlayMusic = userState.GetPlayMusic();
		music.SetSelection((!bPlayMusic) ? 1 : 0);
	}

	public void InitBladePad()
	{
	}

	public void SetOption(byte type)
	{
		curType = type;
		UnitUI unitUI = Res2DManager.GetInstance().vUI[8];
		switch (type)
		{
		case 0:
			SetEnableForSoundPanel(true);
			SetEnableForControlPanel(false);
			break;
		case 1:
			SetEnableForSoundPanel(false);
			SetEnableForControlPanel(true);
			break;
		case 2:
			break;
		}
	}

	public bool Update()
	{
		switch (state)
		{
		case 0:
			state = 1;
			break;
		case 1:
			Create();
			state = 2;
			break;
		case 2:
		{
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

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == navigationBar)
		{
			if (Application.loadedLevelName.Equals("ShopAndCustomize"))
			{
				stateMgr.FrFree();
				UIConstant.ExitShopAndCustomize();
			}
			else
			{
				stateMgr.FrGoToPhase(stateMgr.FrGetPreviousPhase(), false, false, false);
			}
			return;
		}
		if (control == navigationMenu)
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
			case 5:
				stateMgr.FrGoToPhase(11, false, false, true);
				break;
			}
			return;
		}
		if (control == sound)
		{
			bPlaySound = wparam == 0f;
			userState.SetPlaySound(bPlaySound);
			return;
		}
		if (control == music)
		{
			bPlayMusic = wparam == 0f;
			userState.SetPlayMusic(bPlayMusic);
			return;
		}
		if (control == musicVolumeSlider)
		{
			musicVolume = wparam / 100f;
			userState.SetMusicVolume(musicVolume);
			if (!bPlayMusic)
			{
				bPlayMusic = true;
				userState.SetPlayMusic(bPlayMusic);
				music.m_scroller.Velocity = new Vector2(-10f, -10f);
				music.m_scroller.Moving = true;
			}
			return;
		}
		if (control == soundVolumeSlider)
		{
			soundVolume = wparam / 100f;
			userState.SetSoundVolume(soundVolume);
			if (!bPlaySound)
			{
				bPlaySound = true;
				userState.SetPlaySound(bPlaySound);
				sound.m_scroller.Velocity = new Vector2(-10f, -10f);
				sound.m_scroller.Moving = true;
			}
			return;
		}
		if (control == sensitivitySlider)
		{
			sensitivity = (byte)wparam;
			userState.TouchInputSensitivity = (InputSensitivity)sensitivity;
			return;
		}
		if (control == confirmBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			stateMgr.FrGoToPhase(stateMgr.FrGetPreviousPhase(), false, false, false);
			GameApp.GetInstance().Save();
			return;
		}
		for (int i = 0; i < categoryBtn.Length; i++)
		{
			if (categoryBtn[i] == control)
			{
				SetOption((byte)i);
				selectCategoryImg.Rect = categoryBtn[curType].Rect;
				break;
			}
		}
	}
}
