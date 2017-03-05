using UnityEngine;
using System.Collections;

public class EmissionModifier : MonoBehaviour {

	public float minEmission = 0.75f;
	public float maxEmission = 1.5f;
	public float emissionPulseRate = 1f;
	public int materialIndex = 0;
	public Color baseColor;

	private Material mat;
	private float variant;
	
	void Awake () {
		mat = transform.GetComponent<Renderer> ().materials[materialIndex];
		Debug.Log (mat.name);
		variant = Random.Range (0f, 1f);
	}

	void Update () {
		Color emissionColor = baseColor * (minEmission + Mathf.PingPong(variant + Time.time * emissionPulseRate, maxEmission));
		mat.SetColor ("_EmissionColor", emissionColor);
	}

}
