using UnityEngine;

public class AimArm
{
	public Transform mShoulder;

	public Transform mElbow;

	public Transform mHand;

	public Matrix4x4 mHandRelativeToWeapon;

	public AimArm(Transform shoulder, Transform elbow, Transform hand)
	{
		mShoulder = shoulder;
		mElbow = elbow;
		mHand = hand;
	}
}
