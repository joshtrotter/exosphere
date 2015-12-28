using UnityEngine;
using System.Collections;

public class ColliderSwitch : Switch {

	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag("Player")) {
			target.Activate ();
			SwapState();
		}
	}

	void OnTriggerExit (Collider coll) {
		if (coll.CompareTag("Player")) {
			target.Activate ();
			SwapState();
		}
	}
}
