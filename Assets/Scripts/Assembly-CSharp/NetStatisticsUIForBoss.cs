using System;
using System.Collections.Generic;
using UnityEngine;

public class NetStatisticsUIForBoss : NetStatisticsUIBase
{
	private const byte SUBSTATE_BOSS = 0;

	private const byte SUBSTATE_MONSTER = 1;

	private const byte SUBSTATE_PICKUP = 2;

	private const byte SUBSTATE_BONUS = 3;

	private const byte SUBSTATE_OVER = 4;

	private const byte SUBSTATE_UNLOCK = 5;

	private UIImage rankingTitleImg;

	private UIImage monsterTitleImg;

	private UIImage bossTitleImg;

	private UIImage pickupTitleImg;

	private UIImage energyTitleImg;

	private UIImage bonusTitleImg;

	private UIImage[] playerFlagImg;

	private UIImage[] rankingImg;

	private UIImage[] bossCashBGImg;

	private UINumeric[] bossCashNum;

	private UIImage[] monsterCashBGImg;

	private UINumeric[] monsterCashNum;

	private UIImage[] pickupCashBGImg;

	private UINumeric[] pickupCashNum;

	private UIImage[] pickupEnergyBGImg;

	private UINumeric[] pickupEnergyNum;

	private UIImage[] bonusBGImg;

	private UINumeric[] bonusNum;

	private UIImage cashImg;

	private UIImage[] cashMarqueeImg;

	private UIImage[] cashMarqueeMaskImg;

	private UIMarquee[] marqueeNum;

	private static byte[] CONGRATULATION = new byte[2] { 0, 1 };

	private UIDialog newsTxt;

	private UIImage congratulationImg;

	private bool unlock;

	private FrUIText tipsTxt;

	private int totalExp;

	private int totalCash;

	private int totalMithril;

	private int curRank;

	private int curCash;

	private int curMaxCash;

	private int curMarqueeIndex;

	private bool bWillStop;

	private byte subState;

	private static byte[] MARQUEE_VELOCITY = new byte[9] { 5, 6, 7, 8, 9, 10, 11, 12, 13 };

	private static float MARQUEE_ACCELERATION = 10f;

	private DateTime time;

	private static float[] EXTRA_BONUS = new float[3] { 0.2f, 0.15f, 0.1f };

	private float soundFreq;

	public NetStatisticsUIForBoss(UIStateManager stateMgr)
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
	}

	public override void Create()
	{
		base.Create();
		UnitUI unitUI = Res2DManager.GetInstance().vUI[15];
		UnitUI unitUI2 = Res2DManager.GetInstance().vUI[27];
		UnitUI unitUI3 = Res2DManager.GetInstance().vUI[22];
		byte[] module = new byte[2] { 21, 22 };
		rankingTitleImg = new UIImage();
		rankingTitleImg.AddObject(unitUI, 7, module);
		rankingTitleImg.Rect = rankingTitleImg.GetObjectRect();
		bossTitleImg = new UIImage();
		bossTitleImg.AddObject(unitUI, 7, 58 + userState.GetNetStage() - Global.TOTAL_STAGE);
		bossTitleImg.Rect = bossTitleImg.GetObjectRect();
		monsterTitleImg = new UIImage();
		monsterTitleImg.AddObject(unitUI, 7, 64);
		monsterTitleImg.Rect = monsterTitleImg.GetObjectRect();
		pickupTitleImg = new UIImage();
		pickupTitleImg.AddObject(unitUI, 7, 66);
		pickupTitleImg.Rect = pickupTitleImg.GetObjectRect();
		bonusTitleImg = new UIImage();
		bonusTitleImg.AddObject(unitUI, 7, 26);
		bonusTitleImg.Rect = bonusTitleImg.GetObjectRect();
		int num = 0;
		int num2 = 0;
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
		for (int j = 0; j < players.Count; j++)
		{
			AddExtraBonus(players[j]);
			if (players[j] == player)
			{
				num2 = j;
			}
		}
		int num3 = 60;
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 7, 31);
		rankingImg = new UIImage[num];
		for (int k = 0; k < num; k++)
		{
			rankingImg[k] = new UIImage();
			rankingImg[k].AddObject(unitUI, 7, 31 + k);
			rankingImg[k].Rect = new Rect(modulePositionRect.x, modulePositionRect.y - (float)(k * num3), modulePositionRect.width, modulePositionRect.height);
		}
		Rect modulePositionRect2 = unitUI2.GetModulePositionRect(0, 0, 0);
		playerFlagImg = new UIImage[num];
		for (int l = 0; l < num; l++)
		{
			playerFlagImg[l] = new UIImage();
			playerFlagImg[l].AddObject(unitUI2, 0, players[l].GetSeatID());
			playerFlagImg[l].SetColor(UIConstant.COLOR_PLAYER_ICONS[players[l].GetSeatID()]);
			playerFlagImg[l].Rect = new Rect(modulePositionRect.x + 40f, modulePositionRect.y - 10f - (float)(l * num3), modulePositionRect2.width, modulePositionRect2.height);
			if (num2 == l)
			{
				playerFlagImg[l].SetSize(new Vector2(playerFlagImg[l].Rect.width * 1.5f, playerFlagImg[l].Rect.height * 1.5f));
			}
		}
		Rect modulePositionRect3 = unitUI.GetModulePositionRect(0, 7, 27);
		bossCashBGImg = new UIImage[num];
		for (int m = 0; m < num; m++)
		{
			bossCashBGImg[m] = new UIImage();
			bossCashBGImg[m].AddObject(unitUI, 7, 27);
			bossCashBGImg[m].Rect = new Rect(modulePositionRect3.x, modulePositionRect3.y - (float)(m * num3), modulePositionRect3.width, modulePositionRect3.height);
		}
		bossCashNum = new UINumeric[num];
		for (int n = 0; n < num; n++)
		{
			bossCashNum[n] = new UINumeric();
			bossCashNum[n].AlignStyle = UINumeric.enAlignStyle.center;
			bossCashNum[n].SetNumeric(unitUI, 3, UIConstant.FormatNum(players[n].GetBossCash()));
			bossCashNum[n].Rect = bossCashBGImg[n].Rect;
		}
		Rect modulePositionRect4 = unitUI.GetModulePositionRect(0, 7, 28);
		monsterCashBGImg = new UIImage[num];
		for (int num4 = 0; num4 < num; num4++)
		{
			monsterCashBGImg[num4] = new UIImage();
			monsterCashBGImg[num4].AddObject(unitUI, 7, 28);
			monsterCashBGImg[num4].Rect = new Rect(modulePositionRect4.x, modulePositionRect4.y - (float)(num4 * num3), modulePositionRect4.width, modulePositionRect4.height);
		}
		monsterCashNum = new UINumeric[num];
		for (int num5 = 0; num5 < num; num5++)
		{
			monsterCashNum[num5] = new UINumeric();
			monsterCashNum[num5].AlignStyle = UINumeric.enAlignStyle.center;
			monsterCashNum[num5].SetNumeric(unitUI, 3, UIConstant.FormatNum(players[num5].GetMonsterCash()));
			monsterCashNum[num5].Rect = monsterCashBGImg[num5].Rect;
		}
		Rect modulePositionRect5 = unitUI.GetModulePositionRect(0, 7, 29);
		pickupCashBGImg = new UIImage[num];
		for (int num6 = 0; num6 < num; num6++)
		{
			pickupCashBGImg[num6] = new UIImage();
			pickupCashBGImg[num6].AddObject(unitUI, 7, 29);
			pickupCashBGImg[num6].Rect = new Rect(modulePositionRect5.x, modulePositionRect5.y - (float)(num6 * num3), modulePositionRect5.width, modulePositionRect5.height);
		}
		pickupCashNum = new UINumeric[num];
		for (int num7 = 0; num7 < num; num7++)
		{
			pickupCashNum[num7] = new UINumeric();
			pickupCashNum[num7].AlignStyle = UINumeric.enAlignStyle.center;
			pickupCashNum[num7].SetNumeric(unitUI, 3, UIConstant.FormatNum(players[num7].GetBossMithril()));
			pickupCashNum[num7].Rect = pickupCashBGImg[num7].Rect;
		}
		Rect modulePositionRect6 = unitUI.GetModulePositionRect(0, 7, 30);
		bonusBGImg = new UIImage[num];
		for (int num8 = 0; num8 < num; num8++)
		{
			bonusBGImg[num8] = new UIImage();
			bonusBGImg[num8].AddObject(unitUI, 7, 30);
			bonusBGImg[num8].Rect = new Rect(modulePositionRect6.x, modulePositionRect6.y - (float)(num8 * num3), modulePositionRect6.width, modulePositionRect6.height);
		}
		bonusNum = new UINumeric[num];
		for (int num9 = 0; num9 < num; num9++)
		{
			bonusNum[num9] = new UINumeric();
			bonusNum[num9].AlignStyle = UINumeric.enAlignStyle.center;
			bonusNum[num9].SetNumeric(unitUI, 3, UIConstant.FormatNum(players[num9].GetBounsCash() + players[num9].GetPickupCash()));
			bonusNum[num9].Rect = bonusBGImg[num9].Rect;
		}
		curRank = userState.GetRank(userState.GetExp()).rankID;
		List<Rank> rankList = userState.GetRankList();
		totalExp = userState.GetExp() + player.GetExp();
		totalExp = Mathf.Min(totalExp, rankList[rankList.Count - 1].exp);
		cashImg = new UIImage();
		cashImg.AddObject(unitUI, 7, 43, 3);
		cashImg.Rect = cashImg.GetObjectRect();
		cashMarqueeImg = new UIImage[3];
		for (int num10 = 0; num10 < 3; num10++)
		{
			cashMarqueeImg[num10] = new UIImage();
			cashMarqueeImg[num10].AddObject(unitUI, 7, 46 + num10);
			cashMarqueeImg[num10].Rect = cashMarqueeImg[num10].GetObjectRect();
		}
		cashMarqueeMaskImg = new UIImage[9];
		for (int num11 = 0; num11 < 9; num11++)
		{
			cashMarqueeMaskImg[num11] = new UIImage();
			cashMarqueeMaskImg[num11].AddObject(unitUI, 7, 49 + num11);
			cashMarqueeMaskImg[num11].Rect = cashMarqueeMaskImg[num11].GetObjectRect();
		}
		marqueeNum = new UIMarquee[9];
		Rect modulePositionRect7 = unitUI.GetModulePositionRect(0, 1, 0);
		for (int num12 = 0; num12 < 9; num12++)
		{
			marqueeNum[num12] = new UIMarquee();
			marqueeNum[num12].Create(unitUI);
			for (int num13 = 0; num13 < 10; num13++)
			{
				UIMarquee.UINumIcon uINumIcon = new UIMarquee.UINumIcon();
				uINumIcon.m_background = new UIImage();
				uINumIcon.m_background.AddObject(unitUI, 1, num13);
				uINumIcon.m_background.Rect = uINumIcon.m_background.GetObjectRect();
				uINumIcon.Visible = false;
				uINumIcon.Enable = false;
				uINumIcon.Rect = uINumIcon.m_background.Rect;
				marqueeNum[num12].Add(uINumIcon);
			}
			marqueeNum[num12].SetClipRect(cashMarqueeMaskImg[num12].Rect.x, cashMarqueeMaskImg[num12].Rect.y, cashMarqueeMaskImg[num12].Rect.width, cashMarqueeMaskImg[num12].Rect.height);
			float num14 = modulePositionRect7.height + 5f;
			marqueeNum[num12].SetMarquee(0f, 10f * num14, num14);
			marqueeNum[num12].SetSelection(0);
		}
		totalCash = userState.GetCash() + player.GetBossCash() + player.GetMonsterCash() + player.GetBounsCash() + player.GetPickupCash();
		totalCash = Mathf.Min(totalCash, 999999999);
		totalMithril = userState.GetMithril() + player.GetBossMithril();
		totalMithril = Mathf.Min(totalMithril, 999999999);
		congratulationImg = new UIImage();
		congratulationImg.AddObject(unitUI, 6, CONGRATULATION);
		congratulationImg.Rect = congratulationImg.GetObjectRect();
		congratulationImg.Visible = false;
		Rect modulePositionRect8 = unitUI.GetModulePositionRect(0, 6, 2);
		newsTxt = new UIDialog(stateMgr, 0);
		newsTxt.Create();
		Rect modulePositionRect9 = unitUI.GetModulePositionRect(0, 6, 2);
		newsTxt.SetTextShowRect(modulePositionRect9.x, modulePositionRect9.y, modulePositionRect9.width, modulePositionRect9.height);
		newsTxt.Hide();
		newsTxt.SetBlock(false);
		navigationBar.SetDisabledForBackBtn();
		stateMgr.m_UIManager.Add(shadowImg);
		stateMgr.m_UIManager.Add(navigationBar);
		stateMgr.m_UIManager.Add(statisticsImg);
		stateMgr.m_UIManager.Add(statisticsBGImg);
		stateMgr.m_UIManager.Add(skipBtn);
		stateMgr.m_UIManager.Add(continueBtn);
		stateMgr.m_UIManager.Add(rankingTitleImg);
		stateMgr.m_UIManager.Add(bossTitleImg);
		stateMgr.m_UIManager.Add(monsterTitleImg);
		stateMgr.m_UIManager.Add(pickupTitleImg);
		stateMgr.m_UIManager.Add(bonusTitleImg);
		UIImage[] array = rankingImg;
		foreach (UIImage control in array)
		{
			stateMgr.m_UIManager.Add(control);
		}
		UIImage[] array2 = playerFlagImg;
		foreach (UIImage control2 in array2)
		{
			stateMgr.m_UIManager.Add(control2);
		}
		UIImage[] array3 = bossCashBGImg;
		foreach (UIImage control3 in array3)
		{
			stateMgr.m_UIManager.Add(control3);
		}
		UINumeric[] array4 = bossCashNum;
		foreach (UINumeric control4 in array4)
		{
			stateMgr.m_UIManager.Add(control4);
		}
		UIImage[] array5 = monsterCashBGImg;
		foreach (UIImage control5 in array5)
		{
			stateMgr.m_UIManager.Add(control5);
		}
		UINumeric[] array6 = monsterCashNum;
		foreach (UINumeric control6 in array6)
		{
			stateMgr.m_UIManager.Add(control6);
		}
		UIImage[] array7 = pickupCashBGImg;
		foreach (UIImage control7 in array7)
		{
			stateMgr.m_UIManager.Add(control7);
		}
		UINumeric[] array8 = pickupCashNum;
		foreach (UINumeric control8 in array8)
		{
			stateMgr.m_UIManager.Add(control8);
		}
		UIImage[] array9 = bonusBGImg;
		foreach (UIImage control9 in array9)
		{
			stateMgr.m_UIManager.Add(control9);
		}
		UINumeric[] array10 = bonusNum;
		foreach (UINumeric control10 in array10)
		{
			stateMgr.m_UIManager.Add(control10);
		}
		stateMgr.m_UIManager.Add(cashImg);
		UIImage[] array11 = cashMarqueeImg;
		foreach (UIImage control11 in array11)
		{
			stateMgr.m_UIManager.Add(control11);
		}
		UIMarquee[] array12 = marqueeNum;
		foreach (UIMarquee control12 in array12)
		{
			stateMgr.m_UIManager.Add(control12);
		}
		UIImage[] array13 = cashMarqueeMaskImg;
		foreach (UIImage control13 in array13)
		{
			stateMgr.m_UIManager.Add(control13);
		}
		stateMgr.m_UIManager.Add(congratulationImg);
		stateMgr.m_UIManager.Add(newsTxt);
		tipsTxt = new FrUIText();
		tipsTxt.AlignStyle = FrUIText.enAlignStyle.TOP_LEFT;
		tipsTxt.Set("font2", UIConstant.GetMessage(37), UIConstant.FONT_COLOR_TIPS, UIConstant.ScreenLocalWidth - 175f);
		tipsTxt.Rect = new Rect(160f, 0f, UIConstant.ScreenLocalWidth - 175f, 80f);
		if (player.GetBossMithril() > 0)
		{
			tipsTxt.Visible = true;
		}
		else
		{
			tipsTxt.Visible = false;
		}
		stateMgr.m_UIManager.Add(tipsTxt);
		stateMgr.m_UIManager.Add(twitterImg);
		stateMgr.m_UIManager.Add(facebookImg);
		stateMgr.m_UIPopupManager.Add(msgUI);
		if (ipadImg != null)
		{
			stateMgr.m_UIPopupManager.Add(ipadImg);
		}
		SetBossCashVisible(false);
		SetMonsterCashVisible(false);
		SetPickupCashVisible(false);
		SetBonusVisible(false);
		userState.UnLockEquip();
		userState.SetCash(totalCash);
		userState.SetExp(totalExp);
		userState.SetMithril(totalMithril);
		GameApp.GetInstance().Save();
	}

	public void SetBossCashVisible(bool visible)
	{
		UIImage[] array = bossCashBGImg;
		foreach (UIImage uIImage in array)
		{
			uIImage.Visible = visible;
		}
		UINumeric[] array2 = bossCashNum;
		foreach (UINumeric uINumeric in array2)
		{
			uINumeric.Visible = visible;
		}
	}

	public void SetMonsterCashVisible(bool visible)
	{
		UIImage[] array = monsterCashBGImg;
		foreach (UIImage uIImage in array)
		{
			uIImage.Visible = visible;
		}
		UINumeric[] array2 = monsterCashNum;
		foreach (UINumeric uINumeric in array2)
		{
			uINumeric.Visible = visible;
		}
	}

	public void SetPickupCashVisible(bool visible)
	{
		UIImage[] array = pickupCashBGImg;
		foreach (UIImage uIImage in array)
		{
			uIImage.Visible = visible;
		}
		UINumeric[] array2 = pickupCashNum;
		foreach (UINumeric uINumeric in array2)
		{
			uINumeric.Visible = visible;
		}
	}

	public void SetPickupEnergyVisible(bool visible)
	{
		UIImage[] array = pickupEnergyBGImg;
		foreach (UIImage uIImage in array)
		{
			uIImage.Visible = visible;
		}
		UINumeric[] array2 = pickupEnergyNum;
		foreach (UINumeric uINumeric in array2)
		{
			uINumeric.Visible = visible;
		}
	}

	public void SetBonusVisible(bool visible)
	{
		UIImage[] array = bonusBGImg;
		foreach (UIImage uIImage in array)
		{
			uIImage.Visible = visible;
		}
		UINumeric[] array2 = bonusNum;
		foreach (UINumeric uINumeric in array2)
		{
			uINumeric.Visible = visible;
		}
	}

	public override bool Update()
	{
		switch (state)
		{
		case 0:
			player = GameApp.GetInstance().GetGameWorld().GetPlayer();
			userState = GameApp.GetInstance().GetUserState();
			curCash = 0;
			curMaxCash = 0;
			state = 1;
			break;
		case 1:
			Create();
			subState = 0;
			CalcExp();
			CalcCash();
			state = 2;
			break;
		case 2:
		{
			if (subState == 0)
			{
				if (UpdateExp() && UpdateCash() && (DateTime.Now - time).TotalSeconds > 1.5)
				{
					subState = 1;
					CalcCash();
				}
			}
			else if (subState == 1)
			{
				if (UpdateCash() && (DateTime.Now - time).TotalSeconds > 1.5)
				{
					subState = 2;
					CalcCash();
				}
			}
			else if (subState == 2)
			{
				if (UpdateCash() && (DateTime.Now - time).TotalSeconds > 1.5)
				{
					subState = 3;
					CalcCash();
				}
			}
			else if (subState == 3 && UpdateCash())
			{
				subState = 4;
				bWillStop = false;
				curMarqueeIndex = 0;
				continueBtn.Enable = true;
			}
			if (subState < 4 && marqueeNum[8].GetVelocity() != 0f)
			{
				soundFreq += Mathf.Abs(marqueeNum[8].GetVelocity());
				if (soundFreq > marqueeNum[8].m_spacing)
				{
					AudioManager.GetInstance().PlaySound(AudioName.MONEY_UP);
					soundFreq = 0f;
				}
			}
			UITouchInner[] array = iPhoneInputMgr.MockTouches();
			foreach (UITouchInner touch in array)
			{
				if (!(stateMgr.m_UIManager != null) || stateMgr.m_UIManager.HandleInput(touch))
				{
				}
			}
			newsTxt.m_text.Rect = newsTxt.GetTextShowRect();
			break;
		}
		}
		return false;
	}

	public void CalcCash()
	{
		switch (subState)
		{
		case 0:
			curCash = 0;
			curMaxCash = player.GetBossCash();
			bWillStop = true;
			curMarqueeIndex = curMaxCash.ToString().Length;
			PlayMarquee();
			SetBossCashVisible(true);
			break;
		case 1:
			curCash = curMaxCash;
			curMaxCash = player.GetMonsterCash() + player.GetBossCash();
			bWillStop = true;
			curMarqueeIndex = curMaxCash.ToString().Length;
			PlayMarquee();
			SetMonsterCashVisible(true);
			break;
		case 2:
			curCash = curMaxCash;
			curMaxCash = player.GetMonsterCash() + player.GetBossCash();
			bWillStop = false;
			curMarqueeIndex = 0;
			SetPickupCashVisible(true);
			break;
		case 3:
			curCash = curMaxCash;
			curMaxCash = player.GetMonsterCash() + player.GetBounsCash() + player.GetPickupCash() + player.GetBossCash();
			bWillStop = true;
			curMarqueeIndex = curMaxCash.ToString().Length;
			PlayMarquee();
			SetBonusVisible(true);
			break;
		}
		time = DateTime.Now;
	}

	public void CalcExp()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[15];
		List<Rank> rankList = userState.GetRankList();
		if (userState.GetExp() < rankList[rankList.Count - 1].exp)
		{
			int val = userState.GetExp() - rankList[curRank].exp;
			int exp = rankList[curRank + 1].exp;
			exp -= rankList[curRank].exp;
			string text = UIConstant.FormatNum(exp);
			string text2 = UIConstant.FormatNum(val);
		}
	}

	public bool UpdateExp()
	{
		userState.SetExp(totalExp);
		if (userState.GetRank(userState.GetExp()).rankID != curRank)
		{
			curRank = userState.GetRank(userState.GetExp()).rankID;
			unlock = IsUnlockEquip();
		}
		return true;
	}

	public void AddExtraBonus(Player pl)
	{
		int num = 0;
		foreach (Player player in players)
		{
			num += player.GetMonsterCash();
		}
		for (int i = 0; i < players.Count; i++)
		{
			if (pl == players[i])
			{
				int bounsCash = pl.GetBounsCash();
				pl.SetBounsCash((int)((float)bounsCash + (float)num * EXTRA_BONUS[i]));
				break;
			}
		}
	}

	public void PlayMarquee()
	{
		int num = 9 - curMarqueeIndex;
		UnitUI ui = Res2DManager.GetInstance().vUI[15];
		for (int i = 0; i < 9; i++)
		{
			if (subState == 0)
			{
				if (i >= num)
				{
					marqueeNum[i].Play(-MARQUEE_VELOCITY[i], 0f - MARQUEE_ACCELERATION, -1);
					marqueeNum[i].m_numsLst[0].m_background.SetTexture(ui, 1, 10);
				}
			}
			else if (i > num)
			{
				marqueeNum[i].Play(-MARQUEE_VELOCITY[i], 0f - MARQUEE_ACCELERATION, -1);
				marqueeNum[i].m_numsLst[0].m_background.SetTexture(ui, 1, 10);
			}
		}
	}

	public bool UpdateCash()
	{
		bool result = false;
		TimeSpan timeSpan = DateTime.Now - time;
		if (curMarqueeIndex <= 0)
		{
			return true;
		}
		if (subState == 0 && timeSpan.TotalSeconds < 1.0)
		{
			return false;
		}
		int num = 9 - curMarqueeIndex;
		if (bWillStop)
		{
			int value = GetValue(curMaxCash, curMarqueeIndex);
			float velocity = marqueeNum[num].GetVelocity();
			int num2 = (int)((float)value * marqueeNum[num].m_spacing);
			int num3 = 0;
			num3 = ((!(marqueeNum[num].GetMarqueePos() >= (float)num2)) ? ((int)(marqueeNum[num].GetMarqueeMaxPos() - (float)num2 + marqueeNum[num].GetMarqueePos())) : ((int)(marqueeNum[num].GetMarqueePos() - (float)num2)));
			float num4 = (0f - velocity * velocity) / (2f * (float)num3) * 60f;
			if (num3 != 0)
			{
				marqueeNum[num].Play(0f - (float)num3 / 10f, 10f, value);
			}
			else
			{
				marqueeNum[num].SetSelection(value);
				marqueeNum[num].Stop();
			}
			bWillStop = false;
		}
		if (marqueeNum[num].GetSelection() == GetValue(curMaxCash, curMarqueeIndex))
		{
			curMarqueeIndex--;
			bWillStop = true;
			if (curMarqueeIndex <= 0)
			{
				curCash = curMaxCash;
				time = DateTime.Now;
				return true;
			}
		}
		return result;
	}

	public int GetValue(int val, int index)
	{
		int num = (int)Mathf.Pow(10f, index);
		val %= num;
		val /= num / 10;
		return val;
	}

	private void Exit()
	{
		userState.UnLockEquip();
		userState.SetCash(totalCash);
		userState.SetExp(totalExp);
		userState.SetMithril(totalMithril);
		GameApp.GetInstance().Save();
		if (player.Hp <= 0)
		{
			userState.AtomicDeadTimer();
			if (userState.GetDeadTimer() == 0)
			{
				int num = Random.Range(0, 2);
				string msg = UIConstant.GetMessage(19 + num).Replace("[n]", "\n");
				msgUI.CreateQuery(msg, MessageBoxUI.MESSAGE_FLAG_QUERY, MessageBoxUI.EVENT_DEAD_TIPS);
				msgUI.Show();
				return;
			}
		}
		GotoNextLevel();
	}

	private void GotoNextLevel()
	{
		NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
		if (networkManager != null)
		{
			if (networkManager.IsConnected())
			{
				stateMgr.FrFree();
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
		if (control == skipBtn)
		{
			if (subState == 5)
			{
				return;
			}
			subState = 4;
			SetMonsterCashVisible(true);
			SetPickupCashVisible(true);
			SetBonusVisible(true);
			SetBossCashVisible(true);
			userState.SetCash(totalCash);
			userState.SetExp(totalExp);
			userState.SetMithril(totalMithril);
			UpdateExp();
			unlock = IsUnlockEquip();
			curMaxCash = player.GetMonsterCash() + player.GetBounsCash() + player.GetPickupCash() + player.GetBossCash();
			curMarqueeIndex = curMaxCash.ToString().Length;
			UnitUI ui = Res2DManager.GetInstance().vUI[15];
			int num = 9 - curMarqueeIndex;
			for (int i = 0; i < 9; i++)
			{
				if (i >= num)
				{
					int value = GetValue(curMaxCash, curMarqueeIndex);
					marqueeNum[i].SetSelection(value);
					marqueeNum[i].Stop();
					marqueeNum[i].m_numsLst[0].m_background.SetTexture(ui, 1, 10);
					curMarqueeIndex--;
				}
				else
				{
					marqueeNum[i].m_numsLst[0].m_background.SetTexture(ui, 1, 0);
				}
			}
			continueBtn.Enable = true;
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
		}
		else if (control == continueBtn)
		{
			if (subState == 4)
			{
				if (unlock)
				{
					subState = 5;
					SetMonsterCashVisible(false);
					SetPickupCashVisible(false);
					SetBossCashVisible(false);
					bossTitleImg.Visible = false;
					SetBonusVisible(false);
					cashImg.Visible = false;
					rankingTitleImg.Visible = false;
					monsterTitleImg.Visible = false;
					pickupTitleImg.Visible = false;
					bonusTitleImg.Visible = false;
					UIImage[] array = rankingImg;
					foreach (UIImage uIImage in array)
					{
						uIImage.Visible = false;
					}
					UIImage[] array2 = cashMarqueeImg;
					foreach (UIImage uIImage2 in array2)
					{
						uIImage2.Visible = false;
					}
					UIMarquee[] array3 = marqueeNum;
					foreach (UIMarquee uIMarquee in array3)
					{
						uIMarquee.Visible = false;
					}
					UIImage[] array4 = cashMarqueeMaskImg;
					foreach (UIImage uIImage3 in array4)
					{
						uIImage3.Visible = false;
					}
					UIImage[] array5 = playerFlagImg;
					foreach (UIImage uIImage4 in array5)
					{
						uIImage4.Visible = false;
					}
					string text = string.Empty;
					List<Armor> armorListForUnLock = userState.GetArmorListForUnLock();
					for (int num2 = 0; num2 < armorListForUnLock.Count; num2++)
					{
						Armor armor = armorListForUnLock[num2];
						text = text + armor.Name + " UNLOCK\n";
					}
					List<Weapon> weaponListForUnLock = userState.GetWeaponListForUnLock();
					for (int num3 = 0; num3 < weaponListForUnLock.Count; num3++)
					{
						Weapon weapon = weaponListForUnLock[num3];
						text = text + weapon.Name + " UNLOCK\n";
					}
					char[] trimChars = new char[1] { '\n' };
					text = text.Trim(trimChars);
					newsTxt.SetText("font2", text, UIConstant.fontColor_white, newsTxt.GetTextShowRect().width, FrUIText.enAlignStyle.TOP_CENTER);
					newsTxt.Rect = newsTxt.GetTextShowRect();
					newsTxt.Show();
					congratulationImg.Visible = true;
				}
				else
				{
					Exit();
				}
			}
			else if (subState == 5)
			{
				Exit();
			}
			AudioManager.GetInstance().PlaySound(AudioName.CLICK);
		}
		else
		{
			if (control != msgUI)
			{
				return;
			}
			int eventID = msgUI.GetEventID();
			if (eventID == MessageBoxUI.EVENT_DEAD_TIPS)
			{
				AudioManager.GetInstance().PlaySound(AudioName.CLICK);
				msgUI.Hide();
				if (command == 10)
				{
					stateMgr.FrFree();
					userState.bGotoIAP = true;
					UIConstant.GotoShopAndCustomize(2, 4);
					Application.LoadLevel("ShopAndCustomize");
				}
				else
				{
					GotoNextLevel();
				}
			}
		}
	}

	private int GetTotalCash(Player pl)
	{
		return pl.GetBounsCash() + pl.GetPickupCash() + pl.GetMonsterCash() + pl.GetBossCash();
	}

	private bool IsUnlockEquip()
	{
		List<Armor> armorListForUnLock = userState.GetArmorListForUnLock();
		List<Weapon> weaponListForUnLock = userState.GetWeaponListForUnLock();
		if (armorListForUnLock.Count > 0 || weaponListForUnLock.Count > 0)
		{
			return true;
		}
		return false;
	}

	public bool Sort(Player pl)
	{
		if (players.Count == 0)
		{
			players.Add(pl);
		}
		else
		{
			float num = GetTotalCash(pl);
			int num2 = 0;
			int num3 = players.Count - 1;
			int num4 = (num2 + num3) / 2;
			if (num >= (float)GetTotalCash(players[num2]))
			{
				players.Insert(num2, pl);
			}
			else if (num <= (float)GetTotalCash(players[num3]))
			{
				players.Insert(num3 + 1, pl);
			}
			else
			{
				while (num3 - num2 > 1)
				{
					float num5 = GetTotalCash(players[num4]);
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
