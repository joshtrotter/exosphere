using UnityEngine;
using System.Collections;

public class TriggerSwitch : Switch {

	public bool activateOnExit = false;

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player")){
			target.Activate();
		}
	}

	void OnTriggerExit(Collider coll){
		if (activateOnExit && coll.CompareTag("Player")){
			target.Activate();
		}
	}
}
