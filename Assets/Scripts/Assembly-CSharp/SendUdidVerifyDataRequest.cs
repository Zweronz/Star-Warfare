using System.Collections.Generic;
using UnityEngine;

public class SendUdidVerifyDataRequest : Request
{
	private List<VerifyData> mDataList;

	public SendUdidVerifyDataRequest(List<VerifyData> dataList)
	{
		mDataList = dataList;
	}

	public override byte[] GetBytes()
	{
		int num = 1 + BytesBuffer.GetStringByteLength(GameApp.GetInstance().MacAddress) + 11 * mDataList.Count;
		BytesBuffer bytesBuffer = new BytesBuffer(num + 2);
		Debug.Log("bufferLength: " + num);
		bytesBuffer.AddByte(161);
		bytesBuffer.AddByte((byte)num);
		bytesBuffer.AddString(GameApp.GetInstance().MacAddress);
		bytesBuffer.AddByte((byte)mDataList.Count);
		foreach (VerifyData mData in mDataList)
		{
			bytesBuffer.AddShort(mData.ID);
			bytesBuffer.AddByte(mData.Type);
			bytesBuffer.AddInt((int)(mData.RealValue * 100f));
			bytesBuffer.AddInt((int)(mData.VerifyValue * 100f));
		}
		return bytesBuffer.GetBytes();
	}
}
