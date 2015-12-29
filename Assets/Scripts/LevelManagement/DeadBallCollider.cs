using UnityEngine;
using System.Collections;

public class DeadBallCollider : MonoBehaviour {

	private LevelManager levelManager;

	void OnTriggerEnter(Collider coll) 
	{
		if (coll.CompareTag("Player")) {
			GetLevelManager().ReloadLevel();
		}
	}

	LevelManager GetLevelManager() {
		if (levelManager == null) {
			levelManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager>();
		}
		return levelManager;
	}
}
