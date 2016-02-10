using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* this class manages the currently shown UI screens to ensure they are never improperly drawn over one another
 * */
public class UISystemController : MonoBehaviour {

	public static UISystemController controller;

	//list of UISystems in the game from which we will build our dicts
	public UISystem[] UISystems;

	private Dictionary<UISystem, bool> WantsToBeShown = new Dictionary<UISystem, bool>();
	private Dictionary<UISystem, int> UIRanks = new Dictionary<UISystem, int> ();

	private UISystem CurrentlyShownUI = null;
	private int CurrentlyShownUIRank = int.MaxValue;

	void Awake(){
		//set up singleton instance, destroy if a UISystemController already exists.
		if (controller == null) {
			controller = this;
			DontDestroyOnLoad (this);

			for (int i=0; i < UISystems.Length; i++) {
				WantsToBeShown[UISystems[i]] = false;
				UIRanks.Add(UISystems[i],i);
			}

		} else if (controller != this) {
			Destroy(gameObject);
		}
	
	}
	
	//check to see if the android back button (or Esc on computer) has been pressed, and if so, inform the current UI
	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape) && CurrentlyShownUI != null){
			CurrentlyShownUI.BackKey();
		}
	}

	//a UISystem has informed the controller that it wants to be shown
	//It will only be shown if there is not currently a lower ranked UISystem wanting to be shown
	public void RegisterRequest(UISystem NewUI){
		
		//Debug.Log ("Attempting to Register " + NewUI.name + ", Instance ID: " + NewUI.GetInstanceID());

		WantsToBeShown[NewUI] = true;
		if (NewUI != CurrentlyShownUI) {
			CheckForNewUIToShow ();
		}
	}

	//a UISystem has informed the controller that it no longer wishes to be shown
	public void Deregister(UISystem UI){

		//Debug.Log ("Attempting to Deregister " + UI.name + ", Instance ID: " + UI.GetInstanceID());

		WantsToBeShown[UI] = false;

		if (CurrentlyShownUI == UI) {
			CurrentlyShownUI = null;
			CurrentlyShownUIRank = int.MaxValue;
			CheckForNewUIToShow ();
		}
	}

	//ensures that the currently displaying UI is the lowest ranked UI requesting to be shown
	private void CheckForNewUIToShow(){
		UISystem NewUIToShow = null;
		int NewUIRank = CurrentlyShownUIRank;

		
		//Debug.Log ("Checking for New UI to show");

		foreach (UISystem UI in WantsToBeShown.Keys) {
			
			//Debug.Log ("Checking " + UI.name + ", Instance ID: " + UI.GetInstanceID());

			if (WantsToBeShown [UI]) {

				int outRank = int.MaxValue;
				if ((UIRanks.TryGetValue(UI, out outRank)) && outRank < NewUIRank) { //found different UI to display
					NewUIToShow = UI;
					NewUIRank = UIRanks [UI];
				} 
			}

			//Debug.Log ("Check for New UI has made it to step 2");

			if (NewUIToShow != null && NewUIToShow != CurrentlyShownUI) {
				if (CurrentlyShownUI != null) {
					CurrentlyShownUI.Hide ();
				}
				CurrentlyShownUI = NewUIToShow;
				CurrentlyShownUIRank = NewUIRank;
				CurrentlyShownUI.ShowRequestAccepted ();
			}

		}
	}


}
