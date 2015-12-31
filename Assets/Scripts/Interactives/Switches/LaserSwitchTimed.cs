using UnityEngine;
using System.Collections;

//this switch type takes four seconds to charge up and once on cannot be deactivated

public class LaserSwitchTimed : Switch {

	//store a reference to the emitting material on the main laser switch
	private Material glow;

	//store the initial 'On' emission color
	public Color OnColor;
	private Color OffColor;

	//store how long a full charge and discharge should take
	public float chargeTime = 4f;
	private float chargeTick;
	public float dischargeTime = 8f;
	private float dischargeTick;

	private bool charging = false;
	//track how charged the switch is from 0 to MaxChargeLevel (size of ChargeLights array)
	private int ChargeLevel = 0;
	//store references to the lights that turn on as switch is being charged
	public GameObject[] ChargeLights;
	public GameObject laserSink;
	private int MaxChargeLevel;
	
	void Awake () {
		//set offColor as dimmed version of onColor
		OffColor = OnColor * Config.dimIntensity;
		glow = laserSink.GetComponent<Renderer> ().material;
		//set the maxchargelevel to the number of charge lights in the array
		MaxChargeLevel = ChargeLights.Length;
		//set tick times based on charge times and number of lights in the array
		chargeTick = chargeTime / MaxChargeLevel;
		dischargeTick = dischargeTime / MaxChargeLevel;

		//ensure the switch is in the desired start state
		if (currentState == ON_STATE) {
			ChargeLevel = MaxChargeLevel;
			StartCoroutine (ChargeDown ());
		} else {
			SetEmissionAll (OffColor);
		}
	}
	
	public override void OnLaserEnter(){
		if (ChargeLevel != MaxChargeLevel) { //only charge up if not already fully charged
			charging = true;
			TurnOn ();
			StartCoroutine (ChargeUp ());
		}
	}
	
	public override void OnLaserExit(){
		charging = false;
		StartCoroutine (ChargeDown ());
	}

	public override void TurnOn(){
		//target is activated immediately if the switch was off
		if (currentState != ON_STATE) {
			base.TurnOn ();
			SetEmission (glow, OnColor);
		}
	}
	
	public override void TurnOff(){
		if (currentState == ON_STATE) {
			base.TurnOff();
			SetEmission (glow, OffColor);
		}
	}

	//this function changes the emission color of the given material
	public void SetEmission(Material mat, Color color){
		mat.SetColor ("_EmissionColor", color);
	}

	//this function changes the emission color of the switch and chargeLights
	public void SetEmissionAll(Color color){
		SetEmission (glow, color);
		foreach (GameObject light in ChargeLights){
			SetEmission(light.GetComponent<Renderer>().material, color);
		}
	}

	//whilst the laser is pointed at the switch this function will turn on one light per second
	//until the switch is fully charged
	private IEnumerator ChargeUp(){
		float timer = 0;
		while (charging) {
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;
			if (timer > chargeTick){ //increment charge level after each time period
				ChargeLevel += 1;
				timer = 0;
				SetEmission(ChargeLights[ChargeLevel-1].GetComponent<Renderer>().material, OnColor);
			}
			if (ChargeLevel == MaxChargeLevel){ //switch is fully on, stop charging activate target
				charging = false;
			}
		}
	}

	private IEnumerator ChargeDown(){
		float timer = 0;
		while (!charging && ChargeLevel > 0) {
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;
			if (timer > dischargeTick){ //decrement charge level after each time period
				ChargeLevel -= 1;
				timer = 0;
				SetEmission(ChargeLights[ChargeLevel].GetComponent<Renderer>().material, OffColor);
			}
		}
		if (ChargeLevel == 0){ //switch is fully uncharged, deactivate target
			TurnOff();
		}
	}
}
