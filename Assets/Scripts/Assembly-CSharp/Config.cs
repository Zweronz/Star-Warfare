using System.IO;
using System.Xml;
using UnityEngine;

public class Config
{
	private Stream stream;

	private string ip;

	private int port;

	public void Load()
	{
		XmlDocument xmlDocument = new XmlDocument();
		if (!Application.isEditor && Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
		{
			string text = Application.dataPath + "/";
			stream = File.Open(text + "config.xml", FileMode.Open);
			xmlDocument.Load(stream);
			stream.Close();
		}
		else
		{
			TextAsset textAsset = Resources.Load("Config/config") as TextAsset;
			xmlDocument.LoadXml(textAsset.text);
		}
		XmlNode xmlNode = xmlDocument.SelectSingleNode("Config/Server");
		ip = xmlNode.Attributes["IP"].Value;
		port = int.Parse(xmlNode.Attributes["Port"].Value);
	}

	public string GetIP()
	{
		return ip;
	}

	public int GetPort()
	{
		return port;
	}
}
