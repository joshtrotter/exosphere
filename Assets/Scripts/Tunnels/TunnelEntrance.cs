using UnityEngine;
using System.Collections;

public class TunnelEntrance : MonoBehaviour {

	public PhysicMaterial tunnelPhysicMaterial;

	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag ("Player")) {
			coll.material = tunnelPhysicMaterial;
			BallController ball = coll.GetComponent<BallController>();
			ball.GetComponent<TransformController>().currentTransform.DisablePhysicalModifiers(ball);
			ball.GetComponent<LightsController>().TurnLightOn();
			ball.allowBrakeLocks = false;
		}
	}
}
