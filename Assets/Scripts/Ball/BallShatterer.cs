using UnityEngine;
using System.Collections;

public class BallShatterer : MonoBehaviour {
	
	private float shatterForce = 12f;

	void OnCollisionEnter(Collision collision){
		if (collision.impulse.sqrMagnitude > Mathf.Pow (shatterForce,2)) {
			GetComponent<BallDestroyer>().Shatter();
		}
	}

}
