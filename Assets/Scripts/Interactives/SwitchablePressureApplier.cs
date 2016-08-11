using UnityEngine;
using System.Collections;

public class SwitchablePressureApplier : SwitchableObject {

	public PressureReceiver[] receivers;
	public float timeToFullPressure;
	public bool IsActive = false;

	private IEnumerator pressureCoroutine;
	private float currentPressure;

	public void Awake(){
		IsActive = !IsActive;
		Activate ();
	}

	public override void Activate ()
	{
		IsActive = !IsActive;
		if (pressureCoroutine != null) {
			StopCoroutine(pressureCoroutine);
		}
		if (IsActive) {
			pressureCoroutine = applyPressure();
		} else {
			Debug.Log("starting release pressure routine");
			pressureCoroutine = releasePressure();
		}
		StartCoroutine(pressureCoroutine);
	}

	private IEnumerator applyPressure() {
		while (true) {
			yield return new WaitForFixedUpdate();
			if (currentPressure < 1f) {
				currentPressure += Mathf.Clamp01(Time.deltaTime / timeToFullPressure);
			}
			foreach (PressureReceiver receiver in receivers) {
				receiver.Apply(currentPressure);
			}
		}
	}
	
	private IEnumerator releasePressure() {
		while (currentPressure > 0) {
			currentPressure -= Mathf.Clamp01(Time.deltaTime / timeToFullPressure);
			foreach (PressureReceiver receiver in receivers) {
				receiver.Apply(currentPressure);
			}
			yield return new WaitForFixedUpdate();
		}
	}
}
