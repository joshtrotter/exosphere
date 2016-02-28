using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MorphHUDController : MonoBehaviour {

	public Button morphDisplay;

	void OnLevelWasLoaded (){
		MorphRemoved ();
	}

	public void MorphApplied(BallTransform morph){
		morphDisplay.gameObject.SetActive (true);
	}

	public void MorphRemoved(){
		morphDisplay.gameObject.SetActive (false);
	}

	//called by pressing the icon in the top left
	public void RemoveMorph(){
		//if (CrossPlatformInputManager.AxisExists ("Shake")) {
		CrossPlatformInputManager.SetButtonDown ("Shake");
		//}
	}
}
