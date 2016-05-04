using UnityEngine;
using System.Collections;

public class LightTrailTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			coll.gameObject.GetComponent<LightsController>().TurnLightTrailOn();
		}
	}

}
