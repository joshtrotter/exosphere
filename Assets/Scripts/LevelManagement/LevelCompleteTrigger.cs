using UnityEngine;
using System.Collections;

public class LevelCompleteTrigger : MonoBehaviour {
	
	public float delay = 0f;

	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player")){
			StartCoroutine (CompleteLevel());
		}
	}

	private IEnumerator CompleteLevel(){
		yield return new WaitForSeconds(delay);
		LevelCompleteScreen.controller.LevelComplete ();
	}
}
