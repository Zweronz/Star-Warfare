using UnityEngine;

public class AutoStopEmitScript : MonoBehaviour
{
	public float life = 2f;

	protected float createdTime;

	private void Start()
	{
		createdTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - createdTime > life && base.particleEmitter != null)
		{
			base.particleEmitter.emit = false;
		}
	}
}
