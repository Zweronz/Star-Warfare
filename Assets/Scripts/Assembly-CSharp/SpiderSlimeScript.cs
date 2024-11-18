using UnityEngine;

public class SpiderSlimeScript : MonoBehaviour
{
	public int slimeDamage;

	public float maxScale = 1f;

	public float disappearTime = 5f;

	public string colorPropertyName = "_TintColor";

	public Timer slimeTimer;

	public float diffuseSpeed = 5f;

	private float startTime;

	private bool isScaling = true;

	public void Start()
	{
		base.transform.localScale = Vector3.zero;
		startTime = Time.time;
		PlayDiffusionEffect();
	}

	public void Update()
	{
		Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
		if (isScaling)
		{
			if (base.transform.localScale.x < maxScale)
			{
				base.transform.localScale += new Vector3(Time.deltaTime * diffuseSpeed, Time.deltaTime * diffuseSpeed, 0f);
				return;
			}
			base.transform.localScale = new Vector3(maxScale, maxScale, 1f);
			startTime = Time.time;
			PlayBubbleEffect();
			isScaling = false;
		}
		else if (Time.time - startTime > disappearTime)
		{
			Color color = base.GetComponent<Renderer>().material.GetColor(colorPropertyName);
			color.a -= Time.deltaTime;
			if (color.a > 0f)
			{
				base.GetComponent<Renderer>().material.SetColor(colorPropertyName, color);
			}
			else
			{
				Object.DestroyObject(base.gameObject);
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (slimeTimer.Ready() && other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
			if (!(player.GetSkills().GetSkill(SkillsType.FLY) > 0f) && 0 == 0)
			{
				player.OnHit(slimeDamage);
			}
			slimeTimer.Do();
		}
	}

	private void PlayDiffusionEffect()
	{
		GameObject original = Resources.Load("Effect/Spider/sfx_kuosan") as GameObject;
		GameObject gameObject = Object.Instantiate(original, base.transform.position, Quaternion.identity) as GameObject;
		AutoDestroyScript component = gameObject.GetComponent<AutoDestroyScript>();
		if (null != component)
		{
			component.life = maxScale / diffuseSpeed;
		}
		ParticleEmitter component2 = gameObject.GetComponent<ParticleEmitter>();
		if (null != component2)
		{
			component2.rndVelocity = new Vector3(maxScale, 0f, maxScale);
		}
	}

	private void PlayBubbleEffect()
	{
		GameObject original = Resources.Load("Effect/Spider/sfx_paopao") as GameObject;
		GameObject gameObject = Object.Instantiate(original, base.transform.position, Quaternion.identity) as GameObject;
		AutoDestroyScript component = gameObject.GetComponent<AutoDestroyScript>();
		if (null != component)
		{
			component.life = disappearTime;
		}
	}
}
