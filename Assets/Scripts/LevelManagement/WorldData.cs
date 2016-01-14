using UnityEngine;
using System.Collections;

[System.Serializable]
public class WorldData : ScriptableObject {

	public int worldID;
	public string worldName;
	public bool unlocked;
	public LevelPermanentData[] childLevels;
	
}
