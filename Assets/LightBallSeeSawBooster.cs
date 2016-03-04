using UnityEngine;
using System.Collections;

public class LightBallSeeSawBooster : MonoBehaviour {

	public float boostPower = 2f;

	private float initialBoostPower;
	private TransformController transformController;
	private BallController ballController;

	void Awake() {
		transformController = GameObject.FindGameObjectWithTag ("Player").GetComponent<TransformController> ();
		ballController = GameObject.FindGameObjectWithTag ("Player").GetComponent<BallController> ();
		initialBoostPower = ballController.movementModifiers.movePowerScaler;
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player") && ShouldBoostBall()) {
			initialBoostPower = ballController.movementModifiers.movePowerScaler;
			ballController.movementModifiers.movePowerScaler = boostPower;
		}
	}
	
	void OnTriggerExit(Collider coll) {
		if (coll.CompareTag ("Player")) {
			ballController.movementModifiers.movePowerScaler = initialBoostPower;
		}
	}

	private bool ShouldBoostBall() {
		return transformController.currentTransform.GetType () == typeof(LightTransform);
	}
}
