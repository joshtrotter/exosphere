using UnityEngine;
using DG.Tweening;
using System.Collections;

public class TurnTable : PressureReceiver {

	private float baseYRot;
	private float lastPressureAmount;
	private IEnumerator lockingCoroutine;

	//direction the turntable turns
	public bool pressureForClockwiseRotation;
	//how quickly the turntable will turn when the pressure plate is fully down
	public float degreesPerSecond = 45f;
	//degree increments from start rotation to which the turn table will lock to after the player gets off the pressure plate
	public float lockingIncrement = 90f;


	void Awake(){
		baseYRot = transform.localEulerAngles.y;
	}
	
	public override void Apply(float pressureAmount){
		if (pressureAmount > Mathf.Min (lastPressureAmount, 0.95f)) {
			if (lockingCoroutine != null) {
				StopCoroutine (lockingCoroutine);
				lockingCoroutine = null;
			}
			Vector3 newAngle = transform.localEulerAngles;
			newAngle.y += Time.deltaTime * pressureAmount * degreesPerSecond * (pressureForClockwiseRotation ? 1 : -1);
			transform.localRotation = Quaternion.Euler (newAngle);
		} else {
			if (lockingCoroutine == null){
				lockingCoroutine = LockTo90Degrees ();
				StartCoroutine (lockingCoroutine);
			}
		}
		lastPressureAmount = pressureAmount;
	}

	private IEnumerator LockTo90Degrees(){
		Vector3 targetAngle = transform.localEulerAngles;
		targetAngle.y = (baseYRot + Mathf.Round((targetAngle.y - baseYRot) / lockingIncrement) * lockingIncrement) % 360;
		Debug.Log ("Turntable locking to " + targetAngle);
		while (Mathf.Abs(transform.localEulerAngles.y - targetAngle.y) > Mathf.Epsilon) {
			transform.localEulerAngles = Vector3.RotateTowards(transform.localEulerAngles, targetAngle, Time.deltaTime, 1f);
			yield return new WaitForFixedUpdate ();
		}
		transform.localEulerAngles = targetAngle;
	}

	void OnCollisionStay(Collision coll){
		coll.transform.parent = this.transform;
	}
	
	void OnCollisionExit(Collision coll){
		coll.transform.parent = null;
	}
}
