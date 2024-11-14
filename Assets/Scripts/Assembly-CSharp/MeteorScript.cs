using UnityEngine;

public class MeteorScript : MonoBehaviour
{
	public Transform beginPos;

	public Transform endPos;

	public float speed;

	protected float startTime;

	protected float deltaTime;

	private void Start()
	{
		startTime = Time.time;
		base.transform.position = beginPos.position;
		base.transform.up = endPos.position - beginPos.position;
	}

	private void Update()
	{
		deltaTime += Time.deltaTime;
		if (!(deltaTime < 0.03f))
		{
			base.transform.Translate(speed * (endPos.position - beginPos.position).normalized * deltaTime, Space.World);
			if ((base.transform.position - endPos.position).magnitude < 1f)
			{
				Object.Destroy(base.gameObject);
			}
			deltaTime = 0f;
		}
	}
}
