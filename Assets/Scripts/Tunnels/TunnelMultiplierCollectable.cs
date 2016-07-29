using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TunnelMultiplierCollectable : TunnelCollectable {

	public int multiplierIncreaseAmount;
	public float duration;

	private TunnelScoreController scorer;

	public float flyingSpeed = 10f;
	private ParticleSystem hoverEffect;
	private ParticleSystem collectEffect;
	private Transform item;
	private Tween flight;
	
	protected override void Awake() {
		base.Awake();
		this.item = transform.FindChild ("Item");
		this.hoverEffect = transform.FindChild ("HoverEffect").GetComponent<ParticleSystem>();
		this.collectEffect = transform.FindChild ("CollectEffect").GetComponent<ParticleSystem>();
		this.scorer = GameObject.FindObjectOfType<TunnelScoreController>();
	}
	
	
	protected override void ApplyCollectableEffect(GameObject player){
		PopupController.controller.Message ("x" + multiplierIncreaseAmount + " Multiplier Increase!");
		scorer.AddSpecialMultiplier (multiplierIncreaseAmount, duration);
	}
	
	protected override void PlayCollectVisuals(){
		collectEffect.Play();
		hoverEffect.Stop();
		flight = item.DOMoveY (transform.position.y + flyingSpeed * collectEffect.duration, collectEffect.duration);
		flight.Play ();
	}
	
	public override void Reset(){
		base.Reset ();
		flight.Kill ();
		item.localPosition = Vector3.zero;
		item.localEulerAngles = Vector3.zero;
		hoverEffect.Play ();
	}
}
