using System.Collections.Generic;
using UnityEngine;

public class UI3DFrame : UIPanelX, UIHandler
{
	protected GameObject m_Model;

	protected UIMove m_UIMove;

	protected Vector3 m_Pos;

	protected Vector3 m_CenterOffset;

	protected Vector3 m_Rotation;

	protected List<GameObject> m_SubModel = new List<GameObject>();

	public Vector3 CenterOffset
	{
		get
		{
			return m_CenterOffset;
		}
		set
		{
			m_CenterOffset = value;
		}
	}

	public UI3DFrame(Rect rect, Vector3 pos, Vector3 rotation)
	{
		m_UIMove = new UIMove();
		m_UIMove.Rect = rect;
		m_Pos = pos;
		m_Rotation = rotation;
		m_SubModel.Clear();
		Add(m_UIMove);
		SetUIHandler(this);
	}

	public void AddSubModel(GameObject obj)
	{
		m_SubModel.Add(obj);
	}

	public List<GameObject> GetSubModel()
	{
		return m_SubModel;
	}

	public void SetModel(GameObject obj)
	{
		m_Model = obj;
		m_Model.transform.position = m_Pos;
	}

	public GameObject GetModel()
	{
		return m_Model;
	}

	public void SetPosition(Vector3 pos)
	{
		m_Model.transform.position = pos;
		m_Pos = pos;
	}

	public Vector3 GetPosition()
	{
		return m_Pos;
	}

	public void SetScale(Vector3 scale)
	{
		m_Model.transform.localScale = scale;
	}

	public void SetRotate(Vector3 rotate)
	{
		m_Rotation = rotate;
		m_Model.transform.rotation = Quaternion.Euler(rotate);
	}

	public void ClearModels()
	{
		if (m_Model != null)
		{
			Object.Destroy(m_Model);
			m_Model = null;
		}
	}

	public override void Show()
	{
		base.Show();
		m_UIMove.Enable = true;
		m_Model.SetActiveRecursively(true);
	}

	public override void Hide()
	{
		base.Hide();
		m_Model.SetActiveRecursively(false);
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (control == m_UIMove && command == 1 && m_Model != null)
		{
			Vector3 zero = Vector3.zero;
			zero.y = m_Rotation.y + (0f - wparam);
			SetRotate(zero);
		}
	}
}
