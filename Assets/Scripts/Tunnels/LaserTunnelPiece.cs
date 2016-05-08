using UnityEngine;
using System.Collections;

public class LaserTunnelPiece : TunnelPiece {
	
	[System.Serializable]
	public class LaserConfig {
		public string name;
		public float difficultyLevel;
		public GameObject[] lasers;
	}
	
	public LaserConfig[] laserConfigs;
	
	private LaserConfig configInUse;
	
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
	
	private LaserConfig getRandomConfig(TunnelSelectionPreferences prefs) {
		int loopBreaker = 0;
		while (true) {
			LaserConfig candidate = laserConfigs [Random.Range (0, laserConfigs.Length)];
			if (++loopBreaker > 20 || candidate.difficultyLevel <= prefs.preferredDifficulty) {
				return candidate;
			}
		}
	}	
	
	private void applyConfig(LaserConfig config) {
		foreach (GameObject laser in config.lasers) {
			laser.gameObject.SetActive(true);
		}
	}
	
	private void removeConfig(LaserConfig config) {
		foreach (GameObject laser in config.lasers) {
			laser.gameObject.SetActive(false);
		}
	}
}
