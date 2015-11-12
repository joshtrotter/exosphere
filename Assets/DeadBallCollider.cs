using UnityEngine;
using System.Collections;

public class DeadBallCollider : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider coll) 
	{
		if (coll.CompareTag("Player")) {
			Application.LoadLevel (Application.loadedLevel);
		}
	}
}
