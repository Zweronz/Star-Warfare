using UnityEngine;

public class MoveToScript : MonoBehaviour
{
	public Transform to;

	private void Start()
	{
	}

	private void Update()
	{
		Vector3 normalized = (to.position - base.transform.position).normalized;
		base.transform.Translate(normalized * 5f * Time.deltaTime);
	}
}
