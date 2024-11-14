using UnityEngine;

public class UploadDataRequest : Request
{
	protected UserState userState;

	public UploadDataRequest(UserState userState)
	{
		this.userState = userState;
	}

	public override byte[] GetBytes()
	{
		string text = userState.GetSoloSuccBossTime().ToShortDateString();
		byte b = (byte)(Global.BAG_MAX_NUM + Global.AVATAR_PART_NUM + 47 + Global.TOTAL_ARMOR_HEAD_NUM + Global.TOTAL_ARMOR_BODY_NUM + Global.TOTAL_ARMOR_ARM_NUM + Global.TOTAL_ARMOR_FOOT_NUM + Global.TOTAL_ARMOR_BAG_NUM + Global.TOTAL_ITEM_NUM + 1 + 4 + 4 + 4 + 4 + 4 + 2 + 8 + 2 + 2 + 1 + 1 + Global.TOTAL_SOLO_BOSS_NUM + Global.TOTAL_COOP_BOSS_NUM + Global.TOTAL_COOP_BOSS_NUM + text.Length + 1 + 1);
		Debug.Log("Len:" + b);
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(15);
		bytesBuffer.AddByte(b);
		userState.WriteCash(bytesBuffer);
		userState.WriteMithril(bytesBuffer);
		userState.WriteBagMaxNum(bytesBuffer);
		userState.WriteEnergy(bytesBuffer);
		userState.WriteExp(bytesBuffer);
		userState.WriteSaveNum(bytesBuffer);
		userState.WriteLevelId(bytesBuffer);
		userState.WriteBossDate(bytesBuffer);
		userState.WriteBossKillTime(bytesBuffer);
		userState.WriteBossDropMithrilTime(bytesBuffer);
		userState.WriteTwitter(bytesBuffer);
		userState.WriteFacebook(bytesBuffer);
		userState.WriteRewardStatus(bytesBuffer);
		userState.WriteBagPositionBuffer(bytesBuffer);
		userState.WriteArmorState(bytesBuffer);
		userState.WriteWeaponOwnState(bytesBuffer);
		userState.WriteArmorOwnStateHead(bytesBuffer);
		userState.WriteArmorOwnStateBody(bytesBuffer);
		userState.WriteArmorOwnStateArm(bytesBuffer);
		userState.WriteArmorOwnStateFoot(bytesBuffer);
		userState.WriteArmorOwnStateBag(bytesBuffer);
		userState.WritePropsOwnNum(bytesBuffer);
		userState.WriteSuccSoloBoss(bytesBuffer);
		userState.WriteSuccCoopBoss(bytesBuffer);
		userState.WriteSuccCoopBossGetMithril(bytesBuffer);
		userState.WriteSoloSuccBossTime(bytesBuffer);
		return bytesBuffer.GetBytes();
	}
}
