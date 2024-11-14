using UnityEngine;

public class TestScript : MonoBehaviour
{
	private NetworkManager network;

	private void Start()
	{
		network = GameApp.GetInstance().CreateNetwork();
	}

	private void Update()
	{
		Vector3 pos = new Vector3(10f, 20f, 30f);
		Vector3 elurAngles = new Vector3(0f, 200f, 0f);
		SendTransformStateRequest request = new SendTransformStateRequest(pos, elurAngles);
		network.SendRequest(request);
	}
}
