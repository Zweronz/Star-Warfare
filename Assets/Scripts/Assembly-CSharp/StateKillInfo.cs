using UnityEngine;

public class StateKillInfo : MonoBehaviour
{
	[SerializeField]
	private UISprite killer;

	[SerializeField]
	private UISprite action;

	[SerializeField]
	private UISprite killed;

	private float mAlpha;

	private float mFadeOutSpeed = 0.2f;

	private void Awake()
	{
		mAlpha = 1f;
	}

	public bool FadeOut()
	{
		SetAlpha(killer, mAlpha);
		SetAlpha(action, mAlpha);
		SetAlpha(killed, mAlpha);
		mAlpha -= mFadeOutSpeed * Time.deltaTime;
		return mAlpha > 0f;
	}

	private void SetAlpha(UISprite sprite, float a)
	{
		sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, a);
	}

	public void Set(int killerId, HUDAction actionId, int killedId)
	{
		killer.spriteName = GetSpriteName(killerId);
		killed.spriteName = GetSpriteName(killedId);
		action.spriteName = GetActionSpriteName(actionId);
		if (UserStateUI.GetInstance().IsTeamMode())
		{
			switch (actionId)
			{
			case HUDAction.SECURE_VIP:
			case HUDAction.BECOME_VIP:
			case HUDAction.HIT_GIFT:
				killer.color = UIConstant.COLOR_TEAM_VIP_PLAYER_ICONS[killerId / 4];
				killed.color = killer.color;
				break;
			case HUDAction.KILL_VIP:
				killer.color = UIConstant.COLOR_TEAM_PLAYER_ICONS[killerId / 4];
				killed.color = UIConstant.COLOR_TEAM_VIP_PLAYER_ICONS[killedId / 4];
				break;
			case HUDAction.VIP_KILL:
				killer.color = UIConstant.COLOR_TEAM_VIP_PLAYER_ICONS[killerId / 4];
				killed.color = UIConstant.COLOR_TEAM_PLAYER_ICONS[killedId / 4];
				break;
			default:
				killer.color = UIConstant.COLOR_TEAM_PLAYER_ICONS[killerId / 4];
				if (actionId == HUDAction.KILL)
				{
					killed.color = UIConstant.COLOR_TEAM_PLAYER_ICONS[killedId / 4];
				}
				else
				{
					killed.color = killer.color;
				}
				break;
			}
		}
		else
		{
			killer.color = UIConstant.COLOR_PLAYER_ICONS[killerId];
			if (actionId == HUDAction.KILL)
			{
				killed.color = UIConstant.COLOR_PLAYER_ICONS[killedId];
			}
			else
			{
				killed.color = killer.color;
			}
		}
		killer.MakePixelPerfect();
		killed.MakePixelPerfect();
		action.MakePixelPerfect();
	}

	private string GetSpriteName(int id)
	{
		switch (id)
		{
		case 8:
			return "Untitled-35 copy";
		case 9:
			return "hud_115";
		case 10:
			return "hud_117";
		default:
			return "p" + (1 + id);
		}
	}

	private string GetActionSpriteName(HUDAction actionId)
	{
		switch (actionId)
		{
		case HUDAction.KILL:
			return "Untitled-32 copy";
		case HUDAction.SECURE_FLAG:
			return "Untitled-34 copy";
		case HUDAction.DROP_FLAG:
			return "Untitled-36 copy";
		case HUDAction.CATCH_FLAG:
			return "Untitled-37 copy";
		case HUDAction.KILL_VIP:
			return "Untitled-32 copy";
		case HUDAction.VIP_KILL:
			return "Untitled-32 copy";
		case HUDAction.SECURE_VIP:
			return "Untitled-34 copy";
		case HUDAction.BECOME_VIP:
			return "Untitled-64 copy";
		case HUDAction.HIT_GIFT:
			return "Untitled-37 copy";
		default:
			return string.Empty;
		}
	}
}
