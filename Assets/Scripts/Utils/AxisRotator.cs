using UnityEngine;
using System.Collections;

public class AxisRotator : MonoBehaviour {

	public float rotationsPerSecond = 1f;
	public Vector3 rotateVector = Vector3.up;
	public bool randomiseDirection = false;

	// Update is called once per frame
	void Update () {
		transform.Rotate (rotateVector * rotationsPerSecond * 360f * Time.deltaTime);
	}

	void OnEnable(){
		if (randomiseDirection) {
			rotateVector *= (Random.Range (0,1) == 1 ? 1 : -1);
		}
	}
}
