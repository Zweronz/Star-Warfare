using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public abstract class WWWRequest
{
	[CompilerGenerated]
	private sealed class _003CSend_003Ec__Iterator12 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal WWW _003Cwww_003E__0;

		internal DateTime _003CstartTime_003E__1;

		internal bool _003Ctimeout_003E__2;

		internal TimeSpan _003Ctime_003E__3;

		internal byte[] _003CindexBytes_003E__4;

		internal BytesBuffer _003Cbb_003E__5;

		internal Exception _003Cex_003E__6;

		internal int _0024PC;

		internal object _0024current;

		internal WWWRequest _003C_003Ef__this;

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
				_003Cwww_003E__0 = new WWW(_003C_003Ef__this.GetUrl());
				_003CstartTime_003E__1 = DateTime.Now;
				_003Ctimeout_003E__2 = false;
				goto case 1u;
			case 1u:
				if (!_003Cwww_003E__0.isDone)
				{
					_003Ctime_003E__3 = DateTime.Now - _003CstartTime_003E__1;
					if (!(_003Ctime_003E__3.TotalSeconds > 15.0))
					{
						_0024current = new WaitForSeconds(0.5f);
						_0024PC = 1;
						return true;
					}
					_003Ctimeout_003E__2 = true;
				}
				try
				{
					if (_003Ctimeout_003E__2)
					{
						Debug.Log("WWW time out: " + _003C_003Ef__this);
						_003C_003Ef__this.OnTimeOut();
					}
					else if (_003Cwww_003E__0 == null)
					{
						Debug.Log("WWW is null: " + _003C_003Ef__this);
						_003C_003Ef__this.OnResponseError();
					}
					else if (_003Cwww_003E__0.error != null)
					{
						Debug.Log("www.error: " + _003Cwww_003E__0.error + ";  " + _003C_003Ef__this);
						_003C_003Ef__this.OnResponseError();
					}
					else if (_003Cwww_003E__0.bytes != null && _003Cwww_003E__0.bytes.Length > 0)
					{
						_003CindexBytes_003E__4 = _003Cwww_003E__0.bytes;
						_003Cbb_003E__5 = new BytesBuffer(_003CindexBytes_003E__4);
						_003C_003Ef__this.ReadData(_003Cbb_003E__5);
						GameApp.GetInstance().GetWWWManager().getReceivedPacketCache()
							.AddPacket(_003C_003Ef__this);
					}
					else
					{
						Debug.Log("www.bytes is null or www.bytes.Length <= 0: " + _003C_003Ef__this);
						_003C_003Ef__this.OnResponseError();
					}
				}
				catch (Exception ex)
				{
					_003Cex_003E__6 = ex;
					Debug.Log(string.Concat(_003C_003Ef__this, "\n", _003Cex_003E__6.Message));
					_003C_003Ef__this.OnResponseError();
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
			if (_003Cwww_003E__0 != null)
			{
				_003Cwww_003E__0 = null;
			}
		}
	}

	protected const float MAX_WAITING_TIME = 15f;

	protected const string MD5_KEY = "T83x4Pj03o";

	protected int mReceiveErrorCode;

	protected string mSign = string.Empty;

	protected bool mHasVerified;

	public WWWBehaviorScript Behavior { get; set; }

	public IEnumerator Send()
	{
		WWW www = new WWW(GetUrl());
		DateTime startTime = DateTime.Now;
		bool timeout = false;
		while (!www.isDone)
		{
			if ((DateTime.Now - startTime).TotalSeconds > 15.0)
			{
				timeout = true;
				break;
			}
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			if (timeout)
			{
				Debug.Log("WWW time out: " + this);
				OnTimeOut();
			}
			else if (www == null)
			{
				Debug.Log("WWW is null: " + this);
				OnResponseError();
			}
			else if (www.error != null)
			{
				Debug.Log("www.error: " + www.error + ";  " + this);
				OnResponseError();
			}
			else if (www.bytes != null && www.bytes.Length > 0)
			{
				byte[] indexBytes = www.bytes;
				BytesBuffer bb = new BytesBuffer(indexBytes);
				ReadData(bb);
				GameApp.GetInstance().GetWWWManager().getReceivedPacketCache()
					.AddPacket(this);
			}
			else
			{
				Debug.Log("www.bytes is null or www.bytes.Length <= 0: " + this);
				OnResponseError();
			}
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Debug.Log(string.Concat(this, "\n", ex.Message));
			OnResponseError();
		}
		finally
		{
			((_003CSend_003Ec__Iterator12)(object)this)._003C_003E__Finally0();
		}
	}

	protected virtual BytesBuffer GetBody()
	{
		return null;
	}

	protected string ToBase64String(string text)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		return Convert.ToBase64String(bytes);
	}

	protected string Hash(string toHash)
	{
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		string s = toHash + "T83x4Pj03o";
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		bytes = mD5CryptoServiceProvider.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = bytes;
		foreach (byte b in array)
		{
			stringBuilder.AppendFormat("{0:x2}", b);
		}
		return stringBuilder.ToString();
	}

	protected virtual string GetUrl()
	{
		return string.Empty;
	}

	protected virtual void ReadData(BytesBuffer bb)
	{
	}

	public virtual void ProcessLogic()
	{
	}

	public virtual void OnTimeOut()
	{
	}

	public virtual void OnResponseError()
	{
	}

	public virtual void OnVerifyResponseError()
	{
	}

	public virtual void OnVerifyFail(int errorCode)
	{
	}
}
