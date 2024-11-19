using UnityEngine;

internal class RevivalItem : Item
{
	public RevivalItem(ItemID itemID)
	{
		base.ItemID = itemID;
	}

	public override bool Use(Player player, byte bagIndex)
	{
		if (!player.InPlayingState() && GameApp.GetInstance().GetGameWorld().State != GameState.SwitchBossLevel)
		{
			if (GameApp.GetInstance().GetGameMode().IsSingle())
			{
				TakeEffect(player, 0f);
			}
			else
			{
				PlayerUseItemRequest request = new PlayerUseItemRequest(bagIndex, (byte)base.ItemID, 0);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			return true;
		}
		return false;
	}

	public override void TakeEffect(Player player, float hpRate)
	{
		player.StopAnimation(AnimationString.ENEMY_DEAD);
		player.Hp = (int)((float)player.MaxHp * itemEffect.GetEffect(EffectsType.HP_RECOVERY));
		player.Hp = Mathf.Clamp(player.Hp, 0, player.MaxHp);
		player.SetState(Player.IDLE_STATE);
		if (!Application.isMobilePlatform)
		{
			Screen.lockCursor = true;
		}
		if (player.IsLocal())
		{
			Transform transform = Camera.main.transform.Find("Screen_DeadBlood");
			transform.GetComponent<Renderer>().enabled = false;
			ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
			if (null != component)
			{
				component.ResetAngleV();
			}
			GameApp.GetInstance().GetGameWorld().State = GameState.Playing;
			GameApp.GetInstance().GetUserState().Achievement.Rebirth();
		}
	}
}
