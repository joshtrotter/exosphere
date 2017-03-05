using UnityEngine;
using System.Collections;

public class FanBooster : MonoBehaviour {

	public float fanBoostPower = 5f;

	void OnTriggerEnter(Collider coll) {
		TransformController ballTransform = coll.gameObject.GetComponent<TransformController> ();
		if (ballTransform != null && ballTransform.currentTransform.GetType() == typeof(LightTransform)) {
			coll.attachedRigidbody.velocity *= 0.1f; 
		}
		coll.attachedRigidbody.AddForce (Vector3.up * fanBoostPower, ForceMode.Impulse);
	}
}
