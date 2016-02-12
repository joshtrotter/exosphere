using UnityEngine;
using System.Collections;

public class TriggerSwitch : Switch {

	public bool activateOnExit = false;
	public bool activateOnceOnly = false;
	private bool hasBeenActivated;

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player")){
			if (!hasBeenActivated){
				target.Activate();
			}
			if (activateOnceOnly){
				hasBeenActivated = true;
			}
		}
	}

	void OnTriggerExit(Collider coll){
		if (activateOnExit && coll.CompareTag("Player")){
			target.Activate();
		}
	}
}
