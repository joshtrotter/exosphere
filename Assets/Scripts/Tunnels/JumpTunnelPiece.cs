using UnityEngine;
using System.Collections;

public class JumpTunnelPiece : TunnelPiece {
	
	public float maxJumpExtension = 20f;

	public override void setup (TunnelSelectionPreferences prefs, TunnelPiece parent)
	{
		base.setup (prefs, parent);
		float jumpExtension = Mathf.Clamp ((TunnelSpawnController.INSTANCE.getCurrentClearRun () - this.minClearSequenceBefore) / 3, 0, maxJumpExtension);
		this.endOffset = this.endOffset + (Vector3.forward * jumpExtension);
	}
}
