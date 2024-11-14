using UnityEngine;

public class RocketScript : MonoBehaviour
{
	public Transform beginPos;

	public Transform endPos;

	public float speed;

	protected float startTime;

	private void Start()
	{
		startTime = Time.time;
		base.transform.position = beginPos.position;
		base.transform.up = endPos.position - beginPos.position;
	}

	private void Update()
	{
		base.transform.Translate(speed * (endPos.position - beginPos.position).normalized * Time.deltaTime, Space.World);
		base.transform.LookAt(endPos);
		if ((base.transform.position - endPos.position).magnitude < 1f)
		{
			Object.Destroy(base.gameObject);
			GameObject original = Resources.Load("Effect/RPG_EXP") as GameObject;
			Object.Instantiate(original, base.transform.position, Quaternion.identity);
		}
	}
}
