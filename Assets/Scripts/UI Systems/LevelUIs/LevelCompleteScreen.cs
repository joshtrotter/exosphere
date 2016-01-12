using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelCompleteScreen : UISystem {

	public static LevelCompleteScreen controller;

	public Text timeText;
	public Text collectablesText;
	public Text challengeText;
	public Text goldenBallText;

	public Text[] textComponents;

	public Canvas levelCompleteCanvas;

	private float levelTimer;
	private LevelManager levelManager;

	public override void Awake(){
		controller = this;
		base.Awake ();
	}

	public void LevelComplete(float time){
		levelTimer = time;
		RequestToBeShown ();
	}
		
	public override void Show(){
		//update save data
		LevelData levelData = LevelDataManager.manager.GetCurrentLevelData ();
		levelData.Complete ();
		levelData.SetNumCollectablesFound (LevelManager.manager.collected);
		LevelDataManager.manager.Save ();

		timeText.text = levelTimer + "s";
		collectablesText.text = LevelManager.manager.GetNumCollectablesFound ();
		levelCompleteCanvas.gameObject.SetActive (true);
		StartCoroutine (SlowReveal ());
	}

	//TODO remove
	public void BackToMenu(){
		Deregister ();
		HUD.controller.Deregister ();
	}

	public override void Hide(){
		foreach (Text text in textComponents) {
			text.gameObject.SetActive(false);
		}
		levelCompleteCanvas.gameObject.SetActive (false);
	}

	private IEnumerator SlowReveal(){
		foreach (Text text in textComponents) {
			yield return new WaitForSeconds(0.5f);
			text.gameObject.SetActive(true);
		}
	}
}
