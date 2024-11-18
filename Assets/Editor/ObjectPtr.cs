using System;
using Object = UnityEngine.Object;

public static class ObjectPtr
{
	public static IntPtr GetPointer(this Object obj)
	{
		return (IntPtr)typeof(Object).GetMethod("GetCachedPtr", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(obj, null);
	}
}
