#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateWorldData : MonoBehaviour {

	[MenuItem("Assets/Create/WorldData")]
	public static void CreateLevelDataAsset()
	{
		WorldData asset = ScriptableObject.CreateInstance<WorldData> ();
		int num = 0;
		while (AssetDatabase.FindAssets ("WorldData" + num, new string[]{"Assets/ScriptableObjects"}).Length > 0){
			num += 1;
		}
		AssetDatabase.CreateAsset (asset, "Assets/ScriptableObjects/WorldData" + num + ".asset");
		AssetDatabase.SaveAssets ();
		
		EditorUtility.FocusProjectWindow ();
		
		Selection.activeObject = asset;
	}
}
#endif
