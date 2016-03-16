using UnityEngine;
using System.Collections;

public class TunnelExit : MonoBehaviour {

	private AmazeballCam cameraRig;
	
	void Awake() {
		cameraRig = GameObject.FindGameObjectWithTag ("CameraRig").GetComponent<AmazeballCam> ();
	}

	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag ("Player")) {
			coll.material = coll.GetComponent<TransformController>().currentTransform.transformPhysicMaterial;
			BallController ball = coll.GetComponent<BallController>();
			ball.GetComponent<TransformController>().currentTransform.EnablePhysicalModifiers(ball);
			coll.GetComponent<BallController>().ResetMovementModifiersToDefaults();
			cameraRig.removeAngleConstraint();
			cameraRig.resetNeutralTilt();

		}
	}
}
