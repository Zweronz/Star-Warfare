using UnityEngine;

public class IAPRestoreScript : MonoBehaviour
{
	public void RestoreIAPContent(string id)
	{
		Debug.Log("id");
		switch (id)
		{
		case "com.ifreyrgames.starwarfarehd.rookie099cents":
		case "com.ifreyrgames.starwarfare.rookie099cents":
			GameApp.GetInstance().GetUserState().DeliverIAPItem(IAPName.ROOKIE);
			Debug.Log("Deliver Rookie.");
			break;
		case "com.ifreyr.starwarfarehd.sergeant299cents":
		case "com.ifreyr.starwarfare.sergeant299cents":
			GameApp.GetInstance().GetUserState().DeliverIAPItem(IAPName.SERGEANT);
			Debug.Log("Deliver SERGEANT.");
			break;
		}
	}
}
