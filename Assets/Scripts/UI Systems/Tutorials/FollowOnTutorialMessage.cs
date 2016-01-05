using UnityEngine;
using System.Collections;

public class FollowOnTutorialMessage : SwitchableTutorialMessage {

	public override void HasBeenSwitched(){
		OpenMessage ();
	}

	void OnTriggerExit(Collider coll){
		if (coll.CompareTag("Player")){
			CloseMessage ();
		}
	}
}
