using UnityEngine;

public class Util
{
	public static bool IsSaneNumber(float f)
	{
		if (float.IsNaN(f))
		{
			return false;
		}
		if (f == float.PositiveInfinity)
		{
			return false;
		}
		if (f == float.NegativeInfinity)
		{
			return false;
		}
		if (f > 1E+12f)
		{
			return false;
		}
		if (f < -1E+12f)
		{
			return false;
		}
		return true;
	}

	public static Vector3 Clamp(Vector3 v, float length)
	{
		float magnitude = v.magnitude;
		if (magnitude > length)
		{
			return v / magnitude * length;
		}
		return v;
	}

	public static float Mod(float x, float period)
	{
		float num = x % period;
		return (!(num >= 0f)) ? (num + period) : num;
	}

	public static int Mod(int x, int period)
	{
		int num = x % period;
		return (num < 0) ? (num + period) : num;
	}

	public static float Mod(float x)
	{
		return Mod(x, 1f);
	}

	public static int Mod(int x)
	{
		return Mod(x, 1);
	}

	public static float CyclicDiff(float high, float low, float period, bool skipWrap)
	{
		if (!skipWrap)
		{
			high = Mod(high, period);
			low = Mod(low, period);
		}
		return (!(high >= low)) ? (high + period - low) : (high - low);
	}

	public static int CyclicDiff(int high, int low, int period, bool skipWrap)
	{
		if (!skipWrap)
		{
			high = Mod(high, period);
			low = Mod(low, period);
		}
		return (high < low) ? (high + period - low) : (high - low);
	}

	public static float CyclicDiff(float high, float low, float period)
	{
		return CyclicDiff(high, low, period, false);
	}

	public static int CyclicDiff(int high, int low, int period)
	{
		return CyclicDiff(high, low, period, false);
	}

	public static float CyclicDiff(float high, float low)
	{
		return CyclicDiff(high, low, 1f, false);
	}

	public static int CyclicDiff(int high, int low)
	{
		return CyclicDiff(high, low, 1, false);
	}

	public static bool CyclicIsLower(float compared, float comparedTo, float reference, float period)
	{
		compared = Mod(compared, period);
		comparedTo = Mod(comparedTo, period);
		if (CyclicDiff(compared, reference, period, true) < CyclicDiff(comparedTo, reference, period, true))
		{
			return true;
		}
		return false;
	}

	public static bool CyclicIsLower(int compared, int comparedTo, int reference, int period)
	{
		compared = Mod(compared, period);
		comparedTo = Mod(comparedTo, period);
		if (CyclicDiff(compared, reference, period, true) < CyclicDiff(comparedTo, reference, period, true))
		{
			return true;
		}
		return false;
	}

	public static bool CyclicIsLower(float compared, float comparedTo, float reference)
	{
		return CyclicIsLower(compared, comparedTo, reference, 1f);
	}

	public static bool CyclicIsLower(int compared, int comparedTo, int reference)
	{
		return CyclicIsLower(compared, comparedTo, reference, 1);
	}

	public static float CyclicLerp(float a, float b, float t, float period)
	{
		if (Mathf.Abs(b - a) <= period / 2f)
		{
			return a * (1f - t) + b * t;
		}
		if (b < a)
		{
			a -= period;
		}
		else
		{
			b -= period;
		}
		return Mod(a * (1f - t) + b * t);
	}

	public static Vector3 ProjectOntoPlane(Vector3 v, Vector3 normal)
	{
		return v - Vector3.Project(v, normal);
	}

	public static Vector3 SetHeight(Vector3 originalVector, Vector3 referenceHeightVector, Vector3 upVector)
	{
		Vector3 vector = ProjectOntoPlane(originalVector, upVector);
		Vector3 vector2 = Vector3.Project(referenceHeightVector, upVector);
		return vector + vector2;
	}

	public static Vector3 GetHighest(Vector3 a, Vector3 b, Vector3 upVector)
	{
		if (Vector3.Dot(a, upVector) >= Vector3.Dot(b, upVector))
		{
			return a;
		}
		return b;
	}

	public static Vector3 GetLowest(Vector3 a, Vector3 b, Vector3 upVector)
	{
		if (Vector3.Dot(a, upVector) <= Vector3.Dot(b, upVector))
		{
			return a;
		}
		return b;
	}

	public static Matrix4x4 RelativeMatrix(Transform t, Transform relativeTo)
	{
		return relativeTo.worldToLocalMatrix * t.localToWorldMatrix;
	}

	public static Vector3 TransformVector(Matrix4x4 m, Vector3 v)
	{
		return m.MultiplyPoint(v) - m.MultiplyPoint(Vector3.zero);
	}

	public static Vector3 TransformVector(Transform t, Vector3 v)
	{
		return TransformVector(t.localToWorldMatrix, v);
	}

	public static void TransformFromMatrix(Matrix4x4 matrix, Transform trans)
	{
		trans.rotation = QuaternionFromMatrix(matrix);
		trans.position = matrix.GetColumn(3);
	}

	public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
	{
		Quaternion result = default(Quaternion);
		result.w = Mathf.Sqrt(Mathf.Max(0f, 1f + m[0, 0] + m[1, 1] + m[2, 2])) / 2f;
		result.x = Mathf.Sqrt(Mathf.Max(0f, 1f + m[0, 0] - m[1, 1] - m[2, 2])) / 2f;
		result.y = Mathf.Sqrt(Mathf.Max(0f, 1f - m[0, 0] + m[1, 1] - m[2, 2])) / 2f;
		result.z = Mathf.Sqrt(Mathf.Max(0f, 1f - m[0, 0] - m[1, 1] + m[2, 2])) / 2f;
		result.x *= Mathf.Sign(result.x * (m[2, 1] - m[1, 2]));
		result.y *= Mathf.Sign(result.y * (m[0, 2] - m[2, 0]));
		result.z *= Mathf.Sign(result.z * (m[1, 0] - m[0, 1]));
		return result;
	}

	public static Matrix4x4 MatrixFromQuaternion(Quaternion q)
	{
		return CreateMatrix(q * Vector3.right, q * Vector3.up, q * Vector3.forward, Vector3.zero);
	}

	public static Matrix4x4 MatrixFromQuaternionPosition(Quaternion q, Vector3 p)
	{
		Matrix4x4 result = MatrixFromQuaternion(q);
		result.SetColumn(3, p);
		result[3, 3] = 1f;
		return result;
	}

	public static Matrix4x4 MatrixSlerp(Matrix4x4 a, Matrix4x4 b, float t)
	{
		t = Mathf.Clamp01(t);
		Matrix4x4 result = MatrixFromQuaternion(Quaternion.Slerp(QuaternionFromMatrix(a), QuaternionFromMatrix(b), t));
		result.SetColumn(3, a.GetColumn(3) * (1f - t) + b.GetColumn(3) * t);
		result[3, 3] = 1f;
		return result;
	}

	public static Matrix4x4 CreateMatrix(Vector3 right, Vector3 up, Vector3 forward, Vector3 position)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.SetColumn(0, right);
		identity.SetColumn(1, up);
		identity.SetColumn(2, forward);
		identity.SetColumn(3, position);
		identity[3, 3] = 1f;
		return identity;
	}

	public static Matrix4x4 CreateMatrixPosition(Vector3 position)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.SetColumn(3, position);
		identity[3, 3] = 1f;
		return identity;
	}

	public static void TranslateMatrix(ref Matrix4x4 m, Vector3 position)
	{
		m.SetColumn(3, (Vector3)m.GetColumn(3) + position);
		m[3, 3] = 1f;
	}

	public static Vector3 ConstantSlerp(Vector3 from, Vector3 to, float angle)
	{
		float t = Mathf.Min(1f, angle / Vector3.Angle(from, to));
		return Vector3.Slerp(from, to, t);
	}

	public static Quaternion ConstantSlerp(Quaternion from, Quaternion to, float angle)
	{
		float t = Mathf.Min(1f, angle / Quaternion.Angle(from, to));
		return Quaternion.Slerp(from, to, t);
	}

	public static Vector3 ConstantLerp(Vector3 from, Vector3 to, float length)
	{
		return from + Clamp(to - from, length);
	}

	public static Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
	{
		Vector3 from = Vector3.Lerp(a, b, t);
		Vector3 vector = Vector3.Lerp(b, c, t);
		Vector3 to = Vector3.Lerp(c, d, t);
		Vector3 from2 = Vector3.Lerp(from, vector, t);
		Vector3 to2 = Vector3.Lerp(vector, to, t);
		return Vector3.Lerp(from2, to2, t);
	}

	public static float[] GetLineSphereIntersections(Vector3 lineStart, Vector3 lineDir, Vector3 sphereCenter, float sphereRadius)
	{
		float sqrMagnitude = lineDir.sqrMagnitude;
		float num = 2f * (Vector3.Dot(lineStart, lineDir) - Vector3.Dot(lineDir, sphereCenter));
		float num2 = lineStart.sqrMagnitude + sphereCenter.sqrMagnitude - 2f * Vector3.Dot(lineStart, sphereCenter) - sphereRadius * sphereRadius;
		float num3 = num * num - 4f * sqrMagnitude * num2;
		if (num3 < 0f)
		{
			return null;
		}
		float num4 = (0f - num - Mathf.Sqrt(num3)) / (2f * sqrMagnitude);
		float num5 = (0f - num + Mathf.Sqrt(num3)) / (2f * sqrMagnitude);
		if (!(num4 < num5))
		{
			return new float[2] { num5, num4 };
		}
		return new float[2] { num4, num5 };
	}
}
