using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CallibrationUI : MonoBehaviour {

	private AmazeballTiltInput tiltInput;
	
	private BallInputReader ballInputReader;
	private Rigidbody rbBall;
	private Vector3 ballVelocity;
	private Vector3 ballAngularVelocity;

	private Canvas canvas;

	public PauseMenu pauseMenu;

	//does a first time set up each time a new level is loaded/reloaded- called by level manager
	public void SetupCalibration()
	{
		tiltInput = GameObject.FindGameObjectWithTag ("TiltInput").GetComponent<AmazeballTiltInput> ();

		GameObject ball = GameObject.FindGameObjectWithTag ("Player");
		rbBall = ball.GetComponent<Rigidbody> ();
		ballInputReader = ball.GetComponent<BallInputReader> ();

		canvas = GetComponentInChildren<Canvas> ();

		StartCalibration ();
	}


	public void FinishCalibration()
	{
		tiltInput.ConfigureVerticalOrientationOffset ();

		//unfreeze ball
		rbBall.isKinematic = false;
		ballInputReader.enabled = true;
		//reset ball velocity
		rbBall.velocity = ballVelocity;
		rbBall.angularVelocity = ballAngularVelocity;

		//hide UI
		canvas.gameObject.SetActive (false);
		//show pause button again
		pauseMenu.pauseButton.SetActive (true);
	}

	public void StartCalibration()
	{
		//remember ball's current velocity
		ballVelocity = rbBall.velocity;
		ballAngularVelocity = rbBall.angularVelocity;
		//freeze ball
		rbBall.isKinematic = true;
		ballInputReader.enabled = false;

		//show UI
		canvas.gameObject.SetActive (true);
		//hide pause button 
		pauseMenu.pauseButton.SetActive (false);
	}

}
