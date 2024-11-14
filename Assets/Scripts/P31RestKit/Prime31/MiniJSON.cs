using System;
using System.Collections;
using System.Globalization;

namespace Prime31
{
	public class MiniJSON
	{
		private const int TOKEN_NONE = 0;

		private const int TOKEN_CURLY_OPEN = 1;

		private const int TOKEN_CURLY_CLOSE = 2;

		private const int TOKEN_SQUARED_OPEN = 3;

		private const int TOKEN_SQUARED_CLOSE = 4;

		private const int TOKEN_COLON = 5;

		private const int TOKEN_COMMA = 6;

		private const int TOKEN_STRING = 7;

		private const int TOKEN_NUMBER = 8;

		private const int TOKEN_TRUE = 9;

		private const int TOKEN_FALSE = 10;

		private const int TOKEN_NULL = 11;

		private const int BUILDER_CAPACITY = 2000;

		protected static int lastErrorIndex = -1;

		protected static string lastDecode = "";

		public static object jsonDecode(string json)
		{
			lastDecode = json;
			if (json != null)
			{
				char[] json2 = json.ToCharArray();
				int index = 0;
				bool success = true;
				object result = parseValue(json2, ref index, ref success);
				if (success)
				{
					lastErrorIndex = -1;
				}
				else
				{
					lastErrorIndex = index;
				}
				return result;
			}
			return null;
		}

		protected static Hashtable parseObject(char[] json, ref int index)
		{
			Hashtable hashtable = new Hashtable();
			nextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				switch (lookAhead(json, index))
				{
				case 0:
					return null;
				case 6:
					nextToken(json, ref index);
					continue;
				case 2:
					nextToken(json, ref index);
					return hashtable;
				}
				string text = parseString(json, ref index);
				if (text == null)
				{
					return null;
				}
				int num = nextToken(json, ref index);
				if (num != 5)
				{
					return null;
				}
				bool success = true;
				object value = parseValue(json, ref index, ref success);
				if (!success)
				{
					return null;
				}
				hashtable[text] = value;
			}
			return hashtable;
		}

		protected static ArrayList parseArray(char[] json, ref int index)
		{
			ArrayList arrayList = new ArrayList();
			nextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				switch (lookAhead(json, index))
				{
				case 0:
					return null;
				case 6:
					nextToken(json, ref index);
					continue;
				case 4:
					break;
				default:
				{
					bool success = true;
					object value = parseValue(json, ref index, ref success);
					if (!success)
					{
						return null;
					}
					arrayList.Add(value);
					continue;
				}
				}
				nextToken(json, ref index);
				break;
			}
			return arrayList;
		}

		protected static object parseValue(char[] json, ref int index, ref bool success)
		{
			switch (lookAhead(json, index))
			{
			case 7:
				return parseString(json, ref index);
			case 8:
				return parseNumber(json, ref index);
			case 1:
				return parseObject(json, ref index);
			case 3:
				return parseArray(json, ref index);
			case 9:
				nextToken(json, ref index);
				return bool.Parse("TRUE");
			case 10:
				nextToken(json, ref index);
				return bool.Parse("FALSE");
			case 11:
				nextToken(json, ref index);
				return null;
			default:
				success = false;
				return null;
			}
		}

		protected static string parseString(char[] json, ref int index)
		{
			string text = "";
			eatWhitespace(json, ref index);
			char c = json[index++];
			bool flag = false;
			while (!flag && index != json.Length)
			{
				c = json[index++];
				switch (c)
				{
				case '"':
					flag = true;
					break;
				case '\\':
					if (index != json.Length)
					{
						switch (json[index++])
						{
						case '"':
							text += '"';
							continue;
						case '\\':
							text += '\\';
							continue;
						case '/':
							text += '/';
							continue;
						case 'b':
							text += '\b';
							continue;
						case 'f':
							text += '\f';
							continue;
						case 'n':
							text += '\n';
							continue;
						case 'r':
							text += '\r';
							continue;
						case 't':
							text += '\t';
							continue;
						case 'u':
							break;
						default:
							continue;
						}
						int num = json.Length - index;
						if (num >= 4)
						{
							char[] array = new char[4];
							Array.Copy(json, index, array, 0, 4);
							uint utf = uint.Parse(new string(array), NumberStyles.HexNumber);
							text += char.ConvertFromUtf32((int)utf);
							index += 4;
							continue;
						}
					}
					break;
				default:
					text += c;
					continue;
				}
				break;
			}
			if (!flag)
			{
				return null;
			}
			return text;
		}

		protected static double parseNumber(char[] json, ref int index)
		{
			eatWhitespace(json, ref index);
			int lastIndexOfNumber = getLastIndexOfNumber(json, index);
			int num = lastIndexOfNumber - index + 1;
			char[] array = new char[num];
			Array.Copy(json, index, array, 0, num);
			index = lastIndexOfNumber + 1;
			return double.Parse(new string(array));
		}

		protected static int getLastIndexOfNumber(char[] json, int index)
		{
			int i;
			for (i = index; i < json.Length && "0123456789+-.eE".IndexOf(json[i]) != -1; i++)
			{
			}
			return i - 1;
		}

		protected static void eatWhitespace(char[] json, ref int index)
		{
			while (index < json.Length && " \t\n\r".IndexOf(json[index]) != -1)
			{
				index++;
			}
		}

		protected static int lookAhead(char[] json, int index)
		{
			int index2 = index;
			return nextToken(json, ref index2);
		}

		protected static int nextToken(char[] json, ref int index)
		{
			eatWhitespace(json, ref index);
			if (index == json.Length)
			{
				return 0;
			}
			char c = json[index];
			index++;
			switch (c)
			{
			case '{':
				return 1;
			case '}':
				return 2;
			case '[':
				return 3;
			case ']':
				return 4;
			case ',':
				return 6;
			case '"':
				return 7;
			case '-':
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				return 8;
			case ':':
				return 5;
			default:
			{
				index--;
				int num = json.Length - index;
				if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
				{
					index += 5;
					return 10;
				}
				if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
				{
					index += 4;
					return 9;
				}
				if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
				{
					index += 4;
					return 11;
				}
				return 0;
			}
			}
		}
	}
}
