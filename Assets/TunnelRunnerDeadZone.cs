using UnityEngine;
using System.Collections;

public class TunnelRunnerDeadZone : MonoBehaviour {

	void OnTriggerEnter(Collider coll) 
	{
		Debug.Log ("Collision detected");
		if (coll.CompareTag("Player")) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
