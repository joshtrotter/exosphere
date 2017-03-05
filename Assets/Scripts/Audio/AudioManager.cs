using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

	public enum TrackName {
		MENU, TUNNEL1, TUNNEL2, OUTDOOR1, OUTDOOR2, INDOOR1, INDOOR2, RESOLUTION, DARKNESS02 
	}

	[System.Serializable]
	public class MusicTrack {
		public TrackName name;
		public AudioClip clip;
		public float volume = 0.5f;
		public float fadeTime = 1f;
	}

	public static AudioManager INSTANCE;

	public MusicTrack[] tracks;
	public TrackName defaultTrack = TrackName.MENU;

	private MusicTrack currentTrack;
	private AudioSource audioSource;
	
	void Awake () {
		if (INSTANCE == null) {
			INSTANCE = this;
			DontDestroyOnLoad (this);
		} else if (INSTANCE != this) {
			Destroy(gameObject);
		}

		audioSource = GetComponent<AudioSource> ();
		currentTrack = forName (defaultTrack);
		playCurrentClip ();
	}

	public void switchTrack(TrackName name) {
		Debug.Log ("Switching to track " + name);
		currentTrack = forName (name);
		sequenceCurrentClip ();
	}

	private void sequenceCurrentClip() {
		if (audioSource.clip != currentTrack.clip) {
			Debug.Log ("Sequencing track " + currentTrack.name);
			DOTween.Sequence ()
				.Append(audioSource.DOFade(0f, currentTrack.fadeTime / 2f))
				.AppendCallback(playCurrentClip)
				.Append(audioSource.DOFade(currentTrack.volume, currentTrack.fadeTime / 2f))
				.Play();
		}
	}

	private void playCurrentClip() {
		audioSource.clip = currentTrack.clip;
		audioSource.Play ();
	}
	
	private MusicTrack forName(TrackName name) {
		foreach (MusicTrack track in tracks) {
			if (track.name == name) {
				return track;
			}
		}
		return forName (defaultTrack);
	}
}
