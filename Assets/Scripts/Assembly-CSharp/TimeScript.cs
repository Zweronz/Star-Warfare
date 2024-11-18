using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class TimeScript : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CGetLocalNotification_003Ec__Iterator13 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal string _003Curl_003E__0;

		internal WWW _003Cwww_003E__1;

		internal DateTime _003CstartTime_003E__2;

		internal bool _003Ctimeout_003E__3;

		internal TimeSpan _003Ctime_003E__4;

		internal byte[] _003CindexBytes_003E__5;

		internal BytesBuffer _003Cbb_003E__6;

		internal int _003Cid_003E__7;

		internal UserState _003CuserState_003E__8;

		internal string _003CnotiStartDate_003E__9;

		internal string _003CnotiEndDate_003E__10;

		internal short _003CintervalHour_003E__11;

		internal string _003CnotiMsg_003E__12;

		internal DateTime _003CstartDate_003E__13;

		internal DateTime _003CendDate_003E__14;

		internal DateTime _003Cnow_003E__15;

		internal TimeSpan _003Ct_003E__16;

		internal long _003Cn_003E__17;

		internal long _003Cs_003E__18;

		internal long _003Ci_003E__19;

		internal DateTime _003Cdate_003E__20;

		internal Exception _003Cex_003E__21;

		internal int _0024PC;

		internal object _0024current;

		internal TimeScript _003C_003Ef__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _0024current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _0024current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)_0024PC;
			_0024PC = -1;
			switch (num)
			{
			case 0u:
				_003Curl_003E__0 = "http://sw1.freyrgames.com:9001/PromotionServer/NotificationServlet";
				_003Cwww_003E__1 = new WWW(_003Curl_003E__0, _003C_003Ef__this.GetVersion());
				_003CstartTime_003E__2 = DateTime.Now;
				_003Ctimeout_003E__3 = false;
				goto case 1u;
			case 1u:
				if (!_003Cwww_003E__1.isDone)
				{
					Debug.Log("www.isDone: " + _003Cwww_003E__1.isDone);
					_003Ctime_003E__4 = DateTime.Now - _003CstartTime_003E__2;
					if (!(_003Ctime_003E__4.TotalSeconds > 15.0))
					{
						_0024current = new WaitForSeconds(0.5f);
						_0024PC = 1;
						return true;
					}
					_003Ctimeout_003E__3 = true;
				}
				try
				{
					Debug.Log("www.error: " + _003Cwww_003E__1.error);
					if (!_003Ctimeout_003E__3 && _003Cwww_003E__1 != null && _003Cwww_003E__1.error == null && _003Cwww_003E__1.bytes != null && _003Cwww_003E__1.bytes.Length > 0)
					{
						_003CindexBytes_003E__5 = _003Cwww_003E__1.bytes;
						_003Cbb_003E__6 = new BytesBuffer(_003CindexBytes_003E__5);
						_003Cid_003E__7 = _003Cbb_003E__6.ReadInt();
						_003CuserState_003E__8 = GameApp.GetInstance().GetUserState();
						if (_003CuserState_003E__8 != null && _003Cid_003E__7 != _003CuserState_003E__8.m_promotion.id)
						{
							_003CuserState_003E__8.m_promotion.id = _003Cid_003E__7;
							_003CnotiStartDate_003E__9 = _003Cbb_003E__6.ReadStringShortLength();
							_003CnotiEndDate_003E__10 = _003Cbb_003E__6.ReadStringShortLength();
							_003CintervalHour_003E__11 = _003Cbb_003E__6.ReadShort();
							_003CnotiMsg_003E__12 = _003Cbb_003E__6.ReadStringShortLength();
							_003CstartDate_003E__13 = DateTime.Parse(_003CnotiStartDate_003E__9);
							_003CendDate_003E__14 = DateTime.Parse(_003CnotiEndDate_003E__10);
							_003Cnow_003E__15 = DateTime.Now;
							if (_003Cnow_003E__15 < _003CstartDate_003E__13)
							{
								_003Cnow_003E__15 = _003CstartDate_003E__13;
							}
							if (_003Cnow_003E__15 < _003CendDate_003E__14)
							{
								_003Ct_003E__16 = _003CendDate_003E__14 - _003CstartDate_003E__13;
								_003Cn_003E__17 = (long)_003Ct_003E__16.TotalMinutes;
								_003Cs_003E__18 = (long)(_003Cnow_003E__15 - _003CstartDate_003E__13).TotalMinutes;
								for (_003Ci_003E__19 = 0L; _003Ci_003E__19 < _003Cn_003E__17; _003Ci_003E__19 += _003CintervalHour_003E__11 * 60)
								{
									if (_003Ci_003E__19 >= _003Cs_003E__18)
									{
										_003Cdate_003E__20 = _003CstartDate_003E__13.AddMinutes(_003Ci_003E__19);
										Debug.Log("sendNotification: " + _003Cdate_003E__20);
										_003C_003Ef__this.SendLocalNotification(_003Cdate_003E__20, _003CnotiMsg_003E__12);
									}
								}
							}
							GameApp.GetInstance().Save();
						}
					}
				}
				catch (Exception ex)
				{
					_003Cex_003E__21 = ex;
					Debug.Log(_003Cex_003E__21.Message);
				}
				finally
				{
					_003C_003E__Finally0();
				}
				_0024PC = -1;
				break;
			}
			return false;
		}

		[DebuggerHidden]
		public void Dispose()
		{
			_0024PC = -1;
		}

		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		public void _003C_003E__Finally0()
		{
			if (_003Cwww_003E__1 != null)
			{
				_003Cwww_003E__1 = null;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetSalesOff_003Ec__Iterator14 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal string _003Curl_003E__0;

		internal WWW _003Cwww_003E__1;

		internal DateTime _003CstartTime_003E__2;

		internal bool _003Ctimeout_003E__3;

		internal TimeSpan _003Ctime_003E__4;

		internal byte[] _003CindexBytes_003E__5;

		internal BytesBuffer _003Cbb_003E__6;

		internal UserState _003CuserState_003E__7;

		internal byte _003Clen_003E__8;

		internal byte _003Clen1_003E__9;

		internal int _003Ci_003E__10;

		internal int _003Cid_003E__11;

		internal int _003CsalesOff_003E__12;

		internal Exception _003Cex_003E__13;

		internal int _0024PC;

		internal object _0024current;

		internal TimeScript _003C_003Ef__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _0024current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _0024current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)_0024PC;
			_0024PC = -1;
			switch (num)
			{
			case 0u:
				_003Curl_003E__0 = "http://sw1.freyrgames.com:9001/PromotionServer/SalesServlet";
				_003Cwww_003E__1 = new WWW(_003Curl_003E__0, _003C_003Ef__this.GetVersion());
				_003CstartTime_003E__2 = DateTime.Now;
				_003Ctimeout_003E__3 = false;
				goto case 1u;
			case 1u:
				if (!_003Cwww_003E__1.isDone)
				{
					Debug.Log("www.isDone: " + _003Cwww_003E__1.isDone);
					_003Ctime_003E__4 = DateTime.Now - _003CstartTime_003E__2;
					if (!(_003Ctime_003E__4.TotalSeconds > 15.0))
					{
						_0024current = new WaitForSeconds(0.5f);
						_0024PC = 1;
						return true;
					}
					_003Ctimeout_003E__3 = true;
				}
				try
				{
					Debug.Log("www.error: " + _003Cwww_003E__1.error);
					if (!_003Ctimeout_003E__3 && _003Cwww_003E__1 != null && _003Cwww_003E__1.error == null && _003Cwww_003E__1.bytes != null && _003Cwww_003E__1.bytes.Length > 0)
					{
						_003CindexBytes_003E__5 = _003Cwww_003E__1.bytes;
						_003Cbb_003E__6 = new BytesBuffer(_003CindexBytes_003E__5);
						_003CuserState_003E__7 = GameApp.GetInstance().GetUserState();
						if (_003CuserState_003E__7 != null)
						{
							_003CuserState_003E__7.m_promotion.m_salesOff.Clear();
							_003CuserState_003E__7.m_promotion.m_msg = _003Cbb_003E__6.ReadStringShortLength();
							_003Clen_003E__8 = _003Cbb_003E__6.ReadByte();
							_003Clen1_003E__9 = _003Cbb_003E__6.ReadByte();
							for (_003Ci_003E__10 = 0; _003Ci_003E__10 < _003Clen_003E__8; _003Ci_003E__10++)
							{
								_003Cid_003E__11 = _003Cbb_003E__6.ReadByte();
								_003CsalesOff_003E__12 = _003Cbb_003E__6.ReadByte();
								_003CuserState_003E__7.m_promotion.m_salesOff.Add(_003Cid_003E__11, (float)_003CsalesOff_003E__12 / 100f);
								Debug.Log("id / sales :" + _003Cid_003E__11 + " /" + (float)_003CsalesOff_003E__12 / 100f);
							}
							_003CuserState_003E__7.showPromotion = true;
							Debug.Log("userState.showPromotion: " + _003CuserState_003E__7.showPromotion);
						}
					}
				}
				catch (Exception ex)
				{
					_003Cex_003E__13 = ex;
					Debug.Log(_003Cex_003E__13.Message);
				}
				finally
				{
					_003C_003E__Finally0();
				}
				_0024PC = -1;
				break;
			}
			return false;
		}

		[DebuggerHidden]
		public void Dispose()
		{
			_0024PC = -1;
		}

		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		public void _003C_003E__Finally0()
		{
			if (_003Cwww_003E__1 != null)
			{
				_003Cwww_003E__1 = null;
			}
		}
	}

	private int opentime;

	private bool repeat = true;

	private bool isFirstLaunch = true;

	private int gametime;

	private int status;

	private int usemithrils;

	private UserState userState;

	private int rank_id;

	protected int levelcount;

	private void Start()
	{
		userState = GameApp.GetInstance().GetUserState();
		levelcount = Application.loadedLevel;
	}

	private void Update()
	{
		if (!GameApp.GetInstance().isGameStart())
		{
			return;
		}
		if (isFirstLaunch)
		{
			rank_id = userState.GetRank().rankID;
			isFirstLaunch = false;
		}
		status = userState.GetDiscountStatus();
		if (status == 0 && repeat)
		{
			InvokeRepeating("Launch", 0f, 1f);
			repeat = false;
		}
		gametime = userState.GetGameTime();
		status = userState.GetDiscountStatus();
		usemithrils = userState.GetUseMithril();
		if (status != 0)
		{
			return;
		}
		if (gametime > UIConstant.GetRankNeedTime(rank_id) && usemithrils < UIConstant.GetRankNeedMithril(rank_id))
		{
			if (userState.GetDiscountWeapon() == -1)
			{
				int rankID = userState.GetRank().rankID;
				List<Weapon> rankWeapon = UIConstant.GetRankWeapon(rankID);
				List<Weapon> list = new List<Weapon>();
				for (int i = 0; i < rankWeapon.Count; i++)
				{
					if (rankWeapon[i].Level == 0 || rankWeapon[i].Level == 15)
					{
						list.Add(rankWeapon[i]);
					}
				}
				if (list.Count == 0)
				{
					ClearDisCount();
				}
				else
				{
					CancelInvoke();
					userState.SetDiscountStatus(1);
					userState.SetShowNotify(1);
					userState.SetDiscountTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm"));
					int index = Random.Range(0, list.Count - 1);
					int gunID = list[index].GunID;
					userState.SetDiscountWeapon(gunID);
				}
			}
			GameApp.GetInstance().Save();
		}
		else if (gametime > UIConstant.GetRankNeedTime(rank_id))
		{
			ClearDisCount();
			GameApp.GetInstance().Save();
		}
	}

	private void Launch()
	{
		userState.AddGameTime(1);
	}

	public void SetGooglePlayService()
	{
		GameApp.isGoogleServiceReady = false;
	}

	public void ShowSponsorPay()
	{
	}

	public void ClearDisCount()
	{
		userState.SetDiscountStatus(0);
		userState.SetDiscountTime("0000-00-00 00:00");
		userState.SetShowNotify(0);
		userState.SetDiscountWeapon(-1);
		userState.SetGameTime(0);
		userState.SetUseMithril(0);
		Debug.Log("clear");
	}

	public void EnterGame()
	{
		Debug.Log("EnterGame");
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.Kindle:
			GameUIManager.GetInstance().LoadBountyHunter();
			break;
		case AndroidConstant.Version.GooglePlay:
			GameUIManager.GetInstance().LoadCallOfArena();
			break;
		}
	}

	public void ShowMovie()
	{
		GameApp.GetInstance().GetUserState().showmovie = true;
	}

	public void ShowAds()
	{
	}

	public void GetPromotion()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		if (userState != null && userState.m_promotion != null)
		{
			userState.m_promotion.m_salesOff.Clear();
		}
		if (Application.loadedLevelName == "StartMenu" || Application.loadedLevelName == "MultiMenu" || Application.loadedLevelName == "ShopAndCustomize" || Application.loadedLevelName == "SoloMenu")
		{
			StartCoroutine(GetLocalNotification());
			StartCoroutine(GetSalesOff());
		}
	}

	private byte[] GetVersion()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(255);
		bytesBuffer.AddInt(int.Parse(GameApp.GetInstance().GetUserState().version));
		return bytesBuffer.GetBytes();
	}

	private IEnumerator GetLocalNotification()
	{
		string url = "http://sw1.freyrgames.com:9001/PromotionServer/NotificationServlet";
		WWW www = new WWW(url, GetVersion());
		DateTime startTime = DateTime.Now;
		bool timeout = false;
		while (!www.isDone)
		{
			Debug.Log("www.isDone: " + www.isDone);
			if ((DateTime.Now - startTime).TotalSeconds > 15.0)
			{
				timeout = true;
				break;
			}
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			Debug.Log("www.error: " + www.error);
			if (timeout || www == null || www.error != null || www.bytes == null || www.bytes.Length <= 0)
			{
				yield break;
			}
			byte[] indexBytes = www.bytes;
			BytesBuffer bb = new BytesBuffer(indexBytes);
			int id = bb.ReadInt();
			UserState userState = GameApp.GetInstance().GetUserState();
			if (userState == null || id == userState.m_promotion.id)
			{
				yield break;
			}
			userState.m_promotion.id = id;
			string notiStartDate = bb.ReadStringShortLength();
			string notiEndDate = bb.ReadStringShortLength();
			short intervalHour = bb.ReadShort();
			string notiMsg = bb.ReadStringShortLength();
			DateTime startDate = DateTime.Parse(notiStartDate);
			DateTime endDate = DateTime.Parse(notiEndDate);
			DateTime now = DateTime.Now;
			if (now < startDate)
			{
				now = startDate;
			}
			if (now < endDate)
			{
				long j = (long)(endDate - startDate).TotalMinutes;
				long s = (long)(now - startDate).TotalMinutes;
				for (long i = 0L; i < j; i += intervalHour * 60)
				{
					if (i >= s)
					{
						DateTime date = startDate.AddMinutes(i);
						Debug.Log("sendNotification: " + date);
						SendLocalNotification(date, notiMsg);
					}
				}
			}
			GameApp.GetInstance().Save();
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Debug.Log(ex.Message);
		}
		finally
		{
			((_003CGetLocalNotification_003Ec__Iterator13)(object)this)._003C_003E__Finally0();
		}
	}

	private IEnumerator GetSalesOff()
	{
		string url = "http://sw1.freyrgames.com:9001/PromotionServer/SalesServlet";
		WWW www = new WWW(url, GetVersion());
		DateTime startTime = DateTime.Now;
		bool timeout = false;
		while (!www.isDone)
		{
			Debug.Log("www.isDone: " + www.isDone);
			if ((DateTime.Now - startTime).TotalSeconds > 15.0)
			{
				timeout = true;
				break;
			}
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			Debug.Log("www.error: " + www.error);
			if (timeout || www == null || www.error != null || www.bytes == null || www.bytes.Length <= 0)
			{
				yield break;
			}
			byte[] indexBytes = www.bytes;
			BytesBuffer bb = new BytesBuffer(indexBytes);
			UserState userState = GameApp.GetInstance().GetUserState();
			if (userState != null)
			{
				userState.m_promotion.m_salesOff.Clear();
				userState.m_promotion.m_msg = bb.ReadStringShortLength();
				byte len = bb.ReadByte();
				byte len2 = bb.ReadByte();
				for (int i = 0; i < len; i++)
				{
					int id = bb.ReadByte();
					int salesOff = bb.ReadByte();
					userState.m_promotion.m_salesOff.Add(id, (float)salesOff / 100f);
					Debug.Log("id / sales :" + id + " /" + (float)salesOff / 100f);
				}
				userState.showPromotion = true;
				Debug.Log("userState.showPromotion: " + userState.showPromotion);
			}
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Debug.Log(ex.Message);
		}
		finally
		{
			((_003CGetSalesOff_003Ec__Iterator14)(object)this)._003C_003E__Finally0();
		}
	}

	private void SendLocalNotification(DateTime date, string msg)
	{
	}
}
