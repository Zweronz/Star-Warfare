using UnityEngine;

public class StateWaveProcess : MonoBehaviour
{
	[SerializeField]
	private UISprite waveProcess;

	[SerializeField]
	private UISprite waveProcessBG;

	[SerializeField]
	private UISprite waveIcon;

	private void Update()
	{
		float num = UserStateUI.GetInstance().GetWaveProcess();
		waveProcess.fillAmount = num;
		float num2 = waveProcessBG.transform.localPosition.x - waveProcessBG.transform.localScale.x / 2f;
		waveIcon.transform.localPosition = new Vector3(num2 + waveProcessBG.transform.localScale.x * num, waveIcon.transform.localPosition.y, waveIcon.transform.localPosition.z);
	}
}
