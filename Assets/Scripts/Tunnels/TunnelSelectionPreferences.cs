using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TunnelSelectionPreferences {
	
	public Dictionary<TunnelPieceCategory, float> categoryWeights = new Dictionary<TunnelPieceCategory, float>();

	public int maxBucketLevel = 0;

	public float maxDifficulty = 1f;

	public float preferredDifficulty;

}

