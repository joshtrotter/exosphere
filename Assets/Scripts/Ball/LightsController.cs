using UnityEngine;
using System.Collections;

public class LightsController : MonoBehaviour {

	public float minLightIntensity = 0.2f;
	public float minLightRange = 1f;
	public float maxLightIntensity = 1.5f;
	public float maxLightRange = 10f;

	private TransformController transformController;
	private Light ballLight;
	private Rigidbody rb;
	private Renderer renderer;
	private float neutralDrag;
	private float brakeDragFactor;
	private float currentHeat;

	void Awake () {
		transformController = transform.GetComponent<TransformController> ();
		ballLight = transform.GetComponentInChildren<Light> ();
		rb = transform.GetComponent<Rigidbody> ();
		renderer = transform.GetComponent<Renderer> ();
		neutralDrag = rb.drag;
		brakeDragFactor = transform.GetComponent<BrakeController> ().brakeDragFactor;
	}

	void FixedUpdate() {
		UpdateEmission ();
		if (ballLight.enabled) {
			UpdateLights();
		}
	}

	private void UpdateEmission() {
		if (rb.drag > neutralDrag) {
			renderer.material.SetColor ("_EmissionColor", Color.white * Mathf.Lerp (transformController.currentTransform.minEmission, transformController.currentTransform.brakeEmission, CalculateBrakeModifier()));
		} else {
			renderer.material.SetColor ("_EmissionColor", Color.white * Mathf.Lerp (transformController.currentTransform.minEmission, transformController.currentTransform.maxEmission, CalculateSpeedModifier()));
		}  
	}

	private void UpdateLights() {
		ballLight.intensity = Mathf.Lerp (minLightIntensity, maxLightIntensity, HeatAdjustedSpeed(CalculateSpeedModifier()));
		ballLight.range = Mathf.Lerp (minLightRange, maxLightRange, HeatAdjustedSpeed(CalculateSpeedModifier())); 
	}

	private float CalculateSpeedModifier() {
		if (rb.isKinematic) {
			return 1f;
		}
		return Mathf.Clamp01(rb.angularVelocity.magnitude / rb.maxAngularVelocity);
	}

	private float CalculateBrakeModifier() {
		return (rb.drag - neutralDrag) / brakeDragFactor;
	}

	private float HeatAdjustedSpeed(float speed) {
		currentHeat = Mathf.Clamp01(currentHeat + ((speed - 0.75f) * Time.deltaTime));
		Debug.Log (currentHeat);
		return ((speed * 0.25f) + (currentHeat * 0.75f));
	}
	
	public void TurnLightOn() {
		ballLight.enabled = true;
	}

	public void TurnLightOff() {
		ballLight.enabled = false;
	}

	public void SetLightColor(Color color) {
		ballLight.color = color;
	}
}
