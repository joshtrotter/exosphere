using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	public int currentLevel = 1;

	private Vector3 spawnPosition;
	private float cameraAngle;
	private GameObject player;
	private GameObject cameraRig;
	
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
			}
			SetupLevel ();
		}
	}

	private void OnFirstLoad() 
	{
		SetSpawnLocation(GameObject.FindGameObjectWithTag ("StartSpawn").transform);
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

}
