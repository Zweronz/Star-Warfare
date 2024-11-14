using UnityEngine;

public class GiftBombPhysicsScript : MonoBehaviour
{
	public new Animation animation;

	public GiftBombScript giftBombScript;

	private bool isPlaying;

	private string animString = "jump01";

	private bool[] touchGround;

	public bool IsOnGround
	{
		get
		{
			return isPlaying;
		}
	}

	private void Start()
	{
		animation.wrapMode = WrapMode.Loop;
		touchGround = new bool[5];
	}

	private void Update()
	{
		if (isPlaying)
		{
			if (!touchGround[0] && AnimationPlayed(animString, 0f))
			{
				touchGround[0] = true;
				touchGround[1] = false;
				PlayDust();
			}
			else if (!touchGround[1] && AnimationPlayed(animString, 0.217f))
			{
				touchGround[1] = true;
				touchGround[2] = false;
				PlayDust();
			}
			else if (!touchGround[2] && AnimationPlayed(animString, 0.434f))
			{
				touchGround[2] = true;
				touchGround[3] = false;
				PlayDust();
			}
			else if (!touchGround[3] && AnimationPlayed(animString, 0.75f))
			{
				touchGround[3] = true;
				touchGround[4] = false;
				PlayDust();
			}
			else if (!touchGround[4] && AnimationPlayed(animString, 0.9f))
			{
				touchGround[0] = false;
				touchGround[4] = true;
				PlayDust();
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == PhysicsLayer.FLOOR && !isPlaying)
		{
			isPlaying = true;
			animation.Play(animString);
		}
	}

	private bool AnimationPlayed(string name, float percent)
	{
		if (animation[name].time - (float)((int)(animation[name].time / 2f) * 2) >= animation[name].clip.length * percent)
		{
			return true;
		}
		return false;
	}

	private void PlayDust()
	{
		Vector3 position = giftBombScript.gameObject.transform.position;
		GameObject original = Resources.Load("Effect/SatanMachine/xmas_box01") as GameObject;
		Object.Instantiate(original, new Vector3(position.x, 0f, position.z), Quaternion.identity);
	}
}
