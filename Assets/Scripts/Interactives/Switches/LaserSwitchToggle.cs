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
		offColor = onColor * Config.hardDimIntensity;
		glow = laserSink.GetComponent<Renderer> ().material;

		SetEmission (currentState == ON_STATE ? onColor : offColor);
	}

	public override void OnLaserEnter(){
		TurnOn ();
	}

	public override void OnLaserExit(){
		TurnOff();
	}

	public override void TurnOn(){
		base.TurnOn ();
		SetEmission (onColor);
	}
	
	public override void TurnOff(){
		base.TurnOff ();
		SetEmission (offColor);
	}

	//this function changes the emission color of the switch
	public void SetEmission(Color color){
		glow.SetColor ("_EmissionColor", color);
	}
}
