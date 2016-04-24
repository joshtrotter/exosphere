﻿using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class PauseMenu : UISystem {

	public static PauseMenu controller;

	public GameObject unpauseButton;
	public GameObject pauseMenu;
	public CallibrationUI callibrator;
	//private CanvasGroup canvasGroup;

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
	}

	public override void Show ()
	{
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
		pauseMenu.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).SetUpdate (true).OnComplete (ContinueToMainMenu).Play ();
	}

	private void ContinueToMainMenu(){
		Deregister ();
		Debug.Log ("Loading level loader from PauseMenu");
		Application.LoadLevel (0);
		MainMenuController.controller.ReturnFocusToMainMenu ();
	}

	public void LevelSelect(){
		pauseMenu.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).SetUpdate (true).OnComplete (ContinueToLevelSelect).Play ();
	}

	public void RestartLevel(){
		pauseMenu.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).SetUpdate (true).OnComplete (ContinueToRestartLevel).Play ();
	}

	private void ContinueToRestartLevel(){
		Deregister ();
		LevelManager.manager.FirstLoadLevel ();
	}

	private void ContinueToLevelSelect(){
		Deregister ();
		Debug.Log ("Loading level loader from PauseMenu");
		Application.LoadLevel (0);
		MainMenuController.controller.ReturnFocusToWorldLevels ();
	}

	public override void BackKey(){
		Unpause ();
	}
}
