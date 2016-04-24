using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TunnelPiecePool : MonoBehaviour {

	public static TunnelPiecePool INSTANCE;

	public TunnelPiece[] tunnelPieces;

	void Awake() {
		if (INSTANCE != null) {
			Destroy(gameObject);
		} else {
			INSTANCE = this;
		}
	}

	public TunnelPiece spawnNewInstance(TunnelPiece template) {
		return GameObject.Instantiate (template);
	}
	
}
