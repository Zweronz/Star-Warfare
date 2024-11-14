using UnityEngine;

public class Sprite2D : Sprite
{
	public enum CollideType
	{
		Null = 0,
		Circle = 1,
		Rectangle = 2,
		Polygon = 3
	}

	private Vector2 m_LocalPosition;

	private float m_LocalRotation;

	private Vector2 m_Velocity;

	private float m_AngularVelocity;

	private Sprite2D m_Parent;

	private bool m_MountPosition;

	private bool m_MountRotation;

	private CollideType m_LocalCollideType;

	private float[] m_LocalCollidePointers;

	private CollideType m_GlobalCollideType;

	private float[] m_GlobalCollidePointers;

	private bool m_UpdateCollideInfo;

	public new Vector2 Size
	{
		get
		{
			return base.Size;
		}
		set
		{
			base.Size = value;
		}
	}

	public new Vector2 Position
	{
		get
		{
			return m_LocalPosition;
		}
		set
		{
			m_LocalPosition = value;
			Update(0f);
		}
	}

	public Vector2 WorldPosition
	{
		get
		{
			if (m_Parent != null && m_MountPosition)
			{
				return m_Parent.WorldPosition + m_LocalPosition;
			}
			return m_LocalPosition;
		}
	}

	public new float Rotation
	{
		get
		{
			return m_LocalRotation;
		}
		set
		{
			m_LocalRotation = value;
			Update(0f);
		}
	}

	public float WorldRotation
	{
		get
		{
			if (m_Parent != null && m_MountRotation)
			{
				return m_Parent.WorldRotation + m_LocalRotation;
			}
			return m_LocalRotation;
		}
	}

	public Vector2 Velocity
	{
		get
		{
			return m_Velocity;
		}
		set
		{
			m_Velocity = value;
		}
	}

	public float AngularVelocity
	{
		get
		{
			return m_AngularVelocity;
		}
		set
		{
			m_AngularVelocity = value;
		}
	}

	public Sprite2D Parent
	{
		get
		{
			return m_Parent;
		}
		set
		{
			m_Parent = value;
			Update(0f);
		}
	}

	public bool MountPosition
	{
		get
		{
			return m_MountPosition;
		}
		set
		{
			m_MountPosition = value;
			Update(0f);
		}
	}

	public bool MountRotation
	{
		get
		{
			return m_MountRotation;
		}
		set
		{
			m_MountRotation = value;
			Update(0f);
		}
	}

	public CollideType LocalCollideType
	{
		get
		{
			return m_LocalCollideType;
		}
		set
		{
			m_LocalCollideType = value;
			m_UpdateCollideInfo = true;
		}
	}

	public float[] LocalCollidePointers
	{
		get
		{
			return m_LocalCollidePointers;
		}
		set
		{
			m_LocalCollidePointers = value;
			m_UpdateCollideInfo = true;
		}
	}

	public CollideType GlobalCollideType
	{
		get
		{
			if (m_UpdateCollideInfo)
			{
				CalculateCollideInfo();
			}
			return m_GlobalCollideType;
		}
		set
		{
			m_GlobalCollideType = value;
		}
	}

	public float[] GlobalCollidePointers
	{
		get
		{
			if (m_UpdateCollideInfo)
			{
				CalculateCollideInfo();
			}
			return m_GlobalCollidePointers;
		}
		set
		{
			m_GlobalCollidePointers = value;
		}
	}

	public Sprite2D()
	{
		m_LocalPosition = Vector2.zero;
		m_LocalRotation = 0f;
		m_Velocity = Vector2.zero;
		m_AngularVelocity = 0f;
		m_Parent = null;
		m_MountPosition = false;
		m_MountRotation = false;
		m_LocalCollideType = CollideType.Null;
		m_LocalCollidePointers = null;
		m_GlobalCollideType = CollideType.Null;
		m_GlobalCollidePointers = null;
		m_UpdateCollideInfo = true;
	}

	~Sprite2D()
	{
	}

	public bool Pick(Vector2 position)
	{
		float rotation = Rotation;
		if (rotation == 0f)
		{
			return PickNoRotation(position);
		}
		Vector2 worldPosition = WorldPosition;
		Vector2 vector = Size / 2f;
		float num = ((!(vector.x >= vector.y)) ? vector.y : vector.x) * 1.5f;
		if (position.x < worldPosition.x - num || position.x > worldPosition.x + num || position.y < worldPosition.y - num || position.y > worldPosition.y + num)
		{
			return false;
		}
		float num2 = Mathf.Sin(0f - rotation);
		float num3 = Mathf.Cos(0f - rotation);
		Vector2 vector2 = position - worldPosition;
		Vector2 vector3 = new Vector2(vector2.x * num3 - vector2.y * num2, vector2.x * num2 + vector2.y * num3);
		Vector2 vector4 = new Vector2(worldPosition.x + vector3.x, worldPosition.y + vector3.y);
		if (vector4.x < worldPosition.x - vector.x || vector4.x > worldPosition.x + vector.x || vector4.y < worldPosition.y - vector.y || vector4.y > worldPosition.y + vector.y)
		{
			return false;
		}
		return true;
	}

	public bool PickNoRotation(Vector2 position)
	{
		Vector2 worldPosition = WorldPosition;
		Vector2 vector = Size / 2f;
		if (position.x < worldPosition.x - vector.x || position.x > worldPosition.x + vector.x || position.y < worldPosition.y - vector.y || position.y > worldPosition.y + vector.y)
		{
			return false;
		}
		return true;
	}

	public virtual void Update(float delta_time)
	{
		m_LocalPosition += m_Velocity * delta_time;
		m_LocalRotation += m_AngularVelocity * delta_time;
		base.Position = WorldPosition;
		base.Rotation = WorldRotation;
		m_UpdateCollideInfo = true;
	}

	private void CalculateCollideInfo()
	{
		if (m_LocalCollideType == CollideType.Null)
		{
			m_GlobalCollideType = CollideType.Null;
			m_GlobalCollidePointers = null;
			m_UpdateCollideInfo = false;
			return;
		}
		Vector2 size = Size;
		Vector2 vector = size / 2f;
		Vector3 vector2 = WorldPosition;
		float worldRotation = WorldRotation;
		float num = 0f;
		float num2 = 1f;
		if (worldRotation == 0f)
		{
			num = Mathf.Sin(worldRotation);
			num2 = Mathf.Cos(worldRotation);
		}
		switch (m_LocalCollideType)
		{
		case CollideType.Circle:
			m_GlobalCollideType = CollideType.Circle;
			if (worldRotation == 0f)
			{
				m_GlobalCollidePointers = new float[3];
				m_GlobalCollidePointers[0] = vector2.x + m_LocalCollidePointers[0] * vector.x;
				m_GlobalCollidePointers[1] = vector2.y + m_LocalCollidePointers[1] * vector.y;
			}
			else
			{
				float num7 = m_LocalCollidePointers[0] * num2 - m_LocalCollidePointers[1] * num;
				float num8 = m_LocalCollidePointers[0] * num + m_LocalCollidePointers[1] * num2;
				m_GlobalCollidePointers = new float[3];
				m_GlobalCollidePointers[0] = vector2.x + num7 * vector.x;
				m_GlobalCollidePointers[1] = vector2.y + num8 * vector.y;
			}
			m_GlobalCollidePointers[2] = m_LocalCollidePointers[2] * ((!(vector.x <= vector.y)) ? vector.y : vector.x);
			break;
		case CollideType.Rectangle:
		{
			m_GlobalCollideType = CollideType.Rectangle;
			if (worldRotation == 0f)
			{
				m_GlobalCollidePointers = new float[4];
				m_GlobalCollidePointers[0] = vector2.x + m_LocalCollidePointers[0] * vector.x;
				m_GlobalCollidePointers[1] = vector2.y + m_LocalCollidePointers[1] * vector.y;
				m_GlobalCollidePointers[2] = vector2.x + m_LocalCollidePointers[2] * vector.x;
				m_GlobalCollidePointers[3] = vector2.y + m_LocalCollidePointers[3] * vector.y;
				break;
			}
			float[] array = new float[8]
			{
				m_LocalCollidePointers[0],
				m_LocalCollidePointers[1],
				m_LocalCollidePointers[2],
				m_LocalCollidePointers[1],
				m_LocalCollidePointers[2],
				m_LocalCollidePointers[3],
				m_LocalCollidePointers[0],
				m_LocalCollidePointers[3]
			};
			for (int k = 0; k < 4; k++)
			{
				float num9 = array[k * 2] * num2 - array[k * 2 + 1] * num;
				float num10 = array[k * 2] * num + array[k * 2 + 1] * num2;
				m_GlobalCollidePointers[k * 2] = vector2.x + num9 * vector.x;
				m_GlobalCollidePointers[k * 2 + 1] = vector2.y + num10 * vector.y;
			}
			break;
		}
		case CollideType.Polygon:
		{
			m_GlobalCollideType = CollideType.Polygon;
			if (worldRotation == 0f)
			{
				int num3 = m_LocalCollidePointers.Length / 2;
				m_GlobalCollidePointers = new float[num3 * 2];
				for (int i = 0; i < num3; i++)
				{
					m_GlobalCollidePointers[i * 2] = vector2.x + m_LocalCollidePointers[i * 2] * vector.x;
					m_GlobalCollidePointers[i * 2 + 1] = vector2.y + m_LocalCollidePointers[i * 2 + 1] * vector.y;
				}
				break;
			}
			int num4 = m_LocalCollidePointers.Length / 2;
			m_GlobalCollidePointers = new float[num4 * 2];
			for (int j = 0; j < num4; j++)
			{
				float num5 = m_LocalCollidePointers[j * 2] * num2 - m_LocalCollidePointers[j * 2 + 1] * num;
				float num6 = m_LocalCollidePointers[j * 2] * num + m_LocalCollidePointers[j * 2 + 1] * num2;
				m_GlobalCollidePointers[j * 2] = vector2.x + num5 * vector.x;
				m_GlobalCollidePointers[j * 2 + 1] = vector2.y + num6 * vector.y;
			}
			break;
		}
		}
		m_UpdateCollideInfo = false;
	}
}
