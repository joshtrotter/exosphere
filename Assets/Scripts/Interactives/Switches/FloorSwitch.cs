using UnityEngine;
using System.Collections;

public class FloorSwitch : Switch {

	//Switch is on when the lightcone is green, off when it orange

	private ParticleSystem LightCone;

	void Awake(){
		LightCone = GetComponentInChildren<ParticleSystem> ();
		IsOn = !IsOn;
		SwapState ();
	}

	void OnTriggerEnter (Collider coll) {
		//TODO consider whether objects other than the player can activate the switch
		if (coll.CompareTag("Player")) {
			target.Activate ();
			SwapState();
		}
	}

	public override void TurnOn(){
		LightCone.Play ();
		IsOn = true;
	}

	public override void TurnOff(){
		LightCone.Clear ();
		LightCone.Stop ();
		IsOn = false;
	}

}
