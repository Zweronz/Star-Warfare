using System;
using System.Text;

public class AntiCracking
{
	public const string CASH_KEY = "sw_acc";

	public const string MITHRIL_KEY = "sw_mod";

	public const string EXP_KEY = "no_add";

	public const string SKILL_POINT_KEY = "skillZ";

	public const string HP_KEY = "xue";

	public const string SHIELD_KEY = "dun";

	public static string CryptBufferStr(int n, string key)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(n.ToString());
		byte[] bytes2 = Encoding.ASCII.GetBytes(key);
		byte[] array = new byte[bytes.Length];
		for (int i = 0; i < bytes.Length; i++)
		{
			array[i] = (byte)(bytes[i] ^ bytes2[i % bytes2.Length]);
		}
		return Encoding.ASCII.GetString(array);
	}

	public static int DecryptBufferStr(string str, string key)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(str);
		byte[] bytes2 = Encoding.ASCII.GetBytes(key);
		byte[] array = new byte[bytes.Length];
		for (int i = 0; i < bytes.Length; i++)
		{
			array[i] = (byte)(bytes[i] ^ bytes2[i % bytes2.Length]);
		}
		string @string = Encoding.ASCII.GetString(array);
		return Convert.ToInt32(@string);
	}
}
