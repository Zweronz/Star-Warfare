using UnityEngine;

public class AutoDisableScript : MonoBehaviour
{
	public float life;

	protected float createdTime;

	private void OnEnable()
	{
		createdTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - createdTime > life)
		{
			base.gameObject.SetActive(false);
		}
	}
}
