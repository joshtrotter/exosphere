using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class MoveBetweenTargets2D : MonoBehaviour {
	
	public List<Vector3> targetPositions;
	public float timeToMove = 1f;
	
	private Vector3 previousTarget;
	private Vector3 currentTarget;
	private Vector3 startRotation;
	
	protected virtual void Awake() {
		/*for (int i = 0; i < targetPositions.Count; i++) {
			targetPositions[i] += transform.rotation.eulerAngles;
		}

		targetPositions.Add (transform.rotation.eulerAngles);*/
		ResetRotation ();
		currentTarget = FindNewTarget ();
	}
	
	// Use this for initialization
	protected void Start () {
		LoopSequence ();
	}

	
	public void LoopSequence() {
		//Sequence ().OnComplete(LoopSequence);
	}
	
	protected virtual Tween Sequence() {
		//Get a new target
		previousTarget = currentTarget;
		while (currentTarget == previousTarget) {
			currentTarget = FindNewTarget ();
		}
		
		//Sequence up a new look, move, action cycle
		Sequence sequence = DOTween.Sequence ()
		//.Append (MoveToTarget (currentTarget));
			.Append (RotateToTarget (currentTarget));
		return sequence;
		
	}

	public void ResetRotation(){
		startRotation = transform.rotation.eulerAngles;
	}

	protected virtual Tween RotateToTarget(Vector3 target){
		return transform.DORotate (target, timeToMove);
	}

	protected virtual Vector3 FindNewTarget() {
		return targetPositions [Random.Range (0, targetPositions.Count)] + startRotation;
	}
	
}
