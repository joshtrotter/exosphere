using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class SettingsMenu : UISystem {

	public static SettingsMenu controller;

	public GameObject dropPanel;

	public override void Awake(){
		//set up singleton instance
		if (controller == null) {
			controller = this;
			DontDestroyOnLoad (this);
		} else if (controller != this) {
			Destroy(gameObject);
		}
		base.Awake ();
		dropPanel.transform.DOLocalMoveY ((Screen.height * 1.2f), 0).Play ();
		//RequestToBeShown ();
	}

	public override void Show ()
	{
		Time.timeScale = 0f;
		dropPanel.SetActive (true);
		dropPanel.transform.DOLocalMoveY (0, 0.5f).SetUpdate(true).Play ();
	}

	public override void Hide ()
	{
		//Time.timeScale = 1f;
		dropPanel.SetActive (false);
	}

	public override void BackKey ()
	{
		dropPanel.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).SetUpdate(true).OnComplete(Deregister).Play ();
	}

}
