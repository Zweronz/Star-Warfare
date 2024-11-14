using UnityEngine;

public class MantisLaserScript : MonoBehaviour
{
	public Transform mouthTransform;

	public int attackDamage;

	public float speed;

	private Timer laserTimer = new Timer();

	public void Start()
	{
		laserTimer.SetTimer(0.2f, false);
	}

	public void Update()
	{
		base.transform.position = mouthTransform.position;
		base.transform.Translate(0.1f * Vector3.down);
		base.transform.RotateAround(base.transform.position, Vector3.up, speed * Time.deltaTime);
		if (base.transform.forward.y < 0f && base.transform.position.y > 0.1f)
		{
			Vector3 position = base.transform.position + base.transform.forward * (0.1f - base.transform.position.y) / base.transform.forward.y;
			GameObject original = Resources.Load("Effect/LaserHit") as GameObject;
			GameObject gameObject = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (laserTimer.Ready() && other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
			player.OnHit(attackDamage);
			laserTimer.Do();
		}
	}
}
