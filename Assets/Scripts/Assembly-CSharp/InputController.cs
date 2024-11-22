using System.Collections;
using UnityEngine;

public class InputController
{
	public InputInfo inputInfo = new InputInfo();

	public InputInfo previousInputInfo = new InputInfo();

	protected Vector3 moveDirection = Vector3.zero;

	protected TouchInfo lastMoveTouch = new TouchInfo();

	protected TouchInfo lastMoveTouch2 = new TouchInfo();

	protected Hashtable sensitivityMap = new Hashtable();

	protected float sensitivityFactor = 1f;

	protected Touch[] lastTouch = new Touch[2];

	public Vector2 cameraRotation = new Vector2(0f, 0f);

	protected Vector2 deflection;

	protected Vector2 thumbCenter;

	protected Vector2 thumbCenterToScreen;

	protected Vector2 shootThumbCenter;

	protected Vector2 shootThumbCenterToScreen;

	protected Vector2 lastShootTouch = default(Vector2);

	protected float touchX;

	protected float touchY;

	protected float thumbRadius;

	protected int thumbTouchFingerId = -1;

	protected int shootingTouchFingerId = -1;

	protected int moveTouchFingerId = -1;

	protected int moveTouchFingerId2 = -1;

	protected string phaseStr = ".";

	protected GameWorld gameScene;

	protected Player player;

	protected Rect blockRect;

	protected bool bBlock;

	protected bool bBlockCamera;

	public bool up;

	public bool down;

	public bool fire;

	public float x;

	public float y;

	protected float initSensitivityFactor = 1f;

	private bool joyStickInit;

	public bool Block
	{
		get
		{
			return bBlock;
		}
		set
		{
			bBlock = value;
		}
	}

	public bool BlockCamera
	{
		get
		{
			return bBlockCamera;
		}
		set
		{
			bBlockCamera = value;
		}
	}

	public Vector2 CameraRotation
	{
		get
		{
			return cameraRotation;
		}
		set
		{
			cameraRotation = value;
		}
	}

	public Vector2 ThumbCenterToScreen
	{
		get
		{
			return thumbCenterToScreen;
		}
	}

	public Vector2 LastTouchPos
	{
		get
		{
			return new Vector2(thumbCenterToScreen.x + touchX * thumbRadius, thumbCenterToScreen.y + touchY * thumbRadius);
		}
	}

	public Vector2 LastShootTouch
	{
		get
		{
			return lastShootTouch;
		}
	}

	public Vector2 ShootThumbCenterToScreen
	{
		get
		{
			return shootThumbCenterToScreen;
		}
	}

	public float ThumbRadius
	{
		get
		{
			return thumbRadius;
		}
	}

	public void ResetSensitivity(InputSensitivity sensitivity)
	{
		sensitivityFactor = (float)sensitivityMap[sensitivity];
	}

	public void Init()
	{
		thumbCenterToScreen = UserStateUI.GetInstance().GetJoyStick().MoveJoyStickCenter;
		thumbCenterToScreen = UserStateUI.GetInstance().GetJoyStick().ShootJoyStickCenter;
		thumbRadius = UserStateUI.GetInstance().GetJoyStick().Radius;
		sensitivityMap.Add(InputSensitivity.Fast, 1.5f);
		sensitivityMap.Add(InputSensitivity.Normal, 1f);
		sensitivityMap.Add(InputSensitivity.Slow, 0.5f);
		InputSensitivity touchInputSensitivity = GameApp.GetInstance().GetUserState().TouchInputSensitivity;
		ResetSensitivity(touchInputSensitivity);
		initSensitivityFactor = sensitivityFactor;
		lastShootTouch = shootThumbCenterToScreen;
		for (int i = 0; i < 2; i++)
		{
			lastTouch[i] = default(Touch);
		}
		gameScene = GameApp.GetInstance().GetGameWorld();
		player = gameScene.GetPlayer();
		bBlock = false;
	}

	private Vector3 KeyboardInput()
	{
		Vector3 input = new Vector3
		(
			Input.GetKey(KeyCode.A)  ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0,
			0f,
			Input.GetKey(KeyCode.S)  ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0
		);

		if (input.x != 0 && input.z != 0)
		{
			input.x *= 0.5f;
			input.z *= 0.5f;
		}

		return input;
	}

	public void Process()
	{
		if (!joyStickInit)
		{
			if (UserStateUI.GetInstance().GetJoyStick().InitOK)
			{
				thumbCenterToScreen = UserStateUI.GetInstance().GetJoyStick().MoveJoyStickCenter;
				shootThumbCenterToScreen = UserStateUI.GetInstance().GetJoyStick().ShootJoyStickCenter;
				thumbRadius = UserStateUI.GetInstance().GetJoyStick().Radius;
				joyStickInit = true;
				lastShootTouch = shootThumbCenterToScreen;
				Debug.Log("thumbCenterToScreen : " + thumbCenterToScreen);
				Debug.Log("shootThumbCenterToScreen : " + shootThumbCenterToScreen);
				Debug.Log("thumbRadius : " + thumbRadius);
			}
			return;
		}
		if (bBlock)
		{
			cameraRotation = Vector2.zero;
			inputInfo.moving = false;
			inputInfo.fire = false;
			moveDirection = Vector3.zero;
			return;
		}
		bool flag = false;
		if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
		{
			if (Input.GetButton("Fire1") && Screen.lockCursor)
			{
				inputInfo.fire = true;
			}
			else
			{
				inputInfo.fire = false;
			}
			moveDirection = KeyboardInput();
		}
		else
		{
			player = GameApp.GetInstance().GetGameWorld().GetPlayer();
			touchX = 0f;
			touchY = 0f;
			cameraRotation.x = 0f;
			cameraRotation.y = 0f;
			inputInfo.fire = false;
			inputInfo.moving = false;
			if (Input.touchCount == 0)
			{
				thumbTouchFingerId = -1;
				shootingTouchFingerId = -1;
				lastShootTouch = shootThumbCenterToScreen;
			}
			sensitivityFactor = initSensitivityFactor;
			if (player.GetWeapon().GetWeaponType() == WeaponType.Sniper || player.GetWeapon().GetWeaponType() == WeaponType.AdvancedSniper || player.GetWeapon().GetWeaponType() == WeaponType.RelectionSniper)
			{
				Sniper sniper = player.GetWeapon() as Sniper;
				if (sniper.isAiming())
				{
					sensitivityFactor = initSensitivityFactor * 0.5f;
				}
			}
			bool flag2 = false;
			for (int i = 0; i < Input.touchCount && i != 2; i++)
			{
				Touch touch = Input.GetTouch(i);
				flag = blockRect.Contains(touch.position);
				Vector2 vector = touch.position - thumbCenterToScreen;
				bool flag3 = vector.sqrMagnitude < thumbRadius * thumbRadius;
				bool flag4 = touch.fingerId == thumbTouchFingerId;
				if (touch.phase != 0)
				{
					if (touch.phase == TouchPhase.Stationary)
					{
						if (flag3 || flag4)
						{
							if (flag3)
							{
								touchX = vector.x / thumbRadius;
								touchY = vector.y / thumbRadius;
							}
							else
							{
								touchX = vector.x / thumbRadius;
								touchY = vector.y / thumbRadius;
								if (Mathf.Abs(touchX) > Mathf.Abs(touchY))
								{
									touchY /= Mathf.Abs(touchX);
									touchX = ((touchX > 0f) ? 1 : (-1));
								}
								else if (touchY != 0f)
								{
									touchX /= Mathf.Abs(touchY);
									touchY = ((touchY > 0f) ? 1 : (-1));
								}
								else
								{
									touchX = 0f;
									touchY = 0f;
								}
							}
							thumbTouchFingerId = touch.fingerId;
							inputInfo.moving = true;
						}
						else
						{
							Vector2 vector2 = touch.position - shootThumbCenterToScreen;
							bool flag5 = vector2.sqrMagnitude < thumbRadius * thumbRadius;
							flag2 = vector2.sqrMagnitude < thumbRadius * 0.1f * (thumbRadius * 0.1f);
							if (flag5 || shootingTouchFingerId == touch.fingerId)
							{
								if (flag5)
								{
									cameraRotation.x = Mathf.Clamp(vector2.x, 0f - thumbRadius, thumbRadius) * 0.005f * sensitivityFactor;
									lastShootTouch = touch.position;
								}
								else
								{
									cameraRotation.x = Mathf.Sign(vector2.x) * thumbRadius * 0.01f * sensitivityFactor;
									Vector2 normalized = (touch.position - shootThumbCenterToScreen).normalized;
									lastShootTouch = shootThumbCenterToScreen + normalized * thumbRadius;
								}
								inputInfo.fire = true;
								shootingTouchFingerId = touch.fingerId;
							}
						}
					}
					else if (touch.phase == TouchPhase.Moved)
					{
						if (flag3 || flag4)
						{
							if (flag3)
							{
								touchX = vector.x / thumbRadius;
								touchY = vector.y / thumbRadius;
							}
							else
							{
								touchX = vector.x / thumbRadius;
								touchY = vector.y / thumbRadius;
								if (Mathf.Abs(touchX) > Mathf.Abs(touchY))
								{
									touchY /= Mathf.Abs(touchX);
									touchX = ((touchX > 0f) ? 1 : (-1));
								}
								else if (touchY != 0f)
								{
									touchX /= Mathf.Abs(touchY);
									touchY = ((touchY > 0f) ? 1 : (-1));
								}
								else
								{
									touchX = 0f;
									touchY = 0f;
								}
							}
							thumbTouchFingerId = touch.fingerId;
							inputInfo.moving = true;
						}
						else
						{
							if (lastMoveTouch.phase == TouchPhase.Moved && !bBlockCamera)
							{
								if (touch.fingerId == moveTouchFingerId)
								{
									cameraRotation.x = (touch.position.x - lastMoveTouch.position.x) * 0.3f * sensitivityFactor;
									cameraRotation.y = (touch.position.y - lastMoveTouch.position.y) * 0.16f * sensitivityFactor;
								}
								else if (touch.fingerId == moveTouchFingerId2)
								{
									cameraRotation.x = (touch.position.x - lastMoveTouch2.position.x) * 0.3f * sensitivityFactor;
									cameraRotation.y = (touch.position.y - lastMoveTouch2.position.y) * 0.16f * sensitivityFactor;
								}
							}
							if (moveTouchFingerId == -1)
							{
								moveTouchFingerId = touch.fingerId;
							}
							if (moveTouchFingerId != -1 && touch.fingerId != moveTouchFingerId)
							{
								moveTouchFingerId2 = touch.fingerId;
							}
							if (touch.fingerId == moveTouchFingerId)
							{
								lastMoveTouch.phase = TouchPhase.Moved;
								lastMoveTouch.position = touch.position;
							}
							if (touch.fingerId == moveTouchFingerId2)
							{
								lastMoveTouch2.phase = TouchPhase.Moved;
								lastMoveTouch2.position = touch.position;
							}
							Vector2 vector3 = touch.position - shootThumbCenterToScreen;
							bool flag6 = vector3.sqrMagnitude < thumbRadius * thumbRadius;
							flag2 = vector3.sqrMagnitude < thumbRadius * 0.1f * (thumbRadius * 0.1f);
							if (shootingTouchFingerId == touch.fingerId || flag6)
							{
								inputInfo.fire = true;
								if (flag6)
								{
									cameraRotation.x += Mathf.Clamp(vector3.x, 0f - thumbRadius, thumbRadius) * 0.002f * sensitivityFactor;
									lastShootTouch = touch.position;
								}
								else
								{
									Vector2 normalized2 = (touch.position - shootThumbCenterToScreen).normalized;
									lastShootTouch = shootThumbCenterToScreen + normalized2 * thumbRadius;
									cameraRotation.x += Mathf.Sign(vector3.x) * thumbRadius * 0.006f * sensitivityFactor;
								}
								shootingTouchFingerId = touch.fingerId;
							}
						}
					}
					else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						if (touch.fingerId == thumbTouchFingerId)
						{
							thumbTouchFingerId = -1;
						}
						if (touch.fingerId == shootingTouchFingerId)
						{
							shootingTouchFingerId = -1;
							lastShootTouch = shootThumbCenterToScreen;
						}
						if (touch.fingerId == moveTouchFingerId)
						{
							moveTouchFingerId = -1;
							lastMoveTouch.phase = TouchPhase.Ended;
						}
						if (touch.fingerId == moveTouchFingerId2)
						{
							moveTouchFingerId2 = -1;
							lastMoveTouch2.phase = TouchPhase.Ended;
						}
					}
				}
				if ((player.GetWeapon().GetWeaponType() == WeaponType.Sniper || player.GetWeapon().GetWeaponType() == WeaponType.AdvancedSniper || player.GetWeapon().GetWeaponType() == WeaponType.RelectionSniper) && flag2)
				{
					Sniper sniper2 = player.GetWeapon() as Sniper;
					if (sniper2.isAiming())
					{
						cameraRotation.x = 0f;
						cameraRotation.y = 0f;
					}
				}
				lastTouch[i] = touch;
			}
			touchX = Mathf.Clamp(touchX, -1f, 1f);
			touchY = Mathf.Clamp(touchY, -1f, 1f);
			moveDirection = new Vector3(touchX, 0f, touchY);
		}
		if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.z))
		{
			if (moveDirection.x > 0f)
			{
				inputInfo.dir = MoveDirection.Right;
			}
			else
			{
				inputInfo.dir = MoveDirection.Left;
			}
		}
		else if (moveDirection.z > 0f)
		{
			inputInfo.dir = MoveDirection.Forward;
		}
		else
		{
			inputInfo.dir = MoveDirection.Backward;
		}
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			float magnitude = moveDirection.magnitude;
			if (magnitude > 0f)
			{
				moveDirection.x /= moveDirection.magnitude;
				moveDirection.z /= moveDirection.magnitude;
				if (moveDirection.z < 0f)
				{
					moveDirection.z *= 0.9f;
				}
			}
		}
		ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
		GameApp.GetInstance().GetGameWorld().GetPlayer()
			.GetTransform()
			.Rotate(new Vector3(0f, 0f - component.WeaponAngleH, 0f));
		moveDirection = GameApp.GetInstance().GetGameWorld().GetPlayer()
			.GetTransform()
			.TransformDirection(moveDirection);
		GameApp.GetInstance().GetGameWorld().GetPlayer()
			.GetTransform()
			.Rotate(new Vector3(0f, component.WeaponAngleH, 0f));
		if (GameApp.GetInstance().GetGameWorld().State != GameState.SwitchBossLevel)
		{
			moveDirection += Vector3.down * 1.9f;
		}
		inputInfo.moveDirection = moveDirection;
		if (flag)
		{
			cameraRotation.x = 0f;
			cameraRotation.y = 0f;
		}
	}
}
