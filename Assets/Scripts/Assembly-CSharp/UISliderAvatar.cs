using System.Collections.Generic;
using UnityEngine;

public class UISliderAvatar : UIPanelX, UIHandler
{
	public enum Command
	{
		Hide = 0,
		Click = 1,
		SelectIndex = 2
	}

	public class Avatar3D : UIPanelX
	{
		public UIImage m_Lock;

		public UI3DFrame m_obj;

		public float m_scale;

		public Vector3 m_pos;

		public byte m_state;

		public bool m_bMithril;

		public bool m_bStop;

		public int m_order;

		public override void Destory()
		{
			base.Destory();
			if (m_Lock != null)
			{
				m_Lock.Destory();
			}
		}
	}

	public const byte STATE_WORKABLE = 0;

	public const byte STATE_STRETCHING = 1;

	public const byte STATE_RETRACTING = 2;

	public const byte STATE_UNWORKABLE = 3;

	public List<Avatar3D> m_avatar3D = new List<Avatar3D>();

	public UIScroller m_scroller = new UIScroller();

	public Rect m_showRect;

	public byte m_state;

	private int m_selection;

	private Vector2 m_centerPos;

	protected float m_IconWidth;

	protected float m_IconHeight;

	public int m_num;

	public void Create(UnitUI ui)
	{
		SetUIHandler(this);
		SetState(1);
		base.Show();
	}

	public void SetState(byte state)
	{
		m_state = state;
	}

	public void SetInfos(Vector2 pos, float iconWidth, float iconHeight)
	{
		m_centerPos.x = pos.x;
		m_centerPos.y = pos.y;
		m_IconWidth = iconWidth;
		m_IconHeight = iconHeight;
	}

	public void SetPositon(float x, float y)
	{
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
		m_selection = index;
	}

	public float ToMeterX(float val)
	{
		float num = 6f / UIConstant.ScreenLocalWidth;
		return val * num;
	}

	public Vector2 ToMeterScreen(Vector2 val)
	{
		float num = 6f / UIConstant.ScreenLocalWidth;
		float num2 = 4f / UIConstant.ScreenLocalHeight;
		return new Vector2((val.x - UIConstant.ScreenLocalWidth * 0.5f) * num, (val.y - UIConstant.ScreenLocalHeight * 0.25f) * num2);
	}

	public Vector2 ToPixelScreen(Vector2 val)
	{
		float num = UIConstant.ScreenLocalWidth / 6f;
		float num2 = UIConstant.ScreenLocalHeight / 4f;
		return new Vector2((val.x + 3f) * num, (val.y + 1f) * num2);
	}

	public virtual void Add(Avatar3D avatar3D)
	{
		avatar3D.m_obj.SetParent(this);
		avatar3D.m_Lock.SetParent(this);
		m_avatar3D.Add(avatar3D);
	}

	public void Insert(int id, Avatar3D avatar3D)
	{
		avatar3D.m_obj.SetParent(this);
		avatar3D.m_Lock.SetParent(this);
		m_avatar3D.Insert(id, avatar3D);
	}

	public override void Draw()
	{
		base.Draw();
	}

	public override void Destory()
	{
		base.Destory();
		if (m_avatar3D == null)
		{
			return;
		}
		foreach (Avatar3D item in m_avatar3D)
		{
			if (item != null)
			{
				item.Destory();
			}
		}
		m_avatar3D.Clear();
	}

	public override void Update()
	{
		float num = 0f;
		int num2 = (int)(m_scroller.ScrollerPos / m_scroller.Spacing);
		float x = m_centerPos.x;
		float y = m_centerPos.y;
		int count = m_avatar3D.Count;
		float num3 = 0f;
		bool flag = true;
		bool flag2 = true;
		for (int i = 0; i < count; i++)
		{
			Avatar3D avatar3D = m_avatar3D[i];
			UI3DFrame obj = avatar3D.m_obj;
			UIImage @lock = avatar3D.m_Lock;
			if (obj.GetModel() == null)
			{
				continue;
			}
			num = x + m_scroller.Spacing * (float)i - m_scroller.ScrollerPos - 0.5f * m_IconWidth;
			if (num + m_IconWidth < m_showRect.x)
			{
				num = m_showRect.x + (float)count * m_scroller.Spacing - (m_showRect.x - num);
			}
			else if (num + m_IconWidth > m_showRect.x + (float)count * m_scroller.Spacing)
			{
				float num4 = num + m_IconWidth - (m_showRect.x + (float)count * m_scroller.Spacing);
				num = m_showRect.x - m_IconWidth + num4;
			}
			Vector3 position = Vector3.zero;
			obj.GetModel().SetActiveRecursively(true);
			float num5 = 1f;
			if (avatar3D.m_state == 0)
			{
				Transform transform = obj.GetModel().transform.Find("unlock(Clone)");
				transform.gameObject.SetActiveRecursively(false);
				Transform transform2 = obj.GetModel().transform.Find("unlock1(Clone)");
				transform2.gameObject.SetActiveRecursively(false);
				num5 = 0.08f;
			}
			else if (avatar3D.m_state == 15)
			{
				Transform transform3 = obj.GetModel().transform.Find("lock(Clone)");
				transform3.gameObject.SetActiveRecursively(false);
				if (avatar3D.m_bMithril)
				{
					Transform transform4 = obj.GetModel().transform.Find("unlock(Clone)");
					transform4.gameObject.SetActiveRecursively(false);
				}
				else
				{
					Transform transform5 = obj.GetModel().transform.Find("unlock1(Clone)");
					transform5.gameObject.SetActiveRecursively(false);
				}
			}
			else
			{
				Transform transform6 = obj.GetModel().transform.Find("lock(Clone)");
				transform6.gameObject.SetActiveRecursively(false);
				Transform transform7 = obj.GetModel().transform.Find("unlock(Clone)");
				transform7.gameObject.SetActiveRecursively(false);
				Transform transform8 = obj.GetModel().transform.Find("unlock1(Clone)");
				transform8.gameObject.SetActiveRecursively(false);
			}
			float num6 = num + m_IconWidth * 0.5f;
			float num7 = Mathf.Abs(x - num6);
			if (m_state == 1 || m_state == 2)
			{
				num7 = Mathf.Abs(x - (avatar3D.Rect.x + m_IconWidth * 0.5f));
			}
			float num8 = 1f - num7 / m_showRect.width;
			float num9 = 0f;
			float num10 = 0f;
			if (num8 <= 0f)
			{
				num8 = 0.2f;
			}
			if (num8 > 0f)
			{
				if (num8 >= 1f)
				{
					num8 = 1f;
				}
				if (num8 > 0.2f)
				{
					float num11 = 100f * (num7 + 1f) / (num7 * num7 + 100f);
					foreach (GameObject item in obj.GetSubModel())
					{
						if (item.GetComponent<Renderer>() != null)
						{
							Material[] materials = item.GetComponent<Renderer>().materials;
							foreach (Material material in materials)
							{
								Color color = new Color(num11 * num5, num11 * num5, num11 * num5, 1f);
								material.SetColor("_TintColor", color);
							}
						}
					}
				}
				else
				{
					obj.GetModel().SetActiveRecursively(false);
				}
				if (num7 <= 25f)
				{
					obj.GetModel().SetActiveRecursively(false);
				}
				obj.SetScale(new Vector3(num8 * avatar3D.m_scale, num8 * avatar3D.m_scale, num8 * avatar3D.m_scale));
				num9 = (0f - (num8 * avatar3D.m_scale - 1.2f)) * obj.CenterOffset.y;
				num10 = (0f - (num8 * avatar3D.m_scale - 1.2f)) * obj.CenterOffset.x;
				if ((double)Mathf.Abs(num10) < 0.01)
				{
					num10 = 0f;
				}
				num10 -= 1.2f * obj.CenterOffset.x;
			}
			switch (m_state)
			{
			case 0:
			{
				m_scroller.Update();
				float x2 = num + m_IconWidth * 0.5f;
				Vector2 vector3 = ToMeterScreen(new Vector2(x2, 0f));
				position = obj.GetModel().transform.position;
				position.x = vector3.x + num10 + avatar3D.m_pos.x;
				obj.SetPosition(position);
				break;
			}
			case 3:
				obj.GetModel().SetActiveRecursively(false);
				return;
			case 2:
			{
				float num15 = 30f;
				float num16 = ToMeterX(num15);
				float num17 = x;
				if (avatar3D.Rect.x + m_IconWidth * 0.5f > num17)
				{
					avatar3D.Rect = new Rect(avatar3D.Rect.x - num15, avatar3D.Rect.y, m_IconWidth, m_IconHeight);
					flag2 = false;
				}
				else if (avatar3D.Rect.x + m_IconWidth * 0.5f < num17)
				{
					avatar3D.Rect = new Rect(avatar3D.Rect.x + num15, avatar3D.Rect.y, m_IconWidth, m_IconHeight);
					flag2 = false;
				}
				if (Mathf.Abs(avatar3D.Rect.x + m_IconWidth * 0.5f - num17) <= num15)
				{
					avatar3D.Rect = new Rect(num17 - m_IconWidth * 0.5f, avatar3D.Rect.y, m_IconWidth, m_IconHeight);
					if (!avatar3D.m_bStop)
					{
						avatar3D.m_bStop = true;
						m_num++;
					}
				}
				Vector2 vector2 = ToMeterScreen(new Vector2(avatar3D.Rect.x + m_IconWidth * 0.5f, 0f));
				vector2.x += num10 + avatar3D.m_pos.x;
				position.x = vector2.x;
				if (m_num > 5)
				{
					flag2 = true;
				}
				break;
			}
			case 1:
			{
				float num12 = 30f;
				float num13 = ToMeterX(num12);
				float num14 = num + m_IconWidth * 0.5f;
				if (avatar3D.Rect.x + m_IconWidth * 0.5f > num14)
				{
					avatar3D.Rect = new Rect(avatar3D.Rect.x - num12, avatar3D.Rect.y, m_IconWidth, m_IconHeight);
					flag = false;
				}
				else if (avatar3D.Rect.x + m_IconWidth * 0.5f < num14)
				{
					avatar3D.Rect = new Rect(avatar3D.Rect.x + num12, avatar3D.Rect.y, m_IconWidth, m_IconHeight);
					flag = false;
				}
				if (Mathf.Abs(avatar3D.Rect.x + m_IconWidth * 0.5f - num14) <= num12)
				{
					avatar3D.Rect = new Rect(num14 - m_IconWidth * 0.5f, avatar3D.Rect.y, m_IconWidth, m_IconHeight);
					if (!avatar3D.m_bStop)
					{
						avatar3D.m_bStop = true;
						m_num++;
					}
				}
				Vector2 vector = ToMeterScreen(new Vector2(avatar3D.Rect.x + m_IconWidth * 0.5f, 0f));
				vector.x += num10 + avatar3D.m_pos.x;
				position.x = vector.x;
				if (m_num > 5)
				{
					flag = true;
				}
				break;
			}
			}
			@lock.Rect = avatar3D.Rect;
			Vector2 vector4 = ToMeterScreen(new Vector2(x, y));
			obj.SetPosition(new Vector3(position.x, vector4.y + num9 - 1.2f * obj.CenterOffset.y + avatar3D.m_pos.y, 5f + (1f - num8)));
			obj.SetClip(m_showRect);
			@lock.SetClip(m_showRect);
		}
		switch (m_state)
		{
		case 1:
			if (flag)
			{
				m_num = 0;
				SetState(0);
			}
			break;
		case 2:
			if (flag2)
			{
				m_num = 0;
				SetState(3);
			}
			break;
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (m_state == 0)
		{
			m_scroller.HandleInput(touch);
		}
		if (m_state == 0 || m_state == 3)
		{
		}
		return false;
	}

	public new void Clear()
	{
		for (int i = 0; i < m_avatar3D.Count; i++)
		{
			if (m_avatar3D[i].m_obj.GetModel() != null)
			{
				m_avatar3D[i].m_obj.GetModel().SetActive(false);
				m_avatar3D[i].m_obj.ClearModels();
			}
		}
		m_avatar3D.Clear();
	}

	public void ClearWithoutDestroyObj()
	{
		m_avatar3D.Clear();
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == m_scroller)
		{
			if (command == 3)
			{
				int index = (int)(wparam % (float)m_avatar3D.Count);
				m_Parent.SendEvent(this, 2, m_avatar3D[index].Id, lparam);
			}
		}
		else
		{
			m_Parent.SendEvent(this, 1, control.Id, lparam);
		}
	}
}
