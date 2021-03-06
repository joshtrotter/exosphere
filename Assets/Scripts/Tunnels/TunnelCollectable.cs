﻿using UnityEngine;
using System.Collections;

public abstract class TunnelCollectable : MonoBehaviour {

	//affects likelihood of this collectable being chosen over other collectables
	public float weight = 1f;
	//how many of this collectable it is possible to have spawned at once
	public int numInstances = 1;
	private Vector3 startPos;
	private Quaternion startRot;

	protected virtual void Awake(){
		this.startPos = this.transform.localPosition;
		this.startRot = this.transform.localRotation;
	}

	public Vector3 GetStartPos(){
		return startPos;
	}

	public Quaternion GetStartRot(){
		return startRot;
	}

	//after it has been used, return the piece to pool
	public virtual void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			Collect(other.gameObject);
		}
	}
	
	private void Collect(GameObject player){
		ApplyCollectableEffect (player);
		PlayCollectVisuals();
		//the collectable slot will take care of returning the collectable to the pool when it is disabled
	}

	protected abstract void ApplyCollectableEffect(GameObject player);

	protected abstract void PlayCollectVisuals();

	//use to reset position etc.
	public virtual void Reset(){

	}

}
