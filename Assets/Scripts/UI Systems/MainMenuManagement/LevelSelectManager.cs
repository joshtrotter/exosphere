using UnityEngine;
using DG.Tweening;
using System.Collections;

public class LevelSelectManager : MonoBehaviour {

	//find the screens the manager can use to display level info
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

	//track a reference to the menu camera
	private MenuCameraController menuCameraController;

	private WorldSelectManager worldSelectManager;

	private bool focusedOnWorldLayer;
	
	void Awake(){
		screens = GetComponentsInChildren<LevelInfo> ();
		menuCameraController = GetComponentInChildren<MenuCameraController> ();
		worldDisplay = GetComponentInChildren<WorldLevels> ();
		worldSelectManager = GetComponent<WorldSelectManager> ();
		
		currentScreen = screens [0];
		previousScreen = screens [1];
		nextScreen = screens [2];
	}

	public LevelInfo GetCurrentScreen(){
		return currentScreen;
	}

	public WorldData GetCurrentWorld(){
		return currentWorld;
	}

	public void StartWorldLevelsDisplay(WorldData world){
		currentWorld = world;
		currentLevel = world.GetXthChildData (0);
		worldDisplay.DisplayWorldLevels (world);
		menuCameraController.FocusCamera (worldDisplay.transform.localEulerAngles - new Vector3 (36, 0, 0), 0);
		
		menuCameraController.shouldMonitorVerticalSwiping = true;
		ReturnToWorldDisplay ();
	}

	public void ReturnToWorldDisplay(){
		focusedOnWorldLayer = true;
		worldDisplay.transform.localRotation = Quaternion.Euler (currentScreen.transform.localEulerAngles - new Vector3 (36, 0, 0));
		menuCameraController.FocusCamera (worldDisplay.transform.localEulerAngles, 1);
		menuCameraController.shouldMonitorHorizontalSwiping = false;
	}
	
	public void StartLevelInfoDisplay(int levelID){
		focusedOnWorldLayer = false;
		currentLevel = LevelDataManager.manager.GetLevelData (levelID);
		currentScreen.transform.localRotation = Quaternion.Euler (worldDisplay.transform.localEulerAngles - new Vector3 (-36, 0, 0));
		currentScreen.DisplayLevelInfo (currentLevel);

		SetupAsPrevious(previousScreen);
		SetupAsNext (nextScreen);

		menuCameraController.FocusCamera (currentScreen.transform.localEulerAngles, 1);
		menuCameraController.shouldMonitorHorizontalSwiping = true;
	}


	//finds the screen whose localRotation is closest to the given rotation and sets this screen as currentScreen,
	//and sets up other screens around it
	public void SetClosestScreenAsFocused(Quaternion rotation){
		float comparison = (rotation.eulerAngles.y - currentScreen.transform.localEulerAngles.y);
		if (ShouldChangeDownOrRight(comparison)) {
			SetNextAsFocused ();
		} else if (ShouldChangeUpOrLeft(comparison)) {
			SetPreviousAsFocused();
		} //else current screen is still closest, no change needed
	}

	//focuses the screen on the correct screen after a vertical swipe
	public void MoveBetweenVerticalLayers(Quaternion rotation){
		float currentX = focusedOnWorldLayer ? worldDisplay.transform.localEulerAngles.x : currentScreen.transform.localEulerAngles.x;
		float change = (rotation.eulerAngles.x - currentX);
		if (focusedOnWorldLayer) {
			if (ShouldChangeDownOrRight(change)){
				StartLevelInfoDisplay(currentLevel.GetLevelID ());
			} else if (ShouldChangeUpOrLeft(change)){
				ReturnToWorldSelect();
			} else {
				menuCameraController.FocusCamera (worldDisplay.transform.localEulerAngles, 1);
			}
		} else { //focused on currentlevel
			if (ShouldChangeDownOrRight(change)){
				if (currentLevel.IsUnlocked()){
					PlayLevel(currentLevel.GetLevelID ());
				} else {
					menuCameraController.FocusCameraOnCurrentScreen();
				}
			} else if (ShouldChangeUpOrLeft(change)){
				ReturnToWorldDisplay();
			} else {
				menuCameraController.FocusCameraOnCurrentScreen();
			}
		}
	}

	//determines whether a change is strong enough and in the right direction for a upwards or leftward screen change
	private bool ShouldChangeUpOrLeft(float change){
		return (change < 342 && change > 180) || (change < -18 && change > -180);
	}

	//determins whether a change is strong enough and in the right direction for a downwards or rightward screen change
	private bool ShouldChangeDownOrRight(float change){
		return (change > 18 && change < 180) || (change > -342 && change < -180);
	}

	public void ReturnToWorldSelect(){
		menuCameraController.FocusCamera ((worldDisplay.transform.localEulerAngles - new Vector3(36, 0 ,0)), 1);
		menuCameraController.shouldMonitorVerticalSwiping = false;
		worldSelectManager.ExitWorld ();
	}

	public void PlayLevel(int levelID){	
		LevelManager.manager.SetCurrentLevel (currentLevel.GetLevelID ());
		DOTween.CompleteAll ();
		menuCameraController.transform.DOLocalRotate(currentScreen.transform.localEulerAngles + new Vector3 (36, 0, 0), 1).Play ().OnComplete(LevelManager.manager.FirstLoadLevel);  
	
	}
	
	//sets the next level screen to be the new focused and current level screen
	private void SetNextAsFocused(){
		if (nextLevelExists) {
			LevelInfo oldPreviousScreen = previousScreen;
			previousScreen = currentScreen;
			currentScreen = nextScreen;
			nextScreen = oldPreviousScreen;
			SetupScreens ();
		} //else current screen will remain focused

	}

	//se	ts the previous level screen to be the new focused and current level screen
	private void SetPreviousAsFocused(){
		if (previousLevelExists) { 
			LevelInfo oldNextScreen = nextScreen;
			nextScreen = currentScreen;
			currentScreen = previousScreen;
			previousScreen = oldNextScreen;
			SetupScreens ();
		} //else current screen will remain focused
	}

	//moves screens around the current screen and sets them to display correct data
	private void SetupScreens(){
		currentLevel = currentScreen.GetCurrentLevel ();
		SetupAsNext (nextScreen);
		SetupAsPrevious (previousScreen);
	}
	
	private void SetupAsPrevious(LevelInfo screen){		
		LevelData previousLevel = LevelDataManager.manager.GetPreviousLevelData (currentLevel);
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
	public bool IsSafeToPress(){
		return !menuCameraController.IsSwiping ();
	}

	//the main menu controller will call this function when the back button is pressed
	public void BackButton(){
		if (focusedOnWorldLayer){
			ReturnToWorldSelect();
		} else {
			ReturnToWorldDisplay();
		}
	}
}
