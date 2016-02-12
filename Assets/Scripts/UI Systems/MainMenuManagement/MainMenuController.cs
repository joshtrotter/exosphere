﻿using UnityEngine;
using System.Collections;

public class MainMenuController : UISystem {

	public static MainMenuController controller;

	public GameObject mainMenuSystem;
	private LevelSelectManager levelSelectManager;
	private WorldSelectManager worldSelectManager;

	public override void Awake(){
		//set up singleton instance
		Debug.Log ("MainMenuController awake, instance ID: " + GetInstanceID());
		if (controller == null) {
			Debug.Log ("static variable is null");
			controller = this;
			DontDestroyOnLoad (this);
			base.Awake ();

			levelSelectManager = mainMenuSystem.GetComponent<LevelSelectManager>();
			worldSelectManager = mainMenuSystem.GetComponent<WorldSelectManager>();
			OnLevelWasLoaded ();

		} else if (controller != this) {
			Debug.Log ("Destroying");
			Destroy(gameObject);
		}


	}
	
	private void OnLevelWasLoaded(){
		if (controller == this) {
			if (Application.loadedLevel == 0) {
				SetSkybox ();
				RequestToBeShown ();
			} else {
				Deregister ();
			}
		}
	}

	//TODO remove. Causes the game to skip to the troposphere level select menu when started
	void Start(){
		if (Application.loadedLevel == 0 && controller == this) {
			worldSelectManager.EnterWorld (LevelDataManager.manager.GetWorldData (1));
		}
	}

	public override void Show(){
		mainMenuSystem.SetActive (true);
	}

	public override void Hide(){
		mainMenuSystem.SetActive (false);
	}

	public override void BackKey(){
		worldSelectManager.BackButton ();
		levelSelectManager.BackButton ();
	}

	private void SetSkybox(){
		Debug.Log ("Setting skybox for instance " + GetInstanceID());
		WorldData world = levelSelectManager.GetCurrentWorld ();
		if (world != null)
			RenderSettings.skybox = world.skybox;
	}

	public void ReturnFocusToMainMenu(){
		Debug.Log ("returning focus to main menu");
		worldSelectManager.ReturnToOpeningScreen ();
	}

	public void ReturnFocusToWorldLevels(){
		Debug.Log ("Returning focus to world levels");
		levelSelectManager.StartWorldLevelsDisplay (levelSelectManager.GetCurrentWorld());
	}

	public void ReturnFocusToNextLevel(){
		Debug.Log ("returning focus to next level");
		levelSelectManager.StartWorldLevelsDisplay (levelSelectManager.GetCurrentWorld());
		levelSelectManager.StartLevelInfoDisplay (LevelDataManager.manager.GetNextLevelData().GetLevelID());
	}


	public void ReturnFocusToNextWorld(){
		Debug.Log ("Returning focus to next world");
		levelSelectManager.ReturnToWorldSelect ();
		worldSelectManager.MoveScreenToWorld (1);
	}

}