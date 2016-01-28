
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CallibrationUI : UISystem {

	public static CallibrationUI controller;

	private AmazeballTiltInput tiltInput;
	
	private BallInputReader ballInputReader;
	private Rigidbody rbBall;
	private Vector3 ballVelocity;
	private Vector3 ballAngularVelocity;

	private Canvas canvas;

	public override void Awake(){
		//set up singleton instance
		if (controller == null) {
			controller = this;
			DontDestroyOnLoad (this);
		} else if (controller != this) {
			Destroy(gameObject);
		}
		canvas = GetComponentInChildren<Canvas> ();
		base.Awake ();
	}

	public override void ShowRequestAccepted(){
		StartCalibration ();
	}

	//does a first time set up each time a new level is loaded/reloaded- called by level manager
	public void SetupCalibration()
	{
		tiltInput = GameObject.FindGameObjectWithTag ("TiltInput").GetComponent<AmazeballTiltInput> ();

		GameObject ball = GameObject.FindGameObjectWithTag ("Player");
		rbBall = ball.GetComponent<Rigidbody> ();
		ballInputReader = ball.GetComponent<BallInputReader> ();

		RequestToBeShown ();
	}

	public override void Hide ()
	{
		canvas.gameObject.SetActive (false);
	}

	public void FinishCalibration()
	{
		Deregister ();
		tiltInput.ConfigureVerticalOrientationOffset ();
		
		//unfreeze ball
		rbBall.isKinematic = false;
		ballInputReader.enabled = true;
		//reset ball velocity
		rbBall.velocity = ballVelocity;
		rbBall.angularVelocity = ballAngularVelocity;

		Hide ();
	}

	public override void Show ()
	{
		canvas.gameObject.SetActive (true);
	}

	public void StartCalibration()
	{
		//remember ball's current velocity
		ballVelocity = rbBall.velocity;
		ballAngularVelocity = rbBall.angularVelocity;
		//freeze ball
		rbBall.isKinematic = true;
		ballInputReader.enabled = false;

		Show ();
	}

}
