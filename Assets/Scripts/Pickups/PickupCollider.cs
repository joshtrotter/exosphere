using UnityEngine;
using System.Collections;
using DG.Tweening;

/**
 * Adds the associated pickup item to the player inventory whenever the player collides with this gameObject
 */ 
public class PickupCollider : MonoBehaviour {

	public Pickup[] pickups;

	public float flyingSpeed = 10f;
	public float respawnTime = 15f;

	private Transform item;
	private Collider coll;
	private ParticleSystem hoverEffect;
	private ParticleSystem collectEffect;
	
	void Awake() {
		this.item = transform.FindChild ("Item");
		this.coll = GetComponent<Collider> ();
		this.hoverEffect = transform.FindChild ("HoverEffect").GetComponent<ParticleSystem>();
		this.collectEffect = transform.FindChild ("CollectEffect").GetComponent<ParticleSystem>();
	}
	
	void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag("Player")) {
			this.coll.enabled = false;
			PickupController pickupController = coll.gameObject.GetComponent<PickupController>();
			foreach (Pickup pickup in pickups) {
				pickup.Reset();
				pickupController.AddPickup(pickup);
			}
			Collect ();
		}
	}

	private void Collect() {
		collectEffect.Play();
		hoverEffect.Stop();
		item.DOMoveY (transform.position.y + flyingSpeed * collectEffect.duration, collectEffect.duration).OnComplete(OnCollectionComplete).Play();
	}
	
	private void OnCollectionComplete() {
		StartCoroutine (WaitForRespawn());
	}

	private IEnumerator WaitForRespawn() {
		item.gameObject.SetActive(false);
		item.position = new Vector3 (item.position.x, item.position.y - flyingSpeed * collectEffect.duration, item.position.z);
		yield return new WaitForSeconds (respawnTime);
		this.coll.enabled = true;
		item.gameObject.SetActive (true);
		hoverEffect.Play ();
	}
}
