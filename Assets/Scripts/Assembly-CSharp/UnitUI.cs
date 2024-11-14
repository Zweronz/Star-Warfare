using System.IO;
using UnityEngine;

public class UnitUI
{
	private const byte MODULE_TYPE_TOTAL = 4;

	public const byte MODULE_TYPE_IMAGE = 0;

	public const byte MODULE_TYPE_RECT = 1;

	public const byte MODULE_TYPE_TEXT = 2;

	public const byte MODULE_TYPE_LINE = 3;

	public Anim vAnim;

	public Frame vFrame;

	public IModule[] vModule;

	private short[] sDibInfo;

	public bool Load(BinaryReader br)
	{
		bool flag = false;
		sDibInfo = (short[])Res2DManager.LoadData(br, 0, 2);
		LoadModule(br);
		LoadFrame(br);
		LoadAnim(br);
		return true;
	}

	private void LoadModule(BinaryReader br)
	{
		if (vModule == null)
		{
			vModule = new IModule[4];
			vModule[0] = new MImage();
			vModule[0].Load(br);
			vModule[1] = new MRect();
			vModule[1].Load(br);
			vModule[2] = new MText();
			vModule[2].Load(br);
			vModule[3] = new MLine();
			vModule[3].Load(br);
		}
	}

	private void LoadFrame(BinaryReader br)
	{
		if (vFrame == null)
		{
			vFrame = new Frame();
			vFrame.Load(br);
		}
	}

	private void LoadAnim(BinaryReader br)
	{
		if (vAnim == null)
		{
			vAnim = new Anim();
			vAnim.Load(br);
		}
	}

	public short[] GetDibInfo()
	{
		return sDibInfo;
	}

	public void Free()
	{
		vAnim.Free();
		vFrame.Free();
		vModule[0].Free();
		vModule[1].Free();
		vModule[2].Free();
		vModule[3].Free();
		sDibInfo = null;
	}

	public int GetFrameCount(int aIndex)
	{
		return vAnim.GetLength(aIndex);
	}

	public int GetModuleCount(int aIndex, int fIndex)
	{
		int fIndex2 = fIndex % vAnim.GetLength(aIndex);
		int index = vAnim.GetIndex(aIndex, fIndex2);
		return vFrame.GetCount(index);
	}

	public int GetModuleRotation(int aIndex, int fIndex, int mIndex)
	{
		int fIndex2 = fIndex % vAnim.GetLength(aIndex);
		int index = vAnim.GetIndex(aIndex, fIndex2);
		int index2 = vFrame.GetIndex(index, mIndex);
		int type = vFrame.GetType(index, mIndex);
		int result = 0;
		if (type == 0)
		{
			MImage mImage = vModule[0] as MImage;
			result = mImage.GetRotate(index2);
		}
		return result;
	}

	public Rect GetModuleRect(int aIndex, int fIndex, int mIndex)
	{
		int fIndex2 = fIndex % vAnim.GetLength(aIndex);
		int index = vAnim.GetIndex(aIndex, fIndex2);
		int index2 = vFrame.GetIndex(index, mIndex);
		if (vFrame.GetType(index, mIndex) == 0)
		{
			MImage mImage = vModule[0] as MImage;
			return mImage.GetRect(index2);
		}
		return new Rect(0f, 0f, 0f, 0f);
	}

	public Vector2 GetModuleSize(int aIndex, int fIndex, int mIndex)
	{
		int fIndex2 = fIndex % vAnim.GetLength(aIndex);
		int index = vAnim.GetIndex(aIndex, fIndex2);
		int index2 = vFrame.GetIndex(index, mIndex);
		if (vFrame.GetType(index, mIndex) == 0)
		{
			MImage mImage = vModule[0] as MImage;
			return mImage.GetSize(index2);
		}
		return new Vector2(0f, 0f);
	}

	public int GetMaterialIndex(int aIndex, int fIndex, int mIndex)
	{
		int fIndex2 = fIndex % vAnim.GetLength(aIndex);
		int index = vAnim.GetIndex(aIndex, fIndex2);
		int index2 = vFrame.GetIndex(index, mIndex);
		if (vFrame.GetType(index, mIndex) == 0)
		{
			MImage mImage = vModule[0] as MImage;
			return mImage.GetMaterial(index2);
		}
		return 0;
	}

	public Vector2 GetModulePosition(int aIndex, int fIndex, int mIndex)
	{
		int fIndex2 = fIndex % vAnim.GetLength(aIndex);
		Vector2 position = vAnim.GetPosition(aIndex, fIndex2);
		int index = vAnim.GetIndex(aIndex, fIndex2);
		Vector2 position2 = vFrame.GetPosition(index, mIndex);
		int index2 = vFrame.GetIndex(index, mIndex);
		int type = vFrame.GetType(index, mIndex);
		Vector2 vector = Vector2.zero;
		if (type == 0)
		{
			MImage mImage = vModule[0] as MImage;
			vector = mImage.GetSize(index2);
		}
		return new Vector2(position.x + position2.x + UIConstant.ScreenLocalWidth * 0.5f + vector.x * 0.5f, UIConstant.ScreenLocalHeight * 0.5f - (position.y + position2.y) + vector.y * 0.5f);
	}

	public Rect GetModulePositionRect(int aIndex, int fIndex, int mIndex)
	{
		int fIndex2 = fIndex % vAnim.GetLength(aIndex);
		Vector2 position = vAnim.GetPosition(aIndex, fIndex2);
		int index = vAnim.GetIndex(aIndex, fIndex2);
		Vector2 position2 = vFrame.GetPosition(index, mIndex);
		int index2 = vFrame.GetIndex(index, mIndex);
		int type = vFrame.GetType(index, mIndex);
		Vector2 vector = Vector2.zero;
		switch (type)
		{
		case 0:
		{
			MImage mImage = vModule[0] as MImage;
			vector = mImage.GetSize(index2);
			break;
		}
		case 1:
		{
			MRect mRect = vModule[1] as MRect;
			vector = mRect.GetSize(index2);
			break;
		}
		}
		return new Rect(position.x + position2.x + UIConstant.ScreenLocalWidth * 0.5f, UIConstant.ScreenLocalHeight * 0.5f - (position.y + position2.y), vector.x, vector.y);
	}

	public Vector2 GetFrameSize(int aIndex, int fIndex)
	{
		int fIndex2 = fIndex % vAnim.GetLength(aIndex);
		int index = vAnim.GetIndex(aIndex, fIndex2);
		return vFrame.GetSize(index);
	}

	public int GetFrameFreq(int aIndex, int fIndex)
	{
		int fIndex2 = fIndex % vAnim.GetLength(aIndex);
		return vAnim.GetFreq(aIndex, fIndex2);
	}

	public Vector2 GetFramePosition(int aIndex, int fIndex)
	{
		int fIndex2 = fIndex % vAnim.GetLength(aIndex);
		Vector2 position = vAnim.GetPosition(aIndex, fIndex2);
		return new Vector2(position.x + UIConstant.ScreenLocalWidth * 0.5f, UIConstant.ScreenLocalHeight * 0.5f - position.y);
	}
}
