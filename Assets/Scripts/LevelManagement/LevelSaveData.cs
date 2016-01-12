using UnityEngine;
using System.Collections;

/* This class stores the data for each level that needs to be saved
 */
[System.Serializable]
public class LevelSaveData {
	
	public int levelID;
	public bool unlocked, completed, goldenBallCollected;
	public int numCollectablesFound;
	public float fastestTime;
	
	public LevelSaveData(LevelData levelData){
		levelID = levelData.levelID;
		unlocked = levelData.unlocked;
		completed = levelData.completed;
		goldenBallCollected = levelData.goldenBallCollected;
		numCollectablesFound = levelData.numCollectablesFound;
		fastestTime = levelData.fastestTime;
	}

}
