using UnityEngine;
using DG.Tweening;
using System.Collections;

public class LevelSelectManager : MonoBehaviour {

	public static LevelSelectManager manager;

	//track a reference to the screens the manager can use to display level info
	private LevelInfo[] screens;

	private WorldData currentWorld;
	private LevelData currentLevel;
	private bool previousLevelExists;
	private bool nextLevelExists;

	//track how the screens are currently set up
	private LevelInfo currentScreen;
	private LevelInfo previousScreen;
	private LevelInfo nextScreen;

	//track a reference to the worldLevels screen
	private WorldLevels worldDisplay;

	//track a reference to the camera
	private MenuCameraController menuCameraController;
	
	void Awake(){
		//TODO remove
		if (LevelManager.manager == null) {
			Application.LoadLevel (0);
		}

		//set up singleton instance, destroy if a LevelDataManager already exists.
		if (manager == null) {
			manager = this;
			DontDestroyOnLoad (this);
		} else if (manager != this) {
			Destroy(gameObject);
		}
	}

	void Start(){
		screens = GetComponentsInChildren<LevelInfo> ();
		menuCameraController = GetComponentInChildren<MenuCameraController> ();
		worldDisplay = GetComponentInChildren<WorldLevels> ();

		menuCameraController.FocusCamera (worldDisplay.transform.localEulerAngles, 1);

		currentLevel = LevelDataManager.manager.GetCurrentLevelData ();

		currentScreen = screens [0];
		previousScreen = screens [1];
		nextScreen = screens [2];
	}
	
	public LevelInfo GetCurrentScreen(){
		return currentScreen;
	}

	public bool IsANextLevel(){
		return nextLevelExists;
	}

	public bool IsAPreviousLevel(){
		return previousLevelExists;
	}

	public void StartWorldLevelsDisplay(WorldData world){
		currentWorld = world;
		worldDisplay.DisplayWorldLevels (world);
	}

	public void StartLevelInfoDisplay(int levelID){
		currentLevel = LevelDataManager.manager.GetLevelData (levelID);
		//currentScreen.transform.position = new Vector3 (0, 0, 0);
		currentScreen.transform.localRotation = Quaternion.Euler (worldDisplay.transform.localEulerAngles - new Vector3 (-36, 0, 0));
		currentScreen.DisplayLevelInfo (currentLevel);

		SetupAsPrevious(previousScreen);
		SetupAsNext (nextScreen);

		menuCameraController.FocusCamera (currentScreen.transform.localEulerAngles, 1);
		menuCameraController.shouldMonitorSwiping = true;
	}

	public void ReturnToWorldDisplay(){
		worldDisplay.transform.localRotation = Quaternion.Euler (currentScreen.transform.localEulerAngles - new Vector3 (36, 0, 0));
		currentScreen.DisplayLevelInfo (currentLevel);
		menuCameraController.FocusCamera (worldDisplay.transform.localEulerAngles, 1);
		menuCameraController.shouldMonitorSwiping = false;
	}

	//finds the screen whose localRotation is closest to the given rotation and sets this screen as currentScreen,
	//and sets up other screens around it
	public void SetClosestScreenAsFocused(Quaternion rotation){
		float comparison = (rotation.eulerAngles.y - currentScreen.transform.localEulerAngles.y);
		if ((comparison > 18 && comparison < 180) || (comparison > -342 && comparison < -180)) {
			//next screen is closer
			SetNextAsFocused ();
		} else if ((comparison < 342 && comparison > 180) || (comparison < -18 && comparison > -180)) {
			//previous screen is closer
			SetPreviousAsFocused();
		} //else current screen is still closest, no change needed
	}

	//sets the next level screen to be the new focused and current level screen
	private void SetNextAsFocused(){
		if (nextLevelExists) {
			LevelInfo oldPreviousScreen = previousScreen;
			previousScreen = currentScreen;
			currentScreen = nextScreen;
			nextScreen = oldPreviousScreen;
			SetupScreens ();
		}

	}

	//sets the previous level screen to be the new focused and current level screen
	private void SetPreviousAsFocused(){
		if (previousLevelExists) {
			LevelInfo oldNextScreen = nextScreen;
			nextScreen = currentScreen;
			currentScreen = previousScreen;
			previousScreen = oldNextScreen;
			SetupScreens ();
		}
	}

	//moves screens around the current screen and sets them to display correct data
	private void SetupScreens(){
		currentLevel = currentScreen.GetCurrentLevel ();
		SetupAsNext (nextScreen);
		SetupAsPrevious (previousScreen);
	}
	
	private void SetupAsPrevious(LevelInfo screen){		
		LevelData previousLevel = LevelDataManager.manager.GetPreviousLevelData (currentLevel);
		//screen.GetComponentInChildren<MoveBetweenTargets2D> ().transform.localRotation = Quaternion.identity;
		//screen.GetComponentInChildren<MoveBetweenTargets2D> ().ResetRotation ();
		if (previousLevel != null) {
			screen.DisplayLevelInfo (previousLevel);
			screen.transform.localRotation = Quaternion.Euler (currentScreen.transform.localRotation.eulerAngles - new Vector3 (0, 36, 0));
			previousLevelExists = true;
		} else {
			screen.transform.localRotation = Quaternion.Euler (currentScreen.transform.localRotation.eulerAngles - new Vector3 (0, 180, 0));
			previousLevelExists = false;
		}
	}
	
	private void SetupAsNext(LevelInfo screen){
		LevelData nextLevel = LevelDataManager.manager.GetNextLevelData (currentLevel);
		screen.transform.localRotation = Quaternion.Euler(currentScreen.transform.localRotation.eulerAngles + new Vector3 (0, 36, 0));
		//screen.GetComponentInChildren<MoveBetweenTargets2D> ().transform.localRotation = Quaternion.identity;
		//screen.GetComponentInChildren<MoveBetweenTargets2D> ().ResetRotation ();
		if (nextLevel != null) {
			screen.DisplayLevelInfo (nextLevel);
			nextLevelExists = true;
		} else {
			screen.transform.localRotation = Quaternion.Euler (currentScreen.transform.localRotation.eulerAngles + new Vector3 (0, 180, 0));
			nextLevelExists = false;
		}
	}

	//info screens will check before loading a level whether the button press was allowed
	//ie if in the middle of a swipe then the level won't load as the button press was probably not intentional
	public bool IsSafeToPlay(){
		return !menuCameraController.IsSwiping ();
	}
}
