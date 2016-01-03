using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseButton;
	public GameObject unpauseButton;
	public GameObject pauseMenu;
	public CallibrationUI callibrator;
	private CanvasGroup canvasGroup;

	void Awake(){
		unpauseButton.SetActive (false);
		pauseMenu.SetActive (false);
		/*canvasGroup = pauseMenu.GetComponent<CanvasGroup> ();
		canvasGroup.interactable = false;
		canvasGroup.alpha = 0;*/

	}

	public void Pause()
	{
		pauseButton.SetActive (false);
		unpauseButton.SetActive (true);
		pauseMenu.SetActive (true);
		/*canvasGroup.interactable = true;
		canvasGroup.alpha = 1;*/

		Time.timeScale = 0f;
	}

	public void Unpause()
	{
		pauseButton.SetActive (true);
		unpauseButton.SetActive (false);
		pauseMenu.SetActive (false);
		
		/*canvasGroup.interactable = false;
		canvasGroup.alpha = 0;*/
		Time.timeScale = 1f;
	}

	public void Recallibrate(){
		Unpause ();
		callibrator.StartCalibration ();
	}
}
