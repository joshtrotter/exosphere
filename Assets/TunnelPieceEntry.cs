using UnityEngine;
using System.Collections;

public class TunnelPieceEntry : MonoBehaviour {
	
	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag ("Player")) {
			TunnelSpawnController.INSTANCE.OnTunnelPieceEntry();
		}
	}

}
