using UnityEngine;

public class FireLineScript : MonoBehaviour
{
	public Vector3 beginPos;

	public Vector3 endPos;

	public float speed;

	protected float startTime;

	protected float deltaTime;

	private void Start()
	{
		startTime = Time.time;
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			speed *= 2f;
		}
	}

	private void Update()
	{
		deltaTime += Time.deltaTime;
		if (!(deltaTime < 0.03f))
		{
			base.transform.Translate(speed * (endPos - beginPos).normalized * deltaTime, Space.World);
			if ((base.transform.position - endPos).magnitude < 1f)
			{
				base.gameObject.SetActiveRecursively(false);
			}
			deltaTime = 0f;
		}
	}
}
