using UnityEngine;
using System.Collections;

public class Collectable : HasLevelState {

	private const int NOT_COLLECTED_STATE = 0;
	private const int COLLECTED_STATE = 1;

	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag("Player")) {
			RegisterStateChange(COLLECTED_STATE);
			GetLevelManager().RemoveCollectable();
			Collect ();
		}
	}

	public override void ReloadState(int state) {
		if (state == COLLECTED_STATE) {
			Collect ();
		}
	}

	private void Collect() {
		gameObject.SetActive(false);
	}
}
