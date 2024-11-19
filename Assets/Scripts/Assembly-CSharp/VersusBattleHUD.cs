using UnityEngine;

public abstract class VersusBattleHUD : BattleHUD
{
	protected WhoKillsWhoHUD mWhoKillsWho;

	protected WaitVSRebirthHUD mVSRebirth;

	protected UIImage mClockImage;

	protected UINumeric mWinValueNumeric;

	protected UINumeric mFlagTimeValueNumeric;

	protected Vector2 mFlagTimeSize;

	protected bool mShowClock;

	public override bool Create()
	{
		base.Create();
		return true;
	}

	public bool CreateForCTF()
	{
		base.Create();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null)
		{
			return false;
		}
		return true;
	}

	public bool CreateForVIP()
	{
		base.Create();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null)
		{
			return false;
		}
		return true;
	}

	public bool CreateForTMDGift()
	{
		base.Create();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null)
		{
			return false;
		}
		return true;
	}

	public override void Close()
	{
		base.Close();
	}

	public override void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		base.HandleEvent(control, command, wparam, lparam);
		if (control == mVSRebirth)
		{
			mVSRebirth.DoRebirth();
		}
	}

	public override void HideUI()
	{
		base.HideUI();
		if (mVSRebirth != null)
		{
			mVSRebirth.Hide();
		}
	}

	public override void HideUIWhenWaitingRebirth()
	{
		base.HideUIWhenWaitingRebirth();
		useProps.Hide();
	}

	protected override void UpdateAllHUD()
	{
		base.UpdateAllHUD();
	}

	protected override void UpdateAllHUDWhenWaitingRebirth()
	{
		base.UpdateAllHUDWhenWaitingRebirth();
		UpdateWinValue();
		UpdateKillInfo();
		UpdatePopulation();
		UpdateRebirthCountdownValue();
	}

	protected override void UpdateAllHUDWhenFinish()
	{
		base.UpdateAllHUDWhenWaitingRebirth();
		UpdateWinValue();
		UpdatePopulation();
	}

	protected override void UpdateAimIcon()
	{
		base.UpdateAimIcon();
		if (!(Time.time - mLastUpdateAimTime > 0.2f))
		{
			return;
		}
		mLastUpdateAimTime = Time.time;
		bool flag = false;
		Camera mainCamera = Camera.main;
		Transform transform = mainCamera.transform;
		ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
		Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 0.1f));
		Vector3 normalized = (vector - transform.position).normalized;
		Ray ray = new Ray(transform.position + 1.8f * normalized, normalized);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)) && hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
		{
			int userID = int.Parse(hitInfo.collider.gameObject.name);
			Player remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(userID);
			if (remotePlayerByUserID != null && remotePlayerByUserID.InPlayingState() && !remotePlayerByUserID.IsSameTeam(player))
			{
				flag = true;
			}
		}
		if (flag)
		{
			aimImg.SetColor(Color.red);
			aimOnFireImg.SetColor(Color.red);
		}
		else
		{
			aimImg.SetColor(UIConstant.COLOR_AIM);
			aimOnFireImg.SetColor(UIConstant.COLOR_AIM);
		}
	}

	protected virtual void UpdateWinScore()
	{
	}

	protected virtual void UpdatePopulation()
	{
	}

	protected void UpdateRebirthCountdownValue()
	{
		if (mVSRebirth != null && player != null)
		{
			mVSRebirth.SetCountdownValue(player.GetVSRebirthRemainingTime().ToString());
		}
	}

	protected virtual void UpdateWinValue()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null)
		{
			return;
		}
		if (mShowClock)
		{
			int num = Lobby.GetInstance().WinValue * 60 - Lobby.GetInstance().GetVSClock().GetCurrentTimeSeconds();
			if (num < 0)
			{
				num = 0;
			}
			int num2 = num / 60;
			int num3 = num - num2 * 60;
			mWinValueNumeric.SetNumeric(unitUI, 5, string.Format("{0:D2}", num2) + ":" + string.Format("{0:D2}", num3));
		}
		else
		{
			UpdateWinScore();
		}
	}

	public override void AddWhoKillsWho(int killerID, HUDAction action, int killedID)
	{
		if (mWhoKillsWho != null)
		{
			mWhoKillsWho.AddWhoKillsWho(killerID, action, killedID);
		}
	}

	protected void UpdateKillInfo()
	{
		if (mWhoKillsWho != null)
		{
			mWhoKillsWho.UpdateAlpha();
		}
	}

	public virtual void DoAutoBalance()
	{
	}

	public void WaitVSRebirthStart()
	{
		if (mVSRebirth != null)
		{
			mVSRebirth.Show();
			if (!Application.isMobilePlatform)
			{
				Screen.lockCursor = true;
			}
		}
	}

	public void WaitVSRebirthEnd()
	{
		if (mVSRebirth != null)
		{
			mVSRebirth.Hide();
			if (!Application.isMobilePlatform)
			{
				Screen.lockCursor = true;
			}
		}
	}

	public void OnVSRebirth()
	{
		if (mVSRebirth != null)
		{
			mVSRebirth.OnRebirth();
		}
	}
}
