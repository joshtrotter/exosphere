using DG.Tweening;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CallibrationUI : UISystem {

	public static CallibrationUI controller;

	public GameObject dropPanel;

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
		dropPanel.transform.DOLocalMoveY ((Screen.height * 1.2f), 0).Play ();
		base.Awake ();
	}

	public override void ShowRequestAccepted(){
		StartCalibration ();
	}

	//does a first time set up each time a new level is loaded/reloaded- called by level manager
	public void SetupCalibration()
	{

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
		tiltInput = GameObject.FindGameObjectWithTag ("TiltInput").GetComponent<AmazeballTiltInput> ();
		tiltInput.ConfigureVerticalOrientationOffset ();

		//if in tunnel, resume scoring
		if (LevelManager.manager.IsTunnelRunner ()) {
			TunnelScoreController scorer = rbBall.GetComponent<TunnelScoreController>();
			scorer.ResumeScoring();
		}

		//unfreeze ball
		rbBall.isKinematic = false;
		ballInputReader.enabled = true;
		//reset ball velocity
		rbBall.velocity = ballVelocity;
		rbBall.angularVelocity = ballAngularVelocity;

		//reset stored velocities
		ballVelocity = Vector3.zero;
		ballAngularVelocity = Vector3.zero;

		dropPanel.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).OnComplete (Deregister).Play ();
	}

	public override void Show ()
	{
		canvas.gameObject.SetActive (true);
		dropPanel.transform.DOLocalMoveY (0, 0.5f).Play ();
	}

	public void StartCalibration()
	{
		//if in tunnel, halt scoring
		if (LevelManager.manager.IsTunnelRunner ()) {
			TunnelScoreController scorer = rbBall.GetComponent<TunnelScoreController>();
			scorer.HaltScoring();
		}
		//remember ball's current velocity
		ballVelocity = rbBall.velocity;
		ballAngularVelocity = rbBall.angularVelocity;
		//freeze ball
		rbBall.isKinematic = true;
		ballInputReader.enabled = false;

		Show ();
	}

}
