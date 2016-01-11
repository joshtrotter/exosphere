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

	public void CollectableFound(){
		DisplayPopup ("Supply Crate Collected (" + LevelManager.manager.GetNumCollectablesFound() + ")");
	}
	
	public void CheckpointReached(){
		DisplayPopup ("Checkpoint Reached");
	}

	public void MorphApplied(BallTransform morph){
		if (morph.morphName != null) {
			DisplayPopup (morph.morphName);
		}
	}

	private void DisplayPopup(string popup){
		//DOTween.Init ();
		popupText.text = popup;
		DOTween.Sequence ()
			.Append (popupText.DOFade (1, fadeInTime))
			.AppendInterval (displayTime)
			.Append (popupText.DOFade (0, fadeOutTIme));
		//popupText.rectTransform.parent.GetComponent<RectTransform>().DOSizeDelta(new Vector2(0,0), 1).From ().Play ();
	}

}
