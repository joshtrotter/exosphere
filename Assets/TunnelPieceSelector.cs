using UnityEngine;
using System.Collections;

public class TunnelPieceSelector : MonoBehaviour {

	public TunnelPiece spawnChildTunnelPiece(TunnelPiece parent) {
		return TunnelPiecePool.INSTANCE.spawnNewInstance(selectFromPool(parent));
	}

	private TunnelPiece selectFromPool(TunnelPiece parent) {
		return TunnelPiecePool.INSTANCE.tunnelPieces[Random.Range (0, (TunnelPiecePool.INSTANCE.tunnelPieces.Length))];
	}
}
