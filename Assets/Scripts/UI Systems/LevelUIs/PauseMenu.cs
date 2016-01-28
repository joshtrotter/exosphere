using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : UISystem {

	public static PauseMenu controller;

	public GameObject unpauseButton;
	public GameObject pauseMenu;
	public CallibrationUI callibrator;
	private CanvasGroup canvasGroup;

	public override void Awake(){
		//set up singleton instance
		if (controller == null) {
			controller = this;
			DontDestroyOnLoad (this);
		} else if (controller != this) {
			Destroy(gameObject);
		}
		base.Awake ();
	}

	public void Pause()
	{
		RequestToBeShown ();
	}

	public void Unpause()
	{
		Deregister ();
		Hide ();
	}

	public override void Show ()
	{
		Time.timeScale = 0f;
		unpauseButton.SetActive (true);
		pauseMenu.SetActive (true);
	}

	public override void Hide ()
	{
		Time.timeScale = 1f;
		unpauseButton.SetActive (false);
		pauseMenu.SetActive (false);
	}

	public void Recallibrate(){
		Unpause ();
		callibrator.RequestToBeShown ();
	}

	public void MainMenu(){
		Deregister ();
		Debug.Log ("Loading level loader from PauseMenu");
		Application.LoadLevel (0);
		MainMenuController.controller.ReturnFocusToMainMenu ();
	}

	public void LevelSelect(){
		Deregister ();
		Debug.Log ("Loading level loader from PauseMenu");
		Application.LoadLevel (0);
		MainMenuController.controller.ReturnFocusToWorldLevels ();
	}
}
