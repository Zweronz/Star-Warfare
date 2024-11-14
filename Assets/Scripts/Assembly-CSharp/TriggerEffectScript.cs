using UnityEngine;

public class TriggerEffectScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == PhysicsLayer.FLOOR)
		{
			GameObject original = Resources.Load("Effect/SatanMachine/xmas_box01") as GameObject;
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			gameObject.transform.position = base.transform.position;
			AudioManager.GetInstance().PlaySoundSingle("Audio/enemy/SatanMachine/liwuhe");
		}
	}
}
