using UnityEngine;
using System.Collections;

public class TunnelPiece : MonoBehaviour {
	
	//The position offset from the beginning to the end of this tunnel piece (represents the start position of the child piece)
	public Vector3 endOffset;

	//Whether this tunnel can contribute to a clear run sequence
	public bool clearRun = false;

	//Bucket levels are added to the spawn pool as the player progresses
	public int bucketLevel;
	//Indicates the complexity of the piece
	public float difficultyLevel;
	//Indicates how common this piece should be (rarer pieces will be selected less often)
	public int frequency;

	public float baseWeight = 1f;

	public TunnelPieceCategory category;

	[System.Serializable]
	public class ChildCategoryWeight {
		//The category weight that will be modified
		public TunnelPieceCategory category;
		//The distance after the end of this piece that the weight will be modified for
		public float distance;
		//The scale to apply to the weight (use zero to guarantee that the category will not be used)
		public float weightScale;
	}
	public ChildCategoryWeight[] childCategoryWeights;

	public virtual void setup(TunnelSelectionPreferences prefs, TunnelPiece parent) {
		//Standard tunnel pieces don't require any special setup
	}

	public virtual void tearDown() {
		//Standard tunnel pieces don't require any special clean up
	}

	public TunnelPiece spawnChildPiece(TunnelSelectionPreferences prefs) {
		TunnelPiece child = TunnelPiecePool.INSTANCE.takeWeightedRandomPieceFromPool (prefs);
		child.transform.position = transform.position + endOffset;
		child.gameObject.SetActive(true);
		child.setup (prefs, this);
		return child;
	}

	public float length() {
		return endOffset.z;
	}

	public float calculateWeight(TunnelSelectionPreferences prefs) {
		if (validatePiece (prefs)) {
			return (baseWeight + difficultyWeightModifier(prefs)) * categoryWeigthModifier(prefs, category);
		} else {
			return 0f;
		}
	}

	//Allows this piece to update selection preferences for a child/grandchild piece
	public TunnelSelectionPreferences updatePreferences(TunnelSelectionPreferences basePreferences, float distanceToEndOfTunnel) {
		foreach (ChildCategoryWeight ccw in childCategoryWeights) {
			if (ccw.distance >= distanceToEndOfTunnel) {
				float currentWeight = categoryWeigthModifier(basePreferences, ccw.category);
				basePreferences.categoryWeights[ccw.category] = currentWeight * ccw.weightScale;
			}
		}
		return basePreferences;
	}

	private bool validatePiece(TunnelSelectionPreferences prefs) {
		bool isValid = this.bucketLevel <= prefs.maxBucketLevel;
		isValid = isValid && this.difficultyLevel <= prefs.maxDifficulty;
		return isValid;
	}

	private float categoryWeigthModifier(TunnelSelectionPreferences prefs, TunnelPieceCategory category) {
		float weight;
		if (!prefs.categoryWeights.TryGetValue(category, out weight)) {
			weight = 1f;
		}
		return weight;
	}

	private float difficultyWeightModifier(TunnelSelectionPreferences prefs) {
		float difficultyGap = Mathf.Clamp01 (Mathf.Abs (this.difficultyLevel - prefs.preferredDifficulty));
		return 1 - difficultyGap;
	}
}
