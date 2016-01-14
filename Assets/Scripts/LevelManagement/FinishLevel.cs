using UnityEngine;
using System.Collections;

public class FinishLevel : MonoBehaviour {

	private BallController ball;
	private Vector3 targetPos;
	private bool levelComplete;

	public float floatSpeed = 5f;

	void Awake(){
		ball = GameObject.FindGameObjectWithTag ("Player").GetComponent<BallController> ();
		targetPos = transform.position;
		targetPos.y += 8;
	}

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player")){
			StartCoroutine(FloatBallUp());
		}
	}

	private IEnumerator FloatBallUp(){
		ball.GetComponent<Rigidbody> ().isKinematic = true;
		//move ball to centre and float up in air
		while (Vector3.Distance(targetPos, ball.transform.position) > 1E-04f) {
			yield return new WaitForFixedUpdate();
			ball.transform.position = Vector3.MoveTowards(ball.transform.position, targetPos, Time.fixedDeltaTime * floatSpeed);
		}
		if (!levelComplete) {
			levelComplete = true;
			LevelCompleteScreen.controller.LevelComplete (Time.time);
		}
	}
}
