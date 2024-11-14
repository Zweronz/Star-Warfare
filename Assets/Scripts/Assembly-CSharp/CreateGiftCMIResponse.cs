using System.Collections.Generic;

internal class CreateGiftCMIResponse : Response
{
	protected List<CMIGift> m_gifts = new List<CMIGift>();

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		int num = bytesBuffer.ReadByte();
		for (int i = 0; i < num; i++)
		{
			int type = bytesBuffer.ReadByte();
			int id = bytesBuffer.ReadShort();
			byte pointId = bytesBuffer.ReadByte();
			CMIGift cMIGift = new CMIGift(id, (CMIGiftType)type);
			cMIGift.SetPointId(pointId);
			m_gifts.Add(cMIGift);
		}
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
		foreach (CMIGift gift in m_gifts)
		{
			if (gift.IsSpecialGift())
			{
				gameWorld.SetCMISpecialGift(gift.GetType(), (short)gift.GetId(), true, gift.GetPointId());
			}
			else
			{
				gameWorld.CreateCMIGift(gift);
			}
		}
	}
}
