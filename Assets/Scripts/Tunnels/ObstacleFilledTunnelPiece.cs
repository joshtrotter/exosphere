using UnityEngine;
using System.Collections;

public class ObstacleFilledTunnelPiece : TunnelPiece {

	[System.Serializable]
	public class ObstacleConfig {
		public string name = "Config";
		public float difficulty;
		public LayerOfObstacles[] configLayers;
	}

	[System.Serializable]
	public class LayerOfObstacles {
		public float[] allowableLevels = new float[1]{0};
		public GameObject[] obstacles;
		[HideInInspector]
		public float chosenLevel;
	}

	public ObstacleConfig[] obstacleConfigs;
	public int[] allowableRotations;

	protected ObstacleConfig configInUse;
	protected int rotationInUse;
	
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

	private ObstacleConfig getRandomConfig(TunnelSelectionPreferences prefs) {
		int loopBreaker = 0;
		while (true) {
			ObstacleConfig candidate = obstacleConfigs [Random.Range (0, obstacleConfigs.Length)];
			if (++loopBreaker > 20 || candidate.difficulty <= prefs.preferredDifficulty) {
				return candidate;
			}
		}
	}

	protected virtual void applyConfig(ObstacleConfig config) {
		Debug.Log ("Setting up " + config.name);
		foreach (LayerOfObstacles layer in config.configLayers) {
			layer.chosenLevel = layer.allowableLevels[Random.Range(0, layer.allowableLevels.Length)];
			foreach (GameObject obstacle in layer.obstacles){
				obstacle.gameObject.SetActive(true);
				obstacle.transform.Translate(0,0,layer.chosenLevel, Space.World);
			}
		}
		rotationInUse = allowableRotations [Random.Range (0, allowableRotations.Length)];
		transform.GetChild(0).transform.Rotate (Vector3.up * rotationInUse);
	}

	protected virtual void removeConfig(ObstacleConfig config) {
		foreach (LayerOfObstacles layer in config.configLayers) {
			foreach (GameObject obstacle in layer.obstacles){
				obstacle.gameObject.SetActive(false);
				obstacle.transform.Translate(0,0,-layer.chosenLevel, Space.World);
			}
		}
		transform.GetChild(0).transform.Rotate (Vector3.up * -rotationInUse);
	}

}
