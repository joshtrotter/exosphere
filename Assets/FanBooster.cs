using UnityEngine;
using System.Collections;

public class FanBooster : MonoBehaviour {

	public float fanBoostPower = 5f;

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.GetComponent<TransformController> ().currentTransform.GetType() == typeof(LightTransform)) {
			coll.attachedRigidbody.velocity *= 0.1f; 
		}
		coll.attachedRigidbody.AddForce (Vector3.up * fanBoostPower, ForceMode.Impulse);
	}
}
