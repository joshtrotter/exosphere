using UnityEngine;
using System.Collections;

public class TransformCollider : MonoBehaviour {

	public BallTransform ballTransform;
	private LevelManager levelManager;
	
	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag("Player")) {
			TransformController transformController = coll.gameObject.GetComponent<TransformController>();
			transformController.ApplyTransform(ballTransform);
			GetLevelManager().GetComponentInChildren<HUD>().SendMessage("MorphApplied", ballTransform);
		}
	}

	private LevelManager GetLevelManager(){
		if (levelManager == null) {
			levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
		}
		return levelManager;
	}
}
