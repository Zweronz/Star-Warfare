using UnityEngine;

internal class PlayerHitItemResponse : Response
{
	protected int playerID;

	protected int itemID;

	protected int m_hp;

	protected bool criticalDamage;

	protected byte weaponType;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		itemID = bytesBuffer.ReadShort();
		m_hp = bytesBuffer.ReadInt();
		criticalDamage = bytesBuffer.ReadBool();
		weaponType = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player player = gameWorld.GetPlayer();
		if (player == null)
		{
			return;
		}
		int channelID = Lobby.GetInstance().GetChannelID();
		CMIGift giftWithId = gameWorld.GetGiftWithId(itemID);
		if (giftWithId == null)
		{
			return;
		}
		bool flag = false;
		if (playerID == channelID)
		{
			flag = true;
		}
		giftWithId.SetHp(m_hp);
		if (giftWithId.GetHp() <= 0)
		{
			if (giftWithId.IsSpecialGift())
			{
				if (giftWithId.GetType() == CMIGiftType.TYPE_RANDOM_POSITIVE)
				{
					GameObject original = Resources.Load("Effect/xmas_box_hit/xmas_box_explosion") as GameObject;
					GameObject gameObject = Object.Instantiate(original) as GameObject;
					gameObject.transform.position = giftWithId.GetPositionEffect();
				}
				else
				{
					GameObject original2 = Resources.Load("VS/GiftExplosion") as GameObject;
					GameObject gameObject2 = Object.Instantiate(original2) as GameObject;
					gameObject2.transform.position = giftWithId.GetPosition();
					GiftExplosionScript component = gameObject2.GetComponent<GiftExplosionScript>();
					if (component != null)
					{
						component.damage = 500;
						component.explodeRadius = 8f;
						if (flag)
						{
							component.isLocal = true;
						}
						else
						{
							component.isLocal = false;
						}
					}
				}
			}
			else
			{
				GameObject original3 = Resources.Load("Effect/xmas_box_hit/xmas_box_hit") as GameObject;
				GameObject gameObject3 = Object.Instantiate(original3) as GameObject;
				gameObject3.transform.position = giftWithId.GetPositionEffect();
			}
			if (flag)
			{
				player.VSStatistics.AddHitItemScore(giftWithId.GetScore());
				player.VSStatistics.CMIGiftHit++;
				player.VSStatistics.UpdateCashReward();
				NumberManager.GetInstance().ShowScore(giftWithId.GetPositionEffect(), giftWithId.GetScore());
			}
			gameWorld.DestroyGift(giftWithId);
			GameObject gameObject4 = GameObject.Find("GameUI");
			if (!(gameObject4 != null))
			{
				return;
			}
			InGameUIScript component2 = gameObject4.GetComponent<InGameUIScript>();
			if (component2 != null)
			{
				Player playerByUserID = gameWorld.GetPlayerByUserID(playerID);
				if (playerByUserID != null)
				{
					component2.AddWhoKillsWho(playerByUserID.GetSeatID(), HUDAction.HIT_GIFT, 10);
				}
			}
		}
		else if (flag)
		{
			GameObject original4 = Resources.Load("Effect/xmas_box_hit/xmas_box_hit02") as GameObject;
			GameObject gameObject5 = Object.Instantiate(original4) as GameObject;
			gameObject5.transform.position = giftWithId.GetPositionEffect();
		}
	}
}
