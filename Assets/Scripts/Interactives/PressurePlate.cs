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
			if (coll.gameObject.GetComponent<TransformController>().currentTransform.GetType() == typeof(HeavyTransform)) {
				if (pressureCoroutine != null) {
					StopCoroutine(pressureCoroutine);
				}
				pressureCoroutine = applyPressure();
				StartCoroutine(pressureCoroutine);
			}
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.CompareTag ("Player")) {
			if (pressureCoroutine != null) {
				StopCoroutine(pressureCoroutine);
			}
			pressureCoroutine = releasePressure();
			StartCoroutine(pressureCoroutine);
		}
	}

	private IEnumerator applyPressure() {
		while (true) {
			yield return new WaitForFixedUpdate();
			if (currentPressure < 1f) {
				currentPressure += Mathf.Clamp01(Time.deltaTime / plateDepressionTime);
				setPlateHeightByPressure();
			}
			foreach (PressureReceiver receiver in receivers) {
				receiver.Apply(currentPressure);
			}
		}
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

}
