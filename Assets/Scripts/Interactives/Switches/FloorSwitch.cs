using UnityEngine;
using System.Collections;

public class FloorSwitch : Switch {

	private ParticleSystem LightConeOn;
	private ParticleSystem LightConeOff;

	public Color OnColor;
	public Color OffColor;

	void Awake(){
		//set up lightcones
		ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem particle in particles) {
			if (particle.name == "LightConeOn"){
				LightConeOn = particle;
			} else if (particle.name == "LightConeOff") {
				LightConeOff = particle;
			}
		}

		LightConeOn.GetComponent<Renderer> ().material.SetColor ("_Color", OnColor);
		LightConeOff.GetComponent<Renderer> ().material.SetColor ("_Color", OffColor);

		if (currentState == ON_STATE) {
			TurnConeOn();
		} else {
			TurnConeOff();
		}
	}

	void OnTriggerEnter (Collider coll) {
		//TODO consider whether objects other than the player can activate the switch
		if (coll.CompareTag("Player")) {
			SwapState();
		}
	}

	public override void TurnOn(){
		base.TurnOn ();
		TurnConeOn ();
	}

	public override void TurnOff() {
		base.TurnOff ();
		TurnConeOff ();
	}

	private void TurnConeOn() {
		//turn off the Off coloured cone
		LightConeOff.Clear ();
		LightConeOff.Stop ();
		//turn on the On coloured cone
		LightConeOn.Play ();
	}

	private void TurnConeOff() {
		//turn off the On coloured cone
		LightConeOn.Clear ();
		LightConeOn.Stop ();
		//turn on the Off coloured cone
		LightConeOff.Play ();
	}

}
