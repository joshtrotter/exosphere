using UnityEngine;
using System.Collections;

public class SwitchTutorialMessage : SwitchableTutorialMessage {
	
	void OnTriggerEnter(Collider coll)
	{
		if (coll.CompareTag ("Player")) {
			OpenMessage ();
		}
	}
	
	void OnTriggerExit()
	{
		HideMessage ();
	}

	public override void HasBeenSwitched(){
		CloseMessage ();
	}
}
