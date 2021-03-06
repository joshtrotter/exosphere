﻿using UnityEngine;
using System.Collections;

public class ZoomPad : SwitchableObject {

	//private BallController ball;
	public Renderer[] floatingArrows;
	public float zoomSpeed = 3f;
	private Color offColor;
	public Color onColor;
	public bool IsOn = true;
	public float waitTime = 0.3f;

	void Awake(){
		offColor = onColor * Config.softDimIntensity;
		IsOn = !IsOn;
		SwapState ();
	}

	void OnEnable(){
		Awake ();
	}

	public override void Activate(){
		SwapState ();
	}

	private void SwapState(){
		if (IsOn) {
			IsOn = false;
			SetAllLightsColor (offColor);
		} else {
			IsOn = true;
			StartCoroutine(ZoomLights());
		}
	}

	void SetAllLightsColor (Color color)
	{
		foreach (Renderer rend in floatingArrows) {
			rend.material.SetColor ("_EmissionColor", color);
		}
	}

	private IEnumerator ZoomLights(){
		Renderer currentLight;
		while (IsOn) {
			for (int i = 0; i < floatingArrows.Length; i++){
				currentLight = floatingArrows[i];
				currentLight.material.SetColor("_EmissionColor", onColor);
				yield return new WaitForSeconds(waitTime);
				currentLight.material.SetColor("_EmissionColor", offColor);
			}
			yield return new WaitForSeconds(waitTime * 3);
		}
	}

	void OnTriggerEnter(Collider coll){
		if (IsOn){
			float movePower = 10 * coll.attachedRigidbody.mass; 
			if (coll.gameObject.CompareTag("Player")){
				//movePower = coll.GetComponent<BallController>().GetMovePower();
				coll.GetComponent<BallInputReader>().enabled = false;
				coll.GetComponent<BrakeController> ().ReleaseBrakes ();
				coll.GetComponent<LightsController>().TurnLightTrailOn();
			}
			SetAllLightsColor(onColor);
			coll.attachedRigidbody.AddForce(transform.forward * zoomSpeed * movePower, ForceMode.Impulse);
		}
	}

	void OnTriggerExit(Collider coll){
		if (IsOn && coll.CompareTag("Player")){
			coll.GetComponent<BallInputReader>().enabled = true;
		}
	}
}
