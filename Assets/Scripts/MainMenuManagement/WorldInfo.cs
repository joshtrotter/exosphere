using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldInfo : MonoBehaviour {

	public WorldData currentWorld;
	private WorldSelectManager worldSelectManager;
	private bool isLastWorld;

	public Text title;
	public Text completionStatus;
	public Text stars;
	public Button upButton;
	public CanvasRenderer newLevels;
	public Button enterWorldButton;

	void Start(){
		worldSelectManager = GetComponentInParent<WorldSelectManager> ();
	}

	public void DisplayWorldInfo(WorldData newWorld){
		currentWorld = newWorld;
		if (currentWorld.worldID == LevelDataManager.manager.GetNumberOfWorlds ()) {
			isLastWorld = true;
			SetUpAsLastLevel();
		} else {
			isLastWorld = false;
			SetLatestInfo ();
		}
	}

	public void EnterWorld(){
		if (currentWorld.unlocked) {
			worldSelectManager.EnterWorld (currentWorld);
		} else if (!isLastWorld) {
			worldSelectManager.InitiateWorldPurchaseOptions(currentWorld);
		}
	}

	public void ReturnToOpeningScreen(){
		worldSelectManager.MoveScreenToWorld (-currentWorld.worldID);
	}

	private void SetLatestInfo(){
		upButton.interactable = true; 
		enterWorldButton.gameObject.SetActive (true);
		newLevels.gameObject.SetActive (false);
		title.text = currentWorld.worldName;
		completionStatus.text = currentWorld.GetCompletionStatus ();
		stars.text = currentWorld.GetStarStatus ();
	}

	private void SetUpAsLastLevel(){
		upButton.interactable = false; 
		enterWorldButton.gameObject.SetActive (false);
		newLevels.gameObject.SetActive (true);
	}
}
