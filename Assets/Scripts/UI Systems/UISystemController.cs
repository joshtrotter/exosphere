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
		} else if (controller != this) {
			Destroy(gameObject);
		}

		for (int i=0; i < UISystems.Length; i++) {
			WantsToBeShown[UISystems[i]] = false;
			UIRanks.Add(UISystems[i],i);
		}
	}

	//a UISystem has informed the controller that it wants to be shown
	//It will only be shown if there is not currently a lower ranked UISystem wanting to be shown
	public void RegisterRequest(UISystem NewUI){
		WantsToBeShown[NewUI] = true;
		if (NewUI != CurrentlyShownUI) {
			CheckForNewUIToShow ();
		}
	}

	//a UISystem has informed the controller that it no longer wishes to be shown
	public void Deregister(UISystem UI){
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
		foreach (UISystem UI in WantsToBeShown.Keys) {
			if (WantsToBeShown [UI]) {
				if (UIRanks [UI] < NewUIRank) { //found different UI to display
					NewUIToShow = UI;
					NewUIRank = UIRanks [UI];
				} 
			}



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
