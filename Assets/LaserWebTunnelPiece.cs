using UnityEngine;
using System.Collections;

public class LaserWebTunnelPiece : TunnelPiece {
	
	public int spinBucketLevel;

	private AxisRotator axisRot;
	private Quaternion baseRot;

	void Awake() {
		axisRot = GetComponentInChildren<AxisRotator> ();
		baseRot = axisRot.transform.rotation;
	}
	
	public override void setup (TunnelSelectionPreferences prefs, TunnelPiece parent)
	{
		base.setup(prefs, parent);
		axisRot.enabled = (prefs.maxBucketLevel >= spinBucketLevel && Random.Range (0f, 2f) <= 1f);
	}
	
	public override void tearDown ()
	{
		base.tearDown ();
		axisRot.transform.rotation = baseRot;
	}
}
