using UnityEngine;
using System.Collections;

public class MainMenuController : UISystem {

	public static MainMenuController controller;

	public GameObject mainMenuSystem;
	private LevelSelectManager levelSelectManager;
	private WorldSelectManager worldSelectManager;

	public delegate void SetMenuFocus();

	public SetMenuFocus setMenuFocus;

	public override void Awake(){
		//set up singleton instance
		if (controller == null) {
			controller = this;
			DontDestroyOnLoad (this);

			base.Awake ();
		} else if (controller != this) {
			Destroy(gameObject);
		}

		OnLevelWasLoaded ();
		levelSelectManager = mainMenuSystem.GetComponent<LevelSelectManager>();
		worldSelectManager = mainMenuSystem.GetComponent<WorldSelectManager>();
	}
	
	private void OnLevelWasLoaded(){
		if (Application.loadedLevel == 0) {
			if (setMenuFocus != null) setMenuFocus();
			RequestToBeShown ();
		} else {
			Deregister ();
		}
	}

	public override void Show(){
		mainMenuSystem.SetActive (true);
	}

	public override void Hide(){
		mainMenuSystem.SetActive (false);
	}

	public void ReturnFocusToMainMenu(){
		setMenuFocus = new SetMenuFocus(ReturnFocusToMainMenu);
		Debug.Log ("returning focus to main menu");
		worldSelectManager.ReturnToOpeningScreen ();
	}

	public void ReturnFocusToWorldLevels(){
		setMenuFocus = new SetMenuFocus(ReturnFocusToWorldLevels);
		Debug.Log ("Returning focus to world levels");
		levelSelectManager.StartWorldLevelsDisplay (levelSelectManager.GetCurrentWorld());
	}

	public void ReturnFocusToLevel(){
	}
}
