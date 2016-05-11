using UnityEngine;
using System.Collections;

public class ObstacleFilledTunnelPiece : TunnelPiece {

	[System.Serializable]
	public class ObstacleConfig : System.IComparable<ObstacleConfig> {
		public string name = "Config";
		public float difficulty;
		public float weight = 1f;
		public LayerOfObstacles[] configLayers;
		public bool shouldSpin = false;
		public int[] allowableRotations = new int[1]{0};

		public int CompareTo(ObstacleConfig other){
			//sort by difficulty, then by weight
			int diffSort = difficulty.CompareTo (other.difficulty);
			if (diffSort == 0) {
				return weight.CompareTo (other.weight);
			} else {
				return diffSort;
			}
		}
	}

	[System.Serializable]
	public class LayerOfObstacles {
		public float[] allowableLevels = new float[1]{0};
		public GameObject[] obstacles;
		[HideInInspector]
		public float chosenLevel;
	}

	public float minRotsPerSecond = 0.1f;
	public float maxRotsPerSecond = 0.2f;
	public ObstacleConfig[] obstacleConfigs;
	private float[] cumulativeSumOfWeights;
	private float[] difficultyMap;

	protected ObstacleConfig configInUse;
	protected int rotationInUse;
	protected Vector3 startRotation;

	void Awake(){
		//build cumulative sum to assist selection, coupled with a difficulty decider
		System.Array.Sort (obstacleConfigs);
		float weightSum = 0;
		cumulativeSumOfWeights = new float[obstacleConfigs.Length];
		difficultyMap = new float[obstacleConfigs.Length];
		for (int i = 0; i < obstacleConfigs.Length; i++){
			weightSum += obstacleConfigs[i].weight;
			cumulativeSumOfWeights[i] = weightSum;
			difficultyMap[i] = obstacleConfigs[i].difficulty;
		}
	}
	
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
		/*
		int loopBreaker = 0;
		while (true) {
			ObstacleConfig candidate = obstacleConfigs [Random.Range (0, obstacleConfigs.Length)];
			if (++loopBreaker > 20 || candidate.difficulty <= prefs.preferredDifficulty) {
				return candidate;
			}
		}
		*/
		int max = getMaxDifficultyIndex (prefs); 
		Debug.Log (cumulativeSumOfWeights.Length + ", " + max);
		float rand = Random.Range (0, cumulativeSumOfWeights [max]);
		for (int i = 0; i < obstacleConfigs.Length; i++) {
			if (cumulativeSumOfWeights[i] >= rand) {
				ObstacleConfig chosen = obstacleConfigs[i];
				return chosen;
			}
		}

		//this also shouldn't happen
		return obstacleConfigs [0];

	}

	private int getMaxDifficultyIndex (TunnelSelectionPreferences prefs)
	{
		//return the index one higher than the last allowed obstacleConfig based on difficulty
		for (int i = 0; i < difficultyMap.Length; i++) {
			if (difficultyMap [i] > prefs.preferredDifficulty) {
				return i - 1;
			}
		}
		return difficultyMap.Length - 1; //if nothing too hard is found, any value can be picked
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
		rotationInUse = config.allowableRotations [UnityEngine.Random.Range (0, config.allowableRotations.Length)];
		transform.GetChild(0).transform.Rotate (Vector3.up * rotationInUse);
		if (config.shouldSpin) {
			startRotation = transform.GetChild (0).transform.localEulerAngles;
			GetComponentInChildren<AxisRotator>().rotationsPerSecond = (Random.Range(0,2) * 2 - 1) * Random.Range(minRotsPerSecond, maxRotsPerSecond);
		}
	}

	protected virtual void removeConfig(ObstacleConfig config) {
		foreach (LayerOfObstacles layer in config.configLayers) {
			foreach (GameObject obstacle in layer.obstacles){
				obstacle.gameObject.SetActive(false);
				obstacle.transform.Translate(0,0,-layer.chosenLevel, Space.World);
			}
		}
		if (config.shouldSpin) {
			GetComponentInChildren<AxisRotator>().rotationsPerSecond = 0f;
			transform.GetChild (0).transform.localEulerAngles = startRotation;
		}
		transform.GetChild(0).transform.Rotate (Vector3.up * -rotationInUse);
	}

}
