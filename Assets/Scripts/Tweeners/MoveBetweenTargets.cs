using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class MoveBetweenTargets : MonoBehaviour {

	public List<Vector3> targetPositions;
	public float timeToLook = 1f;
	public float timeToMove = 1f;
	public float interval = 1f; 
	public bool playOnAwake = true;
	public bool loop = true;

	private Vector3 previousTarget;
	private Vector3 currentTarget;
	
	protected virtual void Awake() {
		for (int i = 0; i < targetPositions.Count; i++) {
			targetPositions[i] += transform.position;
		}

		targetPositions.Add (transform.position);
		currentTarget = targetPositions [targetPositions.Count - 1];
	}

	// Use this for initialization
	protected void Start () {
		if (playOnAwake) {
			if (loop) {
				LoopSequence ();
			} else {
				SingleSequence ();
			}
		}
	}

	public void SingleSequence() {
		Sequence ();
	}

	public void LoopSequence() {
		Sequence ().OnComplete(LoopSequence);
	}

	protected virtual Tween Sequence() {
		//Get a new target
		previousTarget = currentTarget;
		while (currentTarget == previousTarget) {
			currentTarget = FindNewTarget ();
		}

		//Sequence up a new look, move, action cycle
		Sequence sequence = DOTween.Sequence ()
			.AppendInterval (interval)
			.Append (LookAtTarget (currentTarget))
			.AppendInterval (interval)
			.Append (MoveToTarget (currentTarget))
				.AppendInterval (interval);

		Tween targetAction = DoTargetAction ();
		if (targetAction != null) {
			sequence.Append (targetAction);
		}

		return sequence;

	}

	protected virtual Tween LookAtTarget(Vector3 target) {
		return transform.DOLookAt (target, timeToLook);
	}

	protected virtual Tween MoveToTarget(Vector3 target) {
		return transform.DOMove (target, timeToMove * (Vector3.Distance (transform.position, target) / 8f));
	}

	protected virtual Tween DoTargetAction() {
		return null;
	}

	protected virtual Vector3 FindNewTarget() {
		return targetPositions [Random.Range (0, targetPositions.Count)];
	}

}
