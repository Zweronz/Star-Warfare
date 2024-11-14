using UnityEngine;

public class TrackingMissileScript : MonoBehaviour
{
	public int explosionDamage = 5;

	public float explosionRadius = 5f;

	public float speedValue = 5f;

	public float rotateSpeedValue = 1f;

	public Vector3 targetPosition;

	public Player trackingPlayer;

	public Player localPlayer;

	public float trackingFlyAcceleration;

	public Vector3 initDir = Vector3.zero;

	public float duration = 7f;

	private float mStartTime;

	private bool destroyed;

	private Vector3 mTargetDir;

	public void Start()
	{
		base.transform.LookAt(base.transform.position + initDir);
		mStartTime = Time.time;
	}

	public void Update()
	{
		if (trackingPlayer != null)
		{
			Transform transform = trackingPlayer.GetTransform();
			if (transform != null && trackingPlayer.InPlayingState())
			{
				targetPosition = trackingPlayer.GetTransform().position + Vector3.up * (duration - (Time.time - mStartTime)) / duration;
				mTargetDir = (targetPosition - base.transform.position).normalized;
			}
		}
		Move();
	}

	private void Move()
	{
		Vector3 vector = Vector3.RotateTowards(base.transform.forward, mTargetDir, rotateSpeedValue * Time.deltaTime, 0f);
		base.transform.LookAt(base.transform.position + vector);
		speedValue += trackingFlyAcceleration * Time.deltaTime;
		base.transform.Translate(base.transform.forward * speedValue * Time.deltaTime, Space.World);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!destroyed)
		{
			Explode();
		}
	}

	private void Explode()
	{
		GameObject original = Resources.Load("Effect/update_effect/effect_explosion_001") as GameObject;
		Object.Instantiate(original, base.transform.position, Quaternion.identity);
		AudioManager.GetInstance().PlaySoundAt("Audio/gl/grenade_launcher_boom", base.transform.position);
		Object.DestroyObject(base.gameObject);
		destroyed = true;
		Collider[] array = Physics.OverlapSphere(base.transform.position, explosionRadius, 1 << PhysicsLayer.PLAYER);
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			if (collider.gameObject.layer == PhysicsLayer.PLAYER)
			{
				localPlayer.OnHit(explosionDamage);
			}
		}
	}
}
