using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera/ThirdPersonStandardCamera")]
public class ThirdPersonStandardCameraScript : MonoBehaviour
{
	protected float angelH;

	protected float angelV;

	protected float lastUpdateTime;

	protected float deltaTime;

	public Vector3 pivotPosition;

	public Vector3 pivotPositionNormal;

	public Vector3 pivotPositionRPG;

	protected Vector3 pivotOffset;

	public float cameraDistanceWhenIdle = 4f;

	public float cameraDistanceWhenRPG = 6f;

	public float cameraDistanceWhenAimed = 2f;

	public float cameraDistanceWhenKnocked = 4f;

	public float cameraDistance;

	public float cameraSwingSpeed;

	public float minAngelV;

	public float maxAngelV;

	public float fixedAngelV;

	public bool isAngelVFixed;

	public Transform target;

	protected CameraClipObject[] lastTransparentObjList = new CameraClipObject[5];

	protected Vector3 moveTo;

	protected bool behindWall;

	public bool lastInWall;

	protected bool started;

	public float CAMERA_AIM_FOV = 22f;

	public float CAMERA_NORMAL_FOV = 60f;

	public float CAMERA_RPG_FOV = 80f;

	public float CAMERA_KNOCKED_FOV = 60f;

	public Texture reticle;

	public Texture leftTopReticle;

	public Texture rightTopReticle;

	public Texture leftBottomReticle;

	public Texture rightBottomReticle;

	protected Shader transparentShader;

	protected Shader solidShader;

	protected Shader transparentLightmapShader;

	protected float drx;

	protected float dry;

	protected float winTime = -1f;

	protected Vector2 reticlePosition;

	protected Transform cameraTransform;

	protected GameObject pivot;

	public AudioSource loseAudio;

	protected float weaponAngleH;

	protected float initMinAngleV;

	protected float initMaxAngleV;

	public Transform CameraTransform
	{
		get
		{
			return cameraTransform;
		}
	}

	public Vector2 ReticlePosition
	{
		get
		{
			return reticlePosition;
		}
		set
		{
			reticlePosition = value;
		}
	}

	public float AngelV
	{
		get
		{
			return angelV;
		}
	}

	public float AngelH
	{
		get
		{
			return angelH;
		}
		set
		{
			angelH = value;
		}
	}

	public float WeaponAngleH
	{
		get
		{
			return weaponAngleH;
		}
	}

	private Dictionary<string, string> MainTextureMap = new Dictionary<string, string>()
	{
		{"iPhone/LightMap", "_texBase"}
	};

	public virtual void Init()
	{
		target = GameApp.GetInstance().GetGameWorld().GetPlayer()
			.GetTransform();
		angelH = target.rotation.eulerAngles.y;
		cameraDistance = cameraDistanceWhenIdle;
		base.transform.rotation = Quaternion.Euler(0f - angelV, angelH, 0f);
		Screen.lockCursor = true;
		reticlePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
		if (Application.platform != RuntimePlatform.WindowsPlayer)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				cameraSwingSpeed *= 0.8f;
			}
			else if (Screen.width != 960)
			{
			}
		}
		float[] array = new float[32];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 160f;
		}
		array[PhysicsLayer.SKY] = 1000f;
		base.GetComponent<Camera>().layerCullDistances = array;
		started = true;
		base.GetComponent<Camera>().fov = CAMERA_NORMAL_FOV;
		pivot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pivot.GetComponent<Renderer>().enabled = false;
		for (int j = 0; j < lastTransparentObjList.Length; j++)
		{
			lastTransparentObjList[j] = new CameraClipObject();
		}
		initMinAngleV = minAngelV;
		initMaxAngleV = maxAngelV;
	}

	public virtual void ResetAngleV()
	{
		minAngelV = initMinAngleV;
		maxAngelV = initMaxAngleV;
	}

	public virtual void CreateScreenBlood(float damage)
	{
	}

	public virtual void ZoomIn(float deltaTime)
	{
		base.GetComponent<Camera>().fov = Mathf.Lerp(base.GetComponent<Camera>().fov, CAMERA_AIM_FOV, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		cameraDistance = Mathf.Lerp(cameraDistance, cameraDistanceWhenAimed, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		pivotPosition = Vector3.Lerp(pivotPosition, pivotPositionNormal + pivotOffset, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
	}

	public void SetPivotOffset(Vector3 offset)
	{
		pivotOffset = offset;
	}

	public virtual void ZoomToRPG(float deltaTime)
	{
		base.GetComponent<Camera>().fov = Mathf.Lerp(base.GetComponent<Camera>().fov, CAMERA_RPG_FOV, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		cameraDistance = Mathf.Lerp(cameraDistance, cameraDistanceWhenRPG, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		pivotPosition = Vector3.Lerp(pivotPosition, pivotPositionRPG + pivotOffset, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
	}

	public virtual void ZoomToSniper(float deltaTime)
	{
		base.GetComponent<Camera>().fov = Mathf.Lerp(base.GetComponent<Camera>().fov, CAMERA_AIM_FOV * 0.6f, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		cameraDistance = Mathf.Lerp(cameraDistance, cameraDistanceWhenAimed * 0.05f, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		pivotPosition = Vector3.Lerp(pivotPosition, new Vector3(0.6f, 1.68f, 0f) + pivotOffset, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
	}

	public virtual void ZoomOut(float deltaTime)
	{
		base.GetComponent<Camera>().fov = Mathf.Lerp(base.GetComponent<Camera>().fov, CAMERA_NORMAL_FOV, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		cameraDistance = Mathf.Lerp(cameraDistance, cameraDistanceWhenIdle, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		pivotPosition = Vector3.Lerp(pivotPosition, pivotPositionNormal + pivotOffset, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
	}

	public virtual void ZoomToKnockedView(float deltaTime)
	{
		base.GetComponent<Camera>().fov = Mathf.Lerp(base.GetComponent<Camera>().fov, CAMERA_KNOCKED_FOV, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		cameraDistance = Mathf.Lerp(cameraDistance, cameraDistanceWhenKnocked, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
	}

	private void Awake()
	{
		cameraTransform = Camera.main.transform;
	}

	private void Start()
	{
		solidShader = Shader.Find("iPhone/LightMap");
		transparentShader = Shader.Find("iPhone/AlphaBlend_Color");
		transparentLightmapShader = Shader.Find("iPhone/LightMap_AlphaBlend");
		base.GetComponent<Camera>().nearClipPlane = 10000f;
	}

	private void Update()
	{
		if (pivot != null)
		{
			pivot.transform.position = target.TransformPoint(pivotPosition);
			pivot.transform.rotation = Quaternion.identity;
			pivot.transform.localScale = Vector3.one * 0.3f;
		}
	}

	private void LateUpdate()
	{
		if (!started || target == null)
		{
			return;
		}
		base.GetComponent<Camera>().nearClipPlane = 0.3f;
		deltaTime = Time.deltaTime;
		Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
		float num = player.InputController.CameraRotation.x;
		float num2 = player.InputController.CameraRotation.y;
		if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
		{
			num = Input.GetAxis("Mouse X");
			num2 = Input.GetAxis("Mouse Y");
		}
		if (Time.timeScale != 0f)
		{
			angelH += num * cameraSwingSpeed;
			angelV += num2 * cameraSwingSpeed;
		}
		if (isAngelVFixed)
		{
			angelV = fixedAngelV;
		}
		angelV = Mathf.Clamp(angelV, minAngelV, maxAngelV);
		cameraTransform.rotation = Quaternion.Euler(0f - (angelV + drx), angelH + dry, 0f);
		float num3 = 100f;
		if (player.InPlayingState())
		{
			target.rotation = Quaternion.Euler(0f, angelH, 0f);
			float num4 = 0f;
			bool flag = player.GetSkills().GetSkill(SkillsType.FLY) > 0f;
			WeaponType weaponType = player.GetWeapon().GetWeaponType();
			num4 = ((flag && weaponType != WeaponType.RocketLauncher && weaponType != WeaponType.AutoRocketLauncher && weaponType != WeaponType.LaserGun && weaponType != WeaponType.MachineGun && weaponType != WeaponType.AdvancedMachineGun) ? ((!player.inputController.inputInfo.IsMoving()) ? (player.GetWeapon().Adjuster.angleOffsetH + 12f) : player.GetWeapon().Adjuster.angleOffsetH) : ((player.State != Player.ATTACK_STATE) ? 0f : player.GetWeapon().Adjuster.angleOffsetH));
			if (weaponAngleH > num4)
			{
				weaponAngleH -= 1f;
				if (weaponAngleH < num4)
				{
					weaponAngleH = num4;
				}
			}
			else if (weaponAngleH < num4)
			{
				weaponAngleH += 1f;
				if (weaponAngleH > num4)
				{
					weaponAngleH = num4;
				}
			}
			target.Rotate(new Vector3(0f, weaponAngleH, 0f));
		}
		Vector3 normalized = cameraTransform.TransformDirection(Vector3.forward).normalized;
		moveTo = target.TransformPoint(pivotPosition) - normalized * cameraDistance;
		Vector3 direction = moveTo - player.GetTransform().position;
		Ray ray = new Ray(player.GetTransform().position, direction);
		float magnitude = direction.magnitude;
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, magnitude, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
		{
			base.GetComponent<Camera>().useOcclusionCulling = false;
			GameObject gameObject = hitInfo.collider.gameObject;
			if (gameObject.GetComponent<Renderer>() == null)
			{
				gameObject = gameObject.transform.parent.gameObject;
			}
			if (gameObject.GetComponent<Renderer>() != null)
			{
				gameObject.layer = PhysicsLayer.TRANSPARENT_WALL;
				int num5 = -1;
				for (int i = 0; i < 5 && !(lastTransparentObjList[i].obj == gameObject); i++)
				{
					if (lastTransparentObjList[i].obj == null)
					{
						lastTransparentObjList[i].obj = gameObject;
						num5 = i;
						break;
					}
				}
				if (num5 != -1)
				{
					lastTransparentObjList[num5].shaders = new Shader[gameObject.GetComponent<Renderer>().materials.Length];
					int num6 = 0;
					Material[] materials = gameObject.GetComponent<Renderer>().materials;
					foreach (Material material in materials)
					{
						lastTransparentObjList[num5].shaders[num6] = material.shader;
						Texture texture;
						if (!material.HasProperty("_MainTex"))
						{
							texture = material.GetTexture(MainTextureMap[material.shader.name]);
						}
						else
						{
							texture = material.mainTexture;
						}
						num6++;
						material.shader = transparentShader;
						Color gray = Color.gray;
						gray.a = 0.1f;
						material.SetColor("_TintColor", gray);
						material.SetTexture("_MainTex", texture);
					}
				}
			}
		}
		else
		{
			base.GetComponent<Camera>().useOcclusionCulling = true;
			for (int k = 0; k < 5; k++)
			{
				if (lastTransparentObjList[k].obj != null)
				{
					int num7 = 0;
					Material[] materials2 = lastTransparentObjList[k].obj.GetComponent<Renderer>().materials;
					foreach (Material material2 in materials2)
					{
						material2.shader = lastTransparentObjList[k].shaders[num7];
						num7++;
					}
					lastTransparentObjList[k].obj = null;
				}
			}
		}
		cameraTransform.position = Vector3.Lerp(cameraTransform.position, moveTo, num3 * Time.deltaTime);
		if (!player.InPlayingState())
		{
			minAngelV = -70f;
			maxAngelV = 70f;
			if (winTime == -1f)
			{
				winTime = Time.time;
			}
			float num8 = Time.time - winTime;
			cameraTransform.position = player.GetTransform().TransformPoint(4f * Mathf.Sin((num8 - 1.7f) * 0.3f), 2.5f, 4f * Mathf.Cos((num8 - 1.7f) * 0.3f));
			cameraTransform.LookAt(player.GetTransform());
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			deltaTime = 0f;
		}
	}
}
