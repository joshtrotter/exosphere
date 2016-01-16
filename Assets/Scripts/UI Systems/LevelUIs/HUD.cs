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
		controller = this;
		HUDCanvas = GetComponentInChildren<Canvas> ();
		base.Awake ();
	}

	public override void Show()
	{
		hidden = false;
		HUDCanvas.gameObject.SetActive (true);
	}

	public override void Hide()
	{
		hidden = true;
		HUDCanvas.gameObject.SetActive (false);
	}
}
