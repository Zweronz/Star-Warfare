using UnityEngine;

public class AutoDestroyScript : MonoBehaviour
{
	public float life;

	protected float createdTime;

	private void Start()
	{
		createdTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - createdTime > life)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
