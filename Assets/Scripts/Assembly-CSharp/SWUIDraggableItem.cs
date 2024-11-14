using System.Collections.Generic;
using UnityEngine;

public class SWUIDraggableItem
{
	public UISprite[] Sprites;

	public float SpriteWidth { get; set; }

	public UIDraggableListItem UIDraggableListItem { get; set; }

	public void SetSpriteAlpha(float alpha)
	{
		UISprite[] sprites = Sprites;
		foreach (UISprite uISprite in sprites)
		{
			uISprite.color = new Color(uISprite.color.r, uISprite.color.g, uISprite.color.b, alpha);
		}
	}

	public void SetDepth(int depth)
	{
		UISprite[] sprites = Sprites;
		foreach (UISprite uISprite in sprites)
		{
			uISprite.depth = depth;
		}
	}

	public void SetAmount(float amount)
	{
		UISprite[] sprites = Sprites;
		foreach (UISprite uISprite in sprites)
		{
			uISprite.fillAmount = amount;
		}
	}

	public static List<SWUIDraggableItem> CreateList(List<UIDraggableListItem> list)
	{
		List<SWUIDraggableItem> list2 = new List<SWUIDraggableItem>();
		foreach (UIDraggableListItem item in list)
		{
			SWUIDraggableItem sWUIDraggableItem = new SWUIDraggableItem();
			sWUIDraggableItem.UIDraggableListItem = item;
			sWUIDraggableItem.Sprites = item.gameObject.GetComponentsInChildren<UISprite>(true);
			float num = 0f;
			UISprite[] sprites = sWUIDraggableItem.Sprites;
			foreach (UISprite uISprite in sprites)
			{
				if (uISprite.transform.localScale.x > num)
				{
					num = uISprite.transform.localScale.x;
				}
			}
			sWUIDraggableItem.SpriteWidth = num;
			list2.Add(sWUIDraggableItem);
		}
		return list2;
	}
}
