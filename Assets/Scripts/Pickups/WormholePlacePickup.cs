using System;
using UnityEngine;

public class WormholePlacePickup : Pickup
{
	public WormholeJumpPickup jumpPickup;

	public override String GetId () 
	{
		return "WormholePlace";
	}

	public override int GetMaxCharges() 
	{
		return Int16.MaxValue;
	}

	protected override void Apply(BallController ball)
	{
		jumpPickup.SetJumpLocation (ball.transform);
	}

}


