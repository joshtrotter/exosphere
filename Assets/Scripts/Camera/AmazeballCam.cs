using System;
using UnityEngine;
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
	public float turnSpeed = 3f;  
	// The maximum value of the x axis rotation of the pivot
	public float maxTilt = 15f; 
	// The minimum value of the x axis rotation of the pivot
	public float minTilt = 5f; 
	// The target FPS - used to ensure turn speeds are frame rate independent
	public int targetFps = 60;

	// The rig's y axis rotation
	public float camAngle;                    
	// The pivot's x axis rotation
	private float pivotTilt;                    

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
			
		// Adjust the look angle by an amount proportional to the turn speed and horizontal input.
		camAngle += (x * turnSpeed * Time.deltaTime * targetFps) % 360f;
		// Rotate the rig (the root object) around Y axis only:
		transform.localRotation = Quaternion.Euler (0f, camAngle, 0f);
			
		// Adjust the pivot tilt between zero and the min/max tilt values based on the vertical input
		pivotTilt = y > 0 ? Mathf.Lerp (0, -minTilt, y) : Mathf.Lerp (0, maxTilt, -y);
		// Rotate the pivot around the X axis only
		Quaternion pivotRotation = Quaternion.Euler (pivotTilt, pivotEulers.y, pivotEulers.z);
		pivot.localRotation = Quaternion.Slerp (pivot.localRotation, pivotRotation, Time.deltaTime);
	}

	void FixedUpdate ()
	{
		// Move the rig towards the ball position.
		transform.position = Vector3.Lerp (transform.position, ball.position, Time.deltaTime * moveSpeed);
	}

   
}
