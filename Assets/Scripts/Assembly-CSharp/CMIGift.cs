using UnityEngine;

public class CMIGift
{
	protected GameObject m_giftObject;

	protected int m_hp;

	protected CMIGiftType m_type;

	protected int m_id;

	protected bool m_active;

	protected byte m_pointId;

	private float m_loopTime;

	protected GameObject m_effectObj;

	protected bool m_playEffect;

	public CMIGift()
	{
	}

	public CMIGift(int id, CMIGiftType type)
	{
		m_id = id;
		m_type = type;
	}

	public int GetId()
	{
		return m_id;
	}

	public void SetId(int id)
	{
		m_id = id;
	}

	public void SetType(CMIGiftType type)
	{
		m_type = type;
	}

	public new CMIGiftType GetType()
	{
		return m_type;
	}

	public void CreateObj(GameObject prefab, string name)
	{
		m_giftObject = Object.Instantiate(prefab) as GameObject;
		m_giftObject.name = name;
		m_effectObj = m_giftObject.transform.FindChild(BoneName.GiftEffectPoint).gameObject;
	}

	public void CreateObj(GameObject prefab, Vector3 pos, string name)
	{
		m_giftObject = Object.Instantiate(prefab) as GameObject;
		m_giftObject.transform.position = pos;
		m_giftObject.name = name;
		m_effectObj = m_giftObject.transform.FindChild(BoneName.GiftEffectPoint).gameObject;
	}

	public void Loop(float deltaTime)
	{
		m_loopTime += deltaTime;
		if (!(m_loopTime >= 1f))
		{
			return;
		}
		m_loopTime = 0f;
		if (m_active && m_giftObject != null)
		{
			int num = Random.Range(0, 10);
			if (num >= 9)
			{
				PlayAnimation("jump02", WrapMode.ClampForever);
			}
			else if (AnimationPlayed("jump02", 1f) || (m_giftObject != null && !m_giftObject.animation.isPlaying))
			{
				PlayAnimation("jump01", WrapMode.Loop);
			}
		}
	}

	private bool AnimationPlayed(string name, float percent)
	{
		if (m_giftObject == null || m_giftObject.animation[name] == null)
		{
			return true;
		}
		if (m_giftObject.animation[name].time >= m_giftObject.animation[name].clip.length * percent)
		{
			return true;
		}
		return false;
	}

	private void PlayAnimation(string name, WrapMode mode)
	{
		if (!(m_giftObject == null) && !(m_giftObject.animation[name] == null) && !m_giftObject.animation.IsPlaying(name))
		{
			m_giftObject.animation[name].wrapMode = mode;
			m_giftObject.animation.CrossFade(name);
		}
	}

	public Vector3 GetPosition()
	{
		if (m_giftObject != null && m_giftObject.transform != null)
		{
			return m_giftObject.transform.GetChild(0).position;
		}
		return Vector3.zero;
	}

	public Vector3 GetPositionEffect()
	{
		if (m_effectObj != null)
		{
			return m_effectObj.transform.position;
		}
		return Vector3.zero;
	}

	public Transform GetTransform()
	{
		if (m_giftObject != null && m_giftObject.transform != null)
		{
			return m_giftObject.transform.GetChild(0);
		}
		return null;
	}

	public void SetPosition(Vector3 pos)
	{
		if (m_giftObject != null)
		{
			m_giftObject.transform.position = pos;
		}
	}

	public void DestroyObj()
	{
		if (m_giftObject != null)
		{
			Object.Destroy(m_giftObject);
		}
	}

	public bool IsSpecialGift()
	{
		if (m_type == CMIGiftType.TYPE_RANDOM_POSITIVE || m_type == CMIGiftType.TYPE_RANDOM_NEGATIVE)
		{
			return true;
		}
		return false;
	}

	public void SetActive(bool IsActive)
	{
		m_active = IsActive;
		if (m_giftObject != null)
		{
			m_giftObject.SetActive(IsActive);
		}
	}

	public bool GetActive()
	{
		if (m_giftObject == null)
		{
			return false;
		}
		return m_active;
	}

	public void SetPointId(byte pointId)
	{
		m_pointId = pointId;
	}

	public byte GetPointId()
	{
		return m_pointId;
	}

	public void SetHp(int hp)
	{
		m_hp = hp;
	}

	public int GetHp()
	{
		return m_hp;
	}

	public static int GetIdByName(string name)
	{
		string s = name.Remove(0, 2);
		Debug.Log("name: " + name);
		return int.Parse(s);
	}

	public int GetScore()
	{
		int result = 0;
		switch (m_type)
		{
		case CMIGiftType.TYPE_SMALL:
			result = CMIConfig.SCORE_SMALL;
			break;
		case CMIGiftType.TYPE_MIDDLE:
			result = CMIConfig.SCORE_MIDDLE;
			break;
		case CMIGiftType.TYPE_LARGE:
			result = CMIConfig.SCORE_LARGE;
			break;
		case CMIGiftType.TYPE_RANDOM_POSITIVE:
			result = CMIConfig.SCORE_RANDOM_POSITIVE;
			break;
		case CMIGiftType.TYPE_RANDOM_NEGATIVE:
			result = CMIConfig.SCORE_RANDOM_NEGATIVE;
			break;
		}
		return result;
	}
}
