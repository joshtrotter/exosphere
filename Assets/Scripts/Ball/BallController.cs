using System;
using System.Collections;
using UnityEngine;

/**
 * The purpose of this class is to provide ball behaviours. The BallInputReader will generally be responsible for triggering
 * these behaviours at the appropriate times.
 */
public class BallController : MonoBehaviour
{
	[System.Serializable]
	public class MovementModifiers {
		public float movePowerScaler = 1f;
		public float brakePowerScaler = 1f;
		public float constantMovePower = 0f;
		public bool isControlledFlight = false;
		public bool isGrinding = false;
	}

	// The force added to the ball to move it
	public float movePower = 10f; 
	// The speed with which the balls velocity is adjusted to match the camera rotation (too slow and the ball becomes hard to turn, too fast and the movement feels unrealistic)
	public float turnResponsiveness = 1f;
	// The maximum velocity the ball can rotate at
	public float maxAngularVelocity = 17.5f;
	// Enabling this will cause the ball to lock brakes like a car
	public bool allowBrakeLocks = false;

	// Provides ability to manipulate the balls movement
	public MovementModifiers defaultMovementModifiers;
	private MovementModifiers movementModifiers;
	 
	//Hold a reference to the brake controller script
	private BrakeController brakes;
	//Hold a reference to the balls rigidbody
	private Rigidbody rb;
	
	// The length of the ray used to check if the ball is grounded
	public float groundRayLength = 1f;

	//Store our current target velocity
	private Vector3 targetVelocity;

	//During a controlled flight the ball can be controlled by the player even while in mid-air
	public bool inControlledFlight;
	
	private void Awake ()
	{
		brakes = GetComponent<BrakeController> ();
		rb = GetComponent<Rigidbody> ();

		//once the ball is moving very slowly bring it to a complete stop
		rb.sleepThreshold = 0.2f;
		// Set the maximum angular velocity so that the ball doesn't spin too wildly
		rb.maxAngularVelocity = maxAngularVelocity;

		movementModifiers = defaultMovementModifiers;
	}

	//Invoked when acceleration is being applied
	public void Accelerate (Vector3 moveDirection)
	{
		//Disable acceleration while brake locked
		if (!brakes.IsBrakeLocked() && !movementModifiers.isGrinding) {
			//If we are accelerating then we definitely aren't braking so return the drag values to neutral
			brakes.ReleaseBrakes ();

			//Accelerate away from the camera if we are on the ground - TODO do we want to allow mid-air acceleration?
			if (IsOnGround () || inControlledFlight || movementModifiers.isControlledFlight) {
				rb.AddForce (moveDirection * movePower * movementModifiers.movePowerScaler);

				//If the player attempts to turn sharply at a high velocity then they will skid out
				if (allowBrakeLocks && brakes.CheckForBrakeLockOnTurn(moveDirection)) {
					brakes.LockBrakes();
				}

				//Our target velocity is the current acceleration direction with the current velocity magnitude
				targetVelocity = moveDirection.normalized * rb.velocity.magnitude;
				//If we jump straight to the target velocity the movement will not feel smooth when the camera turn so we will lerp there over time
				rb.velocity = Vector3.Lerp (rb.velocity, targetVelocity, Time.deltaTime * turnResponsiveness);
			} else {
				brakes.ReleaseBrakes();
			}
		}
	}

	//Invoked when the axis controls are in the neutral position
	public void Neutral ()
	{
		brakes.ReleaseBrakes ();
	}

	//Invoked when braking is being applied
	public void Brake (float brakePower)
	{
		if (!brakes.IsBrakeLocked() && !movementModifiers.isGrinding) {
			if (IsReallyOnGround ()) {
				//Lock the brakes if the player is applying a hard brake and the ball is travelling above the brakeLockVelocity...
				if (allowBrakeLocks && brakes.CheckForBrakeLockOnBrake(brakePower)) {
					brakes.LockBrakes();
				} else {
					//...otherwise apply brakes normally
					brakes.ApplyBrakes(brakePower * movementModifiers.brakePowerScaler);
				}
			} else {
				brakes.ReleaseBrakes();
			}
		}
	}

	public void FixedUpdate() {
		if (movementModifiers.constantMovePower > 0f) {
			rb.AddForce(Vector3.Scale (Camera.main.transform.forward, new Vector3 (1, 0, 1)).normalized * movementModifiers.constantMovePower);
		}
	}

	public void ModifyMovement(MovementModifiers modifiers) {
		this.movementModifiers = modifiers;
		updateGrind ();
	}

	public void SetGrind(bool grind) {
		movementModifiers.isGrinding = grind;
		updateGrind ();
	}

	public void ResetMovementModifiersToDefaults() {
		this.movementModifiers = defaultMovementModifiers;
	}

	public Vector3 GetTargetVelocity()
	{
		return targetVelocity;
	}

	public float GetMovePower(){
		return movePower;
	}


	//Utility method for checking if the ball is grounded
	public Boolean IsOnGround ()
	{
		//TODO This is a candidate for future optimization - this method is called a lot and Physics.Raycast is relatively expensive
		return Physics.Raycast (transform.position, Vector3.down, groundRayLength);
	}

	public Boolean IsReallyOnGround ()
	{
		//TODO This is a candidate for future optimization - this method is called a lot and Physics.Raycast is relatively expensive
		return Physics.Raycast (transform.position, Vector3.down, groundRayLength - 0.4f);
	}

	private void updateGrind() {
		rb.freezeRotation = movementModifiers.isGrinding;
		if (movementModifiers.isGrinding) {
			rb.velocity = Vector3.Scale(rb.velocity, new Vector3(1, 0, 1));
		}
	}
	
}
