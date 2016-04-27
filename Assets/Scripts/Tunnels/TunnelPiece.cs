using UnityEngine;
using System.Collections;

public class TunnelPiece : MonoBehaviour {
	
	//The position offset from the beginning to the end of this tunnel piece (represents the start position of the child piece)
	public Vector3 endOffset;

	//Whether this tunnel can contribute to a clear run sequence
	public bool clearRun = false;

	//Used to enforce a minimum distance of clear tunnels before this piece can be placed
	public float minClearSequenceBefore = 0f;
	//Used to enforce a maximum distance of clear tunnels before this piece can be placed
	public float maxClearSequenceBefore = 0f;
	//Used to enforce a minimum distance of clear tunnels that must be placed after this piece
	public float minClearSequenceAfter = 0f;

	//Bucket levels are added to the spawn pool as the player progresses
	public int bucketLevel;
	//Indicates the complexity of the piece
	public float difficultyLevel;
	//Indicates how common this piece should be (rarer pieces will be selected less often)
	public int frequency;

	public TunnelPieceSelector selector;

	public virtual void setup(TunnelSelectionPreferences prefs, TunnelPiece parent) {
		//Standard tunnel pieces don't require any special setup
	}

	public virtual void tearDown() {
		//Standard tunnel pieces don't require any special clean up
	}

	public TunnelPiece spawnChildPiece(TunnelSelectionPreferences prefs) {
		TunnelPiece child = selector.spawnChildTunnelPiece (this, prefs);
		child.transform.position = transform.position + endOffset;
		child.gameObject.SetActive(true);
		child.setup (prefs, this);
		return child;
	}

	public float length() {
		return endOffset.z;
	}
}
