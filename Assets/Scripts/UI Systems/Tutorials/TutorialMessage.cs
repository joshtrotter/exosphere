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


	void Awake(){
		message = message.Replace ("\\n", "\n");
	}

	private TutorialMessageController GetTutorialController(){
		if (controller == null){
			controller = GetLevelManager().GetComponentInChildren<TutorialMessageController>();
		}
		return controller;
	}

	private void OpenMessage(){
		GetTutorialController().DisplayMessage (this);
	}

	private void HideMessage(){
		GetTutorialController().HideMessage ();
	}

	private void CloseMessage(){
		RegisterStateChange (CLOSED_STATE);
		HideMessage ();
		this.gameObject.SetActive (false);
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
	
}
