using System;
using System.Collections;
using UnityEngine;

public class BrakeController : MonoBehaviour
{
	public bool brakeLockEnabled = true;

	//How much drag the brakes should apply to the ball
	public float brakeDragFactor = 10f;
	
	//The minimum brake power that must be applied before a brake lock can occur
	public float hardBrakeTrigger = 0.4f;
	//The minimum velocity for the ball to be travelling above which a hard brake will cause a brake lock to occur
	public float enterBrakeLockVelocity = 8f;
	//The velocity below which a brake lock will end and the player can resume control of the ball
	public float exitBrakeLockVelocity = 4f;
	//The amount to scale base friction by during a brake lock (should be below 1) - this reduces the friction on the ball while sliding
	public float brakeLockFrictionScale = 0.5f;
	// The angle above which a sharp turn will result in a brake lock
	public float enterBrakeLockTurnAngle = 60f;
	
	//The drag values when no brakes are being applied - we store these so we can reset to neutral after a brake
	private float neutralDrag;
	private float neutralAngularDrag;
	
	//The friction value outside of brake lock
	private float neutralFriction;
	
	//Whether the ball is currently brake locked (i.e. skidding out)
	private Boolean isBrakeLocked;
	
	//Hold a reference to the balls rigidbody
	private Rigidbody rb;
	//Hold a reference to the balls physic material
	private PhysicMaterial material;

	private BallController ballController;

	private void Awake()
	{
		rb = GetComponent<Rigidbody> ();
		material = GetComponent<Collider> ().material;
		ballController = GetComponent<BallController> ();

		//store the balls base drag values so that we can return to these after a brake has finished (we increase drag during a brake)
		//TODO assumes that neutral drag values are never altered anywhere else
		neutralDrag = rb.drag;
		neutralAngularDrag = rb.angularDrag;
		neutralFriction = material.dynamicFriction;
	}

	public void SetBrakeLockEnabled(bool enabled) {
		this.brakeLockEnabled = enabled;
		if (!enabled) {
			UnlockBrakes();
		}
	}

	public Boolean IsBrakeLocked() {
		return isBrakeLocked;
	}

	public void ApplyBrakes(float brakePower)
	{
		rb.drag = neutralDrag + (brakePower * brakeDragFactor);
		rb.angularDrag = neutralAngularDrag + (brakePower * brakeDragFactor);
	}

	//Return all drag values to neutral values
	public void ReleaseBrakes ()
	{
		rb.drag = neutralDrag;
		rb.angularDrag = neutralAngularDrag;
	}

	//Lock the brakes if the player attempts to turn sharply while the ball is travelling above the brakeLockVelocity
	public bool CheckForBrakeLockOnTurn(Vector3 moveDirection) {
		return Mathf.Abs (Vector3.Angle (rb.velocity, moveDirection)) > enterBrakeLockTurnAngle && rb.velocity.magnitude > enterBrakeLockVelocity;
	}

	//Lock the brakes if the player is applying a hard brake and the ball is travelling above the brakeLockVelocity
	public bool CheckForBrakeLockOnBrake(float brakePower) {
		return brakePower >= hardBrakeTrigger && rb.velocity.magnitude > enterBrakeLockVelocity;
	}
	
	//Called when the ball should enter a brake lock slide
	public void LockBrakes ()
	{
		if (brakeLockEnabled && ballController.IsReallyOnGround()) {
			//set the brake locked status - this will be used to ignore user input while brake locked
			isBrakeLocked = true;
			//lock the balls current rotation so it looks like it is sliding rather than rolling
			rb.freezeRotation = true;
		
			//remove drag and reduce friction on the ball so that it slides for a while
			rb.drag = 0.0f;
			material.dynamicFriction = neutralFriction * brakeLockFrictionScale;
		
			//Start a coroutine to check for the end of the brake lock
			StartCoroutine (WaitForEndOfBrakeLockSlide ());
		}
	}
	
	//Called when the ball should exit a brake lock slide
	public void UnlockBrakes ()
	{
		ReleaseBrakes ();
		
		rb.freezeRotation = false;
		material.dynamicFriction = neutralFriction;
		
		isBrakeLocked = false;
	}
	
	//Unlocks the brakes only after the balls velocity has reduced below the exitBrakeLockVelocity
	private IEnumerator WaitForEndOfBrakeLockSlide ()
	{
		while (rb.velocity.magnitude > exitBrakeLockVelocity && ballController.IsReallyOnGround()) {
			yield return new WaitForFixedUpdate ();
		}
		UnlockBrakes ();
	}
}


