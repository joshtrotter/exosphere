using UnityEngine;
using System.Collections;

public class AudioOverrider : MonoBehaviour {

	public AudioManager.TrackName[] trackNames;
	
	void Start() {
		AudioManager.INSTANCE.switchTrack(randomTrack ());
	}
	
	private AudioManager.TrackName randomTrack() {
		int index = Random.Range (0, (trackNames.Length));
		return trackNames [index];
	}
}
