using UnityEngine;

public class EarthwormSlimeScript : MonoBehaviour
{
	public int slimeDamage;

	public float maxScale = 1f;

	public float disappearTime = 5f;

	public Timer slimeTimer;

	public float diffuseSpeed = 5f;

	public float slowEffect = 1f;

	private float startTime;

	private bool isScaling = true;

	private bool playerIsInLastFrame;

	public void Start()
	{
		base.transform.localScale = Vector3.zero;
		startTime = Time.time;
	}

	public void Update()
	{
		Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
		if (!(player.GetSkills().GetSkill(SkillsType.FLY) > 0f) && 0 == 0 && player.InPlayingState())
		{
			Collider collider = player.GetCollider();
			if (base.collider.bounds.Intersects(player.GetCollider().bounds))
			{
				player.SlowEffect = slowEffect;
				playerIsInLastFrame = true;
			}
			else if (playerIsInLastFrame)
			{
				player.SlowEffect = 1f;
				playerIsInLastFrame = false;
			}
		}
		if (isScaling)
		{
			if (base.transform.localScale.x < maxScale)
			{
				base.transform.localScale += new Vector3(Time.deltaTime * diffuseSpeed, 0f, Time.deltaTime * diffuseSpeed);
			}
			else
			{
				base.transform.localScale = new Vector3(maxScale, 1f, maxScale);
				isScaling = false;
			}
		}
		if (Time.time - startTime > disappearTime)
		{
			if (playerIsInLastFrame)
			{
				player.SlowEffect = 1f;
			}
			Object.DestroyObject(base.gameObject);
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
}
