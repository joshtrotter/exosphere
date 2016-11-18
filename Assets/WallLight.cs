using UnityEngine;
using System.Collections;

public class WallLight : MonoBehaviour {

	void Awake(){
		Color lightColour = GetComponentInChildren<Light> ().color;
		this.gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", lightColour);
	}
}
