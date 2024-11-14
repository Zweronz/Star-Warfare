using System.Collections;
using UnityEngine;

public class UISpriteSet
{
	public ArrayList m_Sprite = new ArrayList();

	public void AddSprites(UnitUI ui, int frame, int module)
	{
		UISpriteX uISpriteX = new UISpriteX();
		uISpriteX.UI = ui;
		uISpriteX.FrameIdx = frame;
		uISpriteX.ModuleIdx = module;
		uISpriteX.MaterialID = ui.GetMaterialIndex(0, frame, module);
		uISpriteX.TextureRect = ui.GetModuleRect(0, frame, module);
		uISpriteX.Size = ui.GetModuleSize(0, frame, module);
		switch (ui.GetModuleRotation(0, frame, module))
		{
		case 4:
			uISpriteX.FlipX = true;
			break;
		case 5:
			uISpriteX.FlipY = true;
			break;
		}
		uISpriteX.Position = ui.GetModulePosition(0, frame, module);
		m_Sprite.Add(uISpriteX);
	}

	public void AddSprites(UnitUI ui, int frame, byte moduleBegin, byte moduleCount)
	{
		for (int i = moduleBegin; i < moduleBegin + moduleCount; i++)
		{
			AddSprites(ui, frame, i);
		}
	}

	public void AddSprites(UnitUI ui, int frame, byte[] module)
	{
		for (int i = 0; i < module.Length; i++)
		{
			AddSprites(ui, frame, module[i]);
		}
	}

	public void AddSprites(UnitUI ui, int frame)
	{
		int moduleCount = ui.GetModuleCount(0, frame);
		for (int i = 0; i < moduleCount; i++)
		{
			AddSprites(ui, frame, i);
		}
	}

	public void SetSpriteTexture(UnitUI ui, int frame, int module)
	{
		FreeSprites();
		AddSprites(ui, frame, module);
	}

	public void SetSpriteTexture(UnitUI ui, int frame, byte[] module)
	{
		FreeSprites();
		AddSprites(ui, frame, module);
	}

	public void SetSpriteTexture(UnitUI ui, int frame)
	{
		FreeSprites();
		AddSprites(ui, frame);
	}

	public Vector2 GetSpriteSize()
	{
		Rect spritesRect = GetSpritesRect();
		Vector2 result = default(Vector2);
		result.x = spritesRect.width;
		result.y = spritesRect.height;
		return result;
	}

	public Vector2 GetSpriteSize(int index)
	{
		UISpriteX uISpriteX = (UISpriteX)m_Sprite[index];
		return uISpriteX.Size;
	}

	public Rect GetSpritesRect()
	{
		Rect rect = new Rect(0f, 0f, 0f, 0f);
		if (m_Sprite.Count <= 0)
		{
			return rect;
		}
		Vector2 size = ((UISpriteX)m_Sprite[0]).Size;
		Vector2 position = ((UISpriteX)m_Sprite[0]).Position;
		rect.x = position.x - size.x * 0.5f;
		rect.y = position.y - size.y * 0.5f;
		rect.width = size.x;
		rect.height = size.y;
		Rect rct = default(Rect);
		for (int i = 1; i < m_Sprite.Count; i++)
		{
			Vector2 size2 = ((UISpriteX)m_Sprite[i]).Size;
			Vector2 position2 = ((UISpriteX)m_Sprite[i]).Position;
			rct.x = position2.x - size2.x * 0.5f;
			rct.y = position2.y - size2.y * 0.5f;
			rct.width = size2.x;
			rct.height = size2.y;
			rect = UnionRect(rect, rct);
		}
		return rect;
	}

	public Rect GetSpriteInitialRect()
	{
		Rect rect = ((UISpriteX)m_Sprite[0]).UI.GetModulePositionRect(0, ((UISpriteX)m_Sprite[0]).FrameIdx, ((UISpriteX)m_Sprite[0]).ModuleIdx);
		for (int i = 1; i < m_Sprite.Count; i++)
		{
			Rect modulePositionRect = ((UISpriteX)m_Sprite[i]).UI.GetModulePositionRect(0, ((UISpriteX)m_Sprite[i]).FrameIdx, ((UISpriteX)m_Sprite[i]).ModuleIdx);
			rect = UnionRect(rect, modulePositionRect);
		}
		return rect;
	}

	public Vector2 GetCenter()
	{
		if (m_Sprite.Count <= 0)
		{
			return Vector2.zero;
		}
		Vector2 size = ((UISpriteX)m_Sprite[0]).Size;
		Vector2 position = ((UISpriteX)m_Sprite[0]).Position;
		Rect rct = new Rect(position.x - size.x * 0.5f, position.y - size.y * 0.5f, size.x, size.y);
		Rect rct2 = default(Rect);
		for (int i = 1; i < m_Sprite.Count; i++)
		{
			Vector2 size2 = ((UISpriteX)m_Sprite[i]).Size;
			Vector2 position2 = ((UISpriteX)m_Sprite[i]).Position;
			rct2.x = position2.x - size2.x * 0.5f;
			rct2.y = position2.y - size2.y * 0.5f;
			rct2.width = size2.x;
			rct2.height = size2.y;
			rct = UnionRect(rct, rct2);
		}
		Vector2 result = default(Vector2);
		result.x = rct.x + rct.width * 0.5f;
		result.y = rct.y + rct.height * 0.5f;
		return result;
	}

	public void SetSpriteSize(Vector2 size)
	{
		Rect spritesRect = GetSpritesRect();
		float num = size.x / spritesRect.width;
		float num2 = size.y / spritesRect.height;
		Vector2 center = GetCenter();
		Vector2 size3 = default(Vector2);
		Vector2 position = default(Vector2);
		for (int i = 0; i < m_Sprite.Count; i++)
		{
			UISpriteX uISpriteX = (UISpriteX)m_Sprite[i];
			Vector2 size2 = ((UISpriteX)m_Sprite[i]).Size;
			size3.x = size2.x * num;
			size3.y = size2.y * num2;
			uISpriteX.Size = size3;
			position.x = center.x + (uISpriteX.Position.x - center.x) * num;
			position.y = center.y + (uISpriteX.Position.y - center.y) * num2;
			uISpriteX.Position = position;
		}
	}

	public void SetSpritePosition(Vector2 position)
	{
		Vector2 center = GetCenter();
		Vector2 position2 = default(Vector2);
		for (int i = 0; i < m_Sprite.Count; i++)
		{
			UISpriteX uISpriteX = (UISpriteX)m_Sprite[i];
			position2.x = position.x + (uISpriteX.Position.x - center.x);
			position2.y = position.y + (uISpriteX.Position.y - center.y);
			uISpriteX.Position = position2;
		}
	}

	public void SetSpritePosition(int index, Vector2 position)
	{
		UISpriteX uISpriteX = (UISpriteX)m_Sprite[index];
		uISpriteX.Position = position;
	}

	public Vector2 GetSpritePosition()
	{
		return GetCenter();
	}

	public void SetSpriteColor(Color color)
	{
		for (int i = 0; i < m_Sprite.Count; i++)
		{
			((UISpriteX)m_Sprite[i]).Color = color;
		}
	}

	public void SetSpriteRotation(float rotation)
	{
		((UISpriteX)m_Sprite[0]).Rotation = rotation;
	}

	public float GetSpriteRotation()
	{
		return ((UISpriteX)m_Sprite[0]).Rotation;
	}

	public void SetClip(Rect clip_rect)
	{
		for (int i = 0; i < m_Sprite.Count; i++)
		{
			((UISpriteX)m_Sprite[i]).SetClip(clip_rect);
		}
	}

	public void SetClipOffs(int index, Vector2 clip_offs)
	{
		for (int i = 0; i < m_Sprite.Count; i++)
		{
			((UISpriteX)m_Sprite[i]).SetClipOffs(index, clip_offs);
		}
	}

	public void ClearClip()
	{
		for (int i = 0; i < m_Sprite.Count; i++)
		{
			((UISpriteX)m_Sprite[i]).ClearClip();
		}
	}

	public void SetScaleWithInt(bool bInt)
	{
		for (int i = 0; i < m_Sprite.Count; i++)
		{
			((UISpriteX)m_Sprite[i]).ScaleWithInt = bInt;
		}
	}

	public void FreeSprites()
	{
		m_Sprite.Clear();
	}

	public void Destory()
	{
		if (m_Sprite == null)
		{
			return;
		}
		for (int i = 0; i < m_Sprite.Count; i++)
		{
			if (m_Sprite[i] != null)
			{
				((UISpriteX)m_Sprite[i]).Material = null;
			}
		}
	}

	public Rect UnionRect(Rect rct1, Rect rct2)
	{
		Rect result = default(Rect);
		result.x = Mathf.Min(rct1.x, rct2.x);
		result.y = Mathf.Min(rct1.y, rct2.y);
		result.width = Mathf.Max(rct1.x + rct1.width, rct2.x + rct2.width) - result.x;
		result.height = Mathf.Max(rct1.y + rct1.height, rct2.y + rct2.height) - result.y;
		return result;
	}
}
