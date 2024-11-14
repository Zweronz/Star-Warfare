using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public bool bMuteSound = true;

	public bool bMuteMusic = true;

	public float soundVolume = 1f;

	public float musicVolume = 1f;

	public void Awake()
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		bMuteSound = !GameApp.GetInstance().GetUserState().GetPlaySound();
		bMuteMusic = !GameApp.GetInstance().GetUserState().GetPlayMusic();
		soundVolume = GameApp.GetInstance().GetUserState().GetSoundVolume();
		musicVolume = GameApp.GetInstance().GetUserState().GetMusicVolume();
	}

	public static AudioManager GetInstance()
	{
		return Camera.mainCamera.GetComponent<AudioManager>();
	}

	public AudioClip LoadMusic(string name)
	{
		return Resources.Load(name, typeof(AudioClip)) as AudioClip;
	}

	public AudioClip LoadSound(string name)
	{
		return Resources.Load(name, typeof(AudioClip)) as AudioClip;
	}

	public void PlayMusic(string name)
	{
		AudioClip audioClip = LoadMusic(name);
		GameObject gameObject = new GameObject("AudioMusic::" + audioClip.name);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = base.transform.position;
		AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioSource.clip = audioClip;
		audioSource.loop = true;
		audioSource.volume = musicVolume;
		audioSource.playOnAwake = false;
		audioSource.mute = bMuteMusic;
		audioSource.Play();
	}

	public void PlayMusic(string name, float factor)
	{
		AudioClip audioClip = LoadMusic(name);
		GameObject gameObject = new GameObject("AudioMusic::" + audioClip.name);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = base.transform.position;
		AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioSource.clip = audioClip;
		audioSource.loop = true;
		audioSource.volume = musicVolume * factor;
		audioSource.playOnAwake = false;
		audioSource.mute = bMuteMusic;
		audioSource.Play();
	}

	public void PlaySoundSingle(string name)
	{
		AudioClip audioClip = LoadMusic(name);
		foreach (Transform item in base.transform)
		{
			if (item.name.Equals("AudioSound::" + audioClip.name))
			{
				return;
			}
		}
		PlaySound(name);
	}

	public void PlaySoundSingle(string name, string prefix)
	{
		AudioClip audioClip = LoadMusic(name);
		foreach (Transform item in base.transform)
		{
			if (item.name.Equals("AudioSound::" + audioClip.name + prefix))
			{
				return;
			}
		}
		PlaySound(name, prefix);
	}

	public void PlaySoundSingleLoop(string name)
	{
		AudioClip audioClip = LoadMusic(name);
		foreach (Transform item in base.transform)
		{
			if (item.name.Equals("AudioSound::" + audioClip.name))
			{
				return;
			}
		}
		PlaySoundLoop(name);
	}

	public bool IsPlaying(string name)
	{
		foreach (Transform item in base.transform)
		{
			if (item.name.Equals("AudioSound::" + name))
			{
				return true;
			}
		}
		return false;
	}

	public void PlaySoundSingleAt(string name, Vector3 pos)
	{
		AudioClip audioClip = LoadMusic(name);
		foreach (Transform item in base.transform)
		{
			if (item.name.Equals("AudioSound::" + audioClip.name))
			{
				return;
			}
		}
		PlaySoundAt(name, pos);
	}

	public void PlaySoundSingleAt(string name, Vector3 pos, string prefix)
	{
		AudioClip audioClip = LoadMusic(name);
		foreach (Transform item in base.transform)
		{
			if (item.name.Equals("AudioSound::" + audioClip.name + prefix))
			{
				return;
			}
		}
		PlaySoundAt(name, pos, prefix);
	}

	public void PlaySoundAt(string name, Vector3 pos)
	{
		AudioClip audioClip = LoadSound(name);
		GameObject gameObject = new GameObject("AudioSound::" + audioClip.name);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = pos;
		AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioSource.clip = audioClip;
		audioSource.volume = soundVolume;
		audioSource.Play();
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.playOnAwake = false;
		audioSource.mute = bMuteSound;
		audioSource.maxDistance = 100f;
		Object.Destroy(gameObject, audioClip.length);
	}

	public void PlaySoundAt(string name, Vector3 pos, string prefix)
	{
		AudioClip audioClip = LoadSound(name);
		GameObject gameObject = new GameObject("AudioSound::" + audioClip.name + prefix);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = pos;
		AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioSource.clip = audioClip;
		audioSource.volume = soundVolume;
		audioSource.Play();
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.playOnAwake = false;
		audioSource.mute = bMuteSound;
		audioSource.maxDistance = 100f;
		Object.Destroy(gameObject, audioClip.length);
	}

	public void PlaySound(string name)
	{
		AudioClip audioClip = LoadSound(name);
		GameObject gameObject = new GameObject("AudioSound::" + audioClip.name);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = base.transform.position;
		AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioSource.clip = audioClip;
		audioSource.volume = soundVolume;
		audioSource.Play();
		audioSource.playOnAwake = false;
		audioSource.mute = bMuteSound;
		Object.Destroy(gameObject, audioClip.length);
	}

	public void PlaySound(string name, string prefix)
	{
		AudioClip audioClip = LoadSound(name);
		GameObject gameObject = new GameObject("AudioSound::" + audioClip.name + prefix);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = base.transform.position;
		AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioSource.clip = audioClip;
		audioSource.volume = soundVolume;
		audioSource.Play();
		audioSource.playOnAwake = false;
		audioSource.mute = bMuteSound;
		Object.Destroy(gameObject, audioClip.length);
	}

	public void PlaySoundLoop(string name)
	{
		AudioClip audioClip = LoadSound(name);
		GameObject gameObject = new GameObject("AudioSound::" + audioClip.name);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = base.transform.position;
		AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioSource.clip = audioClip;
		audioSource.volume = soundVolume;
		audioSource.loop = true;
		audioSource.Play();
		audioSource.playOnAwake = false;
		audioSource.mute = bMuteSound;
		Object.Destroy(gameObject, audioClip.length);
	}

	public void StopMusic()
	{
		ArrayList arrayList = new ArrayList();
		foreach (Transform item in base.transform)
		{
			if (item.name.StartsWith("AudioMusic::"))
			{
				arrayList.Add(item.gameObject);
			}
		}
		int count = arrayList.Count;
		for (int i = 0; i < count; i++)
		{
			Object.Destroy((GameObject)arrayList[i]);
		}
	}

	public void StopSound()
	{
		ArrayList arrayList = new ArrayList();
		foreach (Transform item in base.transform)
		{
			if (item.name.StartsWith("AudioSound::"))
			{
				arrayList.Add(item.gameObject);
			}
		}
		int count = arrayList.Count;
		for (int i = 0; i < count; i++)
		{
			Object.Destroy((GameObject)arrayList[i]);
		}
	}

	public void StopSound(string name)
	{
		ArrayList arrayList = new ArrayList();
		foreach (Transform item in base.transform)
		{
			if (item.name.StartsWith("AudioSound::") && item.name.Equals("AudioSound::" + name))
			{
				arrayList.Add(item.gameObject);
			}
		}
		int count = arrayList.Count;
		for (int i = 0; i < count; i++)
		{
			Object.Destroy((GameObject)arrayList[i]);
		}
	}

	public void StopSound(string name, string prefix)
	{
		ArrayList arrayList = new ArrayList();
		foreach (Transform item in base.transform)
		{
			if (item.name.StartsWith("AudioSound::") && item.name.Equals("AudioSound::" + name + prefix))
			{
				arrayList.Add(item.gameObject);
			}
		}
		int count = arrayList.Count;
		for (int i = 0; i < count; i++)
		{
			Object.Destroy((GameObject)arrayList[i]);
		}
	}

	public void SetSoundMute(bool bMute)
	{
		bMuteSound = bMute;
		ArrayList arrayList = new ArrayList();
		foreach (Transform item in base.transform)
		{
			if (item.name.StartsWith("AudioSound::"))
			{
				arrayList.Add(item.gameObject);
			}
		}
		int count = arrayList.Count;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = (GameObject)arrayList[i];
			AudioSource audioSource = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
			audioSource.mute = bMute;
		}
	}

	public void SetMusicMute(bool bMute)
	{
		bMuteMusic = bMute;
		ArrayList arrayList = new ArrayList();
		foreach (Transform item in base.transform)
		{
			if (item.name.StartsWith("AudioMusic::"))
			{
				arrayList.Add(item.gameObject);
			}
		}
		GameObject gameObject = GameObject.Find("MenuMusic");
		if (gameObject != null)
		{
			AudioSource audioSource = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
			audioSource.mute = bMute;
		}
		int count = arrayList.Count;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject2 = (GameObject)arrayList[i];
			AudioSource audioSource2 = gameObject2.GetComponent(typeof(AudioSource)) as AudioSource;
			audioSource2.mute = bMute;
		}
	}

	public void SetSoundVolume(float volume)
	{
		soundVolume = volume;
		ArrayList arrayList = new ArrayList();
		foreach (Transform item in base.transform)
		{
			if (item.name.StartsWith("AudioSound::"))
			{
				arrayList.Add(item.gameObject);
			}
		}
		int count = arrayList.Count;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = (GameObject)arrayList[i];
			AudioSource audioSource = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
			audioSource.volume = volume;
		}
	}

	public void SetMusicVolume(float volume)
	{
		musicVolume = volume;
		ArrayList arrayList = new ArrayList();
		foreach (Transform item in base.transform)
		{
			if (item.name.StartsWith("AudioMusic::"))
			{
				arrayList.Add(item.gameObject);
			}
		}
		GameObject gameObject = GameObject.Find("MenuMusic");
		if (gameObject != null)
		{
			AudioSource audioSource = gameObject.GetComponent(typeof(AudioSource)) as AudioSource;
			audioSource.volume = volume;
		}
		int count = arrayList.Count;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject2 = (GameObject)arrayList[i];
			AudioSource audioSource2 = gameObject2.GetComponent(typeof(AudioSource)) as AudioSource;
			audioSource2.volume = volume;
		}
	}
}
