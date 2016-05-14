using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TunnelPointBoosterCollectable : TunnelCollectable {

	public int scoreBoost = 500;

	public Vector3 rotationVector;
	public float rotationsPerSecond = 1f;
	public float flyingSpeed = 10f;
	
	private TunnelScoreController scorer;

	private ParticleSystem hoverEffect;
	private ParticleSystem collectEffect;
	private Collider coll;
	private Transform item;
	
	void Awake() {
		this.coll = GetComponent<Collider> ();
		this.item = transform.FindChild ("Item");
		this.hoverEffect = transform.FindChild ("HoverEffect").GetComponent<ParticleSystem>();
		this.collectEffect = transform.FindChild ("CollectEffect").GetComponent<ParticleSystem>();
		this.scorer = GameObject.FindObjectOfType<TunnelScoreController>();
	}

	void Update() {
		if (isActiveAndEnabled){
			item.RotateAround (coll.bounds.center, rotationVector, rotationsPerSecond * 360f * Time.deltaTime);
		}
	}

	protected override void ApplyCollectableEffect(){
		PopupController.controller.Message ("Bonus Collected! +" + scoreBoost);
		scorer.updateScore (scoreBoost, false);
	}
	
	protected override void PlayCollectVisuals(){
		collectEffect.Play();
		hoverEffect.Stop();
		item.DOMoveY (transform.position.y + flyingSpeed * collectEffect.duration, collectEffect.duration).Play();
	}

	public override void Reset(){
		base.Reset ();
		item.localPosition = Vector3.zero;
		hoverEffect.Play ();
	}
}
