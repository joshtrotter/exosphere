﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DG.Tweening;

public class TunnelRunnerCompleteScreen : UISystem {

	public static TunnelRunnerCompleteScreen controller;

	public Canvas canvas;
	public GameObject dropPanel;

	public GameObject highScorePanel;
	public GameObject lastRunPanel;

	public Text switchScoreButtonText;
	private bool isShowingLastRun;

	public Text lastScoreText;
	public Text lastDistanceText;
	public Text lastCrateCountText;
	public Text lastSpeedText;
	public Text newHighScoreText;

	public Text bestScoreText;
	public Text bestDistanceText;
	public Text bestCrateCountText;
	public Text bestSpeedText;
	
	private int lastScore = 0;
	private float lastDistance;
	private int lastCrateCount;
	private float lastSpeed;

	private int bestScore = 0;
	private float bestDistance;
	private int bestCrateCount;
	private float bestSpeed;

	//allows us to freeze ball while screen is showing
	private BallInputReader ballInputReader;
	private Rigidbody rbBall;

	public override void Awake(){
		//set up singleton instance
		if (controller == null) {
			controller = this;
			DontDestroyOnLoad (this);
		} else if (controller != this) {
			Destroy(gameObject);
		}
		base.Awake ();
		dropPanel.transform.DOLocalMoveY ((Screen.height * 1.2f), 0).Play ();
	}
	
	public override void Show (){
		canvas.gameObject.SetActive (true);
		LoadBestRunData ();
		if (lastScore > 0) {
			switchScoreButtonText.transform.parent.gameObject.SetActive(true);
			ShowLastRun ();
		} else {
			switchScoreButtonText.transform.parent.gameObject.SetActive(false);
			ShowHighScores();
		}

		//freeze ball
		GameObject ball = GameObject.FindGameObjectWithTag ("Player");
		rbBall = ball.GetComponent<Rigidbody> ();
		ballInputReader = ball.GetComponent<BallInputReader> ();
		rbBall.isKinematic = true;
		ballInputReader.enabled = false;

		dropPanel.transform.DOLocalMoveY (0, 0.5f).Play ();
	}

	private void ShowLastRun(){
		isShowingLastRun = true;

		lastScoreText.text = lastScore.ToString();
		lastDistanceText.text = lastDistance.ToString ("F0") + "m";
		lastCrateCountText.text = lastCrateCount.ToString();
		lastSpeedText.text = lastSpeed.ToString ("F1") + "m/s";
		newHighScoreText.text = lastScore >= bestScore ? "New High Score!" : (bestScore - lastScore) + " points below Highscore";

		switchScoreButtonText.text = "Best Run";
		highScorePanel.gameObject.SetActive (false);
		lastRunPanel.gameObject.SetActive (true);
	}

	private void ShowHighScores(){
		isShowingLastRun = false;

		bestScoreText.text = bestScore.ToString();
		bestDistanceText.text = bestDistance.ToString ("F0") + "m";
		bestCrateCountText.text = bestCrateCount.ToString();
		bestSpeedText.text = bestSpeed.ToString ("F1") + "m/s";

		switchScoreButtonText.text = "Last Run";
		lastRunPanel.gameObject.SetActive (false);
		highScorePanel.gameObject.SetActive (true);
	}

	public void SwitchScoreView(){
		if (isShowingLastRun) {
			ShowHighScores();
		} else {
			ShowLastRun();
		}
	}

	public void StartRun(){
		//unfreeze ball if not using mobile input, else leave it for the calibration screen
#if !MOBILE_INPUT	
		rbBall.isKinematic = false;
		ballInputReader.enabled = true;
#endif
		CallibrationUI.controller.FinishCalibration ();
		dropPanel.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).OnComplete (Deregister).Play ();
	}

	public override void Hide (){
		canvas.gameObject.SetActive (false);
	}

	public void UpdateLastRunData(){
		TunnelScoreController scoreController = GameObject.FindObjectOfType<TunnelScoreController> ();
		lastScore = scoreController.GetScore ();
		lastDistance = scoreController.GetDistance ();
		lastSpeed = lastDistance / scoreController.GetRunTime ();

		lastCrateCount = LevelManager.manager.GetNumCollectablesFound ();

		UpdateBestRunData ();
	}

	private void UpdateBestRunData(){
		LoadBestRunData();

		bestScore = Mathf.Max (bestScore, lastScore);
		bestDistance = Mathf.Max (bestDistance, lastDistance);
		bestCrateCount = Mathf.Max (bestCrateCount, lastCrateCount);
		bestSpeed = Mathf.Max (bestSpeed, lastSpeed);

		SaveBestRunData ();
	}

	[System.Serializable]
	private class TunnelRunnerSaveData
	{
		public int saveScore;
		public float saveDistance;
		public int saveCrateCount;
		public float saveSpeed;

		public TunnelRunnerSaveData(int score, float dist, int crates, float speed){
			this.saveScore = score;
			this.saveDistance = dist;
			this.saveCrateCount = crates;
			this.saveSpeed = speed;
		}
	}

	private void LoadBestRunData(){
		if (File.Exists(Application.persistentDataPath + "/exosphereTRData.dat"))
		{	
			Debug.Log ("Loading tunnel runner data");

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/exosphereTRData.dat", FileMode.Open);
			//pull data from file
		 	TunnelRunnerSaveData savedData = (TunnelRunnerSaveData)bf.Deserialize(file);
			file.Close ();
			
			//read contents into local variables
			bestScore = savedData.saveScore;
			bestDistance = savedData.saveDistance;
			bestCrateCount = savedData.saveCrateCount;
			bestSpeed = savedData.saveSpeed;
		}
	}

	private void SaveBestRunData(){
		Debug.Log ("Saving tunnel runner data");

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/exosphereTRData.dat");
		
		TunnelRunnerSaveData dataToBeSaved = new TunnelRunnerSaveData (bestScore, bestDistance, bestCrateCount, bestSpeed);

		//save data to file
		bf.Serialize (file, dataToBeSaved);
		file.Close();
	}

	
	public override void BackKey(){
		MenuButton ();
	}

	public void MenuButton(){
		dropPanel.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).OnComplete (BackToMenu).Play ();
	}

	private void BackToMenu(){
		Debug.Log ("Loading level loader from tunnel runner complete screen");
		Application.LoadLevel (0);
		MainMenuController.controller.ReturnFocusToMainMenu ();
	}
	
}
