using System.Collections.Generic;
using UnityEngine;

public class SendVerifyDataRequest : Request
{
	private List<VerifyData> mDataList;

	public SendVerifyDataRequest(List<VerifyData> dataList)
	{
		mDataList = dataList;
	}

	public override byte[] GetBytes()
	{
		int num = 1 + 11 * mDataList.Count;
		BytesBuffer bytesBuffer = new BytesBuffer(num + 2);
		Debug.Log("bufferLength: " + num);
		bytesBuffer.AddByte(156);
		bytesBuffer.AddByte((byte)num);
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
