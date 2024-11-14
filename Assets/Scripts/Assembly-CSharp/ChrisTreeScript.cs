using UnityEngine;

public class ChrisTreeScript : MonoBehaviour
{
	public GameObject[] RedTree;

	public GameObject[] BlueTree;

	public Vector3 rotationSpeed = new Vector3(0f, 0f, 50f);

	private void Start()
	{
	}

	private void Update()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null || gameWorld.BattleInfo == null || gameWorld.BattleInfo.TeamScores == null)
		{
			return;
		}
		float num = 0.2f;
		for (int i = 0; i < BlueTree.Length; i++)
		{
			if (i == BlueTree.Length - 1)
			{
				BlueTree[i].transform.Rotate(rotationSpeed * Time.deltaTime);
				continue;
			}
			int num2 = (int)((float)Lobby.GetInstance().WinValue * num);
			if (gameWorld.BattleInfo.TeamScores[0] >= num2)
			{
				BlueTree[i].transform.Rotate(rotationSpeed * Time.deltaTime);
			}
			num += 0.2f;
		}
		num = 0.2f;
		for (int j = 0; j < RedTree.Length; j++)
		{
			if (j == RedTree.Length - 1)
			{
				RedTree[j].transform.Rotate(rotationSpeed * Time.deltaTime);
				continue;
			}
			int num3 = (int)((float)Lobby.GetInstance().WinValue * num);
			if (gameWorld.BattleInfo.TeamScores[1] >= num3)
			{
				RedTree[j].transform.Rotate(rotationSpeed * Time.deltaTime);
			}
			num += 0.2f;
		}
	}
}
