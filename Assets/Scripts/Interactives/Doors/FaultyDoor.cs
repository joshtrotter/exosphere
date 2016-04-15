
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FaultyDoor : MoveBetweenTargets {

	float targetY;
	float fullOpenY;
	float fullCloseY;
	bool goingUp = true;
	int movementCounter = 0;
	ParticleSystem[] sparks;

	protected override void Awake ()
	{
		fullCloseY = transform.position.y;
		fullOpenY = fullCloseY + targetPositions [0].y;
		targetY = fullOpenY;
		sparks = GetComponentsInChildren<ParticleSystem> ();

	}

	protected override Tween MoveToTarget (Vector3 target)
	{
		return transform.DOMove (target, targetY == fullCloseY ? 0.5f : Random.Range (0.5f, timeToMove));
	}

	protected override Vector3 FindNewTarget ()
	{
		setRandomInterval ();
		if (movementCounter % 6 == 0) {
			setFullCloseToPoint ();
		} else if (movementCounter % 2 == 0) {
			setRandomOpenToPoint ();
		} else {
			setRandomCloseToPoint ();
		}
		goingUp = !goingUp;
		movementCounter++;
		return new Vector3 (transform.position.x, targetY, transform.position.z);
	}

	protected override Tween DoTargetAction ()
	{
		sparks [Random.Range (0, sparks.Length-1)].Play ();
		return null;
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player")) {
			coll.GetComponent<BallDestroyer>().Crush ();
			DOTween.KillAll ();
			transform.DOMove (new Vector3(transform.position.x, fullCloseY, transform.position.z), 0.2f).Play ();
		}
	}

	
	protected override Tween LookAtTarget (Vector3 target)
	{
		return null;
	}

	
	private void setRandomCloseToPoint() {
		targetY = Random.Range (fullCloseY + 0.75f, targetY);
	}
	
	private void setRandomOpenToPoint() {
		targetY = Random.Range (targetY, fullOpenY);
	}
	
	private void setFullCloseToPoint() {
		targetY = fullCloseY;
	}
	
	private void setRandomInterval () {
		interval = Random.Range (0.5f * interval, interval);
	}
}
