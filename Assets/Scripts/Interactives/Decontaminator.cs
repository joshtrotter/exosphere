using UnityEngine;
using System.Collections;

public class Decontaminator : MonoBehaviour {

	private ParticleSystem[] jets;

	void Awake(){
		jets = GetComponentsInChildren<ParticleSystem> ();
	}

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag ("Player")) {
			foreach (ParticleSystem jet in jets){
				jet.Play ();
			}
			coll.GetComponent<TransformController>().RemoveCurrent();
			coll.GetComponent<PickupController>().RemoveAll();
		}
	}
}
