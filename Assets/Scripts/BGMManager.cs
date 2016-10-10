using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour {

	private AudioSource audioSource = null;
	public float fadeOutTime = 0;

	// Use this for initialization
	void Start () {
		if (audioSource == null) {
			audioSource = GetComponent<AudioSource>();
		}
	}

	// Update is called once per frame
	void Update() {
		if (fadeOutTime > 0) {
			if (audioSource.volume > 0) {
				float delta = Time.deltaTime;
				audioSource.volume -= (audioSource.volume / fadeOutTime) * delta;
				fadeOutTime -= delta;
			} else {
				if (audioSource.isPlaying) {
					audioSource.Stop();
					fadeOutTime = 0;
					audioSource.volume = 0;
				}
			}
		}
	}

	public void Play(string bgmDirectory) {
		audioSource.clip = Resources.Load<AudioClip>(bgmDirectory);
		audioSource.volume = 1;
		audioSource.panStereo = 0;
		audioSource.pitch = 1;
		audioSource.Play();
	}

	public void Stop(float second) {
		fadeOutTime = second;
	}

	public void setVolume(float vol) {
		audioSource.volume = vol;
	}

	public void setPan(float pan) {
		audioSource.panStereo = pan;
	}

	public void setPitch(float pitch) {
		audioSource.pitch = pitch;
	}
}
