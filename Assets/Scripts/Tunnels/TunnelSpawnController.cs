using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TunnelSpawnController : MonoBehaviour {

	public static TunnelSpawnController INSTANCE;
	
	public float minTunnelLength = 160;
	public Transform deadZone;
	public float distanceFactorToIncreaseBucketLevel = 100f;

	private LinkedList<TunnelPiece> tunnel = new LinkedList<TunnelPiece>();

	//A clear run is a sequence of tunnel pieces with no obstacles or speed reducing drops/twists
	private float currentClearRun;

	private float distanceTravelled = 0f;
	
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
		distanceTravelled += tunnel.First.Value.length ();
	}

	public float getCurrentClearRun() {
		return currentClearRun;
	}

	private void trimTunnelStart() {
		TunnelPiece toTrim = tunnel.First.Value;
		if (!isStarterPiece(toTrim)) {
			TunnelPiecePool.INSTANCE.returnToPool (toTrim);		 
		}
		tunnel.RemoveFirst ();
	}

	private bool isStarterPiece(TunnelPiece piece) {
		return piece.CompareTag ("FirstPipe") || piece.CompareTag ("SecondPipe");
	}

	private void extendTunnelEnd() {
		while (tunnelLength() < minTunnelLength) {
			TunnelPiece newPiece = tunnel.Last.Value.spawnChildPiece(buildPreferences());
			updateClearRun(newPiece);
			deadZone.position = deadZone.position + newPiece.endOffset;
			tunnel.AddLast(newPiece);
		}
	}

	private void updateClearRun(TunnelPiece newPiece) {
		if (newPiece.clearRun) {
			currentClearRun += newPiece.length();
		} else {
			currentClearRun = 0;
		}
	}

	private float tunnelLength() {
		float length = 0;
		foreach (TunnelPiece piece in tunnel) {
			length += piece.length ();
		}
		return length;
	}

	private TunnelSelectionPreferences buildPreferences() {
		TunnelSelectionPreferences prefs = new TunnelSelectionPreferences ();
		LinkedListNode<TunnelPiece> currentNode = tunnel.Last;

		float distanceToEndOfTunnel = 0f;
		float currentTunnelDifficulty = 0f;
		float currentTunnelRarity = 0f;

		while (currentNode != null) {
			if (currentNode.Value.minClearSequenceAfter > distanceToEndOfTunnel) {
				prefs.requireCleanRun = true;
			}

			distanceToEndOfTunnel += currentNode.Value.length();
			currentTunnelDifficulty += currentNode.Value.difficultyLevel;
			currentNode = currentNode.Previous;
		}

		int bucketLevel = calculateBucketLevel ();
		prefs.maxBucketLevel = bucketLevel;
		prefs.maxDifficulty = bucketLevel - currentTunnelDifficulty;

		return prefs;
	}

	//Calculate the bucket level as the nth triangle number that would resolve to the distance travelled
	private int calculateBucketLevel() {
		float dist = distanceTravelled / distanceFactorToIncreaseBucketLevel;
		int bucketLevel = 0;
		while (dist > bucketLevel + 1) {
			dist -= ++bucketLevel;
		}
		return bucketLevel;
	}
}
