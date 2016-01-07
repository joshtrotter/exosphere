using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/**
 * The purpose of this class is to read inputs from the CrossPlatformInputManager and trigger the corresponding
 * behaviours on the BallController
 */ 
public class BallInputReader : MonoBehaviour
{
	public float pickupTimeThreshold;

	// Reference to the ball controller.
	private BallController ball;

	//Reference to the pickup controller;
	private PickupController pickups;

	//Reference to the transform controller;
	private TransformController transforms;

	// the world-relative desired move vector, calculated from the camForward and user input.
	private Vector3 move;

	//the braking force being applied
	private float brakeForce;

	// A reference to the main camera in the scenes transform
	private Transform cam; 

	//A reference to the HUD which will be informed of a shake
	private HUD hud;

	private void Awake ()
	{
		// Set up the references to other game objects
		ball = GetComponent<BallController> ();
		pickups = GetComponent<PickupController> ();
		transforms = GetComponent<TransformController> ();
		cam = Camera.main.transform;    
	}

	private void Start()
	{
		hud = GameObject.FindGameObjectWithTag ("LevelManager").GetComponentInChildren<HUD> ();
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

		//If we detect a touch then start tracking it
		if (CrossPlatformInputManager.GetButtonDown ("Right")) {
			StartCoroutine(TrackButtonPress("Right", PickupController.Slot.RIGHT, CrossPlatformInputManager.GetLastKnownPos("Right"), Time.time));
		}
		if (CrossPlatformInputManager.GetButtonDown ("Left")) {
			StartCoroutine(TrackButtonPress("Left", PickupController.Slot.LEFT, CrossPlatformInputManager.GetLastKnownPos("Left"), Time.time));
		}

		//If we detect a shake then trigger the response
		if (CrossPlatformInputManager.GetButtonDown ("Shake")) {
			transforms.RemoveCurrent();
			hud.SendMessage("MorphRemoved");
		}
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
	}

	private IEnumerator TrackButtonPress(String buttonName, PickupController.Slot slot, Vector2 startPos, float startTime)
	{
		bool isDrag = false;
		pickups.TouchPickup (slot);

		//Wait for the button to be released
		while (CrossPlatformInputManager.GetButton(buttonName)) {
			//If the button is not released within the threshold time then this touch is treated as a drag
			if (!isDrag && Time.time - startTime > pickupTimeThreshold) {
				isDrag = true;
				pickups.StartDragging(slot, CrossPlatformInputManager.GetLastKnownPos(buttonName));
			}
			if (isDrag) {
				pickups.Drag(CrossPlatformInputManager.GetLastKnownPos(buttonName));
			}
			yield return new WaitForFixedUpdate();
		}
		if (!isDrag) {
			pickups.UsePickup (slot);
		} else {
			pickups.EndDrag();
		}
	}

}
