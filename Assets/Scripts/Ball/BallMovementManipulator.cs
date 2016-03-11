using UnityEngine;
using System.Collections;

public class BallMovementManipulator : MonoBehaviour {

	public BallController.MovementModifiers ballModifiers;
	public bool resetOnExit = false;

	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag ("Player") && ShouldModify(coll.gameObject)) {
			coll.GetComponent<BallController>().ModifyMovement(ballModifiers);
		}
	}

	void OnTriggerExit (Collider coll) {
		if (resetOnExit && coll.CompareTag ("Player")) {
			coll.GetComponent<BallController>().ResetMovementModifiersToDefaults();
		}
	}

	protected virtual bool ShouldModify(GameObject ball) {
		return true;
	}
}
