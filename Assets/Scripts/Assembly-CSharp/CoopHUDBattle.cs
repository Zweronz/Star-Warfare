using UnityEngine;

public class CoopHUDBattle : GameSubUI
{
	protected Player mPlayer;

	protected UserState mUserState;

	private int mPrevCombo;

	private float mLastUpdateAimTime;

	protected HUDBattle mHudBattle;

	private bool mIsRebirthInit;

	private bool mIsLastStateRebirth;

	protected override void OnCreate()
	{
		mUserState = GameApp.GetInstance().GetUserState();
		mPlayer = GameApp.GetInstance().GetGameWorld().GetPlayer();
		mHudBattle = (HUDBattle)GetGameUI();
		mHudBattle.StateRemotePlayer.SetActive(true);
		mHudBattle.StateAmmo.SetActive(true);
		mHudBattle.StatePlayerHpLabel.SetActive(true);
		InitEnergyMax();
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			mHudBattle.ButtonExitRoom.SetActive(true);
		}
		else
		{
			mHudBattle.ButtonPause.SetActive(true);
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		UpdateEnergy();
		UpdateCombo();
		UpdateAim();
		UpdateRemotePlayer();
		UpdateRebirthTimer();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		mPlayer = null;
		mUserState = null;
	}

	private void InitEnergyMax()
	{
		int enegy = mUserState.Enegy;
		if (enegy <= 0)
		{
			enegy = 500;
		}
		UserStateUI.GetInstance().SetAmmoandMaxAmmo(mUserState.Enegy, mUserState.Enegy);
	}

	private void UpdateEnergy()
	{
		UserStateUI.GetInstance().UpdateAmmo(mUserState.Enegy);
	}

	private void UpdateCombo()
	{
		int combo = mPlayer.GetCombo();
		if (combo > 1)
		{
			if (mPrevCombo != combo)
			{
				mPrevCombo = combo;
				UserStateUI.GetInstance().PushCombo(combo);
			}
		}
		else
		{
			mPrevCombo = 0;
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
		Enemy aimEnemy = null;
		Camera mainCamera = Camera.main;
		Transform transform = mainCamera.transform;
		ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
		Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 0.1f));
		Vector3 normalized = (vector - transform.position).normalized;
		Ray ray = new Ray(transform.position + 1.8f * normalized, normalized);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.ENEMY)) && hitInfo.collider.gameObject.layer == PhysicsLayer.ENEMY)
		{
			aim = true;
			Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(hitInfo.collider.gameObject.name);
			if (enemyByID != null && enemyByID.GetState() != Enemy.DEAD_STATE && !enemyByID.IsBoss())
			{
				aimEnemy = enemyByID;
			}
		}
		UserStateUI.GetInstance().SetAim(aim);
		GameApp.GetInstance().GetGameWorld().GetPlayer()
			.SetAimEnemy(aimEnemy);
	}

	private void UpdateRemotePlayer()
	{
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			mHudBattle.HandleRemotePlayer();
		}
	}

	private void UpdateRebirthTimer()
	{
		if (mPlayer.State == Player.WAIT_REBIRTH_STATE)
		{
			if (!mIsRebirthInit)
			{
				mIsLastStateRebirth = true;
				mIsRebirthInit = true;
				UserStateUI.GetInstance().PopItemList(true);
				mHudBattle.StateRebirth.SetActive(true);
			}
			Timer rebirthTimer = mPlayer.GetRebirthTimer();
			if (!rebirthTimer.Ready())
			{
				float timeSpan = rebirthTimer.GetTimeSpan();
				UserStateUI.GetInstance().UpdateRebirthTime((int)(6f - timeSpan));
			}
		}
		else if (mIsLastStateRebirth)
		{
			UserStateUI.GetInstance().PopItemList(false);
			mIsRebirthInit = false;
			mIsLastStateRebirth = false;
			mHudBattle.StateRebirth.SetActive(false);
		}
	}
}
