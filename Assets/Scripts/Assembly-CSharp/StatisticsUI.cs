using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StatisticsUI : StatisticsBaseUI
{
	private const byte SUBSTATE_EXP = 0;

	private const byte SUBSTATE_COMBO = 1;

	private const byte SUBSTATE_PICKUP = 2;

	private const byte SUBSTATE_OVER = 3;

	private const byte SUBSTATE_UNLOCK = 4;

	private const byte SUBSTATE_UNLOCK_STAGE = 5;

	private static byte[] EXP_TITLE = new byte[2] { 23, 65 };

	private UIImage expShadowImg;

	private UIImage expBarBGImg;

	private UIImage expBarImg;

	private UIImage expImg;

	private UINumeric expNum;

	private static byte[] COMBO_TITLE = new byte[2] { 29, 66 };

	private UIImage comboShadowImg;

	private UIImage comboImg;

	private UIImage bonusImg;

	private UIImage maxComboImg;

	private UINumeric bonusNum;

	private UINumeric maxComboNum;

	private static byte[] PICKUP_TITLE = new byte[2] { 36, 67 };

	private UIImage pickupShadowImg;

	private UIImage pickupImg;

	private UIImage pickupCashImg;

	private UINumeric pickupCashNum;

	private UIImage pickupEnergyImg;

	private UINumeric pickupEnergyNum;

	private UIImage cashImg;

	private UIImage[] cashMarqueeImg;

	private UIImage[] cashMarqueeMaskImg;

	private UIMarquee[] marqueeNum;

	private static byte[] UNLOCK_EQUIP = new byte[2] { 0, 1 };

	private static byte[] UNLOCK_STAGE = new byte[5] { 0, 1, 2, 3, 4 };

	private UIDialog newsTxt;

	private UIImage unlockEquipImg;

	private bool unlock;

	private bool unlockStage;

	private UIImage unlockStageImg;

	private UINumeric unlockStageAwardNum;

	private int totalExp;

	private int totalCash;

	private int curRank;

	private int curCash;

	private int curMaxCash;

	private int curMarqueeIndex;

	private bool bWillStop;

	private byte subState;

	private static byte[] MARQUEE_VELOCITY = new byte[9] { 5, 6, 7, 8, 9, 10, 11, 12, 13 };

	private static float MARQUEE_ACCELERATION = 10f;

	private DateTime time;

	private float soundFreq;

	public StatisticsUI(UIStateManager stateMgr)
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
		Rect modulePositionRect = unitUI.GetModulePositionRect(0, 0, 21);
		expShadowImg = new UIImage();
		expShadowImg.AddObject(unitUI, 0, 22);
		expShadowImg.Rect = expShadowImg.GetObjectRect();
		expShadowImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		expImg = new UIImage();
		expImg.AddObject(unitUI, 0, EXP_TITLE);
		expImg.Rect = expImg.GetObjectRect();
		expBarBGImg = new UIImage();
		expBarBGImg.AddObject(unitUI, 0, 24);
		expBarBGImg.Rect = expBarBGImg.GetObjectRect();
		expBarImg = new UIImage();
		expBarImg.AddObject(unitUI, 0, 25);
		expBarImg.Rect = expBarImg.GetObjectRect();
		curRank = userState.GetRank(userState.GetExp()).rankID;
		List<Rank> rankList = userState.GetRankList();
		totalExp = userState.GetExp() + player.GetExp();
		totalExp = Mathf.Min(totalExp, rankList[rankList.Count - 1].exp);
		expNum = new UINumeric();
		expNum.AlignStyle = UINumeric.enAlignStyle.right;
		expNum.SetNumeric(unitUI, 2, "000,000,000");
		expNum.Rect = unitUI.GetModulePositionRect(0, 0, 26);
		modulePositionRect = unitUI.GetModulePositionRect(0, 0, 27);
		comboShadowImg = new UIImage();
		comboShadowImg.AddObject(unitUI, 0, 28);
		comboShadowImg.Rect = comboShadowImg.GetObjectRect();
		comboShadowImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		comboImg = new UIImage();
		comboImg.AddObject(unitUI, 0, COMBO_TITLE);
		comboImg.Rect = comboImg.GetObjectRect();
		bonusImg = new UIImage();
		bonusImg.AddObject(unitUI, 0, 30);
		bonusImg.Rect = bonusImg.GetObjectRect();
		bonusNum = new UINumeric();
		bonusNum.AlignStyle = UINumeric.enAlignStyle.left;
		bonusNum.SetNumeric(unitUI, 3, UIConstant.FormatNum(player.GetBounsCash()));
		bonusNum.Rect = unitUI.GetModulePositionRect(0, 0, 31);
		maxComboImg = new UIImage();
		maxComboImg.AddObject(unitUI, 0, 32);
		maxComboImg.Rect = maxComboImg.GetObjectRect();
		maxComboNum = new UINumeric();
		maxComboNum.AlignStyle = UINumeric.enAlignStyle.left;
		maxComboNum.SetNumeric(unitUI, 5, player.GetMaxCombo().ToString());
		maxComboNum.Rect = unitUI.GetModulePositionRect(0, 0, 33);
		modulePositionRect = unitUI.GetModulePositionRect(0, 0, 34);
		pickupShadowImg = new UIImage();
		pickupShadowImg.AddObject(unitUI, 0, 35);
		pickupShadowImg.Rect = pickupShadowImg.GetObjectRect();
		pickupShadowImg.SetSize(new Vector2(modulePositionRect.width, modulePositionRect.height));
		pickupImg = new UIImage();
		pickupImg.AddObject(unitUI, 0, PICKUP_TITLE);
		pickupImg.Rect = pickupImg.GetObjectRect();
		pickupCashImg = new UIImage();
		pickupCashNum = new UINumeric();
		pickupEnergyImg = new UIImage();
		pickupEnergyNum = new UINumeric();
		pickupCashImg.AddObject(unitUI, 0, 37);
		pickupCashImg.Rect = pickupCashImg.GetObjectRect();
		pickupCashNum.AlignStyle = UINumeric.enAlignStyle.left;
		pickupCashNum.SetNumeric(unitUI, 3, UIConstant.FormatNum(player.GetPickupCash()));
		pickupCashNum.Rect = unitUI.GetModulePositionRect(0, 0, 38);
		pickupEnergyImg.AddObject(unitUI, 0, 39);
		pickupEnergyImg.Rect = pickupEnergyImg.GetObjectRect();
		pickupEnergyNum.AlignStyle = UINumeric.enAlignStyle.left;
		pickupEnergyNum.SetNumeric(unitUI, 4, UIConstant.FormatNum(player.GetPickupEnegy()));
		pickupEnergyNum.Rect = unitUI.GetModulePositionRect(0, 0, 40);
		cashImg = new UIImage();
		cashImg.AddObject(unitUI, 0, 50, 3);
		cashImg.Rect = cashImg.GetObjectRect();
		cashMarqueeImg = new UIImage[3];
		for (int i = 0; i < 3; i++)
		{
			cashMarqueeImg[i] = new UIImage();
			cashMarqueeImg[i].AddObject(unitUI, 0, 53 + i);
			cashMarqueeImg[i].Rect = cashMarqueeImg[i].GetObjectRect();
		}
		cashMarqueeMaskImg = new UIImage[9];
		for (int j = 0; j < 9; j++)
		{
			cashMarqueeMaskImg[j] = new UIImage();
			cashMarqueeMaskImg[j].AddObject(unitUI, 0, 56 + j);
			cashMarqueeMaskImg[j].Rect = cashMarqueeMaskImg[j].GetObjectRect();
		}
		marqueeNum = new UIMarquee[9];
		Rect modulePositionRect2 = unitUI.GetModulePositionRect(0, 1, 0);
		for (int k = 0; k < 9; k++)
		{
			marqueeNum[k] = new UIMarquee();
			marqueeNum[k].Create(unitUI);
			for (int l = 0; l < 10; l++)
			{
				UIMarquee.UINumIcon uINumIcon = new UIMarquee.UINumIcon();
				uINumIcon.m_background = new UIImage();
				uINumIcon.m_background.AddObject(unitUI, 1, l);
				uINumIcon.m_background.Rect = uINumIcon.m_background.GetObjectRect();
				uINumIcon.Visible = false;
				uINumIcon.Enable = false;
				uINumIcon.Rect = uINumIcon.m_background.Rect;
				marqueeNum[k].Add(uINumIcon);
			}
			marqueeNum[k].SetClipRect(cashMarqueeMaskImg[k].Rect.x, cashMarqueeMaskImg[k].Rect.y, cashMarqueeMaskImg[k].Rect.width, cashMarqueeMaskImg[k].Rect.height);
			float num = modulePositionRect2.height + 5f;
			marqueeNum[k].SetMarquee(0f, 10f * num, num);
			marqueeNum[k].SetSelection(0);
		}
		totalCash = userState.GetCash() + player.GetMonsterCash() + player.GetBounsCash() + player.GetPickupCash() + player.GetBossCash();
		totalCash = Mathf.Min(totalCash, 999999999);
		unlockEquipImg = new UIImage();
		unlockEquipImg.AddObject(unitUI, 6, UNLOCK_EQUIP);
		unlockEquipImg.Rect = unlockEquipImg.GetObjectRect();
		unlockEquipImg.Visible = false;
		Rect modulePositionRect3 = unitUI.GetModulePositionRect(0, 6, 2);
		newsTxt = new UIDialog(stateMgr, 0);
		newsTxt.Create();
		Rect modulePositionRect4 = unitUI.GetModulePositionRect(0, 6, 2);
		newsTxt.SetTextShowRect(modulePositionRect4.x, modulePositionRect4.y, modulePositionRect4.width, modulePositionRect4.height);
		newsTxt.Hide();
		newsTxt.SetBlock(false);
		unlockStageImg = new UIImage();
		unlockStageImg.AddObject(unitUI, 8, UNLOCK_STAGE);
		unlockStageImg.Rect = unlockStageImg.GetObjectRect();
		unlockStageImg.Visible = false;
		unlockStageAwardNum = new UINumeric();
		unlockStageAwardNum.AlignStyle = UINumeric.enAlignStyle.center;
		unlockStageAwardNum.SetNumeric(unitUI, 5, Convert.ToString(0));
		unlockStageAwardNum.Rect = unitUI.GetModulePositionRect(0, 8, 5);
		unlockStageAwardNum.Visible = false;
		unlockStage = IsUnlockStage();
		navigationBar.SetDisabledForBackBtn();
		stateMgr.m_UIManager.Add(shadowImg);
		stateMgr.m_UIManager.Add(navigationBar);
		stateMgr.m_UIManager.Add(statisticsImg);
		stateMgr.m_UIManager.Add(statisticsBGImg);
		stateMgr.m_UIManager.Add(skipBtn);
		stateMgr.m_UIManager.Add(continueBtn);
		stateMgr.m_UIManager.Add(expShadowImg);
		stateMgr.m_UIManager.Add(expBarBGImg);
		stateMgr.m_UIManager.Add(expBarImg);
		stateMgr.m_UIManager.Add(expImg);
		stateMgr.m_UIManager.Add(expNum);
		stateMgr.m_UIManager.Add(comboShadowImg);
		stateMgr.m_UIManager.Add(comboImg);
		stateMgr.m_UIManager.Add(bonusImg);
		stateMgr.m_UIManager.Add(maxComboImg);
		stateMgr.m_UIManager.Add(bonusNum);
		stateMgr.m_UIManager.Add(maxComboNum);
		stateMgr.m_UIManager.Add(pickupShadowImg);
		stateMgr.m_UIManager.Add(pickupImg);
		stateMgr.m_UIManager.Add(pickupCashImg);
		stateMgr.m_UIManager.Add(pickupCashNum);
		stateMgr.m_UIManager.Add(pickupEnergyImg);
		stateMgr.m_UIManager.Add(pickupEnergyNum);
		stateMgr.m_UIManager.Add(cashImg);
		UIImage[] array = cashMarqueeImg;
		foreach (UIImage control in array)
		{
			stateMgr.m_UIManager.Add(control);
		}
		UIMarquee[] array2 = marqueeNum;
		foreach (UIMarquee control2 in array2)
		{
			stateMgr.m_UIManager.Add(control2);
		}
		UIImage[] array3 = cashMarqueeMaskImg;
		foreach (UIImage control3 in array3)
		{
			stateMgr.m_UIManager.Add(control3);
		}
		stateMgr.m_UIManager.Add(unlockEquipImg);
		stateMgr.m_UIManager.Add(newsTxt);
		stateMgr.m_UIManager.Add(unlockStageImg);
		stateMgr.m_UIManager.Add(unlockStageAwardNum);
		stateMgr.m_UIManager.Add(twitterImg);
		stateMgr.m_UIManager.Add(facebookImg);
		stateMgr.m_UIPopupManager.Add(msgUI);
		if (ipadImg != null)
		{
			stateMgr.m_UIPopupManager.Add(ipadImg);
		}
		SetExpVisible(false);
		SetComboVisible(false);
		SetPickupVisible(false);
	}

	public void SetExpVisible(bool visible)
	{
		expShadowImg.Visible = visible;
		expBarBGImg.Visible = visible;
		expBarImg.Visible = visible;
		expImg.Visible = visible;
		expNum.Visible = visible;
	}

	public void SetComboVisible(bool visible)
	{
		comboShadowImg.Visible = visible;
		comboImg.Visible = visible;
		bonusImg.Visible = visible;
		maxComboImg.Visible = visible;
		bonusNum.Visible = visible;
		maxComboNum.Visible = visible;
	}

	public void SetPickupVisible(bool visible)
	{
		pickupShadowImg.Visible = visible;
		pickupImg.Visible = visible;
		pickupCashImg.Visible = visible;
		pickupCashNum.Visible = visible;
		pickupEnergyImg.Visible = visible;
		pickupEnergyNum.Visible = visible;
	}

	public override bool Update()
	{
		switch (state)
		{
		case 0:
			player = GameApp.GetInstance().GetGameWorld().GetPlayer();
			userState = GameApp.GetInstance().GetUserState();
			state = 1;
			break;
		case 1:
			subState = 0;
			Create();
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
			else if (subState == 2 && UpdateCash())
			{
				subState = 3;
				bWillStop = false;
				curMarqueeIndex = 0;
				continueBtn.Enable = true;
			}
			if (subState < 3 && marqueeNum[8].GetVelocity() != 0f)
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
			soundFreq = 0f;
			curCash = 0;
			curMaxCash = player.GetMonsterCash() + player.GetBossCash();
			bWillStop = true;
			curMarqueeIndex = curMaxCash.ToString().Length;
			PlayMarquee();
			SetExpVisible(true);
			break;
		case 1:
			curCash = curMaxCash;
			curMaxCash = player.GetMonsterCash() + player.GetBossCash() + player.GetBounsCash();
			bWillStop = true;
			curMarqueeIndex = curMaxCash.ToString().Length;
			PlayMarquee();
			SetComboVisible(true);
			break;
		case 2:
			curCash = curMaxCash;
			curMaxCash = player.GetMonsterCash() + player.GetBossCash() + player.GetBounsCash() + player.GetPickupCash();
			bWillStop = true;
			curMarqueeIndex = curMaxCash.ToString().Length;
			PlayMarquee();
			SetPickupVisible(true);
			break;
		}
		time = DateTime.Now;
	}

	public void CalcExp()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[15];
		List<Rank> rankList = userState.GetRankList();
		if (userState.GetExp() >= rankList[rankList.Count - 1].exp)
		{
			int exp = rankList[rankList.Count - 1].exp;
			string text = string.Format("{0:0,0}", exp);
			expNum.SetNumeric(ui, 2, text + "/" + text);
			expBarImg.ClearClip();
			return;
		}
		int num = userState.GetExp() - rankList[curRank].exp;
		int exp2 = rankList[curRank + 1].exp;
		exp2 -= rankList[curRank].exp;
		string text2 = UIConstant.FormatNum(exp2);
		string text3 = UIConstant.FormatNum(num);
		int num2 = (int)((float)num * expBarBGImg.Rect.width / (float)exp2);
		expBarImg.SetClipOffs(0, new Vector2(10f, 0f));
		expBarImg.SetClipOffs(1, new Vector2(0f, 0f));
		expBarImg.SetClipOffs(2, new Vector2(-10f, 0f));
		expBarImg.SetClipOffs(3, new Vector2(0f, 0f));
		expBarImg.SetClip(new Rect(expBarImg.Rect.x, expBarImg.Rect.y, num2, expBarImg.Rect.height));
		expNum.SetNumeric(ui, 2, text3 + "/" + text2);
	}

	public bool UpdateExp()
	{
		bool result = false;
		if (userState.GetExp() < totalExp)
		{
			int b = (int)((float)player.GetExp() * 0.02f);
			b = Mathf.Max(1, b);
			userState.AddExp(b);
		}
		if (userState.GetExp() >= totalExp)
		{
			userState.SetExp(totalExp);
			result = true;
		}
		CalcExp();
		if (userState.GetRank(userState.GetExp()).rankID != curRank)
		{
			curRank = userState.GetRank(userState.GetExp()).rankID;
			unlock = IsUnlockEquip();
		}
		return result;
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
		Time.timeScale = 1f;
		base.userState.UnLockEquip();
		Lobby.GetInstance().IsMasterPlayer = false;
		base.userState.SetCash(totalCash);
		base.userState.SetExp(totalExp);
		if (GameApp.GetInstance().GetGameWorld().State == GameState.GameOverUIWin)
		{
			UserState userState = GameApp.GetInstance().GetUserState();
			int num = userState.GetStage();
			if (num < Global.TOTAL_STAGE)
			{
				int subStage = userState.GetSubStage();
				int num2 = num * Global.TOTAL_SUB_STAGE + subStage;
				subStage++;
				if (subStage == Global.TOTAL_SUB_STAGE - 1)
				{
					switch (num)
					{
					case 0:
						base.userState.Achievement.CheckAchievement_FirstLevel();
						base.userState.Achievement.SubmitAllToGameCenter();
						break;
					case 2:
						base.userState.Achievement.CheckAchievement_ThreeLevels();
						base.userState.Achievement.SubmitAllToGameCenter();
						break;
					case 4:
						base.userState.Achievement.CheckAchievement_FiveLevels();
						base.userState.Achievement.SubmitAllToGameCenter();
						break;
					}
					int stage_Idx = (num + 1) % Global.TOTAL_STAGE;
					if (num2 > userState.GetCompletedLevelId())
					{
						base.userState.AddCash(UIConstant.SUCCESS_STAGE_AWARD[num]);
					}
					userState.SetStageState(stage_Idx, 0, 1);
					userState.SetStageState(stage_Idx, 5, 1);
					num++;
					subStage = 0;
					num2++;
				}
				subStage %= Global.TOTAL_SUB_STAGE;
				num %= Global.TOTAL_STAGE;
				if (userState.GetCompletedLevelId() < num2)
				{
					userState.SetCompletedLevelId(num2);
				}
				userState.SetStage((byte)num);
				userState.SetSubStage((byte)subStage);
				userState.SetStageState(num, subStage, 1);
			}
		}
		GameApp.GetInstance().Save();
		if (player.Hp <= 0)
		{
			base.userState.AtomicDeadTimer();
		}
		stateMgr.FrFree();
		Application.LoadLevel("SoloMenu");
	}

	public override void HandleEvent(UIControl control, int command, float wparam, float lparam)
	{
		base.HandleEvent(control, command, wparam, lparam);
		if (control == skipBtn)
		{
			if (subState == 4 || subState == 5)
			{
				return;
			}
			subState = 3;
			SetExpVisible(true);
			SetComboVisible(true);
			SetPickupVisible(true);
			userState.SetCash(totalCash);
			userState.SetExp(totalExp);
			CalcExp();
			unlock = IsUnlockEquip();
			curMaxCash = player.GetMonsterCash() + player.GetBossCash() + player.GetBounsCash() + player.GetPickupCash();
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
			if (subState == 3)
			{
				if (unlock)
				{
					subState = 4;
					SetStatisticsPanel(false);
					UnlockEquip();
				}
				else if (unlockStage)
				{
					subState = 5;
					SetStatisticsPanel(false);
					UnlockStage();
				}
				else
				{
					Exit();
				}
			}
			else if (subState == 4)
			{
				if (unlockStage)
				{
					subState = 5;
					SetUnlockEquipPanel(false);
					UnlockStage();
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
				stateMgr.FrFree();
				if (command == 10)
				{
					UIConstant.GotoShopAndCustomize(1, 4);
					userState.bGotoIAP = true;
					Application.LoadLevel("ShopAndCustomize");
				}
				else
				{
					Application.LoadLevel("SoloMenu");
				}
			}
			else if (eventID == MessageBoxUI.EVENT_CAN_NOT_ACCESS_INTERNET)
			{
				AudioManager.GetInstance().PlaySound(AudioName.CLICK);
				if (command == 9)
				{
					msgUI.Hide();
				}
			}
		}
	}

	private void SetStatisticsPanel(bool visible)
	{
		SetExpVisible(visible);
		SetComboVisible(visible);
		SetPickupVisible(visible);
		cashImg.Visible = visible;
		UIImage[] array = cashMarqueeImg;
		foreach (UIImage uIImage in array)
		{
			uIImage.Visible = visible;
		}
		UIMarquee[] array2 = marqueeNum;
		foreach (UIMarquee uIMarquee in array2)
		{
			uIMarquee.Visible = visible;
		}
		UIImage[] array3 = cashMarqueeMaskImg;
		foreach (UIImage uIImage2 in array3)
		{
			uIImage2.Visible = visible;
		}
	}

	private void SetUnlockEquipPanel(bool visible)
	{
		newsTxt.Visible = visible;
		unlockEquipImg.Visible = visible;
	}

	private void UnlockEquip()
	{
		string text = string.Empty;
		List<Armor> armorListForUnLock = userState.GetArmorListForUnLock();
		for (int i = 0; i < armorListForUnLock.Count; i++)
		{
			Armor armor = armorListForUnLock[i];
			text = text + armor.Name + " UNLOCK\n";
		}
		List<Weapon> weaponListForUnLock = userState.GetWeaponListForUnLock();
		for (int j = 0; j < weaponListForUnLock.Count; j++)
		{
			Weapon weapon = weaponListForUnLock[j];
			text = text + weapon.Name + " UNLOCK\n";
		}
		char[] trimChars = new char[1] { '\n' };
		text = text.Trim(trimChars);
		newsTxt.SetText("font2", text, UIConstant.fontColor_white, newsTxt.GetTextShowRect().width, FrUIText.enAlignStyle.TOP_CENTER);
		newsTxt.Rect = newsTxt.GetTextShowRect();
		newsTxt.Show();
		unlockEquipImg.Visible = true;
	}

	private void UnlockStage()
	{
		UnitUI ui = Res2DManager.GetInstance().vUI[15];
		unlockStageAwardNum.SetNumeric(ui, 5, Convert.ToString(UIConstant.SUCCESS_STAGE_AWARD[GameApp.GetInstance().GetUserState().GetStage()]));
		unlockStageAwardNum.Visible = true;
		unlockStageImg.Visible = true;
	}

	private bool IsUnlockStage()
	{
		if (GameApp.GetInstance().GetGameWorld().State == GameState.GameOverUIWin)
		{
			UserState userState = GameApp.GetInstance().GetUserState();
			int stage = userState.GetStage();
			int subStage = userState.GetSubStage();
			subStage++;
			int num = stage * Global.TOTAL_SUB_STAGE + subStage;
			if (subStage == Global.TOTAL_SUB_STAGE - 1 && userState.GetCompletedLevelId() < num)
			{
				return true;
			}
		}
		return false;
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
}
