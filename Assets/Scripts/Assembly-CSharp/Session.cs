using System;
using System.IO;
using System.Net.Sockets;

public class Session : ICloneable
{
	public enum ExitType
	{
		NormalExit = 0,
		ExceptionExit = 1
	}

	private SessionId _id;

	private Socket _cliSock;

	private byte[] _datagramBuffer;

	private int _datagramLength;

	private MemoryStream _mStream;

	private ExitType _exitType;

	private FileStream _fsStream;

	private object _datagram;

	public object Datagram
	{
		get
		{
			return _datagram;
		}
		set
		{
			_datagram = value;
		}
	}

	public SessionId ID
	{
		get
		{
			return _id;
		}
	}

	public FileStream FsStream
	{
		get
		{
			return _fsStream;
		}
		set
		{
			_fsStream = value;
		}
	}

	public MemoryStream MStream
	{
		get
		{
			return _mStream;
		}
		set
		{
			_mStream = value;
		}
	}

	public Socket ClientSocket
	{
		get
		{
			return _cliSock;
		}
	}

	public ExitType TypeOfExit
	{
		get
		{
			return _exitType;
		}
		set
		{
			_exitType = value;
		}
	}

	public byte[] DatagramBuffer
	{
		get
		{
			return _datagramBuffer;
		}
		set
		{
			_datagramBuffer = value;
		}
	}

	public int DatagramLength
	{
		get
		{
			return _datagramLength;
		}
		set
		{
			_datagramLength = value;
		}
	}

	public Session(Socket cliSock, int buffer)
	{
		_datagramBuffer = new byte[buffer];
		_cliSock = cliSock;
		_datagramLength = 0;
		_id = new SessionId((int)cliSock.Handle);
	}

	public object Clone()
	{
		Session session = new Session(_cliSock, _datagramLength);
		session.TypeOfExit = _exitType;
		return session;
	}

	public override int GetHashCode()
	{
		return (int)_cliSock.Handle;
	}

	public override bool Equals(object obj)
	{
		Session session = (Session)obj;
		return (int)_cliSock.Handle == (int)session.ClientSocket.Handle;
	}

	public override string ToString()
	{
		return string.Format("Session:{0},IP:{1}", _id, _cliSock.RemoteEndPoint.ToString());
	}

	public void Close()
	{
		_cliSock.Shutdown(SocketShutdown.Both);
		_cliSock.Close();
	}
}
