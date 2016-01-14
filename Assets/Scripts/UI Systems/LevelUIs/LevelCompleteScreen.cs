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
		if (LevelManager.manager.goldenBallFound) {
			AddToDisplayList(goldenBallFoundText);
		}
		
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
		GetComponentInChildren<TextGroupBestFit> ().FitText ();
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
	}

	public override void Hide(){
		//make everything invisible to allow step by step reveal on next show
		/*foreach (Text text in displayList) {
			text.gameObject.SetActive(false);
		}*/
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
