using UnityEngine;

[AddComponentMenu("Camera/ThirdPersonCamera")]
public class ThirdPersonCameraScript : MonoBehaviour
{
	protected float angelH;

	protected float angelV;

	protected float lastUpdateTime;

	protected float deltaTime;

	public Vector3 cameraDistanceFromPlayerWhenIdle;

	public Vector3 cameraDistanceFromPlayerWhenAimed;

	public float cameraSwingSpeed;

	public float minAngelV;

	public float maxAngelV;

	public float fixedAngelV;

	public bool isAngelVFixed;

	public Transform target;

	protected GameObject[] lastTransparentObjList = new GameObject[5];

	protected Vector3 moveTo;

	protected bool behindWall;

	public Vector3 cameraDistanceFromPlayer;

	public bool lastInWall;

	protected bool started;

	public float CAMERA_AIM_FOV = 22f;

	public float CAMERA_NORMAL_FOV = 38f;

	public Texture reticle;

	public Texture leftTopReticle;

	public Texture rightTopReticle;

	public Texture leftBottomReticle;

	public Texture rightBottomReticle;

	protected Shader transparentShader;

	protected Shader solidShader;

	protected float drx;

	protected float dry;

	protected float winTime = -1f;

	protected Vector2 reticlePosition;

	protected Transform cameraTransform;

	public AudioSource loseAudio;

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

	public virtual void Init()
	{
		target = GameApp.GetInstance().GetGameWorld().GetPlayer()
			.GetTransform();
		angelH = target.rotation.eulerAngles.y;
		cameraDistanceFromPlayer = cameraDistanceFromPlayerWhenIdle;
		base.transform.position = target.TransformPoint(cameraDistanceFromPlayer);
		base.transform.rotation = Quaternion.Euler(0f - angelV, angelH, 0f);
		Screen.lockCursor = true;
		Cursor.visible = true;
		reticlePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
		if (Application.platform == RuntimePlatform.WindowsPlayer)
		{
			cameraSwingSpeed *= 20f;
		}
		else if (Screen.width == 960)
		{
			cameraSwingSpeed *= 0.4f;
		}
		float[] array = new float[32];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 100f;
		}
		base.GetComponent<Camera>().layerCullDistances = array;
		started = true;
		base.GetComponent<Camera>().fov = CAMERA_NORMAL_FOV;
	}

	public virtual void CreateScreenBlood(float damage)
	{
	}

	public virtual void ZoomIn(float deltaTime)
	{
	}

	public virtual void ZoomOut(float deltaTime)
	{
	}

	private void Awake()
	{
		cameraTransform = Camera.main.transform;
	}

	private void Start()
	{
		solidShader = Shader.Find("iPhone/LightMap");
		transparentShader = Shader.Find("iPhone/AlphaBlend_Color");
	}

	private void Update()
	{
		cameraDistanceFromPlayer = cameraDistanceFromPlayerWhenIdle;
	}

	private void LateUpdate()
	{
		if (started && !(target == null))
		{
			deltaTime = Time.deltaTime;
			float num = 0f;
			float num2 = 0f;
			if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
			{
				num = Input.GetAxis("Mouse X") * 30f * Time.deltaTime;
				num2 = Input.GetAxis("Mouse Y") * 30f * Time.deltaTime;
			}
			if (Time.timeScale != 0f)
			{
				angelH += num * 0.03f * cameraSwingSpeed;
				angelV += num2 * 0.03f * cameraSwingSpeed;
			}
			if (isAngelVFixed)
			{
				angelV = fixedAngelV;
			}
			angelV = Mathf.Clamp(angelV, minAngelV, maxAngelV);
			cameraTransform.rotation = Quaternion.Euler(0f - (angelV + drx), angelH + dry, 0f);
			float num3 = 100f;
			target.rotation = Quaternion.Euler(0f, angelH, 0f);
			moveTo = target.TransformPoint(cameraDistanceFromPlayer);
			cameraTransform.position = Vector3.Lerp(cameraTransform.position, moveTo, num3 * Time.deltaTime);
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				deltaTime = 0f;
			}
		}
	}
}
