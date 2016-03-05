using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TutorialExternalTrigger : MonoBehaviour {

	public TutorialMessage.TutorialSwitchTuple[] tutorials;
	public bool exitTrigger = false;

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player") && !exitTrigger){
			doTrigger ();
		}
	}

	void OnTriggerExit(Collider coll){
		if (coll.CompareTag("Player") && exitTrigger){
			doTrigger ();
		}
	}

	private void doTrigger() {
		foreach (TutorialMessage.TutorialSwitchTuple tutorial in tutorials){
			tutorial.tut.ExternalTriggerCall(tutorial.method);
		}
	}
}
