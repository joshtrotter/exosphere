using UnityEngine;
using System.Collections;

public class JumpTunnelPiece : TunnelPiece {
	
	public float maxJumpExtension = 35f;
	public float maxJumpRise = 5f;
	public Transform jumpEnd;

	private float jumpExtension;
	private float jumpRise;

	public override void setup (TunnelSelectionPreferences prefs, TunnelPiece parent)
	{
		base.setup (prefs, parent);
		jumpExtension = Mathf.Clamp (((TunnelSpawnController.INSTANCE.getCurrentClearRun())) / 4, 0, maxJumpExtension);
		float difficultyBasedMaxJumpRise = Mathf.Lerp (0, maxJumpRise, (1 - (2 * TunnelSpawnController.INSTANCE.GetCurrentAverageDifficulty ())));
		jumpRise = Mathf.Lerp (0, difficultyBasedMaxJumpRise, jumpExtension / maxJumpExtension);
		Debug.Log ("Jump rise = " + jumpRise + ", Jump extension = " + jumpExtension + ", Ave Difficulty = " + TunnelSpawnController.INSTANCE.GetCurrentAverageDifficulty());
		jumpEnd.position = jumpEnd.position + (Vector3.forward * jumpExtension) + (Vector3.up * jumpRise); 
		this.endOffset = this.endOffset + (Vector3.forward * jumpExtension);
		this.endOffset = this.endOffset + (Vector3.up * jumpRise);
	}

	public override void tearDown ()
	{
		base.tearDown ();
		jumpEnd.position = jumpEnd.position + (Vector3.back * jumpExtension) + (Vector3.down * jumpRise); 
		this.endOffset = this.endOffset + (Vector3.back * jumpExtension);
		this.endOffset = this.endOffset + (Vector3.down * jumpRise);
	}

	public override float length(){
		//tricks the tunnelSpawnController into extending the tunnel afterwards to ensure
		//that there is always something waiting at the end of the jump
		return 0f;
	}
}
