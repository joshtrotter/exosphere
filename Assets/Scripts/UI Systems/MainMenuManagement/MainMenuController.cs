using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : UISystem {

	public static MainMenuController controller;

	public GameObject mainMenuSystem;
	private LevelSelectManager levelSelectManager;
	private WorldSelectManager worldSelectManager;

	//used to determine whether a press of the back button should be passed to the levelSelectManager
	private bool hasLaunched = false;
	private bool hasPressedPlay = false;
	private bool inSettings = false;

	public override void Awake(){
		//set up singleton instance
		if (controller == null) {
			controller = this;
			DontDestroyOnLoad (this);
			base.Awake ();

			levelSelectManager = mainMenuSystem.GetComponent<LevelSelectManager>();
			worldSelectManager = mainMenuSystem.GetComponent<WorldSelectManager>();

			OnLevelWasLoaded ();

		} else if (controller != this) {
			Destroy(gameObject);
		}
	}
	
	private void OnLevelWasLoaded(){
		if (controller == this) {
			if (LevelManager.manager.IsLevelLoader()) {
				hasPressedPlay = false;
				SetSkybox ();
				RequestToBeShown ();
			} else {
				Deregister ();
			}
		}
	}

	public void TunnelRunnerButton(){
		worldSelectManager.InitiateTunnelRunnerLaunch ();
	}

	public void SettingsButton(){
		inSettings = true;
		worldSelectManager.OpenSettingsMenu ();
	}


	public void Launch(){
		hasLaunched = true;
		worldSelectManager.EnterWorld (LevelDataManager.manager.GetWorldData (1));
	}

	public override void Show(){
		mainMenuSystem.SetActive (true);
		if (inSettings) {
			worldSelectManager.ReturnFromSettingsMenu();
			inSettings = false;
		}
	}

	public override void Hide(){
		mainMenuSystem.SetActive (false);
	}

	public override void BackKey(){
		//TODO disabled while world select is skipped
		//worldSelectManager.BackButton ();
		if (hasLaunched && !hasPressedPlay) {
			levelSelectManager.BackButton ();
		}
	}

	private void SetSkybox(){
		Debug.Log ("Setting skybox for instance " + GetInstanceID());
		WorldData world = levelSelectManager.GetCurrentWorld ();
		if (world != null)
			RenderSettings.skybox = world.skybox;
	}

	public void ReturnFocusToMainMenu(){
		Debug.Log ("returning focus to main menu");
		hasLaunched = false;
		worldSelectManager.ReturnToOpeningScreen ();
		levelSelectManager.ResetCamera ();
	}

	public void ReturnFocusToWorldLevels(){
		Debug.Log ("Returning focus to world levels");
		hasLaunched = true;
		levelSelectManager.StartWorldLevelsDisplay (levelSelectManager.GetCurrentWorld());
	}

	public void ReturnFocusToNextLevel(){
		Debug.Log ("returning focus to next level");
		hasLaunched = true;
		levelSelectManager.StartWorldLevelsDisplay (levelSelectManager.GetCurrentWorld());
		levelSelectManager.StartLevelInfoDisplay (LevelDataManager.manager.GetNextLevelData().GetLevelID());
	}


	public void ReturnFocusToNextWorld(){
		Debug.Log ("Returning focus to next world");
		hasLaunched = true;
		levelSelectManager.ReturnToWorldSelect ();
		worldSelectManager.MoveScreenToWorld (1);
	}

	public void BlockBackButton(){
		hasPressedPlay = true;
	}

}
