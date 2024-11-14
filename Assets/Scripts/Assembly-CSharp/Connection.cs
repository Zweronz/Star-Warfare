using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class Connection
{
	private bool running;

	private Socket ClientSocket;

	private IPEndPoint ServerInfo;

	private IPEndPoint RobotServerInfo;

	private NetworkStream ns;

	private NetworkManager networkMgr;

	private Receiver receiver;

	private Sender sender;

	private int CLOSE_THREAD_TIMEOUT = 15;

	public Connection(NetworkManager networkMgr)
	{
		this.networkMgr = networkMgr;
		receiver = new Receiver(networkMgr);
		sender = new Sender(networkMgr);
	}

	public void ConnThreadProc()
	{
		try
		{
			ClientSocket.Connect(ServerInfo);
			ns = new NetworkStream(ClientSocket);
			receiver.Init(new BinaryReader(ns));
			sender.Init(new BinaryWriter(ns));
			SetRunning(true);
		}
		catch (Exception ex)
		{
			networkMgr.exceptionMessage = ex.Message;
		}
	}

	public bool IsConnected()
	{
		if (ClientSocket != null)
		{
			return ClientSocket.Connected;
		}
		return false;
	}

	public void StartConn(string ip, int port)
	{
		try
		{
			networkMgr.IsDisconnected = false;
			IPAddress[] hostAddresses = Dns.GetHostAddresses(ip);
			if (hostAddresses[0].AddressFamily == AddressFamily.InterNetworkV6)
			{
				ClientSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
			}
			else
			{
				ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			}
			ServerInfo = new IPEndPoint(hostAddresses[0], port);
			ClientSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, true);
			Thread thread = new Thread(ConnThreadProc);
			thread.Start();
		}
		catch (Exception ex)
		{
			networkMgr.exceptionMessage = ex.Message;
			networkMgr.IsDisconnected = true;
		}
	}

	public void CloseSocket()
	{
		if (ns != null)
		{
			ns.Close();
			ns = null;
		}
		if (ClientSocket == null)
		{
			return;
		}
		try
		{
			ClientSocket.Shutdown(SocketShutdown.Both);
		}
		catch (SocketException ex)
		{
			networkMgr.exceptionMessage = ex.Message;
		}
		finally
		{
			ClientSocket.Close();
			ClientSocket = null;
		}
	}

	public void StopThreadReceiver()
	{
		if (receiver != null)
		{
			receiver.StopThread();
		}
	}

	public void StopThreadSender()
	{
		if (sender != null)
		{
			sender.StopThread();
		}
	}

	public void CloseReceiver()
	{
		if (receiver != null)
		{
			receiver.Close();
		}
	}

	public void CloseSender()
	{
		if (sender != null)
		{
			sender.Close();
		}
	}

	public void Close()
	{
		if (!running)
		{
			return;
		}
		try
		{
			CloseReceiver();
			CloseSender();
			CloseSocket();
			StopThreadReceiver();
			StopThreadSender();
			DateTime now = DateTime.Now;
			TimeSpan timeSpan = DateTime.Now - now;
			float time = Time.time;
			while ((!sender.isExit || !receiver.isExit) && timeSpan.TotalSeconds <= (double)CLOSE_THREAD_TIMEOUT)
			{
				timeSpan = DateTime.Now - now;
			}
			sender = null;
			receiver = null;
		}
		catch (IOException ex)
		{
			networkMgr.exceptionMessage = ex.Message;
		}
		running = false;
	}

	public bool IsRunning()
	{
		return running;
	}

	public void SetRunning(bool run)
	{
		running = run;
	}
}
