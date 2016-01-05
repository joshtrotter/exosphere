using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : UISystem {

	public GameObject unpauseButton;
	public GameObject pauseMenu;
	public CallibrationUI callibrator;
	private CanvasGroup canvasGroup;

	public override void Awake(){
		base.Awake ();
		unpauseButton.SetActive (false);
		pauseMenu.SetActive (false);
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
}
