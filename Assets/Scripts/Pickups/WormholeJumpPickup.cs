using System;
using UnityEngine;

public class WormholeJumpPickup : Pickup
{
	private Transform jumpLocation;

	public override String GetId () 
	{
		return "WormholeJump";
	}

	public override int GetMaxCharges() 
	{
		return Int16.MaxValue;
	}

	public override void Reset()
	{
		base.Reset ();
		this.jumpLocation = null;
	}

	protected override void Apply(BallController ball)
	{
		if (jumpLocation != null) {
			ball.transform.position = jumpLocation.position;
		}
	}

	public void SetJumpLocation(Transform jumpLocation)
	{
		this.jumpLocation = jumpLocation;
	}

}


