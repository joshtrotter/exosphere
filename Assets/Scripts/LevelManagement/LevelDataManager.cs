using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*	This class contains a dictionary of all the level data for other scripts to read, use and change
 *  It manages saving and loading and the subsequent building of LevelData objects out of the correct
 * 	LevelPermanentData and LevelSaveData objects
 */
public class LevelDataManager : MonoBehaviour {

	public static LevelDataManager manager;

	public int levelToLoad;
	
	public LevelPermanentData[] permanentData;
	private Dictionary<int, LevelData> allLevelData = new Dictionary<int, LevelData>();
	//private Dictionary<int, LevelSaveData> savedData = new Dictionary<int, LevelSaveData>();

	void Awake(){
		//set up singleton instance, destroy if a LevelDataManager already exists.
		if (manager == null) {
			manager = this;
			DontDestroyOnLoad (this);
		} else if (manager != this) {
			Destroy(gameObject);
		}

		//initialize allLevelData dict with each of the permanentData objects we have created 
		foreach (LevelPermanentData data in permanentData){
			allLevelData[data.levelID] = new LevelData(data);
		}

		//load and add all the users saved data to the allLevelData dictionary
		Load ();
	}

	//TODO remove
	void Start(){
		allLevelData [levelToLoad].Unlock ();
		//Save ();
		LevelInfo.controller.DisplayLevelInfo (allLevelData [levelToLoad]);
	}

	public LevelData GetCurrentLevelData(){
		return allLevelData [LevelManager.manager.currentLevel];
	}

	//saves all necessary data to file
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/exosphereData.dat");

		List<LevelSaveData> dataToBeSaved = new List<LevelSaveData>();

		//collect all data that needs to be saved
		foreach (LevelData levelData in allLevelData.Values) {
			dataToBeSaved.Add(levelData.GetSavedData());
		}
		//save data to file
		bf.Serialize (file, dataToBeSaved);
		file.Close();
	}

	//loads data into the savedData dictionary if there is any to be loaded
	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/exosphereData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/exosphereData.dat", FileMode.Open);
			//pull data from file
			List<LevelSaveData> saveDataList = (List<LevelSaveData>)bf.Deserialize(file);
			file.Close ();

			//build the savedData dictionary
			foreach (LevelSaveData saveData in saveDataList){
				allLevelData[saveData.levelID].SetSavedData(saveData);
			}
		}

	}
}
