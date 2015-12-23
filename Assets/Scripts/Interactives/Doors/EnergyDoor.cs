using UnityEngine;
using System.Collections;

public class EnergyDoor : Door {

	//store the initial 'On' emission color
	public Color onColor = new Color(0xD3F,0x1CF,0xE2F);
	private Color offColor;

	public ParticleSystem EnergyDoorFront; 
	public ParticleSystem EnergyDoorBack;
	public ParticleSystem EnergyDoorFrontHit; 
	public ParticleSystem EnergyDoorBackHit;

	//store references to each piece of the side frame
	public GameObject frame;
	private Renderer[] side_pieces;

	private Collider coll;

	void Awake(){
		coll = GetComponent<Collider>();
		side_pieces = frame.GetComponentsInChildren<Renderer> ();

		offColor = onColor * Config.dimIntensity;

		//set colour of energy field correctly
		EnergyDoorFront.GetComponent<Renderer>().material.SetColor ("_Color", onColor);
		EnergyDoorFrontHit.GetComponent<Renderer>().material.SetColor ("_Color", onColor);
		EnergyDoorBack.GetComponent<Renderer>().material.SetColor ("_Color", onColor);
		EnergyDoorBackHit.GetComponent<Renderer>().material.SetColor ("_Color", onColor);

		IsClosed = !IsClosed;
		SwapState ();
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
		SetEmissionColor (onColor);

		IsClosed = true;
	}

	public override void Open ()
	{
		EnergyDoorFront.Clear ();
		EnergyDoorBack.Clear ();
		EnergyDoorFront.Stop ();
		EnergyDoorBack.Stop ();

		coll.enabled = false;
		SetEmissionColor (offColor);

		IsClosed = false;
	}

	//this function cycles through all pieces of the frame and changes their emission color
	public void SetEmissionColor(Color color){
		foreach (Renderer side in side_pieces) {
			side.material.SetColor ("_EmissionColor", color);
		}
	}
}
