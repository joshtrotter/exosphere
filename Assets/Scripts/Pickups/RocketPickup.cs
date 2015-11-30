using UnityEngine;
using System;

public class RocketPickup : Pickup
{
	// The force added to the ball when it jumps
	public float rocketPower = 15f; 
	
	public override String GetId () 
	{
		return "Rocket";
	}
	
	public override int GetMaxCharges() 
	{
		return Int16.MaxValue;
	}
	
	protected override void Apply(BallController ball)
	{
		ball.gameObject.GetComponent<Rigidbody>().AddForce (ball.GetTargetVelocity().normalized * rocketPower, ForceMode.Impulse);
	}
}
