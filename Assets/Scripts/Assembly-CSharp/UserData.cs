public class UserData
{
	public PlayerLoginGameServerResponse playerLogin;

	public GetBattleStateResponse battleStatesResponse;

	public int mithril;

	public int saveNum;

	public void ResetUserData()
	{
		playerLogin.ResetUserStateFromNet();
		if (battleStatesResponse != null)
		{
			battleStatesResponse.ResetBattleStateFromNet();
		}
		GameApp.GetInstance().GetUserState().SetMithril(mithril);
	}
}
