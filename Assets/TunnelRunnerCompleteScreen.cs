using UnityEngine;
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
	public Text lastFastestKmText;
	public Text newHighScoreText;

	public Text bestScoreText;
	public Text bestDistanceText;
	public Text bestCrateCountText;
	public Text bestFastestKmText;
	
	private int lastScore = 0;
	private float lastDistance;
	private int lastCrateCount;
	private float lastKmTime;

	private int bestScore = 0;
	private float bestDistance;
	private int bestCrateCount;
	private float bestKmTime;

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
		bestKmTime = float.MaxValue;
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
		lastFastestKmText.text = GetFastestKmString (lastKmTime);
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
		bestFastestKmText.text = GetFastestKmString (bestKmTime);

		switchScoreButtonText.text = "Last Run";
		lastRunPanel.gameObject.SetActive (false);
		highScorePanel.gameObject.SetActive (true);
	}

	/* utility function to correctly represent fastest km times as strings,
	 * taking into account they may have yet to be initialized below MaxValue		
	 */
	private string GetFastestKmString(float kmTime){
		if (kmTime == float.MaxValue) {
			return "N/A";
		} else {
			return (kmTime.ToString ("F1") + "s");
		}
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
#else
		CallibrationUI.controller.FinishCalibration ();
#endif
		dropPanel.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).OnComplete (Deregister).Play ();
	}

	public override void Hide (){
		canvas.gameObject.SetActive (false);
	}

	public void UpdateLastRunData(){
		TunnelScoreController scoreController = GameObject.FindObjectOfType<TunnelScoreController> ();
		lastScore = scoreController.GetScore ();
		lastDistance = scoreController.GetDistance ();
		lastKmTime = scoreController.GetFastestKmTime ();

		lastCrateCount = LevelManager.manager.GetNumCollectablesFound ();

		UpdateBestRunData ();
	}

	private void UpdateBestRunData(){
		LoadBestRunData();

		bestScore = Mathf.Max (bestScore, lastScore);
		bestDistance = Mathf.Max (bestDistance, lastDistance);
		bestCrateCount = Mathf.Max (bestCrateCount, lastCrateCount);
		bestKmTime = Mathf.Min (bestKmTime, lastKmTime);

		SaveBestRunData ();
	}

	[System.Serializable]
	private class TunnelRunnerSaveData
	{
		public int saveScore;
		public float saveDistance;
		public int saveCrateCount;
		public float saveKmTime;

		public TunnelRunnerSaveData(int score, float dist, int crates, float kmTime){
			this.saveScore = score;
			this.saveDistance = dist;
			this.saveCrateCount = crates;
			this.saveKmTime = kmTime;
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
			bestKmTime = savedData.saveKmTime;
		}
	}

	private void SaveBestRunData(){
		Debug.Log ("Saving tunnel runner data");

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/exosphereTRData.dat");
		
		TunnelRunnerSaveData dataToBeSaved = new TunnelRunnerSaveData (bestScore, bestDistance, bestCrateCount, bestKmTime);

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

	private float GetBestKmTime(){
		if (bestKmTime != null) {
			return bestKmTime;
		} else {
			return float.MaxValue;
		}
	}

	public void ClearTunnelRunnerSaveData()
	{
		Debug.Log ("Clearing tunnel runner save data");
		if (File.Exists (Application.persistentDataPath + "/exosphereTRData.dat")) {
			File.Delete (Application.persistentDataPath + "/exosphereTRData.dat");
		}
	}
	
}
