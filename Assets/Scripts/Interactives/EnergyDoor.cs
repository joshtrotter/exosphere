using UnityEngine;
using System.Collections;

public class EnergyDoor : Door {

	public ParticleSystem EnergyDoorFront; 
	public ParticleSystem EnergyDoorBack;
	public ParticleSystem EnergyDoorFrontHit; 
	public ParticleSystem EnergyDoorBackHit;

	//store references to each piece of the side frame
	public GameObject frame;
	private Renderer[] side_pieces;

	private Collider coll;

	//store the initial 'On' emission color
	private Color startColor;


	void Awake(){
		coll = GetComponent<Collider>();
		side_pieces = frame.GetComponentsInChildren<Renderer> ();
		startColor = side_pieces[0].material.GetColor ("_EmissionColor");
		IsClosed = true;
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
		SetEmission (startColor);

		IsClosed = true;
	}

	public override void Open ()
	{
		EnergyDoorFront.Clear ();
		EnergyDoorBack.Clear ();
		EnergyDoorFront.Stop ();
		EnergyDoorBack.Stop ();

		coll.enabled = false;
		SetEmission (Color.black);

		IsClosed = false;
	}

	//this function cycles through all pieces of the frame and changes their emission color
	public void SetEmission(Color color){
		foreach (Renderer side in side_pieces) {
			side.material.SetColor ("_EmissionColor", color);
		}
	}
}
