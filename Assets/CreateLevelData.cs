using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateLevelData {
	[MenuItem("Assets/Create/LevelData")]
	public static void CreateLevelDataAsset()
	{
		LevelData asset = ScriptableObject.CreateInstance<LevelData> ();
		int num = 0;
		while (AssetDatabase.FindAssets ("levelData" + num, new string[]{"Assets/ScriptableObjects"}).Length > 0 && num < 10){
			num += 1;
		}
		AssetDatabase.CreateAsset (asset, "Assets/ScriptableObjects/levelData" + num + ".asset");
		AssetDatabase.SaveAssets ();

		EditorUtility.FocusProjectWindow ();

		Selection.activeObject = asset;
	}

}
