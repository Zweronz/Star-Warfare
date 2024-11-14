using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class HttpRequest
{
	private const string postHeader = "POST {0} HTTP/1.1\r\nHost: {1}\r\nUser-Agent: Nokia5700AP23.01/SymbianOS/9.1 Series60/3.0\r\nAccept: */*\r\nCache-Control: no-cache\r\nContent-Length: {2}\r\nContent-Type: application/x-www-form-urlencoded\r\n\r\n{3}";

	private const string getHeader = "GET {0} HTTP/1.1\r\nHost: {1}\r\nUser-Agent: NNokia5700AP23.01/SymbianOS/9.1 Series60/3.0\r\nAccept: */*\r\nAccept-Encoding: gzip,deflate\r\nCache-Control: no-cache\r\n\r\n";

	public static string GetWebRequest(string url, string msg, int trytimes)
	{
		return Request(url, msg, trytimes);
	}

	public static string Get(Uri uri)
	{
		return Request(uri, null, 2);
	}

	public static string Get(string url)
	{
		return Request(url, null, 2);
	}

	public static string Post(Uri uri, string msg)
	{
		return Request(uri, msg, 2);
	}

	public static string Post(string url, string msg)
	{
		return Request(url, msg, 2);
	}

	private static string Request(string url, string msg, int tryTimes)
	{
		Uri uri = new Uri(url);
		return Request(uri, msg, tryTimes);
	}

	private static string Request(Uri uri, string msg, int tryTimes)
	{
		msg = ((!string.IsNullOrEmpty(msg)) ? string.Format("POST {0} HTTP/1.1\r\nHost: {1}\r\nUser-Agent: Nokia5700AP23.01/SymbianOS/9.1 Series60/3.0\r\nAccept: */*\r\nCache-Control: no-cache\r\nContent-Length: {2}\r\nContent-Type: application/x-www-form-urlencoded\r\n\r\n{3}", uri.PathAndQuery, uri.Host, Encoding.UTF8.GetByteCount(msg), msg) : string.Format("GET {0} HTTP/1.1\r\nHost: {1}\r\nUser-Agent: NNokia5700AP23.01/SymbianOS/9.1 Series60/3.0\r\nAccept: */*\r\nAccept-Encoding: gzip,deflate\r\nCache-Control: no-cache\r\n\r\n", uri.PathAndQuery, uri.Host));
		TcpClient tcpClient = new TcpClient();
		NetworkStream networkStream = null;
		MemoryStream memoryStream = null;
		try
		{
			tcpClient.Connect(uri.Host, uri.Port);
			networkStream = tcpClient.GetStream();
			byte[] bytes = Encoding.UTF8.GetBytes(msg);
			networkStream.Write(bytes, 0, bytes.Length);
			networkStream.Flush();
			if (GetStateCode(networkStream) != 200)
			{
				return string.Empty;
			}
			WebHeaderCollection webHeaderCollection = ParseHttpHeader(networkStream);
			memoryStream = new MemoryStream();
			byte[] array = new byte[20480];
			int num = 0;
			while (networkStream.DataAvailable && (num = networkStream.Read(array, 0, array.Length)) > 0)
			{
				memoryStream.Write(array, 0, num);
			}
			memoryStream.Position = 0L;
			using (StreamReader streamReader = new StreamReader(memoryStream))
			{
				return streamReader.ReadToEnd();
			}
		}
		catch (Exception ex)
		{
			if (tryTimes <= 0)
			{
				throw ex;
			}
			Thread.Sleep(2000);
			return Request(uri, msg, --tryTimes);
		}
		finally
		{
			if (networkStream != null)
			{
				networkStream.Dispose();
			}
			if (tcpClient != null)
			{
				tcpClient.Close();
			}
			if (memoryStream != null)
			{
				memoryStream.Dispose();
			}
		}
	}

	public static int GetStateCode(NetworkStream stream)
	{
		string nextHeaderLine = GetNextHeaderLine(stream);
		if (string.IsNullOrEmpty(nextHeaderLine))
		{
			return 0;
		}
		string[] array = nextHeaderLine.Split(' ');
		if (array.Length != 3)
		{
			return 0;
		}
		int result = 0;
		if (int.TryParse(array[1], out result))
		{
			return result;
		}
		return 0;
	}

	public static WebHeaderCollection ParseHttpHeader(NetworkStream stream)
	{
		WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
		string nextHeaderLine;
		while ((nextHeaderLine = GetNextHeaderLine(stream)).Length != 0)
		{
			try
			{
				webHeaderCollection.Add(nextHeaderLine);
			}
			catch
			{
			}
		}
		return webHeaderCollection;
	}

	public static bool CanConnect(string host)
	{
		return CanConnect(host, 80);
	}

	public static bool CanConnect(string host, int port)
	{
		bool result = false;
		try
		{
			using (TcpClient tcpClient = new TcpClient())
			{
				int sendTimeout = (tcpClient.ReceiveTimeout = 2000);
				tcpClient.SendTimeout = sendTimeout;
				tcpClient.Connect(host, port);
				result = tcpClient.Connected;
				tcpClient.Close();
			}
		}
		catch
		{
		}
		return result;
	}

	private static string GetNextHeaderLine(NetworkStream stream)
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			int num;
			while ((num = stream.ReadByte()) != 10)
			{
				switch (num)
				{
				default:
					memoryStream.WriteByte((byte)num);
					continue;
				case 13:
					continue;
				case -1:
					break;
				}
				break;
			}
			if (memoryStream.Length == 0L)
			{
				return string.Empty;
			}
			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}
	}
}
