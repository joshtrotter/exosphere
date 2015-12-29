using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public float cameraAngle;
	private LevelManager levelManager;
		
	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player")) {
			ResetSpawnPoint();
			ResetCamera();
			ResetPlayer (coll.gameObject);
		}
	}

	private void ResetSpawnPoint() {
		GetLevelManager().SetSpawnLocation(this.transform);
	}

	private void ResetCamera() {
		GetLevelManager().SetCameraRotation(this.cameraAngle);
	}

	private void ResetPlayer(GameObject ball) {
		ball.GetComponent<TransformController> ().RemoveCurrent ();
	}

	LevelManager GetLevelManager() {
		if (levelManager == null) {
			levelManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager>();
		}
		return levelManager;
	}
}
