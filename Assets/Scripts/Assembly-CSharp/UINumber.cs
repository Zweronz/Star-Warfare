using UnityEngine;

public class UINumber : MonoBehaviour
{
	private const float LIFE_TIME = 1.3f;

	private const float MAX_DISTANCE = 80f;

	private const int MIN_FONT_SIZE = 48;

	private const int MAX_FONT_SIZE = 512;

	public UILabel m_Label;

	private float startTime;

	private void Start()
	{
		base.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		Init();
	}

	private void Init()
	{
		float value = Vector3.Distance(Camera.main.transform.position, base.gameObject.transform.position);
		value = Mathf.Clamp(value, 0f, 80f);
		int num = (int)(48f + 464f * (value / 80f));
		m_Label.transform.localScale = new Vector3(num, num, 1f);
		TweenPosition component = m_Label.gameObject.GetComponent<TweenPosition>();
		if (component != null)
		{
			float x = Random.Range(-10, 10) * 15;
			component.from = new Vector3(x, component.from.y, component.from.z);
			component.to = new Vector3(x, component.to.y, component.to.z);
		}
		startTime = Time.time;
	}

	private void Update()
	{
		base.gameObject.transform.LookAt(Camera.main.transform);
		if (Time.time - startTime > 1.3f)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void SetData(Vector3 pos, string str)
	{
		m_Label.text = str;
		base.gameObject.transform.position = pos;
	}
}
