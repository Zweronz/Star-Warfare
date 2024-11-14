using UnityEngine;

public class EnemyShotScript : MonoBehaviour
{
	public Vector3 speed;

	public float explodeRadius = 8f;

	public int attackDamage = 5;

	public int areaDamage = 5;

	public TrajectoryType trType;

	public DamageType damageType;

	public EnemyType enemyType;

	public string explodeEffect;

	private void Start()
	{
	}

	private void Update()
	{
		if (trType == TrajectoryType.Parabola)
		{
			speed += Physics.gravity.y * Vector3.up * Time.deltaTime;
			base.transform.Translate(speed * Time.deltaTime, Space.World);
			base.transform.LookAt(base.transform.position + speed * 10f);
		}
		else if (trType == TrajectoryType.Straight)
		{
			base.transform.Translate(speed * Time.deltaTime, Space.World);
			base.transform.LookAt(base.transform.position + speed * 10f);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != PhysicsLayer.PLAYER && other.gameObject.layer != PhysicsLayer.REMOTE_PLAYER && other.gameObject.layer != PhysicsLayer.FLOOR && other.gameObject.layer != PhysicsLayer.WALL && other.gameObject.layer != PhysicsLayer.TRANSPARENT_WALL)
		{
			return;
		}
		Object.DestroyObject(base.gameObject);
		string path = "Effect/bug_3_bom";
		Vector3 position = base.gameObject.transform.position;
		if (enemyType == EnemyType.Reaver)
		{
			path = "Effect/Reaver/exp";
			if (other.gameObject.layer == PhysicsLayer.FLOOR)
			{
				position += Vector3.down;
			}
		}
		else if (enemyType == EnemyType.Mutalisk)
		{
			path = "Effect/Mutalisk/bug_04_bow";
		}
		GameObject original = Resources.Load(path) as GameObject;
		Object.Instantiate(original, position, Quaternion.identity);
		Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
		if (other.gameObject.layer == PhysicsLayer.PLAYER && (damageType == DamageType.Normal || damageType == DamageType.Sputtering))
		{
			player.OnHit(attackDamage);
		}
		if (damageType != DamageType.Sputtering && damageType != DamageType.Explosion)
		{
			return;
		}
		if (damageType == DamageType.Explosion)
		{
			if (explodeEffect != null && string.Empty != explodeEffect)
			{
				GameObject original2 = Resources.Load(explodeEffect) as GameObject;
				Object.Instantiate(original2, base.transform.position, Quaternion.identity);
			}
			AudioManager.GetInstance().PlaySoundAt("Audio/rpg/rpg-21_boom", base.transform.position);
		}
		if (!((player.GetTransform().position - base.transform.position).sqrMagnitude < explodeRadius * explodeRadius))
		{
			return;
		}
		if (enemyType == EnemyType.Dragon)
		{
			player.OnHit(areaDamage);
			return;
		}
		Ray ray = new Ray(base.transform.position, player.GetTransform().position - base.transform.position);
		float distance = Mathf.Sqrt((base.transform.position - player.GetTransform().position).sqrMagnitude);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER)) && hitInfo.collider.gameObject.layer == PhysicsLayer.PLAYER)
		{
			player.OnHit(areaDamage);
		}
	}
}
