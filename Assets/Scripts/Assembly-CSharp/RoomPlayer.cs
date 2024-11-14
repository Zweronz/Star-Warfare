public class RoomPlayer
{
	protected string playerName;

	private byte rankID;

	public byte RankID
	{
		get
		{
			return rankID;
		}
		set
		{
			rankID = value;
		}
	}

	public string getPlayerName()
	{
		return playerName;
	}

	public void setPlayerName(string playerName)
	{
		this.playerName = playerName;
	}
}
