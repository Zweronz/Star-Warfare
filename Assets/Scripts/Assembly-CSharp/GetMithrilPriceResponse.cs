public class GetMithrilPriceResponse : Response
{
	private float[] iapPrice;

	private float[] iapOrigionalPrice;

	private float[] iapOffPrice;

	private bool m_bOver;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		byte b = bytesBuffer.ReadByte();
		iapPrice = new float[b];
		for (int i = 0; i < b; i++)
		{
			iapPrice[i] = (float)bytesBuffer.ReadInt() / 100f;
		}
		iapOrigionalPrice = new float[b];
		for (int j = 0; j < b; j++)
		{
			iapOrigionalPrice[j] = (float)bytesBuffer.ReadInt() / 100f;
		}
		m_bOver = true;
	}

	public override void ProcessLogic()
	{
		if (m_bOver)
		{
			for (int i = 0; i < UIConstant.IAP_PRICE.Length; i++)
			{
				UIConstant.IAP_PRICE[i] = iapPrice[i];
			}
			for (int j = 0; j < UIConstant.IAP_ORIGIONAL_PRICE.Length; j++)
			{
				UIConstant.IAP_ORIGIONAL_PRICE[j] = iapOrigionalPrice[j].ToString();
			}
			for (int k = 0; k < UIConstant.IAP_SAVING_PRICE.Length; k++)
			{
				UIConstant.IAP_SAVING_PRICE[k] = string.Format("{0:N2}", iapOrigionalPrice[k] - iapPrice[k]);
			}
			UIConstant.IAP_SAVING_PRICE[0] = string.Empty;
			UIConstant.IAP_SAVING_PRICE[1] = string.Empty;
			UIConstant.IAP_SAVING_PRICE[2] = string.Empty;
			IAPUI.m_bUpdate = true;
		}
	}
}
