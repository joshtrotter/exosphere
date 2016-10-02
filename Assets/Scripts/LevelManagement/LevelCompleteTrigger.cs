using UnityEngine;
using System.Collections;

public class LevelCompleteTrigger : MonoBehaviour {
	
	public float delay = 0f;
	public bool freezeCam = false;

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player")){
			StartCoroutine (CompleteLevel(coll));
		}
	}

	private IEnumerator CompleteLevel(Collider ball){
		if (freezeCam) {
			AmazeballCam cam = GameObject.FindGameObjectWithTag("CameraRig").GetComponent<AmazeballCam>();
			cam.moveSpeed = 0f;
		}
		yield return new WaitForSeconds(delay);
		LevelCompleteScreen.controller.LevelComplete ();
	}
}
