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
		LevelManager.manager.SetCurrentLevel (currentLevel.levelID);
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
		levelName.text = currentLevel.levelName;

		//completion and time trials
		//set default values
		targetTime.text = "Hidden";
		fastestTime.text = "None";
		play.interactable = false;
		timeTrial.interactable = false;

		if (!currentLevel.unlocked) {
			levelCompletion.text = "Locked (" + currentLevel.starsRequiredToUnlock + " Stars Required)";
		} else if (currentLevel.completed) {
			levelCompletion.text = "Completed";
			play.interactable = true;
			//set up time trial
			timeTrial.interactable = true;
			targetTime.text = FloatToTimeString(currentLevel.targetTime);
			if (currentLevel.fastestTime != 0f){
				fastestTime.text = FloatToTimeString(currentLevel.fastestTime);
			}
		} else {
			levelCompletion.text = "Not Completed";
			play.interactable = true;
		}

		if (currentLevel.timeTrialCompleted) {
			timeTrialStar.sprite = collectedStar;
		} else {
			timeTrialStar.sprite = uncollectedStar;
		}

		//supply crates
		supplyCratesFound.text = currentLevel.GetNumCollectablesFound ();
		if (currentLevel.allCollectablesFound) {
			supplyCratesStar.sprite = collectedStar;
		} else {
			supplyCratesStar.sprite = uncollectedStar;
		}

		//golden ball
		if (currentLevel.goldenBallCollected) {
			goldenBallFound.text = "Found";
			goldenBallStar.sprite = collectedStar;
		} else {
			goldenBallFound.text = "Not Found";
			goldenBallStar.sprite = uncollectedStar;
		}

	}

	//converts a float in seconds to a MM:SS:FF format string
	private string FloatToTimeString(float time){
		int mins = Mathf.FloorToInt (time / 60);
		int secs = Mathf.FloorToInt (time % 60);
		int milli = Mathf.FloorToInt ((time - Mathf.Floor (time)) * 60);
		return string.Format ("{0:00}:{1:00}:{2:00}", mins, secs, milli);
	}
}
