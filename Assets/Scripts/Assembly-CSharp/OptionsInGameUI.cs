using System;
using UnityEngine;

public class OptionsInGameUI : UIHandler, IUIHandle
{
	private const byte STATE_INIT = 0;

	private const byte STATE_CREATE = 1;

	private const byte STATE_HANDLE = 2;

	public UIStateManager stateMgr;

	private UserState userState;

	private byte state;

	private UIImage shadowImg;

	private FrUIText BGMTitle;

	private FrUIText SFXTitle;

	private UIImage frameBGImg;

	private UIImage frameImg;

	private UISliderCustomize musicVolumeSlider;

	private UISliderCustomize soundVolumeSlider;

	private int ENERGY_BEGIN_WIDTH = 54;

	private int ENERGY_END_WIDTH = 24;

	private static byte FRAME_BEGIN_IMG = 3;

	private static byte FRAME_COUNT_IMG = 30;

	private UIClickButton backBtn;

	private static byte[] BACK_NORMAL = new byte[2] { 34, 48 };

	private static byte[] BACK_PRESSED = new byte[2] { 33, 47 };

	private UISliderSwitch music;

	private UISliderSwitch sound;

	private static byte TYPE_SOUND = 0;

	private static byte TYPE_CONTROLS = 1;

	private static byte TYPE_ADVANCED = 2;

	private bool bPlayMusic = true;

	private bool bPlaySound = true;

	private float musicVolume = 1f;

	private float soundVolume = 1f;

	private static byte[] DECORATE_ICONS = new byte[4] { 49, 50, 51, 52 };

	private UIImage decorateImg;

	public OptionsInGameUI(UIStateManager stateMgr)
	{
		this.stateMgr = stateMgr;
		state = 0;
	}

	public void Init()
	{
		Time.timeScale = 0f;
		state = 0;
		stateMgr.m_UIManager.SetEnable(true);
		stateMgr.m_UIManager.SetUIHandler(this);
		stateMgr.m_UIPopupManager.SetUIHandler(this);
		userState = GameApp.GetInstance().GetUserState();
	}

	public void Close()
	{
		stateMgr.m_UIManager.Remove(shadowImg);
		stateMgr.m_UIManager.Remove(frameBGImg);
		stateMgr.m_UIManager.Remove(frameImg);
		stateMgr.m_UIManager.Remove(BGMTitle);
		stateMgr.m_UIManager.Remove(SFXTitle);
		stateMgr.m_UIManager.Remove(backBtn);
		stateMgr.m_UIManager.Remove(decorateImg);
		stateMgr.m_UIManager.Remove(music);
		stateMgr.m_UIManager.Remove(sound);
		stateMgr.m_UIManager.Remove(musicVolumeSlider);
		stateMgr.m_UIManager.Remove(soundVolumeSlider);
		stateMgr.m_UIPopupManager.RemoveAll();
	}

	public void Create()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[9];
		shadowImg = new UIImage();
		shadowImg.AddObject(unitUI, 0, 0);
		shadowImg.Rect = shadowImg.GetObjectRect();
		if ((float)Screen.width < UIConstant.ScreenLocalWidth || (float)Screen.height < UIConstant.ScreenLocalHeight)
		{
			shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		}
		else
		{
			shadowImg.SetSize(new Vector2(Screen.width, Screen.height));
		}
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 2);
		frameBGImg = new UIImage();
		frameBGImg.AddObject(unitUI, 0, 1);
		frameBGImg.Rect = frameBGImg.GetObjectRect();
		frameBGImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		frameImg = new UIImage();
		frameImg.AddObject(unitUI, 0, FRAME_BEGIN_IMG, FRAME_COUNT_IMG);
		frameImg.Rect = frameImg.GetObjectRect();
		stateMgr.m_UIManager.Add(shadowImg);
		stateMgr.m_UIManager.Add(frameImg);
		stateMgr.m_UIManager.Add(frameBGImg);
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 45);
		BGMTitle = new FrUIText();
		BGMTitle.AlignStyle = FrUIText.enAlignStyle.TOP_RIGHT;
		BGMTitle.Rect = modulePositionRect2;
		BGMTitle.Set("font2", "BGM", UIConstant.FONT_COLOR_EQUIP_NAME, modulePositionRect2.width);
		modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 46);
		SFXTitle = new FrUIText();
		SFXTitle.AlignStyle = FrUIText.enAlignStyle.TOP_RIGHT;
		SFXTitle.Rect = modulePositionRect2;
		SFXTitle.Set("font2", "SOUND FX", UIConstant.FONT_COLOR_EQUIP_NAME, modulePositionRect2.width);
		stateMgr.m_UIManager.Add(BGMTitle);
		stateMgr.m_UIManager.Add(SFXTitle);
		musicVolume = userState.GetMusicVolume();
		musicVolumeSlider = new UISliderCustomize();
		UISliderCustomize.UISliderStruct sliderStruct = new UISliderCustomize.UISliderStruct(0, 42, 0, 43, 0, 44);
		musicVolumeSlider.Create(unitUI, sliderStruct);
		Rect modulePositionRect3 = unitUI.GetModulePositionRect(0, 0, 42);
		musicVolumeSlider.SetScroller(0f, 400f, 4f, 46f, modulePositionRect3);
		musicVolumeSlider.SetSelection((int)(musicVolume * 100f));
		stateMgr.m_UIManager.Add(musicVolumeSlider);
		soundVolume = userState.GetSoundVolume();
		soundVolumeSlider = new UISliderCustomize();
		UISliderCustomize.UISliderStruct sliderStruct2 = new UISliderCustomize.UISliderStruct(0, 37, 0, 38, 0, 39);
		soundVolumeSlider.Create(unitUI, sliderStruct2);
		modulePositionRect3 = unitUI.GetModulePositionRect(0, 0, 37);
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
		backBtn = new UIClickButton();
		backBtn.AddObject(UIButtonBase.State.Normal, unitUI, 0, BACK_NORMAL);
		backBtn.AddObject(UIButtonBase.State.Pressed, unitUI, 0, BACK_PRESSED);
		backBtn.Rect = backBtn.GetObjectRect(UIButtonBase.State.Normal);
		decorateImg = new UIImage();
		decorateImg.AddObject(unitUI, 0, DECORATE_ICONS);
		decorateImg.Rect = decorateImg.GetObjectRect();
		stateMgr.m_UIManager.Add(decorateImg);
		stateMgr.m_UIManager.Add(backBtn);
		UnitUI ui = Res2DManager.GetInstance().vUI[22];
		float num = (float)Screen.width / (float)Screen.height - UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight;
		if (num > 0f)
		{
			float num2 = (int)Math.Abs((float)Screen.height * UIConstant.ScreenLocalWidth / UIConstant.ScreenLocalHeight - (float)Screen.width) / 2;
			num2 *= 1.5f;
			UIImage uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(num2, UIConstant.ScreenLocalHeight));
			uIImage.Rect = new Rect((0f - num2) / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
			uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(num2, UIConstant.ScreenLocalHeight));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth + num2 / 2f, UIConstant.ScreenLocalHeight / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
		}
		else if (num < 0f)
		{
			float num3 = (int)Math.Abs((float)Screen.width * UIConstant.ScreenLocalHeight / UIConstant.ScreenLocalWidth - (float)Screen.height) / 2;
			num3 *= 1.5f;
			UIImage uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num3));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, (0f - num3) / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
			uIImage = new UIImage();
			uIImage.AddObject(ui, 0);
			uIImage.SetColor(Color.black);
			uIImage.SetSize(new Vector2(UIConstant.ScreenLocalWidth, num3));
			uIImage.Rect = new Rect(UIConstant.ScreenLocalWidth / 2f, UIConstant.ScreenLocalHeight + num3 / 2f, 0f, 0f);
			stateMgr.m_UIPopupManager.Add(uIImage);
		}
	}

	public void InitSound()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[9];
		sound.SetBackground(ui, 0, 36);
		sound.SetSlider(ui, 0, 35);
		sound.SetClipRect(sound.backgroundImg.Rect.x + 1f, sound.backgroundImg.Rect.y, sound.backgroundImg.Rect.width - 2f, sound.backgroundImg.Rect.height);
		sound.SetScroller(0f, 67f, 67f, sound.backgroundImg.Rect);
		sound.m_scroller.DeltaTime = 0.016f;
		bPlaySound = userState.GetPlaySound();
		sound.SetSelection((!bPlaySound) ? 1 : 0);
	}

	public void InitMusic()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[9];
		music.SetBackground(ui, 0, 41);
		music.SetSlider(ui, 0, 40);
		music.SetClipRect(music.backgroundImg.Rect.x + 1f, music.backgroundImg.Rect.y, music.backgroundImg.Rect.width - 2f, music.backgroundImg.Rect.height);
		music.SetScroller(0f, 67f, 67f, music.backgroundImg.Rect);
		music.m_scroller.DeltaTime = 0.016f;
		bPlayMusic = userState.GetPlayMusic();
		music.SetSelection((!bPlayMusic) ? 1 : 0);
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
		if (control == sound)
		{
			bPlaySound = wparam == 0f;
			userState.SetPlaySound(bPlaySound);
		}
		else if (control == music)
		{
			bPlayMusic = wparam == 0f;
			userState.SetPlayMusic(bPlayMusic);
		}
		else if (control == backBtn)
		{
			stateMgr.FrGoToPhase(14, false, false, false);
		}
		else if (control == musicVolumeSlider)
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
		}
		else if (control == soundVolumeSlider)
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
		}
	}
}
