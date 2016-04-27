using UnityEngine;
using System.Collections;

public class JumpTunnelPiece : TunnelPiece {
	
	public float maxJumpExtension = 20f;

	public override void setup (TunnelPiece parent)
	{
		base.setup (parent);
		float jumpExtension = Mathf.Clamp ((TunnelSpawnController.INSTANCE.getCurrentClearRun () - this.minClearSequenceBefore) / 2, 0, maxJumpExtension);
		this.endOffset = this.endOffset + (Vector3.forward * jumpExtension);
	}
}
