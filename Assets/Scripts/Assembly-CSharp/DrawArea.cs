using UnityEngine;

public class DrawArea
{
	public Vector3 min;

	public Vector3 max;

	public Vector3 canvasMin = new Vector3(0f, 0f, 0f);

	public Vector3 canvasMax = new Vector3(1f, 1f, 1f);

	public DrawArea(Vector3 min, Vector3 max)
	{
		this.min = min;
		this.max = max;
	}

	public virtual Vector3 Point(Vector3 p)
	{
		return Camera.main.ScreenToWorldPoint(Vector3.Scale(new Vector3((p.x - canvasMin.x) / (canvasMax.x - canvasMin.x), (p.y - canvasMin.y) / (canvasMax.y - canvasMin.y), 0f), max - min) + min + Vector3.forward * Camera.main.nearClipPlane * 1.1f);
	}

	public void DrawLine(Vector3 a, Vector3 b, Color c)
	{
		GL.Color(c);
		GL.Vertex(Point(a));
		GL.Vertex(Point(b));
	}

	public void DrawRay(Vector3 start, Vector3 dir, Color c)
	{
		DrawLine(start, start + dir, c);
	}

	public void DrawRect(Vector3 a, Vector3 b, Color c)
	{
		GL.Color(c);
		GL.Vertex(Point(new Vector3(a.x, a.y, 0f)));
		GL.Vertex(Point(new Vector3(a.x, b.y, 0f)));
		GL.Vertex(Point(new Vector3(b.x, b.y, 0f)));
		GL.Vertex(Point(new Vector3(b.x, a.y, 0f)));
	}

	public void DrawDiamond(Vector3 a, Vector3 b, Color c)
	{
		GL.Color(c);
		GL.Vertex(Point(new Vector3(a.x, (a.y + b.y) / 2f, 0f)));
		GL.Vertex(Point(new Vector3((a.x + b.x) / 2f, b.y, 0f)));
		GL.Vertex(Point(new Vector3(b.x, (a.y + b.y) / 2f, 0f)));
		GL.Vertex(Point(new Vector3((a.x + b.x) / 2f, a.y, 0f)));
	}

	public void DrawRect(Vector3 corner, Vector3 dirA, Vector3 dirB, Color c)
	{
		GL.Color(c);
		Vector3[] array = new Vector3[2] { dirA, dirB };
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				Vector3 vector = corner + array[(j + 1) % 2] * i;
				GL.Vertex(Point(vector));
				GL.Vertex(Point(vector + array[j]));
			}
		}
	}

	public void DrawCube(Vector3 corner, Vector3 dirA, Vector3 dirB, Vector3 dirC, Color c)
	{
		GL.Color(c);
		Vector3[] array = new Vector3[3] { dirA, dirB, dirC };
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					Vector3 vector = corner + array[(k + 1) % 3] * i + array[(k + 2) % 3] * j;
					GL.Vertex(Point(vector));
					GL.Vertex(Point(vector + array[k]));
				}
			}
		}
	}
}
