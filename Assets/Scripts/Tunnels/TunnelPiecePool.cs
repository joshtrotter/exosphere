using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TunnelPiecePool : MonoBehaviour {

	public static TunnelPiecePool INSTANCE;

	public TunnelPiece[] tunnelPiecePrefabs;
	private List<TunnelPiece> availablePool;

	void Awake() {
		if (INSTANCE != null) {
			Destroy(gameObject);
		} else {
			INSTANCE = this;
		}

		initPool ();
	}

	private void initPool() {
		availablePool = new List<TunnelPiece> ();
		for (int i = 0; i < tunnelPiecePrefabs.Length; i++) {
			TunnelPiece template = tunnelPiecePrefabs[i];
			for (int j = 0; j < template.frequency; j++) {
				spawnNewInstance(template);
			}
		}
	}
		
	public TunnelPiece takeWeightedRandomPieceFromPool(TunnelSelectionPreferences prefs) {
		float[] cumulativeSumOfWeights = new float[availablePool.Count];

		//Created a cumulative sum array based on piece weights for the supplied selection preferences
		float weightSum = 0;
		string weightedPool = "";
		for (int i = 0; i < availablePool.Count; i++) {
			TunnelPiece tp = availablePool[i];
			float weight = tp.calculateWeight(prefs);
			weightSum += weight;
			cumulativeSumOfWeights[i] = weightSum;

			if (Debug.isDebugBuild) {
				weightedPool = weightedPool + tp.name + ": " + weight + "\n";
			}
		}

		//Randomly select a piece based upon the weights calculated above (note that a zero weight can never be selected so the piece taken here is already guaranteed valid)
		float rand = Random.Range (0f, weightSum);
		for (int i = 0; i < availablePool.Count; i++) {
			if (cumulativeSumOfWeights[i] >= rand) {
				TunnelPiece chosen = takeFromPool(i);
				if (Debug.isDebugBuild) {
					TunnelSpawnController.INSTANCE.debug.text = weightedPool 
						+ "Chosen Piece: " + chosen.name + "\n" 
						+ "Bucket Level: " + prefs.maxBucketLevel + "\n" 
						+ "Max Difficulty: " + prefs.maxDifficulty;
				}
				return chosen;
			}
		}

		//This shouldn't ever happen
		return takeFromPool (0);
	}

	//This method of selecting pieces is no longer used
	public TunnelPiece takeRandomPieceFromPool() {
		int index = Random.Range (0, (availablePool.Count));
		TunnelPiece piece = availablePool [index];
		availablePool.RemoveAt (index);
		return piece;
	}

	public void returnToPool(TunnelPiece piece) {
		piece.gameObject.SetActive(false);
		availablePool.Add (piece);
	}

	private TunnelPiece takeFromPool(int index) {
		TunnelPiece piece = availablePool [index];
		availablePool.RemoveAt (index);
		return piece;
	}

	private void spawnNewInstance(TunnelPiece template) {
		TunnelPiece instance = Instantiate<TunnelPiece> (template);
		instance.findBall ();
		returnToPool (instance);
	}
	
}
