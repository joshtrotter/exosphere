using UnityEngine;
using System.Collections;

public class LevelInfo : UISystem {

	private Canvas canvas;

	public override void Awake(){
		canvas = GetComponentInChildren<Canvas> ();
		base.Awake ();

	}

	public override void Show(){
		canvas.gameObject.SetActive (true);

	}

	public override void Hide(){
		canvas.gameObject.SetActive (false);
	}
}
