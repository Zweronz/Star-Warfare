public class UploadKillBossStateRequest : Request
{
	protected UserState userState;

	public UploadKillBossStateRequest(UserState userState)
	{
		this.userState = userState;
	}

	public override byte[] GetBytes()
	{
		string text = userState.GetSoloSuccBossTime().ToShortDateString();
		byte b = (byte)(12 + Global.TOTAL_SOLO_BOSS_NUM + Global.TOTAL_COOP_BOSS_NUM + Global.TOTAL_COOP_BOSS_NUM + text.Length + 1);
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(24);
		bytesBuffer.AddByte(b);
		userState.WriteBossDate(bytesBuffer);
		userState.WriteBossKillTime(bytesBuffer);
		userState.WriteBossDropMithrilTime(bytesBuffer);
		userState.WriteSuccSoloBoss(bytesBuffer);
		userState.WriteSuccCoopBoss(bytesBuffer);
		userState.WriteSuccCoopBossGetMithril(bytesBuffer);
		userState.WriteSoloSuccBossTime(bytesBuffer);
		return bytesBuffer.GetBytes();
	}
}
