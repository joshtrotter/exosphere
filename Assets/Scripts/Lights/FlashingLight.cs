using UnityEngine;
using System.Collections;

public class FlashingLight : MonoBehaviour {

	public float period = 1f;
	//on percentage of period, between 0 and 1
	public float dutyCycle = 0.5f;

	private Light light;
	private Color lightColour;
	private Renderer rend;

	void OnEnable(){
		light = GetComponent<Light> ();
		lightColour = light.color;
		rend = this.gameObject.GetComponent<Renderer> ();
		StartCoroutine (Flash ());
	}

	private IEnumerator Flash(){
		while (isActiveAndEnabled) {
			light.enabled = true;
			rend.material.SetColor ("_EmissionColor", lightColour);
			yield return new WaitForSeconds(dutyCycle/period);
			light.enabled = false;
			rend.material.SetColor ("_EmissionColor", Color.black);
			yield return new WaitForSeconds((1-dutyCycle)/period);
		}
	}
}
