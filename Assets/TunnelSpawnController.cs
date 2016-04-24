using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TunnelSpawnController : MonoBehaviour {

	public static TunnelSpawnController INSTANCE;
	public int minTunnelLength = 100;

	private LinkedList<TunnelPiece> tunnel = new LinkedList<TunnelPiece>();
	
	void Awake() {
		if (INSTANCE != null) {
			Destroy(gameObject);
		} else {
			INSTANCE = this;
		}

		tunnel.AddFirst(GameObject.FindGameObjectWithTag("FirstPipe").GetComponent<TunnelPiece>());
		tunnel.AddLast(GameObject.FindGameObjectWithTag("SecondPipe").GetComponent<TunnelPiece>());
	}

	// Use this for initialization
	void Start () {
		extendTunnelEnd();
	}

	public void OnTunnelPieceEntry() {
		trimTunnelStart ();
		extendTunnelEnd ();
	}

	private void trimTunnelStart() {
		TunnelPiece toTrim = tunnel.First.Value;
		tunnel.RemoveFirst ();
		Debug.Log ("Destorying " + toTrim.GetInstanceID ());
		DestroyObject (toTrim.gameObject);
	}

	private void extendTunnelEnd() {
		while (tunnelLength() < minTunnelLength) {
			tunnel.AddLast (tunnel.Last.Value.spawnChildPiece());
		}
	}

	private int tunnelLength() {
		int length = 0;
		foreach (TunnelPiece piece in tunnel) {
			length += piece.pieceLength;
		}
		return length;
	}
}
