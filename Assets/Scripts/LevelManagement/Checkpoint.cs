using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Checkpoint : HasLevelState {

	private const int NOT_ACTIVATED_STATE = 0;
	private const int ACTIVATED_STATE = 1;

	public float cameraAngle;
	public float bounceHeight = 0.25f;
	public float growToScale = 1.5f;
	public float growTime = 0.25f;

	private static LevelManager levelManager;
	private Transform bubbleEffect;

	void Awake() {
		bubbleEffect = transform.FindChild ("BubbleEffect");
	}

	public override void ReloadState(int state) {
		currentState = state;
		if (state == ACTIVATED_STATE) {
			bubbleEffect.gameObject.SetActive(false);
		}
	}
		
	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player")) {
			ResetSpawnPoint();
			ResetCamera();
			if (currentState == NOT_ACTIVATED_STATE) {
				RegisterStateChange(ACTIVATED_STATE);
				HUD.controller.SendMessage("CheckpointReached");
				VisualizeCheckpoint();
			}
			SaveLevelProgress();
		}
	}

	private void SaveLevelProgress(){
		LevelManager.manager.UpdateLevelProgress ();
	}

	private void ResetSpawnPoint() {
		LevelManager.manager.SetSpawnLocation(this.transform.position + new Vector3(0,0.5f,0));
	}

	private void ResetCamera() {
		LevelManager.manager.SetCameraRotation(this.cameraAngle);
	}

	//no longer used
	private void ResetPlayer(GameObject ball) {
		ball.GetComponent<TransformController> ().RemoveCurrent ();
	}

	private void VisualizeCheckpoint() {
		DOTween.Sequence ()
			.Append (bubbleEffect.DOMoveY (bubbleEffect.position.y + bounceHeight, growTime / 2))
				.Append (bubbleEffect.DOScale (growToScale, growTime))
				.AppendInterval (growTime)
				.Append (bubbleEffect.DOScale (1f, growTime))
				.Append (bubbleEffect.DOMoveY (bubbleEffect.position.y, growTime / 2))
				.AppendInterval(growTime)
				.Append (bubbleEffect.DOScale (0f, growTime * 2f))
				.AppendCallback(() => bubbleEffect.gameObject.SetActive(false));
			
	}
}
