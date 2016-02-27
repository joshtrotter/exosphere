using UnityEngine;
using System.Collections;

public class TriggerSwitch : Switch {

	public bool activateOnExit = false;
	public bool activateOnceOnly = false;
	private bool hasBeenActivated;

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player")){
			if (!hasBeenActivated){
				SwapState();
			}
		}
	}

	public override void SwapState(){
		if (activateOnceOnly) {
			hasBeenActivated = true;
		}
		base.SwapState ();
	}

	void OnTriggerExit(Collider coll){
		if (activateOnExit && coll.CompareTag("Player")){
			SwapState();
		}
	}
	
}
