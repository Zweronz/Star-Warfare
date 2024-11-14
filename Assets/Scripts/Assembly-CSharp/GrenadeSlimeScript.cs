using UnityEngine;

public class GrenadeSlimeScript : MonoBehaviour
{
	public int slimeDamage;

	public float disappearTime = 5f;

	public Timer slimeTimer;

	public bool isLocal = true;

	private float startTime;

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - startTime > disappearTime)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (!slimeTimer.Ready())
		{
			return;
		}
		if (other.gameObject.layer == PhysicsLayer.ENEMY)
		{
			DamageProperty damageProperty = new DamageProperty();
			damageProperty.damage = slimeDamage;
			damageProperty.isLocal = isLocal;
			Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID(other.name);
			if (enemyByID != null)
			{
				enemyByID.HitEnemy(damageProperty);
			}
			slimeTimer.Do();
		}
		else if (other.gameObject.layer == PhysicsLayer.REMOTE_PLAYER && GameApp.GetInstance().GetGameMode().IsVSMode() && isLocal)
		{
			GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
			Player player = gameWorld.GetPlayer();
			int num = int.Parse(other.gameObject.name);
			Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(num);
			if (remotePlayerByUserID != null && (!GameApp.GetInstance().GetGameMode().IsTeamMode() || !remotePlayerByUserID.IsSameTeam(player)) && !(remotePlayerByUserID.GetSkills().GetSkill(SkillsType.FLY) > 0f) && 0 == 0)
			{
				PlayerHitPlayerRequest request = new PlayerHitPlayerRequest((short)slimeDamage, num, false, 24);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}
}
