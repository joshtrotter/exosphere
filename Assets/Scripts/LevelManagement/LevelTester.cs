#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class LevelTester : MonoBehaviour {

	public static LevelTester debugger;
	private int thisLevel;

	void Awake(){
		if (debugger == null) {
			debugger = this;
			DontDestroyOnLoad (this);
		} else if (debugger != this) {
			Destroy(gameObject);
		}
	}

	void Start() {
		if (GameObject.FindGameObjectWithTag ("LevelManager") == null) {
			Debug.Log ("About to run level loader");
			Application.LoadLevel(0);
			thisLevel = Application.loadedLevel;
		}
	}

	void OnLevelWasLoaded(){
		//Debug.Log ("Level " + Application.loadedLevel + " loaded.");
		if (Application.loadedLevel == 0 && thisLevel != 0) {
			LevelManager.manager.SetCurrentLevel (thisLevel);
			LevelManager.manager.FirstLoadLevel ();
		}
	}
}
#endif
