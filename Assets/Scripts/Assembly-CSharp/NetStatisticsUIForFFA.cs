using System;
using System.Collections.Generic;
using UnityEngine;

public class NetStatisticsUIForFFA : NetStatisticsUIBase
{
	public class UIPlayerScoreIcon : UIPanelX
	{
		public UIImage m_background;

		public UIImage m_flagIcon;

		public UINumeric m_killsNum;

		public UINumeric m_deathNum;

		public UINumeric m_assistsNum;

		public UINumeric m_scoreNum;

		public UIImage m_ranking;

		public UINumeric m_cashNum;

		public UIPlayerScoreIcon()
		{
			m_background = new UIImage();
			m_ranking = new UIImage();
			m_flagIcon = new UIImage();
			m_killsNum = new UINumeric();
			m_deathNum = new UINumeric();
			m_assistsNum = new UINumeric();
			m_scoreNum = new UINumeric();
			m_cashNum = new UINumeric();
		}

		public void SetAlpha(float alpha)
		{
			m_background.SetColor(new Color(1f, 1f, 1f, alpha));
			m_ranking.SetColor(new Color(1f, 1f, 1f, alpha));
			m_killsNum.SetColor(new Color(1f, 1f, 1f, alpha));
			m_deathNum.SetColor(new Color(1f, 1f, 1f, alpha));
			m_assistsNum.SetColor(new Color(1f, 1f, 1f, alpha));
			m_scoreNum.SetColor(new Color(1f, 1f, 1f, alpha));
			m_cashNum.SetColor(new Color(1f, 1f, 1f, alpha));
		}

		public void SetParentEx()
		{
			m_background.SetParent(this);
			m_ranking.SetParent(this);
			m_flagIcon.SetParent(this);
			m_killsNum.SetParent(this);
			m_deathNum.SetParent(this);
			m_assistsNum.SetParent(this);
			m_scoreNum.SetParent(this);
			m_cashNum.SetParent(this);
		}

		public override void Draw()
		{
			m_background.Draw();
			m_ranking.Draw();
			m_flagIcon.Draw();
			m_killsNum.Draw();
			m_deathNum.Draw();
			m_assistsNum.Draw();
			m_scoreNum.Draw();
			m_cashNum.Draw();
		}

		public override void Destory()
		{
			base.Destory();
			if (m_background != null)
			{
				m_background.Destory();
			}
			if (m_ranking != null)
			{
				m_ranking.Destory();
			}
			if (m_flagIcon != null)
			{
				m_flagIcon.Destory();
			}
			if (m_killsNum != null)
			{
				m_killsNum.Destory();
			}
			if (m_deathNum != null)
			{
				m_deathNum.Destory();
			}
			if (m_assistsNum != null)
			{
				m_assistsNum.Destory();
			}
			if (m_scoreNum != null)
			{
				m_scoreNum.Destory();
			}
			if (m_cashNum != null)
			{
				m_cashNum.Destory();
			}
		}
	}

	private const byte SUBSTATE_SCORE = 0;

	private const byte SUBSTATE_OVER = 1;

	private UIImage killsTitleImg;

	private UIImage deathsTitleImg;

	private UIImage assistsTitleImg;

	private UIImage scoreTitleImg;

	private UIImage rankingTitleImg;

	private UIImage cashTitleImg;

	private byte subState;

	private UIImage nextRoundImg;

	private UINumeric m_waitingTimeNum;

	private static int WAITING_RESTART_TIMER = 15;

	private List<UIPlayerScoreIcon> playerScore = new List<UIPlayerScoreIcon>();

	private DateTime m_time;

	private bool isRestartedGame;

	private static int[] RANK_BONUS = new int[8] { 80, 70, 60, 50, 40, 30, 20, 10 };

	private byte rankId;

	public NetStatisticsUIForFFA(UIStateManager stateMgr)
		: base(stateMgr)
	{
	}

	public override void Init()
	{
		base.Init();
	}

	public override void Close()
	{
		base.Close();
		players.Clear();
		playerScore.Clear();
	}

	public override void Create()
	{
		base.Create();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[15];
		UnitUI ui = Res2DManager.GetInstance().vUI[27];
		UnitUI ui2 = Res2DManager.GetInstance().vUI[0];
		byte[] module = new byte[2] { 21, 22 };
		rankingTitleImg = new UIImage();
		rankingTitleImg.AddObject(unitUI, 9, module);
		rankingTitleImg.Rect = rankingTitleImg.GetObjectRect();
		killsTitleImg = new UIImage();
		killsTitleImg.AddObject(unitUI, 9, 31);
		killsTitleImg.Rect = killsTitleImg.GetObjectRect();
		deathsTitleImg = new UIImage();
		deathsTitleImg.AddObject(unitUI, 9, 32);
		deathsTitleImg.Rect = deathsTitleImg.GetObjectRect();
		assistsTitleImg = new UIImage();
		assistsTitleImg.AddObject(unitUI, 9, 33);
		assistsTitleImg.Rect = assistsTitleImg.GetObjectRect();
		scoreTitleImg = new UIImage();
		scoreTitleImg.AddObject(unitUI, 9, 34);
		scoreTitleImg.Rect = scoreTitleImg.GetObjectRect();
		cashTitleImg = new UIImage();
		cashTitleImg.AddObject(unitUI, 9, 39);
		cashTitleImg.Rect = cashTitleImg.GetObjectRect();
		nextRoundImg = new UIImage();
		nextRoundImg.AddObject(unitUI, 9, 37);
		nextRoundImg.Rect = nextRoundImg.GetObjectRect();
		m_waitingTimeNum = new UINumeric();
		m_waitingTimeNum.AlignStyle = UINumeric.enAlignStyle.left;
		m_waitingTimeNum.SpacingOffsetX = 1f;
		m_waitingTimeNum.SetNumeric(ui2, 0, Convert.ToString(WAITING_RESTART_TIMER));
		m_waitingTimeNum.Rect = unitUI.GetModulePositionRect(0, 9, 38);
		int num = 0;
		players.Clear();
		Sort(player);
		List<RemotePlayer> list = null;
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			list = gameWorld.GetRemotePlayers();
			for (int i = 0; i < list.Count; i++)
			{
				Sort(list[i]);
			}
		}
		num = players.Count;
		playerScore.Clear();
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 9, 35);
		float num2 = modulePositionRect.height - 2f;
		float num3 = 0f;
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 9, 23);
		float num4 = 60f;
		for (int j = 0; j < num; j++)
		{
			UIPlayerScoreIcon uIPlayerScoreIcon = new UIPlayerScoreIcon();
			bool flag = false;
			if (player == players[j])
			{
				flag = true;
				uIPlayerScoreIcon.m_background.AddObject(unitUI, 9, 36);
				rankId = (byte)j;
			}
			else
			{
				uIPlayerScoreIcon.m_background.AddObject(unitUI, 9, 35);
			}
			uIPlayerScoreIcon.m_background.Rect = new Rect(modulePositionRect.x, modulePositionRect.y - num3, modulePositionRect.width, modulePositionRect.height);
			uIPlayerScoreIcon.m_ranking.AddObject(unitUI, 9, 23 + j);
			uIPlayerScoreIcon.m_ranking.Rect = new Rect(modulePositionRect2.x, modulePositionRect.y - num3, modulePositionRect2.width, modulePositionRect.height);
			uIPlayerScoreIcon.m_flagIcon.AddObject(ui, 1, players[j].GetSeatID());
			if (!flag)
			{
				uIPlayerScoreIcon.m_flagIcon.SetColor(new Color(UIConstant.COLOR_PLAYER_ICONS[players[j].GetSeatID()].r, UIConstant.COLOR_PLAYER_ICONS[players[j].GetSeatID()].g, UIConstant.COLOR_PLAYER_ICONS[players[j].GetSeatID()].b, 0.4f));
			}
			else
			{
				uIPlayerScoreIcon.m_flagIcon.SetColor(UIConstant.COLOR_PLAYER_ICONS[players[j].GetSeatID()]);
			}
			uIPlayerScoreIcon.m_flagIcon.Rect = new Rect(modulePositionRect2.x + num4, modulePositionRect.y - num3, 20f, modulePositionRect.height);
			uIPlayerScoreIcon.m_killsNum.AlignStyle = UINumeric.enAlignStyle.center;
			uIPlayerScoreIcon.m_killsNum.SetNumeric(unitUI, 4, UIConstant.FormatNum(players[j].VSStatistics.Kills));
			uIPlayerScoreIcon.m_killsNum.Rect = new Rect(killsTitleImg.Rect.x, modulePositionRect.y - num3, killsTitleImg.Rect.width, modulePositionRect.height);
			uIPlayerScoreIcon.m_deathNum.AlignStyle = UINumeric.enAlignStyle.center;
			uIPlayerScoreIcon.m_deathNum.SetNumeric(unitUI, 3, UIConstant.FormatNum(players[j].VSStatistics.Death));
			uIPlayerScoreIcon.m_deathNum.Rect = new Rect(deathsTitleImg.Rect.x, modulePositionRect.y - num3, deathsTitleImg.Rect.width, modulePositionRect.height);
			uIPlayerScoreIcon.m_assistsNum.AlignStyle = UINumeric.enAlignStyle.center;
			uIPlayerScoreIcon.m_assistsNum.SetNumeric(unitUI, 4, UIConstant.FormatNum(players[j].VSStatistics.Assist));
			uIPlayerScoreIcon.m_assistsNum.Rect = new Rect(assistsTitleImg.Rect.x, modulePositionRect.y - num3, assistsTitleImg.Rect.width, modulePositionRect.height);
			uIPlayerScoreIcon.m_scoreNum.AlignStyle = UINumeric.enAlignStyle.center;
			int num5 = 0;
			num5 = ((!GameApp.GetInstance().GetGameWorld().Exit) ? (GetTotalScore(players[j]) + RANK_BONUS[j]) : GetTotalScore(players[j]));
			uIPlayerScoreIcon.m_scoreNum.SetNumeric(unitUI, 3, UIConstant.FormatNum(num5));
			uIPlayerScoreIcon.m_scoreNum.Rect = new Rect(scoreTitleImg.Rect.x, modulePositionRect.y - num3, scoreTitleImg.Rect.width, modulePositionRect.height);
			uIPlayerScoreIcon.m_cashNum.AlignStyle = UINumeric.enAlignStyle.center;
			uIPlayerScoreIcon.m_cashNum.SetNumeric(unitUI, 3, UIConstant.FormatNum(players[j].VSStatistics.CashReward));
			uIPlayerScoreIcon.m_cashNum.Rect = new Rect(cashTitleImg.Rect.x, modulePositionRect.y - num3, cashTitleImg.Rect.width, modulePositionRect.height);
			if (!flag)
			{
				uIPlayerScoreIcon.SetAlpha(0.4f);
			}
			uIPlayerScoreIcon.Enable = false;
			uIPlayerScoreIcon.Show();
			Add(uIPlayerScoreIcon);
			num3 += num2;
		}
		if (GameApp.GetInstance().GetGameWorld().Exit)
		{
			userState.AddCash(player.VSStatistics.CashReward);
			continueBtn.Visible = true;
			nextRoundImg.Visible = false;
			m_waitingTimeNum.Visible = false;
			continueBtn.Enable = true;
			isRestartedGame = false;
		}
		else
		{
			isRestartedGame = true;
			m_time = DateTime.Now;
			continueBtn.Enable = false;
			continueBtn.Visible = false;
			nextRoundImg.Visible = true;
			m_waitingTimeNum.Visible = true;
		}
		skipBtn.Visible = false;
		stateMgr.m_UIManager.Add(shadowImg);
		stateMgr.m_UIManager.Add(navigationBar);
		stateMgr.m_UIManager.Add(statisticsImg);
		stateMgr.m_UIManager.Add(statisticsBGImg);
		stateMgr.m_UIManager.Add(continueBtn);
		stateMgr.m_UIManager.Add(killsTitleImg);
		stateMgr.m_UIManager.Add(deathsTitleImg);
		stateMgr.m_UIManager.Add(assistsTitleImg);
		stateMgr.m_UIManager.Add(scoreTitleImg);
		stateMgr.m_UIManager.Add(rankingTitleImg);
		stateMgr.m_UIManager.Add(cashTitleImg);
		stateMgr.m_UIManager.Add(nextRoundImg);
		stateMgr.m_UIManager.Add(m_waitingTimeNum);
		foreach (UIPlayerScoreIcon item in playerScore)
		{
			stateMgr.m_UIManager.Add(item);
		}
		stateMgr.m_UIManager.Add(twitterImg);
		stateMgr.m_UIManager.Add(facebookImg);
		stateMgr.m_UIPopupManager.Add(msgUI);
		if (ipadImg != null)
		{
			stateMgr.m_UIPopupManager.Add(ipadImg);
		}
	}

	public void Add(UIPlayerScoreIcon playerIcon)
	{
		playerIcon.SetParentEx();
		playerScore.Add(playerIcon);
	}

	public override bool Update()
	{
		switch (state)
		{
		case 0:
			isRestartedGame = true;
			player = GameApp.GetInstance().GetGameWorld().GetPlayer();
			userState = GameApp.GetInstance().GetUserState();
			state = 1;
			break;
		case 1:
		{
			Create();
			FFAState fFAState = (FFAState)GameApp.GetInstance().GetUserState().GetBattleStates()[2];
			if (GameApp.GetInstance().GetGameWorld().Exit)
			{
				fFAState.AddTotalScore(player.VSStatistics.Score);
			}
			else
			{
				fFAState.AddTotalScore(player.VSStatistics.Score + player.VSStatistics.Bonus + RANK_BONUS[rankId]);
			}
			subState = 0;
			state = 2;
			break;
		}
		case 2:
		{
			if (isRestartedGame)
			{
				TimeSpan timeSpan = DateTime.Now - m_time;
				UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
				if (unitUI != null)
				{
					int num = Mathf.Min((int)timeSpan.TotalSeconds, WAITING_RESTART_TIMER);
					m_waitingTimeNum.SetNumeric(unitUI, 0, Convert.ToString(WAITING_RESTART_TIMER - num));
				}
				if (timeSpan.TotalSeconds >= (double)WAITING_RESTART_TIMER)
				{
					RestartGame();
					isRestartedGame = false;
				}
			}
			UITouchInner[] array = iPhoneInputMgr.MockTouches();
			foreach (UITouchInner touch in array)
			{
				if (!(stateMgr.m_UIManager != null) || stateMgr.m_UIManager.HandleInput(touch))
				{
				}
			}
			break;
		}
		}
		return false;
	}

	private void Exit()
	{
		GameApp.GetInstance().Save();
		GotoNextLevel();
	}

	private void RestartGame()
	{
		if (Lobby.GetInstance().IsMasterPlayer)
		{
			ReStartGameRequest request = new ReStartGameRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	private void GotoNextLevel()
	{
		NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
		if (networkManager != null)
		{
			if (networkManager.IsConnected())
			{
				stateMgr.FrFree();
				LeaveRoomRequest request = new LeaveRoomRequest();
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				Lobby.GetInstance().IsMasterPlayer = false;
				Lobby.GetInstance().SetCurrentRoomID(-1);
				Application.LoadLevel("MultiMenu");
			}
			else
			{
				stateMgr.FrFree();
				Application.LoadLevel("StartMenu");
			}
		}
		else
		{
			stateMgr.FrFree();
			Application.LoadLevel("StartMenu");
		}
	}

	public override void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		base.HandleEvent(control, command, wparam, lparam);
		if (control == continueBtn)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			GotoNextLevel();
		}
		else if (control == navigationBar)
		{
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
			GotoNextLevel();
		}
	}

	private int GetTotalScore(Player pl)
	{
		if (GameApp.GetInstance().GetGameWorld().Exit)
		{
			return pl.VSStatistics.Score;
		}
		return pl.VSStatistics.Score + pl.VSStatistics.Bonus;
	}

	public bool Sort(Player pl)
	{
		if (players.Count == 0)
		{
			players.Add(pl);
		}
		else
		{
			float num = GetTotalScore(pl);
			int num2 = 0;
			int num3 = players.Count - 1;
			int num4 = (num2 + num3) / 2;
			if (num >= (float)GetTotalScore(players[num2]))
			{
				players.Insert(num2, pl);
			}
			else if (num <= (float)GetTotalScore(players[num3]))
			{
				players.Insert(num3 + 1, pl);
			}
			else
			{
				while (num3 - num2 > 1)
				{
					float num5 = GetTotalScore(players[num4]);
					if (num == num5)
					{
						num2 = num4;
						break;
					}
					if (num < num5)
					{
						num2 = num4;
					}
					else
					{
						num3 = num4;
					}
					num4 = (num2 + num3) / 2;
				}
				players.Insert(num2 + 1, pl);
			}
		}
		return true;
	}
}
