using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TunnelPiecePool : MonoBehaviour {

	public static TunnelPiecePool INSTANCE;
//	private static Vector3 poolPos = new Vector3(0, 0, -1000);

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

	public TunnelPiece takeRandomPieceFromPool() {
		int index = Random.Range (0, (availablePool.Count));
		TunnelPiece piece = availablePool [index];
		availablePool.RemoveAt (index);
		Debug.Log ("Selecting " + piece.name);
		return piece;
	}

	public void returnToPool(TunnelPiece piece) {
//		piece.transform.position = poolPos;
		piece.gameObject.SetActive(false);
		availablePool.Add (piece);
	}

	private void spawnNewInstance(TunnelPiece template) {
		TunnelPiece instance = Instantiate<TunnelPiece> (template);
		returnToPool (instance);
	}

	
	//called after a selector rejects too many pieces to avoid infinite loops. Returns first in pool and hopes for the best
	public TunnelPiece takeSafePiece(){
		TunnelPiece piece = availablePool [0];
		availablePool.RemoveAt (0);
		Debug.Log ("Selecting " + piece.name);
		return piece;
	}
	
}
