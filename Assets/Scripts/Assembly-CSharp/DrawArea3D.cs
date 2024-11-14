using UnityEngine;

public class DrawArea3D : DrawArea
{
	public Matrix4x4 matrix;

	public DrawArea3D(Vector3 min, Vector3 max, Matrix4x4 matrix)
		: base(min, max)
	{
		this.matrix = matrix;
	}

	public override Vector3 Point(Vector3 p)
	{
		return matrix.MultiplyPoint3x4(Vector3.Scale(new Vector3((p.x - canvasMin.x) / (canvasMax.x - canvasMin.x), (p.y - canvasMin.y) / (canvasMax.y - canvasMin.y), p.z), max - min) + min);
	}
}
