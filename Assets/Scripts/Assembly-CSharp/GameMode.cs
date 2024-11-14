public class GameMode
{
	public NetworkType TypeOfNetwork { get; set; }

	public Mode ModePlay { get; set; }

	public GameMode(NetworkType nType, Mode gMode)
	{
		TypeOfNetwork = nType;
		ModePlay = gMode;
	}

	public bool IsSingle()
	{
		if (TypeOfNetwork == NetworkType.Single)
		{
			return true;
		}
		return false;
	}

	public bool IsMultiPlayer()
	{
		return !IsSingle();
	}

	public bool IsVSMode()
	{
		if (IsMultiPlayer() && (ModePlay == Mode.VS_FFA || ModePlay == Mode.VS_TDM || ModePlay == Mode.VS_CTF_TDM || ModePlay == Mode.VS_CTF_FFA || ModePlay == Mode.VS_VIP || ModePlay == Mode.VS_CMI))
		{
			return true;
		}
		return false;
	}

	public bool IsVSMode(Mode mode)
	{
		if (IsMultiPlayer() && (mode == Mode.VS_FFA || mode == Mode.VS_TDM || mode == Mode.VS_CTF_TDM || mode == Mode.VS_CTF_FFA || mode == Mode.VS_VIP || mode == Mode.VS_CMI))
		{
			return true;
		}
		return false;
	}

	public bool IsCatchTheFlagMode()
	{
		if (IsMultiPlayer() && (ModePlay == Mode.VS_CTF_TDM || ModePlay == Mode.VS_CTF_FFA))
		{
			return true;
		}
		return false;
	}

	public bool IsCoopMode()
	{
		if (IsMultiPlayer() && (ModePlay == Mode.Survival || ModePlay == Mode.Boss))
		{
			return true;
		}
		return false;
	}

	public bool IsTeamMode()
	{
		if (IsMultiPlayer() && (ModePlay == Mode.VS_TDM || ModePlay == Mode.VS_CTF_TDM || ModePlay == Mode.VS_VIP || ModePlay == Mode.VS_CMI))
		{
			return true;
		}
		return false;
	}

	public bool IsCoopMode(Mode gameMode)
	{
		if (IsMultiPlayer() && (gameMode == Mode.Survival || gameMode == Mode.Boss))
		{
			return true;
		}
		return false;
	}
}
