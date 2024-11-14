using System.Collections;

public class UIPanelX : UIControl, UIContainer
{
	protected ArrayList m_Controls = new ArrayList();

	private UIHandler m_UIHandler;

	public UIPanelX()
	{
		base.Visible = false;
		base.Enable = false;
	}

	public override void Draw()
	{
		base.Draw();
		if (!base.Visible)
		{
			return;
		}
		foreach (UIControl control in m_Controls)
		{
			if (control.Visible)
			{
				control.Draw();
			}
		}
	}

	public void Clear()
	{
		m_Controls.Clear();
	}

	public void Remove(UIControl control)
	{
		m_Controls.Remove(control);
	}

	public virtual void UpdateLogic()
	{
	}

	public override void Update()
	{
		if (!base.Visible)
		{
			return;
		}
		foreach (UIControl control in m_Controls)
		{
			if (control.Visible)
			{
				control.Update();
			}
		}
	}

	public override void Destory()
	{
		foreach (UIControl control in m_Controls)
		{
			if (control != null)
			{
				control.Destory();
			}
		}
		m_Controls.Clear();
	}

	public virtual void Hide()
	{
		base.Visible = false;
		base.Enable = false;
	}

	public virtual void Show()
	{
		base.Visible = true;
		base.Enable = true;
	}

	public void SetUIHandler(UIHandler ui_handler)
	{
		m_UIHandler = ui_handler;
	}

	public void DrawSprite(UISpriteX sprite)
	{
		m_Parent.DrawSprite(sprite);
	}

	public override bool HandleInput(UITouchInner touch)
	{
		for (int num = m_Controls.Count - 1; num >= 0; num--)
		{
			UIControl uIControl = (UIControl)m_Controls[num];
			if (uIControl.Enable && uIControl.HandleInput(touch))
			{
				return true;
			}
		}
		return false;
	}

	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (m_UIHandler != null)
		{
			m_UIHandler.HandleEvent(control, command, wparam, lparam);
		}
		else
		{
			m_Parent.SendEvent(this, command, wparam, lparam);
		}
	}

	public virtual void Add(UIControl control)
	{
		m_Controls.Add(control);
		control.SetParent(this);
	}
}
