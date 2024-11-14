using UnityEngine;

public abstract class IKSolver
{
	public float positionAccuracy = 0.001f;

	public abstract void Solve(Transform[] bones, Vector3 target);
}
