using System;
using System.Collections;
using System.Xml;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
	public string m_ui_cfgxml;

	public string m_ui_material_path;

	public string m_font_path;

	public UIManager m_UIManagerRef;

	public Hashtable m_control_table;

	public Hashtable m_animations;

	public void Start()
	{
		m_control_table = new Hashtable();
		m_animations = new Hashtable();
		XmlElement xmlElement = null;
		string empty = string.Empty;
		TextAsset textAsset = Resources.Load(m_ui_cfgxml) as TextAsset;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("UIElem" == childNode.Name)
			{
				foreach (XmlNode childNode2 in childNode.ChildNodes)
				{
					xmlElement = (XmlElement)childNode2;
					if ("UIButton" == childNode2.Name)
					{
						UIButtonBase uIButtonBase = null;
						empty = xmlElement.GetAttribute("rect").Trim();
						string[] array = empty.Split(',');
						empty = xmlElement.GetAttribute("type").Trim();
						if ("click" == empty)
						{
							uIButtonBase = new UIClickButton();
							((UIClickButton)uIButtonBase).Rect = new Rect(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()), int.Parse(array[3].Trim()));
						}
						else if ("push" == empty)
						{
							uIButtonBase = new UIPushButton();
							((UIPushButton)uIButtonBase).Rect = new Rect(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()), int.Parse(array[3].Trim()));
						}
						else if ("select" == empty)
						{
							uIButtonBase = new UISelectButton();
							((UISelectButton)uIButtonBase).Rect = new Rect(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()), int.Parse(array[3].Trim()));
						}
						if (uIButtonBase != null)
						{
							empty = xmlElement.GetAttribute("id").Trim();
							uIButtonBase.Id = int.Parse(empty);
							empty = xmlElement.GetAttribute("enable").Trim();
							if (empty.Length > 1)
							{
								uIButtonBase.Enable = "true" == empty;
							}
							empty = xmlElement.GetAttribute("visible").Trim();
							if (empty.Length > 1)
							{
								uIButtonBase.Visible = "true" == empty;
							}
							xmlElement = (XmlElement)childNode2.SelectSingleNode("Normal");
							if (xmlElement != null)
							{
								empty = xmlElement.GetAttribute("rect").Trim();
								array = empty.Split(',');
								empty = xmlElement.GetAttribute("material").Trim();
							}
							xmlElement = (XmlElement)childNode2.SelectSingleNode("Press");
							if (xmlElement != null)
							{
								empty = xmlElement.GetAttribute("rect").Trim();
								array = empty.Split(',');
								empty = xmlElement.GetAttribute("material").Trim();
							}
							xmlElement = (XmlElement)childNode2.SelectSingleNode("Disable");
							if (xmlElement != null)
							{
								empty = xmlElement.GetAttribute("rect").Trim();
								array = empty.Split(',');
								empty = xmlElement.GetAttribute("material").Trim();
							}
							xmlElement = (XmlElement)childNode2.SelectSingleNode("Hover");
							if (xmlElement != null)
							{
								empty = xmlElement.GetAttribute("rect").Trim();
								array = empty.Split(',');
								empty = xmlElement.GetAttribute("material").Trim();
								uIButtonBase.SetHoverSprite(LoadUIMaterial(empty), new Rect(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()), int.Parse(array[3].Trim())));
							}
							m_UIManagerRef.Add(uIButtonBase);
							m_control_table.Add(uIButtonBase.Id, uIButtonBase);
						}
					}
					else if ("UIImage" == childNode2.Name)
					{
						UIImage uIImage = new UIImage();
						empty = xmlElement.GetAttribute("id").Trim();
						uIImage.Id = int.Parse(empty);
						empty = xmlElement.GetAttribute("rect").Trim();
						string[] array2 = empty.Split(',');
						uIImage.Rect = new Rect(int.Parse(array2[0]), int.Parse(array2[1].Trim()), int.Parse(array2[2].Trim()), int.Parse(array2[3].Trim()));
						empty = xmlElement.GetAttribute("enable").Trim();
						if (empty.Length > 1)
						{
							uIImage.Enable = "true" == empty;
						}
						empty = xmlElement.GetAttribute("visible").Trim();
						if (empty.Length > 1)
						{
							uIImage.Visible = "true" == empty;
						}
						xmlElement = (XmlElement)childNode2.SelectSingleNode("Texture");
						if (xmlElement != null)
						{
							empty = xmlElement.GetAttribute("rect").Trim();
							array2 = empty.Split(',');
							empty = xmlElement.GetAttribute("material").Trim();
						}
						m_UIManagerRef.Add(uIImage);
						m_control_table.Add(uIImage.Id, uIImage);
					}
					else if ("UIText" == childNode2.Name)
					{
						FrUIText frUIText = new FrUIText();
						empty = xmlElement.GetAttribute("id").Trim();
						frUIText.Id = int.Parse(empty);
						empty = xmlElement.GetAttribute("rect").Trim();
						string[] array3 = empty.Split(',');
						frUIText.Rect = new Rect(int.Parse(array3[0]), int.Parse(array3[1].Trim()), int.Parse(array3[2].Trim()), int.Parse(array3[3].Trim()));
						empty = xmlElement.GetAttribute("chargap").Trim();
						if (empty.Length > 1)
						{
							frUIText.CharacterSpacing = int.Parse(empty);
						}
						empty = xmlElement.GetAttribute("linegap").Trim();
						if (empty.Length > 1)
						{
							frUIText.LineSpacing = int.Parse(empty);
						}
						empty = xmlElement.GetAttribute("autoline").Trim();
						if (empty.Length > 1)
						{
							frUIText.AutoLine = "true" == empty;
						}
						empty = xmlElement.GetAttribute("align").Trim();
						if (empty.Length > 1)
						{
							frUIText.AlignStyle = (FrUIText.enAlignStyle)(int)Enum.Parse(typeof(FrUIText.enAlignStyle), empty);
						}
						empty = xmlElement.GetAttribute("enable").Trim();
						if (empty.Length > 1)
						{
							frUIText.Enable = "true" == empty;
						}
						empty = xmlElement.GetAttribute("visible").Trim();
						if (empty.Length > 1)
						{
							frUIText.Visible = "true" == empty;
						}
						empty = xmlElement.GetAttribute("font").Trim();
						frUIText.SetFont(m_font_path + empty);
						empty = xmlElement.GetAttribute("color").Trim();
						if (empty.Length > 1)
						{
							array3 = empty.Split(',');
							frUIText.SetColor(new Color((float)int.Parse(array3[0].Trim()) / 255f, (float)int.Parse(array3[1].Trim()) / 255f, (float)int.Parse(array3[2].Trim()) / 255f, (float)int.Parse(array3[3].Trim()) / 255f));
						}
						frUIText.SetText(childNode2.InnerText.Trim(' ', '\t', '\r', '\n'));
						m_UIManagerRef.Add(frUIText);
						m_control_table.Add(frUIText.Id, frUIText);
					}
				}
			}
			else
			{
				if (!("UIAnimation" == childNode.Name))
				{
					continue;
				}
				foreach (XmlNode childNode3 in childNode.ChildNodes)
				{
					xmlElement = (XmlElement)childNode3;
					if ("Animation" != childNode3.Name)
					{
						continue;
					}
					UIAnimations uIAnimations = new UIAnimations();
					empty = xmlElement.GetAttribute("id").Trim();
					uIAnimations.animation_id = int.Parse(empty);
					empty = xmlElement.GetAttribute("control_id").Trim();
					string[] array4 = empty.Split(',');
					for (int i = 0; i < array4.Length; i++)
					{
						UIAnimations.ControlData controlData = new UIAnimations.ControlData();
						controlData.control_id = int.Parse(array4[i].Trim());
						uIAnimations.control_data.Add(controlData);
					}
					xmlElement = (XmlElement)childNode3.SelectSingleNode("Translate");
					if (xmlElement != null)
					{
						uIAnimations.translate_have = true;
						empty = xmlElement.GetAttribute("time").Trim();
						uIAnimations.translate_time = float.Parse(empty);
						empty = xmlElement.GetAttribute("offset").Trim();
						if (empty.Length > 0)
						{
							array4 = empty.Split(',');
							uIAnimations.translate_offset.x = int.Parse(array4[0].Trim());
							uIAnimations.translate_offset.y = int.Parse(array4[1].Trim());
						}
						empty = xmlElement.GetAttribute("restore").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.translate_restore = "true" == empty;
						}
						empty = xmlElement.GetAttribute("loop").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.translate_loop = "true" == empty;
						}
						empty = xmlElement.GetAttribute("reverse").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.translate_reverse = "true" == empty;
						}
					}
					xmlElement = (XmlElement)childNode3.SelectSingleNode("Rotate");
					if (xmlElement != null)
					{
						uIAnimations.rotate_have = true;
						empty = xmlElement.GetAttribute("time").Trim();
						uIAnimations.rotate_time = float.Parse(empty);
						empty = xmlElement.GetAttribute("angle").Trim();
						uIAnimations.rotate_angle = (float)Math.PI / 180f * float.Parse(empty);
						empty = xmlElement.GetAttribute("restore").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.rotate_restore = "true" == empty;
						}
						empty = xmlElement.GetAttribute("loop").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.rotate_loop = "true" == empty;
						}
						empty = xmlElement.GetAttribute("reverse").Trim();
						if (empty.Length > 0)
						{
							uIAnimations.rotate_reverse = "true" == empty;
						}
					}
					m_animations.Add(uIAnimations.animation_id, uIAnimations);
				}
			}
		}
	}

	public void Update()
	{
		foreach (UIAnimations value in m_animations.Values)
		{
			if (!value.IsRuning())
			{
				continue;
			}
			value.Update(Time.deltaTime);
			bool flag = false;
			Vector2 vector = new Vector2(0f, 0f);
			if (value.IsTranslating())
			{
				vector = value.GetTranslate();
				flag = true;
			}
			bool flag2 = false;
			float num = 0f;
			if (value.IsRotating())
			{
				num = value.GetRotate();
				flag2 = true;
			}
			for (int i = 0; i < value.control_data.Count; i++)
			{
				UIAnimations.ControlData controlData = (UIAnimations.ControlData)value.control_data[i];
				int control_id = controlData.control_id;
				string text = m_control_table[control_id].GetType().ToString();
				if ("UIClickButton" == text)
				{
					UIClickButton uIClickButton = (UIClickButton)m_control_table[control_id];
					if (flag)
					{
						uIClickButton.Rect = new Rect(vector.x + controlData.pos.x, vector.y + controlData.pos.y, uIClickButton.Rect.width, uIClickButton.Rect.height);
					}
					if (flag2)
					{
						uIClickButton.SetRotate(num);
					}
				}
				else if ("UIPushButton" == text)
				{
					UIPushButton uIPushButton = (UIPushButton)m_control_table[control_id];
					if (flag)
					{
						uIPushButton.Rect = new Rect(vector.x, vector.y, uIPushButton.Rect.width, uIPushButton.Rect.height);
					}
					if (flag2)
					{
						uIPushButton.SetRotate(num);
					}
				}
				else if ("UISelectButton" == text)
				{
					UISelectButton uISelectButton = (UISelectButton)m_control_table[control_id];
					if (flag)
					{
						uISelectButton.Rect = new Rect(vector.x, vector.y, uISelectButton.Rect.width, uISelectButton.Rect.height);
					}
					if (flag2)
					{
						uISelectButton.SetRotate(num);
					}
				}
				else if ("UIImage" == text)
				{
					UIImage uIImage = (UIImage)m_control_table[control_id];
					if (flag)
					{
						uIImage.Rect = new Rect(vector.x, vector.y, uIImage.Rect.width, uIImage.Rect.height);
					}
					if (flag2)
					{
						uIImage.SetRotation(num);
					}
				}
				else if ("UIText" == text)
				{
					FrUIText frUIText = (FrUIText)m_control_table[control_id];
					if (flag)
					{
						frUIText.Rect = new Rect(vector.x, vector.y, frUIText.Rect.width, frUIText.Rect.height);
					}
				}
			}
		}
	}

	public void StartAnimation(int index)
	{
		UIAnimations uIAnimations = (UIAnimations)m_animations[index];
		for (int i = 0; i < uIAnimations.control_data.Count; i++)
		{
			UIAnimations.ControlData controlData = (UIAnimations.ControlData)uIAnimations.control_data[i];
			int control_id = controlData.control_id;
			string text = m_control_table[control_id].GetType().ToString();
			if ("UIClickButton" == text)
			{
				UIClickButton uIClickButton = (UIClickButton)m_control_table[control_id];
				controlData.pos.x = uIClickButton.Rect.x;
				controlData.pos.y = uIClickButton.Rect.y;
				controlData.angle = uIClickButton.GetRotate();
			}
			else if ("UIPushButton" == text)
			{
				UIPushButton uIPushButton = (UIPushButton)m_control_table[control_id];
				controlData.pos.x = uIPushButton.Rect.x;
				controlData.pos.y = uIPushButton.Rect.y;
				controlData.angle = uIPushButton.GetRotate();
			}
			else if ("UISelectButton" == text)
			{
				UISelectButton uISelectButton = (UISelectButton)m_control_table[control_id];
				controlData.pos.x = uISelectButton.Rect.x;
				controlData.pos.y = uISelectButton.Rect.y;
				controlData.angle = uISelectButton.GetRotate();
			}
			else if ("UIImage" == text)
			{
				UIImage uIImage = (UIImage)m_control_table[control_id];
				controlData.pos.x = uIImage.Rect.x;
				controlData.pos.y = uIImage.Rect.y;
				controlData.angle = uIImage.GetRotation();
			}
			else if ("UIText" == text)
			{
				FrUIText frUIText = (FrUIText)m_control_table[control_id];
				controlData.pos.x = frUIText.Rect.x;
				controlData.pos.y = frUIText.Rect.y;
			}
		}
		uIAnimations.Reset();
		uIAnimations.Start();
	}

	public Material LoadUIMaterial(string name)
	{
		string path = m_ui_material_path + name + "_M";
		Material material = Resources.Load(path) as Material;
		if (material == null)
		{
		}
		return material;
	}
}
