using System.Collections.Generic;
using UnityEngine;

public class StateSkill : ComponentUI
{
	[SerializeField]
	private GameObject templeteSkillIcon;

	[SerializeField]
	private UIGrid skillGrid;

	private List<SWSkill> mSkillList = new List<SWSkill>();

	protected override void OnInit()
	{
		base.OnInit();
		List<UserStateUI.SkillUI> skillList = UserStateUI.GetInstance().GetSkillList();
		for (int i = 0; i < skillList.Count; i++)
		{
			SWSkill sWSkill = new SWSkill();
			sWSkill.SkillUI = skillList[i];
			sWSkill.IsJustCoolDown = true;
			GameObject gameObject = Object.Instantiate(templeteSkillIcon) as GameObject;
			gameObject.name = "Skill" + skillList[i].SkillId;
			sWSkill.SkillButton = gameObject;
			gameObject.transform.parent = skillGrid.gameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.SetActive(true);
			AddButtonListener(gameObject);
			mSkillList.Add(sWSkill);
			skillGrid.transform.localPosition += new Vector3(0f, skillGrid.cellHeight * (float)i, 0f);
			Debug.Log("go.name : " + gameObject.name);
		}
		skillGrid.repositionNow = true;
	}

	private void Update()
	{
		foreach (SWSkill mSkill in mSkillList)
		{
			if (mSkill.SkillUI.IsUse)
			{
				mSkill.SkillSprite.fillAmount = 1f - mSkill.SkillUI.PercentOfUsingTime;
			}
			else if (mSkill.SkillUI.IsCoolDown)
			{
				mSkill.IsJustCoolDown = false;
				mSkill.SkillSprite.fillAmount = mSkill.SkillUI.PercentOfCoolDownTime;
			}
			else if (!mSkill.IsJustCoolDown && mSkill.SkillUI.IsJustCoolDownFinish)
			{
				mSkill.IsJustCoolDown = true;
				GameObject gameObject = Object.Instantiate(mSkill.SkillButton) as GameObject;
				gameObject.transform.parent = skillGrid.gameObject.transform;
				gameObject.transform.localPosition = mSkill.SkillButton.transform.localPosition;
				gameObject.transform.localEulerAngles = mSkill.SkillButton.transform.localEulerAngles;
				gameObject.transform.localScale = mSkill.SkillButton.transform.localScale;
				UITweenX component = gameObject.GetComponent<UITweenX>();
				component.PlayForward();
				AutoDestroyScript component2 = gameObject.GetComponent<AutoDestroyScript>();
				component2.enabled = true;
			}
		}
		if (!Application.isMobilePlatform)
		{
			//I have no idea how many skills you can have
			KeyCode[] skillKeys = new KeyCode[6]
			{
				KeyCode.F,
				KeyCode.G,
				KeyCode.H,
				KeyCode.J,
				KeyCode.K,
				KeyCode.L
			};
			for (int i = 0; i < skillKeys.Length; i++)
			{
				if (Input.GetKeyDown(skillKeys[i]) && mSkillList.Count > i)
				{
					OnClickThumb(mSkillList[i].SkillButton);
				}
			}
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		foreach (SWSkill mSkill in mSkillList)
		{
			if (go.Equals(mSkill.SkillButton))
			{
				Debug.Log("swSkill : " + mSkill.SkillUI.SkillId);
				GameUITouchEvent gameUITouchEvent = new GameUITouchEvent();
				gameUITouchEvent.EventID = TouchEventID.HUD_Skill;
				gameUITouchEvent.EventAction = TouchEventAction.Click;
				gameUITouchEvent.ArgObject = mSkill.SkillUI.SkillId;
				AddTouchEventToGameUI(gameUITouchEvent);
				break;
			}
		}
	}
}
