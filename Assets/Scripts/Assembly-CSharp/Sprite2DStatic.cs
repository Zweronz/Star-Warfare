using UnityEngine;

public class Sprite2DStatic : Sprite2D
{
	public struct Frame
	{
		public Material Material;

		public int MaterialId;

		public Rect TextureRect;

		public Vector2 Size;

		public int Duration;
	}

	public Frame ImageFrame
	{
		set
		{
			base.MaterialID = value.MaterialId;
			base.TextureRect = value.TextureRect;
			base.Size = value.Size;
		}
	}
}
