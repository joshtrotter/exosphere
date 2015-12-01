using System;
using System.Collections;
using UnityEngine;

public class BrakeController : MonoBehaviour
{
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
	//Hold a reference to the transform controller
	private TransformController transformController;

	private void Awake()
	{
		rb = GetComponent<Rigidbody> ();
		material = GetComponent<Collider> ().material;
		transformController = GetComponent<TransformController> ();

		//store the balls base drag values so that we can return to these after a brake has finished (we increase drag during a brake)
		neutralDrag = rb.drag;
		neutralAngularDrag = rb.angularDrag;
		neutralFriction = material.dynamicFriction;
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
		bool lockBrakes = Mathf.Abs (Vector3.Angle (rb.velocity, moveDirection)) > enterBrakeLockTurnAngle && rb.velocity.magnitude > enterBrakeLockVelocity;
		if (lockBrakes) {
			LockBrakes ();
		}
		return lockBrakes;
	}

	//Lock the brakes if the player is applying a hard brake and the ball is travelling above the brakeLockVelocity
	public bool CheckForBrakeLockOnBrake(float brakePower) {
		bool lockBrakes = brakePower >= hardBrakeTrigger && rb.velocity.magnitude > enterBrakeLockVelocity;
		if (lockBrakes) {
			LockBrakes ();
		}
		return lockBrakes;
	}
	
	//Called when the ball should enter a brake lock slide
	public void LockBrakes ()
	{
		//set the brake locked status - this will be used to ignore user input while brake locked
		isBrakeLocked = true;
		//lock the balls current rotation so it looks like it is sliding rather than rolling
		rb.freezeRotation = true;
		
		//remove drag and reduce friction on the ball so that it slides for a while
		rb.drag = 0.0f;
		material.dynamicFriction = neutralFriction * brakeLockFrictionScale;
		
		//TODO this is a temporary colour effect - replace with skid marks, smoke, sound effects etc to the skid
		GetComponent<Renderer> ().material.SetColor("_EmissionColor", Color.red * 10f);
		
		//Start a coroutine to check for the end of the brake lock
		StartCoroutine (WaitForEndOfBrakeLockSlide ());
	}
	
	//Called when the ball should exit a brake lock slide
	public void UnlockBrakes ()
	{
		ReleaseBrakes ();
		
		rb.freezeRotation = false;
		material.dynamicFriction = neutralFriction;
		
		isBrakeLocked = false;
		
		//TODO this is temporary - we will be using better effects for the slide later on
		GetComponent<Renderer> ().material.SetColor("_EmissionColor", 
		                                            transformController.currentTransform.transformMaterial.GetColor("_EmissionColor"));
	}
	
	//Unlocks the brakes only after the balls velocity has reduced below the exitBrakeLockVelocity
	private IEnumerator WaitForEndOfBrakeLockSlide ()
	{
		while (rb.velocity.magnitude > exitBrakeLockVelocity) {
			yield return new WaitForFixedUpdate ();
		}
		UnlockBrakes ();
	}
}


