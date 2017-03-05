using UnityEngine;
using System.Collections;

public class MomentumManipulator : MonoBehaviour {

	public float momentumThreshold = 10f;
	public float reductionSpeed = 1f;

	private Coroutine velocityAdjuster;

	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player")) {
			Rigidbody rb = coll.GetComponent<Rigidbody>();
			if (rb.velocity.magnitude > momentumThreshold) {
				velocityAdjuster = StartCoroutine(reduceVelocityToThreshold(rb));
			}
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.CompareTag ("Player")) {
			if (velocityAdjuster != null) {
				StopCoroutine(velocityAdjuster);
				velocityAdjuster = null;
			}
		}
	}

	private IEnumerator reduceVelocityToThreshold(Rigidbody rb) {
		while (rb.velocity.magnitude > momentumThreshold) {
			Vector3 targetVelocity = rb.velocity.normalized * momentumThreshold;
			rb.velocity = Vector3.Lerp (rb.velocity, targetVelocity, Time.deltaTime * reductionSpeed);
			yield return new WaitForFixedUpdate();
		}
	}
}
