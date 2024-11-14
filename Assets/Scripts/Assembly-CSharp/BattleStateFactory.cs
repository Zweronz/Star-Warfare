public class BattleStateFactory
{
	public static BattleStateFactory instance;

	public static BattleStateFactory GetInstance()
	{
		if (instance == null)
		{
			instance = new BattleStateFactory();
		}
		return instance;
	}

	public IBattleState Create(GMBattleState gmstate)
	{
		IBattleState result = null;
		switch (gmstate)
		{
		case GMBattleState.GM_COOP_STATE:
			result = new CoopState();
			break;
		case GMBattleState.GM_FFA_STATE:
			result = new FFAState();
			break;
		case GMBattleState.GM_TDM_STATE:
			result = new TDMState();
			break;
		case GMBattleState.GM_VIP_STATE:
			result = new VIPState();
			break;
		case GMBattleState.GM_CMI_STATE:
			result = new CMIState();
			break;
		}
		return result;
	}
}
