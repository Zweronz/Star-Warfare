using UnityEngine;

public class SynQueue
{
	private int tail = 1;

	private int head;

	private object[] elements;

	public SynQueue(int capability)
	{
		elements = new object[capability];
	}

	public bool offer(object e)
	{
		if ((tail + 1) % elements.Length == head)
		{
			return false;
		}
		elements[tail] = e;
		tail = (tail + 1) % elements.Length;
		return true;
	}

	public object poll()
	{
		if ((head + 1) % elements.Length == tail || elements[(head + 1) % elements.Length] == null)
		{
			return null;
		}
		head = (head + 1) % elements.Length;
		return elements[head];
	}

	public int getTail()
	{
		return tail;
	}

	public int getHead()
	{
		return head;
	}

	public int getSize()
	{
		return elements.Length;
	}

	public object peek()
	{
		return elements[head];
	}

	public bool isEmpty()
	{
		return ((head + 1) % elements.Length == tail) ? true : false;
	}

	public void clear()
	{
		for (int i = 0; i < elements.Length; i++)
		{
			elements[i] = null;
		}
		tail = 1;
		head = 0;
	}

	public void clearHead()
	{
		elements[head] = null;
	}

	public int getDelta()
	{
		int num = head;
		int num2 = tail;
		if (num2 < num)
		{
			return elements.Length - num + num2 - 1;
		}
		return num2 - num - 1;
	}

	public void Print()
	{
		Debug.Log("print send queue:" + getDelta());
		if (tail > head + 1)
		{
			for (int i = head + 1; i < tail; i++)
			{
				Debug.Log(elements[i].GetType());
			}
		}
		else if (head < elements.Length - 1)
		{
			for (int j = head + 1; j < elements.Length; j++)
			{
				Debug.Log(elements[j].GetType());
			}
			for (int k = 0; k < tail; k++)
			{
				Debug.Log(elements[k].GetType());
			}
		}
	}
}
