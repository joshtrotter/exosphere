using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TunnelRunnerCompleteScreen : UISystem {

	public static TunnelRunnerCompleteScreen controller;

	public Canvas canvas;
	public GameObject highScorePanel;
	public GameObject lastRunPanel;

	public Button switchScoreButton;
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


	public override void Awake(){
		//set up singleton instance
		if (controller == null) {
			controller = this;
			DontDestroyOnLoad (this);
		} else if (controller != this) {
			Destroy(gameObject);
		}
		base.Awake ();
	}
	
	public override void Show (){
		canvas.gameObject.SetActive (true);
		if (lastScore > 0) {
			switchScoreButton.interactable = true;
			ShowLastRun ();
		} else {
			switchScoreButton.interactable = false;
			ShowHighScores();
		}
	}

	private void ShowLastRun(){
		isShowingLastRun = true;

		lastScoreText.text = lastScore.ToString();
		lastDistanceText.text = lastDistance.ToString ("F0") + "m";
		lastCrateCountText.text = lastCrateCount.ToString();
		lastSpeedText.text = lastSpeed.ToString ("F1") + "m/s";
		newHighScoreText.text = lastScore >= bestScore ? "New High Score!" : (bestScore - lastScore) + " points below Highscore";

		switchScoreButton.GetComponentInChildren<Text>().text = "Best Run";
		highScorePanel.gameObject.SetActive (false);
		lastRunPanel.gameObject.SetActive (true);
	}

	private void ShowHighScores(){
		isShowingLastRun = false;

		bestScoreText.text = bestScore.ToString();
		bestDistanceText.text = bestDistance.ToString ("F0") + "m";
		bestCrateCountText.text = bestCrateCount.ToString();
		bestSpeedText.text = bestSpeed.ToString ("F1") + "m/s";

		switchScoreButton.GetComponentInChildren<Text>().text = "Last Run";
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
		Deregister ();
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
		if (!(bestScore > 0)) {
			LoadBestRunData();
		}

		bestScore = Mathf.Max (bestScore, lastScore);
		bestDistance = Mathf.Max (bestDistance, lastDistance);
		bestCrateCount = Mathf.Max (bestCrateCount, lastCrateCount);
		bestSpeed = Mathf.Max (bestSpeed, lastSpeed);

		SaveBestRunData ();
	}

	private void LoadBestRunData(){
		//TODO implement tunnel runner score persistance
	}

	private void SaveBestRunData(){
		//TODO implement tunnel runner score pesistance
	}

	
	public override void BackKey(){
		
	}
}
