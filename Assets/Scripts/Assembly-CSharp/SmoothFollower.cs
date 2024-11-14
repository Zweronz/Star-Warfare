using UnityEngine;

public class SmoothFollower
{
	private Vector3 targetPosition;

	private Vector3 position;

	private Vector3 velocity;

	private float smoothingTime;

	private float prediction;

	public SmoothFollower(float smoothingTime)
	{
		targetPosition = Vector3.zero;
		position = Vector3.zero;
		velocity = Vector3.zero;
		this.smoothingTime = smoothingTime;
		prediction = 1f;
	}

	public SmoothFollower(float smoothingTime, float prediction)
	{
		targetPosition = Vector3.zero;
		position = Vector3.zero;
		velocity = Vector3.zero;
		this.smoothingTime = smoothingTime;
		this.prediction = prediction;
	}

	public Vector3 Update(Vector3 targetPositionNew, float deltaTime)
	{
		Vector3 vector = (targetPositionNew - targetPosition) / deltaTime;
		targetPosition = targetPositionNew;
		float num = Mathf.Min(1f, deltaTime / smoothingTime);
		velocity = velocity * (1f - num) + (targetPosition + vector * prediction - position) * num;
		position += velocity * Time.deltaTime;
		return position;
	}

	public Vector3 Update(Vector3 targetPositionNew, float deltaTime, bool reset)
	{
		if (reset)
		{
			targetPosition = targetPositionNew;
			position = targetPositionNew;
			velocity = Vector3.zero;
			return position;
		}
		return Update(targetPositionNew, deltaTime);
	}

	public Vector3 GetPosition()
	{
		return position;
	}

	public Vector3 GetVelocity()
	{
		return velocity;
	}
}
