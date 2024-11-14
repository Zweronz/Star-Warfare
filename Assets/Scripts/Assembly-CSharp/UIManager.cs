using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour, UIContainer
{
	public int LAYER;

	public int DEPTH;

	public bool CLEAR;

	private UIMesh m_UIMesh;

	private SpriteCamera m_SpriteCamera;

	private UIHandler m_UIHandler;

	private ArrayList m_Controls;

	private bool m_Enable;

	public UIManager()
	{
		m_UIMesh = null;
		m_SpriteCamera = null;
		m_UIHandler = null;
		m_Controls = new ArrayList();
		m_Enable = true;
	}

	public void SetUIHandler(UIHandler ui_handler)
	{
		m_UIHandler = ui_handler;
	}

	public void Add(UIControl control)
	{
		m_Controls.Add(control);
		control.SetParent(this);
	}

	public void Remove(UIControl control)
	{
		m_Controls.Remove(control);
	}

	public void RemoveAll()
	{
		m_Controls.Clear();
	}

	public void Destory()
	{
		m_UIMesh.Destory();
	}

	public ArrayList GetControls()
	{
		return m_Controls;
	}

	public void SetEnable(bool enable)
	{
		m_Enable = enable;
	}

	public bool HandleInput(UITouchInner touch)
	{
		if (!m_Enable)
		{
			return false;
		}
		for (int num = m_Controls.Count - 1; num >= 0; num--)
		{
			UIControl uIControl = (UIControl)m_Controls[num];
			if (uIControl.Enable && m_Enable && uIControl.HandleInput(touch))
			{
				return true;
			}
		}
		return false;
	}

	public void Awake()
	{
	}

	public void SetParameter(int layer, int depth, bool clear)
	{
		LAYER = layer;
		DEPTH = depth;
		CLEAR = clear;
	}

	public void Start()
	{
		Initialize();
		InitializeSpriteMesh();
		InitializeSpriteCamera();
	}

	public void LateUpdate()
	{
		m_UIMesh.RemoveAll();
		for (int i = 0; i < m_Controls.Count; i++)
		{
			UIControl uIControl = (UIControl)m_Controls[i];
			uIControl.Update();
			if (uIControl.Visible)
			{
				uIControl.Draw();
			}
		}
		m_UIMesh.DoLateUpdate();
	}

	public void DrawSprite(UISpriteX sprite)
	{
		m_UIMesh.Add(sprite);
	}

	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (m_UIHandler != null)
		{
			m_UIHandler.HandleEvent(control, command, wparam, lparam);
		}
	}

	private void Initialize()
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
	}

	private void InitializeSpriteMesh()
	{
		GameObject gameObject = new GameObject("UIMesh");
		gameObject.transform.parent = base.gameObject.transform;
		m_UIMesh = (UIMesh)gameObject.AddComponent(typeof(UIMesh));
		m_UIMesh.Initialize(LAYER);
	}

	private void InitializeSpriteCamera()
	{
		GameObject gameObject = new GameObject("SpriteCamera");
		gameObject.transform.parent = base.gameObject.transform;
		m_SpriteCamera = (SpriteCamera)gameObject.AddComponent(typeof(SpriteCamera));
		m_SpriteCamera.Initialize(LAYER);
		m_SpriteCamera.SetClear(CLEAR);
		m_SpriteCamera.SetDepth(DEPTH);
		m_SpriteCamera.SetViewport(new Rect(0f, 0f, Screen.width, Screen.height));
	}
}
