using UnityEngine;

public class MantisBoomerangScript : MonoBehaviour
{
	public int attackDamage = 5;

	public Transform target;

	public Vector3 risingSpeed;

	public float attackSpeedValue;

	public float angularVelocity;

	public float risingTime;

	private float startTime;

	private Vector3 targetPos;

	private bool isLock;

	private void Start()
	{
		startTime = Time.time;
		isLock = false;
	}

	private void Update()
	{
		if (Time.time - startTime > risingTime && !isLock)
		{
			isLock = true;
			targetPos = target.position;
			targetPos.y += 0.5f;
		}
		if (isLock)
		{
			Vector3 to = targetPos - base.transform.position;
			to.Normalize();
			to *= attackSpeedValue;
			risingSpeed = Vector3.Slerp(risingSpeed, to, Time.deltaTime * angularVelocity);
		}
		base.transform.Translate(risingSpeed * Time.deltaTime, Space.World);
		AudioManager.GetInstance().PlaySoundSingleAt("Audio/enemy/Mantis/feixingtanglang_fly_attack_03", base.transform.position);
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.gameObject.layer == PhysicsLayer.PLAYER || other.gameObject.layer == PhysicsLayer.REMOTE_PLAYER || other.gameObject.layer == PhysicsLayer.FLOOR || other.gameObject.layer == PhysicsLayer.WALL || other.gameObject.layer == PhysicsLayer.TRANSPARENT_WALL) && other.gameObject.tag != TagName.BOSS_DEFENT)
		{
			Object.DestroyObject(base.gameObject);
			Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
			if (other.gameObject.layer == PhysicsLayer.PLAYER)
			{
				player.OnHit(attackDamage);
			}
		}
	}
}
