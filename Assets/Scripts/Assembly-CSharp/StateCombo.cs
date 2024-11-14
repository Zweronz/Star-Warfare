using UnityEngine;

public class StateCombo : MonoBehaviour
{
	[SerializeField]
	private UILabel numLabel;

	[SerializeField]
	private GameObject numTween;

	[SerializeField]
	private GameObject comboSprite;

	[SerializeField]
	private float numLife = 2f;

	private float mUpdateTime;

	private void Start()
	{
		numTween.SetActive(false);
		comboSprite.SetActive(false);
	}

	private void Update()
	{
		int num = UserStateUI.GetInstance().PopComboNum();
		if (num > 0)
		{
			numTween.SetActive(true);
			comboSprite.SetActive(true);
			numLabel.text = string.Format("{0:D2}", num);
			GameObject gameObject = Object.Instantiate(numTween) as GameObject;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			UILabel componentInChildren = gameObject.GetComponentInChildren<UILabel>();
			componentInChildren.text = string.Format("{0:D2}", num);
			UITweenX component = gameObject.GetComponent<UITweenX>();
			component.PlayForward();
			AutoDestroyScript component2 = gameObject.GetComponent<AutoDestroyScript>();
			component2.enabled = true;
			mUpdateTime = Time.time;
		}
		if (numTween.activeSelf && Time.time - mUpdateTime > numLife)
		{
			numTween.SetActive(false);
			comboSprite.SetActive(false);
		}
	}
}
