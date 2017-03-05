using UnityEngine;
using System.Collections;

public class AudioSwitcher : MonoBehaviour {
	
	public AudioManager.TrackName[] trackNames;

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			AudioManager.INSTANCE.switchTrack(randomTrack ());
		}
	}

	private AudioManager.TrackName randomTrack() {
		int index = Random.Range (0, (trackNames.Length));
		return trackNames [index];
	}
}
