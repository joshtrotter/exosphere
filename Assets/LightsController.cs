using UnityEngine;
using System.Collections;

public class LightsController : MonoBehaviour {

	private Light ballLight;
	
	void Awake () {
		ballLight = transform.GetComponentInChildren<Light> ();
	}
	
	public void TurnLightOn() {
		ballLight.enabled = true;
	}

	public void TurnLightOff() {
		ballLight.enabled = false;
	}
}
