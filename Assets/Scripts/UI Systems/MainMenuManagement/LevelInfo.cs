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
	public Button playButton;

	public Image noDeathsCompletion;
	public Sprite noDeathsComplete;
	public Sprite noDeathsIncomplete;

	//the star images which are used to show whether or not a star has been collected
	public Image[] stars;
	public Sprite collectedStar;
	public Sprite uncollectedStar;

	public Image screenshot;

	//store pointer locations to interpret directions of swipes
	private Vector3 startPos;
	private Vector3 endPos;

	private LevelSelectManager levelSelectManager;

	public Camera worldCamera;

	public void Start(){
		canvas = GetComponentInChildren<Canvas> ();
		levelSelectManager = GetComponentInParent<LevelSelectManager> ();
		//little bit hacky but we're just gonna roll with it
		canvas.renderMode = RenderMode.WorldSpace;
		canvas.worldCamera = worldCamera;
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
		if (levelSelectManager.IsSafeToPress ()) {
			levelSelectManager.PlayLevel (currentLevel.GetLevelID());
		}
	}
	
	//updates all text fields and images with the latest data that has been saved
	private void SetLatestInfo(){
		//Debug.Log ("Displaying data for " + currentLevel.GetLevelName ());
		levelName.text = currentLevel.GetLevelName();
		screenshot.sprite = currentLevel.GetLevelScreenshot ();

		//completion
		levelCompletion.text = currentLevel.GetCompletionStatus ();
		playButton.interactable = currentLevel.IsUnlocked ();

		//supply crates
		supplyCratesFound.text = currentLevel.GetNumCollectablesFoundOutOfTotal ();

		//no deaths
		noDeathsCompletion.sprite = currentLevel.NoDeathsChallengeHasBeenCompleted () ? noDeathsComplete : noDeathsIncomplete;

		//set the right number of stars to the collected state
		int numStars = currentLevel.GetStarsEarned ();
		for (int i = 0; i < 3; i++) {
			if (i < numStars){
				stars[i].sprite = collectedStar;
			} else {
				stars[i].sprite = uncollectedStar;
			}
		}

	}
}