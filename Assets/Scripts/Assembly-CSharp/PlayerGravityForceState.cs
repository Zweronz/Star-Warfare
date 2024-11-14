public class PlayerGravityForceState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		player.DoGravityForce(deltaTime);
		if (player.IsLocal())
		{
			GameApp.GetInstance().GetGameWorld().GetCamera()
				.ZoomOut(deltaTime);
		}
	}
}
