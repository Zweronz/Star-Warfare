using System.Collections.Generic;
using UnityEngine;

public class UIListRoom : UIPanelX, UIHandler
{
	public enum Command
	{
		Click = 0,
		SelectIndex = 1
	}

	public class UIRoomIcon : UIPanelX
	{
		public UIImage m_background;

		public UIImage m_pingIcon;

		public UIImage m_mapIcon;

		public UIImage m_passwordIcon;

		public string m_userName = string.Empty;

		public Rect m_userNamePos;

		public FrUIText m_playerNum;

		public FrUIText m_mapName;

		public FrUIText m_pingValue;

		public bool m_bhasPassword;

		public UIImage m_rankMaskIcon;

		public List<UIImage> m_rankIcon = new List<UIImage>();

		public UIImage m_gameMode;

		public UIImage m_highlightFrame;

		protected int m_TouchFingerId = -1;

		private UIListRoom listRoom;

		public UIRoomIcon(UIListRoom listRoom)
		{
			this.listRoom = listRoom;
		}

		public override void Draw()
		{
			m_background.Draw();
			m_pingIcon.Draw();
			m_playerNum.Draw();
			m_mapIcon.Draw();
			m_mapName.Draw();
			m_pingValue.Draw();
			m_gameMode.Draw();
			if (m_bhasPassword)
			{
				m_passwordIcon.Draw();
			}
			m_rankMaskIcon.Draw();
			foreach (UIImage item in m_rankIcon)
			{
				item.Draw();
			}
			if (m_highlightFrame != null)
			{
				m_highlightFrame.Draw();
			}
		}

		public override void Destory()
		{
			base.Destory();
			if (m_background != null)
			{
				m_background.Destory();
			}
			if (m_pingIcon != null)
			{
				m_pingIcon.Destory();
			}
			if (m_playerNum != null)
			{
				m_playerNum.Destory();
			}
			if (m_mapIcon != null)
			{
				m_mapIcon.Destory();
			}
			if (m_mapName != null)
			{
				m_mapName.Destory();
			}
			if (m_pingValue != null)
			{
				m_pingValue.Destory();
			}
			if (m_passwordIcon != null)
			{
				m_passwordIcon.Destory();
			}
			if (m_rankMaskIcon != null)
			{
				m_rankMaskIcon.Destory();
			}
			if (m_gameMode != null)
			{
				m_gameMode.Destory();
			}
			if (m_rankIcon != null)
			{
				foreach (UIImage item in m_rankIcon)
				{
					item.Destory();
				}
			}
			if (m_highlightFrame != null)
			{
				m_highlightFrame.Destory();
			}
		}

		public void SetParentEx(UIContainer parent)
		{
			m_background.SetParent(parent);
			m_pingIcon.SetParent(parent);
			m_playerNum.SetParent(parent);
			m_mapIcon.SetParent(parent);
			m_pingValue.SetParent(parent);
			m_mapName.SetParent(parent);
			m_passwordIcon.SetParent(parent);
			m_rankMaskIcon.SetParent(parent);
			m_gameMode.SetParent(parent);
			foreach (UIImage item in m_rankIcon)
			{
				item.SetParent(parent);
			}
			if (m_highlightFrame != null)
			{
				m_highlightFrame.SetParent(parent);
			}
			SetParent(parent);
		}

		public override bool HandleInput(UITouchInner touch)
		{
			if (touch.phase == TouchPhase.Began)
			{
				if (PtInRect(touch.position))
				{
					m_TouchFingerId = touch.fingerId;
				}
			}
			else if (touch.phase == TouchPhase.Ended && touch.fingerId == m_TouchFingerId && PtInRect(touch.position))
			{
				m_Parent.SendEvent(this, 0, 0f, 0f);
				m_TouchFingerId = -1;
				return true;
			}
			return false;
		}
	}

	public Rect m_showRect;

	public List<UIRoomIcon> m_roomLst = new List<UIRoomIcon>();

	public UIImage m_brief;

	public UIImage m_selectionImg;

	public int m_selectIndex = -1;

	public string[] mMapNameDescriptions;

	private static byte[] BRIEF_BACKGROUND = new byte[3] { 13, 14, 15 };

	public void Create(UnitUI ui)
	{
		string[] gameText = Res2DManager.GetInstance().GetGameText();
		if (gameText.Length > 5)
		{
			mMapNameDescriptions = Res2DManager.GetInstance().SplitString(gameText[5]);
		}
		m_brief = new UIImage();
		m_brief.AddObject(ui, 0, BRIEF_BACKGROUND);
		m_brief.Rect = m_brief.GetObjectRect();
		m_brief.SetParent(this);
		m_selectionImg = new UIImage();
		m_selectionImg.AddObject(ui, 0, 23);
		m_selectionImg.Rect = m_selectionImg.GetObjectRect();
		m_selectionImg.SetParent(this);
		m_selectionImg.Visible = false;
		SetUIHandler(this);
		base.Show();
	}

	public virtual void Add(UIRoomIcon room)
	{
		room.SetParentEx(this);
		m_roomLst.Add(room);
	}

	public void ClearRoomList()
	{
		m_roomLst.Clear();
	}

	public new void Clear()
	{
		m_roomLst.Clear();
		mMapNameDescriptions = null;
	}

	public void SetSelection(int index)
	{
		if (index == -1)
		{
			m_selectIndex = index;
			m_selectionImg.Visible = false;
		}
		else
		{
			m_selectIndex = (byte)index;
			m_selectionImg.Visible = true;
			m_selectionImg.Rect = new Rect(m_roomLst[index].m_background.Rect.x - 10f, m_roomLst[index].m_background.Rect.y, m_selectionImg.Rect.width, m_roomLst[index].m_background.Rect.height);
		}
	}

	public void SetRoomTexture(int listID, UnitUI ui, Room room)
	{
		float num = 80f;
		float num2 = 400f;
		float width = 64f;
		float num3 = 92f;
		float num4 = 60f;
		int ping = room.getPing();
		int num5 = 0;
		num5 = ((ping > 50) ? ((ping <= 100) ? 1 : ((ping <= 200) ? 2 : ((ping > 350) ? 4 : 3))) : 0);
		m_roomLst[listID].m_pingIcon.Visible = true;
		m_roomLst[listID].m_pingIcon.SetTexture(ui, 10, num5);
		m_roomLst[listID].m_pingIcon.Rect = new Rect(m_roomLst[listID].m_background.Rect.x + m_roomLst[listID].m_background.Rect.width - num, m_roomLst[listID].m_background.Rect.y - 20f, num, m_roomLst[listID].m_background.Rect.height);
		m_roomLst[listID].m_mapIcon.Visible = true;
		if (room.getGameMode() == 2)
		{
			m_roomLst[listID].m_mapIcon.SetTexture(ui, 8, room.getMapID() - Global.TOTAL_SURVIVAL_STAGE);
		}
		else if (room.getGameMode() == 1)
		{
			m_roomLst[listID].m_mapIcon.SetTexture(ui, 7, room.getMapID());
		}
		else
		{
			m_roomLst[listID].m_mapIcon.SetTexture(ui, 9, room.getMapID() - Global.TOTAL_SURVIVAL_STAGE - Global.TOTAL_BOSS_STAGE);
		}
		m_roomLst[listID].m_mapIcon.Rect = new Rect(m_roomLst[listID].m_background.Rect.x + m_roomLst[listID].m_background.Rect.width - num - num3, m_roomLst[listID].m_background.Rect.y + 1f, num3, m_roomLst[listID].m_background.Rect.height);
		m_roomLst[listID].m_userName = room.getRoomName();
		m_roomLst[listID].m_userNamePos = UIConstant.GetRectForScreenAdaptived(new Rect(m_roomLst[listID].m_background.Rect.x + num4, UIConstant.ScreenLocalHeight - m_roomLst[listID].m_background.Rect.y + 16f - m_roomLst[listID].m_background.Rect.height, num2, m_roomLst[listID].m_background.Rect.height));
		m_roomLst[listID].m_mapName.Visible = true;
		if (m_roomLst[listID].m_mapName != null && mMapNameDescriptions[room.getMapID()] != null)
		{
			m_roomLst[listID].m_mapName.Set("font2", mMapNameDescriptions[room.getMapID()], UIConstant.fontColor_cyan, num2);
			m_roomLst[listID].m_mapName.AlignStyle = FrUIText.enAlignStyle.TOP_LEFT;
			m_roomLst[listID].m_mapName.Rect = new Rect(m_roomLst[listID].m_background.Rect.x + num4, m_roomLst[listID].m_background.Rect.y - 30f, num2, m_roomLst[listID].m_background.Rect.height);
		}
		m_roomLst[listID].m_playerNum.Visible = true;
		m_roomLst[listID].m_playerNum.Set("font2", room.getJoinedPlayer() + "/" + room.getMaxPlayer(), UIConstant.fontColor_cyan, width);
		m_roomLst[listID].m_playerNum.AlignStyle = FrUIText.enAlignStyle.CENTER_CENTER;
		m_roomLst[listID].m_playerNum.Rect = new Rect(m_roomLst[listID].m_background.Rect.x + num2, m_roomLst[listID].m_background.Rect.y - 2f, width, m_roomLst[listID].m_background.Rect.height);
		m_roomLst[listID].m_pingValue.Visible = true;
		m_roomLst[listID].m_pingValue.Set("font2", room.getPing().ToString(), UIConstant.fontColor_cyan, num);
		m_roomLst[listID].m_pingValue.Rect = new Rect(m_roomLst[listID].m_background.Rect.x + m_roomLst[listID].m_background.Rect.width - num + 20f, m_roomLst[listID].m_background.Rect.y - 15f, num, m_roomLst[listID].m_background.Rect.height);
		m_roomLst[listID].m_bhasPassword = room.isHasPassword();
		if (m_roomLst[listID].m_bhasPassword)
		{
			m_roomLst[listID].m_passwordIcon.Visible = true;
			m_roomLst[listID].m_passwordIcon.SetTexture(ui, 0, 24);
			m_roomLst[listID].m_passwordIcon.Rect = new Rect(m_roomLst[listID].m_background.Rect.x + 20f, m_roomLst[listID].m_background.Rect.y, 24f, m_roomLst[listID].m_background.Rect.height);
		}
		m_roomLst[listID].m_gameMode.Visible = true;
		if (room.getGameMode() == 2)
		{
			m_roomLst[listID].m_gameMode.SetTexture(ui, 0, 31);
		}
		else if (room.getGameMode() == 1)
		{
			m_roomLst[listID].m_gameMode.SetTexture(ui, 0, 30);
		}
		else if (room.getGameMode() == 4)
		{
			m_roomLst[listID].m_gameMode.SetTexture(ui, 0, 34);
		}
		else if (room.getGameMode() == 3)
		{
			m_roomLst[listID].m_gameMode.SetTexture(ui, 0, 35);
		}
		else if (room.getGameMode() == 5)
		{
			m_roomLst[listID].m_gameMode.SetTexture(ui, 0, 32);
		}
		else if (room.getGameMode() == 8)
		{
			m_roomLst[listID].m_gameMode.SetTexture(ui, 0, 33);
		}
		else if (room.getGameMode() == 6)
		{
			m_roomLst[listID].m_gameMode.SetTexture(ui, 0, 42);
		}
		else if (room.getGameMode() == 9)
		{
			m_roomLst[listID].m_gameMode.SetTexture(ui, 0, 43);
		}
		else
		{
			m_roomLst[listID].m_gameMode.SetTexture(ui, 0, 33);
		}
		m_roomLst[listID].m_gameMode.Rect = new Rect(m_roomLst[listID].m_background.Rect.x + num2 - 100f, m_roomLst[listID].m_background.Rect.y + 20f, 100f, m_roomLst[listID].m_background.Rect.height);
		m_roomLst[listID].m_rankMaskIcon.Visible = true;
		m_roomLst[listID].m_rankMaskIcon.SetTexture(ui, 0, 25);
		m_roomLst[listID].m_rankMaskIcon.Rect = new Rect(m_roomLst[listID].m_background.Rect.x + num4, m_roomLst[listID].m_background.Rect.y - 25f, m_roomLst[listID].m_rankMaskIcon.GetObjectRect().width, m_roomLst[listID].m_background.Rect.height);
		int minJoinRankID = room.getMinJoinRankID();
		int maxJoinRankID = room.getMaxJoinRankID();
		m_roomLst[listID].m_rankIcon.Clear();
		Rect moduleRect = ui.GetModuleRect(0, 3, 0);
		for (int i = minJoinRankID; i <= maxJoinRankID; i++)
		{
			UIImage uIImage = new UIImage();
			uIImage.SetTexture(ui, 3, i);
			uIImage.Rect = new Rect(m_roomLst[listID].m_background.Rect.x + num4 + (float)i * moduleRect.width, m_roomLst[listID].m_background.Rect.y - 25f, moduleRect.width, m_roomLst[listID].m_background.Rect.height);
			m_roomLst[listID].m_rankIcon.Add(uIImage);
			uIImage.SetParent(m_roomLst[listID]);
		}
	}

	public void SetClipRect(float x, float y, float width, float height)
	{
		m_showRect = new Rect(x, y, width, height);
	}

	public override void Draw()
	{
		for (int i = 0; i < m_roomLst.Count; i++)
		{
			m_roomLst[i].Draw();
		}
		m_brief.Draw();
		if (m_selectionImg.Visible)
		{
			m_selectionImg.Draw();
		}
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_roomLst != null)
		{
			foreach (UIRoomIcon item in m_roomLst)
			{
				if (item != null)
				{
					item.Destory();
				}
			}
			m_roomLst.Clear();
		}
		if (m_brief != null)
		{
			m_brief.Destory();
		}
		if (m_selectionImg != null)
		{
			m_selectionImg.Destory();
		}
	}

	public override void Update()
	{
	}

	public override bool HandleInput(UITouchInner touch)
	{
		foreach (UIRoomIcon item in m_roomLst)
		{
			if (item.HandleInput(touch))
			{
				return true;
			}
		}
		if (base.HandleInput(touch))
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
