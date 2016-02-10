using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class LevelCompleteScreen : UISystem {

	public static LevelCompleteScreen controller;

	public GameObject dropPanel;

	public Text supplyCrateTitleText;
	public Text cratesFoundText;
	public Text goldenBallFoundText;
	public Text timeTrialUnlockedText;
	public Text newLevelUnlockedText;
	public Text cratesStarEarned;
	public Text cratesNewBest;
	public Text goldenBallStarEarned;

	private List<Text> displayList = new List<Text> ();

	private int buttonPress;

	public Canvas levelCompleteCanvas;

	private float levelTimer;
	//private LevelManager levelManager;

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
		SetAllInactive ();
	}

	private void SetAllInactive ()
	{
		supplyCrateTitleText.gameObject.SetActive (false);
		cratesFoundText.gameObject.SetActive (false);
		goldenBallFoundText.gameObject.SetActive (false);
		timeTrialUnlockedText.gameObject.SetActive (false);
		newLevelUnlockedText.gameObject.SetActive (false);
		cratesStarEarned.gameObject.SetActive (false);
		cratesNewBest.gameObject.SetActive (false);
		goldenBallStarEarned.gameObject.SetActive (false);
	}

	public void LevelComplete(float time){
		levelTimer = time;
		LevelData levelData = LevelDataManager.manager.GetCurrentLevelData ();
		
		displayList.Clear ();

		//set up summary and update save data
		//crates
		AddToDisplayList (supplyCrateTitleText);
		cratesFoundText.text = LevelManager.manager.GetNumCollectablesFound ();
		AddToDisplayList (cratesFoundText);
		
		//goldenball
		goldenBallFoundText.text = "Golden Ball " + levelData.GetGoldenBallFoundAsString ();
		AddToDisplayList(goldenBallFoundText);
		
		//unlocks
		if (!levelData.HasBeenCompleted()) {
			levelData.Complete ();
			AddToDisplayList(timeTrialUnlockedText);
		}

		//stars and records
		if (levelData.GetNumCollectablesFound() < LevelManager.manager.collected) {
			levelData.SetNumCollectablesFound (LevelManager.manager.collected);
			AddToDisplayList(cratesNewBest);
			if (levelData.AllCollectablesHaveBeenCollected()){
				AddToDisplayList(cratesStarEarned);
			}
		}
		
		if (!levelData.GoldenBallHasBeenCollected() && LevelManager.manager.goldenBallFound){
			levelData.SetGoldenBallCollected();
			AddToDisplayList(goldenBallStarEarned);
		}

		//check for new level unlocks
		//unlock next level if it isn't already
		LevelData nextLevel = LevelDataManager.manager.GetNextLevelData();
		if (nextLevel != null && !nextLevel.IsUnlocked ()) {
			Debug.Log ("Unlocking " + nextLevel.GetLevelName());
			nextLevel.Unlock ();
			AddToDisplayList (newLevelUnlockedText);
		} else { //if the next level is already unlocked, it is possible the player has earnt enough stars to unlock a level even further ahead
			if (LevelDataManager.manager.CheckForNewStarUnlocks() > 0){
				AddToDisplayList (newLevelUnlockedText);
				Debug.Log ("Unlocked new level based on stars");
			}
		}
		
		LevelDataManager.manager.Save ();

		RequestToBeShown ();
	}
		
	public override void Show(){
		levelCompleteCanvas.gameObject.SetActive (true);
		dropPanel.transform.DOLocalMoveY (0, 0.5f).Play ();
		StartCoroutine (RevealSummary ());
	}

	private void AddToDisplayList(Text text){
		//set invisible
		Color color = text.color;
		color.a = 0f;
		text.color = color;
		//enable
		text.gameObject.SetActive (true);
		//add to list
		displayList.Add (text);
	}
	public override void Hide(){
		//make everything invisible to allow step by step reveal on next show
		SetAllInactive ();
		levelCompleteCanvas.gameObject.SetActive (false);
	}

	private IEnumerator RevealSummary(){
		yield return new WaitForSeconds(1.5f);
		foreach (Text text in displayList) {
			text.color = Color.white;
			yield return new WaitForSeconds(1f);
		}
	}

	//functions to deal with button presses and smoothly closing the level complete screen;
	
	public void ButtonPress(int button){
		buttonPress = button;
		dropPanel.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).OnComplete (CallButtonFunction).Play ();
	}
	
	private void CallButtonFunction(){
		Deregister ();
		switch (buttonPress) {
		case 0:
			BackToMenu ();
			break;
		case 1:
			ReplayLevel ();
			break;
		case 2:
			NextLevel ();
			break;
		}
	}
	
	private void BackToMenu(){
		Debug.Log ("Loading level loader from level complete screen");
		Application.LoadLevel (0);
		MainMenuController.controller.ReturnFocusToWorldLevels ();
	}
	
	private void ReplayLevel(){
		Debug.Log ("Restarting level");
		LevelManager.manager.FirstLoadLevel ();
	}
	
	private void NextLevel(){
		Debug.Log ("Loading level loader from level complete screen");
		LevelData nextLevel = LevelDataManager.manager.GetNextLevelData ();
		Application.LoadLevel (0);
		if (nextLevel != null) {
			MainMenuController.controller.ReturnFocusToNextLevel ();
		} else {
			MainMenuController.controller.ReturnFocusToNextWorld ();
		}
	}

	public override void BackKey(){
		ButtonPress (0);
	}
	

}
