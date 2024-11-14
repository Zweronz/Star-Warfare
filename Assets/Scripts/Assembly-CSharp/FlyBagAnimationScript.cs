using UnityEngine;

public class FlyBagAnimationScript : MonoBehaviour
{
	public Player player;

	protected bool enabledFire;

	protected Timer enableFireTimer = new Timer();

	protected Timer disableFireTimer = new Timer();

	private void Start()
	{
		base.animation["front"].wrapMode = WrapMode.Loop;
		base.animation["back"].wrapMode = WrapMode.Loop;
		base.animation["left"].wrapMode = WrapMode.Loop;
		base.animation["right"].wrapMode = WrapMode.Loop;
		base.animation["idle"].wrapMode = WrapMode.Loop;
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
				base.animation.CrossFade("front", 1f);
				break;
			case MoveDirection.Backward:
				base.animation.CrossFade("back", 1f);
				break;
			case MoveDirection.Left:
				base.animation.CrossFade("left", 1f);
				break;
			case MoveDirection.Right:
				base.animation.CrossFade("right", 1f);
				break;
			}
		}
		else
		{
			base.animation.CrossFade("idle", 1f);
		}
	}
}
