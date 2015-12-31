using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public float cameraAngle;
	public float bounceHeight = 0.25f;
	public float growToScale = 1.5f;
	public float growTime = 0.25f;

	private static LevelManager levelManager;
	private Transform bubbleEffect;

	void Awake() {
		bubbleEffect = transform.FindChild ("BubbleEffect");
	}
		
	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player")) {
			ResetSpawnPoint();
			ResetCamera();
			ResetPlayer (coll.gameObject);
			VisualizeCheckpoint();
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

	private void VisualizeCheckpoint() {
		DOTween.Sequence ()
			.Append (bubbleEffect.DOMoveY (bubbleEffect.position.y + bounceHeight, growTime / 2))
				.Append (bubbleEffect.DOScale (growToScale, growTime))
				.AppendInterval (growTime)
				.Append (bubbleEffect.DOScale (1f, growTime))
				.Append (bubbleEffect.DOMoveY (bubbleEffect.position.y, growTime / 2));
			
	}

	private static LevelManager GetLevelManager() {
		if (levelManager == null) {
			levelManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager>();
		}
		return levelManager;
	}
}
