using UnityEngine;

public class RotateScript : MonoBehaviour
{
	public Vector3 rotationSpeed = new Vector3(0f, 0f, 50f);

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(rotationSpeed * Time.deltaTime);
	}
}
