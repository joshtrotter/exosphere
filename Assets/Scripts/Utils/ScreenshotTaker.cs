using UnityEngine;
using System.Collections;

public class ScreenshotTaker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Input.GetKeyDown(KeyCode.K)){
			Debug.Log ("Taking screenshot to " + Application.dataPath);
			HUD.controller.Hide();
			Application.CaptureScreenshot(Application.dataPath + "/Screenshots/" + LevelDataManager.manager.GetCurrentLevelData().GetLevelName() + Time.realtimeSinceStartup + ".png");
			//HUD.controller.Show();
		}
	}
}
