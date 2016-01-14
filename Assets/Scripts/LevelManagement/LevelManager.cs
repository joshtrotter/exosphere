using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	//used for singleton creation
	public static LevelManager manager;
	
	public int currentLevel = 1;

	private Vector3 spawnPosition;
	private float cameraAngle;
	private GameObject player;
	private GameObject cameraRig;

	//private int numCollectables;
	public int collected;
	public bool goldenBallFound;

	private Dictionary<string, int> objectStates = new Dictionary<string, int>();

	private bool firstLoad = true;
	
	void Awake () {
		//set up singleton instance, destroy if a LevelManager already exists.
		if (manager == null) {
			manager = this;
			DontDestroyOnLoad (this);
		} else if (manager != this) {
			Destroy(gameObject);
		}

		//ReloadLevel ();
		//TODO remove
		goldenBallFound = true;
		collected = 5;
	}

	public void SetCurrentLevel(int level) 
	{
		this.currentLevel = level;
	}

	public void ReloadLevel() {
		TearDown ();
		Application.LoadLevel (currentLevel);
	}

	private void TearDown()
	{
		/*if (calCanvas != null) {
			calCanvas.gameObject.SetActive (true);
		}*/

	}

	private void OnLevelWasLoaded() 
	{
		Debug.Log ("Level " + Application.loadedLevel + " Loaded");
		if (Application.loadedLevel == currentLevel) {
			if (firstLoad) {
				OnFirstLoad ();
				firstLoad = false;
			} else {
				OnReload();
			}
			SetupLevel ();
		}
	}

	private void OnFirstLoad() 
	{
		//TODO Designed for testing. If there is no startSpawn enable, the ball will be used
		GameObject StartSpawn = GameObject.FindGameObjectWithTag ("StartSpawn");
		if (StartSpawn == null) {
			StartSpawn = GameObject.FindGameObjectWithTag ("Player");
		}
		SetSpawnLocation(StartSpawn.transform);
		SetCameraRotation (StartSpawn.transform.localRotation.eulerAngles.y);
		//numCollectables = Object.FindObjectsOfType<Collectable> ().Length;
	}

	private void OnReload()
	{
		HasLevelState[] statefulLevelObjects = Object.FindObjectsOfType<HasLevelState> ();
		foreach (HasLevelState obj in statefulLevelObjects) {
			int rememberedState = 0;
			if (objectStates.TryGetValue(obj.uniqueId, out rememberedState)) {
				obj.ReloadState(rememberedState);
			}
		}
	}

	private void SetupLevel() 
	{
		SendPlayerToSpawnPoint ();
		RotateCamera ();
		HUD.controller.RequestToBeShown ();

#if MOBILE_INPUT
		//do this last
		CalibrateTilt ();
#else
		CallibrationUI.controller.Hide ();
#endif
	}

	private void SendPlayerToSpawnPoint() 
	{
		player = GameObject.FindGameObjectWithTag ("Player");	
		player.transform.position = spawnPosition;
		player.transform.rotation = Quaternion.Euler(Vector3.up * cameraAngle);
	}

	private void RotateCamera()
	{
		cameraRig = GameObject.FindGameObjectWithTag ("CameraRig");
		cameraRig.GetComponent<AmazeballCam> ().camAngle = cameraAngle;
	}

	private void CalibrateTilt ()
	{
		CallibrationUI.controller.SetupCalibration ();
	}

	public void SetSpawnLocation(Transform transform) {
		this.spawnPosition = transform.position;
	}

	public void SetCameraRotation(float cameraAngle) {
		this.cameraAngle = cameraAngle;
	}

	public void RegisterObjectState(string objectId, int objectState) {
		objectStates[objectId] = objectState;
	}

	public void RemoveCollectable() {
		collected++;
	}

	public string GetNumCollectablesFound(){
		return collected + "/" + LevelDataManager.manager.GetCurrentLevelData().GetTotalCollectables();
	}

}
