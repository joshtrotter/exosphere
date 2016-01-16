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
	private LevelData previousLevel;
	private LevelData nextLevel;

	public Text levelName;
	public Text levelCompletion;
	public Text supplyCratesFound;
	public Text goldenBallFound;
	public Text targetTime;
	public Text fastestTime;
	public Button playButton;
	public Button timeTrialButton;
	public Button previousButton;
	public Button nextButton;

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
		Deregister ();
		RequestToBeShown ();
	}

	//tells the levelManager to load the currently displayed level
	public void PlayLevelButton(){
		LevelManager.manager.SetCurrentLevel (currentLevel.GetLevelID());
		LevelManager.manager.ReloadLevel ();	
	}

	public void NextLevelButton(){
		DisplayLevelInfo (nextLevel);
	}

	public void PreviousLevelButton(){
		DisplayLevelInfo (previousLevel);
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
		Debug.Log ("Displaying data for " + currentLevel.GetLevelName ());
		levelName.text = currentLevel.GetLevelName();

		//completion
		levelCompletion.text = currentLevel.GetCompletionStatus ();
		playButton.interactable = currentLevel.IsUnlocked ();

		//time trials
		timeTrialButton.interactable = currentLevel.HasBeenCompleted ();
		targetTime.text = currentLevel.GetTargetTimeAsString ();
		fastestTime.text = currentLevel.GetFastestTimeAsString ();
		timeTrialStar.sprite = currentLevel.TimeTrialHasBeenCompleted () ? collectedStar : uncollectedStar;

		//supply crates
		supplyCratesFound.text = currentLevel.GetNumCollectablesFoundOutOfTotal ();
		supplyCratesStar.sprite = currentLevel.AllCollectablesHaveBeenCollected () ? collectedStar : uncollectedStar;

		//golden ball
		goldenBallFound.text = currentLevel.GetGoldenBallFoundAsString ();
		goldenBallStar.sprite = currentLevel.GoldenBallHasBeenCollected () ? collectedStar : uncollectedStar;

		//nav buttons
		nextLevel = LevelDataManager.manager.GetNextLevelData (currentLevel);
		previousLevel = LevelDataManager.manager.GetPreviousLevelData (currentLevel);
		nextButton.interactable = nextLevel != null ? true : false;
		previousButton.interactable = previousLevel != null ? true : false;

	}
}
