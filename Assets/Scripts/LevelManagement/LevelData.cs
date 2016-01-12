using UnityEngine;
using System.Collections;

/* This class combines the saved and permanent data of levels
 * It contains some utility functions such as GetNumCollectablesFound to make it easy for other scritps to displaylevel data
 */
[System.Serializable]
public class LevelData {

	//permanent level info
	public int levelID;
	public string levelName;
	public int parentWorld, totalCollectables, starsRequiredToUnlock;
	public float targetTime;

	//save data info
	public bool unlocked, completed, goldenBallCollected;
	public int numCollectablesFound;
	public float fastestTime;

	//derived info for simple look up
	public bool allCollectablesFound, timeTrialCompleted;

	//initialize using permanent data for a level
	public LevelData(LevelPermanentData permanentData){
		levelID = permanentData.levelID;
		levelName = permanentData.levelName;
		parentWorld = permanentData.parentWorld;
		totalCollectables = permanentData.totalCollectables;
		starsRequiredToUnlock = permanentData.starsRequiredToUnlock;
		targetTime = permanentData.targetTime;
	}

	//this will be called by the LevelDataManager if there is saved data for this level
	public void SetSavedData(LevelSaveData saveData){
		unlocked = saveData.unlocked;
		completed = saveData.completed;
		goldenBallCollected = saveData.goldenBallCollected;
		SetNumCollectablesFound (saveData.numCollectablesFound);
		SetFastestTime (saveData.fastestTime);
	}

	//returns an X/Y string for the number of collectables found
	public string GetNumCollectablesFound(){
		return numCollectablesFound + "/" + totalCollectables;
	}

	//returns the number of stars the user has earned on this level
	public int GetStarsEarned(){
		int starsEarned = 0;
		if (allCollectablesFound)
			starsEarned += 1;
		if (goldenBallCollected)
			starsEarned += 1;
		if (timeTrialCompleted) 
			starsEarned += 1;
		return starsEarned;
	}

	//sets number of collectables found to max out of new number and recorded number
	public void SetNumCollectablesFound(int numFound){
		numCollectablesFound = Mathf.Max (numCollectablesFound,numFound);
		//just in case
		numCollectablesFound = Mathf.Min (numCollectablesFound, totalCollectables);

		if (numCollectablesFound == totalCollectables)
			allCollectablesFound = true;
	}

	//sets fastest time to fastest out of currently recorded and new time
	public void SetFastestTime(float newTime){
		if (fastestTime == 0f)
			fastestTime = float.MaxValue;

		fastestTime = Mathf.Min (fastestTime, newTime);

		if (fastestTime != 0 && fastestTime <= targetTime) {
			timeTrialCompleted = true;
		}
	}

	public void SetGoldenBallCollected(){
		goldenBallCollected = true;
	}

	public void Unlock(){
		unlocked = true;
	}	

	public void Complete(){
		completed = true;
	}

}
