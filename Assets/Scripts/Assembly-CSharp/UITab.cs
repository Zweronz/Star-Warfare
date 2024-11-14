using UnityEngine;

public class UITab : UIControlVisible
{
	public enum State
	{
		Normal = 0,
		Selected = 1
	}

	public enum Command
	{
		Click = 0
	}

	protected State m_State;

	protected int m_TouchFingerId = -1;

	public new Rect Rect
	{
		get
		{
			return base.Rect;
		}
		set
		{
			base.Rect = value;
			Vector2 position = new Vector2(value.x + value.width / 2f, value.y + value.height / 2f);
			SetSpritePosition(0, position);
			SetSpritePosition(1, position);
		}
	}

	public UITab()
	{
		CreateSprite(2);
		m_State = State.Normal;
	}

	public Rect GetObjectRect(State state)
	{
		return GetSpritesRect((int)state);
	}

	public void AddObject(State state, UnitUI ui, int frame, int module)
	{
		AddSprites((int)state, ui, frame, module);
	}

	public void AddObject(State state, UnitUI ui, int frame, byte moduleBegin, byte moduleCount)
	{
		AddSprites((int)state, ui, frame, moduleBegin, moduleCount);
	}

	public void AddObject(State state, UnitUI ui, int frame, byte[] module)
	{
		AddSprites((int)state, ui, frame, module);
	}

	public void AddObject(State state, UnitUI ui, int frame)
	{
		AddSprites((int)state, ui, frame);
	}

	public void SetTexture(State state, UnitUI ui, int frame, int module)
	{
		SetSpriteTexture((int)state, ui, frame, module);
	}

	public void SetTexture(State state, UnitUI ui, int frame, byte[] module)
	{
		SetSpriteTexture((int)state, ui, frame, module);
	}

	public void SetTexture(State state, UnitUI ui, int frame)
	{
		SetSpriteTexture((int)state, ui, frame);
	}

	public void SetColor(State state, Color color)
	{
		SetSpriteColor((int)state, color);
	}

	public void SetSize(State state, Vector2 size)
	{
		SetSpriteSize((int)state, size);
	}

	public Vector2 GetSize(State state)
	{
		return GetSpriteSize((int)state);
	}

	public void SetRotate(float rotate)
	{
		SetSpriteRotation(0, rotate);
		SetSpriteRotation(1, rotate);
	}

	public float GetRotate()
	{
		return GetSpriteRotation((int)m_State);
	}

	public new void SetClip(Rect clip_rect)
	{
		base.SetClip(clip_rect);
	}

	public void SetState(State state)
	{
		m_State = state;
	}

	public override void Draw()
	{
		switch (m_State)
		{
		case State.Normal:
			DrawSpriteSet(0);
			break;
		case State.Selected:
			DrawSpriteSet(1);
			break;
		}
	}

	public override void Destory()
	{
		if (m_SpriteSet[0] != null)
		{
			m_SpriteSet[0].Destory();
		}
		if (m_SpriteSet[1] != null)
		{
			m_SpriteSet[1].Destory();
		}
	}

	public void DrawSpriteSet(int index)
	{
		for (int i = 0; i < m_SpriteSet[index].m_Sprite.Count; i++)
		{
			m_Parent.DrawSprite((UISpriteX)m_SpriteSet[index].m_Sprite[i]);
		}
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
		if (touch.phase == TouchPhase.Ended && touch.fingerId == m_TouchFingerId && PtInRect(touch.position))
		{
			m_State = State.Selected;
			m_Parent.SendEvent(this, 0, 0f, 0f);
			m_TouchFingerId = -1;
			return true;
		}
		return false;
	}
}
