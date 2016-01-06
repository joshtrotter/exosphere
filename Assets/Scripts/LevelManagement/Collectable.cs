using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Collectable : HasLevelState {

	private const int NOT_COLLECTED_STATE = 0;
	private const int COLLECTED_STATE = 1;

	public Vector3 rotationVector;
	public float rotationsPerSecond = 1f;
	public float flyingSpeed = 10f;
	
	private ParticleSystem hoverEffect;
	private ParticleSystem collectEffect;
	private Collider coll;
	private Transform item;

	void Awake() {
		this.coll = GetComponent<Collider> ();
		this.item = transform.FindChild ("Item");
		this.hoverEffect = transform.FindChild ("HoverEffect").GetComponent<ParticleSystem>();
		this.collectEffect = transform.FindChild ("CollectEffect").GetComponent<ParticleSystem>();
	}

	void Update() {
		if (currentState == NOT_COLLECTED_STATE) {
			item.RotateAround (coll.bounds.center, rotationVector, rotationsPerSecond * 360f * Time.deltaTime);
		}
	}

	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag("Player")) {
			RegisterStateChange(COLLECTED_STATE);
			GetLevelManager().RemoveCollectable();
			Collect ();
		}
	}

	public override void ReloadState(int state) {
		if (state == COLLECTED_STATE) {
			OnCollectionComplete ();
		}
	}

	private void Collect() {
		collectEffect.Play();
		hoverEffect.Stop();
		item.DOMoveY (transform.position.y + flyingSpeed * collectEffect.duration, collectEffect.duration).OnComplete(OnCollectionComplete).Play();
	}

	private void OnCollectionComplete() {
		GetLevelManager ().GetComponentInChildren<HUD>().SendMessage ("CollectableFound");
		gameObject.SetActive(false);
	}
}
