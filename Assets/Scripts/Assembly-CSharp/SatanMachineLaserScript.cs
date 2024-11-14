using UnityEngine;

public class SatanMachineLaserScript : MonoBehaviour
{
	public int mDamage;

	public Enemy mEnemy;

	private Player mTargetPlayer;

	private Timer mPlayerHitTimer = new Timer();

	private Vector3 mTargetPos;

	public void SetTargetPlayer(Player target, Vector3 targetPos)
	{
		mTargetPlayer = target;
		mTargetPos = targetPos;
	}

	private void Start()
	{
		mPlayerHitTimer.SetTimer(0.3f, true);
		base.transform.parent.parent.LookAt(mTargetPos);
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		Hit(other);
	}

	private void OnTriggerStay(Collider other)
	{
		Hit(other);
	}

	private void Hit(Collider other)
	{
		if (mEnemy != null && other.gameObject.layer == PhysicsLayer.PLAYER && mTargetPlayer != null && mTargetPlayer.GetTransform() != null && mPlayerHitTimer.Ready())
		{
			mPlayerHitTimer.Do();
			mTargetPlayer.OnHit(mDamage);
		}
	}
}
