using UnityEngine;
using System.Collections;

public class DeadBallCollider : MonoBehaviour {

	private LevelManager levelManager;

	void OnTriggerEnter(Collider coll) 
	{
		if (coll.CompareTag("Player")) {
			LevelManager.manager.ReloadLevel();
		}
	}
}
