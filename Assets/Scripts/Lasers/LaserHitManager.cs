using UnityEngine;
using System.Collections;

/**
 * This class handles laser hits for the attached object. It will heat up the object overtime by varying the color gradient.
 * You can override the DoHitStart and DoHitEnd methods to customise behavior.
 */ 
public class LaserHitManager : MonoBehaviour {

	public ParticleSystem hitParticles;
	public Color heatColor = Color.red;
	public float heatRate = 1f;

	protected bool hitThisFrame;
	protected bool hitLastFrame;

	private float currentHeat;
	private Gradient heatGradient;
	private GradientColorKey[] heatColors;
	private GradientAlphaKey[] heatAlphas;

	private Renderer rend;
			
	void Start () {
		rend = GetComponent<Renderer>();
		hitParticles = GameObject.Instantiate<ParticleSystem> (hitParticles);
		hitParticles.enableEmission = false;
		hitParticles.gameObject.SetActive(false);
		SetupHeatGradients ();

		hitThisFrame = false;
		hitLastFrame = false;
	}

	//Invoked from the ArcReactor system whenever a laser hits the collider this object is attached to
	public void ArcReactorHit(ArcReactorHitInfo hit)
	{
		hitThisFrame = true;
		if (!hitLastFrame) {
			DoHitStart(hit);
		}

		//Update the hit position of the particle effect
		hitParticles.transform.position = hit.raycastHit.point;
		hitParticles.transform.LookAt(hit.raycastHit.point + hit.raycastHit.normal);
	}

	//Called when a new laser hit is detected
	public virtual void DoHitStart(ArcReactorHitInfo hit) 
	{
		hitParticles.gameObject.SetActive (true);
		hitParticles.enableEmission = true;
		StartCoroutine(HeatUpObject ());
	}
	
	//Called when an existing laser hit has ended
	public virtual void DoHitEnd()
	{
		hitParticles.enableEmission = false;
	}

	//Heats up the object while in contact with the laser
	private IEnumerator HeatUpObject()
	{
		//Calculate the heat gradient to use - if we are still cooling down then set the base of the gradient to the base of the previous heat gradient so that we can return to the original colour
		heatColors [0].color = currentHeat > 0 ? heatGradient.Evaluate(0f) : rend.material.color;
		heatAlphas [0].alpha = currentHeat > 0 ? heatGradient.Evaluate(0f).a : rend.material.color.a;
		heatGradient.SetKeys(heatColors, heatAlphas);

		while (hitLastFrame || hitThisFrame) {
			yield return new WaitForEndOfFrame();
			currentHeat = Mathf.Clamp01(currentHeat + heatRate * Time.deltaTime);
			rend.material.color = heatGradient.Evaluate(currentHeat);

			hitLastFrame = hitThisFrame;
			hitThisFrame = false;
		}

		DoHitEnd ();

		//Tween the color back to the starting point unless a new hit is detected
		while (currentHeat > 0 && !hitLastFrame) {
			currentHeat = Mathf.Clamp01(currentHeat - heatRate * Time.deltaTime);
			rend.material.color = heatGradient.Evaluate(currentHeat);
			yield return new WaitForEndOfFrame();
		}
//		hitParticles.gameObject.SetActive (false);
	}

	//Setup gradients for tweening the object between hot and cold
	private void SetupHeatGradients()
	{
		heatGradient = new Gradient();
		heatColors = new GradientColorKey[2];
		heatAlphas = new GradientAlphaKey[2];		
				
		// Populate the color keys at the relative time 0 and 1 (0 and 100%)
		heatColors = new GradientColorKey[2];
		heatColors[0].color = rend.material.color;
		heatColors[0].time = 0.0f;
		heatColors[1].color = heatColor;
		heatColors[1].time = 1.0f;
		
		// Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
		heatAlphas = new GradientAlphaKey[2];
		heatAlphas[0].alpha = 1.0f;
		heatAlphas[0].time = 0.0f;
		heatAlphas[1].alpha = 1.0f;
		heatAlphas[1].time = 1.0f;
		heatGradient.SetKeys(heatColors, heatAlphas);
	}

}
