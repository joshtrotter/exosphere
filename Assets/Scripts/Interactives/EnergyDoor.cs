using UnityEngine;
using System.Collections;

public class EnergyDoor : Door {

	public ParticleSystem EnergyDoorFront; 
	public ParticleSystem EnergyDoorBack;
	public ParticleSystem EnergyDoorFrontHit; 
	public ParticleSystem EnergyDoorBackHit;

	private Collider coll;


	void Awake(){
		coll = GetComponent<Collider>();
		IsClosed = !IsClosed;
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

	public override void Close ()
	{
		EnergyDoorFront.Play ();
		EnergyDoorBack.Play ();
		coll.enabled = true;
		IsClosed = true;
	}

	public override void Open ()
	{
		EnergyDoorFront.Clear ();
		EnergyDoorBack.Clear ();
		EnergyDoorFront.Stop ();
		EnergyDoorBack.Stop ();
		coll.enabled = false;
		IsClosed = false;
	}
}
