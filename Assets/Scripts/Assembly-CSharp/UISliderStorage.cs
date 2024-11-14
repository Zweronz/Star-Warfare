using System.Collections.Generic;
using UnityEngine;

public class UISliderStorage : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0,
		SelectIndex = 1,
		DragExchange = 2,
		DragOutSide = 3,
		DragMove = 4,
		DragEnd = 5,
		DragBegin = 6
	}

	public class UIDragPanel : UIPanelX
	{
		public UIDragGrid m_dragGrid;

		public float m_size;

		public override void Draw()
		{
			m_dragGrid.Draw();
		}

		public new void Clear()
		{
			m_dragGrid.Clear();
		}

		public override void Destory()
		{
			if (m_dragGrid != null)
			{
				m_dragGrid.Destory();
			}
		}

		public override bool HandleInput(UITouchInner touch)
		{
			if (m_dragGrid.HandleInput(touch))
			{
				return true;
			}
			return false;
		}
	}

	public const byte BG_UI_STORAGE = 0;

	private const byte TOUCH_SCROLLER = 1;

	private const byte TOUCH_DRAG_ICON = 2;

	protected int m_FingerId;

	protected Vector2 m_TouchPosition;

	public static byte[,] BG_IMG = new byte[1, 5] { { 0, 6, 4, 28, 2 } };

	public UIScroller m_scroller = new UIScroller();

	public UIImage m_leftArrow;

	public UIImage m_rightArrow;

	public Rect m_showRect;

	protected int m_IconWidth;

	protected int m_IconHeight;

	protected int m_IconShadowWidth;

	protected int m_IconShadowHeight;

	protected int m_panelWidth;

	protected int m_panelHeight;

	protected int m_rowSpacing;

	protected int m_colSpacing;

	public List<UIDragPanel> m_PropsPanels = new List<UIDragPanel>();

	public List<UIDragPanel> m_drawPropsPanels = new List<UIDragPanel>();

	private byte touchAction;

	public int m_selectPanel;

	public void Create(UnitUI ui, int BGIndex)
	{
		Vector2 moduleSize = ui.GetModuleSize(0, BG_IMG[BGIndex, 0], BG_IMG[BGIndex, 1]);
		m_IconWidth = (int)moduleSize.x;
		m_IconHeight = (int)moduleSize.y;
		Vector2 moduleSize2 = ui.GetModuleSize(0, BG_IMG[BGIndex, 0], BG_IMG[BGIndex, 2]);
		m_IconShadowWidth = (int)moduleSize2.x;
		m_IconShadowHeight = (int)moduleSize2.y;
		m_rowSpacing = 10;
		m_colSpacing = 10;
		m_panelWidth = m_IconWidth * 3 + m_rowSpacing * 4;
		m_panelHeight = m_IconHeight * 3 + m_colSpacing * 4;
		Rect modulePositionRect = ui.GetModulePositionRect(0, BG_IMG[BGIndex, 0], BG_IMG[BGIndex, 4]);
		SetClipRect(modulePositionRect.x, modulePositionRect.y, modulePositionRect.width, modulePositionRect.height);
		float num = 250f;
		SetScroller(0f, (float)(int)Global.STORAGE_MAX_PANEL * num, num, modulePositionRect);
		SetUIHandler(this);
		base.Rect = modulePositionRect;
		base.Show();
	}

	public void SetClipRect(float x, float y, float width, float height)
	{
		m_showRect = new Rect(x, y, width, height);
	}

	public void SetScroller(float min, float max, float spacing, Rect rct)
	{
		m_scroller.Loop = true;
		m_scroller.SetScroller(UIScroller.ScrollerDir.Horizontal, min, max, spacing);
		m_scroller.Rect = rct;
		m_scroller.SetParent(this);
	}

	public void SetSelection(int index)
	{
		m_scroller.ScrollerPos = (float)index * m_scroller.Spacing;
		SetEnable(index, true);
		m_selectPanel = index;
	}

	public virtual void Add(UIDragPanel grid)
	{
		grid.SetParent(this);
		m_PropsPanels.Add(grid);
	}

	public override void Draw()
	{
		base.Draw();
		foreach (UIDragPanel drawPropsPanel in m_drawPropsPanels)
		{
			drawPropsPanel.Draw();
		}
	}

	public override void Destory()
	{
		base.Destory();
		if (m_PropsPanels != null)
		{
			foreach (UIDragPanel propsPanel in m_PropsPanels)
			{
				if (propsPanel != null)
				{
					propsPanel.Destory();
				}
			}
			m_PropsPanels.Clear();
		}
		if (m_drawPropsPanels == null)
		{
			return;
		}
		foreach (UIDragPanel drawPropsPanel in m_drawPropsPanels)
		{
			if (drawPropsPanel != null)
			{
				drawPropsPanel.Destory();
			}
		}
		m_drawPropsPanels.Clear();
	}

	public override void Update()
	{
		m_scroller.Update();
		m_drawPropsPanels.Clear();
		float num = 0f;
		int num2 = (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
		float num3 = m_showRect.x + m_showRect.width * 0.5f;
		float num4 = m_showRect.y + m_showRect.height * 0.5f;
		int count = m_PropsPanels.Count;
		float num5 = 0f;
		for (int i = 0; i < count; i++)
		{
			UIDragPanel uIDragPanel = m_PropsPanels[i];
			num = num3 + m_scroller.Spacing * (float)i - m_scroller.ScrollerPos - (float)m_panelWidth * 0.5f;
			if (num + (float)m_panelWidth < m_showRect.x)
			{
				num = m_showRect.x + (float)count * m_scroller.Spacing - (m_showRect.x - num);
			}
			else if (num + (float)m_panelWidth > m_showRect.x + (float)count * m_scroller.Spacing)
			{
				float num6 = num + (float)m_panelWidth - (m_showRect.x + (float)count * m_scroller.Spacing);
				num = m_showRect.x - (float)m_panelWidth + num6;
			}
			float num7 = num + (float)m_panelWidth * 0.5f;
			float num8 = Mathf.Abs(num3 - num7);
			float num9 = 1f - num8 / m_showRect.width;
			if (num9 < 0.2f)
			{
				num9 = 0.2f;
			}
			if (num9 > num5)
			{
				num5 = num9;
			}
			float num10 = (float)m_panelWidth * num9;
			float num11 = (float)m_panelHeight * num9;
			Vector2 leftTop = new Vector2(num7 - num10 * 0.5f, num4 - num11 * 0.5f);
			UpdatePropsPanel(uIDragPanel, leftTop, num9);
			uIDragPanel.m_size = num9;
			Sort(uIDragPanel);
		}
	}

	private void UpdatePropsPanel(UIDragPanel dragPanel, Vector2 leftTop, float scaleFactor)
	{
		List<UIDragGrid.UIDragIcon> elements = dragPanel.m_dragGrid.GetElements();
		for (int i = 0; i < elements.Count; i++)
		{
			UIDragGrid.UIDragIcon uIDragIcon = elements[i];
			UIImage background = uIDragIcon.m_Background;
			UIImage image = uIDragIcon.m_Image;
			UIImage shadow = uIDragIcon.m_Shadow;
			UINumeric num = uIDragIcon.m_num;
			UIMove uIMove = uIDragIcon.m_UIMove;
			int num2 = i % 3;
			int num3 = 2 - i / 3;
			int num4 = (int)((float)m_IconWidth * scaleFactor);
			int num5 = (int)((float)m_IconHeight * scaleFactor);
			int num6 = (int)((float)m_IconShadowWidth * scaleFactor);
			int num7 = (int)((float)m_IconShadowHeight * scaleFactor);
			int num8 = (int)(22f * scaleFactor);
			int num9 = (int)(22f * scaleFactor);
			int num10 = (int)(leftTop.x + (float)(num2 * num4) + (float)((num2 + 1) * m_rowSpacing) * scaleFactor);
			int num11 = (int)(leftTop.y + (float)(num3 * num5) + (float)((num3 + 1) * m_colSpacing) * scaleFactor);
			background.Rect = new Rect(num10, num11, num4, num5);
			uIMove.Rect = new Rect(num10, num11, num4, num5);
			shadow.Rect = new Rect(num10, num11, num4, num5);
			num.Rect = new Rect((float)num10 - 17f * scaleFactor, (float)num11 - 30f * scaleFactor, num4, num5);
			image.Rect = new Rect(num10, num11, num4, num5);
			image.SetSize(new Vector2(uIDragIcon.m_rect.width * scaleFactor, uIDragIcon.m_rect.height * scaleFactor));
			if (dragPanel.m_dragGrid.m_Selected.Id == i)
			{
				dragPanel.m_dragGrid.m_Selected.Rect = new Rect(num10, num11, num4, num5);
				dragPanel.m_dragGrid.m_Selected.SetSize(new Vector2(num4, num5));
				dragPanel.m_dragGrid.m_Selected.SetClip(m_showRect);
			}
			background.SetSize(new Vector2(num4, num5));
			shadow.SetSize(new Vector2(num6, num7));
			num.SetSize(new Vector2(num8, num9));
			background.SetClip(m_showRect);
			shadow.SetClip(m_showRect);
			image.SetClip(m_showRect);
			num.SetClip(m_showRect);
		}
	}

	private void SetEnable(int index, bool enable)
	{
		UIDragPanel uIDragPanel = m_PropsPanels[index];
		uIDragPanel.Enable = enable;
	}

	public void SetAllEnable(bool enable)
	{
		foreach (UIDragPanel propsPanel in m_PropsPanels)
		{
			propsPanel.Enable = enable;
		}
	}

	public new void Clear()
	{
		foreach (UIDragPanel propsPanel in m_PropsPanels)
		{
			propsPanel.Clear();
		}
		m_drawPropsPanels.Clear();
		m_PropsPanels.Clear();
		base.Clear();
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			if (PtInRect(touch.position))
			{
				m_FingerId = touch.fingerId;
				m_TouchPosition = touch.position;
			}
		}
		else
		{
			if (touch.fingerId != m_FingerId)
			{
				return false;
			}
			if (touch.phase == TouchPhase.Moved)
			{
				if (touchAction == 0)
				{
					Vector2 vector = m_TouchPosition - touch.position;
					if (vector.sqrMagnitude > 100f)
					{
						if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
						{
							touchAction = 1;
						}
						else
						{
							touchAction = 2;
						}
					}
				}
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				touchAction = 0;
				m_FingerId = -1;
				m_TouchPosition = new Vector2(0f, 0f);
			}
		}
		switch (touchAction)
		{
		case 0:
			m_scroller.HandleInput(touch);
			foreach (UIDragPanel propsPanel in m_PropsPanels)
			{
				if (propsPanel.Enable && propsPanel.HandleInput(touch))
				{
					return true;
				}
			}
			break;
		case 1:
			m_scroller.HandleInput(touch);
			foreach (UIDragPanel propsPanel2 in m_PropsPanels)
			{
				List<UIDragGrid.UIDragIcon> elements = propsPanel2.m_dragGrid.GetElements();
				for (int i = 0; i < elements.Count; i++)
				{
					UIDragGrid.UIDragIcon uIDragIcon = elements[i];
					uIDragIcon.m_draging = false;
				}
			}
			break;
		case 2:
			foreach (UIDragPanel propsPanel3 in m_PropsPanels)
			{
				if (propsPanel3.Enable && propsPanel3.HandleInput(touch))
				{
					return true;
				}
			}
			break;
		}
		if (base.HandleInput(touch))
		{
			return true;
		}
		return false;
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == m_scroller)
		{
			if (command == 3)
			{
				wparam %= (float)m_PropsPanels.Count;
				m_Parent.SendEvent(this, 1, wparam, lparam);
				SetAllEnable(false);
				SetEnable((int)wparam, true);
				m_selectPanel = (int)wparam;
			}
			return;
		}
		switch (command)
		{
		case 3:
			m_Parent.SendEvent(this, 2, wparam + (float)(m_selectPanel * 9), lparam + (float)(m_selectPanel * 9));
			break;
		case 4:
			m_Parent.SendEvent(this, 3, wparam + (float)(m_selectPanel * 9), lparam + (float)(m_selectPanel * 9));
			break;
		case 1:
			m_Parent.SendEvent(this, 4, wparam + (float)(m_selectPanel * 9), lparam);
			break;
		case 2:
			m_Parent.SendEvent(this, 5, wparam, lparam);
			break;
		case 0:
			m_Parent.SendEvent(this, 6, wparam + (float)(m_selectPanel * 9), lparam);
			break;
		}
	}

	public void ClearSelected()
	{
		foreach (UIDragPanel propsPanel in m_PropsPanels)
		{
			propsPanel.m_dragGrid.m_Selected.Visible = false;
			propsPanel.m_dragGrid.m_Selected.Id = -1;
		}
	}

	public void SetSelected(int id)
	{
		ClearSelected();
		m_PropsPanels[id / 9].m_dragGrid.m_Selected.Visible = true;
		m_PropsPanels[id / 9].m_dragGrid.m_Selected.Id = id % 9;
	}

	public bool Sort(UIDragPanel icon)
	{
		if (m_drawPropsPanels.Count == 0)
		{
			m_drawPropsPanels.Add(icon);
		}
		else
		{
			float size = icon.m_size;
			int num = 0;
			int num2 = m_drawPropsPanels.Count - 1;
			int num3 = (num + num2) / 2;
			if (size <= m_drawPropsPanels[num].m_size)
			{
				m_drawPropsPanels.Insert(num, icon);
			}
			else if (size >= m_drawPropsPanels[num2].m_size)
			{
				m_drawPropsPanels.Insert(num2 + 1, icon);
			}
			else
			{
				while (num2 - num > 1)
				{
					float size2 = m_drawPropsPanels[num3].m_size;
					if (size == size2)
					{
						num = num3;
						break;
					}
					if (size > size2)
					{
						num = num3;
					}
					else
					{
						num2 = num3;
					}
					num3 = (num + num2) / 2;
				}
				m_drawPropsPanels.Insert(num + 1, icon);
			}
		}
		return true;
	}
}
