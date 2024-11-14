using UnityEngine;

public class GetBattleStateResponse : Response
{
	protected IBattleState[] battleStates;

	public override void ReadData(byte[] data)
	{
		BytesBuffer buffer = new BytesBuffer(data);
		battleStates = new IBattleState[5];
		battleStates[0] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_COOP_STATE);
		battleStates[1] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_TDM_STATE);
		battleStates[2] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_FFA_STATE);
		battleStates[3] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_VIP_STATE);
		battleStates[4] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_CMI_STATE);
		for (int i = 0; i < battleStates.Length; i++)
		{
			battleStates[i].ReadFromBuffer(buffer);
		}
	}

	public override void ProcessLogic()
	{
		GameObject gameObject = GameObject.Find("StartMenu");
		if (gameObject != null)
		{
			StartMenuScript component = gameObject.GetComponent<StartMenuScript>();
			if (component != null)
			{
				UserData userData = component.GetUserData();
				userData.battleStatesResponse = this;
			}
		}
	}

	public void ResetBattleStateFromNet()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		userState.SetBattleStates(battleStates);
	}
}
