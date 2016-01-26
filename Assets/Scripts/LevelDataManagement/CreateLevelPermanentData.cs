#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateLevelPermanentData : MonoBehaviour {

	[MenuItem("Assets/Create/LevelPermanentData")]
	public static void CreateLevelDataAsset()
	{
		LevelPermanentData asset = ScriptableObject.CreateInstance<LevelPermanentData> ();
		int num = 0;
		while (AssetDatabase.FindAssets ("LevelPermanentData" + num, new string[]{"Assets/ScriptableObjects"}).Length > 0){
			num += 1;
		}
		AssetDatabase.CreateAsset (asset, "Assets/ScriptableObjects/LevelPermanentData" + num + ".asset");
		AssetDatabase.SaveAssets ();
		
		EditorUtility.FocusProjectWindow ();
		
		Selection.activeObject = asset;
	}
}
#endif
