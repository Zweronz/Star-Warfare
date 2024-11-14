using System.Collections.Generic;
using UnityEngine;

public class UIComboBox : UIPanelX, UIHandler
{
	public enum Dir
	{
		UP = 0,
		DOWN = 1
	}

	public enum Command
	{
		Hide = 0,
		Show = 1,
		Click = 2,
		SelectIndex = 3,
		Moved = 4
	}

	public class UIItemIcon : UIPanelX
	{
		public UIImage m_background;

		public FrUIText m_text;

		public Vector2 m_textOffsetLeftTop = Vector2.zero;

		public Vector2 m_textOffsetWH = Vector2.zero;

		public UIImage m_icon;

		public Vector2 m_iconOffsetLeftTop = Vector2.zero;

		public Vector2 m_iconOffsetWH = Vector2.zero;

		protected int m_TouchFingerId = -1;

		public bool m_active;

		private UIComboBox m_combo;

		public new Rect Rect
		{
			get
			{
				return base.Rect;
			}
			set
			{
				base.Rect = value;
				m_background.Rect = value;
				m_text.Rect = new Rect(value.x + m_textOffsetLeftTop.x, value.y + m_textOffsetLeftTop.y, value.width + m_textOffsetWH.x, value.height + m_textOffsetWH.y);
				m_icon.Rect = new Rect(value.x + m_iconOffsetLeftTop.x, value.y + m_iconOffsetLeftTop.y, value.width + m_iconOffsetWH.x, value.height + m_iconOffsetWH.y);
			}
		}

		public UIItemIcon(UIComboBox combo)
		{
			m_combo = combo;
			m_background = new UIImage();
			m_text = new FrUIText();
			m_icon = new UIImage();
		}

		public override void Draw()
		{
			m_background.Draw();
			m_text.Draw();
			m_icon.Draw();
		}

		public override void Destory()
		{
			base.Destory();
			if (m_background != null)
			{
				m_background.Destory();
			}
			if (m_text != null)
			{
				m_text.Destory();
			}
			if (m_icon != null)
			{
				m_icon.Destory();
			}
		}

		public void SetParentEx(UIContainer parent)
		{
			m_background.SetParent(parent);
			m_text.SetParent(parent);
			m_icon.SetParent(parent);
			SetParent(parent);
		}

		public new void SetClip(Rect clip_rect)
		{
			m_background.SetClip(clip_rect);
			m_text.SetClip(clip_rect);
			m_icon.SetClip(clip_rect);
		}

		public override bool HandleInput(UITouchInner touch)
		{
			if (touch.phase == TouchPhase.Began)
			{
				if (PtInRect(touch.position))
				{
					m_TouchFingerId = touch.fingerId;
					return true;
				}
				return false;
			}
			if (touch.phase == TouchPhase.Ended)
			{
				if ((touch.fingerId == m_TouchFingerId || m_combo.m_state == 0) && PtInRect(touch.position))
				{
					m_Parent.SendEvent(this, 2, 0f, 0f);
					m_TouchFingerId = -1;
					return true;
				}
			}
			else if (touch.phase == TouchPhase.Moved && (touch.fingerId == m_TouchFingerId || m_combo.m_state == 0) && PtInRect(touch.position))
			{
				m_Parent.SendEvent(this, 4, 0f, 0f);
				return true;
			}
			return false;
		}
	}

	public const byte STATE_WORKABLE = 0;

	public const byte STATE_STRETCHING = 1;

	public const byte STATE_RETRACTING = 2;

	public const byte STATE_UNWORKABLE = 3;

	protected Dir m_Dir = Dir.DOWN;

	public List<UIItemIcon> m_itemList = new List<UIItemIcon>();

	public byte m_state;

	protected UIItemIcon m_currentItem;

	protected UIItemIcon m_disableItem;

	private byte m_selectIndex;

	protected int m_IconWidth;

	protected int m_IconHeight;

	public Rect m_showRect;

	private byte[] m_resource;

	public void Create(UnitUI ui, byte[] res)
	{
		m_resource = res;
		Rect modulePositionRect = ui.GetModulePositionRect(0, m_resource[1], m_resource[3]);
		m_IconWidth = (int)modulePositionRect.width;
		m_IconHeight = (int)modulePositionRect.height;
		m_currentItem = new UIItemIcon(this);
		m_currentItem.m_background = new UIImage();
		m_currentItem.m_background.AddObject(ui, m_resource[1], m_resource[2]);
		m_currentItem.m_background.Rect = m_currentItem.m_background.GetObjectRect();
		m_currentItem.m_icon = new UIImage();
		m_currentItem.m_icon.Rect = m_currentItem.m_background.Rect;
		m_currentItem.m_text = new FrUIText();
		m_currentItem.m_text.Rect = m_currentItem.m_background.Rect;
		m_currentItem.Rect = m_currentItem.m_background.Rect;
		m_currentItem.SetParentEx(this);
		m_currentItem.Enable = true;
		m_disableItem = new UIItemIcon(this);
		m_disableItem.m_background = new UIImage();
		m_disableItem.m_background.AddObject(ui, m_resource[1], m_resource[6]);
		m_disableItem.m_background.Rect = m_currentItem.m_background.GetObjectRect();
		m_disableItem.m_icon = new UIImage();
		m_disableItem.m_icon.Rect = m_disableItem.m_background.Rect;
		m_disableItem.m_text = new FrUIText();
		m_disableItem.m_text.Rect = m_disableItem.m_background.Rect;
		m_disableItem.Rect = m_disableItem.m_background.Rect;
		m_disableItem.m_text.SetColor(UIConstant.COLOR_DISABLE_COMBOBOX);
		m_disableItem.SetParentEx(this);
		m_disableItem.Enable = false;
		SetUIHandler(this);
		Show();
		SetState(3);
	}

	public void SetClipRect(float x, float y, float width, float height)
	{
		m_showRect = new Rect(x, y, width, height);
	}

	public new void Clear()
	{
		m_itemList.Clear();
	}

	public void SetState(byte state)
	{
		m_state = state;
	}

	public void SetSelection(int index)
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[m_resource[0]];
		UnitUI ui = Res2DManager.GetInstance().vUI[m_resource[5]];
		m_selectIndex = (byte)index;
		m_currentItem.m_textOffsetLeftTop = m_itemList[index].m_textOffsetLeftTop;
		m_currentItem.m_textOffsetWH = m_itemList[index].m_textOffsetWH;
		m_currentItem.m_text.AlignStyle = m_itemList[index].m_text.AlignStyle;
		m_currentItem.m_text.Set("font2", m_itemList[index].m_text.GetText(), UIConstant.fontColor_cyan, m_IconWidth);
		UISpriteSet spriteSet = m_itemList[index].m_icon.GetSpriteSet();
		if (spriteSet.m_Sprite.Count > 0)
		{
			UISpriteX uISpriteX = (UISpriteX)spriteSet.m_Sprite[0];
			Rect rect = m_currentItem.m_icon.Rect;
			m_currentItem.m_icon.SetTexture(ui, uISpriteX.FrameIdx, uISpriteX.ModuleIdx);
			Vector2 size = m_itemList[index].m_icon.GetSize();
			m_currentItem.m_icon.SetSize(size);
			m_currentItem.m_iconOffsetLeftTop = m_itemList[index].m_iconOffsetLeftTop;
			m_currentItem.m_iconOffsetWH = m_itemList[index].m_iconOffsetWH;
			m_currentItem.m_icon.Rect = rect;
		}
		m_currentItem.Rect = m_currentItem.m_background.Rect;
		ClearActiveItem();
		SetActiveItem(index);
	}

	public byte GetSelectIndex()
	{
		return m_selectIndex;
	}

	public void SetComboBox(Dir dir)
	{
		m_Dir = dir;
	}

	public virtual void Add(UIItemIcon control)
	{
		control.SetParentEx(this);
		m_itemList.Add(control);
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Hide()
	{
		base.Hide();
	}

	public override void Draw()
	{
		if (!base.Visible)
		{
			return;
		}
		foreach (UIItemIcon item in m_itemList)
		{
			if (item.Visible)
			{
				item.Draw();
			}
		}
		m_currentItem.Draw();
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_itemList != null)
		{
			foreach (UIItemIcon item in m_itemList)
			{
				if (item != null)
				{
					item.Destory();
				}
			}
			m_itemList.Clear();
		}
		if (m_currentItem != null)
		{
			m_currentItem.Destory();
		}
		if (m_disableItem != null)
		{
			m_disableItem.Destory();
		}
	}

	public override void Update()
	{
		switch (m_state)
		{
		case 0:
		{
			foreach (UIItemIcon item in m_itemList)
			{
				item.Visible = true;
				item.Enable = true;
				item.SetClip(m_showRect);
			}
			break;
		}
		case 3:
		{
			for (int k = 0; k < m_itemList.Count; k++)
			{
				if (m_Dir == Dir.UP)
				{
					m_itemList[k].Rect = new Rect(m_currentItem.Rect.x, m_currentItem.Rect.y + m_currentItem.Rect.height - (float)((m_itemList.Count - k) * m_IconHeight), m_IconWidth, m_IconHeight);
				}
				else
				{
					m_itemList[k].Rect = new Rect(m_currentItem.Rect.x, m_currentItem.Rect.y + (float)((m_itemList.Count - k - 1) * m_IconHeight), m_IconWidth, m_IconHeight);
				}
				m_itemList[k].SetClip(m_showRect);
				m_itemList[k].Visible = false;
				m_itemList[k].Enable = false;
			}
			break;
		}
		case 2:
		{
			bool flag2 = true;
			float num4 = m_currentItem.Rect.y + (float)m_IconHeight * 0.5f;
			for (int j = 0; j < m_itemList.Count; j++)
			{
				m_itemList[j].Enable = false;
				if (m_Dir == Dir.UP)
				{
					float num5 = num4 - (float)((m_itemList.Count - j - 1) * m_IconHeight);
					if (m_itemList[j].Rect.y + (float)m_IconHeight * 0.5f > num5)
					{
						m_itemList[j].Rect = new Rect(m_itemList[j].Rect.x, m_itemList[j].Rect.y - 40f, m_itemList[j].Rect.width, m_itemList[j].Rect.height);
						flag2 = false;
					}
					if (m_itemList[j].Rect.y + (float)m_IconHeight * 0.5f <= num5)
					{
						m_itemList[j].Rect = new Rect(m_itemList[j].Rect.x, num5 - (float)m_IconHeight * 0.5f, m_itemList[j].Rect.width, m_itemList[j].Rect.height);
					}
				}
				else
				{
					float num6 = num4 + (float)((m_itemList.Count - j - 1) * m_IconHeight);
					if (m_itemList[j].Rect.y + (float)m_IconHeight * 0.5f < num6)
					{
						m_itemList[j].Rect = new Rect(m_itemList[j].Rect.x, m_itemList[j].Rect.y + 40f, m_itemList[j].Rect.width, m_itemList[j].Rect.height);
						flag2 = false;
					}
					if (m_itemList[j].Rect.y + (float)m_IconHeight * 0.5f >= num6)
					{
						m_itemList[j].Rect = new Rect(m_itemList[j].Rect.x, num6 - (float)m_IconHeight * 0.5f, m_itemList[j].Rect.width, m_itemList[j].Rect.height);
					}
				}
			}
			if (flag2)
			{
				SetState(3);
			}
			break;
		}
		case 1:
		{
			bool flag = true;
			float num = m_currentItem.Rect.y + (float)m_IconHeight * 0.5f;
			for (int i = 0; i < m_itemList.Count; i++)
			{
				m_itemList[i].Visible = true;
				if (m_Dir == Dir.UP)
				{
					float num2 = num + (float)((i + 1) * m_IconHeight);
					if (m_itemList[i].Rect.y + (float)m_IconHeight * 0.5f < num2)
					{
						m_itemList[i].Rect = new Rect(m_itemList[i].Rect.x, m_itemList[i].Rect.y + 40f, m_itemList[i].Rect.width, m_itemList[i].Rect.height);
						flag = false;
					}
					if (m_itemList[i].Rect.y + (float)m_IconHeight * 0.5f >= num2)
					{
						m_itemList[i].Rect = new Rect(m_itemList[i].Rect.x, num2 - (float)m_IconHeight * 0.5f, m_itemList[i].Rect.width, m_itemList[i].Rect.height);
					}
				}
				else
				{
					float num3 = num - (float)((i + 1) * m_IconHeight);
					if (m_itemList[i].Rect.y + (float)m_IconHeight * 0.5f > num3)
					{
						m_itemList[i].Rect = new Rect(m_itemList[i].Rect.x, m_itemList[i].Rect.y - 40f, m_itemList[i].Rect.width, m_itemList[i].Rect.height);
						flag = false;
					}
					if (m_itemList[i].Rect.y + (float)m_IconHeight * 0.5f <= num3)
					{
						m_itemList[i].Rect = new Rect(m_itemList[i].Rect.x, num3 - (float)m_IconHeight * 0.5f, m_itemList[i].Rect.width, m_itemList[i].Rect.height);
					}
				}
			}
			if (flag)
			{
				SetState(0);
			}
			break;
		}
		}
	}

	public void SetActiveItem(int index)
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[m_resource[0]];
		Rect rect = m_itemList[index].Rect;
		m_itemList[index].m_background.SetTexture(ui, m_resource[1], m_resource[4]);
		m_itemList[index].m_active = true;
		m_itemList[index].Rect = rect;
		m_itemList[index].SetClip(m_showRect);
	}

	public void ClearActiveItem()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[m_resource[0]];
		for (int i = 0; i < m_itemList.Count; i++)
		{
			if (m_itemList[i].m_active)
			{
				Rect rect = m_itemList[i].Rect;
				m_itemList[i].m_background.SetTexture(ui, m_resource[1], m_resource[3]);
				m_itemList[i].m_active = false;
				m_itemList[i].Rect = rect;
				m_itemList[i].SetClip(m_showRect);
			}
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (m_state == 0)
		{
			if (m_currentItem.HandleInput(touch))
			{
				return true;
			}
			foreach (UIItemIcon item in m_itemList)
			{
				if (item.HandleInput(touch))
				{
					return true;
				}
			}
		}
		else if (m_state == 3 && m_currentItem.HandleInput(touch))
		{
			return true;
		}
		return false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == m_currentItem)
		{
			if (m_state == 3)
			{
				if (command == 2)
				{
					SetState(1);
					m_Parent.SendEvent(this, 1, 0f, 0f);
				}
			}
			else if (m_state == 0)
			{
				SetState(2);
				m_Parent.SendEvent(this, 0, 0f, 0f);
			}
			return;
		}
		for (int i = 0; i < m_itemList.Count; i++)
		{
			if (m_itemList[i] != control)
			{
				continue;
			}
			switch (command)
			{
			case 2:
				SetSelection(i);
				m_Parent.SendEvent(this, 3, i, 0f);
				SetState(2);
				break;
			case 4:
				if (!m_itemList[i].m_active)
				{
					ClearActiveItem();
					SetActiveItem(i);
				}
				break;
			}
			break;
		}
	}
}
