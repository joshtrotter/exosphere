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
		TurnOn ();
	}
	
	void OnTriggerEnter (Collider coll) {
		//TODO consider whether objects other than the player can activate the switch
		if (coll.CompareTag("Player")) {
			if (!used){ 
				target.Activate ();
				SwapState();
			}
		}
	}
	
	public override void TurnOn(){
		//turn on the On coloured cone
		LightConeOn.Play ();
		IsOn = true;
	}
	
	public override void TurnOff(){
		//turn off the On coloured cone
		LightConeOn.Clear ();
		LightConeOn.Stop ();
		IsOn = false;
		used = true; //set used flag to prevent further switching
	}

}
