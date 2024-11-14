using System.Collections;
using UnityEngine;

public class GameUIScript : MonoBehaviour
{
	private NetworkManager networkMgr;

	public Texture2D reticle;

	protected float frames;

	protected float updateInterval = 2f;

	protected float timeLeft;

	protected string fpsStr;

	protected float accum;

	private EnemySpawnScript ess;

	protected Timer timer = new Timer();

	protected bool showUI;

	protected bool showPopUI;

	private IEnumerator Start()
	{
		timer.SetTimer(10f, false);
		timer.Do();
		networkMgr = GameApp.GetInstance().GetNetworkManager();
		yield return 1;
	}

	private void Update()
	{
		timeLeft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames += 1f;
		if (timeLeft <= 0f)
		{
			fpsStr = "FPS:" + accum / frames;
			frames = 0f;
			accum = 0f;
			timeLeft = updateInterval;
		}
		if (ess == null)
		{
			GameObject gameObject = GameObject.Find("EnemySpawns");
			if (gameObject != null)
			{
				ess = gameObject.GetComponent<EnemySpawnScript>();
			}
		}
	}

	public void OnGUI()
	{
		GUI.Label(new Rect(10f, 130f, 200f, 100f), fpsStr);
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player player = gameWorld.GetPlayer();
		if (player != null)
		{
			GUI.Label(new Rect(170f, 20f, 100f, 100f), player.Hp + "/" + player.MaxHp);
			if (player.GetWeapon() != null)
			{
				GUI.Label(new Rect(870f, 70f, 100f, 100f), player.GetWeapon().Name + ":" + player.GetWeapon().Damage);
			}
		}
		GUI.Label(new Rect(10f, 160f, 200f, 100f), "DifficultyLevel " + gameWorld.DifficultyLevel);
	}

	private void LostFunction(int id)
	{
	}
}
