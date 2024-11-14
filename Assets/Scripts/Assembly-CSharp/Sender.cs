using System;
using System.IO;
using System.Threading;
using UnityEngine;

public class Sender
{
	public const int POLL_TIME = 20;

	public const int WRITE_BUF_SIZE = 512;

	private BinaryWriter bw;

	private bool isRunning;

	private Thread t;

	private NetworkManager networkMgr;

	public bool isExit;

	protected DateTime lastTime;

	private byte[] writebuffer = new byte[1024];

	public Sender(NetworkManager networkMgr)
	{
		this.networkMgr = networkMgr;
	}

	public void Init(BinaryWriter bw)
	{
		isRunning = true;
		isExit = false;
		this.bw = bw;
		t = new Thread(Run);
		t.Start();
		lastTime = DateTime.Now;
	}

	public void Run()
	{
		while (isRunning)
		{
			try
			{
				if (bw != null)
				{
					int num = 0;
					bool flag = false;
					while (!networkMgr.getSendPacketCache().isEmpty() && !flag)
					{
						Request request = (Request)networkMgr.getSendPacketCache().SendPacket();
						if (request != null)
						{
							byte[] bytes = request.GetBytes();
							if (bytes.Length + num >= 512)
							{
								flag = true;
							}
							bytes.CopyTo(writebuffer, num);
							num += bytes.Length;
						}
					}
					if (num > 0)
					{
						bw.Write(writebuffer, 0, num);
						bw.Flush();
					}
				}
				else
				{
					Debug.Log("bw is null..");
				}
				Thread.Sleep(20);
			}
			catch (Exception ex)
			{
				Debug.Log("sender:" + ex.Message + "\r\n" + ex.StackTrace);
				networkMgr.exceptionMessage += ex.Message;
				networkMgr.IsDisplayErrorBox = true;
				break;
			}
		}
		networkMgr.IsDisconnected = true;
		isExit = true;
	}

	private byte[] extendWriteBuf(byte[] _writeBuf)
	{
		byte[] array = new byte[_writeBuf.Length * 2];
		_writeBuf.CopyTo(array, 0);
		return array;
	}

	public void StopThread()
	{
		isRunning = false;
	}

	public void Close()
	{
		try
		{
			if (bw != null)
			{
				bw.Close();
				bw = null;
			}
		}
		catch (IOException ex)
		{
			networkMgr.exceptionMessage = ex.Message;
		}
	}
}
