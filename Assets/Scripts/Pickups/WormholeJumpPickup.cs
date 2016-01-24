using System;
using UnityEngine;
using DG.Tweening;

public class WormholeJumpPickup : Pickup
{
	private GameObject cameraRig;
	private GameObject jumpLocation;

	public override String GetId ()
	{
		return "WormholeJump";
	}

	public override int GetMaxCharges ()
	{
		return Int16.MaxValue;
	}

	public override void Reset ()
	{
		base.Reset ();
		this.jumpLocation = null;
	}

	protected override void Apply (BallController ball)
	{
		if (jumpLocation != null) {
			Vector3 currentVelocity = ball.GetComponent<Rigidbody>().velocity;

			ball.gameObject.GetComponent<Renderer>().enabled = false;
			GetCameraRig ().GetComponent<AmazeballCam> ().enabled = false;

			DOTween.Sequence ()
				.Append (GetCameraRig ().transform.DOMove (jumpLocation.transform.position, 2f))
					.Join (GetCameraRig ().transform.DORotateQuaternion (jumpLocation.transform.rotation, 2f))
					.OnComplete (() => CompleteJump(ball, currentVelocity));
		}
	}

	private void CompleteJump (BallController ball, Vector3 currentVelocity)
	{
		GetCameraRig ().GetComponent<AmazeballCam> ().camAngle = GetCameraRig ().transform.rotation.eulerAngles.y;
		GetCameraRig ().GetComponent<AmazeballCam> ().enabled = true;
		ball.gameObject.transform.position = this.jumpLocation.transform.position;

		Vector3 newTargetVelocity = jumpLocation.transform.forward * currentVelocity.magnitude;
		ball.GetComponent<Rigidbody>().velocity = newTargetVelocity;

		ball.gameObject.GetComponent<Renderer>().enabled = true;
	}

	public void SetJumpLocation (Transform jumpLocation)
	{
		this.jumpLocation = new GameObject ();
		this.jumpLocation.transform.position = jumpLocation.position;
		this.jumpLocation.transform.rotation = GetCameraRig ().transform.rotation;
	}

	private GameObject GetCameraRig ()
	{
		if (cameraRig == null) {
			cameraRig = GameObject.FindGameObjectWithTag ("CameraRig");
		}
		return cameraRig;
	}

}


