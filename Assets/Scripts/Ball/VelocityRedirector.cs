using UnityEngine;
using System.Collections;

public class VelocityRedirector : MonoBehaviour {

	public Vector3 desiredDirection = new Vector3 (0,-1,1);
	public float redirectPercentageThreshold = 0.5f;
	public float redirectSpeed = 1f;
	
	private Coroutine velocityAdjuster;

	void Start(){
		desiredDirection.Normalize ();
	}
	
	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player")) {
			Rigidbody rb = coll.GetComponent<Rigidbody>();
			velocityAdjuster = StartCoroutine(redirectVelocity(rb));
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
	
	private IEnumerator redirectVelocity(Rigidbody rb) {
		while (rb.velocity.normalized != desiredDirection) {
			Vector3 direction = rb.velocity.normalized;
			float magnitude = rb.velocity.magnitude;
			direction = Vector3.Lerp (direction, desiredDirection, Time.deltaTime * redirectSpeed);
			rb.velocity = direction * magnitude;
			yield return new WaitForFixedUpdate();
		}
	}
}
