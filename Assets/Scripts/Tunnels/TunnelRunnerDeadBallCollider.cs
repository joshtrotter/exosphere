using UnityEngine;
using System.Collections;

public class TunnelRunnerDeadBallCollider : MonoBehaviour {

	void OnTriggerEnter(Collider coll) 
	{
		if (coll.CompareTag("Player")) {
			TunnelRunnerCompleteScreen.controller.PopDisplayAndReload();
		}
	}

}
