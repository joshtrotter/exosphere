using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialMessage : HasLevelState {
	
	//enum and struct to interact with TutorialExternalTrigger
	public enum TriggerBehaviour{None,Close,Open,Hide,SetReady,SetUnready};

	[System.Serializable]
	public struct TutorialSwitchTuple {
		public TutorialMessage tut;
		public TriggerBehaviour method;
	}
	
	private const int READY_STATE = 0; //can be opened
	private const int UNREADY_STATE = 1; //cannot be opened until changed to ready state
	private const int CLOSED_STATE = 2; //cannot be opened and cannot be set to ready - permanent
	
	private TutorialMessageController controller;
	public string message;
	public string messageCaption = "Tap to Close";
	public GameObject imagePrefab;
	public string imageCaption;
	//determines whether message is on left or right. Images will be automatically sent to the opposite side to message
	public bool messageIsOnLeft = true;

	//determines how the tutorial responds to the players interactions with its trigger collider
	public TriggerBehaviour triggerEnterBehaviour = TriggerBehaviour.Open;
	public TriggerBehaviour triggerExitBehaviour = TriggerBehaviour.Close;

	//determines how long the message will be displayed before it automatically disappears
	//a value of 0 will never timeout
	public float timeOut;

	public TutorialMessage nextPage = null;

	void Awake(){
		message = message.Replace ("\\n", "\n");
	}

	private void OpenMessage(){
		TutorialMessageController.controller.DisplayMessage (this);
		if (timeOut > 0)
			StartCoroutine (CloseAfterTimeOut ());
	}

	private void HideMessage(){
		TutorialMessageController.controller.HideMessage (this);
	}

	private void CloseMessage(){
		if (nextPage != null) {
			TutorialMessageController.controller.DisplayMessage(nextPage);
		}
		RegisterStateChange (CLOSED_STATE);
		HideMessage ();
		this.gameObject.SetActive (false);
	}

	private IEnumerator CloseAfterTimeOut(){
		yield return new WaitForSeconds (timeOut);
		CloseMessage ();
	}
	
	void OnTriggerEnter(Collider coll){
		if (coll.CompareTag("Player")){
			CallBehaviour(triggerEnterBehaviour);
		}
	}
	
	void OnTriggerExit(Collider coll){
		if (coll.CompareTag("Player")){
			CallBehaviour(triggerExitBehaviour);
		}
	}

	public void ExternalTriggerCall(TriggerBehaviour behav = TriggerBehaviour.Close){
		CallBehaviour (behav);
	}


	//calls the desired function based on given behaviour
	void CallBehaviour (TriggerBehaviour behaviour)
	{
		if (behaviour == TriggerBehaviour.SetReady && currentState != CLOSED_STATE) {
			RegisterStateChange(READY_STATE);
			return;
		} 
		if (currentState != READY_STATE)
			return;
		switch (behaviour) {
		case TriggerBehaviour.None:
			break;		
		case TriggerBehaviour.SetUnready:
			RegisterStateChange(UNREADY_STATE);
			break;
		case TriggerBehaviour.Open:
			OpenMessage ();
			break;
		case TriggerBehaviour.Close:
			CloseMessage ();
			break;
		case TriggerBehaviour.Hide:
			HideMessage ();
			break;
		}
	}

	public override void ReloadState(int state) {
		currentState = state;
		if (currentState == CLOSED_STATE)
			CloseMessage ();
	}

	/* Resets the tutorial so that it can be activated again.
	 * Currently used by the Global Tutorial Manager when user Resets Tutorials
	 */
	public void Reset(){
		currentState = READY_STATE;
	}
	
}
