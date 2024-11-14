public class AssistItem : Item
{
	protected Timer assistTimer = new Timer();

	public AssistItem(ItemID itemID)
	{
		base.ItemID = itemID;
	}

	public override bool Use(Player player, byte bagIndex)
	{
		if (player.InPlayingState())
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
		float effect = itemEffect.GetEffect(EffectsType.SPEED_BOOTH);
		float effect2 = itemEffect.GetEffect(EffectsType.ATTACK_BOOTH);
		float effect3 = itemEffect.GetEffect(EffectsType.DAMAGE_REDUCE);
		assistTimer.SetTimer(time, false);
		player.ItemAssist(this);
	}

	public bool TimeOut()
	{
		return assistTimer.Ready();
	}

	public float GetSpeedBooth()
	{
		return itemEffect.GetEffect(EffectsType.SPEED_BOOTH);
	}

	public float GetSpeedBoothWhenGotHit()
	{
		return itemEffect.GetEffect(EffectsType.SPEED_BOOTH_WHEN_GOT_HIT);
	}

	public float GetAttackBooth()
	{
		return itemEffect.GetEffect(EffectsType.ATTACK_BOOTH);
	}

	public float GetAttackBoothWhenSecureFlag()
	{
		return itemEffect.GetEffect(EffectsType.ATTACK_BOOTH_WHEN_SECURE_FLAG);
	}

	public float GetDamageReduce()
	{
		return itemEffect.GetEffect(EffectsType.DAMAGE_REDUCE);
	}
}
