using System;
using UnityEngine;

public class JPGEncoder
{
	public int[] ZigZag = new int[64]
	{
		0, 1, 5, 6, 14, 15, 27, 28, 2, 4,
		7, 13, 16, 26, 29, 42, 3, 8, 12, 17,
		25, 30, 41, 43, 9, 11, 18, 24, 31, 40,
		44, 53, 10, 19, 23, 32, 39, 45, 52, 54,
		20, 22, 33, 38, 46, 51, 55, 60, 21, 34,
		37, 47, 50, 56, 59, 61, 35, 36, 48, 49,
		57, 58, 62, 63
	};

	private int[] YTable = new int[64];

	private int[] UVTable = new int[64];

	private float[] fdtbl_Y = new float[64];

	private float[] fdtbl_UV = new float[64];

	private BitString[] YDC_HT;

	private BitString[] UVDC_HT;

	private BitString[] YAC_HT;

	private BitString[] UVAC_HT;

	private int[] std_dc_luminance_nrcodes = new int[17]
	{
		0, 0, 1, 5, 1, 1, 1, 1, 1, 1,
		0, 0, 0, 0, 0, 0, 0
	};

	private int[] std_dc_luminance_values = new int[12]
	{
		0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
		10, 11
	};

	private int[] std_ac_luminance_nrcodes = new int[17]
	{
		0, 0, 2, 1, 3, 3, 2, 4, 3, 5,
		5, 4, 4, 0, 0, 1, 125
	};

	private int[] std_ac_luminance_values = new int[162]
	{
		1, 2, 3, 0, 4, 17, 5, 18, 33, 49,
		65, 6, 19, 81, 97, 7, 34, 113, 20, 50,
		129, 145, 161, 8, 35, 66, 177, 193, 21, 82,
		209, 240, 36, 51, 98, 114, 130, 9, 10, 22,
		23, 24, 25, 26, 37, 38, 39, 40, 41, 42,
		52, 53, 54, 55, 56, 57, 58, 67, 68, 69,
		70, 71, 72, 73, 74, 83, 84, 85, 86, 87,
		88, 89, 90, 99, 100, 101, 102, 103, 104, 105,
		106, 115, 116, 117, 118, 119, 120, 121, 122, 131,
		132, 133, 134, 135, 136, 137, 138, 146, 147, 148,
		149, 150, 151, 152, 153, 154, 162, 163, 164, 165,
		166, 167, 168, 169, 170, 178, 179, 180, 181, 182,
		183, 184, 185, 186, 194, 195, 196, 197, 198, 199,
		200, 201, 202, 210, 211, 212, 213, 214, 215, 216,
		217, 218, 225, 226, 227, 228, 229, 230, 231, 232,
		233, 234, 241, 242, 243, 244, 245, 246, 247, 248,
		249, 250
	};

	private int[] std_dc_chrominance_nrcodes = new int[17]
	{
		0, 0, 3, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 0, 0, 0, 0, 0
	};

	private int[] std_dc_chrominance_values = new int[12]
	{
		0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
		10, 11
	};

	private int[] std_ac_chrominance_nrcodes = new int[17]
	{
		0, 0, 2, 1, 2, 4, 4, 3, 4, 7,
		5, 4, 4, 0, 1, 2, 119
	};

	private int[] std_ac_chrominance_values = new int[162]
	{
		0, 1, 2, 3, 17, 4, 5, 33, 49, 6,
		18, 65, 81, 7, 97, 113, 19, 34, 50, 129,
		8, 20, 66, 145, 161, 177, 193, 9, 35, 51,
		82, 240, 21, 98, 114, 209, 10, 22, 36, 52,
		225, 37, 241, 23, 24, 25, 26, 38, 39, 40,
		41, 42, 53, 54, 55, 56, 57, 58, 67, 68,
		69, 70, 71, 72, 73, 74, 83, 84, 85, 86,
		87, 88, 89, 90, 99, 100, 101, 102, 103, 104,
		105, 106, 115, 116, 117, 118, 119, 120, 121, 122,
		130, 131, 132, 133, 134, 135, 136, 137, 138, 146,
		147, 148, 149, 150, 151, 152, 153, 154, 162, 163,
		164, 165, 166, 167, 168, 169, 170, 178, 179, 180,
		181, 182, 183, 184, 185, 186, 194, 195, 196, 197,
		198, 199, 200, 201, 202, 210, 211, 212, 213, 214,
		215, 216, 217, 218, 226, 227, 228, 229, 230, 231,
		232, 233, 234, 242, 243, 244, 245, 246, 247, 248,
		249, 250
	};

	private BitString[] bitcode = new BitString[65535];

	private int[] category = new int[65535];

	private int bytenew;

	private int bytepos = 7;

	public ByteArray byteout = new ByteArray();

	private int[] DU = new int[64];

	private float[] YDU = new float[64];

	private float[] UDU = new float[64];

	private float[] VDU = new float[64];

	public bool isDone;

	private BitmapData image;

	private int sf;

	public JPGEncoder(Texture2D texture, float quality)
	{
		image = new BitmapData(texture);
		if (quality <= 0f)
		{
			quality = 1f;
		}
		if (quality > 100f)
		{
			quality = 100f;
		}
		if (quality < 50f)
		{
			sf = (int)(5000f / quality);
		}
		else
		{
			sf = (int)(200f - quality * 2f);
		}
	}

	private void initQuantTables(int sf)
	{
		int[] array = new int[64]
		{
			16, 11, 10, 16, 24, 40, 51, 61, 12, 12,
			14, 19, 26, 58, 60, 55, 14, 13, 16, 24,
			40, 57, 69, 56, 14, 17, 22, 29, 51, 87,
			80, 62, 18, 22, 37, 56, 68, 109, 103, 77,
			24, 35, 55, 64, 81, 104, 113, 92, 49, 64,
			78, 87, 103, 121, 120, 101, 72, 92, 95, 98,
			112, 100, 103, 99
		};
		int i;
		for (i = 0; i < 64; i++)
		{
			float num = Mathf.Floor(((float)(array[i] * sf) + 50f) / 100f);
			if (num < 1f)
			{
				num = 1f;
			}
			else if (num > 255f)
			{
				num = 255f;
			}
			YTable[ZigZag[i]] = (int)num;
		}
		int[] array2 = new int[64]
		{
			17, 18, 24, 47, 99, 99, 99, 99, 18, 21,
			26, 66, 99, 99, 99, 99, 24, 26, 56, 99,
			99, 99, 99, 99, 47, 66, 99, 99, 99, 99,
			99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
			99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
			99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
			99, 99, 99, 99
		};
		for (i = 0; i < 64; i++)
		{
			float num = Mathf.Floor(((float)(array2[i] * sf) + 50f) / 100f);
			if (num < 1f)
			{
				num = 1f;
			}
			else if (num > 255f)
			{
				num = 255f;
			}
			UVTable[ZigZag[i]] = (int)num;
		}
		float[] array3 = new float[8] { 1f, 1.3870399f, 1.306563f, 1.1758755f, 1f, 0.78569496f, 0.5411961f, 0.27589938f };
		i = 0;
		for (int j = 0; j < 8; j++)
		{
			for (int k = 0; k < 8; k++)
			{
				fdtbl_Y[i] = 1f / ((float)YTable[ZigZag[i]] * array3[j] * array3[k] * 8f);
				fdtbl_UV[i] = 1f / ((float)UVTable[ZigZag[i]] * array3[j] * array3[k] * 8f);
				i++;
			}
		}
	}

	private BitString[] computeHuffmanTbl(int[] nrcodes, int[] std_table)
	{
		int num = 0;
		int num2 = 0;
		BitString[] array = new BitString[256];
		for (int i = 1; i <= 16; i++)
		{
			for (int j = 1; j <= nrcodes[i]; j++)
			{
				array[std_table[num2]] = new BitString();
				array[std_table[num2]].val = num;
				array[std_table[num2]].len = i;
				num2++;
				num++;
			}
			num *= 2;
		}
		return array;
	}

	private void initHuffmanTbl()
	{
		YDC_HT = computeHuffmanTbl(std_dc_luminance_nrcodes, std_dc_luminance_values);
		UVDC_HT = computeHuffmanTbl(std_dc_chrominance_nrcodes, std_dc_chrominance_values);
		YAC_HT = computeHuffmanTbl(std_ac_luminance_nrcodes, std_ac_luminance_values);
		UVAC_HT = computeHuffmanTbl(std_ac_chrominance_nrcodes, std_ac_chrominance_values);
	}

	private void initCategoryfloat()
	{
		int num = 1;
		int num2 = 2;
		for (int i = 1; i <= 15; i++)
		{
			for (int j = num; j < num2; j++)
			{
				category[32767 + j] = i;
				BitString bitString = new BitString();
				bitString.len = i;
				bitString.val = j;
				bitcode[32767 + j] = bitString;
			}
			for (int j = -(num2 - 1); j <= -num; j++)
			{
				category[32767 + j] = i;
				BitString bitString = new BitString();
				bitString.len = i;
				bitString.val = num2 - 1 + j;
				bitcode[32767 + j] = bitString;
			}
			num <<= 1;
			num2 <<= 1;
		}
	}

	public byte[] GetBytes()
	{
		if (!isDone)
		{
			Debug.LogError("JPEGEncoder not complete, cannot get bytes!");
			return new byte[1];
		}
		return byteout.GetAllBytes();
	}

	private void writeBits(BitString bs)
	{
		int val = bs.val;
		int num = bs.len - 1;
		while (num >= 0)
		{
			if (((uint)val & Convert.ToUInt32(1 << num)) != 0)
			{
				bytenew |= (int)Convert.ToUInt32(1 << bytepos);
			}
			num--;
			bytepos--;
			if (bytepos < 0)
			{
				if (bytenew == 255)
				{
					writeByte(byte.MaxValue);
					writeByte(0);
				}
				else
				{
					writeByte((byte)bytenew);
				}
				bytepos = 7;
				bytenew = 0;
			}
		}
	}

	private void writeByte(byte value)
	{
		byteout.writeByte(value);
	}

	private void writeWord(int value)
	{
		writeByte((byte)((uint)(value >> 8) & 0xFFu));
		writeByte((byte)((uint)value & 0xFFu));
	}

	private float[] fDCTQuant(float[] data, float[] fdtbl)
	{
		int num = 0;
		for (int i = 0; i < 8; i++)
		{
			float num2 = data[num] + data[num + 7];
			float num3 = data[num] - data[num + 7];
			float num4 = data[num + 1] + data[num + 6];
			float num5 = data[num + 1] - data[num + 6];
			float num6 = data[num + 2] + data[num + 5];
			float num7 = data[num + 2] - data[num + 5];
			float num8 = data[num + 3] + data[num + 4];
			float num9 = data[num + 3] - data[num + 4];
			float num10 = num2 + num8;
			float num11 = num2 - num8;
			float num12 = num4 + num6;
			float num13 = num4 - num6;
			data[num] = num10 + num12;
			data[num + 4] = num10 - num12;
			float num14 = (num13 + num11) * 0.70710677f;
			data[num + 2] = num11 + num14;
			data[num + 6] = num11 - num14;
			num10 = num9 + num7;
			num12 = num7 + num5;
			num13 = num5 + num3;
			float num15 = (num10 - num13) * 0.38268343f;
			float num16 = 0.5411961f * num10 + num15;
			float num17 = 1.306563f * num13 + num15;
			float num18 = num12 * 0.70710677f;
			float num19 = num3 + num18;
			float num20 = num3 - num18;
			data[num + 5] = num20 + num16;
			data[num + 3] = num20 - num16;
			data[num + 1] = num19 + num17;
			data[num + 7] = num19 - num17;
			num += 8;
		}
		num = 0;
		for (int i = 0; i < 8; i++)
		{
			float num2 = data[num] + data[num + 56];
			float num3 = data[num] - data[num + 56];
			float num4 = data[num + 8] + data[num + 48];
			float num5 = data[num + 8] - data[num + 48];
			float num6 = data[num + 16] + data[num + 40];
			float num7 = data[num + 16] - data[num + 40];
			float num8 = data[num + 24] + data[num + 32];
			float num9 = data[num + 24] - data[num + 32];
			float num10 = num2 + num8;
			float num11 = num2 - num8;
			float num12 = num4 + num6;
			float num13 = num4 - num6;
			data[num] = num10 + num12;
			data[num + 32] = num10 - num12;
			float num14 = (num13 + num11) * 0.70710677f;
			data[num + 16] = num11 + num14;
			data[num + 48] = num11 - num14;
			num10 = num9 + num7;
			num12 = num7 + num5;
			num13 = num5 + num3;
			float num15 = (num10 - num13) * 0.38268343f;
			float num16 = 0.5411961f * num10 + num15;
			float num17 = 1.306563f * num13 + num15;
			float num18 = num12 * 0.70710677f;
			float num19 = num3 + num18;
			float num20 = num3 - num18;
			data[num + 40] = num20 + num16;
			data[num + 24] = num20 - num16;
			data[num + 8] = num19 + num17;
			data[num + 56] = num19 - num17;
			num++;
		}
		for (int i = 0; i < 64; i++)
		{
			data[i] = Mathf.Round(data[i] * fdtbl[i]);
		}
		return data;
	}

	private void writeAPP0()
	{
		writeWord(65504);
		writeWord(16);
		writeByte(74);
		writeByte(70);
		writeByte(73);
		writeByte(70);
		writeByte(0);
		writeByte(1);
		writeByte(1);
		writeByte(0);
		writeWord(1);
		writeWord(1);
		writeByte(0);
		writeByte(0);
	}

	private void writeSOF0(int width, int height)
	{
		writeWord(65472);
		writeWord(17);
		writeByte(8);
		writeWord(height);
		writeWord(width);
		writeByte(3);
		writeByte(1);
		writeByte(17);
		writeByte(0);
		writeByte(2);
		writeByte(17);
		writeByte(1);
		writeByte(3);
		writeByte(17);
		writeByte(1);
	}

	private void writeDQT()
	{
		writeWord(65499);
		writeWord(132);
		writeByte(0);
		for (int i = 0; i < 64; i++)
		{
			writeByte((byte)YTable[i]);
		}
		writeByte(1);
		for (int i = 0; i < 64; i++)
		{
			writeByte((byte)UVTable[i]);
		}
	}

	private void writeDHT()
	{
		writeWord(65476);
		writeWord(418);
		writeByte(0);
		for (int i = 0; i < 16; i++)
		{
			writeByte((byte)std_dc_luminance_nrcodes[i + 1]);
		}
		for (int i = 0; i <= 11; i++)
		{
			writeByte((byte)std_dc_luminance_values[i]);
		}
		writeByte(16);
		for (int i = 0; i < 16; i++)
		{
			writeByte((byte)std_ac_luminance_nrcodes[i + 1]);
		}
		for (int i = 0; i <= 161; i++)
		{
			writeByte((byte)std_ac_luminance_values[i]);
		}
		writeByte(1);
		for (int i = 0; i < 16; i++)
		{
			writeByte((byte)std_dc_chrominance_nrcodes[i + 1]);
		}
		for (int i = 0; i <= 11; i++)
		{
			writeByte((byte)std_dc_chrominance_values[i]);
		}
		writeByte(17);
		for (int i = 0; i < 16; i++)
		{
			writeByte((byte)std_ac_chrominance_nrcodes[i + 1]);
		}
		for (int i = 0; i <= 161; i++)
		{
			writeByte((byte)std_ac_chrominance_values[i]);
		}
	}

	private void writeSOS()
	{
		writeWord(65498);
		writeWord(12);
		writeByte(3);
		writeByte(1);
		writeByte(0);
		writeByte(2);
		writeByte(17);
		writeByte(3);
		writeByte(17);
		writeByte(0);
		writeByte(63);
		writeByte(0);
	}

	private float processDU(float[] CDU, float[] fdtbl, float DC, BitString[] HTDC, BitString[] HTAC)
	{
		BitString bs = HTAC[0];
		BitString bs2 = HTAC[240];
		float[] array = fDCTQuant(CDU, fdtbl);
		for (int i = 0; i < 64; i++)
		{
			DU[ZigZag[i]] = (int)array[i];
		}
		int num = (int)((float)DU[0] - DC);
		DC = DU[0];
		if (num == 0)
		{
			writeBits(HTDC[0]);
		}
		else
		{
			writeBits(HTDC[category[32767 + num]]);
			writeBits(bitcode[32767 + num]);
		}
		int num2 = 63;
		while (num2 > 0 && DU[num2] == 0)
		{
			num2--;
		}
		if (num2 == 0)
		{
			writeBits(bs);
			return DC;
		}
		for (int i = 1; i <= num2; i++)
		{
			int num3 = i;
			for (; DU[i] == 0 && i <= num2; i++)
			{
			}
			int num4 = i - num3;
			if (num4 >= 16)
			{
				for (int j = 1; j <= num4 / 16; j++)
				{
					writeBits(bs2);
				}
				num4 &= 0xF;
			}
			writeBits(HTAC[num4 * 16 + category[32767 + DU[i]]]);
			writeBits(bitcode[32767 + DU[i]]);
		}
		if (num2 != 63)
		{
			writeBits(bs);
		}
		return DC;
	}

	private void RGB2YUV(BitmapData img, int xpos, int ypos)
	{
		int num = 0;
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				Color pixelColor = img.getPixelColor(xpos + j, img.height - (ypos + i));
				float num2 = pixelColor.r * 255f;
				float num3 = pixelColor.g * 255f;
				float num4 = pixelColor.b * 255f;
				YDU[num] = 0.299f * num2 + 0.587f * num3 + 0.114f * num4 - 128f;
				UDU[num] = -0.16874f * num2 + -0.33126f * num3 + 0.5f * num4;
				VDU[num] = 0.5f * num2 + -0.41869f * num3 + -0.08131f * num4;
				num++;
			}
		}
	}

	public void doEncoding()
	{
		isDone = false;
		initHuffmanTbl();
		initCategoryfloat();
		initQuantTables(sf);
		encode();
		isDone = true;
		image = null;
	}

	private void encode()
	{
		byteout = new ByteArray();
		bytenew = 0;
		bytepos = 7;
		writeWord(65496);
		writeAPP0();
		writeDQT();
		writeSOF0(image.width, image.height);
		writeDHT();
		writeSOS();
		float dC = 0f;
		float dC2 = 0f;
		float dC3 = 0f;
		bytenew = 0;
		bytepos = 7;
		for (int i = 0; i < image.height; i += 8)
		{
			for (int j = 0; j < image.width; j += 8)
			{
				RGB2YUV(image, j, i);
				dC = processDU(YDU, fdtbl_Y, dC, YDC_HT, YAC_HT);
				dC2 = processDU(UDU, fdtbl_UV, dC2, UVDC_HT, UVAC_HT);
				dC3 = processDU(VDU, fdtbl_UV, dC3, UVDC_HT, UVAC_HT);
			}
		}
		if (bytepos >= 0)
		{
			BitString bitString = new BitString();
			bitString.len = bytepos + 1;
			bitString.val = (1 << bytepos + 1) - 1;
			writeBits(bitString);
		}
		writeWord(65497);
		isDone = true;
	}
}
