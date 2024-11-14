public class RequestType
{
	public const byte PlayerLogin = 1;

	public const byte PlayerLogout = 2;

	public const byte PlayerRegister = 3;

	public const byte CreateRoom = 4;

	public const byte JoinRoom = 5;

	public const byte LeaveRoom = 6;

	public const byte GetRoomList = 7;

	public const byte GetRoomData = 8;

	public const byte StartGame = 9;

	public const byte SearchRoom = 10;

	public const byte QuickJoin = 11;

	public const byte PlayerLoginGameServerResponse = 12;

	public const byte RoomTimeSynchronize = 13;

	public const byte SetRoomPing = 14;

	public const byte UploadData = 15;

	public const byte UploadMithril = 16;

	public const byte UploadArmorAndBag = 17;

	public const byte CreateVSRoom = 18;

	public const byte PlayerJoinTeamStartGame = 19;

	public const byte ChangeSeat = 20;

	public const byte SearchRoomAdvanced = 21;

	public const byte UploadBattleState = 22;

	public const byte UploadOperating = 23;

	public const byte UploadKillBossState = 24;

	public const byte GuestLogin = 25;

	public const byte UploadTwitterAndFacebook = 26;

	public const byte GetMithrilPrice = 27;

	public const byte Test = 50;

	public const byte TimeSynchronize = 100;

	public const byte GetSceneState = 101;

	public const byte SendTransformState = 102;

	public const byte SendPlayerInput = 103;

	public const byte EnemySpawn = 104;

	public const byte EnemyMove = 105;

	public const byte EnemyState = 106;

	public const byte EnemyHit = 107;

	public const byte EnemyOnHit = 108;

	public const byte EnemyDead = 109;

	public const byte PlayerOnHit = 110;

	public const byte PlayerChangeWeapon = 111;

	public const byte PlayerChangeArmor = 112;

	public const byte PlayerFireRocket = 114;

	public const byte PlayerHitPlayer = 115;

	public const byte EnemyShot = 116;

	public const byte PlayerUseItem = 117;

	public const byte PlayerHpRecovery = 118;

	public const byte ItemSpawn = 119;

	public const byte PickUpItem = 120;

	public const byte PlayerOnKnocked = 121;

	public const byte PlayerUploadStatistics = 122;

	public const byte PlayerBuff = 123;

	public const byte SendPlayerShootAngleV = 124;

	public const byte PlayerRebirth = 125;

	public const byte VSGameEnd = 128;

	public const byte VSUploadStatistics = 130;

	public const byte RestartGame = 131;

	public const byte SendVSTime = 133;

	public const byte DropTheFlag = 134;

	public const byte CatchTheFlag = 135;

	public const byte GetTheFlagScore = 136;

	public const byte SendFlagTime = 137;

	public const byte PlayerImpactWave = 145;

	public const byte TrackingGrenade = 146;

	public const byte SatanMachinieGiftBomb = 147;

	public const byte PlayerHitItem = 153;

	public const byte PlayerTrackingWave = 154;

	public const byte ReflectionEnemyRequest = 155;

	public const byte VerifyData = 156;

	public const byte FlyGrenadeCreate = 157;

	public const byte FlyGrenadeFindTarget = 158;

	public const byte FlyGrenadeExplode = 159;

	public const byte PlayerGravityForce = 160;

	public const byte UdidVerifyData = 161;
}
