using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BouncyObject : MonoBehaviour {

	public float repulsePower = 2f;
	public float flashTime = 0.2f;
	public Renderer[] bounceLights;

	public bool autoCorrectCamera = false;
	public float autoCorrectDistance = 20f;
	public float autoCorrectTime = 0.5f;

	private Color flashColor;
	private Color offColor;
	private AmazeballCam cam;

	void Awake(){
		flashColor = bounceLights [0].material.GetColor ("_EmissionColor");
		offColor = flashColor * Config.softDimIntensity;
		TurnLightsOff ();
		if (autoCorrectCamera) {
			cam = GameObject.FindGameObjectWithTag ("CameraRig").GetComponent<AmazeballCam>();
		}
	}

	void OnCollisionEnter(Collision coll){
		float powerModifier = 1f;
		if (coll.gameObject.CompareTag ("Player")) {
			powerModifier = coll.gameObject.GetComponent<BallController>().GetMovePower();
			coll.gameObject.GetComponent<LightsController>().TurnLightTrailOn();
		}

		Debug.DrawRay (this.transform.position, coll.contacts [0].normal * -1000, Color.red, 10);
		StartCoroutine (FlashLights ());
		if (coll.rigidbody != null) {
			coll.rigidbody.AddForce (coll.contacts [0].normal * repulsePower * powerModifier * -1, ForceMode.Impulse);
			if (autoCorrectCamera) {
				cam.lookAhead (autoCorrectDistance, autoCorrectTime);
			}
		}
	}
	
	private void TurnLightsOff(){
		foreach (Renderer light in bounceLights) {
			light.material.SetColor ("_EmissionColor", offColor);
		}
	}

	private IEnumerator FlashLights(){
		foreach (Renderer light in bounceLights) {
			light.material.SetColor ("_EmissionColor", flashColor);
		}
		yield return new WaitForSeconds(flashTime);
		TurnLightsOff();
	}
}
