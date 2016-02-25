using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WormholeJumpPickup : Pickup
{
	public ParticleSystem wormhole;
	public ParticleSystem wormholeExplosion;
	public float wormholeMinTravelTime = 0.5f;
	public float wormholeMaxTravelTime = 2f;
	public float wormholeMinTravelDistance = 16f;
	public float wormholeMaxTravelDistance = 128f;
	public Vector3 wormholePosOffset;

	private GameObject cameraRig;
	private GameObject jumpLocation;

	void Start(){
		RemoveWormhole ();
	}

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
		//this.jumpLocation = null;
		//this.wormhole.gameObject.SetActive (false);
		//this.wormhole.enableEmission = false;
	}

	//clears away a currently placed wormhole after a jump
	private void RemoveWormhole(){
		this.jumpLocation = null;
		this.wormhole.gameObject.SetActive (false);
		this.wormhole.enableEmission = false;
	}

	protected override void Apply (BallController ball)
	{
		if (jumpLocation != null) {
			wormholeExplosion.transform.position = ball.transform.position;
			wormholeExplosion.Play();
			Vector3 currentVelocity = ball.GetComponent<Rigidbody>().velocity;

			ball.gameObject.GetComponent<Renderer>().enabled = false;
			GetCameraRig ().GetComponent<AmazeballCam> ().enabled = false;

			float travelTime = CalculateTravelTimeToWormhole(ball.transform.position);
			//Vibration.Vibrate (Convert.ToInt64(travelTime) * 1000L);

			DOTween.Sequence ()
				.Append (GetCameraRig ().transform.DOMove (jumpLocation.transform.position, travelTime))
					.Join (GetCameraRig ().transform.DORotateQuaternion (jumpLocation.transform.rotation, travelTime))
					.OnComplete (() => CompleteJump(ball, currentVelocity));
		}
	}

	private float CalculateTravelTimeToWormhole(Vector3 ballPos) {
		Vector3 distance = this.jumpLocation.transform.position - ballPos;
		float distanceScale = Mathf.InverseLerp (wormholeMinTravelDistance, wormholeMaxTravelDistance, distance.magnitude);
		return Mathf.Lerp (wormholeMinTravelTime, wormholeMaxTravelTime, distanceScale);
	}

	private void CompleteJump (BallController ball, Vector3 currentVelocity)
	{
		GetCameraRig ().GetComponent<AmazeballCam> ().camAngle = GetCameraRig ().transform.rotation.eulerAngles.y;
		GetCameraRig ().GetComponent<AmazeballCam> ().enabled = true;
		ball.gameObject.transform.position = this.jumpLocation.transform.position;

		Vector3 newTargetVelocity = jumpLocation.transform.forward * currentVelocity.magnitude;
		ball.GetComponent<Rigidbody>().velocity = newTargetVelocity;

		wormholeExplosion.transform.position = wormhole.transform.position;
		wormholeExplosion.Play ();
		ball.gameObject.GetComponent<Renderer>().enabled = true;
		StartCoroutine (UnlockBrakesIfRequired(ball.gameObject.GetComponent<BrakeController> ()));
	}

	public void SetJumpLocation (Transform jumpLocation)
	{
		this.jumpLocation = new GameObject ();
		this.jumpLocation.transform.position = jumpLocation.position;
		this.jumpLocation.transform.rotation = GetCameraRig ().transform.rotation;
		this.wormhole.transform.position = jumpLocation.position + wormholePosOffset;
		this.wormhole.transform.rotation = this.jumpLocation.transform.rotation;
		this.wormhole.gameObject.SetActive (true);
		this.wormhole.enableEmission = true;
	}

	private GameObject GetCameraRig ()
	{
		if (cameraRig == null) {
			cameraRig = GameObject.FindGameObjectWithTag ("CameraRig");
		}
		return cameraRig;
	}

	private IEnumerator UnlockBrakesIfRequired(BrakeController brakes) {
		for (int i = 1; i <= 2; i++) {
			if (brakes.IsBrakeLocked()) {
				brakes.UnlockBrakes ();
			}
			yield return new WaitForEndOfFrame();
		}	
		RemoveWormhole ();
	}

}


