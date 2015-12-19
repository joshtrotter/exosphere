using UnityEngine;
using System.Collections;

public class LaserHeater : LaserModule {
	
	public Color heatColor = Color.red;
	public float heatRate = 1f;
	
	private float currentHeat;
	private Gradient heatGradient;
	private GradientColorKey[] heatColors;
	private GradientAlphaKey[] heatAlphas;

	private bool heating = false;
	
	private Renderer rend;
	
	void Awake () {
		rend = GetComponent<Renderer>();
		SetupHeatGradients ();
	}
	
	//Called when a new laser hit is detected
	public override void DoHitStart(ArcReactorHitInfo hit) 
	{
		heating = true;
		StartCoroutine(HeatUpObject ());
	}
	
	//Called when an existing laser hit has ended
	public override void DoHitEnd()
	{
		heating = false;
	}
	
	//Heats up the object while in contact with the laser
	private IEnumerator HeatUpObject()
	{
		//Calculate the heat gradient to use - if we are still cooling down then set the base of the gradient to the base of the previous heat gradient so that we can return to the original colour
		heatColors [0].color = currentHeat > 0 ? heatGradient.Evaluate(0f) : rend.material.color;
		heatAlphas [0].alpha = currentHeat > 0 ? heatGradient.Evaluate(0f).a : rend.material.color.a;
		heatGradient.SetKeys(heatColors, heatAlphas);
		
		while (heating) {
			yield return new WaitForEndOfFrame();
			currentHeat = Mathf.Clamp01(currentHeat + heatRate * Time.deltaTime);
			rend.material.color = heatGradient.Evaluate(currentHeat);
		}
		
		DoHitEnd ();
		
		//Tween the color back to the starting point unless a new hit is detected
		while (currentHeat > 0 && !heating) {
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

