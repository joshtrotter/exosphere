using UnityEngine;
using System.Collections;

public class EnergyDoor : MonoBehaviour {

	public ParticleSystem EnergyDoorFront; 
	public ParticleSystem EnergyDoorBack;
	public ParticleSystem EnergyDoorFrontHit; 
	public ParticleSystem EnergyDoorBackHit;

	public bool FieldOn = true;

	private Collider coll;


	void Awake(){
		coll = GetComponent<Collider>();
	}

	void Start(){
		FieldOn = !FieldOn;
		SwapState ();
	}

	//allows manual disabling of the field for testing
	void Update(){
		if (Input.GetKeyDown ("o")) {
			SwapState ();
		}
	}

	void OnCollisionEnter(){
		EnergyDoorFrontHit.Play ();
		EnergyDoorBackHit.Play ();
	}

	public void SwapState(){
		if (FieldOn) {
			TurnOff ();		
		} else {
			TurnOn ();
		}
	}

	public void TurnOn ()
	{
		EnergyDoorFront.Play ();
		EnergyDoorBack.Play ();
		coll.enabled = true;
		FieldOn = true;
	}

	void TurnOff ()
	{
		EnergyDoorFront.Stop ();
		EnergyDoorBack.Stop ();
		coll.enabled = false;
		FieldOn = false;
	}
}
