using System.Collections.Generic;
using UnityEngine;

public class UIDragGrid : UIPanelX, UIHandler
{
	public enum Command
	{
		DragBegin = 0,
		DragMove = 1,
		DragEnd = 2,
		DragExchange = 3,
		DragOutSide = 4
	}

	public class UIDragIcon
	{
		public UIMove m_UIMove;

		public UIImage m_Image;

		public UIImage m_Shadow;

		public UIImage m_Background;

		public bool m_draging;

		public Rect m_rect;

		public UINumeric m_num;

		public void Destory()
		{
			if (m_Image != null)
			{
				m_Image.Destory();
			}
			if (m_Shadow != null)
			{
				m_Shadow.Destory();
			}
			if (m_Background != null)
			{
				m_Background.Destory();
			}
			if (m_num != null)
			{
				m_num.Destory();
			}
		}
	}

	protected List<UIDragIcon> m_dragIcons = new List<UIDragIcon>();

	protected int m_GridCount;

	protected Rect m_IconRect;

	public UIImage m_Selected;

	public UIDragGrid()
	{
		SetUIHandler(this);
	}

	public List<UIDragIcon> GetElements()
	{
		return m_dragIcons;
	}

	public UIDragIcon AddGrid(UnitUI ui, int frame, int module)
	{
		UIDragIcon uIDragIcon = new UIDragIcon();
		uIDragIcon.m_draging = false;
		uIDragIcon.m_Background = new UIImage();
		uIDragIcon.m_Background.AddObject(ui, frame, module);
		uIDragIcon.m_Background.Rect = uIDragIcon.m_Background.GetObjectRect();
		uIDragIcon.m_Background.SetParent(this);
		uIDragIcon.m_Image = new UIImage();
		uIDragIcon.m_Image.Rect = uIDragIcon.m_Background.Rect;
		uIDragIcon.m_Image.SetParent(this);
		uIDragIcon.m_UIMove = new UIMove();
		uIDragIcon.m_UIMove.Rect = uIDragIcon.m_Background.Rect;
		uIDragIcon.m_UIMove.Enable = true;
		Add(uIDragIcon.m_UIMove);
		m_IconRect = uIDragIcon.m_Background.Rect;
		m_dragIcons.Add(uIDragIcon);
		return uIDragIcon;
	}

	public UIDragIcon AddGrid(UnitUI ui, int frame, byte[] module)
	{
		UIDragIcon uIDragIcon = new UIDragIcon();
		uIDragIcon.m_draging = false;
		uIDragIcon.m_Background = new UIImage();
		uIDragIcon.m_Background.AddObject(ui, frame, module);
		uIDragIcon.m_Background.Rect = uIDragIcon.m_Background.GetObjectRect();
		uIDragIcon.m_Background.SetParent(this);
		uIDragIcon.m_Image = new UIImage();
		uIDragIcon.m_Image.Rect = uIDragIcon.m_Background.Rect;
		uIDragIcon.m_Image.SetParent(this);
		uIDragIcon.m_UIMove = new UIMove();
		uIDragIcon.m_UIMove.Rect = uIDragIcon.m_Background.Rect;
		uIDragIcon.m_UIMove.Enable = true;
		Add(uIDragIcon.m_UIMove);
		m_IconRect = uIDragIcon.m_Background.Rect;
		m_dragIcons.Add(uIDragIcon);
		return uIDragIcon;
	}

	public void AddShadow(int gridID, UnitUI ui, int frame, int module)
	{
		m_dragIcons[gridID].m_Shadow = new UIImage();
		m_dragIcons[gridID].m_Shadow.AddObject(ui, frame, module);
		m_dragIcons[gridID].m_Shadow.Rect = m_dragIcons[gridID].m_Background.Rect;
		m_dragIcons[gridID].m_Shadow.SetParent(this);
	}

	public void AddNum(int gridID, UnitUI ui, int frame, int num)
	{
		m_dragIcons[gridID].m_num = new UINumeric();
		m_dragIcons[gridID].m_num.SpacingOffsetX = -6f;
		m_dragIcons[gridID].m_num.AlignStyle = UINumeric.enAlignStyle.right;
		m_dragIcons[gridID].m_num.SetNumeric(ui, 1, string.Format("{0:N0}", num));
		m_dragIcons[gridID].m_num.SetParent(this);
		m_dragIcons[gridID].m_num.Rect = m_dragIcons[gridID].m_Background.Rect;
	}

	public void SetSelected(UnitUI ui, int frame, int module)
	{
		m_Selected = new UIImage();
		m_Selected.AddObject(ui, frame, module);
		m_Selected.SetParent(this);
		m_Selected.Visible = false;
		m_Selected.Id = -1;
	}

	public void SetGridTexturePosition(int fromID, int toID)
	{
		m_dragIcons[fromID].m_Image.Rect = m_dragIcons[toID].m_UIMove.Rect;
	}

	public void HideGridTexture(int gridID)
	{
		m_dragIcons[gridID].m_Image.Visible = false;
	}

	public void SetGridTexture(int gridID, UnitUI ui, int frame, int module)
	{
		m_dragIcons[gridID].m_Image.Visible = true;
		m_dragIcons[gridID].m_Image.SetTexture(ui, frame, module);
		m_dragIcons[gridID].m_Image.Rect = m_dragIcons[gridID].m_Background.Rect;
		m_dragIcons[gridID].m_rect = m_dragIcons[gridID].m_Image.GetObjectRect();
	}

	public void SetGridTexture(int gridID, float factory)
	{
		m_dragIcons[gridID].m_Image.SetSize(new Vector2(m_dragIcons[gridID].m_rect.width * factory, m_dragIcons[gridID].m_rect.height * factory));
	}

	public void ClearGridTexture(int gridID)
	{
		m_dragIcons[gridID].m_Image.Visible = false;
		m_dragIcons[gridID].m_Image.Free();
		m_dragIcons[gridID].m_Image.Rect = m_dragIcons[gridID].m_Background.Rect;
	}

	public override void Draw()
	{
		base.Draw();
		if (!base.Visible)
		{
			return;
		}
		foreach (UIDragIcon dragIcon in m_dragIcons)
		{
			if (dragIcon.m_Shadow != null && dragIcon.m_Shadow.Visible)
			{
				dragIcon.m_Shadow.Draw();
			}
		}
		foreach (UIDragIcon dragIcon2 in m_dragIcons)
		{
			if (dragIcon2.m_Background.Visible)
			{
				dragIcon2.m_Background.Draw();
			}
		}
		if (m_Selected != null && m_Selected.Id >= 0)
		{
			m_Selected.Draw();
		}
		foreach (UIDragIcon dragIcon3 in m_dragIcons)
		{
			if (dragIcon3.m_Image.Visible)
			{
				dragIcon3.m_Image.Draw();
			}
		}
		foreach (UIDragIcon dragIcon4 in m_dragIcons)
		{
			if (dragIcon4.m_num != null && dragIcon4.m_num.Visible)
			{
				dragIcon4.m_num.Draw();
			}
		}
	}

	public override void Destory()
	{
		base.Destory();
		if (m_dragIcons == null)
		{
			return;
		}
		foreach (UIDragIcon dragIcon in m_dragIcons)
		{
			if (dragIcon != null)
			{
				dragIcon.Destory();
			}
		}
		m_dragIcons.Clear();
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		for (int i = 0; i < m_dragIcons.Count; i++)
		{
			m_dragIcons[i].m_draging = false;
			UIMove uIMove = m_dragIcons[i].m_UIMove;
			UIImage image = m_dragIcons[i].m_Image;
			if (control != uIMove)
			{
				continue;
			}
			switch (command)
			{
			case 3:
				image.Rect = new Rect(wparam - 0.5f * m_IconRect.width, lparam - 0.5f * m_IconRect.height, m_IconRect.width, m_IconRect.height);
				m_dragIcons[i].m_draging = true;
				m_Parent.SendEvent(this, 1, i, 0f);
				break;
			case 2:
			{
				image.Rect = uIMove.Rect;
				int num = -1;
				for (int j = 0; j < m_dragIcons.Count; j++)
				{
					if (m_dragIcons[j].m_UIMove.Rect.Contains(new Vector2(wparam, lparam)))
					{
						num = j;
						break;
					}
				}
				if (num == -1)
				{
					image.Rect = new Rect(wparam - 0.5f * m_IconRect.width, lparam - 0.5f * m_IconRect.height, m_IconRect.width, m_IconRect.height);
					m_Parent.SendEvent(this, 4, i, 0f);
				}
				else if (i != num)
				{
					image.Rect = m_dragIcons[num].m_UIMove.Rect;
					m_Parent.SendEvent(this, 3, i, num);
				}
				else
				{
					image.Rect = m_dragIcons[num].m_UIMove.Rect;
				}
				m_Parent.SendEvent(this, 2, 0f, 0f);
				break;
			}
			case 0:
				m_Parent.SendEvent(this, 0, i, lparam);
				break;
			}
		}
	}

	public new void Clear()
	{
		m_dragIcons.Clear();
		base.Clear();
	}
}
