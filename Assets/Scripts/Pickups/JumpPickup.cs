using System;
using UnityEngine;

public class JumpPickup : Pickup
{
	// The force added to the ball when it jumps
	public float jumpPower = 8f; 

	public override String GetId () 
	{
		return "Jump";
	}

	public override int GetMaxCharges() 
	{
		return 3;
	}

	protected override void Apply(BallController ball)
	{
		// If on the ground then add an upward force
		if (ball.IsOnGround ()) {
			ball.gameObject.GetComponent<Rigidbody>().AddForce (Vector3.up * jumpPower, ForceMode.Impulse);
		}
	}

}


