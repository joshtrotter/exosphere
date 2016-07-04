using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TunnelPickupCollectable : TunnelCollectable {

	public Pickup[] pickups;
	
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
	}

	
	protected override void ApplyCollectableEffect(GameObject player){
		PickupController pickupController = player.GetComponent<PickupController>();
		foreach (Pickup pickup in pickups) {
			pickup.Reset();
			pickupController.AddPickup(pickup);
		}
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

