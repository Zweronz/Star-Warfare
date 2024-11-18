using UnityEngine;

public abstract class VersusHUDBattle : GameSubUI, IWaitVSRebirth
{
	protected Player mPlayer;

	protected UserState mUserState;

	protected HUDBattle mHudBattle;

	private float mLastUpdateAimTime;

	protected override void OnCreate()
	{
		mUserState = GameApp.GetInstance().GetUserState();
		mPlayer = GameApp.GetInstance().GetGameWorld().GetPlayer();
		UserStateUI.GetInstance().SetTeamMode(GameApp.GetInstance().GetGameMode().IsTeamMode());
		UserStateUI.GetInstance().SetPlayerSeatID(mPlayer.GetSeatID());
		UserStateUI.GetInstance().SetWaitVSRebirth(this);
		mHudBattle = (HUDBattle)GetGameUI();
		mHudBattle.StateKillInfoManager.SetActive(true);
		mHudBattle.StatePlayerSeatIcon.SetActive(true);
		mHudBattle.ButtonExitRoom.SetActive(true);
		UserStateUI.GetInstance().SetTotalScore(Lobby.GetInstance().WinValue);
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		UpdateRebirthCountdownValue();
		UpdateAim();
		if (mHudBattle.InWaitRebirthState())
		{
			if (mHudBattle.StateItem.activeSelf)
			{
				mHudBattle.StateItem.SetActive(false);
			}
		}
		else if (!mHudBattle.StateItem.activeSelf)
		{
			mHudBattle.StateItem.SetActive(true);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		mPlayer = null;
		mUserState = null;
		mHudBattle = null;
	}

	public void WaitVSRebirthStart()
	{
		mHudBattle.StateWaitVSRebirth.Show();
	}

	public void WaitVSRebirthEnd()
	{
		mHudBattle.StateWaitVSRebirth.Hide();
	}

	public void OnVSRebirth()
	{
		mHudBattle.StateWaitVSRebirth.OnRebirth();
	}

	public virtual void DoAutoBalance()
	{
	}

	private void UpdateRebirthCountdownValue()
	{
		mHudBattle.StateWaitVSRebirth.SetCountdownValue(mPlayer.GetVSRebirthRemainingTime().ToString());
	}

	protected void UpdateBattleInformation()
	{
		VSBattleInformation battleInfo = GameApp.GetInstance().GetGameWorld().BattleInfo;
		if (battleInfo != null)
		{
			UserStateUI.GetInstance().SetVSBattleInformation(battleInfo);
		}
	}

	private void UpdateAim()
	{
		if (!(Time.time - mLastUpdateAimTime > 0.2f))
		{
			return;
		}
		mLastUpdateAimTime = Time.time;
		bool aim = false;
		RemotePlayer aimPlayer = null;
		Camera mainCamera = Camera.main;
		Transform transform = mainCamera.transform;
		ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
		Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 0.1f));
		Vector3 normalized = (vector - transform.position).normalized;
		Ray ray = new Ray(transform.position + 1.8f * normalized, normalized);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.GIFT)))
		{
			if (hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				int userID = int.Parse(hitInfo.collider.gameObject.name);
				RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(userID);
				if (remotePlayerByUserID != null && remotePlayerByUserID.InPlayingState() && !remotePlayerByUserID.IsSameTeam(mPlayer))
				{
					aim = true;
					aimPlayer = remotePlayerByUserID;
				}
			}
			else if (hitInfo.collider.gameObject.layer == PhysicsLayer.GIFT)
			{
				aim = true;
			}
		}
		UserStateUI.GetInstance().SetAim(aim);
		GameApp.GetInstance().GetGameWorld().GetPlayer()
			.SetAimPlayer(aimPlayer);
	}

	protected override void OnHide()
	{
		base.OnHide();
		mHudBattle.StateWaitVSRebirth.Hide();
		UserStateUI.GetInstance().SetUsedRiviveTimes(0);
	}
}
