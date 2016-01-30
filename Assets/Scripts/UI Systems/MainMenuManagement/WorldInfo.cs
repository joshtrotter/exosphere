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
	public Text availableForPurchase;
	public Button upButton;
	public CanvasRenderer newLevels;
	public Button enterWorldButton;
	public Image starImage;

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

		if (currentWorld.IsUnlocked ()) {
			availableForPurchase.gameObject.SetActive(false);
			stars.text = currentWorld.GetStarStatus ();
			stars.gameObject.SetActive(true);
			starImage.gameObject.SetActive(true);
		} else {
			stars.gameObject.SetActive(false);
			starImage.gameObject.SetActive(false);
			availableForPurchase.gameObject.SetActive(true);
		}
	}

	private void SetUpAsLastLevel(){
		upButton.interactable = false; 
		enterWorldButton.gameObject.SetActive (false);
		newLevels.gameObject.SetActive (true);
	}
}
