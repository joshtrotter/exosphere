﻿
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CallibrationUI : UISystem {

	private AmazeballTiltInput tiltInput;
	
	private BallInputReader ballInputReader;
	private Rigidbody rbBall;
	private Vector3 ballVelocity;
	private Vector3 ballAngularVelocity;

	private Canvas canvas;

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

		canvas = GetComponentInChildren<Canvas> ();
		Hide ();
		RequestToBeShown ();
	}

	public override void Hide ()
	{
		//hide UI
		canvas.gameObject.SetActive (false);
		//show pause button again
		//pauseMenu.pauseButton.SetActive (true);
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
		//show UI
		canvas.gameObject.SetActive (true);
		//hide pause button 
		//pauseMenu.pauseButton.SetActive (false);
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
