using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class TutorialShake : MonoBehaviour {

	public TutorialMessage.TutorialSwitchTuple tutorial;

	void Update () {
		if (CrossPlatformInputManager.GetButtonDown ("Shake")) {
			if (tutorial.tut.isReady){
				tutorial.tut.ExternalTriggerCall(tutorial.method);
			}
		}
	}
}
