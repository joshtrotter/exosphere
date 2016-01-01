using UnityEngine;
using System.Collections;

public class ColliderSwitch : MonoBehaviour {

	public SwitchableObject target;

	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag("Player")) {
			target.Activate();
		}
	}

	void OnTriggerExit (Collider coll) {
		if (coll.CompareTag("Player")) {
			target.Activate();
		}
	}
}
