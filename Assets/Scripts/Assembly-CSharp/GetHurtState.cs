public class GetHurtState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.AnimationPlayed(AnimationString.Hurt, 1f))
		{
			player.SetState(Player.IDLE_STATE);
		}
	}
}
