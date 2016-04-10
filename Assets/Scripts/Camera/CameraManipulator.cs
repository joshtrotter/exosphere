using UnityEngine;
using System.Collections;

public class CameraManipulator : MonoBehaviour {
		
	public bool camLock = false;
	public float camLockAngle = 90f;
	public float camLockTime = 2f;

	public bool camTilt = false;
	public float camTiltAngle = 45f;
	public float camTiltTime = 1f;

	public bool camZoom = false;
	public float camZoomAmount = -10f;
	public float camZoomTime = 2f;

	public bool resetOnExit = false;
	public float resetTiltTime = 0.25f;
	public float resetZoomTime = 0.25f;
	
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
			if (camZoom) {
				cameraRig.zoomCamera(camZoomAmount, camZoomTime);
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
			if (camZoom) {
				cameraRig.zoomCamera(-camZoomAmount, resetZoomTime);
			}
		}
	}
}
