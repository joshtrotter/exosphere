using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WorldData : ScriptableObject {

	public int worldID;
	public string worldName;
	public bool unlocked;
	public List<LevelPermanentData> childLevels;

	//returns the levelID of the level following on from the currentLevel
	//if the current level is the last in the world, returns 0
	public int GetNextLevelID(LevelPermanentData level){
		int ind = FindChildLevel (level);
		return ind < childLevels.Count - 1 ?  childLevels[ind + 1].levelID : 0;
	}

	//overload
	public int GetNextLevelID(LevelData level){
		return GetNextLevelID (level.GetPermanentData ());
	}

	//returns the levelId of the level preceeding the currentLevel
	//if the current level is the first in the world, returns 0
	public int GetPreviousLevelID(LevelPermanentData level){
		int ind = FindChildLevel (level);
		return ind > 0 ? childLevels[ind - 1].levelID : 0;
	}
	
	//overload
	public int GetPreviousLevelID(LevelData level){
		return GetPreviousLevelID (level.GetPermanentData ());
	}

	//returns the LevelData of the Xth (0-9) level of this world
	public LevelData GetXthChildData(int childNum){
		return LevelDataManager.manager.GetLevelData (childLevels [childNum].levelID);
	}

	//returns the index of the currentLevel in the childLevels array
	private int FindChildLevel (LevelPermanentData level)
	{
		int ind = 0;
		for (int i = 0; i < childLevels.Count; i++) {
			if (childLevels [i] == level) {
				ind = i;
			}
		}
		return ind;
	}
}
