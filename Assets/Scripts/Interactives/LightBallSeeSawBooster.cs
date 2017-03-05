using UnityEngine;
using System.Collections;

public class LightBallSeeSawBooster : BallMovementManipulator {

	protected override bool ShouldModify(GameObject ball) {
		return ball.GetComponent<TransformController>().currentTransform.GetType () == typeof(LightTransform);
	}
}
