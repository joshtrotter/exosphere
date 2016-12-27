
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FaultyDoor : MonoBehaviour {



	public float maxTimeOpen = 2.0f;
	public float minTimeOpen = 1.0f;
	public float triggerWarningTime = 0.25f;

	public float minWobbleHeight = 1.5f;
	public float maxWobbleHeight = 2.25f;
	public float openHeight = 2.70f;

	public float wobbleWaitTime = 0.5f;
	public float wobbleDropTime = 0.2f;
	public float wobbleRaiseTime = 0.5f;
	public float shutWaitTime = 0.5f;
	public float shutDropTime = 0.4f;
	public float shutRaiseTime = 1f;

	public int percentChanceOfSlam = 50;

	private float bottomY;
	
	ParticleSystem[] sparks;

	void Awake () {
		bottomY = transform.position.y;
		sparks = GetComponentsInChildren<ParticleSystem> ();
	}

	void Start () {
		transform.DOMoveY (bottomY + openHeight, shutDropTime).Play();
		ScheduleAction ();
	}

	private void ScheduleAction() {
		if (Random.Range (1, 101) <= percentChanceOfSlam) {
			SlamShut();
		} else { 
			Wobble();
		}
	}

	private void Wobble() {
		SequenceWobble ().OnComplete (ScheduleAction);
	}

	private void SlamShut() {
		SequenceSlamShut ().OnComplete (ScheduleAction);
	}
	
	private Tween SequenceWobble() {	
		//Move the door to open, wait a while, then trigger the next action
		Sequence sequence = DOTween.Sequence ()
			.AppendInterval (Random.Range (minTimeOpen, maxTimeOpen))
				.AppendCallback (TriggerWarning)
				.AppendInterval (triggerWarningTime)
				.Append (MoveToTarget (Random.Range (bottomY + minWobbleHeight, bottomY + maxWobbleHeight), wobbleDropTime))
				.AppendInterval (wobbleWaitTime)
				.Append (MoveToTarget(bottomY + openHeight, wobbleRaiseTime));
			
		return sequence;
	}

	private Tween SequenceSlamShut() {	
		//Move the door to open, wait a while, then trigger the next action
		Sequence sequence = DOTween.Sequence ()
			.AppendInterval (Random.Range (minTimeOpen, maxTimeOpen))
				.AppendCallback (TriggerWarning)
				.AppendInterval (triggerWarningTime)
				.Append (MoveToTarget (bottomY, shutDropTime))
				.AppendInterval (shutWaitTime)
				.Append (MoveToTarget(bottomY + openHeight, shutRaiseTime));
		
		return sequence;
	}

	private void TriggerWarning() {
		Sparks (1);
	}

	private Tween MoveToTarget (float targetY, float time) {
		Sparks (1);
		return transform.DOMoveY (targetY, time);
	}

	private void Sparks(int howMany) {
		for (int i = 0; i < howMany; i++) {
			sparks [Random.Range (0, sparks.Length-1)].Play ();
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.CompareTag ("Player")) {
			coll.GetComponent<BallDestroyer>().Crush ();
			DOTween.KillAll ();
			transform.DOMove (new Vector3(transform.position.x, bottomY, transform.position.z), 0.2f).Play ();
		}
	}
	
}
