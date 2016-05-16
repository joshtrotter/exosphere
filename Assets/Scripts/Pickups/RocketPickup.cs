using UnityEngine;
using System;

public class RocketPickup : Pickup
{
	// The force added to the ball when it launches forward
	public float rocketPower = 15f;
	public int maxMultiplierIncrease = 3;
	
	public override String GetId () 
	{
		return "Rocket";
	}
	
	public override int GetMaxCharges() 
	{
		return 1;
	}
	
	protected override void Apply(BallController ball)
	{
		//ball.gameObject.GetComponent<Rigidbody>().AddForce (ball.GetTargetVelocity().normalized * rocketPower, ForceMode.Impulse);
		ball.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.Scale (Camera.main.transform.forward, new Vector3 (1, 0, 1)).normalized * rocketPower, ForceMode.Impulse);
		ball.GetComponent<LightsController>().TurnLightTrailOn();
		TunnelScoreController scorer = ball.GetComponent<TunnelScoreController> ();
		for (int i = 0; i < maxMultiplierIncrease; i++) {
			scorer.checkMultiplier ();
		}
	}
}
