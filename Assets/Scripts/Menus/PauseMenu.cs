using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseButton;
	public GameObject unpauseButton;
	public GameObject pauseMenu;
	public CallibrationUI callibrator;

	void Awake(){
		unpauseButton.SetActive (false);
		pauseMenu.SetActive (false);
	}

	public void Pause()
	{
		pauseButton.SetActive (false);
		unpauseButton.SetActive (true);
		pauseMenu.SetActive (true);
		Time.timeScale = 0f;
	}

	public void Unpause()
	{
		pauseButton.SetActive (true);
		unpauseButton.SetActive (false);
		pauseMenu.SetActive (false);
		Time.timeScale = 1f;
	}

	public void Recallibrate(){
		Unpause ();
		callibrator.StartCalibration ();
	}
}
