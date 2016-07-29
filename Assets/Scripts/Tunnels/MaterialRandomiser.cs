using UnityEngine;
using System.Collections;

public class MaterialRandomiser : MonoBehaviour {

	public Material[] materials;
	private Renderer rend;

	void Awake() {
		rend = GetComponent<Renderer> ();
	}

	void OnEnable() {
		Debug.Log ("Choosing Random Material");
		rend.material = materials [Random.Range (0, materials.Length)];
	}
}
