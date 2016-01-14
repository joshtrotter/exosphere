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
	public int GetNextLevelID(LevelPermanentData currentLevel){
		int ind = FindChildLevel (currentLevel);
		return ind < childLevels.Count - 1 ?  childLevels[ind + 1].levelID : 0;
	}

	//overload
	public int GetNextLevelID(LevelData currentLevel){
		return GetNextLevelID (currentLevel.GetPermanentData ());
	}

	public int GetPreviousLevelID(LevelPermanentData currentLevel){
		int ind = FindChildLevel (currentLevel);
		return ind > 0 ? childLevels[ind - 1].levelID : 0;
	}
	
	//overload
	public int GetPreviousLevelID(LevelData currentLevel){
		return GetPreviousLevelID (currentLevel.GetPermanentData ());
	}

	private int FindChildLevel (LevelPermanentData currentLevel)
	{
		int ind = 0;
		for (int i = 0; i < childLevels.Count; i++) {
			if (childLevels [i] == currentLevel) {
				ind = i;
			}
		}
		return ind;
	}
}
