using UnityEngine;

public class UIControlVisible : UIControl
{
	protected UISpriteSet[] m_SpriteSet;

	public UIControlVisible()
	{
		m_SpriteSet = null;
	}

	protected void CreateSprite(int count)
	{
		m_SpriteSet = new UISpriteSet[count];
		for (int i = 0; i < count; i++)
		{
			m_SpriteSet[i] = new UISpriteSet();
		}
	}

	protected void AddSprites(int index, UnitUI ui, int frame, int module)
	{
		m_SpriteSet[index].AddSprites(ui, frame, module);
	}

	protected void AddSprites(int index, UnitUI ui, int frame, byte moduleBegin, byte moduleCount)
	{
		m_SpriteSet[index].AddSprites(ui, frame, moduleBegin, moduleCount);
	}

	protected void AddSprites(int index, UnitUI ui, int frame, byte[] module)
	{
		m_SpriteSet[index].AddSprites(ui, frame, module);
	}

	protected void AddSprites(int index, UnitUI ui, int frame)
	{
		m_SpriteSet[index].AddSprites(ui, frame);
	}

	protected Rect GetSpritesRect(int index)
	{
		return m_SpriteSet[index].GetSpritesRect();
	}

	protected void SetSpriteTexture(int index, UnitUI ui, int frame, int module)
	{
		m_SpriteSet[index].SetSpriteTexture(ui, frame, module);
	}

	protected void SetSpriteTexture(int index, UnitUI ui, int frame, byte[] module)
	{
		m_SpriteSet[index].SetSpriteTexture(ui, frame, module);
	}

	protected void SetSpriteTexture(int index, UnitUI ui, int frame)
	{
		m_SpriteSet[index].SetSpriteTexture(ui, frame);
	}

	protected void SetSpriteSize(int index, Vector2 size)
	{
		m_SpriteSet[index].SetSpriteSize(size);
	}

	protected Vector2 GetSpriteSize(int index)
	{
		return m_SpriteSet[index].GetSpriteSize();
	}

	protected Vector2 GetSpriteSize(int index, int subIndex)
	{
		return m_SpriteSet[index].GetSpriteSize(subIndex);
	}

	protected void SetSpriteColor(int index, Color color)
	{
		m_SpriteSet[index].SetSpriteColor(color);
	}

	protected void SetSpritePosition(int index, Vector2 position)
	{
		m_SpriteSet[index].SetSpritePosition(position);
	}

	protected void SetSpritePosition(int index, int subIndex, Vector2 position)
	{
		m_SpriteSet[index].SetSpritePosition(subIndex, position);
	}

	protected void SetSpriteRotation(int index, float rotation)
	{
		m_SpriteSet[index].SetSpriteRotation(rotation);
	}

	protected float GetSpriteRotation(int index)
	{
		return m_SpriteSet[index].GetSpriteRotation();
	}

	protected void FreeSprite(int index)
	{
		m_SpriteSet[index].FreeSprites();
	}

	public Vector2 GetSpritePosition(int index)
	{
		return m_SpriteSet[index].GetCenter();
	}

	public override void Destory()
	{
		if (m_SpriteSet == null)
		{
			return;
		}
		for (int i = 0; i < m_SpriteSet.Length; i++)
		{
			if (m_SpriteSet[i] != null)
			{
				m_SpriteSet[i].Destory();
			}
		}
	}

	public void SetClipOffs(int index, Vector2 clip_offs)
	{
		if (m_SpriteSet != null)
		{
			for (int i = 0; i < m_SpriteSet.Length; i++)
			{
				m_SpriteSet[i].SetClipOffs(index, clip_offs);
			}
		}
	}

	public new void SetClip(Rect clip_rect)
	{
		int num = (int)((double)UIConstant.ScreenLocalWidth * 0.5);
		int num2 = (int)((double)UIConstant.ScreenLocalHeight * 0.5);
		int num3 = (int)((double)Screen.width * 0.5 + (double)((clip_rect.xMin - (float)num) * UIConstant.ScreenAdaptived.x));
		int num4 = (int)((double)Screen.width * 0.5 + (double)((clip_rect.xMax - (float)num) * UIConstant.ScreenAdaptived.x));
		int num5 = (int)((double)Screen.height * 0.5 + (double)((clip_rect.yMin - (float)num2) * UIConstant.ScreenAdaptived.y));
		int num6 = (int)((double)Screen.height * 0.5 + (double)((clip_rect.yMax - (float)num2) * UIConstant.ScreenAdaptived.y));
		Rect clip = new Rect(num3, num5, num4 - num3, num6 - num5);
		base.SetClip(clip);
		if (m_SpriteSet != null)
		{
			for (int i = 0; i < m_SpriteSet.Length; i++)
			{
				m_SpriteSet[i].SetClip(clip_rect);
			}
		}
	}

	public new void ClearClip()
	{
		base.ClearClip();
		if (m_SpriteSet != null)
		{
			for (int i = 0; i < m_SpriteSet.Length; i++)
			{
				m_SpriteSet[i].ClearClip();
			}
		}
	}

	public void SetScaleWithInt(bool bInt)
	{
		if (m_SpriteSet != null)
		{
			for (int i = 0; i < m_SpriteSet.Length; i++)
			{
				m_SpriteSet[i].SetScaleWithInt(bInt);
			}
		}
	}
}
