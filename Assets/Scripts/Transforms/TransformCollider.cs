using UnityEngine;
using System.Collections;

public class TransformCollider : MonoBehaviour {

	public BallTransform ballTransform;
	
	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag("Player")) {
			TransformController transformController = coll.gameObject.GetComponent<TransformController>();
			transformController.ApplyTransform(ballTransform);
		}
	}
}
