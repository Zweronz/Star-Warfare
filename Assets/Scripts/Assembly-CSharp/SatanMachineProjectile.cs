using UnityEngine;

public class SatanMachineProjectile : MonoBehaviour
{
	public Enemy enemy;

	public Player targetPlayer;

	public float explodeRadius;

	public float life = 2f;

	public int damage;

	protected Timer explodeTimer;

	private void Awake()
	{
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	private void Update()
	{
		if (explodeTimer != null && explodeTimer.Ready())
		{
			Explode();
		}
	}

	private void Explode()
	{
		OnExplode();
		if (IsToDestroy())
		{
			Object.Destroy(base.gameObject.transform.parent.gameObject);
		}
		else
		{
			base.gameObject.transform.parent.gameObject.SetActive(false);
		}
		if (enemy == null)
		{
			return;
		}
		Collider[] array = Physics.OverlapSphere(base.transform.position, explodeRadius, 1 << PhysicsLayer.PLAYER);
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			if (collider.gameObject.layer == PhysicsLayer.PLAYER)
			{
				targetPlayer.OnHit(damage);
			}
		}
		targetPlayer = null;
		enemy = null;
		explodeTimer = null;
	}

	protected virtual void OnExplode()
	{
	}

	protected virtual bool IsToDestroy()
	{
		return true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == PhysicsLayer.FLOOR)
		{
			if (explodeTimer == null)
			{
				explodeTimer = new Timer();
				float interval = Random.Range(1, 3);
				explodeTimer.SetTimer(interval, false);
			}
			OnTouchGround();
		}
		else if (other.gameObject.layer == PhysicsLayer.PLAYER || other.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
		{
			Explode();
		}
	}

	protected virtual void OnTouchGround()
	{
	}
}
