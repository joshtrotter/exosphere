using UnityEngine;
using System.Collections;

public class CameraEffectsTrigger : MonoBehaviour {

	private CameraEffectsController cameraRig;

	public bool enableEffectsOnEntry = false;
	public bool disableEffectsOnEntry = false;
	public bool enableEffectsOnExit = false;
	public bool disableEffectsOnExit = false;
	public bool destroyTriggerAfterEntry = false;
	public bool destroyTriggerAfterExit = false;
	public string[] effectsToAffect = new string[0];
	
	void Awake() {
		cameraRig = GameObject.FindGameObjectWithTag ("CameraRig").GetComponent<CameraEffectsController> ();
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("CameraRig")) {
			if (enableEffectsOnEntry) {
				enableEffects ();
			}
			if (disableEffectsOnEntry) {
				disableEffects ();
			}
			if (destroyTriggerAfterEntry) {
				this.gameObject.SetActive(false);
			}
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.CompareTag ("CameraRig")) {
			if (enableEffectsOnExit) {
				enableEffects ();
			}
			if (disableEffectsOnExit) {
				disableEffects ();
			}
			if (destroyTriggerAfterExit) {
				this.gameObject.SetActive(false);
			}
		}
	}

	private void enableEffects() {
		if (effectsToAffect != null && effectsToAffect.Length > 0) {
			foreach (string effect in effectsToAffect) {
				cameraRig.enableEffect(effect);
			}
		} else {
			cameraRig.enableEffects();
		}
	}

	private void disableEffects() {
		if (effectsToAffect != null && effectsToAffect.Length > 0) {
			foreach (string effect in effectsToAffect) {
				cameraRig.disableEffect(effect);
			}
		} else {
			cameraRig.disableEffects();
		}
	}
	
}
