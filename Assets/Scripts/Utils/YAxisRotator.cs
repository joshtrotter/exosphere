using UnityEngine;
using System.Collections;

public class YAxisRotator : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up * 2);
	}
}
