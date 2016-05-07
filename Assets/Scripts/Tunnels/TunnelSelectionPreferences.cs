using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TunnelSelectionPreferences {
	
	public Dictionary<TunnelPieceCategory, float> categoryWeights = new Dictionary<TunnelPieceCategory, float>();

	public bool requireCleanRun = false;

	public int maxBucketLevel = 0;

	public float maxDifficulty = 1f;

}

