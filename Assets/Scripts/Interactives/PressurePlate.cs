using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour {

	public PressureReceiver[] receivers;

	public float plateDepressionTime = 0.25f;
	public float plateDepressionHeight = 0.73f;
	public float plateReleaseDelay = 2f;

	private Transform plate;
	private IEnumerator pressureCoroutine;
	private float currentPressure;

	void Awake() {
		plate = transform.FindChild ("Plate");
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player")) {
			if (isHeavyEnough(coll)) {
				if (pressureCoroutine != null) {
					StopCoroutine(pressureCoroutine);
				}
				pressureCoroutine = applyPressure(coll);
				StartCoroutine(pressureCoroutine);
			}
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.CompareTag ("Player") && isHeavyEnough(coll)) {
			stopApplyingPressure ();
		}
	}

	private IEnumerator applyPressure(Collider coll) {
		while (isHeavyEnough(coll)) {
			yield return new WaitForFixedUpdate();
			if (currentPressure < 1f) {
				currentPressure += Mathf.Clamp01(Time.deltaTime / plateDepressionTime);
				setPlateHeightByPressure();
			}
			foreach (PressureReceiver receiver in receivers) {
				receiver.Apply(currentPressure);
			}
		}
		//if the transform is lost, stop applying pressure
		stopApplyingPressure ();
	}

	private IEnumerator releasePressure() {
		yield return new WaitForSeconds (plateReleaseDelay);
		while (currentPressure > 0) {
			currentPressure -= Mathf.Clamp01(Time.deltaTime / plateDepressionTime);
			setPlateHeightByPressure();
			foreach (PressureReceiver receiver in receivers) {
				receiver.Apply(currentPressure);
			}
			yield return new WaitForFixedUpdate();
		}
	}

	private void setPlateHeightByPressure() {
		plate.localPosition = Vector3.up * -(currentPressure * plateDepressionHeight);
	}

	private void stopApplyingPressure ()
	{
		if (pressureCoroutine != releasePressure()) {
			StopCoroutine (pressureCoroutine);
		}
		pressureCoroutine = releasePressure ();
		StartCoroutine (pressureCoroutine);
	}

	private bool isHeavyEnough(Collider coll){
		//for now, assume coll is the ball and see if it has the heavy transform
		//TODO this could be updated to allow other 'heavy' objects to apply pressure as well
		return (coll.gameObject.GetComponent<TransformController> ().currentTransform.GetType () == typeof(HeavyTransform));
	}
}
