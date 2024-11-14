using UnityEngine;

public class GrenadePhysicsScript : MonoBehaviour
{
	protected Transform proTransform;

	public float life;

	public Vector3 dir;

	protected float createdTime;

	public void Start()
	{
		proTransform = base.transform;
		createdTime = Time.time;
		float num = 15f;
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			num *= VSMath.GL_FLY_BOOTH;
		}
		base.rigidbody.AddForce(dir * num, ForceMode.Impulse);
	}

	public void Update()
	{
		if (Time.time - createdTime > life)
		{
			Object.DestroyObject(base.gameObject);
		}
	}
}
