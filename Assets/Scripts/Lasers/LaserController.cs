using UnityEngine;
using System.Collections;

/**
 * This class is responsible for handling when a laser hits the ball. The response to a hit is handled by the current transform as it depends on the transform state.
 * For example a glass ball will diffuse the laser whereas an air ball might burst.
 */
public class LaserController : LaserHitManager {

	private TransformController transformController;
	private LaserDiffuser laserDiffuser;

	void Awake() 
	{
		transformController = GetComponent<TransformController> ();
		laserDiffuser = GetComponentInChildren<LaserDiffuser> ();	
	}

	//Called when a laser hits the ball - the reaction is processed in the transform
	public override void DoHitStart(ArcReactorHitInfo hitInfo) 
	{
		base.DoHitStart (hitInfo);
		transformController.currentTransform.OnLaserEnter (laserDiffuser, hitInfo);
	}

	//Called when the laser leaves the ball - the reaction is processed in the transform
	public override void DoHitEnd() 
	{
		base.DoHitEnd ();
		transformController.currentTransform.OnLaserExit(laserDiffuser);
	}
}
