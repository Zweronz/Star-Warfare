using UnityEngine;

public class SWSkill
{
	private UISprite mSkillShadow;

	private UISprite mSkillSprite;

	private GameObject mSkillButton;

	public bool IsJustCoolDown { get; set; }

	public UserStateUI.SkillUI SkillUI { get; set; }

	public GameObject SkillButton
	{
		get
		{
			return mSkillButton;
		}
		set
		{
			mSkillButton = value;
			int skillIconId = GetSkillIconId(SkillUI.SkillId);
			UISprite[] componentsInChildren = mSkillButton.GetComponentsInChildren<UISprite>(true);
			UISprite[] array = componentsInChildren;
			foreach (UISprite uISprite in array)
			{
				if (uISprite.gameObject.name.Equals("SkillSprite"))
				{
					mSkillSprite = uISprite;
					mSkillSprite.spriteName = "skill_" + (skillIconId + 1);
					mSkillSprite.MakePixelPerfect();
				}
				if (uISprite.gameObject.name.Equals("SkillShadow"))
				{
					mSkillShadow = uISprite;
					mSkillShadow.spriteName = "skill_" + skillIconId;
					mSkillShadow.MakePixelPerfect();
				}
			}
		}
	}

	public UISprite SkillShadow
	{
		get
		{
			return mSkillShadow;
		}
	}

	public UISprite SkillSprite
	{
		get
		{
			return mSkillSprite;
		}
	}

	private int GetSkillIconId(int skillId)
	{
		Debug.Log("haoc GetSkillIconId");
		switch (skillId)
		{
		case 0:
			return 0;
		case 1:
			return 2;
		case 2:
			return 4;
		case 3:
			return 6;
		case 4:
			return 8;
		case 5:
			return 10;
		case 6:
			return 12;
		case 7:
			return 14;
		case 8:
			return 16;
		case 9:
			return 18;
		default:
			return 0;
		}
	}
}
