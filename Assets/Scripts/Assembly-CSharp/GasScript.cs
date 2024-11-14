using UnityEngine;

public class GasScript : MonoBehaviour
{
	public int damage;

	public Player targetPlayer;

	public Enemy enemy;

	private Timer mPlayerHitTimer = new Timer();

	private float startTime;

	private void OnEnable()
	{
		mPlayerHitTimer.SetTimer(0.2f, true);
	}

	private void OnDisable()
	{
		targetPlayer = null;
		enemy = null;
	}

	private void OnTriggerStay(Collider other)
	{
		if (enemy != null && other.gameObject.layer == PhysicsLayer.PLAYER && targetPlayer != null && targetPlayer.GetTransform() != null && mPlayerHitTimer.Ready())
		{
			mPlayerHitTimer.Do();
			targetPlayer.OnHit(damage);
		}
	}
}
