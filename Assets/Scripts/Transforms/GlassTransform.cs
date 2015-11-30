using UnityEngine;
using System.Collections;

public class GlassTransform : BallTransform {

	public override void OnLaserEnter(LaserDiffuser laserDiffuser, ArcReactorHitInfo hitInfo)
	{
		laserDiffuser.Diffuse (hitInfo);
	}

	public override void OnLaserExit(LaserDiffuser laserDiffuser)
	{
		laserDiffuser.Disable ();
	}

}
