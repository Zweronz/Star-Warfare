using UnityEngine;

public class MantisHookScript : MonoBehaviour
{
	public Vector3 speed;

	public int attackDamage = 5;

	private float speedValue;

	private void Start()
	{
		speedValue = speed.magnitude;
	}

	private void Update()
	{
		if (base.transform.position.y < (float)Global.FLOORHEIGHT + 2f)
		{
			speed.y = 0f;
			speed.Normalize();
			speed *= speedValue;
		}
		base.transform.Translate(speed * Time.deltaTime, Space.World);
		base.transform.LookAt(base.transform.position + speed * 10f);
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
