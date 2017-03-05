using UnityEngine;
using System.Collections;

public class AimForRocket : MonoBehaviour {

	public Transform target;

	void OnTriggerStay(Collider coll) {
		if (coll.CompareTag ("Player")) {
			Vector3 centreDirection = (target.position - coll.transform.position).normalized * 5f;
			coll.attachedRigidbody.AddForce (new Vector3 (centreDirection.x, centreDirection.y, centreDirection.z));
		}
	}
}
