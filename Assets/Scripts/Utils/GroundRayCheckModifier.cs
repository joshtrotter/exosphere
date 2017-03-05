using UnityEngine;
using System.Collections;

public class GroundRayCheckModifier : MonoBehaviour {

	public float rayLengthModifier = 1.5f;

	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag ("Player")) {
			coll.GetComponent<BallController>().groundRayLength += rayLengthModifier;
		}
	}
	
	void OnTriggerExit (Collider coll) {
		if (coll.CompareTag ("Player")) {
			coll.GetComponent<BallController>().groundRayLength -= rayLengthModifier;
		}
	}
}
