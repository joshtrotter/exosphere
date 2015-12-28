using UnityEngine;
using System.Collections;

public class LaserTrigger : MonoBehaviour {

	public LaserTriggerGun gun;

	void OnTriggerEnter(Collider coll) 
	{
		if (coll.CompareTag ("Player")) {
			gun.Trigger();
		}
	}
}
