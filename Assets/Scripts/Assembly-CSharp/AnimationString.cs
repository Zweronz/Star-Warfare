public class AnimationString
{
	public static string Idle = "idle";

	public static string Run = "run";

	public static string Attack = "stand_shoot";

	public static string RunAttack = "run_shoot";

	public static string Hurt = "run_shoot";

	public static string Dead = "dead";

	public static string Attacked = "attacked";

	public static string AttackedBack = "attacked_back";

	public static string FlyIdle = "fly_idle";

	public static string FlyForward = "fly_front";

	public static string FlyBack = "fly_back";

	public static string FlyLeft = "fly_left";

	public static string FlyRight = "fly_right";

	public static string Fly = "fly";

	public static string FlyAttack = "fly_stand_shoot";

	public static string FlyRunAttack = "fly_runshoot";

	public static string WinSpecial = "win";

	public static string WinIdle = "win_idle";

	public static string WinIdleSpecial = "win01";

	public static string WinIdleMachineGun = "idle01_machinegun";

	public static string UIIdle = "idle01";

	public static string UIIdleMachineGun = "idle01_machinegun";

	public static string ENEMY_ATTACK = "attack";

	public static string ENEMY_RUN = "run";

	public static string ENEMY_RUN01 = "run01";

	public static string ENEMY_RUN02 = "run02";

	public static string ENEMY_IDLE = "idle";

	public static string ENEMY_GOTHIT = "attacked";

	public static string ENEMY_DEAD = "dead";

	public static string ENEMY_DEAD01 = "dead01";

	public static string ENEMY_JUMPSTART = "jump01";

	public static string ENEMY_JUMPING = "jump02";

	public static string ENEMY_JUMPGEND = "jump03";

	public static string ENEMY_JUMP = "jump";

	public static string ENEMY_START_FLY = "up";

	public static string ENEMY_FLY_IDLE = "fly_idle";

	public static string ENEMY_FLY = "fly_walk";

	public static string ENEMY_FLY_DIVE = "fly_Sprint";

	public static string ENEMY_FLY_RUSH = "fly_Sprint02";

	public static string ENEMY_FLY_RUSH_END = "fly_Sprint_down";

	public static string ENEMY_PU = "pu";

	public static string ENEMY_PU01 = "pu01";

	public static string ENEMY_LANDING = "down";

	public static string ENEMY_FLY_ATTACK = "fly_attack";

	public static string ENEMY_FLY_ATTACK01 = "fly_attack01";

	public static string ENEMY_ATTACK02 = "flame";

	public static string ENEMY_FLY_GOTHIT = "fly_attacked";

	public static string ENEMY_RAGE = "rage";

	public static string SPIDER_SHOT = "attack";

	public static string SPIDER_CONTINUOUS_SHOT = "attack02";

	public static string SPIDER_DOUBLE_ATTACK = "attack03";

	public static string SPIDER_NORMAL_ATTACK = "attack04";

	public static string SPIDER_RUSH = "chong";

	public static string SPIDER_START_RUSH = "try_run";

	public static string TANK_STARTRUSH = "idle01";

	public static string TANK_RUSH = "chong";

	public static string MANTIS_RUSH_START = "fuchong01";

	public static string MANTIS_RUSH = "fuchong02";

	public static string MANTIS_RUSH_END = "fuchong03";

	public static string MANTIS_CRITICAL_ATTACK = "attack02";

	public static string MANTIS_SHOT = "fly_attack01";

	public static string MANTIS_LASER = "fly_attack02";

	public static string MANTIS_BOOMERANG = "fly_attack03";

	public static string MANTIS_FLY_DEAD = "fly_dead";

	public static string MANTIS_WATCH = "idle02";

	public static string MANTIS_STAND_SHOT = "stand_attack01";

	public static string MANTIS_STAND_LASER = "stand_laser";

	public static string MANTIS_LASER_180 = "fly_attack180";

	public static string EARTHWORM_SHOT = "attack_spit1";

	public static string EARTHWORM_CONTINUOUS_SHOT = "attack_spit2";

	public static string EARTHWORM_DOUBLE_ATTACK = "attack";

	public static string EARTHWORM_NORMAL_ATTACK = "attack";

	public static string EARTHWORM_RUSH = "attack_dash_idle";

	public static string EARTHWORM_SUPER_RUSH = "attack_dash_fastidle";

	public static string EARTHWORM_START_RUSH = "attack_dash_start";

	public static string EARTHWORM_RUSH_END = "attack_dash_over";

	public static string EARTHWORM_DRILLIN = "ground_in";

	public static string EARTHWORM_DRILLOUT = "out";

	public static string SATANMACHINE_STOMP = "attack";

	public static string SATANMACHINE_LAUNCH_MISSILE_1 = "attack02_christ";

	public static string SATANMACHINE_LAUNCH_MISSILE_2 = "attack03_christ";

	public static string SATANMACHINE_SHOT_LASER = "attack01_christ";

	public static string SATANMACHINE_GIFT_BOMB = "attack04_christ";

	public static string SATANMACHINE_START_RAGE = "head_christ";

	public static string SATANMACHINE_WHIRLING_ATTACK_BEGIN = "attack_begin_joky";

	public static string SATANMACHINE_WHIRLING_ATTACK_PROCESS = "attack_joky";

	public static string SATANMACHINE_WHIRLING_ATTACK_END = "attack_end_joky";

	public static string SATANMACHINE_THROW_BALL = "attack01_joky";

	public static string SATANMACHINE_END_RAGE = "head_joky";

	public static string SATANMACHINE_DEAD = "death";

	public static string SATANMACHINE_IDLE_A = "idle01";

	public static string SATANMACHINE_IDLE_B = "idle02";

	public static string GetFlyAniString(MoveDirection dir)
	{
		string result = FlyForward;
		switch (dir)
		{
		case MoveDirection.Forward:
			result = FlyForward;
			break;
		case MoveDirection.Backward:
			result = FlyBack;
			break;
		case MoveDirection.Left:
			result = FlyLeft;
			break;
		case MoveDirection.Right:
			result = FlyRight;
			break;
		}
		return result;
	}
}
