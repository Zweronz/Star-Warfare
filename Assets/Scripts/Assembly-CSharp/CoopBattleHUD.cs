using System;
using System.Collections.Generic;
using UnityEngine;

public class CoopBattleHUD : BattleHUD
{
	protected const byte ONE_BOSS = 1;

	protected const byte TWO_BOSSES = 2;

	public List<UIImage> playersHpImg = new List<UIImage>();

	public List<UIImage> playersHpBGImg = new List<UIImage>();

	public List<UIImage> playersFlagImg = new List<UIImage>();

	protected UINumeric hpNum;

	protected UIImage energyImg;

	protected UIImage energyBGImg;

	protected UIImage energyEFXImg;

	protected UINumeric energyNum;

	protected UIImage comboImg;

	protected UINumeric comboNum;

	protected UIImage comboEFXImg;

	protected UINumeric comboEFXNum;

	protected Vector2 comboSize;

	protected Vector2 comboNumSize;

	protected float comboZoom;

	protected int prevCombo;

	protected UINumeric rebirthTimerNum;

	protected float rebirthTimer;

	protected float rebirthAnimaStartTimer;

	protected bool bRebirthInit;

	protected Vector2 rebirthNumSize;

	protected bool bStageProcess;

	protected UIImage stageProcessImg;

	protected UIImage bugImg;

	protected UIImage stageProcessMarkImg;

	protected Vector2 bugInitPos;

	protected UIImage bugSurvivalImg;

	protected UINumeric enemyCountNum;

	protected bool bHasBoss;

	protected UIImage bossHPImg;

	protected UIImage bossMarkHPImg;

	protected UIImage bossImg;

	protected Vector2 bossHPInitPos;

	protected Vector2 bossHPInitWH;

	protected UIImage otherbossHPImg;

	protected Vector2 otherbossHPInitPos;

	protected Vector2 otherbossHPInitWH;

	protected byte bossCount = 1;

	protected int energyMax;

	protected float destAngleEnergy;

	public CoopBattleHUD(UIStateManager stateMgr)
	{
		base.stateMgr = stateMgr;
	}

	public override void Close()
	{
		base.Close();
		GameUIManager.GetInstance().RemoveAll();
	}

	protected override void UpdateAllHUD()
	{
		base.UpdateAllHUD();
	}

	protected override void UpdateAllHUDWhenWaitingRebirth()
	{
		base.UpdateAllHUDWhenWaitingRebirth();
		UpdateRebirthTimer();
	}

	public override bool Create()
	{
		base.Create();
		GameUIManager.GetInstance().LoadHUD(HUDBattle.HUDType.Coop, stateMgr, this);
		return true;
	}

	public void ClearOtherPlayers()
	{
		playersHpImg.Clear();
		playersHpBGImg.Clear();
	}

	public void ResetForOtherPlayers()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			ClearOtherPlayers();
			UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
			UnitUI ui = Res2DManager.GetInstance().vUI[27];
			Rect modulePositionRect = unitUI.GetModulePositionRect(0, frameIdx, 29);
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			float num = 0f;
			for (int i = 0; i < remotePlayers.Count; i++)
			{
				UIImage uIImage = new UIImage();
				uIImage.AddObject(unitUI, frameIdx, 29);
				uIImage.Rect = new Rect(modulePositionRect.x, modulePositionRect.y - num, modulePositionRect.width, modulePositionRect.height);
				UIImage uIImage2 = new UIImage();
				uIImage2.AddObject(unitUI, frameIdx, 30);
				uIImage2.Rect = uIImage.Rect;
				uIImage2.SetClipOffs(0, new Vector2(4f, 0f));
				uIImage2.SetClipOffs(1, new Vector2(0f, 0f));
				uIImage2.SetClipOffs(2, new Vector2(-4f, 0f));
				uIImage2.SetClipOffs(3, new Vector2(0f, 0f));
				playersHpBGImg.Add(uIImage);
				playersHpImg.Add(uIImage2);
				UIImage uIImage3 = new UIImage();
				uIImage3.AddObject(ui, 1, remotePlayers[i].GetSeatID());
				uIImage3.SetColor(UIConstant.COLOR_PLAYER_ICONS[remotePlayers[i].GetSeatID()]);
				uIImage3.Rect = new Rect(5f, modulePositionRect.y - num - 10f, 20f, 20f);
				playersFlagImg.Add(uIImage3);
				num += 30f;
			}
			for (int j = 0; j < playersFlagImg.Count; j++)
			{
				stateMgr.m_UIManager.Add(playersFlagImg[j]);
				stateMgr.m_UIManager.Add(playersFlagImg[j]);
			}
			for (int k = 0; k < playersHpBGImg.Count; k++)
			{
				stateMgr.m_UIManager.Add(playersHpBGImg[k]);
				stateMgr.m_UIManager.Add(playersHpImg[k]);
			}
		}
	}

	protected override void UpdateHP()
	{
		base.UpdateHP();
	}

	protected override void UpdateAimIcon()
	{
		base.UpdateAimIcon();
		if (Time.time - mLastUpdateAimTime > 0.2f)
		{
			mLastUpdateAimTime = Time.time;
			bool flag = false;
			Camera mainCamera = Camera.main;
			Transform transform = mainCamera.transform;
			ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
			Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 0.1f));
			Vector3 normalized = (vector - transform.position).normalized;
			Ray ray = new Ray(transform.position + 1.8f * normalized, normalized);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.ENEMY)) && hitInfo.collider.gameObject.layer == PhysicsLayer.ENEMY)
			{
				flag = true;
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
	}

	public override void HideUI()
	{
		base.HideUI();
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			for (int i = 0; i < remotePlayers.Count; i++)
			{
				GameObject gameObject = remotePlayers[i].GetGameObject();
				if (gameObject != null && gameObject.active)
				{
					playersHpImg[i].Visible = false;
					playersHpBGImg[i].Visible = false;
					playersFlagImg[i].Visible = false;
				}
			}
		}
		energyEFXImg.Visible = false;
		energyImg.Visible = false;
		energyNum.Visible = false;
		energyBGImg.Visible = false;
		comboImg.Visible = false;
		comboNum.Visible = false;
		comboEFXImg.Visible = false;
		comboEFXNum.Visible = false;
		if (bStageProcess)
		{
			stageProcessImg.Visible = false;
			bugImg.Visible = false;
			stageProcessMarkImg.Visible = false;
		}
		else
		{
			enemyCountNum.Visible = false;
			bugSurvivalImg.Visible = false;
		}
		if (bHasBoss)
		{
			if (bossCount == 1)
			{
				bossHPImg.Visible = false;
				bossImg.Visible = false;
				bossMarkHPImg.Visible = false;
			}
			else
			{
				bossHPImg.Visible = false;
				otherbossHPImg.Visible = false;
				bossImg.Visible = false;
				bossMarkHPImg.Visible = false;
			}
		}
	}

	public override void HideUIWhenWaitingRebirth()
	{
		base.HideUIWhenWaitingRebirth();
		hpNum.Visible = false;
	}

	protected void InitEnergyMax()
	{
		energyMax = userState.Enegy;
		if (energyMax <= 0)
		{
			energyMax = 500;
		}
		UserStateUI.GetInstance().SetAmmoandMaxAmmo(userState.Enegy, userState.Enegy);
	}

	protected void UpdateEnergy()
	{
		UserStateUI.GetInstance().UpdateAmmo(userState.Enegy);
	}

	protected void UpdateCombo()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null)
		{
			return;
		}
		int combo = player.GetCombo();
		if (combo > 1)
		{
			comboImg.Visible = true;
			comboNum.Visible = true;
			comboNum.SetNumeric(unitUI, 1, string.Format("{0:D2}", combo));
			if (combo >= 2)
			{
				if (prevCombo != combo)
				{
					comboZoom = 1f;
					prevCombo = combo;
				}
				comboZoom += 0.05f;
				if (comboZoom > 2f)
				{
					comboZoom = 2f;
				}
				comboEFXImg.Visible = true;
				comboEFXNum.Visible = true;
				comboEFXNum.SetNumeric(unitUI, 1, string.Format("{0:D2}", combo));
				comboEFXImg.SetSize(new Vector2((int)(comboSize.x * comboZoom), (int)(comboSize.y * comboZoom)));
				comboEFXNum.SetSize(new Vector2((int)(comboNumSize.x * comboZoom), (int)(comboNumSize.y * comboZoom)));
				comboEFXImg.SetColor(new Color(UIConstant.COLOR_COMBO.r, UIConstant.COLOR_COMBO.g, UIConstant.COLOR_COMBO.b, UIConstant.COLOR_COMBO.a + 1f - comboZoom));
				comboEFXNum.SetColor(new Color(UIConstant.COLOR_COMBO.r, UIConstant.COLOR_COMBO.g, UIConstant.COLOR_COMBO.b, UIConstant.COLOR_COMBO.a + 1f - comboZoom));
			}
		}
		else
		{
			comboZoom = 1f;
			prevCombo = 0;
			comboImg.Visible = false;
			comboNum.Visible = false;
			comboEFXImg.Visible = false;
			comboEFXNum.Visible = false;
		}
	}

	private void UpdateRebirthTimer()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null)
		{
			return;
		}
		if (player.State == Player.WAIT_REBIRTH_STATE)
		{
			if (!bRebirthInit)
			{
				useProps.SetState(1);
				useProps.SetIdleTimer(5f);
				rebirthAnimaStartTimer = 0f;
				rebirthTimer = 1f;
				bRebirthInit = true;
				rebirthTimerNum.Visible = true;
			}
			Timer timer = player.GetRebirthTimer();
			float num = 1f;
			if (!timer.Ready())
			{
				float timeSpan = timer.GetTimeSpan();
				if (timeSpan >= rebirthTimer)
				{
					rebirthAnimaStartTimer = timeSpan;
					rebirthTimer = timeSpan + 1f;
					return;
				}
				int num2 = (int)(6f - timeSpan);
				rebirthTimerNum.SetNumeric(unitUI, 1, Convert.ToString(num2));
				num = 1f + (timeSpan - rebirthAnimaStartTimer);
				rebirthTimerNum.Visible = true;
				rebirthTimerNum.SetSize(new Vector2((int)(rebirthNumSize.x * num), (int)(rebirthNumSize.y * num)));
				rebirthTimerNum.SetColor(new Color(UIConstant.COLOR_REBIRTH_TIMER.r, UIConstant.COLOR_REBIRTH_TIMER.g, UIConstant.COLOR_REBIRTH_TIMER.b, UIConstant.COLOR_REBIRTH_TIMER.a + 1f - num));
			}
		}
		else
		{
			useProps.SetIdleTimer(3f);
			rebirthAnimaStartTimer = 0f;
			rebirthTimer = 0f;
			bRebirthInit = false;
			rebirthTimerNum.Visible = false;
		}
	}

	private void UpdateWaveProcess()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[17];
		if (unitUI == null)
		{
			return;
		}
		if (bStageProcess)
		{
			if (GameApp.GetInstance().GetGameWorld().TotalEnemyCount < 1)
			{
				UserStateUI.GetInstance().SetWaveProcess(0f);
			}
			else
			{
				UserStateUI.GetInstance().SetWaveProcess((float)GameApp.GetInstance().GetGameWorld().EnemyID / (float)GameApp.GetInstance().GetGameWorld().TotalEnemyCount);
			}
		}
		else
		{
			enemyCountNum.SetNumeric(unitUI, 1, Convert.ToString(player.Kills));
			enemyCountNum.Visible = true;
			bugSurvivalImg.Visible = true;
		}
	}

	private void UpdateBossHP()
	{
		if (bHasBoss && GameApp.GetInstance().GetGameWorld().InBossBattle)
		{
			stageProcessImg.Visible = false;
			bugImg.Visible = false;
			stageProcessMarkImg.Visible = false;
			if (bossCount == 1)
			{
				bossHPImg.Visible = true;
				bossImg.Visible = true;
				bossMarkHPImg.Visible = true;
				Enemy boss = GameApp.GetInstance().GetGameWorld().GetBoss(0);
				int num = (int)(bossHPInitWH.x * (float)boss.HP / (float)boss.MaxHP);
				bossHPImg.Rect = new Rect(bossHPInitPos.x - (bossHPInitWH.x - (float)num), bossHPImg.Rect.y, bossHPInitWH.x, bossHPImg.Rect.height);
				bossHPImg.SetClip(new Rect(bossHPInitPos.x, bossHPImg.Rect.y, bossHPInitWH.x, bossHPImg.Rect.height));
			}
			else
			{
				bossHPImg.Visible = true;
				otherbossHPImg.Visible = true;
				bossImg.Visible = true;
				bossMarkHPImg.Visible = true;
				Enemy boss2 = GameApp.GetInstance().GetGameWorld().GetBoss(0);
				int num2 = (int)(bossHPInitWH.x * (float)boss2.HP / (float)boss2.MaxHP);
				bossHPImg.Rect = new Rect(bossHPInitPos.x + (bossHPInitWH.x - (float)num2), bossHPImg.Rect.y, bossHPInitWH.x, bossHPImg.Rect.height);
				bossHPImg.SetClip(new Rect(bossHPInitPos.x, bossHPImg.Rect.y, bossHPInitWH.x, bossHPImg.Rect.height));
				Enemy boss3 = GameApp.GetInstance().GetGameWorld().GetBoss(1);
				int num3 = (int)(otherbossHPInitWH.x * (float)boss3.HP / (float)boss3.MaxHP);
				otherbossHPImg.Rect = new Rect(otherbossHPInitPos.x - (otherbossHPInitWH.x - (float)num3), otherbossHPImg.Rect.y, otherbossHPInitWH.x, otherbossHPImg.Rect.height);
				otherbossHPImg.SetClip(new Rect(otherbossHPInitPos.x, otherbossHPImg.Rect.y, otherbossHPInitWH.x, otherbossHPImg.Rect.height));
			}
		}
	}

	public void UpdataHPForOtherPlayers()
	{
		if (!GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			return;
		}
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		for (int i = 0; i < remotePlayers.Count; i++)
		{
			GameObject gameObject = remotePlayers[i].GetGameObject();
			if (gameObject != null && gameObject.active)
			{
				playersHpImg[i].Visible = true;
				playersHpBGImg[i].Visible = true;
				playersFlagImg[i].Visible = true;
				float width = (float)remotePlayers[i].Hp * playersHpBGImg[i].Rect.width / (float)remotePlayers[i].MaxHp;
				playersHpImg[i].SetClip(new Rect(playersHpBGImg[i].Rect.x, playersHpBGImg[i].Rect.y, width, playersHpBGImg[i].Rect.height));
			}
			else
			{
				playersHpImg[i].Visible = false;
				playersHpBGImg[i].Visible = false;
				playersFlagImg[i].Visible = false;
			}
		}
	}
}
