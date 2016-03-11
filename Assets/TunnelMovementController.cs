using UnityEngine;
using System.Collections;

public class TunnelMovementController : MonoBehaviour {

	public BallController.MovementModifiers ballModifiers;
	public float camLockAngle = 60f;
	public float camLockTime = 2f;
	public float camTiltAngle = 0f;
	public float camTiltTime = 1f;

	private AmazeballCam cameraRig;
	private float tunnelAngle;

	void Awake() {
		cameraRig = GameObject.FindGameObjectWithTag ("CameraRig").GetComponent<AmazeballCam> ();
		tunnelAngle = transform.eulerAngles.y;
		Debug.Log ("tunnel Angle = " + tunnelAngle);
	}

	// Use this for initialization
	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag ("Player")) {
			//coll.GetComponent<BallController>().movementModifiers = ballModifiers;
			cameraRig.constrainCameraAngle(tunnelAngle, camLockAngle, camLockTime);
			cameraRig.adjustNeutralTilt(camTiltAngle, camTiltTime);
		}
	}

}
