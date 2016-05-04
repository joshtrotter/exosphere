using UnityEngine;
using System.Collections;

public class HoleTunnelPiece : TunnelPiece {

	[System.Serializable]
	public class HoleConfig {
		public float difficulty;
		public GameObject[] holes;
		public int[] allowableRotations;
	}

	public HoleConfig[] holeConfigs;

	private HoleConfig configInUse;
	private int rotationInUse;
	
	public override void setup (TunnelSelectionPreferences prefs, TunnelPiece parent)
	{
		base.setup (prefs, parent);
		configInUse = getRandomConfig (prefs);
		applyConfig (configInUse);
	}

	public override void tearDown ()
	{
		base.tearDown ();
		removeConfig (configInUse);
	}

	private HoleConfig getRandomConfig(TunnelSelectionPreferences prefs) {
		int loopBreaker = 0;
		while (true) {
			HoleConfig candidate = holeConfigs [Random.Range (0, holeConfigs.Length)];
			if (++loopBreaker > 30 || candidate.difficulty <= prefs.maxDifficulty) {
				return candidate;
			}
		}
	}


	private void applyConfig(HoleConfig config) {
		foreach (GameObject hole in config.holes) {
			hole.gameObject.SetActive(false);
		}
		rotationInUse = config.allowableRotations [Random.Range (0, config.allowableRotations.Length)];
		transform.GetChild(0).transform.Rotate (Vector3.up * rotationInUse);
	}

	private void removeConfig(HoleConfig config) {
		foreach (GameObject hole in config.holes) {
			hole.gameObject.SetActive(true);
		}
		transform.GetChild(0).transform.Rotate (Vector3.up * -rotationInUse);
	}

}
