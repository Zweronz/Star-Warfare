using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class RobotReceiver
{
	private bool isRunning;

	private Thread t;

	public bool isExit;

	private DateTime lastTime = DateTime.Now;

	private int packetNum;

	private int byteNum;

	private List<Robot> robotList;

	public RobotReceiver(List<Robot> list)
	{
		robotList = list;
	}

	public void Init()
	{
		isRunning = true;
		isExit = false;
		t = new Thread(Run);
		t.Start();
	}

	public void Run()
	{
		while (isRunning)
		{
			try
			{
				int count = robotList.Count;
				int num = 0;
				while (num < count)
				{
					Robot robot = robotList[num];
					num++;
					NetworkManager network = robot.network;
					if (network == null)
					{
						continue;
					}
					RobotConnection robotConn = network.robotConn;
					if (robotConn == null)
					{
						continue;
					}
					BinaryReader br = robotConn.br;
					Socket clientSocket = robotConn.ClientSocket;
					if (br == null || !clientSocket.Poll(1, SelectMode.SelectRead))
					{
						continue;
					}
					int available = clientSocket.Available;
					Debug.Log("a:" + available);
					if (robotConn.length == -1)
					{
						if (available < 2)
						{
							continue;
						}
						robotConn.comandID = br.ReadByte();
						robotConn.length = br.ReadByte();
					}
					if (robotConn.length == -1 || !clientSocket.Poll(1, SelectMode.SelectRead))
					{
						continue;
					}
					available = clientSocket.Available;
					Debug.Log("b:" + available);
					if (available >= robotConn.length)
					{
						byte[] array = new byte[robotConn.length];
						DateTime now = DateTime.Now;
						int num2 = br.Read(array, 0, robotConn.length);
						robotConn.length = -1;
						if (robotConn.comandID == 100)
						{
							Response response = Response.CreateResponse((byte)robotConn.comandID);
							response.responseID = (byte)robotConn.comandID;
							response.ReadData(array);
							response.ProcessLogic();
						}
						else
						{
							Response response2 = Response.CreateResponse((byte)robotConn.comandID);
							response2.responseID = (byte)robotConn.comandID;
							response2.ReadData(array);
							network.getReceivedPacketCache().AddPacket(response2);
						}
					}
				}
				Thread.Sleep(25);
			}
			catch (Exception ex)
			{
				Debug.Log(ex.StackTrace);
				break;
			}
		}
		isExit = true;
	}

	private void updatePacket(byte length)
	{
		packetNum++;
		byteNum = byteNum + 2 + length;
		TimeSpan timeSpan = DateTime.Now - lastTime;
		if ((float)timeSpan.TotalSeconds > 1f)
		{
			GameApp.GetInstance().AverageReceivePacket = (float)packetNum / (float)timeSpan.TotalSeconds;
			GameApp.GetInstance().AverageReceiveByte = (float)byteNum / (float)timeSpan.TotalSeconds;
			packetNum = 0;
			byteNum = 0;
			lastTime = DateTime.Now;
		}
	}

	public void StopThread()
	{
		isRunning = false;
		if (t != null)
		{
			t.Interrupt();
		}
	}
}
