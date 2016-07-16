using UnityEngine;
using System.Collections;

public class TunnelCollectableSlot : MonoBehaviour {

	//this is the spawn chance (between 0 and 1) that a collectable will spawn in this spot once it has already been chosen (with even probability) out of all other active slots on this piece
	public float spawnChance = 0.5f;
	//this determines whether the collectable will be parented (and move with) this slot, or whether it will simply be placed at the slot's original position
	//currently all colletables will move with the slot, as they are otherwise left behind when the piece flies in
	public bool moveCollectableWithSelf = true;

	private TunnelCollectable currentCollectable;

	public void ConsiderSpawning(){
		if (Random.Range (0f, 1f) <= spawnChance) {
			TunnelCollectableSpawner.INSTANCE.requestCollectable(this);
		}
	}

	public void PlaceCollectable(TunnelCollectable collectable){
		//Debug.Log ("Placing collectable");
		currentCollectable = collectable;
		//if (moveCollectableWithSelf) {
		currentCollectable.transform.parent = this.transform;
		currentCollectable.transform.localPosition = currentCollectable.GetStartPos ();
		currentCollectable.transform.localRotation = currentCollectable.GetStartRot ();
		/*} else {
			currentCollectable.transform.position = this.transform.position;
		}*/
	}

	void OnDisable(){
		if (currentCollectable != null) {
			//currentCollectable.transform.parent = null;
			TunnelCollectableSpawner.INSTANCE.returnToPool (currentCollectable);
			currentCollectable = null;
		}
	}
}
