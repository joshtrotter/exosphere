using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelCompleteScreen : UISystem {

	public static LevelCompleteScreen controller;

	public Text supplyCrateTitleText;
	public Text cratesFoundText;
	public Text goldenBallFoundText;
	public Text timeTrialUnlockedText;
	public Text newLevelUnlockedText;
	public Text cratesStarEarned;
	public Text cratesNewBest;
	public Text goldenBallStarEarned;

	private List<Text> displayList = new List<Text> ();

	public Canvas levelCompleteCanvas;

	private float levelTimer;
	//private LevelManager levelManager;

	public override void Awake(){
		controller = this;
		base.Awake ();
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
		AddToDisplayList(goldenBallFoundText);
		goldenBallFoundText.text = "Golden Ball " + levelData.GetGoldenBallFoundAsString ();
		
		//unlocks
		if (!levelData.HasBeenCompleted()) {
			levelData.Complete ();
			AddToDisplayList(timeTrialUnlockedText);
			AddToDisplayList(newLevelUnlockedText);
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
		
		LevelDataManager.manager.Save ();

		RequestToBeShown ();
	}
		
	public override void Show(){
		levelCompleteCanvas.gameObject.SetActive (true);
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

	//TODO remove
	public void BackToMenu(){
		HUD.controller.Deregister ();
		Deregister ();
		Application.LoadLevel (0);
	}

	public override void Hide(){
		//make everything invisible to allow step by step reveal on next show
		SetAllInactive ();
		levelCompleteCanvas.gameObject.SetActive (false);
	}

	private IEnumerator RevealSummary(){
		yield return new WaitForSeconds(1f);
		foreach (Text text in displayList) {
			text.color = Color.white;
			yield return new WaitForSeconds(1f);
		}
	}
}
