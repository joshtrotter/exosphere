using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	
	public int currentLevel = 1;

	private Vector3 spawnPosition;
	private float cameraAngle;
	private GameObject player;
	private GameObject cameraRig;

	private int numCollectables;
	private int collected;

	private Dictionary<string, int> objectStates = new Dictionary<string, int>();

	private bool firstLoad = true;
	
	void Awake () {
		DontDestroyOnLoad (this);
		ReloadLevel ();
	}

	public void SetCurrentLevel(int level) 
	{
		this.currentLevel = level;
	}

	public void ReloadLevel() {
		Application.LoadLevel (currentLevel);
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
		SetSpawnLocation(GameObject.FindGameObjectWithTag ("StartSpawn").transform);
		numCollectables = Object.FindObjectsOfType<Collectable> ().Length;
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
		collected--;
	}

}
