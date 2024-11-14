using UnityEngine;

[RequireComponent(typeof(UISlider))]
[AddComponentMenu("NGUI/Interaction/Sound Volume")]
public class UISoundVolume : MonoBehaviour
{
	private UISlider mSlider;

	private void Awake()
	{
		mSlider = GetComponent<UISlider>();
		mSlider.sliderValue = NGUITools.soundVolume;
		mSlider.eventReceiver = base.gameObject;
	}

	private void OnSliderChange(float val)
	{
		NGUITools.soundVolume = val;
	}
}
