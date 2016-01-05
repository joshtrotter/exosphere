using UnityEngine;
using System.Collections;

public class SwitchOffTutorialMessage : MonoBehaviour {

	public SwitchableTutorialMessage[] tutorials;

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player")){
			foreach (SwitchableTutorialMessage tutorial in tutorials){
				tutorial.HasBeenSwitched();
			}
		}
	}
}
