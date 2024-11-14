using UnityEngine;

public class StateWinTime : MonoBehaviour
{
	[SerializeField]
	private UISprite typeFillIcon;

	[SerializeField]
	private UISprite typeIcon;

	[SerializeField]
	private UILabel totalTime;

	[SerializeField]
	private TweenScale tweenScale;

	private UserStateUI.TimeInfoUI.TimeType mTimeType;

	private int mTeam = -1;

	private void Awake()
	{
		typeFillIcon.gameObject.SetActive(false);
	}

	private void Update()
	{
		UserStateUI.TimeInfoUI timeInfo = UserStateUI.GetInstance().GetTimeInfo();
		int time = timeInfo.Time1;
		if (time < 0)
		{
			time = 0;
			totalTime.text = "--:--";
			if (tweenScale.enabled)
			{
				tweenScale.enabled = false;
				totalTime.MakePixelPerfect();
			}
		}
		else
		{
			int num = time / 60;
			int num2 = time - num * 60;
			totalTime.text = string.Format("{0:D2}", num) + ":" + string.Format("{0:D2}", num2);
			if (num2 <= 10 && (timeInfo.Type == UserStateUI.TimeInfoUI.TimeType.Flag || timeInfo.Type == UserStateUI.TimeInfoUI.TimeType.VIP))
			{
				if (!tweenScale.enabled)
				{
					tweenScale.enabled = true;
				}
			}
			else if (tweenScale.enabled)
			{
				tweenScale.enabled = false;
				totalTime.MakePixelPerfect();
			}
		}
		if (mTimeType != timeInfo.Type)
		{
			mTimeType = timeInfo.Type;
			typeIcon.spriteName = GetSpriteName(mTimeType);
			typeIcon.MakePixelPerfect();
		}
		if (mTeam != timeInfo.Team)
		{
			mTeam = timeInfo.Team;
			typeIcon.color = GetSpriteColor(mTeam, timeInfo.Type);
			totalTime.color = GetSpriteColor(mTeam, timeInfo.Type);
			typeFillIcon.color = GetSpriteColor(mTeam, timeInfo.Type);
		}
		if (timeInfo.IsIconFillAmountOn)
		{
			if (!typeFillIcon.gameObject.activeSelf)
			{
				typeFillIcon.gameObject.SetActive(true);
				typeFillIcon.spriteName = GetFilledSpriteName(mTimeType);
				typeFillIcon.MakePixelPerfect();
			}
			typeFillIcon.fillAmount = timeInfo.IconFillAmount;
		}
	}

	private string GetSpriteName(UserStateUI.TimeInfoUI.TimeType type)
	{
		switch (type)
		{
		case UserStateUI.TimeInfoUI.TimeType.Clock:
			return "Untitled-33 copy";
		case UserStateUI.TimeInfoUI.TimeType.Flag:
			return "hud_12";
		case UserStateUI.TimeInfoUI.TimeType.VIP:
			return "hud_116";
		default:
			return string.Empty;
		}
	}

	private string GetFilledSpriteName(UserStateUI.TimeInfoUI.TimeType type)
	{
		if (type == UserStateUI.TimeInfoUI.TimeType.VIP)
		{
			return "hud_115";
		}
		return string.Empty;
	}

	private Color GetSpriteColor(int team, UserStateUI.TimeInfoUI.TimeType type)
	{
		if (type == UserStateUI.TimeInfoUI.TimeType.VIP)
		{
			switch (team)
			{
			case -1:
				return Color.white;
			case 0:
				return UIConstant.COLOR_TEAM_VIP_PLAYER_ICONS[0];
			default:
				return UIConstant.COLOR_TEAM_VIP_PLAYER_ICONS[1];
			}
		}
		switch (team)
		{
		case -1:
			return Color.white;
		case 0:
			return UIConstant.COLOR_TEAM_PLAYER_ICONS[0];
		default:
			return UIConstant.COLOR_TEAM_PLAYER_ICONS[1];
		}
	}
}
