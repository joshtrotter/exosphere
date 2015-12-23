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
		//turn off the Off coloured cone
		LightConeOff.Clear ();
		LightConeOff.Stop ();
		//turn on the On coloured cone
		LightConeOn.Play ();
		IsOn = true;
	}

	public override void TurnOff(){
		//turn off the On coloured cone
		LightConeOn.Clear ();
		LightConeOn.Stop ();
		//turn on the Off coloured cone
		LightConeOff.Play ();
		IsOn = false;
	}

}
