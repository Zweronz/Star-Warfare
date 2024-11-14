public class SendPacketCache
{
	public SynQueue synQueue = new SynQueue(10000);

	public object SendPacket()
	{
		object obj = synQueue.poll();
		if (synQueue.getDelta() > 50)
		{
		}
		Request request = obj as Request;
		return obj;
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
