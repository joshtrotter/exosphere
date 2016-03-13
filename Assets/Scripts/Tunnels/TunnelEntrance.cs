using UnityEngine;
using System.Collections;

public class TunnelEntrance : MonoBehaviour {

	public PhysicMaterial tunnelPhysicMaterial;

	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag ("Player")) {
			coll.material = tunnelPhysicMaterial;
		}
	}
}
