using UnityEngine;
using System.Collections;

public class BouncyObject : MonoBehaviour {

	public float repulsePower = 4f;
	public float flashTime = 0.2f;
	public Renderer[] bounceLights;

	private Color flashColor;
	private Color offColor;

	void Awake(){
		flashColor = bounceLights [0].material.GetColor ("_EmissionColor");
		offColor = flashColor * Config.softDimIntensity;
		TurnLightsOff ();
	}

	void OnCollisionEnter(Collision coll){
		float powerModifier = 1f;
		if (coll.gameObject.CompareTag ("Player")) {
			powerModifier = coll.gameObject.GetComponent<BallController>().GetMovePower();
		}
		Debug.Log (coll.contacts [0].normal * repulsePower * powerModifier);
		Debug.DrawRay (this.transform.position, coll.contacts [0].normal * -100, Color.white, 10);
		StartCoroutine (FlashLights ());
		coll.rigidbody.AddForce (coll.contacts [0].normal * repulsePower * powerModifier * -1, ForceMode.Impulse);
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
