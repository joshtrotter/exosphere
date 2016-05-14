using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TunnelCollectableSpawner : MonoBehaviour {

	public static TunnelCollectableSpawner INSTANCE;
	public TunnelCollectable[] collectablePrefabs;

	private List<TunnelCollectable> availablePool;

	void Awake(){
		if (INSTANCE != null) {
			Destroy(gameObject);
		} else {
			INSTANCE = this;
		}

		initPool ();
	}
	
	private void initPool() {
		availablePool = new List<TunnelCollectable> ();
		for (int i = 0; i < collectablePrefabs.Length; i++) {
			TunnelCollectable template = collectablePrefabs[i];
			for (int j = 0; j < template.numInstances; j++) {
				spawnNewInstance(template);
			}
		} 
	}

	//a piece can request a collectable at a specified position, and if there is one available it will be spawned in
	public void requestCollectable(TunnelCollectableSlot slot){
		if (availablePool.Count > 0) {
			TunnelCollectable tc = chooseCollectable();
			spawnCollectable(tc, slot);
		} else { //unlikely but possible
			Debug.Log ("No available collectables left. Ignoring request.");
		}
	}

	private void spawnCollectable(TunnelCollectable tc, TunnelCollectableSlot slot){
		tc.Reset ();
		slot.PlaceCollectable (tc);
		tc.gameObject.SetActive (true);
	}

	private TunnelCollectable chooseCollectable(){
		float[] cumulativeSumOfWeights = new float[availablePool.Count];
		float weightSum = 0;
		for (int i = 0; i < availablePool.Count; i++) {
			TunnelCollectable tc = availablePool[i];
			weightSum += tc.weight;
			cumulativeSumOfWeights[i] = weightSum;
		}
		
		//Randomly select a collectable based upon the weights calculated above
		float rand = Random.Range (0f, weightSum);
		for (int i = 0; i < availablePool.Count; i++) {
			if (cumulativeSumOfWeights[i] >= rand) {
				TunnelCollectable chosen = takeFromPool(i);
				return chosen;
			}
		}

		//this shouldn't happen
		return takeFromPool (0);
	}
	
	public void returnToPool(TunnelCollectable collectable) {
		collectable.gameObject.SetActive(false);
		availablePool.Add (collectable);
	}
	
	private TunnelCollectable takeFromPool(int index) {
		TunnelCollectable collectable = availablePool [index];
		availablePool.RemoveAt (index);
		return collectable;
	}
	
	private void spawnNewInstance(TunnelCollectable template) {
		TunnelCollectable instance = Instantiate<TunnelCollectable> (template);
		returnToPool (instance);
	}
	
}

