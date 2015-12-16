using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	public bool IsOn = true;
	//Switch is on when the lightcone is green, off when it orange

	//store a reference to the object to be activated by the switch
	public SwitchableObject target;
	
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
	
	public void SwapState(){
		if (IsOn) {
			TurnOff();
		} else {
			TurnOn();
		}
	}

	public void TurnOn(){
		LightCone.Play ();
		IsOn = true;
	}

	public void TurnOff(){
		LightCone.Clear ();
		LightCone.Stop ();
		IsOn = false;
	}

}
