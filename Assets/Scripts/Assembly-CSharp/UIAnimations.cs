using System.Collections;
using UnityEngine;

internal class UIAnimations
{
	public class ControlData
	{
		public int control_id;

		public Vector2 pos;

		public float angle;

		public ControlData()
		{
			control_id = -1;
			pos = new Vector2(0f, 0f);
			angle = 0f;
		}
	}

	public enum state
	{
		none = 0,
		doing = 1,
		wait_end = 2,
		end = 3
	}

	public int animation_id;

	public ArrayList control_data;

	private state translate_state;

	private float translate_duringtime;

	private Vector2 translate_current;

	private bool translate_exchange;

	public bool translate_have;

	public Vector2 translate_start;

	public float translate_time;

	public Vector2 translate_offset;

	public bool translate_restore;

	public bool translate_loop;

	public bool translate_reverse;

	private state rotate_state;

	private float rotate_duringtime;

	private float rotate_current;

	private bool rotate_exchange;

	public bool rotate_have;

	public float rotate_start;

	public float rotate_time;

	public float rotate_angle;

	public bool rotate_restore;

	public bool rotate_loop;

	public bool rotate_reverse;

	public state alpha_state;

	public float alpha_duringtime;

	public float alpha_deltafactor;

	public bool alpha_have;

	public float alpha_time;

	public float alpha_factor;

	public bool alpha_restore;

	public bool alpha_loop;

	public UIAnimations()
	{
		animation_id = 0;
		control_data = new ArrayList();
		translate_have = false;
		translate_start = new Vector2(0f, 0f);
		translate_time = 0f;
		translate_offset = new Vector2(0f, 0f);
		translate_restore = false;
		translate_loop = false;
		translate_reverse = false;
		rotate_have = false;
		rotate_start = 0f;
		rotate_time = 0f;
		rotate_angle = 0f;
		rotate_restore = false;
		rotate_loop = false;
		rotate_reverse = false;
		alpha_have = false;
		alpha_time = 0f;
		alpha_factor = 1f;
		alpha_restore = false;
		alpha_loop = false;
	}

	public void Reset()
	{
		translate_state = state.none;
		translate_duringtime = 0f;
		translate_current = new Vector2(0f, 0f);
		translate_exchange = false;
		rotate_state = state.none;
		rotate_duringtime = 0f;
		rotate_current = 0f;
		rotate_exchange = false;
		alpha_state = state.none;
		alpha_duringtime = 0f;
		alpha_deltafactor = 1f;
	}

	public bool IsRuning()
	{
		return translate_state != 0 || rotate_state != 0 || state.none != alpha_state;
	}

	public void Start()
	{
		if (translate_have)
		{
			translate_state = state.doing;
		}
		if (rotate_have)
		{
			rotate_state = state.doing;
		}
	}

	public void Stop()
	{
		translate_state = state.none;
	}

	public bool IsTranslating()
	{
		return state.none != translate_state;
	}

	public Vector2 GetTranslate()
	{
		switch (translate_state)
		{
		case state.wait_end:
			translate_state = state.end;
			break;
		case state.end:
			if (translate_loop)
			{
				translate_state = state.doing;
				if (translate_reverse)
				{
					translate_offset = -translate_offset;
					translate_start = translate_current;
				}
			}
			else if (translate_restore)
			{
				if (translate_reverse)
				{
					translate_offset = -translate_offset;
					translate_start = translate_current;
					if (!translate_exchange)
					{
						translate_exchange = true;
						translate_state = state.doing;
					}
					else
					{
						translate_state = state.none;
					}
				}
				else
				{
					translate_state = state.none;
					translate_current = translate_start;
				}
			}
			else
			{
				translate_state = state.none;
			}
			translate_duringtime = 0f;
			break;
		}
		return translate_current;
	}

	public bool IsRotating()
	{
		return state.none != rotate_state;
	}

	public float GetRotate()
	{
		switch (rotate_state)
		{
		case state.wait_end:
			rotate_state = state.end;
			break;
		case state.end:
			if (rotate_loop)
			{
				rotate_state = state.doing;
				if (rotate_reverse)
				{
					rotate_angle = 0f - rotate_angle;
					rotate_start = rotate_current;
				}
			}
			else if (rotate_restore)
			{
				if (rotate_reverse)
				{
					rotate_angle = 0f - rotate_angle;
					rotate_start = rotate_current;
					if (!rotate_exchange)
					{
						rotate_exchange = true;
						rotate_state = state.doing;
					}
					else
					{
						rotate_state = state.none;
					}
				}
				else
				{
					rotate_state = state.none;
					rotate_current = rotate_start;
				}
			}
			else
			{
				rotate_state = state.none;
			}
			rotate_duringtime = 0f;
			break;
		}
		return rotate_current;
	}

	public void Update(float delta_time)
	{
		if (translate_state == state.doing)
		{
			translate_duringtime += delta_time;
			if (translate_duringtime > translate_time)
			{
				translate_duringtime = translate_time;
				translate_state = state.wait_end;
			}
			translate_current = translate_duringtime / translate_time * translate_offset + translate_start;
		}
		if (rotate_state == state.doing)
		{
			rotate_duringtime += delta_time;
			if (rotate_duringtime > rotate_time)
			{
				rotate_duringtime = rotate_time;
				rotate_state = state.wait_end;
			}
			rotate_current = rotate_duringtime / rotate_time * rotate_angle + rotate_start;
		}
	}
}
