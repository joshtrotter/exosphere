using UnityEngine;
using System.Collections;

/**
 * Adds the associated pickup item to the player inventory whenever the player collides with this gameObject
 */ 
public class PickupCollider : MonoBehaviour {

	public Pickup pickup;
	
	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag("Player")) {
			PickupController pickupController = coll.gameObject.GetComponent<PickupController>();
			pickup.Reset();
			pickupController.AddPickup(pickup);
			Destroy(gameObject);
		}
	}
}
