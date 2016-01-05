using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialMessage : MonoBehaviour {
	
	//enum and struct to interact with TutorialExternalTrigger
	public enum TriggerBehaviour{None,Close,Open,Hide,Ready};

	[System.Serializable]
	public struct TutorialSwitchTuple {
		public TutorialMessage tut;
		public TriggerBehaviour method;
	}

	private TutorialMessageController controller;
	public string message;
	public string messageCaption = "Tap to Close";
	public GameObject imagePrefab;
	public string imageCaption;
	//determines whether message is on left or right. Images will be automatically sent to the opposite side to message
	public bool messageIsOnLeft = true;
	public bool isReady = true;

	//determines how the tutorial responds to the players interactions with its trigger collider
	public TriggerBehaviour triggerEnterBehaviour = TriggerBehaviour.Open;
	public TriggerBehaviour triggerExitBehaviour = TriggerBehaviour.Close;


	void Awake(){
		message = message.Replace ("\\n", "\n");
		controller = GameObject.FindGameObjectWithTag ("TutorialSystem").GetComponent<TutorialMessageController> ();
	}

	private void OpenMessage(){
		controller.DisplayMessage (this);
	}

	private void HideMessage(){
		controller.HideMessage ();
	}

	private void CloseMessage(){
		controller.HideMessage ();
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
		if (behaviour == TriggerBehaviour.Ready) {
			isReady = true;
			return;
		} 
		if (!isReady)
			return;
		switch (behaviour) {
		case TriggerBehaviour.None:
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

}
