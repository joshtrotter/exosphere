using UnityEngine;
using System.Collections;

public class RepulsionMaterial : MonoBehaviour {

	public float minEmission = 0.75f;
	public float maxEmission = 1.5f;

	private Material mat;
	private Color baseColor;
	private float variant;
	
	void Awake () {
		mat = transform.GetComponent<Renderer> ().material;
		baseColor = mat.GetColor ("_EmissionColor");
		variant = Random.Range (0f, 1f);
	}

	void Update () {
		Color emissionColor = baseColor * (1 + Mathf.PingPong(variant + Time.time * 3f, 1f));
		mat.SetColor ("_EmissionColor", emissionColor);
	}

}
