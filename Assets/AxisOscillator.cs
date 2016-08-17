using UnityEngine;
using System.Collections;

public class AxisOscillator : MonoBehaviour {

	public Vector3 endRotation;
	public float frequency = 1f;

	void FixedUpdate() {
		transform.localEulerAngles = endRotation * Mathf.Sin (Time.time * Mathf.PI / frequency);
	}
}
