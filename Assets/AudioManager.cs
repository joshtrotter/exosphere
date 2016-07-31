﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

	public enum TrackName {
		MENU, TUNNEL1, TUNNEL2, OUTDOOR1, INDOOR1 
	}

	[System.Serializable]
	public class MusicTrack {
		public TrackName name;
		public AudioClip clip;
		public float volume = 0.5f;
		public float fadeTime = 1f;
	}

	public MusicTrack[] tracks;
	public TrackName defaultTrack = TrackName.MENU;

	private MusicTrack currentTrack;
	private AudioSource audioSource;
	
	void Awake () {
		audioSource = GetComponent<AudioSource> ();
		switchTrack (defaultTrack);
	}

	public void switchTrack(TrackName name) {
		currentTrack = forName (name);
		playCurrentClip ();
	}

	private void playCurrentClip() {
		if (audioSource.clip != currentTrack.clip) {
			Sequence sequence = DOTween.Sequence ()
				.Append(audioSource.DOFade(0f, currentTrack.fadeTime / 2))
				.Append(audioSource.DOFade(currentTrack.volume, currentTrack.fadeTime / 2));
			audioSource.clip = currentTrack.clip;
			audioSource.PlayDelayed (currentTrack.fadeTime);
		}
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
