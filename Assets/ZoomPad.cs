using UnityEngine;
using System.Collections;

public class ZoomPad : MonoBehaviour {

	private BallController ball;

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.CompareTag("Player")){
			ball = coll.GetComponent<BallController>();
			coll.attachedRigidbody.AddForce(ball.GetTargetVelocity().normalized * 3 * ball.GetMovePower(), ForceMode.Impulse);
		}
	}
}
