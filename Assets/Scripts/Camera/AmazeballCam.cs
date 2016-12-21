using System;
using UnityEngine;
using DG.Tweening;
using UnityStandardAssets.CrossPlatformInput;

/**
 * The purpose of this class is to control the camera as it follows the player ball. The rig will rotate
 * around the ball in response to input on the horizontal axis. The pivot point which is placed slightly
 * above the rig will tilt up and down in response to input on the vertical axis.
 * 
 * This script is designed to be placed on the root object of a camera rig,
 * comprising 3 gameobjects, each parented to the next:
 *
 *	 	Camera Rig
 *	 		Pivot
 *	 			Camera
 */ 
public class AmazeballCam : MonoBehaviour
{
	// How fast the rig will move to keep up with the target's position
	public float moveSpeed = 10f;
	// How fast the rig will rotate from the horiztonal axis input
	//public float turnSpeed = 3f;  
	public float maxTurnSpeed = 7.5f;
	public float minTurnSpeed = 1.5f;
	public float turnSpeedModifier = 1f;
	// The maximum value of the x axis rotation of the pivot
	public float maxBrakeTilt = 15f; 
	// The minimum value of the x axis rotation of the pivot
	public float maxAccelerateTilt = 5f; 
	// Adjust the base x axis rotation of the pivot - may be useful if the ball is falling or being propelled upwards
	public float defaultNeutralTilt = 0f;
	// The target FPS - used to ensure turn speeds are frame rate independent
	public int targetFps = 60;

	// The rig's y axis rotation
	public float camAngle;

	// Constrain the camera angle - used to prevent the camera turning 360 in tunnels 
	private bool constrainAngle = false;
	private float minAngle = -180f;
	private float maxAngle = 180f;

	// The pivot's x axis rotation
	private float pivotTilt; 

	private float neutralTilt;

	// The point at which the camera pivots around
	private Transform pivot;
	private Vector3 pivotEulers;

	// Reference to the ball so we can follow it around
	private Transform ball;

	void Awake ()
	{
		// find the pivot, which must be the parent of the camera
		pivot = GetComponentInChildren<Camera> ().transform.parent;
		pivotEulers = pivot.rotation.eulerAngles;
		neutralTilt = defaultNeutralTilt;
	}

	void Start ()
	{
		ball = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void Update ()
	{
		if (Time.timeScale < float.Epsilon)
			return;
			
		//Read the axis values from the accelerometer (between -1 and +1)
		var x = CrossPlatformInputManager.GetAxis ("Horizontal");
		var y = CrossPlatformInputManager.GetAxis ("Vertical");

		//take settings into account
		float turnSpeed = Mathf.Lerp(minTurnSpeed, maxTurnSpeed, PlayerPrefs.GetFloat ("CamSensitivity", 3f)) * turnSpeedModifier;
		bool inverted = PlayerPrefs.GetInt ("Inverted", 0) == 1;

		// Adjust the look angle by an amount proportional to the turn speed and horizontal input.
		camAngle += x * turnSpeed * Time.deltaTime * targetFps * (inverted ? -1 : 1);
		// Keep the value between -180 and +180
		camAngle = restrictAngleBetween180s (camAngle);

		//Lock the camera angle if the min/max angle constraints are set
		if (constrainAngle) {
			//special case when min and max angle cross the 180 degree centre line
			if (minAngle > 0f && maxAngle < 0f) {
				if (camAngle >= 0) {
					if (camAngle < minAngle) {
						if (Mathf.Abs (maxAngle) + camAngle < minAngle - camAngle) {
							camAngle = maxAngle;
						} else {
							camAngle = minAngle;
						}
					}
				} else {
					if (camAngle < maxAngle) {
						if (camAngle + 360 - minAngle < maxAngle - camAngle) {
							camAngle = minAngle;
						} else {
							camAngle = maxAngle;
						}
					}
				}
			} else {
				camAngle = Mathf.Clamp (camAngle, minAngle, maxAngle);
			}
		}

		// Rotate the rig (the root object) around Y axis only:
		transform.localRotation = Quaternion.Euler (transform.localEulerAngles.x, camAngle, transform.localEulerAngles.z);
			
		// Adjust the pivot tilt between zero and the min/max tilt values based on the vertical input
		pivotTilt = y > 0 ? Mathf.Lerp (neutralTilt, neutralTilt - maxAccelerateTilt, y) : Mathf.Lerp (neutralTilt, neutralTilt + maxBrakeTilt, -y);
		// Rotate the pivot around the X axis only
		Quaternion pivotRotation = Quaternion.Euler (pivotTilt, pivotEulers.y, pivotEulers.z);
		pivot.localRotation = Quaternion.Slerp (pivot.localRotation, pivotRotation, Time.deltaTime);
	}

	void FixedUpdate ()
	{
		// Move the rig towards the ball position.
		transform.position = Vector3.Lerp (transform.position, ball.position, Time.deltaTime * moveSpeed);
	}

	public void movePivot(Vector3 move, float duration = 1f){
		pivot.transform.DOBlendableLocalMoveBy (move, duration).Play ();
	}

	public void constrainCameraAngle(float startAngle, float angleRange, float duration = 1f) {
		if (angleRange > 90) {
			throw new InvalidProgramException("Max supported angleRange is 90");
		}
		this.constrainAngle = true;
		float minAngle = restrictAngleBetween180s(startAngle - angleRange);
		float maxAngle = restrictAngleBetween180s(startAngle + angleRange);
		DOTween.To (()=> this.minAngle, x=> this.minAngle = x, minAngle, duration).Play();
		DOTween.To (()=> this.maxAngle, x=> this.maxAngle = x, maxAngle, duration).Play();
	}

	public void removeAngleConstraint() {
		this.constrainAngle = false;
		minAngle = float.MinValue;
		maxAngle = float.MaxValue;
	}

	public void adjustNeutralTilt(float newNeutralTilt, float duration = 1f) {
		DOTween.To (()=> neutralTilt, x=> neutralTilt = x, newNeutralTilt, duration).Play();
	}

	public void resetNeutralTilt(float duration = 1f) {
		DOTween.To (()=> neutralTilt, x=> neutralTilt = x, defaultNeutralTilt, duration).Play();
	}

	public void zoomCamera(float zoomAmount, float zoomTime = 1f) {
		Debug.Log ("Zooming to " + zoomAmount);
		Camera.main.transform.DOBlendableLocalMoveBy(new Vector3(0f,0f,zoomAmount), zoomTime).Play ();
	}

	public void lookAhead(float distanceAhead, float duration = 1f) {
		lookAt (ball.transform.position + (ball.GetComponent<Rigidbody> ().velocity.normalized * distanceAhead), duration);
	}

	public void lookAt(Vector3 target, float duration = 1f) {
		float camAngleToTarget = Quaternion.LookRotation (target - transform.position).eulerAngles.y;

		if (Mathf.Abs (camAngleToTarget - camAngle) > 180f) {
			if (camAngleToTarget > camAngle) {
				camAngleToTarget = camAngleToTarget - 360f;
			} else {
				camAngleToTarget = camAngleToTarget + 360f;
			}
		}

		DOTween.To (()=> camAngle, x=> camAngle = x, camAngleToTarget, duration).Play();
	}

	private float restrictAngleBetween180s(float angle) {
		if (angle > 180f) {
			angle = angle - 360f;
		} else if (angle < -180f) {
			angle = angle + 360f;
		}
		return angle;
	}

   
}
