using UnityEngine;

public class FlyBagAnimationScript : MonoBehaviour
{
	public Player player;

	protected bool enabledFire;

	protected Timer enableFireTimer = new Timer();

	protected Timer disableFireTimer = new Timer();

	private void Start()
	{
		base.GetComponent<Animation>()["front"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["back"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["left"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["right"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["idle"].wrapMode = WrapMode.Loop;
	}

	private void Update()
	{
		if (player == null || player.inputController == null)
		{
			return;
		}
		InputInfo inputInfo = player.inputController.inputInfo;
		if (inputInfo.IsMoving())
		{
			switch (inputInfo.dir)
			{
			case MoveDirection.Forward:
				base.GetComponent<Animation>().CrossFade("front", 1f);
				break;
			case MoveDirection.Backward:
				base.GetComponent<Animation>().CrossFade("back", 1f);
				break;
			case MoveDirection.Left:
				base.GetComponent<Animation>().CrossFade("left", 1f);
				break;
			case MoveDirection.Right:
				base.GetComponent<Animation>().CrossFade("right", 1f);
				break;
			}
		}
		else
		{
			base.GetComponent<Animation>().CrossFade("idle", 1f);
		}
	}
}
