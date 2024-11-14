public class PlayerKnockedState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		player.GetKnocked();
		if (player.IsLocal())
		{
			GameApp.GetInstance().GetGameWorld().GetCamera()
				.ZoomToKnockedView(deltaTime);
		}
	}
}
