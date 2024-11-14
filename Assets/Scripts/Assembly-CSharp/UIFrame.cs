using UnityEngine;

public class UIFrame : UIControlVisible
{
	public enum Command
	{
		Click = 0
	}

	protected int m_TouchFingerId = -1;

	private int m_Duration = 1;

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
		}
	}

	public UIFrame()
	{
		CreateSprite(1);
	}

	public void SetDuration(int duration)
	{
		m_Duration = duration;
	}

	public int GetDuration()
	{
		return m_Duration;
	}

	public Rect GetObjectRect()
	{
		return GetSpritesRect(0);
	}

	public void AddObject(UnitUI ui, int frame, int module)
	{
		AddSprites(0, ui, frame, module);
	}

	public void AddObject(UnitUI ui, int frame, byte[] module)
	{
		AddSprites(0, ui, frame, module);
	}

	public void AddObject(UnitUI ui, int frame)
	{
		AddSprites(0, ui, frame);
	}

	public void SetTexture(UnitUI ui, int frame, int module)
	{
		SetSpriteTexture(0, ui, frame, module);
	}

	public void SetTexture(UnitUI ui, int frame, byte[] module)
	{
		SetSpriteTexture(0, ui, frame, module);
	}

	public void SetTexture(UnitUI ui, int frame)
	{
		SetSpriteTexture(0, ui, frame);
	}

	public void SetSize(Vector2 size)
	{
		SetSpriteSize(0, size);
	}

	public void SetColor(Color color)
	{
		SetSpriteColor(0, color);
	}

	public void SetRotate(float rotate)
	{
		SetSpriteRotation(0, rotate);
	}

	public float GetRotate()
	{
		return GetSpriteRotation(0);
	}

	public override void Draw()
	{
		for (int i = 0; i < m_SpriteSet[0].m_Sprite.Count; i++)
		{
			m_Parent.DrawSprite((UISpriteX)m_SpriteSet[0].m_Sprite[i]);
		}
	}

	public override void Destory()
	{
		if (m_SpriteSet[0] != null)
		{
			m_SpriteSet[0].Destory();
		}
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			m_TouchFingerId = touch.fingerId;
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
