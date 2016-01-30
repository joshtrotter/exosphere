using UnityEngine;
using System.Collections;

public class TransformCollider : SwitchableObject {

	public BallTransform ballTransform;
	public GameObject morph;
	public float respawnTime = 5f;

	void OnTriggerStay (Collider coll) {
		if (coll.CompareTag("Player") && morph.activeSelf) {
			TransformController transformController = coll.gameObject.GetComponent<TransformController>();
			if (transformController.currentTransform != ballTransform){
				transformController.ApplyTransform(ballTransform);
				StartCoroutine(WaitForRespawn());
			}
		}
	}

	public override void Activate ()
	{
		if (morph.activeSelf) {
			ChamberOff ();
		} else {
			ChamberOn();
		}
	}

	private IEnumerator WaitForRespawn() {
		ChamberOff ();
		yield return new WaitForSeconds (respawnTime);
		ChamberOn ();
	}

	private void ChamberOff() {
		morph.SetActive(false);
	}

	private void ChamberOn() {
		morph.SetActive (true);
	}
}
