using UnityEngine;
using System.Collections;

public class FinishLevel : MonoBehaviour {

	private BallController ball;
	private Vector3 targetPos;
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
		while (Vector3.Distance(targetPos, ball.transform.position) > Vector3.kEpsilon) {
			yield return new WaitForFixedUpdate();
			ball.transform.position = Vector3.MoveTowards(ball.transform.position, targetPos, Time.fixedDeltaTime * floatSpeed);
		}
		GameObject.FindGameObjectWithTag ("LevelManager").GetComponentInChildren<LevelCompleteScreen> ().LevelComplete (Time.time);
	}
}
