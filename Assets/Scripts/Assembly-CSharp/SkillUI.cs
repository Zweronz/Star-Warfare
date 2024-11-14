using UnityEngine;

public class SkillUI : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0
	}

	public UIClickButton powerUpBtn;

	public UIImage powerUpEnableImg;

	public bool bPowerUpLight;

	public float powerUpZoom = 1f;

	public Vector2 powerUpSize;

	public int skillId;

	public new Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			powerUpBtn.Rect = value;
			powerUpEnableImg.Rect = value;
		}
	}

	public SkillUI(int skillId, int frameIdx, int iconIdx)
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[0];
		this.skillId = skillId;
		powerUpBtn = new UIClickButton();
		byte[] module = new byte[2]
		{
			39,
			(byte)(iconIdx + 1)
		};
		byte[] module2 = new byte[2]
		{
			40,
			(byte)iconIdx
		};
		byte[] module3 = new byte[2]
		{
			40,
			(byte)iconIdx
		};
		powerUpBtn.AddObject(UIButtonBase.State.Normal, ui, frameIdx, module);
		powerUpBtn.AddObject(UIButtonBase.State.Pressed, ui, frameIdx, module2);
		powerUpBtn.AddObject(UIButtonBase.State.Disabled, ui, frameIdx, module3);
		powerUpBtn.Rect = powerUpBtn.GetObjectRect(UIButtonBase.State.Normal);
		powerUpBtn.SetParent(this);
		powerUpEnableImg = new UIImage();
		powerUpEnableImg.AddObject(ui, frameIdx, iconIdx + 1);
		powerUpEnableImg.Rect = powerUpEnableImg.GetObjectRect();
		powerUpEnableImg.Visible = false;
		powerUpEnableImg.SetParent(this);
		powerUpSize = new Vector2(powerUpEnableImg.Rect.width, powerUpEnableImg.Rect.height);
		SetUIHandler(this);
		base.Show();
	}

	public override void Draw()
	{
		if (powerUpBtn.Visible)
		{
			powerUpBtn.Draw();
		}
		if (powerUpEnableImg.Visible)
		{
			powerUpEnableImg.Draw();
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (powerUpBtn.HandleInput(touch))
		{
			return true;
		}
		return false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		m_Parent.SendEvent(this, 0, control.Id, lparam);
	}
}
