using UnityEngine;
using System.Collections;

public class LaserSwitchToggle : Switch {

	//store a reference to the emitting material on the laser switch
	private Material glow;
	//store the initial 'On' emission color
	private Color startColor;

	void Awake () {
		glow = GetComponent<Renderer> ().material;
		startColor = glow.GetColor ("_EmissionColor");
		IsOn = !IsOn;
		SwapState ();
	}

	public override void OnLaserEnter(){
		target.Activate ();
		SwapState ();
	}

	public override void OnLaserExit(){
	}

	public override void TurnOn(){
		SetEmission (startColor);
		IsOn = true;
	}
	
	public override void TurnOff(){
		SetEmission (Color.black);
		IsOn = false;
	}

	//this function changes the emission color of the switch
	public void SetEmission(Color color){
		glow.SetColor ("_EmissionColor", color);
	}
}
