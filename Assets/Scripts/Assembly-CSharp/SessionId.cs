public class SessionId
{
	private int _id;

	public int ID
	{
		get
		{
			return _id;
		}
	}

	public SessionId(int id)
	{
		_id = id;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			SessionId sessionId = (SessionId)obj;
			return _id == sessionId._id;
		}
		if (this == null)
		{
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _id;
	}

	public override string ToString()
	{
		return _id.ToString();
	}
}
