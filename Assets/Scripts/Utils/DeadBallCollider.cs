using UnityEngine;
using System.Collections;

public class DeadBallCollider : MonoBehaviour {

	void OnTriggerEnter(Collider coll) 
	{
		if (coll.CompareTag("Player")) {
			Application.LoadLevel (Application.loadedLevel);
		}
	}
}
