using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : UISystem {

	private Canvas HUDCanvas;

	public override void Awake()
	{
		base.Awake ();
		HUDCanvas = GetComponentInChildren<Canvas> ();
	}

	void Start(){
		Hide ();
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
