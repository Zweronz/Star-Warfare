using UnityEngine;

public class MoveAnimationScript : MonoBehaviour
{
	public Vector3 startPosition;

	public Vector3 endPosition;

	public float time;

	public string colorPropertyName = "_TintColor";

	private void Start()
	{
		base.transform.position = startPosition;
	}

	private void Update()
	{
		time += Time.deltaTime;
		if (time > 2f)
		{
			time -= 2f;
		}
		base.transform.position = Vector3.Lerp(startPosition, endPosition, time * 0.5f);
		if (Vector3.Distance(base.transform.position, endPosition) < 2f)
		{
			base.transform.position = startPosition;
		}
		Color white = Color.white;
		white.a = (1f - Mathf.Abs(1f - time)) * 1.2f;
		if (white.a > 1f)
		{
			white.a = 1f;
		}
		base.GetComponent<Renderer>().material.SetColor(colorPropertyName, white);
	}
}
