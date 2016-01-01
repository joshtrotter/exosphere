using UnityEngine;
using System.Collections;

public class FloorSwitchPermanent : Switch {

	
	private ParticleSystem LightConeOn;
	private bool used = false;
	
	public Color OnColor;
	
	void Awake(){
		//set up lightcones
		ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem particle in particles) {
			if (particle.name == "LightConeOn"){
				LightConeOn = particle;
			}
		}
		LightConeOn.GetComponent<Renderer> ().material.SetColor ("_Color", OnColor);
		//will always start on
		TurnConeOn ();
	}
	
	void OnTriggerEnter (Collider coll) {
		//TODO consider whether objects other than the player can activate the switch
		if (coll.CompareTag("Player")) {
			if (!used){ 
				SwapState();
			}
		}
	}
	
	public override void TurnOn(){
		base.TurnOn ();
		TurnConeOn ();
	}
	
	public override void TurnOff(){
		base.TurnOff ();
		TurnConeOff ();
		used = true; //set used flag to prevent further switching
	}

	private void TurnConeOn() {
		//turn on the On coloured cone
		LightConeOn.Play ();
	}
	
	private void TurnConeOff() {
		//turn off the On coloured cone
		LightConeOn.Clear ();
		LightConeOn.Stop ();
	}

}
