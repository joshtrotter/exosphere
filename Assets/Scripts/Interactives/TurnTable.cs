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
	//maximum degree difference the turntable will turn. Values close to or greater than 180 have no effect and will allow the turntable to keep turning indefinitely
	public float maximumTurnAngle = 180f;


	void Awake(){
		baseYRot = transform.localEulerAngles.y;
	}
	
	public override void Apply(float pressureAmount){
		if (pressureAmount > Mathf.Min (lastPressureAmount, 0.95f)) {
			if (lockingCoroutine != null) {
				StopCoroutine (lockingCoroutine);
				lockingCoroutine = null;
			}
			float newAngle = Time.deltaTime * pressureAmount * degreesPerSecond * (pressureForClockwiseRotation ? 1 : -1);
			if (180 - Mathf.Abs((Mathf.Abs(newAngle + transform.localEulerAngles.y - baseYRot) % 360) - 180) > maximumTurnAngle){
				Vector3 newRot = transform.localEulerAngles;
				newRot.y = baseYRot + (maximumTurnAngle * (pressureForClockwiseRotation ? 1 : -1));
				transform.localRotation = Quaternion.Euler (newRot);
			} else {
				transform.localRotation = Quaternion.Euler (transform.localEulerAngles + new Vector3(0f, newAngle, 0f));
			}
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
		//Debug.Log ((baseYRot + Mathf.Round ((targetAngle.y - baseYRot) / lockingIncrement) * lockingIncrement));
		targetAngle.y = (baseYRot + Mathf.Round((targetAngle.y - baseYRot) / lockingIncrement) * lockingIncrement) % 360;
		RegisterStateChange (Mathf.RoundToInt (targetAngle.y / lockingIncrement));
		Debug.Log ("Turntable locking to " + targetAngle);
		Debug.Log ("Setting state to " + Mathf.RoundToInt (targetAngle.y / lockingIncrement));
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

	public override void ReloadState (int state){
		transform.localEulerAngles = transform.localEulerAngles + new Vector3 (0f, state * lockingIncrement, 0f);
	}
}
