using UnityEngine;
using System.Collections;

public class SeeSawMorphChecker : MonoBehaviour {

	public SeeSawController seeSawController;

	private TransformController transformController;
	private Coroutine coroutine;

	void Awake() {
		transformController = GameObject.FindGameObjectWithTag ("Player").GetComponent<TransformController> ();
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player") && IsLightBall()) {
			coroutine = StartCoroutine(CheckForLossOfMorph(coll));
		}
	}
	
	void OnTriggerExit(Collider coll) {
		if (coll.CompareTag ("Player")) {
			if (coroutine != null) {
				StopCoroutine(coroutine);
				coroutine = null;
			}
		}
	}

	private IEnumerator CheckForLossOfMorph(Collider coll) {
		while (IsLightBall ()) {
			yield return new WaitForEndOfFrame();
		}
		seeSawController.ResetBias(coll);
	}

	private bool IsLightBall() {
		return transformController.currentTransform.GetType () == typeof(LightTransform);
	}
}
