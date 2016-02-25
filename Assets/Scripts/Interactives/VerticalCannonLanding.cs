using UnityEngine;
using System.Collections;

// This scripte helps the vertical cannon to re-enable ball control and wall clipping control at the correct time
// It should be placed on a trigger object just above where the ball will land
public class VerticalCannonLanding : MonoBehaviour {

	private VerticalCannon cannon;

	void Awake(){
		cannon = GetComponentInParent<VerticalCannon> ();
	}

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag ("Player")) {
			cannon.SetHasLanded();
		}
	}
}
