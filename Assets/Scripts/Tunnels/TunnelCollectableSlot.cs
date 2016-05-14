using UnityEngine;
using System.Collections;

public class TunnelCollectableSlot : MonoBehaviour {

	//this is the spawn chance (between 0 and 1) that a collectable will spawn in this spot once it has already been chosen (with even probability) out of all other active slots on this piece
	public float spawnChance = 0.5f;
	//this determines whether the collectable will be parented (and move with) this slot, or whether it will simply be placed at the slot's original position
	public bool moveCollectableWithSelf = true;

	private TunnelCollectable currentCollectable;

	public void ConsiderSpawning(){
		if (Random.Range (0f, 1f) <= spawnChance) {
			TunnelCollectableSpawner.INSTANCE.requestCollectable(this);
		}
	}

	public void PlaceCollectable(TunnelCollectable collectable){
		currentCollectable = collectable;
		if (moveCollectableWithSelf) {
			currentCollectable.transform.parent = this.transform;
			currentCollectable.transform.localPosition = Vector3.zero;
		} else {
			currentCollectable.transform.localPosition = this.transform.position;
		}
	}

	void OnDisable(){
		if (currentCollectable != null) {
			TunnelCollectableSpawner.INSTANCE.returnToPool (currentCollectable);
			currentCollectable = null;
		}
	}
}
