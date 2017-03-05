using UnityEngine;
using System.Collections;

public class CameraEffectsController : MonoBehaviour {

	public GameObject[] cameraEffects;

	public void disableEffect(string name) {
		toggleEffect (name, false);
	}

	public void enableEffect(string name) {
		toggleEffect (name, true);
	}

	public void disableEffects() {
		toggleEffects (false);
	}
	
	public void enableEffects() {
		toggleEffects (true);
	}

	private void toggleEffect(string name, bool active) {
		foreach (GameObject go in cameraEffects) {
			if (go.name.Equals(name)) {
				toggleEmission(go, active);
			}
		}
	}

	private void toggleEffects(bool active) {
		foreach (GameObject go in cameraEffects) {
			toggleEmission(go, active);
		}
	}

	private void toggleEmission(GameObject go, bool active) {
		foreach (ParticleSystem ps in go.GetComponentsInChildren<ParticleSystem>()) {
			if (!active) {
				ps.Stop();
			} else {
				ps.Play();
			}
		}
	}
}
