using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TunnelSpawnController : MonoBehaviour {

	public static TunnelSpawnController INSTANCE;

	public Text score;
	public Text debug;
	public float tunnelLengthCheckTime = 2f;
	public float minTunnelLength = 160f;
	public float minTunnelLengthBehind = 40f;
	public Transform ball;
	public Transform deadZone;

	public float distanceFactorToIncreaseBucketLevel = 100f;



	//How far back to check tunnel pieces when building tunnel selection preferences
	public float maxDistanceToCheckPreferenceModifiers = 60f;

	//Maximum settings for the tunnel selection preferences
	public float distanceToMaxSettings = 3000f;
	public float maxBucketLevel = 10f;
	public float maxDifficultyPerPiece = 1f;
	public float baseDifficulty = 0.5f;

	private LinkedList<TunnelPiece> tunnel = new LinkedList<TunnelPiece>();

	//A clear run is a sequence of tunnel pieces with no obstacles or speed reducing drops/twists
	private float currentClearRun;

	private float maxDifficulty;
	
	void Awake() {
		if (INSTANCE != null) {
			Destroy(gameObject);
		} else {
			INSTANCE = this;
		}

		maxDifficulty = ((maxDistanceToCheckPreferenceModifiers / 20f) + 1) * maxDifficultyPerPiece;
		tunnel.AddFirst(GameObject.FindGameObjectWithTag("FirstPipe").GetComponent<TunnelPiece>());
		tunnel.AddLast(GameObject.FindGameObjectWithTag("SecondPipe").GetComponent<TunnelPiece>());
	}

	// Use this for initialization
	void Start () {
		extendTunnelEnd();
		StartCoroutine (CheckTunnelSize ());
	}

	public void OnTunnelPieceEntry() {
		checkForTunnelExtension ();
	}

	private void checkForTunnelExtension() {
		trimTunnelStart ();
		extendTunnelEnd ();
	}

	public float getCurrentClearRun() {
		return currentClearRun;
	}

	private void trimTunnelStart() {
		TunnelPiece toTrim = tunnel.First.Value;
		while (ball.position.z - (toTrim.transform.position.z + toTrim.endOffset.z) > minTunnelLengthBehind) { 
			toTrim.tearDown ();
			if (!isStarterPiece (toTrim)) {
				TunnelPiecePool.INSTANCE.returnToPool (toTrim);		 
			} else {
				toTrim.gameObject.SetActive (false);
			}
			tunnel.RemoveFirst ();
			toTrim = tunnel.First.Value;
		}
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

		while (distanceToEndOfTunnel < maxDistanceToCheckPreferenceModifiers && currentNode != null) {
			TunnelPiece piece = currentNode.Value;
			prefs = piece.updatePreferences(prefs, distanceToEndOfTunnel);
			distanceToEndOfTunnel += piece.length();
			currentTunnelDifficulty += piece.difficultyLevel;
			currentNode = currentNode.Previous;
		}

		int bucketLevel = calculateBucketLevel ();
		prefs.maxBucketLevel = (int) Mathf.Lerp(0, maxBucketLevel, ball.position.z / distanceToMaxSettings);
		prefs.maxDifficulty = Mathf.Lerp(baseDifficulty, maxDifficulty, ball.position.z / distanceToMaxSettings) - currentTunnelDifficulty;
		prefs.preferredDifficulty = prefs.maxDifficulty / 2f;
		return prefs;
	}

	//Calculate the bucket level as the nth triangle number that would resolve to the distance travelled
	private int calculateBucketLevel() {
		float dist = ball.position.z / distanceFactorToIncreaseBucketLevel;
		int bucketLevel = 0;
		while (dist > bucketLevel + 1) {
			dist -= ++bucketLevel;
		}
		return bucketLevel;
	}

	private IEnumerator CheckTunnelSize() {
		while (true) {
			yield return new WaitForSeconds (tunnelLengthCheckTime);
			checkForTunnelExtension ();
		}
	}
}
