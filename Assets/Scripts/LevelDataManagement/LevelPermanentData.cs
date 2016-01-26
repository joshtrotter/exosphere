using UnityEngine;
using System.Collections;

/* this object stores the constant data for a level
 * Instances can be created in the editor under Assets-->Create-->LevelPermanentData
 * Once the data has been filled in for a level, the object should be added to the permanentData list of the
 * LevelDataManager
 */
[System.Serializable]
public class LevelPermanentData : ScriptableObject {

	//the scene index
	public int levelID;

	public string levelName;
	public int parentWorld, totalCollectables, starsRequiredToUnlock;
	public float targetTime;
}
