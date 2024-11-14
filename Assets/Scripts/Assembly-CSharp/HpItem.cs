using UnityEngine;

public class HpItem : Item
{
	public HpItem(ItemID itemID)
	{
		base.ItemID = itemID;
	}

	public override bool Use(Player player, byte bagIndex)
	{
		float skill = player.GetSkills().GetSkill(SkillsType.RECOVERY_BOOTH);
		byte buffValue = (byte)(skill * 100f);
		if (player.InPlayingState())
		{
			if (GameApp.GetInstance().GetGameMode().IsSingle())
			{
				TakeEffect(player, skill);
			}
			else
			{
				PlayerUseItemRequest request = new PlayerUseItemRequest(bagIndex, (byte)base.ItemID, buffValue);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			return true;
		}
		return false;
	}

	public override void TakeEffect(Player player, float hpRate)
	{
		float num = itemEffect.GetEffect(EffectsType.HP_BOOTH) * (1f + hpRate);
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			num = VSMath.GetRecoverInVS(num);
		}
		player.Hp += (int)num;
		player.Hp = Mathf.Clamp(player.Hp, 0, player.MaxHp);
	}
}
