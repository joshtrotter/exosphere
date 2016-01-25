using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class WorldSelectManager : UISystem {

	public CanvasRenderer background;
	private CanvasGroup canvas;

	public override void Awake(){
		canvas = GetComponentInChildren<CanvasGroup> ();
		RequestToBeShown ();
	}

	public void Launch(){
		background.transform.DOLocalMove (new Vector3(0,-716,0), 1).Play ();
	}

	public void EnterWorld(WorldData world){
		LevelSelectManager.manager.StartWorldLevelsDisplay (world);
		Deregister ();
	}

	public override void Show(){
		canvas.alpha = 1;
		canvas.gameObject.SetActive (true);
	}

	public override void Hide(){
		canvas.DOFade (0, 1).Play ().OnComplete(Disable);
	}

	private void Disable(){
		canvas.gameObject.SetActive (false);
	}
}
