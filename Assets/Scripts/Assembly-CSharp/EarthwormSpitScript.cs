using UnityEngine;

public class EarthwormSpitScript : MonoBehaviour
{
	public Vector3 speed;

	public int spitDamage = 5;

	public int slimeDamage = 1;

	public float slimeDisappearTime = 5f;

	public float maxSlimeScale = 1f;

	public float slowEffect = 1f;

	public Timer slimeTimer;

	private void Start()
	{
	}

	private void Update()
	{
		speed += Physics.gravity.y * Vector3.up * Time.deltaTime;
		base.transform.Translate(speed * Time.deltaTime, Space.World);
		base.transform.LookAt(base.transform.position + speed * 10f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == PhysicsLayer.PLAYER || other.gameObject.layer == PhysicsLayer.REMOTE_PLAYER || other.gameObject.layer == PhysicsLayer.FLOOR || other.gameObject.layer == PhysicsLayer.WALL || other.gameObject.layer == PhysicsLayer.TRANSPARENT_WALL)
		{
			Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
			if (other.gameObject.layer == PhysicsLayer.PLAYER)
			{
				player.OnHit(spitDamage);
			}
			Vector3 position = base.transform.position;
			position.y = 0.05f;
			Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			GameObject original = Resources.Load("Effect/update_effect/efffect_worm_Poison_ground_001") as GameObject;
			GameObject gameObject = Object.Instantiate(original, position, rotation) as GameObject;
			EarthwormSlimeScript component = gameObject.GetComponent<EarthwormSlimeScript>();
			component.slimeDamage = slimeDamage;
			component.disappearTime = slimeDisappearTime;
			component.maxScale = maxSlimeScale;
			component.slimeTimer = slimeTimer;
			component.slowEffect = slowEffect;
			Object.DestroyObject(base.gameObject);
		}
	}
}
