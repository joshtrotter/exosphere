using UnityEngine;
using System.Collections;

public class JumpTunnelPiece : TunnelPiece {
	
	public float maxJumpExtension = 35f;
	public float maxJumpRise = 5f;
	public Transform velocityBraker;

	private float jumpExtension;
	private float jumpRise;

	public override void setup (TunnelSelectionPreferences prefs, TunnelPiece parent)
	{
		base.setup (prefs, parent);
		jumpExtension = Mathf.Clamp (((TunnelSpawnController.INSTANCE.getCurrentClearRun())) / 4, 0, maxJumpExtension);
		jumpRise = Mathf.Lerp (0, maxJumpRise, jumpExtension / maxJumpExtension);
		Debug.Log ("Jump rise = " + jumpRise + ", Jump extension = " + jumpExtension);
		velocityBraker.position = velocityBraker.position + (Vector3.forward * jumpExtension) + (Vector3.up * jumpRise); 
		this.endOffset = this.endOffset + (Vector3.forward * jumpExtension);
		this.endOffset = this.endOffset + (Vector3.up * jumpRise);
	}

	public override void tearDown ()
	{
		base.tearDown ();
		velocityBraker.position = velocityBraker.position + (Vector3.back * jumpExtension) + (Vector3.down * jumpRise); 
		this.endOffset = this.endOffset + (Vector3.back * jumpExtension);
		this.endOffset = this.endOffset + (Vector3.down * jumpRise);
	}
}
