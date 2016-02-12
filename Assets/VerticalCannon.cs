using UnityEngine;
using DG.Tweening;
using System.Collections;

public class VerticalCannon : MonoBehaviour {

	public float launchPower = 5f;
	public Renderer[] launchLights;
	public Light flashingLight;
	public GameObject cameraMount;
	private Color startColor;

	void Awake(){
		startColor = launchLights [0].material.GetColor ("_EmissionColor");
		foreach (Renderer rend in launchLights) {
			SetEmissionColor(rend, startColor * Config.softDimIntensity);
		}
	}

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player")){
			StartCoroutine(Launch (coll.attachedRigidbody));
		}
	}

	private IEnumerator Launch(Rigidbody rb){

		rb.velocity = Vector3.zero;
		AmazeballCam camController = GameObject.FindGameObjectWithTag ("CameraRig").GetComponent<AmazeballCam>();
		camController.enabled = false;

		//camController.transform.DOMove (cameraMount.transform.position, 1f).Play ();
		//camController.transform.DORotate (cameraMount.transform.eulerAngles, 1f).Play ();
		camController.transform.DOLookAt (this.transform.position, 1f).Play ();

		yield return new WaitForSeconds (1f);
		StartCoroutine ("FlashLight");
		yield return new WaitForSeconds (1.5f);
		for (int i = 0; i < 4; i++) {
			SetEmissionColor(launchLights[i], startColor);
		}
		yield return new WaitForSeconds (1.5f);
		for (int i = 4; i < launchLights.Length; i++) {
			SetEmissionColor(launchLights[i], startColor);
		}
		yield return new WaitForSeconds (1f);
		StopCoroutine ("FlashLight");

		rb.velocity = Vector3.zero;
		rb.AddForce(Vector3.up * launchPower * rb.mass/*rb.GetComponent<BallController>().GetMovePower ()*/, ForceMode.Impulse);
		camController.enabled = true;
	}

	private IEnumerator FlashLight(){
		while (true) {
			yield return new WaitForSeconds(0.1f);
			flashingLight.enabled = true;
			yield return new WaitForSeconds(0.2f);
			flashingLight.enabled = false;

		}
	}

	private void SetEmissionColor(Renderer rend, Color color){
		rend.material.SetColor ("_EmissionColor", color);
	}
}
