using UnityEngine;
using System.Collections;

public class GameObjectActivator : MonoBehaviour {

	public GameObject[] toActivateOnEnter;
	public GameObject[] toDeactivateOnEnter;
	public GameObject[] toActivateOnExit;
	public GameObject[] toDeactivateOnExit;

	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag ("Player")) {
			foreach (GameObject go in toActivateOnEnter) {
				go.SetActive(true);
			}
			foreach (GameObject go in toDeactivateOnEnter) {
				go.SetActive(false);
			}
		}
	}
	
	void OnTriggerExit (Collider coll) {
		if (coll.CompareTag ("Player")) {
			foreach (GameObject go in toActivateOnExit) {
				go.SetActive(true);
			}
			foreach (GameObject go in toDeactivateOnExit) {
				go.SetActive(false);
			}
		}
	}
}
