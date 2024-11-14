using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class RobotConnection
{
	public int comandID;

	public int length = -1;

	public Socket ClientSocket;

	private IPEndPoint ServerInfo;

	private IPEndPoint RobotServerInfo;

	private NetworkStream ns;

	private NetworkManager networkMgr;

	public BinaryWriter bw;

	public BinaryReader br;

	public bool IsConnected;

	private Session session;

	private int CLOSE_THREAD_TIMEOUT = 15;

	public RobotConnection(NetworkManager networkMgr)
	{
		this.networkMgr = networkMgr;
	}

	public void ConnThreadProc()
	{
		try
		{
			ClientSocket.Connect(ServerInfo);
			ns = new NetworkStream(ClientSocket);
			br = new BinaryReader(ns);
			bw = new BinaryWriter(ns);
		}
		catch (Exception ex)
		{
			networkMgr.exceptionMessage = ex.Message;
		}
	}

	public void Connected(IAsyncResult iar)
	{
		Socket socket = (Socket)iar.AsyncState;
		socket.EndConnect(iar);
		IsConnected = true;
		session = new Session(socket, 1024);
		session.ClientSocket.BeginReceive(session.DatagramBuffer, 0, 1024, SocketFlags.None, RecvData, socket);
	}

	public void SendData(Request request)
	{
		try
		{
			byte[] bytes = request.GetBytes();
			session.ClientSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendCallback, session.ClientSocket);
			Debug.Log(request);
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message + "***\r\n" + ex.StackTrace);
		}
	}

	public void SendCallback(IAsyncResult iar)
	{
		Socket socket = (Socket)iar.AsyncState;
		socket.EndSend(iar);
	}

	public void RecvData(IAsyncResult iar)
	{
		Socket socket = (Socket)iar.AsyncState;
		try
		{
			int num = socket.EndReceive(iar);
			session.DatagramLength += num;
			if (num == 0)
			{
				session.TypeOfExit = Session.ExitType.NormalExit;
				CloseSocket();
			}
			while (true)
			{
				lock (this)
				{
					Message message = new Message();
					MessageStream messageStream = new MessageStream();
					messageStream.Write(session.DatagramBuffer, 0, session.DatagramLength);
					if (messageStream.Read(out message))
					{
						object obj = null;
						byte[] array = new byte[session.DatagramLength - 2 - message.Content.Length];
						Array.Copy(session.DatagramBuffer, message.Content.Length + 2, array, 0, array.Length);
						Array.Clear(session.DatagramBuffer, 0, session.DatagramLength);
						Array.Copy(array, 0, session.DatagramBuffer, 0, array.Length);
						session.DatagramLength = array.Length;
						Response response = Response.CreateResponse(message.Class);
						response.responseID = message.Class;
						response.ReadData(message.Content);
						networkMgr.getReceivedPacketCache().AddPacket(response);
						continue;
					}
					session.ClientSocket.BeginReceive(session.DatagramBuffer, session.DatagramLength, session.DatagramBuffer.Length - session.DatagramLength, SocketFlags.None, RecvData, session.ClientSocket);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message + "***\r\n" + ex.StackTrace);
		}
	}

	public void StartConn(string ip, int port)
	{
		int num = 654311424;
		try
		{
			networkMgr.IsDisconnected = false;
			ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			int ioctl_code = -1744830460;
			byte[] in_value = new byte[12]
			{
				1, 0, 0, 0, 16, 39, 0, 0, 232, 3,
				0, 0
			};
			ClientSocket.IOControl(ioctl_code, in_value, null);
			ServerInfo = new IPEndPoint(IPAddress.Parse(ip), port);
			ClientSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, true);
			ClientSocket.Blocking = false;
			ClientSocket.BeginConnect(ServerInfo, Connected, ClientSocket);
		}
		catch (Exception ex)
		{
			networkMgr.exceptionMessage = ex.Message;
			networkMgr.IsDisconnected = true;
		}
	}

	public void CloseSocket()
	{
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
}
