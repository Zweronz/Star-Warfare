public class SatanMachineCatchingState : CatchingState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		base.NextState(enemy, deltaTime, player);
		SatanMachine satanMachine = enemy as SatanMachine;
		if (satanMachine != null && !AudioManager.GetInstance().IsPlaying("xunlu"))
		{
			satanMachine.PlaySound("Audio/enemy/SatanMachine/xunlu");
		}
	}
}
