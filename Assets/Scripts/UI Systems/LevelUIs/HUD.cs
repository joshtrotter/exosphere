using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : UISystem {

	public static HUD controller;

	private Canvas HUDCanvas;
	[HideInInspector]
	public bool hidden;

	public override void Awake()
	{
		//set up singleton instance
		if (controller == null) {
			controller = this;
			DontDestroyOnLoad (this);
		} else if (controller != this) {
			Destroy(gameObject);
		}
		HUDCanvas = GetComponentInChildren<Canvas> ();
		base.Awake ();
	}

	public override void Show()
	{
		Debug.Log (gameObject.GetInstanceID () + " showing HUD");
		hidden = false;
		HUDCanvas.gameObject.SetActive (true);
	}

	public override void Hide()
	{
		Debug.Log (gameObject.GetInstanceID () + " hiding HUD");
		hidden = true;
		HUDCanvas.gameObject.SetActive (false);
	}

	public override void Deregister(){
		UISystemController.controller.Deregister (this);
		TutorialMessageController.controller.CloseMessage ();
		Hide ();
	}

	public override void BackKey(){
		PauseMenu.controller.Pause ();
	}
}
