using UnityEngine;
using System.Collections;

public class Savepoint : MonoBehaviour {

	public bool saveOnEnter = true;
	public bool saveOnExit;

	void OnTriggerEnter(Collider coll){
		if (saveOnEnter && coll.CompareTag ("Player")) {
			SaveProgress ();
		}
	}

	void OnTriggerExit(Collider coll){
		if (saveOnExit && coll.CompareTag ("Player")) {
			SaveProgress();
		}
	}

	private void SaveProgress(){
		Debug.Log ("progress saved");
		LevelManager.manager.UpdateLevelProgress();
	}
}
