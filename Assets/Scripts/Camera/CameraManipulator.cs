using UnityEngine;
using System.Collections;

public class CameraManipulator : MonoBehaviour {
		
	public bool camLock = false;
	public float camLockAngle = 90f;
	public float camLockTime = 2f;

	public bool camTilt = false;
	public float camTiltAngle = 45f;
	public float camTiltTime = 1f;

	public bool resetOnExit = false;
	public float resetTiltTime = 0.25f;
	
	private AmazeballCam cameraRig;
	private float camLockCentreAngle;
	
	void Awake() {
		cameraRig = GameObject.FindGameObjectWithTag ("CameraRig").GetComponent<AmazeballCam> ();
		camLockCentreAngle = transform.eulerAngles.y;
	}
	
	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag ("Player")) {
			if (camLock) {
				cameraRig.constrainCameraAngle(camLockCentreAngle, camLockAngle, camLockTime);
			}
			if (camTilt) {
				cameraRig.adjustNeutralTilt(camTiltAngle, camTiltTime);
			}
		}
	}
	
	void OnTriggerExit (Collider coll) {
		if (resetOnExit && coll.CompareTag ("Player")) {
			if (camLock) {
				cameraRig.removeAngleConstraint();
			}
			if (camTilt) {
				cameraRig.resetNeutralTilt(resetTiltTime);
			}
		}
	}
}
