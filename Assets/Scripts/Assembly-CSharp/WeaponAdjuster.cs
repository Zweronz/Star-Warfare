public class WeaponAdjuster
{
	public float angleOffsetH = 10f;

	public float angleOffsetV;

	public float angleScaleV = 0.7f;

	public float pivotOffset = 0.5f;

	public WeaponAdjuster(float aoh, float aov, float asv, float po)
	{
		angleOffsetH = aoh;
		angleOffsetV = aov;
		angleScaleV = asv;
		pivotOffset = po;
	}
}
