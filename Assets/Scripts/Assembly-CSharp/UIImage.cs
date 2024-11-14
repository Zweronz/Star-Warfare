using UnityEngine;

public class UIImage : UIControlVisible
{
	public enum Command
	{
		Click = 0
	}

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
		}
	}

	public UIImage()
	{
		base.Enable = false;
		CreateSprite(1);
	}

	public Rect GetObjectRect()
	{
		return GetSpritesRect(0);
	}

	public void AddObject(UnitUI ui, int frame, int module)
	{
		AddSprites(0, ui, frame, module);
	}

	public void AddObject(UnitUI ui, int frame, byte moduleBegin, byte moduleCount)
	{
		AddSprites(0, ui, frame, moduleBegin, moduleCount);
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

	public Vector2 GetSize()
	{
		return GetSpriteSize(0);
	}

	public Vector2 GetSize(int index)
	{
		return GetSpriteSize(0, index);
	}

	public void SetSize(Vector2 size)
	{
		SetSpriteSize(0, size);
	}

	public void SetRotation(float rotation)
	{
		SetSpriteRotation(0, rotation);
	}

	public float GetRotation()
	{
		return GetSpriteRotation(0);
	}

	public void SetColor(Color color)
	{
		SetSpriteColor(0, color);
	}

	public UISpriteSet GetSpriteSet()
	{
		return m_SpriteSet[0];
	}

	public void Free()
	{
		FreeSprite(0);
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
			if (PtInRect(touch.position))
			{
				m_TouchFingerId = touch.fingerId;
				return true;
			}
			return false;
		}
		if (touch.phase == TouchPhase.Ended && touch.fingerId == m_TouchFingerId && PtInRect(touch.position))
		{
			m_Parent.SendEvent(this, 0, 0f, 0f);
			m_TouchFingerId = -1;
			return true;
		}
		return false;
	}
}
