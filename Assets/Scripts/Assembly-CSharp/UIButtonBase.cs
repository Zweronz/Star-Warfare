using UnityEngine;

public class UIButtonBase : UIControlVisible
{
	public enum State
	{
		Normal = 0,
		Pressed = 1,
		Disabled = 2
	}

	protected State m_State;

	protected UISpriteX m_HoverSprite;

	protected Vector2 m_HoverCenterDelta;

	public UIButtonBase()
	{
		CreateSprite(3);
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
		SetSpriteRotation(2, rotate);
	}

	public float GetRotate()
	{
		return GetSpriteRotation((int)m_State);
	}

	public void SetHoverSprite(Material material, Rect texture_rect)
	{
		SetHoverSprite(material, texture_rect, new Vector2(0f, 0f));
	}

	public void SetHoverSprite(Material material, Rect texture_rect, Vector2 center_deta)
	{
		m_HoverSprite = new UISpriteX();
		m_HoverSprite.Material = material;
		m_HoverSprite.TextureRect = texture_rect;
		m_HoverSprite.Size = new Vector2(texture_rect.width, texture_rect.height);
		m_HoverSprite.Position = new Vector2(base.Rect.x + base.Rect.width / 2f, base.Rect.y + base.Rect.height / 2f) + center_deta;
		m_HoverCenterDelta = center_deta;
	}

	public new void SetClip(Rect clip_rect)
	{
		base.SetClip(clip_rect);
		if (m_HoverSprite != null)
		{
			m_HoverSprite.SetClip(clip_rect);
		}
	}

	public override void Draw()
	{
		if (!m_Enable)
		{
			DrawSpriteSet(2);
			return;
		}
		switch (m_State)
		{
		case State.Normal:
			DrawSpriteSet(0);
			break;
		case State.Pressed:
			DrawSpriteSet(1);
			if (m_HoverSprite != null)
			{
				m_HoverSprite.Position = new Vector2(base.Rect.x + base.Rect.width / 2f, base.Rect.y + base.Rect.height / 2f) + m_HoverCenterDelta;
				m_Parent.DrawSprite(m_HoverSprite);
			}
			break;
		}
	}

	public override void Destory()
	{
		if (m_HoverSprite != null)
		{
			m_HoverSprite.Material = null;
		}
		if (m_SpriteSet[0] != null)
		{
			m_SpriteSet[0].Destory();
		}
		if (m_SpriteSet[1] != null)
		{
			m_SpriteSet[1].Destory();
		}
		if (m_SpriteSet[2] != null)
		{
			m_SpriteSet[2].Destory();
		}
	}

	public void DrawSpriteSet(int index)
	{
		for (int i = 0; i < m_SpriteSet[index].m_Sprite.Count; i++)
		{
			m_Parent.DrawSprite((UISpriteX)m_SpriteSet[index].m_Sprite[i]);
		}
	}
}
