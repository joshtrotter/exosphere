using UnityEngine;
using System.Collections;

public class BallShatterer : MonoBehaviour {
	
	private float shatterForce = 8f;

	void OnCollisionEnter(Collision collision){
		//Debug.Log (collision.impulse.magnitude);
		if (collision.impulse.sqrMagnitude > Mathf.Pow (shatterForce,2)) {
			GetComponent<BallDestroyer>().Shatter();
		}
	}

}
