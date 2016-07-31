using UnityEngine;
using System.Collections;

public class AudioSwitcher : MonoBehaviour {

	public AudioManager audioManager;
	public AudioManager.TrackName[] trackNames;

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			audioManager.switchTrack(randomTrack ());
		}
	}

	private AudioManager.TrackName randomTrack() {
		int index = Random.Range (0, (trackNames.Length));
		return trackNames [index];
	}
}
