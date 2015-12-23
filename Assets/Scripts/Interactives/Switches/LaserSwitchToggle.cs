using UnityEngine;
using System.Collections;

public class LaserSwitchToggle : Switch {

	//store a reference to the emitting material on the laser switch
	private Material glow;
	//store the initial 'On' emission color
	public Color onColor;
	private Color offColor;

	public GameObject laserSink;

	void Awake () {
		offColor = onColor * Config.dimIntensity;
		glow = laserSink.GetComponent<Renderer> ().material;
		IsOn = !IsOn;
		SwapState ();
	}

	public override void OnLaserEnter(){
		target.Activate ();
		TurnOn ();
	}

	public override void OnLaserExit(){
		target.Activate ();
		TurnOff();
	}

	public override void TurnOn(){
		SetEmission (onColor);
		IsOn = true;
	}
	
	public override void TurnOff(){
		SetEmission (offColor);
		IsOn = false;
	}

	//this function changes the emission color of the switch
	public void SetEmission(Color color){
		glow.SetColor ("_EmissionColor", color);
	}
}
