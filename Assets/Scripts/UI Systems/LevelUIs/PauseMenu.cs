using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class PauseMenu : UISystem {

	public static PauseMenu controller;

	public GameObject unpauseButton;
	public GameObject pauseMenu;
	public CallibrationUI callibrator;
	public GameObject confirmationPanel;
	public Text confirmationText;
	public GameObject levelSelectButton; 
	//private CanvasGroup canvasGroup;
	private delegate void confirmationContinue();
	private confirmationContinue cont;

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
		Time.timeScale = 1f;
		pauseMenu.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).SetUpdate (true).OnComplete (Deregister).Play ();
		confirmationPanel.SetActive (false);
	}

	public override void Show ()
	{
		if (LevelManager.manager.IsTunnelRunner ()) {
			levelSelectButton.SetActive (false);
		} else {
			levelSelectButton.SetActive(true);
		}
		Time.timeScale = 0f;
		unpauseButton.SetActive (true);
		pauseMenu.transform.DOLocalMoveY ((Screen.height * 1.2f), 0).Play ();
		pauseMenu.SetActive (true);
		pauseMenu.transform.DOLocalMoveY (0, 0.5f).SetUpdate (true).Play ();
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
		cont = ContinueToMainMenu;
		DisplayConfirmWindow ("Return to main menu? Your progress will be lost");
	}


	public void LevelSelect(){
		cont = ContinueToLevelSelect;
		DisplayConfirmWindow ("Return to level select? Your progress will be lost");
	}

	public void RestartLevel(){
		cont = ContinueToRestartLevel;
		DisplayConfirmWindow ("Are you sure you want to restart the level?");
	}

	private void ContinueToMainMenu(){
		Deregister ();
		Debug.Log ("Loading level loader from PauseMenu");
		Application.LoadLevel (0);
		MainMenuController.controller.ReturnFocusToMainMenu ();
	}

	private void ContinueToLevelSelect(){
		Deregister ();
		Debug.Log ("Loading level loader from PauseMenu");
		Application.LoadLevel (0);
		MainMenuController.controller.ReturnFocusToWorldLevels ();
	}
	
	private void ContinueToRestartLevel(){
		Deregister ();
		LevelManager.manager.FirstLoadLevel ();
	}

	private void DisplayConfirmWindow(string text){
		pauseMenu.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).SetUpdate (true).Play ();
		confirmationPanel.transform.DOLocalMoveY (-(Screen.height * 1.2f), 0).SetUpdate (true).Play ();
		confirmationPanel.SetActive (true);
		confirmationPanel.transform.DOLocalMoveY (0, 0.5f).SetUpdate (true).Play ();
		confirmationText.text = text;
	}

	public void Confirm(){
		confirmationPanel.transform.DOLocalMoveY(-(Screen.height * 1.2f), 0.5f).SetUpdate (true).Play ().OnComplete(Continue);
	}

	public void Continue(){
		cont();
	}

	public void Cancel(){
		confirmationPanel.transform.DOLocalMoveY (-(Screen.height * 1.2f), 0.5f).SetUpdate (true).Play ();
		pauseMenu.transform.DOLocalMoveY (0, 0.5f).SetUpdate (true).Play ();
	}


	public override void BackKey(){
		Unpause ();
	}
}
