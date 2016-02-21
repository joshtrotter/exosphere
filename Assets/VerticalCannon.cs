using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityStandardAssets.Cameras;

public class VerticalCannon : MonoBehaviour {

	public float launchPower = 5f;
	public Renderer[] launchLights;
	public Light flashingLight;
	private Color startColor;

	//this variable will be used by the vertical cannon landing script to inform this class that 
	//the ball has reached its landing point
	private bool hasLanded;

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

	private IEnumerator Launch(Rigidbody rb)
	{
		//disable player control of the ball
		BallInputReader ballInput = rb.GetComponent<BallInputReader> ();
		ballInput.enabled = false;
		rb.GetComponent<BrakeController> ().ReleaseBrakes ();

		//position ball within the centre of the cannon correctly
		rb.velocity = Vector3.zero;
		rb.useGravity = false;
		rb.transform.DOMove (this.transform.position + new Vector3 (0, 1f, 0), 5f).Play ();

		//take control of camera and move it to the right place, player can still pan camera
		AmazeballCam camController = GameObject.FindGameObjectWithTag ("CameraRig").GetComponent<AmazeballCam>();
		ProtectCameraFromWallClip wallClipper = camController.GetComponent<ProtectCameraFromWallClip> ();
		wallClipper.enabled = false;
		float camMoveSpeed = camController.moveSpeed;
		camController.moveSpeed = 0f;
		camController.transform.DOLookAt (this.transform.position, 2f).Play ();
		camController.movePivot (new Vector3 (0f, -2f, 0f));

		//flashing lights etc.
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

		//prepare ball and camera for launching
		rb.useGravity = true;
		camController.moveSpeed = camMoveSpeed * 2;
		camController.transform.DOLocalRotate (new Vector3 (0f, camController.transform.localEulerAngles.y, 0f), 1f).Play ();
		camController.movePivot (new Vector3 (0f, 2f, 0f));

		//launch ball
		rb.AddForce(Vector3.up * launchPower * rb.mass, ForceMode.Impulse);

		//wait until a VerticalCannonLanding script lets the cannon know the ball has landed
		hasLanded = false;
		while (!hasLanded) {
			yield return new WaitForEndOfFrame ();
		}
		//re-enable regular ball control and camera function
		camController.moveSpeed = camMoveSpeed;
		ballInput.enabled = true;
		wallClipper.enabled = true;
	}

	public void SetHasLanded(){
		hasLanded = true;
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
