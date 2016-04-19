using UnityEngine;
using System.Collections;

/* This class combines the saved and permanent data of levels
 * It contains some utility functions such as GetNumCollectablesFound to make it easy for other scritps to displaylevel data
 */
[System.Serializable]
public class LevelData {

	private LevelPermanentData permData;
	private LevelSaveData saveData;

	//initialize using permanent data for a level, savedData can either be passed or a new empty object will be created
	public LevelData(LevelPermanentData permanentData, LevelSaveData savedData = null){
		permData = permanentData;
		if (savedData == null) {
			savedData = new LevelSaveData(GetLevelID());
		}
		saveData = savedData;
	}

	//this will be called by the LevelDataManager if there is saved data for this level
	public void SetSavedData(LevelSaveData savedData){
		saveData = savedData;
	}
	
	public LevelSaveData GetSavedData(){
		return saveData;
	}

	public LevelPermanentData GetPermanentData(){
		return permData;	
	}

	public int GetLevelID(){
		return permData.levelID;
	}

	public string GetLevelName(){
		return permData.levelName;
	}

	public int GetParentID(){
		return permData.parentWorld;
	}

	public int GetTotalCollectables(){
		return permData.totalCollectables;
	}

	public int GetStarsRequiredToUnlock(){
		return permData.starsRequiredToUnlock;
	}

	public int GetNumCollectablesFound(){
		return saveData.numCollectablesFound;
	}
	//returns a formatted time string if the target time is visible to the user, otherwise returns "Hidden"
	public string GetTargetTimeAsString(){
		if (HasBeenCompleted ()) {
			return FloatToTimeString (permData.targetTime);
		} else {
			return "Hidden";
		}
	}

	//returns a formatted time string if the user has attempted a time trial, otherwise returns "None"
	public string GetFastestTimeAsString(){
		if (IsUnlocked () && saveData.fastestTime > 0) {
			return FloatToTimeString (saveData.fastestTime);
		} else {
			return "None";
		}
	}

	//converts a float in seconds to a MM:SS:FF format string
	private string FloatToTimeString(float time){
		int mins = Mathf.FloorToInt (time / 60);
		int secs = Mathf.FloorToInt (time % 60);
		int milli = Mathf.FloorToInt ((time - Mathf.Floor (time)) * 60);
		return string.Format ("{0:00}:{1:00}:{2:00}", mins, secs, milli);
	}

	public bool GoldenBallHasBeenCollected(){
		return saveData.goldenBallCollected;
	}

	public bool AllCollectablesHaveBeenCollected(){
		return (saveData.numCollectablesFound == permData.totalCollectables);
	}

	public bool IsUnlocked(){
		return saveData.unlocked;
	}

	public bool HasBeenCompleted(){
		return saveData.completed;
	}

	public bool TimeTrialHasBeenCompleted(){
		return (saveData.fastestTime > 0 && saveData.fastestTime < permData.targetTime);
	}

	//returns "Completed", "Unlocked" or "Locked"
	public string GetCompletionStatus(){
		if (!IsUnlocked()) {
			//return "Locked (" + permData.starsRequiredToUnlock + " stars required)";
			return "Locked";
		} else if (HasBeenCompleted ()) {
			return "Completed";
		} else {
			return "Unlocked";
		}
	}

	//returns an X/Y string for the number of collectables found
	public string GetNumCollectablesFoundOutOfTotal(){
		return saveData.numCollectablesFound + "/" + permData.totalCollectables;
	}

	//returns "Found" or "Not Found"
	public string GetGoldenBallFoundAsString(){
		return GoldenBallHasBeenCollected() ? "Found" : "Not Found";
	}

	//returns the number of stars the user has earned on this level
	public int GetStarsEarned(){
		int starsEarned = 0;
		if (AllCollectablesHaveBeenCollected())
			starsEarned += 1;
		if (GoldenBallHasBeenCollected())
			starsEarned += 1;
		if (TimeTrialHasBeenCompleted()) 
			starsEarned += 1;
		return starsEarned;
	}

	//sets number of collectables found to max out of new number and recorded number
	public void SetNumCollectablesFound(int numFound){
		saveData.numCollectablesFound = Mathf.Max (saveData.numCollectablesFound,numFound);
		//just in case
		saveData.numCollectablesFound = Mathf.Min (saveData.numCollectablesFound, permData.totalCollectables);
	}

	//sets fastest time to fastest out of currently recorded and new time
	public void SetFastestTime(float newTime){
		if (saveData.fastestTime == 0f)
			saveData.fastestTime = float.MaxValue;

		saveData.fastestTime = Mathf.Min (saveData.fastestTime, newTime);
	}

	public void SetGoldenBallCollected(){
		saveData.goldenBallCollected = true;
	}

	public void Unlock(){
		saveData.unlocked = true;
	}	

	//sets the level as completed
	//TODO unlock the next level if necessary
	public void Complete(){
		saveData.completed = true;
	}

}
