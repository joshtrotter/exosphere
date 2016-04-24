using UnityEngine;
using System.Collections;

public class TunnelPiece : MonoBehaviour {

	public int pieceLength = 20;
	public TunnelPieceSelector selector;

	public TunnelPiece spawnChildPiece() {
		TunnelPiece child = selector.spawnChildTunnelPiece (this);
		child.transform.position = transform.position + new Vector3 (0, 0, pieceLength);
		return child;
	}
}
