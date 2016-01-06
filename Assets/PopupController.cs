using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class PopupController : MonoBehaviour {

	public Text popupText;
	public float popUpTime = 3f;
	private int maxFontSize;

	void Awake(){
		maxFontSize = popupText.fontSize;
	}

	public void CollectableFound(){
		DisplayPopup ("Collectable Found");
	}
	
	public void CheckpointReached(){
		DisplayPopup ("Checkpoint Reached");
	}

	private void DisplayPopup(string popup){
		popupText.text = popup;
		RectTransform popupTransform = popupText.GetComponent<RectTransform> ();
		Vector2 targetScale = popupTransform.sizeDelta;
		popupTransform.sizeDelta = new Vector2 (0, 0);
		popupTransform.DOSizeDelta (targetScale, 3);
		popupTransform.DOSizeDelta (new Vector2(0,0), 3);

	}
}
