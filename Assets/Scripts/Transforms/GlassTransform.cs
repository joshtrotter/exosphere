using UnityEngine;
using System.Collections;

public class GlassTransform : BallTransform {

	//We track a reference to the laser diffuser whenever the glass ball is reflecting a laser so that if the glass transform is removed we can immediately disable the reflection
	private LaserDiffuser laserDiffuser;

	public override void Remove(BallController ball)
	{
		//If reflecting then cancel it before removing the transform
		if (laserDiffuser != null) {
			OnLaserExit(laserDiffuser);
		}
	}

	public override void OnLaserEnter(LaserDiffuser laserDiffuser, ArcReactorHitInfo hitInfo)
	{
		laserDiffuser.Diffuse (hitInfo);
		this.laserDiffuser = laserDiffuser;
	}

	public override void OnLaserExit(LaserDiffuser laserDiffuser)
	{
		laserDiffuser.Disable ();
		this.laserDiffuser = null;
	}

}
