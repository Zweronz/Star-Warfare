using System.Collections;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
	public float range = 20f;

	public int damage = 300;

	public float rocketFlySpeed = 16f;

	public float bombRange = 3f;

	public float fireRate = 2f;

	protected Timer fireTimer = new Timer();

	private void Start()
	{
		fireTimer.SetTimer(fireRate, false);
		damage = 300;
	}

	private void Update()
	{
		Enemy enemy = null;
		float num = 999999f;
		if (fireTimer.Ready())
		{
			Hashtable enemies = GameApp.GetInstance().GetGameWorld().GetEnemies();
			object[] array = new object[enemies.Count];
			enemies.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				Enemy enemy2 = enemies[array[i]] as Enemy;
				float num2 = Vector3.Distance(base.transform.position, enemy2.GetTransform().position);
				if (num2 < range && num2 < num)
				{
					enemy = enemy2;
					num = num2;
				}
			}
		}
		if (enemy != null)
		{
			Vector3 vector = base.transform.position + Vector3.one * 0.5f;
			Vector3 normalized = (enemy.GetTransform().position - base.transform.position).normalized;
			vector += normalized * 0.3f;
			GameObject original = Resources.Load("Effect/Projectile") as GameObject;
			GameObject gameObject = Object.Instantiate(original, vector, Quaternion.LookRotation(normalized)) as GameObject;
			ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
			component.dir = normalized;
			component.flySpeed = rocketFlySpeed;
			component.explodeRadius = bombRange;
			component.hitForce = 0f;
			component.life = 8f;
			component.damage = damage;
			component.GunType = WeaponType.RocketLauncher;
			component.targetPos = enemy.GetTransform().position;
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerFireRocketRequest request = new PlayerFireRocketRequest(3, vector, normalized);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			fireTimer.Do();
		}
	}
}
