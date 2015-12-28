using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {

	public const float dimIntensity = 0.35f;

	void Awake () {
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
	}

	void Start() {
		if (Application.isEditor && (GameObject.FindGameObjectWithTag ("LevelManager") == null)) {
			Debug.Log ("About to run level loader");
			Application.LoadLevel(0);
		}
	}

}
