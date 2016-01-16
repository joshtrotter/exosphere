using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextGroupBestFit : MonoBehaviour {
	
	[SerializeField]
	private Text longestText;
	[SerializeField]
	private Text[] otherTexts;

	public void FitText () {
		StartCoroutine (DelayFitText ());
	}

	private IEnumerator DelayFitText(){
		yield return new WaitForEndOfFrame();

		int smallestFontSize = int.MaxValue;

		if (longestText != null) {
			smallestFontSize = longestText.cachedTextGenerator.fontSizeUsedForBestFit;
		} else {
			foreach (Text text in otherTexts) {
				smallestFontSize = Mathf.Min (text.cachedTextGenerator.fontSizeUsedForBestFit, smallestFontSize);
			}
		}

		foreach (Text text in otherTexts) {
			text.resizeTextForBestFit = false;
			text.fontSize = smallestFontSize;
		}

		Debug.Log (longestText.cachedTextGenerator.fontSizeUsedForBestFit);
	}
}
