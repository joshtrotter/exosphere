using UnityEngine;
using System.Collections;

public class TunnelJumpSwatBarrier : MonoBehaviour {
	
	//store the initial 'On' emission color
	public Color onColor = new Color(0xD3F,0x1CF,0xE2F);
	
	public ParticleSystem EnergyBarrier; 
	public ParticleSystem EnergyBarrierHit; 
	
	//store references to each piece of the side frame
	public GameObject frame;
	private Renderer[] side_pieces;

	
	void Awake(){
		side_pieces = frame.GetComponentsInChildren<Renderer> ();
		//set colour of energy field correctly
		EnergyBarrier.GetComponent<Renderer>().material.SetColor ("_Color", onColor);
		EnergyBarrierHit.GetComponent<Renderer>().material.SetColor ("_Color", onColor);
		SetEmissionColor (onColor);

		EnergyBarrier.Play ();

	}
	
	void OnCollisionEnter(){
		EnergyBarrierHit.Play ();
	}
	
	//this function cycles through all pieces of the frame and changes their emission color
	public void SetEmissionColor(Color color){
		foreach (Renderer side in side_pieces) {
			side.material.SetColor ("_EmissionColor", color);
		}
	}
}
