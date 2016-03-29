using UnityEngine;
using System.Collections;

public class GlassTransform : BallTransform {

	//We track a reference to the laser diffuser whenever the glass ball is reflecting a laser so that if the glass transform is removed we can immediately disable the reflection
	private LaserDiffuser laserDiffuser;
	private LaserSnapTo laserSnapTo;

	//Track a reference to the camera so that we can slow down the turn speed when the ball is shooting a laser
	private AmazeballCam cam;
	private float startTurnSpeed;
	public float slowTurnSpeed = 1.5f;

	//A reference to the shattering particle effect that should be used when the ball breaks
	//public ParticleSystem shatterParticles;

	public override void Apply(BallController ball){
		base.Apply (ball);
		cam = GameObject.FindWithTag("CameraRig").GetComponent<AmazeballCam>();
		startTurnSpeed = cam.turnSpeed;
		//enable laser snapping
		laserSnapTo = ball.GetComponent<LaserSnapTo> ();
		laserSnapTo.Enable ();
		//enable ball shattering
		//ball.gameObject.AddComponent<BallShatterer>();


	}

	public override void Remove(BallController ball)
	{
		//ensure camera is at normal speed
		cam.turnSpeed = startTurnSpeed;

		//disable laser snapping
		laserSnapTo.Disable ();
		//ensure that any current snap is ended
		laserSnapTo.DoHitEnd ();


		//If reflecting then cancel it before removing the transform
		if (laserDiffuser != null) {
			OnLaserExit(laserDiffuser);
		}

		//disable ball shattering
		Destroy (ball.gameObject.GetComponent<BallShatterer> ());
	}

	public override void OnLaserEnter(LaserDiffuser laserDiffuser, ArcReactorHitInfo hitInfo)
	{
		//slow down camera movement for easier aiming
		cam.turnSpeed = slowTurnSpeed;

		laserDiffuser.Diffuse (hitInfo);
		this.laserDiffuser = laserDiffuser;
	}

	public override void OnLaserExit(LaserDiffuser laserDiffuser)
	{
		//ensure camera is at normal speed
		cam.turnSpeed = startTurnSpeed;

		laserDiffuser.Disable ();
		this.laserDiffuser = null;
	}

}
