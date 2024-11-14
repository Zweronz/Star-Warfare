public class ReceivedPacketCache
{
	private SynQueue synQueue = new SynQueue(10000);

	public object RetrivePacket()
	{
		return synQueue.poll();
	}

	public void AddPacket(object p)
	{
		synQueue.offer(p);
	}

	public void Clear()
	{
		if (synQueue != null)
		{
			synQueue.clear();
		}
	}

	public bool isEmpty()
	{
		return synQueue.isEmpty();
	}
}
