using UnityEngine;
using System.Collections;

public class FanBlower : MonoBehaviour {

	public float centrePullForce = 2f;
	public float minBlowForce = 0.90f;
	public float maxBlowForce = 1.10f;

	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player")) {
			coll.gameObject.GetComponent<BallController>().inControlledFlight = true;
		}
	}

	void OnTriggerStay(Collider coll) {
		Vector3 centreDirection = (transform.position - coll.transform.position).normalized * centrePullForce;
		coll.attachedRigidbody.AddForce (new Vector3(centreDirection.x, -centrePullForce / centreDirection.y, centreDirection.z) * Random.Range(minBlowForce, maxBlowForce));
	}

	void OnTriggerExit(Collider coll) {
		if (coll.CompareTag ("Player")) {
			coll.gameObject.GetComponent<BallController>().inControlledFlight = false;
		}
	}

}
