using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class TutorialShake : MonoBehaviour {

	public TutorialMessage.TutorialSwitchTuple[] tutorials;

	void Update () {
		if (CrossPlatformInputManager.GetButtonDown ("Shake")) {
			foreach (TutorialMessage.TutorialSwitchTuple tutorial in tutorials){
				tutorial.tut.ExternalTriggerCall(tutorial.method);
			}
		}
	}
}
