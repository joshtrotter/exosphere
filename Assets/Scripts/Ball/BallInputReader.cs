using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/**
 * The purpose of this class is to read inputs from the CrossPlatformInputManager and trigger the corresponding
 * behaviours on the BallController
 */ 
public class BallInputReader : MonoBehaviour
{
	// Reference to the ball controller.
	private BallController ball; 

	// the world-relative desired move vector, calculated from the camForward and user input.
	private Vector3 move;

	//the braking force being applied
	private float brakeForce;

	// A reference to the main camera in the scenes transform
	private Transform cam; 

	// whether the jump button is currently pressed
	private bool jump;

	private void Awake ()
	{
		// Set up the references to other game objects
		ball = GetComponent<BallController> ();
		cam = Camera.main.transform;            
	}

	private void Update ()
	{
		//reset previous frame values
		move = Vector3.zero;
		brakeForce = 0;

		// Get the axis input.
		float v = CrossPlatformInputManager.GetAxis ("Vertical");

		//Accelerate if axis is pushed forward
		if (v > 0) {
			// calculate camera relative direction to move:
			Vector3 camForward = Vector3.Scale (cam.forward, new Vector3 (1, 0, 1)).normalized;
			move = v * camForward;
		}
			//Brake if axis is pulled back
			else {
			brakeForce = Mathf.Abs (v);
		}

		//Get the button input
		jump = CrossPlatformInputManager.GetButton ("Jump");
	}

	private void FixedUpdate ()
	{
		if (move.magnitude > 0) {
			ball.Accelerate (move);
		} else if (brakeForce > 0) {
			ball.Brake (brakeForce);
		} else {
			ball.Neutral ();
		}

		if (jump) {
			ball.Jump ();
			jump = false;
		}
	}

}
