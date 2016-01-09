using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelData : ScriptableObject {

	//constant level info
	public int levelID;
	public string levelName;
	public int parentWorld, totalCollectables, starsRequiredToUnlock;
	public float targetTime;

	//save data info
	public bool unlocked, completed, goldenBallCollected;
	public int numCollectablesFound;
	public float fastestTime;

	//derived info
	public int starsEarned;
	public bool allCollectablesFound, timeTrialCompleted;

}
