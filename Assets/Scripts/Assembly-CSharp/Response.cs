public abstract class Response
{
	public byte responseID;

	public static Response CreateResponse(byte responseID)
	{
		Response result = null;
		switch (responseID)
		{
		case 1:
			result = new PlayerLoginResponse();
			break;
		case 7:
			result = new GetRoomListResponse();
			break;
		case 4:
			result = new CreateRoomResponse();
			break;
		case 8:
			result = new GetRoomDataResponse();
			break;
		case 5:
			result = new JoinRoomResponse();
			break;
		case 9:
			result = new StartGameResponse();
			break;
		case 10:
			result = new SetMasterPlayerResponse();
			break;
		case 132:
			result = new PlayerSpawnResponse();
			break;
		case 12:
			result = new PlayerLoginGameServerResponse();
			break;
		case 13:
			result = new RoomTimeSynchronizeResponse();
			break;
		case 100:
			result = new TimeSynchronizeResponse();
			break;
		case 14:
			result = new GuestLoginResponse();
			break;
		case 20:
			result = new ChangeSeatResponse();
			break;
		case 22:
			result = new GetBattleStateResponse();
			break;
		case 50:
			result = new TestRespones();
			break;
		case 101:
			result = new GetSceneStateResponse();
			break;
		case 102:
			result = new SendTransformStateResponse();
			break;
		case 103:
			result = new SendPlayerInputResponse();
			break;
		case 104:
			result = new EnemySpawnResponse();
			break;
		case 105:
			result = new EnemyMoveResponse();
			break;
		case 106:
			result = new EnemyStateResponse();
			break;
		case 108:
			result = new EnemyOnHitResponse();
			break;
		case 110:
			result = new PlayerOnHitResponse();
			break;
		case 111:
			result = new PlayerChangeWeaponResponse();
			break;
		case 113:
			result = new PlayerLeaveGameResponse();
			break;
		case 114:
			result = new PlayerFireRocketResponse();
			break;
		case 115:
			result = new PlayerHitPlayerResponse();
			break;
		case 116:
			result = new EnemyShotResponse();
			break;
		case 117:
			result = new PlayerUseItemResponse();
			break;
		case 119:
			result = new ItemSpawnResponse();
			break;
		case 120:
			result = new PickUpItemResponse();
			break;
		case 121:
			result = new PlayerOnKnockedResponse();
			break;
		case 122:
			result = new PlayerUploadStatisticsResponse();
			break;
		case 123:
			result = new PlayerBuffResponse();
			break;
		case 124:
			result = new SendPlayerShootAngleVResponse();
			break;
		case 125:
			result = new PlayerRebirthResponse();
			break;
		case 126:
			result = new PlayerKillPlayerResponse();
			break;
		case 127:
			result = new VSScoreResponse();
			break;
		case 128:
			result = new VSGameEndResponse();
			break;
		case 129:
			result = new VSGameAutoBalanceResponse();
			break;
		case 130:
			result = new VSUploadStatisticsResponse();
			break;
		case 131:
			result = new ReStartGameResponse();
			break;
		case 133:
			result = new SendVSTimeResponse();
			break;
		case 27:
			result = new GetMithrilPriceResponse();
			break;
		case 134:
			result = new DropTheFlagResponse();
			break;
		case 135:
			result = new CatchTheFlagResponse();
			break;
		case 136:
			result = new GetTheFlagScoreResponse();
			break;
		case 137:
			result = new SendFlagTimeResponse();
			break;
		case 138:
			result = new RequestFlagPositionResponse();
			break;
		case 139:
			result = new SetPlayerHealthResponse();
			break;
		case 140:
			result = new SendPlayerVIPResponse();
			break;
		case 142:
			result = new GetVIPSceneStateResponse();
			break;
		case 141:
			result = new SendVIPVSResponse();
			break;
		case 143:
			result = new VSWinTeamResponse();
			break;
		case 118:
			result = new PlayerHpRecoveryResponse();
			break;
		case 144:
			result = new SecureVIPPlayerResponse();
			break;
		case 145:
			result = new PlayerImpactWaveResponse();
			break;
		case 146:
			result = new PlayerTrackingGrendeRespone();
			break;
		case 147:
			result = new SatanMachineGiftBombResponse();
			break;
		case 152:
			result = new GetCMISceneStateResponse();
			break;
		case 151:
			result = new CreateGiftCMIResponse();
			break;
		case 150:
			result = new SendCMIVSResponse();
			break;
		case 153:
			result = new PlayerHitItemResponse();
			break;
		case 154:
			result = new PlayerTrackWaveResponse();
			break;
		case 155:
			result = new ReflectionEnemyResponse();
			break;
		case 157:
			result = new FLyGrenadeCreateResponse();
			break;
		case 158:
			result = new FLyGrenadeFindTargetResponse();
			break;
		case 159:
			result = new FLyGrenadeExplodeResponse();
			break;
		case 160:
			result = new PlayerGravityForceResponse();
			break;
		}
		return result;
	}

	public virtual void ReadData(byte[] data)
	{
	}

	public abstract void ProcessLogic();

	public virtual void ProcessRobotLogic(Robot robot)
	{
	}
}
