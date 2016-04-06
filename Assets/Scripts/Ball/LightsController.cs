using UnityEngine;
using System.Collections;

public class LightsController : MonoBehaviour {

	public float minLightIntensity = 0.2f;
	public float minLightRange = 1f;
	public float maxLightIntensity = 1.5f;
	public float maxLightRange = 10f;
	public float minLightTrailSpeed = 0.33f;
	public float minLightTrailTime = 0.5f;
	public float maxLightTrailTime = 2.5f;

	private TransformController transformController;
	private Light ballLight;
	private Rigidbody rb;
	private Renderer renderer;
	private float neutralDrag;
	private float brakeDragFactor;
	private float currentHeat;
	private ArcReactor_Trail trailRenderer;
	private Coroutine trailLightTracker;

	void Awake () {
		transformController = transform.GetComponent<TransformController> ();
		ballLight = transform.GetComponentInChildren<Light> ();
		rb = transform.GetComponent<Rigidbody> ();
		renderer = transform.GetComponent<Renderer> ();
		neutralDrag = rb.drag;
		brakeDragFactor = transform.GetComponent<BrakeController> ().brakeDragFactor;
		trailRenderer = transform.GetComponentInChildren<ArcReactor_Trail> ();
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

	public void TurnLightTrailOn() {
		trailRenderer.enabled = true;
		if (trailLightTracker != null) {
			StopCoroutine(trailLightTracker);
		}
		trailLightTracker = StartCoroutine (TrackTrailLight ());
	}

	public void TurnLightTrailOff() {
		trailRenderer.enabled = false;
	}	

	private IEnumerator TrackTrailLight() {
		float elapsedTime = 0f;
		float elapsedTimeRatio = 1f;
		yield return new WaitForSeconds (minLightTrailTime);
		while (elapsedTime < maxLightTrailTime && CalculateSpeedModifier() > minLightTrailSpeed) {
			elapsedTimeRatio = (maxLightTrailTime - elapsedTime) / maxLightTrailTime;
			trailRenderer.currentArc.sizeMultiplier = CalculateSpeedModifier() * elapsedTimeRatio;
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		TurnLightTrailOff ();
	}
}
