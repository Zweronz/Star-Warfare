using UnityEngine;

public class IK1JointAnalytic : IKSolver
{
	public override void Solve(Transform[] bones, Vector3 target)
	{
		Transform transform = bones[0];
		Transform transform2 = bones[1];
		Transform transform3 = bones[2];
		Vector3 vector = Vector3.Cross(transform3.position - transform.position, Vector3.Cross(transform3.position - transform.position, transform3.position - transform2.position));
		float magnitude = (transform2.position - transform.position).magnitude;
		float magnitude2 = (transform3.position - transform2.position).magnitude;
		Vector3 position = transform.position;
		Vector3 vector2 = findKnee(position, target, magnitude, magnitude2, vector);
		Quaternion quaternion = Quaternion.FromToRotation(transform2.position - transform.position, vector2 - position) * transform.rotation;
		if (float.IsNaN(quaternion.x))
		{
			Debug.LogWarning(string.Concat("hipRot=", quaternion, " pHip=", position, " pAnkle=", target, " fThighLength=", magnitude, " fShinLength=", magnitude2, " vKneeDir=", vector));
		}
		else
		{
			transform.rotation = quaternion;
			transform2.rotation = Quaternion.FromToRotation(transform3.position - transform2.position, target - vector2) * transform2.rotation;
		}
	}

	public Vector3 findKnee(Vector3 pHip, Vector3 pAnkle, float fThigh, float fShin, Vector3 vKneeDir)
	{
		Vector3 vector = pAnkle - pHip;
		float num = vector.magnitude;
		float num2 = (fThigh + fShin) * 0.999f;
		if (num > num2)
		{
			pAnkle = pHip + vector.normalized * num2;
			vector = pAnkle - pHip;
			num = num2;
		}
		float num3 = Mathf.Abs(fThigh - fShin) * 1.001f;
		if (num < num3)
		{
			pAnkle = pHip + vector.normalized * num3;
			vector = pAnkle - pHip;
			num = num3;
		}
		float num4 = (num * num + fThigh * fThigh - fShin * fShin) / 2f / num;
		float num5 = Mathf.Sqrt(fThigh * fThigh - num4 * num4);
		Vector3 vector2 = Vector3.Cross(vector, Vector3.Cross(vKneeDir, vector));
		return pHip + num4 * vector.normalized + num5 * vector2.normalized;
	}
}
