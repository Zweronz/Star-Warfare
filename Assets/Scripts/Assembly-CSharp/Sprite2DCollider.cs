using UnityEngine;

public class Sprite2DCollider
{
	public static bool Collide(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		if (sprite1.GlobalCollideType == Sprite2D.CollideType.Null || sprite2.GlobalCollideType == Sprite2D.CollideType.Null)
		{
			return false;
		}
		float a;
		float a2;
		float a3;
		float a4;
		if (sprite1.WorldRotation == 0f)
		{
			Vector2 worldPosition = sprite1.WorldPosition;
			Vector2 size = sprite1.Size;
			Vector2 vector = size / 2f;
			a = worldPosition.x - vector.x;
			a2 = worldPosition.y - vector.y;
			a3 = worldPosition.x + vector.x;
			a4 = worldPosition.y + vector.y;
		}
		else
		{
			Vector2 worldPosition2 = sprite1.WorldPosition;
			Vector2 size2 = sprite1.Size;
			float num = ((!(size2.x >= size2.y)) ? size2.y : size2.x) * 0.75f;
			a = worldPosition2.x - num;
			a2 = worldPosition2.y - num;
			a3 = worldPosition2.x + num;
			a4 = worldPosition2.y + num;
		}
		float b = 0f;
		float b2 = 0f;
		float b3 = 0f;
		float b4 = 0f;
		if (sprite2.WorldRotation == 0f)
		{
			Vector2 worldPosition3 = sprite2.WorldPosition;
			Vector2 size3 = sprite2.Size;
			Vector2 vector2 = size3 / 2f;
			b = worldPosition3.x - vector2.x;
			b2 = worldPosition3.y - vector2.y;
			b3 = worldPosition3.x + vector2.x;
			b4 = worldPosition3.y + vector2.y;
		}
		else
		{
			Vector2 worldPosition4 = sprite2.WorldPosition;
			Vector2 size4 = sprite2.Size;
			float num2 = ((!(size4.x >= size4.y)) ? size4.y : size4.x) * 0.75f;
			a = worldPosition4.x - num2;
			a2 = worldPosition4.y - num2;
			a3 = worldPosition4.x + num2;
			a4 = worldPosition4.y + num2;
		}
		float num3 = Mathf.Max(a, b);
		float num4 = Mathf.Min(a3, b3);
		if (num3 > num4)
		{
			return false;
		}
		float num5 = Mathf.Max(a2, b2);
		float num6 = Mathf.Min(a4, b4);
		if (num5 > num6)
		{
			return false;
		}
		switch (sprite1.GlobalCollideType)
		{
		case Sprite2D.CollideType.Circle:
			switch (sprite2.GlobalCollideType)
			{
			case Sprite2D.CollideType.Circle:
				return Circle2Circle(sprite1, sprite2, ref collide_position);
			case Sprite2D.CollideType.Rectangle:
				return Circle2Rectangle(sprite1, sprite2, ref collide_position);
			case Sprite2D.CollideType.Polygon:
				return Circle2Polygon(sprite1, sprite2, ref collide_position);
			}
			break;
		case Sprite2D.CollideType.Rectangle:
			switch (sprite2.GlobalCollideType)
			{
			case Sprite2D.CollideType.Circle:
				return Circle2Rectangle(sprite2, sprite1, ref collide_position);
			case Sprite2D.CollideType.Rectangle:
				return Rectangle2Rectangle(sprite1, sprite2, ref collide_position);
			case Sprite2D.CollideType.Polygon:
				return Rectangle2Polygon(sprite1, sprite2, ref collide_position);
			}
			break;
		case Sprite2D.CollideType.Polygon:
			switch (sprite2.GlobalCollideType)
			{
			case Sprite2D.CollideType.Circle:
				return Circle2Polygon(sprite2, sprite1, ref collide_position);
			case Sprite2D.CollideType.Rectangle:
				return Rectangle2Polygon(sprite2, sprite1, ref collide_position);
			case Sprite2D.CollideType.Polygon:
				return Polygon2Polygon(sprite1, sprite2, ref collide_position);
			}
			break;
		}
		return false;
	}

	private static bool Circle2Circle(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}

	private static bool Circle2Rectangle(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}

	private static bool Circle2Polygon(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}

	private static bool Rectangle2Rectangle(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}

	private static bool Rectangle2Polygon(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}

	private static bool Polygon2Polygon(Sprite2D sprite1, Sprite2D sprite2, ref Vector2 collide_position)
	{
		return false;
	}
}
