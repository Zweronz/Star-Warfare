using System.Collections;
using UnityEngine;

public class UIAnimation : UIControl, UIContainer
{
	public class Animation
	{
		public UIFrame[] Frames;

		public int FrameRate;

		public bool Loop;
	}

	public enum Command
	{
		Click = 0
	}

	protected int m_TouchFingerId = -1;

	private Hashtable m_Animations = new Hashtable();

	private Animation m_CurrentAnimation;

	private float m_AnimationTime;

	private int m_currentFrame;

	public UIAnimation(UIManager uiMgr)
	{
		m_Parent = uiMgr;
	}

	public void AddAnimation(string name, UnitUI ui)
	{
		Animation animation = new Animation();
		animation.Loop = false;
		animation.FrameRate = 16;
		animation.Frames = new UIFrame[ui.GetFrameCount(0)];
		for (int i = 0; i < animation.Frames.Length; i++)
		{
			animation.Frames[i] = new UIFrame();
			animation.Frames[i].AddObject(ui, i);
			animation.Frames[i].SetDuration(ui.GetFrameFreq(0, i));
			animation.Frames[i].SetParent(m_Parent);
		}
		m_Animations[name] = animation;
	}

	public void SetAnimationLoop(string name, bool loop)
	{
		if (m_Animations.Contains(name))
		{
			((Animation)m_Animations[name]).Loop = loop;
		}
	}

	public void SetPostion(string name, Rect rct)
	{
		if (m_Animations.Contains(name))
		{
			Animation animation = m_Animations[name] as Animation;
			for (int i = 0; i < animation.Frames.Length; i++)
			{
				animation.Frames[i].Rect = rct;
			}
		}
	}

	public void SetSize(string name, Vector2 size)
	{
		if (m_Animations.Contains(name))
		{
			Animation animation = m_Animations[name] as Animation;
			for (int i = 0; i < animation.Frames.Length; i++)
			{
				animation.Frames[i].SetSize(size);
			}
		}
	}

	public void SetSize(string name, float factor)
	{
		if (m_Animations.Contains(name))
		{
			Animation animation = m_Animations[name] as Animation;
			for (int i = 0; i < animation.Frames.Length; i++)
			{
				Rect objectRect = animation.Frames[i].GetObjectRect();
				animation.Frames[i].SetSize(new Vector2(objectRect.width * factor, objectRect.height * factor));
			}
		}
	}

	public void PlayAnimation(string name)
	{
		if (m_Animations.Contains(name))
		{
			m_CurrentAnimation = (Animation)m_Animations[name];
			m_AnimationTime = 0f;
			m_currentFrame = 0;
		}
	}

	public bool IsOverPlayAnimation()
	{
		if (m_CurrentAnimation != null && m_currentFrame >= m_CurrentAnimation.Frames.Length - 1)
		{
			return true;
		}
		return false;
	}

	public void StopAnimation()
	{
		m_CurrentAnimation = null;
		m_currentFrame = 0;
		m_AnimationTime = 0f;
	}

	public Rect GetRect(string name, int frameIdx)
	{
		return ((Animation)m_Animations[name]).Frames[frameIdx].GetObjectRect();
	}

	public override void Draw()
	{
		if (m_CurrentAnimation == null)
		{
			return;
		}
		int duration = m_CurrentAnimation.Frames[m_currentFrame].GetDuration();
		duration = 1;
		m_AnimationTime += Time.deltaTime;
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
		DrawSpriteSet(m_currentFrame);
	}

	public void DrawSpriteSet(int index)
	{
		m_CurrentAnimation.Frames[index].Rect = base.Rect;
		m_CurrentAnimation.Frames[index].Draw();
	}

	public void DrawSprite(UISpriteX sprite)
	{
		m_Parent.DrawSprite(sprite);
	}

	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
		m_Parent.SendEvent(control, command, wparam, lparam);
	}

	public void Add(UIControl control)
	{
	}

	public override bool HandleInput(UITouchInner touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			m_TouchFingerId = touch.fingerId;
		}
		else if (touch.phase == TouchPhase.Ended && touch.fingerId == m_TouchFingerId && PtInRect(touch.position))
		{
			m_Parent.SendEvent(this, 0, 0f, 0f);
			m_TouchFingerId = -1;
			return true;
		}
		return false;
	}
}
