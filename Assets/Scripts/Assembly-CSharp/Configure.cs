using System.Collections;

public class Configure
{
	private enum ValueType
	{
		Single = 0,
		Array = 1,
		Array2 = 2
	}

	private class Value
	{
		public string key;

		public ValueType type;

		public string val_single;

		public ArrayList val_array = new ArrayList();

		public ArrayList val_array2 = new ArrayList();

		public string comment1;

		public string comment2;
	}

	private class Section
	{
		public string section;

		public string comment1;

		public string comment2;

		public ArrayList values = new ArrayList();
	}

	public ArrayList m_configure = new ArrayList();

	public bool Load(string data)
	{
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		ArrayList arrayList3 = new ArrayList();
		int length = data.Length;
		int num = 0;
		string empty = string.Empty;
		string text = string.Empty;
		while (true)
		{
			int i;
			for (i = num; i < length && (data[i] == '\r' || data[i] == '\n'); i++)
			{
			}
			if (i >= length)
			{
				break;
			}
			int j;
			for (j = i + 1; j < length && data[j] != '\r' && data[j] != '\n'; j++)
			{
			}
			num = j + 1;
			empty = data.Substring(i, j - i);
			string text2 = string.Empty;
			int num2 = empty.IndexOf('#');
			if (num2 != -1)
			{
				text2 = empty.Substring(num2 + 1);
				empty = empty.Substring(0, num2);
			}
			empty = empty.Trim();
			if (empty.Length < 1)
			{
				if (text2.Length >= 1)
				{
					text = text2;
				}
			}
			else
			{
				string value = empty;
				arrayList.Add(value);
				arrayList2.Add(text.Trim());
				arrayList3.Add(text2.Trim());
				text = string.Empty;
			}
		}
		string section = string.Empty;
		for (int k = 0; k < arrayList.Count; k++)
		{
			string text3 = arrayList[k] as string;
			if (text3.Length > 2 && text3[0] == '[' && text3[text3.Length - 1] == ']')
			{
				section = text3.Substring(1, text3.Length - 2);
				AddSection(section, arrayList2[k] as string, arrayList3[k] as string);
				continue;
			}
			int num3 = text3.IndexOf('=');
			if (num3 == -1)
			{
				continue;
			}
			string text4 = text3.Substring(0, num3);
			string text5 = text3.Substring(num3 + 1);
			text4 = text4.Trim();
			text5 = text5.Trim();
			if (text5 == "(")
			{
				ArrayList arrayList4 = new ArrayList();
				int index = k;
				for (k++; k < arrayList.Count; k++)
				{
					text3 = arrayList[k] as string;
					if (text3[0] == ')')
					{
						break;
					}
					if (text3.Length > 2 && text3[0] == '(' && text3[text3.Length - 1] == ')')
					{
						string value2 = text3.Substring(1, text3.Length - 2);
						arrayList4.Add(value2);
						continue;
					}
					return false;
				}
				AddValueArray2(section, text4, arrayList4, arrayList2[index] as string, arrayList3[index] as string);
			}
			else if (text5.Length > 2 && text5[0] == '(' && text5[text5.Length - 1] == ')')
			{
				string line = text5.Substring(1, text5.Length - 2);
				AddValueArray(section, text4, line, arrayList2[k] as string, arrayList3[k] as string);
			}
			else
			{
				AddValueSingle(section, text4, text5, arrayList2[k] as string, arrayList3[k] as string);
			}
		}
		return true;
	}

	public string Save()
	{
		string text = string.Empty;
		for (int i = 0; i < m_configure.Count; i++)
		{
			Section section = m_configure[i] as Section;
			string text2;
			if (section.comment1.Length >= 1)
			{
				text2 = string.Format("#{0}\n", section.comment1);
				text += text2;
			}
			text2 = string.Format("[{0}]", section.section);
			text += text2;
			if (section.comment2.Length > 1)
			{
				text2 = string.Format("\t#{0}", section.comment2);
				text += text2;
			}
			text += "\n";
			for (int j = 0; j < section.values.Count; j++)
			{
				Value value = section.values[j] as Value;
				if (value.comment1.Length >= 1)
				{
					text2 = string.Format("#{0}\n", value.comment1);
					text += text2;
				}
				if (value.type == ValueType.Single)
				{
					text2 = string.Format("{0} = {1}", value.key, value.val_single);
					text += text2;
					if (value.comment2.Length > 1)
					{
						text2 = string.Format("\t#{0}", value.comment2);
						text += text2;
					}
					text += "\n";
				}
				else if (value.type == ValueType.Array)
				{
					text2 = string.Format("{0} = (", value.key);
					text += text2;
					for (int k = 0; k < value.val_array.Count; k++)
					{
						if (k != value.val_array.Count - 1)
						{
							text += value.val_array[k];
							text += " ";
						}
						else
						{
							text += value.val_array[k];
						}
					}
					text += ")";
					if (value.comment2.Length > 1)
					{
						text2 = string.Format("\t#{0}", value.comment2);
						text += text2;
					}
					text += "\n";
				}
				else
				{
					if (value.type != ValueType.Array2)
					{
						continue;
					}
					text2 = string.Format("{0} = (", value.key);
					text += text2;
					if (value.comment2.Length > 1)
					{
						text2 = string.Format("\t#{0}", value.comment2);
						text += text2;
					}
					text += "\n";
					for (int l = 0; l < value.val_array2.Count; l++)
					{
						text += "\t(";
						ArrayList arrayList = value.val_array2[l] as ArrayList;
						for (int m = 0; m < arrayList.Count; m++)
						{
							if (m != arrayList.Count - 1)
							{
								text += arrayList[m];
								text += " ";
							}
							else
							{
								text += arrayList[m];
							}
						}
						text += ")\n";
					}
					text += ")\n";
				}
			}
			text += "\n\n";
		}
		return text;
	}

	public string GetSingle(string section, string key)
	{
		Value value = GetValue(section, key);
		if (value == null)
		{
			return null;
		}
		if (value.type != 0)
		{
			return null;
		}
		return value.val_single;
	}

	public int CountArray(string section, string key)
	{
		Value value = GetValue(section, key);
		if (value == null)
		{
			return -1;
		}
		if (value.type != ValueType.Array)
		{
			return -1;
		}
		return value.val_array.Count;
	}

	public string GetArray(string section, string key, int i)
	{
		Value value = GetValue(section, key);
		if (value == null)
		{
			return null;
		}
		if (value.type != ValueType.Array)
		{
			return null;
		}
		int count = value.val_array.Count;
		if (i < 0 || i >= count)
		{
			return null;
		}
		return value.val_array[i] as string;
	}

	public int CountArray2(string section, string key)
	{
		Value value = GetValue(section, key);
		if (value == null)
		{
			return -1;
		}
		if (value.type != ValueType.Array2)
		{
			return -1;
		}
		return value.val_array2.Count;
	}

	public int CountArray2(string section, string key, int i)
	{
		Value value = GetValue(section, key);
		if (value == null)
		{
			return -1;
		}
		if (value.type != ValueType.Array2)
		{
			return -1;
		}
		int count = value.val_array2.Count;
		if (i < 0 || i >= count)
		{
			return -1;
		}
		ArrayList arrayList = value.val_array2[i] as ArrayList;
		return arrayList.Count;
	}

	public string GetArray2(string section, string key, int i, int j)
	{
		Value value = GetValue(section, key);
		if (value == null)
		{
			return null;
		}
		if (value.type != ValueType.Array2)
		{
			return null;
		}
		int count = value.val_array2.Count;
		if (i < 0 || i >= count)
		{
			return null;
		}
		ArrayList arrayList = value.val_array2[i] as ArrayList;
		int count2 = arrayList.Count;
		if (j < 0 || j >= count2)
		{
			return null;
		}
		return arrayList[j] as string;
	}

	public bool SetSingle(string section, string key, string val)
	{
		Value value = GetValue(section, key);
		if (value == null)
		{
			return false;
		}
		if (value.type != 0)
		{
			return false;
		}
		value.val_single = val;
		return true;
	}

	public bool SetArray(string section, string key, ArrayList val)
	{
		Value value = GetValue(section, key);
		if (value == null)
		{
			return false;
		}
		if (value.type != ValueType.Array)
		{
			return false;
		}
		value.val_array = val;
		return true;
	}

	public bool SetArray2(string section, string key, ArrayList val)
	{
		Value value = GetValue(section, key);
		if (value == null)
		{
			return false;
		}
		if (value.type != ValueType.Array2)
		{
			return false;
		}
		value.val_array2 = val;
		return true;
	}

	private Section GetSection(string section)
	{
		for (int i = 0; i < m_configure.Count; i++)
		{
			Section section2 = m_configure[i] as Section;
			if (section2.section == section)
			{
				return section2;
			}
		}
		return null;
	}

	private Value GetValue(string section, string key)
	{
		Section section2 = GetSection(section);
		if (section2 == null)
		{
			return null;
		}
		for (int i = 0; i < section2.values.Count; i++)
		{
			Value value = section2.values[i] as Value;
			if (value.key == key)
			{
				return value;
			}
		}
		return null;
	}

	private bool AddSection(string section, string comment1, string comment2)
	{
		Section section2 = GetSection(section);
		if (section2 != null)
		{
			return false;
		}
		Section section3 = new Section();
		section3.section = section;
		section3.comment1 = comment1;
		section3.comment2 = comment2;
		m_configure.Add(section3);
		return true;
	}

	private bool AddValueSingle(string section, string key, string value, string comment1, string comment2)
	{
		Section section2 = GetSection(section);
		if (section2 == null)
		{
			return false;
		}
		Value value2 = GetValue(section, key);
		if (value2 != null)
		{
			return false;
		}
		Value value3 = new Value();
		value3.key = key;
		value3.type = ValueType.Single;
		value3.val_single = value;
		value3.comment1 = comment1;
		value3.comment2 = comment2;
		section2.values.Add(value3);
		return true;
	}

	private bool AddValueArray(string section, string key, string line, string comment1, string comment2)
	{
		Section section2 = GetSection(section);
		if (section2 == null)
		{
			return false;
		}
		Value value = GetValue(section, key);
		if (value != null)
		{
			return false;
		}
		Value value2 = new Value();
		value2.key = key;
		value2.type = ValueType.Array;
		value2.comment1 = comment1;
		value2.comment2 = comment2;
		string text = line.Replace('\t', ' ');
		while (true)
		{
			text = text.Trim();
			int num = text.IndexOf(' ');
			if (num == -1)
			{
				break;
			}
			string value3 = text.Substring(0, num);
			text = text.Substring(num + 1);
			value2.val_array.Add(value3);
		}
		text = text.Trim();
		if (text.Length > 0)
		{
			value2.val_array.Add(text);
		}
		section2.values.Add(value2);
		return true;
	}

	private bool AddValueArray2(string section, string key, ArrayList lines, string comment1, string comment2)
	{
		Section section2 = GetSection(section);
		if (section2 == null)
		{
			return false;
		}
		Value value = GetValue(section, key);
		if (value != null)
		{
			return false;
		}
		Value value2 = new Value();
		value2.key = key;
		value2.type = ValueType.Array2;
		value2.comment1 = comment1;
		value2.comment2 = comment2;
		for (int i = 0; i < lines.Count; i++)
		{
			ArrayList arrayList = new ArrayList();
			string text = lines[i] as string;
			text = text.Replace('\t', ' ');
			while (true)
			{
				text = text.Trim();
				int num = text.IndexOf(' ');
				if (num == -1)
				{
					break;
				}
				string value3 = text.Substring(0, num);
				text = text.Substring(num + 1);
				arrayList.Add(value3);
			}
			text = text.Trim();
			if (text.Length > 0)
			{
				arrayList.Add(text);
			}
			value2.val_array2.Add(arrayList);
		}
		section2.values.Add(value2);
		return true;
	}
}
