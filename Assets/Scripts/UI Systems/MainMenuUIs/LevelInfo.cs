using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* this class displays a summary of all info about a level and provides access to the Play button; 
 */
public class LevelInfo : UISystem {

	public static LevelInfo controller;

	private Canvas canvas;

	//a reference to the level we are currently displaying
	private LevelData currentLevel;

	public Text levelName;
	public Text levelCompletion;
	public Text supplyCratesFound;
	public Text goldenBallFound;
	public Text targetTime;
	public Text fastestTime;
	public Button play;
	public Button timeTrial;

	public Image supplyCratesStar;
	public Image goldenBallStar;
	public Image timeTrialStar;

	//the star images which are used to show whether or not a star has been collected
	public Sprite collectedStar;
	public Sprite uncollectedStar;

	public override void Awake(){
		controller = this;
		canvas = GetComponentInChildren<Canvas> ();
		base.Awake ();
	}

	public void DisplayLevelInfo(LevelData newLevel){
		currentLevel = newLevel;
		RequestToBeShown ();
	}

	//tells the levelManager to load the currently displayed level
	public void PlayLevelButton(){
		LevelManager.manager.SetCurrentLevel (currentLevel.GetLevelID());
		LevelManager.manager.ReloadLevel ();	
	}

	public override void Show(){
		SetLatestInfo ();
		canvas.gameObject.SetActive (true);
	}

	public override void Hide(){
		canvas.gameObject.SetActive (false);
	}

	//updates all text fields and images with the latest data that has been saved
	private void SetLatestInfo(){
		levelName.text = currentLevel.GetLevelName();

		//completion and time trials
		targetTime.text = currentLevel.GetTargetTimeAsString ();
		fastestTime.text = currentLevel.GetFastestTimeAsString ();
		levelCompletion.text = currentLevel.GetCompletionStatus ();
		play.interactable = currentLevel.IsUnlocked ();
		timeTrial.interactable = currentLevel.HasBeenCompleted ();

		/*if (!currentLevel.IsUnlocked()) {
			levelCompletion.text = "Locked (" + currentLevel.permData.starsRequiredToUnlock + " Stars Required)";
		} else if (currentLevel.saveData.completed) {
			levelCompletion.text = "Completed";
			play.interactable = true;
			//set up time trial
			timeTrial.interactable = true;
			targetTime.text = FloatToTimeString(currentLevel.permData.targetTime);
			if (currentLevel.saveData.fastestTime != 0f){
				fastestTime.text = FloatToTimeString(currentLevel.saveData.fastestTime);
			}
		} else {
			levelCompletion.text = "Not Completed";
			play.interactable = true;
		}*/

		if (currentLevel.TimeTrialHasBeenCompleted()) {
			timeTrialStar.sprite = collectedStar;
		} else {
			timeTrialStar.sprite = uncollectedStar;
		}

		//supply crates
		supplyCratesFound.text = currentLevel.GetNumCollectablesFoundOutOfTotal ();
		if (currentLevel.HasAllCollectablesFound()) {
			supplyCratesStar.sprite = collectedStar;
		} else {
			supplyCratesStar.sprite = uncollectedStar;
		}

		//golden ball
		if (currentLevel.HasGoldenBallCollected()) {
			goldenBallFound.text = "Found";
			goldenBallStar.sprite = collectedStar;
		} else {
			goldenBallFound.text = "Not Found";
			goldenBallStar.sprite = uncollectedStar;
		}

	}
}
