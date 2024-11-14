using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class TestHttpScript : MonoBehaviour
{
	public string CryptMD5String(string oriStr)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(oriStr);
		MD5 mD = new MD5CryptoServiceProvider();
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
	}
}
