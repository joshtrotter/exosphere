using UnityEngine;
using System.Collections;

//this switch type takes four seconds to charge up and once on cannot be deactivated

public class LaserSwitchCharge : Switch {

	//store a reference to the emitting material on the main laser switch
	private Material glow;

	//store the initial 'On' emission color
	private Color startColor;
	
	private bool charging = false;
	//track how charged the switch is from 0 to 4
	private int ChargeLevel = 0;
	//store references to the lights that turn on as switch is being charged
	public GameObject[] ChargeLights;
	
	void Awake () {
		glow = GetComponent<Renderer> ().material;
		startColor = glow.GetColor ("_EmissionColor");
		if (IsOn) {
			ChargeLevel = 4;
		} else {
			SetEmissionAll (Color.black);
		}
	}
	
	public override void OnLaserEnter(){
		if (ChargeLevel != 4) { //only charge up if not already fully charged
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
	//until the switch is fully charged (4)
	private IEnumerator ChargeUp(){
		print ("Charging");
		SetEmission (glow, startColor);
		float timer = 0;
		while (charging) {
			yield return new WaitForEndOfFrame();
			timer += Time.deltaTime;
			if (timer > 1){ //increment charge level after each second
				ChargeLevel += 1;
				print ("Charge level increased to" + ChargeLevel);
				timer = 0;
				print ("Turning on component " + (ChargeLevel-1));
				SetEmission(ChargeLights[ChargeLevel-1].GetComponent<Renderer>().material, startColor);
			}
			if (ChargeLevel == 4){ //switch is fully on, stop charging activate target
				charging = false;
				target.Activate ();
				TurnOn();
			}
		}
		if (ChargeLevel != 4) { //charge has been interrupted, reset to uncharged
			SetEmissionAll (Color.black);
			ChargeLevel = 0;
		}
	}
}
