using UnityEngine;
using System.Collections;

public class GlassTransform : BallTransform {

	//We track a reference to the laser diffuser whenever the glass ball is reflecting a laser so that if the glass transform is removed we can immediately disable the reflection
	private LaserDiffuser laserDiffuser;
	
	//START ADDED PART
	//Track a reference to the camera so that we can slow down the turn speed when the ball is shooting a laser
	private AmazeballCam cam;
	private float startTurnSpeed;
	public float slowTurnSpeed = 1.5f;

	public override void Apply(BallController ball){
		base.Apply (ball);
		cam = GameObject.FindWithTag("CameraRig").GetComponent<AmazeballCam>();
		startTurnSpeed = cam.turnSpeed;
	} //END ADDED

	public override void Remove(BallController ball)
	{
		//START ADDED
		//ensure camera is at normal speed
		print ("returning cam speed to normal");
		cam.turnSpeed = startTurnSpeed;
		//END ADDED

		//If reflecting then cancel it before removing the transform
		if (laserDiffuser != null) {
			OnLaserExit(laserDiffuser);
		}
	}

	public override void OnLaserEnter(LaserDiffuser laserDiffuser, ArcReactorHitInfo hitInfo)
	{
		//START ADDED
		//slow down camera movement for easier aiming
		print ("setting cam speed to " + slowTurnSpeed);
		cam.turnSpeed = slowTurnSpeed;
		print ("set cam speed to " + slowTurnSpeed);
		//END ADDED

		laserDiffuser.Diffuse (hitInfo);
		this.laserDiffuser = laserDiffuser;
	}

	public override void OnLaserExit(LaserDiffuser laserDiffuser)
	{
		//START ADDED
		//ensure camera is at normal speed
		print ("returning cam speed to normal");
		cam.turnSpeed = startTurnSpeed;
		//END ADDED

		laserDiffuser.Disable ();
		this.laserDiffuser = null;
	}

}
