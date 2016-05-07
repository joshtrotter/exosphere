using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TunnelSpawnController : MonoBehaviour {

	public static TunnelSpawnController INSTANCE;

	public Text score;
	public Text debug;
	public float minTunnelLength = 160;
	public Transform deadZone;
	public float distanceFactorToIncreaseBucketLevel = 100f;

	//How far back to check tunnel pieces when building tunnel selection preferences
	public float maxDistanceToCheckPreferenceModifiers = 60f;

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
		score.text = "" + (int) (distanceTravelled / 10);
	}

	public float getCurrentClearRun() {
		return currentClearRun;
	}

	private void trimTunnelStart() {
		TunnelPiece toTrim = tunnel.First.Value;
		toTrim.tearDown ();
		if (!isStarterPiece (toTrim)) {
			TunnelPiecePool.INSTANCE.returnToPool (toTrim);		 
		} else {
			toTrim.gameObject.SetActive(false);
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

		while (distanceToEndOfTunnel < maxDistanceToCheckPreferenceModifiers && currentNode != null) {
			TunnelPiece piece = currentNode.Value;
			prefs = piece.updatePreferences(prefs, distanceToEndOfTunnel);

//			if (currentNode.Value.minClearSequenceAfter > distanceToEndOfTunnel) {
//				prefs.requireCleanRun = true;
//			}

			distanceToEndOfTunnel += piece.length();
			currentTunnelDifficulty += piece.difficultyLevel;
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
