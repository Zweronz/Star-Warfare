using UnityEngine;

public class StateJoyStick : MonoBehaviour
{
	[SerializeField]
	private Transform moveJoyStickCenter;

	[SerializeField]
	private Transform shootJoyStickCenter;

	[SerializeField]
	private Transform moveJoyStick;

	[SerializeField]
	private Transform shootJoyStick;

	[SerializeField]
	private float radius = 100f;

	private Vector3 offset = new Vector3(Screen.width / 2, Screen.height / 2, 0f);

	private bool init;

	private void Update()
	{
		if (!init)
		{
			init = true;
			UserStateUI.GetInstance().GetJoyStick().MoveJoyStickCenter = GetJoyStickCenter(moveJoyStick, moveJoyStickCenter, true);
			UserStateUI.GetInstance().GetJoyStick().ShootJoyStickCenter = GetJoyStickCenter(shootJoyStick, shootJoyStickCenter, false);
			UserStateUI.GetInstance().GetJoyStick().Radius = GetJoystickRadius();
			UserStateUI.GetInstance().GetJoyStick().InitOK = true;
		}
		bool flag = UserStateUI.GetInstance().GetMoveJoyStickPos().sqrMagnitude < radius * radius;
		moveJoyStick.localPosition = ((!flag) ? GetPT(Vector2.zero, UserStateUI.GetInstance().GetMoveJoyStickPos(), radius) : UserStateUI.GetInstance().GetMoveJoyStickPos());
		flag = UserStateUI.GetInstance().GetShootJoyStickPos().sqrMagnitude < radius * radius;
		shootJoyStick.localPosition = ((!flag) ? GetPT(Vector2.zero, UserStateUI.GetInstance().GetShootJoyStickPos(), radius) : UserStateUI.GetInstance().GetShootJoyStickPos());
	}

	private Vector2 GetJoyStickCenter(Transform joyStick, Transform center, bool left)
	{
		Vector3 localPosition = joyStick.transform.localPosition;
		Transform parent = joyStick.transform.parent;
		while (parent != null && parent.GetComponent<UIRoot>() == null)
		{
			localPosition += parent.localPosition;
			parent = parent.parent;
		}
		float num = NGUITools.FindInParents<UIRoot>(base.gameObject).activeHeight;
		localPosition = localPosition * Screen.height / num;
		return localPosition + offset;
	}

	private float GetJoystickRadius()
	{
		float num = NGUITools.FindInParents<UIRoot>(base.gameObject).activeHeight;
		return radius * (float)Screen.height / num;
	}

	private Vector2 GetPT(Vector2 s, Vector2 e, float r)
	{
		float num6;
		float num7;
		float num8;
		float num9;
		if (s.x != e.x)
		{
			if (s.y != e.y)
			{
				float num = (s.y - e.y) / (s.x - e.x);
				float num2 = s.y - num * s.x;
				float num3 = num * num + 1f;
				float num4 = 2f * num2 * num - 2f * s.y * num - 2f * s.x;
				float num5 = s.x * s.x + (num2 - s.y) * (num2 - s.y) - r * r;
				num6 = (-1f * num4 - Mathf.Sqrt(num4 * num4 - 4f * num3 * num5)) / num3 / 2f;
				num7 = (-1f * num4 + Mathf.Sqrt(num4 * num4 - 4f * num3 * num5)) / num3 / 2f;
				num8 = num * num6 + num2;
				num9 = num * num7 + num2;
			}
			else
			{
				num8 = s.y;
				num9 = s.y;
				num6 = s.x + r;
				num7 = s.x - r;
			}
		}
		else
		{
			num6 = s.x;
			num7 = s.x;
			num8 = s.y + r;
			num9 = s.y - r;
		}
		float x;
		float y;
		if ((num6 - e.x) * (num6 - e.x) + (num8 - e.y) * (num8 - e.y) > (num7 - e.x) * (num7 - e.x) + (num9 - e.y) * (num9 - e.y))
		{
			x = num7;
			y = num9;
		}
		else
		{
			x = num6;
			y = num8;
		}
		Vector2 result = default(Vector2);
		result.x = x;
		result.y = y;
		return result;
	}
}
