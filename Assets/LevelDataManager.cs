using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelDataManager : MonoBehaviour {

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/exosphereData.dat");

		LevelSaveData saveData = new LevelSaveData ();

		bf.Serialize (file, saveData);
		file.Close();
	}

	public void Load()
	{

	}

	[Serializable]
	class LevelSaveData {
	
		int levelID;
		bool unlocked, completed, goldenBallCollected;
		int numCollectablesFound;
		float fastestTime;
	}
}
