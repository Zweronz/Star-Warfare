using UnityEngine;

public class PopupScript : MonoBehaviour, UIHandler
{
	private float startTime;

	public UIManager m_UIManager;

	protected Timer fadeTimer = new Timer();

	private void Awake()
	{
		m_UIManager = base.gameObject.AddComponent<UIManager>() as UIManager;
		m_UIManager.SetParameter(24, 3, false);
		m_UIManager.SetUIHandler(this);
	}

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.position = Vector3.up * 100000f;
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			if (!(m_UIManager != null) || m_UIManager.HandleInput(touch))
			{
			}
		}
	}

	public void FrFree()
	{
		m_UIManager.RemoveAll();
	}

	public void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
	}
}
