using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* this class displays a summary of all info about a level and provides access to the Play button; 
 */
public class LevelInfo : MonoBehaviour {

	private Canvas canvas;

	//a reference to the level we are currently displaying
	private LevelData currentLevel;

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

	//store pointer locations to interpret directions of swipes
	private Vector3 startPos;
	private Vector3 endPos;

	public void Start(){
		canvas = GetComponentInChildren<Canvas> ();
		//little bit hacky but we're just gonna roll with it
		canvas.renderMode = RenderMode.WorldSpace;
	}

	public void DisplayLevelInfo(LevelData newLevel){
		currentLevel = newLevel;
		SetLatestInfo ();
	}

	public LevelData GetCurrentLevel(){
		return currentLevel;
	}

	//tells the levelManager to load the currently displayed level
	public void PlayLevelButton(){
		if (GetComponentInParent<LevelSelectManager> ().IsSafeToPress ()) {
			LevelManager.manager.SetCurrentLevel (currentLevel.GetLevelID ());
			LevelManager.manager.ReloadLevel ();
		}
	}
	
	//updates all text fields and images with the latest data that has been saved
	private void SetLatestInfo(){
		//Debug.Log ("Displaying data for " + currentLevel.GetLevelName ());
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

	}
}