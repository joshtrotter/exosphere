using UnityEngine;
using System.Collections;

public class JumpTunnelPiece : TunnelPiece {
	
	public float maxJumpExtension = 20f;
	public Transform velocityBraker;

	private float jumpExtension;

	public override void setup (TunnelSelectionPreferences prefs, TunnelPiece parent)
	{
		base.setup (prefs, parent);
		jumpExtension = Mathf.Clamp (((TunnelSpawnController.INSTANCE.getCurrentClearRun())) / 4, 0, maxJumpExtension);
		velocityBraker.position = velocityBraker.position + (Vector3.forward * jumpExtension); 
		this.endOffset = this.endOffset + (Vector3.forward * jumpExtension);
	}

	public override void tearDown ()
	{
		base.tearDown ();
		velocityBraker.position = velocityBraker.position + (Vector3.back * jumpExtension); 
		this.endOffset = this.endOffset + (Vector3.back * jumpExtension);
	}
}
