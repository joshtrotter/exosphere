using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TipManager : MonoBehaviour {

	public Text tipDisplay;
	public Tips tipList;

	void Awake(){
		RectTransform rt = GetComponent<RectTransform> ();
		rt.anchoredPosition = new Vector2 (0, rt.anchoredPosition.y + 2 * Screen.height);
	}

	void OnEnable(){
		tipDisplay.text = tipList.tips[Random.Range (0, tipList.tips.Length)];
	}
}
