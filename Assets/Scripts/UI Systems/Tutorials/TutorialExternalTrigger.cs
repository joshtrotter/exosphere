using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TutorialExternalTrigger : MonoBehaviour {

	public TutorialMessage.TutorialSwitchTuple[] tutorials;

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player")){
			foreach (TutorialMessage.TutorialSwitchTuple tutorial in tutorials){
				tutorial.tut.ExternalTriggerCall(tutorial.method);
			}
		}
	}
}
