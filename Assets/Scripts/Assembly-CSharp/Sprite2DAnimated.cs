using System.Collections;

public class Sprite2DAnimated : Sprite2DStatic
{
	public class Animation
	{
		public Frame[] Frames;

		public int FrameRate;

		public bool Loop;
	}

	private Hashtable m_Animations = new Hashtable();

	private Animation m_CurrentAnimation;

	private float m_AnimationTime;

	private int m_currentFrame;

	public override void Update(float delta_time)
	{
		base.Update(delta_time);
		UpdateAnimation(delta_time);
	}

	public Animation CreateAnimation(UnitUI ui)
	{
		Animation animation = new Animation();
		animation.Loop = true;
		animation.FrameRate = 16;
		animation.Frames = new Frame[ui.GetFrameCount(0)];
		for (int i = 0; i < ui.GetFrameCount(0); i++)
		{
			animation.Frames[i] = default(Frame);
			animation.Frames[i].MaterialId = ui.GetMaterialIndex(0, i, 0);
			animation.Frames[i].TextureRect = ui.GetModuleRect(0, i, 0);
			animation.Frames[i].Size = ui.GetModuleSize(0, i, 0);
			animation.Frames[i].Duration = ui.GetFrameFreq(0, i);
		}
		return animation;
	}

	public void AddAnimation(string name, Animation animation)
	{
		m_Animations[name] = animation;
	}

	public void PlayAnimation(string name)
	{
		if (m_Animations.Contains(name))
		{
			m_CurrentAnimation = (Animation)m_Animations[name];
			m_AnimationTime = 0f;
			m_currentFrame = 0;
			UpdateAnimation(0f);
		}
	}

	public void StopAnimation()
	{
		m_CurrentAnimation = null;
		m_currentFrame = 0;
		m_AnimationTime = 0f;
	}

	private void UpdateAnimation(float delta_time)
	{
		if (m_CurrentAnimation == null)
		{
			return;
		}
		int duration = m_CurrentAnimation.Frames[m_currentFrame].Duration;
		duration = 1;
		m_AnimationTime += delta_time;
		m_currentFrame = (int)(m_AnimationTime * (float)m_CurrentAnimation.FrameRate / (float)duration);
		if (m_currentFrame >= m_CurrentAnimation.Frames.Length)
		{
			if (m_CurrentAnimation.Loop)
			{
				m_currentFrame %= m_CurrentAnimation.Frames.Length;
			}
			else
			{
				m_currentFrame = m_CurrentAnimation.Frames.Length - 1;
			}
		}
		base.ImageFrame = m_CurrentAnimation.Frames[m_currentFrame];
	}
}
