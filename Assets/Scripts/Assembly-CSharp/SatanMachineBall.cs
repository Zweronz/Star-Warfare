using UnityEngine;

public class SatanMachineBall : SatanMachineProjectile
{
	private GameObject explosionSmoke;

	private GameObject poisonGas;

	protected override void OnInit()
	{
		GameObject original = Resources.Load("Effect/SatanMachine/xmas_ball02") as GameObject;
		explosionSmoke = Object.Instantiate(original) as GameObject;
		explosionSmoke.SetActive(false);
		original = Resources.Load("Effect/SatanMachine/xmas_ball02_Smoke01") as GameObject;
		poisonGas = Object.Instantiate(original) as GameObject;
		poisonGas.SetActive(false);
	}

	protected override void OnExplode()
	{
		explosionSmoke.SetActive(true);
		explosionSmoke.transform.position = base.transform.position;
		explosionSmoke.transform.rotation = Quaternion.identity;
		poisonGas.SetActive(true);
		poisonGas.transform.position = base.transform.position;
		poisonGas.transform.rotation = Quaternion.identity;
		GasScript component = poisonGas.GetComponent<GasScript>();
		component.targetPlayer = targetPlayer;
		component.enemy = enemy;
		AudioManager.GetInstance().PlaySoundAt("Audio/gl/grenade_launcher_boom", base.transform.position);
	}

	protected override void OnTouchGround()
	{
		AudioManager.GetInstance().PlaySound("Audio/enemy/SatanMachine/piqiu");
		GameObject original = Resources.Load("Effect/SatanMachine/xmas_ball01") as GameObject;
		Object.Instantiate(original, base.transform.position, Quaternion.identity);
	}

	protected override bool IsToDestroy()
	{
		return false;
	}
}
