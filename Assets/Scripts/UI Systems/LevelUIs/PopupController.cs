using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class PopupController : MonoBehaviour {

	public static PopupController controller;

	public Text popupText;
	public float fadeInTime = 1f;
	public float displayTime = 3f;
	public float fadeOutTIme = 1f;

	void Awake(){
		controller = this;
		Color invisible = popupText.color;
		invisible.a = 0;
		popupText.color = invisible;
	}

	void OnLevelWasLoaded(){
		if (Application.loadedLevel != 0) {
			StartCoroutine (WaitToBeShown (LevelDataManager.manager.GetCurrentLevelData().GetLevelName()));
		}
	}

	public void CollectableFound(){
		StartCoroutine(WaitToBeShown("Supply Crate Found (" + LevelManager.manager.GetNumCollectablesFound() + ")"));
	}
	
	public void CheckpointReached(){
		StartCoroutine(WaitToBeShown("Checkpoint Reached"));
	}

	public void MorphApplied(BallTransform morph){
		if (morph.morphName != null) {
			StartCoroutine(WaitToBeShown(morph.morphName));
		}
	}

	public void PickupAcquired(Pickup pickup) {
		if (pickup.GetDisplayName () != null && !"".Equals(pickup.GetDisplayName())) {
			StartCoroutine (WaitToBeShown (pickup.GetDisplayName ()));
		}
	}

	private IEnumerator WaitToBeShown(string popup){
		while (HUD.controller.hidden) {
			yield return new WaitForEndOfFrame();
		}
		DisplayPopup (popup);
	}

	private void DisplayPopup (string popup){
		popupText.text = popup;
		DOTween.Sequence ()
			.Append (popupText.DOFade (1, fadeInTime))
			.AppendInterval (displayTime)
			.Append (popupText.DOFade (0, fadeOutTIme));

	}

}
