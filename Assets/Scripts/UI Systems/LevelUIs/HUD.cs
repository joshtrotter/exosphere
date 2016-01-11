using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : UISystem {

	public static HUD controller;

	private Canvas HUDCanvas;

	public override void Awake()
	{
		controller = this;
		HUDCanvas = GetComponentInChildren<Canvas> ();
		base.Awake ();
		RequestToBeShown ();
	}

	public override void Show()
	{
		HUDCanvas.gameObject.SetActive (true);
	}

	public override void Hide()
	{
		HUDCanvas.gameObject.SetActive (false);
	}
}
