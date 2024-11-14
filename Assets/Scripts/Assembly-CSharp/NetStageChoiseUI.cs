using System.Collections.Generic;
using UnityEngine;

public class NetStageChoiseUI : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0
	}

	private UIImage shadowImg;

	private UIImage stageChoiseImg;

	private UIClickButton confirmBtn;

	private UISliderStage swapStage;

	private UIBlock m_block;

	private List<UIImage> pageNavImg = new List<UIImage>();

	private UIImage selectStagePageImg;

	public byte stageIdx;

	private static byte[] CONFIRM_NORMAL = new byte[2] { 26, 30 };

	private static byte[] CONFIRM_PRESSED = new byte[2] { 24, 29 };

	private Mode gameMode;

	private int stageCount;

	public void Create()
	{
		stageIdx = GameApp.GetInstance().GetUserState().GetNetStage();
		if (stageIdx < Global.TOTAL_SURVIVAL_STAGE && GameApp.GetInstance().GetUserState().GetStageState()[stageIdx, 0] == 0)
		{
			stageIdx = 0;
			GameApp.GetInstance().GetUserState().SetNetStage(stageIdx);
		}
		gameMode = GameApp.GetInstance().GetGameMode().ModePlay;
		UnitUI ui = Res2DManager.GetInstance().vUI[3];
		m_block = new UIBlock();
		m_block.Rect = new Rect(0f, 0f, Screen.width, Screen.height);
		Add(m_block);
		shadowImg = new UIImage();
		shadowImg.AddObject(ui, 0, 27);
		shadowImg.Rect = shadowImg.GetObjectRect();
		shadowImg.SetSize(new Vector2(UIConstant.ScreenLocalWidth, UIConstant.ScreenLocalHeight));
		confirmBtn = new UIClickButton();
		confirmBtn.AddObject(UIButtonBase.State.Normal, ui, 0, CONFIRM_NORMAL);
		confirmBtn.AddObject(UIButtonBase.State.Pressed, ui, 0, CONFIRM_PRESSED);
		confirmBtn.Rect = confirmBtn.GetObjectRect(UIButtonBase.State.Normal);
		swapStage = new UISliderStage();
		swapStage.Create(ui);
		ResetUIStage();
		Add(shadowImg);
		Add(confirmBtn);
		Add(swapStage);
		selectStagePageImg = new UIImage();
		selectStagePageImg.AddObject(ui, 0, 23);
		ResetPageNav();
		SetUIHandler(this);
	}

	public void Init()
	{
		ResetUIStage();
		ResetPageNav();
	}

	public void SetNetStage(Mode mode, byte stageIdx)
	{
		gameMode = mode;
		this.stageIdx = stageIdx;
	}

	public void ResetUIStage()
	{
		swapStage.Clear();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[3];
		stageCount = 0;
		int num = 0;
		if (gameMode == Mode.Boss)
		{
			stageCount = Global.TOTAL_BOSS_STAGE;
			num = Global.TOTAL_BOSS_STAGE;
		}
		else if (gameMode == Mode.Survival)
		{
			stageCount = Global.TOTAL_SURVIVAL_STAGE;
			num = Global.TOTAL_SURVIVAL_STAGE;
		}
		else
		{
			stageCount = Global.TOTAL_VS_STAGE;
			num = Global.TOTAL_VS_STAGE + 2;
		}
		for (int i = 0; i < num; i++)
		{
			UISliderStage.UIStageIcon uIStageIcon = new UISliderStage.UIStageIcon();
			uIStageIcon.m_background = new UIClickButton();
			int num2 = i % stageCount;
			if (gameMode == Mode.Survival)
			{
				uIStageIcon.m_background.AddObject(UIButtonBase.State.Normal, unitUI, 8, num2);
				uIStageIcon.m_background.AddObject(UIButtonBase.State.Pressed, unitUI, 8, num2);
			}
			else if (gameMode == Mode.Boss)
			{
				uIStageIcon.m_background.AddObject(UIButtonBase.State.Normal, unitUI, 9, num2);
				uIStageIcon.m_background.AddObject(UIButtonBase.State.Pressed, unitUI, 9, num2);
			}
			else
			{
				uIStageIcon.m_background.AddObject(UIButtonBase.State.Normal, unitUI, 10, num2);
				uIStageIcon.m_background.AddObject(UIButtonBase.State.Pressed, unitUI, 10, num2);
			}
			uIStageIcon.m_background.Rect = uIStageIcon.m_background.GetObjectRect(UIButtonBase.State.Normal);
			uIStageIcon.m_Lock = new UIImage();
			if (gameMode == Mode.Survival && num2 < Global.TOTAL_STAGE && GameApp.GetInstance().GetUserState().GetStageState()[num2, 0] == 0)
			{
				uIStageIcon.m_Lock.AddObject(unitUI, 0, 28);
				uIStageIcon.m_Lock.Rect = uIStageIcon.m_Lock.GetObjectRect();
				uIStageIcon.m_bLock = true;
			}
			Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 31);
			uIStageIcon.m_level = new UIImage();
			if (gameMode == Mode.Survival)
			{
				uIStageIcon.m_level.AddObject(unitUI, 7, UIConstant.NET_STAGE_LEVEL[num2]);
			}
			else if (gameMode == Mode.Boss)
			{
				uIStageIcon.m_level.AddObject(unitUI, 7, UIConstant.NET_STAGE_LEVEL[num2 + Global.TOTAL_SURVIVAL_STAGE]);
			}
			uIStageIcon.m_level.Rect = modulePositionRect;
			uIStageIcon.m_bLevel = true;
			uIStageIcon.Id = num2;
			uIStageIcon.Rect = uIStageIcon.m_background.Rect;
			uIStageIcon.Add(uIStageIcon.m_background);
			uIStageIcon.Add(uIStageIcon.m_Lock);
			uIStageIcon.Add(uIStageIcon.m_level);
			uIStageIcon.Show();
			swapStage.Add(uIStageIcon);
		}
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 0, 2);
		Rect rect = new Rect(0f, modulePositionRect2.y, UIConstant.ScreenLocalWidth, modulePositionRect2.height);
		swapStage.SetClipRect(rect);
		float num3 = modulePositionRect2.width - 250f;
		swapStage.SetScroller(0f, num3 * (float)num, num3, rect);
		if (gameMode == Mode.Survival)
		{
			swapStage.SetSelection(stageIdx);
		}
		else if (gameMode == Mode.Boss)
		{
			swapStage.SetSelection(stageIdx - Global.TOTAL_SURVIVAL_STAGE);
		}
		else
		{
			swapStage.SetSelection(stageIdx - Global.TOTAL_SURVIVAL_STAGE - Global.TOTAL_BOSS_STAGE);
		}
	}

	public void ResetPageNav()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[3];
		foreach (UIImage item in pageNavImg)
		{
			Remove(item);
		}
		pageNavImg.Clear();
		Remove(selectStagePageImg);
		int num = 0;
		num = ((gameMode == Mode.Boss) ? Global.TOTAL_BOSS_STAGE : ((gameMode != Mode.Survival) ? Global.TOTAL_VS_STAGE : Global.TOTAL_SURVIVAL_STAGE));
		int num2 = 32;
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 20);
		float num3 = modulePositionRect.x + modulePositionRect.width * 0.5f - (float)((num - 1) * num2) * 0.5f;
		for (int i = 0; i < num; i++)
		{
			UIImage uIImage = new UIImage();
			uIImage.AddObject(unitUI, 0, 20);
			uIImage.Rect = new Rect(num3 - modulePositionRect.width * 0.5f, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
			num3 += (float)num2;
			pageNavImg.Add(uIImage);
			Add(uIImage);
		}
		selectStagePageImg.Rect = pageNavImg[ConvertToUIIndex(stageIdx)].Rect;
		Add(selectStagePageImg);
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

	private byte ConvertToUIIndex(byte curId)
	{
		byte b = curId;
		if (gameMode == Mode.Survival)
		{
			return curId;
		}
		if (gameMode == Mode.Boss)
		{
			return (byte)(curId - Global.TOTAL_SURVIVAL_STAGE);
		}
		return (byte)(curId - Global.TOTAL_SURVIVAL_STAGE - Global.TOTAL_BOSS_STAGE);
	}

	private byte ConvertToStageIndex(byte curId)
	{
		byte b = curId;
		if (gameMode == Mode.Survival)
		{
			return curId;
		}
		if (gameMode == Mode.Boss)
		{
			return (byte)(curId + Global.TOTAL_SURVIVAL_STAGE);
		}
		return (byte)(curId + Global.TOTAL_SURVIVAL_STAGE + Global.TOTAL_BOSS_STAGE);
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == confirmBtn)
		{
			if (stageIdx >= Global.TOTAL_SURVIVAL_STAGE || GameApp.GetInstance().GetUserState().GetStageState()[stageIdx, 0] == 1)
			{
				AudioManager.GetInstance().PlaySound(AudioName.CLICK);
				m_Parent.SendEvent(this, 0, (int)stageIdx, lparam);
			}
		}
		else
		{
			if (control != swapStage)
			{
				return;
			}
			switch (command)
			{
			case 0:
				stageIdx = ConvertToStageIndex((byte)wparam);
				if (stageIdx >= Global.TOTAL_SURVIVAL_STAGE || (stageIdx < Global.TOTAL_SURVIVAL_STAGE && GameApp.GetInstance().GetUserState().GetStageState()[stageIdx, 0] == 1))
				{
					AudioManager.GetInstance().PlaySound(AudioName.CLICK);
					m_Parent.SendEvent(this, 0, (int)stageIdx, lparam);
				}
				break;
			case 1:
				AudioManager.GetInstance().PlaySound(AudioName.SWITCH_ITEM);
				stageIdx = ConvertToStageIndex((byte)(wparam % (float)stageCount));
				selectStagePageImg.Rect = pageNavImg[(byte)(wparam % (float)stageCount)].Rect;
				break;
			}
		}
	}
}
