using UnityEngine;
using System.Collections;

//this switch type takes four seconds to charge up and once on cannot be deactivated

public class LaserSwitchCharge : Switch {
	
	//store a reference to the emitting material on the main laser switch
	private Material glow;
	
	//store the initial 'On' emission color
	public Color onColor;
	private Color offColor;
	
	//store how long a full charge and discharge should take
	public float chargeTime = 2f;
	private float chargeTick;

	//track whether the switch is currently charging
	private bool charging = false;
	//store a reference to the laser sink object
	public GameObject laserSink;
	//track how charged the switch is from 0 to MaxChargeLevel (size of ChargeLights array)
	private int ChargeLevel = 0;
	//store references to the lights that turn on as switch is being charged
	public GameObject[] ChargeLights;
	private int MaxChargeLevel;
	
	void Awake () {
		offColor = onColor * Config.dimIntensity;
		glow = laserSink.GetComponent<Renderer>().material;
		//set the maxchargelevel to the number of charge lights in the array
		MaxChargeLevel = ChargeLights.Length;
		//set tick times based on charge times and number of lights in the array
		chargeTick = chargeTime / MaxChargeLevel;

		if (currentState == ON_STATE) {
			ChargeLevel = MaxChargeLevel;
		} else {
			SetEmissionAll (offColor);
		}
	}
	
	public override void OnLaserEnter(){
		if (ChargeLevel != MaxChargeLevel) { //only charge up if not already fully charged
			charging = true;
			StartCoroutine (ChargeUp ());
		}
	}
	
	public override void OnLaserExit(){
		charging = false;
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
		SetEmission (glow, onColor);
		float timer = 0;
		while (charging) {
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;
			if (timer > chargeTick){ //increment charge level after each second
				ChargeLevel += 1;
				timer = 0;
				SetEmission(ChargeLights[ChargeLevel-1].GetComponent<Renderer>().material, onColor);
			}
			if (ChargeLevel == MaxChargeLevel){ //switch is fully on, stop charging activate target
				charging = false;
				TurnOn();
			}
		}
		if (ChargeLevel != MaxChargeLevel) { //charge has been interrupted, reset to uncharged
			SetEmissionAll (offColor);
			ChargeLevel = 0;
		}
	}
}
